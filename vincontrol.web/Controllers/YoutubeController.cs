using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Forms.VideoTrackingManagement;
using vincontrol.Application.Forms.YoutubeManagement;
using vincontrol.Application.ViewModels.VideoTrackingManagement;
using vincontrol.Application.ViewModels.YoutubeManagement;
using vincontrol.Constant;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Security;

namespace Vincontrol.Web.Controllers
{
    public class YoutubeController : SecurityController
    {
        private IInventoryManagementForm _inventoryManagementForm;
        private IYoutubeManagementForm _youtubeManagementForm;
        private IVideoTrackingManagementForm _videoTrackingManagementForm;

        public YoutubeController()
        {
            _inventoryManagementForm = new InventoryManagementForm();
            _youtubeManagementForm = new YoutubeManagementForm();
            _videoTrackingManagementForm = new VideoTrackingManagementForm();
        }

        public ActionResult AddNewVideo(int listingId, int inventoryStatus)
        {
            var inventory = inventoryStatus == Constanst.VehicleStatus.Inventory ? _inventoryManagementForm.GetCarInfo(listingId) : _inventoryManagementForm.GetSoldCarInfo(listingId);
            return View(_youtubeManagementForm.InitializeYoutubePostViewModel(inventory));
        }

        [HttpPost]
        public string AddNewVideo(YoutubeVideoViewModel model)
        {
            try
            {
                var newVideoModel = new VideoTrackingViewModel()
                {
                    DealerId = model.DealerId,
                    InventoryId = model.InventoryId,
                    Url = string.Empty,
                    IsPosted = false,
                    IsSucceeded = false,
                    CreatedDate = DateUtilities.Now(),
                    LastDate = DateUtilities.Now()
                };
               _videoTrackingManagementForm.AddNewVideo(newVideoModel);
            }
            catch (Exception)
            {
                return Constanst.AjaxMessage.Error;
            }

            return Constanst.AjaxMessage.Success;
        }

        public bool CheckCredentialExisting()
        {
            return (!String.IsNullOrEmpty(SessionHandler.Dealer.DealerSetting.YoutubeUsername) && !String.IsNullOrEmpty(SessionHandler.Dealer.DealerSetting.YoutubePassword));
        }
    }
}
