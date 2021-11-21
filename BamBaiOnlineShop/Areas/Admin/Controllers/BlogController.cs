using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BamBaiOnlineShop.Models;
namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        // GET: Admin/Blog
        quangtuanEntities db = new quangtuanEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string Insert_Comment(int Blogid,int CusID,string ContentComment)
        {
            try
            {
                CommentDetail commentDetail = new CommentDetail();
                commentDetail.postid = Blogid;
                commentDetail.content = ContentComment;
                commentDetail.CustomerID = CusID;
                commentDetail.datecomment = DateTime.Now;
                db.CommentDetails.Add(commentDetail);
                db.SaveChanges();
                Blog blog = db.Blogs.Where(x => x.Postid == Blogid).FirstOrDefault();
                blog.comment = blog.comment + 1;
                db.SaveChanges();
                return "Thêm comment thành công";
            }
            catch (Exception)
            {

                return "Thêm thất bại";
            }
        }
        public JsonResult Get_Category()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<TypeBlog> category = db.TypeBlogs.ToList();
            return Json(category, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Get_AllBlog()
        {
            db.Configuration.ProxyCreationEnabled = false;
            /*List<Product> products = db.Products.ToList();*/

            var blogs = (from p in db.Blogs
                            join ct in db.TypeBlogs
                            on p.Typeid equals ct.Typeid
                            select new
                            {
                                p.TypeBlog.TypeName,
                                p.Postid,
                                p.Title,
                                p.datepost,
                                p.content,
                                p.view,
                                p.comment,
                                p.Typeid,
                                p.image,
                                p.writer
                            }).ToList();
            return Json(blogs, JsonRequestBehavior.AllowGet);

        }
        [NoDirectAccess]
        public string Insert_Blog(Blog blog)
        {
            try
            {
                if (blog != null)
                {

                    db.Blogs.Add(blog);
                    db.SaveChanges();
                    return "Blog Added Successfully";

                }
                else
                {
                    return "Blog Not Inserted! Try Again";
                }
            }
            catch (Exception)
            {

                return "Blog Not Inserted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Delete_Blog(Blog blog)
        {
            try
            {
                if (blog != null)
                {
                    var Pro_ = db.Entry(blog);
                    if (Pro_.State == System.Data.Entity.EntityState.Detached)
                    {
                        db.Blogs.Attach(blog);
                        db.Blogs.Remove(blog);
                    }
                    db.SaveChanges();
                    return "Blog Deleted Successfully";

                }
                else
                {
                    return "Blog Not Deleted! Try Again";
                }
            }
            catch (Exception)
            {

                return "Blog Not Deleted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Update_Blog(Blog blog)
        {
            try
            {
                if (blog != null)
                {
                    var Blog_ = db.Entry(blog);
                    Blog blog1 = db.Blogs.Where(x => x.Postid == blog.Postid).FirstOrDefault();
                    blog1.Title = blog.Title;
                    DateTime date2 = new DateTime(2010, 1, 1, 0, 0, 0);
                    int compare = DateTime.Compare(blog.datepost, date2);
                    if ( compare  > 0)
                    {
                        blog1.datepost = blog.datepost;
                    }
                   
                    blog1.content = blog.content;
                    blog1.view = blog.view;
                    blog1.comment = blog.comment;
                    blog1.Typeid = blog.Typeid;
                    blog1.image = blog.image;
                    blog1.writer = blog.writer;
                    db.SaveChanges();
                    return "Blog Updated Successfully";
                }
                else
                {
                    return "Blog Not Updated! Try Again";
                }
            }
            catch (Exception)
            {

                return "Blog Not Updated! Try Again";
            }

        }
        [HttpPost]
        public ContentResult Upload()
        {
            string path = Server.MapPath("~/Content/images/blog/");
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
        public JsonResult GetBlogs()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var blogs = (from p in db.Blogs
                         join ct in db.TypeBlogs
                         on p.Typeid equals ct.Typeid
                         select new
                         {
                             p.TypeBlog.TypeName,
                             p.Postid,
                             p.Title,
                             p.datepost,
                             p.content,
                             p.view,
                             p.comment,
                             p.Typeid,
                             p.image,
                             p.writer
                         }).ToList();
            return Json(new { data = blogs }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult IndexImport()
        {
            return View();
        }
        public ActionResult ImportData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportData(List<Blog> blogs)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                foreach (var i in blogs)
                {
                    db.Blogs.Add(i);
                }
                db.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status } };

        }
    }
}
