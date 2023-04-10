using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BrandMatrix.PresentationLayer.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionRepository subRepository;
        private readonly IOrganizationRepository orgRepository;
        private readonly ILogger<SubscriptionsController> logger;

        public SubscriptionsController(ISubscriptionRepository subRepository,
            IOrganizationRepository orgRepository, ILogger<SubscriptionsController> logger)
        {
            this.subRepository = subRepository;
            this.orgRepository = orgRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Manage()
        {
            try
            {
                return View(await subRepository.GetAllAsync("spFetchAllSubscriptions", null));
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
                    ViewData["Title"] = "Create Subscriptions";
                    ViewData["button-color"] = "btn btn-primary";
                    return View();
                }
                else
                {
                    ViewData["Title"] = "Update Subscriptions";
                    ViewData["button-color"] = "btn btn-success";

                    var model = await subRepository.GetByIdAsync("spFetchSubscriptionById", new List<SqlParameter>
                    {
                         new SqlParameter("@subscriptionId", SqlDbType.Int)
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
        public async Task<IActionResult> CreateOrEdit(Subscriptions model)
        {
            try
            {
                bool res = false;
                if (model.SubscriptionId > 0)
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@subscriptionId", SqlDbType.Int)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.SubscriptionId
                        },
                        new SqlParameter("@Plan", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Plan
                        },
                        new SqlParameter("@Description", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Description
                        },
                        new SqlParameter("@Price", SqlDbType.Decimal)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Price
                        },
                        new SqlParameter("@DurationInDays", SqlDbType.Int)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.DurationInDays
                        }
                    };
                    res = await subRepository.UpdateAsync("spUpdateSubscription", parameters);
                }
                else
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@organizationId", SqlDbType.Int)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.OrganizationId
                        },
                        new SqlParameter("@Plan", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Plan
                        },
                        new SqlParameter("@Description", SqlDbType.NVarChar)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Description
                        },
                        new SqlParameter("@Price", SqlDbType.Decimal)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.Price
                        },
                        new SqlParameter("@DurationInDays", SqlDbType.Int)
                        {
                               Direction = ParameterDirection.Input,
                               Value = model.DurationInDays
                        }
                    };
                    res = await subRepository.CreateAsync("spCreateSubscription", parameters);
                }
                if (res is false) TempData["Error"] = $"{model.Plan} - Subscription Didn't Saved!";
                else TempData["Success"] = $"{model.Plan} - Subscription Saved!";
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
                var model = await subRepository.GetByIdAsync("spFetchSubscriptionById", new List<SqlParameter>
                {
                     new SqlParameter("@SubscriptionId", SqlDbType.Int)
                     {
                           Direction = ParameterDirection.Input,
                           Value = id
                     }
                });

                if (page.ToLower() is "delete")
                {
                    ViewData["Title"] = "Delete Subscription";
                    ViewData["button-color"] = "btn btn-danger";
                }
                else
                {
                    ViewData["Title"] = "Details Subscription";
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
        public async Task<IActionResult> DeleteOrDetails(Subscriptions model)
        {
            try
            {
                var res = await subRepository.DeleteAsync("spDeleteSubscription", new List<SqlParameter>
                {
                     new SqlParameter("@SubscriptionId", SqlDbType.Int)
                     {
                           Direction = ParameterDirection.Input,
                           Value = model.SubscriptionId
                     }
                });

                if (res is false) TempData["Error"] = $"{model.Plan} - Subscription Didn't Deleted!";
                else TempData["Success"] = $"{model.Plan} - Subscription Deleted Permanently!";
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
