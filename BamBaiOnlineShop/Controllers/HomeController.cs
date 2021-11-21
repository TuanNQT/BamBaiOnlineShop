using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BamBaiOnlineShop.Models;
using PagedList;

namespace BamBaiOnlineShop.Controllers
{
    public class HomeController : Controller
    {
        quangtuanEntities db = new quangtuanEntities();
     
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Shop()
        {
            return View();
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult WishList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult addOrder()
        {
            try
            {
                var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];

                var customer = db.Customers.FirstOrDefault(x => x.CustomerID.Equals(session.UserID));

                if (customer.PhoneNumber == null)
                {
                    return Json(new { code = 204, msg = "Vui lòng cập nhật thông tin cá nhân" }, JsonRequestBehavior.AllowGet);
                }

                Order order = new Order();
                order.CustomerID = session.UserID;
                order.OrderDate = DateTime.Now;
                order.ShipDate = DateTime.Now.AddDays(3);
                order.status = false;
                db.Orders.Add(order);
                db.SaveChanges();
                int orderId = order.OrderID;
                var shopingcart = db.ShoppingCarts.Where(x => x.CustomerID == session.UserID);
                #region Gửi email
                //try
                //{
                //    var emailsend = ConfigurationManager.AppSettings.Get("MailAccount");
                //    var senderEmail = new MailAddress(emailsend, "Me");
                //    var receiverEmail = new MailAddress(emailsend, "Receiver");
                //    var password = ConfigurationManager.AppSettings.Get("MailPass");
                //    var sub = customer.FullName + " vừa đặt một đơn hàng";
                //    int i = 0;
                //    decimal tongtien = 0;
                //    string sMsg = "<html><body><table border='1'><caption>Thông tin đặt hàng</caption><tr><th>STT</th><th>Tên hàng</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>";
                //    foreach (ShoppingCart item in shopingcart)
                //    {
                //        i++;
                //        sMsg += "<tr>";
                //        sMsg += "<td>" + i.ToString() + "</td>";
                //        sMsg += "<td>" + item.Product.ModelName + "</td>";
                //        sMsg += "<td>" + item.Quantity.ToString() + "</td>";
                //        sMsg += "<td>" + item.Product.UnitCost.ToString() + "</td>";
                //        sMsg += "<td>" + String.Format("{0:#,###}", item.thanhtien) + "</td>";
                //        sMsg += "<tr>";
                //        tongtien += item.thanhtien;
                //    }
                //    sMsg += "<tr><th colspan='5'>Tổng cộng:"
                //        + String.Format("{0:#,### VNĐ}", tongtien) + "</th></tr></table>";

                //    string body = sMsg;
                //    var smtp = new SmtpClient
                //    {
                //        Host = "smtp.gmail.com",
                //        Port = 587,
                //        EnableSsl = true,
                //        DeliveryMethod = SmtpDeliveryMethod.Network,
                //        UseDefaultCredentials = false,
                //        Credentials = new NetworkCredential(senderEmail.Address, password)
                //    };
                //    using (var mess = new MailMessage(senderEmail, receiverEmail)
                //    {
                //        Subject = sub,
                //        IsBodyHtml = true,
                //        Body = body
                //    })
                //    {
                //        smtp.Send(mess);
                //    }
                //}
                //catch (Exception)
                //{

                //    return Json(new { code = 500, msg = "Gửi email thất bại" }, JsonRequestBehavior.AllowGet);
                //}
                #endregion
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
                var delcart = db.ShoppingCarts.Where(x => x.CustomerID.Equals(session.UserID));
                db.ShoppingCarts.RemoveRange(delcart);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm đơn hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { code = 204, msg = "Lỗi thêm đơn hàng" }, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public JsonResult addCart(int soluong, int id)
        {
            try
            {
                var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
                var data = db.ShoppingCarts.ToList();
                int cusid = session.UserID; 
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
                    return Json(new { code = 200, msg = "Cập nhật giỏ hàng thành công" }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public JsonResult addWishList(int id)
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            try
            {
                var data = db.WishLists.ToList();
                var cusid = session.UserID;
                bool exists = data.Any(x => x.CustomerID == cusid && x.ProductID == id);
                if (exists)
                {
                    return Json(new { code = 200, msg = "Sản phẩm này đã có trong danh sách yêu thích" }, JsonRequestBehavior.AllowGet);
                }
                var wishlist = new WishList();
                wishlist.ProductID = id;
                wishlist.CustomerID = cusid;
                db.WishLists.Add(wishlist);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Đã thêm vào danh sách yêu thích" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json(new { code = 500, msg = "Thêm vào thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult QuickViewCart()
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var data = db.ShoppingCarts.Where(x => x.CustomerID.Equals(session.UserID)).ToList();
                return PartialView(data);
            }

            else
            {
                ViewData["NotLogin"] = "Login Now";
                return PartialView();
            }

        }
        [HttpPost]
        public JsonResult updateCartGiam(int id)
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            var cust =
                        (from c in db.ShoppingCarts
                         where c.CustomerID == session.UserID && c.ProductID == id
                         select c).FirstOrDefault();
            cust.Quantity += -1;
            db.SaveChanges();
            return Json(new { code = 200, msg = "Cập nhật giỏ hàng thành công" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewOrder()
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var data = db.Orders.Where(x => x.CustomerID.Equals(session.UserID)).ToList();
                return PartialView(data);
            }
            else
            {
                ViewBag.message = "Bạn chưa đăng nhập";
            }
            return PartialView();
        }

        public ActionResult DetailsOrder(int id)
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
           /* var check = db.Orders.Where(x => x.CustomerID.Equals(session.UserID) && x.OrderID.Equals(id)).ToList();*/
            if (session != null)
            {
                try
                {
                    var orderDetail = db.OrderDetails.Where(x => x.OrderID.Equals(id) && x.Order.CustomerID.Equals(session.UserID)).ToList();
                    return PartialView(orderDetail);
                }
                catch (Exception)
                {

                    ViewBag.message = "Bạn không thể xem đơn hàng này";
                    return PartialView();
                }
               
            }
            else
            {
                ViewBag.message = "Bạn chưa đăng nhập";
                return PartialView();
            }
        }
        public ActionResult ViewCart()
        {
            return View();
        }

        public ActionResult ListCart(int? page, int? sizepage)
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var data = db.ShoppingCarts.Where(x => x.CustomerID.Equals(session.UserID)).ToList();
                int pageSize = 5;
                if (sizepage != null)
                {
                    pageSize = (int)sizepage;
                }

                int pageNumber = (page ?? 1);
                return PartialView(data.ToPagedList(pageNumber, pageSize));
            }

            else
            {
                ViewData["NotLogin"] = "Login Now";
                return PartialView();
            }


        }

        [HttpPost]
        public JsonResult xoaWishList(int id)
        {
            try
            {
                var data = db.WishLists.Where(x => x.ProductID.Equals(id)).FirstOrDefault();
                db.WishLists.Remove(data);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Đã xóa" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { code = 204, msg = "Lỗi gì đó" }, JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpPost]
        public JsonResult xoaCart(int id)
        {
            try
            {
                var data = db.ShoppingCarts.Where(x => x.ProductID.Equals(id)).FirstOrDefault();
                db.ShoppingCarts.Remove(data);
                db.SaveChanges();
                return Json(new { code = 500, msg = "Đã xóa" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                var data = db.ShoppingCarts.Where(x => x.ProductID.Equals(id)).FirstOrDefault();
                db.ShoppingCarts.Remove(data);
                db.SaveChanges();
                return Json(new { code = 204, msg = "Lỗi gì đó" }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult ShowWishList(string searchString, int? cid, int? page, int? sizepage)
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            int id = session.UserID;
            var data = db.WishLists.Where(x => x.CustomerID == id).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(x => x.Product.ModelNumber.Contains(searchString) || x.Product.ModelName.Contains(searchString)).ToList();
            }
            if (cid != null)
            {
                data = data.Where(x => x.Product.CategoryID == cid).ToList();
            }
            int pageSize = 10;
            if (sizepage != null)
            {
                pageSize = (int)sizepage;
            }

            int pageNumber = (page ?? 1);
            return PartialView(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Reviewshow(int rv)
        {
            var reviewdata = db.Reviews.Where(s => s.ProductID.Equals(rv)).ToList();
            if (reviewdata.Count > 0)
            {
                int sumrating = db.Reviews.Where(x => x.ProductID.Equals(rv)).Sum(x => x.Rating);
                int countrating = db.Reviews.Count(x => x.ProductID.Equals(rv));
                float tb = sumrating / countrating;
                ViewData["rating"] = tb;
            }

            return PartialView(reviewdata);
        }

        public ActionResult Commentshow(int cm, int? page, int? sizepage)
        {

            var commentdata = db.CommentDetails.Where(s => s.postid.Equals(cm)).ToList();
            int pageSize = 5;
            if (sizepage != null)
            {
                pageSize = (int)sizepage;
            }

            int pageNumber = (page ?? 1);
            return PartialView(commentdata.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CommentAdd(int pid, string content)
        {

            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            if (session != null)
            {
                CommentDetail comment = new CommentDetail();
                comment.postid = pid;
                comment.content = content;
                comment.CustomerID = session.UserID;
                comment.datecomment = DateTime.Now;
                db.CommentDetails.Add(comment);
                db.SaveChanges();
                Blog blog = db.Blogs.Where(x => x.Postid ==pid).FirstOrDefault();
                blog.comment += 1;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Addreview(int id, int? rating, string comment, string email)
        {
            if (rating == null)
            {
                rating = 1;
            }
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            if (session != null)
            {
                string name = session.UserName;
                Review review = new Review();
                review.ProductID = id;
                review.Rating = (int)rating;
                review.CustomerName = name;
                review.CustomerEmail = email;
                review.Comments = comment;
                review.datecomment = DateTime.Now;
                db.Reviews.Add(review);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult SendEmail(string name, string email, string subject, string message)
        {
            var emailsend = ConfigurationManager.AppSettings.Get("MailAccount");
            var senderEmail = new MailAddress(emailsend, "Me");
            var receiverEmail = new MailAddress(emailsend, "Receiver");
            var password = ConfigurationManager.AppSettings.Get("MailPass");
            var sub = name + "-----" + email + "-----" + subject;
            string body = message;
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
                Subject = sub,
                IsBodyHtml = true,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            ViewBag.message = "Cảm ơn bạn đã phản hồi tới chúng tôi!!";
            return View("Contact");


        }

        public ActionResult DetailProduct(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult DetailPost(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog post = db.Blogs.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            Blog blog = db.Blogs.Where(x => x.Postid == id).FirstOrDefault();
            blog.view += 1;
            db.SaveChanges();
            return View(post);
        }

        public ActionResult ListProduct()
        {
            var data = db.Products.Take(10).ToList();
            return PartialView(data);
        }

        public ActionResult ListBlogShop(string searchString, int? cid, int? page, int? sizepage)
        {
            var data = db.Blogs.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(x => x.Title.Contains(searchString) || x.content.Contains(searchString)).ToList();
            }
            if (cid != null)
            {
                ViewBag.cate = cid;
                data = data.Where(x => x.Typeid == cid).ToList();
            }
            int pageSize = 9;
            if (sizepage != null)
            {
                pageSize = (int)sizepage;
            }

            int pageNumber = (page ?? 1);
            return PartialView(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListProductShop(string currentFilter, string searchString, int? cid, int? page, int? sizepage)
        {

            if (searchString != null)
            {
                page = 1;

            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var data = db.Products.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(x => x.ModelName.Contains(searchString) || x.ModelNumber.Contains(searchString)).ToList();
            }
            if (cid != null)
            {
                ViewBag.cate = cid;
                data = data.Where(x => x.CategoryID == cid).ToList();
            }

            int pageSize = 9;
            if (sizepage != null)
            {
                pageSize = (int)sizepage;
            }

            int pageNumber = (page ?? 1);
            return PartialView(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListCategories()
        {
            var data = db.Categories.ToList();
            return PartialView(data);
        }

        public ActionResult ListTypeBlog()
        {
            var data = db.TypeBlogs.ToList();
            return PartialView(data);
        }

        public ActionResult quickview(int id)
        {
            var data = db.Products.ToList();
            data = data.Where(x => x.ProductID == id).ToList();
            return PartialView(data);
        }

        public ActionResult Top5sphotList()
        {
            //Lấy top 5 sản phẩm 
            List<sellerBest> query = (from OrderDetails in db.OrderDetails
                                      join product in db.Products
                                             on OrderDetails.ProductID equals product.ProductID
                                      group new { OrderDetails, product } by new
                                      {
                                          OrderDetails.ProductID,
                                          product.ModelName,
                                          product.CategoryID,
                                          product.ProductImage,
                                          product.UnitCost,
                                          product.Status,
                                      } into g
                                      orderby
                                        g.Sum(p => p.OrderDetails.Quantity) descending
                                      select new sellerBest
                                      {
                                          categr = g.Key.CategoryID,
                                          ProID = g.Key.ProductID,
                                          Qty = (int)g.Sum(p => p.OrderDetails.Quantity),
                                          ProName = g.Key.ModelName,
                                          Hinh = g.Key.ProductImage,
                                          status = (bool)g.Key.Status,
                                          DoanhThu = (float)(g.Key.UnitCost * g.Sum(p => p.OrderDetails.Quantity))
                                      }).Take(5).ToList();
            return PartialView(query.ToList());
        }

        public ActionResult Top5sphotBanner()
        {
            //Lấy top 5 sản phẩm 
            List<sellerBest> query = (from OrderDetails in db.OrderDetails
                                      join product in db.Products
                                             on OrderDetails.ProductID equals product.ProductID
                                      group new { OrderDetails, product } by new
                                      {
                                          OrderDetails.ProductID,
                                          product.ModelName,
                                          product.CategoryID,
                                          product.ProductImage,
                                          product.UnitCost,
                                          product.Status,
                                      } into g
                                      orderby
                                        g.Sum(p => p.OrderDetails.Quantity) descending
                                      select new sellerBest
                                      {
                                          categr = g.Key.CategoryID,
                                          ProID = g.Key.ProductID,
                                          Qty = (int)g.Sum(p => p.OrderDetails.Quantity),
                                          ProName = g.Key.ModelName,
                                          Hinh = g.Key.ProductImage,
                                          status = (bool)g.Key.Status,
                                          DoanhThu = (float)(g.Key.UnitCost * g.Sum(p => p.OrderDetails.Quantity))
                                      }).Take(5).ToList();
            return PartialView(query);
        }

        public ActionResult Top3sphotBannerFooter()
        {
            //Lấy top 3 sản phẩm 
            List<sellerBest> query = (from OrderDetails in db.OrderDetails
                                      join product in db.Products
                                             on OrderDetails.ProductID equals product.ProductID
                                      group new { OrderDetails, product } by new
                                      {
                                          OrderDetails.ProductID,
                                          product.ModelName,
                                          product.CategoryID,
                                          product.ProductImage,
                                          product.UnitCost,
                                          product.Status,
                                      } into g
                                      orderby
                                        g.Sum(p => p.OrderDetails.Quantity) descending
                                      select new sellerBest
                                      {
                                          categr = g.Key.CategoryID,
                                          ProID = g.Key.ProductID,
                                          Qty = (int)g.Sum(p => p.OrderDetails.Quantity),
                                          ProName = g.Key.ModelName,
                                          Hinh = g.Key.ProductImage,
                                          status = (bool)g.Key.Status,
                                          DoanhThu = (float)(decimal)(g.Key.UnitCost * g.Sum(p => p.OrderDetails.Quantity))
                                      }).Take(3).ToList();
            return PartialView(query);
        }

        public ActionResult Top5blogBanner()
        {
            var data = db.Blogs.OrderByDescending(x => x.view).Take(5).ToList();
            return PartialView(data);
        }
        public ActionResult Top3blogBannerFooter()
        {
            var data = db.Blogs.OrderByDescending(x => x.view).Take(3).ToList();
            return PartialView(data);
        }
        public ActionResult ListBlog()
        {
            var data = db.Blogs.Take(10).ToList();
            return PartialView(data);
        }
        public ActionResult GoiY(int? gy)
        {
            List<GoiYSP> query = (from od1 in db.OrderDetails
                                  join od2 in db.OrderDetails on od1.OrderID equals od2.OrderID
                                  join p in db.Products on od2.ProductID equals p.ProductID
                                  where
                                    od1.ProductID == gy &&
                                    od2.ProductID != gy
                                  group new { od2, p } by new
                                  {
                                      od2.ProductID,
                                      p.ModelName,
                                      p.ModelNumber,
                                      p.UnitCost,
                                      p.ProductImage,
                                      p.Category.CategoryName,
                                      p.Description,
                                      p.Status
                                  } into g
                                  orderby
                                    g.Count(c => c.od2.ProductID != null) descending
                                  select new GoiYSP
                                  {
                                      prid = g.Key.ProductID,
                                      ten = g.Key.ModelName,
                                      gia = (float)g.Key.UnitCost,
                                      masp = g.Key.ModelNumber,
                                      category = g.Key.CategoryName,
                                      hinh = g.Key.ProductImage,
                                      mota = g.Key.Description,
                                      status = (bool)g.Key.Status
                                  }).Take(3).ToList();
            return PartialView(query);
        }
    }
}