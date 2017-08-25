using System;
using System.Web.Mvc;
using System.Web.Security;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Models;

namespace WhitmanEnterpriseMVC.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        private const string KingRole = "King";
        private const string AdminRole = "Admin";
        private const string ManagerRole = "Manager";
        private const string EmployeeRole = "Employee";
        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult AfterLoggingOn(string role)
        {
            Session["HasAdminRight"] = (role.ToLower().Equals("king") || role.ToLower().Equals("admin")) ? true : false;
            Session["IsEmployee"] = (!role.ToLower().Equals("king") && !role.ToLower().Equals("admin") && !role.ToLower().Equals("manager")) ? true : false;
            return role.ToLower().Equals("admin") ? RedirectToAction("AdminSecurity", "Admin") : RedirectToAction("ViewInventory", "Inventory");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(LogOnViewModel model, string returnURL)
        {
            Session.Clear();
            SessionHandler.IsMaster = false;
            if (!ValidateLogOn(model.UserName, model.Password))
            {
                //ModelState.AddModelError("LoginStatus", "Incorrect username or password.");

                model.LoginStatus = "Incorrect username or password.";

                return View("LogOn", model);
            }

            var dealer = new DealershipViewModel();
            var dealerGroup = new DealerGroupViewModel();
            var user = new UserRoleViewModel();
            var checkUser = SQLHelper.CheckUserExistWithStatus(model.UserName, model.Password);

            if (!checkUser.UserExist)
            {
                model.LoginStatus = "Incorrect username or password.";

                return View("LogOn", model);
            }

            SessionHandler.Single = true;

            if (checkUser.MasterLogin)
            {
                SessionHandler.IsMaster = true;
                
                SQLHelper.MasterLogin(checkUser, ref dealerGroup, ref dealer, ref user);

                Session["CurrentUser"] = user;

                Session["Dealership"] = dealer;

                Session["DealershipName"] = dealer.DealershipName;

                Session["DealerGroup"] = dealerGroup;

                FormsAuth.SignIn(model.UserName, true);

                return AfterLoggingOn(checkUser.Role);
            }

            if (checkUser.MultipleDealerLogin)
            {

                SQLHelper.LoginMultipleStore(checkUser, ref dealerGroup, ref dealer, ref user);

                Session["CurrentUser"] = user;

                Session["Dealership"] = dealer;

                Session["DealershipName"] = dealer.DealershipName;

                Session["DealerGroup"] = dealerGroup;

                if (checkUser.CanSeeAllStores)
                {
                    SessionHandler.Single = false;
                }
             
                FormsAuth.SignIn(model.UserName, true);

                return AfterLoggingOn(checkUser.Role);
            }

            SQLHelper.LoginSingleStore(checkUser, ref dealer, ref user);

            Session["CurrentUser"] = user;

            Session["Dealership"] = dealer;

            Session["DealershipName"] = dealer.DealershipName;

            FormsAuth.SignIn(model.UserName, true);

            return AfterLoggingOn(checkUser.Role);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string LogOnForTimeOut(LogOnViewModel model)
        {
            Session.Clear();
            SessionHandler.IsMaster = false;

            try
            {
                var dealer = new DealershipViewModel();
                var dealerGroup = new DealerGroupViewModel();
                var user = new UserRoleViewModel();
                var checkUser = SQLHelper.CheckUserExistWithStatus(model.UserName, model.Password);

                if (!checkUser.UserExist)
                {
                    return "Incorrect";
                }

                SessionHandler.Single = true;

                if (checkUser.MasterLogin)
                {
                    SessionHandler.IsMaster = true;
                    SQLHelper.MasterLogin(checkUser, ref dealerGroup, ref dealer, ref user);

                    Session["CurrentUser"] = user;

                    Session["Dealership"] = dealer;

                    Session["DealershipName"] = dealer.DealershipName;

                    Session["DealerGroup"] = dealerGroup;

                    FormsAuth.SignIn(model.UserName, true);

                    Session["HasAdminRight"] = (checkUser.Role.ToLower().Equals("king") || checkUser.Role.ToLower().Equals("admin")) ? true : false;
                    Session["IsEmployee"] = (!checkUser.Role.ToLower().Equals("king") && !checkUser.Role.ToLower().Equals("admin") && !checkUser.Role.ToLower().Equals("manager")) ? true : false;
                    //return AfterLoggingOn(checkUser.Role);
                    return checkUser.Role;
                }

                if (checkUser.MultipleDealerLogin)
                {
                    SQLHelper.LoginMultipleStore(checkUser, ref dealerGroup, ref dealer, ref user);

                    Session["CurrentUser"] = user;

                    Session["Dealership"] = dealer;

                    Session["DealershipName"] = dealer.DealershipName;

                    Session["DealerGroup"] = dealerGroup;

                    if (checkUser.CanSeeAllStores)
                    {
                        SessionHandler.Single = false;
                    }

                    FormsAuth.SignIn(model.UserName, true);

                    Session["HasAdminRight"] = (checkUser.Role.ToLower().Equals("king") || checkUser.Role.ToLower().Equals("admin")) ? true : false;
                    Session["IsEmployee"] = (!checkUser.Role.ToLower().Equals("king") && !checkUser.Role.ToLower().Equals("admin") && !checkUser.Role.ToLower().Equals("manager")) ? true : false;
                    //return AfterLoggingOn(checkUser.Role);
                    return checkUser.Role;
                }

                SQLHelper.LoginSingleStore(checkUser, ref dealer, ref user);

                Session["CurrentUser"] = user;

                Session["Dealership"] = dealer;

                Session["DealershipName"] = dealer.DealershipName;

                FormsAuth.SignIn(model.UserName, true);

                Session["HasAdminRight"] = (checkUser.Role.ToLower().Equals("king") || checkUser.Role.ToLower().Equals("admin")) ? true : false;
                Session["IsEmployee"] = (!checkUser.Role.ToLower().Equals("king") && !checkUser.Role.ToLower().Equals("admin") && !checkUser.Role.ToLower().Equals("manager")) ? true : false;
                //return AfterLoggingOn(checkUser.Role);
                return checkUser.Role;
            }
            catch (Exception)
            {
                return "Error";
            }
            
        }

        private bool ValidateLogOn(string userName, string password)
        {


            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("UserName", "You must specify a username.");

            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "You must specify a password.");

            }


            return ModelState.IsValid;
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
        //    Justification = "Needs to take same parameter type as Controller.Redirect()")]
        //public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        //{

        //    if (!ValidateLogOn(userName, password))
        //    {
        //        ViewData["rememberMe"] = rememberMe;
        //        return View();
        //    }

        //    FormsAuth.SignIn(userName, rememberMe);
        //    if (!String.IsNullOrEmpty(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        public ActionResult LogOff()
        {
            FormsAuth.SignOut();

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOffForTimeOut()
        {
            FormsAuth.SignOut();

            Session.Clear();

            return RedirectToAction("IndexForTimeOut", "Home");
        }

        public ActionResult Timeout()
        {
            return View();
        }

        public ActionResult Register()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View();
        }
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }
    }
}
