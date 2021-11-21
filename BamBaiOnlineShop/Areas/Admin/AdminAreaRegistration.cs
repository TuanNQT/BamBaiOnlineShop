using System.Web.Mvc;

namespace BamBaiOnlineShop.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
              "QLDanhMucBlog",
              "Quan-ly-danh-muc-bai-viet",
              new { controller = "BlogType", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              "QLDanhMucSP",
              "Quan-ly-danh-muc-san-pham",
              new { controller = "CategoryProduct", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              "QLKhachHang",
              "Quan-ly-khach-hang",
              new { controller = "AdminAccount", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              "QLAccountAdmin",
              "Quan-ly-account-admin",
              new { controller = "AdminAccount", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              "QLBog",
              "Quan-ly-blog",
              new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              "QLProduct",
              "Quan-ly-san-pham",
              new { controller = "Product", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
               "DangKy",
               "Dang-ky",
               new { controller = "LoginSigninAdmin", action = "SignUp", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "DangNhap",
               "Dang-nhap",
               new { controller = "LoginSigninAdmin", action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "TrangChuAdmin",
               "Trang-chu-admin",
               new {controller ="Default", action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}