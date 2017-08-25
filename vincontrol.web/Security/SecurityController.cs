using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vincontrol.EmailHelper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;

namespace Vincontrol.Web.Security
{
    /// <summary>
    /// This controller will handle user authentication and permissioning.  Any controller that handles application flow
    /// to secure web pages, should inherit from this control in order to gain the PermissionsContext.
    /// </summary>
    public class SecurityController : Controller
    {
        protected IEmail EmailWrapper;
        protected EmailTemplateReader EmailTemplateReader;

        IPermissionContext _permissionsContext;
        public IPermissionContext PermissionsContext
        {
            get { return _permissionsContext; }
            set { _permissionsContext = value; }
        }

        public SecurityController()
        {
            _permissionsContext = new PermissionContext();
            EmailWrapper = new Email();
            EmailTemplateReader = new EmailTemplateReader();
        }

        public SecurityController(IPermissionContext context)
        {
            _permissionsContext = context;
            EmailWrapper = new Email();
            EmailTemplateReader = new EmailTemplateReader();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (_permissionsContext.PermissionData == null || _permissionsContext.PermissionData.Count == 0)
                _permissionsContext.Build(requestContext.HttpContext);

            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHandler.Dealer == null || !filterContext.HttpContext.Request.IsAuthenticated)
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
                        || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("getmarketdatabylistingnationwidewithhttppost")
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
                    || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("carfaxreportfromautotrader")
                      || (filterContext.ActionDescriptor).ActionName.ToLower().Equals("downloadbrochure")
                    )
                {
                        
                }
                else
                {
                    if ((filterContext.ActionDescriptor).ActionName.ToLower().Equals("sendflyerstring"))
                    {
                        
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(Url.Action("Timeout", "Account", new { area = string.Empty }));
                    }
                    
                }
            }

            if (!(filterContext.HttpContext.Request.IsAjaxRequest()))
                SessionHandler.RedirectUrl = string.IsNullOrEmpty(filterContext.HttpContext.Request.RawUrl) ||
                                             filterContext.HttpContext.Request.RawUrl.ToLower().Contains(
                                                 "/security/unauthorized?username")
                                                 ? "/Inventory/ViewInventory"
                                                 : filterContext.HttpContext.Request.RawUrl;
            
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Unauthorized(string username)
        {
            if (string.IsNullOrEmpty(username))
                username = "User";
            return View("Unauthorized", (object)username);
        }

        public void SwitchDealerInGroup(int dealerTransferId)
        {
            var dealerGroup = SessionHandler.DealerGroup;

            var switchDealer = dealerGroup.DealerList.First(t => t.DealershipId.Equals(dealerTransferId));
            SessionHandler.Single = true;
            SessionHandler.Dealer = switchDealer;

            var switchUser = SessionHandler.CurrentUser;
            switchUser.DealershipId = switchDealer.DealershipId;
            SQLHelper.SwitchUserRole(ref switchUser, dealerTransferId.ToString());
            SessionHandler.CurrentUser = switchUser;
            SessionHandler.UserRight.UpdateAllSettingsFromDatabase();
            SessionHandler.NumberOfWSTemplate = SQLHelper.GetNumberOfWSTemplate(SessionHandler.Dealer.DealershipId);
        }
    }
}