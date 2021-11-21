using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BamBaiOnlineShop.Models;
namespace BamBaiOnlineShop.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]

    public class LoginUserController : Controller
    {
        // GET: LoginUser

        quangtuanEntities db = new quangtuanEntities();
        // GET: Admin/LoginAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ResetPass()
        {
            ViewBag.message = "";
            return View();
        }
        [HttpPost]
        public ActionResult ResetPass(string email, string code, string newpass)
        {
            var data = db.Customers.FirstOrDefault(x => x.EmailAddress.Equals(email));
            if (data.EmailAddress.Equals(email) && data.ResetCode.Equals(code))
            {
                data.Password = HashMD5.MD5Hash(newpass);
                data.ResetCode = " ";
                db.SaveChanges();
                ViewBag.message = "Đặt lại mật khẩu thành công";
                return View();
            }
            else
            {
                ViewBag.message = "Thông tin không chính xác";
                return View();
            }

        }
        public ActionResult SendCode(string email)
        {
            var data = db.Customers.FirstOrDefault(x => x.EmailAddress.Equals(email));
            var emailsend = ConfigurationManager.AppSettings.Get("MailAccount");
            var pass = ConfigurationManager.AppSettings.Get("MailPass");
            string code = Membership.GeneratePassword(10, 2);
            if (data != null)
            {
                var senderEmail = new MailAddress(emailsend, "BamBai Shop");
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = pass;
                string body = "<h2>Truy cập https://bambai.online/Reset-PassWord \nSử dụng mã code dưới đây để đặt lại mặt khẩu</h2>\n" + "<h3><b>" + code + "</b></h3>";
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
                    Subject = "Mã đặt lại mật khẩu",
                    IsBodyHtml = true,
                    Body = body

                })
                {
                    smtp.Send(mess);
                }
                data.ResetCode = code;
                db.SaveChanges();
                ViewBag.message = "Sử dụng mã code trong email để đặt lại mật khẩu";
                return View("ResetPass");


            }
            else
            {
                ViewBag.message = "Không tìm thấy người dùng";
            }
            return View("ResetPass");
        }
        public ActionResult Profile()
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            var data = db.Customers.FirstOrDefault(x => x.CustomerID.Equals(session.UserID));
            return View(data);
        }
        public ActionResult Change(string oldpass, string newpass, string ten, string email, string phone, DateTime? birthday, bool? gender)
        {
            var session = (Userlogin)Session[BamBaiOnlineShop.Models.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var data = db.Customers.FirstOrDefault(x => x.CustomerID == session.UserID);
                if (data.Password.Equals(HashMD5.MD5Hash(oldpass)))
                {
                    if (!String.IsNullOrEmpty(newpass))
                    {
                        data.Password = HashMD5.MD5Hash(newpass);
                    }
                    if (!String.IsNullOrEmpty(ten))
                    {
                        data.FullName = ten;
                    }
                    if (!String.IsNullOrEmpty(email))
                    {
                        data.EmailAddress = email;
                    }
                    if (!String.IsNullOrEmpty(phone))
                    {
                        data.PhoneNumber = phone;
                    }
                    if (birthday != null)
                    {
                        data.BirthDay = birthday;
                    }
                    if (gender != null)
                    {
                        data.Gender = gender;
                    }
                    db.SaveChanges();
                    ViewBag.message = "Cập nhật thành công";
                }
                else
                    ViewBag.message = "Mật khẩu không chính xác";
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Reg()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Reg(reg reg)
        {
            var data = db.Customers.ToList();
            foreach (var item in data)
            {
                if (item.EmailAddress.ToLower().Equals(reg.Email.ToLower())) 
                {
                    if (item.FullName.ToLower().Equals(reg.UserName.ToLower()))
                    {
                        ViewBag.Message = "Tài khoản đã tồn tại!!";
                        return View("Error");
                    }
                    ViewBag.Message = "Email đã tồn tại!!";
                    return View("Error");
                }
            }
            var dao = new userDao();
            Customer customer = new Customer();
            customer.FullName = reg.UserName;
            customer.EmailAddress = reg.Email;
            customer.Password = HashMD5.MD5Hash(reg.PassWord);
            var res = dao.Insert(customer);
            if (res == 2)
            {
                /*ViewBag.Message = "Đăng ký thành công";
                return View("regsuccess");*/
                var reslogin = dao.Login(reg.Email, HashMD5.MD5Hash(reg.PassWord));
                if (reslogin)
                {
                    var user = dao.GetById(reg.Email);
                    var usersession = new Userlogin();
                    usersession.UserID = user.CustomerID;
                    usersession.UserName = user.FullName;
                    Session.Add(CommonConstants.USER_SESSION, usersession);
                    /*return Redirect(Request.UrlReferrer.ToString());*/
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        public ActionResult Login(login model)
        {
            Session.Clear();
            if (ModelState.IsValid)
            {
                var dao = new userDao();
                var res = dao.Login(model.UserName, HashMD5.MD5Hash(model.PassWord));
                if (res)
                {
                    var user = dao.GetById(model.UserName);
                    var usersession = new Userlogin();
                    usersession.UserID = user.CustomerID;
                    usersession.UserName = user.FullName;
                    Session.Add(CommonConstants.USER_SESSION, usersession);
                    /*return Redirect(Request.UrlReferrer.ToString());*/
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Thông tin đăng nhập không chính xác";
                    return RedirectToAction("loginError");
                }
            }
            ViewBag.Message = "Thông tin đăng nhập không chính xác";
            return RedirectToAction("loginError");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");

        }
        public ActionResult loginError()
        {
            ViewBag.Message = "Thông tin đăng nhập không chính xác";
            return View("Error");

        }
        public ActionResult regerror()
        {
            ViewBag.Message = "Đăng ký không thành công";
            return View("Error");

        }
        public ActionResult regsuccess()
        {
            ViewBag.Message = "Đăng ký tại khoản thành công";
            return View();

        }

    }
}