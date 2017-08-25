using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.Mvc;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.DomainObject;
using Color = vincontrol.ChromeAutoService.AutomativeService.Color;
using IdentifiedString = vincontrol.ChromeAutoService.AutomativeService.IdentifiedString;
using SelectListItem = vincontrol.DomainObject.ExtendedSelectListItem;

namespace vincontrol.ChromeAutoService
{
    public class ChromeAutoService
    {
        private readonly AccountInfo _mAi;
        private const string BaseOtherTrim = "Base/Other Trims";
        private const string OtherColors = "Other Colors";

        private readonly Description7aPortTypeClient _vinService = new Description7aPortTypeClient();
        
        public ChromeAutoService()
        {
            _mAi = new AccountInfo
            {
                number = System.Configuration.ConfigurationManager.AppSettings["accountNumber"],
                secret = System.Configuration.ConfigurationManager.AppSettings["accountSecret"],
                country = System.Configuration.ConfigurationManager.AppSettings["country"],
                language = System.Configuration.ConfigurationManager.AppSettings["language"]
            };
        }

        #region Automative Service API
        public int[] GetModelYears()
        {
            var baseReuqest = new BaseRequest { accountInfo = this.GetAccountInfo() };

            var temp = _vinService.getModelYears(baseReuqest);

            if (temp != null && temp.modelYear.Any())
                return temp.modelYear;

            return null;
        }

        public AccountInfo GetAccountInfo()
        {
            return this._mAi;
        }

        public IdentifiedString[] GetDivisions(int mModelYear)
        {
            var mDr = new DivisionsRequest { accountInfo = this.GetAccountInfo(), modelYear = mModelYear };

            var temp = _vinService.getDivisions(mDr);

            if (temp != null && temp.division.Any())
                return temp.division;

            return null;
        }

        public IdentifiedString[] GetSubdivisions(int mModelYear)
        {
            var mSdr = new SubdivisionsRequest { accountInfo = this.GetAccountInfo(), modelYear = mModelYear };

            var temp = _vinService.getSubdivisions(mSdr);

            if (temp != null && temp.subdivision.Any())
                return temp.subdivision;

            return null;
        }

        public IdentifiedString[] GetModelsByDivision(int mModelYear, int mDivisionId)
        {
            var mMbdr = new ModelsRequest
            {
                accountInfo = this.GetAccountInfo(),
                modelYear = mModelYear,
                Item = mDivisionId
            };

            var temp = _vinService.getModels(mMbdr);

            if (temp != null && temp.model != null && temp.model.Any())
                return temp.model;

            return null;
        }

        public IdentifiedString[] GetModelsByDivision(int mModelYear, string mDivisionName)
        {
            var mMbdr = new ModelsRequest { accountInfo = this.GetAccountInfo(), modelYear = mModelYear };

            bool flag = false;

            foreach (var dv in GetDivisions(mModelYear).Where(dv => dv.Value.Equals(mDivisionName)))
            {
                mMbdr.Item = dv.id;
                flag = true;
                break;
            }

            if (flag)
            {
                var temp = _vinService.getModels(mMbdr);
                if (temp != null && temp.model.Any())
                    return temp.model;

                return null;
            }

            return null;
        }

        public IdentifiedString[] GetModelsBySubDivision(int mSubdivisionId)
        {
            var mMbsdr = new ModelsRequest { accountInfo = this.GetAccountInfo(), Item = mSubdivisionId };

            var temp = _vinService.getModels(mMbsdr);
            if (temp != null && temp.model.Any())
                return temp.model;

            return null;
        }

        public IdentifiedString[] GetStyles(int mModelId)
        {
            var mSr = new StylesRequest { accountInfo = this.GetAccountInfo(), modelId = mModelId };

            var temp = _vinService.getStyles(mSr);
            if (temp != null && temp.style!=null)
                return temp.style;

            return null;
        }

