using System;
using vincontrol.Constant;
using vincontrol.Data.Model;

namespace vincontrol.Helper
{
    public static class InventoryTypeCheckingHelper
    {
        public static Func<Inventory, bool> IsUsedFunc
        {
            get
            {
                return inventory => inventory.Condition == Constanst.ConditionStatus.Used &&
                                    inventory.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory;
            }
        }

        public static Func<Inventory, bool> IsNewFunc
        {
            get
            {
                return inventory => inventory.Condition == Constanst.ConditionStatus.New &&
                   inventory.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory;
            }
        }

        public static Func<Inventory, bool> IsWholeSaleFunc
        {
            get
            {
                return inventory => inventory.InventoryStatusCodeId == Constanst.InventoryStatus.Wholesale;
            }
        }

        public static Func<Inventory, bool> IsLoanerFunc
        {
            get
            {
                return inventory => inventory.InventoryStatusCodeId == Constanst.InventoryStatus.Loaner;
            }
        }
        public static Func<Inventory, bool> IsAuctionFunc
        {
            get
            {
                return inventory => inventory.InventoryStatusCodeId == Constanst.InventoryStatus.Auction;
            }
        }

        public static Func<Inventory, bool> IsReconFunc
        {
            get
            {
                return inventory => inventory.InventoryStatusCodeId == Constanst.InventoryStatus.Recon;
            }
        }

        public static Func<Inventory, bool> IsTradeNotClearFunc
        {
            get
            {
                return inventory => inventory.InventoryStatusCodeId == Constanst.InventoryStatus.TradeNotClear;
            }
        }

        public static Func<Appraisal, bool> IsRecentAppraisalFunc
        {
            get
            {
                return appraisal => (appraisal.AppraisalStatusCodeId == null || appraisal.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending);
            }
        }

        public static Func<Appraisal, bool> IsPendingAppraisalFunc
        {
            get
            {
                return appraisal => appraisal.AppraisalStatusCodeId == Constanst.AppraisalStatus.Pending;
            }
        }


        public static bool IsUsedInventory(this Inventory inventory)
        {
            return IsUsedFunc(inventory);
        }

        public static bool IsNewInventory(this Inventory inventory)
        {
            return IsNewFunc(inventory);
        }

        public static bool IsWholeSale(this Inventory inventory)
        {
            return IsWholeSaleFunc(inventory);
        }

        public static bool IsLoaner(this Inventory inventory)
        {
            return IsLoanerFunc(inventory);
        }

        public static bool IsAuction(this Inventory inventory)
        {
            return IsAuctionFunc(inventory);
        }

        public static bool IsRecon(this Inventory inventory)
        {
            return IsReconFunc(inventory);
            //return inventory.InventoryStatusCodeId == Constanst.InventoryStatus.Recon;
        }

        public static bool IsTradeNotClear(this Inventory inventory)
        {
            return IsTradeNotClearFunc(inventory);
            //return inventory.InventoryStatusCodeId == Constanst.InventoryStatus.TradeNotClear;
        }

        public static bool IsRecentAppraisal(this Appraisal appraisal)
        {
            return IsRecentAppraisalFunc(appraisal);
            //return (appraisal.AppraisalStatusCodeId == null || appraisal.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending);
        }

        public static bool IsPendingAppraisal(this Appraisal appraisal)
        {
            return IsPendingAppraisalFunc(appraisal);
            //return appraisal.AppraisalStatusCodeId == Constanst.AppraisalStatus.Pending;
        }
    }
}
