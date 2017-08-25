using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.CarFaxManagement
{
    public interface ICarFaxManagementForm
    {
        Carfax GetCarFaxReportByVehicleId(int vehicleId);
    }
}
