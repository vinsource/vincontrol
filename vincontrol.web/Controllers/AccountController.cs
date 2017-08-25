using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using EmailHelper = Vincontrol.Web.HelperClass.EmailHelper;

namespace Vincontrol.Web.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.
        private IAccountManagementForm _accountManagementForm;
        private IDealerManagementForm _dealerManagementForm;
        public AccountController()
            : this(null, null)
        {
            _accountManagementForm=new AccountManagementForm();
            _dealerManagementForm=new DealerManagementForm();
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
            _accountManagementForm = new AccountManagementForm();
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

        public ActionResult AfterLoggingOn(int role)
        {
            SessionHandler.NumberOfWSTemplate =
                _dealerManagementForm.GetDealerWindowStickerTemplate(SessionHandler.Dealer.DealershipId).Count();
            SessionHandler.HasAdminRight = (role == Constanst.RoleType.Admin || role == Constanst.RoleType.Master) ? true : false;
            SessionHandler.IsEmployee = (role == Constanst.RoleType.Employee) ? true : false;
            return role == Constanst.RoleType.Admin || role == Constanst.RoleType.Manager ? RedirectToAction("ViewKpi", "Market") : RedirectToAction("ViewInventory", "Inventory");
        }

        public ActionResult ConfirmLogOut()
        {
            return View();
        }

        public void AfterLoggingOnAfterTimeOut(int role)
        {
            SessionHandler.NumberOfWSTemplate = _dealerManagementForm.GetDealerWindowStickerTemplate(SessionHandler.Dealer.DealershipId).Count();
            SessionHandler.HasAdminRight = (role == Constanst.RoleType.Admin || role == Constanst.RoleType.Master) ? true : false;
            SessionHandler.IsEmployee = (role == Constanst.RoleType.Employee) ? true : false;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(LogOnViewModel model, string returnUrl)
        {

            Session.Clear();

            SessionHandler.IsMaster = false;

            SessionHandler.Single = true;

            if (!ValidateLogOn(model.UserName, model.Password))
            {
                model.LoginStatus = "Incorrect username or password.";

                return View("LogOn", model);
            }

            var searchUser = _accountManagementForm.CheckUserExistWithStatus(model.UserName.ToLower(), model.Password);

            if (searchUser.UserId == 0)
            {
                model.LoginStatus = "Incorrect username or password.";

                return View("LogOn", model);
            }

            SessionHandler.UserId = searchUser.UserId;
            SessionHandler.UserRight = new UserRightSetting(searchUser.RoleId);

            if (searchUser.RoleId == Constanst.RoleType.Master)
            {
                SessionHandler.IsMaster = true;

                var result = _accountManagementForm.MasterLogin(searchUser);

                SessionHandler.CurrentUser = searchUser;

                SessionHandler.Dealer = result.Dealer;

                SessionHandler.DealerGroup = result.DealerGroup;

                FormsAuth.SignIn(model.UserName, true);

                SessionHandler.UserRight.UpdateAllSettingsFromDatabase();
                return AfterLoggingOn(searchUser.RoleId);
            }

            if (searchUser.MultipleDealerLogin)
            {
                var result = _accountManagementForm.LoginMultipleStore(searchUser);

                SessionHandler.CurrentUser = searchUser;

                SessionHandler.Dealer = result.Dealer;

                SessionHandler.DealerGroup = result.DealerGroup;

                if (searchUser.CanSeeAllStores || searchUser.DefaultLogin == 0)
                {
                    SessionHandler.Single = false;
                }

                FormsAuth.SignIn(model.UserName, true);

                SessionHandler.UserRight.UpdateAllSettingsFromDatabase();

                return AfterLoggingOn(searchUser.RoleId);
            }

            var dealerLoginResult = _accountManagementForm.LoginSingleStore(searchUser);

            SessionHandler.CurrentUser = searchUser;

            SessionHandler.Dealer = dealerLoginResult.Dealer;

            FormsAuth.SignIn(model.UserName, true);

            SessionHandler.UserRight.UpdateAllSettingsFromDatabase();

            return AfterLoggingOn(searchUser.RoleId);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string LogOnForTimeOut(LogOnViewModel model)
        {
            Session.Clear();
            SessionHandler.IsMaster = false;
            SessionHandler.Single = true;
            try
            {
                var searchUser = _accountManagementForm.CheckUserExistWithStatus(model.UserName.ToLower(), model.Password);

                SessionHandler.UserId = searchUser.UserId;
                if (searchUser.UserId == 0)
                {
                    return "Incorrect";
                }

                SessionHandler.UserRight = new UserRightSetting(searchUser.RoleId);

                if (searchUser.RoleId == Constanst.RoleType.Master)
                {
                    SessionHandler.IsMaster = true;
                    var result = _accountManagementForm.MasterLogin(searchUser);

                    SessionHandler.CurrentUser = searchUser;

                    SessionHandler.Dealer = result.Dealer;

                    SessionHandler.DealerGroup = result.DealerGroup;

                    FormsAuth.SignIn(model.UserName, true);

                    AfterLoggingOnAfterTimeOut(searchUser.RoleId);
                    SessionHandler.UserRight.UpdateAllSettingsFromDatabase();
                    return searchUser.Role;
                }

                if (searchUser.MultipleDealerLogin)
                {
                    var result = _accountManagementForm.LoginMultipleStore(searchUser);

                    SessionHandler.CurrentUser = searchUser;

                    SessionHandler.Dealer = result.Dealer;
                    SessionHandler.DealerGroup = result.DealerGroup;

                    if (searchUser.CanSeeAllStores)
                    {
                        SessionHandler.Single = false;
                    }

                    FormsAuth.SignIn(model.UserName, true);

                    AfterLoggingOnAfterTimeOut(searchUser.RoleId);
                    SessionHandler.UserRight.UpdateAllSettingsFromDatabase();
                    return searchUser.Role;
                }

                var dealerLoginResult = _accountManagementForm.LoginSingleStore(searchUser);
                SessionHandler.CurrentUser = searchUser;

                SessionHandler.Dealer = dealerLoginResult.Dealer;

                FormsAuth.SignIn(model.UserName, true);

                AfterLoggingOnAfterTimeOut(searchUser.RoleId);
                SessionHandler.UserRight.UpdateAllSettingsFromDatabase();
                return searchUser.Role;
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

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult SendResetPasswordEmail(string email)
        {
            var searchUser = SQLHelper.GetUserByEmail(email);

            if (searchUser != null)
            {
                Guid id = _accountManagementForm.AddGetPasswordRequest(searchUser.UserId);
                EmailHelper.SendEmail(new System.Collections.Generic.List<string>() { email }, "VINControl Password Reset", EmailHelper.CreateBodyEmailForPasswordResetRequest(searchUser.UserId, id));
                return Json("Email sent");
            }
            else
            {
                return Json("Incorrect email");
            }
        }

        public ActionResult ChangePassword(int userId = 0, string forgotPasswordId = "")
        {
            ViewData["USERID"] = userId;
            ViewData["FORGOTPASSWORDID"] = forgotPasswordId;
            ViewData["ISEXPIRED"] = 0;

            if (!string.IsNullOrEmpty(forgotPasswordId))
            {
                if (_accountManagementForm.IsValidResetPasswordRequest(userId, forgotPasswordId, false) == false)
                {
                    ViewData["ISEXPIRED"] = 1;
                }
            }

            return View();
        }

        public ActionResult ResetPassword(int userId, string forgotPasswordId, string newPass)
        {
            if (_accountManagementForm.IsValidResetPasswordRequest(userId, forgotPasswordId, true))
            {
                _accountManagementForm.UpdatePass(userId, newPass);
                return Json("Updated");
            }
            else
            {
                return Json("Request expired");
            }
        }
        public ActionResult UpdateCurrentUserPassword(string currentPass, string newPass)
        {
            var currentUser = SessionHandler.CurrentUser;

            if (currentPass == currentUser.Password)
            {
                _accountManagementForm.UpdatePass(currentUser.UserId,newPass);
                SessionHandler.CurrentUser.Password = newPass;

                return Json("Updated");
            }
            else
            {
                return Json("Incorrect password");
            }
        }
   

        [HttpGet]
        public ActionResult ExtendSession()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
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
