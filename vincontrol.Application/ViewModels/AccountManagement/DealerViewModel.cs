using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.AccountManagement
{
    public class DealerViewModel
    {
        public DealerViewModel(){}

        public DealerViewModel(Dealer obj)
        {
            DealerId = obj.DealerId;
            Name = obj.Name;
            Address = obj.Address;
            City = obj.City;
            State = obj.State;
            ZipCode = obj.ZipCode;
            Phone = obj.Phone;
            Email = obj.Email;
            Logo = obj.Logo;
            Lattitude = obj.Lattitude.GetValueOrDefault();
            Longitude = obj.Longtitude.GetValueOrDefault();
            NotificationSetting = obj.NotificationSetting != null
                                      ? new NotificationSettingViewModel(obj.NotificationSetting)
                                      : new NotificationSettingViewModel();
            Setting = obj.Setting != null ? new Setting(obj.Setting) : new Setting();
            CraigslistSetting = obj.CraigslistSettings.Any() ? new CraigslistSetting(obj.CraigslistSettings.First()) : new CraigslistSetting();
            
        }

        public int DealerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        private string _email;

        public string Email
        {
            get
            {
                return ConfigurationManagement.ConfigurationHandler.IsTestEmail ? ConfigurationManagement.ConfigurationHandler.TestEmail:_email;
            } 
            
            set { _email = value; }
        }
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public bool Active { get; set; }
        public string DealerGroupId { get; set; }
        public bool ImportFeed { get; set; }
        public DateTime DateStamp { get; set; }
        public NotificationSettingViewModel NotificationSetting { get; set; }
        public Setting Setting { get; set; }
        public CraigslistSetting CraigslistSetting { get; set; }
        public string DealerImagesFolder { get; set; }
    }
}
