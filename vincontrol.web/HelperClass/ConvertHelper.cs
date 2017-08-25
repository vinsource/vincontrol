using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.ChromeAutoService;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Models;
using Vincontrol.Web.Handlers;
using vincontrol.Helper;
using SelectListItem = vincontrol.DomainObject.ExtendedSelectListItem;

namespace Vincontrol.Web.HelperClass
{
    public sealed class ConvertHelper
    {
        #region Public Methods

        public static string ConvertArrayToString(string[] array)
        {
            var result = "";

            foreach (var newString in array.Select(tmp => tmp.Replace(",", "")))
            {
                result += newString + " ";
                break;
            }

            return result;
        }

        public static CarInfoFormViewModel GetVehicleInfoForEbay(int listingId)
        {
            var context = new VincontrolEntities();

            var row = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
            if (row != null)
            {
                return new CarInfoFormViewModel(row);
            }
            return null;
        }

        public static AppraisalViewFormModel GetVehicleInfoFromChromeDecode(VehicleDescription vehicleInfo)
        {
            var appraisal = new AppraisalViewFormModel
                                {
                                    VinDecodeSuccess = true,
                                    AppraisalDate = DateTime.Now.ToShortDateString(),
                                    VinNumber = vehicleInfo.vinDescription != null ? vehicleInfo.vinDescription.vin : string.Empty,
                                    AppraisalModel = vehicleInfo.bestModelName,
                                    Make = vehicleInfo.bestMakeName,
                                    SelectedModel = vehicleInfo.bestModelName,
                                    Trim = vehicleInfo.bestTrimName,
                                    ModelYear = vehicleInfo.modelYear,
                                    ExteriorColorList = SelectListHelper.InitalExteriorColorList(vehicleInfo.exteriorColor),
                                    InteriorColorList = SelectListHelper.InitalInteriorColorList(vehicleInfo.interiorColor)
                                };

            if (vehicleInfo.style != null && vehicleInfo.style.Any())
            {
                var firstStyle = vehicleInfo.style.FirstOrDefault();
                if (firstStyle != null)
                {
                    appraisal.Door = firstStyle.passDoors;
                    appraisal.MSRP =Convert.ToDecimal(firstStyle.basePrice.msrp);
                    appraisal.DriveTrainList = SelectListHelper.InitalDriveTrainList(firstStyle.drivetrain.ToString());
                    bool existed;
                    appraisal.TrimList = SelectListHelper.InitalTrimList(appraisal, firstStyle.trim, vehicleInfo.style, firstStyle.id, out existed);
                    if (firstStyle.stockImage != null)
                    {
                        appraisal.DefaultImageUrl = firstStyle.stockImage.url;
                        appraisal.FirstPhoto = firstStyle.stockImage.url;
                    }
                }
            }

            var chromeAutoService = new ChromeAutoService();
            var listPackageOptions = chromeAutoService.GetPackageOptions(vehicleInfo);
            var listNonInstalledOptions = chromeAutoService.GetNonInstalledOptions(vehicleInfo);
                      
            var builderOption = new StringBuilder();

            if (vehicleInfo != null && vehicleInfo.genericEquipment != null)
            {

                foreach (
                    var sd in
                        vehicleInfo.genericEquipment.Where(x => x.installed != null))
                {
                    var category = (CategoryDefinition)sd.Item;
                    builderOption.Append(category.category.Value + ",");

                }

                if (String.IsNullOrEmpty(builderOption.ToString()))
                    appraisal.StandardInstalledOption = "";
                else
                {
                    builderOption.Remove(builderOption.Length - 1, 1);
                    appraisal.StandardInstalledOption = builderOption.ToString().Replace("\'", "\\'");
                }

            }

            appraisal.FactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(listPackageOptions);
            
            appraisal.FactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(listNonInstalledOptions);

            if (vehicleInfo.vinDescription != null && !String.IsNullOrEmpty(vehicleInfo.vinDescription.bodyType))
            {
                appraisal.BodyTypeList = SelectListHelper.InitialBodyTypeList(vehicleInfo.vinDescription.bodyType);
            }
            else
            {
                var bodyType = vehicleInfo.style.Last().bodyType;
                appraisal.BodyTypeList = SelectListHelper.InitialBodyTypeList(vehicleInfo.style != null ? (bodyType != null ? bodyType.Last().Value : vehicleInfo.bestStyleName) : vehicleInfo.bestStyleName);
                if (appraisal.CylinderList == null)
                {
                    appraisal.CylinderList = new BindingList<SelectListItem>();
                }

                if (appraisal.LitersList == null)
                {
                    appraisal.LitersList = new BindingList<SelectListItem>();
                }

                if (appraisal.FuelList == null)
                {
                    appraisal.FuelList = new BindingList<SelectListItem>();
                }
            }

            if (vehicleInfo.engine != null)
            {
                appraisal.FuelList = SelectListHelper.InitialFuelList(vehicleInfo.engine);
                appraisal.CylinderList = SelectListHelper.InitialCylinderList(vehicleInfo.engine);
                appraisal.LitersList = SelectListHelper.InitialLitterList(vehicleInfo.engine);

                var firstEngine = vehicleInfo.engine.FirstOrDefault();
                if (firstEngine != null && firstEngine.fuelEconomy != null)
                {
                    appraisal.FuelEconomyCity = firstEngine.fuelEconomy.city.low.ToString();
                    appraisal.FuelEconomyHighWay = firstEngine.fuelEconomy.hwy.low.ToString();
                }
            }

            if (vehicleInfo.vinDescription != null && vehicleInfo.vinDescription.marketClass != null)
            {
                if (vehicleInfo.vinDescription.marketClass.Any(tmp => tmp.Value.Contains("Truck") || tmp.Value.Contains("Cargo Vans")))
                {
                    appraisal.IsTruck = true;
                }
            }

            return appraisal;
        }

