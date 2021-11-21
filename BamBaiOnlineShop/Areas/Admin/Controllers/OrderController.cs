using BamBaiOnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        quangtuanEntities db = new quangtuanEntities();
        public ActionResult Index()
        {
            return View();
        }
        public string checkOrder(int Orid)
        {
            try
            {
                Order order = db.Orders.Where(x => x.OrderID.Equals(Orid)).FirstOrDefault();
                order.status = true;
                db.SaveChanges();
                return "Xác nhận đơn hàng thành công";
            }
            catch (Exception)
            {

                return "Lỗi xác nhận";
            }

        }
        public JsonResult Get_AllOrder()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var orders = (from p in db.Orders
                         join ct in db.Customers
                         on p.CustomerID equals ct.CustomerID
                         select new
                         {
                             p.OrderID,
                             p.CustomerID,
                             p.Customer.FullName,
                             p.OrderDate,
                             p.ShipDate,
                             p.status
                         }).ToList();
            return Json(orders, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Get_OrderDetailById(int Orid)
        {
            db.Configuration.ProxyCreationEnabled = false;

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
                              p.UnitCost,
                              thanhtien=p.UnitCost * p.Quantity 
                          }).ToList();
            return Json(orderdetails, JsonRequestBehavior.AllowGet);

        }
    }
}