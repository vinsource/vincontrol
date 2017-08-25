using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using vincontrol.DataFeed.AutomotiveService;

namespace vincontrol.DataFeed.Helper
{
    public class AutoServiceHelper
    {
        public AccountInfo Account { get; set; }
        public AutomotiveDescriptionService6PortTypeClient ChromeService { get; set; }
        public List<SelectItem> Results { get; set; }
        public Hashtable TempTable { get; set; }

        public AutoServiceHelper()
        {
            if (ChromeService == null)
                ChromeService = new AutomotiveDescriptionService6PortTypeClient();

            Account = new AccountInfo
                          {
                              accountNumber = System.Configuration.ConfigurationManager.AppSettings["accountNumber"],
                              accountSecret = System.Configuration.ConfigurationManager.AppSettings["accountSecret"]
                          };

            var locale = new Locale
                             {
                                 country = System.Configuration.ConfigurationManager.AppSettings["country"],
                                 language = System.Configuration.ConfigurationManager.AppSettings["language"]
                             };

            Account.locale = locale;
        }

        #region IAutoService Members

        public VehicleInformation GetVehicleInformationFromVinRequest(string vin)
        {
            var returnParameters = new ReturnParameters
            {
                excludeFleetOnlyStyles = false,
                useSafeStandards = true,
                includeAvailableEquipment = true,
                includeExtendedDescriptions = true,
                includeConsumerInformation = true,
                includeExtendedTechnicalSpecifications = true,
                includeRegionSpecificStyles = false,
                enableEnrichedVehicleEquipment = true
            };

            var request = new VehicleInformationFromVinRequest
            {
                accountInfo = Account,
                vin = vin,
                returnParameters = returnParameters
            };

            var result = ChromeService.getVehicleInformationFromVin(request);
            if (result == null)
                return null;

            if (result.responseStatus.responseCode.Equals(ResponseCode.Successful) ||
                result.responseStatus.responseCode.Equals(ResponseCode.SuccessfulUsingAlternateLocale))
                return result;

            return null;
        }

        public StyleInformation GetStyleInformationFromStyleId(int[] mStyleIds, string[] mManufacturerOptionCodes, string[] mEquipmentDescriptions, string mExteriorColorName, string mInteriorColorName)
        {
            var mSifsir = new StyleInformationFromStyleIdRequest();

            var rd = new ReturnParameters
                         {
                             excludeFleetOnlyStyles = true,
                             useSafeStandards = true,
                             includeAvailableEquipment = true,
                             includeExtendedDescriptions = true,
                             includeConsumerInformation = false,
                             includeExtendedTechnicalSpecifications = true,
                             includeRegionSpecificStyles = false,
                             enableEnrichedVehicleEquipment = true
                         };

            mSifsir.accountInfo = Account;
            mSifsir.styleIds = mStyleIds;
            mSifsir.manufacturerOptionCodes = mManufacturerOptionCodes;
            mSifsir.equipmentDescriptions = mEquipmentDescriptions;
            mSifsir.exteriorColorName = mExteriorColorName;
            mSifsir.interiorColorName = mInteriorColorName;
            mSifsir.returnParameters = rd;

            var temp = ChromeService.getStyleInformationFromStyleId(mSifsir);
            if (temp != null)
            {
                return temp.responseStatus.responseCode.Equals(ResponseCode.Successful) ? temp : null;
            }

            return null;
        }
        
        public IEnumerable<SelectItem> InitializeExteriorColors(ExteriorColor[] exteriorColors)
        {
            Results = new List<SelectItem>();
            TempTable = new Hashtable();

            if (exteriorColors != null && exteriorColors.Any())
            {
                bool flag = true;

                foreach (ExteriorColor color in exteriorColors.OrderBy(x => x.colorName).ToList().Where(color => !TempTable.Contains(color.colorCode)))
                {
                    if (flag)
                    {
                        Results.Add(new SelectItem(color.colorName, color.colorCode, true));
                        flag = false;
                    }
                    else
                        Results.Add(new SelectItem(color.colorName, color.colorCode, false));

                    TempTable.Add(color.colorCode, color.colorCode);
                }
            }

            Results.Add(new SelectItem("Other Colors", "Other Colors", false));
            return Results.AsEnumerable();
        }