        public static AppraisalViewFormModel GetVehicleInfoFromChromeDecodeWithStyle(VehicleDescription vehicleInfo, VehicleDescription styleInfo)
        {
            var car = GetVehicleInfoFromChromeDecode(vehicleInfo);

            if (styleInfo != null)
            {
                if (styleInfo.exteriorColor != null && styleInfo.exteriorColor.Any())
                    car.ExteriorColorListForEdit = styleInfo.exteriorColor.GroupBy(x => x.colorName).Select(y => y.First()).ToList();
                else
                {
                    car.ExteriorColorListForEdit = new List<Color>();
                }

                if (styleInfo.interiorColor != null && styleInfo.interiorColor.Any())
                    car.InteriorColorListForEdit = styleInfo.interiorColor.GroupBy(x => x.colorName).Select(y => y.First()).ToList();
                else
                {
                    car.InteriorColorListForEdit = new List<Color>();
                }

                if (SessionHandler.ChromeTrimList != null)
                {
                    car.TrimListEdit = SessionHandler.ChromeTrimList.Select(x => x.Text).Where(p => !String.IsNullOrEmpty(p)).Distinct().ToList();
                    car.TrimList = SessionHandler.ChromeTrimList;
                    foreach (var item in car.TrimList)
                    {
                        item.Selected = item.Text.Equals(styleInfo.style[0].trim);
                    }
                }
                else
                {
                    if (vehicleInfo.bestMakeName.Equals("Mercedes-Benz") && vehicleInfo.modelYear <= 2009)
                    {
                        car.TrimListEdit = vehicleInfo.style.Select(x => x.mfrModelCode).Where(p => !String.IsNullOrEmpty(p)).Distinct().ToList();
                        bool existed;
                        car.TrimList = SelectListHelper.InitalTrimListForMercedesBenz(car, styleInfo.style[0].mfrModelCode, vehicleInfo.style, styleInfo.style[0].id, out existed);
                        SessionHandler.ChromeTrimList = car.TrimList;
                    }
                    else
                    {

                        car.TrimListEdit = vehicleInfo.style.Select(x => x.trim).Where(p => !String.IsNullOrEmpty(p)).Distinct().ToList();
                        bool existed;
                        car.TrimList = SelectListHelper.InitalTrimList(car, styleInfo.style[0].trim, vehicleInfo.style, styleInfo.style[0].id, out existed);
                        SessionHandler.ChromeTrimList = car.TrimList;
                    }

                }

                var chromeAutoService = new ChromeAutoService();
                var listPackageOptions = chromeAutoService.GetPackageOptions(styleInfo);
                var listNonInstalledOptions = chromeAutoService.GetNonInstalledOptions(styleInfo);
                                car.FactoryPackageOptionsEdit = listPackageOptions;
                car.FactoryNonInstalledOptionsEdit = listNonInstalledOptions;
                
            }

            return car;
        }

