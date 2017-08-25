using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.DomainObject;

namespace vincontrol.VinSell.Handlers
{
    public static class SessionHandler
    {
        private const string _user = "User";
        private const string _regionAuctionSummarizeList = "RegionAuctionSummarizeList";
        private const string _auctionsInRegion = "VehiclesInAuctions";
        private const string _auctionsInRegionByDate = "VehiclesInAuctionsByDate";
        private const string _favoriteAuctions = "FavoriteAuctions";
        private const string _notedAuctions = "NotedAuctions";
        private const string _manheimyearmakermodel = "ManheimYearMakeModel";
        private const string _manheimmodelpermake = "ManheimModelPerMake";
        private const string _advancedSearchViewModel = "AdvancedSearchViewModel";
        private const string _scrollTop = "ScrollTop";
        private const string _soldVehicles = "SoldVehicle";
        private const string _carsCom = "CarsCom";
        private const string _autoTrader = "AutoTrader";
        private const string _autoTraderNationwide = "AutoTraderNationwide";
        private const string _carsComNationwide = "CarsComNationwide";
        private const string _canViewBucketJumpReport = "CanViewBucketJumpReport";
        private const string _chartScreen = "ChartScreen";
        private const string _manheimTransactions = "ManheimTransaction";
        private const string _karPowerViewModel = "KarPowerViewModel";
        private const string _manheimPastAuctions = "ManheimPastAuction";
        private const string _dealership = "Dealership";

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

        public static IEnumerable<ManheimYearMakeModel> ManheimModelPerMake
        {
            get
            {
                if (HttpContext.Current.Session[_manheimmodelpermake] == null)
                {
                    return null;
                }
                return (IEnumerable<ManheimYearMakeModel>)HttpContext.Current.Session[_manheimmodelpermake];
            }
            set
            {
                HttpContext.Current.Session[_manheimmodelpermake] = value;
            }
        }

        public static UserViewModel User
        {
            get
            {
                if (HttpContext.Current.Session[_user] == null)
                {
                    return null;
                }
                return (UserViewModel)HttpContext.Current.Session[_user];
            }
            set
            {
                HttpContext.Current.Session[_user] = value;
            }
        }

        public static IList<RegionActionSummarizeViewModel> RegionAuctionSummarizeList
        {
            get
            {
                return (List<RegionActionSummarizeViewModel>)HttpContext.Current.Session[_regionAuctionSummarizeList];
            }
            set
            {
                HttpContext.Current.Session[_regionAuctionSummarizeList] = value;
            }
        }

        public static AuctionListViewModel ManheimYearMakeModelList
        {
            get
            {
                return (AuctionListViewModel)HttpContext.Current.Session[_manheimyearmakermodel];
            }
            set
            {
                HttpContext.Current.Session[_manheimyearmakermodel] = value;
            }
        }

        public static IList<VehicleViewModel> VehiclesInAuctions
        {
            get
            {
                return (List<VehicleViewModel>)HttpContext.Current.Session[_auctionsInRegion];
            }
            set
            {
                HttpContext.Current.Session[_auctionsInRegion] = value;
            }
        }

        public static IList<VehicleViewModel> VehiclesInAuctionsByDate
        {
            get
            {
                return (List<VehicleViewModel>)HttpContext.Current.Session[_auctionsInRegionByDate];
            }
            set
            {
                HttpContext.Current.Session[_auctionsInRegionByDate] = value;
            }
        }

        public static IList<VehicleViewModel> FavoriteAuctions
        {
            get
            {
                return (List<VehicleViewModel>)HttpContext.Current.Session[_favoriteAuctions];
            }
            set
            {
                HttpContext.Current.Session[_favoriteAuctions] = value;
            }
        }

        public static IList<VehicleViewModel> NotedAuctions
        {
            get
            {
                return (List<VehicleViewModel>)HttpContext.Current.Session[_notedAuctions];
            }
            set
            {
                HttpContext.Current.Session[_notedAuctions] = value;
            }
        }

        public static AdvancedSearchViewModel AdvancedSearchViewModel
        {
            get
            {
                return (AdvancedSearchViewModel)HttpContext.Current.Session[_advancedSearchViewModel];
            }
            set
            {
                HttpContext.Current.Session[_advancedSearchViewModel] = value;
            }
        }

        public static int ScrollTop
        {
            get
            {
                return HttpContext.Current.Session[_scrollTop] == null ? 0 : (int)HttpContext.Current.Session[_scrollTop];
            }
            set
            {
                HttpContext.Current.Session[_scrollTop] = value;
            }
        }

        public static IList<VehicleViewModel> SoldVehicles
        {
            get
            {
                return (List<VehicleViewModel>)HttpContext.Current.Session[_soldVehicles];
            }
            set
            {
                HttpContext.Current.Session[_soldVehicles] = value;
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
    }
}