using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class SoldInventoryRepository : SqlRepository<SoldoutInventory>, ISoldInventoryRepository
    {
        public SoldInventoryRepository(ObjectContext context)
            : base(context)
        {

        }

        public IQueryable<SoldoutInventory> GetSoldVehicleForWeek(DateTime dateTime, int dealerId)
        {
           
            var startDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(-7);
            var endDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            return _objectSet.Where(i => i.DealerId == dealerId && i.DateRemoved >= startDateOfTheMonth && i.DateRemoved <= endDateOfTheMonth);
        }

        public IQueryable<SoldoutInventory> GetSoldVehicleForMonth(DateTime dateTime, int dealerId)
        {
            var startDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var endDateOfTheMonth = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).AddDays(1);
            return _objectSet.Where(i => i.DealerId == dealerId && i.DateRemoved >= startDateOfTheMonth && i.DateRemoved < endDateOfTheMonth);

        }
    }
}
