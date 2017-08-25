using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Security;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class SwitchController : SecurityController
    {
        //
        // GET: /Switch/
        
        public ActionResult SwitchDealership(CarInfoFormViewModel switchModel)
        {

            try
            {
                if (Session["DealerGroup"] != null)
                {
                    var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];
                    if (switchModel.SelectedDealerTransfer.Equals("999"))
                    {
                        SessionHandler.Single = false;
                        Session["DealershipName"] = dealerGroup.DealershipGroupName;
                        return Json("Success");
                    }



                    var switchDealer = dealerGroup.DealerList.First(t => t.DealershipId.ToString(CultureInfo.InvariantCulture).Equals(switchModel.SelectedDealerTransfer));

                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        var setting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == switchDealer.DealershipId);

                        switchDealer.DealerGroupId = dealerGroup.DealershipGroupId;
                        switchDealer.InventorySorting = setting.InventorySorting;

                        switchDealer.SoldOut = setting.SoldOut;

                        switchDealer.DefaultStockImageUrl = setting.DefaultStockImageUrl;
                        switchDealer.OverrideStockImage = setting.OverideStockImage.Value;
                        switchDealer.OverrideDealerKbbReport = switchDealer.OverrideDealerKbbReport;
                        switchDealer.DealerInfo = setting.DealerInfo;

                        switchDealer.DealerWarranty = setting.DealerWarranty;

                        switchDealer.TermConditon = setting.TermsAndCondition;

                        switchDealer.EbayToken = setting.EbayToken;

                        switchDealer.EbayInventoryUrl = setting.EbayInventoryURL;

                        switchDealer.CreditUrl = setting.CreditURL;

                        switchDealer.WebSiteUrl = setting.WebSiteURL;

                        switchDealer.ContactUsUrl = setting.ContactUsURL;

                        switchDealer.FacebookUrl = setting.FacebookURL;

                        switchDealer.LogoUrl = setting.LogoURL;

                        switchDealer.ContactPerson = setting.ContactUsURL;

                        switchDealer.CarFax = setting.CarFax;

                        switchDealer.CarFaxPassword = setting.CarFaxPassword;

                        switchDealer.Manheim = setting.Manheim;

                        switchDealer.ManheimPassword = setting.ManheimPassword;

                        switchDealer.KellyBlueBook = setting.KellyBlueBook;

                        switchDealer.KellyPassword = setting.KellyPassword;

                        switchDealer.BlackBook = setting.BlackBook;

                        switchDealer.BlackBookPassword = setting.BlackBookPassword;

                        switchDealer.IntervalBucketJump = setting.IntervalBucketJump.GetValueOrDefault();

                        switchDealer.FirstIntervalJump = setting.FirstTimeRangeBucketJump.GetValueOrDefault();

                        switchDealer.SecondIntervalJump = setting.SecondTimeRangeBucketJump.GetValueOrDefault();

                        switchDealer.LoanerSentence = setting.LoanerSentence;

                        switchDealer.AuctionSentence = setting.AuctionSentence;

                        SessionHandler.Single = true;
                        Session["Dealership"] = switchDealer;

                        Session["DealershipName"] = switchDealer.DealershipName;

                    }


                    return Json("Success");
                }
                else
                {
                    return Json("SessionTimeOut");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }


        }

        public ActionResult SwithDealerWindow()
        {
            var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

            var dealer = (DealershipViewModel)Session["Dealership"];

            var model = new CarInfoFormViewModel()
            {
                DealershipName = dealer.DealershipName,
                SelectedDealerTransfer = (dealerGroup != null && dealerGroup.DealerList != null && dealerGroup.DealerList.Count > 1 && Session["Single"] != null && !(bool)Session["Single"]) ? "999" : dealer.DealershipId.ToString(),
                TransferDealerGroup = SelectListHelper.InitialDealerList(dealerGroup)
            };


            return View("SwitchDealership", model);


        }
    }
}


