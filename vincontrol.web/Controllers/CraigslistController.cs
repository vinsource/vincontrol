using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using VINControl.Craigslist;
using Vincontrol.Web.Security;
using Vincontrol.Web.Handlers;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Forms.DealerManagement;

namespace Vincontrol.Web.Controllers
{
    public class CraigslistController : SecurityController
    {
        private IAccountManagementForm _accountForm;
        private IInventoryManagementForm _inventoryForm;
        private IDealerManagementForm _dealerForm;
        private CraigslistService _craigslistService;
        private DealerViewModel _dealer;
        private CarShortViewModel _car;

        public CraigslistController()
        {
            _accountForm = new AccountManagementForm();
            _inventoryForm = new InventoryManagementForm();
            _dealerForm = new DealerManagementForm();
            _craigslistService = new CraigslistService();
            _dealer = new DealerViewModel();
            _car = new CarShortViewModel();
        }

        public ActionResult Index(int listingId)
        {            
            ViewData["LISTINGID"] = listingId;
            return View("Index", _craigslistService.GetStateList());
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public int AuthenticationChecking()
        {
            _craigslistService.WebRequestPost(SessionHandler.Dealer.DealerCraigslistSetting.Email, SessionHandler.Dealer.DealerCraigslistSetting.Password);
            return _craigslistService.StatusCode;
        }

        public bool LocationChecking()
        {
            var state = SessionHandler.Dealer.DealerCraigslistSetting.State;
            var city = SessionHandler.Dealer.DealerCraigslistSetting.City;
            var location = SessionHandler.Dealer.DealerCraigslistSetting.Location;

            return (state != string.Empty && city != string.Empty && location != string.Empty);
        }

        public JsonResult GetSubLocationList(string location)
        {
            var list = _craigslistService.GetSubLocationList(_craigslistService.GetSubLocationChoosingUrl(location));
            return Json(new {data = list}, JsonRequestBehavior.AllowGet) ;
        }

        public ActionResult GoToPostingPreviewPage(string listingId)
        {
            ViewData["LISTINGID"] = listingId;
            return View("PostingPreview");
        }

        public ActionResult UploadImage(string listingId)
        {
            _dealer = _accountForm.GetDealer(SessionHandler.CurrentUser.DealershipId);
            _dealer.DealerImagesFolder = System.Web.HttpContext.Current.Server.MapPath("/DealerImages");
            _car = _inventoryForm.GetCarInfo(SessionHandler.CurrentUser.DealershipId, Convert.ToInt32(listingId));
            var posting = _craigslistService.GoToPostingPreviewPage(SessionHandler.Dealer.DealerCraigslistSetting.Email, SessionHandler.Dealer.DealerCraigslistSetting.Password, _dealer, _car);
            ViewData["ListingId"] = listingId;
            ViewData["LocationUrl"] = posting.LocationUrl;
            ViewData["CryptedStepCheck"] = posting.CryptedStepCheck;
            ViewData["PostingTitle"] = posting.Post.Title;
            return PartialView("UploadImage", posting);
        }

        [HttpPost]
        public ActionResult GoToPurchasingPage(PostingBasicInfo postData)
        {            
            ViewData["PostingTitle"] = postData.PostingTitle;
            return PartialView("Purchasing", new CreditCardInfo() { ContactEmail = SessionHandler.Dealer.DealerCraigslistSetting.Email, LocationUrl = postData.LocationUrl, CryptedStepCheck = postData.CryptedStepCheck, ListingId = postData.ListingId });
        }

        [HttpPost]
        public string Purchase(CreditCardInfo postData)
        {
            try
            {
                var confirmation = _craigslistService.GoToPurchasingPage(SessionHandler.Dealer.DealerCraigslistSetting.Email, SessionHandler.Dealer.DealerCraigslistSetting.Password, postData);
                _dealerForm.AddNewCraigslistHistory(SessionHandler.CurrentUser.UserId, postData.ListingId, confirmation.PaymentId, confirmation.PostingId);
                return "Success";
            }
            catch (Exception ex)
            {
                return "Error";
            }            
        }
    }

    public class PostingBasicInfo
    {
        public int ListingId { get; set; }
        public string LocationUrl { get; set; }
        public string CryptedStepCheck { get; set; }
        public string PostingTitle { get; set; }
    }
}
