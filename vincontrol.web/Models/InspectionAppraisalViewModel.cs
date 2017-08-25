using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;

namespace Vincontrol.Web.Models
{
    public class InspectionAppraisalViewModel
    {
        public AppraisalInfo AppraisalInfo { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public IEnumerable<WalkaroundInfo> WalkaroundInfo { get; set; }
        public IEnumerable<AppraisalAnswer> AppraisalAnswer { get; set; }
        public PreliminaryReconCost PreliminaryReconCost { get; set; }
        public IEnumerable<WalkaroundObject> WalkaroundObject { get; set; }
        public PreliminaryReconCostObject PreliminaryReconCostObject { get; set; }
    }

    public class WalkaroundObject
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public decimal Total { get; set; }
        public int ItemCount { get; set; }
        public int CarPartID { get; set; }
    }

    public class PreliminaryReconCost
    {
        public decimal MechanicalCost { get; set; }
        public decimal FrontBumperCost { get; set; }
        public decimal RearBumperCost { get; set; }
        public decimal GlassCost { get; set; }
        public decimal TireCost { get; set; }
        public decimal FrontEndCost { get; set; }
        public decimal RearEndCost { get; set; }
        public decimal DriverSideCost { get; set; }
        public decimal PassengerSideCost { get; set; }
        public decimal InteriorCost { get; set; }
        public decimal LightBulbCost { get; set; }
        public decimal OtherCost { get; set; }
        public decimal LMACost { get; set; }

        public decimal Total
        {
            get
            {
                return MechanicalCost + FrontBumperCost + RearBumperCost + GlassCost + TireCost + FrontEndCost + RearEndCost + DriverSideCost +
      PassengerSideCost + InteriorCost + LightBulbCost + OtherCost + LMACost;
            }
        }
    }

    public class PreliminaryReconCostObject
    {
        public int MechanicalCount { get; set; }
        public decimal MechanicalCost { get; set; }
        public decimal MechanicalSubTotal { get; set; }

        public int FrontBumperCount { get; set; }
        public decimal FrontBumperCost { get; set; }
        public decimal FrontBumperSubTotal { get; set; }

        public int RearBumperCount { get; set; }
        public decimal RearBumperCost { get; set; }
        public decimal RearBumperSubTotal { get; set; }

        public int GlassCount { get; set; }
        public decimal GlassCost { get; set; }
        public decimal GlassSubTotal { get; set; }

        public int TireCount { get; set; }
        public decimal TireCost { get; set; }
        public decimal TireSubTotal { get; set; }

        public int FrontEndCount { get; set; }
        public decimal FrontEndCost { get; set; }
        public decimal FrontEndSubTotal { get; set; }

        public int RearEndCount { get; set; }
        public decimal RearEndCost { get; set; }
        public decimal RearEndSubTotal { get; set; }

        public int DriverSideCount { get; set; }
        public decimal DriverSideCost { get; set; }
        public decimal DriverSideSubTotal { get; set; }

        public int PassengerSideCount { get; set; }
        public decimal PassengerSideCost { get; set; }
        public decimal PassengerSideSubTotal { get; set; }

        public int InteriorCount { get; set; }
        public decimal InteriorCost { get; set; }
        public decimal InteriorSubTotal { get; set; }

        public int LightBulbCount { get; set; }
        public decimal LightBulbCost { get; set; }
        public decimal LightBulbSubTotal { get; set; }

        public int OtherCount { get; set; }
        public decimal OtherCost { get; set; }
        public decimal OtherSubTotal { get; set; }

        public int LMACount { get; set; }
        public decimal LMACost { get; set; }
        public decimal LMASubTotal { get; set; }

        public decimal Total
        {
            get
            {
                return MechanicalSubTotal + FrontBumperSubTotal + RearBumperSubTotal + GlassSubTotal + TireSubTotal + FrontEndSubTotal + RearEndSubTotal + DriverSideSubTotal +
      PassengerSideSubTotal + InteriorSubTotal + LightBulbSubTotal + OtherSubTotal + LMASubTotal;
            }
        }
    }

    public class AppraisalInfo
    {
        public AppraisalInfo() { }

