using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace vincontrol.VinTrade
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
              "TradeStepOne",
              "trade/{dealer}/step-one-decode",
              new { controller = "Trade", action = "StepOneDecode", dealer = "" }
              );

            routes.MapRoute(
              "TradeStepOneWithInterestedVehicle",
              "trade/{dealer}/{interestedVehicle}/step-one-decode-vin",
              new { controller = "Trade", action = "StepOneDecodeInterested", dealer = "", interestedVehicle="" }
              );

            routes.MapRoute(
               "TradeStepTwo",
               "trade/{dealer}/step-two-options",
               new { controller = "Trade", action = "StepTwoOptions", dealer = "" }
               );

            routes.MapRoute(
                "TradeStepThree",
                "trade/{dealer}/step-three-contact",
                new { controller = "Trade", action = "StepThreeContact", dealer = "" }
                );

            routes.MapRoute(
               "TradeVehicleValue",
               "trade/{dealer}/vehicle-value",
               new { controller = "Trade", action = "VehicleValue", dealer = "" }
               );
            
            routes.MapRoute(
                "TradeLanding",
                "inventory/{dealer}/{year}-{make}-{model}-{trim}/{vin}",
                new { controller = "Inventory", action = "Index", dealer = "", year = "", make = "", model = "", trim = "", vin = "" }
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}