        public static CarInfoFormViewModel GetVehicleInfoFromChromeDecodeForEdit(VehicleDescription vehicleInfo)
        {
            var car = new CarInfoFormViewModel
            {
                Make = vehicleInfo.bestMakeName,
                Trim = vehicleInfo.bestTrimName,
                ModelYear = vehicleInfo.modelYear,
                MechanicalList = new List<String>(),
                ExteriorList = new List<String>(),
                EntertainmentList = new List<String>(),
                InteriorList = new List<String>(),
                SafetyList = new List<String>(),
            };

            if (vehicleInfo.style != null && vehicleInfo.style.Any())
            {
                car.Msrp = Convert.ToDecimal(vehicleInfo.style.First().basePrice.msrp);
                if (vehicleInfo.style.First().stockImage != null)
                    car.DefaultImageUrl = vehicleInfo.style.First().stockImage.url;
            }

            var chromeAutoService = new ChromeAutoService();
            var listPackageOptions = chromeAutoService.GetPackageOptions(vehicleInfo);
            var listNonInstalledOptions = chromeAutoService.GetNonInstalledOptions(vehicleInfo);

            car.FactoryPackageOptions = listPackageOptions;
            car.FactoryNonInstalledOptions = listNonInstalledOptions;

            return car;
        }

        public static CarInfoFormViewModel GetVehicleInfoFromChromeDecodeWithStyleForEdit(VehicleDescription vehicleInfo, VehicleDescription styleInfo)
        {
            var car = GetVehicleInfoFromChromeDecodeForEdit(vehicleInfo);

            if (styleInfo != null)
            {
                car.Msrp = Convert.ToDecimal(vehicleInfo.basePrice.msrp.high);

                if (styleInfo.exteriorColor != null && styleInfo.exteriorColor.Any())
                    car.ExteriorColorList = styleInfo.exteriorColor.GroupBy(x => x.colorName).Select(y => y.First()).ToList();
                else
                {
                    car.ExteriorColorList = new List<Color>();
                }

                if (styleInfo.interiorColor != null && styleInfo.interiorColor.Any())
                    car.InteriorColorList = styleInfo.interiorColor.GroupBy(x => x.colorName).Select(y => y.First()).ToList();
                else
                {
                    car.InteriorColorList = new List<Color>();
                }

                if (styleInfo.exteriorColor != null && styleInfo.exteriorColor.Any())
                    car.ChromeExteriorColorList = SelectListHelper.InitalExteriorColorList(styleInfo.exteriorColor).ToList();
                else
                {
                    car.ChromeExteriorColorList = new List<SelectListItem>().AsEnumerable();
                }

                if (styleInfo.interiorColor != null && styleInfo.interiorColor.Any())
                    car.ChromeInteriorColorList = SelectListHelper.InitalExteriorColorList(styleInfo.interiorColor);
                else
                {
                    car.ChromeInteriorColorList = new List<SelectListItem>().AsEnumerable();
                }

                car.EditTrimList = SelectListHelper.InitalTrimList(styleInfo.style);

                
                if (styleInfo.engine != null)
                {
                    var firstEngine = vehicleInfo.engine.FirstOrDefault();
                    if (firstEngine != null && firstEngine.fuelEconomy != null)
                    {
                        car.FuelEconomyCity = firstEngine.fuelEconomy.city.low.ToString();
                        car.FuelEconomyHighWay = firstEngine.fuelEconomy.hwy.low.ToString();
                    }
                }

                if (styleInfo.vinDescription != null && styleInfo.vinDescription.marketClass != null)
                {
                    if (styleInfo.vinDescription.marketClass.Any(tmp => tmp.Value.Contains("Truck") || tmp.Value.Contains("Cargo Vans")))
                    {
                        car.IsTruck = true;
                    }
                }
            }

            return car;
        }

        public static AppraisalViewFormModel GetAppraisalModelFromAppriaslId(int appraisalId)
        {
            var context = new VincontrolEntities();
            var row = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

            return row == null ? new AppraisalViewFormModel() : new AppraisalViewFormModel(row);
        }
     

