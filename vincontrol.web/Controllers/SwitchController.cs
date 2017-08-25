using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Security;


namespace Vincontrol.Web.Controllers
{
    public class SwitchController : SecurityController
    {
        //
        // GET: /Switch/

        private IAccountManagementForm _accountManagementForm;
        private IDealerManagementForm _dealerManagementForm;

        public SwitchController()
        {
            _accountManagementForm = new AccountManagementForm();
            _dealerManagementForm = new DealerManagementForm();
        }


        public ActionResult SwitchDealer(string selectedDealerTransferID)
        {

            try
            {
                if (SessionHandler.DealerGroup != null)
                {
                    var dealerGroup = SessionHandler.DealerGroup;
                    if (selectedDealerTransferID.Equals(dealerGroup.DealershipGroupId))
                    {
                        SessionHandler.AllStore = true;
                        return Json("Success");
                    }

                    var switchDealer = dealerGroup.DealerList.First(t => t.DealershipId.ToString(CultureInfo.InvariantCulture).Equals(selectedDealerTransferID));
                    SessionHandler.Single = true;
                    SessionHandler.AllStore = false;
                    SessionHandler.Dealer = switchDealer;

                    var switchUser = SessionHandler.CurrentUser;
                    switchUser.DealershipId = switchDealer.DealershipId;
                    SQLHelper.SwitchUserRole(ref switchUser, selectedDealerTransferID);
                    SessionHandler.CurrentUser = switchUser;
                    SessionHandler.UserRight.UpdateAllSettingsFromDatabase();
                    SessionHandler.NumberOfWSTemplate =
                        _dealerManagementForm.GetDealerWindowStickerTemplate(SessionHandler.Dealer.DealershipId).Count();

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
        
        public ActionResult SwitchDealership(CarInfoFormViewModel switchModel)
        {

            try
            {
                if (SessionHandler.DealerGroup != null)
                {
                    var dealerGroup = SessionHandler.DealerGroup;
                    if (switchModel.SelectedDealerTransfer.Equals("999"))
                    {
                        SessionHandler.Single = false;
                        return Json("Success");
                    }



                    var switchDealer = dealerGroup.DealerList.First(t => t.DealershipId.ToString(CultureInfo.InvariantCulture).Equals(switchModel.SelectedDealerTransfer));
                   
                    SessionHandler.Single = true;
                    SessionHandler.Dealer = switchDealer;
                 
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

   
    }
}


