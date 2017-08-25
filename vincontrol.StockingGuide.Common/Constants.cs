namespace vincontrol.StockingGuide.Common
{
    public static class Constants
    {
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

        public static class KPIInfoType
        {
            public const int Segment =2;
            public const int Brand = 1;
        }

        public static class ConditionStatus
        {
            public const bool New = false;
            public const bool Used = true;
        }

        public static class DealerId
        {
            public const int FreewayIsuzu = 37695;
        }


    }
}