        public static AppraisalViewFormModel GetAppraisalModel(Appraisal apprasial)
        {
            return apprasial == null ? new AppraisalViewFormModel() : new AppraisalViewFormModel(apprasial);
        }

        public static AppraisalViewFormModel UpdateSuccessfulAppraisalModel(AppraisalViewFormModel viewModel, AppraisalViewFormModel row, VehicleDescription vehicleInfo, int dealershipId, string location, bool decodeSuccessfully)
        {
            viewModel.AppraisalGenerateId = row.AppraisalID.ToString();
            viewModel.FactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptionsEdit);
            viewModel.FactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptionsEdit);
            viewModel.SalePrice = row.SalePrice;
            viewModel.DealerCost = row.DealerCost;
            viewModel.ACV = row.ACV;
             
            viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.SelectedModel;

            if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                viewModel.OrginalName += " " + viewModel.Trim;

            viewModel.Mileage = row.Mileage;

            viewModel.DefaultImageUrl = row.DefaultImageUrl ?? string.Empty;

            viewModel.Descriptions = row.Descriptions ?? string.Empty;

            viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeList(row.VehicleType);

            viewModel.DealershipId = dealershipId;

            viewModel.StockNumber = row.StockNumber ?? string.Empty;

            viewModel.VinNumber = row.VinNumber ?? string.Empty;

            viewModel.ModelYear = row.ModelYear;

            viewModel.Make = row.Make ?? string.Empty;

            viewModel.SelectedModel = row.AppraisalModel ?? string.Empty;

            viewModel.AppraisalModel = row.AppraisalModel ?? string.Empty;

            viewModel.AppraisalDate = row.AppraisalDate ?? DateTime.Now.ToString("MM/dd/yyyy");

            viewModel.Location = location;

            viewModel.Trim = row.SelectedTrim ?? string.Empty;

            viewModel.SelectedTrim = row.SelectedTrim ?? string.Empty;

            viewModel.ChromeStyleId = row.ChromeStyleId;
            viewModel.ChromeModelId = row.ChromeModelId;
            if (SessionHandler.ChromeTrimList == null)
            {
                if (vehicleInfo != null)
                {
                    int styleId;
                    if (viewModel.ChromeStyleId != null && Int32.TryParse(viewModel.ChromeStyleId, out styleId))
                    {
                        bool existed;
                        viewModel.TrimList = SelectListHelper.InitalTrimList(viewModel, viewModel.Trim,
                            vehicleInfo.style, styleId, out existed);
                        if (!existed)
                        {
                            viewModel.CusTrim = viewModel.Trim;
                        }
                    }
                    else if (!String.IsNullOrEmpty(viewModel.Trim))
                    {
                        bool existed;
                        viewModel.TrimList = SelectListHelper.InitalTrimList(viewModel, vehicleInfo.style,
                            viewModel.Trim, out existed);
                        if (!existed)
                        {
                            viewModel.CusTrim = viewModel.Trim;
                        }
                    }
                    else
                    {
                        viewModel.TrimList = SelectListHelper.InitalTrimList(vehicleInfo.style);
                    }
                }
            }
            else
            {
                foreach (var item in viewModel.TrimList)
                {
                    item.Selected = item.Text.Equals(row.Trim);
                }

                var selectedTrim = viewModel.TrimList.FirstOrDefault(i => i.Selected);
                if (selectedTrim != null)
                {
                    viewModel.Trim = selectedTrim.Text;
                    viewModel.SelectedTrimItem = selectedTrim.Value;
                }
                else
                {
                    selectedTrim = viewModel.TrimList.FirstOrDefault(i => i.Text.Equals("Base/Other Trims"));
                    viewModel.Trim = selectedTrim.Text;
                    viewModel.SelectedTrimItem = selectedTrim.Value;
                    viewModel.CusTrim = row.Trim;
                }
            }

            viewModel.SelectedExteriorColorCode = row.SelectedExteriorColorCode ?? string.Empty;
            viewModel.SelectedExteriorColorValue = row.SelectedExteriorColorValue ?? string.Empty;
            viewModel.SelectedInteriorColor = row.SelectedInteriorColor ?? string.Empty;

