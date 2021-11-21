using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BamBaiOnlineShop.Models;

namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class DefaultController : BaseController
    {
        // GET: Admin/Default
        quangtuanEntities db = new quangtuanEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewTop5Char()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetTop5Product()
        {
            /*            List<sellerBest> query = (from OrderDetails in db.OrderDetails
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
                                                  }).Take(5).ToList();*/
            var data = db.TopProduct().ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChartByMonth()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ThongKeByMonth()
        {
            var data = (from od in db.OrderDetails
                        join o in db.Orders on od.OrderID equals o.OrderID
                        join p in db.Products on od.ProductID equals p.ProductID
                        where
                          o.OrderDate.Year == DateTime.Now.Year
                        group new { od, o, p } by new
                        {
                            Column1 = (int?)o.OrderDate.Month
                        } into g
                        select new 
                        {
                            Thang = g.Key.Column1.ToString(),
                            Doanh_thu = (float)(float?)g.Sum(p => p.p.UnitCost * p.od.Quantity)
                        }).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ThongKeByCustomer()
        {
            var data = (from c in db.Customers
                        join od in db.Orders on c.CustomerID equals od.CustomerID
                        join ol in db.OrderDetails on od.OrderID equals ol.OrderID
                        join p in db.Products on ol.ProductID equals p.ProductID
                        /* where od.OrderDate.Year == DateTime.Now.Year*/
                        group new { c, p, ol } by new
                        {
                            c.FullName
                        } into g
                        orderby
                       g.Sum(p => p.p.UnitCost * p.ol.Quantity) descending
                        select new 
                        {
                            Label = g.Key.FullName,
                            Y = (float)g.Sum(p => p.p.UnitCost * p.ol.Quantity)
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ThongKeByCategories()
        {
            var data = (from od in db.OrderDetails
                        join p in db.Products on od.ProductID equals p.ProductID
                        join cate in db.Categories on p.CategoryID equals cate.CategoryID
                        group new { od, p, cate } by new
                        {
                            column = cate.CategoryName
                        } into g
                        select new 
                        {
                            Label = g.Key.column.ToString(),
                            Y = (float)(int?)g.Sum(p => p.od.Quantity)
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}