        public IEnumerable<SelectItem> InitializeInteriorColors(InteriorColor[] interiorColors)
        {
            Results = new List<SelectItem>();
            TempTable = new Hashtable();

            if (interiorColors != null && interiorColors.Any())
            {
                bool flag = true;

                foreach (var color in interiorColors.OrderBy(x => x.colorName).ToList().Where(color => !TempTable.Contains(color.colorCode)))
                {
                    if (flag)
                    {
                        Results.Add(new SelectItem(color.colorName, color.colorCode, true));
                        flag = false;
                    }
                    else
                        Results.Add(new SelectItem(color.colorName, color.colorCode, false));

                    TempTable.Add(color.colorCode, color.colorCode);
                }
            }

            Results.Add(new SelectItem("Other Colors", "Other Colors", false));
            return Results.AsEnumerable();
        }

        public IEnumerable<SelectItem> InitializeTrims(Style[] styles)
        {
            Results = new List<SelectItem>();
            TempTable = new Hashtable();

            if (styles != null && styles.Count() > 0)
            {
                bool flag = true;

                foreach (Style trim in styles)
                {
                    var trimName = String.IsNullOrEmpty(trim.trimName) ? (String.IsNullOrEmpty(trim.consumerFriendlyModelName) ? "Base" : trim.consumerFriendlyModelName) : trim.trimName;

                    if (String.IsNullOrEmpty(trimName) || TempTable.Contains(trimName)) continue;
                    if (flag)
                    {
                        Results.Add(new SelectItem(trimName, trim.styleId.ToString(), true));
                        flag = false;
                    }
                    else
                        Results.Add(new SelectItem(trimName, trim.styleId.ToString(), false));

                    TempTable.Add(trimName, trimName);
                }

                return Results.AsEnumerable();
            }

            return Results;
        }

        public IEnumerable<SelectItem> InitializeBodyTypes(BodyType[] bodyTypes)
        {
            Results = new List<SelectItem>();
            bool flag = true;

            if (bodyTypes != null && bodyTypes.Any())
            {
                foreach (var type in bodyTypes)
                {
                    if (flag)
                    {
                        Results.Add(new SelectItem(type.bodyTypeName, type.bodyTypeId.ToString(), true));
                        flag = false;
                    }
                    else
                    {
                        Results.Add(new SelectItem(type.bodyTypeName, type.bodyTypeId.ToString(), false));
                    }
                }
            }

            return Results.AsEnumerable();
        }

        public IEnumerable<SelectItem> InitializeFuels(Engine[] engines)
        {
            Results = new List<SelectItem>();
            bool flag = true;

            if (engines != null && engines.Any())
            {
                foreach (Engine er in engines)
                {
                    string fuelType = er.fuelType;
                    int index = fuelType.LastIndexOf(" ");
                    if (flag)
                    {
                        Results.Add(new SelectItem(fuelType.Substring(0, index), fuelType.Substring(0, index), true));
                        flag = false;
                    }
                    else
                        Results.Add(new SelectItem(fuelType.Substring(0, index), fuelType.Substring(0, index), false));

                }
            }
            return Results.AsEnumerable();
        }

        public IEnumerable<SelectItem> InitializeCylinders(Engine[] engines)
        {
            Results = new List<SelectItem>();
            bool flag = true;

            if (engines != null && engines.Any())
            {
                foreach (Engine er in engines)
                {
                    if (flag)
                    {
                        Results.Add(new SelectItem(er.cylinders.ToString(), er.cylinders.ToString(), true));
                        flag = false;
                    }
                    else
                        Results.Add(new SelectItem(er.cylinders.ToString(), er.cylinders.ToString(), false));
                }
            }

            return Results.AsEnumerable();
        }

        public IEnumerable<SelectItem> InitializeLitters(Engine[] engines)
        {
            Results = new List<SelectItem>();
            bool flag = true;

            if (engines != null && engines.Any())
            {
                foreach (Engine er in engines)
                {
                    if (flag)
                    {
                        Results.Add(new SelectItem(er.displacementL.ToString(), er.displacementL.ToString(), true));
                        flag = false;
                    }
                    else
                        Results.Add(new SelectItem(er.displacementL.ToString(), er.displacementL.ToString(), false));

                }
            }

            return Results.AsEnumerable();
        }

