using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.VehicleLogManagement
{
    public interface IVehicleLogManagementForm
    {
        void AddVehicleLog(VehicleLog log);

        void AddVehicleLog(int? inventoryId, int? userId, string description, int? soldoutInventoryId);
    }
}
