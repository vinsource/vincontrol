using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Vincontrol.Web.HelperClass
{
    public enum ReportTypeBAK
    {
        Excel,
        Pdf
    }

    public static class ConstanstBAK
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

        // 0: New
        // 1: Used
        // 2: Wholesale
        // 3: Appraisal
        // 4: Sold
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

        public static class ChartCarType
        {
            public const int All = 0;
            public const int CarCom = 1;
            public const int Auto = 2;
        }

        public static class ScreenType
        {
            public const int InventoryScreen = 1;
            public const int SoldoutScreen = -1;
            public const int WholesaleScreen = 2;

        }

        public static class SubActivityType
        {
            public const short NewAppraisal = 1;
            public const short NewUser = 7;
            public const short AddToInventory = 13;
            public const short PriceChange = 10;
            public const short ChangePassword = 14;
        }

        public static class ActivityType
        {
            public const short Appraisal = 1;
            public const short Inventory = 2;
            public const short User = 3;
            public const short ShareFlyer = 4;
            public const short ShareBrochure = 5;
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

        public static class VehicleStatus
        {
            public const short Appraisal = 1;
            public const short Inventory = 2;
            public const short SoldOut = 3;
        }

        public static class InventoryStatus
        {
            public const int Retail = 0;//Never Used
            public const int SoldOut = 1;
            // ReSharper disable MemberHidesStaticFromOuterClass
            public const int Inventory = 2;
            // ReSharper restore MemberHidesStaticFromOuterClass
            public const int Wholesale = 3;
            public const int Recon = 4;
            public const int Auction = 5;
            public const int Loaner = 6;
            public const int TradeNotClear = 7;
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

        public static class ConditionStatus
        {
            public const bool New = false;
            public const bool Used = true;
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

        //public static class ChartSelectionType
        //{
        //    public const short Inventory = 1;
        //    public const short Appraisal = 2;
        //    public const short SoldOut = 3;
        //}

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
    }
}