        public IEnumerable<SelectItem> InitializeTranmmissions(GenericEquipment[] equips)
        {
            Results = new List<SelectItem>();
            TempTable = new Hashtable();
            bool flag = true;
            if (equips != null && equips.Any())
            {
                foreach (GenericEquipment ge in equips)
                {
                    switch (ge.categoryId)
                    {
                        case 1220:
                        case 1210:
                        case 1104:
                        case 1103:
                        case 1102:
                        case 1101:
                        case 1130:
                            if (!TempTable.Contains("Automatic"))
                            {
                                if (flag)
                                {
                                    Results.Add(new SelectItem("Automatic", "Automatic", true));
                                    flag = false;
                                }
                                else
                                    Results.Add(new SelectItem("Automatic", "Automatic", false));

                                TempTable.Add("Automatic", "Automatic");
                            }
                            break;
                        case 1148:
                        case 1147:
                        case 1146:
                        case 1108:
                        case 1107:
                        case 1106:
                        case 1105:
                        case 1131:
                            if (!TempTable.Contains("Manual"))
                            {
                                if (flag)
                                {
                                    Results.Add(new SelectItem("Manual", "Manual", true));
                                    flag = false;
                                }
                                else
                                    Results.Add(new SelectItem("Manual", "Manual", false));

                                TempTable.Add("Manual", "Manual");
                            }
                            break;
                    }
                }
            }

            return Results.AsEnumerable();
        }
        
