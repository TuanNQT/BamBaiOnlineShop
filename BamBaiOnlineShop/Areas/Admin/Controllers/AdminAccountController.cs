using BamBaiOnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class AdminAccountController : BaseController
    {
        // GET: Admin/AdminAccount
        quangtuanEntities db = new quangtuanEntities();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Get_AllAdminAccount()
        {
            var data = db.AdminAccounts.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public string Insert(AdminAccount AdminAccount)
        {
            try
            {
                var data = db.AdminAccounts.ToList();
                bool exists = data.Any(x => x.TaiKhoan.Equals(AdminAccount.TaiKhoan));
                if (exists)
                {
                    return "Account already exist";
                }
                else
                {
                    if (AdminAccount != null)
                    {

                        AdminAccount.MatKhau = HashMD5.MD5Hash(AdminAccount.MatKhau);
                        db.AdminAccounts.Add(AdminAccount);
                        db.SaveChanges();
                        return "AdminAccount Added Successfully";

                    }
                    else
                    {
                        return "AdminAccount Not Inserted! Try Again";
                    }
                }


            }
            catch (Exception)
            {

                return "AdminAccount Not Inserted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Delete(AdminAccount AdminAccount)
        {
            try
            {
                if (AdminAccount != null)
                {
                    var Cat_ = db.Entry(AdminAccount);
                    if (Cat_.State == System.Data.Entity.EntityState.Detached)
                    {
                        db.AdminAccounts.Attach(AdminAccount);
                        db.AdminAccounts.Remove(AdminAccount);
                    }
                    db.SaveChanges();
                    return "AdminAccount Deleted Successfully";

                }
                else
                {
                    return "AdminAccount Not Deleted! Try Again";
                }
            }
            catch (Exception)
            {

                return "AdminAccount Not Deleted! Try Again";
            }

        }
        [NoDirectAccess]
        public string Update(AdminAccount AdminAccount)
        {
            try
            {
                if (AdminAccount != null)
                {
                    var Cat = db.Entry(AdminAccount);
                    AdminAccount AdminAccount1 = db.AdminAccounts.Where(x => x.TaiKhoan == AdminAccount.TaiKhoan).First();
                    AdminAccount1.MatKhau = HashMD5.MD5Hash(AdminAccount.MatKhau);
                    db.SaveChanges();
                    return "AdminAccount Updated Successfully";
                }
                else
                {
                    return "AdminAccount Not Updated! Try Again";
                }

            }
            catch (Exception)
            {

                return "AdminAccount Not Updated! Try Again";
            }

        }
    }
}