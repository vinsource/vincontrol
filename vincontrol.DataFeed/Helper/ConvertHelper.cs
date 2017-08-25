using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using vincontrol.Backend.Data;
using vincontrol.DataFeed.Model;

namespace vincontrol.DataFeed.Helper
{
    public class ConvertHelper
    {
        private MySQLHelper _mySqlHelper;
        private FTPClientHelper _ftpClientHelper;
        private FTPExcelHelper _ftpExcelHelper;
        private XMLHelper _xmlHelper;
        private CommonHelper _commonHelper;
        private EmailHelper _emailHelper;
        private AutoServiceHelper _autoServiceHelper;
        private const string Root = "public_html";
        private LogFile _log;

        public string FeedUrl { get; set; }

        public ConvertHelper()
        {
            _mySqlHelper = new MySQLHelper();
            _ftpExcelHelper = new FTPExcelHelper();
            _ftpClientHelper = new FTPClientHelper();
            _xmlHelper = new XMLHelper();
            _commonHelper = new CommonHelper();
            _emailHelper = new EmailHelper();
            _autoServiceHelper = new AutoServiceHelper();
        }
        
        public void ImportFileToDatabase(int dealerId)
        {
            var feedUrl = _mySqlHelper.GetImportDataFeedPath(dealerId);
            if (!String.IsNullOrEmpty(feedUrl))
            {
                try
                {
                    // check to see the feed url has "public_html" or not
                    if (!feedUrl.Contains(Root))
                    {
                        if (feedUrl.Substring(0, 1).Equals("/") || feedUrl.Substring(0, 1).Equals("\\"))
                        {
                            feedUrl = Root + feedUrl;
                        }
                        else
                        {
                            feedUrl = Root + "\\" + feedUrl;
                        }
                    }

                    FeedUrl = feedUrl;
                    ImportFileToDatabase(dealerId, FTPHelper.DownloadFromFtpServer(feedUrl));
                }
                catch (Exception ex)
                {
                    //throw new InvalidOperationException("Cannot open file on ftp");
                    throw;
                }
            }
        }

        public void ImportFileToDatabaseByProfile(int profileId)
        {
            try
            {
                foreach (var dealerId in _mySqlHelper.GetDealerListByProfileId(profileId))
                {
                    ImportFileToDatabase(dealerId);
                }
            }
            catch (Exception ex)
            {
                //throw new InvalidOperationException("Error happens when executing data feed import by profile.");
                throw;
            }
        }

        public void ImportFileToDatabaseByProfileIdAndDealerId(int profileId, int dealerId, Stream data)
        {
            var fileToArray = CommonHelper.GetBytes(data);
            var dataFromFile = GetInventoriesFromFile(profileId, dealerId, data);
            if (dataFromFile.Count == 0) return;

            var fileName = (String.IsNullOrEmpty(FeedUrl)
                                    ? DateTime.Now.ToString("MMddyyyhhmmss") + "_" + dealerId + ".txt"
                                    : DateTime.Now.ToString("MMddyyyhhmmss") + "_" + Path.GetFileName(FeedUrl));

            var importTask = _mySqlHelper.CreateImportDataFeedHistory(profileId, dealerId, fileName);
            ImportFileToDatabase(dealerId, dataFromFile, fileName);
            _mySqlHelper.MarkImportDataFeedTaskCompleted(importTask.Id);
            FTPHelper.StoreBackupFileOnLocal(ConfigurationManager.AppSettings["LocalBackupImportDataFeed"] + "\\" + dealerId, fileName, fileToArray);
        }

