using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Objects
{
    public class MaintenanceInfo
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool IsMaintenance { get; set; }
        public double SessionTimeOut { get; set; }
        public double NotifyBeforeInMinutes { get; set; }

        public string MaintenanceMessage
        {
            get { return string.Format("Vincontrol will be under maintainance from {0} to {1}. We apologize for any inconvenience, please call us at 1.855.VIN.CTRL for more information.", DateStart, DateEnd);}
        }

        public static MaintenanceInfo GetServerMaintenance(int sesstionTimeOut)
        {
            var setting = VINCustomSetting.GetSetting(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,"App_Data/Setting.xml")).ToDictionary(i => i.Key, i => i.Value);
            var maintenanceInfo = new MaintenanceInfo
            {
                DateStart =
                    DateTime.Parse(
                        setting["MaintenanceStartDateTime"].ToString(
                            CultureInfo.InvariantCulture)),
                DateEnd =
                    DateTime.Parse(
                        setting["MaintenanceEndDateTime"].ToString(
                            CultureInfo.InvariantCulture)),
                IsMaintenance = bool.Parse(setting["IsMaintenance"].ToString(CultureInfo.InvariantCulture)),
                NotifyBeforeInMinutes = double.Parse(setting["NotifyBeforeInMinutes"].ToString(CultureInfo.InvariantCulture)),
                SessionTimeOut = sesstionTimeOut
            };
            //maintenanceInfo.IsMaintenance = CheckIfMaintenance(maintenanceInfo);
            return maintenanceInfo;
        }

        private static bool CheckIfMaintenance(MaintenanceInfo maintenanceInfo)
        {
            //within maintenance period
            if (DateTime.Now >= maintenanceInfo.DateStart.AddMinutes(-1 * maintenanceInfo.NotifyBeforeInMinutes) && DateTime.Now < maintenanceInfo.DateEnd)
            {
                return maintenanceInfo.IsMaintenance;
            }

            return false;
        }
    }
}