using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.ChromeAutoService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Helper;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Models;
using Inventory = vincontrol.Data.Model.Inventory;

namespace Vincontrol.Web.HelperClass
{
    //Ex:
    //OC Nissan Irvine presents this 1-owner Carfax Certified 2010 Black Mercedes-Benz GL450 which comes equipped with (Package and Options).
    // This is a competetively priced, hard to find/popular vehicle with low mileage and is very fuel efficient. {Ending Sentence if available}
    //[Dealership] presents this [CarfaxOwner] [Year] [PopularColor] [Make] [Model] [Trim] which comes equipped with [Package] [AdditionalOptions].
    // This is a [MarketRange], [Rare][Popular] vehicle [LowMileage] [FuelEfficient].[EndingSentence]
    public class AutoDescription
    {
        private VincontrolEntities _context;
        private string[] _popularColors = new[] { "Black", "White", "Silver", "Gray" };

        public VincontrolEntities Context
        {
            get { return _context; }
        }

        public AutoDescription()
        {
            _context = new VincontrolEntities();
        }

        public void SaveBatch()
        {
            _context.SaveChanges();
        }

        public bool AssignAutoDescription(int newListingId, DealershipViewModel dealer)
        {
            bool isSuccess = false;
            if (AllowAutoDescription(dealer.DealershipId))
            {
                var newInventory = _context.Inventories.FirstOrDefault(i => i.InventoryId == newListingId);
                var dealerInfo = _context.Dealers.FirstOrDefault(i => i.DealerId == dealer.DealershipId);
                if (newInventory != null && (newInventory.EnableAutoDescription == null || (newInventory.EnableAutoDescription.Value)))
                {
                    var inventoryStatus = newInventory.Condition ? InventoryStatus.Used : InventoryStatus.New;
                    var randomizedTemplate = GetRandomizedTemplate(inventoryStatus);
                    var setting = _context.Settings.FirstOrDefault(i => i.DealerId == dealer.DealershipId);
                    var disclaimer =
              _context.Rebates.Where(
                  i =>
                  (i.DealerId == dealer.DealershipId) && (i.Year == newInventory.Vehicle.Year) && (i.Make == newInventory.Vehicle.Make) && (i.Model == newInventory.Vehicle.Model) && (i.Trim == newInventory.Vehicle.Trim)).Select(j => j.Disclaimer).FirstOrDefault();

                    var autoDescriptionViewModel = new AutoDescriptionViewModel(newInventory)
                    {
                        DealershipName = dealer.DealershipName,
                        EndingSentences = inventoryStatus == InventoryStatus.Used ? setting.EndDescriptionSentence : setting.EndDescriptionSentenceForNew,
                        Address = dealer.DealershipAddress,
                        Phone = dealer.Phone,
                        Disclaimer = disclaimer,
                        DealerId = dealer.DealershipId
                    };
                    if (inventoryStatus == InventoryStatus.New && newInventory.DealerMsrp > 0)
                    {
                        autoDescriptionViewModel.Msrp = newInventory.DealerMsrp.GetValueOrDefault();
                    }

                    var carfax = _context.Carfaxes.FirstOrDefault(x => x.VehicleId == newInventory.VehicleId);

                    if (carfax != null)

                        autoDescriptionViewModel.CarfaxOwner = carfax.Owner.GetValueOrDefault();

                    var serializer = new JavaScriptSerializer();
                    autoDescriptionViewModel.PackageOptionsList =
                        !String.IsNullOrEmpty(newInventory.PackageDescriptions)
                            ? serializer.Deserialize<string[]>(newInventory.PackageDescriptions)
                            : null;

                    autoDescriptionViewModel.Condition = newInventory.Condition;
                    newInventory.DescriptionTemplateId = randomizedTemplate.DescriptionTemplateId;
                    newInventory.Descriptions = GenerateAutoDescription(randomizedTemplate.Content, autoDescriptionViewModel);
                    newInventory.EnableAutoDescription = true;
                }
            }
            return isSuccess;
        }

        private decimal GetMSRPFromChrome(string vinNumber)
        {
            var autoService = new ChromeAutoService();
            var vehicleInfo = autoService.GetVehicleInformationFromVin(vinNumber);
            return Convert.ToDecimal(vehicleInfo.basePrice.msrp.high);
        }

        public void GenerateAutoDescription(int newListingId, DealershipViewModel dealer)
        {
            AssignAutoDescription(newListingId, dealer);
            _context.SaveChanges();
        }

