using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BamBaiOnlineShop.Models;
namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        quangtuanEntities db = new quangtuanEntities();
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Get_Customer()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = db.Customers.ToList();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Category()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Category> category = db.Categories.ToList();
            return Json(category, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Get_CategoryByID(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Category> category = db.Categories.Where(x=>x.CategoryID.Equals(id)).ToList();
            return Json(category, JsonRequestBehavior.AllowGet);

        }
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
        public JsonResult Get_ProductById(string Id)
        {

            int PrId = int.Parse(Id);
            return Json(db.Products.Find(PrId), JsonRequestBehavior.AllowGet);

        }
        public string Insert_review(int PrID, int? Rating, string Comment, string Email)
        {
            if (Rating == null)
            {
                Rating = 1;
            }
            try
            {
                var data = db.Customers.ToList();
                bool exists = data.Any(x => x.EmailAddress.Equals(Email));
                if (exists)
                {
                    var cust =
                        (from c in db.Customers
                         where c.EmailAddress.Equals(Email)
                         select c).FirstOrDefault();
                    Review review = new Review();
                    review.ProductID = PrID;
                    review.Rating = (int)Rating;
                    review.CustomerName = cust.FullName;
                    review.CustomerEmail = Email;
                    review.Comments = Comment;
                    review.datecomment = DateTime.Now;
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return "Thêm thành công";
                }
                else
                {
                    return "Chọn khách hàng khác";
                }
            }
            catch (Exception)
            {

                return "Thêm thất bại !!";
            }
        
        }
        public string Insert_Cart(int CustomerID,int Quantity,int ProductID)
        {
            var data = db.ShoppingCarts.ToList();
            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.CustomerID = CustomerID;
            shoppingCart.Quantity = Quantity;
            shoppingCart.ProductID = ProductID;
            shoppingCart.DateCreated = DateTime.Now;
            bool exists = data.Any(x => x.CustomerID == CustomerID && x.ProductID == ProductID);
            if (exists)
            {
                var cust =
                    (from c in db.ShoppingCarts
                     where c.CustomerID == CustomerID && c.ProductID == ProductID
                     select c).FirstOrDefault();
                cust.Quantity += Quantity;
                db.SaveChanges();
                return "Cập nhật thành công";
            }
                db.ShoppingCarts.Add(shoppingCart);
                db.SaveChanges();
                return "Product Added to cart Successfully";
            

                
        }
        public string Insert_Product(Product product)
        {
            try
            {
                if (product != null)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return "Product Added Successfully";

                }
                else
                {
                    return "Product Not Inserted! Try Again";
                }
            }
            catch (Exception)
            {

                return "Product Not Inserted! Try Again";
            }

        }
        public string Delete_Product(Product product)
        {
            try
            {
                if (product != null)
                {
                    var Pro_ = db.Entry(product);
                    if (Pro_.State == System.Data.Entity.EntityState.Detached)
                    {
                        db.Products.Attach(product);
                        db.Products.Remove(product);
                    }
                    db.SaveChanges();
                    return "Product Deleted Successfully";

                }
                else
                {
                    return "Product Not Deleted! Try Again";
                }
            }
            catch (Exception)
            {

                return "Product Not Deleted! Try Again";
            }

        }
        public string Update_Product(Product product)
        {
            try
            {
                if (product != null)
                {
                    var Pro_ = db.Entry(product);
                    Product product1 = db.Products.Where(x => x.ProductID == product.ProductID).First();
                    product1.ModelName = product.ModelName;
                    product1.ModelNumber = product.ModelNumber;
                    product1.CategoryID = product.CategoryID;
                    product1.UnitCost = product.UnitCost;
                    product1.Description = product.Description;
                    product1.ProductImage = product.ProductImage;
                    product1.Status = product.Status;
                    db.SaveChanges();
                    return "Product Updated Successfully";
                }
                else
                {
                    return "Product Not Updated! Try Again";
                }

            }
            catch (Exception)
            {

                return "Product Not Updated! Try Again";
            }

        }
        [HttpPost]
        public ContentResult Upload()
        {
            string path = Server.MapPath("~/Content/images/items/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            /*foreach (string key in Request.Files)
            {*/
                HttpPostedFileBase postedFile = Request.Files[0];
                postedFile.SaveAs(path + postedFile.FileName);
            /*}*/

            return Content(postedFile.FileName);
        }
        public JsonResult GetProducts()
        {
            db.Configuration.ProxyCreationEnabled = false;

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
            return Json(new { data = products }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ImportIndex()
        {
            return View();
        }
        public ActionResult ImportData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportData(List<Product> products)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                foreach (var i in products)
                {
                    db.Products.Add(i);
                }
                db.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status } };

        }
    }
}