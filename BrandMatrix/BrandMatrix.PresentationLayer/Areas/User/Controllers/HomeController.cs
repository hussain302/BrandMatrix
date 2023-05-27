using BrandMatrix.PresentationLayer.Common;
using Microsoft.AspNetCore.Mvc;

namespace BrandMatrix.PresentationLayer.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        [AuthorizeUser]
        public IActionResult HomePage()
        {
            ViewData["OrgName"] = HttpContext.Session.GetString("OrgName");

            return View();
        }
    }
}