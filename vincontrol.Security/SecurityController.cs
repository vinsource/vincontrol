using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using vincontrol.Data.Interface;

namespace vincontrol.Security
{
    public class SecurityController : Controller
    {
        protected IUnitOfWork UnitOfWork;

        IPermissionContext _permissionsContext;
        public IPermissionContext PermissionsContext
        {
            get { return _permissionsContext; }
            set { _permissionsContext = value; }
        }

        public SecurityController()
        {
            _permissionsContext = new PermissionContext();
        }

        public SecurityController(IPermissionContext context)
        {
            _permissionsContext = context;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //if (_permissionsContext.PermissionData == null || _permissionsContext.PermissionData.Count == 0)
            //    _permissionsContext.Build(requestContext.HttpContext);

            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["User"] == null || !filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if ((filterContext.ActionDescriptor).ActionName.ToLower().Equals("surveyquestionsanswers")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getdealerwebsitenotsecurity")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getcurrentusernotsecurity")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getallinventory")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("insertcustomerinfo")
                        
                        )
                    {

                    }
                    else
                    {
                        filterContext.Result = new JsonResult { Data = "" };    
                    }
                }
                else if 
                    (filterContext.HttpContext.Request.Browser.IsMobileDevice
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewfullchartonmobile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewvehicleonmobile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewvehicleonwindow")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("marketdata")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("manheimdata")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("karpowerdata")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("surveyquestionsanswers")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("employeeprofile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("inventoryprofile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("inventorydetails")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewcommunicationhistory")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getimagelinks")
                    )
                {

                }
                else
                {
                    filterContext.Result = new RedirectResult(Url.Action("Timeout", "Account", new { area = string.Empty }));
                }
            }

            if (!(filterContext.HttpContext.Request.IsAjaxRequest()))
                Session["RedirectUrl"] = filterContext.HttpContext.Request.RawUrl;

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Unauthorized(string username)
        {
            if (string.IsNullOrEmpty(username))
                username = "User";
            return View("Unauthorized", (object)username);
        }
    }
}
