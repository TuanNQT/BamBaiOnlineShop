using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BamBaiOnlineShop.Models;

namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class APIBamBaiController : Controller
    {
        quangtuanEntities db = new quangtuanEntities();
        // GET: Admin/APIBamBai
        public JsonResult Get_AllProduct()
        {
            db.Configuration.ProxyCreationEnabled = false;
            /*List<Product> products = db.Products.ToList();*/

            var products = (from p in db.Products
                            join ct in db.Categories
                            on p.CategoryID equals ct.CategoryID
                            select new
                            {
                                p.Category.CategoryName,
                                p.CategoryID,
                                p.ProductID,
                                p.ModelName,
                                p.ModelNumber,
                                p.ProductImage,
                                p.UnitCost,
                                p.Description,
                                p.Status
                            }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Login(string username, string password)
        {
            password = HashMD5.MD5Hash(password);
            var user = (from ad in db.Customers
                        where ad.EmailAddress == username && ad.Password == password
                        select new { ad.FullName, ad.EmailAddress, ad.CustomerID }).FirstOrDefault();
            if (user != null)
            {
                return Json(new { code = 200, user }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Get_AllCategory()
        {
            var data = (
                        from c in db.Categories
                        join p in db.Products on c.CategoryID equals p.CategoryID into ps
                        from p in ps.DefaultIfEmpty()
                        select new { CategoryName = c.CategoryName, CategoryID = c.CategoryID, CountProduct = p == null ? 0 : ps.Count() }).Distinct().ToList();


            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult addCart(int soluong, int id, int cusid)
        {
            try
            {
                var data = db.ShoppingCarts.ToList();

                var wishlist = db.WishLists.Where(x => x.ProductID.Equals(id)).FirstOrDefault();
                bool exists = data.Any(x => x.CustomerID == cusid && x.ProductID == id);
                if (exists)
                {
                    var cust =
                        (from c in db.ShoppingCarts
                         where c.CustomerID == cusid && c.ProductID == id
                         select c).FirstOrDefault();
                    cust.Quantity += soluong;
                    db.SaveChanges();
                    if (wishlist != null)
                    {
                        db.WishLists.Remove(wishlist);
                        db.SaveChanges();
                    }

                    return Json(new { code = 250, msg = "Cập nhật giỏ hàng thành công" }, JsonRequestBehavior.AllowGet);
                }
                var cart = new ShoppingCart();
                cart.ProductID = id;
                cart.Quantity = soluong;
                cart.CustomerID = cusid;
                cart.DateCreated = DateTime.Now;
                db.ShoppingCarts.Add(cart);
                db.SaveChanges();
                if (wishlist != null)
                {
                    db.WishLists.Remove(wishlist);
                    db.SaveChanges();
                }
                return Json(new { code = 200, msg = "Thêm vào giỏ hàng thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "Thêm vào giỏ hàng thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ViewCart(int id)
        {
            try
            {
                var data = (from c in db.ShoppingCarts
                            where c.CustomerID.Equals(id)
                            select new { CartID = c.CartID, ModelNumber = c.Product.ModelNumber, Quantity = c.Quantity, ProductID = c.ProductID, Image = c.Product.ProductImage, ProductName = c.Product.ModelName, ProductUnit = c.Product.UnitCost, DateCreated = c.DateCreated, Thanhtien = c.Quantity * c.Product.UnitCost, CustomerID = c.CustomerID }).ToList();
                if (data != null)
                {
                    return Json(new { code = 200, data }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { code = 500, msg = "Giỏ hàng trống" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {

                return Json(new { code = 404, msg = "Lỗi request" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult updateCart(int cartID, int soluong)
        {
            try
            {
                var data = (from c in db.ShoppingCarts
                            where c.CartID.Equals(cartID)
                            select c).FirstOrDefault();
                data.Quantity += soluong;
                db.SaveChanges();
                return Json(new { code = 200, msg = "update cart completed" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "cart not updated" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult deleteCart(int cartID)
        {
            try
            {
                var data = (from c in db.ShoppingCarts
                            where c.CartID.Equals(cartID)
                            select c).FirstOrDefault();
                db.ShoppingCarts.Remove(data);
                db.SaveChanges();
                return Json(new { code = 200, msg = "delete item completed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "item not deleted" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult checkout(int id)
        {
            try
            {
                Order order = new Order();
                order.CustomerID = id;
                order.OrderDate = DateTime.Now;
                order.ShipDate = DateTime.Now.AddDays(3);
                order.status = false;
                db.Orders.Add(order);
                db.SaveChanges();
                int orderId = order.OrderID;
                var shopingcart = db.ShoppingCarts.Where(x => x.CustomerID == id);
                foreach (ShoppingCart item in shopingcart)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderID = orderId;
                    orderDetail.ProductID = item.ProductID;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.UnitCost = item.Product.UnitCost;
                    db.OrderDetails.Add(orderDetail);
                }
                db.SaveChanges();
                var delcart = db.ShoppingCarts.Where(x => x.CustomerID.Equals(id));
                db.ShoppingCarts.RemoveRange(delcart);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm đơn hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "Thêm đơn hàng thất bại" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult viewOrder(int id)
        {
            try
            {
                var order = (from o in db.Orders
                             where o.CustomerID.Equals(id)
                             select new
                             {
                                 o.CustomerID,
                                 o.OrderDate,
                                 o.OrderID,
                                 o.ShipDate,
                                 o.status
                             }).ToList();
                if (order != null)
                {
                    return Json(new { code = 200, order }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 250, msg = "Lịch sử đặt hàng trống" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult viewOrderDetails(int Orid)
        {
            try
            {
                var orderdetails = (from p in db.OrderDetails
                                    join pr in db.Products
                                    on p.ProductID equals pr.ProductID
                                    join ct in db.Orders
                                    on p.OrderID equals ct.OrderID
                                    where p.OrderID == Orid
                                    select new
                                    {
                                        p.OrderID,
                                        p.ProductID,
                                        p.Product.ModelName,
                                        p.Quantity,
                                        p.Product.ProductImage,
                                        p.UnitCost,
                                        thanhtien = p.UnitCost * p.Quantity
                                    }).ToList();
                return Json(new { code = 200, orderdetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Register(string username, string email, string password)
        {
            try
            {
                var data = db.Customers.ToList();
                foreach (var item in data)
                {
                    if (item.EmailAddress.ToLower().Equals(email.ToLower()))
                    {
                        if (item.FullName.ToLower().Equals(username.ToLower()))
                        {
                            return Json(new { code = 250, msg = "Tài khoản đã tồn tại" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { code = 300, msg = "Email đã tồn tại" }, JsonRequestBehavior.AllowGet);
                    }
                }
                Customer customer = new Customer();
                customer.EmailAddress = email;
                customer.FullName = username;
                customer.Password = HashMD5.MD5Hash(password);
                db.Customers.Add(customer);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Đăng ký tài khoản thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Reset(string email)
        {
            try
            {
                var data = db.Customers.FirstOrDefault(x => x.EmailAddress.Equals(email));
                var emailsend = ConfigurationManager.AppSettings.Get("MailAccount");
                var pass = ConfigurationManager.AppSettings.Get("MailPass");
                string code = Membership.GeneratePassword(10, 2);
                if (data != null)
                {
                    var senderEmail = new MailAddress(emailsend, "BamBaiShop");
                    var receiverEmail = new MailAddress(email, "Receiver");
                    var password = pass;
                    string body = "<h2>Truy cập https://bambai.online/Reset-PassWord \nSử dụng mã code dưới đây để đặt lại mặt khẩu</h2>\n" + "<h3><b>" + code + "</b></h3>";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = "Mã đặt lại mật khẩu",
                        IsBodyHtml = true,
                        Body = body

                    })
                    {
                        smtp.Send(mess);
                    }
                    data.ResetCode = code;
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Sử dụng mã code trong email để đặt lại mật khẩu" },JsonRequestBehavior.AllowGet);


                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy người dùng" },JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "Error!" },JsonRequestBehavior.AllowGet);
            }
           

        }
    }
}