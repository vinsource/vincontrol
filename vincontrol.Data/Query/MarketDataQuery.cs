using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Query
{
    public class MarketDataQuery
    {
        public class Table
        {
            public class AutoTrader
            {
                public const string Id = "AutoTraderListingId";
                public const string Region1 = "region1_autotrader";
                public const string Region2 = "region2_autotrader";
                public const string Region3 = "region3_autotrader";
                public const string Region41 = "region4_p1_autotrader";
                public const string Region42 = "region4_p2_autotrader";
                public const string Region51 = "region5_p1_autotrader";
                public const string Region52 = "region5_p2_autotrader";
                public const string Region6 = "region6_autotrader";
                public const string Region7 = "region7_autotrader";
                public const string Region8 = "region8_autotrader";
                public const string Region9 = "region9_autotrader";
                public const string Region10 = "region10_autotrader";
            }

            public class CarsCom
            {
                public const string Id = "CarsComListingId";
                public const string Region1 = "region1_carscom";
                public const string Region2 = "region2_carscom";
                public const string Region3 = "region3_carscom";
                public const string Region41 = "region4_p1_carscom";
                public const string Region42 = "region4_p2_carscom";
                public const string Region51 = "region5_p1_carscom";
                public const string Region52 = "region5_p2_carscom";
                public const string Region6 = "region6_carscom";
                public const string Region7 = "region7_carscom";
                public const string Region8 = "region8_carscom";
                public const string Region9 = "region9_carscom";
                public const string Region10 = "region10_carscom";
            }
        }

        public static string InsertQuery(string tableName, marketdata d)
        {
            if (!string.IsNullOrEmpty(d.CountyName)) d.CountyName = d.CountyName.Replace("'", "''");
            if (!string.IsNullOrEmpty(d.AutoTraderListingName)) d.AutoTraderListingName = d.AutoTraderListingName.Replace("'", "''");
            if (!string.IsNullOrEmpty(d.CarsComListingName)) d.CarsComListingName = d.CarsComListingName.Replace("'", "''");
            if (!string.IsNullOrEmpty(d.Dealershipname)) d.Dealershipname = d.Dealershipname.Replace("'", "''");
            if (!string.IsNullOrEmpty(d.Address)) d.Address = d.Address.Replace("'", "''");
            if (!string.IsNullOrEmpty(d.City)) d.City = d.City.Replace("'", "''");
            if (!string.IsNullOrEmpty(d.State)) d.State = d.State.Replace("'", "''");

            var query = @"INSERT INTO {0} ([Year],[Make],[Model],[Trim],[Vin],[AutoTraderStockNo],[CarsComStockNo],[ExteriorColor],[InteriorColor],[BodyStyle],[StartingPrice],[CurrentPrice],[MSRP],[Mileage],[Tranmission],[Engine],[DriveType],[Doors],[FuelType],[AutoTrader],[AutoTraderListingId],[AutoTraderListingName],[AutoTraderListingURL],[AutoTraderThumbnailURL],[AutoTraderDescription],[AutoTraderInstalledFeatures],[CarsCom],[CarsComListingId],[CarsComListingName],[CarsComListingURL],[CarsComThumbnailURL],[CarsComDescription],[CarsComInstalledFeatures],[Ebay],[EbayListingId],[EbayListingName],[EbayURL],[EbayThumbnailURL],[EbayDescription],[EbayInstalledFeatures],[CarFaxURL],[CarFaxType],[AutoCheckURL],[Navigation],[SunRoof],[MoonRoof],[Certified],[Franchise],[UsedNew],[VinControlDealerId],[AutoTraderDealerId],[CarsComDealerId],[Dealershipname],[Address],[City],[CountyName],[State],[ZipCode],[Latitude],[Longitude],[DateAdded],[LastUpdated],[LastUpdatedPrice]) VALUES ({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11},{12},{13},{14},'{15}','{16}','{17}','{18}','{19}',{20},{21},'{22}','{23}','{24}','{25}','{26}',{27},'{28}','{29}','{30}','{31}','{32}','{33}',{34},'{35}','{36}','{37}','{38}','{39}','{40}','{41}',{42},'{43}',{44},{45},{46},{47},{48},{49},{50},{51},{52},'{53}','{54}','{55}','{56}','{57}','{58}',{59},{60},'{61}','{62}','{63}')" + Environment.NewLine;
            return string.Format(query, tableName, d.Year ?? 0, d.Make, d.Model, d.Trim, d.Vin, d.AutoTraderStockNo ?? string.Empty, d.CarsComStockNo ?? string.Empty, d.ExteriorColor ?? string.Empty, d.InteriorColor ?? string.Empty, d.BodyStyle ?? string.Empty, d.StartingPrice ?? 0, d.CurrentPrice ?? 0, 0, d.Mileage ?? 0, d.Tranmission ?? string.Empty, d.Engine ?? string.Empty, d.DriveType ?? string.Empty, d.Doors ?? string.Empty, d.FuelType ?? string.Empty, d.AutoTrader.Value ? 1 : 0, d.AutoTraderListingId ?? 0, d.AutoTraderListingName ?? string.Empty, d.AutoTraderListingURL ?? string.Empty, d.AutoTraderThumbnailURL ?? string.Empty, d.AutoTraderDescription ?? string.Empty, d.AutoTraderInstalledFeatures ?? string.Empty, d.CarsCom.Value ? 1 : 0, d.CarsComListingId, d.CarsComListingName ?? string.Empty, d.CarsComListingURL ?? string.Empty, d.CarsComThumbnailURL ?? string.Empty, d.CarsComDescription ?? string.Empty, d.CarsComInstalledFeatures ?? string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, d.CarFaxURL ?? string.Empty, d.CarFaxType ?? 0, d.AutoCheckURL ?? string.Empty, d.Navigation.Value ? 1 : 0, d.SunRoof.Value ? 1 : 0, d.MoonRoof.Value ? 1 : 0, d.Certified.Value ? 1 : 0, d.Franchise.HasValue && d.Franchise.Value ? 1 : 0, d.UsedNew.Value ? 1 : 0, d.VinControlDealerId ?? 0, d.AutoTraderDealerId ?? 0, d.CarsComDealerId ?? 0, d.Dealershipname ?? string.Empty, d.Address ?? string.Empty, d.City ?? string.Empty, d.CountyName ?? string.Empty, d.State ?? string.Empty, d.ZipCode ?? string.Empty, d.Latitude ?? 0, d.Longitude?? 0, d.DateAdded, d.LastUpdated, d.LastUpdatedPrice);
        }

        public static string InsertToSoldQuery(string tableName, string fieldName, int id)
        {
            var query = @"INSERT INTO [region_dailysold] ([Year],[Make],[Model],[Trim],[Vin],[AutoTraderStockNo],[CarsComStockNo],[ExteriorColor],[InteriorColor],[BodyStyle],[StartingPrice],[CurrentPrice],[MSRP],[Mileage],[Tranmission],[Engine],[DriveType],[Doors],[FuelType],[AutoTrader],[AutoTraderListingId],[AutoTraderListingName],[AutoTraderListingURL],[AutoTraderThumbnailURL],[AutoTraderDescription],[AutoTraderInstalledFeatures],[CarsCom],[CarsComListingId],[CarsComListingName],[CarsComListingURL],[CarsComThumbnailURL],[CarsComDescription],[CarsComInstalledFeatures],[Ebay],[EbayListingId],[EbayListingName],[EbayURL],[EbayThumbnailURL],[EbayDescription],[EbayInstalledFeatures],[CarFaxURL],[CarFaxType],[AutoCheckURL],[Navigation],[SunRoof],[MoonRoof],[Certified],[Franchise],[UsedNew],[VinControlDealerId],[AutoTraderDealerId],[CarsComDealerId],[Dealershipname],[Address],[City],[CountyName],[State],[ZipCode],[Latitude],[Longitude],[DateAdded],[LastUpdated],[LastUpdatedPrice]) SELECT TOP 1 [Year],[Make],[Model],[Trim],[Vin],[AutoTraderStockNo],[CarsComStockNo],[ExteriorColor],[InteriorColor],[BodyStyle],[StartingPrice],[CurrentPrice],[MSRP],[Mileage],[Tranmission],[Engine],[DriveType],[Doors],[FuelType],[AutoTrader],[AutoTraderListingId],[AutoTraderListingName],[AutoTraderListingURL],[AutoTraderThumbnailURL],[AutoTraderDescription],[AutoTraderInstalledFeatures],[CarsCom],[CarsComListingId],[CarsComListingName],[CarsComListingURL],[CarsComThumbnailURL],[CarsComDescription],[CarsComInstalledFeatures],[Ebay],[EbayListingId],[EbayListingName],[EbayURL],[EbayThumbnailURL],[EbayDescription],[EbayInstalledFeatures],[CarFaxURL],[CarFaxType],[AutoCheckURL],[Navigation],[SunRoof],[MoonRoof],[Certified],[Franchise],[UsedNew],[VinControlDealerId],[AutoTraderDealerId],[CarsComDealerId],[Dealershipname],[Address],[City],[CountyName],[State],[ZipCode],[Latitude],[Longitude],[DateAdded],[LastUpdated],[LastUpdatedPrice] FROM {0} WHERE {1} = {2}" + Environment.NewLine;
            return string.Format(query, tableName, fieldName, id);
        }

        public static string DeleteQuery(string tableName, string fieldName, List<int> ids)
        {
            return string.Format("DELETE FROM {0} WHERE {1} IN ({2})", tableName, fieldName, String.Join(",", ids) + Environment.NewLine);
        }

        public static string DeleteQuery(string tableName, string fieldName, int id)
        {
            return string.Format("DELETE FROM {0} WHERE {1} = {2}", tableName, fieldName, id);
        }

        public static string UpdateQuery(string tableName, string fieldName, int id, int year, string make, string model, int price, int mileage, bool certified, bool used, string image, string address, string city, string state, string zip, double? lat, double? lon, int autoTraderDealerId = 0, int carscomDealerId = 0)
        {
            var query = string.Format("UPDATE {0} SET ", tableName);
            query += string.Format("Certified = {0},", certified ? 1 : 0);
            query += string.Format("UsedNew = {0},", used ? 1 : 0);
            query += string.Format("Year = {0},", year);
            query += string.Format("Make = '{0}',", make);
            query += string.Format("Model = '{0}',", model);
            //query += string.Format("AutoTraderThumbnailURL = '{0}',", AutoTraderThumbnailURL);
            query += string.Format("Mileage = {0},", mileage);
            query += string.Format("CurrentPrice = {0},", price);
            query += string.Format("Address = '{0}',", address.Replace("'", "''"));
            query += string.Format("City = '{0}',", city.Replace("'","''"));
            query += string.Format("State = '{0}',", state);
            query += string.Format("ZipCode = '{0}',", zip);
            query += string.Format("Latitude = {0},", lat.GetValueOrDefault());
            query += string.Format("Longitude = {0},", lon.GetValueOrDefault());
            query += tableName.Contains("carscom") ? string.Format("CarsComThumbnailURL = '{0}',", image) : string.Format("AutoTraderThumbnailURL = '{0}',", image);
            if (autoTraderDealerId != 0) query += string.Format("AutoTraderDealerId = {0},", autoTraderDealerId);
            if (carscomDealerId != 0) query += string.Format("CarsComDealerId = {0},", carscomDealerId);
            query += string.Format("LastUpdated = '{0}',", DataCommonHelper.GetChicagoDateTime(DateTime.Now));
            query += string.Format("LastUpdatedPrice = '{0}'", DataCommonHelper.GetChicagoDateTime(DateTime.Now));            
            query += (tableName.Contains("carscom") ? string.Format(" WHERE {0} = '{1}'", fieldName, id) : string.Format(" WHERE {0} = ", fieldName) + id);
            query += Environment.NewLine;

            return query;
        }
    }
}