        public IdentifiedString[] GetTrims(int mYear, string mMake, string mModel)
        {
            var mSr = new VehicleDescriptionRequest()
                          {
                              accountInfo = this.GetAccountInfo(),
                              Items = new object[] {mYear, mMake, mModel},
                              ItemsElementName =
                                  new ItemsChoiceType[]
                                      {
                                          ItemsChoiceType.modelYear, ItemsChoiceType.makeName,
                                          ItemsChoiceType.modelName,
                                      },
                              @switch =
                                  new[]
                                      {
                                          Switch.DisableSafeStandards,
                                          Switch.ShowExtendedDescriptions,
                                          Switch.ShowAvailableEquipment,
                                          Switch.ShowConsumerInformation,
                                          Switch.ShowExtendedTechnicalSpecifications,
                                          Switch.IncludeDefinitions,
                                          Switch.IncludeRegionalVehicles,
                                      }
                          };

            try
            {
                var temp = _vinService.describeVehicle(mSr);
                if (temp != null && temp.style != null && temp.style.Any())
                    return mMake.Equals("Mercedes-Benz") && mYear <= 2009
                               ? temp.style.Select(i => new IdentifiedString() { id = i.id, Value = i.mfrModelCode }).ToArray()
                               : temp.style.Select(i => new IdentifiedString() { id = i.id, Value = i.trim }).ToArray();
            }
            catch (Exception)
            {
                
            }

            return null;
        }

