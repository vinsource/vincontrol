using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.TradeInManagement;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.DomainObject;

namespace vincontrol.VinTrade.Handlers
{
    public static class SessionHandler
    {
        private const string _tradeInDealer = "TradeInDealer";
        private const string _previousVin = "PreviousVin";
        private const string _previousMilage = "PreviousMilage";
        private const string _previousCondition = "PreviousCondition";
        private const string _karPowerYears = "KarPowerYears";
        private const string _tradeInVehicle = "TradeInVehicle";
        private const string _vehicleInfo = "VehicleInfo";
        private const string _landingInfo = "LandingInfo";
        private const string _interestedVehicle = "InterestedVehicle";

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


        public static string InterestedVehicle
        {
            get
            {
                if (HttpContext.Current.Session[_interestedVehicle] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_interestedVehicle];
            }
            set
            {
                HttpContext.Current.Session[_interestedVehicle] = value;
            }
        }


        public static string PreviousVin
        {
            get
            {
                if (HttpContext.Current.Session[_previousVin] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_previousVin];
            }
            set
            {
                HttpContext.Current.Session[_previousVin] = value;
            }
        }

        public static string PreviousMilage
        {
            get
            {
                if (HttpContext.Current.Session[_previousMilage] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_previousMilage];
            }
            set
            {
                HttpContext.Current.Session[_previousMilage] = value;
            }
        }

        public static string PreviousCondition
        {
            get
            {
                if (HttpContext.Current.Session[_previousCondition] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session[_previousCondition];
            }
            set
            {
                HttpContext.Current.Session[_previousCondition] = value;
            }
        }

        public static IEnumerable<ExtendedSelectListItem> ChromeYears
        {
            get
            {
                if (HttpContext.Current.Session[_karPowerYears] == null)
                {
                    return null;
                }
                return (IEnumerable<ExtendedSelectListItem>)HttpContext.Current.Session[_karPowerYears];
            }
            set
            {
                HttpContext.Current.Session[_karPowerYears] = value;
            }
        }
        
        public static TradeInVehicleModel TradeInVehicle
        {
            get
            {
                if (HttpContext.Current.Session[_tradeInVehicle] == null)
                {
                    return null;
                }
                return (TradeInVehicleModel)HttpContext.Current.Session[_tradeInVehicle];
            }
            set
            {
                HttpContext.Current.Session[_tradeInVehicle] = value;
            }
        }

        public static VehicleDescription VehicleInfo
        {
            get
            {
                if (HttpContext.Current.Session[_vehicleInfo] == null)
                {
                    return null;
                }
                return (VehicleDescription)HttpContext.Current.Session[_vehicleInfo];
            }
            set
            {
                HttpContext.Current.Session[_vehicleInfo] = value;
            }
        }

        public static LandingViewModel LandingInfo
        {
            get
            {
                if (HttpContext.Current.Session[_landingInfo] == null)
                {
                    return null;
                }
                return (LandingViewModel)HttpContext.Current.Session[_landingInfo];
            }
            set
            {
                HttpContext.Current.Session[_landingInfo] = value;
            }
        }
    }
}