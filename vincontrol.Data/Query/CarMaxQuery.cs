using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Query
{
    public class CarMaxQuery
    {
        public static string InsertQuery(CarMaxVehicle d)
        {
            var query = @"
INSERT INTO [CarMaxVehicle]
           ([CarMaxVehicleId]
           ,[StoreId]
           ,[Year]
           ,[Make]
           ,[Model]
           ,[Trim]
           ,[Price]
           ,[Miles]
           ,[Vin]
           ,[Stock]
           ,[DriveTrain]
           ,[Transmission]
           ,[ExteriorColor]
           ,[InteriorColor]
           ,[MPGHighway]
           ,[MPGCity]
           ,[Rating]
           ,[Features]
           ,[ThumbnailPhotos]
           ,[FullPhotos]
           ,[CreatedDate]
           ,[UpdatedDate]
           ,[Used]
           ,[Certified]
           ,[Url])
     VALUES
           ({0}
           ,{1}
           ,{2}
           ,'{3}'
           ,'{4}'
           ,'{5}'
           ,{6}
           ,{7}
           ,'{8}'
           ,'{9}'
           ,'{10}'
           ,'{11}'
           ,'{12}'
           ,'{13}'
           ,{14}
           ,{15}
           ,{16}
           ,'{17}'
           ,'{18}'
           ,'{19}'
           ,'{20}'
           ,'{21}'
           ,{22}
           ,{23}
           ,'{24}')
";
            return string.Format(query, d.CarMaxVehicleId, !d.StoreId.HasValue ? "NULL" : d.StoreId.GetValueOrDefault().ToString(), d.Year, d.Make, d.Model, d.Trim, d.Price, d.Miles, d.Vin, d.Stock, d.DriveTrain, d.Transmission, d.ExteriorColor, d.InteriorColor, d.MPGHighway, d.MPGCity, d.Rating, d.Features, d.ThumbnailPhotos, d.FullPhotos, d.CreatedDate, d.UpdatedDate, d.Used ? 1 : 0, d.Certified ? 1 : 0, d.Url);
        }

        public static string InsertToYearQuery(CarMaxVehicle d)
        {
            var query = @"INSERT INTO year ([AutoTrader],CarsCom,Carmax,Vin,StartingPrice,CurrentPrice,DateAdded,LastUpdated,LastUpdatedPrice,Make,Year1,Trim,DriveType,ExteriorColor,InteriorColor,UsedNew,Mileage,ZipCode,Dealershipname,Address,City,State,Latitude,Longitude,CarmaxStoreId,CarMaxVehicleId,CarmaxInstalledFeature,CarmaxListingUrl,CarmaxThumbnailUrl) VALUES ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}',{15},{16},'{17}','{18}','{19}','{20}','{21}',{22},{23},{24},{25},'{26}','{27}','{28}')
";
            return string.Format(query, 0, 0, 1, d.Vin, d.Price, d.Price, d.CreatedDate, d.UpdatedDate, d.UpdatedDate, d.Make, d.Year, d.Trim ?? string.Empty, d.DriveTrain, d.ExteriorColor, d.InteriorColor, d.Used ? 1 : 0, d.Miles, 
                d.CarMaxStore != null ? d.CarMaxStore.ZipCode.ToString() : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.FullName : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.Address : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.City : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.State : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.Latitude : 0,
                d.CarMaxStore != null ? d.CarMaxStore.Longitude : 0,
                d.CarMaxStore != null ? d.CarMaxStore.StoreId : 0,
                d.CarMaxVehicleId,
                d.Features ?? string.Empty,
                d.Url,
                d.ThumbnailPhotos != null ? d.ThumbnailPhotos.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0] : string.Empty);
        }

        public static string InsertToYearSoldQuery(CarMaxVehicleSoldOut d)
        {
            var query = @"INSERT INTO yearsold ([AutoTrader],CarsCom,Carmax,Vin,StartingPrice,CurrentPrice,DateAdded,LastUpdated,LastUpdatedPrice,Make,Year,Trim,DriveType,ExteriorColor,InteriorColor,UsedNew,Mileage,ZipCode,Dealershipname,Address,City,State,Latitude,Longitude,CarmaxStoreId,CarMaxVehicleId,CarmaxInstalledFeature,CarmaxListingUrl,CarmaxThumbnailUrl) VALUES ({0},{1},{2},'{3}',{4},{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}',{15},{16},'{17}','{18}','{19}','{20}','{21}',{22},{23},{24},{25},'{26}','{27}','{28}')
";
            return string.Format(query, 0, 0, 1, d.Vin, d.Price, d.Price, DataCommonHelper.GetChicagoDateTime(DateTime.Now), DataCommonHelper.GetChicagoDateTime(DateTime.Now), DataCommonHelper.GetChicagoDateTime(DateTime.Now), d.Make, d.Year, d.Trim, d.DriveTrain, d.ExteriorColor, d.InteriorColor, d.Used ? 1 : 0, d.Miles,
                d.CarMaxStore != null ? d.CarMaxStore.ZipCode.ToString() : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.FullName : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.Address : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.City : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.State : string.Empty,
                d.CarMaxStore != null ? d.CarMaxStore.Latitude : 0,
                d.CarMaxStore != null ? d.CarMaxStore.Longitude : 0,
                d.CarMaxStore != null ? d.CarMaxStore.StoreId : 0,
                d.CarMaxVehicleId,
                d.Features,
                d.Url,
                d.ThumbnailPhotos != null ? d.ThumbnailPhotos.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0] : string.Empty);
        }

        public static string UpdateQuery(CarMaxVehicle d)
        {
            var query = string.Format("UPDATE CarMaxVehicle SET ");
            query += string.Format("StoreId = {0},", !d.StoreId.HasValue ? "NULL" : d.StoreId.Value.ToString());
            query += string.Format("Year = {0},", d.Year);
            query += string.Format("Make = '{0}',", d.Make);
            query += string.Format("Model = '{0}',", d.Model);
            query += string.Format("Trim = '{0}',", d.Trim);
            query += string.Format("Price = {0},", d.Price);
            query += string.Format("Miles = {0},", d.Miles);
            query += string.Format("Vin = '{0}',", d.Vin);
            query += string.Format("Stock = '{0}',", d.Stock);
            query += string.Format("DriveTrain = '{0}',", d.DriveTrain);
            query += string.Format("Transmission = '{0}',", d.Transmission);
            query += string.Format("ExteriorColor = '{0}',", d.ExteriorColor);
            query += string.Format("InteriorColor = '{0}',", d.InteriorColor);
            query += string.Format("MPGHighway = {0},", d.MPGHighway);
            query += string.Format("MPGCity = {0},", d.MPGCity);
            query += string.Format("Rating = {0},", d.Rating);
            query += string.Format("Features = '{0}',", d.Features);
            query += string.Format("ThumbnailPhotos = '{0}',", d.ThumbnailPhotos);
            query += string.Format("FullPhotos = '{0}',", d.FullPhotos);
            query += string.Format("Certified = {0},", d.Certified ? 1 : 0);
            query += string.Format("Used = {0},", d.Used ? 1 : 0);
            query += string.Format("UpdatedDate = '{0}'", DataCommonHelper.GetChicagoDateTime(DateTime.Now));            
            query += string.Format(" WHERE CarMaxVehicleId = ") + d.CarMaxVehicleId;

            return query;
        }

        public static string MarkSoldQuery()
        {
            var query = @"DELETE FROM CarMaxVehicleSoldOut WHERE DateStamp < (GETDATE() - 60)  
    INSERT INTO CarMaxVehicleSoldOut(VehicleId,CarMaxVehicleId,StoreId,Year,Make,Model,Trim,Price,Miles,Vin,Stock,DriveTrain,Transmission,ExteriorColor,InteriorColor,MPGHighway,MPGCity,Rating,Features,ThumbnailPhotos,FullPhotos,DateStamp,Used,Certified,Url)
    SELECT VehicleId,CarMaxVehicleId,StoreId,Year,Make,Model,Trim,Price,Miles,Vin,Stock,DriveTrain,Transmission,ExteriorColor,InteriorColor,MPGHighway,MPGCity,Rating,Features,ThumbnailPhotos,FullPhotos,UpdatedDate,Used,Certified,Url
    FROM CarMaxVehicle WHERE UpdatedDate < (GETDATE() - 1)
";
            return query;
        }
    }
}