            viewModel.ExteriorColorList = viewModel.ExteriorColorListForEdit != null && viewModel.ExteriorColorListForEdit.Any()
                                              ? SelectListHelper.InitalExteriorColorList(viewModel.ExteriorColorListForEdit.ToArray(), viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim())
                                              : SelectListHelper.InitalExteriorColorList(null, viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

            viewModel.InteriorColorList = viewModel.InteriorColorListForEdit != null && viewModel.InteriorColorListForEdit.Any()
                                              ? SelectListHelper.InitalInteriorColorList(viewModel.InteriorColorListForEdit.ToArray(), viewModel.SelectedInteriorColor)
                                              : SelectListHelper.InitalInteriorColorList(null, viewModel.SelectedInteriorColor);

            if (viewModel.ExteriorColorListForEdit != null && viewModel.ExteriorColorListForEdit.Any())
            {
                var list = viewModel.ExteriorColorListForEdit.Where(t => t.colorName.Equals(viewModel.SelectedExteriorColorValue.Trim()));
                if (!list.Any())
                {
                    viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;

                }
                else
                    viewModel.CusExteriorColor = string.Empty;

            }
            else
                viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;

            if (viewModel.InteriorColorListForEdit != null && viewModel.InteriorColorListForEdit.Any())
            {
                var list = viewModel.InteriorColorListForEdit.Where(t => t.colorName.Equals(viewModel.SelectedInteriorColor));
                if (!list.Any())
                {
                    viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;
                }
                else
                    viewModel.CusInteriorColor = string.Empty;

            }
            else
                viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;

            viewModel.DriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

            viewModel.ExistOptions = String.IsNullOrEmpty(row.SelectedFactoryOptions)
                                         ? null
                                         : (from data in CommonHelper.GetArrayFromString(row.SelectedFactoryOptions) select data).ToList();

            viewModel.ExistPackages = String.IsNullOrEmpty(row.SelectedPackageOptions)
                                          ? null
                                          : (from data in CommonHelper.GetArrayFromString(row.SelectedPackageOptions) select data).ToList();

            viewModel.VehicleType = row.VehicleType;

            viewModel.SelectedVehicleType = row.SelectedVehicleType;
            
            viewModel.Notes = row.Notes;

            viewModel.MSRP = row.MSRP;

            viewModel.CustomerFirstName = row.CustomerFirstName ?? string.Empty;

            viewModel.CustomerLastName = row.CustomerLastName ?? string.Empty;

            viewModel.CustomerAddress = row.CustomerAddress ?? string.Empty;

            viewModel.CustomerCity = row.CustomerCity ?? string.Empty;

            viewModel.CustomerState = row.CustomerState ?? string.Empty;

            viewModel.CustomerZipCode = row.CustomerZipCode ?? string.Empty;

            viewModel.Door = row.Door;

            viewModel.SelectedBodyType = row.SelectedBodyType ?? string.Empty;

            viewModel.SelectedCylinder = row.SelectedCylinder;

            viewModel.SelectedDriveTrain = row.SelectedDriveTrain ?? string.Empty;

            viewModel.SelectedTranmission = row.SelectedTranmission ?? string.Empty;

            viewModel.SelectedLiters = row.SelectedLiters;

            viewModel.SelectedFuel = row.SelectedFuel ?? string.Empty;

            viewModel.VinDecodeSuccess = decodeSuccessfully;

            viewModel.FirstPhoto = row.FirstPhoto;
            return viewModel;
        }

