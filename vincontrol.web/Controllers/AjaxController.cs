using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using vincontrol.Application.Forms.AppraisalManagement;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.ChromeAutoService;
using vincontrol.ChromeAutoService.AutomativeService;
//using Vincontrol.Web.com.chromedata.services.Description7a;
using vincontrol.Data.Model;
using vincontrol.Helper;
using vincontrol.KBB;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;

namespace Vincontrol.Web.Controllers
{
    public class AjaxController : SecurityController
    {
        const double PIx = 3.141592653589793;
        const double Radio = 3958.75587; // Mean radius of Earth in Miles
        private ICarFaxService _carFaxService;
        private IKBBService _kbbService;
        private ICommonManagementForm _commonManagementForm;
        private IInventoryManagementForm _inventoryManagementForm;
        private IAppraisalManagementForm _appraisalManagementForm;
        public AjaxController()
        {
            _carFaxService = new CarFaxService();
            _kbbService = new KBBService();
            _commonManagementForm = new CommonManagementForm();
            _inventoryManagementForm = new InventoryManagementForm();
            _appraisalManagementForm = new AppraisalManagementForm();
        }
      
        public JsonResult YearAjaxPost(string modelYear)
        {
            var autoService = new ChromeAutoService();

            var makeList = autoService.GetDivisions(Convert.ToInt32(modelYear)).ToList();

        
            return Json(makeList.OrderBy(x => x.Value));
        }

        public JsonResult MakeAjaxPost(string modelYear, string makeId)
        {
            var year = Convert.ToInt32(CommonHelper.RemoveSpecialCharactersAndReturnNumber(modelYear));
            var id = Convert.ToInt32(makeId);
            if (id <= 0) { return null; }
            var autoService = new ChromeAutoService();
            var list = autoService.GetModelsByDivision(year, id);
            return list != null ? Json(list.ToList()) : Json("ManualMode");
        }

        public JsonResult ModelAjaxPost(string modelId)
        {
            var id = Convert.ToInt32(modelId);

            if (id <= 0) { return null; }
            var autoService = new ChromeAutoService();

            var styles = autoService.GetStyles(id);

            var list = new List<string>();

            try
            {
                if (styles != null && styles.Any())
                {
                    list.AddRange(autoService.InitializeTrims(styles));

                    int styleId = styles.First().id;

                    var styleInfo = autoService.GetStyleInformationFromStyleId(styleId);

                    if (styleInfo != null)
                    {
                        list.Add(autoService.InitializeDoor(styleInfo));

                        list.AddRange(autoService.InitializeExteriorColors(styleInfo.exteriorColor));

                        list.AddRange(autoService.InitializeInteriorColors(styleInfo.interiorColor));

                        list.AddRange(autoService.InitializeEngineStyles(styleInfo.engine));

                        list.Add(autoService.InitializeBodyStyle(styleInfo));

                        list.AddRange(autoService.InitializePackages(styleInfo));

                        list.AddRange(autoService.InitializeAdditionalOptions(styleInfo));

                        if (styleInfo.style.First().stockImage != null) list.Add(styleInfo.style.First().stockImage.url + "DefaultImage");

                        list.Add(styleInfo.basePrice.msrp.high + "MSRP");

                        list.AddRange(GetDriveXmlFileFromVehicleInfo(styleInfo.style.First().drivetrain.ToString()));
                    }
                }

                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(ex.StackTrace);
            }
        }

        public JsonResult ModelAjaxPostWithYearMakeModel(int year, string make, string model)
        {
            var autoService = new ChromeAutoService();

            var styles = autoService.GetTrims(year, make, model);

            var list = new List<string>();

            try
            {
                if (styles != null && styles.Any())
                {
                    list.AddRange(autoService.InitializeTrims(styles));
                    
                    int styleId = styles.First().id;

                    var styleInfo = autoService.GetStyleInformationFromStyleId(styleId);

                    if (styleInfo != null)
                    {
                        list.Add(autoService.InitializeDoor(styleInfo));

                        list.AddRange(autoService.InitializeExteriorColors(styleInfo.exteriorColor));

                        list.AddRange(autoService.InitializeInteriorColors(styleInfo.interiorColor));

                        list.AddRange(autoService.InitializeEngineStyles(styleInfo.engine));

                        list.Add(autoService.InitializeBodyStyle(styleInfo));

                        list.AddRange(autoService.InitializePackages(styleInfo));

                        list.AddRange(autoService.InitializeAdditionalOptions(styleInfo));

                        if (styleInfo.style.First().stockImage != null) list.Add(styleInfo.style.First().stockImage.url + "DefaultImage");

                        list.Add(styleInfo.basePrice.msrp.high + "MSRP");

                        list.AddRange(GetDriveXmlFileFromVehicleInfo(styleInfo.style.First().drivetrain.ToString()));
                    }
                }

                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(ex.StackTrace);
            }
        }
        
