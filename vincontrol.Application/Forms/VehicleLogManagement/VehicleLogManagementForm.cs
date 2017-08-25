using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.VehicleLogManagement
{
    public class VehicleLogManagementForm : BaseForm, IVehicleLogManagementForm
    {
        #region Constructors
        public VehicleLogManagementForm() : this(new SqlUnitOfWork()) { }

        public VehicleLogManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ICommonManagementForm Members
        #endregion

        public void AddVehicleLog(VehicleLog log)
        {
            UnitOfWork.VehicleLog.AddNewLog(log);
            UnitOfWork.CommitVincontrolModel();
        }

        public void AddVehicleLog(int? inventoryId, int? userId, string description, int? soldoutInventoryId)
        {
            UnitOfWork.VehicleLog.AddNewLog(inventoryId,userId,description,soldoutInventoryId);
            UnitOfWork.CommitVincontrolModel();
        }
    }
}
