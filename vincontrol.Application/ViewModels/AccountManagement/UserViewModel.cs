using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.AccountManagement
{
    public class UserViewModel
    {
        public UserViewModel(){}

        public UserViewModel(User obj)
        {
            Id = obj.UserId;
            UserName = obj.UserName;
            Password = obj.Password;
            Name = obj.Name;
            Phone = obj.CellPhone;
            Email = obj.Email;
            DealerId = obj.DefaultLogin.GetValueOrDefault();
            var firstOrDefault = obj.UserPermissions.FirstOrDefault(i => i.DealerId == DealerId);
            if (firstOrDefault != null) RoleId = firstOrDefault.RoleId;
            var userPermission = obj.UserPermissions.FirstOrDefault(i => i.DealerId == DealerId);
            if (userPermission != null) Role = userPermission.Role.RoleName;
            DealerGroupId = obj.DealerGroupId;
            var orDefault = obj.DealerGroup != null ? obj.DealerGroup.Dealers.FirstOrDefault(i => i.DealerId == obj.DefaultLogin) : null;
            if (orDefault != null) DealerName = orDefault.Name;
            DealerAddress = obj.DealerAddress;
            DealerCity = obj.DealerCity;
            DealerState = obj.DealerState;
            DealerZipCode = obj.DealerZipCode;
            DealerLatitude = obj.DealerLatitude;
            DealerLongtitude = obj.DealerLongtitude;
            Setting = obj.Setting != null ? new Setting(obj.Setting) : new Setting();
            CraigslistSetting = obj.Dealer.CraigslistSettings.Any()
                ? new CraigslistSetting(obj.Dealer.CraigslistSettings.First())
                : new CraigslistSetting();
            Photo = obj.Photo;
            Description = obj.Description;
            TeamId = obj.TeamId.GetValueOrDefault();
            Team = obj.Teams != null && obj.Teams.FirstOrDefault() != null ? obj.Teams.First().Name : (obj.Team != null ? obj.Team.Name : string.Empty);
            DepartmentId = obj.DepartmentId.GetValueOrDefault();
            Department = obj.Department != null ? obj.Department.Name : string.Empty;
        }

        public int Id { get; set; }
        public int DealerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DealerName { get; set; }
        public string DealerEmail { get; set; }
        public string DealerAddress { get; set; }
        public string DealerCity { get; set; }
        public string DealerState { get; set; }
        public string DealerZipCode { get; set; }
        public string DealerLatitude { get; set; }
        public string DealerLongtitude { get; set; }
        public string DealerGroupId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public Setting Setting { get; set; }
        public CraigslistSetting CraigslistSetting { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Team { get; set; }
        public int TeamId { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int HomeDealerId{ get; set; }
    }

    public class Setting
    {
        public Setting() {}

        public Setting(Data.Model.Setting obj)
        {
            CarFax = obj.CarFax;
            CarFaxPassword = obj.CarFaxPassword;

            Manheim = obj.Manheim;
            ManheimPassword = obj.ManheimPassword;

            KellyBlueBook = obj.KellyBlueBook;
            KellyPassword = obj.KellyPassword;

            BlackBook = obj.BlackBook;
            BlackBookPassword = obj.BlackBookPassword;

            YoutubeUsername = obj.YoutubeUsername;
            YoutubePassword = obj.YoutubePassword;

            FirstTimeRangeSurvey = obj.FirstTimeRangeSurvey.GetValueOrDefault();
            SecondTimeRangeSurvey = obj.SecondTimeRangeSurvey.GetValueOrDefault();
            IntervalSurvey = obj.IntervalSurvey.GetValueOrDefault();

            WebsiteUrl = obj.WebSiteUrl;
        }

        public string CarFax { get; set; }
        public string CarFaxPassword { get; set; }

        public string Manheim { get; set; }
        public string ManheimPassword { get; set; }

        public string KellyBlueBook { get; set; }
        public string KellyPassword { get; set; }

        public string BlackBook { get; set; }
        public string BlackBookPassword { get; set; }

        public string YoutubeUsername { get; set; }
        public string YoutubePassword { get; set; }

        public int FirstTimeRangeSurvey { get; set; }
        public int SecondTimeRangeSurvey { get; set; }
        public int IntervalSurvey { get; set; }

        public string WebsiteUrl { get; set; }
    }

    public class CraigslistSetting
    {
        public CraigslistSetting() { }

        public CraigslistSetting(vincontrol.Data.Model.CraigslistSetting obj)
        {
            CraigslistSettingId = obj.CraigslistSettingId;
            DealerId = obj.DealerId;
            Email = obj.Email;
            Password = obj.Password;
            SpecificLocation = obj.SpecificLocation;
            State = obj.State;
            City = obj.City;
            CityUrl = obj.CityUrl;
            Location = obj.Location;
            LocationId = obj.LocationId.GetValueOrDefault();
            EndingSentence = obj.EndingSentence;
        }

        public int CraigslistSettingId { get; set; }
        public int DealerId { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string SpecificLocation { get; set; }
        public string State { get; set; }

        public string City { get; set; }
        public string CityUrl { get; set; }

        public string Location { get; set; }
        public int LocationId { get; set; }
        public string EndingSentence { get; set; }
    }
}
