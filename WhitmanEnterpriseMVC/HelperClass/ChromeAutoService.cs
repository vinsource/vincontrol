using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using WhitmanEnterpriseMVC.Models;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class ChromeAutoService
    {
        private readonly AccountInfo _mAi;

        private readonly Description7a _vinService = new Description7a();

        public AccountInfo GetAccountInfo()
        {
            return this._mAi;
        }

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

        public int[] GetModelYears()
        {
            var baseReuqest = new BaseRequest { accountInfo = this.GetAccountInfo() };

            var temp = _vinService.getModelYears(baseReuqest);

            if (temp != null && temp.modelYear.Any())
                return temp.modelYear;

            return null;
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

            if (temp != null && temp.model.Any())
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
            if (temp != null && temp.style.Any())
                return temp.style;
            
            return null;
        }

        public VehicleDescription GetVehicleInformationFromStyleId(int styleId)
        {
            var mSifsir = new VehicleDescriptionRequest
                              {
                                  accountInfo = this.GetAccountInfo(),
                                  Items = new object[] {styleId},
                                  ItemsElementName = new ItemsChoiceType[] {ItemsChoiceType.styleId},
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

            if (temp != null)
            {
                return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
            }
            
            return null;
        }

        public VehicleDescription GetVehicleInformationFromVin(string mVin)
        {
            var mSifsir = new VehicleDescriptionRequest
                              {
                                  accountInfo = this.GetAccountInfo(),
                                  Items = new object[] {mVin},
                                  ItemsElementName = new ItemsChoiceType[] {ItemsChoiceType.vin},
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
            
            if (temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) && temp.modelYear > 0)
                return temp;
            return null;
        }

        public VehicleDescription GetVehicleInformationFromVin(string mVin, int reducingStyleId)
        {
            var mSifsir = new VehicleDescriptionRequest
                              {
                                  accountInfo = this.GetAccountInfo(),
                                  Items = new object[] {mVin, reducingStyleId},
                                  ItemsElementName = new ItemsChoiceType[] {ItemsChoiceType.vin, ItemsChoiceType.reducingStyleId,},
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
            
            var temp = _vinService.describeVehicle(mSifsir);

            if (temp == null) return null;
            
            return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
        }

        public VehicleDescription GetStyleInformationFromStyleId(int styleId)
        {
            var mSifsir = new VehicleDescriptionRequest
                              {
                                  accountInfo = this.GetAccountInfo(),
                                  Items = new object[] {styleId},
                                  ItemsElementName = new ItemsChoiceType[] {ItemsChoiceType.styleId},
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

        public List<ExtendedFactoryOptions> GetPackageOptions(VehicleDescription vehicleInfo)
        {
            var listPackageOptions = new List<ExtendedFactoryOptions>();
            var hash = new HashSet<string>();
            if (vehicleInfo.factoryOption != null && vehicleInfo.factoryOption.Any())
            {
                foreach (var fo in vehicleInfo.factoryOption.Where(x=>x.standard==false))
                    if (fo.description.Any())
                    {
                        {
                            var optionsName = fo.description.FirstOrDefault();
                            if (optionsName == null || hash.Contains(optionsName)) continue;

                            if (fo.price.msrpMax > 0 && (optionsName.Contains("PKG") || optionsName.Contains("PACKAGE") || optionsName.Contains("EDITION")))
                            {
                                var efo = new ExtendedFactoryOptions();
                                efo.setMSRP(fo.price.msrpMax.ToString("C"));
                                efo.setName(CommonHelper.UpperFirstLetterOfEachWord(optionsName.Replace(",", "")));
                                efo.setStandard(fo.standard);
                                efo.setCategoryName(fo.header != null ? fo.header.Value : string.Empty);
                                efo.Description = (fo.description.Count() >= 2) ? fo.description[1] : fo.description[0];

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
                foreach (var fo in vehicleInfo.factoryOption.Where(x => x.standard == false))
                    if (fo.description.Any())
                    {
                        {
                            var optionsName = fo.description.FirstOrDefault();
                            if (optionsName == null || hash.Contains(optionsName)) continue;

                            if (fo.price.msrpMax > 0 && !optionsName.Contains("PKG") && !optionsName.Contains("PACKAGE") && !optionsName.Contains("EDITION"))
                            {
                                var efo = new ExtendedFactoryOptions();
                                efo.setMSRP(fo.price.msrpMax.ToString("C"));
                                efo.setName(CommonHelper.UpperFirstLetterOfEachWord(optionsName.Replace(",", "")));
                                efo.setStandard(fo.standard);
                                efo.setCategoryName(fo.header != null ? fo.header.Value : string.Empty);
                                efo.Description = fo.description.FirstOrDefault();

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
                    var category = ((CategoryDefinition)(ge.Item)).category.Value;
                    if (!hash.Contains(category) && ge.installed == null)
                    {
                        var efo = new ExtendedFactoryOptions();
                        efo.setMSRP("$0");
                        efo.setName(CommonHelper.UpperFirstLetterOfEachWord(category));
                        efo.setStandard(false);
                        efo.setCategoryName(category);
                        efo.Description = category;

                        listNonInstalledOptions.Add(efo);
                        hash.Add(category);
                    }
                }
            }

            return listNonInstalledOptions;
        }
    }
}

