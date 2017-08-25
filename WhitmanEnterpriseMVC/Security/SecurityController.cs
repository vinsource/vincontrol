using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.Handlers;

namespace WhitmanEnterpriseMVC.Security
{
    /// <summary>
    /// This controller will handle user authentication and permissioning.  Any controller that handles application flow
    /// to secure web pages, should inherit from this control in order to gain the PermissionsContext.
    /// </summary>
    public class SecurityController : Controller
    {
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
            if (_permissionsContext.PermissionData == null || _permissionsContext.PermissionData.Count == 0)
                _permissionsContext.Build(requestContext.HttpContext);

            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHandler.Dealership == null || !filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if ((filterContext.ActionDescriptor).ActionName.ToLower().Equals("karpowerdataonmobile")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("manheimdataonmobile")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getmarketdatabyappraisalfromautotraderwithhttppost")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getmarketdatawithin100milesradius")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getmarketdatabylistingnationwidefromautotraderwithhttppost")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getmarketdatabylistingfromautotraderwithhttppost")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getmarketdatabylistingwithin100milesradius")
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getmarketdatabylistingnationwidefromautotraderwithhttppost")
                        )
                    {
                    }
                    else
                        filterContext.Result = new JsonResult { Data = "_TimeOut_" };
                }
                else if (filterContext.HttpContext.Request.Browser.IsMobileDevice 
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewappraisalonmobile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewiprofileonmobile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewfullchartonmobile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("karpowerdataonmobile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("manheimdataonmobile")
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("viewgooglegraph")
                    )
                {
                        
                }
                else
                {
                    filterContext.Result = new RedirectResult(Url.Action("Timeout", "Account", new { area = string.Empty }));
                }
            }
            
            if (!(filterContext.HttpContext.Request.IsAjaxRequest()))                
                SessionHandler.RedirectUrl = filterContext.HttpContext.Request.RawUrl;
            
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