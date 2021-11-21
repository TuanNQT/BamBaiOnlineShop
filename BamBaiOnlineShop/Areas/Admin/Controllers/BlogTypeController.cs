using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BamBaiOnlineShop.Models;
namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class BlogTypeController : BaseController
    {
        // GET: Admin/BlogType
        quangtuanEntities db = new quangtuanEntities();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Get_AllTypeBlog()
        {
            var data = (
                        from c in db.TypeBlogs
                        join p in db.Blogs on c.Typeid equals p.Typeid into ps
                        from p in ps.DefaultIfEmpty()
                        select new { TypeName = c.TypeName, Typeid = c.Typeid, CountBlog = p == null ? 0 : ps.Count() }).Distinct().ToList();


            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public string Insert(TypeBlog TypeBlog)
        {
            try
            {
                if (TypeBlog != null)
                {
                    db.TypeBlogs.Add(TypeBlog);
                    db.SaveChanges();
                    return "TypeBlog Added Successfully";

                }
                else
                {
                    return "TypeBlog Not Inserted! Try Again";
                }
            }
            catch (Exception)
            {

                return "TypeBlog Not Inserted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Delete(TypeBlog TypeBlog)
        {
            try
            {
                if (TypeBlog != null)
                {
                    var Cat_ = db.Entry(TypeBlog);
                    if (Cat_.State == System.Data.Entity.EntityState.Detached)
                    {
                        db.TypeBlogs.Attach(TypeBlog);
                        db.TypeBlogs.Remove(TypeBlog);
                    }
                    db.SaveChanges();
                    return "TypeBlog Deleted Successfully";

                }
                else
                {
                    return "TypeBlog Not Deleted! Try Again";
                }
            }
            catch (Exception)
            {

                return "TypeBlog Not Deleted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Update(TypeBlog TypeBlog)
        {
            try
            {
                if (TypeBlog != null)
                {
                    var Cat = db.Entry(TypeBlog);
                    TypeBlog TypeBlog1 = db.TypeBlogs.Where(x => x.Typeid == TypeBlog.Typeid).First();
                    TypeBlog1.TypeName = TypeBlog.TypeName;
                    db.SaveChanges();
                    return "TypeBlog Updated Successfully";
                }
                else
                {
                    return "TypeBlog Not Updated! Try Again";
                }

            }
            catch (Exception)
            {

                return "TypeBlog Not Updated! Try Again";
            }

        }
    }
}