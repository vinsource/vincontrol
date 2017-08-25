using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.Objects;

namespace WhitmanEnterpriseMVC.Handlers
{
    public static class SessionHandler
    {
        private const string _dealership = "Dealership";
        private const string _single = "Single";
        private const string _currentUser = "CurrentUser";
        private const string _dealerGroup = "DealerGroup";
        private const string _dealershipName = "DealershipName";
        private const string _tradeInDealer = "TradeInDealer";
        private const string _customerLookUpVin = "CustomerLookUpVin";
        private const string _description = "Description";
        private const string _buyerGuideSetting = "BuyerGuideSetting";
        private const string _dealerId = "DealerId";
        private const string _newUsed = "NewUsed";
        private const string _dealerGroupId = "DealerGroupId";
        private const string _buyerGuide = "BuyerGuide";
        private const string _inventoryObject = "InventoryObject";
        private const string _ebayAds = "EbayAds";
        private const string _canViewBucketJumpReport = "CanViewBucketJumpReport";
        private const string _chartScreen = "ChartScreen";
        private const string _pdfContentForMarketPriceRangeChange = "PDFContentForMarketPriceRangeChange";
        private const string _carsCom = "CarsCom";
        private const string _autoTrader = "AutoTrader";
        private const string _autoTraderNationwide = "AutoTraderNationwide";
        private const string _carsComNationwide = "CarsComNationwide";
        private const string _chromeTrimList = "ChromeTrimList";
        private const string _isMaster = "IsMaster";
        private const string _isNewAppraisal = "IsNewAppraisal";
        private const string _redirectUrl = "RedirectUrl";

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

        public static DealershipViewModel Dealership
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

        public static int DealerId
        {
            get
            {
                if (HttpContext.Current.Session[_dealerId] == null)
                {
                    return -1;
                }
                return (int)HttpContext.Current.Session[_dealerId];
            }
            set
            {
                HttpContext.Current.Session[_dealerId] = value;
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

        public static InventoryFormViewModel InventoryObject
        {
            get
            {
                if (HttpContext.Current.Session[_inventoryObject] == null)
                {
                    return null;
                }
                return (InventoryFormViewModel)HttpContext.Current.Session[_inventoryObject];
            }
            set
            {
                HttpContext.Current.Session[_inventoryObject] = value;
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

        public static string ChartScreen
        {
            get
            {
                if (HttpContext.Current.Session[_chartScreen] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_chartScreen];
            }
            set
            {
                HttpContext.Current.Session[_chartScreen] = value;
            }
        }

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

        public static IEnumerable<SelectListItem> ChromeTrimList
        {
            get
            {
                if (HttpContext.Current.Session[_chromeTrimList] == null)
                {
                    return null;
                }
                return (IEnumerable<SelectListItem>)HttpContext.Current.Session[_chromeTrimList];
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
    }
}