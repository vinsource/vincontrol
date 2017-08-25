using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data;

namespace vincontrol.DataFeed.Model
{
    public class DealerViewModel
    {
        public string Name { get; set; }

        public string FullAddress { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public int DealerId { get; set; }

        public bool CULDFeed { get; set; }

        public string CULDId { get; set; }

        public bool AutoTraderFeed { get; set; }

        public string AutoTraderId { get; set; }

        public string AutoTraderFTPUsername { get; set; }

        public string AutoTraderFTPPassword { get; set; }

        public bool CarsComFeed { get; set; }

        public string CarsComId { get; set; }

        public bool AutoBytelFeed { get; set; }

        public string AutoBytelId { get; set; }

        public bool DealerComFeed { get; set; }

        public string DealerComId { get; set; }
        
        public bool HomeNetFeed { get; set; }

        public string HomeNetId { get; set; }
        
        public bool VastFeed { get; set; }

        public string VastId { get; set; }

        public bool DMIFeed { get; set; }

        public string DMIId { get; set; }

        public bool LTTFFeed { get; set; }

        public string LTTFId { get; set; }

        public bool CostcoFeed { get; set; }

        public string CostcoId { get; set; }
        
        public bool CommericalTruckFeed { get; set; }

        public string CommericalTruckId { get; set; }

        public bool CobaltFeed { get; set; }

        public string CobaltId { get; set; }

        public string CarFaxUserName { get; set; }

        public string CarFaxPassword { get; set; }

        public int Id { get; set; }

        public string DealerGroupId { get; set; }

        public string Country { get; set; }

        public string ZipPostal { get; set; }

        public string LeadsEmail { get; set; }

        public int EmailFormatId { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public string WebPageUrl { get; set; }

        public string WebInventoryUrl { get; set; }

        public DealerViewModel(){}

        public DealerViewModel(dealer obj)
        {
            Id = obj.Id;
            DealerGroupId = obj.DealerGroupId;
            Name = obj.Name;
            Address = obj.Address;
            City = obj.City;
            State = obj.State;
            Country = obj.Country;
            ZipPostal = obj.ZipPostal;
            Phone = obj.Phone;
            LeadsEmail = obj.LeadsEmail;
            EmailFormatId = obj.EmailFormatId.GetValueOrDefault();
            Latitude = obj.Latitude;
            WebPageUrl = obj.WebPageUrl;
            WebInventoryUrl = obj.WebInventoryUrl;
        }
    }
}
