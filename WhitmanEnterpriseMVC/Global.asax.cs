using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.Routing;
using WhitmanEnterpriseMVC.Controllers;
using WhitmanEnterpriseMVC.HelperClass;

namespace WhitmanEnterpriseMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const string KingRole = "King";
        private const string AdminRole = "Admin";
        private const string ManagerRole = "Manager";
        private const string EmployeeRole = "Employee";
        

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Ignore("{*pathInfo}", new { pathInfo = @"^.*(ChartImg.axd)$" });
            routes.MapRoute(
               "TradeIn", // Route name
               "TradeInValue/{dealer}", // URL with parameters
                new { controller = "TradeIn", action = "Index", dealer = "" } // Parameter defaults                
            );

            routes.MapRoute(
               "TradeInWithVin", // Route name
               "TradeInValueWithVin/{dealer}", // URL with parameters
                new { controller = "TradeIn", action = "IndexWithVin", dealer = "" } // Parameter defaults                
             );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "Content",                                              // Route name
                "{controller}/{action}/{content}",                           // URL with parameters
                new { controller = "Home", action = "Index", content = "" }  // Parameter defaults
            );

            routes.MapRoute(
               "TradeInAddComment",                                              // Route name
               "{controller}/{action}/{city}/{state}/{comment}/{name}",                           // URL with parameters
               new { controller = "TradeIn", action = "AddComment", city = "", state = "", comment = "", name="" }  // Parameter defaults
           );

            routes.MapRoute(
                "TradeInSaveComment",                                              // Route name
                "{controller}/{action}/{id}/{city}/{state}/{comment}/{name}",                           // URL with parameters
                new { controller = "TradeIn", action = "SaveComment", city = "", state = "", comment = "", id = "", name = "" }  // Parameter defaults
            );                       
            
          //  routes.MapRoute(
          //    "Error",
          //    "{*url}",
          //    new { controller = "Error", action = "HttpError" }
          //);

            
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            //if (!Roles.RoleExists("King"))
            //    Roles.CreateRole("King");
            //if (!Roles.RoleExists("Admin"))
            //    Roles.CreateRole("Admin");
            //if (!Roles.RoleExists("Manager"))
            //    Roles.CreateRole("Manager");
            //if (!Roles.RoleExists("Employee"))
            //    Roles.CreateRole("Employee");

            
        }
        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();

        //    RegisterRoutes(RouteTable.Routes);

        //    ControllerBuilder.Current.SetControllerFactory(new ErrorHandlingControllerFactory());
        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    HttpContext ctx = System.Web.HttpContext.Current;
        //    Exception exception = ctx.Server.GetLastError();
        //    Response.Clear();
        //    Context.ClearError();
        //    var routeData = new RouteData();
        //    routeData.Values["controller"] = "Error";
        //    routeData.Values["action"] = "HttpError";
        //    routeData.Values["ex"] = exception;
        //    var obj = new object();
        //    if (exception != null)
        //    {
        //        string exMsg = "User: '" + User.Identity.Name + "' DateTime: '" +
        //                       DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss") + "'" + Environment.NewLine +
        //                       "Source: " + exception.Source + Environment.NewLine +
        //                       "Message: " + exception.Message + Environment.NewLine +
        //                       "Inner Exception: " + exception.InnerException + Environment.NewLine +
        //                       "Stack trace: " + exception.StackTrace;
        //        //WriteToLogFile(exMsg);
        //    }
        //    IController sample = new ErrorController();
        //    sample.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //}

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //Session.Timeout = 1;
        }

    }
}