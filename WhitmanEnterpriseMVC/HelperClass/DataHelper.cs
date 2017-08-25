using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;
using WhitmanEnterpriseMVC.Interfaces;
using WhitmanEnterpriseMVC.Controllers;
using WhitmanEnterpriseMVC.Models;
using System.Globalization;
using WhitmanEnterpriseMVC.DatabaseModel;
using System.IO;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class PriceChangeItem
    {
        public DateTime ChangedDate { get; set; }
        public decimal ChangedPrice { get; set; }
    }

    public static class DataHelper
    {
        public static IEnumerable<PriceChangeHistory> GetFilter(IEnumerable<PriceChangeHistory> priceChangeHistory, ChartTimeType chartTimeType)
        {
            switch (chartTimeType)
            {
                case ChartTimeType.Last7Days:
                    return GetLast7Days(priceChangeHistory);
                case ChartTimeType.LastMonth:
                    return GetLastMonth(priceChangeHistory);
                case ChartTimeType.FromBeginning:
                    return priceChangeHistory;
                default:
                    return GetThisMonth(priceChangeHistory);
            }
        }

        private static IEnumerable<PriceChangeHistory> GetLast7Days(IEnumerable<PriceChangeHistory> priceChangeHistory)
        {
            return priceChangeHistory.Where(i => i.DateStamp.Date >= DateTime.Now.Date.AddDays(-6));
        }

        private static IEnumerable<PriceChangeHistory> GetThisMonth(IEnumerable<PriceChangeHistory> priceChangeHistory)
        {
            return priceChangeHistory.Where(i => i.DateStamp.Month == DateTime.Now.Month && i.DateStamp.Year == DateTime.Now.Year);
        }

        private static IEnumerable<PriceChangeHistory> GetLastMonth(IEnumerable<PriceChangeHistory> priceChangeHistory)
        {
            return priceChangeHistory.Where(i => i.DateStamp.Month == DateTime.Now.AddMonths(-1).Month && i.DateStamp.Year == DateTime.Now.AddMonths(-1).Year);
        }

        public static IEnumerable<PriceChangeItem> GetFilter(IEnumerable<PriceChangeHistory> priceChangeHistory, ChartTimeType chartTimeType, DateTime? createdDate)
        {
            var priceHistory = priceChangeHistory.GroupBy(i => i.DateStamp.Date).Select(i => new PriceChangeItem { ChangedDate = i.Key, ChangedPrice = i.OrderByDescending(j => j.DateStamp).FirstOrDefault().NewSalePrice }).ToList();
            if (createdDate.HasValue)
            {
                priceHistory.Add(new PriceChangeItem { ChangedDate = createdDate.Value, ChangedPrice = priceChangeHistory.OrderBy(i => i.DateStamp).Select(i => i.OldSalePrice).FirstOrDefault() });
            }

            switch (chartTimeType)
            {
                case ChartTimeType.Last7Days:
                    return GetLast7Days(priceHistory);
                case ChartTimeType.LastMonth:
                    return GetLastMonth(priceHistory);
                case ChartTimeType.FromBeginning:
                    return GetPriceList(priceHistory, priceHistory.Min(i => i.ChangedDate).Date, GetMaxDate(priceHistory.Max(i => i.ChangedDate), DateTime.Now.Date), DateTime.Now.Date);
                default:
                case ChartTimeType.ThisMonth:
                    return GetThisMonth(priceHistory);
            }
        }

        private static DateTime GetMaxDate(DateTime date, DateTime otherDate)
        {
            return date >= otherDate ? date : otherDate;
        }

        private static IEnumerable<PriceChangeItem> GetLast7Days(IEnumerable<PriceChangeItem> priceChangeHistory)
        {
            return GetPriceList(priceChangeHistory, DateTime.Now.Date.AddDays(-6), DateTime.Now.Date, DateTime.Now.Date);
        }

        private static IEnumerable<PriceChangeItem> GetPriceList(IEnumerable<PriceChangeItem> priceChangeHistory, DateTime startDate, DateTime endDate, DateTime maxLimitDate)
        {
            var continuousPriceChangeHistory = new List<PriceChangeItem>();
            for (var changeDate = startDate; changeDate <= endDate && changeDate <= maxLimitDate; changeDate = changeDate.AddDays(1))
            {
                var price = priceChangeHistory.Where(i => i.ChangedDate <= changeDate).OrderByDescending(i => i.ChangedDate).
                        FirstOrDefault();
                if (price != null)
                {
                    continuousPriceChangeHistory.Add(new PriceChangeItem()
                    {
                        ChangedDate = changeDate,
                        ChangedPrice = price.ChangedPrice
                    });
                }
            }
            return continuousPriceChangeHistory;
        }

        private static IEnumerable<PriceChangeItem> GetThisMonth(IEnumerable<PriceChangeItem> priceChangeHistory)
        {
            return GetPriceList(priceChangeHistory, PDFController.FirstDayOfMonthFromDateTime(DateTime.Now), PDFController.LastDayOfMonthFromDateTime(DateTime.Now), DateTime.Now.Date);
        }

        private static IEnumerable<PriceChangeItem> GetLastMonth(IEnumerable<PriceChangeItem> priceChangeHistory)
        {
            return GetPriceList(priceChangeHistory, PDFController.FirstDayOfMonthFromDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)), PDFController.LastDayOfMonthFromDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)), DateTime.Now.Date);
        }

        public static IEnumerable<PriceChangeHistory> GetPriceChangeList(string listingId, ChartTimeType type, int inventoryStatus)
        {
            var priceChangeList = new List<PriceChangeHistory>();

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var convertedListingId = Convert.ToInt32(listingId);
                if (inventoryStatus == 1)
                {
                    var history =
                        context.vincontrolpricechangeshistories.Where(
                            i => i.ListingId == convertedListingId && i.Type.ToLower().Equals("inventory")).ToList();
                    if (history.Count > 0)
                    {
                        priceChangeList = history.Select(i => new PriceChangeHistory()
                            {
                                AttachFile = i.AttachFile,
                                UserStamp = i.UserStamp,
                                DateStamp = i.DateStamp.Value,
                                NewSalePrice = i.NewPrice.Value,
                                OldSalePrice = i.OldPrice.Value,
                                ListingId = i.ListingId.Value
                            }).ToList();

                    }
                }
                else
                {
                    var soldCard =
                        context.whitmanenterprisedealershipinventorysoldouts.First(i => i.ListingID == convertedListingId);

                    if (soldCard.OldListingId.GetValueOrDefault() > 0)
                    {
                        var history =
                    context.vincontrolpricechangeshistories.Where(
                        i => i.ListingId == soldCard.OldListingId && i.Type.ToLower().Equals("inventory")).ToList();
                        if (history.Count > 0)
                        {
                            priceChangeList = history.Select(i => new PriceChangeHistory()
                            {
                                AttachFile = i.AttachFile,
                                UserStamp = i.UserStamp,
                                DateStamp = i.DateStamp.Value,
                                NewSalePrice = i.NewPrice.Value,
                                OldSalePrice = i.OldPrice.Value,
                                ListingId = i.ListingId.Value
                            }).ToList();

                        }
                    }
                }
            }
            return GetFilter(priceChangeList, type);
        }

        public static IEnumerable<PriceChangeItem> GetPriceChangeListForChart(string listingId, ChartTimeType type, DateTime createdDate, int inventoryStatus)
        {
            var priceChangeList = new List<PriceChangeHistory>();

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var convertedListingId = Convert.ToInt32(listingId);
                if (inventoryStatus == 1)
                {

                    var history =
                        context.vincontrolpricechangeshistories.Where(
                            i => i.ListingId == convertedListingId && i.Type.ToLower().Equals("inventory")).ToList();
                    if (history.Count > 0)
                    {
                        priceChangeList = history.Select(i => new PriceChangeHistory()
                            {
                                AttachFile = i.AttachFile,
                                UserStamp = i.UserStamp,
                                DateStamp = i.DateStamp.Value,
                                NewSalePrice = i.NewPrice.Value,
                                OldSalePrice = i.OldPrice.Value,
                                ListingId = i.ListingId.Value
                            }).ToList();

                    }
                }
                else
                {
                    var soldCard =
                        context.whitmanenterprisedealershipinventorysoldouts.First(i => i.ListingID == convertedListingId);

                    if (soldCard.OldListingId.GetValueOrDefault() > 0)
                    {
                        var history =
                    context.vincontrolpricechangeshistories.Where(
                        i => i.ListingId == soldCard.OldListingId && i.Type.ToLower().Equals("inventory")).ToList();
                        if (history.Count > 0)
                        {
                            priceChangeList = history.Select(i => new PriceChangeHistory()
                            {
                                AttachFile = i.AttachFile,
                                UserStamp = i.UserStamp,
                                DateStamp = i.DateStamp.Value,
                                NewSalePrice = i.NewPrice.Value,
                                OldSalePrice = i.OldPrice.Value,
                                ListingId = i.ListingId.Value
                            }).ToList();

                        }
                    }
                }
            }
            return GetFilter(priceChangeList, type, createdDate);
        }

        public static DateTime? GetCreatedDate(string listingId, int inventoryStatus)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int id = int.Parse(listingId);
                if(inventoryStatus==1)
                    return context.whitmanenterprisedealershipinventories.Where(i => i.ListingID == id).Select(i => i.DateInStock).FirstOrDefault();
                else
                {
                    return context.whitmanenterprisedealershipinventorysoldouts.Where(i => i.ListingID == id).Select(i => i.DateInStock).FirstOrDefault();
                }

            }
        }

        public static AppraisalViewFormModel GetAppraisalViewModel(string vin)
        {
            var viewModel = new AppraisalViewFormModel();

            var autoService = new ChromeAutoService();

            var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);

            if (vehicleInfo != null)
            {
                viewModel = ConvertHelper.GetVehicleInfoFromChromeDecode(vehicleInfo);

                viewModel.AppraisalDate = DateTime.Now.ToShortDateString();

            }

            if (viewModel.IsTruck)
            {
                viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                viewModel.TruckClassList = SelectListHelper.InitalTruckClassList();
            }
            return viewModel;
        }

        public static JavaScriptModel GetJavaScripModel(string vin, DealershipViewModel dealer)
        {
            var autoService = new ChromeAutoService();
            var model = new JavaScriptModel();

            if (SQLHelper.CheckVinExist(vin, dealer))
            {
                var context = new whitmanenterprisewarehouseEntities();
                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.VINNumber == vin && x.DealershipId == dealer.DealershipId);
                model.ListingId = row.ListingID.ToString(CultureInfo.InvariantCulture);
                model.Status = "Inventory";
            }
            else if (SQLHelper.CheckVinExistInSoldOut(vin, dealer))
            {
                var context = new whitmanenterprisewarehouseEntities();
                var row = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.VINNumber == vin && x.DealershipId == dealer.DealershipId);
                model.ListingId = row.ListingID.ToString(CultureInfo.InvariantCulture);
                model.Status = "SoldOut";
            }
            else if (SQLHelper.CheckVinExistInAppraisal(vin, dealer))
            {
                var context = new whitmanenterprisewarehouseEntities();
                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.VINNumber == vin && x.DealershipId == dealer.DealershipId);
                model.AppraisalId = row.idAppraisal.ToString(CultureInfo.InvariantCulture);
                model.Status = "Appraisal";
                //return RedirectToAction("ViewProfileForAppraisal", "Appraisal", new { AppraisalId = row["AppraisalID"].ToString() });
            }
            else
            {
                var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);
                if (vehicleInfo != null)
                {
                    model.Vin = vin;
                    model.Status = "VinProcessing";
                }
                else
                {
                    model.Vin = vin;
                    model.Status = "VinInvalid";
                }
            }

            return model;
        }

        public static MemoryStream GetCustomerInfoStream(int id, DealershipViewModel dealerSessionInfo)
        {
            TradeInVehicleModel vehicle = GetTradeinVehicle(id);
            MemoryStream workStream = PDFHelper.WritePDF(EmailHelper.CreateBannerBodyForPdf(vehicle, dealerSessionInfo));
            return workStream;
        }

        public static TradeInVehicleModel GetTradeinVehicle(int id)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var customer = context.vincontrolbannercustomers.Where(e => e.TradeInCustomerId == id).FirstOrDefault();
            decimal result;

            TradeInVehicleModel vehicle = new TradeInVehicleModel()
            {
                Condition = customer.Condition,
                CustomerEmail = customer.Email,
                CustomerFirstName = customer.FirstName,
                CustomerLastName = customer.LastName,
                CustomerPhone = customer.Phone,
                SelectedMake = customer.Make,
                SelectedModel = customer.Model,
                SelectedTrim = customer.Trim,
                SelectedYear = customer.Year.HasValue ? customer.Year.Value.ToString() : String.Empty,
                Mileage = customer.Mileage.HasValue ? customer.Mileage.Value.ToString() : String.Empty,
                SelectedOptionList = customer.SelectedOptions,
                TradeInFairPrice = decimal.TryParse(customer.TradeInFairValue, out result) ? result.ToString("c0") : customer.TradeInFairValue
            };
            return vehicle;
        }

        private static IEnumerable<SelectListItem> InitializeOptions(Standard[] options)
        {
            var results = new List<SelectListItem>();
            var tempTable = new Hashtable();

            if (options != null && options.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                options =
                    options.Where(
                        option => option.header.Equals("INTERIOR") || option.header.Equals("ENTERTAINMENT")).Take(32)
                        .ToArray();


                foreach (var option in options)
                {

                    string name = option.description;
                    var newString = regex.Replace(name, new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    if (!tempTable.Contains(name) && !name.Contains("PKG") &&
                        !name.Contains("PACKAGE"))
                    {
                        string uniqueOption = newString;// + " " + option.msrp.highValue.ToString("C");
                        results.Add(new SelectListItem() { Selected = false, Text = uniqueOption, Value = uniqueOption });
                        tempTable.Add(name, option);
                    }

                }
            }

            return results;
        }

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

        public static InspectionAppraisalViewModel GetPendingAppraisal(int id)
        {
            var model = new InspectionAppraisalViewModel();
            //try
            //{
            var receivedAppraisalId = Convert.ToInt32(id);
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var existingAppraisal = context.whitmanenterpriseappraisals.Where(a => a.idAppraisal == receivedAppraisalId).FirstOrDefault();
                if (existingAppraisal != null)
                {
                    model.AppraisalInfo = new AppraisalInfo()
                    {
                        AppraisalId = existingAppraisal.idAppraisal,
                        VinNumber = existingAppraisal.VINNumber ?? "",
                        StockNumber = existingAppraisal.StockNumber ?? "",
                        Year = existingAppraisal.ModelYear ?? 2012,
                        Make = existingAppraisal.Make ?? "",
                        Model = existingAppraisal.Model ?? "",
                        Trim = existingAppraisal.Trim ?? "",
                        ExteriorColor = existingAppraisal.ExteriorColor ?? "",
                        InteriorColor = existingAppraisal.InteriorColor ?? "",
                        Transmission = existingAppraisal.Tranmission ?? "",
                        Odometer = existingAppraisal.Mileage ?? "",
                        Cylinders = existingAppraisal.Cylinders ?? "",
                        Liters = existingAppraisal.Liters ?? "",
                        Doors = existingAppraisal.Doors ?? "",
                        Fuel = existingAppraisal.FuelType ?? "",
                        MSRP = existingAppraisal.MSRP ?? "",
                        DriveType = existingAppraisal.DriveTrain ?? "",
                        ImageUrl = existingAppraisal.DefaultImageUrl,
                        Photo = existingAppraisal.Photo,
                        Options = String.IsNullOrEmpty(existingAppraisal.CarsOptions) ? new List<string>() : existingAppraisal.CarsOptions.Split(',').ToList(),
                        Packages = String.IsNullOrEmpty(existingAppraisal.CarsPackages) ? new List<string>() : existingAppraisal.CarsPackages.Split(',').ToList(),
                        StandardOptions = String.IsNullOrEmpty(existingAppraisal.StandardOptions) ? new List<string>() : existingAppraisal.StandardOptions.Split(',').ToList(),
                        AppraisalDate = existingAppraisal.DateStamp.HasValue ? existingAppraisal.DateStamp.Value.ToShortDateString() : String.Empty,
                        AppraisalTime = existingAppraisal.DateStamp.HasValue ? existingAppraisal.DateStamp.Value.ToShortTimeString() : String.Empty,
                        EngineType = existingAppraisal.EngineType,
                        AppraisalBy = GetAppraisalName(existingAppraisal.UserStamp)
                    };

                    var autoService = new ChromeAutoService();
                    if (!(String.IsNullOrEmpty(model.AppraisalInfo.VinNumber)) && model.AppraisalInfo.VinNumber != "None")
                    {
                        var vehicleInfo = autoService.GetVehicleInformationFromVin(model.AppraisalInfo.VinNumber);
                        model.AppraisalInfo.StandardOptions = InitializeOptions(vehicleInfo.standard).Select(o => o.Text).ToList();
                    }
                    else
                    {
                        IdentifiedString[] divisionList = autoService.GetDivisions(model.AppraisalInfo.Year);
                        var make = SelectListHelper.InitialMakeList(divisionList).Where(m => m.Text.Equals(model.AppraisalInfo.Make)).First();
                        var chromeModel = autoService.GetModelsByDivision(model.AppraisalInfo.Year, Convert.ToInt32(make.Value.Split('|')[0])).Where(m => m.Value.Equals(model.AppraisalInfo.Model)).FirstOrDefault();
                        if (chromeModel != null)
                        {
                            var styles = autoService.GetStyles(chromeModel.id);
                            int styleId = Convert.ToInt32(styles.First().id);
                            var styleInfo = autoService.GetStyleInformationFromStyleId(styleId);
                            model.AppraisalInfo.StandardOptions = InitializeOptions(styleInfo.standard).Select(o => o.Text).ToList();
                        }
                    }

                    model.CustomerInfo = new CustomerInfo()
                    {
                        FirstName = existingAppraisal.FirstName,
                        LastName = existingAppraisal.LastName,
                        Phone = existingAppraisal.Phone,
                        Email = existingAppraisal.Email,
                        City = existingAppraisal.City,
                        State = existingAppraisal.State,
                        Zip = existingAppraisal.ZipCode,
                        Street = existingAppraisal.Street,
                        Signature = existingAppraisal.Signature
                    };

                    var walkarounds = context.vincontrolwalkarounds.Where(i => i.whitmanenterpriseappraisal.idAppraisal == receivedAppraisalId).OrderBy(i => i.order).ToList();
                    if (walkarounds.Count > 0)
                    {
                        model.WalkaroundInfo = walkarounds.Select(w => new WalkaroundInfo()
                        {
                            Order = w.order ?? 0,
                            Note = GetNoteContent(w.note, w.order),
                            X = w.x ?? 0,
                            Y = w.y ?? 0
                        });
                    }

                    //var answers =
                    //    context.vincontrolanswers.Where(i => i.whitmanenterpriseappraisal.idAppraisal == receivedAppraisalId).Select(
                    //        a => new AppraisalAnswer()
                    //        {
                    //            Question = a.vincontrolquestions.shortdescription,
                    //            Answer = a.answer,
                    //            Comment = a.comment,
                    //            QuestionType = a.vincontrolquestions.vincontrolquestiontype.code??4                                
                    //        }).ToList();

                    var answers =
                       context.vincontrolanswers.Where(i => i.whitmanenterpriseappraisal.idAppraisal == receivedAppraisalId).Select(
                           a => new
                           {
                               QuestionId = a.vincontrolquestion.questionid,
                               Question = a.vincontrolquestion.shortdescription,
                               Answer = a.answer,
                               Comment = a.comment,
                               QuestionType = a.vincontrolquestion.vincontrolquestiontype.code ?? 4,
                               Order = a.vincontrolquestion.order

                           }).ToList();
                    //AppraisalAnswer()
                    var questions = context.vincontrolquestions.Include("vincontrolquestiontype");
                    var list = new List<AppraisalAnswer>();
                    foreach (var question in questions)
                    {
                        var answer = answers.FirstOrDefault(a => a.QuestionId == question.questionid);
                        if (answer != null)
                        {
                            list.Add(new AppraisalAnswer()
                                {
                                    Question = answer.Question,
                                    Answer = GetAnswer(answer.Answer, question.vincontrolquestiontype.unit),
                                    Comment = GetShortString(answer.Comment),
                                    QuestionType = answer.QuestionType,
                                    Order = answer.Order ?? 0
                                });
                        }
                        else
                        {
                            list.Add(new AppraisalAnswer()
                            {
                                Question = question.shortdescription,
                                Answer = String.Empty,
                                Comment = String.Empty,
                                QuestionType = question.vincontrolquestiontype.code ?? 4,
                                Order = question.order ?? 0
                            });
                        }
                    }

                    if (answers.Count > 0)
                    {
                        model.AppraisalAnswer = list.OrderBy(o => o.Order);
                    }
                    else
                    {
                        model.AppraisalAnswer = list;
                    }
                }
            }
            //}
            //catch (Exception)
            //{

            //}

            return model;

        }

        private static string GetAnswer(string answer, string unit)
        {
            if (String.IsNullOrEmpty(unit) || answer.Contains(unit))
            {
                return answer;
            }

            if (string.IsNullOrEmpty(answer))
            {
                return string.Empty;
            }

            return string.Format("{0} {1}(s)", answer, unit);

        }

        private static string GetNoteContent(string content, int? order)
        {
            if (order == null)
            {
                return content;
            }

            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            switch (order.Value)
            {
                case 1:
                    return string.Format("Front driver side tire: {0}", content);
                case 2:
                    return string.Format("Front passenger side tire: {0}", content);
                case 3:
                    return string.Format("Rear driver side tire: {0}", content);
                case 4:
                    return string.Format("Rear passenger side tire: {0}", content);
                default:
                    return content;
            }
        }

        private static string GetAppraisalName(string name)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var user = (from u in context.whitmanenterpriseusers
                        where u.UserName == name
                        select u).FirstOrDefault();
            if (user != null)
            {
                return user.Name;
            }
            else
            {
                var masterUser = (from u in context.whitmanenterprisedealergroups
                                  where u.MasterUserName == name
                                  select u).FirstOrDefault();
                if (masterUser != null)
                {
                    return masterUser.DealerGroupName;
                }
                else
                    return String.Empty;
            }

        }

        private static string GetShortString(string content)
        {
            if (content.Length <= 43)
            {
                return content;
            }
            else
            {
                string otherString = content.Substring(0, 45);
                if (content[43] == ' ')
                {
                    return otherString;
                }
                else
                {
                    int index = otherString.LastIndexOf(' ');
                    return otherString.Substring(0, index);
                }
            }
        }

        public static string FilterCarModelForMarket(string modelWord)
        {
            if (!String.IsNullOrEmpty(modelWord))
            {
                modelWord = modelWord.Replace("Sdn", "");

                modelWord = modelWord.Replace("XL", "");

             

                modelWord = modelWord.Replace("Sedan", "");

                modelWord = modelWord.Replace("Wagon", "");

                modelWord = modelWord.Replace("Coupe", "");

                modelWord = modelWord.Replace("Cpe", "");

                modelWord = modelWord.Replace("Convertible", "");

                modelWord = modelWord.Replace("Utility", "");

                modelWord = modelWord.Replace("New", "");

                modelWord = modelWord.Replace("Hybrid", "");

                modelWord = modelWord.Replace("2D", "");

                modelWord = modelWord.Replace("4D", "");

                modelWord = modelWord.Replace("4MATIC", "");

                modelWord = modelWord.Replace("Unlimited", "");

                if (modelWord.ToLower().Contains("silverado 2500hd classic"))
                    modelWord = "Silverado 2500";

                if (modelWord.ToLower().Contains("silverado 2500hd"))
                    modelWord = "Silverado 2500";

                if (modelWord.ToLower().Contains("silverado 3500hd"))
                    modelWord = "Silverado 2500";

                if (modelWord.Contains("Super Duty"))
                    modelWord = modelWord.Replace("Super Duty", "");

                modelWord = modelWord.Replace(" ", "");

                modelWord = modelWord.Replace("-", "");

                modelWord = modelWord.Replace("4WDTruck", "");

                modelWord = modelWord.Replace("2WDTruck", "");

                modelWord = modelWord.Replace("AWDTruck", "");

                modelWord = modelWord.Replace("SRW", "");



                return modelWord.Trim();

            }

            else
                return String.Empty;
        }


        public static IQueryable<MarketCarInfo> GetNationwideMarketData(IQueryable<MarketCarInfo> query, string make,
                                                                        string modelWord, string trim)
        {
            if (make.ToLower().Equals("bmw") && modelWord.ToLower().Contains("series"))
            {
                modelWord = trim;
            }

            if (make.ToLower().Equals("mercedes-benz") && modelWord.ToLower().Contains("class"))
            {
                modelWord = trim;

                modelWord = modelWord.Replace("Sport", "");

                modelWord = modelWord.Replace("Luxry", "");

                modelWord = modelWord.Replace("BlueTEC", "");

                modelWord = modelWord.Replace("BTC", "");

                modelWord = modelWord.Replace("CDI", "");

                modelWord = modelWord.Replace("BLK", "");

                if (modelWord.Length >= 2)
                {

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("S4"))

                        modelWord = modelWord.Replace("S4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("V4"))

                        modelWord = modelWord.Replace("V4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("W4"))

                        modelWord = modelWord.Replace("W4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("AE"))

                        modelWord = modelWord.Replace("AE", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("WZ"))

                        modelWord = modelWord.Replace("WZ", "");
                }

                if (modelWord[modelWord.Length - 1] == 'W')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);


                if (modelWord[modelWord.Length - 1] == 'R')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);



                if (modelWord[modelWord.Length - 1] == 'V')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);
                if (modelWord[modelWord.Length - 1] == 'C')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);


                if (modelWord[modelWord.Length - 1] == 'A')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);
                if (modelWord[modelWord.Length - 1] == 'K')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

            }

            if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xk"))
            {
                modelWord = "xk";
            }
            if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xj"))
            {
                modelWord = "xj";
            }

            modelWord = FilterCarModelForMarket(modelWord);

            if (!make.ToLower().Equals("land rover"))
            {

                var result = query.Where(i => i.Make == make &&
                                              ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                              i.CurrentPrice > 0 && i.Mileage > 0);

                return result;
            }

            else
            {
                if (modelWord.ToLower().Contains("rangeroversport"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroversport") &&
                                                   i.CurrentPrice > 0 && i.Mileage > 0));
                    return result;
                }
                else if (modelWord.ToLower().Contains("rangeroverevoque"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroverevoque") &&
                                                   i.CurrentPrice > 0 && i.Mileage > 0));
                    return result;
                }
                else if (modelWord.ToLower().Equals("rangerover"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                 ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("rangerover") &&
                                                   !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("sport") &&
                                                       !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("evoque") &&
                                                i.CurrentPrice > 0 && i.Mileage > 0));

                    return result;
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                           ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                             i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                            i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }





        }

        public static IQueryable<MarketCarInfo> GetStateMarketData(IQueryable<MarketCarInfo> query, string make, string modelWord, string state, string trim)
        {
            if (make.ToLower().Equals("bmw") && modelWord.ToLower().Contains("series"))
            {
                modelWord = trim;
            }

            if (make.ToLower().Equals("mercedes-benz") && modelWord.ToLower().Contains("class"))
            {
                modelWord = trim;

                modelWord = modelWord.Replace("Sport", "");

                modelWord = modelWord.Replace("Luxry", "");

                modelWord = modelWord.Replace("BlueTEC", "");

                modelWord = modelWord.Replace("BTC", "");

                modelWord = modelWord.Replace("CDI", "");

                modelWord = modelWord.Replace("BLK", "");

                if (modelWord.Length >= 2)
                {

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("S4"))

                        modelWord = modelWord.Replace("S4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("V4"))

                        modelWord = modelWord.Replace("V4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("W4"))

                        modelWord = modelWord.Replace("W4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("AE"))

                        modelWord = modelWord.Replace("AE", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("WZ"))

                        modelWord = modelWord.Replace("WZ", "");
                }

        
                if (modelWord[modelWord.Length - 1] == 'W')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);


                if (modelWord[modelWord.Length - 1] == 'R')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);



                if (modelWord[modelWord.Length - 1] == 'V')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);
                if (modelWord[modelWord.Length - 1] == 'C')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);


                if (modelWord[modelWord.Length - 1] == 'A')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);
                if (modelWord[modelWord.Length - 1] == 'K')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

            }

            if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xk"))
            {
                modelWord = "xk";
            }

            modelWord = FilterCarModelForMarket(modelWord);

            if (!make.ToLower().Equals("land rover"))
            {

                var result = query.Where(i => i.State == state && i.Make == make &&
                                              ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                i.Trim.Replace(" ", "").Trim().ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                              i.CurrentPrice > 0 && i.Mileage > 0);

                return result;
            }

            else
            {
                if (modelWord.ToLower().Contains("rangeroversport"))
                {
                    var result = query.Where(i => i.State == state && i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroversport") &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0));

                    return result;
                }
                else if (modelWord.ToLower().Contains("rangeroverevoque"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroverevoque") &&
                                                   i.CurrentPrice > 0 && i.Mileage > 0));
                    return result;
                }
                else if (modelWord.ToLower().Equals("rangerover"))
                {
                    var result = query.Where(i => i.State == state && i.Make == make &&
                                                 ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("rangerover") &&
                                                   !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("sport") &&
                                                         !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("evoque") &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0));

                    return result;
                }
                else
                {
                    var result = query.Where(i => i.State == state && i.Make == make &&
                                           ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                             i.Trim.Replace(" ", "").Trim().ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                           i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }





        }


        public static IQueryable<manheim_auctions> GetManheimAuctionMarketData(int year,string make, string modelWord, string trim)
        {
            var context = new vincontrolscrappingEntities();

            if (make.ToLower().Equals("bmw") && modelWord.ToLower().Contains("series"))
            {
                modelWord = trim;
            }

            if (make.ToLower().Equals("mercedes-benz") && modelWord.ToLower().Contains("class"))
            {
                modelWord = trim;

                modelWord = modelWord.Replace("Sport", "");

                modelWord = modelWord.Replace("Luxry", "");

                modelWord = modelWord.Replace("BlueTEC", "");

            
                modelWord = modelWord.Replace("BTC", "");

                modelWord = modelWord.Replace("CDI", "");

                modelWord = modelWord.Replace("BLK", "");

           


                if (modelWord.Length >= 2)
                {

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("S4"))

                        modelWord = modelWord.Replace("S4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("V4"))

                        modelWord = modelWord.Replace("V4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("W4"))

                        modelWord = modelWord.Replace("W4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("AE"))

                        modelWord = modelWord.Replace("AE", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("WZ"))

                        modelWord = modelWord.Replace("WZ", "");
                }

                if (modelWord[modelWord.Length - 1] == 'W')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);


                if (modelWord[modelWord.Length - 1] == 'R')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);



                if (modelWord[modelWord.Length - 1] == 'V')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);
                if (modelWord[modelWord.Length - 1] == 'C')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);


                if (modelWord[modelWord.Length - 1] == 'A')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);
                if (modelWord[modelWord.Length - 1] == 'K')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

            }

            modelWord = FilterCarModelForMarket(modelWord);

            if (!make.ToLower().Equals("land rover"))
            {

                var result = context.manheim_auctions.Where(i =>i.Year==year&& i.Make == make &&
                                              ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                i.Trim.Replace(" ", "").Trim().ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) && i.Mmr > 0);

                return result;
            }

            else
            {
                if (modelWord.ToLower().Contains("rangeroversport"))
                {
                    var result = context.manheim_auctions.Where(i => i.Year == year && i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroversport") 
                                                   && i.Mmr > 0));

                    return result;
                }
                else if (modelWord.ToLower().Contains("rangeroverevoque"))
                {
                    var result = context.manheim_auctions.Where(i => i.Year == year && i.Make == make &&
                                                     ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                       i.Trim.Replace(" ", "").ToLower()).Contains("rangeroverevoque")
                                                      && i.Mmr > 0));

                    return result;
                }
                else if (modelWord.ToLower().Equals("rangerover"))
                {
                    var result = context.manheim_auctions.Where(i => i.Year == year && i.Make == make &&
                                                 ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("rangerover") &&
                                                   !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("sport")
                                                   && !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("evoque") && i.Mmr > 0));

                    return result;
                }
                else
                {
                    var result = context.manheim_auctions.Where(i => i.Year == year && i.Make == make &&
                                           ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                             i.Trim.Replace(" ", "").Trim().ToLower()).Contains(modelWord.Replace(" ", "").ToLower()))
                                           && i.Mmr > 0);

                    return result;
                }
            }





        }
    }
}