        public VehicleDescription GetVehicleInformationFromStyleId(int styleId)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = this.GetAccountInfo(),
                Items = new object[] { styleId },
                ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.styleId },
                @switch =
                    new[]
                                          {
                                              Switch.DisableSafeStandards,
                                              Switch.ShowExtendedDescriptions,
                                              Switch.ShowAvailableEquipment,
                                              Switch.ShowConsumerInformation,
                                              Switch.ShowExtendedTechnicalSpecifications,
                                              Switch.IncludeDefinitions,
                                              Switch.IncludeRegionalVehicles,
                                          }
            };

            try
            {
                var temp = _vinService.describeVehicle(mSifsir);

                if (temp != null)
                {
                    return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
                }
            }
            catch (Exception)
            {
                
            }

            return null;
        }

        public VehicleDescription GetVehicleInformationFromVin(string mVin)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = this.GetAccountInfo(),
                Items = new object[] { mVin },
                ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.vin },
                @switch =
                    new[]
                                          {
                                              Switch.DisableSafeStandards,
                                              Switch.ShowExtendedDescriptions,
                                              Switch.ShowAvailableEquipment,
                                              Switch.ShowConsumerInformation,
                                              Switch.ShowExtendedTechnicalSpecifications,
                                              Switch.IncludeDefinitions,
                                              Switch.IncludeRegionalVehicles,
                                              
                                          }
            };

            try
            {
                var temp = _vinService.describeVehicle(mSifsir);

                if (temp == null) return null;

                if (temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) && temp.vinDescription.modelYear > 0)
                    return temp;
                return null;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }

        
        }

        public VehicleDescription GetVehicleInformationFromVin(string mVin, int reducingStyleId)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = this.GetAccountInfo(),
                Items = new object[] { mVin, reducingStyleId },
                ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.vin, ItemsChoiceType.reducingStyleId, },
                @switch =
                    new[]
                                          {
                                              Switch.DisableSafeStandards,
                                              Switch.ShowExtendedDescriptions,
                                              Switch.ShowAvailableEquipment,
                                              Switch.ShowConsumerInformation,
                                              Switch.ShowExtendedTechnicalSpecifications,
                                              Switch.IncludeDefinitions,
                                              Switch.IncludeRegionalVehicles,
                                          }
            };

            var temp = _vinService.describeVehicle(mSifsir);

            if (temp == null) return null;

            return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
        }

        public VehicleDescription GetVehicleInformationFromYearMakeModel(int mYear, string mMake, string mModel)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = this.GetAccountInfo(),
                Items = new object[] { mYear, mMake, mModel },
                ItemsElementName =
                    new ItemsChoiceType[]
                                          {
                                              ItemsChoiceType.modelYear, ItemsChoiceType.makeName,
                                              ItemsChoiceType.modelName,
                                          },
                @switch =
                    new[]
                                          {
                                              Switch.DisableSafeStandards,
                                              Switch.ShowExtendedDescriptions,
                                              Switch.ShowAvailableEquipment,
                                              Switch.ShowConsumerInformation,
                                              Switch.ShowExtendedTechnicalSpecifications,
                                              Switch.IncludeDefinitions,
                                              Switch.IncludeRegionalVehicles,
                                          }
            };

            var temp = _vinService.describeVehicle(mSifsir);

            if (temp == null) return null;

            return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
        }

        public VehicleDescription GetStyleInformationFromStyleId(int styleId)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = this.GetAccountInfo(),
                Items = new object[] { styleId },
                ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.styleId },
                @switch =
                    new[]
                                          {
                                              Switch.DisableSafeStandards,
                                              Switch.ShowExtendedDescriptions,
                                              Switch.ShowAvailableEquipment,
                                              Switch.ShowConsumerInformation,
                                              Switch.ShowExtendedTechnicalSpecifications,
                                              Switch.IncludeDefinitions,
                                              Switch.IncludeRegionalVehicles,
                                          }
            };

            try
            {
                var temp = _vinService.describeVehicle(mSifsir);

                if (temp == null) return null;

                return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ValidateVin(string mVin)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = this.GetAccountInfo(),
                Items = new object[] { mVin },
                ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.vin },
                @switch = new[] { Switch.DisableSafeStandards, Switch.IncludeDefinitions, Switch.IncludeRegionalVehicles, Switch.ShowAvailableEquipment, Switch.ShowConsumerInformation, Switch.ShowExtendedDescriptions, Switch.ShowExtendedTechnicalSpecifications }
            };

            var temp = _vinService.describeVehicle(mSifsir);
            if (temp != null)
            {
                if (temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) || temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.ConditionallySuccessful))
                    return true;
            }

            return false;
        }
        public static string UpperFirstLetterOfEachWord(string value)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        public List<ExtendedFactoryOptions> GetPackageOptions(VehicleDescription vehicleInfo)
        {
            var listPackageOptions = new List<ExtendedFactoryOptions>();
            var hash = new HashSet<string>();
            if (vehicleInfo.factoryOption != null && vehicleInfo.factoryOption.Any())
            {
                foreach (var fo in vehicleInfo.factoryOption)
                    if (fo.description.Any())
                    {
                        {
                            var optionsName = fo.description.FirstOrDefault();
                            if (optionsName == null || hash.Contains(optionsName)) continue;

                            if (optionsName.Contains("PKG") || optionsName.Contains("PACK") || optionsName.Contains("EDITION"))
                            {
                                var efo = new ExtendedFactoryOptions
                                {
                                    Msrp = fo.price.msrpMax.ToString("C"),
                                    Name = UpperFirstLetterOfEachWord(optionsName.Replace(",", "")),
                                    Standard = fo.standard,
                                    CategoryName = fo.header != null ? fo.header.Value : string.Empty,
                                    Description = (fo.description.Count() >= 2) ? fo.description[1] : fo.description[0],
                                    OemCode = fo.oemCode
                                };

                                listPackageOptions.Add(efo);
                                hash.Add(optionsName);
                            }
                        }
                    }
            }
           
            return listPackageOptions;
        }

        public List<ExtendedFactoryOptions> GetNonInstalledOptions(VehicleDescription vehicleInfo)
        {
            var listNonInstalledOptions = new List<ExtendedFactoryOptions>();
            var hash = new HashSet<string>();
            if (vehicleInfo.factoryOption != null && vehicleInfo.factoryOption.Any())
            {
                foreach (var fo in vehicleInfo.factoryOption)
                    if (fo.description.Any())
                    {
                        {
                            var optionsName = fo.description.FirstOrDefault();
                            if (optionsName == null || hash.Contains(optionsName)) continue;

                            if (!optionsName.Contains("PKG") && !optionsName.Contains("PACK") && !optionsName.Contains("EDITION"))
                            {  
                                var efo = new ExtendedFactoryOptions
                                {
                                    Msrp = fo.price.msrpMax.ToString("C"),
                                    Name = UpperFirstLetterOfEachWord(optionsName.Replace(",", "")),
                                    Standard = fo.standard,
                                    CategoryName = fo.header != null ? fo.header.Value : string.Empty,
                                    Description = fo.description.FirstOrDefault(),
                                    OemCode = fo.oemCode
                                };

                                listNonInstalledOptions.Add(efo);
                                hash.Add(optionsName);
                            }
                        }
                    }
            }

            // Get addtional options from generic equipment
            if (vehicleInfo.genericEquipment != null && vehicleInfo.genericEquipment.Any())
            {
                foreach (var ge in vehicleInfo.genericEquipment)
                {
                    var category = ((CategoryDefinition)(ge.Item));
                    if (hash.Contains(category.category.Value) || ge.installed != null) continue;

                    var efo = new ExtendedFactoryOptions
                    {
                        Msrp = "$0",
                        Name = UpperFirstLetterOfEachWord(category.category.Value),
                        Standard = false,
                        CategoryName = category.@group != null ? category.@group.Value : string.Empty,
                        Description = category.category.Value
                    };

                    listNonInstalledOptions.Add(efo);
                    hash.Add(category.category.Value);
                }
            }

            return listNonInstalledOptions;
        }
        #endregion

        #region Helper

        public List<string> InitializeTrims(IdentifiedString[] styles)
        {
            var hash = new HashSet<string>();
            var list = new List<string>();
            var baseTrimId = 0;
            if (styles == null || !styles.Any()) return list;

            foreach (var s in styles)
            {
                if (String.IsNullOrEmpty(s.Value))
                {
                    var uniqueTrim = "Base/Other Trims" + "Trim" + "StyleId*" + s.id;
                    if (baseTrimId == 0) baseTrimId = s.id;

                    if (!hash.Contains(s.Value)) list.Add(uniqueTrim);

                    hash.Add(s.Value);
                }
                else
                {
                    var uniqueTrim = s.Value + "Trim" + "StyleId*" + s.id;
                    if (baseTrimId == 0) baseTrimId = s.id;

                    if (!hash.Contains(s.Value)) list.Add(uniqueTrim);

                    hash.Add(s.Value);
                }
            }

            // Adding Base/Other Trims
            if (!list.Any(i => i.Contains(BaseOtherTrim)) && baseTrimId > 0) list.Add("Base/Other Trims" + "Trim" + "StyleId*" + baseTrimId);

            return list;
        }

        public List<string> InitializeExteriorColors(Color[] exteriorList)
        {
            var hash = new HashSet<string>();
            var list = new List<string>();
            if (exteriorList != null)
            {
                foreach (Color ec in exteriorList)
                {
                    var uniqueColor = ec.colorName + "|" + ec.colorCode + "Exterior";
                    if (String.IsNullOrEmpty(ec.colorName) || hash.Contains(ec.colorName)) continue;
                    list.Add(uniqueColor);
                    hash.Add(ec.colorName);
                }
            }

            return list;
        }

        public List<string> InitializeInteriorColors(Color[] interiorList)
        {
            var hash = new HashSet<string>();
            var list = new List<string>();
            if (interiorList != null)
            {
                foreach (Color ic in interiorList)
                {
                    var uniqueColor = ic.colorName + "Interior";
                    if (String.IsNullOrEmpty(ic.colorName) || hash.Contains(ic.colorName)) continue;
                    list.Add(uniqueColor);
                    hash.Add(ic.colorName);
                }
            }

            return list;
        }

        public List<string> InitializeEngineStyles(Engine[] engines)
        {
            var list = new List<string>();

            if (engines != null)
            {
                foreach (Engine er in engines)
                {
                    var fuelType = er.fuelType.Value;
                    int index = fuelType.LastIndexOf(" ", StringComparison.Ordinal);
                    var uniqueCylinder = er.cylinders + "Cylinder";
                    var uniqueFuel = fuelType.Substring(0, index) + "Fuel";
                    if (er.displacement != null)
                    {
                        var uniqueLitter = er.displacement.liters + "Litter";
                        if (!list.Contains(uniqueLitter)) list.Add(uniqueLitter);
                    }
                     
                    if (!list.Contains(uniqueCylinder)) list.Add(uniqueCylinder);
                    if (!list.Contains(uniqueFuel)) list.Add(uniqueFuel);
                    
                }
            }

            return list;
        }

        public string InitializeBodyStyle(VehicleDescription styleInfo)
        {
            if (styleInfo.vinDescription != null && !String.IsNullOrEmpty(styleInfo.vinDescription.bodyType))
            {
                var uniqueBodyType = styleInfo.vinDescription.bodyType + "BodyType";
                return (uniqueBodyType);
            }

            try
            {
                var uniqueBodyType = styleInfo.style.Last().bodyType.Last().Value + "BodyType";
                return (uniqueBodyType);
            }
            catch (Exception)
            {
                var uniqueBodyType = styleInfo.bestStyleName + "BodyType";
                return (uniqueBodyType);
            }
        }
        public List<string> InitializePackages(VehicleDescription styleInfo)
        {
            var packageOptions = GetPackageOptions(styleInfo);

            return (from fo in packageOptions let name = fo.Name.Replace(",", "") select name + "*" + fo.Msrp + "*" + fo.Description + "*" + fo.OemCode + "*nonchecked" + "Package").ToList();
        }

        public List<string> InitializeAdditionalOptions(VehicleDescription styleInfo)
        {
            
            var addOptions = GetNonInstalledOptions(styleInfo);

            return addOptions.Select(fo => fo.Name + "*" + fo.Msrp + "*" + fo.Description + "*" + fo.OemCode + "*nonchecked" + "Optional").ToList();
        }

        public List<string> InitializePackages(VehicleDescription styleInfo, IList<string> existPackages)
        {
            var list = new List<string>();
            
            var packageOptions = GetPackageOptions(styleInfo);

            foreach (var fo in packageOptions)
            {
                var name = fo.Name.Replace(",", "");
                if (existPackages.Any(x => x == name))
                {
                    var uniquePackage = name + "*" + fo.Msrp + "*" + fo.Description + "*" + fo.OemCode + "*checked" + "Package";
                    list.Add(uniquePackage);
                }
                else
                {
                    var uniquePackage = name + "*" + fo.Msrp + "*" + fo.Description + "*" + fo.OemCode + "*nonchecked" + "Package";
                    list.Add(uniquePackage);
                }
              
            }

            return list;
        }

        public List<string> InitializeAdditionalOptions(VehicleDescription styleInfo, IList<string> existOptions)
        {
          
            var list = new List<string>();

            var addOptions = GetNonInstalledOptions(styleInfo);
            foreach (var fo in addOptions)
            {

                if (existOptions.Any(x => x == fo.Name))
                {
                    var uniqueOption = fo.Name + "*" + fo.Msrp + "*" + fo.Description + "*" + fo.OemCode + "*checked" + "Optional";

                    list.Add(uniqueOption);
                }
                else
                {
                    var uniqueOption = fo.Name + "*" + fo.Msrp + "*" + fo.Description + "*" + fo.OemCode + "*nonchecked" + "Optional";

                    list.Add(uniqueOption);
                }
            }
           

            return list;
        }

        public string InitializeDoor(VehicleDescription styleInfo)
        {
            return styleInfo.style.First().passDoors + "PassengerDoors";
        }

        public IEnumerable<SelectListItem> InitalTrimList(IdentifiedString[] styleList, string trimName)
        {
            var returnList = new List<SelectListItem>();
            bool isOthers = true;
            if (styleList != null && styleList.Any())
            {
                var firstStyle = styleList.First();

                var hash = new HashSet<string>();

                foreach (var style in styleList)
                {
                    if (!hash.Contains(style.Value))
                    {
                        if (String.IsNullOrEmpty(style.Value))
                        {
                            firstStyle = style;
                        }
                        else
                        {
                            returnList.Add(CreateSelectListItem(style.Value, style.id + "|" + style.Value, style.Value.ToLower().Equals(trimName.ToLower())));
                            if (style.Value.ToLower().Equals(trimName.ToLower()))
                            {
                                isOthers = false;
                            }
                        }

                        hash.Add(style.Value);
                    }
                }

                returnList.Add(CreateSelectListItem("Base/Other Trims", firstStyle.id + "|" + "Base/Other Trims", isOthers));
            }
            else
            {
                returnList.Add(CreateSelectListItem("Base/Other Trims", 0+ "|" + "Base/Other Trims", false));
            }

            return returnList;
        }

        public IEnumerable<SelectListItem> InitalExteriorColorList(Color[] exteriorColorArray)
        {
            var returnList = new List<SelectListItem>();
            var hash = new Hashtable();

            if (exteriorColorArray != null && exteriorColorArray.Any())
            {
                var flag = true;

                foreach (var color in exteriorColorArray.OrderBy(x => x.colorName))
                {
                    if (hash.Contains(color.colorName)) continue;

                    if (flag)
                    {
                        returnList.Add(CreateSelectListItem(color.colorName, color.colorCode, true));
                        flag = false;
                    }
                    else
                        returnList.Add(CreateSelectListItem(color.colorName, color.colorCode, false));

                    hash.Add(color.colorName, color.colorCode);
                }

                returnList = returnList.OrderBy(i => i.Text).ToList();
                returnList.Add(CreateSelectListItem(OtherColors, OtherColors, false));
            }
            else
            {
                returnList.Add(CreateSelectListItem(OtherColors, OtherColors, true));
            }
       
            return returnList.AsEnumerable();
        }

        public IEnumerable<SelectListItem> InitalInteriorColorList(Color[] interiorColorArray)
        {
            var returnList = new List<SelectListItem>();
            var hash = new Hashtable();

            if (interiorColorArray != null && interiorColorArray.Any())
            {
                var flag = true;

                foreach (var color in interiorColorArray.OrderBy(x => x.colorName))
                {
                    if (hash.Contains(color.colorName)) continue;
                    if (flag)
                    {
                        returnList.Add(CreateSelectListItem(color.colorName, color.colorName, true));
                        flag = false;
                    }
                    else
                        returnList.Add(CreateSelectListItem(color.colorName, color.colorName, false));

                    hash.Add(color.colorName, color.colorName);
                }

                returnList.Add(CreateSelectListItem(OtherColors, OtherColors, false));

                return returnList.AsEnumerable();
            }
            else
            {
                returnList.Add(CreateSelectListItem(OtherColors, OtherColors, true));
            }
           
            return returnList.AsEnumerable();
        }

        public IEnumerable<SelectListItem> InitialBodyTypeList(string bodyType)
        {
            var returnList = new List<SelectListItem> { CreateSelectListItem(bodyType, bodyType, true) };

            return returnList.AsEnumerable();
        }

        public IEnumerable<SelectListItem> InitialFuelList(Engine[] engineList)
        {
            var returnList = new List<SelectListItem>();

            var flag = true;

            foreach (Engine er in engineList)
            {
                var fuelType = er.fuelType.Value;
                var index = fuelType.LastIndexOf(" ", StringComparison.Ordinal);
                if (returnList.Any(i => i.Text.ToLower().Equals(fuelType.Substring(0, index).ToLower()))) continue;
                if (flag)
                {
                    returnList.Add(CreateSelectListItem(fuelType.Substring(0, index), fuelType.Substring(0, index), true));
                    flag = false;
                }
                else returnList.Add(CreateSelectListItem(fuelType.Substring(0, index), fuelType.Substring(0, index), false));
            }

            return returnList.AsEnumerable();

        }

        public IEnumerable<SelectListItem> InitialCylinderList(Engine[] engineList)
        {
            var returnList = new List<SelectListItem>();

            var flag = true;

            foreach (Engine er in engineList)
            {
                if (flag)
                {
                    returnList.Add(CreateSelectListItem(er.cylinders.ToString(), er.cylinders.ToString(), true));
                    flag = false;
                }
                else
                    returnList.Add(CreateSelectListItem(er.cylinders.ToString(), er.cylinders.ToString(), false));
            }

            return returnList.AsEnumerable();
        }

        public IEnumerable<SelectListItem> InitialLitterList(Engine[] engineList)
        {
            var returnList = new List<SelectListItem>();

            var flag = true;

            foreach (Engine er in engineList)
            {
                if (flag)
                {
                    returnList.Add(CreateSelectListItem(er.displacement.liters.ToString(), er.displacement.liters.ToString(), true));
                    flag = false;
                }
                else
                    returnList.Add(CreateSelectListItem(er.displacement.liters.ToString(), er.displacement.liters.ToString(), false));
            }

            return returnList.AsEnumerable();
        }

        #endregion

        private SelectListItem CreateSelectListItem(string text, string value, bool selected)
        {
            return new SelectListItem()
                       {
                           Text = text,
                           Value = value,
                           Selected = selected
                       };
        }
    }

    
}