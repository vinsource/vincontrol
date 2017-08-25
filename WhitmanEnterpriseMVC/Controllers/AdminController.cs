using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Security;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class AdminController : SecurityController
    {
        private const string PermissionCode = "ADMIN";
        private const string AcceptedValues = "READONLY, ALLACCESS";

        private const string KingRole = "King";
        private const string AdminRole = "Admin";
        private const string ManagerRole = "Manager";
        private const string EmployeeRole = "Employee";


        //[Authorize(Roles = "Admin")]
        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult AdminSecurity()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

                var context = new whitmanenterprisewarehouseEntities();

                var dealerSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

                var yearList = context.whitmanenterprisedealershipinventories.Where(x => x.DealershipId == dealer.DealershipId && x.NewUsed == "New").Select(x => x.ModelYear).Distinct();

                var distincList = context.vincontrolrebates.Where(x => x.DealerId == dealer.DealershipId);

                var rebateList = new List<ManafacturerRebateDistinctModel>();

                if (distincList.Any())
                {

                    foreach (var tmp in distincList)
                    {
                        var rebateModel = new ManafacturerRebateDistinctModel
                        {
                            UniqueId = tmp.vincontrolrebateid,
                            Year = tmp.Year.GetValueOrDefault(),
                            Make = tmp.Make,
                            Model = tmp.Model,
                            Trim = tmp.Trim,
                            Disclaimer = tmp.Disclaimer,
                            BodyType = tmp.BodyType,
                            RebateAmount = tmp.ManufactureReabte
                        };

                        rebateList.Add(rebateModel);
                    }
                }

                var hashSet = new HashSet<string>();

                var userList = new List<UserRoleViewModel>();
                var userFilter =
                    from e in context.whitmanenterpriseusersnotifications
                    from et in context.whitmanenterpriseusers
                    where
                        e.DealershipId == dealer.DealershipId &&
                        e.UserName == et.UserName && et.Active.Value

                    select new
                    {
                        et.Name,
                        et.UserName,
                        et.Password,
                        et.Email,
                        et.Cellphone,
                        et.RoleName,
                        e.PriceChangeNotification,
                        et.Active,
                        e.WholeNotification,
                        e.InventoryNotification,
                        e.C24Hnotification,
                        e.NoteNotification,
                        e.AppraisalNotification,
                        e.AgingNotification,
                        e.MarketPriceRangeNotification,
                        e.BucketJumpNotification,
                        e.ImageUploadNotification
                    };

                foreach (var row in userFilter)
                {
                    if (!hashSet.Contains(row.UserName))
                    {
                        var user = new UserRoleViewModel()
                        {
                            Name = row.Name,
                            UserName = row.UserName,
                            PassWord = row.Password,
                            Email = row.Email,
                            Cellphone = row.Cellphone,
                            Role = row.RoleName,
                            Active = row.Active.Value,
                            AppraisalNotification =
                                row.AppraisalNotification.GetValueOrDefault(),
                            WholeSaleNotfication = row.WholeNotification.GetValueOrDefault(),
                            InventoryNotfication = row.C24Hnotification.GetValueOrDefault(),
                            NoteNotification = row.NoteNotification.GetValueOrDefault(),
                            PriceChangeNotification =
                                row.PriceChangeNotification.GetValueOrDefault(),
                            AgeingBucketJumpNotification = row.AgingNotification.GetValueOrDefault(),
                            MarketPriceRangeChangeNotification = row.MarketPriceRangeNotification.GetValueOrDefault(),
                            BucketJumpReportNotification = row.BucketJumpNotification.GetValueOrDefault(),
                            ImageUploadNotification = row.ImageUploadNotification.GetValueOrDefault(),
                        };

                        userList.Add(user);
                    }

                    hashSet.Add(row.UserName);
                }

                if (dealerSetting != null)
                {
                    var viewModel = new AdminViewModel()
                    {
                        Cragislist = dealerSetting.Cragislist,
                        CraigslistPassword = dealerSetting.CraigslistPassword,
                        Ebay = dealerSetting.Ebay,
                        EbayPassword = dealerSetting.EbayPassword,
                        CarFax = dealerSetting.CarFax,
                        CarFaxPassword = EncryptionHelper.EncryptString(dealerSetting.CarFaxPassword),
                        Manheim = dealerSetting.Manheim,
                        ManheimPassword = EncryptionHelper.EncryptString(dealerSetting.ManheimPassword),
                        KellyBlueBook = dealerSetting.KellyBlueBook,
                        KellyPassword = EncryptionHelper.EncryptString(dealerSetting.KellyPassword),
                        BlackBook = dealerSetting.BlackBook,
                        BlackBookPassword = EncryptionHelper.EncryptString(dealerSetting.BlackBookPassword),
                        AutoCheck = dealerSetting.AutoCheck,
                        AutoCheckPassword = dealerSetting.AutoCheckPassword,
                        SortSet = dealerSetting.InventorySorting,
                        SortSetList = SelectListHelper.InitalSortSetList(dealerSetting.InventorySorting),
                        AppraisalNotification = dealerSetting.AppraisalNotification.GetValueOrDefault(),
                        WholeSaleNotfication = dealerSetting.WholeNotification.GetValueOrDefault(),
                        InventoryNotfication = dealerSetting.InventoryNotification.GetValueOrDefault(),
                        TwentyFourHourNotification = dealerSetting.C24Hnotification.GetValueOrDefault(),
                        NoteNotification = dealerSetting.NoteNotification.GetValueOrDefault(),
                        PriceChangeNotification = dealerSetting.PriceChangeNotification.GetValueOrDefault(),
                        MarketPriceRangeChangeNotification = dealerSetting.MarketPriceRangeChangeNotification.GetValueOrDefault(),
                        AgeingBucketJumpNotification = dealerSetting.AgeingBucketNotification.GetValueOrDefault(),
                        ImageUploadNotification = dealerSetting.ImageUploadNotification.GetValueOrDefault(),
                        RetailPriceWSNotification = dealerSetting.RetailPrice.GetValueOrDefault(),
                        DealerDiscountWSNotification = dealerSetting.DealerDiscount.GetValueOrDefault(),
                        ManufacturerReabteWsNotification = dealerSetting.ManufacturerReabte.GetValueOrDefault(),
                        SalePriceWsNotification = dealerSetting.SalePrice.GetValueOrDefault(),
                        RetailPriceWSNotificationText = dealerSetting.RetailPriceText,
                        DealerDiscountWSNotificationText = dealerSetting.DealerDiscountText,
                        ManufacturerReabteWsNotificationText = dealerSetting.ManufactureReabateText,
                        SalePriceWsNotificationText = dealerSetting.SalePriceText,
                        ManufacturerWarranty = dealerSetting.ManufacturerWarranty,
                        DealerCertified = dealerSetting.DealerCertified,
                        ManufacturerCertified = dealerSetting.ManufacturerCertified,
                        FirstRange = dealerSetting.FirstTimeRangeBucketJump.GetValueOrDefault(),
                        SecondRange = dealerSetting.SecondTimeRangeBucketJump.GetValueOrDefault(),
                        BucketJumpReportNotification = dealerSetting.BucketJumpNotification.GetValueOrDefault(),
                        DealerCertifiedDuration = dealerSetting.DealerCertifiedDuration,
                        ManufacturerCertifiedDuration = dealerSetting.ManufacturerCertifiedDuration,
                        ShippingInfo = dealerSetting.ShippingInfo,
                        DealerInfo = dealerSetting.DealerInfo,
                        DealerWarrantyInfo = dealerSetting.DealerWarranty,
                        TermConditon = dealerSetting.TermsAndCondition,
                        StartSentence = String.IsNullOrEmpty(dealerSetting.StartDescriptionSentence) ? "" : dealerSetting.StartDescriptionSentence,
                        EndSentence = String.IsNullOrEmpty(dealerSetting.EndDescriptionSentence) ? "" : dealerSetting.EndDescriptionSentence,
                        AuctionSentence = String.IsNullOrEmpty(dealerSetting.AuctionSentence) ? "" : dealerSetting.AuctionSentence,
                        SoldAction = dealerSetting.SoldOut,
                        SoldActionList = SelectListHelper.InitalSoldOutList(dealerSetting.SoldOut),
                        Users = userList.AsEnumerable(),
                        DealershipName = dealer.DealershipName,
                        DealershipId = dealer.DealershipId,
                        DealershipAddress = dealer.DealershipAddress,
                        Address = dealer.Address,
                        City = dealer.City,
                        State = dealer.State,
                        Phone = dealer.Phone,
                        ZipCode = dealer.ZipCode,
                        Email = dealer.Email,
                        DefaultStockImageURL = dealerSetting.DefaultStockImageUrl,
                        OverrideStockImage = dealerSetting.OverideStockImage.GetValueOrDefault(),
                        Dealer = dealer,
                        YearsList = SelectListHelper.InitialYearList(yearList),
                        MakesList = new List<SelectListItem>().AsEnumerable(),
                        ModelsList = new List<SelectListItem>().AsEnumerable(),
                        TrimsList = new List<SelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<SelectListItem>().AsEnumerable(),
                        RebateList = rebateList,
                        EnableAutoDescription = dealerSetting.AutoDescription ?? false,
                        AutoDescriptionSubscribe = dealerSetting.AutoDescriptionSubscribe ?? false,
                        WarrantyTypes = LinqHelper.GetWarrantyTypeList(SessionHandler.Dealership)
                    };

                    viewModel.BasicWarrantyTypes = viewModel.WarrantyTypes.Count() > 0 ? viewModel.WarrantyTypes.Where(i => i.DealerId == 0).Select(i => new SelectListItem() {Selected = false, Text = i.Name, Value = i.Id.ToString()}).ToList() : new List<SelectListItem>();

                    if (Session["DealerGroup"] != null)
                    {
                        viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                        viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);

                        viewModel.MutipleDealer = true;
                    }
                    else
                        viewModel.DealerList = SelectListHelper.InitialDealerList();

                    viewModel.IntervalList = SelectListHelper.InitialIntervalListForAdmin(dealerSetting.IntervalBucketJump.GetValueOrDefault());

                    viewModel.LandingPage = "Default";

                    viewModel.Comments = GetComments().ToList();

                    viewModel.VarianceCost = GetVarianceCost();

                    viewModel.ButtonPermissions = SQLHelper.GetButtonList(dealer.DealershipId, "Profile");

                    return View("AdminControl", viewModel);
                }
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
            return null;
        }

    
        public ActionResult SaveVarianceCost(string cost)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

                var dealer = context.whitmanenterprisedealerships.Where(e => e.idWhitmanenterpriseDealership == dealerSessionInfo.DealershipId).FirstOrDefault();
                if (dealer != null)
                {
                    decimal result;
                    dealer.PriceVariance = decimal.TryParse(cost, out result) ? result : 0;
                    context.SaveChanges();
                }

                return View("VarianceCost", new VariantCodeViewModel() { Variance = (dealer.PriceVariance.HasValue ? dealer.PriceVariance.Value : 0) });

            }
        }

        public VariantCodeViewModel GetVarianceCost()
        {

            var context = new whitmanenterprisewarehouseEntities();
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            var dealer = context.whitmanenterprisedealerships.Where(e => e.idWhitmanenterpriseDealership == dealerSessionInfo.DealershipId).FirstOrDefault();
            return new VariantCodeViewModel() { Variance = (dealer.PriceVariance.HasValue ? dealer.PriceVariance.Value : 0) };

        }

        private IQueryable<TradeinCommentViewModel> GetComments()
        {

            var context = new whitmanenterprisewarehouseEntities();
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

            var comments =
                context.vincontroltradeinbannercomments.Where(
                    x => x.DealerId == dealerSessionInfo.DealershipId).Select(e => new TradeinCommentViewModel()
                    {
                        City = e.City,
                        Content = e.Content,
                        State = e.State,
                        ID = e.SmartCommentId,
                        Name = e.Name
                    });
            return comments;
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult AdminSecurityLanding(string LandingPage)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

                var context = new whitmanenterprisewarehouseEntities();

                var dealerSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

                var yearList = context.whitmanenterprisedealershipinventories.Where(x => x.DealershipId == dealer.DealershipId && x.NewUsed == "New").Select(x => x.ModelYear).Distinct();


                var distincList = context.vincontrolrebates.Where(x => x.DealerId == dealer.DealershipId);

                var rebateList = new List<ManafacturerRebateDistinctModel>();


                foreach (var tmp in distincList)
                {
                    var rebateModel = new ManafacturerRebateDistinctModel
                    {
                        UniqueId = tmp.vincontrolrebateid,
                        Year = tmp.Year.GetValueOrDefault(),
                        Make = tmp.Make,
                        Model = tmp.Model,
                        Trim = tmp.Trim,
                        Disclaimer = tmp.Disclaimer,
                        BodyType = tmp.BodyType,
                        RebateAmount = tmp.ManufactureReabte
                    };

                    rebateList.Add(rebateModel);
                }

                var hashSet = new HashSet<string>();

                var userList = new List<UserRoleViewModel>();
                var userFilter =
                    from e in context.whitmanenterpriseusersnotifications
                    from et in context.whitmanenterpriseusers
                    where
                        e.DealershipId == dealer.DealershipId &&
                        e.UserName == et.UserName && et.Active.Value

                    select new
                    {
                        et.Name,
                        et.UserName,
                        et.Password,
                        et.Email,
                        et.Cellphone,
                        et.RoleName,
                        e.PriceChangeNotification,
                        et.Active,
                        e.WholeNotification,
                        e.InventoryNotification,
                        e.C24Hnotification,
                        e.NoteNotification,
                        e.AppraisalNotification

                    };




                foreach (var row in userFilter)
                {
                    if (!hashSet.Contains(row.UserName))
                    {
                        var user = new UserRoleViewModel()
                        {
                            Name = row.Name,
                            UserName = row.UserName,
                            PassWord = row.Password,
                            Email = row.Email,
                            Cellphone = row.Cellphone,
                            Role = row.RoleName,
                            Active = row.Active.Value,
                            AppraisalNotification = row.AppraisalNotification.GetValueOrDefault(),
                            WholeSaleNotfication = row.WholeNotification.GetValueOrDefault(),
                            InventoryNotfication = row.C24Hnotification.GetValueOrDefault(),
                            NoteNotification = row.NoteNotification.GetValueOrDefault(),
                            PriceChangeNotification = row.PriceChangeNotification.GetValueOrDefault()
                        };
                        userList.Add(user);

                    }
                    hashSet.Add(row.UserName);

                }


                if (dealerSetting != null)
                {
                    var viewModel = new AdminViewModel()
                    {
                        Cragislist = dealerSetting.Cragislist,
                        CraigslistPassword = dealerSetting.CraigslistPassword,
                        Ebay = dealerSetting.Ebay,
                        EbayPassword = dealerSetting.EbayPassword,
                        CarFax = dealerSetting.CarFax,
                        CarFaxPassword = EncryptionHelper.EncryptString(dealerSetting.CarFaxPassword),
                        Manheim = dealerSetting.Manheim,
                        ManheimPassword = EncryptionHelper.EncryptString(dealerSetting.ManheimPassword),
                        KellyBlueBook = dealerSetting.KellyBlueBook,
                        KellyPassword = EncryptionHelper.EncryptString(dealerSetting.KellyPassword),
                        BlackBook = dealerSetting.BlackBook,
                        BlackBookPassword = EncryptionHelper.EncryptString(dealerSetting.BlackBookPassword),
                        AutoCheck = dealerSetting.AutoCheck,
                        AutoCheckPassword = dealerSetting.AutoCheckPassword,
                        SortSet = dealerSetting.InventorySorting,
                        SortSetList = SelectListHelper.InitalSortSetList(dealerSetting.InventorySorting),
                        AppraisalNotification = dealerSetting.AppraisalNotification.GetValueOrDefault(),
                        WholeSaleNotfication = dealerSetting.WholeNotification.GetValueOrDefault(),
                        InventoryNotfication = dealerSetting.InventoryNotification.GetValueOrDefault(),
                        TwentyFourHourNotification = dealerSetting.C24Hnotification.GetValueOrDefault(),
                        NoteNotification = dealerSetting.NoteNotification.GetValueOrDefault(),
                        PriceChangeNotification = dealerSetting.PriceChangeNotification.GetValueOrDefault(),
                        RetailPriceWSNotification = dealerSetting.RetailPrice.GetValueOrDefault(),
                        DealerDiscountWSNotification = dealerSetting.DealerDiscount.GetValueOrDefault(),
                        ManufacturerReabteWsNotification = dealerSetting.ManufacturerReabte.GetValueOrDefault(),
                        SalePriceWsNotification = dealerSetting.SalePrice.GetValueOrDefault(),
                        ManufacturerWarranty = dealerSetting.ManufacturerWarranty,
                        DealerCertified = dealerSetting.DealerCertified,
                        ManufacturerCertified = dealerSetting.ManufacturerCertified,
                        //ManufacturerWarrantyDuration = dealerSetting.ManufacturerWarrantyDuration,
                        DealerCertifiedDuration = dealerSetting.DealerCertifiedDuration,
                        ManufacturerCertifiedDuration = dealerSetting.ManufacturerCertifiedDuration,
                        ShippingInfo = dealerSetting.ShippingInfo,
                        DealerInfo = dealerSetting.DealerInfo,
                        DealerWarrantyInfo = dealerSetting.DealerWarranty,
                        TermConditon = dealerSetting.TermsAndCondition,
                        StartSentence = String.IsNullOrEmpty(dealerSetting.StartDescriptionSentence) ? "" : dealerSetting.StartDescriptionSentence,
                        EndSentence = String.IsNullOrEmpty(dealerSetting.EndDescriptionSentence) ? "" : dealerSetting.EndDescriptionSentence,
                        AuctionSentence = String.IsNullOrEmpty(dealerSetting.AuctionSentence) ? "" : dealerSetting.AuctionSentence,
                        SoldAction = dealerSetting.SoldOut,
                        SoldActionList = SelectListHelper.InitalSoldOutList(dealerSetting.SoldOut),
                        Users = userList.AsEnumerable(),
                        DealershipName = dealer.DealershipName,
                        DealershipId = dealer.DealershipId,
                        //DealershipPhoneNumber=dealer.DealershipPhoneNumber,
                        DealershipAddress = dealer.DealershipAddress,
                        Address = dealer.Address,
                        City = dealer.City,
                        State = dealer.State,
                        Phone = dealer.Phone,
                        ZipCode = dealer.ZipCode,
                        DefaultStockImageURL = dealerSetting.DefaultStockImageUrl,
                        OverrideStockImage = dealerSetting.OverideStockImage.GetValueOrDefault(),
                        Dealer = dealer,
                        YearsList = SelectListHelper.InitialYearList(yearList),
                        MakesList = new List<SelectListItem>().AsEnumerable(),

                        ModelsList = new List<SelectListItem>().AsEnumerable(),

                        TrimsList = new List<SelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<SelectListItem>().AsEnumerable(),
                        RebateList = rebateList,
                        WarrantyTypes = LinqHelper.GetWarrantyTypeList(SessionHandler.Dealership)
                    };

                    viewModel.BasicWarrantyTypes = viewModel.WarrantyTypes.Count() > 0 ? viewModel.WarrantyTypes.Where(i => i.DealerId == 0).Select(i => new SelectListItem() { Selected = false, Text = i.Name, Value = i.Id.ToString() }).ToList() : new List<SelectListItem>();

                    if (Session["DealerGroup"] != null)
                    {
                        viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                        viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);

                        viewModel.MutipleDealer = true;

                    }
                    else
                        viewModel.DealerList = SelectListHelper.InitialDealerList();

                    viewModel.LandingPage = LandingPage;

                    viewModel.IntervalList = SelectListHelper.InitialIntervalListForAdmin(dealerSetting.IntervalBucketJump.GetValueOrDefault());
                    viewModel.Comments = GetComments().ToList();
                    viewModel.VarianceCost = GetVarianceCost();
                    viewModel.ButtonPermissions = SQLHelper.GetButtonList(dealer.DealershipId, "Profile");

                    return View("AdminControl", viewModel);
                }
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
            return null;
        }

        public ActionResult UpdateSetting(AdminViewModel admin)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                if (!admin.ManheimPasswordChanged)
                {
                    admin.ManheimPassword = EncryptionHelper.DecryptString(admin.ManheimPassword);
                }

                if (!admin.CarFaxPasswordChanged)
                {
                    admin.CarFaxPassword = EncryptionHelper.DecryptString(admin.CarFaxPassword);
                }

                if (!admin.KellyPasswordChanged)
                {
                    admin.KellyPassword = EncryptionHelper.DecryptString(admin.KellyPassword);
                }

                if (!admin.BlackBookPasswordChanged)
                {
                    admin.BlackBookPassword = EncryptionHelper.DecryptString(admin.BlackBookPassword);
                }

                SQLHelper.UpdateAppSetting(dealer.DealershipId, admin);

                dealer.EnableAutoDescription = admin.EnableAutoDescription;

                dealer.InventorySorting = admin.SortSet;

                dealer.SoldOut = admin.SoldAction;

                dealer.Manheim = admin.Manheim;

                dealer.ManheimPassword = EncryptionHelper.EncryptString(admin.ManheimPassword);

                dealer.CarFax = admin.CarFax;

                dealer.CarFaxPassword = EncryptionHelper.EncryptString(admin.CarFaxPassword);

                dealer.KellyBlueBook = admin.KellyBlueBook;

                dealer.KellyPassword = EncryptionHelper.EncryptString(admin.KellyPassword);

                dealer.BlackBook = admin.BlackBook;

                dealer.BlackBookPassword = EncryptionHelper.EncryptString(admin.BlackBookPassword);

                dealer.Email = admin.Email;

                dealer.FirstIntervalJump = admin.FirstRange;

                dealer.SecondIntervalJump = admin.SecondRange;

                dealer.FirstIntervalJump = admin.SelectedInterval;

                //dealer.City = admin.City;

                //dealer.Address = admin.Address;

                //dealer.State = admin.State;

                //dealer.ZipCode = admin.ZipCode;

                //dealer.AuctionSentence = admin.AuctionSentence;

                //dealer.StartSentence = admin.StartSentence;

                //dealer.EndSentence = admin.EndSentence;


                return RedirectToAction("AdminSecurity");
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult UpdatePassword(string Pass, string Username)
        {
            SQLHelper.UpdatePass(Pass, Username);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }

        public ActionResult UpdateEmail(string Email, string Username)
        {
            SQLHelper.UpdateEmail(Email, Username);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }

        public ActionResult UpdateCellPhone(string CellPhone, string UserName)
        {
            if (Session["Dealership"] != null)
            {
                SQLHelper.UpdateCellPhone(CellPhone, UserName);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult ChangeRole(string Role, string UserName)
        {
            if (Session["Dealership"] != null)
            {
                SQLHelper.ChangeRole(Role, UserName);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult DeleteUser(string UserName)
        {
            if (Session["Dealership"] != null)
            {
                SQLHelper.DeleteUser(UserName);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult EditUser(string UserName)
        {
            if (Session["Dealership"] != null)
            {
                SQLHelper.DeleteUser(UserName);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        public ActionResult CheckUserExist(string UserName, int DealerId)
        {
            if (Session["Dealership"] != null)
            {
                bool userExist = SQLHelper.CheckUserNameExist(UserName);

                var user = new UserRoleViewModel();

                if (userExist)
                    user.IsUserExist = "Exist";

                return Json(user);
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult AddSingleUser(string Name, string UserName, string Password, string Email, string CellPhone, string UserLevel)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                var user = new UserRoleViewModel()
                {
                    Name = Name,
                    UserName = UserName,
                    PassWord = Password,
                    Email = Email,
                    Cellphone = CellPhone,
                    Role = UserLevel,
                    DealershipId = dealer.DealershipId,
                    Active = true,
                    DefaultLogin = dealer.DealershipId


                };




                bool flag = SQLHelper.AddUser(user);
                if (flag == false)
                    user.IsUserExist = "Exist";


                if (Request.IsAjaxRequest())
                {
                    return Json(user);

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = "ALLACCESS")]
        public ActionResult AddUser(string Name, string UserName, string Password, string Email, string CellPhone, string UserLevel, string DealerList, string DefaultLogin)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                var user = new UserRoleViewModel()
                {
                    Name = Name,
                    UserName = UserName,
                    PassWord = Password,
                    Email = Email,
                    Cellphone = CellPhone,
                    Role = UserLevel,
                    DealershipId = dealer.DealershipId,
                    Active = true,
                    DefaultLogin = dealer.DealershipId


                };

                if (Session["DealerGroup"] != null)
                {
                    bool userExist = SQLHelper.CheckUserNameExist(UserName);
                    if (userExist)
                        user.IsUserExist = "Exist";
                    else
                    {
                        var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                        var addList = DealerList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var s in addList)
                        {
                            user.DealershipId = Convert.ToInt32(s);
                            user.DealerGroupId = dealerGroup.DealershipGroupId;
                            if (String.IsNullOrEmpty(DefaultLogin))
                                user.DefaultLogin = Convert.ToInt32(dealerGroup.DealershipGroupDefaultLogin);
                            else
                            {
                                user.DefaultLogin = Convert.ToInt32(DefaultLogin);
                            }
                            SQLHelper.AddUserMultipleStore(user);
                            SQLHelper.AddUserNotification(user);
                        }

                        LinqHelper.AddNewActivity(SessionHandler.Dealership.DealershipId, "New User " + user.UserName + " With " + user.Role + " Role", String.Format("User Name: {0};Email: {1};Cell Phone: {2};Role: {3}", user.UserName, user.Email, user.Cellphone, user.Role), Constanst.ActivityType.NewUser);
                    }

                }
                if (Request.IsAjaxRequest())
                {
                    return Json(user);
                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        public ActionResult UpdateNotification(bool Notify, int Notificationkind)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                SQLHelper.UpdateNotification(Notify, dealer.DealershipId, Notificationkind);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        public ActionResult WindowStickerNotify(bool Notify, int Notificationkind)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                SQLHelper.UpdateWindowStickerNotification(Notify, dealer.DealershipId, Notificationkind);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        public ActionResult UpdateOverideStockImage(bool OverideStockImage)
        {
            if (Session["Dealership"] != null)
            {
                DealershipViewModel dealer = (DealershipViewModel)Session["Dealership"];

                SQLHelper.UpdateOverideStockImage(dealer.DealershipId, OverideStockImage);

                dealer.OverrideStockImage = OverideStockImage;

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        public ActionResult UpdateNotificationPerUser(bool Notify, string UserName, int Notificationkind)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var tmp = "";

                switch (Notificationkind)
                {
                    case 0:
                        tmp = UserName.Replace("AppraisalCheckbox", "");
                        break;
                    case 1:
                        tmp = UserName.Replace("WholeSaleCheckbox", "");
                        break;
                    case 2:
                        tmp = UserName.Replace("InventoryCheckbox", "");
                        break;
                    case 3:
                        tmp = UserName.Replace("24HCheckbox", "");
                        break;
                    case 4:
                        tmp = UserName.Replace("NoteCheckbox", "");
                        break;
                    case 5:
                        tmp = UserName.Replace("PriceCheckbox", "");
                        break;
                    case 6:
                        tmp = UserName.Replace("AgeCheckbox", "");
                        break;
                    case 7:
                        tmp = UserName.Replace("MarketPriceRangeCheckbox", "");
                        break;
                    case 8:
                        tmp = UserName.Replace("BucketJumpCheckbox", "");
                        break;
                    case 9:
                        tmp = UserName.Replace("ImageUploadCheckbox", "");
                        break;
                }



                SQLHelper.UpdateNotificationPerUser(Notify, dealer.DealershipId, tmp, Notificationkind);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully " + tmp);

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel()
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        public ActionResult UpdateDefaultStockImage(string defaultStockImageUrl)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                SQLHelper.UpdateDefaultStockImageUrl(dealer.DealershipId, defaultStockImageUrl);

                dealer.DefaultStockImageUrl = defaultStockImageUrl;

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        public ActionResult DeleteStockImage()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                SQLHelper.UpdateDefaultStockImageUrl(dealer.DealershipId, "");

                dealer.DefaultStockImageUrl = "";

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }
        }

        public ActionResult ChoooseDealerForUser()
        {
            var viewModel = new DealerGroupViewModel();

            if (Session["DealerGroup"] != null)
            {
                viewModel = (DealerGroupViewModel)Session["DealerGroup"];

            }

            return View("DealerForUser", viewModel);
        }

        public ActionResult EditUserForDefaultLogin(string username)
        {
            var viewModel = new DealerGroupViewModel();

            if (Session["DealerGroup"] != null)
            {
                var defaultLoginForUser = SQLHelper.GetDefaultLoginFromUserName(username);
                viewModel = (DealerGroupViewModel)Session["DealerGroup"];
                //viewModel.DealerList.Add(new DealershipViewModel(){DealershipName = viewModel.DealershipGroupName, DealershipId = Constanst.DealerGroupConst});
                viewModel.DefaultLoginForUser = defaultLoginForUser;

            }
            ViewData["UserName"] = username;
            return View("EditUser", viewModel);
        }

        public ActionResult UpdateDefaultLogin(string Username, string DefaultLogin)
        {
            SQLHelper.UpdateDefaultLogin(Username, DefaultLogin);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }

        public JsonResult YearAjaxPost(int YearId)
        {
            var contextVinControl = new whitmanenterprisewarehouseEntities();

            var dealer = (DealershipViewModel)Session["Dealership"];

            var rebateModel = new ManufacturerRebateViewModel()
            {
                Year = YearId,
            };

            var makeList =
                contextVinControl.whitmanenterprisedealershipinventories.Where(
                    x => x.DealershipId == dealer.DealershipId && x.NewUsed == "New" && x.ModelYear == YearId);

            if (makeList.Select(x => x.Make).Distinct().Count() == 1)
            {
                rebateModel.MakeList = makeList.Select(x => x.Make).Distinct().ToList();

                rebateModel.ModelList = makeList.Select(x => x.Model).Distinct().ToList();
            }
            else
            {
                rebateModel.MakeList = makeList.Select(x => x.Make).Distinct().ToList();
            }

            return new DataContractJsonResult(rebateModel);
        }

        public JsonResult ModelAjaxPost(int YearId, string MakeId, string ModelId)
        {
            var contextVinControl = new whitmanenterprisewarehouseEntities();

            var dealer = (DealershipViewModel)Session["Dealership"];

            var rebateModel = new ManufacturerRebateViewModel()
            {
                Year = YearId,
            };

            var result =
                contextVinControl.whitmanenterprisedealershipinventories.Where(
                    x => x.DealershipId == dealer.DealershipId && x.NewUsed == "New" && x.ModelYear == YearId && x.Make == MakeId && x.Model == ModelId);

            var trimList = new List<string>();

            foreach (var tmp in result.Select(x => x.Trim).Distinct().ToList())
            {
                if (String.IsNullOrEmpty(tmp))
                    trimList.Add("Unspecified");
                else
                {
                    trimList.Add(tmp);
                }
            }

            rebateModel.TrimList = trimList;

            rebateModel.BodyTypeList = result.Select(x => x.BodyType).Distinct().ToList();

            return new DataContractJsonResult(rebateModel);
        }

        public JsonResult ApplyReabte(int YearId, string MakeId, string ModelId, string TrimId, string BodyType, string RebateAmount, string Disclaimer)
        {
            try
            {
                var contextVinControl = new whitmanenterprisewarehouseEntities();

                var dealer = (DealershipViewModel)Session["Dealership"];

                var trimIdList = new List<ManafacturerRebateDistinctModel>();

                if (TrimId.Equals("All Trims"))
                {
                    var result =
                    contextVinControl.whitmanenterprisedealershipinventories.Where(
                        x => x.DealershipId == dealer.DealershipId && x.NewUsed == "New" && x.ModelYear == YearId && x.Make == MakeId && x.Model == ModelId);

                    foreach (var tmp in result)
                    {
                        tmp.ManufacturerRebate = CommonHelper.RemoveSpecialCharactersForMsrp(RebateAmount);
                        tmp.Disclaimer = Disclaimer;

                    }

                    foreach (var tmp in result.Select(x => x.Trim).Distinct().ToList())
                    {
                        var newRabate = new vincontrolrebate()
                        {
                            Year = YearId,
                            Make = MakeId,
                            Model = ModelId,
                            Trim = tmp,
                            BodyType = BodyType,
                            ManufactureReabte =
                                CommonHelper.RemoveSpecialCharactersForMsrp(RebateAmount),
                            Disclaimer = Disclaimer,
                            DateAdded = DateTime.Now,
                            LastUpdated = DateTime.Now,
                            DealerId = dealer.DealershipId
                        };
                        contextVinControl.AddTovincontrolrebates(newRabate);

                    }
                    contextVinControl.SaveChanges();

                    var newRebateList =
                        contextVinControl.vincontrolrebates.Where(
                            x => x.Year == YearId && x.Make == MakeId && x.Model == ModelId).ToList();

                    foreach (var tmp in newRebateList)
                    {
                        var rebate = new ManafacturerRebateDistinctModel()
                        {
                            UniqueId = tmp.vincontrolrebateid,
                            Trim = tmp.Trim,
                        };

                        trimIdList.Add(rebate);
                    }


                    return new DataContractJsonResult(trimIdList);
                }
                else
                {

                    if (TrimId.Equals("Unspecified"))
                        TrimId = "";

                    var result =
                        contextVinControl.whitmanenterprisedealershipinventories.Where(
                            x => x.DealershipId == dealer.DealershipId && x.NewUsed == "New" && x.ModelYear == YearId && x.Make == MakeId && x.Model == ModelId && x.Trim == TrimId);

                    foreach (var tmp in result)
                    {
                        tmp.ManufacturerRebate = CommonHelper.RemoveSpecialCharactersForMsrp(RebateAmount);
                        tmp.Disclaimer = Disclaimer;
                    }


                    var newRabate = new vincontrolrebate()
                    {
                        Year = YearId,
                        Make = MakeId,
                        Model = ModelId,
                        Trim = TrimId,
                        BodyType = BodyType,
                        ManufactureReabte = CommonHelper.RemoveSpecialCharactersForMsrp(RebateAmount),
                        Disclaimer = Disclaimer,
                        DateAdded = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        DealerId = dealer.DealershipId
                    };

                    contextVinControl.AddTovincontrolrebates(newRabate);

                    contextVinControl.SaveChanges();

                    var newRebateList =
                        contextVinControl.vincontrolrebates.Where(
                            x => x.Year == YearId && x.Make == MakeId && x.Model == ModelId && x.Trim == TrimId).ToList();

                    foreach (var tmp in newRebateList)
                    {
                        var rebate = new ManafacturerRebateDistinctModel()
                        {
                            UniqueId = tmp.vincontrolrebateid,
                            Trim = tmp.Trim,
                        };

                        trimIdList.Add(rebate);
                    }

                    return new DataContractJsonResult(trimIdList);
                }


            }
            catch (Exception ex)
            {
                return new DataContractJsonResult("Error");
            }

        }

        public JsonResult DeleteRebate(int rebateId)
        {
            try
            {
                var contextVinControl = new whitmanenterprisewarehouseEntities();

                var dealer = (DealershipViewModel)Session["Dealership"];


                var searchrebate = contextVinControl.vincontrolrebates.FirstOrDefault(x => x.vincontrolrebateid == rebateId);

                var result =
                    contextVinControl.whitmanenterprisedealershipinventories.Where(
                        x => x.DealershipId == dealer.DealershipId && x.NewUsed == "New" && x.ModelYear == searchrebate.Year && x.Make == searchrebate.Make && x.Model == searchrebate.Model && x.Trim == searchrebate.Trim);

                foreach (var tmp in result)
                {
                    tmp.ManufacturerRebate = null;
                    tmp.Disclaimer = null;
                }

                contextVinControl.Attach(searchrebate);

                contextVinControl.DeleteObject(searchrebate);

                contextVinControl.SaveChanges();

                return new DataContractJsonResult("Success");
            }
            catch (Exception)
            {
                return new DataContractJsonResult("Error");
            }

        }

        public ActionResult UpdateRebateAmount(string RebateAmount, int RebateId)
        {
            SQLHelper.UpdateRebateAmount(RebateAmount, RebateId);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }

        public ActionResult UpdateRebateDisclaimer(string RebateDisclaimer, int RebateId)
        {
            SQLHelper.UpdateRebateDisclaimer(RebateDisclaimer, RebateId);

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
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var existingBuyerGuide = context.vincontrolwarrantytypes.FirstOrDefault(i => i.Id == convertedId);
                    if (existingBuyerGuide != null)
                    {
                        context.DeleteObject(existingBuyerGuide);
                        context.SaveChanges();
                        return "True";
                    }
                    else
                    {
                        return "Can not find the buyer guide.";
                    }
                }
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
                if (SessionHandler.Dealership == null)
                {
                    return "TimeOut";
                }

                var convertedId = Convert.ToInt32(listingId);
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var existingBuyerGuideWithName =
                            context.vincontrolwarrantytypes.FirstOrDefault(
                                i =>
                                i.DealerId == SessionHandler.Dealership.DealershipId &&
                                i.Name.ToLower().Trim().Equals(name.ToLower().Trim()) && i.Id != convertedId);

                    if (existingBuyerGuideWithName == null)
                    {
                        var existingBuyerGuide = context.vincontrolwarrantytypes.FirstOrDefault(i => i.Id == convertedId);
                        if (existingBuyerGuide != null)
                        {
                            existingBuyerGuide.Name = name;
                            existingBuyerGuide.Category = Convert.ToInt32(category);
                            context.SaveChanges();
                            return "True";
                        }
                        else
                        {
                            return "Can not find the buyer guide.";
                        }
                    }
                    else
                    {
                        return "Existing";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string AddNewBuyerGuide(string name, string category)
        {
            if (SessionHandler.Dealership == null)
            {
                return "TimeOut";
            }
            else
            {
                try
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        var existingBuyerGuide =
                            context.vincontrolwarrantytypes.FirstOrDefault(
                                i =>
                                i.DealerId == SessionHandler.Dealership.DealershipId &&
                                i.Name.ToLower().Trim().Equals(name.ToLower().Trim()));
                        if (existingBuyerGuide == null)
                        {

                            var newBuyerGuide = new vincontrolwarrantytype()
                            {
                                DealerId = SessionHandler.Dealership.DealershipId,
                                IsActive = true,
                                Name = name,
                                Category = Convert.ToInt32(category)
                            };
                            context.AddTovincontrolwarrantytypes(newBuyerGuide);
                            context.SaveChanges();
                            return newBuyerGuide.Id.ToString();
                        }
                        else
                        {
                            return "Existing";
                        }
                    }
                }
                catch (Exception ex)
                {
                    return "Error";
                }

            }
        }

        public void UpdateButtonPermission(string groupId, string buttonId, bool canSee)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var convertedGroupId = Convert.ToInt32(groupId);
                var convertedButtonId = Convert.ToInt32(buttonId);
                var existingPermission =
                    context.vincontroldealershipbuttons.FirstOrDefault(
                        i =>
                        i.DealershipId == SessionHandler.Dealership.DealershipId && i.GroupId == convertedGroupId &&
                        i.ButtonId == convertedButtonId);
                if (existingPermission != null)
                {
                    existingPermission.CanSee = canSee;
                    context.SaveChanges();
                }
                else
                {
                    var newPermissoin = new vincontroldealershipbutton()
                                            {
                                                ButtonId = convertedButtonId,
                                                GroupId = convertedGroupId,
                                                DealershipId = SessionHandler.Dealership.DealershipId,
                                                CanSee = canSee
                                            };
                    context.AddTovincontroldealershipbuttons(newPermissoin);
                    context.SaveChanges();
                }
            }
        }
    }
}
