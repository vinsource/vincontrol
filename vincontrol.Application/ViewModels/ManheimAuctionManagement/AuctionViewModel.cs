using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    [DataContract]
    [Serializable]
    public class VehicleViewModel
    {
        public VehicleViewModel() { }

        public VehicleViewModel(manheim_vehicles obj) 
        {
            Id = obj.Id;
            Vin = obj.Vin;
            Year = obj.Year;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
            Mileage = obj.Mileage.GetValueOrDefault();
            FuelType = obj.FuelType;
            Engine = obj.Engine;
            Litters = obj.Litters;
            Doors = obj.Doors;
            BodyStyle = obj.BodyStyle;
            VehicleType = obj.VehicleType;
            DriveTrain = obj.DriveTrain;
            Transmission = obj.Transmission;
            ExteriorColor = obj.ExteriorColor;
            InteriorColor = obj.InteriorColor;
            AsIs = obj.AsIs.GetValueOrDefault();
            Cr = obj.Cr.Contains("Condition Report") || obj.Cr.Contains("Not") || obj.Cr.Contains("Seller") || obj.Cr.Contains("Damage Estimate") ? "N/A" : obj.Cr;
            Mmr = obj.Mmr;
            MmrAbove = obj.MmrAbove.GetValueOrDefault();
            MmrBelow = obj.MmrBelow.GetValueOrDefault();
            Lane = obj.Lane.GetValueOrDefault();
            Run = obj.Run.GetValueOrDefault();
            AuctionDate = obj.SaleDate.GetValueOrDefault();
            Status = obj.Status;
            Region = obj.Auction;
            Url = obj.Url;
            Images = obj.Images;
            Seller = obj.Seller;
            DateStamp = obj.DateStamp.GetValueOrDefault();
            Equipments = String.IsNullOrEmpty(obj.Equipment) ? new List<string>() : obj.Equipment.Split(',').ToList();
            Airbags = obj.Airbags;
            Stereo = obj.Stereo;
            InteriorType = obj.InteriorType;
            IsFavorite = obj.IsFavorite;
            HasNote = obj.HasNote;
            RegionName = obj.AuctionName;
            Comment = obj.Comment;
            DateStamp = obj.DateStamp.GetValueOrDefault();
            Category = obj.Category;
            LaneDescription = obj.LaneDescription;
            CrUrl = obj.CrUrl;
            IsSold = false;
            IsPast = obj.SaleDate.GetValueOrDefault() < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public VehicleViewModel(manheim_vehicles_sold obj)
        {
            Id = obj.Id;
            Vin = obj.Vin;
            Year = obj.Year;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
            Mileage = obj.Mileage.GetValueOrDefault();
            FuelType = obj.FuelType;
            Engine = obj.Engine;
            Litters = obj.Litters;
            Doors = obj.Doors;
            BodyStyle = obj.BodyStyle;
            VehicleType = obj.VehicleType;
            DriveTrain = obj.DriveTrain;
            Transmission = obj.Transmission;
            ExteriorColor = obj.ExteriorColor;
            InteriorColor = obj.InteriorColor;
            AsIs = obj.AsIs.GetValueOrDefault();
            Cr = obj.Cr.Contains("Condition Report") || obj.Cr.Contains("Not") || obj.Cr.Contains("Seller") || obj.Cr.Contains("Damage Estimate") ? "N/A" : obj.Cr;
            Mmr = obj.Mmr;
            MmrAbove = obj.MmrAbove.GetValueOrDefault();
            MmrBelow = obj.MmrBelow.GetValueOrDefault();
            Lane = obj.Lane.GetValueOrDefault();
            Run = obj.Run.GetValueOrDefault();
            AuctionDate = obj.SaleDate.GetValueOrDefault();
            Status = obj.Status;
            Region = obj.Auction;
            Url = obj.Url;
            Images = obj.Images;
            Seller = obj.Seller;
            DateStamp = obj.DateStamp.GetValueOrDefault();
            Equipments = String.IsNullOrEmpty(obj.Equipment) ? new List<string>() : obj.Equipment.Split(',').ToList();
            Airbags = obj.Airbags;
            Stereo = obj.Stereo;
            InteriorType = obj.InteriorType;
            IsFavorite = obj.IsFavorite;
            HasNote = obj.HasNote;
            RegionName = obj.AuctionName;
            Comment = obj.Comment;
            DateStamp = obj.DateStamp.GetValueOrDefault();
            SoldDate = obj.DateStampSold.GetValueOrDefault();
            Category = obj.Category;
            LaneDescription = obj.LaneDescription;
            CrUrl = obj.CrUrl;
            IsSold = true;
            IsPast = obj.SaleDate.GetValueOrDefault() < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public VehicleViewModel(Inventory obj)
        {
            Year = obj.Vehicle.Year.GetValueOrDefault();
            Make = obj.Vehicle.Make;
            Vin = obj.Vehicle.Vin;
            Model = obj.Vehicle.Model;
            VehicleId = obj.Vehicle.VehicleId;
            ManheimTrimid = obj.Vehicle.ManheimTrimId.GetValueOrDefault();
        }

        public VehicleViewModel(SoldoutInventory obj)
        {
            Year = obj.Vehicle.Year.GetValueOrDefault();
            Make = obj.Vehicle.Make;
            Vin = obj.Vehicle.Vin;
            Model = obj.Vehicle.Model;
            VehicleId = obj.Vehicle.VehicleId;
            ManheimTrimid = obj.Vehicle.ManheimTrimId.GetValueOrDefault();
        }

        public int Id { get; set; }
        public int NextId { get; set; }
        public int PreviousId { get; set; }
        public int VehicleId { get; set; }
        [DataMember(Name = "vin")]
        public string Vin { get; set; }
        [DataMember(Name = "year")]
        public int Year { get; set; }
        [DataMember(Name = "make")]
        public string Make { get; set; }
        [DataMember(Name = "model")]
        public string Model { get; set; }
        [DataMember(Name = "trim")]
        public string Trim { get; set; }
        [DataMember(Name = "mileage")]
        public int Mileage { get; set; }
        public string FuelType { get; set; }
        public string Engine { get; set; }
        public string Litters { get; set; }
        public int Doors { get; set; }
        public string BodyStyle { get; set; }
        public string VehicleType { get; set; }
        public string DriveTrain { get; set; }
        public string Transmission { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public bool AsIs { get; set; }
        [DataMember(Name = "cr")]
        public string Cr { get; set; }
        public string CrUrl { get; set; }
        [DataMember(Name = "mmr")]
        public int Mmr { get; set; }
        public int MmrAbove { get; set; }
        public int MmrBelow { get; set; }
        [DataMember(Name = "lane")]
        public int Lane { get; set; }
        [DataMember(Name = "run")]
        public int Run { get; set; }
        public DateTime AuctionDate { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }
        public string RegionName { get; set; }
        public string State { get; set; }
        public string Url { get; set; }
        public string Images { get; set; }
        [DataMember(Name = "seller")]
        public string Seller { get; set; }
        public string Note { get; set; }
        public DateTime DateStamp { get; set; }
        public DateTime SoldDate { get; set; }
        public bool IsFavorite { get; set; }
        public bool HasNote { get; set; }
        public bool IsSold { get; set; }
        public bool IsPast { get; set; }
        public List<string> Equipments { get; set; }
        public string Airbags { get; set; }
        public string Stereo { get; set; }
        public string InteriorType { get; set; }
        public string Comment { get; set; }
        public string Category { get; set; }
        public string LaneDescription { get; set; }
        public int DealerId { get; set; }
        public int ManheimTrimid { get; set; }

        public CarFaxViewModel CarFax { get; set; }
        public List<ManheimWholesaleViewModel> ManheimWholesale { get; set; }
        public List<ManheimTransactionViewModel> ManheimTransactions { get; set; }
        public List<SmallKarPowerViewModel> KarpowerWholesale { get; set; }
        public ChartGraph MarketValue { get; set; }
    }
}