        public JsonResult StyleAjaxPost(string styleId,int listingId)
        {
            int id ;
            var vin = string.Empty;
            List<string> existOptions=null;
            List<string> existPackages = null;
            var list = new List<string>();

            if (listingId > 0)
            {
                var inventory = _inventoryManagementForm.GetInventory(listingId);
                if (inventory != null)
                {
                    existOptions = String.IsNullOrEmpty(inventory.AdditionalOptions)
                        ? null
                        : (from data in
                            inventory.AdditionalOptions.Split(new[] {",", "|"}, StringSplitOptions.RemoveEmptyEntries)
                            select data.Trim()).ToList();

                    existPackages = String.IsNullOrEmpty(inventory.AdditionalPackages)
                        ? null
                        : (from data in
                            inventory.AdditionalPackages.Split(new[] {",", "|"}, StringSplitOptions.RemoveEmptyEntries)
                            select data.Trim()).ToList();

                    vin = inventory.Vehicle.Vin;
                }
                else
                {
                    var soldInventory = _inventoryManagementForm.GetSoldInventory(listingId);
                    existOptions = String.IsNullOrEmpty(soldInventory.AdditionalOptions)
                        ? null
                        : (from data in
                               soldInventory.AdditionalOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                            select data.Trim()).ToList();

                    existPackages = String.IsNullOrEmpty(soldInventory.AdditionalPackages)
                        ? null
                        : (from data in
                               soldInventory.AdditionalPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                            select data.Trim()).ToList();

                    vin = soldInventory.Vehicle.Vin;
                }

            }

            if (Int32.TryParse(styleId, out id))
            {
                var autoService = new ChromeAutoService();

                var styleInfo = autoService.GetStyleInformationFromStyleId(id);

                if (styleInfo != null)
                {
                    list.Add(autoService.InitializeDoor(styleInfo));

                    list.AddRange(autoService.InitializeExteriorColors(styleInfo.exteriorColor));

                    list.AddRange(autoService.InitializeInteriorColors(styleInfo.interiorColor));

                    list.AddRange(autoService.InitializeEngineStyles(styleInfo.engine));

                    list.Add(autoService.InitializeBodyStyle(styleInfo));

                    list.AddRange(existPackages != null
                        ? autoService.InitializePackages(styleInfo, existPackages)
                        : autoService.InitializePackages(styleInfo));

                    list.AddRange(existOptions != null
                        ? autoService.InitializeAdditionalOptions(styleInfo, existOptions)
                        : autoService.InitializeAdditionalOptions(styleInfo));


                    if (styleInfo.style.First().stockImage != null) list.Add(styleInfo.style.First().stockImage.url + "DefaultImage");

                    list.Add(styleInfo.basePrice.msrp.high + "MSRP");

                    if (styleInfo.genericEquipment != null) list.AddRange(GetDriveXmlFileFromVehicleInfo(styleInfo.style.First().drivetrain.ToString()));
                }
                else
                {

                    var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);

                    if (vehicleInfo != null)
                    {

                        list.Add(autoService.InitializeDoor(vehicleInfo));

                        list.AddRange(autoService.InitializeExteriorColors(vehicleInfo.exteriorColor));

                        list.AddRange(autoService.InitializeInteriorColors(vehicleInfo.interiorColor));

                        list.AddRange(autoService.InitializeEngineStyles(vehicleInfo.engine));

                        list.Add(autoService.InitializeBodyStyle(vehicleInfo));

                        list.AddRange(existPackages != null
                            ? autoService.InitializePackages(vehicleInfo, existPackages)
                            : autoService.InitializePackages(vehicleInfo));

                        list.AddRange(existOptions != null
                            ? autoService.InitializeAdditionalOptions(vehicleInfo, existOptions)
                            : autoService.InitializeAdditionalOptions(vehicleInfo));


                        if (vehicleInfo.style.First().stockImage != null) list.Add(vehicleInfo.style.First().stockImage.url + "DefaultImage");

                        list.Add(vehicleInfo.basePrice.msrp.high + "MSRP");

                        if (vehicleInfo.genericEquipment != null) list.AddRange(GetDriveXmlFileFromVehicleInfo(vehicleInfo.style.First().drivetrain.ToString()));
                    }
                }

               
            }

            return Json(list);
        }

