using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface ISoldInventoryRepository: IRepository<SoldoutInventory>
    {
        IQueryable<SoldoutInventory> GetSoldVehicleForWeek(DateTime dateTime, int dealerId);

        IQueryable<SoldoutInventory> GetSoldVehicleForMonth(DateTime dateTime, int dealerId);
    }
}
