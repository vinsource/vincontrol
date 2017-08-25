using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.Handlers;
using System.Linq;
using WhitmanEnterpriseMVC.DatabaseModel;
using Newtonsoft.Json.Linq;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public sealed class SQLHelper
    {
        public SQLHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private const string KingRole = "King";
        private const string AdminRole = "Admin";
        private const string ManagerRole = "Manager";
        private const string EmployeeRole = "Employee";

        /*
         * 0: No Exist In Database
         * 1: Exist In Database. Data is useable
         * 2. Exist In Database. Data is obsolete
         * 
         * */

        public static int CheckVinHasKbbReport(string vin)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                if (context.whitmanenterprisekbbs.Any(o => o.Vin.Equals(vin)))
                {
                    var firstTmp = context.whitmanenterprisekbbs.First(o => o.Vin == vin);

                    DateTime dt = DateTime.Parse(firstTmp.ExpiredDate.ToString());

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
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                foreach (KellyBlueBookTrimReport tmp in kbb.TrimReportList)
                {
                    var e = new whitmanenterprisekbb()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInPrice.TradeInFairPrice),
                        TradeInGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInPrice.TradeInGoodPrice),
                        TradeInVeryGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInPrice.TradeInVeryGoodPrice),
                        BaseWholeSale = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.BaseWholesale),
                        MileageAdjustment = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.MileageAdjustment.ToString()),
                        WholeSale = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.WholeSale),
                        TrimId = tmp.TrimId,
                        DateAdded = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        ExpiredDate = CommonHelper.GetNextFriday(),
                        Vin = kbb.Vin
                    };


                    //Add to memory

                    context.AddTowhitmanenterprisekbbs(e);

                }

                if (kbb.TrimReportList.Count == 1)
                {
                    var searchVehicleResult = context.whitmanenterprisedealershipinventories.Where(x => x.VINNumber == kbb.Vin);

                    foreach (var tmp in searchVehicleResult)
                    {
                        tmp.KBBTrimId = kbb.TrimReportList.First().TrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        public static void AddSimpleKbbReportFromKarPower(List<SmallKarPowerViewModel> karpowerResult, string vin, string type)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                foreach (var tmp in karpowerResult)
                {
                    var existingKbb = context.whitmanenterprisekbbs.OrderByDescending(i => i.DateAdded).FirstOrDefault(i => i.Vin == vin && i.TrimId == tmp.SelectedTrimId && i.Type.Equals(type));
                    if (existingKbb == null)
                    {
                        var e = new whitmanenterprisekbb()
                        {
                            Trim = tmp.SelectedTrimName,
                            BaseWholeSale = tmp.BaseWholesale,
                            MileageAdjustment = tmp.MileageAdjustment,
                            WholeSale = tmp.Wholesale,
                            TrimId = tmp.SelectedTrimId,
                            DateAdded = DateTime.Now,
                            LastUpdated = DateTime.Now,
                            ExpiredDate = CommonHelper.GetNextFriday(),
                            Vin = vin,
                            Type = type
                        };

                        //Add to memory

                        context.AddTowhitmanenterprisekbbs(e);
                    }
                }

                if (karpowerResult.Count == 1)
                {
                    var searchVehicleResult = context.whitmanenterprisedealershipinventories.Where(x => x.VINNumber == vin);

                    foreach (var tmp in searchVehicleResult)
                    {
                        tmp.KBBTrimId = karpowerResult.First().SelectedTrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        public static void UpdateSimpleKbbReport(KellyBlueBookViewModel kbb)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterprisekbbs.Where(x => x.Vin == kbb.Vin).ToList();
                foreach (var tmp in searchResult)
                {
                    context.Attach(tmp);
                    context.DeleteObject(tmp);
                }

                context.SaveChanges();

                foreach (var tmp in kbb.TrimReportList)
                {
                    var e = new whitmanenterprisekbb()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInPrice.TradeInFairPrice),
                        TradeInGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInPrice.TradeInGoodPrice),
                        TradeInVeryGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInPrice.TradeInVeryGoodPrice),
                        BaseWholeSale = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.BaseWholesale),
                        MileageAdjustment = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.MileageAdjustment.ToString()),
                        WholeSale = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.WholeSale),
                        TrimId = tmp.TrimId,
                        DateAdded = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        ExpiredDate = CommonHelper.GetNextFriday(),
                        Vin = kbb.Vin
                    };


                    //Add to memory

                    context.AddTowhitmanenterprisekbbs(e);

                }

                if (kbb.TrimReportList.Count == 1)
                {
                    var searchVehicleResult = context.whitmanenterprisedealershipinventories.Where(x => x.VINNumber == kbb.Vin);

                    foreach (var tmp in searchVehicleResult)
                    {
                        tmp.KBBTrimId = kbb.TrimReportList.First().TrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        public static int CheckVinHasBbReport(string Vin)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisebbs.Any(o => o.Vin.Equals(Vin)))
                {
                    var firstTmp = context.whitmanenterprisebbs.FirstOrDefault(o => o.Vin.Equals(Vin));

                    DateTime dt = DateTime.Parse(firstTmp.ExpiredDate.ToString());

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
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                foreach (BlackBookTrimReport tmp in bb.TrimReportList)
                {
                    whitmanenterprisebb e = new whitmanenterprisebb()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInRough),
                        TradeInGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInAvg),
                        TradeInVeryGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInClean),
                        DateAdded = DateTime.Now,
                        ExpiredDate = CommonHelper.GetNextFriday(),
                        Vin = bb.Vin
                    };


                    //Add to memory

                    context.AddTowhitmanenterprisebbs(e);

                }

                context.SaveChanges();
            }
        }

        public static void UpdateSimpleBBReport(BlackBookViewModel bb)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterprisebbs.Where(x => x.Vin == bb.Vin).ToList();
                foreach (var tmp in searchResult)
                {
                    context.Attach(tmp);
                    context.DeleteObject(tmp);
                }

                context.SaveChanges();

                foreach (BlackBookTrimReport tmp in bb.TrimReportList)
                {
                    whitmanenterprisebb e = new whitmanenterprisebb()
                    {
                        Trim = tmp.TrimName,
                        TradeInFairPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInRough),
                        TradeInGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInAvg),
                        TradeInVeryGoodPrice = CommonHelper.RemoveSpecialCharactersForMsrp(tmp.TradeInClean),
                        DateAdded = DateTime.Now,
                        ExpiredDate = CommonHelper.GetNextFriday(),
                        Vin = bb.Vin
                    };


                    //Add to memory

                    context.AddTowhitmanenterprisebbs(e);

                }

                context.SaveChanges();
            }
        }

        public static int CheckVinHasCarFaxReport(string Vin)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenteprisecarfaxes.Any(o => o.Vin.Equals(Vin)))
                {
                    var firstTmp = context.whitmanenteprisecarfaxes.FirstOrDefault(o => o.Vin.Equals(Vin));

                    DateTime dt = DateTime.Parse(firstTmp.ExpiredDate.ToString());

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
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int number = 0;

                bool flag =
              Int32.TryParse(
                 carfax.NumberofOwners, out number);

                var e = new whitmanenteprisecarfax()
                {
                    Vin = carfax.Vin,
                    DateAdded = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    ExpiredDate = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3),
                    Owner = number,
                    ServiceRecord = carfax.ServiceRecords,
                    PriorRental = carfax.PriorRental,
                    Accident = carfax.AccidentCounts



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

                //Add to memory

                context.AddTowhitmanenteprisecarfaxes(e);


                context.SaveChanges();
            }
        }

        public static void UpdateCarFaxReport(CarFaxViewModel carfax)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
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

                var findCarFax = context.whitmanenteprisecarfaxes.First(x => x.Vin.Equals(carfax.Vin));

                findCarFax.Owner = number;

                findCarFax.WindowSticker = builder.ToString();

                findCarFax.PriorRental = carfax.PriorRental;

                findCarFax.Accident = carfax.AccidentCounts;

                findCarFax.LastUpdated = DateTime.Now;

                findCarFax.ExpiredDate = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3);

                context.SaveChanges();
            }
        }

        public static void MasterLogin(UserRoleViewModel specificuser, ref DealerGroupViewModel dealerGroup, ref DealershipViewModel defaultDealer, ref UserRoleViewModel user)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var dealerList = context.whitmanenterprisedealerships.Where(x => x.DealerGroupID == specificuser.DealerGroupId);

            var dealerDefault = dealerList.FirstOrDefault(x => x.idWhitmanenterpriseDealership == specificuser.DefaultLogin);

            var dealerDefaultSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == specificuser.DefaultLogin);

            user = new UserRoleViewModel()
                       {
                           UserName = specificuser.UserName,

                           PassWord = specificuser.PassWord,

                           DealershipId = specificuser.DefaultLogin,

                           DealerGroupId = specificuser.DealerGroupId,

                           DefaultLogin = specificuser.DefaultLogin,

                           Role = /*KingRole*/ specificuser.Role,

                           ProfileButtonPermissions = GetButtonList(specificuser.DefaultLogin, "Profile").FirstOrDefault(i => i.GroupName.ToLower().Equals(specificuser.Role.ToLower()))
                       };

            dealerGroup = new DealerGroupViewModel()
                              {
                                  DealershipGroupId = specificuser.DealerGroupId,

                                  DealershipGroupName = specificuser.Name,

                                  DealershipGroupDefaultLogin = specificuser.DefaultLogin
                              };

            defaultDealer = new DealershipViewModel()
                                {
                                    DealershipId = specificuser.DefaultLogin,
                                    DealershipName = dealerDefault.DealershipName,
                                    DealershipAddress = dealerDefault.DealershipAddress,
                                    Address = dealerDefault.Address,
                                    City = dealerDefault.City,
                                    State = dealerDefault.State,
                                    ZipCode = dealerDefault.ZipCode,
                                    Phone = dealerDefault.Phone,
                                    Email = dealerDefault.Email,
                                    DealerGroupId = dealerDefault.DealerGroupID,
                                    OverrideDealerKbbReport = dealerDefault.OverideDealerKBBReport.GetValueOrDefault(),
                                    Latitude = dealerDefault.Lattitude,
                                    Longtitude = dealerDefault.Longtitude,
                                    InventorySorting = dealerDefaultSetting.InventorySorting,
                                    SoldOut = dealerDefaultSetting.SoldOut,
                                    DefaultStockImageUrl = dealerDefaultSetting.DefaultStockImageUrl,
                                    OverrideStockImage = dealerDefaultSetting.OverideStockImage.GetValueOrDefault(),
                                    DealerInfo = dealerDefaultSetting.DealerInfo,
                                    DealerWarranty = dealerDefaultSetting.DealerWarranty,
                                    TermConditon = dealerDefaultSetting.TermsAndCondition,
                                    EbayToken = dealerDefaultSetting.EbayToken,
                                    EbayInventoryUrl = dealerDefaultSetting.EbayInventoryURL,
                                    CreditUrl = dealerDefaultSetting.CreditURL,
                                    WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                                    ContactUsUrl = dealerDefaultSetting.ContactUsURL,
                                    FacebookUrl = dealerDefaultSetting.FacebookURL,
                                    LogoUrl = dealerDefaultSetting.LogoURL,
                                    ContactPerson = dealerDefaultSetting.ContactPerson,
                                    CarFax = dealerDefaultSetting.CarFax,
                                    CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                                    Manheim = dealerDefaultSetting.Manheim,
                                    ManheimPassword = dealerDefaultSetting.ManheimPassword,
                                    KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                                    KellyPassword = dealerDefaultSetting.KellyPassword,
                                    BlackBook = dealerDefaultSetting.BlackBook,
                                    BlackBookPassword = dealerDefaultSetting.BlackBookPassword,
                                    EnableAutoDescription = dealerDefaultSetting.AutoDescription.GetValueOrDefault(),
                                    FirstIntervalJump = dealerDefaultSetting.FirstTimeRangeBucketJump.GetValueOrDefault(),
                                    SecondIntervalJump = dealerDefaultSetting.SecondTimeRangeBucketJump.GetValueOrDefault(),
                                    IntervalBucketJump = dealerDefaultSetting.IntervalBucketJump.GetValueOrDefault(),
                                    LoanerSentence = dealerDefaultSetting.LoanerSentence,
                                    AuctionSentence = dealerDefaultSetting.AuctionSentence,
                                };

            dealerGroup.DealerList = new List<DealershipViewModel>();

            foreach (var row in dealerList)
            {
                var tmp = new DealershipViewModel
                              {
                                  DealershipId = row.idWhitmanenterpriseDealership,
                                  DealershipName = row.DealershipName,
                                  DealershipAddress = row.DealershipAddress,
                                  Address = row.Address,
                                  City = row.City,
                                  State = row.State,
                                  ZipCode = row.ZipCode,
                                  Phone = row.Phone,
                                  Latitude = row.Lattitude,
                                  Longtitude = row.Longtitude,
                                  Email = row.Email
                              };

                dealerGroup.DealerList.Add(tmp);
            }
        }

        public static void LoginSingleStore(UserRoleViewModel specificuser, ref DealershipViewModel dealer, ref UserRoleViewModel user)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var dealerDefault = context.whitmanenterprisedealerships.FirstOrDefault(x => x.idWhitmanenterpriseDealership == specificuser.DealershipId);

            var dealerDefaultSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == specificuser.DealershipId);

            user = new UserRoleViewModel()
            {
                DealershipId = specificuser.DealershipId,

                DefaultLogin = specificuser.DefaultLogin,

                Name = specificuser.Name,

                Role = /*KingRole*/ specificuser.Role,

                ProfileButtonPermissions = GetButtonList(specificuser.DefaultLogin, "Profile").FirstOrDefault(i => i.GroupName.ToLower().Equals(specificuser.Role.ToLower()))
            };

            dealer = new DealershipViewModel()
            {
                DealershipId = specificuser.DefaultLogin,
                DealershipName = dealerDefault.DealershipName,
                DealershipAddress = dealerDefault.DealershipAddress,
                Address = dealerDefault.Address,
                City = dealerDefault.City,
                State = dealerDefault.State,
                ZipCode = dealerDefault.ZipCode,
                Phone = dealerDefault.Phone,
                Email = dealerDefault.Email,
                Latitude = dealerDefault.Lattitude,
                Longtitude = dealerDefault.Longtitude,
                OverrideDealerKbbReport = dealerDefault.OverideDealerKBBReport.GetValueOrDefault(),
                DealerGroupId = dealerDefault.DealerGroupID,
                InventorySorting = dealerDefaultSetting.InventorySorting,
                SoldOut = dealerDefaultSetting.SoldOut,
                DefaultStockImageUrl = dealerDefaultSetting.DefaultStockImageUrl,
                OverrideStockImage = dealerDefaultSetting.OverideStockImage.GetValueOrDefault(),
                DealerInfo = dealerDefaultSetting.DealerInfo,
                DealerWarranty = dealerDefaultSetting.DealerWarranty,
                TermConditon = dealerDefaultSetting.TermsAndCondition,
                EbayToken = dealerDefaultSetting.EbayToken,
                EbayInventoryUrl = dealerDefaultSetting.EbayInventoryURL,
                CreditUrl = dealerDefaultSetting.CreditURL,
                WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                ContactUsUrl = dealerDefaultSetting.ContactUsURL,
                FacebookUrl = dealerDefaultSetting.FacebookURL,
                LogoUrl = dealerDefaultSetting.LogoURL,
                ContactPerson = dealerDefaultSetting.ContactPerson,
                CarFax = dealerDefaultSetting.CarFax,
                CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                Manheim = dealerDefaultSetting.Manheim,
                ManheimPassword = dealerDefaultSetting.ManheimPassword,
                KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                KellyPassword = dealerDefaultSetting.KellyPassword,
                BlackBook = dealerDefaultSetting.BlackBook,
                BlackBookPassword = dealerDefaultSetting.BlackBookPassword,
                EnableAutoDescription = dealerDefaultSetting.AutoDescription.GetValueOrDefault(),
                FirstIntervalJump = dealerDefaultSetting.FirstTimeRangeBucketJump.GetValueOrDefault(),
                SecondIntervalJump = dealerDefaultSetting.SecondTimeRangeBucketJump.GetValueOrDefault(),
                IntervalBucketJump = dealerDefaultSetting.IntervalBucketJump.GetValueOrDefault(),
                LoanerSentence = dealerDefaultSetting.LoanerSentence,
                AuctionSentence = dealerDefaultSetting.AuctionSentence,
            };
        }

        public static void LoginMultipleStore(UserRoleViewModel specificuser, ref DealerGroupViewModel dealerGroup, ref DealershipViewModel dealer, ref UserRoleViewModel user)
        {
            var context = new whitmanenterprisewarehouseEntities();

            //get dealer group
            var specifidealerGroup = context.whitmanenterprisedealergroups.FirstOrDefault(x => x.DealerGroupId == specificuser.DealerGroupId);

            var specificedealerList = from x in context.whitmanenterpriseusers
                                      where x.UserName == specificuser.UserName && x.Active.Value
                                      select x.DealershipID.Value;

            //specificuser.DefaultLogin==Constanst.DealerGroupConst means user can view all store
            var dealerList = context.whitmanenterprisedealerships.Where(LogicHelper.BuildContainsExpression<whitmanenterprisedealership, int>(e => e.idWhitmanenterpriseDealership, specificedealerList));

            if (specificuser.DefaultLogin == Constanst.DealerGroupConst)
                specificuser.DefaultLogin = dealerList.First().idWhitmanenterpriseDealership;

            var dealerDefault = dealerList.FirstOrDefault(x => x.idWhitmanenterpriseDealership == specificuser.DefaultLogin);

            var dealerDefaultSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == specificuser.DefaultLogin);

            user = new UserRoleViewModel()
                       {
                           UserName = specifidealerGroup.MasterUserName,

                           PassWord = specifidealerGroup.MasterLogin,

                           DealershipId = specificuser.DealershipId,

                           DealerGroupId = specifidealerGroup.DealerGroupId,

                           DefaultLogin = specificuser.DefaultLogin,

                           Role = /*KingRole*/ specificuser.Role,

                           ProfileButtonPermissions = GetButtonList(specificuser.DefaultLogin, "Profile").FirstOrDefault(i => i.GroupName.ToLower().Equals(specificuser.Role.ToLower()))
                       };

            dealerGroup = new DealerGroupViewModel()
                              {
                                  DealershipGroupId = specifidealerGroup.DealerGroupId,

                                  DealershipGroupName = specifidealerGroup.DealerGroupName,

                                  DealershipGroupDefaultLogin = specifidealerGroup.DefaultDealerID.GetValueOrDefault(),
                              };

            dealer = new DealershipViewModel()
                         {
                             DealershipId = specificuser.DefaultLogin,
                             DealershipName = dealerDefault.DealershipName,
                             DealershipAddress = dealerDefault.DealershipAddress,
                             Address = dealerDefault.Address,
                             City = dealerDefault.City,
                             State = dealerDefault.State,
                             ZipCode = dealerDefault.ZipCode,
                             Phone = dealerDefault.Phone,
                             Email = dealerDefault.Email,
                             Latitude = dealerDefault.Lattitude,
                             Longtitude = dealerDefault.Longtitude,
                             DealerGroupId = dealerDefault.DealerGroupID,
                             OverrideDealerKbbReport = dealerDefault.OverideDealerKBBReport.GetValueOrDefault(),
                             InventorySorting = dealerDefaultSetting.InventorySorting,
                             SoldOut = dealerDefaultSetting.SoldOut,
                             DefaultStockImageUrl = dealerDefaultSetting.DefaultStockImageUrl,
                             OverrideStockImage = dealerDefaultSetting.OverideStockImage.GetValueOrDefault(),
                             DealerInfo = dealerDefaultSetting.DealerInfo,
                             DealerWarranty = dealerDefaultSetting.DealerWarranty,
                             TermConditon = dealerDefaultSetting.TermsAndCondition,
                             EbayToken = dealerDefaultSetting.EbayToken,
                             EbayInventoryUrl = dealerDefaultSetting.EbayInventoryURL,
                             CreditUrl = dealerDefaultSetting.CreditURL,
                             WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                             ContactUsUrl = dealerDefaultSetting.ContactUsURL,
                             FacebookUrl = dealerDefaultSetting.FacebookURL,
                             LogoUrl = dealerDefaultSetting.LogoURL,
                             ContactPerson = dealerDefaultSetting.ContactPerson,
                             CarFax = dealerDefaultSetting.CarFax,
                             CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                             Manheim = dealerDefaultSetting.Manheim,
                             ManheimPassword = dealerDefaultSetting.ManheimPassword,
                             KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                             KellyPassword = dealerDefaultSetting.KellyPassword,
                             BlackBook = dealerDefaultSetting.BlackBook,
                             BlackBookPassword = dealerDefaultSetting.BlackBookPassword,
                             EnableAutoDescription = dealerDefaultSetting.AutoDescription.GetValueOrDefault(),
                             FirstIntervalJump = dealerDefaultSetting.FirstTimeRangeBucketJump.GetValueOrDefault(),
                             SecondIntervalJump = dealerDefaultSetting.SecondTimeRangeBucketJump.GetValueOrDefault(),
                             IntervalBucketJump = dealerDefaultSetting.IntervalBucketJump.GetValueOrDefault(),
                             LoanerSentence = dealerDefaultSetting.LoanerSentence,
                             AuctionSentence = dealerDefaultSetting.AuctionSentence,
                         };

            dealerGroup.DealerList = new List<DealershipViewModel>();

            foreach (var row in dealerList)
            {
                var tmp = new DealershipViewModel
                {
                    DealershipId = row.idWhitmanenterpriseDealership,
                    DealershipName = row.DealershipName,
                    DealershipAddress = row.DealershipAddress,
                    Address = row.Address,
                    City = row.City,
                    State = row.State,
                    ZipCode = row.ZipCode,
                    Phone = row.Phone,
                    Latitude = row.Lattitude,
                    Longtitude = row.Longtitude

                };

                dealerGroup.DealerList.Add(tmp);
            }

        }

        public static string GenerateAppraisalIdByDealerId(int dealerID)
        {
            var context = new whitmanenterprisewarehouseEntities();

            string nextAppraisalId = "";

            try
            {

                if (context.whitmanenterpriseappraisals.Any(x => x.DealershipId == dealerID))
                {
                    var result = context.whitmanenterpriseappraisals.Where(x => x.DealershipId == dealerID);
                    string latestAppraisalId = result.OrderByDescending(a => a.AppraisalDate).First().AppraisalID;

                    if (!String.IsNullOrEmpty(latestAppraisalId))
                    {
                        string tmp = latestAppraisalId.Substring(latestAppraisalId.Length - 4);

                        int number = Convert.ToInt32(tmp);

                        if (number < 1999)
                        {
                            number = number + 1;

                            nextAppraisalId = latestAppraisalId.Substring(0, latestAppraisalId.Length - 4) + number;
                        }
                        else
                        {
                            char firstletter = latestAppraisalId.ToCharArray().ElementAt(0);
                            char secondletter = latestAppraisalId.ToCharArray().ElementAt(1);

                            char nextSecondChar;

                            if (secondletter == 'Z')
                            {
                                nextSecondChar = 'A';
                                firstletter = (char)(((Int64)firstletter) + 1);
                            }
                            else
                                nextSecondChar = (char)(((Int64)secondletter) + 1);

                            nextAppraisalId = firstletter + secondletter + "1000";

                        }

                    }
                    else
                        nextAppraisalId = "AA1000";
                }
                else
                    nextAppraisalId = "AA1000";


            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("Exception is " + ex.Message + "<br/>" + "<br>");
            }


            return nextAppraisalId;
        }

        public static bool CheckVinExist(string vin, DealershipViewModel dealer)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisedealershipinventories.Any(o => o.DealershipId == dealer.DealershipId && o.VINNumber == vin))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CheckSimilarVinExist(string vin, DealershipViewModel dealer)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisedealershipinventories.Any(o => o.DealershipId == dealer.DealershipId && o.VINNumber.ToLower().Equals(vin.ToLower())))
                {
                    int numberofResult =
                        context.whitmanenterprisedealershipinventories.Count(
                            o => o.DealershipId == dealer.DealershipId && o.VINNumber.ToLower().Equals(vin.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static int CheckStockExist(string stock, DealershipViewModel dealer)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisedealershipinventories.Any(o => o.DealershipId == dealer.DealershipId && o.StockNumber.ToLower().Contains(stock.ToLower())))
                {
                    int numberofResult =context.whitmanenterprisedealershipinventories.Count(o => o.DealershipId == dealer.DealershipId && o.StockNumber.ToLower().Contains(stock.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static int CheckStockExistInGroup(string stock, DealerGroupViewModel dealerGroup)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var dealerList = from e in context.whitmanenterprisedealerships
                                 where e.DealerGroupID == dealerGroup.DealershipGroupId
                                 select e.idWhitmanenterpriseDealership;

                var avaiInventory = context.whitmanenterprisedealershipinventories.Where(LogicHelper.BuildContainsExpression<whitmanenterprisedealershipinventory, int>(e => e.DealershipId.Value, dealerList));
                if (avaiInventory.Any(o => o.StockNumber.ToLower().Contains(stock.ToLower())))
                {
                    int numberofResult =
                       avaiInventory.Count(
                            o => o.StockNumber.ToLower().Contains(stock.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static int CheckVinExistInGroup(string vin, DealerGroupViewModel dealerGroup)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var dealerList = from e in context.whitmanenterprisedealerships
                                 where e.DealerGroupID == dealerGroup.DealershipGroupId
                                 select e.idWhitmanenterpriseDealership;

                var avaiInventory = context.whitmanenterprisedealershipinventories.Where(LogicHelper.BuildContainsExpression<whitmanenterprisedealershipinventory, int>(e => e.DealershipId.Value, dealerList));
                if (avaiInventory.Any(o => o.VINNumber.ToLower().Equals(vin.ToLower())))
                {
                    int numberofResult = avaiInventory.Count(o => o.VINNumber.ToLower().Equals(vin.ToLower()));
                    return numberofResult;
                }
            }
            return 0;
        }

        public static bool CheckVinExistInAppraisal(string vin, DealershipViewModel dealer)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterpriseappraisals.Any(o => o.VINNumber == vin && o.DealershipId == dealer.DealershipId))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CheckVinExistInGroupForAppraisal(string vin, DealerGroupViewModel dealerGroup)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var dealerList = from e in context.whitmanenterprisedealerships
                                 where e.DealerGroupID == dealerGroup.DealershipGroupId
                                 select e.idWhitmanenterpriseDealership;

                var avaiInventory = context.whitmanenterpriseappraisals.Where(LogicHelper.BuildContainsExpression<whitmanenterpriseappraisal, int>(e => e.DealershipId.Value, dealerList));
                if (avaiInventory.Any(o => o.VINNumber.ToLower().Equals(vin.ToLower())))
                {
                    return avaiInventory.First(o => o.VINNumber.ToLower().Equals(vin.ToLower())).idAppraisal;
                }
            }
            return 0;
        }

        public static bool CheckVinExistInSoldOut(string vin, DealershipViewModel dealer)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisedealershipinventorysoldouts.Any(o => o.VINNumber == vin && o.DealershipId == dealer.DealershipId))
                {
                    return true;
                }
            }
            return false;
        }

        public static string UpdateSalePrice(int ListingId, string salePrice)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(ListingId);

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == lid);

                var oldPrice = searchResult.SalePrice;

                searchResult.SalePrice = salePrice;

                searchResult.LastUpdated = DateTime.Now;

                if (searchResult.DealershipId == 2183 || searchResult.DealershipId == 10997)
                {

                    var salePriceNumber = 0;

                    bool flag = Int32.TryParse(salePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        searchResult.RetailPrice = (Math.Round(salePriceNumber*1.1)).ToString(CultureInfo.InvariantCulture);

                        searchResult.WindowStickerPrice = salePriceNumber.ToString(CultureInfo.InvariantCulture);
                        
                        searchResult.DealerDiscount =
                            (Convert.ToInt32(searchResult.RetailPrice) -
                             Convert.ToInt32(searchResult.WindowStickerPrice)).ToString(CultureInfo.InvariantCulture);
                    }
                }

                context.SaveChanges();

                return oldPrice;
            }

        }

        public static void UpdateDescription(int ListingId, string Dscription)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.LastUpdated = DateTime.Now;

                searchResult.Descriptions = Dscription;

                context.SaveChanges();
            }

        }

        public static void UpdateKBBOptions(int ListingId, string OptionSelect, int TrimId, string BaseWholeSale, string WholeSale, string MileageAdjustment)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.LastUpdated = DateTime.Now;

                searchResult.KBBOptionsId = OptionSelect;

                searchResult.KBBTrimId = TrimId;

                if (context.whitmanenterprisekbbs.Any(x => x.TrimId == TrimId))
                {
                    var searchVinKBBResults = context.whitmanenterprisekbbs.Where(x => x.TrimId == TrimId);

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

        public static void UpdateKBBOptionsForAppraisal(int appraisalId, string optionSelect, int trimId, string baseWholeSale, string wholeSale, string mileageAdjustment)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == appraisalId);

                searchResult.LastUpdated = DateTime.Now;

                searchResult.KBBOptionsId = optionSelect;

                searchResult.KBBTrimId = trimId;

                if (context.whitmanenterprisekbbs.Any(x => x.TrimId == trimId))
                {
                    var searchVinKBBResults = context.whitmanenterprisekbbs.Where(x => x.TrimId == trimId);

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

        public static void UpdateReconStatus(int ListingId, bool status)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.LastUpdated = DateTime.Now;

                searchResult.Recon = status;

                context.SaveChanges();
            }

        }

        public static void UpdateMileage(string ListingId, string Mileage)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int LID = Convert.ToInt32(ListingId);

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

                searchResult.Mileage = Mileage;


                searchResult.LastUpdated = DateTime.Now;


                context.SaveChanges();
            }


        }

        public static void UpdateWarrantyInfo(int WarrantyInfo, int ListingId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.WarrantyInfo = WarrantyInfo;


                searchResult.LastUpdated = DateTime.Now;


                context.SaveChanges();
            }

        }

        public static void UpdatePriorRental(bool PriorRental, int ListingId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.PriorRental = PriorRental;


                searchResult.LastUpdated = DateTime.Now;


                context.SaveChanges();
            }

        }

        public static void UpdateDealerDemo(bool DealerDemo, int ListingId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.DealerDemo = DealerDemo;


                searchResult.LastUpdated = DateTime.Now;


                context.SaveChanges();
            }

        }

        public static void UpdateUnwind(bool Unwind, int ListingId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.Unwind = Unwind;


                searchResult.LastUpdated = DateTime.Now;


                context.SaveChanges();
            }

        }

        public static void UpdateDefaultStockImageUrl(int dealershipId, string defaultStockImageUrl)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealershipId);

                searchResult.DefaultStockImageUrl = defaultStockImageUrl;

                context.SaveChanges();
            }



        }

        public static void UpdateOverideStockImage(int dealershipId, bool OverideStockImage)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealershipId);

                searchResult.OverideStockImage = OverideStockImage;


                context.SaveChanges();
            }


        }

        public static string UpdateDealerCost(int ListingId, string dealerCost)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                var oldDealerCost = searchResult.DealerCost;

                searchResult.DealerCost = dealerCost;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();

                return oldDealerCost;
            }


        }

        public static void UpdateAppraisal(AppraisalViewFormModel appraisal)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int Id = Convert.ToInt32(appraisal.AppraisalGenerateId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == Id);

                var pureMileage = CommonHelper.RemoveSpecialCharactersForPurePrice(appraisal.Mileage);

                if (!searchResult.Mileage.Equals(pureMileage))
                {

                    if (searchResult.KBBTrimId == null)

                        KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.VINNumber, appraisal.DealershipZipCode,
                                                          pureMileage);
                    else
                    {
                        KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.VINNumber, appraisal.DealershipZipCode,
                                                          pureMileage, searchResult.KBBTrimId.Value,
                                                          searchResult.KBBOptionsId);

                    }

                }

                searchResult.VINNumber = appraisal.VinNumber;

                searchResult.StockNumber = appraisal.StockNumber;

                searchResult.Make = appraisal.Make;

                searchResult.Model = appraisal.SelectedModel;

                searchResult.ExteriorColor = appraisal.SelectedExteriorColorValue;
                searchResult.ColorCode = appraisal.SelectedExteriorColorCode;

                searchResult.InteriorColor = appraisal.SelectedInteriorColor;

                searchResult.Trim = appraisal.SelectedTrim;
                searchResult.ChromeStyleId = appraisal.ChromeStyleId;

                searchResult.Mileage = appraisal.Mileage;

                searchResult.Tranmission = appraisal.SelectedTranmission;

                searchResult.Cylinders = appraisal.SelectedCylinder;

                searchResult.Liters = appraisal.SelectedLiters;

                searchResult.Doors = appraisal.Door;

                searchResult.BodyType = appraisal.SelectedBodyType;

                searchResult.FuelType = appraisal.SelectedFuel;

                searchResult.DriveTrain = appraisal.SelectedDriveTrain;

                searchResult.Note = appraisal.Notes;

                searchResult.CarsOptions = appraisal.SelectedFactoryOptions;

                searchResult.CarsPackages = appraisal.SelectedPackageOptions;

                searchResult.MSRP = CommonHelper.RemoveSpecialCharactersForPurePrice(appraisal.MSRP);

                searchResult.Mileage = pureMileage;

                searchResult.Note = appraisal.Notes;

                searchResult.LastUpdated = DateTime.Now;

                searchResult.TruckCategory = null;
                searchResult.TruckClass = null;
                searchResult.TruckType = null;
                searchResult.VehicleType = "Car";
                searchResult.PackageDescriptions = appraisal.SelectedPackagesDescription;
                if (searchResult.Trim != null && searchResult.Trim.Equals("Base/Other Trims"))
                {
                    searchResult.Trim = String.Empty;
                }

                context.SaveChanges();

            }

        }

        public static void UpdateTruckAppraisal(AppraisalViewFormModel appraisal)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int Id = Convert.ToInt32(appraisal.AppraisalGenerateId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == Id);

                var pureMileage = CommonHelper.RemoveSpecialCharactersForPurePrice(appraisal.Mileage);


                if (!searchResult.Mileage.Equals(pureMileage))
                {

                    if (searchResult.KBBTrimId == null)

                        KellyBlueBookHelper.GetDirectFullReport(searchResult.VINNumber, appraisal.DealershipZipCode,
                                                          pureMileage);
                    else
                    {
                        KellyBlueBookHelper.GetDirectFullReport(searchResult.VINNumber, appraisal.DealershipZipCode,
                                                          pureMileage, searchResult.KBBTrimId.Value,
                                                          searchResult.KBBOptionsId);

                    }

                }


                searchResult.VINNumber = appraisal.VinNumber;
                searchResult.StockNumber = appraisal.StockNumber;

                searchResult.Make = appraisal.Make;

                searchResult.Model = appraisal.SelectedModel;

                searchResult.ExteriorColor = appraisal.SelectedExteriorColorValue;
                searchResult.ColorCode = appraisal.SelectedExteriorColorCode;

                searchResult.InteriorColor = appraisal.SelectedInteriorColor;

                searchResult.Trim = appraisal.SelectedTrim;
                searchResult.ChromeStyleId = appraisal.ChromeStyleId;
                searchResult.Mileage = appraisal.Mileage;

                searchResult.Tranmission = appraisal.SelectedTranmission;

                searchResult.Cylinders = appraisal.SelectedCylinder;

                searchResult.Liters = appraisal.SelectedLiters;

                searchResult.Doors = appraisal.Door;

                searchResult.BodyType = appraisal.SelectedBodyType;

                searchResult.FuelType = appraisal.SelectedFuel;

                searchResult.DriveTrain = appraisal.SelectedDriveTrain;

                searchResult.Note = appraisal.Notes;

                searchResult.CarsOptions = appraisal.SelectedFactoryOptions;

                searchResult.CarsPackages = appraisal.SelectedPackageOptions;

                searchResult.MSRP = CommonHelper.RemoveSpecialCharactersForPurePrice(appraisal.MSRP);

                searchResult.Mileage = pureMileage;

                searchResult.Note = appraisal.Notes;

                searchResult.LastUpdated = DateTime.Now;
                searchResult.PackageDescriptions = appraisal.SelectedPackagesDescription;


                searchResult.TruckCategory = appraisal.SelectedTruckCategory;
                searchResult.TruckClass = appraisal.SelectedTruckClass;
                searchResult.TruckType = appraisal.SelectedTruckType;
                searchResult.VehicleType = "Truck";

                if (searchResult.Trim != null && searchResult.Trim.Equals("Base/Other Trims"))
                {
                    searchResult.Trim = String.Empty;
                }

                context.SaveChanges();

            }

        }

        public static void UpdateIProfile(int ListingId, string Vin, string StockNumber, string ModelYear, string Make, string Model, string ExteriorColor, string InteriorColor, string Trim, string Mileage, string Tranmission, string Cylinders, string Liters, string Doors, string Style, string Fuel, string Drive, string Description, string Packages, string Options, string MSRP, bool Certified, string RetailPrice, string DiscountPrice, string ManufacturerRebate, string WindowStickerPrice, DealershipViewModel dealer, string exteriorColorCode, string Title, string chromeStyleId, string CusTrim, string selectedPackagesDescription, bool aCar, bool brandedTitle)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                if (!searchResult.Mileage.Equals(Mileage))
                {
                    if (searchResult.NewUsed == "Used")
                    {
                        if (searchResult.KBBTrimId == null)

                            KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.VINNumber, dealer.ZipCode, Mileage);
                        else
                        {
                            KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.VINNumber, dealer.ZipCode, Mileage, searchResult.KBBTrimId.Value, searchResult.KBBOptionsId);

                        }
                    }


                }

                searchResult.VINNumber = Vin;

                searchResult.AdditionalTitle = Title;

                searchResult.StockNumber = StockNumber;

                searchResult.ModelYear = Convert.ToInt32(ModelYear);

                searchResult.Make = Make;

                searchResult.Model = Model;

                searchResult.ExteriorColor = ExteriorColor;

                searchResult.InteriorColor = InteriorColor;

                UpdateTrimFromCustomTrim(Trim, chromeStyleId, CusTrim, searchResult);

                searchResult.Mileage = Mileage;

                searchResult.Tranmission = Tranmission;

                searchResult.Cylinders = Cylinders;

                searchResult.Liters = Liters;

                searchResult.Doors = Doors;

                searchResult.BodyType = Style;

                searchResult.FuelType = Fuel;

                searchResult.DriveTrain = Drive;

                searchResult.Descriptions = Description;

                searchResult.CarsOptions = Options;

                searchResult.CarsPackages = Packages;

                searchResult.MSRP = MSRP;

                searchResult.VehicleType = "Car";

                searchResult.Certified = (Certified) ? true : false;

                searchResult.RetailPrice = RetailPrice;

                searchResult.ManufacturerRebate = ManufacturerRebate;

                searchResult.DealerDiscount = DiscountPrice;

                searchResult.WindowStickerPrice = WindowStickerPrice;

                searchResult.LastUpdated = DateTime.Now;

                searchResult.ColorCode = exteriorColorCode;
                searchResult.PackageDescriptions = selectedPackagesDescription;
                searchResult.ACar = aCar;
                searchResult.BrandedTitle = brandedTitle;
                try
                {
                    var autoDescription = new AutoDescription();

                    if (autoDescription.AllowAutoDescription(searchResult, dealer.DealershipId))
                    {
                        var newDescription = autoDescription.GenerateAutoDescription(searchResult);
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

        private static void UpdateTrimFromCustomTrim(string Trim, string chromeStyleId, string CusTrim, whitmanenterprisedealershipinventory searchResult)
        {

            if (!String.IsNullOrEmpty(Trim))
            {
                var result = Trim.Split('|');

                if (result.Count() > 1)
                {
                    if (!result[1].Equals("Base/Other Trims"))
                    {
                        searchResult.Trim = result[1];
                    }
                    else
                    {
                        searchResult.Trim = CusTrim ?? String.Empty;
                    }
                    searchResult.ChromeStyleId = result[0];
                }
                else if (!String.IsNullOrEmpty(CusTrim))
                {
                    if (CusTrim.Equals("Base/Other Trims"))
                        searchResult.Trim = "";
                    else
                    {
                        searchResult.Trim = CusTrim;
                    }
                    searchResult.ChromeStyleId = null;
                }
                else
                {
                    searchResult.Trim = Trim.Equals("Base/Other Trims") ? "" : Trim;
                    searchResult.ChromeStyleId = chromeStyleId;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(CusTrim))
                {
                    if (CusTrim.Equals("Base/Other Trims"))
                        searchResult.Trim = "";
                    else
                    {
                        searchResult.Trim = CusTrim;
                    }
                }
            }
        }

        public static void UpdateITruckProfile(string ListingId, string Vin, string StockNumber, string ModelYear, string Make, string Model, string ExteriorColor, string InteriorColor, string Trim, string Mileage, string Tranmission, string Cylinders, string Liters, string Doors, string Style, string Fuel, string Drive, string Description, string Packages, string Options, string MSRP, bool Certified, string TruckType, string TruckClass, string TruckCategory, string RetailPrice, string DiscountPrice, string ManufacturerRebate, string WindowStickerPrice, DealershipViewModel dealer, string colorCode, string Title, string chromeStyleId, string CusTrim, string selectedPackagesDescription, bool aCar, bool brandedTitle)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int LID = Convert.ToInt32(ListingId);

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

                if (!searchResult.Mileage.Equals(Mileage))
                {
                    if (searchResult.NewUsed == "Used")
                    {
                        if (searchResult.KBBTrimId == null)

                            KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.VINNumber, dealer.ZipCode, Mileage);
                        else
                        {
                            KellyBlueBookHelper.GetFullReportWithSavingChanges(searchResult.VINNumber, dealer.ZipCode, Mileage, searchResult.KBBTrimId.Value, searchResult.KBBOptionsId);

                        }
                    }


                }

                searchResult.VINNumber = Vin;

                searchResult.AdditionalTitle = Title;

                searchResult.StockNumber = StockNumber;

                searchResult.ModelYear = Convert.ToInt32(ModelYear);

                searchResult.Make = Make;

                searchResult.Model = Model;

                searchResult.ExteriorColor = ExteriorColor;
                searchResult.ColorCode = colorCode;


                searchResult.InteriorColor = InteriorColor;

                UpdateTrimFromCustomTrim(Trim, chromeStyleId, CusTrim, searchResult);



                searchResult.Mileage = Mileage;

                searchResult.Tranmission = Tranmission;

                searchResult.Cylinders = Cylinders;

                searchResult.Liters = Liters;

                searchResult.Doors = Doors;

                searchResult.BodyType = Style;

                searchResult.FuelType = Fuel;

                searchResult.DriveTrain = Drive;

                searchResult.Descriptions = Description;

                searchResult.CarsPackages = Packages;

                searchResult.CarsOptions = Options;

                searchResult.MSRP = MSRP;

                searchResult.Certified = (Certified) ? true : false;

                searchResult.RetailPrice = RetailPrice;

                searchResult.ManufacturerRebate = ManufacturerRebate;

                searchResult.DealerDiscount = DiscountPrice;

                searchResult.WindowStickerPrice = WindowStickerPrice;

                searchResult.LastUpdated = DateTime.Now;

                searchResult.VehicleType = "Truck";

                searchResult.TruckCategory = TruckCategory;

                searchResult.TruckClass = TruckClass;

                searchResult.TruckType = TruckType;
                searchResult.PackageDescriptions = selectedPackagesDescription;
                searchResult.ACar = aCar;
                searchResult.BrandedTitle = brandedTitle;

                try
                {
                    var autoDescription = new AutoDescription();

                    if (autoDescription.AllowAutoDescription(searchResult, dealer.DealershipId))
                    {
                        var newDescription = autoDescription.GenerateAutoDescription(searchResult);
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

        public static string UpdateACV(int ListingId, string ACV)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {


                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                var oldAcv = searchResult.ACV;

                searchResult.ACV = ACV;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();

                return oldAcv;
            }
        }

        public static void UpdateAcvForAppraisal(string appraisalId, string acv)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.ACV = acv;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerFirstNameForAppraisal(string appraisalId, string customerFirstName)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.FirstName = customerFirstName;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerLastNameForAppraisal(string appraisalId, string customerLastName)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.LastName = customerLastName;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerAddressForAppraisal(string appraisalId, string customerAddress)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.Address = customerAddress;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerCityForAppraisal(string appraisalId, string customerCity)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.City = customerCity;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerStateForAppraisal(string appraisalId, string customerState)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.State = customerState;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerZipCodeForAppraisal(string appraisalId, string customerZipCode)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.ZipCode = customerZipCode;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCustomerEmailForAppraisal(string appraisalId, string customerEmail)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(appraisalId);

                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == lid);

                searchResult.Email = customerEmail;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }


        public static void UpdateCarImageURL(int ListingId, string CarImageUrl, string ThumbnailImageUrl)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.CarImageUrl = CarImageUrl;

                searchResult.ThumbnailImageURL = ThumbnailImageUrl;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateCarImageSoldURL(int ListingId, string CarImageUrl, string ThumbnailImageUrl)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.CarImageUrl = CarImageUrl;

                searchResult.ThumbnailImageURL = ThumbnailImageUrl;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static void UpdateAppSetting(int dealershipId, AdminViewModel admin)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealershipId);

                var searchDealer = context.whitmanenterprisedealerships.FirstOrDefault(x => x.idWhitmanenterpriseDealership == dealershipId);

                searchResult.InventorySorting = admin.SortSet;

                searchResult.SoldOut = admin.SoldAction;


                searchResult.CarFax = admin.CarFax;

                searchResult.CarFaxPassword = admin.CarFaxPassword;
                searchResult.Manheim = admin.Manheim;

                searchResult.ManheimPassword = admin.ManheimPassword;
                searchResult.KellyBlueBook = admin.KellyBlueBook;

                searchResult.KellyPassword = admin.KellyPassword;
                searchResult.BlackBook = admin.BlackBook;

                searchResult.BlackBookPassword = admin.BlackBookPassword;

                searchResult.TermsAndCondition = admin.TermConditon;

                searchResult.DealerWarranty = admin.DealerWarrantyInfo;

                searchResult.StartDescriptionSentence = admin.StartSentence;

                searchResult.EndDescriptionSentence = admin.EndSentence;

                searchResult.AuctionSentence = admin.AuctionSentence;

                searchResult.ShippingInfo = admin.ShippingInfo;

                searchResult.FirstTimeRangeBucketJump = admin.FirstRange;
                searchResult.SecondTimeRangeBucketJump = admin.SecondRange;
                searchResult.IntervalBucketJump = admin.SelectedInterval;

                //searchDealer.Address = admin.Address;
                //searchDealer.City = admin.City;
                //searchDealer.State = admin.State;
                //searchDealer.ZipCode = admin.ZipCode;
                searchDealer.Phone = admin.Phone;
                searchDealer.Email = admin.Email;

                searchResult.SalePriceText = admin.SalePriceWsNotificationText;
                searchResult.DealerDiscountText = admin.DealerDiscountWSNotificationText;
                searchResult.ManufactureReabateText = admin.ManufacturerReabteWsNotificationText;
                searchResult.RetailPriceText = admin.RetailPriceWSNotificationText;
                searchResult.AutoDescription = admin.EnableAutoDescription;

                context.SaveChanges();
            }



        }

        public static void UpdatePass(string Pass, string Username)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterpriseusers.Where(x => x.UserName == Username);

                foreach (var tmp in searchResult)
                {
                    tmp.Password = Pass;

                }


                context.SaveChanges();
            }


        }

        public static void UpdateEmail(string Email, string Username)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterpriseusers.Where(x => x.UserName == Username);

                foreach (var tmp in searchResult)
                {
                    tmp.Email = Email;

                }



                context.SaveChanges();
            }

        }

        public static void UpdateCellPhone(string CellPhone, string Username)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterpriseusers.Where(x => x.UserName == Username);

                foreach (var tmp in searchResult)
                {
                    tmp.Cellphone = CellPhone;

                }


                context.SaveChanges();
            }

        }

        public static void UpdateDefaultLogin(string Username, string DefaultLogin)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterpriseusers.Where(x => x.UserName == Username);

                foreach (var tmp in searchResult)
                {
                    tmp.DefaultLogin = Convert.ToInt32(DefaultLogin);

                }


                context.SaveChanges();
            }

        }

        public static void ChangeRole(string Role, string Username)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterpriseusers.Where(x => x.UserName == Username);

                foreach (var tmp in searchResult)
                {
                    tmp.RoleName = Role;
                    UpdateUserGroup(Role, tmp.UserName);
                }
                context.SaveChanges();
            }


        }

        private static void UpdateUserGroup(string role, string userName)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var user = context.vincontrolusers.Where(x => x.username == userName).FirstOrDefault();
                if (user != null)
                {
                    user.rolename = role;
                    var userGroup = context.vincontrolusergroups.Where(x => x.vincontroluser.userid == user.userid).FirstOrDefault();
                    var groupId = GetUserGroupByRoleName(role);
                    var vincontrolGroup = context.vincontrolgroups.Where(i => i.groupid == groupId).FirstOrDefault();
                    if (userGroup != null)
                        userGroup.vincontrolgroup = vincontrolGroup;
                }

                context.SaveChanges();
            }
        }

        public static void DeleteUser(string Username)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var searchResult = context.whitmanenterpriseusers.Where(x => x.UserName == Username);

                foreach (var tmp in searchResult)
                {
                    tmp.Active = false;

                }

                context.SaveChanges();
            }


        }

        public static bool checkMasterUserExist(string userName, string passWord)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisedealergroups.Any(o => o.MasterUserName == userName && o.MasterLogin == passWord))
                {
                    return true;
                }
            }
            return false;
        }

        public static UserRoleViewModel CheckUserExistWithStatus(string userName, string passWord)
        {
            var user = new UserRoleViewModel();
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                //check if master login
                if (context.whitmanenterprisedealergroups.Any(o => o.MasterUserName == userName && o.MasterLogin == passWord))
                {
                    var result =
                     context.whitmanenterprisedealergroups.FirstOrDefault(
                         o => o.MasterUserName == userName && o.MasterLogin == passWord);
                    user = new UserRoleViewModel()
                    {
                        UserExist = true,
                        MasterLogin = true,
                        UserName = result.MasterUserName,
                        PassWord = result.MasterLogin,
                        Name = result.DealerGroupName,
                        MultipleDealerLogin = true,
                        DealershipId = result.DefaultDealerID.GetValueOrDefault(),
                        DefaultLogin = result.DefaultDealerID.GetValueOrDefault(),
                        DealerGroupId = result.DealerGroupId,
                        Role = KingRole

                    };
                    user.UserExist = true;
                }
                else if (context.whitmanenterpriseusers.Any(o => o.UserName == userName && o.Password == passWord && o.Active.Value))
                {

                    var result =
                        context.whitmanenterpriseusers.Where(
                            o => o.UserName == userName && o.Password == passWord).ToList();

                    if (result.Count == 1 && result[0].DefaultLogin!=Constanst.DealerGroupConst)
                    {

                        user = new UserRoleViewModel()
                                   {
                                       UserExist = true,
                                       UserName = result.FirstOrDefault().UserName,
                                       PassWord = result.FirstOrDefault().Password,
                                       Name = result.FirstOrDefault().Name,
                                       MultipleDealerLogin = false,
                                       DealershipId = result.FirstOrDefault().DealershipID.GetValueOrDefault(),
                                       DefaultLogin = result.FirstOrDefault().DefaultLogin.GetValueOrDefault(),
                                       Role = result.FirstOrDefault().RoleName
                                   };
                    }
                    else
                    {
                       
                        //user in multiple store case
                        user = new UserRoleViewModel()
                        {
                            UserExist = true,
                            UserName = result.FirstOrDefault().UserName,
                            PassWord = result.FirstOrDefault().Password,
                            Name = result.FirstOrDefault().Name,
                            MultipleDealerLogin = true,
                            DealershipId = result.FirstOrDefault().DefaultLogin.GetValueOrDefault(),
                            DefaultLogin = result.FirstOrDefault().DefaultLogin.GetValueOrDefault(),
                            DealerGroupId = result.FirstOrDefault().DealerGroupId,
                            Role = result.FirstOrDefault().RoleName

                        };

                        //user can see all stores in dealergroup case
                        if (result[0].DefaultLogin == Constanst.DealerGroupConst)
                        {
                            user.CanSeeAllStores = true;
                        }
                    }

                    user.UserExist = true;

                }


            }


            return user;

        }

        public static int GetDefaultLoginFromUserName(string userName)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var result =
                   context.whitmanenterpriseusers.First(
                       o => o.UserName == userName);
                return result.DefaultLogin.GetValueOrDefault();
            }
            return 0;
        }

        public static bool CheckUserNameExist(string userName, int dealerID)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterpriseusers.Any(o => o.UserName == userName && o.DealershipID == dealerID))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckUserNameExist(string userName)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterpriseusers.Any(o => o.UserName == userName))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AddUser(UserRoleViewModel user)
        {
            if (!CheckUserNameExist(user.UserName))
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var wUser = new whitmanenterpriseuser()
                    {
                        Name = user.Name,
                        UserName = user.UserName,
                        Password = user.PassWord,
                        Email = user.Email,
                        Cellphone = user.Cellphone,
                        RoleName = user.Role,
                        DealershipID = user.DealershipId,
                        Active = true,
                        DefaultLogin = user.DefaultLogin,
                    };

                    var wUserNotify = new whitmanenterpriseusersnotification()
                    {
                        UserName = user.UserName,
                        DealershipId = user.DealershipId,
                        AppraisalNotification = false,
                        C24Hnotification = false,
                        InventoryNotification = false,
                        NoteNotification = false,
                        PriceChangeNotification = false,
                        WholeNotification = false,
                    };

                    var vincontrolUser = new vincontroluser()
                    {
                        username = user.UserName,
                        expirationdate = DateTime.Today.AddYears(2),
                        accountstatus = true,
                        password = user.PassWord,
                        rolename = user.Role
                    };

                    var groupId = GetUserGroupByRoleName(user.Role);
                    var vincontrolGroup = context.vincontrolgroups.Where(i => i.groupid == groupId).FirstOrDefault();

                    context.AddTowhitmanenterpriseusers(wUser);

                    context.AddTowhitmanenterpriseusersnotifications(wUserNotify);

                    context.AddTovincontrolusers(vincontrolUser);

                    context.AddTovincontrolusergroups(new vincontrolusergroup()
                    {
                        vincontroluser = vincontrolUser,
                        vincontrolgroup = vincontrolGroup,
                        userstamp = "System",
                        datestamp = DateTime.Now
                    });

                    context.SaveChanges();

                    //LinqHelper.AddNewActivity(SessionHandler.Dealership.DealershipId, "New User " + user.UserName + " With " + user.Role + " Role", String.Format("User Id: {0};User Name: {1};Email: {2};Cell Phone: {3};Role: {4}", wUser.idwhitmanenterpriseuser, user.UserName, user.Email, user.Cellphone, user.Role), "NewUser");
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private static int GetUserGroupByRoleName(string roleName)
        {
            var userGroup = 0;
            switch (roleName)
            {
                case "Admin":
                    userGroup = 1;
                    break;
                case "Manager":
                    userGroup = 2;
                    break;
                case "Employee":
                    userGroup = 3;
                    break;
                default:
                    userGroup = 3;
                    break;
            }

            return userGroup;
        }

        public static void AddUserMultipleStore(UserRoleViewModel user)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var wUser = new whitmanenterpriseuser()
                {
                    Name = user.Name,
                    UserName = user.UserName,
                    Password = user.PassWord,
                    Email = user.Email,
                    Cellphone = user.Cellphone,
                    RoleName = user.Role,
                    DealershipID = user.DealershipId,
                    DealerGroupId = user.DealerGroupId,
                    DefaultLogin = user.DefaultLogin,
                    Active = true
                };

                var vincontrolUser = new vincontroluser()
                {
                    username = user.UserName,
                    expirationdate = DateTime.Today.AddYears(2),
                    accountstatus = true,
                    password = user.PassWord,
                    rolename = user.Role
                };

                var groupId = GetUserGroupByRoleName(user.Role);
                var vincontrolGroup =
                    context.vincontrolgroups.Where(i => i.groupid == groupId).
                        FirstOrDefault();

                context.AddTovincontrolusers(vincontrolUser);

                context.AddTovincontrolusergroups(new vincontrolusergroup()
                {
                    vincontroluser = vincontrolUser,
                    vincontrolgroup = vincontrolGroup,
                    userstamp = "System",
                    datestamp = DateTime.Now
                });

                context.AddTowhitmanenterpriseusers(wUser);
                context.SaveChanges();
            }



        }

        public static void AddUserNotification(UserRoleViewModel user)
        {


            using (var context = new whitmanenterprisewarehouseEntities())
            {


                var wUserNotify = new whitmanenterpriseusersnotification()
                {

                    UserName = user.UserName,
                    DealershipId = user.DealershipId,
                    AppraisalNotification = false,
                    C24Hnotification = false,
                    InventoryNotification = false,
                    NoteNotification = false,
                    PriceChangeNotification = false,
                    WholeNotification = false




                };



                context.AddTowhitmanenterpriseusersnotifications(wUserNotify);



                context.SaveChanges();
            }

        }

        public static void TransferToWholeSaleFromInventory(int ListingId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var wdi = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);


                var vehicle = new vincontrolwholesaleinventory()
                                  {
                                      ModelYear = wdi.ModelYear,
                                      Make = wdi.Make,
                                      Model = wdi.Model,
                                      Trim = wdi.Trim,
                                      VINNumber = wdi.VINNumber,
                                      StockNumber = wdi.StockNumber,
                                      SalePrice = wdi.SalePrice,
                                      MSRP = wdi.MSRP,
                                      Mileage = wdi.Mileage,
                                      ExteriorColor = wdi.ExteriorColor,
                                      InteriorColor = wdi.InteriorColor,
                                      InteriorSurface = wdi.InteriorSurface,
                                      BodyType = wdi.BodyType,
                                      Cylinders = wdi.Cylinders,
                                      Liters = wdi.Liters,
                                      EngineType = wdi.EngineType,
                                      DriveTrain = wdi.DriveTrain,
                                      FuelType = wdi.FuelType,
                                      Tranmission = wdi.Tranmission,
                                      Doors = wdi.Doors,
                                      Certified = wdi.Certified,
                                      CarsOptions = wdi.CarsOptions,
                                      CarsPackages = wdi.CarsPackages,
                                      StandardOptions = wdi.StandardOptions,
                                      Descriptions = wdi.Descriptions,
                                      CarImageUrl = wdi.CarImageUrl,
                                      ThumbnailImageURL = wdi.ThumbnailImageURL,
                                      DateInStock = wdi.DateInStock,
                                      LastUpdated = DateTime.Now,
                                      DealershipName = wdi.DealershipName,
                                      DealershipAddress = wdi.DealershipAddress,
                                      DealershipCity = wdi.DealershipCity,
                                      DealershipState = wdi.DealershipState,
                                      DealershipPhone = wdi.DealershipPhone,
                                      DealershipId = wdi.DealershipId,
                                      DealershipZipCode = wdi.DealershipZipCode,
                                      DefaultImageUrl = wdi.DefaultImageUrl,
                                      NewUsed = wdi.NewUsed,
                                      AddToInventoryBy = wdi.AddToInventoryBy,
                                      AppraisalID = wdi.AppraisalID,
                                      ACV = wdi.ACV,
                                      DealerCost = wdi.DealerCost,
                                      FuelEconomyCity = wdi.FuelEconomyCity,
                                      FuelEconomyHighWay = wdi.FuelEconomyHighWay,
                                      WarrantyInfo = wdi.WarrantyInfo,
                                      RetailPrice = wdi.RetailPrice,
                                      DealerDiscount = wdi.DealerDiscount,
                                      ManufacturerRebate = wdi.ManufacturerRebate,
                                      WindowStickerPrice = wdi.WindowStickerPrice,
                                      CarFaxOwner = wdi.CarFaxOwner,
                                      PriorRental = wdi.PriorRental,
                                      DealerDemo = wdi.DealerDemo,
                                      Unwind = wdi.Unwind,
                                      Recon = wdi.Recon,
                                      KBBOptionsId = wdi.KBBOptionsId,
                                      KBBTrimId = wdi.KBBTrimId,
                                      Disclaimer = wdi.Disclaimer,
                                      AdditionalTitle = wdi.AdditionalTitle,
                                      ColorCode = wdi.ColorCode,
                                      ChromeModelId = wdi.ChromeModelId,
                                      ChromeStyleId = wdi.ChromeStyleId


                                  };

                vehicle.VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType;
                vehicle.TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory;
                vehicle.TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass;
                vehicle.TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType;

                context.AddTovincontrolwholesaleinventories(vehicle);

                context.Attach(wdi);

                context.DeleteObject(wdi);

                context.SaveChanges();




            }
        }

        public static void TransferToWholeSaleFromSoldInventory(int ListingId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var wdi = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == ListingId);



                var vehicle = new vincontrolwholesaleinventory()
                {

                    ModelYear = wdi.ModelYear,
                    Make = wdi.Make,
                    Model = wdi.Model,
                    Trim = wdi.Trim,
                    VINNumber = wdi.VINNumber,
                    StockNumber = wdi.StockNumber,
                    SalePrice = wdi.SalePrice,
                    MSRP = wdi.MSRP,
                    Mileage = wdi.Mileage,
                    ExteriorColor = wdi.ExteriorColor,
                    InteriorColor = wdi.InteriorColor,
                    InteriorSurface = wdi.InteriorSurface,
                    BodyType = wdi.BodyType,
                    Cylinders = wdi.Cylinders,
                    Liters = wdi.Liters,
                    EngineType = wdi.EngineType,
                    DriveTrain = wdi.DriveTrain,
                    FuelType = wdi.FuelType,
                    Tranmission = wdi.Tranmission,
                    Doors = wdi.Doors,
                    Certified = wdi.Certified,
                    CarsOptions = wdi.CarsOptions,
                    CarsPackages = wdi.CarsPackages,
                    StandardOptions = wdi.StandardOptions,
                    Descriptions = wdi.Descriptions,
                    CarImageUrl = wdi.CarImageUrl,
                    ThumbnailImageURL = wdi.ThumbnailImageURL,
                    DateInStock = wdi.DateInStock,
                    LastUpdated = DateTime.Now,
                    DealershipName = wdi.DealershipName,
                    DealershipAddress = wdi.DealershipAddress,
                    DealershipCity = wdi.DealershipCity,
                    DealershipState = wdi.DealershipState,
                    DealershipPhone = wdi.DealershipPhone,
                    DealershipId = wdi.DealershipId,
                    DealershipZipCode = wdi.DealershipZipCode,
                    DefaultImageUrl = wdi.DefaultImageUrl,
                    NewUsed = wdi.NewUsed,
                    AddToInventoryBy = wdi.AddToInventoryBy,
                    AppraisalID = wdi.AppraisalID,
                    ACV = wdi.ACV,
                    DealerCost = wdi.DealerCost,
                    FuelEconomyCity = wdi.FuelEconomyCity,
                    FuelEconomyHighWay = wdi.FuelEconomyHighWay,
                    WarrantyInfo = wdi.WarrantyInfo,
                    RetailPrice = wdi.RetailPrice,
                    DealerDiscount = wdi.DealerDiscount,
                    ManufacturerRebate = wdi.ManufacturerRebate,
                    WindowStickerPrice = wdi.WindowStickerPrice,
                    CarFaxOwner = wdi.CarFaxOwner,
                    PriorRental = wdi.PriorRental,
                    Recon = wdi.Recon,
                    KBBOptionsId = wdi.KBBOptionsId,
                    KBBTrimId = wdi.KBBTrimId,
                    Disclaimer = wdi.Disclaimer,
                    ColorCode = wdi.ColorCode,
                    ChromeModelId = wdi.ChromeModelId,
                    ChromeStyleId = wdi.ChromeStyleId
                };

                vehicle.VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType;
                vehicle.TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory;
                vehicle.TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass;
                vehicle.TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType;

                var removeVehicle =
                    context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == wdi.ListingID);

                context.AddTovincontrolwholesaleinventories(vehicle);

                context.Attach(removeVehicle);

                context.DeleteObject(removeVehicle);

                context.SaveChanges();




            }
        }

        public static int TransferToInventoryFromWholesale(int listingId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var wdi = context.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == listingId);

                var vehicle = new whitmanenterprisedealershipinventory()
                {
                    ModelYear = wdi.ModelYear,
                    Make = wdi.Make,
                    Model = wdi.Model,
                    Trim = wdi.Trim,
                    VINNumber = wdi.VINNumber,
                    StockNumber = wdi.StockNumber,
                    SalePrice = wdi.SalePrice,
                    MSRP = wdi.MSRP,
                    Mileage = wdi.Mileage,
                    ExteriorColor = wdi.ExteriorColor,
                    InteriorColor = wdi.InteriorColor,
                    InteriorSurface = wdi.InteriorSurface,
                    BodyType = wdi.BodyType,
                    Cylinders = wdi.Cylinders,
                    Liters = wdi.Liters,
                    EngineType = wdi.EngineType,
                    DriveTrain = wdi.DriveTrain,
                    FuelType = wdi.FuelType,
                    Tranmission = wdi.Tranmission,
                    Doors = wdi.Doors,
                    Certified = wdi.Certified,
                    CarsOptions = wdi.CarsOptions,
                    CarsPackages = wdi.CarsPackages,
                    StandardOptions = wdi.StandardOptions,
                    Descriptions = wdi.Descriptions,
                    CarImageUrl = wdi.CarImageUrl,
                    ThumbnailImageURL = wdi.ThumbnailImageURL,
                    DateInStock = wdi.DateInStock,
                    LastUpdated = DateTime.Now,
                    DealershipName = wdi.DealershipName,
                    DealershipAddress = wdi.DealershipAddress,
                    DealershipCity = wdi.DealershipCity,
                    DealershipState = wdi.DealershipState,
                    DealershipPhone = wdi.DealershipPhone,
                    DealershipId = wdi.DealershipId,
                    DealershipZipCode = wdi.DealershipZipCode,
                    DefaultImageUrl = wdi.DefaultImageUrl,
                    NewUsed = wdi.NewUsed,
                    AddToInventoryBy = wdi.AddToInventoryBy,
                    AppraisalID = wdi.AppraisalID,
                    ACV = wdi.ACV,
                    DealerCost = wdi.DealerCost,
                    FuelEconomyCity = wdi.FuelEconomyCity,
                    FuelEconomyHighWay = wdi.FuelEconomyHighWay,
                    WarrantyInfo = wdi.WarrantyInfo,
                    RetailPrice = wdi.RetailPrice,
                    DealerDiscount = wdi.DealerDiscount,
                    ManufacturerRebate = wdi.ManufacturerRebate,
                    WindowStickerPrice = wdi.WindowStickerPrice,
                    CarFaxOwner = wdi.CarFaxOwner,
                    PriorRental = wdi.PriorRental,
                    Unwind = wdi.Unwind,
                    DealerDemo = wdi.DealerDemo,
                    Recon = wdi.Recon,
                    KBBOptionsId = wdi.KBBOptionsId,
                    KBBTrimId = wdi.KBBTrimId,
                    Disclaimer = wdi.Disclaimer,
                    AdditionalTitle = wdi.AdditionalTitle,
                    ColorCode = wdi.ColorCode,
                    ChromeModelId = wdi.ChromeModelId,
                    ChromeStyleId = wdi.ChromeStyleId


                };

                vehicle.VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType;
                vehicle.TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory;
                vehicle.TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass;
                vehicle.TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType;

                var removeVehicle =
                    context.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == wdi.ListingID);

                context.AddTowhitmanenterprisedealershipinventories(vehicle);

                context.Attach(removeVehicle);

                context.DeleteObject(removeVehicle);

                context.SaveChanges();

                return vehicle.ListingID;


            }
        }

        public static void AddToWholeSaleFromAppraisal(AppraisalViewFormModel appraisal, DealershipViewModel dealer)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                int carfaxOwner = 0;


                var firstOrDefault = context.whitmanenteprisecarfaxes.FirstOrDefault(x => x.Vin == appraisal.VinNumber);
                if (firstOrDefault != null)
                    carfaxOwner = firstOrDefault.Owner.GetValueOrDefault();



                var vehicle = new vincontrolwholesaleinventory()
                {
                    //ListingID = autoListingId,
                    ModelYear = appraisal.ModelYear,
                    Make = String.IsNullOrEmpty(appraisal.Make) ? "" : appraisal.Make,
                    Model =
                        String.IsNullOrEmpty(appraisal.AppraisalModel) ? "" : appraisal.AppraisalModel,
                    Trim = String.IsNullOrEmpty(appraisal.SelectedTrim) ? "" : appraisal.SelectedTrim,
                    VINNumber = String.IsNullOrEmpty(appraisal.VinNumber) ? "" : appraisal.VinNumber,
                    StockNumber =
                        String.IsNullOrEmpty(appraisal.StockNumber) ? "" : appraisal.StockNumber,
                    SalePrice = String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    MSRP = String.IsNullOrEmpty(appraisal.MSRP) ? "" : appraisal.MSRP,
                    Mileage = String.IsNullOrEmpty(appraisal.Mileage) ? "" : appraisal.Mileage,
                    ExteriorColor =
                        String.IsNullOrEmpty(appraisal.SelectedExteriorColorValue)
                            ? ""
                            : appraisal.SelectedExteriorColorValue,
                    InteriorColor =
                        String.IsNullOrEmpty(appraisal.SelectedInteriorColor)
                            ? ""
                            : appraisal.SelectedInteriorColor,
                    InteriorSurface =
                        String.IsNullOrEmpty(appraisal.InteriorSurface)
                            ? ""
                            : appraisal.InteriorSurface,
                    BodyType =
                        String.IsNullOrEmpty(appraisal.SelectedBodyType)
                            ? ""
                            : appraisal.SelectedBodyType,
                    Cylinders =
                        String.IsNullOrEmpty(appraisal.SelectedCylinder)
                            ? ""
                            : appraisal.SelectedCylinder,
                    Liters =
                        String.IsNullOrEmpty(appraisal.SelectedLiters) ? "" : appraisal.SelectedLiters,
                    EngineType =
                        String.IsNullOrEmpty(appraisal.EngineType) ? "" : appraisal.EngineType,
                    DriveTrain =
                        String.IsNullOrEmpty(appraisal.SelectedDriveTrain)
                            ? ""
                            : appraisal.SelectedDriveTrain,
                    FuelType =
                        String.IsNullOrEmpty(appraisal.SelectedFuel) ? "" : appraisal.SelectedFuel,
                    Tranmission =
                        String.IsNullOrEmpty(appraisal.SelectedTranmission)
                            ? ""
                            : appraisal.SelectedTranmission,
                    Doors = String.IsNullOrEmpty(appraisal.Door) ? "" : appraisal.Door,
                    Certified = false,
                    StandardOptions =
                        String.IsNullOrEmpty(appraisal.StandardInstalledOption)
                            ? ""
                            : appraisal.StandardInstalledOption.Replace("\'", "\\'"),
                    CarsOptions =
                        String.IsNullOrEmpty(appraisal.SelectedFactoryOptions)
                            ? ""
                            : appraisal.SelectedFactoryOptions.Replace("\'", "\\'"),
                    CarsPackages =
                        String.IsNullOrEmpty(appraisal.SelectedPackageOptions)
                            ? ""
                            : appraisal.SelectedPackageOptions.Replace("\'", "\\'"),
                    Descriptions =
                        String.IsNullOrEmpty(appraisal.Descriptions)
                            ? ""
                            : appraisal.Descriptions.Replace("\'", "\\'"),
                    CarImageUrl =
                        String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                            ? ""
                            : appraisal.DefaultImageUrl,
                    ThumbnailImageURL =
                        String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                            ? ""
                            : appraisal.DefaultImageUrl,
                    DateInStock = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    DealershipName =
                        String.IsNullOrEmpty(appraisal.DealershipName)
                            ? ""
                            : appraisal.DealershipName.Replace("\'", "\\'"),
                    DealershipAddress =
                        String.IsNullOrEmpty(appraisal.DealershipAddress)
                            ? ""
                            : appraisal.DealershipAddress.Replace("\'", "\\'"),
                    DealershipCity =
                        String.IsNullOrEmpty(appraisal.DealershipCity)
                            ? ""
                            : appraisal.DealershipCity.Replace("\'", "\\'"),
                    DealershipState =
                        String.IsNullOrEmpty(appraisal.DealershipState)
                            ? ""
                            : appraisal.DealershipState,
                    DealershipPhone =
                        String.IsNullOrEmpty(appraisal.DealershipPhone)
                            ? ""
                            : appraisal.DealershipPhone,
                    DealershipZipCode =
                        String.IsNullOrEmpty(appraisal.DealershipZipCode)
                            ? ""
                            : appraisal.DealershipZipCode,
                    DealershipId =
                        String.IsNullOrEmpty(appraisal.DealershipId.ToString())
                            ? 0
                            : appraisal.DealershipId,
                    DefaultImageUrl =
                        String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                            ? ""
                            : appraisal.DefaultImageUrl,
                    NewUsed = "Used",
                    AddToInventoryBy =
                        String.IsNullOrEmpty(appraisal.AddToInventoryBy)
                            ? ""
                            : appraisal.AddToInventoryBy,
                    AppraisalID =
                        String.IsNullOrEmpty(appraisal.AppraisalGenerateId)
                            ? ""
                            : appraisal.AppraisalGenerateId,
                    ACV = appraisal.ACV,
                    DealerCost = appraisal.DealerCost,
                    FuelEconomyHighWay = appraisal.FuelEconomyHighWay,
                    FuelEconomyCity = appraisal.FuelEconomyCity,
                    PriorRental = false,
                    WindowStickerPrice =
                        String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    RetailPrice = String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    DealerDiscount = "0",
                    ManufacturerRebate = "0",
                    MarketRange = 0,
                    Recon = false,
                    CarFaxOwner = carfaxOwner,



                };

                if (appraisal.IsTruck)
                {
                    vehicle.TruckCategory = appraisal.SelectedTruckCategory;
                    vehicle.TruckClass = appraisal.SelectedTruckClass;
                    vehicle.TruckType = appraisal.SelectedTruckType;
                    vehicle.VehicleType = "Truck";
                }
                else
                    vehicle.VehicleType = "Car";



                if (dealer.OverrideStockImage)
                {
                    vehicle.CarImageUrl = String.IsNullOrEmpty(dealer.DefaultStockImageUrl)
                                              ? ""
                                              : dealer.DefaultStockImageUrl;

                    vehicle.ThumbnailImageURL = String.IsNullOrEmpty(dealer.DefaultStockImageUrl)
                                                    ? ""
                                                    : dealer.DefaultStockImageUrl;
                }
                else
                {
                    vehicle.CarImageUrl = String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                                              ? ""
                                              : appraisal.DefaultImageUrl;

                    vehicle.ThumbnailImageURL = String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                                                    ? ""
                                                    : appraisal.DefaultImageUrl;
                }


                context.AddTovincontrolwholesaleinventories(vehicle);

                context.SaveChanges();




            }
        }

        public static void MarkSoldVehicle(whitmanenterprisedealershipinventory wdi, string userName, CustomeInfoModel customer)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var vehicle = new whitmanenterprisedealershipinventorysoldout
                    {

                        ModelYear = wdi.ModelYear,
                        Make = wdi.Make,
                        Model = wdi.Model,
                        Trim = wdi.Trim,
                        VINNumber = wdi.VINNumber,
                        StockNumber = wdi.StockNumber,
                        SalePrice = wdi.SalePrice,
                        MSRP = wdi.MSRP,
                        Mileage = wdi.Mileage,
                        ExteriorColor = wdi.ExteriorColor,
                        InteriorColor = wdi.InteriorColor,
                        InteriorSurface = wdi.InteriorSurface,
                        BodyType = wdi.BodyType,
                        Cylinders = wdi.Cylinders,
                        Liters = wdi.Liters,
                        EngineType = wdi.EngineType,
                        DriveTrain = wdi.DriveTrain,
                        FuelType = wdi.FuelType,
                        Tranmission = wdi.Tranmission,
                        Doors = wdi.Doors,
                        Certified = wdi.Certified,
                        CarsOptions = wdi.CarsOptions,
                        CarsPackages = wdi.CarsPackages,
                        Descriptions = wdi.Descriptions,
                        CarImageUrl = wdi.CarImageUrl,
                        ThumbnailImageURL = wdi.ThumbnailImageURL,
                        DateInStock = wdi.DateInStock,
                        LastUpdated = DateTime.Now,
                        DealershipName = wdi.DealershipName,
                        DealershipAddress = wdi.DealershipAddress,
                        DealershipCity = wdi.DealershipCity,
                        DealershipState = wdi.DealershipState,
                        DealershipPhone = wdi.DealershipPhone,
                        DealershipId = wdi.DealershipId,
                        DefaultImageUrl = wdi.DefaultImageUrl,
                        NewUsed = wdi.NewUsed,
                        AddToInventoryBy = wdi.AddToInventoryBy,
                        RemoveBy = userName,
                        AppraisalID = wdi.AppraisalID,
                        DataFeed = customer.DeleteImmediately,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Address = customer.Address,
                        City = customer.City,
                        State = customer.State,
                        ZipCode = customer.ZipCode,
                        Country = customer.Country,
                        DateRemoved = DateTime.Now,
                        ACV = wdi.ACV,
                        BrandedTitle = wdi.BrandedTitle,
                        
                        DealerCost = wdi.DealerCost,
                        FuelEconomyCity = wdi.FuelEconomyCity,
                        FuelEconomyHighWay = wdi.FuelEconomyHighWay,
                        StandardOptions = wdi.StandardOptions,
                        WarrantyInfo = wdi.WarrantyInfo,
                        RetailPrice = wdi.RetailPrice,
                        DealerDiscount = wdi.DealerDiscount,
                        ManufacturerRebate = wdi.ManufacturerRebate,
                        WindowStickerPrice = wdi.WindowStickerPrice,
                        DealershipZipCode = wdi.DealershipZipCode,
                        CarFaxOwner = wdi.CarFaxOwner,
                        Recon = wdi.Recon,
                        KBBOptionsId = wdi.KBBOptionsId,
                        KBBTrimId = wdi.KBBTrimId,
                        PriorRental = wdi.PriorRental,
                        DealerDemo = wdi.DealerDemo,
                        Unwind = wdi.Unwind,
                        TruckCategory = wdi.TruckCategory,
                        TruckClass = wdi.TruckClass,
                        TruckType = wdi.TruckType,
                        Disclaimer = wdi.Disclaimer,
                        AdditionalTitle = wdi.AdditionalTitle,
                        ChromeModelId = wdi.ChromeModelId,
                        ChromeStyleId = wdi.ChromeStyleId,
                        ColorCode = wdi.ColorCode,
                        Loaner = wdi.Loaner,
                        Auction = wdi.Auction,
                        BucketJumpCompleteDay = wdi.BucketJumpCompleteDay,
                        ACar = wdi.ACar,
                        EnableAutoDescription = wdi.EnableAutoDescription,
                        OldListingId = wdi.ListingID,
                        ManheimTrimId = wdi.ManheimTrimId,
                        MarketTrim = wdi.MarketTrim,
                        PackageDescriptions = wdi.PackageDescriptions,
                        Template = wdi.Template,
                        IsFeatured = wdi.IsFeatured,
                        VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType,



                    };

                vehicle.TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory;
                vehicle.TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass;
                vehicle.TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType;

                var removeVehicle = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == wdi.ListingID);

                context.AddTowhitmanenterprisedealershipinventorysoldouts(vehicle);

                context.Attach(removeVehicle);

                context.DeleteObject(removeVehicle);

                context.SaveChanges();




            }



        }

        public static void UpdateNotification(bool notify, int dealershipID, int notificationkind)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var setting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealershipID);

                var usersetting = context.whitmanenterpriseusersnotifications.Where(x => x.DealershipId == dealershipID);

                if (notificationkind == 0)
                {
                    setting.AppraisalNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.AppraisalNotification = notify;
                        }
                    }
                 
                }
                else if (notificationkind == 1)
                {
                    setting.WholeNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.WholeNotification = notify;
                        }
                    }
                }
                else if (notificationkind == 2)
                {
                    setting.InventoryNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.InventoryNotification = notify;
                        }
                    }
                }
                else if (notificationkind == 3)
                {
                    setting.C24Hnotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.C24Hnotification = notify;
                        }
                    }
                }
                else if (notificationkind == 4)
                {
                    setting.NoteNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.NoteNotification = notify;
                        }
                    }
                }
                else if (notificationkind == 5)
                {
                    setting.PriceChangeNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.PriceChangeNotification = notify;
                        }
                    }
                }
                else if (notificationkind == 6)
                {
                    setting.AgeingBucketNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.AgingNotification = notify;
                        }
                    }
                }
                else if (notificationkind == 7)
                {
                    setting.MarketPriceRangeChangeNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.MarketPriceRangeNotification = notify;
                        }
                    }
                }
                else if (notificationkind == 8)
                {
                    setting.BucketJumpNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.BucketJumpNotification = notify;
                        }
                    }
                }
                else if (notificationkind == 9)
                {
                    setting.ImageUploadNotification = notify;
                    if (notify == false)
                    {
                        foreach (var tmp in usersetting)
                        {
                            tmp.ImageUploadNotification = notify;
                        }
                    }
                   
                }

                context.SaveChanges();


            }


        }

        public static void UpdateWindowStickerNotification(bool notify, int dealershipID, int notificationkind)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var setting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealershipID);

                if (notificationkind == 1)
                    setting.RetailPrice = notify;
                else if (notificationkind == 2)
                    setting.DealerDiscount = notify;
                else if (notificationkind == 3)
                    setting.ManufacturerReabte = notify;
                else if (notificationkind == 4)
                    setting.SalePrice = notify;


                context.SaveChanges();


            }


        }

        public static int MarkUnsoldVehicle(whitmanenterprisedealershipinventorysoldout wdi)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {

                var vehicle = new whitmanenterprisedealershipinventory
                    {

                        ModelYear = wdi.ModelYear,
                        Make = wdi.Make,
                        Model = wdi.Model,
                        Trim = wdi.Trim,
                        VINNumber = wdi.VINNumber,
                        StockNumber = wdi.StockNumber,
                        SalePrice = wdi.SalePrice,
                        MSRP = wdi.MSRP,
                        Mileage = wdi.Mileage,
                        ExteriorColor = wdi.ExteriorColor,
                        InteriorColor = wdi.InteriorColor,
                        InteriorSurface = wdi.InteriorSurface,
                        BodyType = wdi.BodyType,
                        Cylinders = wdi.Cylinders,
                        Liters = wdi.Liters,
                        EngineType = wdi.EngineType,
                        DriveTrain = wdi.DriveTrain,
                        FuelType = wdi.FuelType,
                        Tranmission = wdi.Tranmission,
                        Doors = wdi.Doors,
                        Certified = wdi.Certified,
                        CarsOptions = wdi.CarsOptions,
                        CarsPackages = wdi.CarsPackages,
                        StandardOptions = wdi.StandardOptions,
                        Descriptions = wdi.Descriptions,
                        CarImageUrl = wdi.CarImageUrl,
                        ThumbnailImageURL = wdi.ThumbnailImageURL,
                        DateInStock = wdi.DateInStock,
                        LastUpdated = DateTime.Now,
                        DealershipName = wdi.DealershipName,
                        DealershipAddress = wdi.DealershipAddress,
                        DealershipCity = wdi.DealershipCity,
                        DealershipState = wdi.DealershipState,
                        DealershipPhone = wdi.DealershipPhone,
                        DealershipId = wdi.DealershipId,
                        DealershipZipCode = wdi.ZipCode,
                        DefaultImageUrl = wdi.DefaultImageUrl,
                        NewUsed = wdi.NewUsed,
                        AddToInventoryBy = wdi.AddToInventoryBy,
                        AppraisalID = wdi.AppraisalID,
                        ACV = wdi.ACV,
                        DealerCost = wdi.DealerCost,
                        FuelEconomyCity = wdi.FuelEconomyCity,
                        FuelEconomyHighWay = wdi.FuelEconomyHighWay,
                        WarrantyInfo = wdi.WarrantyInfo,
                        RetailPrice = wdi.RetailPrice,
                        DealerDiscount = wdi.DealerDiscount,
                        ManufacturerRebate = wdi.ManufacturerRebate,
                        WindowStickerPrice = wdi.WindowStickerPrice,
                        CarFaxOwner = wdi.CarFaxOwner,
                        PriorRental = wdi.PriorRental,
                        DealerDemo = wdi.DealerDemo,
                        Unwind = wdi.Unwind,
                        Recon = wdi.Recon,
                        KBBOptionsId = wdi.KBBOptionsId,
                        KBBTrimId = wdi.KBBTrimId,
                        Disclaimer = wdi.Disclaimer,
                        AdditionalTitle = wdi.AdditionalTitle,
                        ColorCode = wdi.ColorCode,
                        ChromeModelId = wdi.ChromeModelId,
                        ChromeStyleId = wdi.ChromeStyleId,
                        BucketJumpCompleteDay = wdi.BucketJumpCompleteDay,
                        ACar = wdi.ACar,
                        BrandedTitle = wdi.BrandedTitle,
                        EnableAutoDescription = wdi.EnableAutoDescription,

                        ManheimTrimId = wdi.ManheimTrimId,
                        MarketTrim = wdi.MarketTrim,
                        PackageDescriptions = wdi.PackageDescriptions,
                        Template = wdi.Template,
                        VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType,
                        TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory,
                        TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass,
                        TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType,

                    };

                var removeVehicle = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == wdi.ListingID);

                context.AddTowhitmanenterprisedealershipinventories(vehicle);

                context.Attach(removeVehicle);

                context.DeleteObject(removeVehicle);

                context.SaveChanges();

                return vehicle.ListingID;
            }



        }

        public static void UpdateNotificationPerUser(bool notify, int dealershipID, string UserName, int notificationkind)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var setting = context.whitmanenterpriseusersnotifications.FirstOrDefault(x => x.DealershipId == dealershipID && x.UserName == UserName);

                if (notificationkind == 0)
                    setting.AppraisalNotification = notify;
                else if (notificationkind == 1)
                    setting.WholeNotification = notify;
                else if (notificationkind == 2)
                    setting.InventoryNotification = notify;
                else if (notificationkind == 3)
                    setting.C24Hnotification = notify;
                else if (notificationkind == 4)
                    setting.NoteNotification = notify;
                else if (notificationkind == 5)
                    setting.PriceChangeNotification = notify;
                else if (notificationkind == 6)
                    setting.AgingNotification = notify;
                else if (notificationkind == 7)
                    setting.MarketPriceRangeNotification = notify;
                else if (notificationkind == 8)
                    setting.BucketJumpNotification = notify;
                else if (notificationkind == 9)
                    setting.ImageUploadNotification = notify;
                context.SaveChanges();


            }



        }

        public static int InsertToInvetory(int appraisalId, DealershipViewModel dealer, bool isRecon)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var appraisal = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == appraisalId);

                int carfaxOwner = 0;


                var firstOrDefault = context.whitmanenteprisecarfaxes.FirstOrDefault(x => x.Vin == appraisal.VINNumber);

                if (firstOrDefault != null)
                    carfaxOwner = firstOrDefault.Owner.GetValueOrDefault();



                var vehicle = new whitmanenterprisedealershipinventory()
                {
                    ModelYear = appraisal.ModelYear,
                    Make = String.IsNullOrEmpty(appraisal.Make) ? "" : appraisal.Make,
                    Model =
                        String.IsNullOrEmpty(appraisal.Model) ? "" : appraisal.Model.Trim(),
                    Trim = String.IsNullOrEmpty(appraisal.Trim) ? "" : appraisal.Trim.Trim(),
                    VINNumber = String.IsNullOrEmpty(appraisal.VINNumber) ? "" : appraisal.VINNumber,
                    StockNumber =
                        String.IsNullOrEmpty(appraisal.StockNumber) ? "" : appraisal.StockNumber,
                    SalePrice = String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    MSRP = String.IsNullOrEmpty(appraisal.MSRP) ? "" : CommonHelper.RemoveSpecialCharactersForPurePrice(appraisal.MSRP),
                    Mileage = String.IsNullOrEmpty(appraisal.Mileage) ? "" : appraisal.Mileage,
                    ExteriorColor =
                        String.IsNullOrEmpty(appraisal.ExteriorColor)
                            ? ""
                            : appraisal.ExteriorColor,
                    ColorCode = String.IsNullOrEmpty(appraisal.ColorCode)
                            ? ""
                            : appraisal.ColorCode,
                    InteriorColor =
                        String.IsNullOrEmpty(appraisal.InteriorColor)
                            ? ""
                            : appraisal.InteriorColor,
                    InteriorSurface =
                        String.IsNullOrEmpty(appraisal.InteriorSurface)
                            ? ""
                            : appraisal.InteriorSurface,
                    BodyType =
                        String.IsNullOrEmpty(appraisal.BodyType)
                            ? ""
                            : appraisal.BodyType,
                    Cylinders =
                        String.IsNullOrEmpty(appraisal.Cylinders)
                            ? ""
                            : appraisal.Cylinders,
                    Liters =
                        String.IsNullOrEmpty(appraisal.Liters) ? "" : appraisal.Liters,
                    EngineType =
                        String.IsNullOrEmpty(appraisal.EngineType) ? "" : appraisal.EngineType,
                    DriveTrain =
                        String.IsNullOrEmpty(appraisal.DriveTrain)
                            ? ""
                            : appraisal.DriveTrain,
                    FuelType =
                        String.IsNullOrEmpty(appraisal.FuelType) ? "" : appraisal.FuelType,
                    Tranmission =
                        String.IsNullOrEmpty(appraisal.Tranmission)
                            ? ""
                            : appraisal.Tranmission,
                    Doors = String.IsNullOrEmpty(appraisal.Doors) ? "" : appraisal.Doors,
                    Certified = false,
                    StandardOptions =
                        String.IsNullOrEmpty(appraisal.StandardOptions)
                            ? ""
                            : appraisal.StandardOptions.Replace("\'", "\\'"),
                    CarsOptions =
                        String.IsNullOrEmpty(appraisal.CarsOptions)
                            ? ""
                            : appraisal.CarsOptions.Replace("\'", "\\'"),
                    CarsPackages =
                        String.IsNullOrEmpty(appraisal.CarsPackages)
                            ? ""
                            : appraisal.CarsPackages.Replace("\'", "\\'"),
                    Descriptions =
                        String.IsNullOrEmpty(appraisal.Descriptions)
                            ? ""
                            : appraisal.Descriptions.Replace("\'", "\\'"),
              
                    DateInStock = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    DealershipName = dealer.DealershipName,

                    DealershipAddress = dealer.DealershipAddress,

                    DealershipCity =
                       dealer.City,
                    DealershipState =
                        dealer.State,
                    DealershipPhone =
                       dealer.DealershipPhoneNumber,
                    DealershipZipCode =
                       dealer.ZipCode,
                    DealershipId =
                        dealer.DealershipId,
                    DefaultImageUrl =
                        String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                            ? ""
                            : appraisal.DefaultImageUrl,
                    NewUsed = "Used",
                    AddToInventoryBy =
                        String.IsNullOrEmpty(appraisal.AppraisalBy)
                            ? ""
                            : appraisal.AppraisalBy,
                    AppraisalID =
                        String.IsNullOrEmpty(appraisal.AppraisalID)
                            ? ""
                            : appraisal.AppraisalID,
                    ACV = appraisal.ACV,
                    DealerCost = appraisal.DealerCost,
                    FuelEconomyHighWay = appraisal.FuelEconomyHighWay,
                    FuelEconomyCity = appraisal.FuelEconomyCity,
                    PriorRental = false,
                    DealerDemo = false,
                    Unwind = false,
                    WindowStickerPrice =
                        String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    RetailPrice = String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    KBBOptionsId = String.IsNullOrEmpty(appraisal.KBBOptionsId) ? "" : appraisal.KBBOptionsId,
                    DealerDiscount = "0",
                    ManufacturerRebate = "0",
                    MarketRange = 0,
                    Recon = isRecon,
                    CarFaxOwner = carfaxOwner,
                    ChromeStyleId = appraisal.ChromeStyleId,
                    ChromeModelId = appraisal.ChromeModelId,
                    EnableAutoDescription = dealer.EnableAutoDescription,
                    PackageDescriptions = appraisal.PackageDescriptions,
                    AdditionalTitle = appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.Model + " " + appraisal.Trim,
                };

                vehicle.TruckCategory = appraisal.TruckCategory;
                vehicle.TruckClass = appraisal.TruckClass;
                vehicle.TruckType = appraisal.TruckType;
                vehicle.VehicleType = appraisal.VehicleType;


                if (dealer.OverrideStockImage)
                {
                    vehicle.CarImageUrl = String.IsNullOrEmpty(dealer.DefaultStockImageUrl)
                                              ? ""
                                              : dealer.DefaultStockImageUrl;

                    vehicle.ThumbnailImageURL = String.IsNullOrEmpty(dealer.DefaultStockImageUrl)
                                                    ? ""
                                                    : dealer.DefaultStockImageUrl;
                }
                else
                {
                    vehicle.CarImageUrl = String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                                              ? ""
                                              : appraisal.DefaultImageUrl;

                    vehicle.ThumbnailImageURL = String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                                                    ? ""
                                                    : appraisal.DefaultImageUrl;
                }

                context.AddTowhitmanenterprisedealershipinventories(vehicle);

                if (
                    context.whitmanenterpriseappraisals.Any(
                        x => x.AppraisalID == appraisal.AppraisalID && x.DealershipId == appraisal.DealershipId))
                {
                    var removeAppraisal =
                        context.whitmanenterpriseappraisals.FirstOrDefault(
                            x =>
                            x.AppraisalID == appraisal.AppraisalID && x.DealershipId == appraisal.DealershipId);

                    removeAppraisal.InventoryAdd = true;
                }


                context.SaveChanges();

                LinqHelper.AddNewActivity(SessionHandler.Dealership.DealershipId,
                        "Add To Inventory " + appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.Model + " " + appraisal.Trim,
                        String.Format("Appraisal Id: {0};Listing Id: {1}; Year: {2};Make: {3};Model: {4};Trim: {5};Milage: {6};MSRP: {7}", appraisal.idAppraisal, vehicle.ListingID, appraisal.ModelYear, appraisal.Make, appraisal.Model, appraisal.Trim, appraisal.Mileage, Convert.ToDecimal(appraisal.MSRP).ToString("c0")),
                        Constanst.ActivityType.AddToInventory);

                return vehicle.ListingID;

            }
        }

        public static int InsertToWholeSale(int AppraisalId, DealershipViewModel dealer)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var appraisal = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == AppraisalId);



                int carfaxOwner = 0;


                var firstOrDefault = context.whitmanenteprisecarfaxes.FirstOrDefault(x => x.Vin == appraisal.VINNumber);
                if (firstOrDefault != null)
                    carfaxOwner = firstOrDefault.Owner.GetValueOrDefault();


                var vehicle = new vincontrolwholesaleinventory()
                {

                    ModelYear = appraisal.ModelYear,
                    Make = String.IsNullOrEmpty(appraisal.Make) ? "" : appraisal.Make,
                    Model =
                        String.IsNullOrEmpty(appraisal.Model) ? "" : appraisal.Model,
                    Trim = String.IsNullOrEmpty(appraisal.Trim) ? "" : appraisal.Trim,
                    VINNumber = String.IsNullOrEmpty(appraisal.VINNumber) ? "" : appraisal.VINNumber,
                    StockNumber =
                        String.IsNullOrEmpty(appraisal.StockNumber) ? "" : appraisal.StockNumber,
                    SalePrice = String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    MSRP = String.IsNullOrEmpty(appraisal.MSRP) ? "" : CommonHelper.RemoveSpecialCharactersForPurePrice(appraisal.MSRP),
                    Mileage = String.IsNullOrEmpty(appraisal.Mileage) ? "" : appraisal.Mileage,
                    ExteriorColor =
                        String.IsNullOrEmpty(appraisal.ExteriorColor)
                            ? ""
                            : appraisal.ExteriorColor,
                    ColorCode =
                      String.IsNullOrEmpty(appraisal.ColorCode)
                          ? ""
                          : appraisal.ColorCode,
                    InteriorColor =
                        String.IsNullOrEmpty(appraisal.InteriorColor)
                            ? ""
                            : appraisal.InteriorColor,
                    InteriorSurface =
                        String.IsNullOrEmpty(appraisal.InteriorSurface)
                            ? ""
                            : appraisal.InteriorSurface,
                    BodyType =
                        String.IsNullOrEmpty(appraisal.BodyType)
                            ? ""
                            : appraisal.BodyType,
                    Cylinders =
                        String.IsNullOrEmpty(appraisal.Cylinders)
                            ? ""
                            : appraisal.Cylinders,
                    Liters =
                        String.IsNullOrEmpty(appraisal.Liters) ? "" : appraisal.Liters,
                    EngineType =
                        String.IsNullOrEmpty(appraisal.EngineType) ? "" : appraisal.EngineType,
                    DriveTrain =
                        String.IsNullOrEmpty(appraisal.DriveTrain)
                            ? ""
                            : appraisal.DriveTrain,
                    FuelType =
                        String.IsNullOrEmpty(appraisal.FuelType) ? "" : appraisal.FuelType,
                    Tranmission =
                        String.IsNullOrEmpty(appraisal.Tranmission)
                            ? ""
                            : appraisal.Tranmission,
                    Doors = String.IsNullOrEmpty(appraisal.Doors) ? "" : appraisal.Doors,
                    Certified = false,
                    StandardOptions =
                        String.IsNullOrEmpty(appraisal.StandardOptions)
                            ? ""
                            : appraisal.StandardOptions.Replace("\'", "\\'"),
                    CarsOptions =
                        String.IsNullOrEmpty(appraisal.CarsOptions)
                            ? ""
                            : appraisal.CarsOptions.Replace("\'", "\\'"),
                    CarsPackages =
                        String.IsNullOrEmpty(appraisal.CarsPackages)
                            ? ""
                            : appraisal.CarsPackages.Replace("\'", "\\'"),
                    Descriptions =
                        String.IsNullOrEmpty(appraisal.Descriptions)
                            ? ""
                            : appraisal.Descriptions.Replace("\'", "\\'"),
                    CarImageUrl =
                        String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                            ? ""
                            : appraisal.DefaultImageUrl,
                    ThumbnailImageURL =
                        String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                            ? ""
                            : appraisal.DefaultImageUrl,
                    DateInStock = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    DealershipName = dealer.DealershipName,

                    DealershipAddress = dealer.DealershipAddress,

                    DealershipCity =
                       dealer.City,
                    DealershipState =
                        dealer.State,
                    DealershipPhone =
                       dealer.DealershipPhoneNumber,
                    DealershipZipCode =
                       dealer.ZipCode,
                    DealershipId =
                        dealer.DealershipId,
                    DefaultImageUrl =
                        String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                            ? ""
                            : appraisal.DefaultImageUrl,
                    NewUsed = "Used",
                    AddToInventoryBy =
                        String.IsNullOrEmpty(appraisal.AppraisalBy)
                            ? ""
                            : appraisal.AppraisalBy,
                    AppraisalID =
                        String.IsNullOrEmpty(appraisal.AppraisalID)
                            ? ""
                            : appraisal.AppraisalID,
                    ACV = appraisal.ACV,
                    DealerCost = appraisal.DealerCost,
                    FuelEconomyHighWay = appraisal.FuelEconomyHighWay,
                    FuelEconomyCity = appraisal.FuelEconomyCity,
                    PriorRental = false,
                    WindowStickerPrice =
                        String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    RetailPrice = String.IsNullOrEmpty(appraisal.SalePrice) ? "" : appraisal.SalePrice,
                    KBBOptionsId = String.IsNullOrEmpty(appraisal.KBBOptionsId) ? "" : appraisal.KBBOptionsId,
                    DealerDiscount = "0",
                    ManufacturerRebate = "0",
                    MarketRange = 0,
                    Recon = false,
                    CarFaxOwner = carfaxOwner,
                    ChromeStyleId = appraisal.ChromeStyleId


                };

                vehicle.TruckCategory = appraisal.TruckCategory;
                vehicle.TruckClass = appraisal.TruckClass;
                vehicle.TruckType = appraisal.TruckType;
                vehicle.VehicleType = appraisal.VehicleType;


                if (dealer.OverrideStockImage)
                {
                    vehicle.CarImageUrl = String.IsNullOrEmpty(dealer.DefaultStockImageUrl)
                                              ? ""
                                              : dealer.DefaultStockImageUrl;

                    vehicle.ThumbnailImageURL = String.IsNullOrEmpty(dealer.DefaultStockImageUrl)
                                                    ? ""
                                                    : dealer.DefaultStockImageUrl;
                }
                else
                {
                    vehicle.CarImageUrl = String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                                              ? ""
                                              : appraisal.DefaultImageUrl;

                    vehicle.ThumbnailImageURL = String.IsNullOrEmpty(appraisal.DefaultImageUrl)
                                                    ? ""
                                                    : appraisal.DefaultImageUrl;
                }
             


                context.AddTovincontrolwholesaleinventories(vehicle);

                if (
                    context.whitmanenterpriseappraisals.Any(
                        x => x.AppraisalID == appraisal.AppraisalID && x.DealershipId == appraisal.DealershipId))
                {
                    var removeAppraisal =
                        context.whitmanenterpriseappraisals.FirstOrDefault(
                            x =>
                            x.AppraisalID == appraisal.AppraisalID && x.DealershipId == appraisal.DealershipId);

                    removeAppraisal.InventoryAdd = true;
                }


                context.SaveChanges();

                return vehicle.ListingID;

            }
        }

        public static void SaveExistAppraisalToDatabase(AppraisalViewFormModel appraisal)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.AppraisalID == appraisal.AppraisalGenerateId && x.DealershipId == appraisal.DealershipId);


                searchResult.FirstName = String.IsNullOrEmpty(appraisal.CustomerFirstName) ? "" : appraisal.CustomerFirstName;

                searchResult.LastName = String.IsNullOrEmpty(appraisal.CustomerLastName) ? "" : appraisal.CustomerLastName;

                searchResult.Address = String.IsNullOrEmpty(appraisal.CustomerAddress) ? "" : appraisal.CustomerAddress;

                searchResult.City = String.IsNullOrEmpty(appraisal.CustomerCity) ? "" : appraisal.CustomerCity;

                searchResult.State = String.IsNullOrEmpty(appraisal.CustomerState) ? "" : appraisal.CustomerState;

                searchResult.ZipCode = String.IsNullOrEmpty(appraisal.CustomerZipCode) ? "" : appraisal.CustomerZipCode;

                searchResult.ACV = String.IsNullOrEmpty(appraisal.ACV) ? "" : CommonHelper.RemoveSpecialCharactersForPurePrice(appraisal.ACV);


                context.SaveChanges();

            }


        }

        public static whitmanenterpriseappraisal InsertAppraisalToDatabase(AppraisalViewFormModel appraisal, DealershipViewModel dealer)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var app = new whitmanenterpriseappraisal()
                    {
                        idAppraisal = context.whitmanenterpriseappraisals.Max(x => x.idAppraisal) + 1,
                        ModelYear = appraisal.ModelYear,
                        Make = String.IsNullOrEmpty(appraisal.Make) ? "" : appraisal.Make,
                        Model = String.IsNullOrEmpty(appraisal.AppraisalModel) ? "" : appraisal.AppraisalModel,
                        Trim = GetTrim(appraisal),
                        VINNumber = String.IsNullOrEmpty(appraisal.VinNumber) ? "" : appraisal.VinNumber,
                        StockNumber = String.IsNullOrEmpty(appraisal.StockNumber) ? "" : appraisal.StockNumber,
                        MSRP = String.IsNullOrEmpty(appraisal.MSRP) ? "" : appraisal.MSRP,
                        Mileage =
                            String.IsNullOrEmpty(appraisal.Mileage)
                                ? ""
                                : CommonHelper.RemoveSpecialCharactersForMsrp(appraisal.Mileage),
                        ExteriorColor =
                            String.IsNullOrEmpty(appraisal.SelectedExteriorColorValue)
                                ? ""
                                : appraisal.SelectedExteriorColorValue,
                        ColorCode =
                            String.IsNullOrEmpty(appraisal.SelectedExteriorColorCode)
                                ? ""
                                : appraisal.SelectedExteriorColorCode,
                        InteriorColor =
                            String.IsNullOrEmpty(appraisal.SelectedInteriorColor) ? "" : appraisal.SelectedInteriorColor,
                        InteriorSurface =
                            String.IsNullOrEmpty(appraisal.InteriorSurface) ? "" : appraisal.InteriorSurface,
                        BodyType = String.IsNullOrEmpty(appraisal.SelectedBodyType) ? "" : appraisal.SelectedBodyType,
                        Cylinders = String.IsNullOrEmpty(appraisal.SelectedCylinder) ? "" : appraisal.SelectedCylinder,
                        Liters = String.IsNullOrEmpty(appraisal.SelectedLiters) ? "" : appraisal.SelectedLiters,
                        EngineType = String.IsNullOrEmpty(appraisal.EngineType) ? "" : appraisal.EngineType,
                        DriveTrain =
                            String.IsNullOrEmpty(appraisal.SelectedDriveTrain) ? "" : appraisal.SelectedDriveTrain,
                        FuelType = String.IsNullOrEmpty(appraisal.SelectedFuel) ? "" : appraisal.SelectedFuel,
                        Tranmission =
                            String.IsNullOrEmpty(appraisal.SelectedTranmission) ? "" : appraisal.SelectedTranmission,
                        Doors = String.IsNullOrEmpty(appraisal.Door) ? "" : appraisal.Door,
                        CarImageUrl = String.IsNullOrEmpty(appraisal.DefaultImageUrl) ? "" : appraisal.DefaultImageUrl,
                        Certified = false,
                        StandardOptions =
                            String.IsNullOrEmpty(appraisal.StandardInstalledOption)
                                ? ""
                                : appraisal.StandardInstalledOption.Replace("\'", "\\'"),
                        CarsOptions =
                            String.IsNullOrEmpty(appraisal.SelectedFactoryOptions)
                                ? ""
                                : appraisal.SelectedFactoryOptions.Replace("\'", "\\'"),
                        CarsPackages =
                            String.IsNullOrEmpty(appraisal.SelectedPackageOptions)
                                ? ""
                                : appraisal.SelectedPackageOptions.Replace("\'", "\\'"),
                        Descriptions =
                            String.IsNullOrEmpty(appraisal.Descriptions)
                                ? ""
                                : appraisal.Descriptions.Replace("\'", "\\'"),
                        AppraisalDate = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        DealershipName = dealer.DealershipName,
                        DealershipAddress = dealer.DealershipAddress,
                        DealershipCity = dealer.City,
                        DealershipState = dealer.State,
                        DealershipPhone = dealer.Phone,
                        DealershipId = dealer.DealershipId,
                        DefaultImageUrl =
                            String.IsNullOrEmpty(appraisal.DefaultImageUrl) ? "" : appraisal.DefaultImageUrl,
                        AppraisalID =
                            String.IsNullOrEmpty(appraisal.AppraisalGenerateId) ? "" : appraisal.AppraisalGenerateId,
                        ACV = appraisal.ACV,
                        DealerCost = appraisal.DealerCost,
                        FirstName = String.IsNullOrEmpty(appraisal.CustomerFirstName) ? "" : appraisal.CustomerFirstName,
                        LastName = String.IsNullOrEmpty(appraisal.CustomerLastName) ? "" : appraisal.CustomerLastName,
                        Address = String.IsNullOrEmpty(appraisal.CustomerAddress) ? "" : appraisal.CustomerAddress,
                        City = String.IsNullOrEmpty(appraisal.CustomerCity) ? "" : appraisal.CustomerCity,
                        State = String.IsNullOrEmpty(appraisal.CustomerState) ? "" : appraisal.CustomerState,
                        ZipCode = String.IsNullOrEmpty(appraisal.CustomerZipCode) ? "" : appraisal.CustomerZipCode,
                        AppraisalBy = String.IsNullOrEmpty(appraisal.AppraisalBy) ? "" : appraisal.AppraisalBy,
                        AppraisalType = String.IsNullOrEmpty(appraisal.AppraisalType) ? "" : appraisal.AppraisalType,
                        ChromeModelId = String.IsNullOrEmpty(appraisal.ChromeModelId) ? "" : appraisal.ChromeModelId,
                        ChromeStyleId = String.IsNullOrEmpty(appraisal.ChromeStyleId) ? "" : appraisal.ChromeStyleId,
                        FuelEconomyCity =
                            String.IsNullOrEmpty(appraisal.FuelEconomyCity) ? "" : appraisal.FuelEconomyCity,
                        FuelEconomyHighWay =
                            String.IsNullOrEmpty(appraisal.FuelEconomyHighWay) ? "" : appraisal.FuelEconomyHighWay,
                        InventoryAdd = false,
                        Carfaxowner =
                            (appraisal.CarFax == null || String.IsNullOrEmpty(appraisal.CarFax.NumberofOwners))
                                ? -1
                                : Convert.ToInt32(appraisal.CarFax.NumberofOwners),
                        PackageDescriptions = appraisal.SelectedPackagesDescription,
                        Location = appraisal.Location


                    };

                if (appraisal.IsTruck)
                {
                    app.TruckCategory = appraisal.SelectedTruckCategory;
                    app.TruckClass = appraisal.SelectedTruckClass;
                    app.TruckType = appraisal.SelectedTruckType;
                    app.VehicleType = "Truck";
                }
                else
                    app.VehicleType = "Car";

                context.AddTowhitmanenterpriseappraisals(app);

                context.SaveChanges();
                
                LinqHelper.AddNewActivity(dealer.DealershipId,
                                          String.Format("New Apprasial {0} {1} {2} {3}", appraisal.ModelYear,
                                                        appraisal.Make, appraisal.AppraisalModel, app.Trim),
                                          String.Format(
                                              "Appraisal Id: {0};Vin: {1}; Year: {2};Make: {3};Model: {4};Trim: {5};Milage: {6};MSRP: {7}",
                                              app.idAppraisal, appraisal.VinNumber, appraisal.ModelYear, appraisal.Make,
                                              appraisal.AppraisalModel, app.Trim, appraisal.Mileage,
                                              Convert.ToDecimal(appraisal.MSRP).ToString("c0")),
                                          Constanst.ActivityType.NewAppraisal);

                return app;
            }
        }

        private static string GetTrim(AppraisalViewFormModel appraisal)
        {
            return String.IsNullOrEmpty(appraisal.SelectedTrim) ? "" : appraisal.SelectedTrim;
        }

        public static string GetEbayCategoryId(string make, string model)
        {
            string defaultId = "6472";
           
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchMakeResult = context.whitmanenterpriseebayvehiclecategories.Where(x => x.Make == make);
                foreach (var tmp in searchMakeResult)
                {
                    string ebayModel = CommonHelper.RemoveSpecialCharacters(tmp.Model).ToLowerInvariant();
                    string chromeModel = CommonHelper.RemoveSpecialCharacters(model).ToLowerInvariant();

                    if (chromeModel.Equals(ebayModel.Trim()) || chromeModel.Contains(ebayModel.Trim()) || ebayModel.Contains(chromeModel.Trim()))
                    {
                        defaultId = tmp.EbayCategoryID.ToString(CultureInfo.InvariantCulture);
                        break;
                    }
                }

            }
            return defaultId;


        }

        public static void InsertOrUpdateEbayAd(string listingId, PostEbayAds ebayAd,int dealerId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int lid = Convert.ToInt32(listingId);
                
                if (context.whitmanenterprisebayads.Any(o => o.ListingId == lid))
                {
                    var ebay = context.whitmanenterprisebayads.FirstOrDefault(x => x.ListingId == lid);

                    ebay.EbayAdId = ebayAd.ebayAdID;

                    ebay.EbayAdURL = ebayAd.ebayAdURL;

                    ebay.EbayAdStartTime = ebayAd.ebayAdStartTime;

                    ebay.EbayAdEndTime = ebayAd.ebayAdEndTime;

                    context.SaveChanges();
                }
                else
                {
                    var ebay = new whitmanenterprisebayad()
                    {
                        ListingId = lid,

                        EbayAdId = ebayAd.ebayAdID,

                        EbayAdURL = ebayAd.ebayAdURL,

                        EbayAdStartTime = ebayAd.ebayAdStartTime,

                        EbayAdEndTime = ebayAd.ebayAdEndTime,

                        DealerId = dealerId

                        
                    };

                    context.AddTowhitmanenterprisebayads(ebay);

                    context.SaveChanges();
                }

            }



        }

        public static void TransferVehicle(int ListingId, int transferDealerShipId, string newStockNumber,
                                           DealerGroupViewModel dealerGroup)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult =
                    context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                searchResult.DealershipId = transferDealerShipId;

                searchResult.DealershipName =
                    dealerGroup.DealerList.First(x => x.DealershipId == transferDealerShipId).DealershipName;

                searchResult.DealershipAddress =
                    dealerGroup.DealerList.First(x => x.DealershipId == transferDealerShipId).DealershipAddress;

                searchResult.DealershipCity =
                    dealerGroup.DealerList.First(x => x.DealershipId == transferDealerShipId).City;

                searchResult.DealershipState =
                    dealerGroup.DealerList.First(x => x.DealershipId == transferDealerShipId).State;

                searchResult.DealershipZipCode =
                    dealerGroup.DealerList.First(x => x.DealershipId == transferDealerShipId).ZipCode;


                searchResult.DealershipPhone =
                    dealerGroup.DealerList.First(x => x.DealershipId == transferDealerShipId).Phone;


                searchResult.StockNumber = newStockNumber;

                searchResult.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }

        }

        public static bool CheckStockNumberExist(string stockNumber, int dealershipId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisedealershipinventories.Any(o => o.StockNumber == stockNumber && o.DealershipId == dealershipId))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<AppraisalViewFormModel> GetListOfAppraisal(int dealerId)
        {
            var fullList = new List<AppraisalViewFormModel>();
            var context = new whitmanenterprisewarehouseEntities();
            DateTime dtPrevious60Days = DateTime.Now.AddDays(-60);

            var result = context.whitmanenterpriseappraisals.Where(x => x.DealershipId == dealerId && (x.Status == null || x.Status != "Pending") && x.InventoryAdd == false && x.AppraisalDate >= dtPrevious60Days).OrderByDescending(x => x.AppraisalDate);

            foreach (var app in result)
            {
                var appraisalTmp = new AppraisalViewFormModel
                                       {
                                           AppraisalID = app.idAppraisal,
                                           Make = app.Make,
                                           ModelYear = app.ModelYear.GetValueOrDefault(),
                                           AppraisalModel = app.Model,
                                           VinNumber = app.VINNumber,
                                           ACV = app.ACV,
                                           CarImagesUrl = app.CarImageUrl,
                                           DefaultImageUrl = app.DefaultImageUrl,
                                           AppraisalDate = app.AppraisalDate.GetValueOrDefault().ToShortDateString(),
                                           AppraisalGenerateId = app.AppraisalID
                                       };

                fullList.Add(appraisalTmp);
            }

            return fullList;

        }

        public static List<string> GetListOfTruckCategoryByTruckType(string truckType)
        {

            var context = new whitmanenterprisewarehouseEntities();

            var returnList = new List<string>();

            if (context.vincontroltruckcategories.Any(x => x.TruckType == truckType))
            {
                var result = context.vincontroltruckcategories.Where(x => x.TruckType == truckType);

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
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var customerTradeIn = new vincontrolbannercustomer()
                {
                    Condition = vehicle.Condition,
                    CreatedDate = DateTime.Now,
                    DealerId = vehicle.DealerId,
                    Email = vehicle.CustomerEmail,
                    FirstName = vehicle.CustomerFirstName,
                    LastName = vehicle.CustomerLastName,
                    Phone = CommonHelper.RemoveSpecialCharactersForPurePrice(vehicle.CustomerPhone),
                    KBBVehicelId = vehicle.VehicleId,
                    Year = Convert.ToInt32(vehicle.SelectedYear),
                    Make = vehicle.SelectedMakeValue,
                    Model = vehicle.SelectedModelValue,
                    Trim = vehicle.SelectedTrimValue,
                    Mileage = vehicle.MileageNumber,
                    TradeInFairValue = CommonHelper.RemoveSpecialCharactersForPurePrice(vehicle.TradeInFairPrice),
                    TradeInMaxValue = CommonHelper.RemoveSpecialCharactersForPurePrice(vehicle.TradeInGoodPrice),
                    Vin = vehicle.Vin,
                    SelectedOptions = vehicle.SelectedOptionList,
                    EmailContent = vehicle.EmailTextContent,
                    ADFEmailContent = vehicle.EmailADFContent,
                    Receivers = GetReceivers(vehicle.Receivers)
                };

                context.AddTovincontrolbannercustomers(customerTradeIn);
                context.SaveChanges();

                return customerTradeIn.TradeInCustomerId;
            }
        }

        private static string GetReceivers(List<string> receivers)
        {
            StringBuilder result = new StringBuilder();
            foreach (string receiver in receivers)
            {
                result.Append(receiver + ",");
            }
            if (result.Length > 0)
            {
                result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }

        public static void UpdateRebateAmount(string rebateAmount, int rebateId)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.vincontrolrebates.First(x => x.vincontrolrebateid == rebateId);

                var result =
                      context.whitmanenterprisedealershipinventories.Where(
                          x => x.DealershipId == searchResult.DealerId && x.NewUsed == "New" && x.ModelYear == searchResult.Year && x.Make == searchResult.Make && x.Model == searchResult.Model && x.Trim == searchResult.Trim);

                int rebateAmountnumber = 0;
                Int32.TryParse(rebateAmount, out rebateAmountnumber);

                foreach (var tmp in result)
                {
                    int salePriceNumber = 0;
                    Int32.TryParse(tmp.SalePrice, out salePriceNumber);

                    tmp.ManufacturerRebate = CommonHelper.RemoveSpecialCharactersForMsrp(rebateAmount);
                    tmp.RetailPrice = (salePriceNumber + rebateAmountnumber).ToString(CultureInfo.InvariantCulture);
                }


                searchResult.ManufactureReabte = rebateAmount;

                context.SaveChanges();
            }

        }

        public static void UpdateRebateDisclaimer(string rebateDisclaimer, int rebateId)
        {

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var searchResult = context.vincontrolrebates.First(x => x.vincontrolrebateid == rebateId);

                var result =
                     context.whitmanenterprisedealershipinventories.Where(
                         x => x.DealershipId == searchResult.DealerId && x.NewUsed == "New" && x.ModelYear == searchResult.Year && x.Make == searchResult.Make && x.Model == searchResult.Model && x.Trim == searchResult.Trim);

                foreach (var tmp in result)
                {
                    tmp.Disclaimer = rebateDisclaimer;
                }

                searchResult.Disclaimer = rebateDisclaimer;

                context.SaveChanges();
            }

        }

        public static CarImage GetImageURLs(int listingId)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var row =
                context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == listingId);
            if (row == null)
            {
                return null;
            }
            else
            {
                return new CarImage()
                {
                    ImageURLs = row.CarImageUrl,
                    ThumnailURLs = row.ThumbnailImageURL
                };
            }

        }

        public static void ReplaceCarImageURL(ImageViewModel image)
        {
            if (image.InventoryStatus == 1)
            {
                //if (image.ImageUploadFiles.Contains(":80"))
                //{
                //    image.ImageUploadFiles = image.ImageUploadFiles.Replace(":8082", "");
                //    image.ImageUploadFiles = image.ImageUploadFiles.Replace(":80", "");
                //    image.ThumbnailImageUploadFiles = image.ThumbnailImageUploadFiles.Replace(":8082", "");
                //    image.ThumbnailImageUploadFiles = image.ThumbnailImageUploadFiles.Replace(":80", "");
                //}
                SQLHelper.UpdateCarImageURL(image.ListingId, image.ImageUploadFiles, image.ThumbnailImageUploadFiles);
            }
            else if (image.InventoryStatus == -1)
            {
                //if (image.ImageUploadFiles.Contains(":80"))
                //{
                //    image.ImageUploadFiles = image.ImageUploadFiles.Replace(":8082", "");
                //    image.ImageUploadFiles = image.ImageUploadFiles.Replace(":80", "");
                //    image.ThumbnailImageUploadFiles = image.ThumbnailImageUploadFiles.Replace(":8082", "");
                //    image.ThumbnailImageUploadFiles = image.ThumbnailImageUploadFiles.Replace(":80", "");

                //}
                SQLHelper.UpdateCarImageSoldURL(image.ListingId, image.ImageUploadFiles, image.ThumbnailImageUploadFiles);

            }
        }

        public static void UpdateCarImageURLField(ImageViewModel image)
        {
            var context = new whitmanenterprisewarehouseEntities();

            if (image.InventoryStatus == 1)
            {

                var row =
                    context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == image.ListingId);

                string PreviousNormalPhotos = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                string PreviousThumbnailPhotos = String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                     ? ""
                                                     : row.ThumbnailImageURL.ToString();

                if (!String.IsNullOrEmpty(image.ImageUploadFiles) &&
                    !String.IsNullOrEmpty(image.ThumbnailImageUploadFiles))
                {

                    string carImageURL = "";

                    string carThumbnailURL = "";

                    string[] arrayImageUploadFiles = image.ImageUploadFiles.Split(new string[] { "," },
                                                                                  StringSplitOptions.RemoveEmptyEntries);

                    string[] arrayThumbnailImageUploadFiles = image.ThumbnailImageUploadFiles.Split(new string[] { "," },
                                                                                                    StringSplitOptions.
                                                                                                        RemoveEmptyEntries);

                    foreach (string tmp in arrayImageUploadFiles)
                        carImageURL += tmp + ",";

                    if (!String.IsNullOrEmpty(carImageURL))
                        carImageURL = carImageURL.Substring(0, carImageURL.Length - 1);

                    foreach (string tmp in arrayThumbnailImageUploadFiles)
                        carThumbnailURL += tmp + ",";

                    if (!String.IsNullOrEmpty(carThumbnailURL))
                        carThumbnailURL = carThumbnailURL.Substring(0, carThumbnailURL.Length - 1);

                    if (!String.IsNullOrEmpty(PreviousNormalPhotos))
                        carImageURL = PreviousNormalPhotos + "," + carImageURL;
                    if (!String.IsNullOrEmpty(PreviousThumbnailPhotos))
                        carThumbnailURL = PreviousThumbnailPhotos + "," + carThumbnailURL;


                    SQLHelper.UpdateCarImageURL(image.ListingId, carImageURL, carThumbnailURL);

                }
            }
            else
                if (image.InventoryStatus == -1)
                {
                    var row =
                       context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == image.ListingId);

                    string PreviousNormalPhotos = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                    string PreviousThumbnailPhotos = String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                         ? ""
                                                         : row.ThumbnailImageURL.ToString();

                    if (!String.IsNullOrEmpty(image.ImageUploadFiles) &&
                        !String.IsNullOrEmpty(image.ThumbnailImageUploadFiles))
                    {

                        string carImageURL = "";

                        string carThumbnailURL = "";

                        string[] arrayImageUploadFiles = image.ImageUploadFiles.Split(new string[] { "," },
                                                                                      StringSplitOptions.RemoveEmptyEntries);

                        string[] arrayThumbnailImageUploadFiles = image.ThumbnailImageUploadFiles.Split(new string[] { "," },
                                                                                                        StringSplitOptions.
                                                                                                            RemoveEmptyEntries);

                        foreach (string tmp in arrayImageUploadFiles)
                            carImageURL += tmp + ",";

                        if (!String.IsNullOrEmpty(carImageURL))
                            carImageURL = carImageURL.Substring(0, carImageURL.Length - 1);

                        foreach (string tmp in arrayThumbnailImageUploadFiles)
                            carThumbnailURL += tmp + ",";

                        if (!String.IsNullOrEmpty(carThumbnailURL))
                            carThumbnailURL = carThumbnailURL.Substring(0, carThumbnailURL.Length - 1);

                        if (!String.IsNullOrEmpty(PreviousNormalPhotos))
                            carImageURL = PreviousNormalPhotos + "," + carImageURL;
                        if (!String.IsNullOrEmpty(PreviousThumbnailPhotos))
                            carThumbnailURL = PreviousThumbnailPhotos + "," + carThumbnailURL;


                        SQLHelper.UpdateCarImageSoldURL(image.ListingId, carImageURL, carThumbnailURL);

                    }
                }
        }

        public static IList<ButtonPermissionViewModel> GetButtonList(int dealerId, string screen)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var result = new List<ButtonPermissionViewModel>();

                var groups = context.vincontrolgroups.Where(i => !i.groupname.ToLower().Equals("admin")).ToList();

                var buttons = context.vincontrolbuttons.Where(i => i.Screen.ToLower().Equals(screen.ToLower())).ToList();
                if (buttons.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        var buttonModels = buttons.Select(i => new Button()
                                                                   {
                                                                       ButtonId = i.Id,
                                                                       ButtonName = i.Button,
                                                                       CanSee =
                                                                           i.vincontroldealershipbuttons != null &&
                                                                           i.vincontroldealershipbuttons.Any(
                                                                               ii => ii.DealershipId == dealerId && ii.ButtonId == i.Id && ii.GroupId == group.groupid)
                                                                               ? i.vincontroldealershipbuttons.First(ii => ii.DealershipId == dealerId && ii.ButtonId == i.Id && ii.GroupId == group.groupid).CanSee
                                                                               : false
                                                                   }).ToList();
                        var buttonPermission = new ButtonPermissionViewModel()
                                                   {
                                                       Buttons = buttonModels,
                                                       DealershipId = dealerId,
                                                       GroupId = group.groupid,
                                                       GroupName = group.groupname
                                                   };
                        result.Add(buttonPermission);
                    }
                }

                return result;
            }
        }

        public static bool CheckDealershipButtonGroupExist(int dealerId, string groupName)
        {
            if (groupName.ToLower().Equals("admin") || groupName.ToLower().Equals("king")) return false;
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var group = context.vincontrolgroups.FirstOrDefault(i => i.groupname.ToLower().Equals(groupName.ToLower()));
                return context.vincontroldealershipbuttons.Any(i => i.DealershipId == dealerId && i.GroupId == group.groupid) ? true : false;
            }
        }

      
    }

    public class CarImage
    {
        public string ThumnailURLs { get; set; }
        public string ImageURLs { get; set; }
    }
}
