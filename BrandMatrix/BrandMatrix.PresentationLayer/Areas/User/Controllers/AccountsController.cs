using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Models.DomainModels;
using BrandMatrix.Models.ViewModels;
using BrandMatrix.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace BrandMatrix.PresentationLayer.Areas.User.Controllers
{
    [Area("User")]
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> logger;
        private readonly IOrganizationRepository orgRepository;

        public AccountsController(ILogger<AccountsController> logger, IOrganizationRepository orgRepository)
        {
            this.logger = logger;
            this.orgRepository = orgRepository;
        }

        [HttpGet]
        public IActionResult SigninUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SigninUser(LoginModel model)
        {
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult SignupUserStepOne()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignupUserStepOne(Organizations model)
        {
            try
            {
                HttpContext.Session.Set("personalInfo", JsonSerializer.SerializeToUtf8Bytes(model));
                TempData["Success"] = "Step one completed successfully!";
                return RedirectToAction(nameof(SignupUserStepTwo));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(SigninUser));
            }
        }

        [HttpGet]
        public IActionResult SignupUserStepTwo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignupUserStepTwo(Organizations addressInfo)
        {
            try
            {
                var personalInfo = JsonSerializer.Deserialize<Organizations>(HttpContext.Session.Get("personalInfo"));
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@organizationName", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = personalInfo.OrganizationName
                    },
                    new SqlParameter("@ownerName", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = personalInfo.OwnerName
                    },
                    new SqlParameter("@email", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = personalInfo.Email
                    },
                    new SqlParameter("@phone", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = personalInfo.Phone
                    },
                    new SqlParameter("@website", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = personalInfo.Website
                    },
                    new SqlParameter("@address", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = addressInfo.Address
                    },
                    new SqlParameter("@city", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = addressInfo.City
                    },
                    new SqlParameter("@state", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = addressInfo.State
                    },
                    new SqlParameter("@country", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = addressInfo.Country
                    },
                    new SqlParameter("@zipCode", SqlDbType.NVarChar)
                    {
                           Direction = ParameterDirection.Input,
                           Value = addressInfo.ZipCode
                    },
                };
                var response = await orgRepository.CreateAsync("spCreateOrganization", parameters);

                if(response) TempData["Success"] = "Organization Created successfully. You will Contacted Soon!";
                else TempData["Error"] = "Account Creation Unsuccessful. Please Contact Customer Support";

                HttpContext.Session.Clear();
                return RedirectToAction(nameof(SigninUser));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(SigninUser));
            }
        }
    }
}