        public IEnumerable<string> InitializeOptionsWithNameOnly(FactoryOption[] options)
        {
            var results = new List<string>();
            TempTable = new Hashtable();

            if (options != null && options.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                foreach (var option in options)
                {
                    string fullDescription = ConvertArrayToString(option.descriptions);
                    string name = Trim(fullDescription, 40);
                    var newString = regex.Replace(name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    if (!TempTable.Contains(name) && option.msrp.highValue > 0 && !name.Contains("PKG") &&
                        !name.Contains("PACKAGE"))
                    {
                        string uniqueOption = newString /*+ " " + option.msrp.highValue.ToString("C")*/;
                        results.Add(uniqueOption);
                        TempTable.Add(name, option);
                    }
                }
            }

            return results;
        }

        public IEnumerable<string> InitializeStandardOptionsWithNameOnly(Standard[] options)
        {
            var results = new List<string>();
            TempTable = new Hashtable();

            if (options != null && options.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                foreach (Standard option in options)
                {
                    string name = Trim(option.description, 40);
                    if (!TempTable.Contains(name) && option.installed)
                    {
                        var newString = regex.Replace(name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        results.Add(newString);
                        TempTable.Add(name, option);
                    }
                }
            }

            return results;
        }

        public IEnumerable<string> InitializePackagesWithNameOnly(FactoryOption[] packages)
        {
            var results = new List<string>();
            TempTable = new Hashtable();

            if (packages != null && packages.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                foreach (var option in packages)
                {
                    string fullDescription = ConvertArrayToString(option.descriptions);
                    string name = Trim(fullDescription, 40);
                    var newString = regex.Replace(name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    if (!TempTable.Contains(name) && option.msrp.highValue > 0 &&
                        (name.Contains("PKG") || name.Contains("PACKAGE")))
                    {
                        string uniquePackage = newString/* + " " + option.msrp.highValue.ToString("C")*/;
                        results.Add(uniquePackage);
                        TempTable.Add(name, option);
                    }
                }
            }

            return results;
        }

        public IEnumerable<ExtendedSelectItemWithPrice> InitializeOptions(FactoryOption[] options)
        {
            var results = new List<ExtendedSelectItemWithPrice>();
            TempTable = new Hashtable();

            if (options != null && options.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                foreach (var option in options)
                {
                    var name = ConvertArrayToString(option.descriptions);
                    var realDescription = (option.descriptions != null && option.descriptions.Count() >= 2) ? option.descriptions[1] : name;
                    var fullName = (option.descriptions != null && option.descriptions.Any()) ? option.descriptions[0] : string.Empty;
                    var trimName = Trim(name, 40);
                    var newString = regex.Replace(trimName, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    if (!TempTable.Contains(trimName) && option.msrp.highValue > 0 && !trimName.Contains("PKG") &&
                        !trimName.Contains("PACKAGE"))
                    {
                        string uniqueOption = newString + " " + option.msrp.highValue.ToString("C");
                        results.Add(new ExtendedSelectItemWithPrice(uniqueOption, option.chromeOptionCode,
                                                                    option.msrp.highValue.ToString(), false, realDescription, fullName));
                        TempTable.Add(trimName, option);
                    }
                }
            }

            return results;
        }

        public IEnumerable<ExtendedSelectItemWithPrice> InitializePackages(FactoryOption[] packages)
        {
            var results = new List<ExtendedSelectItemWithPrice>();
            TempTable = new Hashtable();

            if (packages != null && packages.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                foreach (var option in packages)
                {
                    string name = ConvertArrayToString(option.descriptions);
                    var realDescription = (option.descriptions != null && option.descriptions.Count() >= 2) ? option.descriptions[1] : name;
                    var fullName = (option.descriptions != null && option.descriptions.Any()) ? option.descriptions[0] : string.Empty;
                    string trimName = Trim(name, 40);
                    var newString = regex.Replace(trimName, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    if (!TempTable.Contains(trimName) && option.msrp.highValue > 0 &&
                        (trimName.Contains("PKG") || trimName.Contains("PACKAGE")))
                    {
                        string uniquePackage = newString + " " + option.msrp.highValue.ToString("C");
                        results.Add(new ExtendedSelectItemWithPrice(uniquePackage, option.chromeOptionCode,
                                                                    option.msrp.highValue.ToString(), false, realDescription, fullName));
                        TempTable.Add(trimName, option);
                    }
                }
            }

            return results;
        }

        public IEnumerable<ExtendedSelectItemWithPrice> InitializeOptionsWithoutPrice(FactoryOption[] options)
        {
            var results = new List<ExtendedSelectItemWithPrice>();
            TempTable = new Hashtable();

            if (options != null && options.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                foreach (var option in options)
                {
                    string name = ConvertArrayToString(option.descriptions);
                    string trimName = Trim(name, 40);
                    var fullName = (option.descriptions != null && option.descriptions.Any()) ? option.descriptions[0] : string.Empty;
                    var newString = regex.Replace(trimName, new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                    trimName = regex.Replace(trimName, new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                    var realDescription = (option.descriptions != null && option.descriptions.Count() >= 2) ? option.descriptions[1] : trimName;

                    if (!TempTable.Contains(trimName) && option.msrp.highValue > 0 && !trimName.Contains("PKG") &&
                        !trimName.Contains("PACKAGE"))
                    {
                        string uniqueOption = newString;
                        results.Add(new ExtendedSelectItemWithPrice(uniqueOption, option.chromeOptionCode,
                                                                    option.msrp.highValue.ToString(), false, realDescription,fullName));
                        TempTable.Add(trimName, option);
                    }
                }
            }

            return results;
        }

        public IEnumerable<ExtendedSelectItemWithPrice> InitializePackagesWithoutPrice(FactoryOption[] packages)
        {
            var results = new List<ExtendedSelectItemWithPrice>();
            TempTable = new Hashtable();

            if (packages != null && packages.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                foreach (var option in packages)
                {
                    string name = ConvertArrayToString(option.descriptions);
                    string trimName = Trim(name, 40); 
                    var fullName = (option.descriptions != null && option.descriptions.Any()) ? option.descriptions[0] : string.Empty;
                   
                    var newString = regex.Replace(trimName, new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                    name = regex.Replace(name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                    var realDescription = (option.descriptions != null && option.descriptions.Count() >= 2) ? option.descriptions[1] : name;

                    if (!TempTable.Contains(trimName) && option.msrp.highValue > 0 &&
                        (trimName.Contains("PKG") || trimName.Contains("PACKAGE")))
                    {
                        string uniquePackage = newString;
                        results.Add(new ExtendedSelectItemWithPrice(uniquePackage, option.chromeOptionCode,
                                                                    option.msrp.highValue.ToString(), false, realDescription,fullName));
                        TempTable.Add(trimName, option);
                    }
                }
            }

            return results;
        }

        public Style[] GetStyles(int modelId)
        {
            var m_Sr = new StylesRequest { accountInfo = Account, modelId = modelId };
            Style[] temp = ChromeService.getStyles(m_Sr);

            if (temp != null && temp.Length != 0)
                return temp;
            return null;
        }

        #endregion

        private static string Trim(string s, int maxSize)
        {
            if (s == null)
                return string.Empty;

            int count = Math.Min(s.Length, maxSize);
            return s.Substring(0, count);
        }

        private static string ConvertArrayToString(string[] array)
        {
            string result = "";

            foreach (string tmp in array)
            {
                string newString = tmp.Replace(",", "");
                result += newString + " ";
                break;
            }
            return result;
        }
    }
    
    public class SelectItem
    {
        public bool Selected { get; set; }
        
        public string Text { get; set; }
        
        public string Value { get; set; }

        public SelectItem() { }

        public SelectItem(string text, string value, bool selected)
        {
            Text = text;
            Value = value;
            Selected = selected;
        }
    }
    
    public class ExtendedSelectItemWithPrice
    {
        public bool Selected { get; set; }
        
        public string Text { get; set; }
        
        public string Value { get; set; }

        public string FullValue { get; set; }
        
        public string Price { get; set; }

        public string Description { get; set; }

        public ExtendedSelectItemWithPrice(string text, string value, string price, bool selected, string description, string fullValue)
        {
            Text = text;
            Value = value;
            Price = price;
            Selected = selected;
            Description = description;
            FullValue = fullValue;
        }
    }
}
