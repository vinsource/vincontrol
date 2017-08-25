using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Web;
using Microsoft.Ajax.Utilities;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Models;
using System.Linq;
using Appraisal = vincontrol.Data.Model.Appraisal;
using BlackBookTrimReport = Vincontrol.Web.Models.BlackBookTrimReport;
using BlackBookViewModel = Vincontrol.Web.Models.BlackBookViewModel;
using Inventory = vincontrol.Data.Model.Inventory;
using KellyBlueBookTrimReport = Vincontrol.Web.Models.KellyBlueBookTrimReport;
using KellyBlueBookViewModel = Vincontrol.Web.Models.KellyBlueBookViewModel;
using vincontrol.Helper;
using Button = vincontrol.Application.ViewModels.CommonManagement.Button;
using CraigslistSetting = vincontrol.Data.Model.CraigslistSetting;
using Setting = vincontrol.Data.Model.Setting;

namespace Vincontrol.Web.HelperClass
{
    public sealed class SQLHelper
    {
      

        public static int CheckVinHasKbbReport(string vin)
        {
            using (var context = new VincontrolEntities())
            {

                if (context.KellyBlueBooks.Any(o => o.Vin.Equals(vin)))
                {
                    var firstTmp = context.KellyBlueBooks.First(o => o.Vin == vin);

                    DateTime dt = DateTime.Parse(firstTmp.Expiration.ToString());

                    if (dt.Date > DateTime.Now.Date)

                        return 1;
                    else
                        return 2;
                }
            }
            return 0;


        }

