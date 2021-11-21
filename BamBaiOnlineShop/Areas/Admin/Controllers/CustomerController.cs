using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BamBaiOnlineShop.Models;
namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        quangtuanEntities db = new quangtuanEntities();
        // GET: Admin/Customer
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string addOrder(int? cusid)
        {
            if (cusid !=null)
            {
                var data = db.ShoppingCarts.ToList();
                bool exists = data.Any(x => x.CustomerID == cusid);
                if (exists)
                {
                    Order order = new Order();
                    order.CustomerID = (int)cusid;
                    order.OrderDate = DateTime.Now;
                    order.ShipDate = DateTime.Now.AddDays(3);
                    order.status = false;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    int orderId = order.OrderID;
                    var shopingcart = db.ShoppingCarts.Where(x => x.CustomerID == cusid);
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
                    db.ShoppingCartRemoveItem(cusid);
                    db.SaveChanges();
                    return "Thêm đơn hàng thành công";
                }
                else
                {
                    return "Giỏ hàng của khách hàng này trống";
                }
            }
            else
            {
                return "Error get Customer";
            }


        }
        public JsonResult Get_AllCustomer()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = db.Customers.ToList();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public string Insert_Customer(Customer customer)
        {
            var data = db.Customers.ToList();
            bool exists = data.Any(x =>x.EmailAddress.Equals(customer.EmailAddress));
            if (exists)
            {
                return "Email already exist";
            }
            else
            {
                if (customer != null)
                {
                    customer.Password = HashMD5.MD5Hash(customer.Password);
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return "Customer Added Successfully";

                }
                else
                {
                    return "Customer Not Inserted! Try Again";
                }
            }

        }
        [NoDirectAccess]
        public string Delete_Customer(Customer customer)
        {
            try
            {
                if (customer != null)
                {
                    var Pro_ = db.Entry(customer);
                    if (Pro_.State == System.Data.Entity.EntityState.Detached)
                    {
                        db.Customers.Attach(customer);
                        db.Customers.Remove(customer);
                    }
                    db.SaveChanges();
                    return "Customer Deleted Successfully";

                }
                else
                {
                    return "Customer Not Deleted! Try Again";
                }
            }
            catch (Exception)
            {

                return "Customer Not Deleted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Update_Customer(Customer customer)
        {
            if (customer != null)
            {
                var Pro_ = db.Entry(customer);
                Customer customer1 = db.Customers.Where(x => x.CustomerID == customer.CustomerID).FirstOrDefault();
                customer1.FullName = customer.FullName;
                customer1.EmailAddress = customer.EmailAddress;
                customer1.Password =HashMD5.MD5Hash(customer.Password);
                customer1.PhoneNumber = customer.PhoneNumber;
                customer1.Gender = customer.Gender;
                customer1.BirthDay = customer.BirthDay;
                customer1.ResetCode = customer.ResetCode;
                db.SaveChanges();
                return "Customer Updated Successfully";
            }
            else
            {
                return "Customer Not Updated! Try Again";
            }
        }
    }
}