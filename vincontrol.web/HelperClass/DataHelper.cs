using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.ChromeAutoService;
//using Vincontrol.Web.com.chromedata.services.Description7a;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.Controllers;
using Vincontrol.Web.Handlers;
//using Vincontrol.Web.Interfaces;
using Vincontrol.Web.Models;
using IdentifiedString = vincontrol.ChromeAutoService.AutomativeService.IdentifiedString;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.HelperClass
{
    public class PriceChangeItem
    {
        public DateTime ChangedDate { get; set; }
        public decimal ChangedPrice { get; set; }
    }

    public static class DataHelper
    {
        public static IEnumerable<Models.PriceChangeHistory> GetFilter(IEnumerable<Models.PriceChangeHistory> priceChangeHistory, ChartTimeType chartTimeType)
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

        private static IEnumerable<Models.PriceChangeHistory> GetLast7Days(IEnumerable<Models.PriceChangeHistory> priceChangeHistory)
        {
            return priceChangeHistory.Where(i => i.DateStamp.Date >= DateTime.Now.Date.AddDays(-6));
        }

        private static IEnumerable<Models.PriceChangeHistory> GetThisMonth(IEnumerable<Models.PriceChangeHistory> priceChangeHistory)
        {
            return priceChangeHistory.Where(i => i.DateStamp.Month == DateTime.Now.Month && i.DateStamp.Year == DateTime.Now.Year);
        }

        private static IEnumerable<Models.PriceChangeHistory> GetLastMonth(IEnumerable<Models.PriceChangeHistory> priceChangeHistory)
        {
            return priceChangeHistory.Where(i => i.DateStamp.Month == DateTime.Now.AddMonths(-1).Month && i.DateStamp.Year == DateTime.Now.AddMonths(-1).Year);
        }

        public static IEnumerable<PriceChangeItem> GetFilter(IEnumerable<Models.PriceChangeHistory> priceChangeHistory, ChartTimeType chartTimeType, DateTime? createdDate)
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

        public static IEnumerable<Models.PriceChangeHistory> GetPriceChangeList(string listingId, ChartTimeType type, int inventoryStatus)
        {
            var priceChangeList = new List<Models.PriceChangeHistory>();

            using (var context = new VincontrolEntities())
            {
                var convertedListingId = Convert.ToInt32(listingId);
                if (inventoryStatus == 1)
                {
                    var history =
                        context.PriceChangeHistories.Where(
                            i => i.ListingId == convertedListingId && i.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory).ToList();
                    if (history.Count > 0)
                    {
                        priceChangeList = history.Select(i => new Models.PriceChangeHistory()
                            {
                                AttachFile = i.AttachFile,
                                UserStamp = i.UserStamp,
                                DateStamp = i.DateStamp.Value,
                                NewSalePrice = i.NewPrice.Value,
                                OldSalePrice = i.OldPrice.Value,
                                ListingId = i.ListingId
                            }).ToList();

                    }
                }
                else
                {
                    var soldCard =
                        context.SoldoutInventories.First(i => i.SoldoutInventoryId == convertedListingId);

                    //if (soldCard.OldListingId.GetValueOrDefault() > 0)
                    //{
                    var history =
                context.PriceChangeHistories.Where(
                    i => i.ListingId == soldCard.SoldoutInventoryId && i.VehicleStatusCodeId == Constanst.VehicleStatus.SoldOut).ToList();
                    if (history.Count > 0)
                    {
                        priceChangeList = history.Select(i => new Models.PriceChangeHistory()
                        {
                            AttachFile = i.AttachFile,
                            UserStamp = i.UserStamp,
                            DateStamp = i.DateStamp.Value,
                            NewSalePrice = i.NewPrice.Value,
                            OldSalePrice = i.OldPrice.Value,
                            ListingId = i.ListingId
                        }).ToList();

                    }
                    //}
                }
            }
            return GetFilter(priceChangeList, type);
        }

        public static IEnumerable<PriceChangeItem> GetPriceChangeListForChart(string listingId, ChartTimeType type, DateTime createdDate, int inventoryStatus)
        {
            var priceChangeList = new List<Models.PriceChangeHistory>();

            using (var context = new VincontrolEntities())
            {
                var convertedListingId = Convert.ToInt32(listingId);
                if (inventoryStatus == 1)
                {

                    var history =
                        context.PriceChangeHistories.Where(
                            i => i.ListingId == convertedListingId && i.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory).ToList();
                    if (history.Count > 0)
                    {
                        priceChangeList = history.Select(i => new Models.PriceChangeHistory()
                            {
                                AttachFile = i.AttachFile,
                                UserStamp = i.UserStamp,
                                DateStamp = i.DateStamp.Value,
                                NewSalePrice = i.NewPrice.Value,
                                OldSalePrice = i.OldPrice.Value,
                                ListingId = i.ListingId
                            }).ToList();

                    }
                }
                else
                {
                    var soldCard =
                        context.SoldoutInventories.First(i => i.SoldoutInventoryId == convertedListingId);

                    //if (soldCard.OldListingId.GetValueOrDefault() > 0)
                    //{
                    var history =
                context.PriceChangeHistories.Where(
                    i => i.ListingId == soldCard.SoldoutInventoryId && i.VehicleStatusCodeId == Constanst.VehicleStatus.SoldOut).ToList();
                    if (history.Count > 0)
                    {
                        priceChangeList = history.Select(i => new Models.PriceChangeHistory()
                        {
                            AttachFile = i.AttachFile,
                            UserStamp = i.UserStamp,
                            DateStamp = i.DateStamp.Value,
                            NewSalePrice = i.NewPrice.Value,
                            OldSalePrice = i.OldPrice.Value,
                            ListingId = i.ListingId
                        }).ToList();

                    }
                    //}
                }
            }
            return GetFilter(priceChangeList, type, createdDate);
        }

        public static DateTime? GetCreatedDate(string listingId, int inventoryStatus)
        {
            using (var context = new VincontrolEntities())
            {
                int id = int.Parse(listingId);
                if (inventoryStatus == 1)
                    return context.Inventories.Where(i => i.InventoryId == id).Select(i => i.DateInStock).FirstOrDefault();
                else
                {
                    return context.SoldoutInventories.Where(i => i.SoldoutInventoryId == id).Select(i => i.DateInStock).FirstOrDefault();
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
            using (var context = new VincontrolEntities())
            {
                if (context.Vehicles.Any(x => x.Vin == vin))
                {
                    var result =
                string.IsNullOrEmpty(dealer.DealerGroupId) ?
                context.Vehicles.Where(x => x.Vin == vin)
                    .Select(v => new VinDatabaseInfo
                    {
                        InventoryItem = v.Inventories.FirstOrDefault(i => i.DealerId == dealer.DealershipId),
                        Appraisals = v.Appraisals.Where(i => i.DealerId == dealer.DealershipId),
                        SoldoutItem = v.SoldoutInventories.FirstOrDefault(i => i.DealerId == dealer.DealershipId)
                    }).FirstOrDefault()
                : context.Vehicles.Where(x => x.Vin == vin)
                    .Select(v => new VinDatabaseInfo
                    {
                        InventoryItem = v.Inventories.FirstOrDefault(i => i.Dealer.DealerGroupId == dealer.DealerGroupId),
                        Appraisals = v.Appraisals.Where(i => i.Dealer.DealerGroupId == dealer.DealerGroupId),
                        SoldoutItem = v.SoldoutInventories.FirstOrDefault(i => i.Dealer.DealerGroupId == dealer.DealerGroupId)
                    }).FirstOrDefault();
                    SessionHandler.SetVinDatabaseInfo(vin, result);

                    if (result != null && result.InventoryItem != null)
                    {
                        model.ListingId = result.InventoryItem.InventoryId.ToString(CultureInfo.InvariantCulture);
                        model.Status = "Inventory";
                    }
                    else if (result != null && result.SoldoutItem != null)
                    {
                        model.ListingId = result.SoldoutItem.SoldoutInventoryId.ToString(CultureInfo.InvariantCulture);
                        model.Status = "SoldOut";
                    }
                    else if (result != null && result.Appraisals.Any())
                    {
                        model.AppraisalId = result.Appraisals.Last().AppraisalId.ToString(CultureInfo.InvariantCulture);
                        if (DateTime.Now.Subtract(result.Appraisals.Last().AppraisalDate.GetValueOrDefault()).Days <= 30)
                            model.IsEdit = true;
                        else
                            model.IsEdit = false;
                        model.AppraisalDate = result.Appraisals.Last().AppraisalDate.GetValueOrDefault().ToShortDateString();
                        model.Status = "Appraisal";
                    }
                    else
                    {
                        var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);
                        SessionHandler.SetVehicleDescriptionData(vin, vehicleInfo);
                        if (vehicleInfo != null && vehicleInfo.style != null)
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
                }
                else
                {
                    var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);
                    SessionHandler.SetVehicleDescriptionData(vin, vehicleInfo);
                    if (vehicleInfo != null && vehicleInfo.style != null)
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

         
        }

        public static MemoryStream GetCustomerInfoStream(int id, DealershipViewModel dealerSessionInfo)
        {
            TradeInVehicleModel vehicle = GetTradeinVehicle(id);
            MemoryStream workStream = PDFHelper.WritePDF(EmailHelper.CreateBannerBodyForPdf(vehicle, dealerSessionInfo));
            return workStream;
        }

        public static TradeInVehicleModel GetTradeinVehicle(int id)
        {
            var context = new VincontrolEntities();
            var customer = context.TradeInCustomers.Where(e => e.TradeInCustomerId == id).FirstOrDefault();

            return new TradeInVehicleModel(customer);
        }

        private static IEnumerable<SelectListItem> InitializeOptions(Standard[] options)
        {
            var results = new List<SelectListItem>();
            var tempTable = new Hashtable();

            if (options != null && options.Any())
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                //options =
                //    options.Where(
                //        option => option.header.Equals("INTERIOR") || option.header.Equals("ENTERTAINMENT")).Take(32)
                //        .ToArray();

                options =
                    options.Take(32).ToArray();


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
            var receivedAppraisalId = Convert.ToInt32(id);

            using (var context = new VincontrolEntities())
            {
                var existingAppraisal = context.Appraisals.Include("AppraisalCustomer").FirstOrDefault(a => a.AppraisalId == receivedAppraisalId);
                if (existingAppraisal != null)
                {
                    SetVehicleInfo(model, existingAppraisal);
                    model.CustomerInfo = GetCustomerInfo(existingAppraisal);
                    SetWalkaroundInfoAndPreliminaryReconCost(context, receivedAppraisalId, model);
                    SetCustomerQuestionnaireInfo(context, receivedAppraisalId, model);
                }
            }

            if (model.AppraisalInfo.Transmission.Length > 10)
            {
                model.AppraisalInfo.Transmission = model.AppraisalInfo.Transmission.Substring(0, 9) + "...";
            }
            return model;

        }



        private static void SetCustomerQuestionnaireInfo(VincontrolEntities context, int receivedAppraisalId,
            InspectionAppraisalViewModel model)
        {
            var answers =
                context.Answers.Where(i => i.Appraisal.AppraisalId == receivedAppraisalId).Select(
                    a => new
                    {
                        QuestionId = a.Question.QuestionId,
                        Question = a.Question.ShortDesciption,
                        Answer = a.Answer1,
                        Comment = a.Comment,
                        QuestionType = a.Question.QuestionType.Code ?? 4,
                        Order = a.Question.Order
                    }).ToList();
            //AppraisalAnswer()
            var questions = context.Questions.Include("QuestionType");
            var list = new List<AppraisalAnswer>();
            foreach (var question in questions)
            {
                var answer = answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
                if (answer != null)
                {
                    list.Add(new AppraisalAnswer()
                    {
                        Question = answer.Question,
                        Answer = GetAnswer(answer.Answer, question.QuestionType.Unit),
                        Comment = GetShortString(answer.Comment),
                        QuestionType = answer.QuestionType,
                        Order = answer.Order ?? 0
                    });
                }
                else
                {
                    list.Add(new AppraisalAnswer()
                    {
                        Question = question.ShortDesciption,
                        Answer = String.Empty,
                        Comment = String.Empty,
                        QuestionType = question.QuestionType.Code ?? 4,
                        Order = question.Order ?? 0
                    });
                }
            }

            model.AppraisalAnswer = answers.Count > 0
                ? (IEnumerable<AppraisalAnswer>)list.OrderBy(o => o.Order)
                : list;
        }



        private static void SetWalkaroundInfoAndPreliminaryReconCost(VincontrolEntities context, int receivedAppraisalId,
            InspectionAppraisalViewModel model)
        {
            var walkaroundInfos =
                context.Walkarounds.Where(i => i.Appraisal.AppraisalId == receivedAppraisalId).OrderBy(i=>i.CarPart.Name).ThenBy(i => i.Order).ToList();
            if (walkaroundInfos.Count > 0)
            {
                model.WalkaroundInfo = walkaroundInfos.Select(w => new WalkaroundInfo()
                                                                       {
                                                                           Order = w.Order ?? 0,
                                                                           //Note = GetNoteContent(w.Note, w.Order),
                                                                           Note =
                                                                               GetNoteContentNew(w.Note, w.CarPart.Name,
                                                                                                 w.CarPartId != null
                                                                                                     ? w.CarPartId.Value
                                                                                                     : 0,
                                                                                                 w.Order != null
                                                                                                     ? w.Order.Value
                                                                                                     : 0),
                                                                           X = w.X ?? 0,
                                                                           Y = w.Y ?? 0
                                                                       }).ToList();
            }

            var row = context.InspectionFormCosts.FirstOrDefault(x => x.AppraisalID == receivedAppraisalId);
            var viewModel = new InspectionFormCostModel();
            if (row != null)
            {
                model.PreliminaryReconCostObject = new PreliminaryReconCostObject()
                {
                    MechanicalSubTotal = row.Mechanical != null ? row.Mechanical.Value : 0,
                    FrontBumperSubTotal = row.FrontBumper != null ? row.FrontBumper.Value : 0,
                    RearBumperSubTotal = row.RearBumper != null ? row.RearBumper.Value : 0,
                    GlassSubTotal = row.Glass != null ? row.Glass.Value : 0,

                    TireSubTotal = row.Tires != null ? row.Tires.Value : 0,
                    FrontEndSubTotal = row.FrontEnd != null ? row.FrontEnd.Value : 0,
                    RearEndSubTotal = row.RearEnd != null ? row.RearEnd.Value : 0,
                    DriverSideSubTotal = row.DriverSide != null ? row.DriverSide.Value : 0,
                    PassengerSideSubTotal = row.PassengerSide != null ? row.PassengerSide.Value : 0,
                    InteriorSubTotal = row.Interior != null ? row.Interior.Value : 0,

                    LightBulbSubTotal = row.LightsBulbs != null ? row.LightsBulbs.Value : 0,
                    OtherSubTotal = row.Other != null ? row.Other.Value : 0,
                    LMASubTotal = row.LMA != null ? row.LMA.Value : 0
                };
            }
            else
            {
                model.WalkaroundObject =  GetWalkaroundObject(context, receivedAppraisalId);
                model.PreliminaryReconCostObject = GetPreliminaryReconCostObject(model.WalkaroundObject.ToList());
            }
        }

        public static PreliminaryReconCostObject GetRetailForAppraisal(VincontrolEntities context, int receivedAppraisalId)
        {
            List<WalkaroundObject> walkaroundObjects = GetWalkaroundObject(context, receivedAppraisalId);
            return GetPreliminaryReconCostObject(walkaroundObjects);
        }

        private static List<WalkaroundObject> GetWalkaroundObject(VincontrolEntities context, int receivedAppraisalId)
        {
            var walkarounds =
                context.Walkarounds.Include("CarPart").Where(i => i.Appraisal.AppraisalId == receivedAppraisalId).OrderBy(i => i.CarPartId).ToList();

            WalkaroundObject walkaroundCarPart1Object = new WalkaroundObject();

            List<WalkaroundObject> walkaroundObjects = new List<WalkaroundObject>();
            if (walkarounds.Count > 0)
            {
                List<Walkaround> waCarPart1 =
                    walkarounds.Where(x => x.CarPartId == 1 && x.Note != "New" && x.Note != "75%").ToList();
                if (waCarPart1.Count > 0)
                {
                    walkaroundCarPart1Object.CarPartID = waCarPart1.FirstOrDefault().CarPartId.Value;
                    walkaroundCarPart1Object.Name = waCarPart1.FirstOrDefault().CarPart.Name;
                    walkaroundCarPart1Object.ItemCount = waCarPart1.Count();
                    walkaroundObjects.Add(walkaroundCarPart1Object);
                }

                var setting =
                    context.Settings.FirstOrDefault(i => i.DealerId == SessionHandler.Dealer.DealershipId);
                if (setting != null)
                {
                    AddWalkAround(walkaroundObjects, walkarounds, 2, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 3, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 4, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 5, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 6, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 7, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 8, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 9, setting);
                    AddWalkAround(walkaroundObjects, walkarounds, 20, setting);
                    //AddWalkAroundForLightBulb(walkaroundObjects, walkarounds, setting);
                }
               
            }
            return walkaroundObjects;
        }

       
        private static void AddWalkAround(List<WalkaroundObject> walkaroundObjects, List<Walkaround> walkarounds, int carPartID, Setting setting)
        {
            WalkaroundObject walkaroundCarPartObject = new WalkaroundObject();
            List<Walkaround> waCarParts;
            if (carPartID == 4)
            {
                waCarParts = walkarounds.Where(x => x.CarPartId == carPartID&& x.Order!=7&&x.Order!=8).ToList();
            }
            else if (carPartID == 20)
            {
                waCarParts = walkarounds.Where(x => x.CarPartId == 4 && (x.Order == 7 || x.Order == 8)).ToList();
                
            }
            else
            {
                waCarParts = walkarounds.Where(x => x.CarPartId == carPartID).ToList();
            }

            if (waCarParts.Count > 0)
            {
                walkaroundCarPartObject.CarPartID = carPartID;
                walkaroundCarPartObject.Name = waCarParts.FirstOrDefault().CarPart.Name;
                walkaroundCarPartObject.Total = 0;
                if (waCarParts.Count() > 1)
                {
                    foreach (var carPart in waCarParts)
                    {
                        walkaroundCarPartObject.Total += GetRetailPrice(carPart.Note, setting, carPart.Order);
                        
                    }
                }
                else
                {
                    foreach (var item in waCarParts.FirstOrDefault().Note.Split(';'))
                    {
                        walkaroundCarPartObject.Total += GetRetailPrice(item, setting, waCarParts.FirstOrDefault().Order);
                    }
                }

                if (waCarParts.Count() > 1)
                    walkaroundCarPartObject.ItemCount = waCarParts.Count();
                else
                    walkaroundCarPartObject.ItemCount = waCarParts[0].Note.Split(';').Count();
                walkaroundObjects.Add(walkaroundCarPartObject);
            }
        }

        private static decimal GetRetailPrice(string note, Setting setting, int? order=0)
        {
            switch (note)
            {
                case "Scratch":
                    return setting.ScratchRetail??0;
                case "Major Scratch":
                    return setting.MajorScratchRetail ?? 0;
                case "Dent":
                    return setting.DentRetail ?? 0;
                case "Major Dent":
                    return setting.MajorDentRetail ?? 0;
                case "Paint Damage":
                    return setting.PaintDamageRetail ?? 0;
                case "Repainted Panel":
                    return setting.RepaintedPanelRetail ?? 0;
                case "Rust":
                    return setting.RustRetail ?? 0;
                case "Acident":
                case "Accident":
                    return setting.AcidentRetail ?? 0;
                case "Missing Parts":
                    return setting.MissingPartsRetail ?? 0;
                case "Crack windshield":
                    switch (order)
                    {
                        case 1:
                            return setting.FrontWindShieldRetail ?? 0;
                        case 6:
                            return setting.RearWindShieldRetail ?? 0;
                        case 2:
                            return setting.DriverWindowRetail ?? 0;
                        case 3:
                        case 4:
                        case 5:
                            return setting.PassengerWindowRetail ?? 0;
                        case 7:
                            return setting.DriverSideMirrorRetail ?? 0;
                        case 8:
                            return setting.PassengerSideMirrorRetail ?? 0;
                            
                    }
                    return setting.MissingPartsRetail ?? 0;
                default:
                    return 0;
            }
        }

        public static PreliminaryReconCost GetPreliminaryReconCost(List<Walkaround> walkarounds)
        {
            var costSummary =
                walkarounds.GroupBy(i => i.CarPartId).Select(i => new KeyValuePair<short?, int>(i.Key, i.Count())).ToList();

            //model.PreliminaryReconCost.MechanicalCost =
            //    costSummary.Single(i => i.Key == Constanst.CarPart.).Quantity* SessionHandler.Dealer.DealerSetting.DriverSideRetail;
            var preliminaryReconCost = new PreliminaryReconCost()
            {
                FrontBumperCost = (
                    GetQuantity(costSummary, i => i.Key == Constanst.CarPart.FrontBumper))*
                                  SessionHandler.Dealer.DealerSetting.FrontBumperCost,
                RearBumperCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.RearBumper))*
                    SessionHandler.Dealer.DealerSetting.RearBumperCost,
                GlassCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.Glass))*
                    SessionHandler.Dealer.DealerSetting.GlassCost,
                TireCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.VehicleTires))*
                    SessionHandler.Dealer.DealerSetting.VehicleTireCost,
                FrontEndCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.FrontEnd))*
                    SessionHandler.Dealer.DealerSetting.FrontEndCost,
                RearEndCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.RearEnd))*
                    SessionHandler.Dealer.DealerSetting.RearEndCost,
                DriverSideCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.DriverSide))*
                    SessionHandler.Dealer.DealerSetting.DriverSideCost,
                PassengerSideCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.PassengerSide))*
                    SessionHandler.Dealer.DealerSetting.PassengerSideCost,
                //model.PreliminaryReconCost.InteriorCost =
                //   costSummary.Single(i => i.Key == Constanst.CarPart.).Quantity * SessionHandler.Dealer.DealerSetting.DriverSideRetail;
                LightBulbCost =
                    (GetQuantity(costSummary, i => i.Key == Constanst.CarPart.LightBulb))*
                    SessionHandler.Dealer.DealerSetting.LightBulbCost
                //model.PreliminaryReconCost.OtherCost =
                //  costSummary.Single(i => i.Key == Constanst.CarPart.).Quantity * SessionHandler.Dealer.DealerSetting.DriverSideRetail;
                //model.PreliminaryReconCost.LMACost =
                //  costSummary.Single(i => i.Key == Constanst.CarPart.DriverSide).Quantity * SessionHandler.Dealer.DealerSetting.DriverSideRetail;
            };
            return preliminaryReconCost;
        }

        public static PreliminaryReconCostObject GetPreliminaryReconCostObject(List<WalkaroundObject> walkarounds)
        {
            var preliminaryReconCost = new PreliminaryReconCostObject();
            foreach (var item in walkarounds)
            {
                switch (item.CarPartID)
                {
                    case Constanst.CarPart.FrontBumper:
                        preliminaryReconCost.FrontBumperCount = item.ItemCount;
                        preliminaryReconCost.FrontBumperCost = SessionHandler.Dealer.DealerSetting.FrontBumperRetail;
                        //preliminaryReconCost.FrontBumperSubTotal = item.ItemCount *
                        //                                           SessionHandler.Dealer.DealerSetting.FrontBumperRetail;
                        preliminaryReconCost.FrontBumperSubTotal = item.Total;
                        break;
                    case Constanst.CarPart.RearBumper:
                        preliminaryReconCost.RearBumperCount = item.ItemCount;
                        preliminaryReconCost.RearBumperCost = SessionHandler.Dealer.DealerSetting.RearBumperRetail;
                        preliminaryReconCost.RearBumperSubTotal = item.Total;
                        break;
                    case Constanst.CarPart.Glass:
                        preliminaryReconCost.GlassCount = item.ItemCount;
                        preliminaryReconCost.GlassCost = SessionHandler.Dealer.DealerSetting.GlassRetail;
                        preliminaryReconCost.GlassSubTotal = item.Total;
                        break;
                    case Constanst.CarPart.VehicleTires:
                        preliminaryReconCost.TireCount = item.ItemCount;
                        preliminaryReconCost.TireCost = SessionHandler.Dealer.DealerSetting.VehicleTireRetail;
                        preliminaryReconCost.TireSubTotal = item.ItemCount *
                                                                   SessionHandler.Dealer.DealerSetting.VehicleTireRetail;
                        break;
                    case Constanst.CarPart.FrontEnd:
                        preliminaryReconCost.FrontEndCount = item.ItemCount;
                        preliminaryReconCost.FrontEndCost = SessionHandler.Dealer.DealerSetting.FrontEndRetail;
                        preliminaryReconCost.FrontEndSubTotal = item.Total;
                        break;
                    case Constanst.CarPart.RearEnd:
                        preliminaryReconCost.RearEndCount = item.ItemCount;
                        preliminaryReconCost.RearEndCost = SessionHandler.Dealer.DealerSetting.RearEndRetail;
                        preliminaryReconCost.RearEndSubTotal = item.Total;
                        break;
                    case Constanst.CarPart.DriverSide:
                        preliminaryReconCost.DriverSideCount = item.ItemCount;
                        preliminaryReconCost.DriverSideCost = SessionHandler.Dealer.DealerSetting.DriverSideRetail;
                        preliminaryReconCost.DriverSideSubTotal = item.Total;
                        break;
                    case Constanst.CarPart.PassengerSide:
                        preliminaryReconCost.PassengerSideCount = item.ItemCount;
                        preliminaryReconCost.PassengerSideCost = SessionHandler.Dealer.DealerSetting.PassengerSideRetail;
                        preliminaryReconCost.PassengerSideSubTotal = item.Total;
                        break;
                    case Constanst.CarPart.LightBulb:
                        preliminaryReconCost.LightBulbCount = item.ItemCount;
                        preliminaryReconCost.LightBulbCost = SessionHandler.Dealer.DealerSetting.LightBulbRetail;
                        preliminaryReconCost.LightBulbSubTotal = item.ItemCount *
                                                                   SessionHandler.Dealer.DealerSetting.LightBulbRetail;
                        break;
                    case Constanst.CarPart.Other:
                        preliminaryReconCost.OtherCount = item.ItemCount;
                        preliminaryReconCost.OtherCost = SessionHandler.Dealer.DealerSetting.LightBulbRetail;
                        preliminaryReconCost.OtherSubTotal = item.Total;
                        break;
                        //How about other?
                        
                }
            }
            return preliminaryReconCost;
        }

        private static int GetQuantity(List<KeyValuePair<short?, int>> costSummary, Func<KeyValuePair<short?, int>, bool> func)
        {
            return costSummary.FirstOrDefault(func).Value;
        }

        private static void SetVehicleInfo(InspectionAppraisalViewModel model, Appraisal existingAppraisal)
        {
            model.AppraisalInfo = new AppraisalInfo(existingAppraisal)
            {
                AppraisalById = existingAppraisal.VinGenieUserId
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
                var make =
                    SelectListHelper.InitialMakeList(divisionList).Where(m => m.Text.Equals(model.AppraisalInfo.Make)).First();
                var chromeModel =
                    autoService.GetModelsByDivision(model.AppraisalInfo.Year, Convert.ToInt32(make.Value.Split('|')[0]))
                        .Where(m => m.Value.Equals(model.AppraisalInfo.Model))
                        .FirstOrDefault();
                if (chromeModel != null)
                {
                    var styles = autoService.GetStyles(chromeModel.id);
                    int styleId = Convert.ToInt32(styles.First().id);
                    var styleInfo = autoService.GetStyleInformationFromStyleId(styleId);
                    model.AppraisalInfo.StandardOptions = InitializeOptions(styleInfo.standard).Select(o => o.Text).ToList();
                }
            }
        }

        private static CustomerInfo GetCustomerInfo(Appraisal existingAppraisal)
        {
            return existingAppraisal.AppraisalCustomer != null
                ? new CustomerInfo()
                {
                    FirstName = existingAppraisal.AppraisalCustomer.FirstName,
                    LastName = existingAppraisal.AppraisalCustomer.LastName,
                    Phone = existingAppraisal.AppraisalCustomer.Phone,
                    Email = existingAppraisal.AppraisalCustomer.Email,
                    City = existingAppraisal.AppraisalCustomer.City,
                    State = existingAppraisal.AppraisalCustomer.State,
                    Zip = existingAppraisal.AppraisalCustomer.ZipCode,
                    Street = existingAppraisal.AppraisalCustomer.Street,
                    Address = existingAppraisal.AppraisalCustomer.Address,
                }
                : new CustomerInfo()
                {
                };
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

        private static string GetNoteContentNew(string content, string carPartName, int carPartID,int order)
        {
            if (carPartID == 4)
            {
                switch (order)
                {
                    case 1:
                        return "<div><b>Damaged Front windshield</b></div>";
                    case 6:
                        return "<div><b>Damaged Rear windshield</b></div>"; 
                    case 2:
                        return "<div><b>Damaged Driver window</b></div>";
                    case 3:
                    case 4:
                    case 5:
                        return "<div><b>Damaged Passenger window</b></div>"; 
                    case 7:
                        return "<div><b>Damaged Driver side mirror </b></div>"; 
                    case 8:
                        return "<div><b>Damaged Passenger side mirror</b></div>"; 
                }
               
            }
            else if(carPartID == 1)
            {
                return string.Format("<div><b>{0} {1}</b></div> {2} ", carPartName, order,
                                   GetDivsFromContent(content));
            }
            else if (carPartID == 9)
            {
                switch (order)
                {
                    case 1:
                        return "<div><b>Damaged Front Left Light/Bult</b></div>";
                    case 2:
                        return "<div><b>Damaged Front Right Light/Bult</b></div>";
                    case 3:
                        return "<div><b>Damaged Rear Left Light/Bult</b></div>";
                    case 4:
                        return "<div><b>Damaged Rear Right Light/Bult</b></div>";
                }
            }
            return string.Format("<div><b>{0}</b></div> <div>{1}</div>", carPartName, GetDivsFromContent(content));
        }

        private static string GetDivsFromContent(string content)
        {
            var value = string.Empty;
            foreach (var item in content.Split(';'))
            {
                value += string.Format("<div>{0}</div>", item);
            }

            return value;
        }

        private static string GetAppraisalName(string name)
        {
            var context = new VincontrolEntities();
            var user = (from u in context.Users
                        where u.UserName == name
                        select u).FirstOrDefault();
            if (user != null)
            {
                return user.Name;
            }
            else
            {
                var masterUser = (from u in context.DealerGroups
                                  where u.DealerGroupName == name
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

                modelWord = modelWord.Replace("Wagon", "");

                modelWord = modelWord.Replace("(Natl)", "");

                modelWord = modelWord.Replace("Sedan", "");

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

                modelWord = modelWord.Replace("SportWagen", "");

                modelWord = modelWord.Replace("LWB", "");


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
                                                                        string modelWord, string trim, bool ignoredTrim)
        {
            var originalModelWorld = modelWord;
            if (modelWord != null)
            {
                if (make.ToLower().Equals("bmw") && modelWord.ToLower().Contains("series"))
                {
                    modelWord = trim;
                }

                if (make.ToLower().Equals("mercedes-benz") && modelWord.ToLower().Contains("class"))
                {
                    modelWord = trim;

                    if (string.IsNullOrEmpty(modelWord))
                        modelWord = string.Empty;

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

                    if (modelWord.Length >= 1)
                    {

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

                }

                if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xk"))
                {
                    modelWord = "xk";
                }
                if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xj"))
                {
                    modelWord = "xj";
                }
                if (make.ToLower().Equals("mazda") && modelWord.ToLower().Contains("mx-5 miata"))
                {
                    modelWord = "miata mx5";
                }
            }

            modelWord = FilterCarModelForMarket(modelWord);

            if (!make.ToLower().Equals("land rover"))
            {
                if (ignoredTrim && String.IsNullOrEmpty(modelWord) && make.ToLower().Equals("bmw") && originalModelWorld.ToLower().Contains("series"))
                {
                    return HandleModelContainSeries(query, make, originalModelWorld);
                }
                else if (modelWord.ToLower().Contains("cls550"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                 ((i.Model == "cls550" && i.CurrentPrice > 0 && i.Mileage > 0)));
                    return result;
                }
                else if (modelWord.ToLower().Contains("s550"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                 ((i.Model=="s550" &&i.CurrentPrice > 0 && i.Mileage > 0)));
                    return result;
                }
             
                else
                {
                    return query.Where(i => i.Make == make &&
                                             ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                               i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                             i.CurrentPrice > 0 && i.Mileage > 0);
                }



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

        private static IQueryable<MarketCarInfo> HandleModelContainSeries(IQueryable<MarketCarInfo> query, string make, string originalModelWorld)
        {
            if (originalModelWorld.ToLower().Contains("1 series"))
            {
                return GetQuery(query, make, "1");
            }

            if (originalModelWorld.ToLower().Contains("2 series"))
            {
                return GetQuery(query, make, "2");
            }
            if (originalModelWorld.ToLower().Contains("3 series"))
            {
                return GetQuery(query, make, "3");
            }
            if (originalModelWorld.ToLower().Contains("4 series"))
            {
                return GetQuery(query, make, "4");
            }
            if (originalModelWorld.ToLower().Contains("5 series"))
            {
                return GetQuery(query, make, "5");
            }
            if (originalModelWorld.ToLower().Contains("6 series"))
            {
                return GetQuery(query, make, "6");
            }
            if (originalModelWorld.ToLower().Contains("7 series"))
            {
                return GetQuery(query, make, "7");
            }
            return query.Where(i => i.Make == make &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);

        }

        private static IQueryable<MarketCarInfo> GetQuery(IQueryable<MarketCarInfo> query, string make, string number)
        {
            return query.Where(i => i.Make == make &&
                                    i.CurrentPrice > 0 && i.Mileage > 0 &&
                                    ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                      i.Trim.Replace(" ", "").ToLower()).StartsWith(number) &&
                                     (i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                      i.Trim.Replace(" ", "").ToLower()).EndsWith("i")));
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

        public static DateTime ParseDateTimeFromString(string szDateTime, bool bIsFromDate)
        {
            DateTime result = DateTime.Today;

            if (!string.IsNullOrEmpty(szDateTime))
            {
                string[] arrDate = szDateTime.Split('/');
                int nDay = 1;
                int nMonth = 1;
                int nYear = 2000;

                if (arrDate.Count() > 2)
                {
                    nMonth = Convert.ToInt32(arrDate[0]);
                    nDay = Convert.ToInt32(arrDate[1]);
                    nYear = Convert.ToInt32(arrDate[2]);
                }

                if (bIsFromDate)
                {
                    result = new DateTime(nYear, nMonth, nDay, 0, 0, 0);
                }
                else
                {
                    result = new DateTime(nYear, nMonth, nDay, 23, 59, 59);
                }
            }

            return result;
        }

        public static List<CarInfoFormViewModel> SortList(List<CarInfoFormViewModel> list, string sortBy, bool isUp)
        {
            var result = new List<CarInfoFormViewModel>(list);

            switch (sortBy)
            {
                case "year":
                    if (isUp)
                        result = list.OrderBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "make":
                    if (isUp)
                        result = list.OrderBy(x => x.Make).ThenBy(x => x.Year).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.Make).ThenByDescending(x => x.Year).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "model":
                    if (isUp)
                        result = list.OrderBy(x => x.Model).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.Model).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "trim":
                    if (isUp)
                        result = list.OrderBy(x => x.Trim).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.Trim).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "age":
                    if (isUp)
                        result = list.OrderBy(x => x.DaysInInvenotry).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "market":
                    if (isUp)
                        result = list.OrderBy(x => x.MarketRange).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ToList();
                    else
                        result = list.OrderByDescending(x => x.MarketRange).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ToList();
                    break;
                case "miles":
                    if (isUp)
                        result = list.OrderBy(x => x.Mileage).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.Mileage).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "price":
                    if (isUp)
                        result = list.OrderBy(x => x.SalePrice).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.SalePrice).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "color":
                    if (isUp)
                        result = list.OrderBy(x => x.ExteriorColor).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.ExteriorColor).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "stock":
                    if (isUp)
                        result = list.OrderBy(x => x.Stock).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.Stock).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "vin":
                    if (isUp)
                        result = list.OrderBy(x => x.Vin).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.Vin).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "owners":
                    if (isUp)
                        result = list.OrderBy(x => x.CarFaxOwner).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList();
                    else
                        result = list.OrderByDescending(x => x.CarFaxOwner).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
            }

            return result;
        }
    }
}
