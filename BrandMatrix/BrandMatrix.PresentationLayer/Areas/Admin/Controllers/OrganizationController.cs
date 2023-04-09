using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BrandMatrix.PresentationLayer.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationRepository orgRepository;
        private readonly ILogger<OrganizationController> logger;

        public OrganizationController(IOrganizationRepository orgRepository, ILogger<OrganizationController> logger)
        {
            this.orgRepository = orgRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Manage()
        {
            try
            {
                return View(await orgRepository.GetAllAsync("spFetchAllOrganizations", null));
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
                if (id == null)
                {
                    ViewData["Title"] = "Create";
                    ViewData["button-color"] = "btn btn-primary";
                    return View();
                }
                else
                {
                    ViewData["Title"] = "Update";
                    ViewData["button-color"] = "btn btn-success";

                    var model = await orgRepository.GetByIdAsync("spFetchOrganizationById", new List<SqlParameter>
                    {
                         new SqlParameter("@organizationId", SqlDbType.Int)
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
        public async Task<IActionResult> CreateOrEdit(Organizations model)
        {
            try
            {
                bool res = false;
                if (model.OrganizationId > 0)
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@organizationId", SqlDbType.Int)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.OrganizationId
                        },
                        new SqlParameter("@organizationName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.OrganizationName
                        },
                        new SqlParameter("@ownerName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.OwnerName
                        },
                        new SqlParameter("@email", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Email
                        },
                        new SqlParameter("@phone", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Phone
                        },
                        new SqlParameter("@website", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Website
                        },
                        new SqlParameter("@address", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Address
                        },
                        new SqlParameter("@city", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.City
                        },
                        new SqlParameter("@state", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.State
                        },
                        new SqlParameter("@country", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Country
                        },
                        new SqlParameter("@zipCode", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.ZipCode
                        },
                    };
                    res = await orgRepository.UpdateAsync("spUpdateOrganization", parameters);
                }
                else
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@organizationName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.OrganizationName
                        },
                        new SqlParameter("@ownerName", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.OwnerName
                        },
                        new SqlParameter("@email", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Email
                        },
                        new SqlParameter("@phone", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Phone
                        },
                        new SqlParameter("@website", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Website
                        },
                        new SqlParameter("@address", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Address
                        },
                        new SqlParameter("@city", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.City
                        },
                        new SqlParameter("@state", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.State
                        },
                        new SqlParameter("@country", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Country
                        },
                        new SqlParameter("@zipCode", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.ZipCode
                        },
                    };
                    res = await orgRepository.CreateAsync("spCreateOrganization", parameters);
                }
                if (res is false) TempData["Error"] = $"{model.OrganizationName} - Organization Didn't Saved!";
                else TempData["Success"] = $"{model.OrganizationName} - Organization Saved!";
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
                var model = await orgRepository.GetByIdAsync("spFetchOrganizationById", new List<SqlParameter>
                {
                     new SqlParameter("@organizationId", SqlDbType.Int)
                     {
                           Direction = ParameterDirection.Input,
                           Value = id
                     }
                });

                if (page.ToLower() is "delete")
                {
                    ViewData["Title"] = "Delete";
                    ViewData["button-color"] = "btn btn-danger";
                }
                else
                {
                    ViewData["Title"] = "Details";
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
        public async Task<IActionResult> DeleteOrDetails(Organizations model)
        {
            try
            {
                var res = await orgRepository.DeleteAsync("spDeleteOrganization", new List<SqlParameter>
                {
                     new SqlParameter("@organizationId", SqlDbType.Int)
                     {
                           Direction = ParameterDirection.Input,
                           Value = model.OrganizationId
                     }
                });
                if (res is false) TempData["Error"] = $"{model.OrganizationName} - Organization Didn't Deleted!";
                else TempData["Success"] = $"{model.OrganizationName} - Organization Deleted Permanently!";
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
