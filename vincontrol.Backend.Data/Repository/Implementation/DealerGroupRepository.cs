using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using vincontrol.Backend.Data.Interface;
using vincontrol.Backend.Data.Repository.Interface;
using vincontrol.Backend.Data.Ultility;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.Data.Repository.Implementation
{
    public class DealerGroupRepository : IDealerGroupRepository
    {
        public DealerGroupRepository()
        {
        }

        #region Implementation of IDealerGroupRepository

        public IList<DealerGroup> GetDealerGroups()
        {
            return new List<DealerGroup>();
            //return _unitOfWork..GetAll().Select(Initializer.DealerGroup).ToList();
        }

        public void AddDealerGroup(DealerGroup dealerGroup)
        {
            //_unitOfWork.DealerGroup.Add(new whitmanenterprisedealergroup(){DealerGroupId = dealerGroup.DealerGroupId, DealerGroupName = dealerGroup.DealerGroupName, MasterUserName = dealerGroup.MasterUserName, MasterLogin = dealerGroup.MasterLogin});
            //_unitOfWork.Commit();
        }

        #endregion
    }
}
