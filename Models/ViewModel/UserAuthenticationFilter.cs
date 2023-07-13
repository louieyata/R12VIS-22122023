using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace R12VIS.Models.ViewModel
{
    public class UserAuthenticationFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] == null)
            {
                // User is not logged in or session expired
                // Perform the appropriate action, such as redirecting to the login page
                filterContext.Result = new RedirectResult("~/Users/Login");
            }
        }
    }
}