        public AppraisalInfo(Appraisal existingAppraisal)
        {
            AppraisalId = existingAppraisal.AppraisalId;
            VinNumber = existingAppraisal.Vehicle.Vin ?? "";
            StockNumber = existingAppraisal.Stock ?? "";
            Year = existingAppraisal.Vehicle.Year ?? 2012;
            Make = existingAppraisal.Vehicle.Make ?? "";
            Model = existingAppraisal.Vehicle.Model ?? "";
            Trim = existingAppraisal.Vehicle.Trim ?? "";
            ExteriorColor = existingAppraisal.ExteriorColor ?? "";
            InteriorColor = existingAppraisal.Vehicle.InteriorColor ?? "";
            Transmission = existingAppraisal.Vehicle.Tranmission ?? "";
            Odometer = existingAppraisal.Mileage.GetValueOrDefault().ToString();
            Cylinders = existingAppraisal.Vehicle.Cylinders.GetValueOrDefault().ToString();
            Liters = existingAppraisal.Vehicle.Litter.GetValueOrDefault().ToString();
            Doors = existingAppraisal.Vehicle.Doors.GetValueOrDefault().ToString();
            Fuel = existingAppraisal.Vehicle.FuelType ?? "";
            MSRP = existingAppraisal.Vehicle.Msrp.GetValueOrDefault().ToString();
            DriveType = existingAppraisal.Vehicle.DriveTrain ?? "";
            ImageUrl = existingAppraisal.Vehicle.DefaultStockImage;
            //Photo = existingAppraisal.Photo;
            Options = String.IsNullOrEmpty(existingAppraisal.AdditionalOptions)
                          ? new List<string>()
                          : existingAppraisal.AdditionalOptions.Split(',').ToList();
            Packages = String.IsNullOrEmpty(existingAppraisal.AdditionalPackages)
                           ? new List<string>()
                           : existingAppraisal.AdditionalPackages.Split(',').ToList();
            StandardOptions = String.IsNullOrEmpty(existingAppraisal.Vehicle.StandardOptions)
                                  ? new List<string>()
                                  : existingAppraisal.Vehicle.StandardOptions.Split(',').ToList();
            AppraisalDate = existingAppraisal.AppraisalDate.HasValue
                                ? existingAppraisal.AppraisalDate.Value.ToShortDateString()
                                : String.Empty;
            AppraisalTime = existingAppraisal.AppraisalDate.HasValue
                                ? existingAppraisal.AppraisalDate.Value.ToShortTimeString()
                                : String.Empty;
            EngineType = existingAppraisal.Vehicle.EngineType;
            AppraisalById = (existingAppraisal.AppraisalById ?? 0);
            ACV = existingAppraisal.ACV;
            SignatureUrl = existingAppraisal.AppraisalCustomer != null
                               ? existingAppraisal.AppraisalCustomer.SignatureUrl
                               : string.Empty;
            AppraisalBy = existingAppraisal.User1.Name;
        }

        public int AppraisalId { get; set; }
        public string VinNumber { get; set; }
        public string StockNumber { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string Transmission { get; set; }
        public string Odometer { get; set; }
        public string Cylinders { get; set; }
        public string Liters { get; set; }
        public string Doors { get; set; }
        public string Fuel { get; set; }
        public string MSRP { get; set; }
        public string DriveType { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Photo { get; set; }
        public List<string> Options { get; set; }
        public List<string> StandardOptions { get; set; }
        public List<string> Packages { get; set; }
        public string AppraisalDate { get; set; }
        public int? AppraisalById { get; set; }
        public string AppraisalTime { get; set; }
        public string EngineType { get; set; }
        public string AppraisalBy { get; set; }
        public decimal? ACV { get; set; }
        public string SignatureUrl { get; set; }
    }

    public class CustomerInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Street { get; set; }
        public byte[] Signature { get; set; }
        public string Address { get; set; }
    }

    public class WalkaroundInfo
    {
        public int Order { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Note { get; set; }
    }

    public class AppraisalAnswer
    {
        public int AppraisalId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Comment { get; set; }
        public int QuestionType { get; set; }
        public int Order { get; set; }
    }
}
