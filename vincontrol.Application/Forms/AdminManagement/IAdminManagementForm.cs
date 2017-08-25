using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.AdminManagement;

namespace vincontrol.Application.Forms.AdminManagement
{
    public interface IAdminManagementForm
    {
        void SaveSetting(AdminViewModel model);
        AdminViewModel LoadSetting(int dealerId);
        void UpdateBeginAgingSurveyNotification(int number, int dealerId);
        void UpdateDiscontinueAgingSurveyNotification(int number, int dealerId);
        void UpdateIntervalSurvey(int number, int dealerId);
    }
}
