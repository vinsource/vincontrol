using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class VehicleLogRepository : IVehicleLogRepository
    {
        private VincontrolEntities _context;

        public VehicleLogRepository(VincontrolEntities context)
        {
            _context = context;
        }

        public void AddNewLog(VehicleLog obj)
        {
            _context.VehicleLogs.AddObject(obj);
        }

        public void AddNewLog(int? inventoryId, int? userId, string description, int? soldoutInventoryId)
        {
            var newLog = new VehicleLog() { InventoryId = inventoryId, UserId = userId, Description = description, SoldOutInventoryId = soldoutInventoryId, DateStamp = DateUtilities.Now()};
            _context.VehicleLogs.AddObject(newLog);
        }

        public IQueryable<VehicleLog> GetVehicleLogs(int inventoryId)
        {
            return _context.VehicleLogs.Where(x => x.InventoryId == inventoryId);
        }

        public IQueryable<VehicleLog> GetSoldVehicleLogs(int soldinventoryId)
        {
            return _context.VehicleLogs.Where(x => x.SoldOutInventoryId == soldinventoryId);
        }
    }
}
