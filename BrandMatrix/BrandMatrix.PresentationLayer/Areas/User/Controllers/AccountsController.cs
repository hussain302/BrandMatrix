using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Models.DomainModels;
using BrandMatrix.Models.ViewModels;
using BrandMatrix.PresentationLayer.Common;
using BrandMatrix.Utils;
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
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        public AccountsController(ILogger<AccountsController> logger, IOrganizationRepository orgRepository,
            IConfiguration configuration, IUserRepository userRepository)
        {
            this.logger = logger;
            this.orgRepository = orgRepository;
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult SigninUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SigninUser(LoginModel model)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@email", SqlDbType.NVarChar) { Direction = ParameterDirection.Input, Value = model.Email }
                };
                Models.DomainModels.User user = await userRepository.LoginUserAsync("spLoginUser", parameters);           
                if (string.IsNullOrWhiteSpace(user.OrganizationName) || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(user.Password))
                {
                    TempData["Error"] = "User not found";
                    return RedirectToAction(nameof(SigninUser));
                }
                bool decryptedPassword = PasswordHasher.VerifyPassword(password:model.Password, hashedPassword: user.Password);
                string message = (decryptedPassword) ? $"{user.FirstName}, {user.LastName} logged in successfully!" : "You have entered as invalid password!";
                if (decryptedPassword) TempData["Success"] = message;
                else
                {
                    throw new Exception(message);
                }
                HttpContext.Session.SetString("LoginUser", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("OrgName", $"{user.OrganizationName}");
                return RedirectToAction("HomePage", "Home", new { area = "User" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(SigninUser));
            }
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

                if (response)
                {
                    var sender = configuration.GetValue<string>("senderEmail");

                    var password = configuration.GetValue<string>("senderEmailKey");

                    var msgBody = SendEmail.EmailTemp.Replace("{Owner Name}", personalInfo.OwnerName)
                                                     .Replace("{Organization Name}", personalInfo.OrganizationName);

                    var adminMsgBody = SendEmail.EmailTempAdmin.Replace("{Owner Name}", personalInfo.OwnerName)
                                                               .Replace("{Organization Name}", personalInfo.OrganizationName)
                                                               .Replace("{email}", personalInfo.Email)
                                                               .Replace("{phone}", personalInfo.Phone)
                                                               .Replace("{address}", addressInfo.Address)
                                                               .Replace("{city}", addressInfo.Address)
                                                               .Replace("{state}", addressInfo.Address)
                                                               .Replace("{country}", addressInfo.Address)
                                                               .Replace("{zipCode}", addressInfo.Address);

                    if (!string.IsNullOrWhiteSpace(sender) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(personalInfo.Email))
                    {
                        var res  = await SendEmail.PostAnEmail(
                                         senderEmail: sender,
                                         senderEmailKey: password,
                                         to: personalInfo.Email,
                                         subject: $"{personalInfo.OrganizationName} Registered Successfully!",
                                         body: msgBody);

                        if (res)
                        {
                            await SendEmail.PostAnEmail(
                                  senderEmail: sender,
                                  senderEmailKey: password,
                                  to: sender,
                                  subject: $"New Subscription Request {personalInfo.OrganizationName}",
                                  body: adminMsgBody);
                        }
                    }

                    TempData["Success"] = "Organization Created successfully. You will Contacted Soon!";
                }

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
        
        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("LoginUser");
                HttpContext.Session.Remove("OrgName");
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
