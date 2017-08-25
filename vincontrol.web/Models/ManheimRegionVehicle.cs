using vincontrol.Data.Model;

namespace Vincontrol.Web.Models
{
    public class ManheimRegionVehicle 
    {
        public manheim_vehicles ManheimVehicle { get; set; }
        public string Region { get; set; }
        public string VinsellUrl { get; set; }

        public ManheimRegionVehicle(manheim_vehicles vehicle, string region,int userId)
        {
            Region = region;
            ManheimVehicle = vehicle;
            VinsellUrl = "http://vinsell.com/Auction/DetailVehicleFromVincontrol?userId=" + userId + "&id=" + vehicle.Id;
        }
    }
}