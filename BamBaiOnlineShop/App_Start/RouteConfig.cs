using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BamBaiOnlineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Send Email",
              url: "phan-hoi",
              defaults: new { controller = "Home", action = "SendEmail", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "Product Details",
               url: "chi-tiet/san-pham-{id}",
               defaults: new { controller = "Home", action = "DetailProduct", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Post Details",
              url: "chi-tiet/bai-viet-{id}",
              defaults: new { controller = "Home", action = "DetailPost", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "TrangChu",
               url: "Trang-chu",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "ResetPass",
               url: "Reset-PassWord",
               defaults: new { controller = "LoginUser", action = "ResetPass", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "SendCode",
              url: "Send-Code",
              defaults: new { controller = "LoginUser", action = "SendCode", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Log Reg",
              url: "Login-Register",
              defaults: new { controller = "LoginUser", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "ViewOrder",
              url: "Track-order",
              defaults: new { controller = "Home", action = "ViewOrder", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "Order Details",
             url: "chi-tiet/don-hang-{id}",
             defaults: new { controller = "Home", action = "DetailsOrder", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Profile",
             url: "Thong-tin-ca-nhan",
             defaults: new { controller = "LoginUser", action = "Profile", id = UrlParameter.Optional }
         );
            routes.MapRoute(
               name: "Blog",
               url: "Trang-chu/Blog",
               defaults: new { controller = "Home", action = "Blog", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Lien he",
               url: "Trang-chu/Lien-he",
               defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "WishList",
              url: "Trang-chu/Yeu-thich",
              defaults: new { controller = "Home", action = "WishList", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Shop",
              url: "Trang-chu/Shop",
              defaults: new { controller = "Home", action = "Shop", id = UrlParameter.Optional }
          );          
                routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "BamBaiOnlineShop.Controllers" }
            );
        }
    }
}
