using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;
using vincontrol.DomainObject;

namespace vincontrol.Data.Repository.Implementation
{
    public class CarFaxRepository : ICarFaxRepository
    {
        private VincontrolEntities _context;
        private VinsellEntities _contextVinsell;

        public CarFaxRepository(VincontrolEntities context,VinsellEntities contextVinsell)
        {
            _context = context;
            _contextVinsell = contextVinsell;
        }

        #region ICarFaxRepository' Members

        public Carfax GetCarFaxReportByVin(string vin)
        {
            return _context.Carfaxes.FirstOrDefault(o => o.Vin==vin);
        }

        public Carfax GetCarFaxReportByVehicleId(int vehicleId)
        {
            return _context.Carfaxes.FirstOrDefault(o => o.VehicleId==vehicleId);
        }

        public int CheckVinHasCarFaxReport(Carfax obj)
        {
            if (obj != null)
            {
                var dt = DateTime.Parse(obj.Expiration.ToString());

                return dt.Date >= DateTime.Now.Date ? 1 : 2;
            }

            return 0;
        }

        public void UpdatePriorRental(string vin)
        {
            var vehicles = _context.Inventories.Where(i => i.Vehicle.Vin==vin);
            if (vehicles.Any())
                foreach (var vehicle in vehicles)
                {
                    vehicle.PriorRental = true;
                }
        }

        public void AddCarFaxReport(Carfax carfax)
        {
            _context.AddToCarfaxes(carfax);
        }

        public void AddCarFaxReportForVinsell(manheim_Carfax carfax)
        {
            _contextVinsell.AddTomanheim_Carfax(carfax);
        }

        #endregion
    }
}
