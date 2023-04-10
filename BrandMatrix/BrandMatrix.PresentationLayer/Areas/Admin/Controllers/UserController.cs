using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BrandMatrix.PresentationLayer.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IOrganizationRepository orgRepository;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUserRepository userRepository,
            IOrganizationRepository orgRepository, ILogger<UsersController> logger)
        {
            this.userRepository = userRepository;
            this.orgRepository = orgRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Manage()
        {
            try
            {
                return View(await userRepository.GetAllAsync("spFetchAllUsers", null));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(Manage));
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            try
            {
                ViewBag.Orgs = await orgRepository.GetAllAsync("spFetchAllOrganizations", null);
                
                if (id == null)
                {
                    ViewData["Title"] = "Create User";
                    ViewData["button-color"] = "btn btn-primary";
                    return View();
                }
                else
                {
                    ViewData["Title"] = "Update User";
                    ViewData["button-color"] = "btn btn-success";

                    var model = await userRepository.GetByIdAsync("spFetchUsersbyUserId", new List<SqlParameter>
                    {
                         new SqlParameter("@UserId", SqlDbType.Int)
                         {
                               Direction = ParameterDirection.Input,
                               Value = id
                         }
                    });

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Manage");
            }
        }

        [HttpPost]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> CreateOrEdit(Models.DomainModels.User model)
        {
            try
            {
                bool res = false;
                //Edit
                if (model.UserId > 0)
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@UserId", SqlDbType.Int)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.UserId
                        },
                        new SqlParameter("@FirstName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.FirstName
                        },
                        new SqlParameter("@LastName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.LastName
                        },
                        new SqlParameter("@Email", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Email
                        },
                        new SqlParameter("@Role", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Role
                        },
                        new SqlParameter("@PhoneNumber", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.PhoneNumber
                        }
                    };
                    res = await userRepository.UpdateAsync("spUpdateUser", parameters);
                }
                //Create
                else
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@organizationId", SqlDbType.Int)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.OrganizationId
                        },
                        new SqlParameter("@FirstName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.FirstName
                        },
                        new SqlParameter("@LastName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.LastName
                        },
                        new SqlParameter("@Email", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Email
                        },
                        new SqlParameter("@Role", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Role
                        },
                        new SqlParameter("@PhoneNumber", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.PhoneNumber
                        },
                        new SqlParameter("@Password", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = PasswordHasher.HashPassword(model.Password)
                        }
                    };
                    res = await userRepository.CreateAsync("spCreateUser", parameters);
                }
                if (res is false) TempData["Error"] = $"{model.Email} - User Didn't Saved!";
                else TempData["Success"] = $"{model.Email} - User Saved!";
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Manage");
            }
        }


        [HttpGet]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> DeleteOrDetails(int? id, string page)
        {
            try
            {
                var model = await userRepository.GetByIdAsync("spFetchUsersbyUserId", new List<SqlParameter>
                {
                     new SqlParameter("@UserId", SqlDbType.Int)
                     {
                           Direction = ParameterDirection.Input,
                           Value = id
                     }
                });

                if (page.ToLower() is "delete")
                {
                    ViewData["Title"] = "Delete User";
                    ViewData["button-color"] = "btn btn-danger";
                }
                else
                {
                    ViewData["Title"] = "Details User";
                    ViewData["button-color"] = "btn btn-warning";
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Manage");
            }
        }

        [HttpPost]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> DeleteOrDetails(BrandMatrix.Models.DomainModels.User model)
        {
            try
            {
                var res = await userRepository.DeleteAsync("spDeleteUser", new List<SqlParameter>
                {
                     new SqlParameter("@UserId", SqlDbType.Int)
                     {
                           Direction = ParameterDirection.Input,
                           Value = model.UserId
                     }
                });

                if (res is false) TempData["Error"] = $"{model.Email} - User Didn't Deleted!";
                else TempData["Success"] = $"{model.Email} - User Deleted Permanently!";
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Manage");
            }
        }
    }
}
