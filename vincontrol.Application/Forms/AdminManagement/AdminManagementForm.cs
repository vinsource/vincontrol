using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.AdminManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.AdminManagement
{
    public class AdminManagementForm : BaseForm, IAdminManagementForm
    {
        #region Constructors
        public AdminManagementForm() : this(new SqlUnitOfWork()) { }

        public AdminManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region IAdminManagement Members

        public void SaveSetting(AdminViewModel model)
        {
            var existingSetting = UnitOfWork.Admin.GetSetting(model.DealershipId);
            if (existingSetting != null)
            {
                existingSetting.CarFax = model.CarFax;
                existingSetting.CarFaxPassword = model.CarFaxPassword;
                existingSetting.Manheim = model.Manheim;
                existingSetting.ManheimPassword = model.ManheimPassword;
                existingSetting.KellyBlueBook = model.KellyBlueBook;
                existingSetting.KellyPassword = model.KellyPassword;
                existingSetting.BlackBook = model.BlackBook;
                existingSetting.BlackBookPassword = model.BlackBookPassword;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public AdminViewModel LoadSetting(int dealerId)
        {
            var existingSetting = UnitOfWork.Admin.GetSetting(dealerId);
            return existingSetting == null ? null : new AdminViewModel(existingSetting);
        }

        public void UpdateBeginAgingSurveyNotification(int number, int dealerId)
        {
            var existingSetting = UnitOfWork.Admin.GetSetting(dealerId);
            if (existingSetting != null)
            {
                existingSetting.FirstTimeRangeSurvey = number;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateDiscontinueAgingSurveyNotification(int number, int dealerId)
        {
            var existingSetting = UnitOfWork.Admin.GetSetting(dealerId);
            if (existingSetting != null)
            {
                existingSetting.SecondTimeRangeSurvey = number;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateIntervalSurvey(int number, int dealerId)
        {
            var existingSetting = UnitOfWork.Admin.GetSetting(dealerId);
            if (existingSetting != null)
            {
                existingSetting.IntervalSurvey = number;
                UnitOfWork.CommitVincontrolModel();
            }
        }
        #endregion
    }
}
