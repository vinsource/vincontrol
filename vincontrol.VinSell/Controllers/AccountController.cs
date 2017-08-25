using System;
using System.Web.Mvc;
using System.Web.Security;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.VinSell.Handlers;

namespace vincontrol.VinSell.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        private IAccountManagementForm _accountManagementForm;
        private IDealerManagementForm _dealerManagementForm;

        public AccountController()
        {
            _accountManagementForm = new AccountManagementForm();
            _dealerManagementForm = new DealerManagementForm();
        }

        #region Public Methods

        public string LogOn(LogOnViewModel model)
        {
            Session.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = _accountManagementForm.LogOn(model.UserName, (model.Password));
                    SessionHandler.User = existingUser;
                    

                    if (existingUser != null)
                    {
                        SessionHandler.Dealer = _dealerManagementForm.GetDealerById(existingUser.DealerId);
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return "Success";
                    }
                }
                catch (Exception)
                {
                    return "Error";
                }
                
            }

            return "Incorrect";
        }

        public ActionResult LogOnFromVincontrol(int userid)
        {
            //Session.Clear();
            var model = new LogOnViewModel();
            if (ModelState.IsValid)
            {
              
                try
                {
                    var existingUser = _accountManagementForm.GetUser(userid);
                    
                    if (existingUser != null)
                    {
                        SessionHandler.User = existingUser;
                        model = new LogOnViewModel()
                        {
                            UserName = existingUser.UserName,
                            Password = existingUser.Password,

                        };
                        SessionHandler.Dealer = _dealerManagementForm.GetDealerById(existingUser.DealerId);
                        FormsAuthentication.SetAuthCookie(existingUser.UserName, true);
                        model.LoginStatus = "Success";
                        return RedirectToAction("Index", "Auction");
                    }
                 


                   
                }
                catch (Exception)
                {
                    model.LoginStatus = "Error";
                    return null;
                }

            }

            model.LoginStatus = "Incorrect";
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogIn(LogOnViewModel model, string returnURL)
        {
            Session.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = _accountManagementForm.LogOn(model.UserName, (model.Password));
                    SessionHandler.User = existingUser;
                 
                  
                    if (existingUser != null)
                    {
                        SessionHandler.Dealer = _dealerManagementForm.GetDealerById(existingUser.DealerId);
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        model.LoginStatus = "Success";
                        return RedirectToAction("Index", "Auction");
                    }
                }
                catch (Exception)
                {
                    model.LoginStatus = "Error";
                    return View(model);
                }

            }

            model.LoginStatus = "Incorrect";
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Timeout()
        {
            return View();
        }

        public ActionResult LogOffForTimeOut()
        {
            FormsAuthentication.SignOut();

            Session.Clear();

            return RedirectToAction("IndexForTimeout", "Home");
        }

        #endregion
    }
}
