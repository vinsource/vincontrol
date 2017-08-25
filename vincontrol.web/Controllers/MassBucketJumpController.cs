using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.InventoryManagement;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Security;

namespace Vincontrol.Web.Controllers
{
    public class MassBucketJumpController : SecurityController
    {
        private IInventoryManagementForm _inventoryManagementForm;

        public MassBucketJumpController()
        {
            _inventoryManagementForm = new InventoryManagementForm();
        }

        public ActionResult Index()
        {
            if (!SessionHandler.Dealer.IsPendragon || !SessionHandler.AllStore)
                return RedirectToAction("ViewInventory", "Inventory");

            if (SessionHandler.MassBucketJumpViewInfo == null)
            {
                SessionHandler.MassBucketJumpViewInfo = new ViewInfo { IsUp = true, SortFieldName = SessionHandler.Dealer.DealerSetting.InventorySorting, CurrentState = 0, CurrentView = BucketJumpView.LandRover.ToString() };
            }
            return View();
        }

        public string UpdateCertified(int inventoryId, bool certified, decimal amount)
        {
            try
            {
                _inventoryManagementForm.UpdateMassBucketJumpCertified(inventoryId, certified, amount);
                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public string UpdateACar(int inventoryId, bool acar, decimal amount)
        {
            try
            {
                _inventoryManagementForm.UpdateMassBucketJumpACar(inventoryId, acar, amount);
                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public ActionResult Apply(int id)
        {
            var massBucketJump = _inventoryManagementForm.GetMassBucketJumpByInventoryId(id);
            if (massBucketJump != null)
            {
                ViewData["InventoryId"] = id;
                ViewData["MarketDealer"] = massBucketJump.MarketDealer;
                ViewData["MarketDealerImage"] = massBucketJump.MarketDealerImage;
                ViewData["MarketPrice"] = Convert.ToInt32(massBucketJump.MarketPrice);
                ViewData["MarketYear"] = massBucketJump.MarketYear;
                ViewData["MarketMake"] = massBucketJump.MarketMake;
                ViewData["MarketModel"] = massBucketJump.MarketModel;
                ViewData["MarketColor"] = massBucketJump.MarketColor;
                ViewData["MarketOdometer"] = Convert.ToInt32(massBucketJump.MarketOdometer);
                ViewData["MarketPlusPrice"] = Convert.ToInt32(massBucketJump.MarketPlusPrice);
                ViewData["WholesaleWithOptions"] = massBucketJump.WholesaleWithOptions;
                ViewData["WholesaleWithoutOptions"] = massBucketJump.WholesaleWithoutOptions;
                ViewData["MarketOption"] = massBucketJump.MarketOption;
                ViewData["MarketVIN"] = massBucketJump.MarketVIN;
            }

            return View();
        }

        public ActionResult DailyBucketJumpReport()
        {
            return View();
        }
    }
}
