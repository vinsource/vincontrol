using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Data.Repository.Interface
{
    public interface ICarFaxRepository
    {
        Carfax GetCarFaxReportByVin(string vin);
        Carfax GetCarFaxReportByVehicleId(int vehicleId);
        int CheckVinHasCarFaxReport(Carfax obj);
        void UpdatePriorRental(string vin);
        void AddCarFaxReport(Carfax carfax);
        void AddCarFaxReportForVinsell(manheim_Carfax carfax);
    }
}
