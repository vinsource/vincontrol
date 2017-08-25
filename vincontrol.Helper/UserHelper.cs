using System;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Helper
{
    public class UserHelper
    {
        public static DealerUser CheckUserExistWithStatus(string userName, string passWord)
        {
            var user = new DealerUser();
            using (var context = new VincontrolEntities())
            {
                if (context.Users.Any(o => o.UserName == userName && o.Password == passWord && o.Active.Value))
                {
                    var result = context.Users.Where(o => o.UserName == userName && o.Password == passWord && o.Active.Value).ToList();
                    var firstOrDefault = result.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        var firstDealer = context.Dealers.FirstOrDefault(x => x.DealerId == firstOrDefault.DefaultLogin);
                        user.DealerId = firstOrDefault.DefaultLogin.GetValueOrDefault();
                        RoleType roleType;
                        user.Role = Enum.TryParse(firstOrDefault.UserPermissions.FirstOrDefault().Role.RoleName, out roleType) ? roleType : RoleType.Employee;
                        user.Username = firstOrDefault.UserName;
                        user.Name = firstOrDefault.Name;
                        if (firstDealer != null)
                        {
                            user.DealerName = firstDealer.Name;
                            user.Latitude = firstDealer.Lattitude.GetValueOrDefault().ToString().Trim();
                            user.Longtitude = firstDealer.Longtitude.GetValueOrDefault().ToString().Trim();

                        }
                    }
                }
                //check if master login
                else if (context.DealerGroups.Any(o => o.DealerGroupName == userName && o.Users.FirstOrDefault().Password == passWord))
                {
                    var result = context.DealerGroups.FirstOrDefault(o => o.DealerGroupName == userName && o.Users.FirstOrDefault().Password == passWord);
                    if (result != null)
                    {
                        user.DealerId = result.DefaultDealerId;
                        user.Username = result.DealerGroupName;
                        user.Name = result.DealerGroupName;
                        user.Role=RoleType.Master;
                        var firstDealer = context.Dealers.FirstOrDefault(x => x.DealerId ==  user.DealerId);
                        if (firstDealer != null)
                        {
                            user.DealerName = firstDealer.Name;
                            user.Latitude = firstDealer.Lattitude.GetValueOrDefault().ToString().Trim();
                            user.Longtitude = firstDealer.Longtitude.GetValueOrDefault().ToString().Trim();

                        }
                    }
                }

               
                else
                {
                    user = null;
                }

                if (user != null)
                {
                    LoadUserSetting(user);
                }
            }

            return user;
        }

        private static void LoadUserSetting(DealerUser user)
        {
            using (var context = new VincontrolEntities())
            {
                var setting = context.Settings.FirstOrDefault(i => i.DealerId == user.DealerId);
                if (setting != null)
                {
                    user.DealerSetting = new DealerSetting { CarFaxPassword = setting.CarFaxPassword, CarFaxUsername = setting.CarFax, KBBUserName = setting.KellyBlueBook, KBBPassword = setting.KellyPassword, MainheimUserName = setting.Manheim, MainheimPassword = setting.ManheimPassword };
                }
            }
        }
    }
}
