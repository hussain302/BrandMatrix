using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrandMatrix.PresentationLayer.Common
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var user = session.GetString("adminUser");
            var isAdmin = session.GetBool("IsAdmin");

            if (string.IsNullOrEmpty(user) || !isAdmin)
            {
                // User is not authorized, redirect or return unauthorized response
                context.Result = new RedirectToActionResult("SignInAdmin", "Accounts", new { area = "Admin" });
            }

            base.OnActionExecuting(context);
        }
    }

    public class AuthorizeUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var user = session.GetString("LoginUser");
            
            if (string.IsNullOrEmpty(user))
            {
                // User is not authorized, redirect or return unauthorized response
                context.Result = new RedirectToActionResult("SigninUser", "Accounts", new { area = "User" });
            }

            base.OnActionExecuting(context);
        }
    }

}
