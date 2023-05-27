using Microsoft.AspNetCore.Mvc;

namespace BrandMatrix.PresentationLayer.Areas.User.Controllers
{
    [Area("User")]
    public class UsersController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }
    }
}
