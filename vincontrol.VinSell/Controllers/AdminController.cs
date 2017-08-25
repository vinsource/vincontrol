using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.AdminManagement;
using vincontrol.Application.ViewModels.AdminManagement;
using vincontrol.Helper;
using vincontrol.VinSell.Handlers;

namespace vincontrol.VinSell.Controllers
{
    public class AdminController : BaseController
    {
        private IAdminManagementForm _adminManagement;

        public AdminController()
        {
            _adminManagement = new AdminManagementForm();
        }

        public ActionResult Index()
        {
            var model = new AdminViewModel()
                            {
                                DealershipId = SessionHandler.User.DealerId,
                                UserStamp = SessionHandler.User.UserName,
                                CarFax = SessionHandler.User.Setting.CarFax,
                                CarFaxPassword = EncryptionHelper.EncryptString(SessionHandler.User.Setting.CarFaxPassword),
                                Manheim = SessionHandler.User.Setting.Manheim,
                                ManheimPassword = EncryptionHelper.EncryptString(SessionHandler.User.Setting.ManheimPassword),
                                KellyBlueBook = SessionHandler.User.Setting.KellyBlueBook,
                                KellyPassword = EncryptionHelper.EncryptString(SessionHandler.User.Setting.KellyPassword),
                                BlackBook = SessionHandler.User.Setting.BlackBook,
                                BlackBookPassword = EncryptionHelper.EncryptString(SessionHandler.User.Setting.BlackBookPassword)
                            };
            return View(model);
        }

        [HttpPost]
        public string SaveSetting(AdminViewModel model)
        {
            try
            {
                if (!model.ManheimPasswordChanged)
                {
                    model.ManheimPassword = EncryptionHelper.DecryptString(model.ManheimPassword);
                }

                if (!model.CarFaxPasswordChanged)
                {
                    model.CarFaxPassword = EncryptionHelper.DecryptString(model.CarFaxPassword);
                }

                if (!model.KellyPasswordChanged)
                {
                    model.KellyPassword = EncryptionHelper.DecryptString(model.KellyPassword);
                }

                if (!model.BlackBookPasswordChanged)
                {
                    model.BlackBookPassword = EncryptionHelper.DecryptString(model.BlackBookPassword);
                }

                _adminManagement.SaveSetting(model);
                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
        }
    }
}
