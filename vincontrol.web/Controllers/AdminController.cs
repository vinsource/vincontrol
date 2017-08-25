using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.Vinsocial.Forms.TeamManagement;
using vincontrol.Constant;
using VINControl.Craigslist;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;
using Setting = vincontrol.Data.Model.Setting;
using EncryptionHelper = vincontrol.Helper.EncryptionHelper;
using vincontrol.Helper;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.Controllers
{
    public class AdminController : SecurityController
    {
        private const string PermissionCode = "ADMIN";
        private const string AcceptedValues = "READONLY, ALLACCESS";

        private CraigslistService _craigslistService;
        private ICommonManagementForm _commonManagementForm;
        private IDealerManagementForm _dealerManagementForm;
        private IAccountManagementForm _accountManagementForm;
        private IInventoryManagementForm _inventoryManagementForm;

        public AdminController()
        {
            _craigslistService = new CraigslistService();
            _commonManagementForm = new CommonManagementForm();
            _dealerManagementForm=new DealerManagementForm();
            _accountManagementForm=new AccountManagementForm();
            _inventoryManagementForm=new InventoryManagementForm();
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult AdminSecurity()
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
                var dealerSetting = dealer.DealerSetting;
                var yearList = _dealerManagementForm.GetYearList(dealer.DealershipId);
                SessionHandler.CanViewBucketJumpReport = dealerSetting != null && dealerSetting.CanViewBucketJumpReport;

                var userList = _accountManagementForm.GetUserList(dealer.DealershipId);

                if (dealerSetting != null)
                {
                    var viewModel = new AdminViewModel(dealer)
                    {
                        YearsList = SelectListHelper.InitialYearList(yearList),
                        MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        SoldActionList = SelectListHelper.InitalSoldOutList(dealerSetting.SoldOut),
                        Users = userList.AsEnumerable(),
                        SortSetList = SelectListHelper.InitalSortSetList(dealerSetting.InventorySorting),
                        EncodeCarFaxPassword = EncryptionHelper.EncryptString(dealerSetting.CarFaxPassword),
                        EncodeManheimPassword = EncryptionHelper.EncryptString(dealerSetting.ManheimPassword),
                        EncodeKellyPassword = EncryptionHelper.EncryptString(dealerSetting.KellyPassword),
                        EncodeBlackBookPassword = EncryptionHelper.EncryptString(dealerSetting.BlackBookPassword)
                    };


                    if (SessionHandler.DealerGroup != null)
                    {
                        viewModel.DealerGroup = SessionHandler.DealerGroup;
                        SessionHandler.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                        viewModel.MutipleDealer = true;
                    }
                    else
                        viewModel.DealerList = SelectListHelper.InitialDealerList();

                    viewModel.IntervalList = SelectListHelper.InitialIntervalListForAdmin(dealerSetting.IntervalBucketJump);
                    viewModel.LandingPage = "Default";
                    viewModel.DealerCraigslistSetting.EncodePassword = EncryptionHelper.EncryptString(viewModel.DealerCraigslistSetting.Password);
                    return View("AdminControl", viewModel);
                }
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
            return null;
        }

        public ActionResult LoadCraigslistSetting()
        {
            var viewModel = new AdminViewModel(SessionHandler.Dealer);
            viewModel.DealerCraigslistSetting.States = _commonManagementForm.GetStates();
            return PartialView("_CraigslistSettingPartial", viewModel);
        }

        public ActionResult GetBuyerGuideList()
        {
            var viewModel = new AdminViewModel
                {
                    WarrantyTypes = _dealerManagementForm.GetWarrantyTypeList(SessionHandler.Dealer.DealershipId)
                };
            viewModel.BasicWarrantyTypes = viewModel.WarrantyTypes.Any() ? viewModel.WarrantyTypes.Select(i => new SelectListItem { Selected = false, Text = i.Name, Value = i.Id.ToString(CultureInfo.InvariantCulture) }).ToList() : new List<SelectListItem>();
            return PartialView("BuyerGuideData", viewModel);

        }

        public ActionResult GetRebateList()
        {
            var dealer = SessionHandler.Dealer;
            var distincList = _dealerManagementForm.GetRebates(dealer.DealershipId);
            var rebateList = new List<ManafacturerRebateDistinctModel>();
            rebateList.AddRange(distincList.Select(tmp => new ManafacturerRebateDistinctModel
                                                              {
                                                                  UniqueId = tmp.RebateId,
                                                                  Year = tmp.Year,
                                                                  Make = tmp.Make,
                                                                  Model = tmp.Model,
                                                                  Trim = tmp.Trim,
                                                                  Disclaimer = tmp.Disclaimer,
                                                                  BodyType = tmp.BodyType,
                                                                  RebateAmount = tmp.ManufactureReabte,
                                                                  CreateDate =
                                                                      tmp.DateStamp != null
                                                                          ? tmp.DateStamp.Value
                                                                          : DateTime.Now,
                                                                  ExpirationDate =
                                                                      tmp.ExpiredDate != null
                                                                          ? tmp.ExpiredDate.Value
                                                                          : DateTime.Now
                                                              }));

            var viewModel = new AdminViewModel
                {
                    RebateList = rebateList,
                };
            return PartialView("RebateList", viewModel);

        }

        [HttpPost]
        public ActionResult GetRebateListSort(int type, bool isUp)
        {

            var dealer = SessionHandler.Dealer;
            var distincList = _dealerManagementForm.GetRebates(dealer.DealershipId);
            var rebateList = new List<ManafacturerRebateDistinctModel>();
            rebateList.AddRange(distincList.Select(tmp => new ManafacturerRebateDistinctModel
            {
                UniqueId = tmp.RebateId,
                Year = tmp.Year,
                Make = tmp.Make,
                Model = tmp.Model,
                Trim = tmp.Trim,
                Disclaimer = tmp.Disclaimer,
                BodyType = tmp.BodyType,
                RebateAmount = tmp.ManufactureReabte,
                CreateDate =
                    tmp.DateStamp != null
                        ? tmp.DateStamp.Value
                        : DateTime.Now,
                ExpirationDate =
                    tmp.ExpiredDate != null
                        ? tmp.ExpiredDate.Value
                        : DateTime.Now
            }));

            switch (type)
            {
                case 1:
                    rebateList = isUp ? rebateList.OrderBy(x => x.Year).ToList() : rebateList.OrderByDescending(x => x.Year).ToList();
                    break;
                case 2:
                    rebateList = isUp ? rebateList.OrderBy(x => x.Make).ToList() : rebateList.OrderByDescending(x => x.Make).ToList();
                    break;
                case 3:
                    rebateList = isUp ? rebateList.OrderBy(x => x.Model).ToList() : rebateList.OrderByDescending(x => x.Model).ToList();
                    break;
                case 4:
                    rebateList = isUp ? rebateList.OrderBy(x => x.Trim).ToList() : rebateList.OrderByDescending(x => x.Trim).ToList();
                    break;
                case 5:
                    rebateList = isUp ? rebateList.OrderBy(x => x.BodyType).ToList() : rebateList.OrderByDescending(x => x.BodyType).ToList();
                    break;
                case 6:
                    rebateList = isUp ? rebateList.OrderBy(x => x.RebateAmount).ToList() : rebateList.OrderByDescending(x => x.RebateAmount).ToList();
                    break;
                case 7:
                    rebateList = isUp ? rebateList.OrderBy(x => x.ExpirationDate).ToList() : rebateList.OrderByDescending(x => x.ExpirationDate).ToList();
                    break;
                case 8:
                    rebateList = isUp ? rebateList.OrderBy(x => x.CreateDate).ToList() : rebateList.OrderByDescending(x => x.CreateDate).ToList();
                    break;
                default:
                    rebateList = rebateList.OrderBy(x => x.Year).ToList();
                    break;
            }
            var viewModel = new AdminViewModel
                {
                    RebateList = rebateList,
                };
            return PartialView("RebateList", viewModel);

        }


        public ActionResult GetUserRightList(string search)
        {
            if (SessionHandler.Dealer != null)
            {
                var viewModel = VincontrolLinqHelper.GetUserRoleViewModel(SessionHandler.Dealer.DealershipId, search);
                return PartialView("UserRightList", viewModel);
            }
            return Content(String.Empty);
        }


        public ActionResult GetButtonPermissionList(string search)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
                var viewModel = new AdminViewModel
                    {
                        ButtonPermissions =_dealerManagementForm.GetButtonList(dealer.DealershipId, "Profile"),
                    };

                return PartialView("ButtonPermissionList", viewModel);
            }
            return Content(String.Empty);
        }

      
        public ActionResult UpdateStockingGuideSetting(Setting setting)
        {
            if (SessionHandler.Dealer != null)
            {
                SQLHelper.UpdateStockingGuideSetting(SessionHandler.Dealer.DealershipId, setting);

                //SessionHandler.Dealer.DealerSetting.BrandName = setting.BrandName;
                SessionHandler.Dealer.DealerSetting.AverageCost = setting.AverageCost ?? 0;
                SessionHandler.Dealer.DealerSetting.AverageProfitUsage = setting.AverageProfitUsage ?? 0;

                SessionHandler.Dealer.DealerSetting.VehicleTireCost = setting.TireCost ?? 0;
                SessionHandler.Dealer.DealerSetting.LightBulbCost = setting.LightBulbCost ?? 0;
                SessionHandler.Dealer.DealerSetting.FrontWindShieldCost = setting.FrontWindShieldCost ?? 0;
                SessionHandler.Dealer.DealerSetting.RearWindShieldCost = setting.RearWindShieldCost ?? 0;
                SessionHandler.Dealer.DealerSetting.DriverWindowCost = setting.DriverWindowCost ?? 0;
                SessionHandler.Dealer.DealerSetting.PassengerWindowCost = setting.PassengerWindowCost ?? 0;
                SessionHandler.Dealer.DealerSetting.DriverSideMirrorCost = setting.DriverSideMirrorCost ?? 0;
                SessionHandler.Dealer.DealerSetting.PassengerSideMirrorCost = setting.PassengerSideMirrorCost ?? 0;
                SessionHandler.Dealer.DealerSetting.ScratchCost = setting.ScratchCost ?? 0;
                SessionHandler.Dealer.DealerSetting.MajorScratchCost = setting.MajorScratchCost ?? 0;
                SessionHandler.Dealer.DealerSetting.DentCost = setting.DentCost ?? 0;
                SessionHandler.Dealer.DealerSetting.MajorDentCost = setting.MajorDentCost ?? 0;
                SessionHandler.Dealer.DealerSetting.PaintDamageCost = setting.PaintDamageCost ?? 0;
                SessionHandler.Dealer.DealerSetting.RepaintedPanelCost = setting.RepaintedPanelCost ?? 0;
                SessionHandler.Dealer.DealerSetting.RustCost = setting.RustCost ?? 0;
                SessionHandler.Dealer.DealerSetting.AcidentCost = setting.AcidentCost ?? 0;
                SessionHandler.Dealer.DealerSetting.MissingPartsCost = setting.MissingPartsCost ?? 0;

                SessionHandler.Dealer.DealerSetting.VehicleTireRetail = setting.TireRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.LightBulbRetail = setting.LightBulbRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.FrontWindShieldRetail = setting.FrontWindShieldRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.RearWindShieldRetail = setting.RearWindShieldRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.DriverWindowRetail = setting.DriverWindowRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.PassengerWindowRetail = setting.PassengerWindowRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.DriverSideMirrorRetail = setting.DriverSideMirrorRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.PassengerSideMirrorRetail = setting.PassengerSideMirrorRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.ScratchRetail = setting.ScratchRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.MajorScratchRetail = setting.MajorScratchRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.DentRetail = setting.DentRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.MajorDentRetail = setting.MajorDentRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.PaintDamageRetail = setting.PaintDamageRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.RepaintedPanelRetail = setting.RepaintedPanelRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.RustRetail = setting.RustRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.AcidentRetail = setting.AcidentRetail ?? 0;
                SessionHandler.Dealer.DealerSetting.MissingPartsRetail = setting.MissingPartsRetail ?? 0;

                if (setting.AverageProfitUsage == 1)
                {
                    SessionHandler.Dealer.DealerSetting.AverageProfit = setting.AverageProfit ?? 0;
                }
                else
                {
                    SessionHandler.Dealer.DealerSetting.AverageProfitPercentage = setting.AverageProfit ?? 0;
                }

                return RedirectToAction("AdminSecurity");
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult UpdateContentSetting(AdminViewModel admin)
        {
            if (SessionHandler.Dealer != null)
            {
                SQLHelper.UpdateContentAppSetting(SessionHandler.Dealer.DealershipId, admin);
                SessionHandler.Dealer.DealerSetting.SalePriceWsNotificationText = admin.DealerSetting.SalePriceWsNotificationText;
                SessionHandler.Dealer.DealerSetting.DealerDiscountWSNotificationText = admin.DealerSetting.DealerDiscountWSNotificationText;
                SessionHandler.Dealer.DealerSetting.ManufacturerReabteWsNotificationText = admin.DealerSetting.ManufacturerReabteWsNotificationText;
                SessionHandler.Dealer.DealerSetting.RetailPriceWSNotificationText = admin.DealerSetting.RetailPriceWSNotificationText;
                SessionHandler.Dealer.DealerSetting.EnableAutoDescription = admin.DealerSetting.EnableAutoDescription;
                SessionHandler.InventoryViewInfo.SortFieldName = SessionHandler.Dealer.DealerSetting.InventorySorting = admin.DealerSetting.InventorySorting;
                SessionHandler.InventoryViewInfo.IsUp = true;
                SessionHandler.Dealer.DealerSetting.PriceVariance = admin.DealerSetting.PriceVariance;
                SessionHandler.Dealer.DealerSetting.SoldOut = admin.DealerSetting.SoldOut;
                SessionHandler.Dealer.DealerSetting.EbayContactInfoEmail = admin.DealerSetting.EbayContactInfoEmail;
                SessionHandler.Dealer.DealerSetting.EbayContactInfoName = admin.DealerSetting.EbayContactInfoName;
                SessionHandler.Dealer.DealerSetting.EbayContactInfoPhone = admin.DealerSetting.EbayContactInfoPhone;
                SessionHandler.Dealer.Email = admin.Email;

                SessionHandler.Dealer.DealerSetting.BrandName = admin.SelectedBrandName;


                return RedirectToAction("AdminSecurity");
            }
            return RedirectToAction("LogOff", "Account");
        }
        public ActionResult UpdateNotificationSetting(AdminViewModel admin)
        {
            if (SessionHandler.Dealer != null)
            {
                SQLHelper.UpdateNotificationAppSetting(SessionHandler.Dealer.DealershipId, admin);

                SessionHandler.Dealer.DealerSetting.FirstTimeRangeBucketJump = admin.DealerSetting.FirstTimeRangeBucketJump;
                SessionHandler.Dealer.DealerSetting.SecondTimeRangeBucketJump = admin.DealerSetting.SecondTimeRangeBucketJump;
                SessionHandler.Dealer.DealerSetting.IntervalBucketJump = admin.DealerSetting.IntervalBucketJump;

                return RedirectToAction("AdminSecurity");
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult UpdatePasswordSetting(AdminViewModel admin)
        {
            if (SessionHandler.Dealer != null)
            {
                if (!string.IsNullOrEmpty(admin.EncodeManheimPassword))
                    admin.DealerSetting.ManheimPassword = !admin.ManheimPasswordChanged ? EncryptionHelper.DecryptString(admin.EncodeManheimPassword) : admin.EncodeManheimPassword;

                if (!string.IsNullOrEmpty(admin.EncodeCarFaxPassword))
                    admin.DealerSetting.CarFaxPassword = !admin.CarFaxPasswordChanged ? EncryptionHelper.DecryptString(admin.EncodeCarFaxPassword) : admin.EncodeCarFaxPassword;

                if (!string.IsNullOrEmpty(admin.EncodeKellyPassword))
                    admin.DealerSetting.KellyPassword = !admin.KellyPasswordChanged ? EncryptionHelper.DecryptString(admin.EncodeKellyPassword) : admin.EncodeKellyPassword;

                if (!string.IsNullOrEmpty(admin.EncodeBlackBookPassword))
                    admin.DealerSetting.BlackBookPassword = !admin.BlackBookPasswordChanged ? EncryptionHelper.DecryptString(admin.EncodeBlackBookPassword) : admin.EncodeBlackBookPassword;

                if (!string.IsNullOrEmpty(admin.DealerCraigslistSetting.EncodePassword))
                    admin.DealerCraigslistSetting.Password = !admin.DealerCraigslistSetting.PasswordChanged
                        ? EncryptionHelper.DecryptString(admin.DealerCraigslistSetting.EncodePassword)
                        : admin.DealerCraigslistSetting.EncodePassword;

                SQLHelper.UpdatePasswordAppSetting(SessionHandler.Dealer.DealershipId, admin);

                SessionHandler.Dealer.DealerSetting.Manheim = admin.DealerSetting.Manheim;
                SessionHandler.Dealer.DealerSetting.ManheimPassword = admin.DealerSetting.ManheimPassword;
                SessionHandler.Dealer.DealerSetting.CarFax = admin.DealerSetting.CarFax;
                SessionHandler.Dealer.DealerSetting.CarFaxPassword = admin.DealerSetting.CarFaxPassword;
                SessionHandler.Dealer.DealerSetting.KellyBlueBook = admin.DealerSetting.KellyBlueBook;
                SessionHandler.Dealer.DealerSetting.KellyPassword = admin.DealerSetting.KellyPassword;
                SessionHandler.Dealer.DealerSetting.BlackBook = admin.DealerSetting.BlackBook;
                SessionHandler.Dealer.DealerSetting.BlackBookPassword = admin.DealerSetting.BlackBookPassword;
                SessionHandler.Dealer.DealerCraigslistSetting.Email = admin.DealerCraigslistSetting.Email ?? string.Empty;
                SessionHandler.Dealer.DealerCraigslistSetting.Password = admin.DealerCraigslistSetting.Password ?? string.Empty;
                SessionHandler.Dealer.DealerCraigslistSetting.State = admin.DealerCraigslistSetting.State ?? string.Empty;
                SessionHandler.Dealer.DealerCraigslistSetting.City = admin.DealerCraigslistSetting.CityUrl.Equals("0") ? string.Empty : admin.DealerCraigslistSetting.City;
                SessionHandler.Dealer.DealerCraigslistSetting.CityUrl = admin.DealerCraigslistSetting.CityUrl;
                SessionHandler.Dealer.DealerCraigslistSetting.Location = admin.DealerCraigslistSetting.LocationId.Equals(0) ? string.Empty : admin.DealerCraigslistSetting.Location;
                SessionHandler.Dealer.DealerCraigslistSetting.LocationId = admin.DealerCraigslistSetting.LocationId;

                return RedirectToAction("AdminSecurity");
            }
            return RedirectToAction("LogOff", "Account");
        }
        public ActionResult UpdatePassword(string pass, int userId)
        {
            _accountManagementForm.UpdatePass(userId,pass);
           
            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }
        public ActionResult UpdateEmail(string email, int userId)
        {
            var userEmailExist = _accountManagementForm.CheckExistingActiveEmail(email);

            if (!userEmailExist)
            {
                _accountManagementForm.UpdateEmail(userId,email);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");
                }
            }
            else
            {
                var user = _accountManagementForm.GetUser(userId);
                return Json(user.Email);
            }

            return Json("Not Updated");
        }
        public ActionResult UpdateCellPhone(string cellPhone, int userId)
        {
            if (SessionHandler.Dealer != null)
            {
                _accountManagementForm.UpdatePhone(userId, cellPhone);
                
                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult ChangeRole(int role, int userId)
        {
            if (SessionHandler.Dealer != null)
            {
                var teamManagement = new TeamManagementForm();
                var commonManagement = new vincontrol.Application.Vinchat.Forms.CommonManagement.CommonManagementForm();

                var currentUser = _accountManagementForm.GetUserById(userId);

                if (currentUser != null && currentUser.TeamId == null)
                {
                    var firstOrDefault = _accountManagementForm.GetUserPermission(userId,
                        SessionHandler.Dealer.DealershipId);
                    if (firstOrDefault != null)
                    {
                        var previousRole = (short) firstOrDefault.RoleId;

                        if ((role == Constanst.RoleType.Manager || role == Constanst.RoleType.Admin) &&
                            previousRole == Constanst.RoleType.Employee)
                        {
                            //Add friend for this user
                            teamManagement.AddRosterUser(currentUser);
                        }

                        if ((previousRole == Constanst.RoleType.Manager ||
                             previousRole == Constanst.RoleType.Admin) &&
                            role == Constanst.RoleType.Employee)
                        {
                            //Add friend for this user
                            commonManagement.DeleteRosterUsers(currentUser.UserName);
                        }
                    }
                }


                _accountManagementForm.ChangeRole(role, userId);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult DeleteUser(int? userId)
        {
            if (userId == null)
            {
                return Json("Not Updated");
            }

            if (SessionHandler.Dealer != null)
            {
                _accountManagementForm.DeleteUser(userId.Value);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        public ActionResult CheckUserExist(string userName, string userEmail, int dealerId)
        {
            if (SessionHandler.Dealer != null)
            {
                var userNameExist = _accountManagementForm.CheckExistingUsername(userName);
                var userEmailExist = _accountManagementForm.CheckExistingActiveEmail(userEmail);

                var user = new UserRoleViewModel();

                if (userNameExist)
                {
                    user.IsUserNameExist = "Exist";
                }

                if (userEmailExist)
                {
                    user.IsUserEmailExist = "Exist";
                }

                return Json(user);
            }
            else
            {
                var user = new UserRoleViewModel
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult AddSingleUser(string name, string userName, string password, string email, string cellPhone, int roleId)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
                var user = new UserViewModel()
                {
                    Name = name,
                    UserName = userName.ToLower(),
                    Password = password,
                    Email = email,
                    Phone = cellPhone,
                    RoleId = roleId,
                    DealerId = dealer.DealershipId,
                  
                };

                _accountManagementForm.AddUser(user);

                if (Request.IsAjaxRequest())
                {
                    return Json(user);

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult AddUser(string name, string userName, string password, string email, string cellPhone,
            int roleId, string dealerList, int defaultLogin)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;


                var user = new UserViewModel()
                {
                    Name = name,
                    UserName = userName.ToLower(),
                    Password = password,
                    Email = email,
                    Phone = cellPhone,
                    RoleId = roleId,
                    DealerId = dealer.DealershipId,
                    HomeDealerId=defaultLogin

                };

                if (SessionHandler.DealerGroup != null)
                {
                    user.DealerGroupId = SessionHandler.DealerGroup.DealershipGroupId;

                    var addList = dealerList.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                    _accountManagementForm.AddUser(user, addList);

                    //var userResult = SQLHelper.AddUserMultipleStore(user, addList);
                    //var teamManagement = new TeamManagementForm();
                    //if (user.RoleId == Constanst.RoleType.Manager || user.RoleId == Constanst.RoleType.Admin)
                    //{
                    //    foreach (var dealerId in addList)
                    //    {
                    //        int result = 0;
                    //        int.TryParse(dealerId, out result);
                    //        if (result != 0)
                    //        {
                    //            teamManagement.AddRosterUser(userResult, result);
                    //        }

                    //    }
                    //}


                }
                if (Request.IsAjaxRequest())
                {
                    return Json(user);
                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult EditUserRight(int userId, int roleId, string dealerList, int defaultLogin)
        {
            if (SessionHandler.Dealer != null)
            {
                var user = new UserRoleViewModel
                {
                    RoleId = roleId,
                    UserId = userId,
                    DefaultLogin = defaultLogin
                };

                if (SessionHandler.DealerGroup != null)
                {
                    var addList = dealerList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    _accountManagementForm.ChangeUserPermission(user,addList);
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(user);
                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }


        public ActionResult UpdateNotification(bool notify, int notificationkind)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                _accountManagementForm.UpdateNotification(notify, dealer.DealershipId, notificationkind);

                switch (notificationkind)
                {
                    case 0:
                     
                        SessionHandler.Dealer.NotificationSetting.AppraisalNotification = notify;

                        break;
                    case 1:
                        
                        SessionHandler.Dealer.NotificationSetting.WholeSaleNotfication = notify;

                        break;
                    case 2:
                     
                        SessionHandler.Dealer.NotificationSetting.InventoryNotfication = notify;

                      
                        break;
                    case 3:
                        
                        SessionHandler.Dealer.NotificationSetting.TwentyFourHourNotification = notify;

                    
                        break;
                    case 4:
                     
                        SessionHandler.Dealer.NotificationSetting.NoteNotification = notify;

                  
                        break;
                    case 5:
                       
                        SessionHandler.Dealer.NotificationSetting.PriceChangeNotification = notify;

                        break;
                    case 6:
                       
                        SessionHandler.Dealer.NotificationSetting.AgeingBucketJumpNotification = notify;

                     
                        break;
                    case 7:
                       
                        SessionHandler.Dealer.NotificationSetting.MarketPriceRangeChangeNotification = notify;

                        break;
                    case 8:
                      
                        SessionHandler.Dealer.NotificationSetting.BucketJumpReportNotification = notify;

                        
                        break;
                    case 9:
                      
                        SessionHandler.Dealer.NotificationSetting.ImageUploadNotification = notify;

                        break;
                }

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }
        public ActionResult WindowStickerNotify(bool notify, int notificationkind)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                _dealerManagementForm.UpdateWindowStickerNotification(notify, dealer.DealershipId, notificationkind);
               
                if (notificationkind == Constanst.NotificationType.RetailPrice)
                    dealer.DealerSetting.RetailPriceWsNotification = notify;
                else if (notificationkind == Constanst.NotificationType.DealerDiscount)
                    dealer.DealerSetting.DealerDiscountWSNotification = notify;
                else if (notificationkind == Constanst.NotificationType.Manufacturer)
                    dealer.DealerSetting.ManufacturerReabteWsNotification = notify;
                else if (notificationkind == Constanst.NotificationType.SalePrice)
                    dealer.DealerSetting.SalePriceWsNotification = notify;

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }
        public ActionResult UpdateOverideStockImage(bool overideStockImage)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
                _dealerManagementForm.UpdateOverideStockImage(dealer.DealershipId, overideStockImage);
                dealer.DealerSetting.OverrideStockImage = overideStockImage;
                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");
                }
                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }
        public ActionResult UpdateNotificationPerUser(bool notify, string userId, int notificationkind)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                var tmp = string.Empty;
              
                switch (notificationkind)
                {
                    case 0:
                        tmp = userId.Replace("AppraisalCheckbox", string.Empty);
                        break;
                    case 1:
                        tmp = userId.Replace("WholeSaleCheckbox", string.Empty);
                        break;
                    case 2:
                        tmp = userId.Replace("InventoryCheckbox", string.Empty);
                        break;
                    case 3:
                        tmp = userId.Replace("24HCheckbox", string.Empty);
                        break;
                    case 4:
                        tmp = userId.Replace("NoteCheckbox", string.Empty);
                        break;
                    case 5:
                        tmp = userId.Replace("PriceCheckbox", string.Empty);
                        break;
                    case 6:
                        tmp = userId.Replace("AgeCheckbox", string.Empty);
                        break;
                    case 7:
                        tmp = userId.Replace("MarketPriceRangeCheckbox", string.Empty);
                        break;
                    case 8:
                        tmp = userId.Replace("BucketJumpCheckbox", string.Empty);
                        break;
                    case 9:
                        tmp = userId.Replace("ImageUploadCheckbox", string.Empty);
                        break;
                }

                _accountManagementForm.UpdateNotificationPerUser(notify, dealer.DealershipId, int.Parse(tmp), notificationkind);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully " + tmp);

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }
        public ActionResult UpdateDefaultStockImage(string defaultStockImageUrl)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                _dealerManagementForm.UpdateDefaultStockImageUrl(dealer.DealershipId, defaultStockImageUrl);

                dealer.DealerSetting.DefaultStockImageUrl = defaultStockImageUrl;

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }
        public ActionResult DeleteStockImage()
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                _dealerManagementForm.UpdateDefaultStockImageUrl(dealer.DealershipId, string.Empty);

                dealer.DealerSetting.DefaultStockImageUrl = "";

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        public ActionResult ChoooseDealerForUser(int? userId)
        {
            var viewModel = new DealerGroupViewModel();

            if (SessionHandler.DealerGroup != null)
            {
                viewModel = SessionHandler.DealerGroup;
            }

            ViewData["UserId"] = userId ?? 0;
            ViewData["RoleId"] = 0;

            if (userId != null)
            {
                var userInfo = _accountManagementForm.GetUserPermissionList(userId.GetValueOrDefault());

                if (userInfo.Any())
                {
                    ViewData["UserDefaultLogin"] = userInfo.First().User.DefaultLogin;
                    ViewData["RoleId"] = userInfo.First().RoleId;
                }

                ViewData["DealerList"] = userInfo.Select(i => i.DealerId).ToList();
                ViewData["Mode"] = "Edit";

            }
            else
            {
                ViewData["UserDefaultLogin"] = viewModel.DealershipGroupDefaultLogin;
                ViewData["Mode"] = "New";

            }

            return View("DealerForUser", viewModel);
        }


        public ActionResult UpdateDefaultLogin(string username, int defaultLogin)
        {
            _accountManagementForm.UpdateDefaultLogin(username, defaultLogin);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }

        public JsonResult YearAjaxPost(int yearId)
        {
         
            var dealer = SessionHandler.Dealer;

            var rebateModel = new ManufacturerRebateViewModel
                {
                    Year = yearId,
                };
            var makeList = _inventoryManagementForm.GetAllNewInventories(dealer.DealershipId).Where(x => x.Vehicle.Year == yearId).
                                   Select(i => new { i.Vehicle.Make, i.Vehicle.Model });


            rebateModel.MakeList = makeList.Select(x => x.Make).Distinct().ToList();


            return new DataContractJsonResult(rebateModel);
        }

        public JsonResult MakeAjaxPost(int yearId, string makeId)
        {

            var dealer = SessionHandler.Dealer;

            var rebateModel = new ManufacturerRebateViewModel
            {
                Year = yearId,
            };
            var makeList = _inventoryManagementForm.GetAllNewInventories(dealer.DealershipId).Where(x => x.Vehicle.Year == yearId && x.Vehicle.Make==makeId).
                          Select(i => new { i.Vehicle.Make, i.Vehicle.Model });
            rebateModel.ModelList = makeList.Select(x => x.Model).Distinct().ToList();

            return new DataContractJsonResult(rebateModel);
        }

        public JsonResult ModelAjaxPost(int yearId, string makeId, string modelId)
        {
           
            var dealer = SessionHandler.Dealer;

            var rebateModel = new ManufacturerRebateViewModel
            {
                Year = yearId,
            };

            var result =
                _inventoryManagementForm.GetAllNewInventories(dealer.DealershipId)
                    .Where(x => x.Vehicle.Year == yearId && x.Vehicle.Make == makeId && x.Vehicle.Model == modelId);

            var trimList = result.Select(x => x.Vehicle.Trim).Distinct().ToList().Select(tmp => String.IsNullOrEmpty(tmp) ? "Unspecified" : tmp).ToList();

            if (trimList.Any(x => x == "Unspecified"))
            {
                trimList.Remove(trimList.First(x => x == "Unspecified"));
                trimList.Add("Unspecified");
            }

            rebateModel.TrimList = trimList;

            return new DataContractJsonResult(rebateModel);
        }

        public JsonResult BodyTypeAjaxPost(int yearId, string makeId, string modelId, string trimId)
        {
            if (trimId.Equals("Unspecified"))
                trimId = "";

            var dealer = SessionHandler.Dealer;

            var rebateModel = new ManufacturerRebateViewModel
            {
                Year = yearId,
            };

            var result = _inventoryManagementForm.GetAllNewInventories(dealer.DealershipId)
                    .Where(x => x.Vehicle.Year == yearId && x.Vehicle.Make == makeId && x.Vehicle.Model == modelId && x.Vehicle.Trim==trimId);

            rebateModel.BodyTypeList = result.Select(x => x.Vehicle.BodyType).Distinct().ToList();

            return new DataContractJsonResult(rebateModel);
        }

        private string GetBodyType(int yearId, string makeId, string modelId, string trimId)
        {
        
            if (trimId.Equals("Unspecified"))
                trimId = "";

            var dealer = SessionHandler.Dealer;

            var result = _inventoryManagementForm.GetAllNewInventories(dealer.DealershipId)
                    .Where(x => x.Vehicle.Year == yearId && x.Vehicle.Make == makeId && x.Vehicle.Model == modelId && x.Vehicle.Trim == trimId);

            return result.Select(x => x.Vehicle.BodyType).Distinct().FirstOrDefault();
        }

        public JsonResult ApplyRebate(int yearId, string makeId, string modelId, string trimId, string trims,
            string bodyType, string rebateAmount, string disclaimer, DateTime? createDate, DateTime? expirationDate)
        {
            try
            {
                var dealer = SessionHandler.Dealer;

                if (trimId.Equals("Unspecified"))
                    trimId = "";

                if (trimId == "All Trims")
                {
                    var listTrims = trims.Split(',').ToList();
                    foreach (var listTrim in listTrims)
                    {
                        if (listTrim != "All Trims")
                        {
                            trimId = listTrim;

                            if (_dealerManagementForm.CheckAnyRebate(yearId, makeId, modelId, trimId,
                                dealer.DealershipId))
                            {
                                return new DataContractJsonResult("Duplicate");
                            }

                            bodyType = GetBodyType(yearId, makeId, modelId, trimId);

                            if (createDate.HasValue &&
                                createDate.Value.Date.ToShortDateString().Equals(DateTime.Now.Date.ToShortDateString()))
                            {
                                var rebateNumber =
                                    Convert.ToInt32(CommonHelper.RemoveSpecialCharactersForMsrp(rebateAmount));
                                _inventoryManagementForm.UpdateRebateInfo(yearId, makeId, modelId, trimId,
                                    dealer.DealershipId, rebateNumber, disclaimer, expirationDate.GetValueOrDefault());

                            }
                            var newRebate = new Rebate
                            {
                                Year = yearId,
                                Make = makeId,
                                Model = modelId,
                                Trim = trimId,
                                BodyType = bodyType,
                                ManufactureReabte =
                                    CommonHelper.RemoveSpecialCharactersForMsrp(rebateAmount),
                                Disclaimer = disclaimer,
                                DateStamp = createDate,
                                DealerId = dealer.DealershipId,
                                ExpiredDate = expirationDate
                            };

                            _dealerManagementForm.AddNewRebate(newRebate);




                        }
                    }
                    return new DataContractJsonResult("");
                }
                else
                {
                    if (_dealerManagementForm.CheckAnyRebate(yearId, makeId, modelId, trimId, dealer.DealershipId))
                    {
                        return new DataContractJsonResult("Duplicate");
                    }

                    if (createDate.HasValue &&
                        createDate.Value.Date.ToShortDateString().Equals(DateTime.Now.Date.ToShortDateString()))
                    {
                        var rebateNumber =
                                 Convert.ToInt32(CommonHelper.RemoveSpecialCharactersForMsrp(rebateAmount));
                        _inventoryManagementForm.UpdateRebateInfo(yearId, makeId, modelId, trimId,
                            dealer.DealershipId, rebateNumber, disclaimer, expirationDate.GetValueOrDefault());
                    }

                    var newRebate = new Rebate
                    {
                        Year = yearId,
                        Make = makeId,
                        Model = modelId,
                        Trim = trimId,
                        BodyType = bodyType,
                        ManufactureReabte =
                            CommonHelper.RemoveSpecialCharactersForMsrp(rebateAmount),
                        Disclaimer = disclaimer,
                        DateStamp = createDate,
                        DealerId = dealer.DealershipId,
                        ExpiredDate = expirationDate
                    };

                    _dealerManagementForm.AddNewRebate(newRebate);

                    return new DataContractJsonResult("");

                }



            }
            catch (Exception)
            {
               

                return new DataContractJsonResult("Error");
            }

        }

 

        public JsonResult DeleteRebate(int rebateId)
        {
            try
            {
               _dealerManagementForm.DeleteRebate(rebateId);
                return new DataContractJsonResult("Success");
            }
            catch (Exception)
            {
                return new DataContractJsonResult("Error");
            }
        }

        public ActionResult SetDisclaimerContent(string content, int rebateId)
        {
            _dealerManagementForm.SetDisclaimerContent(content.Trim(), rebateId);
            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");
            }
            return Json("Not Updated");
        }

        private int GetRebateStatus(DateTime create, DateTime expire)
        {
            var today = DateTime.Now.Date;

            if (create > today)
                return Constanst.RebateStatus.NotActivated;

            if (expire < today)
                return Constanst.RebateStatus.Expired;

            return Constanst.RebateStatus.Activating;
        }

        public ActionResult UpdateRebateCreateDate(string date, int rebateId)
        {
            try
            {

                _dealerManagementForm.UpdateRebateCreateDate(date,rebateId);

              
                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");
                }

                return Json("Not Updated");
            }

            catch (Exception ex)
            {
                return Json("Not Updated");
            }
        }

        public ActionResult UpdateRebateExpirationDate(string date, int rebateId)
        {
            try
            {

                _dealerManagementForm.UpdateRebateExpirationDate(date, rebateId);
                var rebate = _dealerManagementForm.GetRebate(rebateId);

                var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (rebate.ExpiredDate.GetValueOrDefault() >= todayDate)
                {
                    _dealerManagementForm.ActivateRebate(rebateId);
                }
                else
                {
                    _dealerManagementForm.DeactivateRebate(rebateId);
                }


                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");
                }

                return Json("Not Updated");

            }
            catch (Exception ex)
            {
                return Json("Not Updated");
            }
        }

        public ActionResult UpdateRebateAmount(string rebateAmount, int rebateId)
        {
            _dealerManagementForm.UpdateRebateAmount(rebateAmount, rebateId);
            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");
            }
            return Json("Not Updated");
        }
        public ActionResult UpdateRebateDisclaimer(string rebateDisclaimer, int rebateId)
        {
            _dealerManagementForm.UpdateRebateDisclaimer(rebateDisclaimer, rebateId);
            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");
            }
            return Json("Not Updated");
        }

        public string RemoveBuyerGuide(int id)
        {
            try
            {
                var convertedId = Convert.ToInt32(id);
                var flag = _dealerManagementForm.RemoveBuyerGuide(convertedId);
                if (flag)
                    return "True";
                return "Can not find the buyer guide.";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateBuyerGuide(string listingId, string name, string category)
        {
            try
            {
                if (SessionHandler.Dealer == null)
                {
                    return "TimeOut";
                }

                var convertedId = Convert.ToInt32(listingId);
                using (var context = new VincontrolEntities())
                {
                    var existingBuyerGuideWithName =
                            context.WarrantyTypes.FirstOrDefault(
                                i =>
                                i.DealerId == SessionHandler.Dealer.DealershipId &&
                                i.Name.ToLower().Trim().Equals(name.ToLower().Trim()) && i.WarrantyTypeId != convertedId);

                    if (existingBuyerGuideWithName == null)
                    {
                        var existingBuyerGuide = context.WarrantyTypes.FirstOrDefault(i => i.WarrantyTypeId == convertedId);
                        if (existingBuyerGuide != null)
                        {
                            existingBuyerGuide.Name = name;
                            existingBuyerGuide.Category = Convert.ToInt32(category);
                            context.SaveChanges();
                            return "True";
                        }
                        return "Can not find the buyer guide.";
                    }
                    return "Existing";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string AddNewBuyerGuide(string name, string category)
        {
            if (SessionHandler.Dealer == null)
            {
                return "TimeOut";
            }
            try
            {
                if (_dealerManagementForm.CheckBuyerGuide(SessionHandler.Dealer.DealershipId, name))
                    return "Existing";
                var returnValue= _dealerManagementForm.AddBuyerGuide(SessionHandler.Dealer.DealershipId, name,
                    Convert.ToInt32(category));
                return returnValue.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public void UpdateButtonPermission(string groupId, string buttonId, bool canSee)
        {

            var convertedGroupId = Convert.ToInt32(groupId);
            var convertedButtonId = Convert.ToInt32(buttonId);
            _dealerManagementForm.UpdateButtonPermission(SessionHandler.Dealer.DealershipId, convertedGroupId,
                convertedButtonId, canSee);


        }

        public void UpdateDescriptionSetting(string description, int descriptionCategory)
        {
            _dealerManagementForm.UpdateContentAppSetting(SessionHandler.Dealer.DealershipId, description, descriptionCategory);
            switch (descriptionCategory)
            {
                case Constanst.DescriptionSettingCategory.LoanerSentence:
                    SessionHandler.Dealer.DealerSetting.LoanerSentence = description;

                    break;
                case Constanst.DescriptionSettingCategory.AuctionSentence:
                    SessionHandler.Dealer.DealerSetting.AuctionSentence = description;

                    break;
                case Constanst.DescriptionSettingCategory.DealerWarranty:
                    SessionHandler.Dealer.DealerSetting.DealerWarranty = description;
                    break;
                case Constanst.DescriptionSettingCategory.ShippingInfo:
                    SessionHandler.Dealer.DealerSetting.ShippingInfo = description;
                    break;
                case Constanst.DescriptionSettingCategory.StartSentence:
                    SessionHandler.Dealer.DealerSetting.StartSentence = description;
                    break;
                case Constanst.DescriptionSettingCategory.TermConditon:
                    SessionHandler.Dealer.DealerSetting.TermConditon = description;
                    break;
            }
        }

        public void UpdateEndingDescriptionSetting(string descriptionUsed, string descriptionNew, int descriptionCategory)
        {
            _dealerManagementForm.UpdateContentAppSetting(SessionHandler.Dealer.DealershipId, descriptionNew, descriptionUsed, descriptionCategory);
            switch (descriptionCategory)
            {
                case Constanst.DescriptionSettingCategory.EndSentence:
                    SessionHandler.Dealer.DealerSetting.EndSentence = descriptionUsed;
                    SessionHandler.Dealer.DealerSetting.EndSentenceForNew = descriptionNew;
                    break;
            }
        }
        public void ClearDescriptionSetting(int descriptionCategory)
        {
            _dealerManagementForm.UpdateContentAppSetting(SessionHandler.Dealer.DealershipId, descriptionCategory);
            switch (descriptionCategory)
            {
                case Constanst.DescriptionSettingCategory.LoanerSentence:
                    SessionHandler.Dealer.DealerSetting.LoanerSentence = string.Empty;

                    break;
                case Constanst.DescriptionSettingCategory.AuctionSentence:
                    SessionHandler.Dealer.DealerSetting.AuctionSentence = string.Empty;

                    break;
                case Constanst.DescriptionSettingCategory.DealerWarranty:
                    SessionHandler.Dealer.DealerSetting.DealerWarranty = string.Empty;
                    break;
                case Constanst.DescriptionSettingCategory.EndSentence:
                    SessionHandler.Dealer.DealerSetting.EndSentence = string.Empty;
                    break;
                case Constanst.DescriptionSettingCategory.ShippingInfo:
                    SessionHandler.Dealer.DealerSetting.ShippingInfo = string.Empty;
                    break;
                case Constanst.DescriptionSettingCategory.StartSentence:
                    SessionHandler.Dealer.DealerSetting.StartSentence = string.Empty;
                    break;
                case Constanst.DescriptionSettingCategory.TermConditon:
                    SessionHandler.Dealer.DealerSetting.TermConditon = string.Empty;
                    break;
            }
        }

    }
}