        public static AppraisalViewFormModel UpdateSuccessfulAppraisalModelWithoutVin(AppraisalViewFormModel viewModel, AppraisalViewFormModel row, VehicleDescription vehicleInfo, int dealershipId, string location, bool decodeSuccessfully)
        {
            viewModel.AppraisalGenerateId = row.AppraisalID.ToString();

            viewModel.SalePrice = row.SalePrice;

            viewModel.DealerCost = row.DealerCost;

            viewModel.ACV = row.ACV;

            viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.AppraisalModel;

            if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                viewModel.OrginalName += " " + viewModel.Trim;

            viewModel.Mileage = row.Mileage;

            viewModel.DefaultImageUrl = row.DefaultImageUrl ?? string.Empty;

            viewModel.Descriptions = row.Descriptions ?? string.Empty;

            viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeList(row.VehicleType);

            viewModel.DealershipId = dealershipId;

            viewModel.StockNumber = row.StockNumber ?? string.Empty;

            viewModel.VinNumber = row.VinNumber ?? string.Empty;

            viewModel.ModelYear = row.ModelYear;

            viewModel.Make = row.Make ?? string.Empty;

            viewModel.SelectedModel = row.AppraisalModel ?? string.Empty;

            viewModel.AppraisalModel = row.AppraisalModel ?? string.Empty;

            viewModel.AppraisalDate = row.AppraisalDate ?? DateTime.Now.ToString("MM/dd/yyyy");

            viewModel.Location = location;

            viewModel.Trim = row.SelectedTrim;

            viewModel.ChromeStyleId = row.ChromeStyleId;
            //int styleId;
            //if (viewModel.ChromeStyleId != null && Int32.TryParse(viewModel.ChromeStyleId, out styleId))
            //{
            //    bool existed;
            //    viewModel.TrimList = SelectListHelper.InitalTrimList(viewModel, viewModel.Trim, vehicleInfo.style, styleId, out existed);
            //    if (!existed)
            //    {
            //        viewModel.CusTrim = viewModel.Trim;
            //    }
            //}
            //else if (!String.IsNullOrEmpty(viewModel.Trim))
            //{
            //    bool existed;
            //    viewModel.TrimList = SelectListHelper.InitalTrimList(viewModel, vehicleInfo.style, viewModel.Trim, out existed);
            //    if (!existed)
            //    {
            //        viewModel.CusTrim = viewModel.Trim;
            //    }
            //}
            //else
            //{
            //    viewModel.TrimList = SelectListHelper.InitalTrimList(vehicleInfo.style);
            //}

            viewModel.BodyTypeList = SelectListHelper.InitialBodyTypeList(row.SelectedBodyType);

            viewModel.SelectedExteriorColorCode = row.SelectedExteriorColorCode ?? string.Empty;
            viewModel.SelectedExteriorColorValue = row.SelectedExteriorColorValue ?? string.Empty;
            viewModel.SelectedInteriorColor = row.SelectedInteriorColor ?? string.Empty;

            // Other exterior color
            viewModel.CusExteriorColor = String.IsNullOrEmpty(viewModel.SelectedExteriorColorCode) ? viewModel.SelectedExteriorColorValue : String.Empty;

            // Other interior color
            if (viewModel.InteriorColorList != null && viewModel.InteriorColorList.Any())
            {
                var list = viewModel.InteriorColorList.Where(t => t.Text.Equals(viewModel.SelectedInteriorColor));
                if (!list.Any())
                {
                    viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;
                }
                else
                    viewModel.CusInteriorColor = string.Empty;
            }
            else
                viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;

            viewModel.DriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

            viewModel.ExistOptions = String.IsNullOrEmpty(row.SelectedFactoryOptions)
                                         ? null
                                         : (from data in CommonHelper.GetArrayFromString(row.SelectedFactoryOptions) select data).ToList();

            viewModel.ExistPackages = String.IsNullOrEmpty(row.SelectedPackageOptions)
                                          ? null
                                          : (from data in CommonHelper.GetArrayFromString(row.SelectedPackageOptions) select data).ToList();

            viewModel.VehicleType = row.VehicleType;

            viewModel.Notes = row.Notes;

            viewModel.MSRP = row.MSRP;

            viewModel.CustomerFirstName = row.CustomerFirstName ?? string.Empty;

            viewModel.CustomerLastName = row.CustomerLastName ?? string.Empty;

            viewModel.CustomerAddress = row.CustomerAddress ?? string.Empty;

            viewModel.CustomerCity = row.CustomerCity ?? string.Empty;

            viewModel.CustomerState = row.CustomerState ?? string.Empty;

            viewModel.CustomerZipCode = row.CustomerZipCode ?? string.Empty;

            viewModel.Door = row.Door ;

            viewModel.SelectedBodyType = row.SelectedBodyType ?? string.Empty;

            viewModel.SelectedCylinder = row.SelectedCylinder;

            viewModel.SelectedDriveTrain = row.SelectedDriveTrain ?? string.Empty;

            viewModel.SelectedTranmission = row.SelectedTranmission ?? string.Empty;

            viewModel.SelectedLiters = row.SelectedLiters ;

            viewModel.SelectedFuel = row.SelectedFuel ?? string.Empty;

            viewModel.VinDecodeSuccess = decodeSuccessfully;

            return viewModel;
        }

