using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WhitmanEnterpriseMVC.HelperClass
{
    public enum ReportType
    {
        Excel,
        Pdf
    }

    public static class Constanst
    {
        //public static string CustomSettingFilePath = ;
        public static string Inventory = "Inventory";
        public static string Appraisal = "Appraisal";
        public static string AutoTrader = "AutoTrader";
        public static string CarsCom = "CarsCom";
        public static string ListingId = "ListingId";
        public static string CarTitle = "CarTitle";
        public static string ViewGraph = "ViewGraph";
        public static string ViewGraphInAppraisal = "ViewGraphInAppraisal";
        public static string ViewGoogleGraph = "ViewGoogleGraph";
        public static int DealerGroupConst = 0;

        public static class ActivityType
        {
            public const string NewAppraisal = "NewAppraisal";
            public const string NewUser = "NewUser";
            public const string AddToInventory = "AddToInventory";
            public const string PriceChange = "PriceChange";
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
        }

        public static class VehicleTable
        {
            public const string Inventory = "Inventory";
            public const string Appraisal = "Appraisal";
            public const string WholeSale = "WholeSale";
            public const string SoldOut = "SoldOut";
        }
    }
}
