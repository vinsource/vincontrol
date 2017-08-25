using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Handlers
{
    public enum CurrentViewEnum
    {
        Inventory = 0,
        NewInventory,
        LoanerInventory,
        AuctionInventory,
        ReconInventory,
        WholesaleInventory,
        SoldInventory,
        TradeNotClear,
        ExpressBucketJump,
        TodayBucketJump,
        ACar,
        MissingContent,
        NoContent,
        UsedSold,
        NewSold
    }

    public enum BucketJumpView
    {
        LandRover = 0,
        Jaguar,
        AL,
        MZ,
        GroupTodayBucketJump,
        Saved
    }
}