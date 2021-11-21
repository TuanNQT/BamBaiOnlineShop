using BamBaiOnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class CategoryProductController : BaseController
    {
        // GET: Admin/CategoryProduct
        quangtuanEntities db = new quangtuanEntities();
        // GET: Admin/category
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Get_AllCategory()
        {
            var data = (
                        from c in db.Categories
                        join p in db.Products on c.CategoryID equals p.CategoryID into ps
                        from p in ps.DefaultIfEmpty()
                        select new { CategoryName = c.CategoryName,CategoryID=c.CategoryID, CountProduct = p == null ? 0 : ps.Count() }).Distinct().ToList();


            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public string Insert(Category category)
        {
            try
            {
                if (category != null)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return "Category Added Successfully";

                }
                else
                {
                    return "Category Not Inserted! Try Again";
                }
            }
            catch (Exception)
            {

                return "Category Not Inserted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Delete(Category category)
        {
            try
            {
                if (category != null)
                {
                    var Cat_ = db.Entry(category);
                    if (Cat_.State == System.Data.Entity.EntityState.Detached)
                    {
                        db.Categories.Attach(category);
                        db.Categories.Remove(category);
                    }
                    db.SaveChanges();
                    return "Category Deleted Successfully";

                }
                else
                {
                    return "Category Not Deleted! Try Again";
                }
            }
            catch (Exception)
            {

                return "Category Not Deleted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Update(Category category)
        {
            try
            {
                if (category != null)
                {
                    var Cat = db.Entry(category);
                    Category category1 = db.Categories.Where(x => x.CategoryID == category.CategoryID).First();
                    category1.CategoryName = category.CategoryName;
                    db.SaveChanges();
                    return "Category Updated Successfully";
                }
                else
                {
                    return "Category Not Updated! Try Again";
                }

            }
            catch (Exception)
            {

                return "Category Not Updated! Try Again";
            }

        }
    }
}