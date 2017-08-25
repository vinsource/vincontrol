using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using Vincontrol.Web.DatabaseModel;
using SelectListItem = System.Web.Mvc.SelectListItem;
using VehicleConfiguration = Vincontrol.Web.KBBServiceEndPoint.VehicleConfiguration;

namespace Vincontrol.Web.Models
{
    public class TradeInVehicleModel
    {
        public TradeInVehicleModel(){ }

        public TradeInVehicleModel(TradeInCustomer customer)
        {
            Condition = customer.Condition;
            CustomerEmail = customer.Email;
            CustomerFirstName = customer.FirstName;
            CustomerLastName = customer.LastName;
            CustomerPhone = customer.Phone;
            SelectedMake = customer.Make;
            SelectedModel = customer.Model;
            SelectedTrim = customer.Trim;
            SelectedYear = customer.Year.HasValue ? customer.Year.Value.ToString() : String.Empty;
            Mileage = customer.Mileage.HasValue ? customer.Mileage.Value.ToString() : String.Empty;
            SelectedOptionList = customer.SelectedOptions;
            decimal result;
            TradeInFairPrice = customer.TradeInFairValue;
        }

        public IEnumerable<ExtendedSelectListItem> YearsList { get; set; }

        public IEnumerable<ExtendedSelectListItem> MakesList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ModelsList { get; set; }

        public IEnumerable<ExtendedSelectListItem> TrimsList { get; set; }

        public List<ExtendedEquipmentOption> OptionalEquipment { get; set; }

        public List<VehicleConfiguration> SpecificKBBTrimList { get; set; }

        public string TradeInCustomerId { get; set; }

        public int VehicleId { get; set; }

        public string EncryptVehicleId { get; set; }

        public string Vin { get; set; }

        public string ZipCode { get; set; }

        public bool ValidVin { get; set; }

        public string SelectedMake { get; set; }

        public string SelectedModel { get; set; }

        public string SelectedTrim { get; set; }

        public string SelectedMakeValue { get; set; }

        public string SelectedModelValue { get; set; }

        public string SelectedTrimValue { get; set; }

        public string SelectedYear { get; set; }

        public string SelectedOptions { get; set; }

        public decimal? TradeInFairPrice { get; set; }

        public decimal? TradeInGoodPrice { get; set; }

        public string TradeInVeryGoodPrice { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string Dealer { get; set; }

        public int DealerId { get; set; }

        public string DealerName { get; set; }

        public string Mileage { get; set; }

        public int MileageNumber { get; set; }

        public string Condition { get; set; }

        public decimal SelectedOptionAdjustment { get; set; }

        public string SelectedOptionList { get; set; }

        public bool CarFaxSuccess { get; set; }

        public CarFaxViewModel CarFax { get; set; }

        public string EmailTextContent { get; set; }
        public string EmailADFContent { get; set; }
        public List<string> Receivers { get; set; }
        public List<CustomerReview> ReviewList { get; set; }
        public bool SkipStepTwo { get; set; }

        public bool IsValidDealer { get; set; }
    }


    public class CustomerReview
    {
        public string ReviewContent { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string State { get; set; }
    }


}