        public string GenerateAutoDescription(Inventory newInventory, DealershipViewModel dealer)
        {
            using (var context = new VincontrolEntities())
            {
                if (newInventory != null)
                {
                    var inventoryStatus = newInventory.Condition == Constanst.ConditionStatus.Used ? InventoryStatus.Used : InventoryStatus.New;
                    var randomizedTemplate = GetRandomizedTemplate(inventoryStatus);
                    var setting = context.Settings.FirstOrDefault(i => i.DealerId == dealer.DealershipId);
                    var disclaimer =
                                  context.Rebates.Where(
                                      i =>
                                      (i.DealerId == newInventory.DealerId) && (i.Year == newInventory.Vehicle.Year) &&
                                      (i.Make == newInventory.Vehicle.Make) && (i.Model == newInventory.Vehicle.Model) &&
                                      (i.Trim == newInventory.Vehicle.Trim)).Select(j => j.Disclaimer).FirstOrDefault();
                    var autoDescriptionViewModel = new AutoDescriptionViewModel(newInventory)
                                                       {
                                                           DealershipName = dealer.DealershipName,
                                                           EndingSentences = inventoryStatus == InventoryStatus.Used ? setting.EndDescriptionSentence : setting.EndDescriptionSentenceForNew,
                                                           Address = dealer.DealershipAddress,
                                                           Phone = dealer.Phone,
                                                           Disclaimer = disclaimer,
                                                           DealerId = dealer.DealershipId
                                                       };
                    // Do some trick
                    switch (autoDescriptionViewModel.DealerId)
                    {
                        case 23063: autoDescriptionViewModel.DealershipName = "Hornburg Los Angeles"; break;
                        case 1563: autoDescriptionViewModel.DealershipName = "Hornburg Santa Monica"; break;
                        case 1541: autoDescriptionViewModel.DealershipName = "Our Dealership"; break;
                        case 2584: autoDescriptionViewModel.DealershipName = "Our Dealership"; break;
                    }

                    if (inventoryStatus == InventoryStatus.New && newInventory.DealerMsrp > 0)
                    {
                        autoDescriptionViewModel.Msrp = newInventory.DealerMsrp.GetValueOrDefault();
                    }

                    var carfax = context.Carfaxes.FirstOrDefault(x => x.VehicleId == newInventory.VehicleId);

                    if (carfax != null)

                        autoDescriptionViewModel.CarfaxOwner = carfax.Owner.GetValueOrDefault();

                    var serializer = new JavaScriptSerializer();
                    autoDescriptionViewModel.PackageOptionsList =
                        !String.IsNullOrEmpty(newInventory.PackageDescriptions)
                            ? serializer.Deserialize<string[]>(newInventory.PackageDescriptions)
                            : null;

                    autoDescriptionViewModel.Condition = newInventory.Condition;
                    newInventory.DescriptionTemplateId = randomizedTemplate.DescriptionTemplateId;
                    newInventory.Descriptions = GenerateAutoDescription(randomizedTemplate.Content, autoDescriptionViewModel);
                    newInventory.EnableAutoDescription = true;
                    context.SaveChanges();

                    return newInventory.Descriptions;
                }
            }

            return string.Empty;
        }

        public string GenerateAutoDescription(string template, AutoDescriptionViewModel viewModel)
        {
            var result = template;

            var templateParser = CreateTemplateParser(viewModel);
            result = templateParser.ParseTemplateString(result);

            //remove space before comma
            var listString = result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in listString)
            {
                item.Trim();
            }
            result = string.Join(", ", listString);

            //remove space before dot
            listString = result.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in listString)
            {
                item.Trim();
            }
            result = string.Join(". ", listString);

