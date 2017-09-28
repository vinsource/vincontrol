using System;
using System.Collections.Generic;
using System.Web;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.AdvancedSearch;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Constant;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.Models;
using Vincontrol.Web.Objects;
using VINControl.Craigslist;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.Handlers
{
    public static class SessionHandler
    {
        private const string _appraisalNationwide = "AppraisalNationwide";
        private const string _appraisalSoldNationwide = "AppraisalSoldNationwide";
        private const string _soldNationwide = "SoldNationwide";
        private const string _inventoryViewInfo = "InventoryViewInfo";
        private const string _bucketJumpViewInfo = "BucketJumpViewInfo";
        private const string _kpiViewInfo = "KPIViewInfo";
        private const string _dealership = "Dealership";
        private const string _single = "Single";
        private const string _allStores = "AllStores";
        private const string _currentUser = "CurrentUser";
        private const string _dealerGroup = "DealerGroup";
        private const string _dealershipName = "DealershipName";
        private const string _tradeInDealer = "TradeInDealer";
        private const string _customerLookUpVin = "CustomerLookUpVin";
        private const string _description = "Description";
        private const string _buyerGuideSetting = "BuyerGuideSetting";
        private const string _mobileDealerId = "MobileDealerId";
        private const string _newUsed = "Condition";
        private const string _dealerGroupId = "DealerGroupId";
        private const string _buyerGuide = "BuyerGuide";
        private const string _kpiInventoryList = "KpiInventoryList";
        private const string _kpiConditionInventoryList = "KpiConditionInventoryList";
        private const string _kpiCondition = "KpiCondition";
        private const string _ebayAds = "EbayAds";
        private const string _canViewBucketJumpReport = "CanViewBucketJumpReport";
        //private const string _chartScreen = "ChartScreen";
        private const string _pdfContentForMarketPriceRangeChange = "PDFContentForMarketPriceRangeChange";
        private const string _carsCom = "CarsCom";
        private const string _autoTrader = "AutoTrader";
        private const string _autoTraderNationwide = "AutoTraderNationwide";
        private const string _carsComNationwide = "CarsComNationwide";
        private const string _nationwide = "Nationwide";
        private const string _chromeTrimList = "ChromeTrimList";
        private const string _isMaster = "IsMaster";
        private const string _isNewAppraisal = "IsNewAppraisal";
        private const string _redirectUrl = "RedirectUrl";
        private const string _currentView = "CurrentView";
        private const string _dealerList = "DealerList";
        private const string _hasAdminRight = "HasAdminRight";
        private const string _isEmployee = "IsEmployee";
        private const string _karPowerViewModel = "KarPowerViewModel";
        private const string _inventoryList = "InventoryList";
        private const string _userId = "UserId";
        private const string _startSessionTime = "StartSessionTime";
        private const string _userRight = "UserRight";
        private const string _numberOfWSTemplate = "NumberOfWSTemplate";
        private const string _stockingGuideChart = "StockingGuideChart";
        private const string _vehicleDesriptionData = "VehicleDesriptionData";
        private const string _vinDatabaseInfo = "VinDatabaseInfo";
        private const string _advanceSearchUsedInventory = "AdvanceSearchUsedInventory";
        private const string _advanceSearchNewInventory = "AdvanceSearchNewInventory";
        private const string _advanceSearchLoanerInventory = "AdvanceSearchLoanerInventory";
        private const string _advanceSearchAuctionInventory = "AdvanceSearchAuctionInventory";
        private const string _advanceSearchReconInventory = "AdvanceSearchReconInventory";
        private const string _advanceSearchWholesaleInventory = "AdvanceSearchWholesaleInventory";
        private const string _advanceSearchTradeNotClearInventory = "AdvanceSearchTradeNotClearInventory";
        private const string _advanceSearchSoldInventory = "AdvanceSearchSoldInventory";
        private const string _advanceSearchRecentAppraisal = "AdvanceSearchRecentAppraisal";
        private const string _advanceSearchPendingAppraisal = "AdvanceSearchPendingAppraisal";
        private const string _manheimTransactions = "ManheimTransaction";
        private const string _manheimPastAuctions = "ManheimPastAuction";
        private const string _stockingGuideCars = "StockingGuideCars";
        private const string _stockingGuideAuctions = "StockingGuideAuctions";
        private const string _creditCardInfo = "CreditCardInfo";


        public static VehicleDescription GetVehicleDescriptionData(string key)
        {
            string result =_vehicleDesriptionData+key;
            return (VehicleDescription)HttpContext.Current.Session[result];
        }

        public static void SetVehicleDescriptionData(string key, VehicleDescription data)
        {
            string result =_vehicleDesriptionData+key;
            HttpContext.Current.Session[result] = data;
        }

        public static VinDatabaseInfo GetVinDatabaseInfo(string key)
        {
            string result = _vinDatabaseInfo + key;
            return (VinDatabaseInfo)HttpContext.Current.Session[result];
        }

        public static void SetVinDatabaseInfo(string key, VinDatabaseInfo data)
        {
            string result = _vinDatabaseInfo + key;
            HttpContext.Current.Session[result] = data;
        }

        public static List<AdvanceSearchItem> AdvanceSearchUsedInventory
        {
            get {
                return HttpContext.Current.Session[_advanceSearchUsedInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchUsedInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchUsedInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchNewInventory
        {
            get {
                return HttpContext.Current.Session[_advanceSearchNewInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchNewInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchNewInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchLoanerInventory
        {
            get {
                return HttpContext.Current.Session[_advanceSearchLoanerInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchLoanerInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchLoanerInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchAuctionInventory
        {
            get
            {
                return HttpContext.Current.Session[_advanceSearchAuctionInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchAuctionInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchAuctionInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchReconInventory
        {
            get
            {
                return HttpContext.Current.Session[_advanceSearchReconInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchReconInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchReconInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchWholesaleInventory
        {
            get
            {
                return HttpContext.Current.Session[_advanceSearchWholesaleInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchWholesaleInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchWholesaleInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchTradeNotClearInventory
        {
            get
            {
                return HttpContext.Current.Session[_advanceSearchTradeNotClearInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchTradeNotClearInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchTradeNotClearInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchSoldInventory
        {
            get
            {
                return HttpContext.Current.Session[_advanceSearchSoldInventory] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchSoldInventory];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchSoldInventory] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchRecentAppraisal
        {
            get
            {
                return HttpContext.Current.Session[_advanceSearchRecentAppraisal] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchRecentAppraisal];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchRecentAppraisal] = value;
            }
        }

        public static List<AdvanceSearchItem> AdvanceSearchPendingAppraisal
        {
            get
            {
                return HttpContext.Current.Session[_advanceSearchPendingAppraisal] == null
                    ? null
                    : (List<AdvanceSearchItem>)HttpContext.Current.Session[_advanceSearchPendingAppraisal];
            }
            set
            {
                HttpContext.Current.Session[_advanceSearchPendingAppraisal] = value;
            }
        }

        public static DateTime? StartSessionTime
        {
            get
            {
                //if (HttpContext.Current.Session[_startSessionTime] == null)
                //{
                //    return 0;
                //}
                return (DateTime?)HttpContext.Current.Session[_startSessionTime];
            }
            set
            {
                HttpContext.Current.Session[_startSessionTime] = value;
            }
        }

        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session[_userId] == null)
                {
                    return 0;
                }
                return (int)HttpContext.Current.Session[_userId];
            }
            set
            {
                HttpContext.Current.Session[_userId] = value;
            }
        }

        public static bool HasAdminRight
        {
            get
            {
                if (HttpContext.Current.Session[_hasAdminRight] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session[_hasAdminRight];
            }
            set
            {
                HttpContext.Current.Session[_hasAdminRight] = value;
            }
        }

        public static int NumberOfWSTemplate
        {
            get
            {
                if (HttpContext.Current.Session[_numberOfWSTemplate] == null)
                {
                    return 0;
                }
                return (int)HttpContext.Current.Session[_numberOfWSTemplate];
            }
            set
            {
                HttpContext.Current.Session[_numberOfWSTemplate] = value;
            }
        }

        public static bool IsEmployee
        {
            get
            {
                if (HttpContext.Current.Session[_isEmployee] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session[_isEmployee];
            }
            set
            {
                HttpContext.Current.Session[_isEmployee] = value;
            }
        }

        public static string MaintenanceMessage
        {
            get
            {
                var maintenanceObject = MaintenanceInfo.GetServerMaintenance(0);
                if (maintenanceObject.IsMaintenance)
                {
                    return maintenanceObject.MaintenanceMessage;
                }
                else
                {
                    return "&nbsp;";
                }
            }
        }

        public static IEnumerable<ExtendedSelectListItem> DealerList
        {
            get
            {
                if (HttpContext.Current.Session[_dealerList] == null)
                {
                    return null;
                }
                return (IEnumerable<ExtendedSelectListItem>)HttpContext.Current.Session[_dealerList];
            }
            set
            {
                HttpContext.Current.Session[_dealerList] = value;
            }
        }

        public static KarPowerViewModel KarPowerViewModel
        {
            get
            {
                if (HttpContext.Current.Session[_karPowerViewModel] == null)
                {
                    return null;
                }
                return (KarPowerViewModel)HttpContext.Current.Session[_karPowerViewModel];
            }
            set
            {
                HttpContext.Current.Session[_karPowerViewModel] = value;
            }
        }

        public static UserRoleViewModel CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[_currentUser] == null)
                {
                    return null;
                }
                return (UserRoleViewModel)HttpContext.Current.Session[_currentUser];
            }
            set
            {
                HttpContext.Current.Session[_currentUser] = value;
            }
        }

        public static DealershipViewModel Dealer
        {
            get
            {
                if (HttpContext.Current.Session[_dealership] == null)
                {
                    return null;
                }
                return (DealershipViewModel)HttpContext.Current.Session[_dealership];
            }
            set
            {
                HttpContext.Current.Session[_dealership] = value;
            }
        }

        public static ChartGraph StockingGuideChart
        {
            get
            {
                if (HttpContext.Current.Session[_stockingGuideChart] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_stockingGuideChart];
            }
            set
            {
                HttpContext.Current.Session[_stockingGuideChart] = value;
            }
        }

        public static bool IsMaster
        {
            get
            {
                if (HttpContext.Current.Session[_isMaster] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session[_isMaster];
            }
            set
            {
                HttpContext.Current.Session[_isMaster] = value;
            }
        }

        public static bool Single
        {
            get
            {
                if (HttpContext.Current.Session[_single] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session[_single];
            }
            set
            {
                HttpContext.Current.Session[_single] = value;
            }
        }
        public static bool AllStore
        {
            get
            {
                if (HttpContext.Current.Session[_allStores] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session[_allStores];
            }
            set
            {
                HttpContext.Current.Session[_allStores] = value;
            }
        }
        public static DealerGroupViewModel DealerGroup
        {
            get
            {
                if (HttpContext.Current.Session[_dealerGroup] == null)
                {
                    return null;
                }
                return (DealerGroupViewModel)HttpContext.Current.Session[_dealerGroup];
            }
            set
            {
                HttpContext.Current.Session[_dealerGroup] = value;
            }
        }

        public static string DealershipName
        {
            get
            {
                if (HttpContext.Current.Session[_dealershipName] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_dealershipName];
            }
            set
            {
                HttpContext.Current.Session[_dealershipName] = value;
            }
        }

        public static DealershipViewModel TradeInDealer
        {
            get
            {
                if (HttpContext.Current.Session[_tradeInDealer] == null)
                {
                    return null;
                }
                return (DealershipViewModel)HttpContext.Current.Session[_tradeInDealer];
            }
            set
            {
                HttpContext.Current.Session[_tradeInDealer] = value;
            }
        }

        public static string CustomerLookUpVin
        {
            get
            {
                if (HttpContext.Current.Session[_customerLookUpVin] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_customerLookUpVin];
            }
            set
            {
                HttpContext.Current.Session[_customerLookUpVin] = value;
            }
        }

        public static List<DescriptionSentenceGroup> Description
        {
            get
            {
                if (HttpContext.Current.Session[_description] == null)
                {
                    return null;
                }
                return (List<DescriptionSentenceGroup>)HttpContext.Current.Session[_description];
            }
            set
            {
                HttpContext.Current.Session[_description] = value;
            }
        }

        public static AdminBuyerGuideViewModel BuyerGuideSetting
        {
            get
            {
                if (HttpContext.Current.Session[_buyerGuideSetting] == null)
                {
                    return null;
                }
                return (AdminBuyerGuideViewModel)HttpContext.Current.Session[_buyerGuideSetting];
            }
            set
            {
                HttpContext.Current.Session[_buyerGuideSetting] = value;
            }
        }

        //For Mobile Only
        public static int MobileDealerId
        {
            get
            {
                if (HttpContext.Current.Session[_mobileDealerId] == null)
                {
                    return -1;
                }
                return (int)HttpContext.Current.Session[_mobileDealerId];
            }
            set
            {
                HttpContext.Current.Session[_mobileDealerId] = value;
            }
        }

        public static string NewUsed
        {
            get
            {
                if (HttpContext.Current.Session[_newUsed] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_newUsed];
            }
            set
            {
                HttpContext.Current.Session[_newUsed] = value;
            }
        }

        public static int DealerGroupId
        {
            get
            {
                if (HttpContext.Current.Session[_dealerGroupId] == null)
                {
                    return -1;
                }
                return (int)HttpContext.Current.Session[_dealerGroupId];
            }
            set
            {
                HttpContext.Current.Session[_dealerGroupId] = value;
            }
        }

        public static BuyerGuideViewModel BuyerGuide
        {
            get
            {
                if (HttpContext.Current.Session[_buyerGuide] == null)
                {
                    return null;
                }
                return (BuyerGuideViewModel)HttpContext.Current.Session[_buyerGuide];
            }
            set
            {
                HttpContext.Current.Session[_buyerGuide] = value;
            }
        }

        public static InventoryFormViewModel KpiInventoryList
        {
            get
            {
                if (HttpContext.Current.Session[_kpiInventoryList] == null)
                {
                    return null;
                }
                return (InventoryFormViewModel)HttpContext.Current.Session[_kpiInventoryList];
            }
            set
            {
                HttpContext.Current.Session[_kpiInventoryList] = value;
            }
        }

        public static List<CarInfoFormViewModel> KpiConditionInventoryList
        {
            get
            {
                if (HttpContext.Current.Session[_kpiConditionInventoryList] == null)
                {
                    return null;
                }
                return (List<CarInfoFormViewModel>)HttpContext.Current.Session[_kpiConditionInventoryList];
            }
            set
            {
                HttpContext.Current.Session[_kpiConditionInventoryList] = value;
            }
        }

        public static int KpiConditon
        {
            get
            {
                if (HttpContext.Current.Session[_kpiCondition] == null)
                {
                    return 0;
                }
                return (int)HttpContext.Current.Session[_kpiCondition];
            }
            set
            {
                HttpContext.Current.Session[_kpiCondition] = value;
            }
        }

        public static List<InventoryInfo> InventoryList
        {
            get
            {
                if (HttpContext.Current.Session[_inventoryList] == null)
                {
                    return null;
                }
                return (List<InventoryInfo>)HttpContext.Current.Session[_inventoryList];
            }
            set
            {
                HttpContext.Current.Session[_inventoryList] = value;
            }
        }

        public static EbayFormViewModel EbayAds
        {
            get
            {
                if (HttpContext.Current.Session[_ebayAds] == null)
                {
                    return null;
                }
                return (EbayFormViewModel)HttpContext.Current.Session[_ebayAds];
            }
            set
            {
                HttpContext.Current.Session[_ebayAds] = value;
            }
        }

        public static bool? CanViewBucketJumpReport
        {
            get
            {
                if (HttpContext.Current.Session[_canViewBucketJumpReport] == null)
                {
                    return null;
                }
                return (bool)HttpContext.Current.Session[_canViewBucketJumpReport];
            }
            set
            {
                HttpContext.Current.Session[_canViewBucketJumpReport] = value;
            }
        }

        //public static string ChartScreen
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session[_chartScreen] == null)
        //        {
        //            return "";
        //        }
        //        return (string)HttpContext.Current.Session[_chartScreen];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session[_chartScreen] = value;
        //    }
        //}

        public static string PDFContentForMarketPriceRangeChange
        {
            get
            {
                if (HttpContext.Current.Session[_pdfContentForMarketPriceRangeChange] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_pdfContentForMarketPriceRangeChange];
            }
            set
            {
                HttpContext.Current.Session[_pdfContentForMarketPriceRangeChange] = value;
            }
        }

        public static ChartGraph CarsCom
        {
            get
            {
                if (HttpContext.Current.Session[_carsCom] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_carsCom];
            }
            set
            {
                HttpContext.Current.Session[_carsCom] = value;
            }
        }

        public static ChartGraph AutoTrader
        {
            get
            {
                if (HttpContext.Current.Session[_autoTrader] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_autoTrader];
            }
            set
            {
                HttpContext.Current.Session[_autoTrader] = value;
            }
        }

        public static ChartGraph AutoTraderNationwide
        {
            get
            {
                if (HttpContext.Current.Session[_autoTraderNationwide] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_autoTraderNationwide];
            }
            set
            {
                HttpContext.Current.Session[_autoTraderNationwide] = value;
            }
        }

        public static ChartGraph CarsComNationwide
        {
            get
            {
                if (HttpContext.Current.Session[_carsComNationwide] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_carsComNationwide];
            }
            set
            {
                HttpContext.Current.Session[_carsComNationwide] = value;
            }
        }

        public static ChartGraph Nationwide
        {
            get
            {
                if (HttpContext.Current.Session[_nationwide] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_nationwide];
            }
            set
            {
                HttpContext.Current.Session[_nationwide] = value;
            }
        }

        public static ChartGraph AppraisalNationwide
        {
            get
            {
                if (HttpContext.Current.Session[_appraisalNationwide] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_appraisalNationwide];
            }
            set
            {
                HttpContext.Current.Session[_appraisalNationwide] = value;
            }
        }

        public static ChartGraph SoldNationwide
        {
            get
            {
                if (HttpContext.Current.Session[_soldNationwide] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_soldNationwide];
            }
            set
            {
                HttpContext.Current.Session[_soldNationwide] = value;
            }
        }

        public static ChartGraph AppraisalSoldNationwide
        {
            get
            {
                if (HttpContext.Current.Session[_appraisalSoldNationwide] == null)
                {
                    return null;
                }
                return (ChartGraph)HttpContext.Current.Session[_appraisalSoldNationwide];
            }
            set
            {
                HttpContext.Current.Session[_appraisalSoldNationwide] = value;
            }
        }

        public static IEnumerable<ExtendedSelectListItem> ChromeTrimList
        {
            get
            {
                if (HttpContext.Current.Session[_chromeTrimList] == null)
                {
                    return null;
                }
                return (IEnumerable<ExtendedSelectListItem>)HttpContext.Current.Session[_chromeTrimList];
            }
            set
            {
                HttpContext.Current.Session[_chromeTrimList] = value;
            }
        }

        public static bool IsNewAppraisal
        {
            get
            {
                if (HttpContext.Current.Session[_isNewAppraisal] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session[_isNewAppraisal];
            }
            set
            {
                HttpContext.Current.Session[_isNewAppraisal] = value;
            }
        }

        public static string RedirectUrl
        {
            get
            {
                if (HttpContext.Current.Session[_redirectUrl] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_redirectUrl];
            }
            set
            {
                HttpContext.Current.Session[_redirectUrl] = value;
            }
        }

        public static string CurrentView
        {
            get
            {
                if (HttpContext.Current.Session[_currentView] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_currentView];
            }
            set
            {
                HttpContext.Current.Session[_currentView] = value;
            }
        }

        public static ViewInfo InventoryViewInfo
        {
            get
            {
                if (HttpContext.Current.Session[_inventoryViewInfo] == null)
                {
                    HttpContext.Current.Session[_inventoryViewInfo] = new ViewInfo() { IsUp = true, SortFieldName = SessionHandler.Dealer.DealerSetting.InventorySorting, CurrentState = 0, CurrentView = CurrentViewEnum.Inventory.ToString() };
                }
                return (ViewInfo)HttpContext.Current.Session[_inventoryViewInfo];
            }
            set
            {
                HttpContext.Current.Session[_inventoryViewInfo] = value;
            }
        }

        public static ViewInfo MassBucketJumpViewInfo
        {
            get
            {
                if (HttpContext.Current.Session[_bucketJumpViewInfo] == null)
                {
                    HttpContext.Current.Session[_bucketJumpViewInfo] = new ViewInfo() { IsUp = true, SortFieldName = SessionHandler.Dealer.DealerSetting.InventorySorting, CurrentState = 0, CurrentView = BucketJumpView.LandRover.ToString() };
                }
                return (ViewInfo)HttpContext.Current.Session[_bucketJumpViewInfo];
            }
            set
            {
                HttpContext.Current.Session[_bucketJumpViewInfo] = value;
            }
        }

        public static ViewInfo KPIViewInfo
        {
            get
            {
                if (HttpContext.Current.Session[_kpiViewInfo] == null)
                {
                    HttpContext.Current.Session[_kpiViewInfo] = new ViewInfo() { };
                }
                return (ViewInfo)HttpContext.Current.Session[_kpiViewInfo];
            }
            set
            {
                HttpContext.Current.Session[_kpiViewInfo] = value;
            }
        }

        public static UserRightSetting UserRight
        {
            get
            {
                if (HttpContext.Current.Session[_userRight] == null)
                {
                    HttpContext.Current.Session[_userRight] = new UserRightSetting(Constanst.RoleType.Employee) { };
                }

                return (UserRightSetting)HttpContext.Current.Session[_userRight];
            }
            set
            {
                HttpContext.Current.Session[_userRight] = value;
            }
        }

        public static ManheimReport ManheimTransactionsReport
        {
            get
            {
                return HttpContext.Current.Session[_manheimTransactions] == null
                    ? null
                    : (ManheimReport)HttpContext.Current.Session[_manheimTransactions];
            }
            set
            {
                HttpContext.Current.Session[_manheimTransactions] = value;
            }
        }

        public static ManheimReport ManheimPastAuctionsReport
        {
            get
            {
                return HttpContext.Current.Session[_manheimPastAuctions] == null
                    ? null
                    : (ManheimReport)HttpContext.Current.Session[_manheimPastAuctions];
            }
            set
            {
                HttpContext.Current.Session[_manheimPastAuctions] = value;
            }
        }

        public static List<vincontrol.Data.Model.Appraisal> StockingGuideCarsReport
        {
            get
            {
                return HttpContext.Current.Session[_stockingGuideCars] == null
                    ? null
                    : (List<vincontrol.Data.Model.Appraisal>)HttpContext.Current.Session[_stockingGuideCars];
            }
            set
            {
                HttpContext.Current.Session[_stockingGuideCars] = value;
            }
        }

        public static List<Vincontrol.Web.Models.ManheimRegionVehicle> StockingGuideAuctionsReport
        {
            get
            {
                return HttpContext.Current.Session[_stockingGuideAuctions] == null
                    ? null
                    : (List<Vincontrol.Web.Models.ManheimRegionVehicle>)HttpContext.Current.Session[_stockingGuideAuctions];
            }
            set
            {
                HttpContext.Current.Session[_stockingGuideAuctions] = value;
            }
        }

        public static CreditCardInfo CreditCardInfo
        {
            get
            {
                if (HttpContext.Current.Session[_creditCardInfo] == null)
                {
                    return null;
                }
                return (CreditCardInfo)HttpContext.Current.Session[_creditCardInfo];
            }
            set
            {
                HttpContext.Current.Session[_creditCardInfo] = value;
            }
        }
    }

    public class ViewInfo
    {
        private string _sortFieldName;
        public string SortFieldName { get { return _sortFieldName; }set
        {
            if(!string.IsNullOrEmpty(value))
            _sortFieldName =  value.ToLower();
        } }
        public bool IsUp { get; set; }
        public string CurrentTab { get; set; }
        public string CurrentView { get; set; }
        public int CurrentState { get; set; }
    }
}