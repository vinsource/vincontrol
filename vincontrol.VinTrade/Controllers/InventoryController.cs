using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.AdminManagement;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Vinsocial.Forms.ReviewManagement;
using vincontrol.Application.Forms.TradeInManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.TradeInManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.VinTrade.Handlers;
using vincontrol.VinTrade.Helpers;
using vincontrol.CarFax;

namespace vincontrol.VinTrade.Controllers
{
    public class InventoryController : Controller
    {
        private IAdminManagementForm _adminManagement;
        private IDealerManagementForm _dealerManagement;
        private ICommonManagementForm _commonManagement;
        private ITradeInManagementForm _tradeInManagement;
        private IReviewManagementForm _reviewManagement;
        private ICarFaxService _carFaxService;
        private const string CarfaxWarningImage = "http://vincontrol.com/alpha/content/CarfaxWarning.jpg";
        public InventoryController()
        {
            _adminManagement = new AdminManagementForm();
            _dealerManagement = new DealerManagementForm();
            _commonManagement = new CommonManagementForm();
            _tradeInManagement = new TradeInManagementForm();
            _reviewManagement = new ReviewManagementForm();
            _carFaxService=new CarFaxService();
        }

        public ActionResult Index(string dealer, string year, string make, string model, string trim, string vin)
        {
            var viewModel = new LandingViewModel();
            
            var dealerInfo = _dealerManagement.GetDealerByName(dealer);
            SessionHandler.TradeInDealer = dealerInfo;
            viewModel.DealerInfo = dealerInfo;
            
            if (!String.IsNullOrEmpty(vin))
            {
                var existingInventory = _tradeInManagement.GetInventory(vin, dealerInfo.DealershipId);
                viewModel.VehicleInfo = existingInventory;

                if (existingInventory.InventoryCondition == Constanst.ConditionStatus.Used)
                {
                    var carfax = _carFaxService.GetCarFaxReportInDatabase(existingInventory.VehicleId);

                    if (carfax.Success)
                        viewModel.VehicleInfo.CarFax = carfax;
                    else
                    {
                        viewModel.VehicleInfo.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(existingInventory.VehicleId, existingInventory.Vin, dealerInfo.DealerSetting.CarFax, dealerInfo.DealerSetting.CarFaxPassword);
                    }

                    if (viewModel.VehicleInfo.CarFax != null)
                    {
                        var reportList = new List<CarFaxWindowSticker>();
                        foreach (var tmp in viewModel.VehicleInfo.CarFax.ReportList)
                        {
                            if (!tmp.Image.Equals(CarfaxWarningImage))
                            {
                                reportList.Add(tmp);
                            }
                        }
                    }
                }
            }
          
            // Getting reviews
            viewModel.DealerReview.UserReviews = _reviewManagement.GetGoodUserReviews(dealerInfo.DealershipId);
            viewModel.DealerReview.OverallScore = viewModel.DealerReview.UserReviews.Any() ? viewModel.DealerReview.UserReviews.Average(i => i.Rating) : 0;

            SessionHandler.LandingInfo = viewModel;
            return View("NewTradeIn", viewModel);
        }

        //public ActionResult Index(string dealer, int inventoryId, string year, string make, string model, string trim, string vin)
        //{
        //    var viewModel = new LandingViewModel();

        //    if (inventoryId > 0)
        //    {
        //        var existingInventory = _tradeInManagement.GetInventory(inventoryId);

        //        SessionHandler.TradeInDealer = existingInventory.DealerRef;
        //        viewModel.DealerInfo = existingInventory.DealerRef;


        //        viewModel.VehicleInfo = existingInventory;

        //        if (existingInventory.InventoryCondition == Constanst.ConditionStatus.Used)
        //        {
        //            var dealerSetting = _adminManagement.LoadSetting(existingInventory.DealerId);
        //            var carfax = _carFaxService.GetCarFaxReportInDatabase(existingInventory.VehicleId);

        //            if (carfax.Success)
        //                viewModel.VehicleInfo.CarFax = carfax;
        //            else
        //            {
        //                viewModel.VehicleInfo.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(existingInventory.VehicleId, existingInventory.Vin, dealerSetting.CarFax, dealerSetting.CarFaxPassword);
        //            }

        //            if (viewModel.VehicleInfo.CarFax != null)
        //            {
        //                var reportList = new List<CarFaxWindowSticker>();
        //                foreach (var tmp in viewModel.VehicleInfo.CarFax.ReportList)
        //                {
        //                    if (!tmp.Image.Equals(CarfaxWarningImage))
        //                    {
        //                        reportList.Add(tmp);
        //                    }
        //                }
        //            }
        //        }
        //        viewModel.DealerReview.UserReviews = _reviewManagement.GetGoodUserReviews(existingInventory.DealerId);
        //        viewModel.DealerReview.OverallScore = viewModel.DealerReview.UserReviews.Any() ? viewModel.DealerReview.UserReviews.Average(i => i.Rating) : 0;

        //    }

        //    // Getting reviews

        //    SessionHandler.LandingInfo = viewModel;
        //    return View("NewTradeIn", viewModel);
        //}

        public ActionResult PrintTradeIn()
        {
            return View("PrintTradeIn", SessionHandler.LandingInfo);
        }

        [HttpPost]
        public void InsertCustomerInfo(ContactViewModel customerinformation)
        {
            // Sending email
            customerinformation.DealerName = SessionHandler.TradeInDealer.DealershipName;
            try
            {
                LandingPageEmailHelper.SendEmail(customerinformation, SessionHandler.TradeInDealer);
            }
            catch (Exception)
            {
                // Failed.
            }
            
            _tradeInManagement.AddNewContact(customerinformation);
        }

        [HttpPost]
        public void InsertPriceAlert(PriceAlertViewModel model)
        {
            var customerinformation = new ContactViewModel()
            {
                DealerName = SessionHandler.TradeInDealer.DealershipName,
                firstname = model.FirstName,
                lastname = model.LastName,
                phone_number = model.Phone,
                email_address = model.Email,
                contact_type = 1,
                vinnumber = SessionHandler.LandingInfo.VehicleInfo.Vin,
                StockNumber = SessionHandler.LandingInfo.VehicleInfo.StockNumber,
                Trim = SessionHandler.LandingInfo.VehicleInfo.SelectedTrim
                
            };

            try
            {
                LandingPageEmailHelper.SendEmail(customerinformation, SessionHandler.TradeInDealer);
            }
            catch (Exception)
            {
                // Failed.
            }
            _tradeInManagement.AddNewPriceAlert(model);
        }
    }
}
