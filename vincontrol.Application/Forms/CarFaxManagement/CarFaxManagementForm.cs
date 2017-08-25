using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.CarFaxManagement
{
    public class CarFaxManagementForm : BaseForm, ICarFaxManagementForm
    {

        public CarFaxManagementForm() : this(new SqlUnitOfWork())
        {
            /*_carfaxService = new CarFaxService();*/
        }

        public CarFaxManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public Carfax GetCarFaxReportByVehicleId(int vehicleId)
        {
            return UnitOfWork.CarFax.GetCarFaxReportByVehicleId(vehicleId);
        }
    }
}