        public static void AddSimpleKbbReport(KellyBlueBookViewModel kbb)
        {
            using (var context = new VincontrolEntities())
            {
                foreach (KellyBlueBookTrimReport tmp in kbb.TrimReportList)
                {
                    var e = new KellyBlueBook()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = Convert.ToDecimal(tmp.TradeInPrice.TradeInFairPrice),
                        TradeInGoodPrice = Convert.ToDecimal(tmp.TradeInPrice.TradeInGoodPrice),
                        TradeInVeryGoodPrice = Convert.ToDecimal(tmp.TradeInPrice.TradeInVeryGoodPrice),
                        BaseWholeSale = Convert.ToDecimal(tmp.BaseWholesale),
                        MileageAdjustment = (tmp.MileageAdjustment),
                        WholeSale = Convert.ToDecimal(tmp.WholeSale),
                        TrimId = tmp.TrimId,
                        DateStamp = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Expiration = CommonHelper.GetNextFriday(),
                        Vin = kbb.Vin
                    };


                    //Add to memory

                    context.AddToKellyBlueBooks(e);

                }

                if (kbb.TrimReportList.Count == 1)
                {
                    var searchVehicleResult = context.Inventories.Where(x => x.Vehicle.Vin == kbb.Vin);

                    foreach (var tmp in searchVehicleResult)
                    {
                        tmp.Vehicle.KBBTrimId = kbb.TrimReportList.First().TrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        public static void AddSimpleKbbReportFromKarPower(List<SmallKarPowerViewModel> karpowerResult, string vin, short type)
        {
            using (var context = new VincontrolEntities())
            {
                foreach (var tmp in karpowerResult)
                {
                    var existingKbb = context.KellyBlueBooks.OrderByDescending(i => i.DateStamp).FirstOrDefault(i => i.Vin == vin && i.TrimId == tmp.SelectedTrimId && i.VehicleStatusCodeId == type);
                    if (existingKbb == null)
                    {
                        var e = new KellyBlueBook()
                        {
                            Trim = tmp.SelectedTrimName,
                            BaseWholeSale = tmp.BaseWholesale,
                            MileageAdjustment = tmp.MileageAdjustment,
                            WholeSale = tmp.Wholesale,
                            TrimId = tmp.SelectedTrimId,
                            DateStamp = DateTime.Now,
                            LastUpdated = DateTime.Now,
                            Expiration = CommonHelper.GetNextFriday(),
                            Vin = vin,
                            VehicleStatusCodeId = type
                        };

                        //Add to memory

                        context.AddToKellyBlueBooks(e);
                    }
                }

                if (karpowerResult.Count == 1)
                {
                    var searchVehicleResult = context.Inventories.Where(x => x.Vehicle.Vin == vin);

                    foreach (var tmp in searchVehicleResult)
                    {
                        tmp.Vehicle.KBBTrimId = karpowerResult.First().SelectedTrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        public static void UpdateSimpleKbbReport(KellyBlueBookViewModel kbb)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.KellyBlueBooks.Where(x => x.Vin == kbb.Vin).ToList();
                foreach (var tmp in searchResult)
                {
                    context.Attach(tmp);
                    context.DeleteObject(tmp);
                }

                context.SaveChanges();

                foreach (var tmp in kbb.TrimReportList)
                {
                    var e = new KellyBlueBook()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = tmp.TradeInPrice.TradeInFairPrice,
                        TradeInGoodPrice = tmp.TradeInPrice.TradeInGoodPrice,
                        TradeInVeryGoodPrice = tmp.TradeInPrice.TradeInVeryGoodPrice,
                        BaseWholeSale = tmp.BaseWholesale,
                        MileageAdjustment = tmp.MileageAdjustment,
                        WholeSale = tmp.WholeSale,
                        TrimId = tmp.TrimId,
                        DateStamp = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Expiration = CommonHelper.GetNextFriday(),
                        Vin = kbb.Vin
                    };


                    //Add to memory

                    context.AddToKellyBlueBooks(e);

                }

                if (kbb.TrimReportList.Count == 1)
                {
                    var searchVehicleResult = context.Inventories.Where(x => x.Vehicle.Vin == kbb.Vin);

                    foreach (var tmp in searchVehicleResult)
                    {
                        tmp.Vehicle.KBBTrimId = kbb.TrimReportList.First().TrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        public static int CheckVinHasBbReport(string Vin)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.BlackBooks.Any(o => o.Vin.Equals(Vin)))
                {
                    var firstTmp = context.BlackBooks.FirstOrDefault(o => o.Vin.Equals(Vin));

                    DateTime dt = DateTime.Parse(firstTmp.Expiration.ToString());

                    if (dt.Date > DateTime.Now.Date)

                        return 1;
                    else
                        return 2;
                }
            }
            return 0;


        }

        public static void AddSimpleBbReport(BlackBookViewModel bb)
        {
            using (var context = new VincontrolEntities())
            {
                foreach (BlackBookTrimReport tmp in bb.TrimReportList)
                {
                    var e = new BlackBook()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = tmp.TradeInRough,
                        TradeInGoodPrice = tmp.TradeInAvg,
                        TradeInVeryGoodPrice = tmp.TradeInClean,
                        DateStamp = DateTime.Now,
                        Expiration = CommonHelper.GetNextFriday(),
                        Vin = bb.Vin
                    };


                    //Add to memory

                    context.AddToBlackBooks(e);

                }

                context.SaveChanges();
            }
        }

        public static void UpdateSimpleBBReport(BlackBookViewModel bb)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.BlackBooks.Where(x => x.Vin == bb.Vin).ToList();
                foreach (var tmp in searchResult)
                {
                    context.Attach(tmp);
                    context.DeleteObject(tmp);
                }

                context.SaveChanges();

                foreach (BlackBookTrimReport tmp in bb.TrimReportList)
                {
                    BlackBook e = new BlackBook()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = tmp.TradeInRough,
                        TradeInGoodPrice = tmp.TradeInAvg,
                        TradeInVeryGoodPrice = tmp.TradeInClean,
                        DateStamp = DateTime.Now,
                        Expiration = CommonHelper.GetNextFriday(),
                        Vin = bb.Vin
                    };


                    //Add to memory

                    context.AddToBlackBooks(e);

                }

                context.SaveChanges();
            }
        }

        public static int CheckVinHasCarFaxReport(string Vin)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.Carfaxes.Any(o => o.Vin.Equals(Vin)))
                {
                    var firstTmp = context.Carfaxes.FirstOrDefault(o => o.Vin.Equals(Vin));

                    DateTime dt = DateTime.Parse(firstTmp.Expiration.ToString());

                    if (dt.Date >= DateTime.Now.Date)
                        return 1;
                    else
                        return 2;
                }
            }
            return 0;


        }

        public static void AddCarFaxReport(CarFaxViewModel carfax)
        {
            using (var context = new VincontrolEntities())
            {
                int number = 0;

                bool flag =
              Int32.TryParse(
                 carfax.NumberofOwners, out number);

                var e = new Carfax()
                {
                    Vin = carfax.Vin,
                    DateStamp = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Expiration = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3),
                    Owner = number,
                    ServiceRecord = carfax.ServiceRecords,
                    PriorRental = carfax.PriorRental,
                    Accident = carfax.AccidentCounts,
                    VehicleId = carfax.VehicleId


                };
                var builder = new StringBuilder();
                foreach (CarFaxWindowSticker tmp in carfax.ReportList)
                {
                    builder.Append(tmp.Text);
                    builder.Append("|");
                    builder.Append(tmp.Image);
                    builder.Append(",");
                }
                if (!String.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Remove(builder.ToString().Length - 1, 1);
                    e.WindowSticker = builder.ToString();
                }


                context.AddToCarfaxes(e);


                context.SaveChanges();
            }
        }

        public static void UpdateCarFaxReport(CarFaxViewModel carfax)
        {
            using (var context = new VincontrolEntities())
            {

                var builder = new StringBuilder();
                int number;
                Int32.TryParse(
                    carfax.NumberofOwners, out number);


                foreach (CarFaxWindowSticker tmp in carfax.ReportList)
                {
                    builder.Append(tmp.Text);
                    builder.Append("|");
                    builder.Append(tmp.Image);
                    builder.Append(",");
                }
                if (!String.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Remove(builder.ToString().Length - 1, 1);

                }

                var findCarFax = context.Carfaxes.First(x => x.Vin.Equals(carfax.Vin));

                findCarFax.Owner = number;

                findCarFax.WindowSticker = builder.ToString();

                findCarFax.PriorRental = carfax.PriorRental;

                findCarFax.Accident = carfax.AccidentCounts;

                findCarFax.LastUpdated = DateTime.Now;

                findCarFax.Expiration = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3);

                context.SaveChanges();
            }
        }

        public static DealerLoginResult MasterLogin(UserRoleViewModel searchuser)
        {
            DealerGroupViewModel dealerGroup = null;
            DealershipViewModel defaultDealer = null;
            using (var context = new VincontrolEntities())
            {
                var dealer = context.Dealers.Include("Setting").FirstOrDefault(x => x.DealerId == searchuser.DefaultLogin);

                if (dealer != null)
                {

                    dealerGroup = new DealerGroupViewModel()
                        {
                            DealershipGroupId = searchuser.DealerGroupId,

                            DealershipGroupName = searchuser.Name,

                            DealershipGroupDefaultLogin = searchuser.DefaultLogin
                        };

                    defaultDealer = new DealershipViewModel(dealer)
                        {

                        };




                    dealerGroup.DealerList = new List<DealershipViewModel>();

                    var dealerList = context.Dealers.Include("Setting").Where(x => x.DealerGroupId == searchuser.DealerGroupId);

                    foreach (var row in dealerList)
                    {
                        var tmp = new DealershipViewModel(row);
                        dealerGroup.DealerList.Add(tmp);
                    }
                }
            }
            return new DealerLoginResult() { Dealer = defaultDealer, DealerGroup = dealerGroup };
        }

        public static DealerLoginResult LoginSingleStore(UserRoleViewModel searchuser)
        {
            DealershipViewModel dealer = null;
            using (var context = new VincontrolEntities())
            {
                var defaultDealer = context.Dealers.Include("Setting").FirstOrDefault(x => x.DealerId == searchuser.DealershipId);

                if (defaultDealer != null)
                {
                    dealer = new DealershipViewModel(defaultDealer);
                }
            }
            return new DealerLoginResult() { Dealer = dealer };
        }

        public static DealerLoginResult LoginMultipleStore(UserRoleViewModel searchuser)
        {
            DealerGroupViewModel dealerGroup = new DealerGroupViewModel();
            DealershipViewModel dealer = null;
            using (var context = new VincontrolEntities())
            {
                dealerGroup.DealerList = new List<DealershipViewModel>();

                foreach (
                    var row in
                        context.Dealers.Include("Setting").Where(LogicHelper.BuildContainsExpression<Dealer, int>(e => e.DealerId, searchuser.AccessDealerPermissions.Select(x => x.DealerId)))
                    )
                {
                    var tmp = new DealershipViewModel(row);
                    dealerGroup.DealerList.Add(tmp);
                }

                var defaultDealer = context.Dealers.Include("Setting").FirstOrDefault(x => x.DealerId == searchuser.DefaultLogin);

                if (defaultDealer != null)
                {
                    if (searchuser.DefaultLogin == 0)
                    {
                        var group = context.DealerGroups.FirstOrDefault(x => x.DealerGroupId == searchuser.DealerGroupId);

                        dealerGroup.DealershipGroupId = group.DealerGroupId;
                        dealerGroup.DealershipGroupName = group.DealerGroupName;
                        dealerGroup.DealershipGroupDefaultLogin = dealerGroup.DealerList.FirstOrDefault().DealershipId;

                        defaultDealer = context.Dealers.FirstOrDefault(x => x.DealerId == dealerGroup.DealershipGroupDefaultLogin);
                    }
                    else
                    {
                        dealerGroup.DealershipGroupId = defaultDealer.DealerGroupId;
                        dealerGroup.DealershipGroupName = defaultDealer.DealerGroup.DealerGroupName;
                        dealerGroup.DealershipGroupDefaultLogin = defaultDealer.DealerGroup.DefaultDealerId;
                    }

                    dealer = new DealershipViewModel(defaultDealer)
                    {
                        DealershipId = searchuser.DefaultLogin
                    };
                }
            }

            return new DealerLoginResult { Dealer = dealer, DealerGroup = dealerGroup };

        }


        public static bool CheckVinExist(string vin, DealershipViewModel dealer)
        {

            using (var context = new VincontrolEntities())
            {
                if (context.Inventories.Any(o => o.DealerId == dealer.DealershipId && o.Vehicle.Vin == vin))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CheckSimilarVinExist(string vin, DealershipViewModel dealer)
        {

            using (var context = new VincontrolEntities())
            {
                if (context.Inventories.Any(o => o.DealerId == dealer.DealershipId && o.Vehicle.Vin.ToLower().Equals(vin.ToLower())))
                {
                    int numberofResult =
                        context.Inventories.Count(
                            o => o.DealerId == dealer.DealershipId && o.Vehicle.Vin.ToLower().Equals(vin.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static int CheckStockExist(string stock, DealershipViewModel dealer)
        {

            using (var context = new VincontrolEntities())
            {
                if (context.Inventories.Any(o => o.DealerId == dealer.DealershipId && o.Stock.ToLower().Contains(stock.ToLower())))
                {
                    int numberofResult = context.Inventories.Count(o => o.DealerId == dealer.DealershipId && o.Stock.ToLower().Contains(stock.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static int CheckStockExistInGroup(string stock, DealerGroupViewModel dealerGroup)
        {

            using (var context = new VincontrolEntities())
            {
                var dealerList = from e in context.Dealers
                                 where e.DealerGroupId == dealerGroup.DealershipGroupId
                                 select e.DealerId;

                var avaiInventory = context.Inventories.Where(LogicHelper.BuildContainsExpression<Inventory, int>(e => e.DealerId, dealerList));
                if (avaiInventory.Any(o => o.Stock.ToLower().Contains(stock.ToLower())))
                {
                    int numberofResult =
                       avaiInventory.Count(
                            o => o.Stock.ToLower().Contains(stock.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static int CheckVinExistInGroup(string vin, DealerGroupViewModel dealerGroup)
        {

            using (var context = new VincontrolEntities())
            {
                var dealerList = from e in context.Dealers
                                 where e.DealerGroupId == dealerGroup.DealershipGroupId
                                 select e.DealerId;

                var avaiInventory = context.Inventories.Where(LogicHelper.BuildContainsExpression<Inventory, int>(e => e.DealerId, dealerList));
                if (avaiInventory.Any(o => o.Vehicle.Vin.ToLower().Equals(vin.ToLower())))
                {
                    int numberofResult = avaiInventory.Count(o => o.Vehicle.Vin.ToLower().Equals(vin.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static bool CheckVinExistInAppraisal(string vin, DealershipViewModel dealer)
        {

            using (var context = new VincontrolEntities())
            {
                if (context.Appraisals.Any(o => o.Vehicle.Vin == vin && o.DealerId == dealer.DealershipId))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CheckVinExistInGroupForAppraisal(string vin, DealerGroupViewModel dealerGroup)
        {

            using (var context = new VincontrolEntities())
            {
                var dealerList = from e in context.Dealers
                                 where e.DealerGroupId == dealerGroup.DealershipGroupId
                                 select e.DealerId;

                var avaiInventory = context.Appraisals.Where(LogicHelper.BuildContainsExpression<Appraisal, int>(e => e.DealerId, dealerList));
                if (avaiInventory.Any(o => o.Vehicle.Vin.ToLower().Equals(vin.ToLower())))
                {
                    return avaiInventory.First(o => o.Vehicle.Vin.ToLower().Equals(vin.ToLower())).AppraisalId;
                }
            }
            return 0;
        }

        public static bool CheckVinExistInSoldOut(string vin, DealershipViewModel dealer)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.SoldoutInventories.Any(o => o.Vehicle.Vin == vin && o.DealerId == dealer.DealershipId))
                {
                    return true;
                }
            }
            return false;
        }

   
    

  
        public static void UpdateKBBOptions(int ListingId, string OptionSelect, int TrimId, decimal BaseWholeSale, decimal WholeSale, decimal MileageAdjustment)
        {
            using (var context = new VincontrolEntities())
            {

                var searchResult = context.Inventories.FirstOrDefault(x => x.InventoryId == ListingId);

                searchResult.LastUpdated = DateTime.Now;

                searchResult.Vehicle.KBBOptionsId = OptionSelect;

                searchResult.Vehicle.KBBTrimId = TrimId;

                if (context.KellyBlueBooks.Any(x => x.TrimId == TrimId))
                {
                    var searchVinKBBResults = context.KellyBlueBooks.Where(x => x.TrimId == TrimId);

                    foreach (var searchVinKBB in searchVinKBBResults)
                    {
                        searchVinKBB.BaseWholeSale = BaseWholeSale;

                        searchVinKBB.WholeSale = WholeSale;

                        searchVinKBB.MileageAdjustment = MileageAdjustment;

                        searchVinKBB.LastUpdated = DateTime.Now;
                    }


                }

                context.SaveChanges();
            }

        }

        public static void UpdateKBBOptionsForAppraisal(int appraisalId, string optionSelect, int trimId, decimal baseWholeSale, decimal wholeSale, decimal mileageAdjustment)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

                searchResult.LastUpdated = DateTime.Now;

                searchResult.Vehicle.KBBOptionsId = optionSelect;

                searchResult.Vehicle.KBBTrimId = trimId;

                if (context.KellyBlueBooks.Any(x => x.TrimId == trimId))
                {
                    var searchVinKBBResults = context.KellyBlueBooks.Where(x => x.TrimId == trimId);

                    foreach (var searchVinKBB in searchVinKBBResults)
                    {
                        searchVinKBB.BaseWholeSale = baseWholeSale;

                        searchVinKBB.WholeSale = wholeSale;

                        searchVinKBB.MileageAdjustment = mileageAdjustment;

                        searchVinKBB.LastUpdated = DateTime.Now;
                    }


                }

                context.SaveChanges();
            }

        }

   
      



        public static void UpdateDefaultStockImageUrl(int dealershipId, string defaultStockImageUrl)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);

                if (searchResult != null)
                {

                    searchResult.DefaultStockImageUrl = defaultStockImageUrl;

                    context.SaveChanges();
                }
            }



        }

        


        public static void UpdateMsrp(int ListingId, decimal? msrp)
        {

            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Inventories.FirstOrDefault(x => x.InventoryId == ListingId);

                if (searchResult != null)
                {
                    var oldMsrp = searchResult.DealerMsrp;

                    if (oldMsrp.GetValueOrDefault() == msrp.GetValueOrDefault()) return;
                    
                    searchResult.DealerMsrp = msrp;

                    searchResult.Vehicle.Msrp = msrp;

                    searchResult.LastUpdated = DateTime.Now;
                    
                    var emailWaitingList = new EmailWaitingList()
                    {
                        ListingId = searchResult.InventoryId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.MsrpChange,
                        OldValue = oldMsrp,
                        NewValue = searchResult.DealerMsrp,
                        UserId = SessionHandler.CurrentUser.UserId

                    };
                    var log = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description = Constanst.VehicleLogSentence.MsrpChangeByUser.Replace("OLDPRICE", oldMsrp.GetValueOrDefault().ToString("C0"))
                            .Replace("NEWPRICE", searchResult.DealerMsrp.GetValueOrDefault().ToString("C0"))
                            .Replace("USER", SessionHandler.CurrentUser.FullName),
                        InventoryId = searchResult.InventoryId,
                        UserId = SessionHandler.CurrentUser.UserId
                    };

                    context.AddToVehicleLogs(log);

                    context.AddToEmailWaitingLists(emailWaitingList);

                    context.SaveChanges();
                }
               
            }


        }

        public static void UpdateDealerDiscount(int ListingId, decimal? discount)
        {

            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Inventories.FirstOrDefault(x => x.InventoryId == ListingId);

                if (searchResult != null)
                {
                    var oldDealerDiscount = searchResult.DealerDiscount;

                    searchResult.DealerDiscount = discount;

                    searchResult.LastUpdated = DateTime.Now;

                       
                    var emailWaitingList = new EmailWaitingList()
                    {
                        ListingId = searchResult.InventoryId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.DealerDiscountChange,
                        OldValue = oldDealerDiscount,
                        NewValue = searchResult.DealerDiscount,
                        UserId = SessionHandler.CurrentUser.UserId

                    };
                    var log = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description = Constanst.VehicleLogSentence.DealerDiscountChangeByUser.Replace("OLDPRICE", oldDealerDiscount.GetValueOrDefault().ToString("C0"))
                            .Replace("NEWPRICE", searchResult.DealerDiscount.GetValueOrDefault().ToString("C0"))
                            .Replace("USER", SessionHandler.CurrentUser.FullName),
                        InventoryId = searchResult.InventoryId,
                        UserId = SessionHandler.CurrentUser.UserId
                    };

                    context.AddToVehicleLogs(log);

                    context.AddToEmailWaitingLists(emailWaitingList);

                    context.SaveChanges();

                    context.SaveChanges();


                }





            }


        }

        public static void UpdateAppraisal(AppraisalViewFormModel appraisal)
        {
            using (var context = new VincontrolEntities())
            {
                int Id = Convert.ToInt32(appraisal.AppraisalGenerateId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == Id);

                var pureMileage = appraisal.Mileage;

                if (searchResult != null)
                {
                    searchResult.Vehicle.Vin = appraisal.VinNumber;

                    searchResult.Stock = appraisal.StockNumber;

                    searchResult.Vehicle.Make = appraisal.Make;

                    searchResult.Vehicle.Model = appraisal.SelectedModel;

                    searchResult.ExteriorColor = appraisal.SelectedExteriorColorValue;
                    searchResult.Vehicle.ColorCode = appraisal.SelectedExteriorColorCode;

                    searchResult.Vehicle.InteriorColor = appraisal.SelectedInteriorColor;

                    searchResult.Vehicle.Trim = appraisal.SelectedTrim;
                    searchResult.Vehicle.ChromeStyleId = appraisal.ChromeStyleId;

                    searchResult.Mileage = appraisal.Mileage;

                    searchResult.Vehicle.Tranmission = appraisal.SelectedTranmission;

                    searchResult.Vehicle.Cylinders = appraisal.SelectedCylinder;

                    searchResult.Vehicle.Litter = appraisal.SelectedLiters;

                    searchResult.Vehicle.Doors = appraisal.Door;

                    searchResult.Vehicle.BodyType = appraisal.SelectedBodyType;

                    searchResult.Vehicle.FuelType = appraisal.SelectedFuel;

                    searchResult.Vehicle.DriveTrain = appraisal.SelectedDriveTrain;

                    searchResult.Note = appraisal.Notes;

                    searchResult.AdditionalOptions = appraisal.SelectedFactoryOptions;

                    searchResult.AdditionalPackages = appraisal.SelectedPackageOptions;

                    searchResult.OptionCodes = appraisal.AfterSelectedOptionCodes;

                    searchResult.Vehicle.Msrp = appraisal.MSRP;

                    searchResult.Mileage = pureMileage;

                    searchResult.Note = appraisal.Notes;

                    searchResult.LastUpdated = DateTime.Now;

                    searchResult.Vehicle.VehicleType = Constanst.VehicleType.Car;
                    searchResult.PackageDescriptions = appraisal.SelectedPackagesDescription;
                    if (searchResult.Vehicle.Trim != null && searchResult.Vehicle.Trim.Equals("Base/Other Trims"))
                    {
                        searchResult.Vehicle.Trim = String.Empty;
                    }
                }

                context.SaveChanges();

            }

        }

        public static void UpdateTruckAppraisal(AppraisalViewFormModel appraisal)
        {
            using (var context = new VincontrolEntities())
            {
                int Id = Convert.ToInt32(appraisal.AppraisalGenerateId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == Id);

                var pureMileage = appraisal.Mileage;

                if (searchResult != null)
                {
                    searchResult.Vehicle.Vin = appraisal.VinNumber;

                    searchResult.Stock = appraisal.StockNumber;

                    searchResult.Vehicle.Make = appraisal.Make;

                    searchResult.Vehicle.Model = appraisal.SelectedModel;

                    searchResult.ExteriorColor = appraisal.SelectedExteriorColorValue;
                    searchResult.Vehicle.ColorCode = appraisal.SelectedExteriorColorCode;

                    searchResult.Vehicle.InteriorColor = appraisal.SelectedInteriorColor;

                    searchResult.Vehicle.Trim = appraisal.SelectedTrim;

                    searchResult.Vehicle.ChromeStyleId = appraisal.ChromeStyleId;
                
                    searchResult.Mileage = appraisal.Mileage;

                    searchResult.Vehicle.Tranmission = appraisal.SelectedTranmission;

                    searchResult.Vehicle.Cylinders = appraisal.SelectedCylinder;

                    searchResult.Vehicle.Litter = appraisal.SelectedLiters;

                    searchResult.Vehicle.Doors = appraisal.Door;

                    searchResult.Vehicle.BodyType = appraisal.SelectedBodyType;

                    searchResult.Vehicle.FuelType = appraisal.SelectedFuel;

                    searchResult.Vehicle.DriveTrain = appraisal.SelectedDriveTrain;

                    searchResult.Note = appraisal.Notes;

                    searchResult.AdditionalOptions = appraisal.SelectedFactoryOptions;

                    searchResult.AdditionalPackages = appraisal.SelectedPackageOptions;

                    searchResult.Vehicle.Msrp = appraisal.MSRP;

                    searchResult.Mileage = pureMileage;

                    searchResult.Note = appraisal.Notes;

                    searchResult.LastUpdated = DateTime.Now;
                
                    searchResult.PackageDescriptions = appraisal.SelectedPackagesDescription;
                
                    searchResult.Vehicle.TruckType = appraisal.SelectedTruckType;
                    searchResult.Vehicle.TruckCategoryId = appraisal.SelectedTruckCategoryId.Equals(0) ? (int?)null : appraisal.SelectedTruckCategoryId;
                    searchResult.Vehicle.TruckClassId = appraisal.SelectedTruckClassId.Equals(0) ? (int?)null : appraisal.SelectedTruckClassId;
                    searchResult.Vehicle.VehicleType = Constanst.VehicleType.Truck;

                    if (searchResult.Vehicle.Trim != null && searchResult.Vehicle.Trim.Equals("Base/Other Trims"))
                    {
                        searchResult.Vehicle.Trim = String.Empty;
                    }
                }

                context.SaveChanges();
            }

        }

        public static void UpdateIProfile(CarInfoFormViewModel car, DealershipViewModel dealer)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Inventories.FirstOrDefault(x => x.InventoryId == car.ListingId);

                if (searchResult != null)
                {
                    searchResult.Vehicle.Vin = car.Vin;

                    searchResult.AdditionalTitle = car.Title;

                    if (!String.IsNullOrEmpty(searchResult.Stock) && !searchResult.Stock.Equals(car.Stock))
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.StockChangeByUser
                                    .Replace("STOCK1", searchResult.Stock)
                                    .Replace("STOCK2", car.Stock)
                                    .Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }
                    
                    searchResult.Stock = car.Stock;

                    searchResult.Vehicle.Year = Convert.ToInt32(car.ModelYear);

                    searchResult.Vehicle.Make = car.Make;

                    searchResult.Vehicle.Model = car.VehicleModel;

                    if (!String.IsNullOrEmpty(searchResult.ExteriorColor))
                    {
                        if (!searchResult.ExteriorColor.Equals(car.SelectedExteriorColorValue))
                        {
                            var log = new VehicleLog()
                            {
                                DateStamp = DateTime.Now,
                                Description =
                                    Constanst.VehicleLogSentence.ExteriorColorChangeByUser
                                        .Replace("COLOR1", searchResult.ExteriorColor)
                                        .Replace("COLOR2", car.SelectedExteriorColorValue)
                                        .Replace("USER", SessionHandler.CurrentUser.FullName),
                                InventoryId = searchResult.InventoryId,
                                UserId = SessionHandler.CurrentUser.UserId
                            };

                            context.AddToVehicleLogs(log);
                        }
                    }
                    else
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.ExteriorColorChangeByUserFromNull
                                    .Replace("COLOR", car.SelectedExteriorColorValue)
                                    .Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }


                    searchResult.ExteriorColor = car.SelectedExteriorColorValue;

                    if (!String.IsNullOrEmpty(searchResult.Vehicle.InteriorColor))
                    {
                        if (!searchResult.Vehicle.InteriorColor.Equals(car.SelectedInteriorColor))
                        {
                            var log = new VehicleLog()
                            {
                                DateStamp = DateTime.Now,
                                Description =
                                    Constanst.VehicleLogSentence.InteriorColorChangeByUser
                                        .Replace("COLOR1", searchResult.Vehicle.InteriorColor)
                                        .Replace("COLOR2", car.SelectedInteriorColor)
                                        .Replace("USER", SessionHandler.CurrentUser.FullName),
                                InventoryId = searchResult.InventoryId,
                                UserId = SessionHandler.CurrentUser.UserId
                            };

                            context.AddToVehicleLogs(log);
                        }
                    }
                    else
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.InteriorColorChangeByUserFromNull
                                    .Replace("COLOR", car.SelectedInteriorColor)
                                    .Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }
                 

                    searchResult.Vehicle.InteriorColor = car.SelectedInteriorColor;

                    UpdateTrimFromCustomTrim(car.Trim, car.ChromeStyleId, car.CusTrim, searchResult);

                    searchResult.Mileage = car.Mileage;

                    searchResult.Vehicle.Tranmission = car.SelectedTranmission;

                    searchResult.Vehicle.Cylinders = car.Cylinder;

                    searchResult.Vehicle.Litter = car.Litter;

                    searchResult.Vehicle.Doors = car.Door;

                    searchResult.Vehicle.BodyType = car.BodyType;

                    searchResult.Vehicle.FuelType = car.Fuel;

                    searchResult.Vehicle.DriveTrain = car.SelectedDriveTrain;

                    searchResult.Descriptions = car.Description;

                    if (!String.IsNullOrEmpty(searchResult.AdditionalOptions))
                    {
                        if (!searchResult.AdditionalOptions.Equals(car.AfterSelectedOptions))
                        {
                            var log = new VehicleLog()
                            {
                                DateStamp = DateTime.Now,
                                Description =
                                    Constanst.VehicleLogSentence.AddtionalOptionsChangeByUser
                                        .Replace("OPTIONLIST1", searchResult.AdditionalOptions)
                                        .Replace("OPTIONLIST2", car.AfterSelectedOptions)
                                        .Replace("USER", SessionHandler.CurrentUser.FullName),
                                InventoryId = searchResult.InventoryId,
                                UserId = SessionHandler.CurrentUser.UserId
                            };

                            context.AddToVehicleLogs(log);
                        }
                    }
                    else
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.AddtionalOptionsChangeByUserFromNull
                                    .Replace("OPTIONLIST", car.AfterSelectedOptions)
                                    .Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }

                    searchResult.AdditionalOptions = car.AfterSelectedOptions;

                    if (!String.IsNullOrEmpty(searchResult.AdditionalPackages))
                    {
                        if (!searchResult.AdditionalPackages.Equals(car.AfterSelectedPackage))
                        {
                            var log = new VehicleLog()
                            {
                                DateStamp = DateTime.Now,
                                Description =
                                    Constanst.VehicleLogSentence.AddtionalPackagesChangeByUser
                                        .Replace("PACKAGELIST1", searchResult.AdditionalPackages)
                                        .Replace("PACKAGELIST2", car.AfterSelectedPackage)
                                        .Replace("USER", SessionHandler.CurrentUser.FullName),
                                InventoryId = searchResult.InventoryId,
                                UserId = SessionHandler.CurrentUser.UserId
                            };

                            context.AddToVehicleLogs(log);
                        }
                    }
                    else
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.AddtionalPackagesChangeByUserFromNull
                                    .Replace("PACKAGELIST", car.AfterSelectedPackage)
                                    .Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }

                    searchResult.AdditionalPackages = car.AfterSelectedPackage;

                    searchResult.OptionCodes = car.AfterSelectedOptionCodes;

                    searchResult.Vehicle.Msrp = car.Msrp;
                    

                    if (searchResult.Vehicle.VehicleType!=null && car.SelectedVehicleType!=null)
                    {
                        var vehicleType=car.SelectedVehicleType.Equals("Car")
                            ? Constanst.VehicleType.Car
                            : Constanst.VehicleType.Truck;
                        if (searchResult.Vehicle.VehicleType != vehicleType)
                        {
                            var log = new VehicleLog()
                            {
                                DateStamp = DateTime.Now,
                                Description =
                                    Constanst.VehicleLogSentence.VehicleTypeChangeByUser
                                    .Replace("TYPE1", searchResult.Vehicle.VehicleType == Constanst.VehicleType.Car?"Car":"Truck")
                                    .Replace("TYPE2", car.SelectedVehicleType)
                                    .Replace("USER", SessionHandler.CurrentUser.FullName),
                                InventoryId = searchResult.InventoryId,
                                UserId = SessionHandler.CurrentUser.UserId
                            };
                            context.AddToVehicleLogs(log);
                        }
                 
                       
                    }

                    searchResult.Vehicle.VehicleType = car.SelectedVehicleType == null? Constanst.VehicleType.Car
                        : car.SelectedVehicleType.Equals("Car")? Constanst.VehicleType.Car: Constanst.VehicleType.Truck;

                    if (car.IsCertified)
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.CertifiedChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }


                    searchResult.Certified = (car.IsCertified);

                    searchResult.RetailPrice = car.RetailPrice;

                    searchResult.ManufacturerRebate = car.ManufacturerRebate;

                    searchResult.DealerDiscount = car.DealerDiscount;

                    searchResult.WindowStickerPrice = car.WindowStickerPrice;

                    searchResult.LastUpdated = DateTime.Now;

                    searchResult.Vehicle.ColorCode = car.SelectedExteriorColorCode;
                    
                    searchResult.PackageDescriptions = car.SelectedPackagesDescription;

                    if (car.ACar)
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.AcarChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }

                    
                    searchResult.ACar = car.ACar;


                    if (car.BrandedTitle)
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.BrandedTitleChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = searchResult.InventoryId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };

                        context.AddToVehicleLogs(log);
                    }
                    
                    searchResult.BrandedTitle = car.BrandedTitle;
                    
                    searchResult.PackageDescriptions = car.SelectedPackagesDescription;

                    searchResult.Vehicle.TruckType = car.SelectedTruckType;
                    searchResult.Vehicle.TruckCategoryId = car.SelectedTruckCategoryId.Equals(0) ? (int?) null : car.SelectedTruckCategoryId;
                    searchResult.Vehicle.TruckClassId = car.SelectedTruckClassId.Equals(0) ? (int?) null : car.SelectedTruckClassId;
                  
                }

                context.SaveChanges();

            }

        }

        private static void UpdateTrimFromCustomTrim(string Trim, string chromeStyleId, string CusTrim, Inventory searchResult)
        {

            if (!String.IsNullOrEmpty(Trim))
            {
                var result = Trim.Split('|');

                if (result.Count() > 1)
                {
                    if (!result[1].Equals("Base/Other Trims"))
                    {
                        searchResult.Vehicle.Trim = result[1];
                    }
                    else
                    {
                        searchResult.Vehicle.Trim = CusTrim ?? String.Empty;
                    }
                    searchResult.Vehicle.ChromeStyleId = result[0];
                }
                else if (!String.IsNullOrEmpty(CusTrim))
                {
                    if (CusTrim.Equals("Base/Other Trims"))
                        searchResult.Vehicle.Trim = "";
                    else
                    {
                        searchResult.Vehicle.Trim = CusTrim;
                    }
                    searchResult.Vehicle.ChromeStyleId = null;
                }
                else
                {
                    searchResult.Vehicle.Trim = Trim.Equals("Base/Other Trims") ? "" : Trim;
                    searchResult.Vehicle.ChromeStyleId = chromeStyleId;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(CusTrim))
                {
                    if (CusTrim.Equals("Base/Other Trims"))
                        searchResult.Vehicle.Trim = "";
                    else
                    {
                        searchResult.Vehicle.Trim = CusTrim;
                    }
                }
            }
        }

        public static void UpdateITruckProfile(string ListingId, string Vin, string StockNumber, string ModelYear, string Make, string Model, string ExteriorColor, string InteriorColor, string Trim, string Mileage, string Tranmission, string Cylinders, string Liters, string Doors, string Style, string Fuel, string Drive, string Description, string Packages, string Options, string MSRP, bool Certified, string TruckType, string TruckClass, string TruckCategory, string RetailPrice, string DiscountPrice, string ManufacturerRebate, string WindowStickerPrice, DealershipViewModel dealer, string colorCode, string Title, string chromeStyleId, string CusTrim, string selectedPackagesDescription, bool aCar, bool brandedTitle)
        {
            using (var context = new VincontrolEntities())
            {
                int LID = Convert.ToInt32(ListingId);

                var searchResult = context.Inventories.FirstOrDefault(x => x.InventoryId == LID);

                if (searchResult.Mileage == Convert.ToInt64(Mileage))
                {
                    if (searchResult.Condition == Constanst.ConditionStatus.Used)
                    {
                        //if (searchResult.Vehicle.KBBTrimId == null)

                        //    KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.Vehicle.Vin, dealer.ZipCode, Convert.ToInt64(Mileage));
                        //else
                        //{
                        //    KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.Vehicle.Vin, dealer.ZipCode, Convert.ToInt64(Mileage), searchResult.Vehicle.KBBTrimId.Value, searchResult.Vehicle.KBBOptionsId);
                        //}
                    }
                }

                searchResult.Vehicle.Vin = Vin;

                searchResult.AdditionalTitle = Title;

                searchResult.Stock = StockNumber;

                searchResult.Vehicle.Year = Convert.ToInt32(ModelYear);

                searchResult.Vehicle.Make = Make;

                searchResult.Vehicle.Model = Model;

                searchResult.ExteriorColor = ExteriorColor;
                searchResult.Vehicle.ColorCode = colorCode;


                searchResult.Vehicle.InteriorColor = InteriorColor;

                UpdateTrimFromCustomTrim(Trim, chromeStyleId, CusTrim, searchResult);



                searchResult.Mileage = Convert.ToInt64(Mileage);

                searchResult.Vehicle.Tranmission = Tranmission;

                searchResult.Vehicle.Cylinders = Convert.ToInt32(Cylinders);

                searchResult.Vehicle.Litter = Convert.ToInt32(Liters);

                searchResult.Vehicle.Doors = Convert.ToInt32(Doors);

                searchResult.Vehicle.BodyType = Style;

                searchResult.Vehicle.FuelType = Fuel;

                searchResult.Vehicle.DriveTrain = Drive;

                searchResult.Descriptions = Description;

                searchResult.AdditionalPackages = Packages;

                searchResult.AdditionalOptions = Options;

                searchResult.Vehicle.Msrp = Convert.ToDecimal(MSRP);

                searchResult.Certified = (Certified) ? true : false;

                searchResult.RetailPrice = Convert.ToDecimal(RetailPrice);

                searchResult.ManufacturerRebate = Convert.ToInt32(ManufacturerRebate);

                searchResult.DealerDiscount = Convert.ToDecimal(DiscountPrice);

                searchResult.WindowStickerPrice = Convert.ToDecimal(WindowStickerPrice);

                searchResult.LastUpdated = DateTime.Now;

                searchResult.Vehicle.VehicleType = Constanst.VehicleType.Truck;

                //searchResult.TruckCategory = TruckCategory;

                //searchResult.TruckClass = TruckClass;

                //searchResult.TruckType = TruckType;
                searchResult.PackageDescriptions = selectedPackagesDescription;
                searchResult.ACar = aCar;
                searchResult.BrandedTitle = brandedTitle;

                try
                {
                    var autoDescription = new AutoDescription();

                    if (autoDescription.AllowAutoDescription(searchResult, dealer.DealershipId))
                    {
                        var newDescription = autoDescription.GenerateAutoDescription(searchResult, dealer);
                        searchResult.Descriptions = newDescription;
                    }
                }
                catch (Exception)
                {
                    // error happen when generating autodescription
                }

                context.SaveChanges();



            }

        }

        public static void UpdateAcvForAppraisal(string appraisalId, decimal? acv)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                searchResult.ACV = acv;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static decimal? UpdateAcvForAppraisalSearch(string appraisalId, decimal? acv)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                var oldAcv = searchResult.ACV;

                searchResult.ACV = acv;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();

                return oldAcv.GetValueOrDefault();
            }

        }

        public static void UpdateMileageForAppraisal(int appraisalId, long mileage)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

                searchResult.Mileage = mileage;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerFirstNameForAppraisal(string appraisalId, string customerFirstName)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                searchResult.AppraisalCustomer.FirstName = customerFirstName;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerLastNameForAppraisal(string appraisalId, string customerLastName)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                if (searchResult != null)
                {

                    searchResult.AppraisalCustomer.LastName = customerLastName;

                    searchResult.LastUpdated = DateTime.Now;

                    context.SaveChanges();
                }
            }

        }

        public static void UpdateCustomerAddressForAppraisal(string appraisalId, string customerAddress)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                if (searchResult != null)
                {

                    searchResult.AppraisalCustomer.Address = customerAddress;

                    searchResult.LastUpdated = DateTime.Now;

                    context.SaveChanges();
                }
            }

        }

        public static void UpdateCustomerCityForAppraisal(string appraisalId, string customerCity)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                if (searchResult != null)
                {

                    searchResult.AppraisalCustomer.City = customerCity;

                    searchResult.LastUpdated = DateTime.Now;

                    context.SaveChanges();
                }
            }

        }

        public static void UpdateCustomerStateForAppraisal(string appraisalId, string customerState)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                if (searchResult != null)
                {

                    searchResult.AppraisalCustomer.State = customerState;

                    searchResult.LastUpdated = DateTime.Now;

                    context.SaveChanges();
                }
            }

        }

        public static void UpdateCustomerZipCodeForAppraisal(string appraisalId, string customerZipCode)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                if (searchResult != null)
                {

                    searchResult.AppraisalCustomer.ZipCode = customerZipCode;

                    searchResult.LastUpdated = DateTime.Now;

                    context.SaveChanges();
                }
            }

        }

        public static void UpdateCustomerEmailForAppraisal(string appraisalId, string customerEmail)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == lid);

                if (searchResult != null)
                {
                    searchResult.AppraisalCustomer.Email = customerEmail;

                    searchResult.LastUpdated = DateTime.Now;

                    context.SaveChanges();
                }

                
            }

        }


        public static void UpdateCarImageUrl(int listingId, string carImageUrl, string thumbnailImageUrl,string user,int inventoryStatus)
        {
            using (var context = new VincontrolEntities())
            {
                if (inventoryStatus == Constanst.VehicleStatus.Inventory)
                {
                    var searchResult = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

                    if (searchResult != null)
                    {

                        searchResult.PhotoUrl = carImageUrl;

                        searchResult.ThumbnailUrl = thumbnailImageUrl;

                        searchResult.LastUpdated = DateTime.Now;

                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            InventoryId = listingId,
                            UserId = null
                        };

                        if (!String.IsNullOrEmpty(searchResult.PhotoUrl))
                        {

                            log.Description = Constanst.VehicleLogSentence.ImageChangeByUser
                                .Replace("IMAGES",
                                    searchResult.PhotoUrl.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                        .Count()
                                        .ToString(CultureInfo.InvariantCulture))
                                .Replace("USER", user);

                        }
                        else
                        {
                            log.Description = Constanst.VehicleLogSentence.ImageDeleteByUser
                                .Replace("USER", user);
                        }

                        context.AddToVehicleLogs(log);

                        context.SaveChanges();
                    }
                }
                if (inventoryStatus == Constanst.VehicleStatus.SoldOut)
                {
                    var searchResult = context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                    if (searchResult != null)
                    {

                        searchResult.PhotoUrl = carImageUrl;

                        searchResult.ThumbnailUrl = thumbnailImageUrl;

                        searchResult.LastUpdated = DateTime.Now;

                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            SoldOutInventoryId = listingId,
                            UserId = null
                        };

                        if (!String.IsNullOrEmpty(searchResult.PhotoUrl))
                        {

                            log.Description = Constanst.VehicleLogSentence.ImageChangeByUser
                                .Replace("IMAGES",
                                    searchResult.PhotoUrl.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                        .Count()
                                        .ToString(CultureInfo.InvariantCulture))
                                .Replace("USER", user);

                        }
                        else
                        {
                            log.Description = Constanst.VehicleLogSentence.ImageDeleteByUser
                                .Replace("USER", user);
                        }

                        context.AddToVehicleLogs(log);

                        context.SaveChanges();
                    }
                } 

               

                
            }

        }

        public static void UpdateAppraisalCarImageUrl(int appraisalId, string carImageUrl, string thumbnailImageUrl)
        {
            using (var context = new VincontrolEntities())
            {

                var searchResult = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

                if (searchResult != null)
                {

                    searchResult.PhotoUrl = carImageUrl;

                    searchResult.ThumbnailUrl = thumbnailImageUrl;

                    searchResult.LastUpdated = DateTime.Now;
                    context.SaveChanges();
                }

           
            }

        }

       

        public static void UpdateContentAppSetting(int dealershipId, AdminViewModel admin)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);
                if (searchResult != null)
                {
                    searchResult.InventorySorting = admin.DealerSetting.InventorySorting;
                    searchResult.SoldOut = admin.DealerSetting.SoldOut;
                    searchResult.PriceVariance = admin.DealerSetting.PriceVariance;
                    searchResult.SalePriceText = admin.DealerSetting.SalePriceWsNotificationText;
                    searchResult.DealerDiscountText = admin.DealerSetting.DealerDiscountWSNotificationText;
                    searchResult.ManufactureReabateText = admin.DealerSetting.ManufacturerReabteWsNotificationText;
                    searchResult.RetailPriceText = admin.DealerSetting.RetailPriceWSNotificationText;
                    searchResult.EbayContactInfoName = admin.DealerSetting.EbayContactInfoName;
                    searchResult.EbayContactInfoPhone = admin.DealerSetting.EbayContactInfoPhone;
                    searchResult.EbayContactInfoEmail = admin.DealerSetting.EbayContactInfoEmail;

                    searchResult.BrandName = admin.SelectedBrandName;
                    context.SaveChanges();
                }
            }
        }

       

        public static void UpdatePasswordAppSetting(int dealershipId, AdminViewModel admin)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);
                if (searchResult != null)
                {
                    searchResult.CarFax = admin.DealerSetting.CarFax;
                    searchResult.CarFaxPassword = admin.DealerSetting.CarFaxPassword;
                    searchResult.Manheim = admin.DealerSetting.Manheim;
                    searchResult.ManheimPassword = admin.DealerSetting.ManheimPassword;
                    searchResult.KellyBlueBook = admin.DealerSetting.KellyBlueBook;
                    searchResult.KellyPassword = admin.DealerSetting.KellyPassword;
                    searchResult.BlackBook = admin.DealerSetting.BlackBook;
                    searchResult.BlackBookPassword = admin.DealerSetting.BlackBookPassword;
                    context.SaveChanges();
                }

                var craigslistSetting = context.CraigslistSettings.FirstOrDefault(x => x.DealerId == dealershipId);
                if (craigslistSetting != null)
                {
                    craigslistSetting.Email = admin.DealerCraigslistSetting.Email ?? string.Empty;
                    craigslistSetting.Password = admin.DealerCraigslistSetting.Password ?? string.Empty;
                    craigslistSetting.State = admin.DealerCraigslistSetting.State;
                    craigslistSetting.City = admin.DealerCraigslistSetting.CityUrl.Equals("0") ? string.Empty : admin.DealerCraigslistSetting.City;
                    craigslistSetting.CityUrl = admin.DealerCraigslistSetting.CityUrl;
                    craigslistSetting.Location = admin.DealerCraigslistSetting.LocationId.Equals(0) ? string.Empty : admin.DealerCraigslistSetting.Location;
                    craigslistSetting.LocationId = admin.DealerCraigslistSetting.LocationId;
                    craigslistSetting.SpecificLocation = admin.DealerCraigslistSetting.SpecificLocation;
                    craigslistSetting.EndingSentence = HttpUtility.UrlDecode(admin.DealerCraigslistSetting.EndingSentence);
                    context.SaveChanges();
                }
                else
                {
                    var newSetting = new CraigslistSetting()
                    {
                        DealerId = dealershipId,
                        Email = admin.DealerCraigslistSetting.Email ?? string.Empty,
                        Password = admin.DealerCraigslistSetting.Password ?? string.Empty,
                        State = admin.DealerCraigslistSetting.State,
                        City = admin.DealerCraigslistSetting.CityUrl.Equals("0") ? string.Empty : admin.DealerCraigslistSetting.City,
                        CityUrl = admin.DealerCraigslistSetting.CityUrl,
                        Location = admin.DealerCraigslistSetting.LocationId.Equals(0) ? string.Empty : admin.DealerCraigslistSetting.Location,
                        LocationId = admin.DealerCraigslistSetting.LocationId,
                        SpecificLocation = admin.DealerCraigslistSetting.SpecificLocation,
                        EndingSentence = HttpUtility.UrlDecode(admin.DealerCraigslistSetting.EndingSentence)
                    };
                    context.AddToCraigslistSettings(newSetting);
                    context.SaveChanges();
                }
            }
        }

        public static void UpdateStockingGuideSetting(int dealershipId, Setting setting)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);

                if (searchResult != null)
                {
                    searchResult.BrandName = setting.BrandName;
                    searchResult.AverageCost = setting.AverageCost;
                    searchResult.AverageProfitUsage = setting.AverageProfitUsage;
                    searchResult.TireCost = setting.TireCost;
                    searchResult.FrontBumperCost = setting.FrontBumperCost;
                    searchResult.RearBumperCost = setting.RearBumperCost;
                    searchResult.GlassCost = setting.GlassCost;
                    searchResult.FrontEndCost = setting.FrontEndCost;
                    searchResult.RearEndCost = setting.RearEndCost;
                    searchResult.DriverSideCost = setting.DriverSideCost;
                    searchResult.PassengerSideCost = setting.PassengerSideCost;
                    searchResult.LightBulbCost = setting.LightBulbCost;
                    searchResult.FrontWindShieldCost = setting.FrontWindShieldCost;
                    searchResult.RearWindShieldCost = setting.RearWindShieldCost;
                    searchResult.DriverWindowCost = setting.DriverWindowCost;
                    searchResult.PassengerWindowCost = setting.PassengerWindowCost;
                    searchResult.DriverSideMirrorCost = setting.DriverSideMirrorCost;
                    searchResult.PassengerSideMirrorCost = setting.PassengerSideMirrorCost;
                    searchResult.ScratchCost = setting.ScratchCost;
                    searchResult.MajorScratchCost = setting.MajorScratchCost;
                    searchResult.DentCost = setting.DentCost;
                    searchResult.MajorDentCost = setting.MajorDentCost;
                    searchResult.PaintDamageCost = setting.PaintDamageCost;
                    searchResult.RepaintedPanelCost = setting.RepaintedPanelCost;
                    searchResult.RustCost = setting.RustCost;
                    searchResult.AcidentCost = setting.AcidentCost;
                    searchResult.MissingPartsCost = setting.MissingPartsCost;
                    

                    searchResult.TireRetail = setting.TireRetail;
                    searchResult.FrontBumperRetail = setting.FrontBumperRetail;
                    searchResult.RearBumperRetail = setting.RearBumperRetail;
                    searchResult.GlassRetail = setting.GlassRetail;
                    searchResult.FrontEndRetail = setting.FrontEndRetail;
                    searchResult.RearEndRetail = setting.RearEndRetail;
                    searchResult.DriverSideRetail = setting.DriverSideRetail;
                    searchResult.PassengerSideRetail = setting.PassengerSideRetail;
                    searchResult.LightBulbRetail = setting.LightBulbRetail;
                    searchResult.FrontWindShieldRetail = setting.FrontWindShieldRetail;
                    searchResult.RearWindShieldRetail = setting.RearWindShieldRetail;
                    searchResult.DriverWindowRetail = setting.DriverWindowRetail;
                    searchResult.PassengerWindowRetail = setting.PassengerWindowRetail;
                    searchResult.DriverSideMirrorRetail = setting.DriverSideMirrorRetail;
                    searchResult.PassengerSideMirrorRetail = setting.PassengerSideMirrorRetail;
                    searchResult.ScratchRetail = setting.ScratchRetail;
                    searchResult.MajorScratchRetail = setting.MajorScratchRetail;
                    searchResult.DentRetail = setting.DentRetail;
                    searchResult.MajorDentRetail = setting.MajorDentRetail;
                    searchResult.PaintDamageRetail = setting.PaintDamageRetail;
                    searchResult.RepaintedPanelRetail = setting.RepaintedPanelRetail;
                    searchResult.RustRetail = setting.RustRetail;
                    searchResult.AcidentRetail = setting.AcidentRetail;
                    searchResult.MissingPartsRetail = setting.MissingPartsRetail;
                    

                    if (setting.AverageProfitUsage == 1)
                    {
                        searchResult.AverageProfit = setting.AverageProfit;
                    }
                    else
                    {
                        searchResult.AverageProfitPercentage = setting.AverageProfitPercentage;
                    }

                    context.SaveChanges();
                }
            }
        }

        public static void UpdateNotificationAppSetting(int dealershipId, AdminViewModel admin)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);
                if (searchResult != null)
                {
                    searchResult.FirstTimeRangeBucketJump = admin.DealerSetting.FirstTimeRangeBucketJump;
                    searchResult.SecondTimeRangeBucketJump = admin.DealerSetting.SecondTimeRangeBucketJump;
                    searchResult.IntervalBucketJump = admin.DealerSetting.IntervalBucketJump;
                    context.SaveChanges();
                }
            }
        }

        public static void UpdatePass(string pass, int userId)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Users.FirstOrDefault(x => x.UserId == userId);

                if (searchResult != null)
                {
                    searchResult.Password = pass;

                    context.SaveChanges();
                }
            }
        }

        public static void UpdateEmail(string email, int userId)
        {

            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (searchResult != null)
                {
                    searchResult.Email = email;

                    context.SaveChanges();
                }
            }

        }

        public static void UpdateCellPhone(string cellPhone, int userId)
        {

            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Users.FirstOrDefault(x => x.UserId == userId);

                if (searchResult != null)
                {
                    searchResult.CellPhone = cellPhone;

                    context.SaveChanges();
                }
            }

        }

        public static void UpdateDefaultLogin(string username, int defaultLogin)
        {

            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Users.FirstOrDefault(x => x.UserName == username);

                if (searchResult != null)
                {
                    searchResult.DefaultLogin = defaultLogin;

                    context.SaveChanges();
                }
            }

        }

   

        public static void DeleteUser(int userId)
        {
            using (var context = new VincontrolEntities())
            {

                var searchResult = context.Users.Where(x => x.UserId == userId);
                var commonManagement = new vincontrol.Application.Vinchat.Forms.CommonManagement.CommonManagementForm();
               

                foreach (var tmp in searchResult)
                {
                    tmp.Active = false;
                     commonManagement.DeleteRosterUsers(tmp.UserName);
                }

                context.SaveChanges();
            }
        }

        public static bool CheckMasterUserExist(string userName, string passWord)
        {
            using (var context = new VincontrolEntities())
            {
                var user = context.Users.FirstOrDefault(o => o.UserName == userName && o.UserName == passWord);
                if (user != null)
                {
                    var checkRole =
                        context.UserPermissions.FirstOrDefault(
                            x => x.UserId == user.UserId && x.RoleId == Constanst.RoleType.Master);
                    if (checkRole != null)
                        return true;
                }
            }
            return false;
        }

        public static void SwitchUserRole(ref UserRoleViewModel currentUser, string selectedDealerTransferID)
        {
            using (var context = new VincontrolEntities())
            {
                int userId = currentUser.UserId;
                var associateRoles = context.UserPermissions.Where(x => x.UserId == userId);
                int switchDealerId = 0;

                if (associateRoles.Any() && int.TryParse(selectedDealerTransferID, out switchDealerId))
                {
                    var firstOrDefault = associateRoles.FirstOrDefault(x => x.DealerId == switchDealerId);

                    if (firstOrDefault != null && firstOrDefault.RoleId != currentUser.RoleId)
                    {
                        currentUser.RoleId = firstOrDefault.RoleId;

                        switch (currentUser.RoleId)
                        {
                            case Constanst.RoleType.Admin:
                                currentUser.Role = Constanst.RoleTypeText.Admin;
                                break;
                            case Constanst.RoleType.Manager:
                                currentUser.Role = Constanst.RoleTypeText.Manager;
                                break;
                            case Constanst.RoleType.Employee:
                                currentUser.Role = Constanst.RoleTypeText.Employee;
                                break;
                        }
                    }

                    if (currentUser.RoleId != Constanst.RoleType.Admin && currentUser.RoleId != Constanst.RoleType.Master)
                    {
                        currentUser.ProfileButtonPermissions = GetButtonList(currentUser, "Profile");
                    }
                }
            }
        }

        public static UserRoleViewModel CheckUserExistWithStatus(string userName, string passWord)
        {
            var user = new UserRoleViewModel();
            using (var context = new VincontrolEntities())
            {
                var result =
                    context.Users.FirstOrDefault(
                        o => o.UserName == userName && o.Password == passWord && o.Active == true);
                if (result != null)
                {

                    var associateRoles = context.UserPermissions.Where(x => x.UserId == result.UserId);

                    var accessDealerPermissions = associateRoles.Select(i => new RoleDealerAccess
                    {
                        DealerId = i.DealerId,
                        RoleId = i.RoleId

                    }).ToList();

                    if (associateRoles.Any())
                    {
                        if (associateRoles.Count() == 1 &&
                            associateRoles.First().RoleId == Constanst.RoleType.Master)
                        {


                            user = new UserRoleViewModel
                            {
                                UserId = result.UserId,
                                Username = result.UserName,
                                Password = result.Password,
                                MasterLogin = true,
                                Name = result.DealerGroup.DealerGroupName,
                                FullName = result.Name,
                                Cellphone = result.CellPhone,
                                MultipleDealerLogin = true,
                                DealershipId = result.DefaultLogin.GetValueOrDefault(),
                                DefaultLogin = result.DefaultLogin.GetValueOrDefault(),
                                DealerGroupId = result.DealerGroupId,
                                RoleId = Constanst.RoleType.Master,
                                Role = Constanst.RoleTypeText.Master,
                                AccessDealerPermissions = accessDealerPermissions

                            };
                        }
                        else
                        {
                            user = new UserRoleViewModel
                            {
                                UserId = result.UserId,
                                Username = result.UserName,
                                Password = result.Password,
                                Name = associateRoles.First().Dealer.Name,
                                FullName = result.Name,
                                Cellphone = result.CellPhone,
                                DealershipId = result.DefaultLogin.GetValueOrDefault(),
                                DefaultLogin = result.DefaultLogin.GetValueOrDefault(),
                                DealerGroupId = result.DealerGroupId,
                                RoleId = associateRoles.First().RoleId,
                                AccessDealerPermissions = accessDealerPermissions

                            };

                            if (user.RoleId == Constanst.RoleType.Admin)
                                user.Role = Constanst.RoleTypeText.Admin;
                            else if (user.RoleId == Constanst.RoleType.Manager)
                            {
                                user.Role = Constanst.RoleTypeText.Manager;

                                user.ProfileButtonPermissions = GetButtonList(user, "Profile");

                            }
                            else if (user.RoleId == Constanst.RoleType.Employee)
                            {
                                user.Role = Constanst.RoleTypeText.Employee;
                                user.ProfileButtonPermissions = GetButtonList(user, "Profile");
                            }

                            if (associateRoles.Count() > 1)
                                user.MultipleDealerLogin = true;
                        }
                    }

                }
            }


            return user;

        }

        public static UserRoleViewModel CheckUserExistWithStatus(int UserId)
        {
            var user = new UserRoleViewModel();
            using (var context = new VincontrolEntities())
            {
                var result =
                    context.Users.FirstOrDefault(o => o.UserId == UserId);
                if (result != null)
                {

                    var associateRoles = context.UserPermissions.Where(x => x.UserId == result.UserId);

                    var accessDealerPermissions = associateRoles.Select(i => new RoleDealerAccess
                    {
                        DealerId = i.DealerId,
                        RoleId = i.RoleId

                    }).ToList();

                    if (associateRoles.Any())
                    {
                        if (associateRoles.Count() == 1 &&
                            associateRoles.First().RoleId == Constanst.RoleType.Master)
                        {


                            user = new UserRoleViewModel
                            {
                                UserId = result.UserId,
                                Username = result.UserName,
                                Password = result.Password,
                                MasterLogin = true,
                                Name = result.DealerGroup.DealerGroupName,
                                FullName = result.Name,
                                Cellphone = result.CellPhone,
                                MultipleDealerLogin = true,
                                DealershipId = result.DefaultLogin.GetValueOrDefault(),
                                DefaultLogin = result.DefaultLogin.GetValueOrDefault(),
                                DealerGroupId = result.DealerGroupId,
                                RoleId = Constanst.RoleType.Master,
                                Role = Constanst.RoleTypeText.Master,
                                AccessDealerPermissions = accessDealerPermissions

                            };
                        }
                        else
                        {
                            user = new UserRoleViewModel
                            {
                                UserId = result.UserId,
                                Username = result.UserName,
                                Password = result.Password,
                                Name = associateRoles.First().Dealer.Name,
                                FullName = result.Name,
                                Cellphone = result.CellPhone,
                                DealershipId = result.DefaultLogin.GetValueOrDefault(),
                                DefaultLogin = result.DefaultLogin.GetValueOrDefault(),
                                DealerGroupId = result.DealerGroupId,
                                RoleId = associateRoles.First().RoleId,
                                AccessDealerPermissions = accessDealerPermissions

                            };

                            if (user.RoleId == Constanst.RoleType.Admin)
                                user.Role = Constanst.RoleTypeText.Admin;
                            else if (user.RoleId == Constanst.RoleType.Manager)
                            {
                                user.Role = Constanst.RoleTypeText.Manager;

                                user.ProfileButtonPermissions = GetButtonList(user, "Profile");

                            }
                            else if (user.RoleId == Constanst.RoleType.Employee)
                            {
                                user.Role = Constanst.RoleTypeText.Employee;
                                user.ProfileButtonPermissions = GetButtonList(user, "Profile");
                            }

                            if (associateRoles.Count() > 1)
                                user.MultipleDealerLogin = true;
                        }
                    }
                }

            }


            return user;

        }

        public static User GetDefaultLoginFromUserName(int userId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                   context.Users.First(
                       o => o.UserId == userId);
                return result;
            }
        }

        public static bool CheckUserNameExist(string userName, int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.Users.Any(o => o.UserName == userName))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckUserNameExist(string userName)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.Users.Any(o => o.UserName == userName))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckUserEmailExist(string email, int notThisUserId = 0)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.Users.Any(o => o.Email.Trim().ToLower() == email.Trim().ToLower() && o.UserId != notThisUserId))
                {
                    return true;
                }
            }
            return false;
        }

        public static User GetUserByName(string userName)
        {
            using (var context = new VincontrolEntities())
            {
                return (context.Users.FirstOrDefault(o => o.UserName == userName));
            }
        }

        public static User GetUserByEmail(string email)
        {
            using (var context = new VincontrolEntities())
            {
                return (context.Users.FirstOrDefault(o => !string.IsNullOrEmpty(o.Email) && o.Email.ToLower() == email.ToLower()));
            }
        }

        public static bool AddUser(UserRoleViewModel user, out bool bIsUserNameExist, out bool bIsUserEmailExist)
        {
            bIsUserNameExist = CheckUserNameExist(user.Username);
            bIsUserEmailExist = CheckUserEmailExist(user.Email);

            if (!bIsUserNameExist && !bIsUserEmailExist)
            {
                using (var context = new VincontrolEntities())
                {
                    var vincontrolUser = new User
                    {
                        UserName = user.Username,
                        Expiration = DateTime.Now.AddYears(10),
                        Name = user.Name,
                        DealerGroupId = user.DealerGroupId,
                        Password = user.Password,
                        CellPhone = user.Cellphone,
                        Active = true,
                        DefaultLogin = user.DefaultLogin,
                        Email = user.Email
                    };

                    context.AddToUsers(vincontrolUser);

                    var permissionUser = new UserPermission
                    {
                        DealerId = user.DefaultLogin,
                        RoleId = user.RoleId,
                        UserId = vincontrolUser.UserId

                    };

                    context.AddToUserPermissions(permissionUser);
                    context.SaveChanges();
                }
                //TODO:AddUserChat
                //if (user.RoleId == 1)
                //{
                //    using (var contextVinChat = new Entities())
                //    {
                //        var userVinChat = new user
                //                              {
                //                                  username = user.Username,
                //                                  password = user.Password,
                //                                  created = DateTime.Now
                //                              };
                //        contextVinChat.users.Add(userVinChat);

                //        var context = new VincontrolEntities();
                //        var listUserGroup = context.Users.Where(o => o.DealerGroupId == user.DealerGroupId);
                //        foreach (var userVinControl in listUserGroup)
                //        {
                //            var rosterUserA = new rosteruser()
                //            {
                //                username = userVinControl.UserName,
                //                jid = string.Format("{0}@{1}", user.Username, "localhost"),
                //                nick = string.Empty,
                //                subscription = "B",
                //                ask = "N",
                //                askmessage = string.Empty,
                //                server = "N",
                //                subscribe = string.Empty,
                //                type = "item"
                //            };

                //            contextVinChat.rosterusers.Add(rosterUserA);

                //            var rosterUserB = new rosteruser()
                //            {
                //                username = user.Username,
                //                jid = string.Format("{0}@{1}", userVinControl.UserName, "localhost"),
                //                nick = string.Empty,
                //                subscription = "B",
                //                ask = "N",
                //                askmessage = string.Empty,
                //                server = "N",
                //                subscribe = string.Empty,
                //                type = "item"
                //            };

                //            contextVinChat.rosterusers.Add(rosterUserB);
                //        }

                //        contextVinChat.SaveChanges();
                //    }
                //}

                return true;
            }

            return false;
        }


        public static User AddUserMultipleStore(UserRoleViewModel user, string[] dealerList)
        {
            using (var context = new VincontrolEntities())
            {
                var vincontrolUser = new User
                {
                    UserName = user.Username,
                    Expiration = DateTime.Now.AddYears(10),
                    Name = user.Name,
                    DealerGroupId = user.DealerGroupId,
                    Password = user.Password,
                    CellPhone = user.Cellphone,
                    Active = true,
                    DefaultLogin = user.DefaultLogin,
                    Email = user.Email,

                };
                context.AddToUsers(vincontrolUser);

                foreach (var tmp in dealerList)
                {
                    var permissionUser = new UserPermission
                    {
                        DealerId = Convert.ToInt32(tmp),
                        RoleId = user.RoleId,
                        UserId = user.UserId,

                    };

                    context.AddToUserPermissions(permissionUser);
                }

                //TODO:AddUserChat
                //if (user.RoleId == 1)
                //{
                //    using (var contextVinChat = new Entities())
                //    {
                //        var userVinChat = new user
                //        {
                //            username = user.Username,
                //            password = user.Password,
                //            created = DateTime.Now
                //        };
                //        contextVinChat.users.Add(userVinChat);

                //        var listUserGroup = context.Users.Where(o => o.DealerGroupId == user.DealerGroupId);
                //        foreach (var userVinControl in listUserGroup)
                //        {
                //            var rosterUserA = new rosteruser()
                //            {
                //                username = userVinControl.UserName,
                //                jid = string.Format("{0}@{1}", user.Username, "localhost"),
                //                nick = string.Empty,
                //                subscription = "B",
                //                ask = "N",
                //                askmessage = string.Empty,
                //                server = "N",
                //                subscribe = string.Empty,
                //                type = "item"
                //            };

                //            contextVinChat.rosterusers.Add(rosterUserA);

                //            var rosterUserB = new rosteruser()
                //            {
                //                username = user.Username,
                //                jid = string.Format("{0}@{1}", userVinControl.UserName, "localhost"),
                //                nick = string.Empty,
                //                subscription = "B",
                //                ask = "N",
                //                askmessage = string.Empty,
                //                server = "N",
                //                subscribe = string.Empty,
                //                type = "item"
                //            };

                //            contextVinChat.rosterusers.Add(rosterUserB);
                //        }

                //        contextVinChat.SaveChanges();
                //    }
                //}

                context.SaveChanges();
                return vincontrolUser;
            }

        }

        public static void AssignUserPermission(UserRoleViewModel user, string[] dealerList)
        {
            using (var context = new VincontrolEntities())
            {
                var dbUser = context.Users.FirstOrDefault(i => i.UserId == user.UserId);
                if (dbUser != null) dbUser.DefaultLogin = user.DefaultLogin;

                var permission = context.UserPermissions.Where(i => i.UserId == user.UserId);
                foreach (var item in permission)
                {
                    context.DeleteObject(item);
                }

                foreach (var tmp in dealerList)
                {

                    var permissionUser = new UserPermission
                    {
                        DealerId = Convert.ToInt32(tmp),
                        RoleId = user.RoleId,
                        UserId = user.UserId
                    };

                    context.AddToUserPermissions(permissionUser);


                    context.SaveChanges();
                }

            }
        }


        public static void TransferToWholeSaleFromInventory(int listingId)
        {
            using (var context = new VincontrolEntities())
            {
                var inventory = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

                if (inventory != null)
                {
                    inventory.InventoryStatusCodeId = Constanst.InventoryStatus.Wholesale;
                    context.SaveChanges();
                }
            }
        }


        public static void TransferToWholeSaleFromSoldInventory(int listingId)
        {
            using (var context = new VincontrolEntities())
            {
                var soldOutInventory = context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                var vehicle = MappingHanlder.ConvertToInventory(soldOutInventory, Constanst.InventoryStatus.Wholesale);

                context.DeleteObject(soldOutInventory);

                context.AddToInventories(vehicle);

                context.SaveChanges();


            }
        }

        public static int TransferToInventoryFromWholesale(int listingId)
        {
            using (var context = new VincontrolEntities())
            {
                var wdi = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
                if (wdi != null)
                {
                    wdi.InventoryStatusCodeId = Constanst.InventoryStatus.Inventory;
                    context.SaveChanges();
                    return wdi.InventoryId;
                }
            }
            return 0;
        }


        public static int MarkSoldVehicle(int listingId, CustomeInfoModel customer)
        {
            using (var context = new VincontrolEntities())
            {
                var inventory = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
                if (inventory != null)
                {
                    var inventoryId = inventory.InventoryId;

                    var soldVehicle = MappingHanlder.ConvertToSoldoutInventory(inventory, SessionHandler.CurrentUser.Username, customer);
                    
                    var vehicleLogs = context.VehicleLogs.Where(i => i.InventoryId == inventoryId);

                    var facebookLogs = context.FBPostTrackings.Where(i => i.InventoryId == inventoryId);

                    var vehiclesLogIdentity = new List<int>();

                    var facebooksLogIdentity = new List<int>();

                    if (vehicleLogs.Any())
                    {
                        vehiclesLogIdentity = vehicleLogs.Select(x => x.VehicleLogId).ToList();

                        foreach (var vehicleLog in vehicleLogs)
                        {
                            vehicleLog.InventoryId = null;


                        }

                    }
                    if (facebookLogs.Any())
                    {
                        facebooksLogIdentity = facebookLogs.Select(x => x.FBPostId).ToList();

                        foreach (var facebookLog in facebookLogs)
                        {
                            facebookLog.InventoryId = null;


                        }

                    }

                    context.DeleteObject(inventory);

                    context.AddToSoldoutInventories(soldVehicle);
                    
                    context.SaveChanges();

                    var priceChanges =
                        context.PriceChangeHistories.Where(
                            i =>
                            i.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory &&
                            i.ListingId == inventoryId);

                    foreach (var priceChange in priceChanges)
                    {
                        priceChange.VehicleStatusCodeId = Constanst.VehicleStatus.SoldOut;
                        priceChange.ListingId = soldVehicle.SoldoutInventoryId;
                    }

                    foreach (var tmpLog in context.VehicleLogs.Where(x => x.InventoryId == null).ToList())
                    {
                        if (vehiclesLogIdentity.Contains(tmpLog.VehicleLogId))
                        {
                            tmpLog.SoldOutInventoryId = soldVehicle.SoldoutInventoryId;

                        }
                    }
                    foreach (var tmpLog in context.FBPostTrackings.Where(x => x.InventoryId == null).ToList())
                    {
                        if (facebooksLogIdentity.Contains(tmpLog.FBPostId))
                        {
                            tmpLog.SoldInventoryId = soldVehicle.SoldoutInventoryId;

                        }
                    }

                    var log = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description =Constanst.VehicleLogSentence.SoldByUser.Replace("USER",SessionHandler.CurrentUser.FullName),
                        SoldOutInventoryId = soldVehicle.SoldoutInventoryId,
                        UserId = SessionHandler.CurrentUser.UserId
                    };

                    context.AddToVehicleLogs(log);

                    context.SaveChanges();

                    return soldVehicle.SoldoutInventoryId;
                }
                
                return 0;



            }



        }

        public static int MarkSoldAppraisal(int appraisalId, CustomeInfoModel customer)
        {
            using (var context = new VincontrolEntities())
            {
                var appraisal = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

                var soldVehicle = MappingHanlder.ConvertToSoldoutInventory(appraisal, SessionHandler.CurrentUser.Username, customer);

                var log = new VehicleLog()
                {
                    DateStamp = DateTime.Now,
                    Description = Constanst.VehicleLogSentence.AppraisedAndSoldByUser.Replace("USER", SessionHandler.CurrentUser.FullName),
                    SoldOutInventoryId = soldVehicle.SoldoutInventoryId,
                    UserId = SessionHandler.CurrentUser.UserId
                };

                context.AddToVehicleLogs(log);

                context.AddToSoldoutInventories(soldVehicle);

                context.SaveChanges();

                return soldVehicle.SoldoutInventoryId;

            }
        }

   

        public static void UpdateNotificationPerUser(bool notify, int dealerId, int userId, int notificationkind)
        {

            using (var context = new VincontrolEntities())
            {
                var setting = context.UserNotifications.FirstOrDefault(x => x.DealerId == dealerId && x.UserId == userId);
                if (setting == null)
                {
                    setting = new UserNotification();
                    setting.UserId = userId;
                    setting.DealerId = dealerId;
                    setting.DateStamp = DateTime.Now;
                    switch (notificationkind)
                    {
                        case 0:
                            setting.AppraisalNotified = notify;
                            break;
                        case 1:
                            setting.WholesaleNotified = notify;
                            break;
                        case 2:
                            setting.InventoryNotified = notify;
                            break;
                        case 3:
                            setting.C24hNotified = notify;
                            break;
                        case 4:
                            setting.NoteNotified = notify;
                            break;
                        case 5:
                            setting.PriceChangeNotified = notify;
                            break;
                        case 6:
                            setting.AgingNotified = notify;
                            break;
                        case 7:
                            setting.MarketPriceRangeNotified = notify;
                            break;
                        case 8:
                            setting.BucketJumpNotified = notify;
                            break;
                        case 9:
                            setting.ImageUploadNotified = notify;
                            break;
                    }
                    context.AddToUserNotifications(setting);
                }
                else
                {
                    switch (notificationkind)
                    {
                        case 0:
                            setting.AppraisalNotified = notify;
                            break;
                        case 1:
                            setting.WholesaleNotified = notify;
                            break;
                        case 2:
                            setting.InventoryNotified = notify;
                            break;
                        case 3:
                            setting.C24hNotified = notify;
                            break;
                        case 4:
                            setting.NoteNotified = notify;
                            break;
                        case 5:
                            setting.PriceChangeNotified = notify;
                            break;
                        case 6:
                            setting.AgingNotified = notify;
                            break;
                        case 7:
                            setting.MarketPriceRangeNotified = notify;
                            break;
                        case 8:
                            setting.BucketJumpNotified = notify;
                            break;
                        case 9:
                            setting.ImageUploadNotified = notify;
                            break;
                    }
                }
                context.SaveChanges();


            }



        }

        public static void UpdateWindowStickerNotification(bool notify, int dealerId, int notificationkind)
        {
            using (var context = new VincontrolEntities())
            {
                var setting = context.Settings.FirstOrDefault(x => x.DealerId == dealerId);
                if (setting != null)
                {
                    if (notificationkind == Constanst.NotificationType.RetailPrice)
                        setting.RetailPrice = notify;
                    else if (notificationkind == Constanst.NotificationType.DealerDiscount)
                        setting.DealerDiscount = notify;
                    else if (notificationkind == Constanst.NotificationType.Manufacturer)
                        setting.ManufacturerReabte = notify;
                    else if (notificationkind == Constanst.NotificationType.SalePrice)
                        setting.SalePrice = notify;
                    context.SaveChanges();
                }
            }
        }

        public static int MarkUnsoldVehicle(int listingId)
        {
            using (var context = new VincontrolEntities())
            {
                var soldOutInventory = context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                var vehicle = MappingHanlder.ConvertToInventory(soldOutInventory, Constanst.InventoryStatus.Inventory);

                var vehicleLogs = context.VehicleLogs.Where(i => i.SoldOutInventoryId == listingId);

                var facebookLogs = context.FBPostTrackings.Where(i => i.SoldInventoryId == listingId);

                var vehiclesLogIdentity = new List<int>();

                var facebooksLogIdentity = new List<int>();

                if (vehicleLogs.Any())
                {
                    vehiclesLogIdentity = vehicleLogs.Select(x => x.VehicleLogId).ToList();

                    foreach (var vehicleLog in vehicleLogs)
                    {
                        vehicleLog.SoldOutInventoryId = null;


                    }

                }

                if (facebookLogs.Any())
                {
                    facebooksLogIdentity = facebookLogs.Select(x => x.FBPostId).ToList();

                    foreach (var facebookLog in facebookLogs)
                    {
                        facebookLog.SoldInventoryId = null;


                    }

                }


                context.DeleteObject(soldOutInventory);

                context.AddToInventories(vehicle);

                context.SaveChanges();

                foreach (var tmpLog in context.VehicleLogs.Where(x => x.SoldOutInventoryId == null).ToList())
                {
                    if (vehiclesLogIdentity.Contains(tmpLog.VehicleLogId))
                    {
                        tmpLog.InventoryId = vehicle.InventoryId;

                    }
                }
                foreach (var tmpLog in context.FBPostTrackings.Where(x => x.SoldInventoryId == null).ToList())
                {
                    if (facebooksLogIdentity.Contains(tmpLog.FBPostId))
                    {
                        tmpLog.InventoryId = vehicle.InventoryId;

                    }
                }


                var log = new VehicleLog()
                {
                    DateStamp = DateTime.Now,
                    Description = Constanst.VehicleLogSentence.UnSoldToInventoryByUser.Replace("USER", SessionHandler.CurrentUser.FullName),
                    InventoryId = vehicle.InventoryId,
                    UserId = SessionHandler.CurrentUser.UserId
                };

                context.AddToVehicleLogs(log);

                context.SaveChanges();

                return vehicle.InventoryId;
            }



        }

        public static int MarkUnsoldVehicle(int listingId, short statusID)
        {
            using (var context = new VincontrolEntities())
            {
                var soldOutInventory = context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                var vehicle = MappingHanlder.ConvertToInventory(soldOutInventory, statusID);

                var vehicleLogs = context.VehicleLogs.Where(i => i.SoldOutInventoryId == listingId);

                var facebookLogs = context.FBPostTrackings.Where(i => i.SoldInventoryId == listingId);


                var vehiclesLogIdentity = new List<int>();

                var facebooksLogIdentity = new List<int>();

                if (vehicleLogs.Any())
                {
                    vehiclesLogIdentity = vehicleLogs.Select(x => x.VehicleLogId).ToList();

                    foreach (var vehicleLog in vehicleLogs)
                    {
                        vehicleLog.SoldOutInventoryId = null;


                    }

                }
                if (facebookLogs.Any())
                {
                    facebooksLogIdentity = facebookLogs.Select(x => x.FBPostId).ToList();

                    foreach (var facebookLog in facebookLogs)
                    {
                        facebookLog.SoldInventoryId = null;


                    }

                }

                context.DeleteObject(soldOutInventory);

                context.AddToInventories(vehicle);

                context.SaveChanges();


                foreach (var tmpLog in context.VehicleLogs.Where(x => x.SoldOutInventoryId == null).ToList())
                {
                    if (vehiclesLogIdentity.Contains(tmpLog.VehicleLogId))
                    {
                        tmpLog.InventoryId = vehicle.InventoryId;

                    }
                }
                foreach (var tmpLog in context.FBPostTrackings.Where(x => x.SoldInventoryId == null).ToList())
                {
                    if (facebooksLogIdentity.Contains(tmpLog.FBPostId))
                    {
                        tmpLog.InventoryId = vehicle.InventoryId;

                    }
                }
                var log = new VehicleLog()
                {
                    DateStamp = DateTime.Now,
                    Description = Constanst.VehicleLogSentence.UnSoldToInventoryByUser.Replace("USER", SessionHandler.CurrentUser.FullName),
                    InventoryId = vehicle.InventoryId,
                    UserId = SessionHandler.CurrentUser.UserId
                };

                context.AddToVehicleLogs(log);

                context.SaveChanges();

                return vehicle.InventoryId;
            }



        }

        public static int InsertToInvetory(int appraisalId,string stock, short inventoryStatusCodeId, DealershipViewModel dealer)
        {

            using (var context = new VincontrolEntities())
            {
                var appraisal = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

                var vehicle = MappingHanlder.ConvertToInventory(appraisal, dealer);
                vehicle.AddToInventoryById = SessionHandler.CurrentUser.UserId;
                vehicle.InventoryStatusCodeId = inventoryStatusCodeId;
                vehicle.Stock = stock;

                context.AddToInventories(vehicle);

                context.SaveChanges();

               
                var appraisallog = new VehicleLog()
                {
                    DateStamp = appraisal.AppraisalDate,
                    InventoryId = vehicle.InventoryId,
                    UserId = SessionHandler.CurrentUser.UserId
                };

                if (appraisal.User != null)
                {
                    appraisallog.Description = Constanst.VehicleLogSentence.AppraisedByUser.Replace("USER",
                        appraisal.User.Name);
                }
                else
                {
                    appraisallog.Description = Constanst.VehicleLogSentence.AppraisedByUser.Replace("USER",
                    SessionHandler.CurrentUser.FullName);
                }
                var inventorylog = new VehicleLog()
                {
                    DateStamp = DateTime.Now,
                
                    InventoryId = vehicle.InventoryId,
                    UserId = SessionHandler.CurrentUser.UserId
                };

                if (inventoryStatusCodeId == Constanst.InventoryStatus.Inventory)
                    inventorylog.Description = Constanst.VehicleLogSentence.AddToInventoryByUser.Replace("USER",
                        SessionHandler.CurrentUser.FullName);

                if (inventoryStatusCodeId == Constanst.InventoryStatus.Wholesale)
                    inventorylog.Description = Constanst.VehicleLogSentence.AddToWholesaleByUser.Replace("USER",
                     SessionHandler.CurrentUser.FullName);

                if (inventoryStatusCodeId == Constanst.InventoryStatus.Recon)
                    inventorylog.Description = Constanst.VehicleLogSentence.AddToReconByUser.Replace("USER",
                     SessionHandler.CurrentUser.FullName);

                if (inventoryStatusCodeId == Constanst.InventoryStatus.Auction)
                    inventorylog.Description = Constanst.VehicleLogSentence.AddToAuctionByUser.Replace("USER",
                     SessionHandler.CurrentUser.FullName);

                if (inventoryStatusCodeId == Constanst.InventoryStatus.Loaner)
                    inventorylog.Description = Constanst.VehicleLogSentence.AddToLoanerByUser.Replace("USER",
                     SessionHandler.CurrentUser.FullName);

                if (inventoryStatusCodeId == Constanst.InventoryStatus.TradeNotClear)
                    inventorylog.Description = Constanst.VehicleLogSentence.AddToTradeNotClearByUser.Replace("USER",
                     SessionHandler.CurrentUser.FullName);

                if (inventoryStatusCodeId == Constanst.InventoryStatus.SoldOut)
                    inventorylog.Description = Constanst.VehicleLogSentence.SoldByUser.Replace("USER",
                     SessionHandler.CurrentUser.FullName);

                context.AddToVehicleLogs(appraisallog);

                context.AddToVehicleLogs(inventorylog);

                context.SaveChanges();

                var emailWaitingList = new EmailWaitingList()
                {
                    ListingId = vehicle.InventoryId,
                    DateStamp = DateTime.Now,
                    Expire = false,
                    NotificationTypeCodeId = Constanst.NotificationType.Inventory,
                    UserId = SessionHandler.CurrentUser.UserId

                };
        
                if (inventoryStatusCodeId == Constanst.InventoryStatus.Wholesale)
                {
                    emailWaitingList.NotificationTypeCodeId = Constanst.NotificationType.Wholesale;
                }

                context.AddToEmailWaitingLists(emailWaitingList);

                context.SaveChanges();

                return vehicle.InventoryId;

            }
        }

    
        public static int InsertToWholeSale(int appraisalId, DealershipViewModel dealer)
        {

            using (var context = new VincontrolEntities())
            {
                var appraisal = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

                var vehicle = MappingHanlder.ConvertToInventory(appraisal, dealer);

                vehicle.InventoryStatusCodeId = Constanst.InventoryStatus.Wholesale;

                context.AddToInventories(vehicle);

                context.SaveChanges();

                return vehicle.InventoryId;

            }

        }

        public static int InsertToWholeSale(int appraisalId, string stock, DealershipViewModel dealer)
        {

            using (var context = new VincontrolEntities())
            {
                var appraisal = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

                var vehicle = MappingHanlder.ConvertToInventory(appraisal, dealer);

                vehicle.Stock = stock;

                vehicle.InventoryStatusCodeId = Constanst.InventoryStatus.Wholesale;

                context.AddToInventories(vehicle);

                context.SaveChanges();

                return vehicle.InventoryId;

            }

        }

        public static Appraisal InsertAppraisalToDatabase(AppraisalViewFormModel appraisal, DealershipViewModel dealer)
        {
            using (var context = new VincontrolEntities())
            {
                var searchVehicle = context.Vehicles.FirstOrDefault(x => x.Vin == appraisal.VinNumber);

                if (searchVehicle == null)
                {
                    var newVehicle = MappingHanlder.ConvertToVehicle(appraisal);
                    context.AddToVehicles(newVehicle);
               
                    context.SaveChanges();

                    var newAppraisal = MappingHanlder.ConvertToAppraisal(appraisal);
                    newAppraisal.DealerId = dealer.DealershipId;
                    newAppraisal.VehicleId = newVehicle.VehicleId;
                    newAppraisal.AppraisalStatusCodeId = Constanst.AppraisalStatus.Pending;

                    context.AddToAppraisals(newAppraisal);
                    context.SaveChanges();

                    var emailWaitingList = new EmailWaitingList()
                    {
                        AppraisalId = newAppraisal.AppraisalId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.Appraisal,
                        UserId = SessionHandler.CurrentUser.UserId

                    };

                    context.AddToEmailWaitingLists(emailWaitingList);
                    context.SaveChanges();

                    return newAppraisal;

                }
                else
                {
                  

                    var newAppraisal = MappingHanlder.ConvertToAppraisal(appraisal);
                    newAppraisal.DealerId = dealer.DealershipId;
                    newAppraisal.VehicleId = searchVehicle.VehicleId;
                    newAppraisal.AppraisalStatusCodeId = Constanst.AppraisalStatus.Pending;

                    context.AddToAppraisals(newAppraisal);
                    context.SaveChanges();

                    var emailWaitingList = new EmailWaitingList()
                    {
                        AppraisalId = newAppraisal.AppraisalId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.Appraisal,
                        UserId = SessionHandler.CurrentUser.UserId

                    };


                    context.AddToEmailWaitingLists(emailWaitingList);
                    context.SaveChanges();

                    return newAppraisal;
                }

             

                
            }
        }

        private static string GetTrim(AppraisalViewFormModel appraisal)
        {
            return String.IsNullOrEmpty(appraisal.SelectedTrim) ? "" : appraisal.SelectedTrim;
        }

        public static int GetEbayCategoryId(string make, string model)
        {
            var defaultId = 6472;

            using (var context = new VincontrolEntities())
            {
                var searchMakeResult = context.EbayCategories.Where(x => x.Make == make);

                if (searchMakeResult.Any())
                {
                    var chromeModel = CommonHelper.RemoveSpecialCharacters(model).ToLowerInvariant();

                    foreach (var tmp in searchMakeResult)
                    {
                        var ebayModel = CommonHelper.RemoveSpecialCharacters(tmp.Model).ToLowerInvariant();

                        if (chromeModel.Equals(ebayModel.Trim()) || chromeModel.Contains(ebayModel.Trim()) ||
                            ebayModel.Contains(chromeModel.Trim()))
                        {
                            defaultId = tmp.EbayCategoryID;
                            break;
                        }
                    }

                    if (defaultId == 6472)
                    {
                        var otherCategorySameMake = searchMakeResult.FirstOrDefault(x => x.Model.Equals("Other"));
                        if (otherCategorySameMake != null)
                            defaultId = otherCategorySameMake.EbayCategoryID;

                    }
                }

            }
            return defaultId;


        }

        public static void InsertOrUpdateEbayAd(int dealerId, string listingId, PostEbayAds ebayAd)
        {
            using (var context = new VincontrolEntities())
            {
                int lid = Convert.ToInt32(listingId);

                if (context.Ebays.Any(o => o.InventoryId == lid))
                {
                    var ebay = context.Ebays.FirstOrDefault(x => x.InventoryId == lid);

                    ebay.EbayAdId = ebayAd.EbayAdID;

                    ebay.EbayAdURL = ebayAd.EbayAdUrl;

                    ebay.DateStamp = ebayAd.EbayAdStartTime;

                    ebay.Expiration = ebayAd.EbayAdEndTime;

                    ebay.DealerId = dealerId;

                    var log = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description =
                            Constanst.VehicleLogSentence.EbayCreatedByUser.Replace("USER",
                                SessionHandler.CurrentUser.FullName),
                        InventoryId = lid,
                        UserId = SessionHandler.CurrentUser.UserId
                    };

                    context.AddToVehicleLogs(log);
                    context.SaveChanges();

                }
                else
                {
                    var ebay = new Ebay()
                    {
                        InventoryId = lid,

                        EbayAdId = ebayAd.EbayAdID,

                        EbayAdURL = ebayAd.EbayAdUrl,

                        DateStamp = ebayAd.EbayAdStartTime,

                        Expiration = ebayAd.EbayAdEndTime,

                        DealerId = dealerId
                    };

                    context.AddToEbays(ebay);

                    var log = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description =
                            Constanst.VehicleLogSentence.EbayCreatedByUser.Replace("USER",
                                SessionHandler.CurrentUser.FullName),
                        InventoryId = lid,
                        UserId = SessionHandler.CurrentUser.UserId
                    };

                    context.AddToVehicleLogs(log);
                    context.SaveChanges();
                }

            }



        }

        public static void TransferVehicle(int ListingId, int transferDealerShipId, string newStockNumber,
                                           DealerGroupViewModel dealerGroup)
        {

            using (var context = new VincontrolEntities())
            {
                var searchResult =
                    context.Inventories.FirstOrDefault(x => x.InventoryId == ListingId);

                if (searchResult != null)
                {
                    var oldDealerName = searchResult.Dealer.Name;
                    searchResult.DealerId = transferDealerShipId;

                    searchResult.Stock = newStockNumber;

                    searchResult.LastUpdated = DateTime.Now;

                
                    context.SaveChanges();

                    var log = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description =
                            Constanst.VehicleLogSentence.VehicleTransferByUser
                            .Replace("DEALER1", oldDealerName)
                            .Replace("DEALER2", searchResult.Dealer.Name)
                            .Replace("USER", SessionHandler.CurrentUser.FullName),
                        InventoryId = ListingId,
                        UserId = SessionHandler.CurrentUser.UserId
                    };

                    context.AddToVehicleLogs(log);

                    context.SaveChanges();
                    

                }

               
            }

        }

        public static bool CheckStockNumberExist(string stockNumber, int dealershipId)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.Inventories.Any(o => o.Stock == stockNumber && o.DealerId == dealershipId))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<AppraisalViewFormModel> GetListOfAppraisal(int dealerId)
        {
            var fullList = new List<AppraisalViewFormModel>();
            var context = new VincontrolEntities();
            DateTime dtPrevious60Days = DateTime.Now.AddDays(-60);

            var result = context.Appraisals.Where(x => x.DealerId == dealerId && (x.AppraisalStatusCodeId == null || x.AppraisalStatusCodeId == Constanst.AppraisalType.WebAppraisal) && x.AppraisalDate >= dtPrevious60Days).OrderByDescending(x => x.AppraisalDate.Value);

            foreach (var app in result)
            {
                var appraisalTmp = new AppraisalViewFormModel
                                       {
                                           AppraisalID = app.AppraisalId,
                                           Make = app.Vehicle.Make,
                                           ModelYear = app.Vehicle.Year.GetValueOrDefault(),
                                           AppraisalModel = app.Vehicle.Model,
                                           VinNumber = app.Vehicle.Vin,
                                           ACV = app.ACV,
                                           CarImagesUrl = app.PhotoUrl,
                                           DefaultImageUrl = app.Vehicle.DefaultStockImage,
                                           AppraisalDate = app.AppraisalDate.GetValueOrDefault().ToShortDateString(),
                                           AppraisalGenerateId = app.AppraisalId.ToString()
                                       };

                fullList.Add(appraisalTmp);
            }

            return fullList;

        }

        public static List<string> GetListOfTruckCategoryByTruckType(string truckType)
        {

            var context = new VincontrolEntities();

            var returnList = new List<string>();

            if (context.TruckCategories.Any(x => x.TruckType == truckType))
            {
                var result = context.TruckCategories.Where(x => x.TruckType == truckType);

                returnList.Add("Pickup Truck");

                foreach (var app in result)
                {
                    if (!app.CategoryName.Equals("Pickup Truck"))
                        returnList.Add(app.CategoryName);
                }
            }


            return returnList;

        }

        public static int AddTradeInCustomerVehicle(TradeInVehicleModel vehicle)
        {
            using (var context = new VincontrolEntities())
            {
                var customerTradeIn = new TradeInCustomer()
                {
                    Condition = vehicle.Condition,
                    DateStamp = DateTime.Now,
                    DealerId = vehicle.DealerId,
                    Email = vehicle.CustomerEmail,
                    FirstName = vehicle.CustomerFirstName,
                    LastName = vehicle.CustomerLastName,
                    Phone = CommonHelper.RemoveSpecialCharactersForPurePrice(vehicle.CustomerPhone),
                    //KBBVehicelId = vehicle.VehicleId,
                    Year = Convert.ToInt32(vehicle.SelectedYear),
                    Make = vehicle.SelectedMakeValue,
                    Model = vehicle.SelectedModelValue,
                    Trim = vehicle.SelectedTrimValue,
                    Mileage = vehicle.MileageNumber,
                    TradeInFairValue = vehicle.TradeInFairPrice,
                    TradeInMaxValue = vehicle.TradeInGoodPrice,
                    Vin = vehicle.Vin,
                    SelectedOptions = vehicle.SelectedOptionList
                    //EmailContent = vehicle.EmailTextContent,
                    //ADFEmailContent = vehicle.EmailADFContent,
                    //Receivers = GetReceivers(vehicle.Receivers)
                };

                context.AddToTradeInCustomers(customerTradeIn);
                context.SaveChanges();

                return customerTradeIn.TradeInCustomerId;
            }
        }

       
     

        public static CarImage GetImageUrLs(int listingId, int vehicleStatus)
        {
            var context = new VincontrolEntities();
            if (vehicleStatus == Constanst.VehicleStatus.Inventory)
            {
                var row =
                    context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
                if (row != null)
                {
                    return new CarImage()
                    {
                        ImageURLs = row.PhotoUrl,
                        ThumnailURLs = row.ThumbnailUrl
                    };
                }
               
            }
            if (vehicleStatus == Constanst.VehicleStatus.Appraisal)
            {
                var row =
                    context.Appraisals.FirstOrDefault(x => x.AppraisalId == listingId);
                if (row != null)
                {
                    return new CarImage()
                    {
                        ImageURLs = row.PhotoUrl,
                        ThumnailURLs = row.ThumbnailUrl
                    };
                }
            }
            if (vehicleStatus == Constanst.VehicleStatus.SoldOut)
            {
                var row =
                    context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);
                if (row != null)
                {
                    return new CarImage()
                    {
                        ImageURLs = row.PhotoUrl,
                        ThumnailURLs = row.ThumbnailUrl
                    };
                }
            }
            return null;
           
        }

        public static void ReplaceCarImageUrl(ImageViewModel image)
        {
            if (image.InventoryStatus == Constanst.VehicleStatus.Inventory || image.InventoryStatus == Constanst.VehicleStatus.SoldOut)
            {
                UpdateCarImageUrl(image.ListingId, image.ImageUploadFiles, image.ThumbnailImageUploadFiles, image.UserUpload, image.InventoryStatus);
            }
            if (image.InventoryStatus == Constanst.VehicleStatus.Appraisal)
            {
                UpdateAppraisalCarImageUrl(image.AppraisalId, image.ImageUploadFiles, image.ThumbnailImageUploadFiles);

            }
     
        }


        public static IList<ButtonPermissionViewModel> GetButtonList(int dealerId, string screen)
        {
            using (var context = new VincontrolEntities())
            {
                var result = new List<ButtonPermissionViewModel>();

                var groups = context.Roles.Where(i => i.RoleId != Constanst.RoleType.Admin).ToList();

                var buttons = context.Buttons.Include("DealerButtons").Where(i => i.Screen.ToLower().Equals(screen.ToLower())).ToList();

                if (buttons.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        var buttonModels = buttons.Select(i => new Button()
                                                                   {
                                                                       ButtonId = i.ButtonId,
                                                                       ButtonName = i.Button1,
                                                                       CanSee =
                                                                           i.DealerButtons != null &&
                                                                           i.DealerButtons.Any(
                                                                               ii => ii.DealerId == dealerId && ii.ButtonId == i.ButtonId && ii.GroupId == group.RoleId)
                                                                               ? i.DealerButtons.First(ii => ii.DealerId == dealerId && ii.ButtonId == i.ButtonId && ii.GroupId == group.RoleId).CanSee
                                                                               : false
                                                                   }).ToList();
                        var buttonPermission = new ButtonPermissionViewModel()
                                                   {
                                                       Buttons = buttonModels,
                                                       DealershipId = dealerId,
                                                       GroupId = group.RoleId,
                                                       GroupName = group.RoleName
                                                   };
                        result.Add(buttonPermission);
                    }
                }

                return result;
            }
        }

        public static ButtonPermissionViewModel GetButtonList(UserRoleViewModel user, string screen)
        {
            using (var context = new VincontrolEntities())
            {
                var result = new ButtonPermissionViewModel();

                var buttons = context.Buttons.Where(i => i.Screen.ToLower().Equals(screen.ToLower())).ToList();

                if (buttons.Count > 0)
                {

                    var buttonModels = buttons.Select(i => new Button()
                    {
                        ButtonId = i.ButtonId,
                        ButtonName = i.Button1,
                        CanSee = i.DealerButtons != null &&
                            i.DealerButtons.Any(ii => ii.DealerId == user.DealershipId && ii.ButtonId == i.ButtonId && ii.GroupId == user.RoleId)
                                ? i.DealerButtons.First(
                                    ii =>
                                    ii.DealerId == user.DealershipId && ii.ButtonId == i.ButtonId && ii.GroupId == user.RoleId).CanSee
                                : false
                    }).ToList();


                    result = new ButtonPermissionViewModel()
                    {
                        Buttons = buttonModels,
                        DealershipId = user.DealershipId,
                        GroupId = user.RoleId,
                        GroupName = user.Role
                    };

                }


                return result;
            }
        }

        public static bool CheckDealershipButtonGroupExist(int dealerId, int roleId)
        {
            if (roleId.Equals(Constanst.RoleType.Admin) || roleId.Equals(Constanst.RoleType.Master)) return false;
            using (var context = new VincontrolEntities())
            {
                //var group = context.Roles.FirstOrDefault(i => i.RoleId.Equals(roleId));
                return context.DealerButtons.Any(i => i.DealerId == dealerId && i.GroupId == roleId) ? true : false;
            }
        }


        public static void SetDisclaimerContent(string content, int rebateId)
        {
            using (var context = new VincontrolEntities())
            {
                var searchResult = context.Rebates.First(x => x.RebateId == rebateId);

                var result =
                      context.Inventories.Where(
                          x => x.DealerId == searchResult.DealerId && x.Condition == Constanst.ConditionStatus.New && x.Vehicle.Year == searchResult.Year && x.Vehicle.Make == searchResult.Make && x.Vehicle.Model == searchResult.Model && x.Vehicle.Trim == searchResult.Trim);

                foreach (var tmp in result)
                {
                    tmp.Disclaimer = content;
                }

                searchResult.Disclaimer = content;
                context.SaveChanges();
            }
        }

   

      
        public static List<string> GetListBrandName()
        {
            using (var context = new VincontrolEntities())
            {
                return context.Makes.Select(x => x.Value).Distinct().ToList();
            }
        }

        public static List<Make> GetListBrand()
        {
            using (var context = new VincontrolEntities())
            {
                return context.Makes.Distinct().ToList();
            }
        }

        public static List<Make> GetListBrandByBrandNames(string brandNames)
        {
            using (var context = new VincontrolEntities())
            {
                List<string> brands = new List<string>();
                brands = brandNames.Split(',').Select(x => x.Trim().ToLower()).ToList();
                var listBrands = context.Makes.Distinct();
                var result = listBrands.Where(x => brands.Contains(x.Value.ToLower())).ToList();
                return result;
            }
        }

        public static int GetNumberOfWSTemplate(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                return context.DealerWSTemplates.Where(x => x.DealerId == dealerId).Count();
            }
        }
    }

    public class CarImage
    {
        public string ThumnailURLs { get; set; }
        public string ImageURLs { get; set; }
    }

  
}