            listString = result.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in listString)
            {
                item.Trim();
            }
            result = string.Join("! ", listString);

            result = Regex.Replace(result, @"\s+", " ");
            result = RemoveDummyCharacters(result);
            return result.TrimStart();
        }

        public string GenerateAutoDescriptionForSentences(string sentence, AutoDescriptionViewModel viewModel)
        {
            var result = sentence;

            //var templateParser = new TemplateParser();

            //templateParser.AddTag(Token.Dealership, viewModel.DealershipName);
            //templateParser.AddTag(Token.CarfaxOwner, viewModel.CarfaxOwner == 1 ? "CARFAX 1-Owner" : string.Empty);
            //templateParser.AddTag(Token.WellMaintainedPriorRental, viewModel.PriorRental ? "well maintained prior rental" : string.Empty);
            //templateParser.AddTag(Token.Year, viewModel.Year.ToString(CultureInfo.InvariantCulture));
            //templateParser.AddTag(Token.Make, viewModel.Make);
            //templateParser.AddTag(Token.Model, viewModel.Model);
            //templateParser.AddTag(Token.Trim, viewModel.Trim);
            //templateParser.AddTag(Token.BodyType, viewModel.BodyType);
            //templateParser.AddTag(Token.Transmission, viewModel.Transmission);


            //result = templateParser.ParseTemplateString(result);

            result = result.Replace("[Dealership]", viewModel.DealershipName);

            result = result.Replace(Token.CarfaxOwner, viewModel.CarfaxOwner == 1 ? "CARFAX 1-Owner" : string.Empty);

            result = result.Replace(Token.WellMaintainedPriorRental, viewModel.PriorRental ? " well maintained prior rental" : string.Empty);

            result = result.Replace(Token.Year, viewModel.Year.ToString(CultureInfo.InvariantCulture));

            result = result.Replace(Token.Make, viewModel.Make);

            result = result.Replace(Token.Model, viewModel.Model);

            result = result.Replace(Token.Trim, viewModel.Trim);

            result = result.Replace(Token.BodyType, viewModel.BodyType);

            result = result.Replace(Token.Transmission, viewModel.Transmission);

            result = result.Replace(Token.PopularColor, _popularColors.Contains(viewModel.Color) ? viewModel.Color : string.Empty);



            return result;
        }

        public bool AllowAutoDescription(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var setting = context.Settings.FirstOrDefault(i => i.DealerId == dealerId);
                return setting != null && (setting.AutoDescription.GetValueOrDefault()) ? true : false;
            }
        }

        public bool AllowAutoDescription(Inventory inventory, int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var setting = context.Settings.FirstOrDefault(i => i.DealerId == dealerId);
                return inventory != null && (inventory.EnableAutoDescription.GetValueOrDefault() || inventory.EnableAutoDescription == null) && setting != null && (setting.AutoDescription.GetValueOrDefault()) ? true : false;
            }
        }

        #region Private Methods

        private TemplateParser CreateTemplateParser(AutoDescriptionViewModel viewModel)
        {
            var templateParser = new TemplateParser();

            if (viewModel.PriorRental)
                templateParser.AddTag(Token.RentalUnwindDealerDemo, "Prior Rental.");


            if (viewModel.DealerDemo)
                templateParser.AddTag(Token.RentalUnwindDealerDemo, "Dealer Demo.");


            if (viewModel.Unwind)
                templateParser.AddTag(Token.RentalUnwindDealerDemo, "Unwind.");


            if (viewModel.BrandedTitle)
                templateParser.AddTag(Token.BrandedTitle, "Branded Title.");

            templateParser.AddTag(Token.Dealership, viewModel.DealershipName);
            templateParser.AddTag(Token.CarfaxOwner, viewModel.CarfaxOwner == 1 ? "CARFAX 1-Owner" : string.Empty);
            templateParser.AddTag(Token.Year, viewModel.Year.ToString(CultureInfo.InvariantCulture));
            templateParser.AddTag(Token.Make, viewModel.Make);
            templateParser.AddTag(Token.Model, viewModel.Model);
            templateParser.AddTag(Token.Trim, viewModel.Trim);
            templateParser.AddTag(Token.Transmission, viewModel.Transmission);

            templateParser.AddTag(Token.Certified, viewModel.Certified ? "Certified" : string.Empty);
            templateParser.AddTag(Token.CertifiedBy,
                                  viewModel.Certified ? "Certified by " + viewModel.Make + "." : string.Empty);
            templateParser.AddTag(Token.NewArrival, viewModel.DaysInInventory <= 30 ? "NEW ARRIVAL!!" : string.Empty);
            templateParser.AddTag(Token.Address,
                                  String.IsNullOrEmpty(viewModel.Address) ? Token.Address : viewModel.Address);
            templateParser.AddTag(Token.Phone, String.IsNullOrEmpty(viewModel.Phone) ? Token.Phone : viewModel.Phone);
            templateParser.AddTag(Token.PopularColor,
                                  _popularColors.Contains(viewModel.Color) ? viewModel.Color : string.Empty);


            if (!String.IsNullOrEmpty(viewModel.Disclaimer))
            {
                templateParser.AddTag(Token.Disclaimer, viewModel.Disclaimer);
            }

            if (viewModel.Msrp > 0)
            {
                templateParser.AddTag(Token.MSRP, viewModel.Msrp.ToString());
                templateParser.AddTag(Token.WithMSRP, "MSRP $");
            }

            var combinedPackages = string.Empty;

            if (!String.IsNullOrEmpty(viewModel.Packages))
            {
                var fullFillPackageOptionsList = new List<string>();
                if (viewModel.PackageOptionsList != null)
                    fullFillPackageOptionsList.AddRange(viewModel.PackageOptionsList);
                if (fullFillPackageOptionsList.Count <= viewModel.PackagesList.Length)
                {
                    int loopNumber = viewModel.PackagesList.Length - fullFillPackageOptionsList.Count;
                    for (var i = 0; i < loopNumber; i++)
                    {
                        fullFillPackageOptionsList.Add(string.Empty);
                    }
                }


                for (var i = 0; i < viewModel.PackagesList.Length; i++)
                {
                    if (!String.IsNullOrEmpty(fullFillPackageOptionsList[i]))
                    {

                        combinedPackages += String.Format("the {0} (consisting of {1}),",
                                                          viewModel.PackagesList[i],
                                                          fullFillPackageOptionsList[i].Replace("-inc:", ""));


                    }
                    else
                    {
                        combinedPackages += String.Format("the {0},", viewModel.PackagesList[i]);
                    }
                }

                if (!String.IsNullOrEmpty(combinedPackages))
                    combinedPackages = combinedPackages.Substring(0, combinedPackages.Length - 1);

                templateParser.AddTag(Token.Packages, combinedPackages);

                templateParser.AddTag(Token.WithPackage, ", which comes equipped with");

                if (!String.IsNullOrEmpty(viewModel.AdditionalOptions))
                {
                    templateParser.AddTag(Token.AdditionalOptions,
                                          " as well as the following Options: " + viewModel.AdditionalOptions);
                }
                else
                {
                    templateParser.AddTag(string.Empty);
                }

            }
            else
            {
                if (!String.IsNullOrEmpty(viewModel.AdditionalOptions))
                {
                    templateParser.AddTag(Token.WithPackage, ", which comes equipped with");

                    templateParser.AddTag(Token.AdditionalOptions,
                                          "the following Options: " + viewModel.AdditionalOptions);
                }
                else
                {
                    templateParser.AddTag(string.Empty);
                }
            }

            switch (viewModel.MarketRange)
            {
                case 1:
                    templateParser.AddTag(Token.MarketRange, "Priced below market, this vehicle is sure to sell fast!");
                    break;
                case 2:
                    templateParser.AddTag(Token.MarketRange, "Very competitively priced, this vehicle is a good deal!");
                    break;
                default:
                    templateParser.AddTag(Token.MarketRange, "");
                    break;
            }

            if (viewModel.CarsOnMarket < 30)
            {
                templateParser.AddTag(Token.Rare, viewModel.CarsOnMarket < 30 ? "a hard to find model" : string.Empty);
                templateParser.AddTag(Token.Popular, string.Empty);
                if (viewModel.FuelHighway >= 28)
                {
                    templateParser.AddTag(Token.FuelEfficient,
                                          " and is very fuel efficient, getting " + viewModel.FuelHighway +
                                          " miles per gallon");
                }
                else
                {
                    templateParser.AddTag(Token.FuelEfficient, string.Empty);
                }
            }
            else if (viewModel.CarsOnMarket >= 30)
            {
                templateParser.AddTag(Token.Rare, string.Empty);
                templateParser.AddTag(Token.Popular, viewModel.CarsOnMarket >= 30 ? "a popular model" : string.Empty);
                if (viewModel.FuelHighway >= 28)
                {
                    templateParser.AddTag(Token.FuelEfficient,
                                          " and is very fuel efficient, getting " + viewModel.FuelHighway +
                                          " miles per gallon");
                }
                else
                {
                    templateParser.AddTag(Token.FuelEfficient, string.Empty);
                }
            }

            if (DateTime.Now.Year > viewModel.Year)
            {
                templateParser.AddTag(Token.LowMileage,
                                      viewModel.Mileage / (DateTime.Now.Year - viewModel.Year) <= 12000
                                          ? "It also has low mileage."
                                          : string.Empty);
            }
            else
            {
                templateParser.AddTag(Token.LowMileage, "It also has low mileage.");

            }

            if (viewModel.Certified)
            {


                if (viewModel.Make.ToLower().Equals("land rover") || viewModel.Make.ToLower().Equals("jaguar"))
                {
                    templateParser.AddTag(Token.Warranty,
                                          viewModel.Make + " Certified with 6 year/100,000 mile.");

                }
                else
                {
                    templateParser.AddTag(Token.Warranty,
                                          viewModel.Make + " Certified.");
                }
            }
            else
            {
                switch (viewModel.Warranty)
                {

                    case 1:
                        templateParser.AddTag(Token.Warranty, String.Empty);
                        break;
                    case 2:
                        templateParser.AddTag(Token.Warranty,
                                              "This car is still covered by manufacturer warranty.");
                        break;
                    case 3:
                        if (viewModel.DealerId == 12056 || viewModel.DealerId == 35421)
                            templateParser.AddTag(Token.Warranty, "");
                        else
                        {
                            templateParser.AddTag(Token.Warranty, "This car is still dealer certified.");
                        }

                        break;
                    case 4:
                        templateParser.AddTag(Token.Warranty, "This car is still manufacturer certified.");
                        break;
                    default:
                        templateParser.AddTag(Token.Warranty, String.Empty);
                        break;
                }
            }





            templateParser.AddTag(Token.Address, viewModel.Address);
            templateParser.AddTag(Token.Phone, viewModel.Phone);
            templateParser.AddTag(Token.EndingSentence, viewModel.EndingSentences);
            templateParser.AddTag(Token.BeginningKindOfSentence,
                                  GenerateAutoDescriptionForSentences(
                                      GetRandomizedSentence(1, viewModel.DealershipName), viewModel));
            templateParser.AddTag(Token.NearTheEndSentence,
                                  GenerateAutoDescriptionForSentences(
                                      GetRandomizedSentence(2, viewModel.DealershipName), viewModel));

            if (viewModel.DealerId != 1660)
                templateParser.AddTag(Token.VeryLastSentence,
                                      GetVeryLastSentence(String.Format("http://vinadvisor@DOTcom/trade/{0}/step-one-decode",
                                                                        viewModel.DealershipName.Replace(" ", String.Empty).Replace("-", String.Empty).ToLower())));

            return templateParser;
        }


        private DescriptionTemplate GetRandomizedTemplate(InventoryStatus inventoryStatus)
        {
            using (var context = new VincontrolEntities())
            {
                var random = new Random();
                var status = inventoryStatus.ToString();
                var activeTemplates = context.DescriptionTemplates.Where(i => i.IsActive && i.Type.Equals(status));
                int randomElement = random.Next(0, activeTemplates.Count());
                return activeTemplates.ToList().ElementAt(randomElement);
            }
        }

        private string GetRandomizedSentence(int typeId, string dealershipName)
        {
            using (var context = new VincontrolEntities())
            {
                var random = new Random();
                if (context.Sentences.Any(i => i.SentenceTypeId == typeId))
                {
                    var activeSentences = context.Sentences.Where(i => i.SentenceTypeId == typeId);
                    int randomElement = random.Next(0, activeSentences.Count());
                    var returnSentence = activeSentences.ToList().ElementAt(randomElement).Sentence1;

                    if (returnSentence.Contains("[Dealership]"))
                    {
                        returnSentence = returnSentence.Replace("[Dealership]", dealershipName);
                    }

                    return returnSentence;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string GetVeryLastSentence(string url)
        {
            return String.Format("If you are planning on trading in a car, try our easy-to-use trade-in appraisal calculator! {0}", url);
        }

        private DescriptionTemplate GetTemplate(int templateId)
        {
            using (var context = new VincontrolEntities())
            {
                return context.DescriptionTemplates.FirstOrDefault(i => i.DescriptionTemplateId == templateId);
            }
        }

        //TODO: will replace it with better way
        private string RemoveDummyCharacters(string content)
        {
            content = content.Replace(". .", ".");
            content = content.Replace(" .", ".");
            content = content.Replace("@DOT", ".");
            content = content.Replace("! !", "!");
            content = content.Replace(" ,", ",");

            return content;
        }

        #endregion
    }

    public enum InventoryStatus
    {
        New, Used
    }

    public class Token
    {
        public const string Dealership = "[Dealership]";
        public const string CarfaxOwner = "[CarfaxOwner]";
        public const string Year = "[Year]";
        public const string Make = "[Make]";
        public const string Model = "[Model]";
        public const string Trim = "[Trim]";
        public const string Transmission = "[Transmission]";
        public const string BodyType = "[BodyType]";
        public const string PopularColor = "[PopularColor]";
        public const string WithPackage = "[WithPackage]";
        public const string Packages = "[Packages]";
        public const string AdditionalOptions = "[AdditionalOptions]";
        public const string MarketRange = "[MarketRange]";
        public const string Popular = "[Popular]";
        public const string LowMileage = "[LowMileage]";
        public const string FuelEfficient = "[FuelEfficient]";
        public const string EndingSentence = "[EndingSentence]";
        public const string VeryLastSentence = "[VeryLastSentence]";
        public const string NearTheEndSentence = "[NearTheEndSentence]";
        public const string BeginningKindOfSentence = "[BeginningKindOfSentence]";
        public const string Rare = "[Rare]";

        public const string NewArrival = "[NewArrival]";
        public const string Certified = "[Certified]";
        public const string CertifiedBy = "[CertifiedBy]";
        public const string Warranty = "[Warranty]";
        public const string Address = "[Address]";
        public const string Phone = "[Phone]";
        public const string RentalUnwindDealerDemo = "[RentalUnwindDealerDemo]";
        public const string WellMaintainedPriorRental = " [WellMaintainedPriorRental]";


        public const string BrandedTitle = "[BrandedTitle]";
        public const string Disclaimer = "[Disclaimer]";
        public const string MSRP = "[MSRP]";
        public const string WithMSRP = "[WithMSRP]";

    }

    public class TemplateTag
    {
        #region Constructors

        public TemplateTag()
        {
            _tag = string.Empty;
            _value = string.Empty;
        }

        public TemplateTag(string tag, string value)
        {
            _tag = tag;
            _value = value;
        }
        #endregion

        #region Tag
        public event EventHandler TagChanged;
        protected virtual void OnTagChanged()
        {
            if (TagChanged != null)
                TagChanged(this, EventArgs.Empty);
        }

        private string _tag;
        public string Tag
        {
            get { return _tag; }
            set
            {
                if (_tag == value) return;
                _tag = value;
                OnTagChanged();
            }
        }
        #endregion

        #region Value
        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                OnValueChanged();
            }
        }
        #endregion
    }

    public class TemplateParser
    {
        public TemplateParser() { }

        #region Template Tags - Adding, Updating, Removeing and Clearing Tags

        public void AddTag(TemplateTag templateTag)
        {
            _templateTags[templateTag.Tag] = templateTag;
        }

        public void AddTag(string tag)
        {
            AddTag(new TemplateTag(tag, string.Empty));
        }

        public void AddTag(string tag, string value)
        {
            AddTag(new TemplateTag(tag, value));
        }

        public void RemoveTag(string tag)
        {
            _templateTags.Remove(tag);
        }

        public void ClearTags()
        {
            _templateTags.Clear();
        }
        #endregion

        #region Template Parsers

        private string ReplaceTagHandler(Match token)
        {
            return _templateTags.Contains(token.Value) ? ((TemplateTag)_templateTags[token.Value]).Value : string.Empty;
        }

        public string ParseTemplateString(string template)
        {
            var replaceCallback = new MatchEvaluator(ReplaceTagHandler);
            var newString = Regex.Replace(template, _matchPattern, replaceCallback);

            return newString;
        }

        public string ParseTemplateFile(string templateFilename)
        {
            var fileBuffer = FileToBuffer(templateFilename);
            return ParseTemplateString(fileBuffer);
        }
        #endregion

        #region Find all Template Tags

        public void FindTagsInTemplateString(string template)
        {
            MatchCollection tags = Regex.Matches(template, _matchPattern);

            foreach (Match tag in tags) AddTag(tag.ToString());
        }

        public void FindTagsInTemplateFile(string templateFilename)
        {
            var fileBuffer = FileToBuffer(templateFilename);
            FindTagsInTemplateString(fileBuffer);
        }
        #endregion

        #region Read File into FileBuffer
        private string FileToBuffer(string filename)
        {
            if (!File.Exists(filename)) throw new ArgumentNullException(filename, "Template file does not exist");

            var reader = new StreamReader(filename);
            string fileBuffer = reader.ReadToEnd();
            reader.Close();

            return fileBuffer;
        }
        #endregion

        #region MatchPattern
        private string _matchPattern = @"(\[\w+\])"; // @"(\[%\w+%\])"
        public string MatchPattern
        {
            get { return _matchPattern; }
            set { _matchPattern = value; }
        }
        #endregion

        #region TemplateTags
        private Hashtable _templateTags = new Hashtable();
        public Hashtable TemplateTags
        {
            get { return _templateTags; }
        }
        #endregion
    }
}