        public static AppraisalViewFormModel UpdateAppraisalBeforeSaving(FormCollection form, AppraisalViewFormModel appraisal, DealershipViewModel dealer, string userName, int userID)
        {
            
            if (!String.IsNullOrEmpty(appraisal.SelectedTrim) && appraisal.SelectedTrim.Contains("|"))
            {
                var result = appraisal.SelectedTrim.Split('|');
                if (result.Count() > 1)
                {
                    appraisal.ChromeStyleId = result[0];
                    var trim = result[1];
                    appraisal.SelectedTrim = trim.Equals("Base/Other Trims") ? (appraisal.CusTrim ?? String.Empty) : trim;
                }
            }
            else if (!String.IsNullOrEmpty(appraisal.CusTrim))
            {
                appraisal.SelectedTrim = appraisal.CusTrim;
                appraisal.ChromeStyleId = null;
            }

            appraisal.SelectedPackageOptions = appraisal.AfterSelectedPackage;

            appraisal.SelectedFactoryOptions = appraisal.AfterSelectedOptions;

            appraisal.Title = appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.AppraisalModel + (!String.IsNullOrEmpty(appraisal.SelectedTrim) ? " " + appraisal.SelectedTrim : "");

            //appraisal.SalePrice = appraisal.MSRP;
            appraisal.SalePrice = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(form["MSRP"]));