        public void ImportFileToDatabase(int dealerId, byte[] data)
        {
            //var fileToArray = CommonHelper.GetBytes(data);
            var dataFromFile = GetInventoriesFromFile(dealerId, data);
            if (dataFromFile.Count == 0) return;
            var fileName = (String.IsNullOrEmpty(FeedUrl)
                                    ? DateTime.Now.ToString("MMddyyyhhmmss") + "_" + dealerId + ".txt"
                                    : DateTime.Now.ToString("MMddyyyhhmmss") + "_" + Path.GetFileName(FeedUrl));
            var importTask = _mySqlHelper.CreateImportDataFeedHistory(_mySqlHelper.GetDealerSettingByDealerId(dealerId).ImportDataFeedProfileId.GetValueOrDefault(), dealerId, fileName);
            ImportFileToDatabase(dealerId, dataFromFile, fileName);
            _mySqlHelper.MarkImportDataFeedTaskCompleted(importTask.Id);
            FTPHelper.StoreBackupFileOnLocal(ConfigurationManager.AppSettings["LocalBackupImportDataFeed"] + "\\" + dealerId, fileName, data);
        }

        private void ImportFileToDatabase(int dealerId, IEnumerable<VehicleViewModel> dataFromFile, string fileName)
        {
            _log = new LogFile(ConfigurationManager.AppSettings["DataFeedLog"], fileName);
            _log.ErrorLog("START: Import data feed for " + dealerId);

            var count = 0;
            
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var inventories = context.whitmanenterprisedealershipinventories.Where(i => i.DealershipId == dealerId).ToList();

                foreach (var wdi in inventories)
                {
                    if (!(dataFromFile.Select(i => i.Vin).Contains(wdi.VINNumber)))
                    {
                        var vehicle = new whitmanenterprisedealershipinventorysoldout()
                        {
                            ModelYear = wdi.ModelYear,
                            Make = wdi.Make,
                            Model = wdi.Model,
                            Trim = wdi.Trim,
                            VINNumber = wdi.VINNumber,
                            StockNumber = wdi.StockNumber,
                            SalePrice = wdi.SalePrice,
                            MSRP = wdi.MSRP,
                            Mileage = wdi.Mileage,
                            ExteriorColor = wdi.ExteriorColor,
                            InteriorColor = wdi.InteriorColor,
                            InteriorSurface = wdi.InteriorSurface,
                            BodyType = wdi.BodyType,
                            Cylinders = wdi.Cylinders,
                            Liters = wdi.Liters,
                            EngineType = wdi.EngineType,
                            DriveTrain = wdi.DriveTrain,
                            FuelType = wdi.FuelType,
                            Tranmission = wdi.Tranmission,
                            Doors = wdi.Doors,
                            Certified = wdi.Certified,
                            CarsOptions = wdi.CarsOptions,
                            CarsPackages = wdi.CarsPackages,
                            Descriptions = wdi.Descriptions,
                            CarImageUrl = wdi.CarImageUrl,
                            ThumbnailImageURL = wdi.ThumbnailImageURL,
                            DateInStock = wdi.DateInStock,
                            LastUpdated = DateTime.Now,
                            DealershipName = wdi.DealershipName,
                            DealershipAddress = wdi.DealershipAddress,
                            DealershipCity = wdi.DealershipCity,
                            DealershipState = wdi.DealershipState,
                            DealershipPhone = wdi.DealershipPhone,
                            DealershipId = wdi.DealershipId,
                            DefaultImageUrl = wdi.DefaultImageUrl,
                            NewUsed = wdi.NewUsed,
                            AddToInventoryBy = wdi.AddToInventoryBy,
                            RemoveBy = "VinControlAdmin",
                            AppraisalID = wdi.AppraisalID,
                            //DataFeed = customer.DeleteImmediately,
                            //FirstName = customer.FirstName,
                            //LastName = customer.LastName,
                            //Address = customer.Address,
                            //City = customer.City,
                            //State = customer.State,
                            //ZipCode = customer.ZipCode,
                            //Country = customer.Country,
                            DateRemoved = DateTime.Now,
                            ACV = wdi.ACV,
                            DealerCost = wdi.DealerCost,
                            FuelEconomyCity = wdi.FuelEconomyCity,
                            FuelEconomyHighWay = wdi.FuelEconomyHighWay,
                            StandardOptions = wdi.StandardOptions,
                            WarrantyInfo = wdi.WarrantyInfo,
                            RetailPrice = wdi.RetailPrice,
                            DealerDiscount = wdi.DealerDiscount,
                            ManufacturerRebate = wdi.ManufacturerRebate,
                            WindowStickerPrice = wdi.WindowStickerPrice,
                            DealershipZipCode = wdi.DealershipZipCode,
                            CarFaxOwner = wdi.CarFaxOwner,
                            Recon = wdi.Recon,
                            KBBOptionsId = wdi.KBBOptionsId,
                            KBBTrimId = wdi.KBBTrimId,
                            PriorRental = wdi.PriorRental,
                            DealerDemo = wdi.DealerDemo,
                            Unwind = wdi.Unwind,
                            TruckCategory = wdi.TruckCategory,
                            TruckClass = wdi.TruckClass,
                            TruckType = wdi.TruckType,
                            Disclaimer = wdi.Disclaimer,
                            AdditionalTitle = wdi.AdditionalTitle,
                            ChromeModelId = wdi.ChromeModelId,
                            ChromeStyleId = wdi.ChromeStyleId,
                            ColorCode = wdi.ColorCode
                        };

                        vehicle.VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType;
                        vehicle.TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory;
                        vehicle.TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass;
                        vehicle.TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType;

                        context.AddTowhitmanenterprisedealershipinventorysoldouts(vehicle);

                        context.DeleteObject(wdi);
                        context.SaveChanges();
                    }
                }

                foreach (var item in dataFromFile)
                {
                    var existingVin = inventories.FirstOrDefault(i => i.VINNumber.Equals(item.Vin));
                    if (existingVin != null)
                    {
                        existingVin.SalePrice = item.SalePrice;
                        existingVin.LastUpdated = DateTime.Now;
                        context.SaveChanges();
                    }
                    else
                    {
                        var newRecord = new whitmanenterprisedealershipinventory()
                        {
                            ModelYear = item.Year,
                            Make = item.Make,
                            Model = item.Model,
                            Trim = item.Trim,
                            VINNumber = item.Vin,
                            StockNumber = item.StockNumber,
                            SalePrice = item.SalePrice,
                            MSRP = item.MSRP,
                            DealerMSRP = item.MSRP,
                            Mileage = item.Mileage,
                            ExteriorColor = item.ExteriorColor,
                            InteriorColor = item.InteriorColor,
                            InteriorSurface = item.InteriorSurface,
                            BodyType = item.BodyType,
                            Cylinders = item.Cylinders,
                            Liters = item.Liters,
                            EngineType = item.EngineType,
                            DriveTrain = item.DriveTrain,
                            FuelType = item.FuelType,
                            Tranmission = item.Tranmission,
                            Doors = item.Doors,
                            FuelEconomyCity = item.FuelEconomyCity,
                            FuelEconomyHighWay = item.FuelEconomyHighWay,
                            Certified = item.Certified,
                            StandardOptions = item.StandardOptions,
                            CarsOptions = item.CarsOptions,
                            Descriptions = item.Descriptions,
                            CarImageUrl = item.CarImageUrl,
                            ThumbnailImageURL = item.ThumbnalImageurl,
                            DateInStock = item.DateInStock,
                            LastUpdated = DateTime.Now,
                            DealershipId = item.DealerId,
                            DealershipName = item.DealershipName,
                            DealershipAddress = item.DealershipAddress,
                            DealershipCity = item.DealershipCity,
                            DealershipState = item.DealershipState,
                            DealershipPhone = item.DealershipPhone,
                            DealershipZipCode = item.DealershipZipCode,
                            DealerCost = item.DealerCost,
                            DealerDiscount = item.DealerDiscount,
                            ACV = item.ACV,
                            NewUsed = item.NewUsed,
                            AddToInventoryBy = "VinControlAdmin",
                            AppraisalID = item.AppraisalId,
                            VehicleType = String.IsNullOrEmpty(item.VehicleType) ? "Car" : item.VehicleType,
                            RetailPrice = item.RetailPrice,
                            WindowStickerPrice = item.WindowStickerPrice,
                            ManufacturerRebate = item.ManufacturerRebate,
                            PriorRental = item.PriorRental,
                            TruckClass = item.TruckClass,
                            TruckType = item.TruckType,
                            TruckCategory = item.TruckCategory
                        };

                        // normalize data with Vin Decode
                        if (!String.IsNullOrEmpty(item.Vin))
                        {
                            try
                            {
                                var vehicleFromChromeService = _autoServiceHelper.GetVehicleInformationFromVinRequest(item.Vin);
                                if (vehicleFromChromeService != null)
                                {
                                    newRecord.ModelYear = vehicleFromChromeService.modelYear;
                                    newRecord.Make = vehicleFromChromeService.vinMakeName;
                                    newRecord.Model = vehicleFromChromeService.vinModelName;
                                    newRecord.Trim = vehicleFromChromeService.vinStyleName;
                                    newRecord.Doors = vehicleFromChromeService.vehicleSpecification.numberOfPassengerDoors.ToString();
                                    newRecord.MSRP = vehicleFromChromeService.baseMsrp.highValue.ToString("C");
                                    newRecord.DealerMSRP = vehicleFromChromeService.baseMsrp.highValue.ToString("C");

                                    newRecord.DealerCost = newRecord.DealerCost.Contains(".") ? newRecord.DealerCost.Substring(0, newRecord.DealerCost.IndexOf(".")) : newRecord.DealerCost;
                                    newRecord.ACV = newRecord.ACV.Contains(".") ? newRecord.ACV.Substring(0, newRecord.ACV.IndexOf(".")) : newRecord.ACV;
                                    newRecord.SalePrice = newRecord.SalePrice.Contains(".") ? newRecord.SalePrice.Substring(0, newRecord.SalePrice.IndexOf(".")) : newRecord.SalePrice;
                                    newRecord.WindowStickerPrice = newRecord.WindowStickerPrice.Contains(".") ? newRecord.WindowStickerPrice.Substring(0, newRecord.WindowStickerPrice.IndexOf(".")) : newRecord.WindowStickerPrice;

                                    if (newRecord.CarImageUrl.Contains("|"))
                                    {
                                        newRecord.CarImageUrl = newRecord.CarImageUrl.Replace("|", ",");
                                    }

                                    if (String.IsNullOrEmpty(newRecord.ExteriorColor))
                                    {
                                        var exteriorColors = vehicleFromChromeService.colorDescription.exteriorColors !=
                                                             null
                                                                 ? _autoServiceHelper.InitializeExteriorColors(
                                                                     vehicleFromChromeService.colorDescription.
                                                                         exteriorColors)
                                                                 : null;
                                        if (exteriorColors != null && exteriorColors.Count() > 0)
                                        {
                                            newRecord.ExteriorColor = exteriorColors.FirstOrDefault().Text;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(newRecord.InteriorColor))
                                    {
                                        var interiorColors = vehicleFromChromeService.colorDescription.interiorColors !=
                                                             null
                                                                 ? _autoServiceHelper.InitializeInteriorColors(
                                                                     vehicleFromChromeService.colorDescription.
                                                                         interiorColors)
                                                                 : null;
                                        if (interiorColors != null && interiorColors.Count() > 0)
                                        {
                                            newRecord.InteriorColor = interiorColors.FirstOrDefault().Text;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(newRecord.BodyType) && vehicleFromChromeService.vehicleSpecification.bodyTypes != null && vehicleFromChromeService.vehicleSpecification.bodyTypes.Any())
                                    {
                                        var bodyTypes = _autoServiceHelper.InitializeBodyTypes(vehicleFromChromeService.vehicleSpecification.bodyTypes);
                                        if (bodyTypes != null && bodyTypes.Count() > 0)
                                        {
                                            newRecord.BodyType = bodyTypes.FirstOrDefault().Text;
                                        }
                                    }
                                    
                                    if (vehicleFromChromeService.vehicleSpecification.engines != null && vehicleFromChromeService.vehicleSpecification.engines.Any())
                                    {
                                        foreach (var er in vehicleFromChromeService.vehicleSpecification.engines)
                                        {
                                            string fuelType = er.fuelType;
                                            int index = fuelType.LastIndexOf(" ");

                                            newRecord.Cylinders = er.cylinders.ToString();
                                            newRecord.FuelType = fuelType.Substring(0, index);
                                            newRecord.Liters = er.displacementL.ToString();

                                            newRecord.FuelEconomyCity = er.fuelEconomyCityValue.lowValue.ToString();
                                            newRecord.FuelEconomyHighWay = er.fuelEconomyHwyValue.lowValue.ToString();
                                            break;
                                        }
                                    }

                                    if (vehicleFromChromeService.genericEquipment != null)
                                    {
                                        foreach (var ge in vehicleFromChromeService.genericEquipment)
                                        {
                                            if (ge.categoryId == 1130 || ge.categoryId == 1101 || ge.categoryId == 1102 || ge.categoryId == 1103 || ge.categoryId == 1104 || ge.categoryId == 1210 || ge.categoryId == 1220)
                                            {
                                                newRecord.Tranmission = "Automatic";
                                                break;
                                            }
                                            if (ge.categoryId == 1131 || ge.categoryId == 1105 || ge.categoryId == 1106 || ge.categoryId == 1107 || ge.categoryId == 1108 || ge.categoryId == 1146 || ge.categoryId == 1147 || ge.categoryId == 1148)
                                            {
                                                newRecord.Tranmission = "Manual";
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _log.ErrorLog("ERROR: " + ex.Message);
                            }

                        }

                        context.AddTowhitmanenterprisedealershipinventories(newRecord);
                        count++;

                        if (count > 100)
                        {
                            context.SaveChanges();
                            count = 0;
                        }
                    }
                    
                }

                context.SaveChanges();
            }

            _log.ErrorLog("END: Import data feed for " + dealerId);
        }
        
        #region Private Methods

        private List<VehicleViewModel> GetInventoriesFromFile(int profileId, int dealerId, Stream data)
        {
            var mappingTemplate = _xmlHelper.LoadMappingTemplateFromProfileId(profileId);
            if (mappingTemplate == null)
                throw new InvalidOperationException("There is no mapping template for this dealer");

            List<VehicleViewModel> dtAutoTrader = null;

            CachedCsvReader csv = _ftpExcelHelper.ParseDataFile(data, mappingTemplate.HasHeader, mappingTemplate.Delimeter);

            if (csv != null)
            {
                var dtTemporaryNoheader = new DataTable();

                dtAutoTrader = new List<VehicleViewModel>();

                dtTemporaryNoheader.Load(csv);

                foreach (DataRow drRow in dtTemporaryNoheader.Rows)
                {
                    try
                    {
                        var vehicle = new VehicleViewModel(_commonHelper, drRow, mappingTemplate) { DealerId = dealerId };

                        dtAutoTrader.Add(vehicle);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Wrong mapping! " + ex.Message);
                    }
                }
            }

            return dtAutoTrader;
        }

        private List<VehicleViewModel> GetInventoriesFromFile(int dealerId, byte[] data)
        {
            var mappingTemplate = _xmlHelper.LoadMappingTemplate(dealerId);
            if (mappingTemplate == null)
                throw new InvalidOperationException("There is no mapping template for this dealer");

            List<VehicleViewModel> dtAutoTrader = null;

            CachedCsvReader csv = _ftpExcelHelper.ParseDataFile(data, mappingTemplate.HasHeader, mappingTemplate.Delimeter);

            if (csv != null)
            {
                var dtTemporaryNoheader = new DataTable();

                dtAutoTrader = new List<VehicleViewModel>();

                dtTemporaryNoheader.Load(csv);

                foreach (DataRow drRow in dtTemporaryNoheader.Rows)
                {
                    try
                    {
                        var vehicle = new VehicleViewModel(_commonHelper, drRow, mappingTemplate) { DealerId = dealerId };

                        dtAutoTrader.Add(vehicle);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Wrong mapping! " + ex.Message);
                    }
                }
            }

            return dtAutoTrader;
        }

        #endregion
    }
}
