using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.HTMLExtension;
using Vincontrol.Web.Models;
using Color = vincontrol.ChromeAutoService.AutomativeService.Color;
using System.IO.Compression;

namespace System.Web.Mvc.Html
{
    public static class HTMLControlExtension
    {
        public static string DatePicker(this HtmlHelper htmlHelper, string name)
        {
            return "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " runat=\"server\"" + " />";
        }
        public static string SubmitButton(this HtmlHelper helper, string text)
        {
            return "<input id=\"LoginButton\" type=\"submit\" class=\"menuButton\" value=\"" + text + "\" />";
        }
        public static bool CanSeeButton(CarInfoFormViewModel model, string buttonName)
        {
            //var group = model.ButtonPermissions.FirstOrDefault(i => i.GroupId.Equals(SessionHandler.CurrentUser.RoleId));
            //if (SessionHandler.CurrentUser.RoleId.Equals(Constanst.RoleType.Admin)
            //    || SessionHandler.CurrentUser.RoleId.Equals(Constanst.RoleType.Master)
            //    || !SQLHelper.CheckDealershipButtonGroupExist(SessionHandler.Dealer.DealershipId, SessionHandler.CurrentUser.RoleId)
            //    || (group != null && group.Buttons.First(i => i.ButtonName.ToLower().Equals(buttonName.ToLower())).CanSee))
            //    return true;
            //return false;
            if (SessionHandler.CurrentUser.RoleId.Equals(Constanst.RoleType.Admin)
               || SessionHandler.CurrentUser.RoleId.Equals(Constanst.RoleType.Master)
                || (SessionHandler.CurrentUser.ProfileButtonPermissions != null && SessionHandler.CurrentUser.ProfileButtonPermissions.Buttons.First(i => i.ButtonName.ToLower().Equals(buttonName.ToLower())).CanSee))
                return true;
            return false;
        }
        public static bool CanSeeButton(string buttonName)
        {
            if (SessionHandler.CurrentUser.RoleId.Equals(Constanst.RoleType.Admin)
                || SessionHandler.CurrentUser.RoleId.Equals(Constanst.RoleType.Master)
                || !SQLHelper.CheckDealershipButtonGroupExist(SessionHandler.Dealer.DealershipId, SessionHandler.CurrentUser.RoleId)
                || (SessionHandler.CurrentUser.ProfileButtonPermissions != null && SessionHandler.CurrentUser.ProfileButtonPermissions.Buttons.First(i => i.ButtonName.ToLower().Equals(buttonName.ToLower())).CanSee))
                return true;
            return false;
        }
        public static string DynamicHtmlControlAppraisal(this HtmlHelper<AppraisalViewFormModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";

            switch (fieldName)
            {
                case "Date":
                    result = "<input class=\"z-index\" type=\"text\" disabled=\"disabled\" id=\"" + name + "\" name=\"" + name + "\" value=\"" +
                             DateTime.Now.ToShortDateString() + "\" />";
                    break;
                default:
                    result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                    break;
            }
            return result;
        }
        public static string DynamicHtmlControl(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";

            if (model.Success)
            {
                switch (fieldName)
                {
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.Stock))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Stock + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;
                    case "ExteriorColor":


                        result = "<select class=\"z-index\" style=\"width:70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            foreach (var ec in model.ExteriorColorList)
                            {
                                result += "<option>" + ec.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;
                    case "InteriorColor":


                        result = "<select class=\"z-index\" style=\"width:70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            foreach (var ic in model.InteriorColorList)
                            {
                                result += "<option>" + ic.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;

                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;



                    case "Odometer":
                        if ((model.Mileage == 0))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litter + "\" />";
                        break;
                    case "Tranmission":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Msrp + "\" />";
                        break;
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\" name=\"Packages\">";

                            result += "<ul class=\"options\">";

                            foreach (var fo in model.FactoryPackageOptions)
                            {
                                var newString = regex.Replace(fo.Name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.Msrp+ "</li>";
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\" >";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages available for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\" name=\"Options\">";
                        result += "<ul class=\"options\">";
                        if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                var newString = regex.Replace(fo.Name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.Msrp + "</li>";
                            }
                        }

                        result += "</ul>";
                        result += "</div>";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;
                    case "JavaScript":
                        //result = generateGraphJavaScript(model);
                        break;
                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;

                }
            }
            else
            {
                string defaultName = "Select a make";
                string defaultId = "0";
                switch (fieldName)
                {
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + DateTime.Now.ToShortDateString() + " />";
                        break;
                    case "Stock":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + model.ModelYear + " />";
                        break;
                    case "Make":
                        result += "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "<option value=\"" + defaultId + "****" + defaultName + "\">" + defaultName + "</option>";
                        if (model.MakeNameList != null)
                        {
                            foreach (var dv in model.MakeNameList)
                            {
                                result += "<option value=\"" + dv.id + "****" + dv.Value + "\">" + dv.Value + "</option>";
                            }
                        }
                        result += "</select>";


                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Model":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "Trim":
                        result = "<select class=\"z-index\"  id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "ExteriorColor":
                        result = "<select class=\"z-index\"  style=\"width: 70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "InteriorColor":
                        result = "<select class=\"z-index\"  style=\"width: 70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 40px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 40px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;


                    case "Odometer":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Cylinders":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break; ;
                    case "Litters":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "Tranmission":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "Type":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "BodyType":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "FuelType":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        //result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        //result += "</select>";

                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        break;
                    case "FactoryPackageOption":
                        result += "<div id=\"Packages\" name=\"Packages\">";
                        for (int i = 0; i < 10; i++)
                        {
                            result += "<li>";
                            result += "<input type=\"checkbox\" class=\"z-index hider\" onclick=\"javascript:changeMSRP(this)\"   name=\"selectedOptions\" price=\"PackagePrice\" value=\"Package\"  />";
                            result += "<label class=\"z-index hider\"  for=\"" + name + "\">" + "</label>";
                            result += "</li>";
                        }
                        result += "</div>";
                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\" name=\"Options\">";
                        for (int i = 0; i < 40; i++)
                        {
                            result += "<li>";
                            result += "<input type=\"checkbox\" class=\"z-index hider\" onclick=\"javascript:changeMSRP(this)\"  name=\"selectedOptions\" price=\"OptionPrice\" value=\"Options\" />";
                            result += "<label class=\"z-index hider\"  for=\"" + name + "\">" + "</label>";
                            result += "</li>";


                        }
                        result += "</div>";
                        break;

                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarImageUrl + "\"" + " />"; ;
                        break;
                    case "HiddenPhotoProfile":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                        break;

                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;

                    default:
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"error\"" + " />";

                        break;

                }
            }
            return result;
        }
        public static string DynamicHtmlControlForProfile(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";

            if (model.Success)
            {
                switch (fieldName)
                {
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.Stock))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Stock + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;
                    case "ExteriorColor":
                        var ExteriorColorList = model.ExteriorColorList;
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        foreach (var ec in ExteriorColorList)
                        {
                            result += "<option value=\"" + ec.colorName + "\">" + ec.colorName + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "InteriorColor":
                        var InteriorColorList = model.InteriorColorList;
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        foreach (var ic in InteriorColorList)
                        {
                            result += "<option value=\"" + ic.colorName + "\">" + ic.colorName + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusExteriorColor + "\" />";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusInteriorColor + "\" />";

                        break;
                    case "Odometer":
                        if (model.Mileage == 0)
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litter + "\" />";
                        break;
                    case "Tranmission":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;

                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Msrp + "\" />";
                        break;
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\" name=\"Packages\">";

                            result += "<ul class=\"options\">";

                            foreach (var fo in model.FactoryPackageOptions)
                            {
                                var newString = regex.Replace(fo.Name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.Msrp + "</li>";
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages available for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\" name=\"Options\">";
                        result += "<ul class=\"options\">";
                        if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                var newString = regex.Replace(fo.Name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.Msrp+ "</li>";
                            }
                        }

                        result += "</ul>";
                        result += "</div>";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "HiddenEngineType":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Engine + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;

                    case "HiddenTrim":


                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + "\" />";

                        break;
                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;

                }
            }
            else
            {

                switch (fieldName)
                {

                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + DateTime.Now.ToShortDateString() + " />";
                        break;
                    case "Stock":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + model.ModelYear + " />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + model.Make + " />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Model":
                        var modelList = model.ModelList;

                        if (modelList != null && modelList.Count > 0)
                        {

                            result += "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";


                            foreach (var md in modelList)
                            {
                                result += "<option value=\"" + md.id + "****" + md.Value + "\">" + md.Value + "</option>";
                            }
                            result += "</select>";
                        }
                        else
                        {
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        }

                        break;
                    case "Trim":
                        var styleList = model.StyleList;

                        if (styleList != null && styleList.Count > 0)
                        {
                            result += "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                            foreach (var st in styleList)
                            {
                                result += "<option value=\"" + st.id + "****" + st.trim + "\">" + st.trim + "</option>";
                            }
                            result += "</select>";
                        }
                        else
                        {
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        }

                        break;
                    case "HiddenEngineType":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Engine + "\" />";
                        break;
                    case "HiddenTrim":

                        modelList = model.ModelList;
                        if (modelList != null && modelList.Count > 0)
                        {
                            result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + modelList.First().id + "\" />";
                        }
                        break;
                    case "ExteriorColor":
                        var ExteriorColorList = model.ExteriorColorList;
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (ExteriorColorList != null && ExteriorColorList.Count > 0)
                        {
                            foreach (var ec in ExteriorColorList)
                            {
                                result += "<option value=\"" + ec.colorName + "\">" + ec.colorName + "</option>";
                            }
                        }
                        else
                        {
                            result += "<option value=\"" + model.ExteriorColor + "\">" + model.ExteriorColor + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "InteriorColor":
                        var InteriorColorList = model.InteriorColorList;
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (InteriorColorList != null && InteriorColorList.Count > 0)
                        {
                            foreach (var ic in InteriorColorList)
                            {
                                result += "<option value=\"" + ic.colorName + "\">" + ic.colorName + "</option>";
                            }
                        }
                        else
                        {
                            result += "<option value=\"" + model.InteriorColor + "\">" + model.InteriorColor + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusExteriorColor + "\" />";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusInteriorColor + "\" />";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;


                    case "Odometer":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litter + "\" />";
                        break;
                    case "Tranmission":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Type":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Msrp + "\" />";
                        break;
                    case "FactoryPackageOption":
                        result += "<div id=\"Packages\">";

                        result += "</div>";
                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\">";
                        result += "</div>";
                        break;

                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />"; ;
                        break;
                    case "HiddenPhotoProfile":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                        break;

                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;

                    default:
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"error\"" + " />";

                        break;

                }
            }
            return result;
        }
        public static string DynamicHtmlLabel(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            //var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string result = "";
            switch (fieldName)
            {
                case "MarketRangeEditProfile":
                    if (SessionHandler.CurrentView == CurrentViewEnum.SoldInventory.ToString())
                    {
                        result = "<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = model.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/question.gif") +
                                               "\" style=\"height: 30px; width: 25px;\" /></a>";
                    }
                    else
                    {

                        if (model.MarketRange == 3)
                            result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") +
                                           "\" style=\"height: 30px; width: 25px;\" /></a>";
                        else if (model.MarketRange == 2)
                            result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") +
                                           "\" style=\"height: 30px; width: 25px;\" /></a>";

                        else if (model.MarketRange == 1)
                            result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") +
                                           "\" style=\"height: 30px; width: 25px;\" /></a>";

                        else
                            result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/question.gif") +
                                               "\" style=\"height: 30px; width: 25px;\" /></a>";
                    }

                    break;

                case "Vin":
                    result = "Vin: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.Vin + "</label>";
                    break;
                case "Date":
                    result = "<label  for=\"" + name + "\">" + DateTime.Now.ToShortDateString() + "</label>";
                    break;
                case "Stock":
                    result = "Stock: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.Stock + "</label>";
                    break;
                case "AppraisalID":
                    result = "Appraisal: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.AppraisalGenerateId + "</label>";
                    break;
                case "Year":
                    result = "Year: <label  for=\"" + name + "\">" + model.ModelYear + "</label>";
                    break;
                case "Make":
                    result = "Make: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.Make + "</label>";
                    break;
                case "Model":
                    result = "Model: <label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Model, 15) + "</label>";
                    break;
                case "Trim":
                    result = "Trim: <label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Trim, 15) + "</label>";
                    break;
                case "ExteriorColor":
                    if (String.IsNullOrEmpty(model.CusExteriorColor))
                        result = "Exterior: <label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.ExteriorColor, 15) + "</label>";
                    else
                        result = "Exterior: <label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusExteriorColor, 15) + "</label>";
                    break;
                case "InteriorColor":
                    if (String.IsNullOrEmpty(model.CusInteriorColor))
                        result = "Interior: <label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.InteriorColor, 15) + "</label>";
                    else
                        result = "Interior: <label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusInteriorColor, 15) + "</label>";

                    break;
                case "DaysInInventory":
                    result = "Age: <label id=\"" + name + "\" for=\"" + name + "\">" + model.DaysInInvenotry + " day(s) </label>";
                    break;
                case "Odometer":
                    result = "Odometer: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Mileage.ToString("C0").Replace("$", "") + "</label>";
                    break;
                case "Cylinders":
                    result = "Cylinders: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Cylinder + "</label>";
                    break;
                case "Litters":
                    result = "Liters/CC: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Litter + "</label>";
                    break;
                case "Tranmission":
                    result = "Transmission: <label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Tranmission, 10) + "</label>";
                    break;
                case "Doors":
                    result = "Doors: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Door + "</label>";

                    break;

                case "BodyType":
                    result = "BodyType: <label id=\"" + name + "\" for=\"" + name + "\">" + model.BodyType + "</label>";
                    break;
                case "FuelType":
                    result = "Fuel: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Fuel + "</label>";
                    break;
                case "DriveType":
                    result = "Drive Type: <label id=\"" + name + "\" for=\"" + name + "\">" + GetShortDrive(model.WheelDrive) + "</label>";
                    break;
                case "MSRP":
                    result = "MSRP: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Msrp + "</label>";
                    break;
                case "Style":
                    result = "Style: <label id=\"" + name + "\" for=\"" + name + "\">" + model.BodyType + "</label>";
                    break;
                case "SalePrice":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.SalePrice + "</label>";

                    break;
                case "DealerCost":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.DealerCost + "</label>";

                    break;
                case "ACV":
                    result = "<label  id=\"" + name + "\">" + model.Acv + "</label>";

                    break;
                case "TruckType":
                    result = "Truck Type: <label  id=\"" + name + "\">" + model.SelectedTruckType + "</label>";

                    break;
                case "TruckClass":
                    result = "Truck Class: <label  id=\"" + name + "\">" + model.SelectedTruckClassId + "</label>";

                    break;
                case "TruckCategory":
                    result = "Truck Category: <label  id=\"" + name + "\">" + CommonHelper.TrimString(model.TruckCategory, 10) + "</label>";

                    break;
                case "Title":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.OrginalName + "</label>";
                    break;
                case "TitleWithoutTrim":
                    string title = model.ModelYear + " " + model.Make + " " + model.Model;
                    if (title.Length > 21)
                    {
                        if (title.Length < 30)
                            result = "<label style=\"font-size: .8em\" id=\"" + name + "\" for=\"" + name + "\">" +
                                     title + "</label>";
                        else
                        {
                            result = "<label style=\"font-size: .8em\" id=\"" + name + "\" for=\"" + name + "\">" +
                                title.Substring(0, 30) + "</label>";
                        }
                    }
                    else
                        result = "<label id=\"" + name + "\" for=\"" + name + "\">" + title + "</label>";
                    break;
                case "MarketPriceLowest":
                    result = "<label for=\"" + name + "\">" + model.LowestPrice + "</label>";
                    break;
                case "MarketPriceAverage":
                    result = "<label for=\"" + name + "\">" + model.AveragePriceOnMarket + "</label>";
                    break;
                case "MarketPriceHighest":
                    result = "<label for=\"" + name + "\">" + model.HighestPrice + "</label>";
                    break;


                case "CarImage":
                    if (!String.IsNullOrEmpty(model.SinglePhoto))
                        result = "<img id=\"pIdImage\" name=\"pIdImage\" src=\"" + model.SinglePhoto + "\"" +
                                 " width=\"160\" style=\"max-height: 240px;\" />" + Environment.NewLine;
                    else
                    {
                        model.SinglePhoto = "http://vincontrol.com/alpha/no-images.jpg";
                        result = "<img id=\"pIdImage\" name=\"pIdImage\" src=\"" + model.SinglePhoto + "\"" +
                                 " width=\"160\" style=\"max-height: 240px;\" />" + Environment.NewLine;
                    }
                    result += "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";

                    break;
                case "HiddenPhotoProfile":
                    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                    break;
               
                case "SmallCarImage":
                    if (model.MutiplePhotos != null)
                    {
                        if (model.MutiplePhotos.Count != 0)
                        {
                            foreach (string url in model.MutiplePhotos)
                                result += "<li class=\"float\"><img src=\"" + url + "\"" + " /></li>";
                        }
                    }

                    break;
                case "GoogleMap":

                    result = "<img id=\"googleMap\" src=\"http://maps.googleapis.com/maps/api/staticmap?center=" + model.Latitude + "," + model.Longtitude + "&markers=color:red%7C" + model.Latitude + "," + model.Longtitude + "&zoom=14&size=320x320&sensor=false" + "\"/>" + Environment.NewLine;
                    break;

                case "Description":
                    if (!String.IsNullOrEmpty(model.Description))
                    {
                        if (model.Description.Length > 150)
                            result = "<label id=\"" + name + "\" for=\"" + name + "\">Description: " + CommonHelper.TrimString(model.Description, 150) + "..." + "</label>";

                        else
                            result = "<label id=\"" + name + "\" for=\"" + name + "\">Description: " + CommonHelper.TrimString(model.Description) + "</label>";

                    }
                    else
                        result = "<label id=\"" + name + "\" for=\"" + name + "\">Description: </label>";
                    break;



                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }
        public static string DynamicHtmlLabel(this HtmlHelper<AppraisalViewFormModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";
            switch (fieldName)
            {
                case "TitleWithoutTrim":
                    string title = model.ModelYear + " " + model.Make + " " + model.SelectedModel;
                    if (title.Length > 21)
                        result = "<label style=\"font-size: .8em\" id=\"" + name + "\" for=\"" + name + "\">" + title + "</label>";
                    else
                        result = "<label id=\"" + name + "\" for=\"" + name + "\">" + title + "</label>";
                    break;
                case "Vin":
                    result = "Vin: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.VinNumber + "</label>";
                    break;
                case "Date":
                    result = "<label  for=\"" + name + "\">" + DateTime.Now.ToShortDateString() + "</label>";
                    break;
                case "Stock":
                    result = "Stock: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.StockNumber + "</label>";
                    break;
                case "AppraisalID":
                    result = "Appraisal: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.AppraisalGenerateId + "</label>";
                    break;
                case "Year":
                    result = "Year: <label  for=\"" + name + "\">" + model.ModelYear + "</label>";
                    break;
                case "Make":
                    result = "Make: <label  id=\"" + name + "\"  for=\"" + name + "\">" + model.Make + "</label>";
                    break;
                case "Model":
                    result = "Model: <label title=\"" + model.AppraisalModel + "\"  id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.AppraisalModel, 15) + "</label>";
                    break;
                case "Trim":
                    result = "Trim: <label title=\"" + model.SelectedTrim + "\" id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedTrim, 15) + "</label>";
                    break;

                case "ExteriorColor":
                    if (String.IsNullOrEmpty(model.CusExteriorColor))
                        result = "Exterior: <label title=\"" + model.SelectedExteriorColorValue + "\"  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedExteriorColorValue, 15) + "</label>";
                    else
                        result = "Exterior: <label title=\"" + model.CusExteriorColor + "\"  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusExteriorColor, 15) + "</label>";


                    //result = "Exterior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.trimString(model.CusExteriorColor==null?String.Empty:model.CusExteriorColor, 15) + "</label>";
                    break;
                case "InteriorColor":
                    if (String.IsNullOrEmpty(model.CusInteriorColor))
                        result = "Interior: <label title=\"" + model.SelectedInteriorColor + "\"  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedInteriorColor, 15) + "</label>";
                    else
                        result = "Interior: <label title=\"" + model.CusInteriorColor + "\"  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusInteriorColor, 15) + "</label>";

                    break;
                case "TruckType":
                    result = "Truck Type: <label  id=\"" + name + "\">" + model.TruckType + "</label>";

                    break;
                case "TruckClass":
                    result = "Truck Class: <label  id=\"" + name + "\">" + model.TruckClass + "</label>";

                    break;
                case "TruckCategory":
                    result = "Truck Category: <label title=\"" + model.SelectedTruckCategoryId + "\"  id=\"" + name + "\">" + CommonHelper.TrimString(model.TruckCategory, 10) + "</label>";
                    break;
                case "Odometer":
                    result = "Odometer: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Mileage.ToString("C0").Replace("$", "") + "</label>";
                    break;
                case "Cylinders":
                    result = "Cylinders: <label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedCylinder + "</label>";
                    break;
                case "Litters":
                    result = "Liters/CC: <label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedLiters + "</label>";
                    break;
                case "Tranmission":
                    result = "Transmission: <label title=\"" + model.SelectedTranmission + "\" id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedTranmission, 10) + "</label>";
                    break;
                case "Doors":
                    result = "Doors: <label id=\"" + name + "\" for=\"" + name + "\">" + model.Door + "</label>";

                    break;

                case "BodyType":
                    result = "BodyType: <label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedBodyType + "</label>";
                    break;
                case "FuelType":
                    result = "Fuel: <label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedFuel + "</label>";
                    break;
                case "DriveType":
                    result = "Drive Type: <label title=\"" + model.SelectedDriveTrain + "\" id=\"" + name + "\" for=\"" + name + "\">" + GetShortDrive(model.SelectedDriveTrain) + "</label>";
                    break;
                case "MSRP":
                    result = "MSRP: <label id=\"" + name + "\" for=\"" + name + "\">" + model.MSRP + "</label>";
                    break;
                case "Style":
                    result = "Style: <label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedBodyType + "</label>";
                    break;
                case "SalePrice":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.SalePrice + "</label>";

                    break;
                case "DealerCost":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.DealerCost + "</label>";

                    break;
                case "ACV":
                    result = "<label  for=\"" + name + "\">" + model.ACV + "</label>";

                    break;

                case "Title":
                    result = "<label style=\"font-size: .9em\" id=\"" + name + "\" for=\"" + name + "\">" + model.Title + "</label>";
                    break;
                case "MarketPriceLowest":
                    result = "<label for=\"" + name + "\">" + model.LowestPrice + "</label>";
                    break;

                case "MarketPriceHighest":
                    result = "<label for=\"" + name + "\">" + model.HighestPrice + "</label>";
                    break;

                case "CarImage":
                   
                    result = "<img id=\"pIdImage\" name=\"pIdImage\" src=\"" + model.FirstPhoto + "\"" + " width=\"160\" style=\"max-height: 240px;\" />" + Environment.NewLine;
                 
                    break;
                case "HiddenPhotoProfile":
                    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                    break;
                case "Description":
                    if (!String.IsNullOrEmpty(model.Descriptions))
                    {
                        if (model.Descriptions.Length > 150)
                            result = "<label title=\"" + model.Descriptions + "\" id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Descriptions, 150) + "..." + "</label>";

                        else
                            result = "<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Descriptions) + "</label>";

                    }
                    else
                    {
                        result = "<label id=\"" + name + "\" for=\"" + name + "\"></label>";
                    }
                    break;
                case "Notes":
                    if (!String.IsNullOrEmpty(model.Notes))
                    {
                        if (model.Notes.Length > 150)
                            result = "Notes: <label title=\"" + model.Notes + "\" id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Notes, 150) + "..." + "</label>";

                        else
                            result = "Notes: <label title=\"" + model.Notes + "\" id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Notes) + "</label>";

                    }
                    else
                    {
                        result = "Notes: <label id=\"" + name + "\" for=\"" + name + "\"></label>";
                    }
                    break;
                case "Location":
                    result = "Location:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Location + "</label>";

                    break;
                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }
        public static string DynamicHtmlLabelAppraisal(this HtmlHelper<AppraisalListViewModel> htmlHelper, string fieldName)
        {

            var model = htmlHelper.ViewData.Model;

            var builder = new StringBuilder();

            switch (fieldName)
            {

                case "txtPendingAppraisalGridMiddle":
                    builder.Append(BuildAppraisal(htmlHelper, model.UnlimitedAppraisals));
                    break;
                case "txtAppraisalGridMiddle":
                    builder.Append(BuildAppraisal(htmlHelper, model.UnlimitedAppraisals));

                    break;
                case "txtAppraisalGridSideBar":

                    foreach (AppraisalViewFormModel car in model.UnlimitedAppraisals)
                    {
                        builder.AppendLine("<font size=\"2\">");
                        builder.Append("<li>" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", car.CarImagesUrl, "", new { AppraisalId = car.AppraisalID }, null, new { @class = "mThumb" }) + Environment.NewLine);
                        builder.Append(" <ul class=\"info\">" + Environment.NewLine);
                        builder.Append("<li><span class=\"item_title\">" + car.ModelYear + " " + CommonHelper.TrimString(car.Make, 10) + "<br />" + Environment.NewLine);
                        builder.Append(car.AppraisalModel + "</span></a></li>" + Environment.NewLine);
                        builder.Append("<li><span class=\"value\">" + car.ACV + "</span> <span class=\"date\">" + car.AppraisalDate + "</span></li>" + Environment.NewLine);
                        builder.Append("</ul>" + Environment.NewLine);
                        builder.Append("</li>" + Environment.NewLine);
                        builder.Append("</font>" + Environment.NewLine);

                    }
                    break;

                case "txtPendingAppraisalGridSideBar":

                    foreach (AppraisalViewFormModel car in model.UnlimitedAppraisals)
                    {
                        builder.AppendLine("<font size=\"2\">");
                        builder.Append("<li>" + Environment.NewLine);

                        if (car.IsPhotoFromVingenie)

                            builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", /*"/Appraisal/RenderPhoto?appraisalId=" + car.AppraisalID*/car.PhotoUrl, "", new { AppraisalId = car.AppraisalID }, null, new { @class = "mThumb" }) + Environment.NewLine);
                        else
                        {
                            builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", car.DefaultImageUrl, "", new { AppraisalId = car.AppraisalID }, null, new { @class = "mThumb" }) + Environment.NewLine);
                        }
                        builder.Append(" <ul class=\"info\">" + Environment.NewLine);
                        builder.Append("<li><span class=\"item_title\">" + car.ModelYear + " " + CommonHelper.TrimString(car.Make, 10) + "<br />" + Environment.NewLine);
                        builder.Append(car.AppraisalModel + "</span></a></li>" + Environment.NewLine);
                        builder.Append("<li><span class=\"value\">" + car.ACV + "</span> <span class=\"date\">" + car.AppraisalDate + "</span></li>" + Environment.NewLine);
                        builder.Append("</ul>" + Environment.NewLine);
                        builder.Append("</li>" + Environment.NewLine);
                        builder.Append("</font>" + Environment.NewLine);

                    }
                    break;

                case "SortOption":
                    builder.Append("<select name=\"sortBy\" id=\"sortBy\" onchange=\"javascript:submitFormSort(this);\" > " + Environment.NewLine);

                    builder.Append("<option> Select an option...</option>" + Environment.NewLine);

                    builder.Append("<option>Year</option>" + Environment.NewLine);

                    builder.Append("<option>Make</option>" + Environment.NewLine);

                    builder.Append("<option>Model</option>" + Environment.NewLine);

                    builder.Append("<option>Age</option>" + Environment.NewLine);

                    builder.Append("</select>" + Environment.NewLine);
                    break;


                default:
                    builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + "\" value=\"error\"" + " />");

                    break;



            }



            return builder.ToString();
        }
        public static string DynamicHtmlLabelForKPI(this HtmlHelper<InventoryFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            string result = "";

            var carsList = model.CarsList;

            var queryAbove = carsList.Where(tmp => tmp.MarketRange == 3);

            var queryAve = carsList.Where(tmp => tmp.MarketRange == 2);

            var queryBelow = carsList.Where(tmp => tmp.MarketRange == 1);

            //var queryUnknown = carsList.Where(tmp => tmp.MarketRange == 0  );


            var queryHasPic = carsList.Where(tmp => tmp.HasImage);


            var queryHasDescriptions = carsList.Where(tmp => tmp.HasDescription);


            var queryHasSalePrice = carsList.Where(tmp => tmp.HasSalePrice);
            double lastPercentage;
            switch (fieldName)
            {
                case "AboveMarket":
                    result = queryAbove.Count().ToString();
                    break;
                case "AverageMarket":
                    //result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=10") + "\"><div class='kpi_pdp_items_value kpi_pdp_price'>" + queryAve.Count() + "</div></a>";
                    result = queryAve.Count().ToString();
                    break;
                case "BelowMarket":
                    result = queryBelow.Count().ToString();
                    break;
                case "PercentAboveMarket":
                    result = GetPercentageString((double)queryAbove.Count() / carsList.Count);
                    break;
                case "PercentAverageMarket":
                    result = GetPercentageString((double)queryAve.Count() / carsList.Count);
                    break;
                case "PercentBelowMarket":
                    lastPercentage = 1 - (double)queryAbove.Count() / carsList.Count - (double)queryAve.Count() / carsList.Count;
                    result = GetPercentageString(lastPercentage);
                    break;
                case "PercentPics":
                    {
                        lastPercentage = (double)queryHasPic.Count() / carsList.Count;

                        if (lastPercentage >= 0 && lastPercentage <= 0.33)
                            result = GetPercentageString(lastPercentage);
                        else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                            result = GetPercentageString(lastPercentage);
                        else
                            result = GetPercentageString(lastPercentage);
                    }
                    break;
                case "NPercentPics":
                    {
                        lastPercentage = (double)queryHasPic.Count() / carsList.Count;

                        if (lastPercentage >= 0 && lastPercentage <= 0.33)
                            result = GetPercentageString(lastPercentage);
                        else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                            result = GetPercentageString(lastPercentage);
                        else
                            result = GetPercentageString(lastPercentage);
                    }
                    break;
                case "PercentDescriptions":
                    {
                        lastPercentage = (double)queryHasDescriptions.Count() / carsList.Count;

                        if (lastPercentage >= 0 && lastPercentage <= 0.33)
                            result = GetPercentageString(lastPercentage);
                        else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                            result = GetPercentageString(lastPercentage);
                        else
                            result = GetPercentageString(lastPercentage);
                    }
                    break;
                case "NPercentDescriptions":
                    {
                        lastPercentage = (double)queryHasDescriptions.Count() / carsList.Count;

                        if (lastPercentage >= 0 && lastPercentage <= 0.33)
                            result = GetPercentageString(lastPercentage);
                        else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                            result = GetPercentageString(lastPercentage);
                        else
                            result = GetPercentageString(lastPercentage);
                    }
                    break;
                case "PercentSalePrice":
                    {
                        lastPercentage = (double)queryHasSalePrice.Count() / carsList.Count;

                        if (lastPercentage >= 0 && lastPercentage <= 0.33)
                            result = GetPercentageString(lastPercentage);
                        else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                            result = GetPercentageString(lastPercentage);
                        else
                            result = GetPercentageString(lastPercentage);
                    }
                    break;
                case "NPercentSalePrice":
                    {
                        lastPercentage = (double)queryHasSalePrice.Count() / carsList.Count;

                        if (lastPercentage >= 0 && lastPercentage <= 0.33)
                            result = GetPercentageString(lastPercentage);
                        else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                            result = GetPercentageString(lastPercentage);
                        else
                            result = GetPercentageString(lastPercentage);
                    }
                    break;
                case "HiddenInventoryGauge":
                    {
                        double percent = Math.Ceiling((((queryAbove.Count() + queryBelow.Count()) * .5 + queryAve.Count()) / carsList.Count) * 100);
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
                    }
                    break;
                case "HiddenContentGauge":
                    {
                        double percent = Math.Ceiling(((queryHasPic.Count() + (double)queryHasDescriptions.Count() + queryHasSalePrice.Count()) * 100) / (3 * carsList.Count));
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
                    }
                    break;
                case "0-15InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "0-15InInventoryPriceAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);
                        result = queryInInventory.ToList().Average(x => x.SalePrice).ToString("C0");
                    }
                    break;
                case "0-15InInventoryOther":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(
                                tmp => tmp.DaysInInvenotry <= 15 && (tmp.MarketRange == 4 || tmp.MarketRange == 0));
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "0-15InInventoryAbove":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry <= 15 && tmp.MarketRange == 3);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "0-15InInventoryAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry <= 15 && tmp.MarketRange == 2);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "0-15InInventoryBelow":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry <= 15 && tmp.MarketRange == 1);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;

                case "16-30InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "16-30InInventoryPriceAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);
                        result = queryInInventory.ToList().Average(x => x.SalePrice).ToString("C0");
                    }
                    break;
                case "16-30InInventoryOther":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30 && (tmp.MarketRange == 4 || tmp.MarketRange == 0));
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "16-30InInventoryAbove":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30 && tmp.MarketRange == 3);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "16-30InInventoryAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30 && tmp.MarketRange == 2);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "16-30InInventoryBelow":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30 && tmp.MarketRange == 1);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;

