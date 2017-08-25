using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using vincontrol.Constant;

namespace vincontrol.Data.Model.Truck
{
    #region Partial Class

    public partial class CommercialTruckSoldOut
    {
        public CommercialTruckSoldOut(){}

        public CommercialTruckSoldOut(CommercialTruck obj)
        {
            TruckId = obj.TruckId;
            CommercialTruckId = obj.CommercialTruckId;
            DealerId = obj.DealerId.Equals(0) ? (int?) null : obj.DealerId;
            Year = obj.Year;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
            Vin = obj.Vin;
            Stock = obj.Stock;
            ExteriorColor = obj.ExteriorColor;
            InteriorColor = obj.InteriorColor;
            BodyStyle = obj.BodyStyle;
            Litter = obj.Litter;
            Engine = obj.Engine;
            Fuel = obj.Fuel;
            Transmission = obj.Transmission;
            DriveTrain = obj.DriveTrain;
            Price = obj.Price;
            Mileage = obj.Mileage;
            Description = obj.Description;
            Package = obj.Package;
            IsNew = obj.IsNew;
            Images = obj.Images;
            Category = obj.Category;
            Class = obj.Class;
            Url = obj.Url;
            DateStamp = DateTime.Now;
        }
    }

    #endregion
}
