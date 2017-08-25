using System;
using vincontrol.Backend.Data;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.Data.Ultility
{
    public class Initializer
    {
        public static DealerGroup DealerGroup(whitmanenterprisedealergroup obj)
        {
            if (obj == null)
                return null;

            return new DealerGroup()
                       {
                           DealerGroupName = obj.DealerGroupName,
                           DateAdded = DateTime.Now,
                           MasterLogin = obj.MasterLogin,
                           MasterUserName = obj.MasterUserName,
                           DealerGroupId = obj.DealerGroupId
                       };
        }
    }
}