        public JsonResult StyleAjaxAppraisalPost(string styleId, int listingId)
        {
            int id;

            var list = new List<string>();
            List<string> existOptions = null;
            List<string> existPackages = null;

            var inventory = _appraisalManagementForm.GetAppraisal(listingId);
            if (inventory != null)
            {
                existOptions = String.IsNullOrEmpty(inventory.AdditionalOptions)
                    ? null
                    : (from data in
                        inventory.AdditionalOptions.Split(new[] {",", "|"}, StringSplitOptions.RemoveEmptyEntries)
                        select data.Trim()).ToList();

                existPackages = String.IsNullOrEmpty(inventory.AdditionalPackages)
                    ? null
                    : (from data in
                        inventory.AdditionalPackages.Split(new[] {",", "|"}, StringSplitOptions.RemoveEmptyEntries)
                        select data.Trim()).ToList();
            }

            if (Int32.TryParse(styleId, out id))
            {
                var autoService = new ChromeAutoService();

                var styleInfo = autoService.GetStyleInformationFromStyleId(id);

                list.Add(autoService.InitializeDoor(styleInfo));

                list.AddRange(autoService.InitializeExteriorColors(styleInfo.exteriorColor));

                list.AddRange(autoService.InitializeInteriorColors(styleInfo.interiorColor));

                list.AddRange(autoService.InitializeEngineStyles(styleInfo.engine));

                list.Add(autoService.InitializeBodyStyle(styleInfo));

                list.AddRange(existPackages != null
                    ? autoService.InitializePackages(styleInfo, existPackages)
                    : autoService.InitializePackages(styleInfo));

                list.AddRange(existOptions != null
                    ? autoService.InitializeAdditionalOptions(styleInfo, existOptions)
                    : autoService.InitializeAdditionalOptions(styleInfo));


                if (styleInfo.style.First().stockImage != null) list.Add(styleInfo.style.First().stockImage.url + "DefaultImage");

                list.Add(styleInfo.basePrice.msrp.high + "MSRP");

                if (styleInfo.genericEquipment != null) list.AddRange(GetDriveXmlFileFromVehicleInfo(styleInfo.style.First().drivetrain.ToString()));
            }

            return Json(list);
        }

        public JsonResult TruckCategoryAjaxPost(string truckType)
        {
            var list = SQLHelper.GetListOfTruckCategoryByTruckType(truckType).ToList();

            return Json(list);
        }

        private IEnumerable<string> GetDriveXmlFileFromVehicleInfo(string driveTrain)
        {
            var list = new List<string>();

            var driveNodeList = XMLHelper.selectElements("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"));

            var driveNodeSpecified = XMLHelper.selectOneElement("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"), "Name=" + driveTrain);

            if (driveNodeSpecified != null)
            {
                list.Add(driveNodeSpecified.Attributes["Value"].Value + "WheelDrive");

                foreach (XmlNode driveNode in driveNodeList)
                {

                    if (!driveNode.Attributes["Value"].Value.Equals(driveNodeSpecified.Attributes["Value"].Value))
                    {
                        list.Add(driveNode.Attributes["Value"].Value + "WheelDrive");

                    }
                }
                list.Add("Other DrivesWheelDrive");
            }
            else
            {
                foreach (XmlNode driveNode in driveNodeList)
                {
                    list.Add(driveNode.Attributes["Value"].Value + "WheelDrive");
                }
                list.Add("Other DrivesWheelDrive");
            }
            
            return list;
        }
        
        public JsonResult DistanceBetweenPlaces(double lat1, double lon1, double lat2, double lon2)
        {
            double dlon = convertToRadians(lon2 - lon1);
            double dlat = convertToRadians(lat2 - lat1);

            double a = (Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(convertToRadians(lat1))) * Math.Cos(convertToRadians(lat2)) * Math.Pow(Math.Sin(dlon / 2), 2);
            double angle = 2 * Math.Asin(Math.Sqrt(a));
            
            return Json(angle * Radio);
        }

        private double convertToRadians(double val)
        {
            return val * PIx / 180;
        }
        
        public JsonResult FlipDescription(string title, bool yesNo)
        {
            if (Session["Description"]!=null )
            {
                var sentenceGroups = (List<DescriptionSentenceGroup>) Session["Description"];

                var list = new List<string>();

                var searchResult =
                    sentenceGroups.FirstOrDefault(x => x.Title == title);

                var sentenceGroup =
                    searchResult.Sentences.Where(x => x.YesNo == yesNo).OrderBy(t => Guid.NewGuid()).ToList().Take(3);

                foreach (var tmp in sentenceGroup)
                {
                    list.Add(tmp.DescriptionSentence);
                }
                return Json(list);
            }

            return Json("SessionTimeOut");
        }
   

        public JsonResult GetTruckCategoriesByType(string type)
        {
            return Json(_commonManagementForm.GetTruckCategoriesByType(type), JsonRequestBehavior.AllowGet);
        }

        public bool HasKBBAuthorization()
        {
            try
            {
                return _kbbService.IsAuthorized(SessionHandler.Dealer.DealerSetting.KellyBlueBook, SessionHandler.Dealer.DealerSetting.KellyPassword);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
