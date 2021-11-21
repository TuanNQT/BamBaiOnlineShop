using BamBaiOnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BamBaiOnlineShop.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        protected override void OnActionExecuting(ActionExecutingContext
            filterContext)
        {
            var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
               System.Web.Routing.RouteValueDictionary(new
               {
                   Controller = "LoginSigninAdmin",
                   action =
               "Index",
                   Areas = "Admin"
               }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}