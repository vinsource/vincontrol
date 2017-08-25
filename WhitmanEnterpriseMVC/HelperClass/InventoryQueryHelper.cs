using System.Collections.Generic;
using System.Linq;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Handlers;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public static class InventoryQueryHelper
    {

        public static IQueryable<whitmanenterprisedealership> GetSingleOrGroupDealer(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.whitmanenterprisedealerships.Where(
                         LogicHelper.BuildContainsExpression
                             <whitmanenterprisedealership, int>(
                                 e => e.idWhitmanenterpriseDealership, GetDealerList(context)));

            }

            return context.whitmanenterprisedealerships.Where(e => e.idWhitmanenterpriseDealership == SessionHandler.Dealership.DealershipId);
        }

        public static IQueryable<vincontroldealershipactivity> GetSingleOrGroupDealerActivity(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.vincontroldealershipactivities.Where(
                         LogicHelper.BuildContainsExpression
                             <vincontroldealershipactivity, int>(
                                 e => e.DealerId.Value, GetDealerList(context)));

            }

            return context.vincontroldealershipactivities.Where(e => e.DealerId == SessionHandler.Dealership.DealershipId);
        }

        public static IQueryable<whitmanenterprisesetting> GetSingleOrGroupSetting(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.whitmanenterprisesettings.Where(
                         LogicHelper.BuildContainsExpression
                             <whitmanenterprisesetting, int>(
                                 e => e.DealershipId.Value, GetDealerList(context)));

            }

            return context.whitmanenterprisesettings.Where(e => e.DealershipId == SessionHandler.Dealership.DealershipId);
        }

        public static IQueryable<vincontrolbuyerguide> GetSingleOrGroupBuyerGuide(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.vincontrolbuyerguides.Where(
                         LogicHelper.BuildContainsExpression
                             <vincontrolbuyerguide, int>(
                                 e => e.dealershipId.Value, GetDealerList(context)));

            }

            return context.vincontrolbuyerguides.Where(e => e.dealershipId == SessionHandler.Dealership.DealershipId);
        }

        public static IQueryable<whitmanenterprisedealershipinventory> GetSingleOrGroupInventory(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.whitmanenterprisedealershipinventories.Where(
                         LogicHelper.BuildContainsExpression
                             <whitmanenterprisedealershipinventory, int>(
                                 e => e.DealershipId.Value, GetDealerList(context)));

            }

            return context.whitmanenterprisedealershipinventories.Where(e => e.DealershipId == SessionHandler.Dealership.DealershipId);
        }

        private static IEnumerable<int> GetDealerList(whitmanenterprisewarehouseEntities context)
        {
            if (SessionHandler.IsMaster)
            {
                return from e in context.whitmanenterprisedealerships
                       where
                           e.DealerGroupID ==
                           SessionHandler.DealerGroup.DealershipGroupId
                       select e.idWhitmanenterpriseDealership;
            }

            return SessionHandler.DealerGroup.DealerList.Select(i => i.DealershipId);
        }

        public static IQueryable<whitmanenterprisedealershipinventorysoldout> GetSingleOrGroupSoldoutInventory(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return
                                    context.whitmanenterprisedealershipinventorysoldouts.Where(
                                        LogicHelper.BuildContainsExpression
                                            <whitmanenterprisedealershipinventorysoldout, int>(
                                                e => e.DealershipId.Value, GetDealerList(context)));
            }

            return context.whitmanenterprisedealershipinventorysoldouts.Where(e => e.DealershipId == SessionHandler.Dealership.DealershipId);
        }

        public static IQueryable<vincontrolwholesaleinventory> GetSingleOrGroupWholesaleInventory(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return
                      context.vincontrolwholesaleinventories.Where(
                          LogicHelper.BuildContainsExpression
                              <vincontrolwholesaleinventory, int>(
                                  e => e.DealershipId.Value, GetDealerList(context)));

            }

            return context.vincontrolwholesaleinventories.Where(e => e.DealershipId == SessionHandler.Dealership.DealershipId);
        }

        public static IQueryable<whitmanenterpriseappraisal> GetSingleOrGroupAppraisal(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return context.whitmanenterpriseappraisals.Where(LogicHelper.BuildContainsExpression<whitmanenterpriseappraisal, int>(
                                 e => e.DealershipId.Value, GetDealerList(context)));
            }

            return context.whitmanenterpriseappraisals.Where(e => e.DealershipId == SessionHandler.Dealership.DealershipId);
        }

        public static IQueryable<vincontrolbannercustomer> GetSingleOrGroupTradein(whitmanenterprisewarehouseEntities context)
        {
            if (!SessionHandler.Single)
            {
                return
                     context.vincontrolbannercustomers.Where(
                         LogicHelper.BuildContainsExpression
                             <vincontrolbannercustomer, int>(
                                 e => e.DealerId.Value, GetDealerList(context)));
            }

            return context.vincontrolbannercustomers.Where(e => e.DealerId == SessionHandler.Dealership.DealershipId);
        }
    }
}