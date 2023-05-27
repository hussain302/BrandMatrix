using BrandMatrix.PresentationLayer.Common;
using Microsoft.AspNetCore.Mvc;

namespace BrandMatrix.PresentationLayer.Areas.Admin.Controllers
{
    [CustomAuthorization]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
