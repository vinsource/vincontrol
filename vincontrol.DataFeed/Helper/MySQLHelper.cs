using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DataFeed.Model;
using vincontrol.Backend.Data;

namespace vincontrol.DataFeed.Helper
{
    public class MySQLHelper
    {
        private CommonHelper _commonHelper;

        public MySQLHelper()
        {
            _commonHelper = new CommonHelper();
        }

        public bool CheckVehicleExist(string vin, int dealerId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisedealershipinventories.Any(o => o.VINNumber == vin && o.DealershipId == dealerId))
                    return true;
            }
            return false;
        }

        public int CheckVinHasKbbReport(string vin)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterprisekbbs.Any(o => o.Vin.Equals(vin)))
                {
                    var firstTmp = context.whitmanenterprisekbbs.FirstOrDefault(o => o.Vin.Equals(vin));

                    var dt = DateTime.Parse(firstTmp.ExpiredDate.ToString());

                    return dt.Date > DateTime.Now.Date ? 1 : 2;
                }
            }

            return 0;
        }
        
        public List<VehicleViewModel> GetDealerInventory(int dealerId)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var dtExistInventoryByDealerId = context.whitmanenterprisedealershipinventories.Where(x => x.DealershipId == dealerId).ToList();

            return dtExistInventoryByDealerId.Select(tmp => new VehicleViewModel(tmp)).ToList();
        }

        public List<VehicleViewModel> GetDealerInventory(int dealerId, bool isNew)
        {
            var result = GetDealerInventory(dealerId);
            result = isNew
                         ? result.Where(i => i.Recon == false && i.NewUsed.ToLower().Equals("New")).ToList()
                         : result.Where(i => i.Recon == false && i.NewUsed.ToLower().Equals("Used")).ToList();

            return result;
        }

        public List<VehicleViewModel> GetDealerWholeSaleInventory(int dealerId)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var dtExistInventoryByDealerId = context.vincontrolwholesaleinventories.Where(x => x.DealershipId == dealerId).AsEnumerable();

            return dtExistInventoryByDealerId.Select(tmp => new VehicleViewModel(tmp)).ToList();
        }
        
        public List<DealerViewModel> GetDealerImportList()
        {
            var context = new whitmanenterprisewarehouseEntities();

            var finalist = new List<DealerViewModel>();

            var dtExistInventoryByDealerId = from i in context.vincontrondnsimportlists
                                             from it in context.whitmanenterprisesettings
                                             where i.DealerId == it.DealershipId
                                             select new
                                             {
                                                 i.DealerId,
                                                 i.Name,
                                                 i.Address,
                                                 i.City,
                                                 i.State,
                                                 i.ZipCode,
                                                 it.CarFax,
                                                 it.CarFaxPassword,
                                                 i.Phone
                                             };

            foreach (var tmp in dtExistInventoryByDealerId)
            {
                var dealer = new DealerViewModel()
                {
                    Name = tmp.Name,
                    Address = tmp.Address,
                    City = tmp.City,
                    State = tmp.State,
                    ZipCode = tmp.ZipCode,
                    DealerId = tmp.DealerId,
                    CarFaxUserName = tmp.CarFax,
                    CarFaxPassword = tmp.CarFaxPassword,
                    Phone = tmp.Phone
                };

                finalist.Add(dealer);
            }
            return finalist;
        }

        public List<VehicleViewModel> GetDealerInventoryFromFullSeviceCraigList(string dealerId)
        {
            var context = new whitmanenterprisecraigslistEntities();

            var convertedDealerId = Convert.ToInt32(dealerId);

            var dtExistInventoryByDealerId = context.whitmanenterprisecraigslistinventories.Where(x => x.DealershipId == convertedDealerId).AsEnumerable();

            var list = new List<VehicleViewModel>();

            foreach (var tmp in dtExistInventoryByDealerId)
            {
                var vehicle = new VehicleViewModel()
                {
                    ListingId = tmp.ListingID,
                    Year = Convert.ToInt32(tmp.ModelYear),
                    Make = tmp.Make,
                    Model = tmp.Model,
                    Trim = tmp.Trim,
                    Vin = tmp.VINNumber,
                    StockNumber = tmp.StockNumber,
                    SalePrice = tmp.SalePrice,
                    MSRP = tmp.MSRP,
                    Mileage = tmp.Mileage,
                    ExteriorColor = tmp.ExteriorColor,
                    InteriorColor = tmp.InteriorColor,
                    InteriorSurface = tmp.InteriorSurface,
                    BodyType = tmp.BodyType,
                    EngineType = tmp.EngineType,
                    DriveTrain = tmp.DriveTrain,
                    Cylinders = tmp.Cylinders,
                    Liters = tmp.Liters,
                    FuelType = tmp.FuelType,
                    Tranmission = tmp.Tranmission,
                    Doors = tmp.Doors,
                    Certified = tmp.Certified.GetValueOrDefault(),
                    CarsOptions = tmp.CarsOptions,
                    Descriptions = tmp.Descriptions,
                    CarImageUrl = tmp.CarImageUrl,
                    DateInStock = DateTime.Now.AddDays(tmp.Age.GetValueOrDefault() * (-1)),
                    DealershipName = tmp.DealershipName,
                    DealershipAddress = tmp.DealershipAddress,
                    DealershipCity = tmp.DealershipCity,
                    DealershipState = tmp.DealershipState,
                    DealershipZipCode = tmp.DealershipZipCode,
                    DealerCost = tmp.DealerCost,
                    ACV = tmp.ACV,
                    DefaultImageUrl = tmp.DefaultImageUrl,
                    NewUsed = tmp.NewUsed,
                    Age = tmp.Age.GetValueOrDefault()
                };

                list.Add(vehicle);
            }

            return list;
        }

        public List<VehicleViewModel> GetDealerInventory(int[] dealerIdArray)
        {
            var context = new whitmanenterprisecraigslistEntities();

            var dtExistInventoryByDealerId = new List<whitmanenterprisecraigslistinventory>();

            var list = new List<VehicleViewModel>();

            foreach (var dealerId in dealerIdArray)
            {
                string did = dealerId + "";
                dtExistInventoryByDealerId.AddRange(context.whitmanenterprisecraigslistinventories.Where(x => x.DealershipId.Equals(did)).AsEnumerable());
            }
            
            foreach (var tmp in dtExistInventoryByDealerId)
            {
                if (tmp.DealershipId.Equals("44674"))
                {
                    var vehicle = new VehicleViewModel()
                    {
                        ListingId = tmp.ListingID,
                        Year = Convert.ToInt32(tmp.ModelYear),
                        Make = tmp.Make,
                        Model = tmp.Model,
                        Trim = tmp.Trim,
                        Vin = tmp.VINNumber,
                        StockNumber = tmp.StockNumber,
                        SalePrice = tmp.SalePrice,
                        MSRP = tmp.MSRP,
                        Mileage = tmp.Mileage,
                        ExteriorColor = tmp.ExteriorColor,
                        InteriorColor = tmp.InteriorColor,
                        InteriorSurface = tmp.InteriorSurface,
                        BodyType = tmp.BodyType,
                        EngineType = tmp.EngineType,
                        DriveTrain = tmp.DriveTrain,
                        Cylinders = tmp.Cylinders,
                        Liters = tmp.Liters,
                        FuelType = tmp.FuelType,
                        Tranmission = tmp.Tranmission,
                        Doors = tmp.Doors,
                        Certified = tmp.Certified.GetValueOrDefault(),
                        StandardOptions = tmp.CarsOptions,
                        CarsOptions = tmp.CarsOptions,
                        Descriptions = tmp.Descriptions,
                        CarImageUrl = tmp.CarImageUrl,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DealershipName = tmp.DealershipName,
                        DealershipAddress = tmp.DealershipAddress,
                        DealershipCity = tmp.DealershipCity,
                        DealershipState = tmp.DealershipState,
                        DealershipZipCode = tmp.DealershipZipCode,
                        DealerId = Convert.ToInt32(tmp.DealershipId),
                        DealerCost = tmp.DealerCost,
                        ACV = tmp.ACV,
                        DefaultImageUrl = tmp.DefaultImageUrl,
                        NewUsed = tmp.NewUsed
                    };

                    list.Add(vehicle);
                }
                else
                {
                    if (!tmp.Make.Equals("Aston Martin") && tmp.NewUsed.Equals("Used"))
                    {
                        var vehicle = new VehicleViewModel()
                        {
                            ListingId = tmp.ListingID,
                            Year = Convert.ToInt32(tmp.ModelYear),
                            Make = tmp.Make,
                            Model = tmp.Model,
                            Trim = tmp.Trim,
                            Vin = tmp.VINNumber,
                            StockNumber = tmp.StockNumber,
                            SalePrice = tmp.SalePrice,
                            MSRP = tmp.MSRP,
                            Mileage = tmp.Mileage,
                            ExteriorColor = tmp.ExteriorColor,
                            InteriorColor = tmp.InteriorColor,
                            InteriorSurface = tmp.InteriorSurface,
                            BodyType = tmp.BodyType,
                            EngineType = tmp.EngineType,
                            DriveTrain = tmp.DriveTrain,
                            Cylinders = tmp.Cylinders,
                            Liters = tmp.Liters,
                            FuelType = tmp.FuelType,
                            Tranmission = tmp.Tranmission,
                            Doors = tmp.Doors,
                            Certified = tmp.Certified.GetValueOrDefault(),
                            StandardOptions = tmp.CarsOptions,
                            CarsOptions = tmp.CarsOptions,
                            Descriptions = tmp.Descriptions,
                            CarImageUrl = tmp.CarImageUrl,
                            DateInStock = tmp.DateInStock.GetValueOrDefault(),
                            DealershipName = tmp.DealershipName,
                            DealershipAddress = tmp.DealershipAddress,
                            DealershipCity = tmp.DealershipCity,
                            DealershipState = tmp.DealershipState,
                            DealershipZipCode = tmp.DealershipZipCode,
                            DealerId = Convert.ToInt32(tmp.DealershipId),
                            DealerCost = tmp.DealerCost,
                            ACV = tmp.ACV,
                            DefaultImageUrl = tmp.DefaultImageUrl,
                            NewUsed = tmp.NewUsed
                        };

                        list.Add(vehicle);
                    }
                }
            }

            return list;
        }

        public void MarkSoldVehicle(List<VehicleViewModel> listSoldOut, DealerViewModel dealer)
        {
            var context = new whitmanenterprisewarehouseEntities();

            int maxid = context.whitmanenterprisedealershipinventorysoldouts.Max(x => x.ListingID) + 1;

            var currentSoldOutList = context.whitmanenterprisedealershipinventorysoldouts.Where(x => x.DealershipId == dealer.DealerId);

            foreach (var tmp in listSoldOut)
            {
                var vehicle = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == tmp.ListingId);

                bool existSoldOut = currentSoldOutList.Any(x => x.VINNumber == tmp.Vin && x.DealershipId == dealer.DealerId);

                if (!String.IsNullOrEmpty(vehicle.VINNumber) && !existSoldOut)
                {
                    var v = new whitmanenterprisedealershipinventorysoldout()
                    {
                        ListingID = maxid,
                        ModelYear = vehicle.ModelYear,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        VINNumber = vehicle.VINNumber,
                        StockNumber = vehicle.StockNumber,
                        SalePrice = vehicle.SalePrice,
                        RetailPrice = vehicle.RetailPrice,
                        DealerDiscount = vehicle.DealerDiscount,
                        ManufacturerRebate = vehicle.ManufacturerRebate,
                        MSRP = vehicle.MSRP,
                        Mileage = vehicle.Mileage,
                        ExteriorColor = vehicle.ExteriorColor,
                        InteriorColor = vehicle.InteriorColor,
                        InteriorSurface = vehicle.InteriorSurface,
                        BodyType = vehicle.BodyType,
                        EngineType = vehicle.EngineType,
                        DriveTrain = vehicle.DriveTrain,
                        Cylinders = vehicle.Cylinders,
                        Liters = vehicle.Liters,
                        FuelType = vehicle.FuelType,
                        Tranmission = vehicle.Tranmission,
                        Doors = vehicle.Doors,
                        Certified = vehicle.Certified,
                        CarsOptions = vehicle.CarsOptions,
                        Descriptions = vehicle.Descriptions,
                        CarImageUrl = vehicle.CarImageUrl,
                        ThumbnailImageURL = vehicle.ThumbnailImageURL,
                        DateInStock = vehicle.DateInStock,
                        DealershipName = vehicle.DealershipName,
                        DealershipAddress = vehicle.DealershipAddress,
                        DealershipCity = vehicle.DealershipCity,
                        DealershipState = vehicle.DealershipState,
                        DealershipZipCode = vehicle.DealershipZipCode,
                        DealershipId = vehicle.DealershipId,
                        DealerCost = vehicle.DealerCost,
                        ACV = vehicle.ACV,
                        DefaultImageUrl = vehicle.DefaultImageUrl,
                        NewUsed = vehicle.NewUsed,
                        VehicleType = vehicle.VehicleType,
                        TruckCategory = vehicle.TruckCategory,
                        TruckClass = vehicle.TruckClass,
                        TruckType = vehicle.TruckType,
                        FuelEconomyCity = vehicle.FuelEconomyCity,
                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                        DateRemoved = DateTime.Now,
                        WindowStickerPrice = vehicle.WindowStickerPrice,
                        CarFaxOwner = vehicle.CarFaxOwner,
                        StandardOptions = vehicle.StandardOptions,
                        KBBOptionsId = vehicle.KBBOptionsId,
                        Recon = vehicle.Recon,
                        PriorRental = vehicle.PriorRental,
                        KBBTrimId = vehicle.KBBTrimId
                    };

                    context.AddTowhitmanenterprisedealershipinventorysoldouts(v);
                    maxid++;
                }
                
                context.Attach(vehicle);
                context.DeleteObject(vehicle);
            }

            context.SaveChanges();
        }

        public void UpdateSmartComment(List<VehicleUpdateInfoViewModel> listUpdatePriceAndImageURL, DealerViewModel dealer)
        {
            var context = new whitmanenterprisewarehouseEntities();

            if (dealer.DealerId == 10997)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    result.Descriptions = vehicleUpdate.Description;
                }

                context.SaveChanges();
            }
            else if (dealer.DealerId == 3636)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    result.Descriptions = vehicleUpdate.Description;
                }

                context.SaveChanges();
            }
        }

        public void UpdateSalePrice(List<VehicleUpdateInfoViewModel> listUpdatePriceAndImageURL, DealerViewModel dealer)
        {
            var context = new whitmanenterprisewarehouseEntities();

            if (dealer.DealerId == 2183)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        result.WindowStickerPrice = salePriceNumber.ToString();

                        int pumpPriceNumber = salePriceNumber;

                        result.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                        result.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                    }

                    result.NewUsed = vehicleUpdate.NewUsed;
                    result.DateInStock = vehicleUpdate.DateInStock;
                    result.Mileage = vehicleUpdate.Mileage;
                    result.Recon = vehicleUpdate.Recon;
                    result.LastUpdated = DateTime.Now;
                }
            }
            else if (dealer.DealerId == 1660)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        result.SalePrice = salePriceNumber.ToString();

                        result.WindowStickerPrice = salePriceNumber.ToString();

                        int pumpPriceNumber = salePriceNumber;

                        result.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                        result.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                    }

                    result.NewUsed = vehicleUpdate.NewUsed;

                    result.Certified = vehicleUpdate.Certified;

                    result.ExteriorColor = vehicleUpdate.ExteriorColor;

                    result.InteriorColor = vehicleUpdate.InteriorColor;

                    result.DateInStock = vehicleUpdate.DateInStock;

                    result.Mileage = vehicleUpdate.Mileage;

                    result.Recon = vehicleUpdate.Recon;

                    result.LastUpdated = DateTime.Now;

                    if (vehicleUpdate.Certified)
                        result.WarrantyInfo = 4;
                }
            }
            else if (dealer.DealerId == 44675)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        result.SalePrice = salePriceNumber.ToString();

                        result.WindowStickerPrice = salePriceNumber.ToString();

                        int pumpPriceNumber = salePriceNumber;

                        result.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                        result.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                    }

                    result.NewUsed = vehicleUpdate.NewUsed;

                    result.CarImageUrl = vehicleUpdate.CarImageURL;

                    result.Certified = vehicleUpdate.Certified;

                    result.ExteriorColor = vehicleUpdate.ExteriorColor;

                    result.InteriorColor = vehicleUpdate.InteriorColor;

                    result.DateInStock = vehicleUpdate.DateInStock;

                    result.Mileage = vehicleUpdate.Mileage;

                    result.Recon = vehicleUpdate.Recon;

                    result.LastUpdated = DateTime.Now;

                    if (vehicleUpdate.Certified)
                        result.WarrantyInfo = 4;
                }
            }
            else if (dealer.DealerId == 12293)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        result.SalePrice = salePriceNumber.ToString();

                        result.WindowStickerPrice = salePriceNumber.ToString();

                        int pumpPriceNumber = salePriceNumber;

                        result.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                        result.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                    }

                    result.NewUsed = vehicleUpdate.NewUsed;

                    result.CarImageUrl = vehicleUpdate.CarImageURL;

                    result.Certified = vehicleUpdate.Certified;

                    result.ExteriorColor = vehicleUpdate.ExteriorColor;

                    result.InteriorColor = vehicleUpdate.InteriorColor;

                    result.DateInStock = vehicleUpdate.DateInStock;

                    result.Mileage = vehicleUpdate.Mileage;

                    result.Recon = vehicleUpdate.Recon;

                    result.LastUpdated = DateTime.Now;

                    if (vehicleUpdate.Certified)
                        result.WarrantyInfo = 4;
                }
            }
            else if (dealer.DealerId == 10997)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        result.SalePrice = vehicleUpdate.SalePrice;

                        result.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        result.DealerDiscount = (Convert.ToInt32(result.RetailPrice) - salePriceNumber).ToString();

                        result.WindowStickerPrice = salePriceNumber.ToString();
                    }
                    else
                    {
                        result.SalePrice = salePriceNumber.ToString();
                    }

                    result.NewUsed = vehicleUpdate.NewUsed;
                    
                    result.DateInStock = vehicleUpdate.DateInStock;
                    
                    result.Mileage = vehicleUpdate.Mileage;
                    
                    result.PriorRental = vehicleUpdate.PriorRental;
                    
                    result.Recon = vehicleUpdate.Recon;

                    result.LastUpdated = DateTime.Now;
                }
            }
            else if (dealer.DealerId == 37695)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        result.SalePrice = vehicleUpdate.SalePrice;

                        result.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        result.DealerDiscount = (Convert.ToInt32(result.RetailPrice) - salePriceNumber).ToString();

                        result.WindowStickerPrice = salePriceNumber.ToString();
                    }
                    else
                    {
                        result.SalePrice = salePriceNumber.ToString();
                    }
                    
                    result.NewUsed = vehicleUpdate.NewUsed;
                    
                    result.DateInStock = vehicleUpdate.DateInStock;
                    
                    result.Mileage = vehicleUpdate.Mileage;
                    
                    result.Recon = vehicleUpdate.Recon;

                    result.LastUpdated = DateTime.Now;
                }
            }
            else if (dealer.DealerId == 12293 || dealer.DealerId == 2584 || dealer.DealerId == 23063 || dealer.DealerId == 1563 || dealer.DealerId == 44674 || dealer.DealerId == 1541)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        result.SalePrice = vehicleUpdate.SalePrice;

                        result.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        result.DealerDiscount = (Convert.ToInt32(result.RetailPrice) - salePriceNumber).ToString();

                        result.WindowStickerPrice = salePriceNumber.ToString();
                    }
                    else
                    {
                        result.SalePrice = salePriceNumber.ToString();
                    }

                    result.NewUsed = vehicleUpdate.NewUsed;

                    result.Mileage = vehicleUpdate.Mileage;
                    
                    result.Recon = vehicleUpdate.Recon;

                    result.LastUpdated = DateTime.Now;
                }
            }
            else if (dealer.DealerId == 5438)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    if (vehicleUpdate.NewUsed == "Used")
                    {
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            salePriceNumber = salePriceNumber + 2000;

                            result.SalePrice = vehicleUpdate.SalePrice;

                            result.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                            result.DealerDiscount = (Convert.ToInt32(result.RetailPrice) - salePriceNumber).ToString();

                            result.WindowStickerPrice = salePriceNumber.ToString();
                        }

                        result.NewUsed = vehicleUpdate.NewUsed;

                        result.Mileage = vehicleUpdate.Mileage;

                        result.Recon = vehicleUpdate.Recon;

                        result.LastUpdated = DateTime.Now;
                    }
                }
            }
            else if (dealer.DealerId == 3636)
            {
                foreach (var vehicleUpdate in listUpdatePriceAndImageURL)
                {
                    var result = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == vehicleUpdate.ListingId);

                    if (vehicleUpdate.NewUsed == "Used")
                    {
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            salePriceNumber = salePriceNumber + 2000;

                            result.SalePrice = vehicleUpdate.SalePrice;

                            result.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                            result.DealerDiscount = (Convert.ToInt32(result.RetailPrice) - salePriceNumber).ToString();

                            result.WindowStickerPrice = salePriceNumber.ToString();
                        }

                        result.MSRP = vehicleUpdate.MSRP;
                    }
                    else if (vehicleUpdate.NewUsed == "New")
                    {
                        int salePriceNumber;

                        Int32.TryParse(vehicleUpdate.SalePrice, out salePriceNumber);

                        result.SalePrice = salePriceNumber.ToString();

                        result.RetailPrice = salePriceNumber.ToString();

                        result.MSRP = vehicleUpdate.MSRP;

                        if (context.vincontrolrebates.Any(x => x.Year == result.ModelYear && x.Make == result.Make && x.Model == result.Model && x.Trim == result.Trim))
                        {
                            result.ManufacturerRebate = context.vincontrolrebates.First(x => x.Year == result.ModelYear && x.Make == result.Make && x.Model == result.Model && x.Trim == result.Trim).ManufactureReabte;

                            int rebateAmmount;

                            Int32.TryParse(result.ManufacturerRebate, out rebateAmmount);

                            int finalPrice = salePriceNumber - rebateAmmount;

                            result.SalePrice = finalPrice.ToString();
                        }
                        else
                        {
                            result.ManufacturerRebate = "0";
                        }
                    }

                    result.Tranmission = vehicleUpdate.Tranmission;

                    result.DealerCost = vehicleUpdate.DealerCost;

                    result.ACV = vehicleUpdate.ACV;

                    result.NewUsed = vehicleUpdate.NewUsed;

                    result.Mileage = vehicleUpdate.Mileage;

                    result.Recon = vehicleUpdate.Recon;

                    result.PreWholeSale = vehicleUpdate.WholeSale;

                    result.LastUpdated = DateTime.Now;
                }
            }

            context.SaveChanges();
        }
        
        public void InsertToInvetory(List<VehicleViewModel> dtNewInventory, DealerViewModel dealer)
        {
            var context = new whitmanenterprisewarehouseEntities();

            if (dealer.DealerId == 2183)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;

                var soldIDealerInventory = context.whitmanenterprisedealershipinventorysoldouts.Where(x => x.DealershipId == dealer.DealerId);

                foreach (var vehicle in dtNewInventory)
                {
                    if (soldIDealerInventory.Any(x => x.VINNumber == vehicle.Vin))
                    {
                        var searchResult = soldIDealerInventory.First(x => x.VINNumber == vehicle.Vin);

                        var v = new whitmanenterprisedealershipinventory()
                        {
                            ListingID = maxId,
                            ModelYear = vehicle.Year,
                            Make = vehicle.Make,
                            Model = vehicle.Model,
                            Trim = vehicle.Trim,
                            VINNumber = vehicle.Vin,
                            StockNumber = vehicle.StockNumber,
                            SalePrice = vehicle.SalePrice,
                            MSRP = vehicle.MSRP,
                            Mileage = vehicle.Mileage,
                            ExteriorColor = _commonHelper.UppercaseWords(vehicle.ExteriorColor),
                            InteriorColor = _commonHelper.UppercaseWords(vehicle.InteriorColor),
                            InteriorSurface = vehicle.InteriorSurface,
                            BodyType = vehicle.BodyType,
                            EngineType = vehicle.EngineType,
                            DriveTrain = vehicle.DriveTrain,
                            Cylinders = vehicle.Cylinders,
                            Liters = vehicle.Liters,
                            FuelType = vehicle.FuelType,
                            Tranmission = vehicle.Tranmission,
                            Doors = vehicle.Doors,
                            Certified = vehicle.Certified,
                            CarsOptions = vehicle.CarsOptions,
                            Descriptions = vehicle.Descriptions,
                            DefaultImageUrl = vehicle.DefaultImageURL,
                            ThumbnailImageURL = searchResult.ThumbnailImageURL,
                            CarImageUrl = searchResult.CarImageUrl,
                            LastUpdated = DateTime.Now,
                            DealershipName = vehicle.DealershipName,
                            DealershipAddress = dealer.Address,
                            DealershipCity = dealer.City,
                            DealershipState = dealer.State,
                            DealershipZipCode = dealer.ZipCode,
                            DealershipId = dealer.DealerId,
                            DealershipPhone = dealer.Phone,
                            DealerCost = vehicle.DealerCost,
                            ACV = vehicle.ACV,
                            NewUsed = vehicle.NewUsed,
                            AddToInventoryBy = "VinControlAdmin",
                            FuelEconomyCity = vehicle.FuelEconomyCity,
                            FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                            StandardOptions = vehicle.StandardOptions,
                            Recon = vehicle.Recon,
                            PriorRental = false
                        };

                        v.DateInStock = vehicle.DateInStock;

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }
                        
                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                        
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            v.WindowStickerPrice = salePriceNumber.ToString();

                            int pumpPriceNumber = salePriceNumber + 2000;

                            v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;
                        
                        context.AddTowhitmanenterprisedealershipinventories(v);
                        
                        var removeVehicle = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == searchResult.ListingID);

                        context.Attach(removeVehicle);

                        context.DeleteObject(removeVehicle);

                        maxId++;
                    }
                    else
                    {
                        var v = new whitmanenterprisedealershipinventory
                                    {
                                        ListingID = maxId,
                                        ModelYear = vehicle.Year,
                                        Make = vehicle.Make,
                                        Model = vehicle.Model,
                                        Trim = vehicle.Trim,
                                        VINNumber = vehicle.Vin,
                                        StockNumber = vehicle.StockNumber,
                                        SalePrice = vehicle.SalePrice,
                                        MSRP = vehicle.MSRP,
                                        Mileage = vehicle.Mileage,
                                        ExteriorColor = _commonHelper.UppercaseWords(vehicle.ExteriorColor),
                                        InteriorColor = _commonHelper.UppercaseWords(vehicle.InteriorColor),
                                        InteriorSurface = vehicle.InteriorSurface,
                                        BodyType = vehicle.BodyType,
                                        EngineType = vehicle.EngineType,
                                        DriveTrain = vehicle.DriveTrain,
                                        Cylinders = vehicle.Cylinders,
                                        Liters = vehicle.Liters,
                                        FuelType = vehicle.FuelType,
                                        Tranmission = vehicle.Tranmission,
                                        Doors = vehicle.Doors,
                                        Certified = vehicle.Certified,
                                        CarsOptions = vehicle.CarsOptions,
                                        Descriptions = vehicle.Descriptions,
                                        DefaultImageUrl = vehicle.DefaultImageURL,
                                        ThumbnailImageURL = vehicle.DefaultImageURL,
                                        CarImageUrl = vehicle.DefaultImageURL,
                                        LastUpdated = DateTime.Now,
                                        DealershipName = vehicle.DealershipName,
                                        DealershipAddress = dealer.Address,
                                        DealershipCity = dealer.City,
                                        DealershipState = dealer.State,
                                        DealershipZipCode = dealer.ZipCode,
                                        DealershipId = dealer.DealerId,
                                        DealershipPhone = dealer.Phone,
                                        DealerCost = vehicle.DealerCost,
                                        ACV = vehicle.ACV,
                                        NewUsed = vehicle.NewUsed,
                                        AddToInventoryBy = "VinControlAdmin",
                                        FuelEconomyCity = vehicle.FuelEconomyCity,
                                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                                        StandardOptions = vehicle.StandardOptions,
                                        Recon = vehicle.Recon,
                                        PriorRental = false,
                                        DateInStock = vehicle.DateInStock
                                    };

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }
                        
                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                        
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            v.WindowStickerPrice = salePriceNumber.ToString();

                            int pumpPriceNumber = salePriceNumber + 2000;

                            v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                        context.AddTowhitmanenterprisedealershipinventories(v);
                        maxId++;
                    }
                }
            }
            else if (dealer.DealerId == 1660)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                var soldIDealerInventory = context.whitmanenterprisedealershipinventorysoldouts.Where(x => x.DealershipId == dealer.DealerId);

                foreach (var vehicle in dtNewInventory)
                {
                    if (soldIDealerInventory.Any(x => x.VINNumber == vehicle.Vin))
                    {
                        var searchResult = soldIDealerInventory.First(x => x.VINNumber == vehicle.Vin);

                        var v = new whitmanenterprisedealershipinventory
                                    {
                                        ListingID = maxId,
                                        ModelYear = vehicle.Year,
                                        Make = vehicle.Make,
                                        Model = vehicle.Model,
                                        Trim = vehicle.Trim,
                                        VINNumber = vehicle.Vin,
                                        StockNumber = vehicle.StockNumber,
                                        SalePrice = vehicle.SalePrice,
                                        MSRP = vehicle.MSRP,
                                        Mileage = vehicle.Mileage,
                                        ExteriorColor = _commonHelper.UppercaseWords(vehicle.ExteriorColor),
                                        InteriorColor = _commonHelper.UppercaseWords(vehicle.InteriorColor),
                                        InteriorSurface = vehicle.InteriorSurface,
                                        BodyType = vehicle.BodyType,
                                        EngineType = vehicle.EngineType,
                                        DriveTrain = vehicle.DriveTrain,
                                        Cylinders = vehicle.Cylinders,
                                        Liters = vehicle.Liters,
                                        FuelType = vehicle.FuelType,
                                        Tranmission = vehicle.Tranmission,
                                        Doors = vehicle.Doors,
                                        Certified = vehicle.Certified,
                                        CarsOptions = vehicle.CarsOptions,
                                        Descriptions = vehicle.Descriptions,
                                        DefaultImageUrl = vehicle.DefaultImageURL,
                                        ThumbnailImageURL = searchResult.ThumbnailImageURL,
                                        CarImageUrl = searchResult.CarImageUrl,
                                        LastUpdated = DateTime.Now,
                                        DealershipName = vehicle.DealershipName,
                                        DealershipAddress = dealer.Address,
                                        DealershipCity = dealer.City,
                                        DealershipState = dealer.State,
                                        DealershipZipCode = dealer.ZipCode,
                                        DealershipId = dealer.DealerId,
                                        DealershipPhone = dealer.Phone,
                                        DealerCost = vehicle.DealerCost,
                                        ACV = vehicle.ACV,
                                        NewUsed = vehicle.NewUsed,
                                        AddToInventoryBy = "VinControlAdmin",
                                        FuelEconomyCity = vehicle.FuelEconomyCity,
                                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                                        StandardOptions = vehicle.StandardOptions,
                                        Recon = vehicle.Recon,
                                        PriorRental = false,
                                        DateInStock = vehicle.DateInStock
                                    };

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }

                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                        
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            v.WindowStickerPrice = salePriceNumber.ToString();

                            int pumpPriceNumber = salePriceNumber + 2000;

                            v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;
                        
                        context.AddTowhitmanenterprisedealershipinventories(v);

                        var removeVehicle = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == searchResult.ListingID);

                        context.Attach(removeVehicle);

                        context.DeleteObject(removeVehicle);

                        maxId++;
                    }
                    else
                    {
                        var v = new whitmanenterprisedealershipinventory()
                        {
                            ListingID = maxId,
                            ModelYear = vehicle.Year,
                            Make = vehicle.Make,
                            Model = vehicle.Model,
                            Trim = vehicle.Trim,
                            VINNumber = vehicle.Vin,
                            StockNumber = vehicle.StockNumber,
                            SalePrice = vehicle.SalePrice,
                            MSRP = vehicle.MSRP,
                            Mileage = vehicle.Mileage,
                            ExteriorColor = _commonHelper.UppercaseWords(vehicle.ExteriorColor),
                            InteriorColor = _commonHelper.UppercaseWords(vehicle.InteriorColor),
                            InteriorSurface = vehicle.InteriorSurface,
                            BodyType = vehicle.BodyType,
                            EngineType = vehicle.EngineType,
                            DriveTrain = vehicle.DriveTrain,
                            Cylinders = vehicle.Cylinders,
                            Liters = vehicle.Liters,
                            FuelType = vehicle.FuelType,
                            Tranmission = vehicle.Tranmission,
                            Doors = vehicle.Doors,
                            Certified = vehicle.Certified,
                            CarsOptions = vehicle.CarsOptions,
                            Descriptions = vehicle.Descriptions,
                            DefaultImageUrl = vehicle.DefaultImageURL,
                            ThumbnailImageURL = vehicle.DefaultImageURL,
                            CarImageUrl = vehicle.DefaultImageURL,
                            LastUpdated = DateTime.Now,
                            DealershipName = vehicle.DealershipName,
                            DealershipAddress = dealer.Address,
                            DealershipCity = dealer.City,
                            DealershipState = dealer.State,
                            DealershipZipCode = dealer.ZipCode,
                            DealershipId = dealer.DealerId,
                            DealershipPhone = dealer.Phone,
                            DealerCost = vehicle.DealerCost,
                            ACV = vehicle.ACV,
                            NewUsed = vehicle.NewUsed,
                            AddToInventoryBy = "VinControlAdmin",
                            FuelEconomyCity = vehicle.FuelEconomyCity,
                            FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                            StandardOptions = vehicle.StandardOptions,
                            Recon = vehicle.Recon,
                            PriorRental = false
                        };

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }

                        v.DateInStock = vehicle.DateInStock;

                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                        
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            v.SalePrice = salePriceNumber.ToString();

                            v.WindowStickerPrice = salePriceNumber.ToString();

                            int pumpPriceNumber = salePriceNumber + 2000;

                            v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                        context.AddTowhitmanenterprisedealershipinventories(v);
                        maxId++;
                    }
                }
            }
            else if (dealer.DealerId == 10997)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;

                var soldIDealerInventory = context.whitmanenterprisedealershipinventorysoldouts.Where(x => x.DealershipId == dealer.DealerId);

                foreach (var vehicle in dtNewInventory)
                {
                    if (soldIDealerInventory.Any(x => x.VINNumber == vehicle.Vin))
                    {
                        var searchResult = soldIDealerInventory.First(x => x.VINNumber == vehicle.Vin);

                        var v = new whitmanenterprisedealershipinventory
                                    {
                                        ListingID = maxId,
                                        ModelYear = vehicle.Year,
                                        Make = vehicle.Make,
                                        Model = vehicle.Model,
                                        Trim = vehicle.Trim,
                                        VINNumber = vehicle.Vin,
                                        StockNumber = vehicle.StockNumber,
                                        SalePrice = vehicle.SalePrice,
                                        MSRP = vehicle.MSRP,
                                        Mileage = vehicle.Mileage,
                                        ExteriorColor = vehicle.ExteriorColor,
                                        InteriorColor = vehicle.InteriorColor,
                                        InteriorSurface = vehicle.InteriorSurface,
                                        BodyType = vehicle.BodyType,
                                        EngineType = vehicle.EngineType,
                                        DriveTrain = vehicle.DriveTrain,
                                        Cylinders = vehicle.Cylinders,
                                        Liters = vehicle.Liters,
                                        FuelType = vehicle.FuelType,
                                        Tranmission = vehicle.Tranmission,
                                        Doors = vehicle.Doors,
                                        Certified = vehicle.Certified,
                                        CarsOptions = vehicle.CarsOptions,
                                        Descriptions = vehicle.Descriptions,
                                        DefaultImageUrl = vehicle.DefaultImageURL,
                                        ThumbnailImageURL = searchResult.ThumbnailImageURL,
                                        CarImageUrl = searchResult.CarImageUrl,
                                        LastUpdated = DateTime.Now,
                                        DealershipName = vehicle.DealershipName,
                                        DealershipAddress = dealer.Address,
                                        DealershipCity = dealer.City,
                                        DealershipState = dealer.State,
                                        DealershipZipCode = dealer.ZipCode,
                                        DealershipId = dealer.DealerId,
                                        DealershipPhone = dealer.Phone,
                                        DealerCost = vehicle.DealerCost,
                                        ACV = vehicle.ACV,
                                        NewUsed = vehicle.NewUsed,
                                        AddToInventoryBy = "VinControlAdmin",
                                        FuelEconomyCity = vehicle.FuelEconomyCity,
                                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                                        StandardOptions = vehicle.StandardOptions,
                                        Recon = vehicle.Recon,
                                        PriorRental = vehicle.PriorRental,
                                        DateInStock = vehicle.DateInStock
                                    };

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }
                        
                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                        
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            v.WindowStickerPrice = salePriceNumber.ToString();

                            int pumpPriceNumber = salePriceNumber + 2000;

                            v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;
                        
                        context.AddTowhitmanenterprisedealershipinventories(v);

                        var removeVehicle = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == searchResult.ListingID);

                        context.Attach(removeVehicle);

                        context.DeleteObject(removeVehicle);

                        maxId++;
                    }
                    else
                    {
                        var v = new whitmanenterprisedealershipinventory()
                        {
                            ListingID = maxId,
                            ModelYear = vehicle.Year,
                            Make = vehicle.Make,
                            Model = vehicle.Model,
                            Trim = vehicle.Trim,
                            VINNumber = vehicle.Vin,
                            StockNumber = vehicle.StockNumber,
                            SalePrice = vehicle.SalePrice,
                            MSRP = vehicle.MSRP,
                            Mileage = vehicle.Mileage,
                            ExteriorColor = vehicle.ExteriorColor,
                            InteriorColor = vehicle.InteriorColor,
                            InteriorSurface = vehicle.InteriorSurface,
                            BodyType = vehicle.BodyType,
                            EngineType = vehicle.EngineType,
                            DriveTrain = vehicle.DriveTrain,
                            Cylinders = vehicle.Cylinders,
                            Liters = vehicle.Liters,
                            FuelType = vehicle.FuelType,
                            Tranmission = vehicle.Tranmission,
                            Doors = vehicle.Doors,
                            Certified = vehicle.Certified,
                            CarsOptions = vehicle.CarsOptions,
                            Descriptions = vehicle.Descriptions,
                            ThumbnailImageURL = vehicle.DefaultImageURL,
                            CarImageUrl = vehicle.DefaultImageURL,
                            DateInStock = vehicle.DateInStock,
                            LastUpdated = DateTime.Now,
                            DealershipName = vehicle.DealershipName,
                            DealershipAddress = dealer.Address,
                            DealershipCity = dealer.City,
                            DealershipState = dealer.State,
                            DealershipZipCode = dealer.ZipCode,
                            DealershipId = dealer.DealerId,
                            DealershipPhone = dealer.Phone,
                            DealerCost = vehicle.DealerCost,
                            ACV = vehicle.ACV,
                            DefaultImageUrl = vehicle.DefaultImageUrl,
                            NewUsed = vehicle.NewUsed,
                            AddToInventoryBy = "VinControlAdmin",
                            FuelEconomyCity = vehicle.FuelEconomyCity,
                            FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                            StandardOptions = vehicle.StandardOptions,
                            Recon = vehicle.Recon,
                            PriorRental = false
                        };

                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                        
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            salePriceNumber = salePriceNumber + 2000;

                            v.WindowStickerPrice = salePriceNumber.ToString();

                            v.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Convert.ToInt32(v.RetailPrice) - salePriceNumber).ToString();
                        }

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                        context.AddTowhitmanenterprisedealershipinventories(v);
                        maxId++;
                    }
                }
            }
            else if (dealer.DealerId == 44675)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                foreach (var vehicle in dtNewInventory)
                {
                    var v = new whitmanenterprisedealershipinventory()
                    {
                        ListingID = maxId,
                        ModelYear = vehicle.Year,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        VINNumber = vehicle.Vin,
                        StockNumber = vehicle.StockNumber,
                        SalePrice = vehicle.SalePrice,
                        MSRP = vehicle.MSRP,
                        Mileage = vehicle.Mileage,
                        ExteriorColor = vehicle.ExteriorColor,
                        InteriorColor = vehicle.InteriorColor,
                        InteriorSurface = vehicle.InteriorSurface,
                        BodyType = vehicle.BodyType,
                        EngineType = vehicle.EngineType,
                        DriveTrain = vehicle.DriveTrain,
                        Cylinders = vehicle.Cylinders,
                        Liters = vehicle.Liters,
                        FuelType = vehicle.FuelType,
                        Tranmission = vehicle.Tranmission,
                        Doors = vehicle.Doors,
                        Certified = vehicle.Certified,
                        CarsOptions = vehicle.CarsOptions,
                        Descriptions = vehicle.Descriptions,
                        ThumbnailImageURL = vehicle.CarImageUrl,
                        CarImageUrl = vehicle.CarImageUrl,
                        DateInStock = vehicle.DateInStock,
                        LastUpdated = DateTime.Now,
                        DealershipName = vehicle.DealershipName,
                        DealershipAddress = dealer.Address,
                        DealershipCity = dealer.City,
                        DealershipState = dealer.State,
                        DealershipZipCode = dealer.ZipCode,
                        DealershipId = dealer.DealerId,
                        DealershipPhone = dealer.Phone,
                        DealerCost = vehicle.DealerCost,
                        ACV = vehicle.ACV,
                        DefaultImageUrl = vehicle.DefaultImageUrl,
                        NewUsed = vehicle.NewUsed,
                        AddToInventoryBy = "VinControlAdmin",
                        FuelEconomyCity = vehicle.FuelEconomyCity,
                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                        StandardOptions = vehicle.StandardOptions,
                        Recon = vehicle.Recon,
                        PriorRental = false,
                    };

                    v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";

                    if (v.Make.Equals("BMW"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                            v.Model = v.Trim;
                    }

                    if (v.Make.Equals("Mercedes-Benz"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                            v.Model = v.Trim;
                    }

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        v.WindowStickerPrice = salePriceNumber.ToString();

                        v.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        v.DealerDiscount = (Convert.ToInt32(v.RetailPrice) - salePriceNumber).ToString();
                    }

                    if (vehicle.NewUsed == "New")
                        v.CarFaxOwner = 0;
                    else
                        v.CarFaxOwner = vehicle.CarFaxOwner;

                    context.AddTowhitmanenterprisedealershipinventories(v);
                    maxId++;
                }
            }
            else if (dealer.DealerId == 37695)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                foreach (var vehicle in dtNewInventory)
                {
                    var v = new whitmanenterprisedealershipinventory()
                    {
                        ListingID = maxId,
                        ModelYear = vehicle.Year,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        VINNumber = vehicle.Vin,
                        StockNumber = vehicle.StockNumber,
                        SalePrice = vehicle.SalePrice,
                        MSRP = vehicle.MSRP,
                        Mileage = vehicle.Mileage,
                        ExteriorColor = vehicle.ExteriorColor,
                        InteriorColor = vehicle.InteriorColor,
                        InteriorSurface = vehicle.InteriorSurface,
                        BodyType = vehicle.BodyType,
                        EngineType = vehicle.EngineType,
                        DriveTrain = vehicle.DriveTrain,
                        Cylinders = vehicle.Cylinders,
                        Liters = vehicle.Liters,
                        FuelType = vehicle.FuelType,
                        Tranmission = vehicle.Tranmission,
                        Doors = vehicle.Doors,
                        Certified = vehicle.Certified,
                        CarsOptions = vehicle.CarsOptions,
                        Descriptions = vehicle.Descriptions,
                        ThumbnailImageURL = vehicle.DefaultImageURL,
                        CarImageUrl = vehicle.DefaultImageURL,
                        DateInStock = vehicle.DateInStock,
                        LastUpdated = DateTime.Now,
                        DealershipName = vehicle.DealershipName,
                        DealershipAddress = dealer.Address,
                        DealershipCity = dealer.City,
                        DealershipState = dealer.State,
                        DealershipZipCode = dealer.ZipCode,
                        DealershipId = dealer.DealerId,
                        DealershipPhone = dealer.Phone,
                        DealerCost = vehicle.DealerCost,
                        ACV = vehicle.ACV,
                        DefaultImageUrl = vehicle.DefaultImageUrl,
                        NewUsed = vehicle.NewUsed,
                        AddToInventoryBy = "VinControlAdmin",
                        FuelEconomyCity = vehicle.FuelEconomyCity,
                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                        StandardOptions = vehicle.StandardOptions,
                        Recon = vehicle.Recon,
                        PriorRental = false,
                    };

                    v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";

                    if (v.Make.Equals("BMW"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                            v.Model = v.Trim;
                    }

                    if (v.Make.Equals("Mercedes-Benz"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                            v.Model = v.Trim;
                    }

                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        v.WindowStickerPrice = salePriceNumber.ToString();

                        v.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        v.DealerDiscount = (Convert.ToInt32(v.RetailPrice) - salePriceNumber).ToString();
                    }

                    v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                    context.AddTowhitmanenterprisedealershipinventories(v);
                    maxId++;
                }
            }
            else if (dealer.DealerId == 12293)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                foreach (var vehicle in dtNewInventory)
                {
                    var v = new whitmanenterprisedealershipinventory()
                    {
                        ListingID = maxId,
                        ModelYear = vehicle.Year,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        VINNumber = vehicle.Vin,
                        StockNumber = vehicle.StockNumber,
                        SalePrice = vehicle.SalePrice,
                        MSRP = vehicle.MSRP,
                        Mileage = vehicle.Mileage,
                        ExteriorColor = vehicle.ExteriorColor,
                        InteriorColor = vehicle.InteriorColor,
                        InteriorSurface = vehicle.InteriorSurface,
                        BodyType = vehicle.BodyType,
                        EngineType = vehicle.EngineType,
                        DriveTrain = vehicle.DriveTrain,
                        Cylinders = vehicle.Cylinders,
                        Liters = vehicle.Liters,
                        FuelType = vehicle.FuelType,
                        Tranmission = vehicle.Tranmission,
                        Doors = vehicle.Doors,
                        Certified = vehicle.Certified,
                        CarsOptions = vehicle.CarsOptions,
                        Descriptions = vehicle.Descriptions,
                        ThumbnailImageURL = vehicle.CarImageUrl,
                        CarImageUrl = vehicle.CarImageUrl,
                        DateInStock = vehicle.DateInStock,
                        LastUpdated = DateTime.Now,
                        DealershipName = vehicle.DealershipName,
                        DealershipAddress = dealer.Address,
                        DealershipCity = dealer.City,
                        DealershipState = dealer.State,
                        DealershipZipCode = dealer.ZipCode,
                        DealershipId = dealer.DealerId,
                        DealershipPhone = dealer.Phone,
                        DealerCost = vehicle.DealerCost,
                        ACV = vehicle.ACV,
                        DefaultImageUrl = vehicle.DefaultImageUrl,
                        NewUsed = vehicle.NewUsed,
                        AddToInventoryBy = "VinControlAdmin",
                        FuelEconomyCity = vehicle.FuelEconomyCity,
                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                        StandardOptions = vehicle.StandardOptions,
                        Recon = vehicle.Recon,
                        PriorRental = false,
                    };

                    if (v.Make.Equals("BMW"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                            v.Model = v.Trim;
                    }

                    if (v.Make.Equals("Mercedes-Benz"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                            v.Model = v.Trim;
                    }

                    v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                    
                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        v.WindowStickerPrice = salePriceNumber.ToString();

                        v.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        v.DealerDiscount = (Convert.ToInt32(v.RetailPrice) - salePriceNumber).ToString();
                    }

                    v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                    context.AddTowhitmanenterprisedealershipinventories(v);
                    maxId++;
                }
            }
            else if (dealer.DealerId == 12293 || dealer.DealerId == 2584 || dealer.DealerId == 23063 || dealer.DealerId == 1563 || dealer.DealerId == 44674 || dealer.DealerId == 1541)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                foreach (var vehicle in dtNewInventory)
                {
                    var v = new whitmanenterprisedealershipinventory()
                    {
                        ListingID = maxId,
                        ModelYear = vehicle.Year,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        VINNumber = vehicle.Vin,
                        StockNumber = vehicle.StockNumber,
                        SalePrice = vehicle.SalePrice,
                        MSRP = vehicle.MSRP,
                        Mileage = vehicle.Mileage,
                        ExteriorColor = vehicle.ExteriorColor,
                        InteriorColor = vehicle.InteriorColor,
                        InteriorSurface = vehicle.InteriorSurface,
                        BodyType = vehicle.BodyType,
                        EngineType = vehicle.EngineType,
                        DriveTrain = vehicle.DriveTrain,
                        Cylinders = vehicle.Cylinders,
                        Liters = vehicle.Liters,
                        FuelType = vehicle.FuelType,
                        Tranmission = vehicle.Tranmission,
                        Doors = vehicle.Doors,
                        Certified = vehicle.Certified,
                        CarsOptions = vehicle.CarsOptions,
                        Descriptions = vehicle.Descriptions,
                        ThumbnailImageURL = vehicle.ThumbnalImageurl,
                        CarImageUrl = vehicle.CarImageUrl,
                        DateInStock = vehicle.DateInStock,
                        LastUpdated = DateTime.Now,
                        DealershipName = vehicle.DealershipName,
                        DealershipAddress = dealer.Address,
                        DealershipCity = dealer.City,
                        DealershipState = dealer.State,
                        DealershipZipCode = dealer.ZipCode,
                        DealershipPhone = dealer.Phone,
                        DealershipId = vehicle.DealerId,
                        DealerCost = vehicle.DealerCost,
                        ACV = vehicle.ACV,
                        DefaultImageUrl = vehicle.DefaultImageUrl,
                        NewUsed = vehicle.NewUsed,
                        AddToInventoryBy = "VinControlAdmin",
                        FuelEconomyCity = vehicle.FuelEconomyCity,
                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                        StandardOptions = vehicle.StandardOptions,
                        Recon = vehicle.Recon,
                        PriorRental = false,
                    };

                    v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                    if (v.Make.Equals("BMW"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                            v.Model = v.Trim;
                    }

                    if (v.Make.Equals("Mercedes-Benz"))
                    {
                        if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                            v.Model = v.Trim;
                    }
                    
                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        v.WindowStickerPrice = salePriceNumber.ToString();

                        v.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        v.DealerDiscount = (Convert.ToInt32(v.RetailPrice) - salePriceNumber).ToString();
                    }

                    v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                    context.AddTowhitmanenterprisedealershipinventories(v);
                    maxId++;
                }
            }
            else if (dealer.DealerId == 5438)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;

                var soldIDealerInventory = context.whitmanenterprisedealershipinventorysoldouts.Where(x => x.DealershipId == dealer.DealerId);
                
                foreach (var vehicle in dtNewInventory)
                {
                    if (!soldIDealerInventory.Any(x => x.VINNumber == vehicle.Vin))
                    {
                        var v = new whitmanenterprisedealershipinventory()
                        {
                            ListingID = maxId,
                            ModelYear = vehicle.Year,
                            Make = vehicle.Make,
                            Model = vehicle.Model,
                            Trim = vehicle.Trim,
                            VINNumber = vehicle.Vin,
                            StockNumber = vehicle.StockNumber,
                            SalePrice = vehicle.SalePrice,
                            MSRP = vehicle.MSRP,
                            Mileage = vehicle.Mileage,
                            ExteriorColor = vehicle.ExteriorColor,
                            InteriorColor = vehicle.InteriorColor,
                            InteriorSurface = vehicle.InteriorSurface,
                            BodyType = vehicle.BodyType,
                            EngineType = vehicle.EngineType,
                            DriveTrain = vehicle.DriveTrain,
                            Cylinders = vehicle.Cylinders,
                            Liters = vehicle.Liters,
                            FuelType = vehicle.FuelType,
                            Tranmission = vehicle.Tranmission,
                            Doors = vehicle.Doors,
                            Certified = vehicle.Certified,
                            CarsOptions = vehicle.CarsOptions,
                            Descriptions = vehicle.Descriptions,
                            DefaultImageUrl = vehicle.DefaultImageURL,
                            ThumbnailImageURL = vehicle.ThumbnalImageurl,
                            CarImageUrl = vehicle.CarImageUrl,
                            LastUpdated = DateTime.Now,
                            DealershipName = vehicle.DealershipName,
                            DealershipAddress = dealer.Address,
                            DealershipCity = dealer.City,
                            DealershipState = dealer.State,
                            DealershipZipCode = dealer.ZipCode,
                            DealershipId = dealer.DealerId,
                            DealershipPhone = dealer.Phone,
                            DealerCost = vehicle.DealerCost,
                            ACV = vehicle.ACV,
                            NewUsed = vehicle.NewUsed,
                            AddToInventoryBy = "VinControlAdmin",
                            FuelEconomyCity = vehicle.FuelEconomyCity,
                            FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                            StandardOptions = vehicle.StandardOptions,
                            Recon = vehicle.Recon,
                            PriorRental = false
                        };

                        v.DateInStock = vehicle.DateInStock;

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }
                        
                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";

                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            v.WindowStickerPrice = salePriceNumber.ToString();

                            int pumpPriceNumber = salePriceNumber + 2000;

                            v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;
                        
                        context.AddTowhitmanenterprisedealershipinventories(v);

                        maxId++;
                    }
                }
            }
            else if (dealer.DealerId == 3636)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                
                var soldIDealerInventory = context.whitmanenterprisedealershipinventorysoldouts.Where(x => x.DealershipId == dealer.DealerId);

                foreach (var vehicle in dtNewInventory)
                {
                    if (soldIDealerInventory.Any(x => x.VINNumber == vehicle.Vin))
                    {
                        var searchResult = soldIDealerInventory.First(x => x.VINNumber == vehicle.Vin);

                        var v = new whitmanenterprisedealershipinventory
                                    {
                                        ListingID = maxId,
                                        ModelYear = vehicle.Year,
                                        Make = vehicle.Make,
                                        Model = vehicle.Model,
                                        Trim = vehicle.Trim,
                                        VINNumber = vehicle.Vin,
                                        StockNumber = vehicle.StockNumber,
                                        SalePrice = vehicle.SalePrice,
                                        MSRP = vehicle.MSRP,
                                        Mileage = vehicle.Mileage,
                                        ExteriorColor = _commonHelper.UppercaseWords(vehicle.ExteriorColor),
                                        InteriorColor = _commonHelper.UppercaseWords(vehicle.InteriorColor),
                                        InteriorSurface = vehicle.InteriorSurface,
                                        BodyType = vehicle.BodyType,
                                        EngineType = vehicle.EngineType,
                                        DriveTrain = vehicle.DriveTrain,
                                        Cylinders = vehicle.Cylinders,
                                        Liters = vehicle.Liters,
                                        FuelType = vehicle.FuelType,
                                        Tranmission = vehicle.Tranmission,
                                        Doors = vehicle.Doors,
                                        Certified = vehicle.Certified,
                                        CarsOptions = vehicle.CarsOptions,
                                        Descriptions = vehicle.Descriptions,
                                        DefaultImageUrl = vehicle.DefaultImageURL,
                                        ThumbnailImageURL = searchResult.ThumbnailImageURL,
                                        CarImageUrl = searchResult.CarImageUrl,
                                        LastUpdated = DateTime.Now,
                                        DealershipName = vehicle.DealershipName,
                                        DealershipAddress = dealer.Address,
                                        DealershipCity = dealer.City,
                                        DealershipState = dealer.State,
                                        DealershipZipCode = dealer.ZipCode,
                                        DealershipId = dealer.DealerId,
                                        DealershipPhone = dealer.Phone,
                                        DealerCost = vehicle.DealerCost,
                                        ACV = vehicle.ACV,
                                        NewUsed = vehicle.NewUsed,
                                        AddToInventoryBy = "VinControlAdmin",
                                        FuelEconomyCity = vehicle.FuelEconomyCity,
                                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                                        StandardOptions = vehicle.StandardOptions,
                                        Recon = vehicle.Recon,
                                        PriorRental = false,
                                        DateInStock = vehicle.DateInStock
                                    };

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }
                        
                        v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                        
                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (flag && salePriceNumber != 0)
                        {
                            v.WindowStickerPrice = salePriceNumber.ToString();

                            int pumpPriceNumber = salePriceNumber + 2000;

                            v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                            v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;
                        
                        context.AddTowhitmanenterprisedealershipinventories(v);

                        var removeVehicle = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == searchResult.ListingID);

                        context.Attach(removeVehicle);

                        context.DeleteObject(removeVehicle);

                        maxId++;
                    }
                    else
                    {
                        var v = new whitmanenterprisedealershipinventory
                                    {
                                        ListingID = maxId,
                                        ModelYear = vehicle.Year,
                                        Make = vehicle.Make,
                                        Model = vehicle.Model,
                                        Trim = vehicle.Trim,
                                        VINNumber = vehicle.Vin,
                                        StockNumber = vehicle.StockNumber,
                                        SalePrice = vehicle.SalePrice,
                                        MSRP = vehicle.MSRP,
                                        Mileage = vehicle.Mileage,
                                        ExteriorColor = _commonHelper.UppercaseWords(vehicle.ExteriorColor),
                                        InteriorColor = _commonHelper.UppercaseWords(vehicle.InteriorColor),
                                        InteriorSurface = vehicle.InteriorSurface,
                                        BodyType = vehicle.BodyType,
                                        EngineType = vehicle.EngineType,
                                        DriveTrain = vehicle.DriveTrain,
                                        Cylinders = vehicle.Cylinders,
                                        Liters = vehicle.Liters,
                                        FuelType = vehicle.FuelType,
                                        Tranmission = vehicle.Tranmission,
                                        Doors = vehicle.Doors,
                                        Certified = vehicle.Certified,
                                        CarsOptions = vehicle.CarsOptions,
                                        Descriptions = vehicle.Descriptions,
                                        ThumbnailImageURL = "",
                                        CarImageUrl = "",
                                        DateInStock = vehicle.DateInStock,
                                        LastUpdated = DateTime.Now,
                                        DealershipName = vehicle.DealershipName,
                                        DealershipAddress = dealer.Address,
                                        DealershipCity = dealer.City,
                                        DealershipState = dealer.State,
                                        DealershipZipCode = dealer.ZipCode,
                                        DealershipId = dealer.DealerId,
                                        DealershipPhone = dealer.Phone,
                                        DealerCost = vehicle.DealerCost,
                                        ACV = vehicle.ACV,
                                        DefaultImageUrl = vehicle.DefaultImageUrl,
                                        NewUsed = vehicle.NewUsed,
                                        AddToInventoryBy = "VinControlAdmin",
                                        FuelEconomyCity = vehicle.FuelEconomyCity,
                                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                                        StandardOptions = vehicle.StandardOptions,
                                        Recon = vehicle.Recon,
                                        PriorRental = false,
                                        PreWholeSale = vehicle.WholeSale,
                                        VehicleType = vehicle.IsTruck ? "Truck" : "Car"
                                    };

                        if (v.Make.Equals("BMW"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Series"))
                                v.Model = v.Trim;
                        }

                        if (v.Make.Equals("Mercedes-Benz"))
                        {
                            if (!String.IsNullOrEmpty(v.Model) && v.Model.Contains("Class"))
                                v.Model = v.Trim;
                        }

                        int salePriceNumber;

                        bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                        if (v.NewUsed == "Used")
                        {
                            if (flag && salePriceNumber != 0)
                            {
                                salePriceNumber = salePriceNumber + 2000;

                                v.WindowStickerPrice = salePriceNumber.ToString();

                                v.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                                v.DealerDiscount = (Convert.ToInt32(v.RetailPrice) - salePriceNumber).ToString();
                            }
                        }
                        else if (v.NewUsed == "New")
                        {
                            v.SalePrice = salePriceNumber.ToString();
                            v.RetailPrice = salePriceNumber.ToString();

                            if (context.vincontrolrebates.Any(x => x.Year == v.ModelYear && x.Make == v.Make && x.Model == v.Model && x.Trim == v.Trim))
                            {
                                v.ManufacturerRebate = context.vincontrolrebates.First(x => x.Year == v.ModelYear && x.Make == v.Make && x.Model == v.Model && x.Trim == v.Trim).ManufactureReabte;

                                int rebateAmmount;

                                Int32.TryParse(v.ManufacturerRebate, out rebateAmmount);

                                int finalPrice = salePriceNumber - rebateAmmount;

                                v.SalePrice = finalPrice.ToString();
                            }
                            else
                            {
                                v.ManufacturerRebate = "0";
                            }
                        }

                        v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                        context.AddTowhitmanenterprisedealershipinventories(v);
                        maxId++;
                    }
                }
            }

            context.SaveChanges();
        }
        
        public void InsertToInvetoryRecon(List<VehicleViewModel> dtNewInventory, DealerViewModel dealer)
        {
            var context = new whitmanenterprisewarehouseEntities();

            if (dealer.DealerId == 2183)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                foreach (var vehicle in dtNewInventory)
                {
                    var v = new whitmanenterprisedealershipinventory()
                    {
                        ListingID = maxId,
                        ModelYear = vehicle.Year,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        VINNumber = vehicle.Vin,
                        StockNumber = vehicle.StockNumber,
                        SalePrice = "0",
                        MSRP = vehicle.MSRP,
                        Mileage = vehicle.Mileage,
                        ExteriorColor = vehicle.ExteriorColor,
                        InteriorColor = vehicle.InteriorColor,
                        InteriorSurface = vehicle.InteriorSurface,
                        BodyType = vehicle.BodyType,
                        EngineType = vehicle.EngineType,
                        DriveTrain = vehicle.DriveTrain,
                        Cylinders = vehicle.Cylinders,
                        Liters = vehicle.Liters,
                        FuelType = vehicle.FuelType,
                        Tranmission = vehicle.Tranmission,
                        Doors = vehicle.Doors,
                        Certified = vehicle.Certified,
                        CarsOptions = vehicle.CarsOptions,
                        Descriptions = vehicle.Descriptions,
                        ThumbnailImageURL = vehicle.DefaultImageURL,
                        CarImageUrl = vehicle.DefaultImageURL,
                        DateInStock = DateTime.Now.AddDays(vehicle.DaysInInventory * (-1)),
                        LastUpdated = DateTime.Now,
                        DealershipName = vehicle.DealershipName,
                        DealershipAddress = vehicle.DealershipAddress,
                        DealershipCity = vehicle.DealershipCity,
                        DealershipState = vehicle.DealershipState,
                        DealershipZipCode = vehicle.DealershipZipCode,
                        DealershipId = vehicle.DealerId,
                        DealerCost = vehicle.DealerCost,
                        ACV = vehicle.ACV,
                        DefaultImageUrl = vehicle.DefaultImageUrl,
                        NewUsed = vehicle.NewUsed,
                        AddToInventoryBy = "VinControlAdmin",
                        FuelEconomyCity = vehicle.FuelEconomyCity,
                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                        StandardOptions = vehicle.StandardOptions,
                        Recon = true
                    };

                    v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                    
                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        v.WindowStickerPrice = salePriceNumber.ToString();

                        int pumpPriceNumber = salePriceNumber + 2000;

                        v.RetailPrice = (Math.Round(pumpPriceNumber * 1.1)).ToString();

                        v.DealerDiscount = (Math.Round(pumpPriceNumber * 1.1) - salePriceNumber).ToString();
                    }

                    v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                    context.AddTowhitmanenterprisedealershipinventories(v);
                    maxId++;
                }
            }
            else if (dealer.DealerId == 10997)
            {
                int maxId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                foreach (var vehicle in dtNewInventory)
                {
                    var v = new whitmanenterprisedealershipinventory()
                    {
                        ListingID = maxId,
                        ModelYear = vehicle.Year,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        VINNumber = vehicle.Vin,
                        StockNumber = vehicle.StockNumber,
                        SalePrice = vehicle.SalePrice,
                        MSRP = vehicle.MSRP,
                        Mileage = vehicle.Mileage,
                        ExteriorColor = vehicle.ExteriorColor,
                        InteriorColor = vehicle.InteriorColor,
                        InteriorSurface = vehicle.InteriorSurface,
                        BodyType = vehicle.BodyType,
                        EngineType = vehicle.EngineType,
                        DriveTrain = vehicle.DriveTrain,
                        Cylinders = vehicle.Cylinders,
                        Liters = vehicle.Liters,
                        FuelType = vehicle.FuelType,
                        Tranmission = vehicle.Tranmission,
                        Doors = vehicle.Doors,
                        Certified = vehicle.Certified,
                        CarsOptions = vehicle.CarsOptions,
                        Descriptions = vehicle.Descriptions,
                        ThumbnailImageURL = vehicle.DefaultImageURL,
                        CarImageUrl = vehicle.DefaultImageURL,
                        DateInStock = DateTime.Now.AddDays(vehicle.DaysInInventory * (-1)),
                        LastUpdated = DateTime.Now,
                        DealershipName = vehicle.DealershipName,
                        DealershipAddress = vehicle.DealershipAddress,
                        DealershipCity = vehicle.DealershipCity,
                        DealershipState = vehicle.DealershipState,
                        DealershipZipCode = vehicle.DealershipZipCode,
                        DealershipId = vehicle.DealerId,
                        DealerCost = vehicle.DealerCost,
                        ACV = vehicle.ACV,
                        DefaultImageUrl = vehicle.DefaultImageUrl,
                        NewUsed = vehicle.NewUsed,
                        AddToInventoryBy = "VinControlAdmin",
                        FuelEconomyCity = vehicle.FuelEconomyCity,
                        FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                        StandardOptions = vehicle.StandardOptions,
                        Recon = true
                    };

                    v.VehicleType = vehicle.IsTruck ? "Truck" : "Car";
                    
                    int salePriceNumber;

                    bool flag = Int32.TryParse(vehicle.SalePrice, out salePriceNumber);

                    if (flag && salePriceNumber != 0)
                    {
                        salePriceNumber = salePriceNumber + 2000;

                        v.WindowStickerPrice = salePriceNumber.ToString();

                        v.RetailPrice = (Math.Round(salePriceNumber * 1.1)).ToString();

                        v.DealerDiscount = (Convert.ToInt32(v.RetailPrice) - salePriceNumber).ToString();
                    }

                    v.CarFaxOwner = vehicle.NewUsed == "New" ? 0 : vehicle.CarFaxOwner;

                    context.AddTowhitmanenterprisedealershipinventories(v);
                    maxId++;
                }
            }

            context.SaveChanges();
        }

        public void InsertToAppraisal(List<VehicleViewModel> dtNewInventory)
        {
            var context = new whitmanenterprisewarehouseEntities();

            foreach (var vehicle in dtNewInventory)
            {
                var v = new whitmanenterpriseappraisal()
                {
                    ModelYear = vehicle.Year,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    Trim = vehicle.Trim,
                    VINNumber = vehicle.Vin,
                    StockNumber = vehicle.StockNumber,
                    SalePrice = vehicle.SalePrice,
                    MSRP = vehicle.MSRP,
                    Mileage = vehicle.Mileage,
                    ExteriorColor = vehicle.ExteriorColor,
                    InteriorColor = vehicle.InteriorColor,
                    InteriorSurface = vehicle.InteriorSurface,
                    BodyType = vehicle.BodyType,
                    EngineType = vehicle.EngineType,
                    DriveTrain = vehicle.DriveTrain,
                    Cylinders = vehicle.Cylinders,
                    Liters = vehicle.Liters,
                    FuelType = vehicle.FuelType,
                    Tranmission = vehicle.Tranmission,
                    Doors = vehicle.Doors,
                    Certified = vehicle.Certified,
                    CarsOptions = vehicle.CarsOptions,
                    Descriptions = vehicle.Descriptions,
                    CarImageUrl = vehicle.CarImageUrl,
                    AppraisalDate = DateTime.Now.AddDays(vehicle.DaysInInventory * (-1)),
                    LastUpdated = DateTime.Now,
                    DealershipName = vehicle.DealershipName,
                    DealershipAddress = vehicle.DealershipAddress,
                    DealershipCity = vehicle.DealershipCity,
                    DealershipState = vehicle.DealershipState,
                    DealershipId = vehicle.DealerId,
                    DealerCost = vehicle.DealerCost,
                    ACV = vehicle.ACV,
                    DefaultImageUrl = vehicle.DefaultImageUrl,
                    StandardOptions = vehicle.StandardOptions,
                    FuelEconomyCity = vehicle.FuelEconomyCity,
                    FuelEconomyHighWay = vehicle.FuelEconomyHighWay,
                    ChromeModelId = "",
                    ChromeStyleId = "",
                    VehicleType = "Car",
                    TruckCategory = "",
                    TruckClass = "",
                    TruckType = ""
                };

                context.AddTowhitmanenterpriseappraisals(v);
            }

            context.SaveChanges();
        }
        
        public int CheckVinHasCarFaxReport(string vin)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenteprisecarfaxes.Any(o => o.Vin.Equals(vin)))
                {
                    var firstTmp = context.whitmanenteprisecarfaxes.FirstOrDefault(o => o.Vin.Equals(vin));

                    DateTime dt = DateTime.Parse(firstTmp.ExpiredDate.ToString());

                    return dt.Date >= DateTime.Now.Date ? 1 : 2;
                }
            }

            return 0;
        }
        
        public void TransferToWholeSaleFromInventory(int[] listingIdList)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                foreach (var listingId in listingIdList)
                {
                    var wdi = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == listingId);

                    int autoListingId = 1000;
                    if (context.vincontrolwholesaleinventories.Any())
                        autoListingId = context.vincontrolwholesaleinventories.Max(x => x.ListingID) + 1;

                    if (!context.vincontrolwholesaleinventories.Any(x => x.VINNumber == wdi.VINNumber && x.DealershipId == wdi.DealershipId))
                    {
                        var vehicle = new vincontrolwholesaleinventory
                                          {
                                              ListingID = autoListingId,
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
                                              AppraisalID = wdi.AppraisalID,
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
                                              PriorRental = wdi.PriorRental,
                                              VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType,
                                              TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory,
                                              TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass,
                                              TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType,
                                          };
                        
                        context.AddTovincontrolwholesaleinventories(vehicle);
                    }

                    context.Attach(wdi);

                    context.DeleteObject(wdi);

                    context.SaveChanges();
                }
            }
        }

        public int MarkUnsoldVehicle(whitmanenterprisedealershipinventorysoldout wdi)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int autoId = context.whitmanenterprisedealershipinventories.Max(x => x.ListingID) + 1;
                var vehicle = new whitmanenterprisedealershipinventory
                                  {
                                      ListingID = autoId,
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
                                      StandardOptions = wdi.StandardOptions,
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
                                      DealershipZipCode = wdi.ZipCode,
                                      DefaultImageUrl = wdi.DefaultImageUrl,
                                      NewUsed = wdi.NewUsed,
                                      AddToInventoryBy = wdi.AddToInventoryBy,
                                      AppraisalID = wdi.AppraisalID,
                                      ACV = wdi.ACV,
                                      DealerCost = wdi.DealerCost,
                                      FuelEconomyCity = wdi.FuelEconomyCity,
                                      FuelEconomyHighWay = wdi.FuelEconomyHighWay,
                                      WarrantyInfo = wdi.WarrantyInfo,
                                      RetailPrice = wdi.RetailPrice,
                                      DealerDiscount = wdi.DealerDiscount,
                                      ManufacturerRebate = wdi.ManufacturerRebate,
                                      WindowStickerPrice = wdi.WindowStickerPrice,
                                      CarFaxOwner = wdi.CarFaxOwner,
                                      PriorRental = wdi.PriorRental,
                                      Recon = wdi.Recon,
                                      KBBOptionsId = wdi.KBBOptionsId,
                                      KBBTrimId = wdi.KBBTrimId,
                                      Disclaimer = wdi.Disclaimer,
                                      VehicleType = String.IsNullOrEmpty(wdi.VehicleType) ? "" : wdi.VehicleType,
                                      TruckCategory = String.IsNullOrEmpty(wdi.TruckCategory) ? "" : wdi.TruckCategory,
                                      TruckClass = String.IsNullOrEmpty(wdi.TruckClass) ? "" : wdi.TruckClass,
                                      TruckType = String.IsNullOrEmpty(wdi.TruckType) ? "" : wdi.TruckType
                                  };

                var removeVehicle = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == wdi.ListingID);

                context.AddTowhitmanenterprisedealershipinventories(vehicle);

                context.Attach(removeVehicle);

                context.DeleteObject(removeVehicle);

                context.SaveChanges();

                return autoId;
            }
        }

        public List<Column> GetInventoryColumnNames()
        {
            var columns = new List<Column>();
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var tableName = context.whitmanenterprisedealershipinventories.Name;
                var properties = context.whitmanenterprisedealershipinventories.EntitySet.ElementType.Properties;//context.whitmanenterprisedealershipinventories.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    columns.Add(new Column() { Selected = false, Text = propertyInfo.Name, Value = String.Format("{0}.{1}", tableName, propertyInfo.Name) });
                }
            }

            return columns.OrderBy(i=>i.Text).ToList();
        }

        public List<DealerViewModel> GetSampleExportList()
        {
            var result = new List<DealerViewModel>();
            using (var context = new vincontrolwarehouseEntities())
            {
                var dealers = context.dealers.OrderBy(i => i.Name).Take(10).Skip(0).ToList();
                foreach (var dealer in dealers)
                {
                    result.Add(new DealerViewModel(dealer));
                }
            }

            return result;
        }

        public List<DealerViewModel> GetDealerExportList(int companyProfileId)
        {
            var result = new List<DealerViewModel>();
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingCompanyProfile = context.datafeedprofiles.FirstOrDefault(i => i.Id == companyProfileId && (i.Discontinued == null || (i.Discontinued != null && !i.Discontinued.Value)));
                if (existingCompanyProfile != null)
                {
                    var lookups = context.datafeedlookups.Where(i => i.DataFeedProfileId == existingCompanyProfile.Id).ToList();
                    foreach (var datafeedlookup in lookups)
                    {
                        //if (!datafeedlookup.Discontinued.GetValueOrDefault())
                        //    result.Add(new DealerViewModel(datafeedlookup.dealer));
                    }
                }
            }

            return result;
        }

        public datafeedprofile LoadCompanyProfile(int companyProfileId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                return context.datafeedprofiles.FirstOrDefault(i => i.Id == companyProfileId);
            }
        }

        public ImportHistoryViewModel GetLastImportDepositedFeed(int dealerId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var lastItem = context.importservicehistories.OrderByDescending(i => i.RunningDate.GetValueOrDefault()).FirstOrDefault(i => i.DealerId == dealerId);
                return lastItem == null ? null : new ImportHistoryViewModel(lastItem);
            }
        }

        public ExportHistoryViewModel GetLastExportDepositedFeed(int datafeedProfileId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var lastItem = context.exportservicehistories.OrderByDescending(i => i.RunningDate.GetValueOrDefault()).FirstOrDefault(i => i.DatafeedProfileId == datafeedProfileId);
                return lastItem == null ? null : new ExportHistoryViewModel(lastItem);
            }
        }

        public string GetImportDataFeedPath(int dealerId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var importProfile = context.dealers_dealersetting.FirstOrDefault(i => i.DealershipId == dealerId);
                return importProfile != null? importProfile.ImportFeedUrl : string.Empty;
            }
        }

        public List<int> GetDealerListByProfileId(int importProfileId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                return context.dealers_dealersetting.Where(i => i.ImportDataFeedProfileId == importProfileId).Select(i => i.DealershipId.Value).ToList();
            }
        }

        public string GetDealerExportFileName(int dealerId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var lookupProfile = context.datafeedlookups.FirstOrDefault(i => i.DealerId == dealerId);
                return lookupProfile != null ? lookupProfile.FileName : dealerId.ToString();
            }
        }

        public string GetDealerExportFileName(int dealerId, datafeedprofile companyProfile)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var lookupProfile = context.datafeedlookups.FirstOrDefault(i => i.DealerId == dealerId&& i.DataFeedProfileId== companyProfile.Id);
                return lookupProfile != null ? lookupProfile.FileName : dealerId.ToString();
            }
        }

        public dealers_dealersetting GetDealerSettingByDealerId(int dealerId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                return context.dealers_dealersetting.FirstOrDefault(i => i.DealershipId == dealerId);
            }
        }

        public importservicehistory CreateImportDataFeedHistory(int profileId, int dealerId, string fileName)
        {
            var importHistory = new importservicehistory();

            using (var context = new vincontrolwarehouseEntities())
            {
                importHistory.RunningDate = DateTime.Now;
                importHistory.Status = RunningStatus.Running;
                importHistory.DealerId = dealerId;
                importHistory.ArchiveFileName = fileName;
                importHistory.ImportProfileId = profileId;

                context.AddToimportservicehistories(importHistory);
                context.SaveChanges();
            }

            return importHistory;
        }

        public void MarkImportDataFeedTaskCompleted(int id)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingTask = context.importservicehistories.FirstOrDefault(i => i.Id == id);
                if (existingTask != null)
                {
                    existingTask.Status = RunningStatus.Completed;
                    existingTask.CompletedDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public exportservicehistory CreateExportDataFeedHistory(int profileId, int dealerId, string fileName)
        {
            var exportHistory = new exportservicehistory();

            using (var context = new vincontrolwarehouseEntities())
            {
                exportHistory.RunningDate = DateTime.Now;
                exportHistory.Status = RunningStatus.Running;
                exportHistory.DealerId = dealerId;
                exportHistory.ArchiveFileName = fileName;
                exportHistory.DatafeedProfileId = profileId;

                context.AddToexportservicehistories(exportHistory);
                context.SaveChanges();
            }

            return exportHistory;
        }

        public void MarkExportDataFeedTaskCompleted(int id)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingTask = context.exportservicehistories.FirstOrDefault(i => i.Id == id);
                if (existingTask != null)
                {
                    existingTask.Status = RunningStatus.Completed;
                    existingTask.RunningDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }
    }
}
