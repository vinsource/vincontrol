using System.Collections.Generic;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.Data.Repository.Interface
{
    public interface IDealerGroupRepository
    {
        IList<DealerGroup> GetDealerGroups();
        void AddDealerGroup(DealerGroup dealerGroup);
    }
}