                case "31-60InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "31-60InInventoryPriceAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);
                        result = queryInInventory.ToList().Average(x => x.SalePrice).ToString("C0");
                    }
                    break;
                case "31-60InInventoryOther":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60 && (tmp.MarketRange == 4 || tmp.MarketRange == 0));
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "31-60InInventoryAbove":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60 && tmp.MarketRange == 3);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "31-60InInventoryAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60 && tmp.MarketRange == 2);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "31-60InInventoryBelow":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60 && tmp.MarketRange == 1);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;

                case "61-90InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "61-90InInventoryPriceAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);
                        result = queryInInventory.ToList().Average(x => x.SalePrice).ToString("C0");
                    }
                    break;
                case "61-90InInventoryOther":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90 && (tmp.MarketRange == 4 || tmp.MarketRange == 0));
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "61-90InInventoryAbove":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90 && tmp.MarketRange == 3);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "61-90InInventoryAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90 && tmp.MarketRange == 2);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "61-90InInventoryBelow":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90 && tmp.MarketRange == 1);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;

                case "90OverInInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "90OverInInventoryPriceAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

                        result = queryInInventory.Count() > 0
                                     ? queryInInventory.ToList().Average(x => x.SalePrice).ToString("C0")
                                     : "0";
                    }
                    break;
                case "90OverInInventoryOther":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 90 && (tmp.MarketRange == 4 || tmp.MarketRange == 0));
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "90OverInInventoryAbove":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 90 && tmp.MarketRange == 3);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "90OverInInventoryAvg":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 90 && tmp.MarketRange == 2);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;
                case "90OverInInventoryBelow":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory =
                            carsList.Where(tmp => tmp.DaysInInvenotry > 90 && tmp.MarketRange == 1);
                        result = queryInInventory.ToList().Count.ToString();
                    }
                    break;

                case "Percent0-15InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);
                        result = GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count);
                    }
                    break;
                case "Percent16-30InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

                        result = GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count);
                    }
                    break;
                case "Percent31-60InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);
                        result = GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count);
                    }
                    break;
                case "Percent61-90InInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory1 = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);
                        double result1 =
                            Convert.ToDouble(GetPercentageString((double)queryInInventory1.ToList().Count / carsList.Count)
                                                .Replace("%", ""));
                        IEnumerable<CarInfoFormViewModel> queryInInventory2 = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);
                        double result2 =
                            Convert.ToDouble(GetPercentageString((double)queryInInventory2.ToList().Count / carsList.Count)
                                                .Replace("%", ""));
                        IEnumerable<CarInfoFormViewModel> queryInInventory3 = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);
                        double result3 =
                            Convert.ToDouble(GetPercentageString((double)queryInInventory3.ToList().Count / carsList.Count)
                                                .Replace("%", ""));
                        IEnumerable<CarInfoFormViewModel> queryInInventory4 = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);
                        double result4 =
                            Convert.ToDouble(
                                GetPercentageString((double)queryInInventory4.ToList().Count / carsList.Count)
                                    .Replace("%", ""));
                        if (result1 + result2 + result3 + result4 > 100)
                        {
                            result = GetPercentageString((100 - result1 - result2 - result3) / 100);
                        }
                        else
                        {
                            result = GetPercentageString((double)queryInInventory4.ToList().Count / carsList.Count);
                        }
                    }
                    break;
                case "Percent90OverInInventory":
                    {
                        IEnumerable<CarInfoFormViewModel> queryInInventory1 = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);
                        double result1 =
                            Convert.ToDouble(GetPercentageString((double)queryInInventory1.ToList().Count / carsList.Count)
                                                .Replace("%", ""));
                        IEnumerable<CarInfoFormViewModel> queryInInventory2 = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);
                        double result2 =
                            Convert.ToDouble(GetPercentageString((double)queryInInventory2.ToList().Count / carsList.Count)
                                                .Replace("%", ""));
                        IEnumerable<CarInfoFormViewModel> queryInInventory3 = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);
                        double result3 =
                            Convert.ToDouble(GetPercentageString((double)queryInInventory3.ToList().Count / carsList.Count)
                                                .Replace("%", ""));
                        IEnumerable<CarInfoFormViewModel> queryInInventory4 = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);
                        double result4 =
                            Convert.ToDouble(GetPercentageString((double)queryInInventory4.ToList().Count / carsList.Count)
                                                .Replace("%", ""));
                        if (result1 + result2 + result3 + result4 > 100)
                        {
                            result4 = Convert.ToDouble(GetPercentageString((100 - result1 - result2 - result3) / 100).Replace("%", ""));
                        }
                        result = GetPercentageString((100 - result1 - result2 - result3 - result4) / 100);
                    }
                    break;
                case "KPISideBar":
                    {
                        var builder = new StringBuilder();
                        int i = 0;
                        int j = 0;
                        foreach (CarInfoFormViewModel car in model.SubSetList)
                        {
                            i = i + 1;
                            j = i % 2;
                            if (j.Equals(1))
                                builder.Append("<div class='kpi_list_items kpi_list_items_odd' >");
                            else
                            {
                                builder.Append("<div class='kpi_list_items' >");
                            }
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum kpi_list_img' > ");
                            builder.Append("<img src='" + car.SinglePhoto + "' width='65px;' height='45px;' />");
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum kpi_list_vin' > " + (car.Vin.Length < 8 ? car.Vin : car.Vin.Substring(car.Vin.Length - 8, car.Vin.Length - (car.Vin.Length - 8))));
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum' > " + car.Stock);
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum' > " + car.ModelYear);
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum_medium' title='" + car.Make + "'> " + car.Make);
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum_long' title='" + car.Model + "'> " + car.Model);
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum_medium' title='" + car.Trim + "'> " + car.Trim);
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum_long' title='" + car.ExteriorColor + "'> " + car.ExteriorColor);
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<a href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingID=") + car.ListingId + "\">");
                            builder.Append("<div class='kpi_list_collum kpi_list_carfaxowner' > " + car.CarFaxOwner);
                            builder.Append("</div>");
                            builder.Append("</a>");
                            builder.Append("<div class='kpi_list_collum' > " + car.DaysInInvenotry);
                            builder.Append("</div>");
                            builder.Append("<div class='kpi_list_collum kpi_list_marketData' > " + car.CarRanking + "/" + car.NumberOfCar);
                            builder.Append("</div>");
                            builder.Append("<div class='kpi_list_collum kpi_list_miles' > " + car.Mileage.ToString("#0,0"));
                            builder.Append("</div>");
                            builder.Append("<div class='kpi_list_collum kpi_list_price' > " + car.SalePrice.ToString("#0,0"));
                            builder.Append("</div>");
                            builder.Append("</div>");
                            //builder.Append("<li>" + Environment.NewLine);
                            //builder.Append(ImageLinkHelper.ImageLinkToInventoryFromMarket(htmlHelper, "ViewIProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { @class = "mThumb", width = "97px", height = "97px" }) + Environment.NewLine);

                            //builder.Append("<ul class=\"info\">" + Environment.NewLine);
                            //builder.Append("<li><span class=\"item_title\">" + car.ModelYear + " " + car.Make + "<br />" + Environment.NewLine);
                            //builder.Append(car.Model + "</span></a></li>" + Environment.NewLine);
                            //builder.Append("<li><span class=\"value\">" + "$" + car.SalePrice + "</span> <span class=\"date\">" + car.DateInStock.Value.ToShortDateString() + "</span></li>" + Environment.NewLine);
                            //builder.Append("</ul>" + Environment.NewLine);
                            //builder.Append("</li>" + Environment.NewLine);

                        }

                        result = builder.ToString();
                    }
                    break;
            }

            //if (fieldName.Equals("AboveMarket"))
            //{
            //    result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=9") + "\">" + queryAbove.Count() + "</a>";
            //}
            //else if (fieldName.Equals("AverageMarket"))
            //{
            //    result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=10") + "\">" + queryAve.Count() + "</a>";
            //}
            //else if (fieldName.Equals("BelowMarket"))
            //{
            //    result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=11") + "\">" + queryBelow.Count() + "</a>";
            //}
            //else if (fieldName.Equals("PercentAboveMarket"))
            //{
            //    result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=9") + "\">" + GetPercentageString((double)queryAbove.Count() / carsList.Count) + "</a>";

            //}
            //else if (fieldName.Equals("PercentAverageMarket"))
            //{
            //    result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=10") + "\">" + GetPercentageString((double)queryAve.Count() / carsList.Count) + "</a>";

            //}
            //else if (fieldName.Equals("PercentBelowMarket"))
            //{
            //    double lastPercentage = 1 - (double)queryAbove.Count() / carsList.Count - (double)queryAve.Count() / carsList.Count;

            //    result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=11") + "\">" + GetPercentageString(lastPercentage) + "</a>";

            //}


            //else if (fieldName.Equals("PercentPics"))
            //{
            //    double lastPercentage = (double)queryHasPic.Count() / carsList.Count;

            //    if (lastPercentage >= 0 && lastPercentage <= 0.33)
            //        result = "<div class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
            //        result = "<div class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else
            //        result = "<div class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //}

            //else if (fieldName.Equals("PercentDescriptions"))
            //{
            //    double lastPercentage = (double)queryHasDescriptions.Count() / carsList.Count;

            //    if (lastPercentage >= 0 && lastPercentage <= 0.33)
            //        result = "<div class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
            //        result = "<div class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else
            //        result = "<div class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //}

            //else if (fieldName.Equals("PercentSalePrice"))
            //{
            //    double lastPercentage = (double)queryHasSalePrice.Count() / carsList.Count;

            //    if (lastPercentage >= 0 && lastPercentage <= 0.33)
            //        result = "<div class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
            //        result = "<div class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else
            //        result = "<div class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //}

            //else if (fieldName.Equals("HiddenInventoryGauge"))
            //{
            //    double percent = Math.Ceiling((((queryAbove.Count() + queryBelow.Count()) * .5 + queryAve.Count()) / carsList.Count) * 100);

            //    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            //}

            //else if (fieldName.Equals("HiddenContentGauge"))
            //{
            //    double percent = Math.Ceiling(((queryHasPic.Count() + (double)queryHasDescriptions.Count() + queryHasSalePrice.Count()) * 100) / (3 * carsList.Count));

            //    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            //}

            //else if (fieldName.Equals("0-15InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

            //    result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=1") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("16-30InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

            //    result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=2") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("31-60InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

            //    result = "<a  class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=3") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("61-90InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

            //    result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=4") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("90OverInInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

            //    result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=5") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("Percent0-15InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

            //    result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=1") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("Percent16-30InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

            //    result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=2") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("Percent31-60InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

            //    result = "<a class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=3") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("Percent61-90InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

            //    result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=4") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("Percent90OverInInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

            //    result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=5") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}



            //else if (fieldName.Equals("NPercentPics"))
            //{
            //    double lastPercentage = (double)queryHasPic.Count() / carsList.Count;

            //    if (lastPercentage >= 0 && lastPercentage <= 0.33)
            //        result = "<div class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
            //        result = "<div class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else
            //        result = "<div class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //}

            //else if (fieldName.Equals("NPercentDescriptions"))
            //{
            //    double lastPercentage = (double)queryHasDescriptions.Count() / carsList.Count;

            //    if (lastPercentage >= 0 && lastPercentage <= 0.33)
            //        result = "<div class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
            //        result = "<div class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else
            //        result = "<div class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //}

            //else if (fieldName.Equals("NPercentSalePrice"))
            //{
            //    double lastPercentage = (double)queryHasSalePrice.Count() / carsList.Count;

            //    if (lastPercentage >= 0 && lastPercentage <= 0.33)
            //        result = "<div class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
            //        result = "<div class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //    else
            //        result = "<div class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></div>";
            //}

            //else if (fieldName.Equals("NHiddenInventoryGauge"))
            //{
            //    double percent = Math.Ceiling((((queryAbove.Count() + queryBelow.Count()) * .5 + queryAve.Count()) / carsList.Count) * 100);

            //    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            //}

            //else if (fieldName.Equals("NHiddenContentGauge"))
            //{
            //    double percent = Math.Ceiling(((queryHasPic.Count() + (double)queryHasDescriptions.Count() + queryHasSalePrice.Count()) * 100) / (3 * carsList.Count));

            //    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            //}

            //else if (fieldName.Equals("N0-15InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

            //    result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=1") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("N16-30InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

            //    result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=2") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("N31-60InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

            //    result = "<a  class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=3") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("N61-90InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

            //    result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=4") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("N90OverInInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

            //    result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=5") + "\">" + queryInInventory.ToList().Count + "</a>";
            //}
            //else if (fieldName.Equals("NPercent0-15InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

            //    result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=1") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("NPercent16-30InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

            //    result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=2") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("NPercent31-60InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

            //    result = "<a class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=3") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("NPercent61-90InInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

            //    result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=4") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}
            //else if (fieldName.Equals("NPercent90OverInInventory"))
            //{
            //    IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

            //    result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=5") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            //}

            //if (fieldName.Equals("KPISideBar"))
            //{

            //    var builder = new StringBuilder();
            //    builder.AppendLine("<font size=\"1\">");
            //    foreach (CarInfoFormViewModel car in model.SubSetList)
            //    {
            //        builder.Append("<li>" + Environment.NewLine);
            //        builder.Append(ImageLinkHelper.ImageLinkToInventoryFromMarket(htmlHelper, "ViewIProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { @class = "mThumb", width = "97px", height = "97px" }) + Environment.NewLine);

            //        builder.Append("<ul class=\"info\">" + Environment.NewLine);
            //        builder.Append("<li><span class=\"item_title\">" + car.ModelYear + " " + car.Make + "<br />" + Environment.NewLine);
            //        builder.Append(car.Model + "</span></a></li>" + Environment.NewLine);
            //        builder.Append("<li><span class=\"value\">" + "$" + car.SalePrice + "</span> <span class=\"date\">" + car.DateInStock.Value.ToShortDateString() + "</span></li>" + Environment.NewLine);
            //        builder.Append("</ul>" + Environment.NewLine);
            //        builder.Append("</li>" + Environment.NewLine);

            //    }
            //    builder.AppendLine("</font>");
            //    result = builder.ToString();
            //}
            //else
            //{
            //    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + " value=\"error\"" + " />";
            //}


            return result;
        }
        public static string DynamicHtmlLabelForInventory(this HtmlHelper<InventoryFormViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var builder = new StringBuilder();

            bool flag = true;


            switch (fieldName)
            {
                case "Numeric":
                    builder.Append("<script type=\"text/javascript\" language=\"javascript\">" + Environment.NewLine);
                    foreach (CarInfoFormViewModel car in model.CarsList.Where(p => p.IsSold == false))
                    {
                        builder.Append("$(\"#" + car.ListingId + "\").numeric({ decimal: false, negative: false }, function() { alert(\"Positive integers only\"); this.value = \"\"; this.focus(); }" + Environment.NewLine);
                        builder.Append("$(\"#" + car.ListingId + "\").numeric({ decimal: false, negative: false }, function() { alert(\"Positive integers only\"); this.value = \"\"; this.focus(); }" + Environment.NewLine);
                    }
                    builder.Append("</script>");
                    break;
                case "ExpressBucketJump":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {
                        if (flag)
                        {
                            builder.Append("<div id=\"div_" + car.ListingId + "\" class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            builder.Append("<div id=\"div_" + car.ListingId + "\" class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;
                        }

                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);
                        builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewIProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { width = 640, height = 480 }) + Environment.NewLine);
                        builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);
                        builder.Append("  </div>  " + Environment.NewLine);
                        builder.Append("  </div>  " + Environment.NewLine);
                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);
                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px\" >" + Environment.NewLine);
                        //if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                        //    builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" style=\"position: absolute;left: 2px\" value=\"True\" checked />" + Environment.NewLine);
                        //else
                        //    builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" style=\"position: absolute;left: 2px\" value=\"False\" />" + Environment.NewLine);

                        if (car.MarketRange == 3)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(CultureInfo.InvariantCulture), new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Stock, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);


                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        if (car.NotDoneBucketJump)
                            builder.Append("<a id=\"doneTodayBucketJump_" + car.ListingId + "_" + car.DaysInInvenotry + "\" href=\"javascript:;\" style=\"background-color:#860000;padding: 2px 12px;\">Done</a>");
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.Vin))
                        {
                            if (car.Vin.Length > 7)
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", "VIN " + car.Vin.Substring(car.Vin.Length - 7), new { ListingID = car.ListingId }));
                            }
                            else
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", "VIN " + car.Vin, new { ListingID = car.ListingId }));
                        }

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        builder.Append(car.CarRanking + "-" + car.NumberOfCar + " Market" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);

                        builder.Append(car.MarketLowestPrice + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        // set market ranking
                        builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);

                        builder.Append(car.MarketAveragePrice + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        // set market ranking

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);

                        builder.Append(car.MarketHighestPrice + Environment.NewLine);
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        if (car.CarFaxOwner < 1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);



                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        var odometerNumber = (car.Mileage);

                        if (odometerNumber > 0)
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        var salePriceNumber = (car.SalePrice);

                        if (salePriceNumber > 0)
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;

                case "InventoryGrid":
                    builder.Append(BuildInventory(htmlHelper, model.CarsList, true));
                    break;

                case "SmallInventoryGrid":
                    builder.Append(BuildInventory(htmlHelper, model.CarsList, false));
                    break;
                case "SmallReconInventoryGrid":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {

                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px;\" >" + Environment.NewLine);
                        if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"True\" checked />" + Environment.NewLine);
                        else
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"False\" />" + Environment.NewLine);
                        builder.Append(" </div>" + Environment.NewLine);
                        // add IsFeatured? checkbox

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        if (car.MarketRange == 3)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell noBorder shorter column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (car.IsTruck)
                            {
                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.Stock, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Stock, new { ListingID = car.ListingId }));
                                    //builder.Append(car.StockNumber + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                    //builder.Append(car.ModelYear + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.Make, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                                    //builder.Append(car.Make + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Model, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Trim, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);
                            }
                            else
                            {
                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.Stock, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Stock, new { ListingID = car.ListingId }));
                                    //builder.Append(car.StockNumber + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                    //builder.Append(car.ModelYear + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.Make, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                                    //builder.Append(car.Make + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Model, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Trim, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Stock,
                                                                        new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          car.ModelYear.ToString(),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make,
                                                                          new { ListingID = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          CommonHelper.TrimString(car.Model, 12),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          CommonHelper.TrimString(car.Trim, 12),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);
                        }

                        builder.Append("<div class=\"short cell column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        //builder.Append("<div class=\"cell short column\" title=\"Number of Owners (Based on Carfax)\">" + Environment.NewLine);
                        //if (car.CarFaxOwner < 1)
                        //    builder.Append("Unknown" + Environment.NewLine);
                        //else if (car.CarFaxOwner == 1)

                        //    builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        //else
                        //{
                        //    builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        //}

                        //builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        if (car.Reconstatus)
                            builder.Append("<input type=\"checkbox\" checked=\"checked\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"checkbox\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"Days in Inventory\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        var odometerNumber = (car.Mileage);

                        if (odometerNumber > 0)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter wide column\">" + Environment.NewLine);

                        var salePriceNumber = (car.SalePrice);

                        if (salePriceNumber > 0)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\"  onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;
                case "SmallSoldInventoryGrid":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {

                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px;\" >" + Environment.NewLine);
                        if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"True\" checked />" + Environment.NewLine);
                        else
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"False\" />" + Environment.NewLine);
                        builder.Append(" </div>" + Environment.NewLine);
                        // add IsFeatured? checkbox

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        //if (car.MarketRange == 3)
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        //else if (car.MarketRange == 2)
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        //else if (car.MarketRange == 1)
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        //else
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell noBorder shorter column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Stock, new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"short cell column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\" title=\"Number of Owners (Based on Carfax)\">" + Environment.NewLine);
                        if (car.CarFaxOwner < 1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"Days in Inventory\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        var odometerNumber = (car.Mileage);

                        if (odometerNumber > 0)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter wide column\">" + Environment.NewLine);

                        var salePriceNumber = (car.SalePrice);

                        if (salePriceNumber > 0)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\"  onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);


                        //builder.Append("</div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);





                    }




                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;



                case "InventorySoldGrid":

                    builder.Append("<div id=\"sold\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList.Where(p => p.IsSold == true))
                    {
                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }


                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);




                        builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);

                        builder.Append("<div class=\"text\">" + Environment.NewLine);

                        builder.Append("<h2>SOLD</h2>" + Environment.NewLine);


                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewISoldProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { width = 640, height = 480 }) + Environment.NewLine);


                        builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);



                        builder.Append("  </div>  " + Environment.NewLine);

                        builder.Append("  </div>  " + Environment.NewLine);


                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        if (car.MarketRange == 3)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make, new { ListingID = car.ListingId }));


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Stock, new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(car.DaysInInvenotry + " Days Old");

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);



                        builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.Vin))
                        {
                            if (car.Vin.Length > 7)
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", "VIN " + car.Vin.Substring(car.Vin.Length - 7), new { ListingID = car.ListingId }));

                            }
                            else
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", "VIN " + car.Vin, new { ListingID = car.ListingId }));

                        }
                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                        if (car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                            builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.InteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                        builder.Append(0 + " Market" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);


                        //builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        ////builder.Append(car.NumberofCarOnCarsCom + " Cars.com" + Environment.NewLine);
                        //builder.Append(Environment.NewLine);
                        ////builder.Append(0 + " Cars.com" + Environment.NewLine);
                        //builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        if (car.CarFaxOwner < 1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                        //builder.Append(car.NumberofCarOnEbay + " Ebay" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        //builder.Append(0 + " Ebay" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        //builder.Append("<a href=\"\">EB</a>/<a href=\"https://post.craigslist.org/c/orc?lang=en\" onclick=\"window.open('https://post.craigslist.org/c/orc?lang=en')\">CL</a>" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "MarkUnsold", "Unsold", new { ListingID = car.ListingId }));

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"" + car.SoldOutDaysLeft + " Days Left " + " \">" + Environment.NewLine);

                        builder.Append(car.SoldOutDaysLeft + " Days");

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);



                    }
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);

                    break;
                case "SortOption":
                    builder.Append("<select name=\"sortBy\" id=\"sortBy\" onchange=\"this.form.submit();\" > " + Environment.NewLine);

                    builder.Append("<option> Select an option...</option>" + Environment.NewLine);

                    builder.Append("<option>Year</option>" + Environment.NewLine);

                    builder.Append("<option>Make</option>" + Environment.NewLine);

                    builder.Append("<option>Model</option>" + Environment.NewLine);


                    builder.Append("<option>Price</option>" + Environment.NewLine);


                    builder.Append("<option>Color</option>" + Environment.NewLine);

                    builder.Append("<option>Age</option>" + Environment.NewLine);

                    builder.Append("</select>" + Environment.NewLine);
                    break;

                default:
                    builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + "\" value=\"error\"" + " />");

                    break;
            }




            return builder.ToString();
        }
        private static void DynamicHtmlLabelForAdvancedSearchByCategory(HtmlHelper<List<CarInfoFormViewModel>> htmlHelper, UrlHelper urlHelper, StringBuilder builder, List<CarInfoFormViewModel> model, bool flag, string name)
        {
            builder.Append("<div id=\"table\" class=\"" + name + "\" >" + Environment.NewLine);
            builder.Append("<div style=\"width: 100%; height: 20px; overflow: hidden; border-bottom: 4px solid #DDD; font-size:16px\">" + name + "</div>");

            foreach (var car in model)
            {
                var controllerName = "Inventory";
                var actionName = string.Empty;
                var actionForViewOnly = string.Empty;
                var paramName = string.Empty;

                switch (car.Type)
                {
                    case 2:
                        actionForViewOnly = "ViewIProfile";
                        actionName = "ViewIProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                        controllerName = "Wholesale";
                        break;
                    case 3:
                        actionForViewOnly = "ViewProfileForAppraisal";
                        actionName = car.IsTruck ? "EditAppraisalForTruck" : "EditAppraisal";
                        controllerName = "Appraisal";
                        break;
                    case 4:
                        actionForViewOnly = "ViewISoldProfile";
                        actionName = "ViewISoldProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                        break;
                    default:
                        actionForViewOnly = "ViewIProfile";
                        actionName = car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                        break;
                }


                if (flag)
                {
                    builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                    flag = false;
                }
                else
                {
                    builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                    flag = true;
                }

                builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);
                builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);
                if (controllerName != "Appraisal")
                    builder.Append(ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto, "", new { car.ListingId }, null, new { width = 50, height = 50 }) + Environment.NewLine);
                else
                    builder.Append(ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto, "", new { AppraisalId = car.ListingId }, null, new { width = 50, height = 50 }) + Environment.NewLine);
                builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);
                builder.Append("  </div>  " + Environment.NewLine);
                builder.Append("  </div>  " + Environment.NewLine);
                builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);
                builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);
                builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);

                switch (car.MarketRange)
                {
                    case 3:
                        //builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        if (controllerName != "Appraisal")
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        break;
                    case 2:
                        //builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        if (controllerName != "Appraisal")
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        break;
                    case 1:
                        //builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        if (controllerName != "Appraisal")
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        break;
                    default:
                        builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        break;
                }

                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.ModelYear.ToString(), new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.ModelYear.ToString(), new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.Make, new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.Make, new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Model, 12),
                                                                    new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Model, 12),
                                                                    new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Trim, 12),
                                                                    new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Trim, 12),
                                                                    new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.Stock, new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.Stock, new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                builder.Append(car.DaysInInvenotry + " Days");
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                var odometerNumber = (car.Mileage);

                if (odometerNumber > 0)
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                else
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                var salePriceNumber = (car.SalePrice);

                if (salePriceNumber > 0)
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                else
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" disabled=\"disabled\"/>" + Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);
                builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                if (!String.IsNullOrEmpty(car.Vin) && controllerName != "Appraisal")
                {
                    if (car.Vin.Length > 7)
                    {
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName, "VIN " + car.Vin.Substring(car.Vin.Length - 7), new { car.ListingId }));
                    }
                    else
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName, "VIN " + car.Vin, new { car.ListingId }));
                }

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                    builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                else
                    builder.Append(car.ExteriorColor + Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                    builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                else
                    builder.Append(car.InteriorColor + Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                builder.Append(0 + " Market" + Environment.NewLine);
                builder.Append(Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);

                builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                if (car.CarFaxOwner < 1)
                    builder.Append("Unknown" + Environment.NewLine);
                else if (car.CarFaxOwner == 1)

                    builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                else
                {
                    builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                }
                builder.Append(Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);

                builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);

                //if (car.Reconstatus)
                //    builder.Append("<input type=\"checkbox\" checked=\"checked\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);
                //else
                //    builder.Append("<input type=\"checkbox\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);

                builder.Append(Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                //builder.Append("<a class=\"iframe\" href=\"" + urlHelper.Content("~/Market/ViewEbay?ListingId=" + car.ListingId) + "\">Ebay</a>" + Environment.NewLine);
                builder.Append("<a class=\"iframe\" href=\"javascript:;\"></a>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);

                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                if (controllerName != "Appraisal" && controllerName != "Wholesale")
                {
                    builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" +
                                   urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) +
                                   "\">BG</a>/<a title=\"Window Sticker\" target=\"_blank\" onclick=\"javascript:openWindowSticker(" +
                                   car.ListingId + ")" + "\">WS</a>" + Environment.NewLine);
                }
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
            }
            builder.Append("</div>" + Environment.NewLine);
        }
        public static string DynamicHtmlLabelForAdvancedSearch(this HtmlHelper<List<CarInfoFormViewModel>> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var builder = new StringBuilder();
            bool flag = true;
            int noOfType = model.Select(e => e.Type).Distinct().Count();

            if (noOfType > 1)
            {
                builder.Append("<div id=\"inven\">" + Environment.NewLine);
                builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);

                var newInventory = model.Where(e => e.Type == Constanst.CarInfoType.New).ToList();
                if (newInventory.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, newInventory, flag, "New");
                }

                var usedInventory = model.Where(e => e.Type == Constanst.CarInfoType.Used).ToList();
                if (usedInventory.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, usedInventory, flag, "Used");
                }

                var soldInventory = model.Where(e => e.Type == Constanst.CarInfoType.Sold).ToList();
                if (soldInventory.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, soldInventory, flag, "Sold");
                }

                var wholeSale = model.Where(e => e.Type == Constanst.CarInfoType.Wholesale).ToList();
                if (wholeSale.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, wholeSale, flag, "Wholesale");
                }

                var appraisal = model.Where(e => e.Type == Constanst.CarInfoType.Appraisal).ToList();
                if (appraisal.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, appraisal, flag, "Appraisals");
                }

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
            }
            else
            {
                switch (fieldName)
                {
                    case "InventoryGrid":

                        builder.Append("<div id=\"inven\">" + Environment.NewLine);
                        builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                        builder.Append("<div id=\"table\" >" + Environment.NewLine);
                        foreach (var car in model)
                        {
                            var controllerName = "Inventory";
                            var actionName = string.Empty;
                            var actionForViewOnly = string.Empty;
                            var paramName = string.Empty;

                            switch (car.Type)
                            {
                                case 2:
                                    actionForViewOnly = "ViewIProfile";
                                    actionName = "ViewIProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                                    controllerName = "Inventory";
                                    break;
                                case 3:
                                    actionForViewOnly = "ViewProfileForAppraisal";
                                    actionName = car.IsTruck ? "EditAppraisalForTruck" : "EditAppraisal";
                                    controllerName = "Appraisal";
                                    break;
                                case 4:
                                    actionForViewOnly = "ViewISoldProfile";
                                    actionName = "ViewISoldProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                                    break;
                                default:
                                    actionForViewOnly = "ViewIProfile";
                                    actionName = car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                                    break;
                            }

                            if (flag)
                            {
                                builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                                flag = false;
                            }
                            else
                            {
                                builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                                flag = true;
                            }

                            builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);
                            builder.Append(
                                " <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" +
                                Environment.NewLine);
                            if (controllerName != "Appraisal")
                                builder.Append(
                                    ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto,
                                                              "", new { car.ListingId }, null,
                                                              new { width = 50, height = 50 }) + Environment.NewLine);
                            else
                                builder.Append(
                                    ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto,
                                                              "", new { AppraisalId = car.ListingId }, null,
                                                              new { width = 50, height = 50 }) + Environment.NewLine);
                            builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" +
                                           car.HealthLevel + "\" />" + Environment.NewLine);
                            builder.Append("  </div>  " + Environment.NewLine);
                            builder.Append("  </div>  " + Environment.NewLine);
                            builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);
                            builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);
                            builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);

                            switch (car.MarketRange)
                            {
                                case 3:
                                    if (controllerName != "Appraisal")
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    else
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    break;
                                case 2:
                                    if (controllerName != "Appraisal")
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    else
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    break;
                                case 1:
                                    if (controllerName != "Appraisal")
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    else
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    break;
                                default:
                                    builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                                    break;
                            }

                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.ModelYear.ToString(),
                                                                                new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.ModelYear.ToString(),
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.Make, new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.Make,
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Model, 12),
                                                                                new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Model, 12),
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Trim, 12),
                                                                                new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Trim, 12),
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.Stock, new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.Stock,
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                            builder.Append(car.DaysInInvenotry + " Days");
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            var odometerNumber = (car.Mileage);

                            if (odometerNumber > 0)
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" +
                                               odometerNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                            else
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" +
                                               car.Mileage + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            var salePriceNumber = (car.SalePrice);

                            if (salePriceNumber > 0)
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" +
                                               salePriceNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                            else
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" +
                                               car.SalePrice + "\" disabled=\"disabled\"/>" + Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                            if (!String.IsNullOrEmpty(car.Vin) && controllerName != "Appraisal")
                            {
                                if (car.Vin.Length > 7)
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                  "VIN " +
                                                                                  car.Vin.Substring(car.Vin.Length - 7),
                                                                                  new { car.ListingId }));
                                }
                                else
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                  "VIN " + car.Vin, new { car.ListingId }));
                            }

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                            if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                                builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                            else
                                builder.Append(car.ExteriorColor + Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                            if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                                builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                            else
                                builder.Append(car.InteriorColor + Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                            builder.Append(0 + " Market" + Environment.NewLine);
                            builder.Append(Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                            if (car.CarFaxOwner < 1)
                                builder.Append("Unknown" + Environment.NewLine);
                            else if (car.CarFaxOwner == 1)

                                builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                            else
                            {
                                builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                            }
                            builder.Append(Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);

                            builder.Append(Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            builder.Append("<a class=\"iframe\" href=\"javascript:;\"></a>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                            if (controllerName != "Appraisal" && controllerName != "Wholesale")
                            {
                                builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" +
                                               urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) +
                                               "\">BG</a>/<a title=\"Window Sticker\" target=\"_blank\" onclick=\"javascript:openWindowSticker(" +
                                               car.ListingId + ")" + "\">WS</a>" + Environment.NewLine);
                            }
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);

                        }

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        break;

                    default:
                        builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" +
                                       fieldName + "\" value=\"error\"" + " />");
                        break;
                }
            }
            return builder.ToString();
        }
        public static string DynamicHtmlForm(this HtmlHelper<CarInfoFormViewModel> htmlHelper)
        {
            var model = htmlHelper.ViewData.Model;
            string result = "";
            if (model.Success)
            {
                result = " <form name=\"viewProfileForm\"  action=\"../Appraisal/ViewProfileByVin\" method=\"post\"  onsubmit=\"return validateForm()\" >";
            }
            else
            {
                result = " <form name=\"viewProfileForm\" action=\"../Appraisal/ViewProfileByYear\" method=\"post\"  onsubmit=\"return validateForm()\" >";
            }



            return result;
        }
        public static string DynamicHtmlImageForm(this HtmlHelper<ImageViewModel> htmlHelper)
        {
            var model = htmlHelper.ViewData.Model;
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string result = "";

            result = "<form id=\"ImageHandlerForm\" action=\"" + urlHelper.Content("~/ImageHandlers/Handler.ashx") + "?ListingId=" + model.ListingId + "&DealerId=" + model.DealerId + "&Vin=" + model.Vin + "&Overlay=1" + "\" method=\"POST\" enctype=\"multipart/form-data\" >";

            return result;
        }
        private static string GetPercentageString(double ratio)
        {
            return ratio.ToString("0%");
        }
        public static string DynamicHtmlLabelAdmin(this HtmlHelper<AdminViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var builder = new StringBuilder();
            switch (fieldName)
            {
                case "SortSetting":
                    if (model.DealerSetting.InventorySorting.Equals("Make"))
                    {
                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Make\">Make (Default)</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }
                    else if (model.DealerSetting.InventorySorting.Equals("Model"))
                    {

                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Model\">Model (Default)</option>");
                        builder.Append("<option value=\"Make\">Make</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }
                    else if (model.DealerSetting.InventorySorting.Equals("Year"))
                    {

                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Model\">Year (Default)</option>");
                        builder.Append("<option value=\"Make\">Make</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }
                    else if (model.DealerSetting.InventorySorting.Equals("Market"))
                    {

                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Market\">Market (Default)</option>");
                        builder.Append("<option value=\"Make\">Make</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("</select>");
                    }
                    else
                    {
                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Make\">Make (Default)</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }



                    break;
                case "SoldOut":

                    if (model.DealerSetting.InventorySorting.Equals("Delete (Default)"))
                    {

                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"0\">Delete (Default)</option>");
                        builder.Append("<option value=\"3\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"5\">Display as Sold (5 Days)</option>");
                        builder.Append("<option value=\"7\">Display as Sold (7 Days)</option>");
                        builder.Append("<option value=\"30\">Display as Sold (30 Days)</option>");
                        builder.Append("</select>");
                    }
                    else if (model.DealerSetting.InventorySorting.Equals("Display as Sold (3 Days)"))
                    {

                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"3\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"0\"> Delete (Default)</option>");
                        builder.Append("<option value=\"5\">Display as Sold (5 Days)</option>");
                        builder.Append("<option value=\"7\">Display as Sold (7 Days)</option>");
                        builder.Append("<option value=\"7\">Display as Sold (30 Days)</option>");
                        builder.Append("</select>");
                    }
                    else if (model.DealerSetting.InventorySorting.Equals("Display as Sold (5 Days)"))
                    {

                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"0\">Display as Sold (5 Days)</option>");
                        builder.Append("<option value=\"0\">Delete (Default)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (7 Days)</option>");
                        builder.Append("</select>");
                    }
                    else
                    {
                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"0\">Display as Sold (7 Days)</option>");
                        builder.Append("<option value=\"0\">Delete (Default)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (5 Days)</option>");
                        builder.Append("</select>");
                    }



                    break;
            
                case "ApprasialUsersNotificationList":

                    int index = 0;

                    List<int> listAppraisalNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {

                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.AppraisalNotification)
                        //    builder.Append(" <li id=\"Appraisal" + user.Username + "\"><input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:appraisalNotifyPerUser(this);\"   id=\"AppraisalCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"Appraisal" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:appraisalNotifyPerUser(this);\"   id=\"AppraisalCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");
                        if (!listAppraisalNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.AppraisalNotification, user.AppraisalNotification,
                                            "appraisalNotifyPerUser", "AppraisalCheckbox");
                            listAppraisalNotifyPerUser.Add(user.UserId);
                        }
                    }
                    break;
                case "WholeSaleUsersNotificationList":


                    index = 0;

                    List<int> listWholeSaleNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.WholeSaleNotfication)
                        //    builder.Append(" <li id=\"WholeSale" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:wholeSaleNotifyPerUser(this);\" id=\"WholeSaleCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"WholeSale" + user.Username + "\"><input type=\"checkbox\" disabled=true  onclick=\"javascript:wholeSaleNotifyPerUser(this);\" id=\"WholeSaleCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");


                        //if (user.WholeSaleNotfication)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:wholeSaleNotifyPerUser(this);\"   id=\"WholeSaleCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:wholeSaleNotifyPerUser(this);\"   id=\"WholeSaleCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");

                        if (!listWholeSaleNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.WholeSaleNotfication, user.WholeSaleNotfication,
                                            "wholeSaleNotifyPerUser", "WholeSaleCheckbox");
                            listWholeSaleNotifyPerUser.Add(user.UserId);
                        }

                    }


                    break;
                case "InventoryUsersNotificationList":


                    index = 0;

                    List<int> listInventoryNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.InventoryNotfication)
                        //    builder.Append(" <li id=\"Inventory" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:inventoryNotifyPerUser(this);\" id=\"InventoryCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"Inventory" + user.Username + "\"><input type=\"checkbox\" disabled=true  onclick=\"javascript:inventoryNotifyPerUser(this);\" id=\"InventoryCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");
                        //if (user.InventoryNotfication)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:inventoryNotifyPerUser(this);\"   id=\"InventoryCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:inventoryNotifyPerUser(this);\"   id=\"InventoryCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        if (!listInventoryNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.InventoryNotfication, user.InventoryNotfication,
                                            "inventoryNotifyPerUser", "InventoryCheckbox");
                            listInventoryNotifyPerUser.Add(user.UserId);
                        }

                    }


                    break;
                case "24HUsersNotificationList":


                    index = 0;

                    List<int> listTwentyfourhourNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.TwentyFourHourNotification)
                        //    builder.Append(" <li id=\"24H" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\"  onclick=\"javascript:twentyfourhourNotifyPerUser(this);\" id=\"24HCheckbox" + user.UserId + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"24H" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:twentyfourhourNotifyPerUser(this);\" id=\"24HCheckbox" + user.UserId + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");

                        //if (user.TwentyFourHourNotification)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:twentyfourhourNotifyPerUser(this);\"   id=\"24HCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:twentyfourhourNotifyPerUser(this);\"   id=\"24HCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        if (!listTwentyfourhourNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.TwentyFourHourNotification,
                                            user.TwentyFourHourNotification, "twentyfourhourNotifyPerUser",
                                            "24HCheckbox");
                            listTwentyfourhourNotifyPerUser.Add(user.UserId);
                        }

                    }


                    break;
                case "NoteUsersNotificationList":


                    index = 0;

                    List<int> listNoteNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.NoteNotification)
                        //    builder.Append(" <li id=\"Note" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:noteNotifyPerUser(this);\" id=\"NoteCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"Note" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:noteNotifyPerUser(this);\" id=\"NoteCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");


                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");
                        //if (user.NoteNotification)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:noteNotifyPerUser(this);\"   id=\"NoteCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:noteNotifyPerUser(this);\"   id=\"NoteCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        if (!listNoteNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.NoteNotification, user.NoteNotification,
                                            "noteNotifyPerUser", "NoteCheckbox");
                            listNoteNotifyPerUser.Add(user.UserId);
                        }

                    }


                    break;
                case "PriceUsersNotificationList":

                    index = 0;

                    List<int> listPriceNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.PriceChangeNotification)
                        //    builder.Append(" <li id=\"Price" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:priceNotifyPerUser(this);\" id=\"PriceCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"Price" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:priceNotifyPerUser(this);\" id=\"PriceCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");
                        //if (user.PriceChangeNotification)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:priceNotifyPerUser(this);\"   id=\"PriceCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:priceNotifyPerUser(this);\"   id=\"PriceCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        if (!listPriceNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.PriceChangeNotification,
                                            user.PriceChangeNotification, "priceNotifyPerUser", "PriceCheckbox");
                            listPriceNotifyPerUser.Add(user.UserId);
                        }


                    }


                    break;
                case "BucketJumpReportUsersNotificationList":

                    index = 0;

                    List<int> listBucketJumpReportNotifyPerUser = new List<int>();
                    foreach (var user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.BucketJumpReportNotification)
                        //    builder.Append(" <li id=\"BucketJump" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:bucketJumpReportNotifyPerUser(this);\" id=\"BucketJumpCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"BucketJump" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:bucketJumpReportNotifyPerUser(this);\" id=\"BucketJumpCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");
                        //if (user.BucketJumpReportNotification)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:bucketJumpReportNotifyPerUser(this);\"   id=\"BucketJumpCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:bucketJumpReportNotifyPerUser(this);\"   id=\"BucketJumpCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        if (!listBucketJumpReportNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.BucketJumpReportNotification,
                                            user.BucketJumpReportNotification, "bucketJumpReportNotifyPerUser",
                                            "BucketJumpCheckbox");
                            listBucketJumpReportNotifyPerUser.Add(user.UserId);
                        }

                    }


                    break;
                case "AgeingBucketJumpUsersNotificationList":

                    index = 0;

                    List<int> listAgeNotifyPerUser = new List<int>();
                    foreach (var user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.AgeingBucketJumpNotification)
                        //    builder.Append(" <li id=\"Age" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:ageNotifyPerUser(this);\" id=\"AgeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"Age" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:ageNotifyPerUser(this);\" id=\"AgeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");
                        //if (user.AgeingBucketJumpNotification)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:ageNotifyPerUser(this);\"   id=\"AgeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:ageNotifyPerUser(this);\"   id=\"AgeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        if (!listAgeNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.AgeingBucketJumpNotification,
                                            user.AgeingBucketJumpNotification, "ageNotifyPerUser", "AgeCheckbox");
                            listAgeNotifyPerUser.Add(user.UserId);
                        }

                    }


                    break;
                case "MarketPriceRangeNotificationList":

                    index = 0;

                    List<int> listMarketPriceRangeNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.MarketPriceRangeChangeNotification)
                        //    builder.Append(" <li id=\"MarketPriceRange" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:marketPriceRangeNotifyPerUser(this);\" id=\"MarketPriceRangeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"MarketPriceRange" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:marketPriceRangeNotifyPerUser(this);\" id=\"MarketPriceRangeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");

                        //if (user.MarketPriceRangeChangeNotification)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:marketPriceRangeNotifyPerUser(this);\"   id=\"MarketPriceRangeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:marketPriceRangeNotifyPerUser(this);\"   id=\"MarketPriceRangeCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");

                        if (!listMarketPriceRangeNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.MarketPriceRangeChangeNotification,
                                            user.MarketPriceRangeChangeNotification, "marketPriceRangeNotifyPerUser",
                                            "MarketPriceRangeCheckbox");
                            listMarketPriceRangeNotifyPerUser.Add(user.UserId);
                        }

                    }


                    break;

                case "ImageUploadNotificationList":

                    index = 0;

                    List<int> listImageUploadNotifyPerUser = new List<int>();
                    foreach (UserRoleViewModel user in model.Users)
                    {
                        //if (index % 5 == 0)
                        //    builder.Append("<ul class=\"column\">");

                        //if (user.ImageUploadNotification)
                        //    builder.Append(" <li id=\"ImageUpload" + user.Username + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:imageUploadNotifyPerUser(this);\" id=\"ImageUploadCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");
                        //else
                        //    builder.Append(" <li id=\"ImageUpload" + user.Username + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:imageUploadNotifyPerUser(this);\" id=\"ImageUploadCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + " </li>");

                        //index++;

                        //if (index % 5 == 0)
                        //    builder.Append("</ul>");

                        //if (user.ImageUploadNotification)
                        //    builder.Append("<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:imageUploadNotifyPerUser(this);\"   id=\"ImageUploadCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        //else
                        //    builder.Append("<label><input type=\"checkbox\" disabled=true onclick=\"javascript:imageUploadNotifyPerUser(this);\"   id=\"ImageUploadCheckbox" + user.UserId.ToString() + "\"/> " + user.Username + "</label>");
                        if (!listImageUploadNotifyPerUser.Contains(user.UserId))
                        {
                            SetItemCheckBox(user.Username, user.UserId, builder,
                                            model.NotificationSetting.ImageUploadNotification,
                                            user.ImageUploadNotification, "imageUploadNotifyPerUser",
                                            "ImageUploadCheckbox");
                            listImageUploadNotifyPerUser.Add(user.UserId);
                        }


                    }


                    break;
                case "AppraisalNotification":
                    if (model.NotificationSetting.AppraisalNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:appraisalNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:appraisalNotify(this);\" />On/Off");
                    break;
                case "WholeSaleNotification":
                    if (model.NotificationSetting.WholeSaleNotfication)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:WholesaleNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:WholesaleNotify(this);\" />On/Off");
                    break;
                case "InventoryNotfication":
                    if (model.NotificationSetting.InventoryNotfication)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:InventoryNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:InventoryNotify(this);\" />On/Off");
                    break;
                case "TwentyFourHourNotification":
                    if (model.NotificationSetting.TwentyFourHourNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:TwentyFourHourNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:TwentyFourHourNotify(this);\" />On/Off");
                    break;
                case "NoteNotification":
                    if (model.NotificationSetting.NoteNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:NoteNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:NoteNotify(this);\" />On/Off");
                    break;
                case "PriceChangeNotification":
                    if (model.NotificationSetting.PriceChangeNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:PriceNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:PriceNotify(this);\" />On/Off");
                    break;
                case "MarketPriceRangeChangeNotification":
                    if (model.NotificationSetting.MarketPriceRangeChangeNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:MarketPriceRangeNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:MarketPriceRangeNotify(this);\" />On/Off");
                    break;
                case "ImageUploadNotification":
                    if (model.NotificationSetting.ImageUploadNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:ImageUploadNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:ImageUploadNotify(this);\" />On/Off");
                    break;
                case "BucketJumpReportNotification":
                    if (model.NotificationSetting.BucketJumpReportNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:BucketJumpReportNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:BucketJumpReportNotify(this);\" />On/Off");
                    break;
                case "AgeingBucketJumpNotification":
                    if (model.NotificationSetting.AgeingBucketJumpNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:AgeNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:AgeNotify(this);\" />On/Off");
                    break;
                case "OverrideStockImage":
                    if (model.DealerSetting.OverrideStockImage)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:OverideStockImage(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:OverideStockImage(this);\" />");
                    break;

                case "RetailPriceWSNotification":
                    if (model.DealerSetting.RetailPriceWsNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:retailPriceWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:retailPriceWindowStickerNotify(this);\" />");
                    break;

                case "DealerDiscountWSNotification":
                    if (model.DealerSetting.DealerDiscountWSNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:dealerDiscountWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:dealerDiscountWindowStickerNotify(this);\" />");
                    break;

                case "ManufacturerReabateNotification":
                    if (model.DealerSetting.ManufacturerReabteWsNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:manufacturerRebateWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:manufacturerRebateWindowStickerNotify(this);\" />");
                    break;

                case "SalePriceNotification":
                    if (model.DealerSetting.SalePriceWsNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:salePriceWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:salePriceWindowStickerNotify(this);\" />");
                    break;

                case "StockingGuide_BrandList":
                    List<string> listBrand = SQLHelper.GetListBrandName();
                    List<string> selectedBrands = new List<string>();

                    if (!string.IsNullOrEmpty(model.DealerSetting.BrandName))
                    {
                        selectedBrands = model.DealerSetting.BrandName.Split(',').Select(p => p.Trim()).ToList();
                    }

                    for (int i = 0; i < listBrand.Count; i++)
                    {
                        string szName = listBrand[i];
                        string szClass = "";

                        if (i % 2 == 0)
                        {
                            szClass = "admin_stockingguide_brand_item admin_stockingguide_brand_even_item";
                        }
                        else
                        {
                            szClass = "admin_stockingguide_brand_odd_item";
                        }

                        string szHtmlCheck = "";

                        if (selectedBrands.Contains(szName))
                        {
                            szHtmlCheck = "<input type=\"checkbox\" class=\"admin_stockingguide_brand_item_check\" checked=\"checked\" />";
                        }
                        else
                        {
                            szHtmlCheck = "<input type=\"checkbox\" class=\"admin_stockingguide_brand_item_check\"/>";
                        }

                        string szHtmlLabel = "<div class=\"admin_stockingguide_brand_item_label\">" + szName + "</div>";
                        string szHtmlBrandItem = "<div class=\"" + szClass + "\">" + szHtmlCheck + szHtmlLabel + "</div>";
                        builder.Append(szHtmlBrandItem);
                    }

                    break;

                default:
                    builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + "\" value=\"error\"" + " />");

                    break;



            }



            return builder.ToString();
        }
        private static void SetItemCheckBox(string userName, int userId, StringBuilder builder, bool dealerNotified, bool userNotified, string javascriptName, string checkboxId)
        {
            if (userNotified)
            {
                builder.Append(
                    "<label> <input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:" + javascriptName + "(this);\"   id=\"" + checkboxId +
                   userId.ToString() + "\"/> " + userName + "</label>");
            }
            else
            {
                builder.Append(
                    "<label><input type=\"checkbox\" " + (dealerNotified ? String.Empty : "disabled=true") +
                    " onclick=\"javascript:" + javascriptName + "(this);\"   id=\"" + checkboxId +
                    userId.ToString() + "\"/> " + userName + "</label>");
            }
        }
        public static string DynamicHtmlLabelForEbay(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            //var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";
            switch (fieldName)
            {
                case "Vin":
                    result = "VIN <label for=\"" + fieldName + "\">" + model.Vin + "</label>";
                    break;

                case "Year":
                    result = "<label for=\"" + fieldName + "\">" + model.ModelYear + "</label>";
                    break;

                case "Make":
                    result = "<label for=\"" + fieldName + "\">" + model.Make + "</label>";
                    break;

                case "Model":
                    result = "<label for=\"" + fieldName + "\">" + model.Model + "</label>";
                    break;

                case "Stock":
                    result = "Stock #<label for=\"" + fieldName + "\">" + model.Stock + "</label>";
                    break;


                case "Mileage":
                    result = "<label for=\"" + fieldName + "\">" + model.Mileage + " Miles" + "</label>";
                    break;

                case "ExtColor":
                    result = "<label for=\"" + fieldName + "\">" + model.ExteriorColor + "</label>";
                    break;

                case "LargeImage":
                    result = "<img id=\"largeImage\" name=\"largeImage\" src=\"" + model.DefaultImageUrl + "\"/>" + Environment.NewLine;
                    break;


                case "LoadImage":

                    result = "<img id=\"image1\" name=\"image1\" class=\"selected\" src=\"" + model.DefaultImageUrl + "\"/>" + Environment.NewLine;
                    break;


                case "FactoryPackageOption":

                    string[] carOptions = model.CarsOptions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var fo in carOptions)
                    {

                        result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo + "\"/>" + fo + "</li>";
                    }

                    break;

                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }
        public static string DynamicHtmlLabelForSticker(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            //var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";
            switch (fieldName)
            {

                case "CarName":
                    result = model.CarName;
                    break;
                case "Tranmission":
                    result = model.Tranmission;
                    break;
                case "ExteriorColor":
                    result = model.ExteriorColor;
                    break;
                case "InteriorColor":
                    result = model.InteriorColor;
                    break;
                case "Vin":
                    result = "Vin :" + model.Vin;
                    break;

                case "Stock":
                    result = "Stock #" + model.Stock;
                    break;

                case "Year":
                    result = "<label for=\"" + fieldName + "\">" + model.ModelYear + "</label>";
                    break;

                case "Make":
                    result = "<label for=\"" + fieldName + "\">" + model.Make + "</label>";
                    break;

                case "FuelEconomyCity":
                    result = "<label for=\"" + fieldName + "\">" + model.FuelEconomyCity + "</label>";
                    break;
                case "FuelEconomyHighWay":
                    result = "<label for=\"" + fieldName + "\">" + model.FuelEconomyHighWay + "</label>";
                    break;
                case "SalePrice":
                    var salePriceNumber = (model.SalePrice);
                    if (salePriceNumber > 0)
                        result = "<label for=\"" + fieldName + "\">" + salePriceNumber.ToString("C") + "</label>";
                    else
                        result = "<label for=\"" + fieldName + "\">" + model.SalePrice + "</label>";
                    break;
                case "MSRP":
                    result = "<label for=\"" + fieldName + "\">" + model.Msrp + "</label>";
                    break;
                case "BarCode":
                    result = "<img height=\"70\" style=\"position: relative; z-index:300;\" src=\"http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" + model.Vin + "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false\" />";
                    break;

                case "Logo":
                    result = "<img height=\"60\" width=\"260\" src=\"" + model.LogoUrl + "\"/>";
                    break;

                case "MECHANICALLIST":
                    foreach (string tmp in model.MechanicalList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "EXTERIORLIST":
                    foreach (string tmp in model.ExteriorList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "ENTERTAINMENTLIST":
                    foreach (string tmp in model.EntertainmentList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "INTERIORLIST":
                    foreach (string tmp in model.InteriorList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "SAFETYLIST":
                    foreach (string tmp in model.SafetyList)
                        result += "<li class=\"\">" + tmp + "</li>";

                    break;

                case "FactoryPackageOption":


                    if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                    {
                        foreach (var fo in model.FactoryPackageOptions)
                        {
                            result += "<li>" + fo.Name + "</li>";
                            result += "<li class=\"pricing\">" + fo.Msrp + " </li>";
                        }
                    }

                    break;
                case "FactoryOption":

                    if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                    {
                        foreach (var fo in model.FactoryNonInstalledOptions)
                        {
                            result += "<li>" + fo.Name + "</li>";
                            result += "<li class=\"pricing\">" + fo.Msrp+ " </li>";
                        }
                    }
                    break;

                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }
        public static string DynamicHtmlControlForIprofile(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            string result = "";

            if (model.Success)
            {
                switch (fieldName)
                {
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            if (model.ExistOptions == null)
                            {
                                foreach (var fo in model.FactoryPackageOptions)
                                {

                                    result += "<li> <input type=\"checkbox\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                                }
                            }
                            else
                            {

                                var packageExist = model.ExistOptions.Where(t => t.Contains("Pkg") || t.Contains("Package"));

                                if (packageExist == null || packageExist.Count() == 0)
                                {

                                    foreach (var fo in model.FactoryPackageOptions)
                                    {

                                        result += "<li> <input type=\"checkbox\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                                    }
                                }
                                else
                                {
                                    foreach (var fo in model.FactoryPackageOptions)
                                    {
                                        var tmp = model.ExistOptions.Where(t => fo.Name.Contains(t));
                                        if (tmp == null || tmp.Count() == 0)

                                            result += "<li> <input type=\"checkbox\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                                        else
                                            result += "<li> <input type=\"checkbox\" checked=\"yes\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                                    }
                                }
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages available for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\">";
                        result += "<ul class=\"options\">";

                        if (model.ExistOptions == null)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                            }
                        }
                        else
                        {
                            var optionExist = model.ExistOptions.Where(t => !t.Contains("Pkg") && !t.Contains("Package"));
                            if (optionExist == null || optionExist.Count() == 0)
                            {
                                if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                                {
                                    foreach (var fo in model.FactoryNonInstalledOptions)
                                    {
                                        result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                                    }
                                }
                            }
                            else
                            {
                                foreach (var fo in model.FactoryNonInstalledOptions)
                                {
                                    var tmp = model.ExistOptions.Where(t => fo.Name.Contains(t));
                                    if (tmp == null || tmp.Count() == 0)

                                        result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                                    else
                                        result += "<li> <input type=\"checkbox\" checked=\"yes\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                                }
                            }
                        }


                        result += "</ul>";
                        result += "</div>";
                        break;
                    case "hiddenListingId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\"" + " />"; ;
                        break;
                    case "hiddenDealershipId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerId + "\"" + " />"; ;
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarImageUrl + "\"" + " />"; ;
                        break;
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.Stock))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Stock + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;
                    case "ExteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            var selectedExteriorColor = model.ExteriorColorList.Where(x => x.colorName.ToUpperInvariant().Equals(model.ExteriorColor.ToUpperInvariant()));

                            var extractExteriorColorList = model.ExteriorColorList.Where(x => !x.colorName.ToUpperInvariant().Equals(model.ExteriorColor.ToUpperInvariant()));

                            if (selectedExteriorColor == null || selectedExteriorColor.Count() == 0)
                            {
                                result += "<option>" + "Other Colors" + "</option>";

                                foreach (var ec in model.ExteriorColorList)
                                {
                                    result += "<option>" + ec.colorName + "</option>";
                                }


                            }
                            else
                            {
                                var exteriorColorList = new List<Color>();

                                exteriorColorList.Add(selectedExteriorColor.First());

                                exteriorColorList.AddRange(extractExteriorColorList);
                                foreach (var ec in exteriorColorList)
                                {
                                    result += "<option>" + ec.colorName + "</option>";
                                }
                                result += "<option>" + "Other Colors" + "</option>";
                            }
                        }
                        else
                        {
                            result += "<option>" + "Other Colors" + "</option>";
                        }

                        result += "</select>";


                        break;
                    case "InteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            var selectedInteriorColor = model.InteriorColorList.Where(x => x.colorName.ToUpperInvariant().Equals(model.InteriorColor.ToUpperInvariant()));

                            var extractInteriorColorList = model.InteriorColorList.Where(x => !x.colorName.ToUpperInvariant().Equals(model.InteriorColor.ToUpperInvariant()));

                            if (selectedInteriorColor == null || selectedInteriorColor.Count() == 0)
                            {
                                result += "<option>" + "Other Colors" + "</option>";

                                foreach (var ec in model.InteriorColorList)
                                {

                                    result += "<option>" + ec.colorName + "</option>";
                                }
                            }
                            else
                            {
                                var interiorColorList = new List<Color>();

                                interiorColorList.Add(selectedInteriorColor.First());

                                interiorColorList.AddRange(extractInteriorColorList);

                                foreach (var ic in interiorColorList)
                                {
                                    result += "<option>" + ic.colorName + "</option>";
                                }
                                result += "<option>" + "Other Colors" + "</option>";

                            }
                        }
                        else
                        {
                            result += "<option>" + "Other Colors" + "</option>";
                        }

                        result += "</select>";


                        break;

                    case "CustomExtColor":
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            var list = model.ExteriorColorList.Where(t => t.colorName.Equals(model.ExteriorColor));

                            if (list == null || list.Count() == 0)
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                            else
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";
                        }
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";

                        break;
                    case "CustomIntColor":

                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            var list = model.InteriorColorList.Where(t => t.colorName.Equals(model.InteriorColor));

                            if (list == null || list.Count() == 0)
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";

                            else
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";
                        }
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";



                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;



                    case "Odometer":
                        if (model.Mileage == 0)
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litter + "\" />";
                        break;
                    case "Tranmission":
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        if (String.IsNullOrEmpty(model.Tranmission))
                        {
                            result += "<option>Automatic</option>";
                            result += "<option>Manual</option>";
                            result += "<option>Shiftable Automatic Tranmission</option>";
                        }
                        else
                        {
                            if (model.Tranmission.Equals("Automatic"))
                            {
                                result += "<option>Automatic</option>";
                                result += "<option>Manual</option>";
                                result += "<option>Shiftable Automatic Tranmission</option>";
                            }
                            else if (model.Tranmission.Equals("Manual"))
                            {
                                result += "<option>Manual</option>";
                                result += "<option>Automatic</option>";
                                result += "<option>Shiftable Automatic Tranmission</option>";
                            }

                            else if (model.Tranmission.Equals("Shiftable Automatic Tranmission"))
                            {
                                result += "<option>Shiftable Automatic Tranmission</option>";
                                result += "<option>Manual</option>";
                                result += "<option>Automatic</option>";

                            }
                            else
                            {
                                result += "<option>Automatic</option>";
                                result += "<option>Manual</option>";
                                result += "<option>Shiftable Automatic Tranmission</option>";
                            }
                        }

                        result += "<option>" + "Other Tranmission" + "</option>";
                        result += "</select>";
                        //result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<select class=\"z-index\" style=\"width:130px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        XmlNode driveNode = XMLHelper.selectOneElement("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"), "Value=" + model.WheelDrive);
                        XmlNodeList allNode = XMLHelper.selectElements("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"));
                        if (driveNode != null)
                        {
                            result += "<option>" + driveNode.Attributes["Value"].Value + "</option>";

                            foreach (XmlNode node in allNode)
                            {
                                if (!node.Attributes["Value"].Value.Equals(driveNode.Attributes["Value"].Value))
                                    result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        else
                        {
                            foreach (XmlNode node in allNode)
                            {
                                result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        result += "<option>" + "Other Drives" + "</option>";
                        result += "</select>";


                        break;

                    //result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                    //break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Msrp + "\" />";
                        break;
                    case "SalePrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SalePrice + "\" />";
                        break;
                    case "RetailPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "DealerDiscount":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "ManufacturerRebate":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;

                    case "WindowStickerPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;

                    case "EditRetailPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "EditDealerDiscount":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "EditManufacturerRebate":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;

                    case "EditWindowStickerPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;

                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;
                    case "Certified":
                        if (model.IsCertified)
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\" checked=\"checked\" />";
                        else
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\"/>";
                        break;
                    case "PriorRental":
                        if (model.PriorRental)
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\" checked=\"checked\" />";
                        else
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\"/>";
                        break;
                    case "EditPriorRental":
                        if (model.PriorRental)
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditDealerDemo":
                        if (model.DealerDemo)
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditUnwind":
                        if (model.Unwind)
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "Warranty":
                        if (model.WarrantyInfo == 0)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";


                        }
                        else if (model.WarrantyInfo == 1)
                        {
                            result +=
                                "<input type=\"radio\" id=\"warrantyasis\" checked=\"checked\" value=\"1\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }

                        else if (model.WarrantyInfo == 2)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 3)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantydealercertified\"  value=\"3\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 4)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }


                        break;


                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                }
            }
            else
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                switch (fieldName)
                {
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            foreach (var fo in model.FactoryPackageOptions)
                            {
                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.Name+ fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages selected for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\">";
                        result += "<ul class=\"options\">";
                        if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.Name + fo.Msrp + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.Name + fo.Msrp + "</li>";
                            }
                        }

                        result += "</ul>";
                        result += "</div>";
                        break;

                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.Stock))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Stock + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;

                    case "ExteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            foreach (Color ec in model.ExteriorColorList)
                            {
                                result += "<option>" + ec.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;
                    case "InteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            foreach (Color ic in model.InteriorColorList)
                            {
                                result += "<option>" + ic.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;
                    case "EditPriorRental":
                        if (model.PriorRental)
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditDealerDemo":
                        if (model.DealerDemo)
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditUnwind":
                        if (model.Unwind)
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "CustomExtColor":

                        if (!String.IsNullOrEmpty(model.ExteriorColor))

                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"/>";

                        break;
                    case "CustomIntColor":

                        if (!String.IsNullOrEmpty(model.InteriorColor))
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"/>";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;

                    case "RetailPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "DealerDiscount":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "ManufacturerRebate":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;
                    case "SalePrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SalePrice + "\" />";
                        break;
                    case "Warranty":
                        if (model.WarrantyInfo == 0)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";


                        }
                        else if (model.WarrantyInfo == 1)
                        {
                            result +=
                                "<input type=\"radio\" id=\"warrantyasis\" checked=\"checked\" value=\"1\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";
                        }

                        else if (model.WarrantyInfo == 2)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 3)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantydealercertified\"  value=\"3\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 4)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }

                        break;


                    case "WindowStickerPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;

                    case "EditRetailPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "EditDealerDiscount":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "EditManufacturerRebate":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;

                    case "EditWindowStickerPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;
                    case "Odometer":
                        if ((model.Mileage == 0))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litter + "\" />";
                        break;
                    case "Tranmission":
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        if (String.IsNullOrEmpty(model.Tranmission))
                        {
                            result += "<option>Automatic</option>";
                            result += "<option>Manual</option>";
                        }
                        else
                        {
                            if (model.Tranmission.Equals("Automatic"))
                            {
                                result += "<option>Automatic</option>";
                                result += "<option>Manual</option>";
                            }
                            else
                            {
                                result += "<option>Manual</option>";
                                result += "<option>Automatic</option>";
                            }
                        }

                        result += "<option>" + "Other Tranmission" + "</option>";
                        result += "</select>";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<select class=\"z-index\" style=\"width:130px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        XmlNode driveNode = XMLHelper.selectOneElement("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"), "Value=" + model.WheelDrive);
                        XmlNodeList allNode = XMLHelper.selectElements("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"));
                        if (driveNode != null)
                        {
                            result += "<option>" + driveNode.Attributes["Value"].Value + "</option>";

                            foreach (XmlNode node in allNode)
                            {
                                if (!node.Attributes["Value"].Value.Equals(driveNode.Attributes["Value"].Value))
                                    result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        else
                        {
                            foreach (XmlNode node in allNode)
                            {
                                result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        result += "<option>" + "Other Drives" + "</option>";
                        result += "</select>";

                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Msrp + "\" />";
                        break;

                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;
                    case "hiddenListingId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\"" + " />"; ;
                        break;
                    case "hiddenDealershipId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerId + "\"" + " />"; ;
                        break;
                    case "Certified":
                        if (model.IsCertified)
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\" checked=\"checked\" />";
                        else
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\"/>";
                        break;

                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                }
            }

            return result;
        }
        public static string DynamicHtmlControlForBuyerGuide(this HtmlHelper<BuyerGuideViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            string result = "";

            switch (fieldName)
            {
                case "BackGroundImage":



                    if (model.PriorRental)
                        result += "<img src=\"" + urlHelper.Content("~/images/bGuideBG-prior-rental.jpg") + "\"/>";
                    else
                    {
                        result += "<img src=\"" + urlHelper.Content("~/images/bGuideBG.png") + "\"/>";
                    }
                    break;
                case "BackGroundImageInSpanish":


                    if (model.PriorRental)
                        result += "<img src=\"" + urlHelper.Content("~/images/bg-spanish-prior-rental.jpg") + "\"/>";
                    else
                    {
                        result += "<img src=\"" + urlHelper.Content("~/images/bGuideBG-spanish-resize.png") + "\"/>";
                    }
                    break;
                case "Warranty":


                    if (model.Warranty == 1)
                        result += "<span id=\"asIs\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                    else
                    {
                        result += "<span id=\"fullLimit\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                        if (model.Warranty == 3)
                            result += "<span id=\"limited\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></img></span>";
                    }
                    break;
                case "WarrantySpanish":


                    if (model.Warranty == 1)
                        result += "<span id=\"asIsSpanish\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                    else
                    {
                        result += "<span id=\"fullLimitSpanish\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                        if (model.Warranty == 3)
                            result += "<span id=\"limitedSpanish\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></img></span>";
                    }
                    break;
                case "WarrantySpanishBi":


                    if (model.Warranty == 1)
                        result += "<span id=\"asIsSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                    else
                    {
                        result += "<span id=\"fullLimitSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                        if (model.Warranty == 3)
                            result += "<span id=\"limitedSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></img></span>";
                    }
                    break;
                case "WarrantyInfo":
                    if (model.Warranty == 1 && !String.IsNullOrEmpty(model.AsWarranty))
                        result += "<p id=\"systems\"><img src=\"" + model.AsWarranty + "\"></img></p>";
                    if (model.Warranty == 2 && !String.IsNullOrEmpty(model.ManufacturerWarranty))
                        result += "<p id=\"systems\"><img src=\"" + model.ManufacturerWarranty + "\"></img></p>";
                    else if (model.Warranty == 3 && !String.IsNullOrEmpty(model.DealerCertified))
                        result += "<p id=\"systems\"><img src=\"" + model.DealerCertified + "\"></img></p>";
                    else if (model.Warranty == 4 && !String.IsNullOrEmpty(model.ManufacturerCertified))
                        result += "<p id=\"systems\"><img src=\"" + model.ManufacturerCertified + "\"></img></p>";

                    break;
                case "WarrantyInfoSpanish":
                    if (model.Warranty == 1 && !String.IsNullOrEmpty(model.AsWarrantyInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.AsWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 2 && !String.IsNullOrEmpty(model.ManufacturerWarrantyInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.ManufacturerWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 3 && !String.IsNullOrEmpty(model.DealerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.DealerCertifiedInSpanish + "\"></img></p>";
                    if (model.Warranty == 4 && !String.IsNullOrEmpty(model.ManufacturerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.ManufacturerCertifiedInSpanish + "\"></img></p>";

                    break;
                case "WarrantyInfoSpanishBi":
                    if (model.Warranty == 1 && !String.IsNullOrEmpty(model.AsWarrantyInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.AsWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 2 && !String.IsNullOrEmpty(model.ManufacturerWarrantyInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.ManufacturerWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 3 && !String.IsNullOrEmpty(model.DealerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.DealerCertifiedInSpanish + "\"></img></p>";
                    if (model.Warranty == 4 && !String.IsNullOrEmpty(model.ManufacturerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.ManufacturerCertifiedInSpanish + "\"></img></p>";

                    break;
                case "ServiceContract":
                    if (model.Warranty == 4)
                        result += "<span id=\"contract\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    else if (model.ServiceContract)
                        result += "<span id=\"contract\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    break;
                case "ServiceContractSpanish":
                    if (model.Warranty == 4)
                        result += "<span id=\"contractSpanish\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    else if (model.ServiceContract)
                        result += "<span id=\"contractSpanish\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";

                    break;
                case "ServiceContractSpanishBi":
                    if (model.Warranty == 4)
                        result += "<span id=\"contractSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    else if (model.ServiceContract)
                        result += "<span id=\"contractSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";

                    break;
                case "WarrantyFullLimit":
                    if (model.Warranty == 3)
                        result += " <span id=\"labor\">100*</span><span id=\"parts\">100*</span>";
                    else if (model.Warranty == 4)
                        result += " <span id=\"labor\">100*</span><span id=\"parts\">100*</span>";

                    break;
                case "WarrantyFullLimitSpanish":
                    if (model.Warranty == 3)
                        result += " <span id=\"laborSpanish\">100*</span><span id=\"partsSpanish\">100*</span>";
                    else if (model.Warranty == 4)
                        result += " <span id=\"laborSpanish\">100*</span><span id=\"partsSpanish\">100*</span>";

                    break;
                case "WarrantyFullLimitSpanishBi":
                    if (model.Warranty == 3)
                        result += " <span id=\"laborSpanishBi\">100*</span><span id=\"partsSpanish\">100*</span>";
                    else if (model.Warranty == 4)
                        result += " <span id=\"laborSpanishBi\">100*</span><span id=\"partsSpanish\">100*</span>";

                    break;
                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + " value=\"error\"" + " />";
                    break;
            }


            return result;
        }

        public static ExtendedSelectListItem CreateSelectListItem(string text, string value, bool selected)
        {
            var item = new ExtendedSelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selected
            };

            return item;
        }
        public static string DynamicHtmlControlDealerSwitch()
        {

            var dealerGroup = SessionHandler.DealerGroup;

            var dealer = SessionHandler.Dealer;

            var builder = new StringBuilder();

            if (dealerGroup == null || (dealerGroup.DealerList.Any() && dealerGroup.DealerList.Count == 1))
            {
                builder.AppendLine(" <select id=\"DDLDealer\">");

                builder.AppendLine("<option value=\"" + dealer.DealershipId + "\" selected=\"selected\">");
                
                builder.AppendLine(dealer.DealershipName);

                builder.AppendLine("</option>");
                                    
                builder.AppendLine(" </select>");

                return builder.ToString();

            }
            else
            {
                if (SessionHandler.AllStore == false)
                {
                    builder.AppendLine(" <select id=\"DDLDealer\">");

                    builder.AppendLine("<option value=\"" + dealer.DealershipId + "\" selected=\"selected\">");

                    builder.AppendLine(dealer.DealershipName);

                    builder.AppendLine("</option>");

                    foreach (var tmp in dealerGroup.DealerList.Where(x => x.DealershipId != dealer.DealershipId))
                    {

                        builder.AppendLine("<option value=\"" + tmp.DealershipId + "\">");

                        builder.AppendLine(tmp.DealershipName);

                        builder.AppendLine("</option>");
                    }

                    builder.AppendLine("<option value=\"" + dealerGroup.DealershipGroupId + "\">");

                    builder.AppendLine(dealerGroup.DealershipGroupName);

                    builder.AppendLine("</option>");

                    builder.AppendLine(" </select>");
                }
                else
                {
                    builder.AppendLine(" <select id=\"DDLDealer\">");

                    builder.AppendLine("<option value=\"" + dealerGroup.DealershipGroupId + "\" selected=\"selected\">");

                    builder.AppendLine(dealerGroup.DealershipGroupName);

                    foreach (var tmp in dealerGroup.DealerList)
                    {

                        builder.AppendLine("<option value=\"" + tmp.DealershipId + "\">");

                        builder.AppendLine(tmp.DealershipName);

                        builder.AppendLine("</option>");
                    }

                   

                    builder.AppendLine("</option>");

                    builder.AppendLine(" </select>");
                }

                

                return builder.ToString();
            }


        
        }

        public static MvcHtmlString ToJson(this HtmlHelper html, object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return MvcHtmlString.Create(serializer.Serialize(obj));
        }
        public static MvcHtmlString ToJson(this HtmlHelper html, object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return MvcHtmlString.Create(serializer.Serialize(obj));
        }
        public static string BuildInventory(HtmlHelper html, List<CarInfoFormViewModel> cars, bool isGrid)
        {
            Dictionary<string, object> _params = new Dictionary<string, object>();
            foreach (var car in cars)
            {
                var Url = new UrlHelper(html.ViewContext.RequestContext);

                if (car.Type == Constanst.CarInfoType.Wholesale)
                {
                    car.ClassFilter = "Wholesale";
                    car.URLDetail = Url.Action("ViewIProfile",
                                                     "Inventory", new { ListingID = car.ListingId });

                    car.URLEdit = Url.Action("ViewIProfile",
                                                     "Inventory", new { ListingID = car.ListingId });

                    car.UrlImage = ImageLinkHelper.ImageLink(html, "ViewIProfile", car.SinglePhoto, "Inventory",
                                                         new { ListingID = car.ListingId }, null,
                                                         new { Alt = "Image" });
                }
                else if (car.Type == Constanst.CarInfoType.Sold)
                {
                    car.ClassFilter = "Sold";
                    car.URLDetail = Url.Action("ViewISoldProfile",
                                                     "Inventory", new { ListingID = car.ListingId });
                    car.URLEdit = Url.Action("ViewISoldProfile",
                                                     "Inventory", new { ListingID = car.ListingId });

                    if (car.ListingId == 45133)
                    {
                        car.UrlImage = ImageLinkHelper.ImageLink(html, "ViewISoldProfile", car.SinglePhoto, "Inventory",
                                                         new { ListingID = car.ListingId }, null,
                                                         new { Alt = "Image" });
                    }
                    else
                    {
                        car.UrlImage = ImageLinkHelper.ImageLink(html, "ViewISoldProfile", car.SinglePhoto, "Inventory",
                                                         new { ListingID = car.ListingId }, null,
                                                         new { Alt = "Image" });
                    }
                }
                else if (car.Type == Constanst.CarInfoType.Appraisal)
                {
                    car.ClassFilter = "Appraisals";
                    car.URLDetail = Url.Action("EditAppraisal",
                                                     "Appraisal", new { appraisalId = car.ListingId });
                    car.URLEdit = Url.Action("EditAppraisal",
                                                     "Appraisal", new { appraisalId = car.ListingId });

                    car.UrlImage = ImageLinkHelper.ImageLink(html, "EditAppraisal", car.SinglePhoto, "Appraisal",
                                                         new { ListingID = car.ListingId }, null,
                                                         new { Alt = "Image" });
                }
                else
                {
                    if (car.Type == Constanst.CarInfoType.New)
                    {
                        car.ClassFilter = "New";
                    }
                    else if (car.Type == Constanst.CarInfoType.Used)
                    {
                        car.ClassFilter = "Used";
                    }
                    car.URLDetail = Url.Action("ViewIProfile",
                                                     "Inventory", new { ListingID = car.ListingId });
                    car.URLEdit = Url.Action("EditIProfile",
                                                 "Inventory", new { ListingID = car.ListingId });

                    car.UrlImage = ImageLinkHelper.ImageLink(html, "ViewIProfile", car.SinglePhoto, "Inventory",
                                                         new { ListingID = car.ListingId }, null,
                                                         new { Alt = "Image" });
                }

                if (car.CarRanking == 0 && car.NumberOfCar == 0)
                    car.MarketData = "NA";
                else
                    car.MarketData = string.Format("{0}/{1}", car.CarRanking, car.NumberOfCar);


                if (!String.IsNullOrEmpty(car.Make) && car.Make.Length > 10)
                    car.ShortMake = car.Make.Substring(0, 9);
                else
                {
                    car.ShortMake = car.Make;
                }

                if (!String.IsNullOrEmpty(car.Model) && car.Model.Length > 10)
                    car.ShortModel = car.Model.Substring(0, 9);
                else
                {
                    car.ShortModel = car.Model;
                }

                if (!String.IsNullOrEmpty(car.Vin) && car.Vin.Length > 8)
                    car.SortVin = car.Vin.Substring(car.Vin.Length - 8, 8);
                else
                {
                    car.SortVin = car.Vin;
                }

                if (!String.IsNullOrEmpty(car.Trim) && car.Trim.Length > 7)
                    car.StrTrim = car.Trim.Substring(0, 8);
                else
                    car.StrTrim = car.Trim;

                if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 6)
                    car.StrExteriorColor = car.ExteriorColor.Substring(0, 7);
                else
                    car.StrExteriorColor = car.ExteriorColor;

                if (car.CarFaxOwner < 1)
                    car.StrCarFaxOwner = "NA";
                else if (car.CarFaxOwner == 1)
                    car.StrCarFaxOwner = car.CarFaxOwner + " Owner";
                else
                    car.StrCarFaxOwner = car.CarFaxOwner + " Owners";

                if (car.DaysInInvenotry == 1)
                    car.StrDaysInInvenotry = car.DaysInInvenotry + " Day";
                else
                {
                    car.StrDaysInInvenotry = car.DaysInInvenotry + " Days";
                }

                car.URLEbay = Url.Content("~/Market/ViewEbay?ListingId=" + car.ListingId);

                car.URLBG = Url.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId);

                car.URLWS = Url.Content("~/PDF/PrintSticker?ListingId=" + car.ListingId);

                #region Mileage
                long odometerNumber = 0;
                bool odometerFlag = Int64.TryParse(car.Mileage.ToString(), out odometerNumber);
                if (odometerFlag)
                    car.StrMileage = odometerNumber.ToString("#,##0");
                else
                    car.StrMileage = car.Mileage.ToString();
                #endregion


                #region Price
                decimal salePriceNumber = 0;
                bool salePriceFlag = Decimal.TryParse(car.SalePrice.ToString(), out salePriceNumber);
                if (salePriceFlag)
                    car.StrSalePrice = salePriceNumber.ToString("#,##0");
                else
                    car.StrSalePrice = car.SalePrice.ToString();
                #endregion
            }
            _params.Add("Inventories", cars);

            string content = string.Empty;
            if (isGrid)
                content = NVelocityExtension.NVelocityHelper.GetHTMLTemplate(@"\Inventory\InventoryGrid.htm", _params).ToString();
            else
                content = NVelocityExtension.NVelocityHelper.GetHTMLTemplate(@"\Inventory\Inventory.htm", _params).ToString();

            return content;
        }
        public static string BuildAppraisal(HtmlHelper html, List<AppraisalViewFormModel> cars)
        {
            var _params = new Dictionary<string, object>();

            foreach (var car in cars)
            {
                var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

                if (car.IsPhotoFromVingenie)
                    car.URLImage = ImageLinkHelper.ImageLink(html, "ViewProfileForAppraisal",
                       car.PhotoUrl, "",
                                                             new { AppraisalId = car.AppraisalID }, null,
                                                             new { width = 47 });
                else
                {
                    car.URLImage = ImageLinkHelper.ImageLink(html, "ViewProfileForAppraisal", car.DefaultImageUrl,
                                                             "", new { AppraisalId = car.AppraisalID }, null,
                                                             new { width = 65, height = 61 });
                }

                car.URLDetail = urlHelper.Action("ViewProfileForAppraisal",
                                                     "Appraisal", new { AppraisalId = car.AppraisalID });

                if (!String.IsNullOrEmpty(car.Make) && car.Make.Length > 10)
                    car.ShortMake = car.Make.Substring(0, 9);
                else
                {
                    car.ShortMake = car.Make;
                }

                if (!String.IsNullOrEmpty(car.VinNumber) && car.VinNumber.Length > 6)
                    car.SortVin = car.VinNumber.Substring(0, 7);
                else
                {
                    car.SortVin = car.VinNumber;
                }

                if (!String.IsNullOrEmpty(car.Trim) && car.Trim.Length > 7)
                    car.StrTrim = car.Trim.Substring(0, 8);
                else
                    car.StrTrim = car.Trim;

                if (!String.IsNullOrEmpty(car.AppraisalModel) && car.AppraisalModel.Length > 7)
                    car.AppraisalModelSort = car.AppraisalModel.Substring(0, 8);
                else
                    car.AppraisalModelSort = car.AppraisalModel;

                if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 6)
                    car.StrExteriorColor = car.ExteriorColor.Substring(0, 7);
                else
                    car.StrExteriorColor = car.ExteriorColor;

                if (car.CarFax != null)
                {
                    if (car.CarFax.NumberofOwners == "-1")
                        car.StrCarFaxOwner = "NA";
                    else if (car.CarFax.NumberofOwners == "1")
                        car.StrCarFaxOwner = car.CarFax.NumberofOwners + " Owner";
                    else
                        car.StrCarFaxOwner = car.CarFax.NumberofOwners + " Owners";
                }
                else
                {
                    car.StrCarFaxOwner = "NA";
                }

                car.StrClientName = string.Format("{0} {1}", car.CustomerFirstName, car.CustomerLastName);

                #region Mileage
                long odometerNumber = 0;
                bool odometerFlag = Int64.TryParse(car.Mileage.ToString(), out odometerNumber);
                if (odometerFlag)
                    car.StrMileage = odometerNumber.ToString("#,##0");
                else
                    car.StrMileage = car.Mileage.ToString();
                #endregion

                #region Price
                decimal salePriceNumber = 0;
                bool salePriceFlag = Decimal.TryParse(car.ACV.ToString(), out salePriceNumber);
                if (salePriceFlag)
                    car.StrACV = salePriceNumber.ToString("#,##0");
                else
                    car.StrACV = car.ACV.HasValue ? car.ACV.Value.ToString() : "";
                #endregion
            }

            _params.Add("Appraisals", cars);
            string content = string.Empty;

            content = NVelocityExtension.NVelocityHelper.GetHTMLTemplate(@"\Appraisals\Appraisal.htm", _params).ToString();

            return content;
        }
        public static string GetShortDrive(string wheelDrive)
        {
            XmlNode driveNode = XMLHelper.selectOneElement("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"), "Value=" + wheelDrive);

            if (driveNode != null)

                return driveNode.Attributes["Short"].Value;

            return "Other";

        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>());
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, string optionLabel)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), optionLabel);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), optionLabel, htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, object htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), optionLabel, htmlAttributes);
        }
    }
}

public class CacheFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Gets or sets the cache duration in seconds. The default is 10 seconds.
    /// </summary>
    /// <value>The cache duration in seconds.</value>
    public int Duration
    {
        get;
        set;
    }

    public CacheFilterAttribute()
    {
        Duration = 15;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (Duration <= 0) return;

        HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
        TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

        cache.SetCacheability(HttpCacheability.Public);
        cache.SetExpires(DateTime.Now.Add(cacheDuration));
        cache.SetMaxAge(cacheDuration);
        cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
    }
}

public class CompressFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        HttpRequestBase request = filterContext.HttpContext.Request;

        string acceptEncoding = request.Headers["Accept-Encoding"];

        if (string.IsNullOrEmpty(acceptEncoding)) return;

        acceptEncoding = acceptEncoding.ToUpperInvariant();

        HttpResponseBase response = filterContext.HttpContext.Response;

        if (acceptEncoding.Contains("GZIP"))
        {
            response.AppendHeader("Content-encoding", "gzip");
            response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
        }
        else if (acceptEncoding.Contains("DEFLATE"))
        {
            response.AppendHeader("Content-encoding", "deflate");
            response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
        }
    }
}