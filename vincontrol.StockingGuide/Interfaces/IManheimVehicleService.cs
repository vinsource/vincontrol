using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IManheimVehicleService
    {
        IQueryable<manheim_vehicles> GetManheimWithTrim(string make, string model, string trim);
        IQueryable<manheim_vehicles> GetManheimWithModel(string make, string model);
        IQueryable<manheim_vehicles> GetManheimWithMake(string make);
    }
}
