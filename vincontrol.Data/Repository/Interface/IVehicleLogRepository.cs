using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IVehicleLogRepository
    {
        void AddNewLog(VehicleLog obj);

        void AddNewLog(int? inventoryId, int? userId, string description, int? soldoutInventoryId);

        IQueryable<VehicleLog> GetVehicleLogs(int inventoryId);

        IQueryable<VehicleLog> GetSoldVehicleLogs(int soldinventoryId);
    }
}
