using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.FacebookManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Forms.VehicleLogManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.FacebookManagement;
using vincontrol.ConfigurationManagement;
using vincontrol.Constant;
using Vincontrol.Facebook;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Security;

namespace Vincontrol.Web.Controllers
{
    public class FacebookController : SecurityController
    {
        private const string Success = "Success";
        private IInventoryManagementForm _inventoryManagementForm;
        private IFacebookWrapper _facebookWrapper;
        private IFacebookManagementForm _facebookForm;
        private IVehicleLogManagementForm _vehicleLogForm;

        public FacebookController()
        {
            _inventoryManagementForm = new InventoryManagementForm();
            _facebookWrapper = new FacebookWrapper();
            _facebookForm = new FacebookManagementForm();
            _vehicleLogForm=new VehicleLogManagementForm();
        }

        public ActionResult AddNewPost(int listingId, int inventoryStatus)
        {
            var inventory = inventoryStatus==Constanst.VehicleStatus.Inventory ? _inventoryManagementForm.GetCarInfo( listingId) : _inventoryManagementForm.GetSoldCarInfo(listingId);
            var viewModel = _facebookForm.InitializeFacebookPostViewModel();
            viewModel.InventoryStatus = inventoryStatus;
            viewModel.InventoryId = listingId;
            viewModel.DealerId = SessionHandler.Dealer.DealershipId;
            viewModel.UserId = SessionHandler.CurrentUser.UserId;
            viewModel.PublishDate = DateTime.Now;
            viewModel.Link = !String.IsNullOrEmpty(inventory.CarImageUrl) ? inventory.CarImageUrl.Split(',')[0] : inventory.DefaultImageUrl;
            viewModel.Content += String.Format("{0} {1} {2} {3}", inventory.ModelYear, inventory.Make, inventory.Model, inventory.Trim);
            viewModel.Content += "\r\n\r\nVIN: " + inventory.Vin;
            viewModel.Content += "\r\nStock: " + inventory.StockNumber;
            viewModel.Content += "\r\nOdometer: " + inventory.Odometer.ToString("0,0");
            viewModel.Content += "\r\nPrice: " + inventory.Price.ToString("c0");
            viewModel.Content += "\r\nExterior Color: " + inventory.ExteriorColor;
            if (!string.IsNullOrEmpty(inventory.Fuel)) viewModel.Content += "\r\nFuel: " + inventory.Fuel;
            if (!string.IsNullOrEmpty(inventory.Tranmission)) viewModel.Content += "\r\nTranmission: " + inventory.Tranmission;
            if (!string.IsNullOrEmpty(inventory.Description)) viewModel.Content += "\r\nDescription: " + inventory.Description;

            return View(viewModel);
        }

        [HttpPost]
        public string AddNewPost(FacebookPostViewModel model, HttpPostedFileBase attachedFile)
        {
            try
            {
                var accessToken = _facebookWrapper.GetShortToken(SessionHandler.Dealer.DealershipId);

                if (String.IsNullOrEmpty(accessToken))
                    return Constanst.AjaxMessage.Error;

                model.AccessToken = accessToken;
                DirectPosting(model);
            }
            catch (Exception)
            {
                return Constanst.AjaxMessage.Error;
            }

            return Success;
        }

        [HttpPost]
        public string AddNewPhoto(HttpPostedFileBase attachedFile)
        {
            try
            {
                if (attachedFile == null || attachedFile.ContentLength == 0) return string.Empty;
                string pathForSaving = Server.MapPath(ConfigurationHandler.UploadFolderPath);
                if (FileExtensions.CreateFolderIfNeeded(pathForSaving))
                {
                    string fileExtension = Path.GetExtension(attachedFile.FileName);
                    var fileName = string.Format("facebook_post_{0}{1}", DateTime.Now.Ticks, fileExtension);
                    var localFilePath = Path.Combine(pathForSaving, fileName);
                    attachedFile.SaveAs(localFilePath);
                    return localFilePath;
                }

                return string.Empty;
            }
            catch (Exception)
            {

            }

            return Success;
        }

        [HttpPost]
        public string GetAccessToken()
        {
            var accessToken = ConfigurationHandler.FacebookPageToken; 
            return accessToken;
        }

        public bool CheckCredentialExisting()
        {
            return _facebookForm.CheckCredentialExisting(SessionHandler.Dealer.DealershipId);
        }

        #region Private
        private void DirectPosting(FacebookPostViewModel model)
        {
            if (string.IsNullOrEmpty(model.Picture))
            {
                var facebookPostId = _facebookWrapper.Post(_facebookForm.GetPageId(model.DealerId).ToString(CultureInfo.InvariantCulture), model);

                if (facebookPostId > 0)
                {
                    model.RealPostId = facebookPostId;

                    if(model.InventoryStatus==Constanst.VehicleStatus.Inventory)
                        _vehicleLogForm.AddVehicleLog(model.InventoryId, model.UserId,
                        Constanst.VehicleLogSentence.FacebookByUser.Replace("USER",SessionHandler.CurrentUser.FullName)
                        .Replace("LINK", SessionHandler.Dealer.FacebookUrl + "/posts/" + model.RealPostId)
                        ,null);
                    else
                    {
                        _vehicleLogForm.AddVehicleLog(null, model.UserId,
                       Constanst.VehicleLogSentence.FacebookByUser.Replace("USER", SessionHandler.CurrentUser.FullName)
                       .Replace("LINK", SessionHandler.Dealer.FacebookUrl + "/posts/" + model.RealPostId)
                       , model.InventoryId);
                    }

                    _facebookForm.AddFbPostTracking(model);

                }
            }
            else
            {
                var extension = Path.GetExtension(model.Picture);
                switch (extension.ToLower())
                {
                    case ".gif":
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                        _facebookWrapper.PostPhoto(_facebookForm.GetPageId(model.DealerId).ToString(CultureInfo.InvariantCulture), model);
                        break;
                    case ".mpg":
                    case ".mpeg":
                    case ".mp4":
                    case ".mpeg4":
                    case ".wmv":
                    case ".3gp":
                    case ".avi":
                    case ".flv":
                        _facebookWrapper.PostVideo(_facebookForm.GetPageId(model.DealerId).ToString(CultureInfo.InvariantCulture), model);
                        break;
                }
            }
        }
        #endregion
    }
}
