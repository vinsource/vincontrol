using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.VinMarket.ViewModels.CommonManagement
{
    public class CarMaxVehicleViewModel
    {
        public CarMaxVehicleViewModel() { }

        public CarMaxVehicleViewModel(CarMaxVehicle obj) 
        {
            VehicleId = obj.VehicleId;
            CarMaxVehicleId = obj.CarMaxVehicleId;
            StoreId = obj.StoreId.GetValueOrDefault();
            Year = obj.Year;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
            Price = obj.Price;
            Miles = obj.Miles;
            Vin = obj.Vin;
            Stock = obj.Stock;
            DriveTrain = obj.DriveTrain;
            Transmission = obj.Transmission;
            ExteriorColor = obj.ExteriorColor;
            InteriorColor = obj.InteriorColor;
            MPGHighway = obj.MPGHighway;
            MPGCity = obj.MPGCity;
            Rating = obj.Rating;
            Features = obj.Features;
            ThumbnailPhotos = obj.ThumbnailPhotos;
            FullPhotos = obj.FullPhotos;
            Certified = obj.Certified;
            Used = obj.Used;
            Url = obj.Url;
            CreatedDate = obj.CreatedDate;
            UpdatedDate = obj.UpdatedDate;
        }

        public long VehicleId { get; set; }
        public long CarMaxVehicleId { get; set; }
        public int StoreId { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int Price { get; set; }
        public int Miles { get; set; }
        public string Vin { get; set; }
        public string Stock { get; set; }
        public string DriveTrain { get; set; }
        public string Transmission { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public int MPGHighway { get; set; }
        public int MPGCity { get; set; }
        public decimal Rating { get; set; }
        public string Features { get; set; }
        public string ThumbnailPhotos { get; set; }
        public string FullPhotos { get; set; }
        public bool Certified { get; set; }
        public bool Used { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class CarMaxStoreViewModel
    {
        public CarMaxStoreViewModel() { }

        public CarMaxStoreViewModel(CarMaxStore obj) 
        {
            StoreId = obj.StoreId;
            CarMaxStoreId = obj.CarMaxStoreId;
            Name = obj.Name;
            FullName = obj.FullName;
            Url = obj.Url;
            Address = obj.Address;
            City = obj.City;
            State = obj.State;
            ZipCode = obj.ZipCode;
            Phone = obj.Phone;
            CreatedDate = obj.CreatedDate;
            UpdatedDate = obj.UpdatedDate;
            Latitude = obj.Latitude.GetValueOrDefault();
            Longitude = obj.Longitude.GetValueOrDefault();
        }

        public int StoreId { get; set; }
        public long CarMaxStoreId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public string Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
