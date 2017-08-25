using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Appraisal = vincontrol.Data.Model.Appraisal;
using Inventory = vincontrol.Data.Model.Inventory;
using SoldoutInventory = vincontrol.Data.Model.SoldoutInventory;

namespace Vincontrol.Web.HelperClass
{
    public static class InventoryQueryHelper
    {

        public static IQueryable<Dealer> GetSingleOrGroupDealer(VincontrolEntities context)
        {
            if (SessionHandler.AllStore)
            {
                return context.Dealers.Where(
                         LogicHelper.BuildContainsExpression
                             <Dealer, int>(
                                 e => e.DealerId, GetDealerList(context)));

            }

            return context.Dealers.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<AppraisalDealerActivity> GetSingleOrGroupDealerAppraisalActivity(VincontrolEntities context)
        {
            if (SessionHandler.AllStore)
            {
                return context.AppraisalDealerActivities.Include("DealerActivitySubTypeCode").Where(
                    LogicHelper.BuildContainsExpression
                        <AppraisalDealerActivity, int>(
                            e => e.DealerId, GetDealerList(context)));

            }

            return context.AppraisalDealerActivities.Include("DealerActivitySubTypeCode").Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<InventoryDealerActivity> GetSingleOrGroupDealerInventoryActivity(VincontrolEntities context)
        {
            if (SessionHandler.AllStore)
            {
                return context.InventoryDealerActivities.Include("DealerActivitySubTypeCode").Where(
                    LogicHelper.BuildContainsExpression
                        <InventoryDealerActivity, int>(
                            e => e.DealerId, GetDealerList(context)));

            }

            return context.InventoryDealerActivities.Include("DealerActivitySubTypeCode").Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<UserDealerActivity> GetSingleOrGroupDealerUserActivity(VincontrolEntities context)
        {
            if (SessionHandler.AllStore)
            {
                return context.UserDealerActivities.Include("DealerActivitySubTypeCode").Where(
                    LogicHelper.BuildContainsExpression
                        <UserDealerActivity, int>(
                            e => e.DealerId, GetDealerList(context)));

            }

            return context.UserDealerActivities.Include("DealerActivitySubTypeCode").Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<Setting> GetSingleOrGroupSetting(VincontrolEntities context)
        {
            if (SessionHandler.AllStore)
            {
                return context.Settings.Where(
                         LogicHelper.BuildContainsExpression
                             <Setting, int>(
                                 e => e.DealerId, GetDealerList(context)));

            }

            return context.Settings.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<BuyerGuide> GetSingleOrGroupBuyerGuide(VincontrolEntities context)
        {
            if (SessionHandler.AllStore)
            {
                return context.BuyerGuides.Where(
                         LogicHelper.BuildContainsExpression
                             <BuyerGuide, int>(
                                 e => e.DealerId, GetDealerList(context)));

            }

            return context.BuyerGuides.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }


        public static IQueryable<Inventory> GetSingleOrGroupInventory(VincontrolEntities context, bool isInCludeVehicle=false, bool isIncludeTruckCategory=false, bool isIncludeTruckClass=false)
        {
            ObjectQuery<Inventory> objectQuery = context.Inventories;
            if (isInCludeVehicle) objectQuery = objectQuery.Include("Vehicle");
            if (isIncludeTruckCategory) objectQuery = objectQuery.Include("Vehicle.TruckCategory");
            if (isIncludeTruckClass) objectQuery = objectQuery.Include("Vehicle.TruckClass");
           
          
            if (SessionHandler.AllStore)
            {
                return objectQuery.Where(
                    LogicHelper.BuildContainsExpression
                        <Inventory, int>(
                            e => e.DealerId, GetDealerList(context)));

            }

            return objectQuery.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IEnumerable<int> GetDealerList(VincontrolEntities context)
        {
            if (SessionHandler.IsMaster)
            {
                return from e in context.Dealers
                       where
                           e.DealerGroupId ==
                           SessionHandler.DealerGroup.DealershipGroupId
                       select e.DealerId;
            }

            return (SessionHandler.DealerGroup != null)
                ? SessionHandler.DealerGroup.DealerList.Select(i => i.DealershipId)
                : new List<int> {SessionHandler.Dealer.DealershipId};
        }

        public static IQueryable<SoldoutInventory> GetSingleOrGroupSoldoutInventory(VincontrolEntities context,bool isInCludeVehicle =false, bool isIncludeTruckCategory =false, bool isIncludeTruckClass = false)
        {
            ObjectQuery<SoldoutInventory> objectQuery =context.SoldoutInventories;
            if (isInCludeVehicle) objectQuery = objectQuery.Include("Vehicle");
            if (isIncludeTruckCategory) objectQuery = objectQuery.Include("Vehicle.TruckCategory");
            if (isIncludeTruckClass) objectQuery = objectQuery.Include("Vehicle.TruckClass");
           
                if (!SessionHandler.AllStore)
                {
                    return
                                        objectQuery.Where(
                                            LogicHelper.BuildContainsExpression
                                                <SoldoutInventory, int>(
                                                    e => e.DealerId, GetDealerList(context)));
                }

          return objectQuery.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);

       }

        public static IQueryable<Inventory> GetSingleOrGroupWholesaleInventory(VincontrolEntities context, bool isInCludeVehicle = false, bool isIncludeTruckCategory = false, bool isIncludeTruckClass = false)
        {
            return GetSingleOrGroupInventory(context, isInCludeVehicle,isIncludeTruckCategory,isIncludeTruckClass).Where(e=>e.InventoryStatusCodeId == Constanst.InventoryStatus.Wholesale);
        }


        public static IQueryable<Inventory> GetSingleOrGroupLoanerInventory(VincontrolEntities context, bool isInCludeVehicle = false, bool isIncludeTruckCategory = false, bool isIncludeTruckClass = false)
        {
            return GetSingleOrGroupInventory(context, isInCludeVehicle, isIncludeTruckCategory, isIncludeTruckClass).Where(e => e.InventoryStatusCodeId == Constanst.InventoryStatus.Loaner);

        }

        public static IQueryable<Inventory> GetSingleOrGroupAuctionInventory(VincontrolEntities context, bool isInCludeVehicle = false, bool isIncludeTruckCategory = false, bool isIncludeTruckClass = false)
        {
            return GetSingleOrGroupInventory(context, isInCludeVehicle, isIncludeTruckCategory, isIncludeTruckClass).Where(e => e.InventoryStatusCodeId == Constanst.InventoryStatus.Auction);

        }

        public static IQueryable<Inventory> GetSingleOrGroupReconInventory(VincontrolEntities context, bool isInCludeVehicle = false, bool isIncludeTruckCategory = false, bool isIncludeTruckClass = false)
        {
            return GetSingleOrGroupInventory(context, isInCludeVehicle, isIncludeTruckCategory, isIncludeTruckClass).Where(e => e.InventoryStatusCodeId == Constanst.InventoryStatus.Recon);

        }

        public static IQueryable<Inventory> GetSingleOrGroupTradeNotClearInventory(VincontrolEntities context, bool isInCludeVehicle = false, bool isIncludeTruckCategory = false, bool isIncludeTruckClass = false)
        {
            return GetSingleOrGroupInventory(context, isInCludeVehicle, isIncludeTruckCategory, isIncludeTruckClass).Where(e => e.InventoryStatusCodeId == Constanst.InventoryStatus.TradeNotClear);

        }

        public static IQueryable<Appraisal> GetSingleOrGroupAppraisalIncludeUser(VincontrolEntities context)
        {
            if (!SessionHandler.Single)
            {
                return
                     context.Appraisals.Include("User").Include("User1").Where(
                         LogicHelper.BuildContainsExpression
                             <Appraisal, int>(
                                 e => e.DealerId, GetDealerList(context)));
            }

            return context.Appraisals.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<Appraisal> GetSingleOrGroupAppraisal(VincontrolEntities context, bool isInCludeVehicle = false, bool isIncludeTruckCategory= false, bool isIncludeTruckClass=false )
        {
            ObjectQuery<Appraisal> objectQuery = context.Appraisals;
            if (isInCludeVehicle) objectQuery = objectQuery.Include("Vehicle");
            if (isIncludeTruckCategory) objectQuery = objectQuery.Include("Vehicle.TruckCategory");
            if (isIncludeTruckClass) objectQuery = objectQuery.Include("Vehicle.TruckClass");
           
            if (!SessionHandler.Single)
            {
                return
                   objectQuery.Where(
                        LogicHelper.BuildContainsExpression
                            <Appraisal, int>(
                                e => e.DealerId, GetDealerList(context)));
            }

            return objectQuery.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<TradeInCustomer> GetSingleOrGroupTradein(VincontrolEntities context)
        {
            if (!SessionHandler.Single)
            {
                return
                     context.TradeInCustomers.Where(
                         LogicHelper.BuildContainsExpression
                             <TradeInCustomer, int>(
                                 e => e.DealerId, GetDealerList(context)));
            }

            return context.TradeInCustomers.Where(e => e.DealerId == SessionHandler.Dealer.DealershipId);
        }

        public static IQueryable<FlyerShareDealerActivity> GetSingleOrGroupDealerShareFlyerActivity(VincontrolEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.FlyerShareDealerActivities.Where(
                    LogicHelper.BuildContainsExpression
                        <FlyerShareDealerActivity, int>(
                            e => e.DealerId, GetDealerList(context)));

            }

            return
                context.FlyerShareDealerActivities.Where(
                    e => e.DealerId == SessionHandler.Dealer.DealershipId && e.IsBrochure == false);
        }

        public static IQueryable<FlyerShareDealerActivity> GetSingleOrGroupDealerShareBrochureActivity(VincontrolEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.FlyerShareDealerActivities.Where(
                    LogicHelper.BuildContainsExpression
                        <FlyerShareDealerActivity, int>(
                            e => e.DealerId, GetDealerList(context)));

            }

            return
                context.FlyerShareDealerActivities.Where(
                    e => e.DealerId == SessionHandler.Dealer.DealershipId && e.IsBrochure == true);
        }
    }
}