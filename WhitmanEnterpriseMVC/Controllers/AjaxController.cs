using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Xml;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.Models;
using System.Collections;
using System.Text.RegularExpressions;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using WhitmanEnterpriseMVC.Security;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class AjaxController : SecurityController
    {
        const double PIx = 3.141592653589793;
        const double Radio = 3958.75587; // Mean radius of Earth in Miles
      
        public JsonResult YearAjaxPost(string modelYear)
        {
            var autoService = new ChromeAutoService();

            var makeList = autoService.GetDivisions(Convert.ToInt32(modelYear)).ToList();

            var addOnManufacturerNodeList = XMLHelper.selectElements("Make", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/AddManufactures.xml"));

            foreach (XmlNode node in addOnManufacturerNodeList)
            {
                var idtmp = Convert.ToInt32(node.Attributes["divisionId"].Value);

                if (makeList.All(x => x.id != idtmp))
                {
                    makeList.Add(new IdentifiedString()
                                     {
                                         id = idtmp,
                                         Value = node.Attributes["divisionName"].Value
                                     });
                }

            }

            return Json(makeList.OrderBy(x => x.Value));
        }

        public JsonResult MakeAjaxPost(string modelYear, string makeId)
        {
            var year = Convert.ToInt32(modelYear);
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

            var hash = new HashSet<string>();

            var list = new List<string>();

            try
            {
                if (styles != null && styles.Any())
                {
                    foreach (var s in styles)
                    {
                        if (String.IsNullOrEmpty(s.Value))
                        {
                            string uniqueTrim = "Base/Other Trims" + "Trim" + "StyleId*" + s.id;

                            if (!hash.Contains(uniqueTrim))
                            {
                                list.Add(uniqueTrim);
                            }

                            hash.Add(uniqueTrim);
                        }
                        else
                        {
                            string uniqueTrim = s.Value + "Trim" + "StyleId*" + s.id;

                            if (!hash.Contains(uniqueTrim))
                            {
                                list.Add(uniqueTrim);
                            }

                            hash.Add(uniqueTrim);
                        }
                    }

                    int styleId = styles.First().id;

                    var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);

                    var styleInfo = autoService.GetStyleInformationFromStyleId(styleId);

                    if (styleInfo != null)
                    {
                        var exteriorList = styleInfo.exteriorColor;

                        var interiorList = styleInfo.interiorColor;

                        var uniqueDoor = styleInfo.style.First().passDoors.ToString(CultureInfo.InvariantCulture) + "PassengerDoors";

                        list.Add(uniqueDoor);

                        if (exteriorList != null)
                        {
                            foreach (Color ec in exteriorList)
                            {
                                string uniqueColor = ec.colorName + "|" + ec.colorCode + "Exterior";
                                if (!String.IsNullOrEmpty(ec.colorName) && !hash.Contains(ec.colorName))
                                {
                                    list.Add(uniqueColor);
                                    hash.Add(ec.colorName);
                                }
                            }
                        }

                        if (interiorList != null)
                        {
                            foreach (Color ic in interiorList)
                            {
                                string uniqueColor = ic.colorName + "Interior";
                                if (!String.IsNullOrEmpty(ic.colorName) && !hash.Contains(ic.colorName))
                                {
                                    list.Add(uniqueColor);
                                    hash.Add(ic.colorName);
                                }
                            }
                        }

                        if (styleInfo.engine != null)
                        {
                            foreach (Engine er in styleInfo.engine)
                            {
                                string fuelType = er.fuelType.Value;
                                int index = fuelType.LastIndexOf(" ", System.StringComparison.Ordinal);
                                string uniqueCylinder = er.cylinders + "Cylinder";
                                string uniqueFuel = fuelType.Substring(0, index) + "Fuel";
                                string uniqueLitter = er.displacement.liters + "Litter";
                                if (!list.Contains(uniqueCylinder)) list.Add(uniqueCylinder);
                                if (!list.Contains(uniqueFuel)) list.Add(uniqueFuel);
                                if (!list.Contains(uniqueLitter)) list.Add(uniqueLitter);

                            }
                        }

                        if (styleInfo.vinDescription != null && !String.IsNullOrEmpty(styleInfo.vinDescription.bodyType))
                        {
                            string uniqueBodyType = styleInfo.vinDescription.bodyType + "BodyType";
                            list.Add(uniqueBodyType);
                        }
                        else
                        {
                            try
                            {
                                string uniqueBodyType = styleInfo.style.Last().bodyType.Last().Value + "BodyType";
                                list.Add(uniqueBodyType);
                            }
                            catch (Exception)
                            {
                                string uniqueBodyType = styleInfo.bestStyleName + "BodyType";
                                list.Add(uniqueBodyType);
                            }                            
                        }

                        if (styleInfo.factoryOption != null && styleInfo.factoryOption.Any())
                        {
                            foreach (var fo in styleInfo.factoryOption)
                            {
                                var optionsName = fo.description.First();

                                if (!hash.Contains(optionsName) && fo.price.msrpMax > 0 && !optionsName.Contains("PKG") && !optionsName.Contains("PACKAGE"))
                                {
                                    string description = fo.description.Any() ? fo.description[0] : String.Empty;
                                    string uniqueOption = CommonHelper.UpperFirstLetterOfEachWord(optionsName.Replace(",","")) + " " + fo.price.msrpMax.ToString("C") + "*" + description + "Optional";
                                    list.Add(uniqueOption);
                                    hash.Add(optionsName);
                                }

                                if (!hash.Contains(optionsName) && fo.price.msrpMax > 0 && (optionsName.Contains("PKG") || optionsName.Contains("PACKAGE")))
                                {
                                    string description = fo.description.Count() >= 2
                                                             ? fo.description[1]
                                                             : fo.description[0];
                                    string uniquePackage = CommonHelper.UpperFirstLetterOfEachWord(optionsName.Replace(",", "")) + " " + fo.price.msrpMax.ToString("C") + "*" + description + "Package";
                                    list.Add(uniquePackage);
                                    hash.Add(optionsName);
                                }
                            }
                        }

                        // Get addtional options from generic equipment
                        if (styleInfo.genericEquipment != null && styleInfo.genericEquipment.Any())
                        {
                            foreach (var ge in styleInfo.genericEquipment)
                            {
                                var category = ((CategoryDefinition)(ge.Item)).category.Value;
                                if (!hash.Contains(category) && ge.installed == null)
                                {
                                    string uniqueOption = CommonHelper.UpperFirstLetterOfEachWord(category) + " " + "$0" + "*" + category + "Optional";

                                    list.Add(uniqueOption);
                                    hash.Add(category);
                                }
                            }
                        }

                        if (styleInfo.style.First().stockImage != null)
                            list.Add(styleInfo.style.First().stockImage.url + "DefaultImage");

                        list.Add(styleInfo.basePrice.msrp.high.ToString() + "MSRP");

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
        
        public JsonResult StyleAjaxPost(string styleId)
        {
            int id ;

            var list = new List<string>();

            if (Int32.TryParse(styleId, out id))
            {
                var hash = new HashSet<string>();

                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);

                var autoService = new ChromeAutoService();

                var styleInfo = autoService.GetStyleInformationFromStyleId(id);

                var exteriorList = styleInfo.exteriorColor;

                var interiorList = styleInfo.interiorColor;

                string uniqueDoor = styleInfo.style.First().passDoors.ToString(CultureInfo.InvariantCulture) + "PassengerDoors";

                list.Add(uniqueDoor);
               
                if (exteriorList != null)
                {
                    foreach (Color ec in exteriorList)
                    {
                        string uniqueColor = ec.colorName + "|" + ec.colorCode + "Exterior";
                        if (!String.IsNullOrEmpty(ec.colorName) && !hash.Contains(ec.colorName))
                        {
                            list.Add(uniqueColor);
                            hash.Add(ec.colorName);
                        }
                    }
                }

                if (interiorList != null)
                {
                    foreach (Color ic in interiorList)
                    {
                        string uniqueColor = ic.colorName + "Interior";
                        if (!String.IsNullOrEmpty(ic.colorName) && !hash.Contains(ic.colorName))
                        {
                            list.Add(uniqueColor);
                            hash.Add(ic.colorName);
                        }
                    }
                }

                if (styleInfo.engine != null)
                {
                    foreach (Engine er in styleInfo.engine)
                    {
                        string fuelType = er.fuelType.Value;
                        int index = fuelType.LastIndexOf(" ", System.StringComparison.Ordinal);
                        string uniqueCylinder = er.cylinders + "Cylinder";
                        string uniqueFuel = fuelType.Substring(0, index) + "Fuel";
                        string uniqueLitter = er.displacement.liters + "Litter";
                        if (!list.Contains(uniqueCylinder)) list.Add(uniqueCylinder);
                        if (!list.Contains(uniqueFuel)) list.Add(uniqueFuel);
                        if (!list.Contains(uniqueLitter)) list.Add(uniqueLitter);
                    }
                }

                if (styleInfo.vinDescription != null && !String.IsNullOrEmpty(styleInfo.vinDescription.bodyType))
                {
                    string uniqueBodyType = styleInfo.vinDescription.bodyType + "BodyType";
                    list.Add(uniqueBodyType);
                }
                else
                {
                    try
                    {
                        string uniqueBodyType = styleInfo.style.Last().bodyType.Last().Value + "BodyType";
                        list.Add(uniqueBodyType);
                    }
                    catch (Exception)
                    {
                        string uniqueBodyType = styleInfo.bestStyleName + "BodyType";
                        list.Add(uniqueBodyType);
                    }
                }

                if (styleInfo.factoryOption != null && styleInfo.factoryOption.Any())
                {
                    foreach (var fo in styleInfo.factoryOption)
                    {
                        var optionsName = fo.description.First();

                        if (!hash.Contains(optionsName) && fo.price.msrpMax > 0 && !optionsName.Contains("PKG") && !optionsName.Contains("PACKAGE"))
                        {
                            string description = fo.description.Any() ? fo.description[0] : String.Empty;
                            string uniqueOption = CommonHelper.UpperFirstLetterOfEachWord(optionsName.Replace(",", "")) + " " + fo.price.msrpMax.ToString("C") + "*" + description + "Optional";
                            list.Add(uniqueOption);
                            hash.Add(optionsName);
                        }

                        if (!hash.Contains(optionsName) && fo.price.msrpMax > 0 && (optionsName.Contains("PKG") || optionsName.Contains("PACKAGE")))
                        {
                            string description = fo.description.Count() >= 2 ? fo.description[1] : fo.description[0];
                            string uniquePackage = CommonHelper.UpperFirstLetterOfEachWord(optionsName.Replace(",", "")) + " " + fo.price.msrpMax.ToString("C") + "*" + description + "Package";
                            list.Add(uniquePackage);
                            hash.Add(optionsName);
                        }
                    }
                }

                // Get addtional options from generic equipment
                if (styleInfo.genericEquipment != null && styleInfo.genericEquipment.Any())
                {
                    foreach (var ge in styleInfo.genericEquipment)
                    {
                        var category = ((CategoryDefinition)(ge.Item)).category.Value;
                        if (!hash.Contains(category) && ge.installed == null)
                        {
                            string uniqueOption = CommonHelper.UpperFirstLetterOfEachWord(category) + " " + "$0" + "*" + category + "Optional";

                            list.Add(uniqueOption);
                            hash.Add(category);
                        }
                    }
                }

                if (styleInfo.style.First().stockImage != null)
                    list.Add(styleInfo.style.First().stockImage.url + "DefaultImage");

                list.Add(styleInfo.basePrice.msrp.high + "MSRP");


                if (styleInfo.genericEquipment != null)
                {
                    list.AddRange(GetDriveXmlFileFromVehicleInfo(styleInfo.style.First().drivetrain.ToString()));
                }
            }

            return Json(list);
        }

        public JsonResult TruckCategoryAjaxPost(string truckType)
        {

            var list = new List<string>();
            
            foreach (var tmp in SQLHelper.GetListOfTruckCategoryByTruckType(truckType))
            {
                list.Add(tmp);
            }

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

            }
            else
            {
                foreach (XmlNode driveNode in driveNodeList)
                {
                    list.Add(driveNode.Attributes["Value"].Value + "WheelDrive");
                }
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
        
        public JsonResult GetCarFaxReport(int appraisalId)
        {
            var carfax = new CarFaxViewModel()
                             {
                                 Success = false
                             };

            if (Session["Dealership"] == null)
            {
                return new DataContractJsonResult(carfax);
            }

            var contextVinControl = new whitmanenterprisewarehouseEntities();

            var targetCar =
                contextVinControl.whitmanenterpriseappraisals.First(
                    (x => x.idAppraisal == appraisalId));

            var dealer = (DealershipViewModel)Session["Dealership"];

            carfax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(targetCar.VINNumber, dealer.CarFax,
                                                                 dealer.CarFaxPassword);

            return new DataContractJsonResult(carfax);

        }
    }
}
