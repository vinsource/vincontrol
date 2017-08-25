using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Constant
{
    public enum ReportType
    {
        Excel,
        Pdf
    }
    public static class Constanst
    {
        public static string Inventory = "Inventory";
        public static string Appraisal = "Appraisal";
        public static int AutoTrader = 1;
        public static int CarsCom = 2;
        public static string ListingId = "ListingId";
        public static string CarTitle = "CarTitle";
        public static string PageChartTitle = "PageChartTitle";
        public static string CarMileAndPrice = "CarMileAndPrice";
        public static string ViewGraph = "ViewGraph";
        public static string ViewGraphInAppraisal = "ViewGraphInAppraisal";
        public static string ViewGoogleGraph = "ViewGoogleGraph";
        public static int DealerGroupConst = 0;

        public static class AdvancedResultType
        {
            public const int Used = 1;
            public const int New = 0;
            public const int Loaner = 5;
            public const int Auction = 6;
            public const int Recon = 7;
            public const int Wholesale = 2;
            public const int TradeNotClear = 8;
            public const int Soldout = 4;
            public const int RecentAppraisal = 9;
            public const int PendingAppraisal = 10;
        }

        public static class ActivityType
        {
            public const string NewAppraisal = "NewAppraisal";
            public const string NewUser = "NewUser";
            public const string AddToInventory = "AddToInventory";
            public const string PriceChange = "PriceChange";
            public const short Appraisal = 1;
            public const short Inventory = 2;
            public const short User = 3;
            public const short ShareFlyer = 4;
            public const short ShareBrochure = 5;
        }

        public static class SubActivityType
        {
            public const short NewAppraisal = 1;
            public const short NewUser = 7;
            public const short AddToInventory = 13;
            public const short PriceChange = 10;
            public const short ChangePassword = 14;
        }

        public static class VehicleTable
        {
            public const string Inventory = "Inventory";
            public const string Appraisal = "Appraisal";
            public const string WholeSale = "WholeSale";
            public const string SoldOut = "SoldOut";
        }

        public static class ScreenType
        {
            public const int InventoryScreen = 1;
            public const int SoldoutScreen = -1;
            public const int WholesaleScreen = 2;
        }

        public static class ProfileButton
        {
            public const string EditProfile = "Edit Profile";
            public const string Ebay = "Ebay";
            public const string WS = "WS";
            public const string BG = "BG";
            public const string Transfer = "Transfer";
            public const string MarkSold = "Mark Sold";
            public const string Wholesale = "Wholesale";
            public const string PriceTracking = "Price Tracking";
            public const string BucketJumpTracking = "Bucket Jump Tracking";
            public const string ACV = "ACV";
            public const string DealerCost = "DealerCost";
            public const string Status = "Status";
            public const string Craigslist = "Craigslist";
        }

        public static class TruckType
        {
            public const string Truck = "Truck";
            public const string Trailer = "Trailer";
            public const string TruckBody = "Truck Body";

        }

        public static class InventoryStatusText
        {
            public const string SoldOut = "Sold";
            public const string Inventory = "Inventory";
            public const string Wholesale = "Wholesale";
            public const string Recon = "Recon";
            public const string Auction = "Auction";
            public const string Loaner = "Loaner";
            public const string TradeNotClear = "Trade Not Clear";
        }

        public static class ChartCarType
        {
            public const int All = 0;
            public const int CarCom = 1;
            public const int Auto = 2;
        }

        public static class CarPart
        {
            public const int VehicleTires = 1;
            public const int FrontBumper = 2;
            public const int RearBumper = 3;
            public const int Glass = 4;
            public const int FrontEnd = 5;
            public const int RearEnd = 6;
            public const int DriverSide = 7;
            public const int PassengerSide = 8;
            public const int LightBulb = 9;
            public const int Other = 20;
        }

        public static class VehicleStatus
        {
            public const short Appraisal = 1;
            public const short Inventory = 2;
            public const short SoldOut = 3;
            public const short Vinsell = 4;
        }

        public static class InventoryStatus
        {
            public const int Retail = 0;//Never Used
            public const int SoldOut = 1;
            public const int Inventory = 2;
            public const int Wholesale = 3;
            public const int Recon = 4;
            public const int Auction = 5;
            public const int Loaner = 6;
            public const int TradeNotClear = 7;
        }
        
        public static class ConditionStatus
        {
            public const bool New = false;
            public const bool Used = true;
        }
        
        public static class CarInfoType
        {
            public const int New = 0;
            public const int Used = 1;
            public const int Wholesale = 2;
            public const int Appraisal = 3;
            public const int Sold = 4;
            public const int Loaner = 5;
            public const int Auction = 6;
            public const int Recon = 7;
            public const int TradeNotClear = 8;
            public const int Recent = 9;
            public const int Pending = 10;
        }

        public static class ActivityTypeId
        {
            public const short Appraisal = 1;
            public const short Inventory = 2;
            public const short User = 3;
            public const short ShareFlyer = 4;
            public const short ShareBrochure = 5;
        }
        public static class NotificationType
        {
            public const int Appraisal = 1;
            public const int Wholesale = 2;
            public const int Inventory = 3;
            public const int _24H = 4;
            public const int Note = 5;
            public const int PriceChange = 6;
            public const int AgeingBucket = 7;
            public const int BucketJump = 8;
            public const int MarketPriceRangeChange = 9;
            public const int ImageUpload = 10;
            public const int RetailPrice = 11;
            public const int DealerDiscount = 12;
            public const int Manufacturer = 13;
            public const int SalePrice = 14;
            public const int AgingSurveyNotification = 15;
            public const int AcvChange = 16;
            public const int DealerCostChange = 17;
            public const int MsrpChange = 18;
            public const int DealerDiscountChange = 19;
            public const int SalePriceChange = 20;
            public const int MileageChange = 21;
            public const int ShareFyler = 22;
            public const int SendBrochure = 23;
        }
        
        public static class AppraisalStatus
        {
            public const int Pending = 1;
            public const int Approved = 2;
        }

        public static class TradeInStatus
        {
            public const string Deleted = "Deleted";
            public const string Pending = "Pending";
            public const string Dead = "Dead";
            public const string Done = "Done";
        }
        public static class ChartSelectionType
        {
            public const short Inventory = 1;
            public const short Appraisal = 2;
            public const short SoldOut = 3;
        }

        public static class VehicleType
        {
            public const short Car = 1;
            public const short Truck = 2;
        }
        
        public static class AppraisalType
        {
            public const short WebAppraisal = 0;
            public const short MobileAppraisal = 1;
        }

        public static class RoleType
        {
            public const short Admin = 1;
            public const short Manager = 2;
            public const short Employee = 3;
            public const short Master = 4;
        }
        public static class RoleTypeText
        {
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string Employee = "Employee";
            public const string Master = "Master";
        }

        public static class Message
        {
            public const string Unauthorized = "Unauthorized";

        }

        public static class RoleName
        {
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string Employee = "Employee";
            public const string Master = "Master";
        }

        public static class Departments
        {
            public const short Sales = 1;
            public const short Service = 2;
            public const short Finance = 3;
            public const short Parts = 4;
        }

        public static class SurveyStatusIds
        {
            public const int Sent = 1;
            public const int Submitted = 2;
            public const int Resolved = 3;
            public const int Closed = 4;
            public const int Followup = 5;
            public const int Viewed = 6;
        }

        public static class SurveyStatusNames
        {
            public const string Sent = "Sent";
            public const string Submitted = "Submitted";
            public const string Resolved = "Resolved";
            public const string Closed = "Closed";
            public const string Followup = "Followup";
            public const string Viewed = "Viewed";
        }

        public static class CustomerLevelIds
        {
            public const int Low = 1;
            public const int Moderate = 2;
            public const int Critical = 3;
        }

        public static class CustomerLevelNames
        {
            public const string Low = "Low";
            public const string Moderate = "Moderate";
            public const string Critical = "Critical";
        }

        public static class AjaxMessage
        {
            public const string Success = "Success";
            public const string Error = "Error";
        }

        public static class PermissionCodes
        {
            public const string ReadOnly = "READONLY";
            public const string AllAccess = "ALLACCESS";
            public const string ReadOnlyAllAccess = "READONLY,ALLACCESS";
        }

        public static class CommunicationType
        {
            public const int InboundCall = 1;
            public const int OutboundCall = 2;
            public const int InboundEmail = 3;
            public const int OutboundEmail = 4;
            public const int Request = 5;
            public const int SurveyResult = 6;
            public const int Close = 7;
            public const int Followup = 8;
            public const int Resolve = 9;
        }

        public static class CommunicationNoteType
        {
            public const int NoAnswer = 1;
            public const int LeftAMessage = 2;
            public const int WrongNumber = 3;
        }

        public static class SurveyEmailTemplate
        {
            public const string FirstName = "[FirstName]";
            public const string LastName = "[LastName]";
            public const string Vehicle = "[Vehicle]";
            public const string DealerName = "[DealerName]";
        }

        public static class CommunicationStatusIds
        {
            public const int Resolved = 1;
            public const int Followup = 2;
            public const int Closed = 3;
        }

        public static class FBPostStatus
        {
            public const string Done = "Done";
        }

        public static class FBRequestField
        {
            public const string AccessToken = "access_token";
            public const string GrantType = "grant_type";
            public const string ClientId = "client_id";
            public const string ClientSecret = "client_secret";
        }

        public static class FBInsightType
        {
            public const string PageFan = "page_fans";
            public const string PageImpressionUnique = "page_impressions_unique";
            public const string PagePostImpressionUnique = "page_posts_impressions_unique";
            public const string PageEngagedUser = "page_engaged_users";
            public const string PageConsumption = "page_consumptions";
            public const string PostImpressionUnique = "post_impressions_unique";
        }

        public static class SoldOutAction
        {
            public const string _Delete = "Delete (Default)";
            public const string _3Days = "Display as Sold (3 Days)";
            public const string _5Days = "Display as Sold (5 Days)";
            public const string _7Days = "Display as Sold (7 Days)";
            public const string _30Days = "Display as Sold (30 Days)";
        }

        public static class InventoryTab
        {
            public const string Used = "inventory_used_tab";
            public const string New = "inventory_new_tab";
            public const string Loaner = "inventory_loaner_tab";
            public const string Auction = "inventory_auction_tab";
            public const string Wholesale = "inventory_wholesale_tab";
            public const string Recon = "inventory_recon_tab";
            public const string TradeNotClear = "inventory_tradenotclear_tab";
            public const string SoldCar = "inventory_soldcars_tab";
        }

        public static class BucketJumpTab
        {
            public const string LandRover = "bucketjump_landrover_tab";
            public const string Jaguar = "bucketjump_jaguar_tab";
            public const string AL = "bucketjump_al_tab";
            public const string MZ = "bucketjump_mz_tab";
            public const string Today = "bucketjump_today_tab";
            public const string Saved = "bucketjump_saved_tab";
        }

        public static class KpiCondition
        {
            public const int MissingPics = 1;
            public const int MissingPrice = 2;
            public const int MissingDescription = 3;
            public const int AboveMarket = 4;
            public const int AverageMarket = 5;
            public const int BelowMarket = 6;
            public const int From0To15 = 7;
            public const int From0To15Perecent = 8;
            public const int From16To30 = 9;
            public const int From16To30Perecent = 10;
            public const int From31To60 = 11;
            public const int From31To60Perecent = 12;

            public const int From61To90 = 13;
            public const int From61To90Perecent = 14;
            public const int Above90 = 15;
            public const int Above90Perecent = 16;

            public const int From0To15Above = 17;
            public const int From0To15At = 18;
            public const int From0To15Below = 19;
            public const int From0To15Other = 20;

            public const int From16To30Above = 21;
            public const int From16To30At = 22;
            public const int From16To30Below = 23;
            public const int From16To30Other = 24;

            public const int From31To60Above = 25;
            public const int From31To60At = 26;
            public const int From31To60Below = 27;
            public const int From31To60Other = 28;

            public const int From61To90Above = 29;
            public const int From61To90At = 30;
            public const int From61To90Below = 31;
            public const int From61To90Other = 32;

            public const int Over90Above = 33;
            public const int Over90At = 34;
            public const int Over90Below = 35;
            public const int Over90Other = 36;

        }


        public static class SortOption
        {
            public const int AppraisalDate = 0;
            public const int Vin = 1;
            public const int Stock = 2;
            public const int Year = 3;
            public const int Make = 4;
            public const int Model = 5;
            public const int Trim = 6;
            public const int Color = 7;
            public const int Owner = 8;
            public const int Age = 9;
            public const int MarketData = 10;
            public const int Mileage = 11;
            public const int Price = 12;

        }


        public static class DescriptionSettingCategory
        {
            public const int DealerWarranty = 1;
            public const int ShippingInfo = 2;
            public const int TermConditon = 3;
            public const int StartSentence = 4;
            public const int EndSentence = 5;
            public const int AuctionSentence = 6;
            public const int LoanerSentence = 7;
            public const int EndSentenceForNew = 8;

        }

        public static class StatisticValues
        {
            public const int UnitsInventory = 1;
            public const int SalesPerMonth = 2;
            public const int TotalSalesValues = 3;
            public const int AverageCost = 4;
            public const int GrossProfit = 5;
        }

        public static class RebateStatus
        {
            public const int NotActivated = 0;
            public const int Activating = 1;
            public const int Expired = 2;
        }

        public static class StockingGuideSource
        {
            public const int OtherBrand = 1;
            public const int Brand = 2;
            public const int Market = 3;
            
        }

        public static class AuctionRegion
        {
            public const int National = 1;
            public const int Regional = 2;
            

        }

        public static class AuctionRegionText
        {
            public const string WestCoast = "West Coast";
            public const string MidWest = "Mid West";
            public const string SouthEast = "South East";
            public const string NorthEast = "North East";
            public const string SouthWest = "South West";
            public const string Canada = "Canada";


        }

        public static class ReportType
        {
            public const int Excel = 1;
            public const int Pdf = 2;


        }

        public static class LanguageVersion
        {
            public const int English = 1;
            public const int Spanish = 2;


        }

        public static class VehicleLogSentence
        {
            public const string AppraisedByUser = "Vehicle was appraised by USER.";
            public const string AddToInventoryByAutomatedProcess = "Vehicle was added to inventory by automated feed process.";
            public const string AddToInventoryFromAppraisalByAutomatedProcess = "Vehicle was added to inventory by automated feed process.";
            public const string AddToInventoryByUser = "Vehicle was added to inventory from appraisal by USER.";
            public const string AddToWholesaleByUser = "Vehicle was added to wholesale from appraisal by USER.";
            public const string AddToAuctionByUser = "Vehicle was added to auction from appraisal by USER.";
            public const string AddToLoanerByUser = "Vehicle was added to loaner from appraisal by USER.";
            public const string AddToTradeNotClearByUser = "Vehicle was added to trade not clear from appraisal by USER.";
            public const string AddToReconByUser = "Vehicle was added to recon from appraisal by USER.";
            public const string AddToInventoryFromSoldByAutomatedProcess = "Vehicle was added back to inventory from sold inventory by automated feed process.";
            public const string PriceChangeByAutomatedProcess = "The price was changed from OLDPRICE to NEWPRICE from the outside source pushing to vincontrol by automated feed process.";
            public const string PriceChangeByUser = "The price was changed from OLDPRICE to NEWPRICE by USER.";
            public const string AcvChangeByUser = "The acv was changed from OLDPRICE to NEWPRICE by USER.";
            public const string DealerCostChangeByUser = "The dealer cost was changed from OLDPRICE to NEWPRICE by USER.";
            public const string MsrpChangeByUser = "The dealer cost was changed from OLDPRICE to NEWPRICE by USER.";
            public const string DealerDiscountChangeByUser = "The dealer cost was changed from OLDPRICE to NEWPRICE by USER.";
            public const string MileageChangeByUser = "The mileage was changed from OLDMILEAGE to NEWMILEAGE by USER.";
            public const string WarrantyChangeByUser = "The vehicle warranty was changed by USER.";
            public const string PriorRentalChangeByUser = "The vehicle attribute was changed to prior rental by USER.";
            public const string DealerDemoChangeByUser = "The vehicle attribute was changed to dealer demo by USER.";
            public const string UnWindChangeByUser = "The vehicle attribute was changed to unwind by USER.";
            public const string DescriptionChangeByUser = "The vehicle description was changed by USER.";
            public const string AcarChangeByUser = "The vehicle attribute was changed to Acar by USER.";
            public const string CertifiedChangeByUser = "The vehicle was marked as a certifed car by USER.";
            public const string StockChangeByUser = "The stock was changed from STOCK1 to STOCK2 by USER.";
            public const string ExteriorColorChangeByUser = "The exterior color was changed from COLOR1 to COLOR2 by USER.";
            public const string InteriorColorChangeByUser = "The interior color was changed from COLOR1 to COLOR2 by USER.";
            public const string ExteriorColorChangeByUserFromNull = "The exterior color was changed to COLOR by USER.";
            public const string InteriorColorChangeByUserFromNull = "The interior color was changed to COLOR by USER.";

            public const string AddtionalOptionsChangeByUser = "The additional options were changed from OPTIONLIST1 to OPTIONLIST2 by USER.";

            public const string AddtionalOptionsChangeByUserFromNull = "The additional options were changed to OPTIONLIST by USER.";

            public const string AddtionalPackagesChangeByUser = "The additional packages were changed from PACKAGELIST1 to PACKAGELIST2 by USER.";

            public const string AddtionalPackagesChangeByUserFromNull = "The additional packages were changed to PACKAGELIST by USER.";
     
            public const string VehicleTypeChangeByUser = "The vehicle type was changed from TYPE1 to TYPE2 by USER.";
            public const string BrandedTitleChangeByUser = "The vehicle attribute was changed to branded title by USER.";
            public const string IsFeaturedChangeByUser = "The vehicle was set as a featured car by USER.";
            public const string NonFeaturedChangeByUser = "The vehicle was unset as a featured car by USER.";
            public const string PriceChangeFromZeroByAutomatedProcess = "The price was changed to NEWPRICE from the outside source pushing to vincontrol by automated feed process.";
            public const string PriceChangeFromZeroByUser = "The price was changed to NEWPRICE by USER.";
            public const string SoldByAutomatedProcess = "Vehicle was marked sold by automated feed process.";
            public const string SoldByUser = "Vehicle was marked sold by USER.";
            public const string UnSoldToInventoryByUser = "Vehicle was marked unsold and brought back to inventory by USER.";
            public const string AppraisedAndSoldByUser = "Vehicle was appraised and marked sold by USER.";
            public const string DescriptionByAutomatedProcess ="Vehicle description was generated from DescriptionContent by automated description process.";
            public const string ImageByAutomatedProcess = "Vehicle images were added to the vehicle by Vincapture. Number of images : IMAGES.";
            public const string ImageChangeByUser = "Vehicle images were added to the vehicle or modified by USER. Number of images : IMAGES.";
            public const string ImageDeleteByUser = "Vehicle images were deleted all by USER.";
            public const string ImageByThirdPartyAutomatedProcess = "Vehicle images were added to the vehicle from third party source. Number of images : IMAGES.";
            public const string ChangeStatusByUser = "Vehicle status was changed from OLDSTATUS to NEWSTATUS by USER.";
            public const string ChangeStatusByAuto = "Vehicle status was changed from OLDSTATUS to NEWSTATUS by automated feed process.";
            public const string WindowStickerCreatedByUser = "Widnow sticker was created by USER.";
            public const string BuyerGuideCreatedByUser = "Buyer Guide was created by USER.";
            public const string BucketJumpCreatedByUser = "Bucket jump was created by USER. The price was suggestively changed from OLDPRICE to NEWPRICE.";
            public const string BucketJumpDoneByUser = "Bucket jump was finished by USER. The price was suggestively changed from OLDPRICE to NEWPRICE.";
            public const string EbayCreatedByUser = "Ebay listing was created by USER.";
            public const string VehicleTransferByUser = "The vehicle was transfered from DEALER1 to DEALER2 by USER.";
            public const string ShareFlyerByUser = "A flyer was shared to customer by USER.";
            public const string SendBrochureByUser = "A brochure was shared to customer by USER.";
            public const string FacebookByUser = "A facebook article was posted by USER ( LINK )";
            public const string CraigslistByCldms = "An ad on craigslist was posted in CITY by USER from CLDMS.";
            public const string SilentSalemanByUser = "An option sticker(silent saleman) was created by USER.";
            public const string RebateApplied =
                "Rebate of $[rebate amount] was implemented today, changing effective price from $[sales price] to ($[sales price] – $[rebate amount]). This rebate will expire on $[rebate expiration date]";
            public const string RebateExpired =
               "Rebate of $[rebate amount] has expired today, changing effective price to $[sales price]";
            public const string RebateRevision =
             "The existing rebate applied to this car has been changed. The new rebate amount of $[rebate amount] was implemented today, resulting a new effective price of ($[sales price] – $[rebate amount]). This rebate will now expire on [rebate expiration date].";
        }

        public static class EmailSubject
        {
            public const string TodayBucketJump = "Today Bucket Jump";


            public const string AddToInventory = "Add To Inventory";

            public const string AddToWholesale = "Add To Wholesale";

            public const string PriceUpdate = "Price Update";

            public const string MileageChange = "Mileage Change";

            public const string AcvUpdate = "ACV Update";

            public const string DeaelrCostUpdate = "Dealer Cost Update";

            public const string NewAppraisal = "New Appraisal";

            public const string ShareFlyer = "You received a flyer from  DEALER";

            public const string SendBrochure = "You received a brochure from  DEALER";

            public const string ExpiredRebates = "Expired Rebates";


            public const string AppliedRebates = "Applied Rebates";

        }

        public static class MarketRange
        {
            public static int MarketUp = 3;
            public static int MarketIn = 2;
            public static int MarketDown = 1;
        }

        public static class LogInventoryStatusText
        {

            public const string Wholesale = " (As Wholesale Status)";
            public const string Recon = " (As Recon Status)";
            public const string Auction = " (As Auction Status)";
            public const string Loaner = " (As Loaner Status)";
            public const string TradeNotClear = " (As Trade Not Clear Status)";
            public const string Retail = " (As Retail Status)";
        }

        public static class PendragoDealer
        {
            public const int HornburgWholesale=44684;
        }
    }
}
