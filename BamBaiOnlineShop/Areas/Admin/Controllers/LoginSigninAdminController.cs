using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BamBaiOnlineShop.Models;
namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class LoginSigninAdminController : Controller
    {
        quangtuanEntities db = new quangtuanEntities();
        // GET: Admin/LoginAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public string Reg(string taikhoan, string matkhau)
        {
            Session.Clear();
            /*            var data = db.AdminAccounts.ToList();
                        bool exists = data.Any(x => x.TaiKhoan.Equals(taikhoan));
                        if (exists)
                        {
                            return "Tài khoản đã tồn tại";
                        }
                        else
                        {
                            AdminAccount adminAccount = new AdminAccount();
                            adminAccount.TaiKhoan = taikhoan;
                            adminAccount.MatKhau = HashMD5.MD5Hash(matkhau);
                            db.AdminAccounts.Add(adminAccount);
                                var adminSession = new AdminLogin();
                                adminSession.AdminName = adminAccount.TaiKhoan;
                                Session.Add(CommonConstants.ADMIN_SESSION, adminSession);
                            db.SaveChanges();
                            return "Đăng ký thành công";
                        }*/
            return "Liên hệ admin để tạo acc quản trị";
            
        }
        [HttpPost]
        public string Login(string taikhoan,string matkhau)
        {
                Session.Clear();
                matkhau = HashMD5.MD5Hash(matkhau);
                var check = (from ad in db.AdminAccounts
                            where ad.TaiKhoan == taikhoan && ad.MatKhau == matkhau
                            select ad).FirstOrDefault();
                if (check != null)
                {
                var adminSession = new AdminLogin();
                adminSession.AdminName = taikhoan;
                /*userSession.UserID = user.id;*/
                Session.Add(CommonConstants.ADMIN_SESSION, adminSession);
                return "Đăng nhập thành công";
              
              
                }
                else
                {
                return "Thông tin đăng nhập không chính xác";
                }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");

        }
    }
}