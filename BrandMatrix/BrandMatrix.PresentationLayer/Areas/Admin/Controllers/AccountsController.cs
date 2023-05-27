using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using BrandMatrix.Models.DomainModels;
using BrandMatrix.PresentationLayer.Common;

namespace BrandMatrix.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly IAdminRepository adminRepository;
        private readonly ILogger<AccountsController> logger;

        public AccountsController(IAdminRepository adminRepository, ILogger<AccountsController> logger)
        {
            this.adminRepository = adminRepository;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult SignInAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignInAdmin(Models.DomainModels.Admin loginModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginModel.Username) || string.IsNullOrWhiteSpace(loginModel.Password)) throw new Exception("Username/Password cant be empty!");

                bool isAuthenticated = false;
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Username", SqlDbType.NVarChar) { Direction = ParameterDirection.Input, Value = loginModel.Username },
                    new SqlParameter("@Password", SqlDbType.NVarChar) { Direction = ParameterDirection.Input, Value = loginModel.Password },
                    new SqlParameter("@IsAuthenticated", SqlDbType.Bit) { Direction = ParameterDirection.Output, Value = isAuthenticated },
                };
                Models.DomainModels.Admin admin = await adminRepository.LoginAdminAsync("spLoginAdmin", parameters);
                isAuthenticated = Convert.ToBoolean(parameters[2].Value);
                if (isAuthenticated is false)
                {
                    throw new Exception("You have entered as invalid Username/Password!");
                }
                HttpContext.Session.SetString("adminUser", "adminUser");
                HttpContext.Session.SetBool("IsAdmin", true);
                TempData["Success"] = $"Admin logged in successfully!";
                return RedirectToAction("Dashboard", "Home", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(SignInAdmin));
            }
        }

        [HttpGet]
        public IActionResult LogoutAdmin()
        {
            try
            {
                HttpContext.Session.Remove("User");
                HttpContext.Session.Remove("IsAdmin");
                return RedirectToAction(nameof(SignInAdmin));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(SignInAdmin));
            }
        }

    }
}