            //appraisal.MSRP =appraisal.MSRP;
            appraisal.MSRP = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(form["MSRP"]));

            appraisal.AppraisalBy = userName;

            appraisal.AppraisalById = userID;

            //appraisal.Mileage = appraisal.Mileage ;
            appraisal.Mileage = Convert.ToInt64(CommonHelper.RemoveSpecialCharactersForMsrp(form["Mileage"]));

            appraisal.AppraisalModel = appraisal.SelectedModel;

            appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;

            var _carFaxService = new CarFaxService();
            try
            {
                appraisal.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);
            }
            catch (Exception)
            {
                appraisal.CarFax = new CarFaxViewModel()
                                       {
                                           Success = false
                                       };
            }

            appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt64(appraisal.Mileage), dealer.State);

            if (!String.IsNullOrEmpty(appraisal.SelectedTrim) &&
                appraisal.SelectedTrim.Trim().Equals("Base/Other Trims") &&
                !String.IsNullOrEmpty(appraisal.CusExteriorColor))
            {
                appraisal.SelectedTrim = appraisal.CusTrim;
            }

            if (!String.IsNullOrEmpty(appraisal.SelectedExteriorColorValue) &&
                appraisal.SelectedExteriorColorValue.Trim().Equals("Other Colors") &&
                !String.IsNullOrEmpty(appraisal.CusExteriorColor))
            {
                appraisal.SelectedExteriorColorValue = appraisal.CusExteriorColor;
            }

            if (!String.IsNullOrEmpty(appraisal.SelectedInteriorColor) &&
                appraisal.SelectedInteriorColor.Equals("Other Colors") &&
                !String.IsNullOrEmpty(appraisal.CusInteriorColor))
            {
                appraisal.SelectedInteriorColor = appraisal.CusInteriorColor;
            }

            return appraisal;
        }

        public static AppraisalViewFormModel ConvertDataRowToCustomerApppraisal(int customerId, DealershipViewModel dealer)
        {
            var context = new VincontrolEntities();
            var row = context.TradeInCustomers.FirstOrDefault(x => x.TradeInCustomerId == customerId);

            AppraisalViewFormModel appraisal = new AppraisalViewFormModel()
            {
                AppraisalID = row.TradeInCustomerId,
                //AppraisalGenerateId = row.AppraisalID,
                //Notes = row.Note,
                ModelYear = Convert.ToInt32(row.Year),
                Make = String.IsNullOrEmpty(row.Make) ? "" : row.Make,
                AppraisalModel = String.IsNullOrEmpty(row.Model) ? "" : row.Model,
                SelectedTrim = String.IsNullOrEmpty(row.Trim) ? "" : row.Trim,
                VinNumber = String.IsNullOrEmpty(row.Vin) ? "" : row.Vin,
                //StockNumber = String.IsNullOrEmpty(row.StockNumber) ? "" : row.StockNumber,
                //SalePrice = String.IsNullOrEmpty(row.SalePrice) ? "" : row.SalePrice,
                //MSRP = String.IsNullOrEmpty(row.MSRP) ? "" : row.MSRP,
                Mileage =row.Mileage.GetValueOrDefault(),
                //SelectedCylinder = String.IsNullOrEmpty(row.Cylinders) ? "" : row.Cylinders,
                //SelectedExteriorColor = String.IsNullOrEmpty(row.ExteriorColor) ? "" : row.ExteriorColor,
                //SelectedInteriorColor = String.IsNullOrEmpty(row.InteriorColor) ? "" : row.InteriorColor,
                //SelectedBodyType = String.IsNullOrEmpty(row.BodyType) ? "" : row.BodyType,
                //EngineType = String.IsNullOrEmpty(row.EngineType) ? "" : row.EngineType,
                //SelectedDriveTrain = String.IsNullOrEmpty(rowk.DriveTrain) ? "" : row.DriveTrain,
                //SelectedFuel = String.IsNullOrEmpty(row.FuelType) ? "" : row.FuelType,
                //SelectedTranmission = String.IsNullOrEmpty(row.Tranmission) ? "" : row.Tranmission,
                //Door = String.IsNullOrEmpty(row.Doors) ? "" : row.Doors,
                //SelectedLiters = String.IsNullOrEmpty(row.Liters) ? "" : row.Liters,
                //SelectedFactoryOptions = String.IsNullOrEmpty(row.CarsOptions) ? "" : row.CarsOptions,
                //SelectedPackageOptions = String.IsNullOrEmpty(row.CarsPackages) ? "" : row.CarsPackages,
                //Descriptions = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions,
                //Notes = String.IsNullOrEmpty(row.Note) ? "" : row.Note,
                //DealerCost = String.IsNullOrEmpty(row.DealerCost) ? "" : row.DealerCost,
                //DefaultImageUrl = String.IsNullOrEmpty(row.DefaultImageUrl) ? "" : row.DefaultImageUrl,
                //DateOfAppraisal = String.IsNullOrEmpty(row.AppraisalDate.Value.ToShortDateString())
                //                                ? DateTime.Now
                //                                : row.AppraisalDate.Value,
                CustomerFirstName = String.IsNullOrEmpty(row.FirstName) ? "" : row.FirstName,
                CustomerLastName = String.IsNullOrEmpty(row.LastName) ? "" : row.LastName,
                //ACV = row.TradeInFairValue
                //CustomerAddress = String.IsNullOrEmpty(row.Address) ? "" : row.Address,
                //CustomerCity = String.IsNullOrEmpty(row.City) ? "" : row.City,
                //CustomerState = String.IsNullOrEmpty(row.State) ? "" : row.State,
                //CustomerZipCode = String.IsNullOrEmpty(row.ZipCode) ? "" : row.ZipCode,
                //AppraisalBy = String.IsNullOrEmpty(row.AppraisalBy) ? "" : row.AppraisalBy,
                //AppraisalType = String.IsNullOrEmpty(row.AppraisalType) ? "" : row.AppraisalType,
                //ChromeModelId = String.IsNullOrEmpty(row.ChromeModelId) ? "" : row.ChromeModelId,
                //ChromeStyleId = String.IsNullOrEmpty(row.ChromeStyleId) ? "" : row.ChromeStyleId,
                //StandardOptions = String.IsNullOrEmpty(row.StandardOptions) ? "" : row.StandardOptions,
                //IsCertified = row.Certified.GetValueOrDefault(),
                //VehicleType = row.VehicleType,
                //TruckCategory = row.TruckCategory,
                //TruckType = row.TruckType,
                //TruckClass = row.TruckClass,          
                //   KbbTrimId = (row.KBBTrimId != null)?row.KBBTrimId.Value:0,
                //KbbOptionsId = row.KBBOptionsId
            };
            var carFaxService = new CarFaxService();
            appraisal.CarFax = carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);

            appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt64(appraisal.Mileage), dealer.State);

            appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;

            return appraisal;

            //int ACV = 0;

            //bool ACVFlag = Int32.TryParse(appraisal.ACV, out ACV);

            //if (ACVFlag)
            //    appraisal.ACV = ACV.ToString("#,##0");            
        }

        #endregion

        #region Private Methods
        #endregion

    }
}
