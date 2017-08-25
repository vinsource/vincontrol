using System.Collections.Generic;
using vincontrol.Data.Model;


namespace vincontrol.StockingGuide.Interfaces
{
    public interface IStateService
    {
        List<manheim_states> GetRegionCodeMapping();

    }
}
