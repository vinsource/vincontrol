using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Helper
{
    public class ManheimMapper
    {
        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<manheim_vehicles_sold> objectSet)
        {
            return objectSet.Select(i => new MarketCarInfo()
            {
                CurrentPrice = i.Mmr,
                DateAdded = i.SaleDate,
                ExteriorColor = i.ExteriorColor,
                FuelType = i.FuelType,
                InteriorColor = i.InteriorColor,
                Make = i.Make,
                Mileage = i.Mileage.Value,
                Model = i.Model,
                Trim = i.Trim,
                Vin = i.Vin,
                BodyStyle = i.BodyStyle,
                Year = i.Year,
                AuctionCode = i.Auction,
                Engine = i.Engine,
                

            });
        }
       
    }
}
