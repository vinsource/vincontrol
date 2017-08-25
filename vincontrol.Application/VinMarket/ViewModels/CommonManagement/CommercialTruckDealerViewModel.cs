using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model.Truck;

namespace vincontrol.Application.VinMarket.ViewModels.CommonManagement
{
    public class CommercialTruckDealerViewModel
    {
        public CommercialTruckDealerViewModel() { }
        
        public CommercialTruckDealerViewModel(CommercialTruckDealer obj)
        {
            TruckDealerId = obj.CommercialTruckDealerId;
            DealerId = obj.DealerId;
            DealerGroupId = obj.DealerGroupId;
            Name = obj.Name;
            Address = obj.Address;
            City = obj.City;
            State = obj.State;
            ZipCode = obj.ZipCode;
            Phone = obj.Phone;
            Email = obj.Email;
            Latitude = obj.Latitude;
            Longitude = obj.Longitude;
            DateStamp = obj.DateStamp;
        }

        public int TruckDealerId { get; set; }
        public int DealerId { get; set; }
        public string DealerGroupId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime DateStamp { get; set; }
    }

    public class CommercialTruckViewModel
    {
        public CommercialTruckViewModel(){}

        public CommercialTruckViewModel(Data.Model.Truck.CommercialTruck obj)
        {
            TruckId = obj.TruckId;
            CommercialTruckId = obj.CommercialTruckId;
            DealerId = obj.DealerId.GetValueOrDefault();
            Year = obj.Year;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
            Vin = obj.Vin;
            Stock = obj.Stock;
            ExteriorColor = obj.ExteriorColor;
            InteriorColor = obj.InteriorColor;
            BodyStyle = obj.BodyStyle;
            Engine = obj.Engine;
            Litter = obj.Litter;
            Fuel = obj.Fuel;
            Transmission = obj.Transmission;
            Drivetrain = obj.DriveTrain;
            Price = obj.Price;
            Mileage = obj.Mileage;
            Description = obj.Description;
            Package = obj.Package;
            IsNew = obj.IsNew;
            Images = obj.Images;
            Category = obj.Category;
            Class = obj.Class;
            Url = obj.Url;
            DateStamp = obj.DateStamp;
            Updated = obj.Updated;
        }

        public int TruckId { get; set; }
        public int CommercialTruckId { get; set; }
        public int DealerId { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Vin { get; set; }
        public string Stock { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string BodyStyle { get; set; }
        public string Engine { get; set; }
        public string Litter { get; set; }
        public string Fuel { get; set; }
        public string Transmission { get; set; }
        public string Drivetrain { get; set; }
        public int Price { get; set; }
        public int Mileage { get; set; }
        public string Description { get; set; }
        public string Package { get; set; }
        public bool IsNew { get; set; }
        public string Images { get; set; }
        public string Category { get; set; }
        public string Class { get; set; }
        public string Url { get; set; }
        public DateTime DateStamp { get; set; }
        public DateTime Updated { get; set; }
    }

    public class SoldOutCommercialTruckViewModel
    {
        public SoldOutCommercialTruckViewModel(){}

        public SoldOutCommercialTruckViewModel(Data.Model.Truck.CommercialTruckSoldOut obj)
        {
            SoldOutTruckId = obj.SoldOutTruckId;
            TruckId = obj.TruckId;
            CommercialTruckId = obj.CommercialTruckId;
            DealerId = obj.DealerId.GetValueOrDefault();
            Year = obj.Year;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
            Vin = obj.Vin;
            Stock = obj.Stock;
            ExteriorColor = obj.ExteriorColor;
            InteriorColor = obj.InteriorColor;
            BodyStyle = obj.BodyStyle;
            Engine = obj.Engine;
            Litter = obj.Litter;
            Fuel = obj.Fuel;
            Transmission = obj.Transmission;
            Drivetrain = obj.DriveTrain;
            Price = obj.Price;
            Mileage = obj.Mileage;
            Description = obj.Description;
            Package = obj.Package;
            IsNew = obj.IsNew;
            Images = obj.Images;
            Category = obj.Category;
            Class = obj.Class;
            Url = obj.Url;
            DateStamp = obj.DateStamp;
        }

        public int SoldOutTruckId { get; set; }
        public int TruckId { get; set; }
        public int CommercialTruckId { get; set; }
        public int DealerId { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Vin { get; set; }
        public string Stock { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string BodyStyle { get; set; }
        public string Engine { get; set; }
        public string Litter { get; set; }
        public string Fuel { get; set; }
        public string Transmission { get; set; }
        public string Drivetrain { get; set; }
        public int Price { get; set; }
        public int Mileage { get; set; }
        public string Description { get; set; }
        public string Package { get; set; }
        public bool IsNew { get; set; }
        public string Images { get; set; }
        public string Category { get; set; }
        public string Class { get; set; }
        public string Url { get; set; }
        public DateTime DateStamp { get; set; }
    }
}
