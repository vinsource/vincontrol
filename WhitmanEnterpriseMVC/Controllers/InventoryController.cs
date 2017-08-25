using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Security;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using System.IO;
using System.Web.UI.DataVisualization.Charting;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class InventoryController : SecurityController
    {
        //
        // GET: /Inventory/

        public ActionResult Index()
        {
            return View("ViewInventory");
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult TransferToWholeSaleFromInventory(int ListingId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                SQLHelper.TransferToWholeSaleFromInventory(ListingId);

                return RedirectToAction("ViewWholeSaleInventory");

            }

        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult TransferToWholeSaleFromSoldInventory(int ListingId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {

                SQLHelper.TransferToWholeSaleFromSoldInventory(ListingId);



                return RedirectToAction("ViewWholeSaleInventory");

            }

        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult TransferToInventoryFromWholesale(int ListingId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {

                int autoListingId = SQLHelper.TransferToInventoryFromWholesale(ListingId);

                return RedirectToAction("ViewIProfile", new { ListingID = autoListingId });


            }

        }

        [VinControlAuthorization(PermissionCode = "WHOLESALE", AcceptedValues = "ALLACCESS")]
        public ActionResult ViewWholeSaleInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list = new List<CarInfoFormViewModel>();

                foreach (var tmp in InventoryQueryHelper.GetSingleOrGroupWholesaleInventory(context))
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")

                    };
                    list.Add(car);
                }


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                Session["InventoryObject"] = viewModel;


                return View("ViewWholeSaleInventory", viewModel);
            }
        }

        public ActionResult MissContentSmallInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            var context = new whitmanenterprisewarehouseEntities();

            var avaiInventory =
                               from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                               where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                               select e;

            var viewModel = new InventoryFormViewModel { IsCompactView = false,IsMissingContent = true};

            if (Session["DealerGroup"] != null)
            {
                viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
            }
            else
                viewModel.DealerList = SelectListHelper.InitialDealerList();


            var list = new List<CarInfoFormViewModel>();


            foreach (var tmp in avaiInventory)
            {
                var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days,
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                              StringSplitOptions.
                                                                  RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsFeatured = tmp.IsFeatured,
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                        DefaultImageUrl = tmp.DefaultImageUrl

                    };
                car.HealthLevel = LogicHelper.GetHealthLevel(tmp);

                list.Add(car);
            }

            list = list.Where(x => x.HealthLevel > 0 && x.HealthLevel < 3).ToList();


            //SET SORTING
            if (dealer.InventorySorting.Equals("Year"))
                viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
            else if (dealer.InventorySorting.Equals("Make"))
                viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
            else if (dealer.InventorySorting.Equals("Model"))
                viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
            else if (dealer.InventorySorting.Equals("Age"))
                viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
            else
                viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

            viewModel.previousCriteria = dealer.InventorySorting;

            viewModel.sortASCOrder = false;

            viewModel.CurrentOrSoldInventory = true;

            Session["InventoryObject"] = viewModel;

            return View("ViewSmallMissContentInventory", viewModel);
        }

        public ActionResult NoContentSmallInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            var context = new whitmanenterprisewarehouseEntities();

            var avaiInventory =
                                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                                where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                                select e;



            var viewModel = new InventoryFormViewModel { IsCompactView = false, IsMissingContent=false };

            if (Session["DealerGroup"] != null)
            {
                viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
            }
            else
                viewModel.DealerList = SelectListHelper.InitialDealerList();


            var list = new List<CarInfoFormViewModel>();


            foreach (var tmp in avaiInventory)
            {
                var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                              StringSplitOptions.
                                                                  RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsFeatured = tmp.IsFeatured,
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                        DefaultImageUrl = tmp.DefaultImageUrl
                    };
                list.Add(car);
            }


            list = list.Where(x => x.HealthLevel == 3).ToList();
            
            //SET SORTING
            if (dealer.InventorySorting.Equals("Year"))
                viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
            else if (dealer.InventorySorting.Equals("Make"))
                viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
            else if (dealer.InventorySorting.Equals("Model"))
                viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
            else if (dealer.InventorySorting.Equals("Age"))
                viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
            else
                viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

            viewModel.previousCriteria = dealer.InventorySorting;

            viewModel.sortASCOrder = false;

            Session["InventoryObject"] = viewModel;


            return View("ViewSmallMissContentInventory", viewModel);
        }

        public ActionResult MissContentNewSmallInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("new") && (e.Recon == null || !((bool)e.Recon))
                    select e;

                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsFeatured = tmp.IsFeatured,
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")

                    };
                    list.Add(car);
                }



                list = list.Where(x => x.HealthLevel > 0 && x.HealthLevel < 3).ToList();


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();

                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ViewSmallNewInventory", viewModel);
            }
        }
        public ActionResult NoContentNewSmallInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where  e.NewUsed.ToLower().Equals("new") && (e.Recon == null || !((bool)e.Recon))
                    select e;


                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        CarRanking = tmp.CarRanking.GetValueOrDefault(),
                        NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")
                    };
                    list.Add(car);
                }


                list = list.Where(x => x.HealthLevel == 3).ToList();


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ViewSmallNewInventory", viewModel);
            }
        }

        public ActionResult MissContentInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();


                var avaiInventory =
                                   from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                                   where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                                   select e;


                var viewModel = new InventoryFormViewModel { IsCompactView = false, IsMissingContent = true };

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.ToString().Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")

                    };
                    list.Add(car);
                }

                list = list.Where(x => x.HealthLevel > 0 && x.HealthLevel < 3).ToList();


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();

                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ViewInventory", viewModel);
            }
        }
        public ActionResult NoContentInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("used")
                    select e;

           


                var viewModel = new InventoryFormViewModel { IsCompactView = false,IsMissingContent = false};

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.ToString().Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        CarRanking = tmp.CarRanking.GetValueOrDefault(),
                        NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")
                    };
                    list.Add(car);
                }

            
                list = list.Where(x => x.HealthLevel == 3).ToList();


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
               
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ViewFullMissContentInventory", viewModel);
            }
        }
        public ActionResult MissContentNewInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("new")
                    select e;

          
                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                
                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.ToString().Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")

                    };
                    list.Add(car);
                }

             

                list = list.Where(x => x.HealthLevel > 0 && x.HealthLevel < 3).ToList();


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ViewInventory", viewModel);
            }
        }
        public ActionResult NoContentNewInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("new")
                    select e;

                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var hashtmp = new Hashtable();


                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Trim = tmp.Trim,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.ToString().Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        CarRanking = tmp.CarRanking.GetValueOrDefault(),
                        NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")
                    };
                    list.Add(car);
                }

                list = list.Where(x => x.HealthLevel == 3).ToList();


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();

                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ViewInventory", viewModel);
            }
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewInventory()
        {
            //if (!(bool)Session["Single"])
            ////dealer group case
            //{
            //    return RedirectToAction("ViewInventoryForAllStores");
            //}

            return HandleInventoryView(delegate(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context)
                                           {
                                               var avaiInventory =
                                                   from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                                                   where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                                                   select e;
                                               return GetInventory(avaiInventory, dealer, "ViewSmallInventory", false);
                                           });
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ACarInventory()
        {
            return HandleInventoryView(delegate(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context)
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where  e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon)) && (e.ACar != null && e.ACar.Value)
                    select e;
                return GetInventory(avaiInventory, dealer, "ViewSmallACarInventory", false);
            });
        }


        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ACarLargeInventory()
        {
            return HandleInventoryView(delegate(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context)
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon)) && (e.ACar != null && e.ACar.Value)
                    select e;
                return GetInventory(avaiInventory, dealer, "ViewFullACarInventory", false);
            });
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ACarNewInventory()
        {
            return HandleInventoryView(delegate(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context)
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("new") && (e.Recon == null || !((bool)e.Recon)) && (e.ACar != null && e.ACar.Value)
                    select e;
                return GetInventory(avaiInventory, dealer, "ViewSmallNewInventory", false);
            });
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewLoanerInventory()
        {
            return HandleInventoryView(delegate(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context)
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where  e.NewUsed.ToLower().Equals("used") && (e.Loaner == true)
                    select e;
                return GetInventory(avaiInventory, dealer, "ViewSmallLoanerInventory", false);
            });
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewAuctionInventory()
        {
            return HandleInventoryView(delegate(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context)
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where  e.NewUsed.ToLower().Equals("used") && (e.Auction == true)
                    select e;
                return GetInventory(avaiInventory, dealer, "ViewSmallAuctionInventory", false);
            });
        }

        public ActionResult ViewReconInventory()
        {
            return HandleInventoryView(delegate(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context)
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.Recon != null && (bool)e.Recon
                    select e;
                return GetInventory(avaiInventory, dealer, "ViewSmallReconInventory", true);
            });
        }

        public ActionResult ViewSearchInventory(string searchString, string searchBy)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                if (searchBy.Equals("Stock"))
                {
                    var sessionSingle = SessionHandler.Single;

                    //IQueryable<whitmanenterprisedealershipinventory> avaiInventory;

                    //if (sessionSingle)
                    //{
                    //    avaiInventory =
                    //        from e in context.whitmanenterprisedealershipinventories
                    //        where
                    //            e.DealershipId == dealer.DealershipId && e.StockNumber.Contains(searchString)
                    //        select e;
                    //}
                    //else
                    //{
                    //    var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];
                    //    var dealerList = from e in context.whitmanenterprisedealerships
                    //                     where e.DealerGroupID == dealerGroup.DealershipGroupId
                    //                     select e.idWhitmanenterpriseDealership;

                    //    var allInventory = context.whitmanenterprisedealershipinventories.Where(LogicHelper.BuildContainsExpression<whitmanenterprisedealershipinventory, int>(e => e.DealershipId.Value, dealerList));
                    //    avaiInventory = allInventory.Where(e => e.StockNumber.Contains(searchString));
                    //}



                    var viewModel = new InventoryFormViewModel { IsCompactView = false };

                    if (Session["DealerGroup"] != null)
                    {
                        viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                        viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                    }
                    else
                        viewModel.DealerList = SelectListHelper.InitialDealerList();

                    var list = new List<CarInfoFormViewModel>();

                    foreach (var tmp in InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(i => i.StockNumber.Contains(searchString)))
                    {
                        var car = new CarInfoFormViewModel()
                                      {
                                          ListingId = tmp.ListingID,
                                          ModelYear = tmp.ModelYear.Value,
                                          StockNumber = tmp.StockNumber,
                                          Model = tmp.Model,
                                          Make = tmp.Make,
                                          Mileage = tmp.Mileage,
                                          Trim = tmp.Trim,
                                          ChromeStyleId = tmp.ChromeStyleId,
                                          ChromeModelId = tmp.ChromeModelId,
                                          Vin = tmp.VINNumber,
                                          ExteriorColor =
                                              String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                                          InteriorColor =
                                              String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                                          HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                                          HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                                          HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                                          IsSold = false,
                                          CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                                          DateInStock = tmp.DateInStock.Value,
                                          DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                                          HealthLevel = LogicHelper.GetHealthLevel(tmp),
                                          SinglePhoto =
                                              String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                                  ? tmp.DefaultImageUrl
                                                  : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                                StringSplitOptions.
                                                                                    RemoveEmptyEntries).FirstOrDefault(),
                                          SalePrice = tmp.SalePrice,
                                          Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                                          MarketRange = tmp.MarketRange.GetValueOrDefault(),
                                          Reconstatus = tmp.Recon.GetValueOrDefault(),
                                          CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                                          IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                                          ACar = tmp.ACar.GetValueOrDefault()

                                      };
                        list.Add(car);
                    }


                    //SET SORTING
                    if (dealer.InventorySorting.Equals("Year"))
                        viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                    else if (dealer.InventorySorting.Equals("Make"))
                        viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                    else if (dealer.InventorySorting.Equals("Model"))
                        viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                    else if (dealer.InventorySorting.Equals("Age"))
                        viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                    else
                        viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                    viewModel.previousCriteria = dealer.InventorySorting;

                    viewModel.sortASCOrder = false;

                    viewModel.CurrentOrSoldInventory = true;

                    Session["InventoryObject"] = viewModel;

                    return View("ViewSmallInventory", viewModel);
                }
                else
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.NewUsed.ToLower().Equals("used") &&
                            e.VINNumber.Contains(searchString)
                        select e;


                    var viewModel = new InventoryFormViewModel();
                    viewModel.IsCompactView = false;

                    if (Session["DealerGroup"] != null)
                    {
                        viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                        viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                    }
                    else
                        viewModel.DealerList = SelectListHelper.InitialDealerList();

                    var list = new List<CarInfoFormViewModel>();

                    foreach (var tmp in avaiInventory)
                    {
                        var car = new CarInfoFormViewModel()
                        {
                            ListingId = tmp.ListingID,
                            ModelYear = tmp.ModelYear.Value,
                            StockNumber = tmp.StockNumber,
                            Model = tmp.Model,
                            Make = tmp.Make,
                            Mileage = tmp.Mileage,
                            Trim = tmp.Trim,
                            ChromeStyleId = tmp.ChromeStyleId,
                            ChromeModelId = tmp.ChromeModelId,
                            Vin = tmp.VINNumber,
                            ExteriorColor =
                                String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                            InteriorColor =
                                String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                            HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                            HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                            HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                            IsSold = false,
                            CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                            DateInStock = tmp.DateInStock.Value,
                            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                            HealthLevel = LogicHelper.GetHealthLevel(tmp),
                            SinglePhoto =
                                String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                    ? tmp.DefaultImageUrl
                                    : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                  StringSplitOptions.
                                                                      RemoveEmptyEntries).FirstOrDefault(),
                            SalePrice = tmp.SalePrice,
                            Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                            MarketRange = tmp.MarketRange.GetValueOrDefault(),
                            Reconstatus = tmp.Recon.GetValueOrDefault(),
                            CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                            IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")

                        };
                        list.Add(car);
                    }


                    //SET SORTING
                    if (dealer.InventorySorting.Equals("Year"))
                        viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                    else if (dealer.InventorySorting.Equals("Make"))
                        viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                    else if (dealer.InventorySorting.Equals("Model"))
                        viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                    else if (dealer.InventorySorting.Equals("Age"))
                        viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                    else
                        viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                    viewModel.previousCriteria = dealer.InventorySorting;

                    viewModel.sortASCOrder = false;

                    viewModel.CurrentOrSoldInventory = true;

                    Session["InventoryObject"] = viewModel;

                    return View("ViewSmallInventory", viewModel);
                }
            }
        }

        public ActionResult GetImages(int id, int status)
        {
            var result = GetImageResult(id, status);
            return Content(result);
        }

        public ActionResult GetImageLinks(int id)
        {
            var result = GetImageResultForGallery(id);
            ViewData["result"] = result;
            return View("ImageLink");
        }

        private string GetImageResult(int id, int status)
        {
            var result = String.Empty;

            using (var context = new whitmanenterprisewarehouseEntities())
            {

                if (status == 1)
                {
                    var content =
                        context.whitmanenterprisedealershipinventories.Where((i => i.ListingID == id))
                               .Select((i => i.ThumbnailImageURL))
                               .FirstOrDefault();

                    if (!String.IsNullOrEmpty(content))
                    {
                        var list = CommonHelper.GetArrayFromString(content);
                        var index = 1;
                        foreach (var tmp in list)
                        {
                            result += " <li class=\"selector\">" + Environment.NewLine;
                            result += " <div class=\"centerImage\">" + Environment.NewLine;
                            string fullSizeImage = tmp.Replace("ThumbnailSizeImages", "NormalSizeImages");

                            result += " <a id=\"" + index + "\" class=\"image\" rel=\"group1\" href=\"" + fullSizeImage +
                                      "\">" +
                                      Environment.NewLine;
                            //result += "<img id=\"" + index + "\" class=\"image\" src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                      Environment.NewLine;

                            result += "</a>" + Environment.NewLine;
                            result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                      "\" name=\"image" +
                                      index++ + "\" />" + Environment.NewLine;

                            result += "</div>" + Environment.NewLine;
                            result += "</li>" + Environment.NewLine;
                        }

                        if (list.Length < 75)
                        {
                            var loopNumber = 75 - list.Length;


                            for (int i = 0; i < loopNumber; i++)
                            {
                                var urlHelper = new UrlHelper(Request.RequestContext);
                                string tmp = "";

                                if (i % 3 == 0)
                                    tmp += urlHelper.Content("~/Images/40x40grey1.jpg");
                                else if (i % 3 == 1)
                                    tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                                else
                                    tmp += urlHelper.Content("~/Images/40x40grey3.jpg");

                                result += " <li class=\"selector\">" + Environment.NewLine;
                                result += " <div class=\"centerImage\">" + Environment.NewLine;

                                result += " <a class=\"image\" rel=\"group1\" href=\"" + tmp + "\">" +
                                          Environment.NewLine;
                                result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Default\" />" +
                                          Environment.NewLine;

                                result += "</a>" + Environment.NewLine;
                                result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                          "\" name=\"image" +
                                          index++ + "\" />" + Environment.NewLine;

                                result += "</div>";
                                result += "</li>" + Environment.NewLine;
                                ;

                            }
                        }
                    }
                    else
                    {
                        //int index = 1;

                        for (int i = 0; i < 75; i++)
                        {
                            var urlHelper = new UrlHelper(Request.RequestContext);
                            string tmp = "";

                            if (i % 3 == 0)

                                tmp += urlHelper.Content("~/Images/40x40grey1.jpg");


                            else if (i % 3 == 1)

                                tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                            else
                                tmp += urlHelper.Content("~/Images/40x40grey3.jpg");
                            result += " <li class=\"selector\">";
                            result += " <div class=\"centerImage\">";
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" />" + Environment.NewLine;
                            //result += " <input type=\"checkbox\" name=\"image" + index++ + "\" />";
                            result += "</div>";
                            result += "</li>" + Environment.NewLine;
                            ;
                        }
                    }
                }
                else if (status == -1)
                {
                    var content =
                       context.whitmanenterprisedealershipinventorysoldouts.Where((i => i.ListingID == id))
                              .Select((i => i.ThumbnailImageURL))
                              .FirstOrDefault();

                    if (!String.IsNullOrEmpty(content))
                    {
                        var list = CommonHelper.GetArrayFromString(content);
                        var index = 1;
                        foreach (var tmp in list)
                        {
                            result += " <li class=\"selector\">" + Environment.NewLine;
                            result += " <div class=\"centerImage\">" + Environment.NewLine;
                            string fullSizeImage = tmp.Replace("ThumbnailSizeImages", "NormalSizeImages");

                            result += " <a id=\"" + index + "\" class=\"image\" rel=\"group1\" href=\"" + fullSizeImage +
                                      "\">" +
                                      Environment.NewLine;
                            //result += "<img id=\"" + index + "\" class=\"image\" src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                      Environment.NewLine;

                            result += "</a>" + Environment.NewLine;
                            result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                      "\" name=\"image" +
                                      index++ + "\" />" + Environment.NewLine;

                            result += "</div>" + Environment.NewLine;
                            result += "</li>" + Environment.NewLine;
                        }

                        if (list.Length < 75)
                        {
                            var loopNumber = 75 - list.Length;


                            for (int i = 0; i < loopNumber; i++)
                            {
                                var urlHelper = new UrlHelper(Request.RequestContext);
                                string tmp = "";

                                if (i % 3 == 0)
                                    tmp += urlHelper.Content("~/Images/40x40grey1.jpg");
                                else if (i % 3 == 1)
                                    tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                                else
                                    tmp += urlHelper.Content("~/Images/40x40grey3.jpg");

                                result += " <li class=\"selector\">" + Environment.NewLine;
                                result += " <div class=\"centerImage\">" + Environment.NewLine;

                                result += " <a class=\"image\" rel=\"group1\" href=\"" + tmp + "\">" +
                                          Environment.NewLine;
                                result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Default\" />" +
                                          Environment.NewLine;

                                result += "</a>" + Environment.NewLine;
                                result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                          "\" name=\"image" +
                                          index++ + "\" />" + Environment.NewLine;

                                result += "</div>";
                                result += "</li>" + Environment.NewLine;
                                ;

                            }
                        }
                    }
                    else
                    {
                        //int index = 1;

                        for (int i = 0; i < 75; i++)
                        {
                            var urlHelper = new UrlHelper(Request.RequestContext);
                            string tmp = "";

                            if (i % 3 == 0)

                                tmp += urlHelper.Content("~/Images/40x40grey1.jpg");


                            else if (i % 3 == 1)

                                tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                            else
                                tmp += urlHelper.Content("~/Images/40x40grey3.jpg");
                            result += " <li class=\"selector\">";
                            result += " <div class=\"centerImage\">";
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" />" + Environment.NewLine;
                            //result += " <input type=\"checkbox\" name=\"image" + index++ + "\" />";
                            result += "</div>";
                            result += "</li>" + Environment.NewLine;
                            ;
                        }
                    }
                }
                else
                {
                    var content =
                       context.vincontrolwholesaleinventories.Where((i => i.ListingID == id))
                              .Select((i => i.ThumbnailImageURL))
                              .FirstOrDefault();

                    if (!String.IsNullOrEmpty(content))
                    {
                        var list = CommonHelper.GetArrayFromString(content);
                        var index = 1;
                        foreach (var tmp in list)
                        {
                            result += " <li class=\"selector\">" + Environment.NewLine;
                            result += " <div class=\"centerImage\">" + Environment.NewLine;
                            string fullSizeImage = tmp.Replace("ThumbnailSizeImages", "NormalSizeImages");

                            result += " <a id=\"" + index + "\" class=\"image\" rel=\"group1\" href=\"" + fullSizeImage +
                                      "\">" +
                                      Environment.NewLine;
                            //result += "<img id=\"" + index + "\" class=\"image\" src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                      Environment.NewLine;

                            result += "</a>" + Environment.NewLine;
                            result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                      "\" name=\"image" +
                                      index++ + "\" />" + Environment.NewLine;

                            result += "</div>" + Environment.NewLine;
                            result += "</li>" + Environment.NewLine;
                        }

                        if (list.Length < 75)
                        {
                            var loopNumber = 75 - list.Length;


                            for (int i = 0; i < loopNumber; i++)
                            {
                                var urlHelper = new UrlHelper(Request.RequestContext);
                                string tmp = "";

                                if (i % 3 == 0)
                                    tmp += urlHelper.Content("~/Images/40x40grey1.jpg");
                                else if (i % 3 == 1)
                                    tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                                else
                                    tmp += urlHelper.Content("~/Images/40x40grey3.jpg");

                                result += " <li class=\"selector\">" + Environment.NewLine;
                                result += " <div class=\"centerImage\">" + Environment.NewLine;

                                result += " <a class=\"image\" rel=\"group1\" href=\"" + tmp + "\">" +
                                          Environment.NewLine;
                                result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Default\" />" +
                                          Environment.NewLine;

                                result += "</a>" + Environment.NewLine;
                                result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                          "\" name=\"image" +
                                          index++ + "\" />" + Environment.NewLine;

                                result += "</div>";
                                result += "</li>" + Environment.NewLine;
                                ;

                            }
                        }
                    }
                    else
                    {
                        //int index = 1;

                        for (int i = 0; i < 75; i++)
                        {
                            var urlHelper = new UrlHelper(Request.RequestContext);
                            string tmp = "";

                            if (i % 3 == 0)

                                tmp += urlHelper.Content("~/Images/40x40grey1.jpg");


                            else if (i % 3 == 1)

                                tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                            else
                                tmp += urlHelper.Content("~/Images/40x40grey3.jpg");
                            result += " <li class=\"selector\">";
                            result += " <div class=\"centerImage\">";
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" />" + Environment.NewLine;
                            //result += " <input type=\"checkbox\" name=\"image" + index++ + "\" />";
                            result += "</div>";
                            result += "</li>" + Environment.NewLine;
                            ;
                        }
                    }
                }

            }
            return result;
        }

        private static string GetImageResultForGallery(int id)
        {
            var result = String.Empty;

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var content =
                    context.whitmanenterprisedealershipinventories.Where((i => i.ListingID == id))
                           .Select((i => i.ThumbnailImageURL))
                           .FirstOrDefault();

                if (!String.IsNullOrEmpty(content))
                {
                    var list = CommonHelper.GetArrayFromString(content);
                    var index = 1;
                    result = list.Aggregate(result, (current, tmp) => current + ("<img src=\"" + tmp.Replace("ThumbnailSizeImages", "NormalSizeImages") + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine));
                }
            }
            return result;
        }

        public ActionResult AdvancedSearch()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var viewModel = new AdvancedSearchViewModel();
            var tempList = new List<SelectListItem>();
            var dealer = (DealershipViewModel)Session["Dealership"];

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var avaiInventory = new List<whitmanenterprisedealershipinventory>();

                var newInventory = GetNewInventory(dealer);
                var usedInventory = GetUsedInventory(dealer);
                var soldInventory = GetSoldInventory(dealer);
                var wholesale = GetWholesaleInventory(dealer);
                var appraisedInventory = GetAppraisedInventory(dealer);
                Session["AdvanceSearch_NewInventory"] = newInventory;
                Session["AdvanceSearch_UsedInventory"] = usedInventory;
                Session["AdvanceSearch_SoldInventory"] = soldInventory;
                Session["AdvanceSearch_Wholesale"] = wholesale;
                Session["AdvanceSearch_AppraisedInventory"] = appraisedInventory;

                avaiInventory.AddRange(newInventory);
                avaiInventory.AddRange(usedInventory);
                avaiInventory.AddRange(soldInventory);
                avaiInventory.AddRange(wholesale);
                avaiInventory.AddRange(appraisedInventory);

             
                var year = avaiInventory.Where(i => i.ModelYear != null ).Select(i => i.ModelYear.GetValueOrDefault()).Distinct().OrderByDescending(i => i).ToList();
                viewModel.Years.AddRange(year.Select(i => new SelectListItem() { Selected = false, Text = i.ToString(), Value = i.ToString() }));

            }

            return View("AdvancedSearch", viewModel);
        }

        private List<whitmanenterprisedealershipinventory> GetNewInventory(DealershipViewModel dealershipViewModel)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var result = InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    .Where(e =>
                           (e.Recon == null || !((bool)e.Recon)) &&
                           e.NewUsed.Equals("New"))
                    .ToList();
                foreach (var item in result)
                {
                    item.StockNumber = item.StockNumber ?? string.Empty;
                    item.Trim = item.Trim ?? string.Empty;
                    item.VINNumber = item.VINNumber ?? string.Empty;
                    item.ExteriorColor = item.ExteriorColor ?? string.Empty;
                    item.InteriorColor = item.InteriorColor ?? string.Empty;
                    item.Tranmission = item.Tranmission ?? string.Empty;
                    item.Descriptions = item.Descriptions ?? string.Empty;
                    item.StandardOptions = item.StandardOptions ?? string.Empty;
                    item.InteriorSurface = item.InteriorSurface ?? string.Empty;
                    item.Doors = item.Doors ?? string.Empty;
                    item.Type = 0;
                }

                return result;
            }
        }

        private List<whitmanenterprisedealershipinventory> GetUsedInventory(DealershipViewModel dealershipViewModel)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var result = InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    .Where(e =>
                           (e.Recon == null || !((bool)e.Recon)) &&
                           e.NewUsed.Equals("Used"))
                    .ToList();
                foreach (var item in result)
                {
                    item.StockNumber = item.StockNumber ?? string.Empty;
                    item.Trim = item.Trim ?? string.Empty;
                    item.VINNumber = item.VINNumber ?? string.Empty;
                    item.ExteriorColor = item.ExteriorColor ?? string.Empty;
                    item.InteriorColor = item.InteriorColor ?? string.Empty;
                    item.Tranmission = item.Tranmission ?? string.Empty;
                    item.Descriptions = item.Descriptions ?? string.Empty;
                    item.StandardOptions = item.StandardOptions ?? string.Empty;
                    item.InteriorSurface = item.InteriorSurface ?? string.Empty;
                    item.Doors = item.Doors ?? string.Empty;
                    item.Type = 1;
                }

                return result;
            }
        }

        private List<whitmanenterprisedealershipinventory> GetSoldInventory(DealershipViewModel dealershipViewModel)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var soldoutInventory = InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context)
                    .Where(e =>(e.Recon == null || !((bool)e.Recon))).ToList();
                return soldoutInventory.Select(e => new whitmanenterprisedealershipinventory()
                                                        {
                                                            ListingID = e.ListingID,
                                                            ModelYear = e.ModelYear,
                                                            StockNumber = e.StockNumber ?? string.Empty,
                                                            Model = e.Model,
                                                            Make = e.Make,
                                                            Mileage = e.Mileage,
                                                            Trim = e.Trim ?? string.Empty,
                                                            VINNumber = e.VINNumber ?? string.Empty,
                                                            ExteriorColor = e.ExteriorColor ?? string.Empty,
                                                            InteriorColor = e.InteriorColor ?? string.Empty,
                                                            CarImageUrl = e.CarImageUrl,
                                                            SalePrice = e.SalePrice,
                                                            DateInStock = e.DateInStock,
                                                            DefaultImageUrl = e.DefaultImageUrl,
                                                            ThumbnailImageURL = e.ThumbnailImageURL,
                                                            MarketRange = 0, // TODO: Soldout table doesn't have reference to MarketRange
                                                            Recon = e.Recon.GetValueOrDefault(),
                                                            CarFaxOwner = e.CarFaxOwner.GetValueOrDefault(),
                                                            Tranmission = e.Tranmission ?? string.Empty,
                                                            Descriptions = e.Descriptions ?? string.Empty,
                                                            StandardOptions = e.StandardOptions ?? string.Empty,
                                                            InteriorSurface = e.InteriorSurface ?? string.Empty,
                                                            Doors = e.Doors ?? string.Empty,
                                                            Type = 4
                                                        }).ToList();
            }
        }

        private List<whitmanenterprisedealershipinventory> GetWholesaleInventory(DealershipViewModel dealershipViewModel)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var wholesaleInventory = InventoryQueryHelper.GetSingleOrGroupWholesaleInventory(context)
                    .Where(e =>(e.Recon == null || !((bool)e.Recon))).ToList();
                return wholesaleInventory.Select(e => new whitmanenterprisedealershipinventory()
                                                          {
                                                              ListingID = e.ListingID,
                                                              ModelYear = e.ModelYear,
                                                              StockNumber = e.StockNumber ?? string.Empty,
                                                              Model = e.Model,
                                                              Make = e.Make,
                                                              Mileage = e.Mileage,
                                                              Trim = e.Trim ?? string.Empty,
                                                              VINNumber = e.VINNumber ?? string.Empty,
                                                              ExteriorColor = e.ExteriorColor ?? string.Empty,
                                                              InteriorColor = e.InteriorColor ?? string.Empty,
                                                              CarImageUrl = e.CarImageUrl,
                                                              SalePrice = e.SalePrice,
                                                              DateInStock = e.DateInStock,
                                                              DefaultImageUrl = e.DefaultImageUrl,
                                                              ThumbnailImageURL = e.ThumbnailImageURL,
                                                              MarketRange = e.MarketRange.GetValueOrDefault(),
                                                              Recon = e.Recon.GetValueOrDefault(),
                                                              CarFaxOwner = e.CarFaxOwner.GetValueOrDefault(),
                                                              Tranmission = e.Tranmission ?? string.Empty,
                                                              Descriptions = e.Descriptions ?? string.Empty,
                                                              StandardOptions = e.StandardOptions ?? string.Empty,
                                                              InteriorSurface = e.InteriorSurface ?? string.Empty,
                                                              Doors = e.Doors ?? string.Empty,
                                                              Type = 2
                                                          }).ToList();
            }
        }

        private List<whitmanenterprisedealershipinventory> GetAppraisedInventory(DealershipViewModel dealershipViewModel)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var appraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
                    .Where(e =>(e.Status == null || e.Status != "Pending")).ToList();
                return appraisal.Select(e => new whitmanenterprisedealershipinventory()
                                                 {
                                                     ListingID = e.idAppraisal,
                                                     ModelYear = e.ModelYear,
                                                     StockNumber = e.StockNumber ?? string.Empty,
                                                     Model = e.Model,
                                                     Make = e.Make,
                                                     Mileage = e.Mileage,
                                                     Trim = e.Trim ?? string.Empty,
                                                     VINNumber = e.VINNumber ?? string.Empty,
                                                     ExteriorColor = e.ExteriorColor ?? string.Empty,
                                                     InteriorColor = e.InteriorColor ?? string.Empty,
                                                     CarImageUrl = e.CarImageUrl,
                                                     SalePrice = CommonHelper.RemoveSpecialCharactersForMsrp(e.SalePrice),
                                                     DateInStock = null,
                                                     DefaultImageUrl = e.DefaultImageUrl,
                                                     ThumbnailImageURL = string.Empty,// TODO
                                                     MarketRange = 0,// TODO
                                                     Recon = false,
                                                     CarFaxOwner = 0, // TODO
                                                     Tranmission = e.Tranmission ?? string.Empty,
                                                     Descriptions = e.Descriptions ?? string.Empty,
                                                     StandardOptions = e.StandardOptions ?? string.Empty,
                                                     InteriorSurface = e.InteriorSurface ?? string.Empty,
                                                     Doors = e.Doors ?? string.Empty,
                                                     Type = 3
                                                 }).ToList();
            }
        }

        private bool ReadyToSearch(AdvancedSearchViewModel model)
        {
            return (!string.IsNullOrEmpty(model.Text) ||
                    !string.IsNullOrEmpty(model.SelectedCategory) ||
                    !string.IsNullOrEmpty(model.SelectedYear) ||
                    !string.IsNullOrEmpty(model.SelectedMake) ||
                    !string.IsNullOrEmpty(model.SelectedModel));
        }

        [HttpPost]
        public ActionResult GenerateMakes(AdvancedSearchViewModel model)
        {
            model.Makes.Clear();
            if (string.IsNullOrEmpty(model.SelectedYear))
            {
                //model.Makes.Add(new SelectListItem() {Selected = true, Text = "----", Value = "" });
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                var avaiInventory = new List<whitmanenterprisedealershipinventory>();
                var newInventory = Session["AdvanceSearch_NewInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_NewInventory"] : GetNewInventory(dealer);
                var usedInventory = Session["AdvanceSearch_UsedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_UsedInventory"] : GetUsedInventory(dealer);
                var soldInventory = Session["AdvanceSearch_SoldInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_SoldInventory"] : GetSoldInventory(dealer);
                var wholesale = Session["AdvanceSearch_Wholesale"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_Wholesale"] : GetWholesaleInventory(dealer);
                var appraisedInventory = Session["AdvanceSearch_AppraisedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_AppraisedInventory"] : GetAppraisedInventory(dealer);

                avaiInventory.AddRange(newInventory);
                avaiInventory.AddRange(usedInventory);
                avaiInventory.AddRange(soldInventory);
                avaiInventory.AddRange(wholesale);
                avaiInventory.AddRange(appraisedInventory);
                var selectedYear = Convert.ToInt32(model.SelectedYear);
                var make = avaiInventory.Where(i => !String.IsNullOrEmpty(i.Make) && i.ModelYear.GetValueOrDefault() == selectedYear ).Select(i => i.Make).Distinct().OrderBy(i => i).ToList();
                model.Makes.AddRange(make.Select(i => new SelectListItem() { Selected = false, Text = i.ToString(), Value = i.ToString() }));
            }

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult GenerateModels(AdvancedSearchViewModel model)
        {
            model.Models.Clear();
            if (string.IsNullOrEmpty(model.SelectedMake))
            {
                //model.Models.Add(new SelectListItem() {Selected = true, Text = "----", Value = "" });
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                var avaiInventory = new List<whitmanenterprisedealershipinventory>();
                var newInventory = Session["AdvanceSearch_NewInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_NewInventory"] : GetNewInventory(dealer);
                var usedInventory = Session["AdvanceSearch_UsedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_UsedInventory"] : GetUsedInventory(dealer);
                var soldInventory = Session["AdvanceSearch_SoldInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_SoldInventory"] : GetSoldInventory(dealer);
                var wholesale = Session["AdvanceSearch_Wholesale"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_Wholesale"] : GetWholesaleInventory(dealer);
                var appraisedInventory = Session["AdvanceSearch_AppraisedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_AppraisedInventory"] : GetAppraisedInventory(dealer);

                avaiInventory.AddRange(newInventory);
                avaiInventory.AddRange(usedInventory);
                avaiInventory.AddRange(soldInventory);
                avaiInventory.AddRange(wholesale);
                avaiInventory.AddRange(appraisedInventory);
                var selectedYear = Convert.ToInt32(model.SelectedYear);
                if (avaiInventory.Any(i => !String.IsNullOrEmpty(i.Model) && i.Make.ToLower() == model.SelectedMake.ToLower() && i.ModelYear.GetValueOrDefault() == selectedYear ))
                {
                    var modelList = avaiInventory.Where(i => !String.IsNullOrEmpty(i.Model) && i.Make.ToLower() == model.SelectedMake.ToLower() && i.ModelYear.GetValueOrDefault() == selectedYear && i.DealershipId == dealer.DealershipId)
                                     .Select(i => i.Model)
                                     .Distinct()
                                     .OrderBy(i => i)
                                     .ToList();

                    model.Models.AddRange(modelList.Select(i => new SelectListItem() { Selected = false, Text = i.ToString(CultureInfo.InvariantCulture), Value = i.ToString(CultureInfo.InvariantCulture) }));

                }
            }

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AdvancedSearchResult(AdvancedSearchViewModel model)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];
            var avaiInventory = new List<whitmanenterprisedealershipinventory>();
            var listOfResults = new List<CarInfoFormViewModel>();
            TempData["HasMultipleCategory"] = 0;

            if (ReadyToSearch(model))
            {
                try
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        if (!string.IsNullOrEmpty(model.SelectedCategory))
                        {
                            switch (model.SelectedCategory)
                            {
                                case "Used":
                                    avaiInventory = Session["AdvanceSearch_UsedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_UsedInventory"] : GetUsedInventory(dealer);
                                    break;
                                case "New":
                                    avaiInventory = Session["AdvanceSearch_NewInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_NewInventory"] : GetNewInventory(dealer);
                                    break;
                                case "Sold":
                                    avaiInventory = Session["AdvanceSearch_SoldInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_SoldInventory"] : GetSoldInventory(dealer);
                                    break;
                                case "Wholesale":
                                    avaiInventory = Session["AdvanceSearch_Wholesale"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_Wholesale"] : GetWholesaleInventory(dealer);
                                    break;
                                case "Appraisals":
                                    avaiInventory = Session["AdvanceSearch_AppraisedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_AppraisedInventory"] : GetAppraisedInventory(dealer);
                                    break;
                                default: // Get All Inventories
                                    var newInventory = Session["AdvanceSearch_NewInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_NewInventory"] : GetNewInventory(dealer);
                                    var usedInventory = Session["AdvanceSearch_UsedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_UsedInventory"] : GetUsedInventory(dealer);
                                    var soldInventory = Session["AdvanceSearch_SoldInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_SoldInventory"] : GetSoldInventory(dealer);
                                    var wholesale = Session["AdvanceSearch_Wholesale"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_Wholesale"] : GetWholesaleInventory(dealer);
                                    var appraisedInventory = Session["AdvanceSearch_AppraisedInventory"] != null ? (List<whitmanenterprisedealershipinventory>)Session["AdvanceSearch_AppraisedInventory"] : GetAppraisedInventory(dealer);

                                    avaiInventory.AddRange(newInventory);
                                    avaiInventory.AddRange(usedInventory);
                                    avaiInventory.AddRange(soldInventory);
                                    avaiInventory.AddRange(wholesale);
                                    avaiInventory.AddRange(appraisedInventory);
                                    TempData["HasMultipleCategory"] = 1;
                                    break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("HttpError", "Error");
                }

                if (!string.IsNullOrEmpty(model.SelectedYear))
                    avaiInventory = avaiInventory.Where(tmp => tmp.ModelYear == int.Parse(model.SelectedYear)).ToList();

                if (!string.IsNullOrEmpty(model.SelectedMake))
                    avaiInventory = avaiInventory.Where(tmp => tmp.Make == model.SelectedMake).ToList();

                if (!string.IsNullOrEmpty(model.SelectedModel))
                    avaiInventory = avaiInventory.Where(tmp => tmp.Model == model.SelectedModel).ToList();

                if (!string.IsNullOrEmpty(model.Text))
                    avaiInventory = avaiInventory.Where(tmp => (!String.IsNullOrEmpty(tmp.StockNumber) && tmp.StockNumber.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.VINNumber) && tmp.VINNumber.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.Model) && tmp.Model.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.Make) && tmp.Make.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.Trim) && tmp.Trim.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.ExteriorColor) && tmp.ExteriorColor.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.InteriorColor) && tmp.InteriorColor.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.Tranmission) && tmp.Tranmission.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.Descriptions) && tmp.Descriptions.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.StandardOptions) && tmp.StandardOptions.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.InteriorSurface) && tmp.InteriorSurface.ToLower().Contains(model.Text.ToLower())) ||
                                                                    (!String.IsNullOrEmpty(tmp.Doors) && tmp.Doors.ToLower().Contains(model.Text.ToLower()))).ToList();

                listOfResults = avaiInventory.Select(tmp => new CarInfoFormViewModel()
                                                                {
                                                                    ListingId = tmp.ListingID,
                                                                    ModelYear = tmp.ModelYear ?? 0,
                                                                    StockNumber = tmp.StockNumber,
                                                                    Model = tmp.Model,
                                                                    Make = tmp.Make,
                                                                    Mileage = CommonHelper.FormatPurePrice(tmp.Mileage).ToString("0,0#"),
                                                                    Trim = tmp.Trim,
                                                                    ChromeStyleId = tmp.ChromeStyleId,
                                                                    ChromeModelId = tmp.ChromeModelId,
                                                                    Vin = tmp.VINNumber,
                                                                    ExteriorColor =
                                                                        String.IsNullOrEmpty(tmp.ExteriorColor)
                                                                            ? ""
                                                                            : tmp.ExteriorColor,
                                                                    InteriorColor =
                                                                        String.IsNullOrEmpty(tmp.InteriorColor)
                                                                            ? ""
                                                                            : tmp.InteriorColor,
                                                                    HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                                                                    HasDescription =
                                                                        !String.IsNullOrEmpty(tmp.Descriptions),
                                                                    HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                                                                    IsSold = false,
                                                                    CarName =
                                                                        tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                                                                    DateInStock = tmp.DateInStock ?? DateTime.Now,
                                                                    DaysInInvenotry =
                                                                        DateTime.Now.Subtract(tmp.DateInStock ??
                                                                                              DateTime.Now).Days,
                                                                    HealthLevel = LogicHelper.GetHealthLevel(tmp),
                                                                    SinglePhoto =
                                                                        String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                                                            ? tmp.DefaultImageUrl
                                                                            : tmp.ThumbnailImageURL.Split(
                                                                                new string[] { ",", "|" },
                                                                                StringSplitOptions.RemoveEmptyEntries).
                                                                                  FirstOrDefault(),
                                                                    SalePrice = CommonHelper.FormatPurePrice(tmp.SalePrice).ToString("0,0#"),
                                                                    Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                                                                    MarketRange = tmp.MarketRange.GetValueOrDefault(),
                                                                    Reconstatus = tmp.Recon.GetValueOrDefault(),
                                                                    CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                                                                    Type = tmp.Type
                                                                }).ToList();

                TempData["NumberOfNewInventory"] = listOfResults.Where(i => i.Type == 0).Count();
                TempData["NumberOfUsedInventory"] = listOfResults.Where(i => i.Type == 1).Count();
                TempData["NumberOfSoldInventory"] = listOfResults.Where(i => i.Type == 4).Count();
                TempData["NumberOfWholesaleInventory"] = listOfResults.Where(i => i.Type == 2).Count();
                TempData["NumberOfAppraisalInventory"] = listOfResults.Where(i => i.Type == 3).Count();

                //Sorting
                var isAsc = Session["AdvancedSearch_IsAsc"] != null ? (bool)Session["AdvancedSearch_IsAsc"] : false;
                switch (model.SortField)
                {
                    case "Year":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.ModelYear).ToList() : listOfResults.OrderByDescending(i => i.ModelYear).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Make":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.Make).ToList() : listOfResults.OrderByDescending(i => i.Make).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Model":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.Model).ToList() : listOfResults.OrderByDescending(i => i.Model).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Trim":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.Trim).ToList() : listOfResults.OrderByDescending(i => i.Trim).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Stock":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.StockNumber).ToList() : listOfResults.OrderByDescending(i => i.StockNumber).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Age":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.DaysInInvenotry).ToList() : listOfResults.OrderByDescending(i => i.DaysInInvenotry).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Mile":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.Mileage).ToList() : listOfResults.OrderByDescending(i => i.Mileage).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Price":
                        listOfResults = isAsc ? listOfResults.OrderBy(i => i.SalePrice).ToList() : listOfResults.OrderByDescending(i => i.SalePrice).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    default:
                        Session["AdvancedSearch_IsAsc"] = true;
                        break;
                }
            }

            return PartialView(listOfResults);
        }

        public ActionResult ViewSoldInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();


                var soldInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context)
                    where e.NewUsed.ToLower().Equals("used")
                    select e;

                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in soldInventory)
                {
                    int daysInInventory = DateTime.Now.Subtract(tmp.DateRemoved.GetValueOrDefault()).Days;

                    if (daysInInventory <= 30)
                    {
                        var car = new CarInfoFormViewModel()
                                      {
                                          ListingId = tmp.ListingID,
                                          ModelYear = tmp.ModelYear.GetValueOrDefault(),
                                          StockNumber = tmp.StockNumber,
                                          Model = tmp.Model,
                                          Make = tmp.Make,
                                          Mileage = tmp.Mileage,
                                          Trim = tmp.Trim,
                                          ChromeStyleId = tmp.ChromeStyleId,
                                          Vin = tmp.VINNumber,
                                          ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                                          InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                                          HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                                          HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                                          HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                                          IsSold = true,
                                          CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                                          DateInStock = tmp.DateInStock.GetValueOrDefault(),
                                          DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                                          HealthLevel = LogicHelper.GetHealthLevelSoldOut(tmp),
                                          SinglePhoto =
                                              String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                                  ? tmp.DefaultImageUrl
                                                  : tmp.ThumbnailImageURL.ToString(CultureInfo.InvariantCulture).Split(new string[] { ",", "|" },
                                                                                           StringSplitOptions.
                                                                                               RemoveEmptyEntries).FirstOrDefault(),
                                          SalePrice = tmp.SalePrice,
                                          Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                                          SoldOutDaysLeft =
                                              CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory) < 0
                                                  ? 0
                                                  : CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory),
                                          Reconstatus = tmp.Recon.GetValueOrDefault(),
                                          CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                                          IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                                          IsFeatured = tmp.IsFeatured,

                                      };

                        list.Add(car);
                    }
                }


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();

                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = false;

                Session["InventoryObject"] = viewModel;

                return View("ViewSmallInventory", viewModel);
            }
        }

        public ActionResult ExpressBucketJump()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                    select e;


                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list = new List<CarInfoFormViewModel>();

                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.GetValueOrDefault(),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        DealerCost = tmp.DealerCost,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsFeatured = tmp.IsFeatured,
                        CarRanking = tmp.CarRanking.GetValueOrDefault(),
                        NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                        MarketLowestPrice = tmp.MarketLowestPrice.GetValueOrDefault().ToString("c0"),
                        MarketAveragePrice = tmp.MarketAveragePrice.GetValueOrDefault().ToString("c0"),
                        MarketHighestPrice = tmp.MarketHighestPrice.GetValueOrDefault().ToString("c0"),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")

                    };
                    list.Add(car);
                }

                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ExpressBucketJump", viewModel);
            }
        }

        public ActionResult TodayBucketJump()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {


                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                    select e;


                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list = new List<CarInfoFormViewModel>();

                foreach (var tmp in avaiInventory)
                {
                    var dateatMidnight = new DateTime(tmp.DateInStock.GetValueOrDefault().Year,
                                                      tmp.DateInStock.GetValueOrDefault().Month,
                                                      tmp.DateInStock.GetValueOrDefault().Day);

                    int daysInInventory = DateTime.Now.Subtract(dateatMidnight).Days;
                    int bucketDay = tmp.BucketJumpCompleteDay.GetValueOrDefault();
                    int nextBucketDay = 0;
                    if (bucketDay == 0 || bucketDay < dealer.FirstIntervalJump)
                        nextBucketDay = dealer.FirstIntervalJump;
                    else if (bucketDay < dealer.SecondIntervalJump)
                        nextBucketDay = dealer.SecondIntervalJump;
                    else if (bucketDay >= dealer.SecondIntervalJump)
                        nextBucketDay = bucketDay + dealer.IntervalBucketJump;

                    bool flag = /*bucketDay == 0 || */nextBucketDay <= daysInInventory;

                    if (flag)
                    {
                        var car = new CarInfoFormViewModel()
                            {
                                ListingId = tmp.ListingID,
                                ModelYear = tmp.ModelYear.GetValueOrDefault(),
                                StockNumber = tmp.StockNumber,
                                Model = tmp.Model,
                                Make = tmp.Make,
                                Mileage = tmp.Mileage,
                                Trim = tmp.Trim,
                                ChromeStyleId = tmp.ChromeStyleId,
                                ChromeModelId = tmp.ChromeModelId,
                                Vin = tmp.VINNumber,
                                ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                                InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                                HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                                HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                                HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                                IsSold = false,
                                CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                                DateInStock = tmp.DateInStock.GetValueOrDefault(),
                                DaysInInvenotry = daysInInventory,
                                HealthLevel = LogicHelper.GetHealthLevel(tmp),
                                SinglePhoto =
                                    String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                        ? tmp.DefaultImageUrl
                                        : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                      StringSplitOptions.
                                                                          RemoveEmptyEntries).FirstOrDefault(),
                                SalePrice = tmp.SalePrice,
                                DealerCost = tmp.DealerCost,
                                Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                                MarketRange = tmp.MarketRange.GetValueOrDefault(),
                                Reconstatus = tmp.Recon.GetValueOrDefault(),
                                CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                                IsFeatured = tmp.IsFeatured,
                                CarRanking = tmp.CarRanking.GetValueOrDefault(),
                                NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                                MarketLowestPrice = tmp.MarketLowestPrice.GetValueOrDefault().ToString("c0"),
                                MarketAveragePrice = tmp.MarketAveragePrice.GetValueOrDefault().ToString("c0"),
                                MarketHighestPrice = tmp.MarketHighestPrice.GetValueOrDefault().ToString("c0"),
                                IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                                NotDoneBucketJump = true

                            };
                        list.Add(car);
                    }

                }

                viewModel.CarsList = list.OrderByDescending(x => x.DaysInInvenotry).ToList();

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;

                return View("TodayBucketJump", viewModel);
            }
        }

        public ActionResult ViewFullInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where  e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                    select e;


                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list = new List<CarInfoFormViewModel>();

                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        StockNumber = tmp.StockNumber,
                        Model = tmp.Model,
                        Make = tmp.Make,
                        Mileage = tmp.Mileage,
                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                        IsSold = false,
                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                        DateInStock = tmp.DateInStock.Value,
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        HealthLevel = LogicHelper.GetHealthLevel(tmp),
                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),
                        SalePrice = tmp.SalePrice,
                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                        MarketRange = tmp.MarketRange.GetValueOrDefault(),
                        Reconstatus = tmp.Recon.GetValueOrDefault(),
                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                        IsFeatured = tmp.IsFeatured,
                        CarRanking = tmp.CarRanking.GetValueOrDefault(),
                        NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                        Loaner = tmp.Loaner.GetValueOrDefault(),
                        Auction = tmp.Auction.GetValueOrDefault(),
                        ACar = tmp.ACar.GetValueOrDefault()
                    };
                    list.Add(car);
                }

                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                Session["InventoryObject"] = viewModel;


                return View("ViewInventory", viewModel);
            }
        }



      
        public ActionResult ViewFullSoldInventory()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();


                var soldInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context)
                    where e.NewUsed.ToLower().Equals("used")
                    select e;


                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in soldInventory)
                {
                    int daysInInventory = DateTime.Now.Subtract(tmp.DateRemoved.Value).Days;

                    if (daysInInventory <= 30)
                    {
                        var car = new CarInfoFormViewModel()
                        {
                            ListingId = tmp.ListingID,
                            ModelYear = tmp.ModelYear.GetValueOrDefault(),
                            StockNumber = tmp.StockNumber,
                            Model = tmp.Model,
                            Make = tmp.Make,
                            Mileage = tmp.Mileage,
                            Trim = tmp.Trim,
                            ChromeStyleId = tmp.ChromeStyleId,
                            Vin = tmp.VINNumber,
                            ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                            InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                            HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                            HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                            HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                            IsSold = true,
                            CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                            DateInStock = tmp.DateInStock.Value,
                            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                            HealthLevel = LogicHelper.GetHealthLevelSoldOut(tmp),
                            SinglePhoto =
                                String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                    ? tmp.DefaultImageUrl
                                    : tmp.ThumbnailImageURL.ToString(CultureInfo.InvariantCulture).Split(new string[] { ",", "|" },
                                                                             StringSplitOptions.
                                                                                 RemoveEmptyEntries).FirstOrDefault(),
                            SalePrice = tmp.SalePrice,
                            Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                            SoldOutDaysLeft =
                                CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory) < 0
                                    ? 0
                                    : CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory),
                            Reconstatus = tmp.Recon.GetValueOrDefault(),
                            CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                            CarRanking = tmp.CarRanking.GetValueOrDefault(),
                            NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                            IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                            IsFeatured = tmp.IsFeatured,
                        };

                        list.Add(car);
                    }
                }


                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();

                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = false;

                Session["InventoryObject"] = viewModel;


                return View("ViewInventory", viewModel);
            }
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewNewInventory()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];



                var context = new whitmanenterprisewarehouseEntities();

                var avaiInventory = from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                                    where  e.NewUsed.ToLower().Equals("new") && (e.Recon == null || !((bool)e.Recon))
                                    select e;

                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);

                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {

                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,

                        ModelYear = tmp.ModelYear.GetValueOrDefault(),

                        StockNumber = tmp.StockNumber,

                        Model = tmp.Model,

                        Make = tmp.Make,

                        Mileage = tmp.Mileage,

                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,

                        Vin = tmp.VINNumber,

                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,

                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),

                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),

                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),

                        IsSold = false,

                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,

                        DateInStock = tmp.DateInStock.GetValueOrDefault(),

                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,

                        HealthLevel = LogicHelper.GetHealthLevel(tmp),

                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),

                        SalePrice = tmp.SalePrice,

                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),

                        Reconstatus = tmp.Recon.GetValueOrDefault(),

                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),

                        IsFeatured = tmp.IsFeatured,
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                        ACar = tmp.ACar.GetValueOrDefault()


                    };
                    list.Add(car);



                }



                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;


                Session["InventoryObject"] = viewModel;

                return View("ViewSmallNewInventory", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewNewSoldInventory()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var soldInventory = from e in InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context)
                                    where  e.NewUsed.ToLower().Equals("new")
                                    select e;


                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);

                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();

                foreach (var tmp in soldInventory)
                {

                    int daysInInventory = DateTime.Now.Subtract(tmp.DateRemoved.Value).Days;

                    if (daysInInventory <= 30)
                    {
                        var car = new CarInfoFormViewModel()
                        {
                            ListingId = tmp.ListingID,

                            ModelYear = tmp.ModelYear.GetValueOrDefault(),

                            StockNumber = tmp.StockNumber,

                            Model = tmp.Model,

                            Make = tmp.Make,

                            Mileage = tmp.Mileage,

                            Trim = tmp.Trim,
                            ChromeStyleId = tmp.ChromeStyleId,

                            Vin = tmp.VINNumber,

                            ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                            InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,

                            HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),

                            HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),

                            HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),

                            IsSold = true,

                            CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,

                            DateInStock = tmp.DateInStock.GetValueOrDefault(),

                            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,

                            HealthLevel = LogicHelper.GetHealthLevelSoldOut(tmp),

                            SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailImageURL) ? tmp.DefaultImageUrl : tmp.ThumbnailImageURL.ToString().Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(),

                            SalePrice = tmp.SalePrice,

                            Price = CommonHelper.FormatPurePrice(tmp.SalePrice),


                            SoldOutDaysLeft = CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory) < 0 ? 0 : CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory),

                            CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                            IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")

                        };

                        list.Add(car);


                    }
                }



                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = false;

                Session["InventoryObject"] = viewModel;

                return View("ViewSmallNewInventory", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewFullNewInventory()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];



                var context = new whitmanenterprisewarehouseEntities();

                var avaiInventory = from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                                    where e.NewUsed.ToLower().Equals("new")
                                    select e;

                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);

                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {

                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,

                        ModelYear = tmp.ModelYear.GetValueOrDefault(),

                        StockNumber = tmp.StockNumber,

                        Model = tmp.Model,

                        Make = tmp.Make,

                        Mileage = tmp.Mileage,

                        Trim = tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,

                        Vin = tmp.VINNumber,

                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,

                        HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),

                        HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),

                        HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),

                        IsSold = false,

                        CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,

                        DateInStock = tmp.DateInStock.Value,

                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,

                        HealthLevel = LogicHelper.GetHealthLevel(tmp),

                        SinglePhoto =
                            String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                                ? tmp.DefaultImageUrl
                                : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                         StringSplitOptions.
                                                                             RemoveEmptyEntries).FirstOrDefault(),

                        SalePrice = tmp.SalePrice,

                        Price = CommonHelper.FormatPurePrice(tmp.SalePrice),

                        Reconstatus = tmp.Recon.GetValueOrDefault(),

                        CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),

                        IsFeatured = tmp.IsFeatured,

                        CarRanking = tmp.CarRanking.GetValueOrDefault(),

                        NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                        IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                        ACar = tmp.ACar.GetValueOrDefault()
                    };
                    list.Add(car);



                }



                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;


                Session["InventoryObject"] = viewModel;

                return View("ViewNewInventory", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewFullNewSoldInventory()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var soldInventory = from e in InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context)
                                    where  e.NewUsed.ToLower().Equals("new")
                                    select e;


                var viewModel = new InventoryFormViewModel();
                viewModel.IsCompactView = false;

                if (Session["DealerGroup"] != null)
                {
                    viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);

                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();


                var list = new List<CarInfoFormViewModel>();

                foreach (var tmp in soldInventory)
                {

                    int daysInInventory = DateTime.Now.Subtract(tmp.DateRemoved.Value).Days;

                    if (daysInInventory <= 30)
                    {
                        var car = new CarInfoFormViewModel()
                        {
                            ListingId = tmp.ListingID,

                            ModelYear = tmp.ModelYear.GetValueOrDefault(),

                            StockNumber = tmp.StockNumber,

                            Model = tmp.Model,

                            Make = tmp.Make,

                            Mileage = tmp.Mileage,

                            Trim = tmp.Trim,
                            ChromeStyleId = tmp.ChromeStyleId,

                            Vin = tmp.VINNumber,

                            ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                            InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,

                            HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),

                            HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),

                            HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),

                            IsSold = true,

                            CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,

                            DateInStock = tmp.DateInStock.Value,

                            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,

                            HealthLevel = LogicHelper.GetHealthLevelSoldOut(tmp),

                            SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailImageURL) ? tmp.DefaultImageUrl : tmp.ThumbnailImageURL.ToString().Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(),

                            SalePrice = tmp.SalePrice,

                            Price = CommonHelper.FormatPurePrice(tmp.SalePrice),


                            SoldOutDaysLeft = CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory) < 0 ? 0 : CommonHelper.CountDaysLeft(dealer.SoldOut, daysInInventory),

                            CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),

                            CarRanking = tmp.CarRanking.GetValueOrDefault(),

                            NumberOfCar = tmp.NumberOfCar.GetValueOrDefault(),
                            IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car")
                        };

                        list.Add(car);


                    }
                }



                //SET SORTING
                if (dealer.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = false;

                Session["InventoryObject"] = viewModel;

                return View("ViewNewInventory", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }


        private void ResetSessionValue()
        {
            Session["AutoTrader"] = null;
            Session["CarsCom"] = null;
            Session["AutoTraderNationwide"] = null;
            Session["CarsComNationwide"] = null;
            SessionHandler.ChromeTrimList = null;
        }

        public ActionResult ViewIProfile(int ListingID)
        {
            ResetSessionValue();
            if (Session["Dealership"] != null)
            {
                var context = new whitmanenterprisewarehouseEntities();

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingID);
                // if the car doesn't exist in the inventory, then check it in wholesale
                if (row == null)
                    return RedirectToAction("ViewIWholesaleProfile", new { ListingID });

                var dealer = (DealershipViewModel)Session["Dealership"];

                SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

                var sortField = (from c in context.whitmanenterprisesettings
                                 where c.idwhitmanenterprisesetting == dealer.DealershipId
                                 select c.InventorySorting).FirstOrDefault();
                if (sortField == null)
                {
                    sortField = "Age";
                }
                int doorNumber = 0;
                Int32.TryParse(row.Doors, out doorNumber);

                var viewModel = new CarInfoFormViewModel
                    {
                        SavedSelections = new ChartSelection(),
                        Vin = String.IsNullOrEmpty(row.VINNumber) ? "" : row.VINNumber,
                        Make = String.IsNullOrEmpty(row.Make) ? "" : row.Make,
                        ModelYear =
                            String.IsNullOrEmpty(row.ModelYear.GetValueOrDefault()
                                                    .ToString(CultureInfo.InvariantCulture))
                                ? 0
                                : row.ModelYear.GetValueOrDefault(),
                        Model = String.IsNullOrEmpty(row.Model) ? "" : row.Model,
                        Litters = String.IsNullOrEmpty(row.Liters) ? "" : row.Liters,
                        MSRP = String.IsNullOrEmpty(row.MSRP) ? "" : row.MSRP,
                        Fuel = String.IsNullOrEmpty(row.FuelType) ? "" : row.FuelType,
                        Engine = String.IsNullOrEmpty(row.EngineType) ? "" : row.EngineType,
                        Trim = String.IsNullOrEmpty(row.Trim) ? "" : row.Trim,
                        ChromeStyleId = row.ChromeStyleId,
                        ChromeModelId = row.ChromeModelId,
                        Cylinder = String.IsNullOrEmpty(row.Cylinders) ? "" : row.Cylinders,
                        Tranmission = String.IsNullOrEmpty(row.Tranmission) ? "" : row.Tranmission,
                        IsCertified = row.Certified.GetValueOrDefault(),
                        PriorRental = row.PriorRental.GetValueOrDefault(),
                        WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? "" : row.DriveTrain,
                        StockNumber = String.IsNullOrEmpty(row.StockNumber) ? "" : row.StockNumber,
                        ExteriorColor = String.IsNullOrEmpty(row.ExteriorColor) ? "" : row.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(row.InteriorColor) ? "" : row.InteriorColor,
                        Mileage = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage,
                        PreviousListingId = GetPreviousProfile(row, sortField),
                        NextListingId = GetNextProfile(row, sortField),
                        Door = String.IsNullOrEmpty(row.Doors) ? 0 : doorNumber,
                        BodyType = String.IsNullOrEmpty(row.BodyType) ? "" : row.BodyType,
                        DefaultImageUrl = String.IsNullOrEmpty(row.DefaultImageUrl) ? "" : row.DefaultImageUrl,
                        Description = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions,
                        ButtonPermissions = SQLHelper.GetButtonList(dealer.DealershipId, "Profile")
                    };

                viewModel.WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? viewModel.WheelDrive : row.DriveTrain;

                viewModel.ListingId = row.ListingID;

                viewModel.DealershipId = dealer.DealershipId;

                viewModel.UploadPhotosURL = String.IsNullOrEmpty(row.ThumbnailImageURL) ? null : (from data in row.ThumbnailImageURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                viewModel.InventoryStatus = 1;

                if (String.IsNullOrEmpty(row.SalePrice))
                    viewModel.SalePrice = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.SalePrice, out priceFormat);
                    if (flag)
                        viewModel.SalePrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerCost))
                    viewModel.DealerCost = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerCost, out priceFormat);
                    if (flag)
                        viewModel.DealerCost = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ACV))
                    viewModel.ACV = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ACV, out priceFormat);
                    if (flag)
                        viewModel.ACV = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.RetailPrice))
                    viewModel.RetailPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.RetailPrice, out priceFormat);
                    if (flag)
                        viewModel.RetailPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerDiscount))
                    viewModel.DealerDiscount = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                    if (flag)
                        viewModel.DealerDiscount = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.WindowStickerPrice))
                    viewModel.WindowStickerPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                    if (flag)
                        viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ManufacturerRebate))
                    viewModel.ManufacturerRebate = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                    if (flag)
                        viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
                }





                if (String.IsNullOrEmpty(row.CarImageUrl))
                    viewModel.SinglePhoto = row.DefaultImageUrl;
                else
                {


                    viewModel.SinglePhoto =
                                              String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                  ? row.DefaultImageUrl
                                                  : row.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                                           StringSplitOptions.
                                                                                               RemoveEmptyEntries).FirstOrDefault();

                }


                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;



                if (row.NewUsed == "Used")
                {
                    try
                    {
                        viewModel.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(viewModel.Vin, dealer.CarFax, dealer.CarFaxPassword);
                    }
                    catch (Exception)
                    {

                    }

                    //if (row.KBBTrimId == null)

                    //    viewModel.KBB = KellyBlueBookHelper.GetFullReport(viewModel.Vin, dealer.ZipCode, viewModel.Mileage);
                    //else
                    //{
                    //    viewModel.KBB = KellyBlueBookHelper.GetFullReport(viewModel.Vin, dealer.ZipCode, viewModel.Mileage, row.KBBTrimId.Value, row.KBBOptionsId);
                    //}
                }
                else
                {
                    var carfax = new CarFaxViewModel {ReportList = new List<CarFaxWindowSticker>(), Success = false};

                    var dealerPrice = new KellyBlueBookViewModel()
                    {
                        Vin = viewModel.Vin,

                    };

                    viewModel.CarFax = carfax;

                    viewModel.KBB = dealerPrice;
                }



                viewModel.BB = BlackBookService.GetFullReport(viewModel.Vin, viewModel.Mileage, dealer.State);

                var existingChartSelection = context.vincontrolchartselections.Where(s => s.listingId == ListingID && s.screen == "Inventory" && s.sourceType == "AutoTrader").FirstOrDefault();
                if (existingChartSelection != null)
                {
                    viewModel.SavedSelections.IsAll = existingChartSelection.isAll != null ? Convert.ToBoolean(existingChartSelection.isAll) : false;
                    viewModel.SavedSelections.IsCarsCom = existingChartSelection.isCarsCom != null ? Convert.ToBoolean(existingChartSelection.isCarsCom) : false;
                    viewModel.SavedSelections.IsCertified = existingChartSelection.isCertified != null ? Convert.ToBoolean(existingChartSelection.isCertified) : false;
                    viewModel.SavedSelections.IsFranchise = existingChartSelection.isFranchise != null ? Convert.ToBoolean(existingChartSelection.isFranchise) : false;
                    viewModel.SavedSelections.IsIndependant = existingChartSelection.isIndependant != null ? Convert.ToBoolean(existingChartSelection.isIndependant) : false;
                    viewModel.SavedSelections.Options = existingChartSelection.options.ToLower() ?? "";
                    viewModel.SavedSelections.Trims = existingChartSelection.trims.ToLower() ?? "";
                }

                if (Session["DealerGroup"] != null)
                    viewModel.MultipleDealers = true;
                else
                    viewModel.MultipleDealers = false;

                // include Manheim Wholesales values
                //try
                //{
                //    var manheimCredential = LINQHelper.GetManheimCredential(dealer.DealershipId);
                //    if (manheimCredential != null)
                //        viewModel.ManheimWholesales = LINQHelper.ManheimReport(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim());
                //    else
                //        viewModel.ManheimWholesales = new List<ManheimWholesaleViewModel>();
                //}
                //catch (Exception)
                //{
                //    viewModel.ManheimWholesales = new List<ManheimWholesaleViewModel>();
                //}

                // include KarPower values
                //try
                //{
                //    viewModel.KarPowerData = new List<SmallKarPowerViewModel>();
                //    var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealer.DealershipId);
                //    if (setting != null)
                //    {
                //        var karpowerService = new KarPowerService();
                //        viewModel.KarPowerData = karpowerService.Execute(row.VINNumber, row.Mileage, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword);
                //    }
                //}
                //catch (Exception)
                //{

                //}

                viewModel.DateInStock = row.DateInStock;
                viewModel.DaysInInvenotry = DateTime.Now.Subtract(row.DateInStock.Value).Days;

                if (!String.IsNullOrEmpty(row.VehicleType) && row.VehicleType.Equals("Truck"))
                {
                    viewModel.SelectedTruckType = row.TruckType;

                    viewModel.SelectedTruckClass = row.TruckClass;

                    viewModel.SelectedTruckCategory = row.TruckCategory;


                    return View("iTruckProfile", viewModel);
                }
                ViewData["CarFaxResponse"] = viewModel.CarFax.CarFaxXMLResponse;
                viewModel.CarFaxDealerId = dealer.CarFax;

                return View("iProfile", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult ViewIProfileOnMobile(string token, int listingId)
        {
            ResetSessionValue();
            SessionHandler.Single = true;
            Session["IsEmployee"] = false;
            SessionHandler.CurrentUser = new UserRoleViewModel() { Role = "Admin" };
                int dealerId = 2299;
                try
                {
                    dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(token.Replace(" ", "+")).Split('|')[0]);
                }
                catch (Exception) { }

                SessionHandler.DealerId = dealerId;

                var context = new whitmanenterprisewarehouseEntities();
                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == listingId);
                // if the car doesn't exist in the inventory, then check it in wholesale
                if (row == null)
                    return RedirectToAction("ViewIWholesaleProfile", new { ListingID = listingId });

                SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealerId);

                var sortField = (from c in context.whitmanenterprisesettings
                                 where c.idwhitmanenterprisesetting == dealerId
                                 select c.InventorySorting).FirstOrDefault();
                if (sortField == null)
                {
                    sortField = "Age";
                }
                int doorNumber = 0;
                Int32.TryParse(row.Doors, out doorNumber);

                var viewModel = new CarInfoFormViewModel
                {
                    SavedSelections = new ChartSelection(),
                    Vin = String.IsNullOrEmpty(row.VINNumber) ? "" : row.VINNumber,
                    Make = String.IsNullOrEmpty(row.Make) ? "" : row.Make,
                    ModelYear =
                        String.IsNullOrEmpty(row.ModelYear.GetValueOrDefault()
                                                .ToString(CultureInfo.InvariantCulture))
                            ? 0
                            : row.ModelYear.GetValueOrDefault(),
                    Model = String.IsNullOrEmpty(row.Model) ? "" : row.Model,
                    Litters = String.IsNullOrEmpty(row.Liters) ? "" : row.Liters,
                    MSRP = String.IsNullOrEmpty(row.MSRP) ? "" : row.MSRP,
                    Fuel = String.IsNullOrEmpty(row.FuelType) ? "" : row.FuelType,
                    Engine = String.IsNullOrEmpty(row.EngineType) ? "" : row.EngineType,
                    Trim = String.IsNullOrEmpty(row.Trim) ? "" : row.Trim,
                    ChromeStyleId = row.ChromeStyleId,
                    ChromeModelId = row.ChromeModelId,
                    Cylinder = String.IsNullOrEmpty(row.Cylinders) ? "" : row.Cylinders,
                    Tranmission = String.IsNullOrEmpty(row.Tranmission) ? "" : row.Tranmission,
                    IsCertified = row.Certified.GetValueOrDefault(),
                    PriorRental = row.PriorRental.GetValueOrDefault(),
                    WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? "" : row.DriveTrain,
                    StockNumber = String.IsNullOrEmpty(row.StockNumber) ? "" : row.StockNumber,
                    ExteriorColor = String.IsNullOrEmpty(row.ExteriorColor) ? "" : row.ExteriorColor,
                    InteriorColor = String.IsNullOrEmpty(row.InteriorColor) ? "" : row.InteriorColor,
                    Mileage = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage,
                    PreviousListingId = GetPreviousProfile(row, sortField),
                    NextListingId = GetNextProfile(row, sortField),
                    Door = String.IsNullOrEmpty(row.Doors) ? 0 : doorNumber,
                    BodyType = String.IsNullOrEmpty(row.BodyType) ? "" : row.BodyType,
                    DefaultImageUrl = String.IsNullOrEmpty(row.DefaultImageUrl) ? "" : row.DefaultImageUrl,
                    Description = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions,
                    ButtonPermissions = SQLHelper.GetButtonList(dealerId, "Profile")
                };

                viewModel.WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? viewModel.WheelDrive : row.DriveTrain;

                viewModel.ListingId = row.ListingID;

                viewModel.DealershipId = dealerId;

                viewModel.UploadPhotosURL = String.IsNullOrEmpty(row.ThumbnailImageURL) ? null : (from data in row.ThumbnailImageURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                viewModel.InventoryStatus = 1;

                if (String.IsNullOrEmpty(row.SalePrice))
                    viewModel.SalePrice = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.SalePrice, out priceFormat);
                    if (flag)
                        viewModel.SalePrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerCost))
                    viewModel.DealerCost = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerCost, out priceFormat);
                    if (flag)
                        viewModel.DealerCost = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ACV))
                    viewModel.ACV = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ACV, out priceFormat);
                    if (flag)
                        viewModel.ACV = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.RetailPrice))
                    viewModel.RetailPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.RetailPrice, out priceFormat);
                    if (flag)
                        viewModel.RetailPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerDiscount))
                    viewModel.DealerDiscount = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                    if (flag)
                        viewModel.DealerDiscount = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.WindowStickerPrice))
                    viewModel.WindowStickerPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                    if (flag)
                        viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ManufacturerRebate))
                    viewModel.ManufacturerRebate = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                    if (flag)
                        viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
                }
                
                if (String.IsNullOrEmpty(row.CarImageUrl))
                    viewModel.SinglePhoto = row.DefaultImageUrl;
                else
                {


                    viewModel.SinglePhoto =
                                              String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                  ? row.DefaultImageUrl
                                                  : row.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                                           StringSplitOptions.
                                                                                               RemoveEmptyEntries).FirstOrDefault();

                }
                
                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

                var dealer = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealerId);
                if (row.NewUsed == "Used")
                {
                    try
                    {
                        viewModel.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(viewModel.Vin, dealer.CarFax, dealer.CarFaxPassword);
                    }
                    catch (Exception)
                    {

                    }

                }
                else
                {
                    var carfax = new CarFaxViewModel();

                    carfax.ReportList = new List<CarFaxWindowSticker>();

                    carfax.Success = false;

                    var dealerPrice = new KellyBlueBookViewModel()
                    {
                        Vin = viewModel.Vin,

                    };

                    viewModel.CarFax = carfax;

                    viewModel.KBB = dealerPrice;
                }
                
                var existingChartSelection = context.vincontrolchartselections.Where(s => s.listingId == listingId && s.screen == "Inventory" && s.sourceType == "AutoTrader").FirstOrDefault();
                if (existingChartSelection != null)
                {
                    viewModel.SavedSelections.IsAll = existingChartSelection.isAll != null ? Convert.ToBoolean(existingChartSelection.isAll) : false;
                    viewModel.SavedSelections.IsCarsCom = existingChartSelection.isCarsCom != null ? Convert.ToBoolean(existingChartSelection.isCarsCom) : false;
                    viewModel.SavedSelections.IsCertified = existingChartSelection.isCertified != null ? Convert.ToBoolean(existingChartSelection.isCertified) : false;
                    viewModel.SavedSelections.IsFranchise = existingChartSelection.isFranchise != null ? Convert.ToBoolean(existingChartSelection.isFranchise) : false;
                    viewModel.SavedSelections.IsIndependant = existingChartSelection.isIndependant != null ? Convert.ToBoolean(existingChartSelection.isIndependant) : false;
                    viewModel.SavedSelections.Options = existingChartSelection.options.ToLower() ?? "";
                    viewModel.SavedSelections.Trims = existingChartSelection.trims.ToLower() ?? "";
                }

                if (Session["DealerGroup"] != null)
                    viewModel.MultipleDealers = true;
                else
                    viewModel.MultipleDealers = false;

                viewModel.DateInStock = row.DateInStock;
                viewModel.DaysInInvenotry = DateTime.Now.Subtract(row.DateInStock.Value).Days;

                if (!String.IsNullOrEmpty(row.VehicleType) && row.VehicleType.Equals("Truck"))
                {
                    viewModel.SelectedTruckType = row.TruckType;

                    viewModel.SelectedTruckClass = row.TruckClass;

                    viewModel.SelectedTruckCategory = row.TruckCategory;

                    return View("iTruckProfileOnMobile", viewModel);
                }
                ViewData["CarFaxResponse"] = viewModel.CarFax.CarFaxXMLResponse;
                viewModel.CarFaxDealerId = dealer.CarFax;

                return View("iProfileOnMobile", viewModel);            
        }

        public ActionResult ManheimData(string listingId, int InventoryStatus)
        {
            List<ManheimWholesaleViewModel> result;
            try
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    int convertedListingId = Convert.ToInt32(listingId);
                    if (InventoryStatus == 1)
                    {
                        var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == convertedListingId);
                        var dealer = (DealershipViewModel)Session["Dealership"];
                        var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = LinqHelper.ManheimReport(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                    else if (InventoryStatus == -1)
                    {
                        var row = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == convertedListingId);
                        var dealer = (DealershipViewModel)Session["Dealership"];
                        var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = LinqHelper.ManheimReportForSoldCars(row, manheimCredential.Manheim.Trim(),
                                                              manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                    else
                    {
                        var row = context.vincontrolwholesaleinventories.FirstOrDefault( x => x.ListingID == convertedListingId);
                        var dealer = (DealershipViewModel)Session["Dealership"];
                        var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = LinqHelper.ManheimReportForWholesale(row, manheimCredential.Manheim.Trim(),
                                                              manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return PartialView("ManheimData", result);
        }

        public ActionResult ManheimDataOnMobile(string listingId, int InventoryStatus)
        {
            List<ManheimWholesaleViewModel> result;
            try
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    int convertedListingId = Convert.ToInt32(listingId);
                    if (InventoryStatus == 1)
                    {
                        var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == convertedListingId);
                        var dealer = (DealershipViewModel)Session["Dealership"];
                        var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = LinqHelper.ManheimReport(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                    else if (InventoryStatus == -1)
                    {
                        var row = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == convertedListingId);
                        var dealer = (DealershipViewModel)Session["Dealership"];
                        var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = LinqHelper.ManheimReportForSoldCars(row, manheimCredential.Manheim.Trim(),
                                                              manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                    else
                    {
                        var row = context.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == convertedListingId);
                        var dealer = (DealershipViewModel)Session["Dealership"];
                        var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = LinqHelper.ManheimReportForWholesale(row, manheimCredential.Manheim.Trim(),
                                                              manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return PartialView("ManheimData", result);
        }

     
        public ActionResult KarPowerData(int listingId, int InventoryStatus)
        {
            ViewData["LISTINGID"] = listingId;

            var kbbStatus = string.Empty;
            try
            {
                kbbStatus = System.Configuration.ConfigurationManager.AppSettings["KBBStatus"];
            }
            catch (Exception)
            {
                // in case we forgot include KBBStatus in web.config
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int convertedListingId = Convert.ToInt32(listingId);
                var dealer = (DealershipViewModel)Session["Dealership"];
                var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealer.DealershipId);

                CarInfoFormViewModel row;
                string returnedView = "KarPowerData";
                string type = Constanst.VehicleTable.Inventory;

                if (InventoryStatus == 1)
                {
                    var existingInventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == convertedListingId);
                    row = new CarInfoFormViewModel()
                              {
                                  Vin = existingInventory.VINNumber,
                                  Mileage = existingInventory.Mileage,
                                  KBBTrimId = existingInventory.KBBTrimId,
                                  KBBOptionsId = existingInventory.KBBOptionsId,
                                  NewUsed = existingInventory.NewUsed
                              };
                }
                else if (InventoryStatus == -1)
                {
                    var existingSoldOutInventory = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == convertedListingId);
                    row = new CarInfoFormViewModel()
                    {
                        Vin = existingSoldOutInventory.VINNumber,
                        Mileage = existingSoldOutInventory.Mileage,
                        KBBTrimId = existingSoldOutInventory.KBBTrimId,
                        KBBOptionsId = existingSoldOutInventory.KBBOptionsId,
                        NewUsed = existingSoldOutInventory.NewUsed
                    };

                    returnedView = "KarPowerDataSoldOut";
                    type = Constanst.VehicleTable.SoldOut;
                }
                else
                {
                    var existingWholesaleInventory = context.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == convertedListingId);
                    row = new CarInfoFormViewModel()
                    {
                        Vin = existingWholesaleInventory.VINNumber,
                        Mileage = existingWholesaleInventory.Mileage,
                        KBBTrimId = existingWholesaleInventory.KBBTrimId,
                        KBBOptionsId = existingWholesaleInventory.KBBOptionsId,
                        NewUsed = existingWholesaleInventory.NewUsed
                    };

                    returnedView = "KarPowerDataWholesale";
                    type = Constanst.VehicleTable.WholeSale;
                }

                if (!kbbStatus.ToLower().Equals("inactive") && (setting.KellyBlueBookAccessRight != null && setting.KellyBlueBookAccessRight.Value))
                {
                    var result = new KellyBlueBookViewModel();

                    if (row.NewUsed == "Used")
                    {
                        try
                        {
                            result = row.KBBTrimId == null
                                         ? KellyBlueBookHelper.GetFullReport(row.Vin, dealer.ZipCode,
                                                                             row.Mileage)
                                         : KellyBlueBookHelper.GetFullReport(row.Vin, dealer.ZipCode,
                                                                             row.Mileage, row.KBBTrimId.Value,
                                                                             row.KBBOptionsId);
                            result.Success = true;
                        }
                        catch (Exception)
                        {

                        }
                    }

                    return PartialView("KBBData", result);
                }
                else
                {
                    List<SmallKarPowerViewModel> result;
                    if (setting != null)
                    {
                        try
                        {
                            if (context.whitmanenterprisekbbs.Any(x => x.Vin == row.Vin && x.Type.Equals(type)))
                            {
                                var searchResult = context.whitmanenterprisekbbs.Where(x => x.Vin == row.Vin &&x.Type.Equals(type)).OrderBy(x => x.DateAdded).ToList();

                                result = new List<SmallKarPowerViewModel>();

                                foreach (var tmp in searchResult)
                                {
                                    var kbbModel = new SmallKarPowerViewModel()
                                    {
                                        BaseWholesale = tmp.BaseWholeSale,
                                        MileageAdjustment = tmp.MileageAdjustment,
                                        SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                        SelectedTrimName = tmp.Trim,
                                        Wholesale = tmp.WholeSale,
                                        IsSelected = row.KBBTrimId.GetValueOrDefault() == 0 ? true : (tmp.TrimId == row.KBBTrimId),
                                    };

                                    result.Add(kbbModel);
                                }

                                if (result.Count > 0 && !result.Any(i => i.IsSelected))
                                {
                                    foreach (var item in result)
                                    {
                                        item.IsSelected = true;
                                    }
                                }
                            }
                            else
                            {
                                var karpowerService = new KarPowerService();
                                if (row.KBBTrimId == null || String.IsNullOrEmpty(row.KBBOptionsId))
                                    result = karpowerService.Execute(row.Vin, row.Mileage, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, type);
                                else
                                {
                                    result = karpowerService.Execute(row.Vin, row.Mileage, row.KBBTrimId.GetValueOrDefault(), row.KBBOptionsId, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, type);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            result = new List<SmallKarPowerViewModel>();
                        }

                    }
                    else
                        result = new List<SmallKarPowerViewModel>();

                    ViewData["LISTINGID"] = convertedListingId;
                    return PartialView(returnedView, result);
                }

            }
        }

        public ActionResult KarPowerDataOnMobile(int listingId, int InventoryStatus)
        {
            ViewData["LISTINGID"] = listingId;

            var kbbStatus = string.Empty;
            try
            {
                kbbStatus = System.Configuration.ConfigurationManager.AppSettings["KBBStatus"];
            }
            catch (Exception)
            {
                // in case we forgot include KBBStatus in web.config
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int convertedListingId = Convert.ToInt32(listingId);
                //var dealer = (DealershipViewModel)Session["Dealership"];
                var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == SessionHandler.DealerId);

                CarInfoFormViewModel row;
                string returnedView = "KarPowerData";
                string type = Constanst.VehicleTable.Inventory;

                if (InventoryStatus == 1)
                {
                    var existingInventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == convertedListingId);
                    row = new CarInfoFormViewModel()
                    {
                        Vin = existingInventory.VINNumber,
                        Mileage = existingInventory.Mileage,
                        KBBTrimId = existingInventory.KBBTrimId,
                        KBBOptionsId = existingInventory.KBBOptionsId,
                        NewUsed = existingInventory.NewUsed
                    };
                }
                else if (InventoryStatus == -1)
                {
                    var existingSoldOutInventory = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == convertedListingId);
                    row = new CarInfoFormViewModel()
                    {
                        Vin = existingSoldOutInventory.VINNumber,
                        Mileage = existingSoldOutInventory.Mileage,
                        KBBTrimId = existingSoldOutInventory.KBBTrimId,
                        KBBOptionsId = existingSoldOutInventory.KBBOptionsId,
                        NewUsed = existingSoldOutInventory.NewUsed
                    };

                    returnedView = "KarPowerDataSoldOut";
                    type = Constanst.VehicleTable.SoldOut;
                }
                else
                {
                    var existingWholesaleInventory = context.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == convertedListingId);
                    row = new CarInfoFormViewModel()
                    {
                        Vin = existingWholesaleInventory.VINNumber,
                        Mileage = existingWholesaleInventory.Mileage,
                        KBBTrimId = existingWholesaleInventory.KBBTrimId,
                        KBBOptionsId = existingWholesaleInventory.KBBOptionsId,
                        NewUsed = existingWholesaleInventory.NewUsed
                    };

                    returnedView = "KarPowerDataWholesale";
                    type = Constanst.VehicleTable.WholeSale;
                }

                if (!kbbStatus.ToLower().Equals("inactive") && (setting.KellyBlueBookAccessRight != null && setting.KellyBlueBookAccessRight.Value))
                {
                    var result = new KellyBlueBookViewModel();

                    if (row.NewUsed == "Used")
                    {
                        try
                        {
                            result = row.KBBTrimId == null
                                         ? KellyBlueBookHelper.GetFullReport(row.Vin, "dealer.ZipCode",
                                                                             row.Mileage)
                                         : KellyBlueBookHelper.GetFullReport(row.Vin, "dealer.ZipCode",
                                                                             row.Mileage, row.KBBTrimId.Value,
                                                                             row.KBBOptionsId);
                            result.Success = true;
                        }
                        catch (Exception)
                        {

                        }
                    }

                    return PartialView("KBBData", result);
                }
                else
                {
                    List<SmallKarPowerViewModel> result;
                    if (setting != null)
                    {
                        try
                        {
                            if (context.whitmanenterprisekbbs.Any(x => x.Vin == row.Vin && x.Type.Equals(type)))
                            {
                                var searchResult = context.whitmanenterprisekbbs.Where(x => x.Vin == row.Vin &&  x.Type.Equals(type)).OrderBy(x => x.DateAdded).ToList();

                                result = new List<SmallKarPowerViewModel>();

                                foreach (var tmp in searchResult)
                                {
                                    var kbbModel = new SmallKarPowerViewModel()
                                    {
                                        BaseWholesale = tmp.BaseWholeSale,
                                        MileageAdjustment = tmp.MileageAdjustment,
                                        SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                        SelectedTrimName = tmp.Trim,
                                        Wholesale = tmp.WholeSale,
                                        IsSelected = row.KBBTrimId.GetValueOrDefault() == 0 ? true : (tmp.TrimId == row.KBBTrimId),
                                    };

                                    result.Add(kbbModel);
                                }

                                if (result.Count > 0 && !result.Any(i => i.IsSelected))
                                {
                                    foreach (var item in result)
                                    {
                                        item.IsSelected = true;
                                    }
                                }
                            }
                            else
                            {
                                var karpowerService = new KarPowerService();
                                if (row.KBBTrimId == null || String.IsNullOrEmpty(row.KBBOptionsId))
                                    result = karpowerService.Execute(row.Vin, row.Mileage, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, type);
                                else
                                {
                                    result = karpowerService.Execute(row.Vin, row.Mileage, row.KBBTrimId.GetValueOrDefault(), row.KBBOptionsId, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, type);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            result = new List<SmallKarPowerViewModel>();
                        }

                    }
                    else
                        result = new List<SmallKarPowerViewModel>();

                    ViewData["LISTINGID"] = convertedListingId;
                    return PartialView(returnedView, result);
                }

            }
        }

        [HttpPost]
        public string UpdateMarketUp(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var vehicle = context.whitmanenterprisedealershipinventories.Where(i => i.ListingID == listingId).FirstOrDefault();
                    if (vehicle != null)
                    {
                        vehicle.MarketRange = 3;
                        context.SaveChanges();

                        return "Updated successfully.";
                    }
                    else
                        return "This vehicle doesn't exist in inventory.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string UpdateMarketIn(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var vehicle = context.whitmanenterprisedealershipinventories.Where(i => i.ListingID == listingId).FirstOrDefault();
                    if (vehicle != null)
                    {
                        vehicle.MarketRange = 2;
                        context.SaveChanges();

                        return "Updated successfully.";
                    }
                    else
                        return "This vehicle doesn't exist in inventory.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string UpdateMarketDown(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var vehicle = context.whitmanenterprisedealershipinventories.Where(i => i.ListingID == listingId).FirstOrDefault();
                    if (vehicle != null)
                    {
                        vehicle.MarketRange = 1;
                        context.SaveChanges();

                        return "Updated successfully.";
                    }
                    else
                        return "This vehicle doesn't exist in inventory.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private int GetPreviousProfile(whitmanenterprisedealershipinventory row, string sortField)
        {
            if (Session["InventoryObject"] != null)
            {

                var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

                var sortList = viewModel.CarsList;

                var searchIndex = sortList.FindIndex(x => x.ListingId == row.ListingID);

                if (searchIndex == 0)
                    return -1;
                else
                {
                    if (searchIndex > 0 && searchIndex <= sortList.Count)
                        return sortList.ElementAt(searchIndex - 1).ListingId;
                    else
                        return -1;
                }
            }
            else
            {
                return -1;
            }


            //var context = new whitmanenterprisewarehouseEntities();
            //var test = context.whitmanenterprisedealershipinventories
            //    .Where(GetLessExpression(row, sortField))
            //    .Where(i => i.ListingID != row.ListingID && i.DealershipId == row.DealershipId && i.NewUsed.ToLower().Equals("used") && (i.Recon == null || !((bool)i.Recon)))
            //    .OrderByDescending(i => i.DateInStock)
            //    .ThenBy(e => e.ListingID)
            //    .Take(1).ToList().FirstOrDefault();

            //if (test != null)
            //{
            //    return test.ListingID;
            //}
            //return -1;
        }

        private int GetNextProfile(whitmanenterprisedealershipinventory row, string sortField)
        {
            if (Session["InventoryObject"] != null)
            {
                var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

                var sortList = viewModel.CarsList;

                var searchIndex = sortList.FindIndex(x => x.ListingId == row.ListingID);

                if (searchIndex == sortList.Count - 1)
                    return -1;
                else
                {
                    if (searchIndex <= sortList.Count - 2)
                        return sortList.ElementAt(searchIndex + 1).ListingId;
                    else
                        return -1;
                }

            }

            else
            {
                return -1;
            }
            //var context = new whitmanenterprisewarehouseEntities();
            //var test = context.whitmanenterprisedealershipinventories
            //    .Where(GetGreaterExpression(row, sortField))
            //    .Where(i => i.ListingID != row.ListingID && i.DealershipId == row.DealershipId && i.NewUsed.ToLower().Equals("used") && (i.Recon == null || !((bool)i.Recon)))
            //    .OrderBy(i => i.DateInStock)
            //    .ThenBy(e => e.ListingID)
            //    .Take(1).ToList().FirstOrDefault();

            //if (test != null)
            //{
            //    return test.ListingID;
            //}
            //return -1;
        }

        private System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventory, bool>> GetGreaterExpression(whitmanenterprisedealershipinventory target, string sortField)
        {
            switch (sortField)
            {
                /*
            case "Make":
                return i => i.Make.CompareTo(row.Make) >= 0 ? true : false;
            case "Model":
                return i => i.Model.CompareTo(row.Model) >= 0 ? true : false;
            case "Year":
                int year = row.ModelYear?? 0;
                return i => i.ModelYear.HasValue?;
            case "Market":
                return i => i.SalePrice.CompareTo(row.SalePrice) >= 0 ? true : false;
                 * */
                case "Age":
                default:

                    return i => (i.DateInStock == target.DateInStock) ? (i.ListingID > target.ListingID) : i.DateInStock > target.DateInStock;
            }
        }

        private System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventory, bool>> GetLessExpression(whitmanenterprisedealershipinventory target, string sortField)
        {
            switch (sortField)
            {
                /*
            case "Make":
                return i => i.Make.CompareTo(row.Make) >= 0 ? true : false;
            case "Model":
                return i => i.Model.CompareTo(row.Model) >= 0 ? true : false;
            case "Year":
                int year = row.ModelYear?? 0;
                return i => i.ModelYear.HasValue?;
            case "Market":
                return i => i.SalePrice.CompareTo(row.SalePrice) >= 0 ? true : false;
                 * */
                case "Age":
                default:

                    return i => (i.DateInStock == target.DateInStock) ? (i.ListingID < target.ListingID) : i.DateInStock < target.DateInStock;
            }
        }

        private bool Test(whitmanenterprisedealershipinventory item, whitmanenterprisedealershipinventory target)
        {
            DateTime targetDate = target.DateInStock ?? DateTime.MinValue;
            DateTime itemDate = item.DateInStock ?? DateTime.MinValue;
            if (targetDate == itemDate)
            {
                return item.ListingID > target.ListingID;
            }
            else
            {
                return itemDate > targetDate;
            }

        }


        public ActionResult ViewIWholesaleProfile(int ListingID)
        {
            ResetSessionValue();
            if (Session["Dealership"] != null)
            {
                var context = new whitmanenterprisewarehouseEntities();

                var row = context.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == ListingID);

                var dealer = (DealershipViewModel)Session["Dealership"];

                int doorNumber; Int32.TryParse(row.Doors, out doorNumber);

                var viewModel = new CarInfoFormViewModel
                    {
                        SavedSelections = new ChartSelection(),
                        Vin = String.IsNullOrEmpty(row.VINNumber) ? "" : row.VINNumber,
                        Make = String.IsNullOrEmpty(row.Make) ? "" : row.Make,
                        ModelYear =
                            String.IsNullOrEmpty(row.ModelYear.GetValueOrDefault()
                                                    .ToString(CultureInfo.InvariantCulture))
                                ? 0
                                : row.ModelYear.GetValueOrDefault(),
                        Model = String.IsNullOrEmpty(row.Model) ? "" : row.Model,
                        Litters = String.IsNullOrEmpty(row.Liters) ? "" : row.Liters,
                        MSRP = String.IsNullOrEmpty(row.MSRP) ? "" : row.MSRP,
                        Fuel = String.IsNullOrEmpty(row.FuelType) ? "" : row.FuelType,
                        Engine = String.IsNullOrEmpty(row.EngineType) ? "" : row.EngineType,
                        Trim = String.IsNullOrEmpty(row.Trim) ? "" : row.Trim,
                        ChromeStyleId = row.ChromeStyleId,
                        ChromeModelId = row.ChromeModelId,
                        Cylinder = String.IsNullOrEmpty(row.Cylinders) ? "" : row.Cylinders,
                        Tranmission = String.IsNullOrEmpty(row.Tranmission) ? "" : row.Tranmission,
                        IsCertified = row.Certified.GetValueOrDefault(),
                        PriorRental = row.PriorRental.GetValueOrDefault(),
                        WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? "" : row.DriveTrain,
                        StockNumber = String.IsNullOrEmpty(row.StockNumber) ? "" : row.StockNumber,
                        ExteriorColor = String.IsNullOrEmpty(row.ExteriorColor) ? "" : row.ExteriorColor,
                        InteriorColor = String.IsNullOrEmpty(row.InteriorColor) ? "" : row.InteriorColor,
                        Mileage = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage,
                        Door = String.IsNullOrEmpty(row.Doors) ? 0 : doorNumber,
                        BodyType = String.IsNullOrEmpty(row.BodyType) ? "" : row.BodyType,
                        DefaultImageUrl = String.IsNullOrEmpty(row.DefaultImageUrl) ? "" : row.DefaultImageUrl,
                        Description = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions,
                        ListingId = row.ListingID,
                        DealershipId = dealer.DealershipId,
                        UploadPhotosURL =
                            String.IsNullOrEmpty(row.ThumbnailImageURL)
                                ? null
                                : (from data in
                                       row.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                   StringSplitOptions.RemoveEmptyEntries)
                                   select data).ToList(),
                        CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl,
                        InventoryStatus = 2,
                        DateInStock = row.DateInStock,
                        DaysInInvenotry = DateTime.Now.Subtract(row.DateInStock.Value).Days,
                        CarFaxDealerId = dealer.CarFax,
                        ButtonPermissions = SQLHelper.GetButtonList(dealer.DealershipId, "Profile")
                    };


                viewModel.WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? viewModel.WheelDrive : row.DriveTrain;



                if (String.IsNullOrEmpty(row.SalePrice))
                    viewModel.SalePrice = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.SalePrice, out priceFormat);
                    if (flag)
                        viewModel.SalePrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerCost))
                    viewModel.DealerCost = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerCost, out priceFormat);
                    if (flag)
                        viewModel.DealerCost = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ACV))
                    viewModel.ACV = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ACV, out priceFormat);
                    if (flag)
                        viewModel.ACV = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.RetailPrice))
                    viewModel.RetailPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.RetailPrice, out priceFormat);
                    if (flag)
                        viewModel.RetailPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerDiscount))
                    viewModel.DealerDiscount = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                    if (flag)
                        viewModel.DealerDiscount = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.WindowStickerPrice))
                    viewModel.WindowStickerPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                    if (flag)
                        viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ManufacturerRebate))
                    viewModel.ManufacturerRebate = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                    if (flag)
                        viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
                }


                if (String.IsNullOrEmpty(row.CarImageUrl))
                    viewModel.SinglePhoto = row.DefaultImageUrl;
                else
                {


                    viewModel.SinglePhoto =
                                              String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                  ? row.DefaultImageUrl
                                                  : row.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                                           StringSplitOptions.
                                                                                               RemoveEmptyEntries).FirstOrDefault();

                }


                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;



                if (row.NewUsed == "Used")
                {
                    try
                    {
                        viewModel.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(viewModel.Vin, dealer.CarFax, dealer.CarFaxPassword);
                    }
                    catch (Exception)
                    {

                    }

                }
                else
                {
                    var carfax = new CarFaxViewModel();

                    carfax.ReportList = new List<CarFaxWindowSticker>();

                    carfax.Success = false;

                    var dealerPrice = new KellyBlueBookViewModel()
                    {
                        Vin = viewModel.Vin,

                    };

                    viewModel.CarFax = carfax;

                    viewModel.KBB = dealerPrice;
                }

                viewModel.BB = BlackBookService.GetFullReport(viewModel.Vin, viewModel.Mileage, dealer.State);

                viewModel.DaysInInvenotry = DateTime.Now.Subtract(row.DateInStock.Value).Days;

                if (Session["DealerGroup"] != null)
                    viewModel.MultipleDealers = true;
                else
                    viewModel.MultipleDealers = false;



                return View("iWholesaleProfile", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }



        }

        public ActionResult ViewISoldProfile(int listingId)
        {
            ResetSessionValue();

            if (Session["Dealership"] != null)
            {
                var context = new whitmanenterprisewarehouseEntities();

                var row =
                    context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == listingId);
                var viewModel = new CarInfoFormViewModel();

                var dealer = (DealershipViewModel)Session["Dealership"];

                SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

                viewModel.SavedSelections = new ChartSelection();

                viewModel.ChromeStyleId = row.ChromeStyleId;

                viewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "" : row.VINNumber;

                viewModel.Make = String.IsNullOrEmpty(row.Make) ? "" : row.Make;

                viewModel.ModelYear = String.IsNullOrEmpty(row.ModelYear.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)) ? 0 : row.ModelYear.GetValueOrDefault();

                viewModel.Model = String.IsNullOrEmpty(row.Model) ? "" : row.Model;

                viewModel.Litters = String.IsNullOrEmpty(row.Liters) ? "" : row.Liters;

                viewModel.MSRP = String.IsNullOrEmpty(row.MSRP) ? "" : row.MSRP;

                viewModel.Fuel = String.IsNullOrEmpty(row.FuelType) ? "" : row.FuelType;

                viewModel.Engine = String.IsNullOrEmpty(row.EngineType) ? "" : row.EngineType;

                viewModel.Trim = String.IsNullOrEmpty(row.Trim) ? "" : row.Trim;

                viewModel.Cylinder = String.IsNullOrEmpty(row.Cylinders) ? "" : row.Cylinders;

                viewModel.Tranmission = String.IsNullOrEmpty(row.Tranmission) ? "" : row.Tranmission;

                viewModel.IsCertified = row.Certified.GetValueOrDefault();

                viewModel.PriorRental = row.PriorRental.GetValueOrDefault();

                viewModel.WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? "" : row.DriveTrain;

                viewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber) ? "" : row.StockNumber;

                viewModel.ExteriorColor = String.IsNullOrEmpty(row.ExteriorColor) ? "" : row.ExteriorColor;

                viewModel.InteriorColor = String.IsNullOrEmpty(row.InteriorColor) ? "" : row.InteriorColor;

                viewModel.Mileage = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;

                viewModel.Door = String.IsNullOrEmpty(row.Doors) ? 0 : Convert.ToInt32(row.Doors);

                viewModel.BodyType = String.IsNullOrEmpty(row.BodyType) ? "" : row.BodyType;

                viewModel.WheelDrive = String.IsNullOrEmpty(row.DriveTrain) ? viewModel.WheelDrive : row.DriveTrain;

                viewModel.CarFaxDealerId = dealer.CarFax;

                if (String.IsNullOrEmpty(row.SalePrice))
                    viewModel.SalePrice = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.SalePrice, out priceFormat);
                    if (flag)
                        viewModel.SalePrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerCost))
                    viewModel.DealerCost = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerCost, out priceFormat);
                    if (flag)
                        viewModel.DealerCost = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ACV))
                    viewModel.ACV = "NA";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ACV, out priceFormat);
                    if (flag)
                        viewModel.ACV = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.RetailPrice))
                    viewModel.RetailPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.RetailPrice, out priceFormat);
                    if (flag)
                        viewModel.RetailPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerDiscount))
                    viewModel.DealerDiscount = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                    if (flag)
                        viewModel.DealerDiscount = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.WindowStickerPrice))
                    viewModel.WindowStickerPrice = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                    if (flag)
                        viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ManufacturerRebate))
                    viewModel.ManufacturerRebate = "";
                else
                {
                    double priceFormat = 0;
                    bool flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                    if (flag)
                        viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
                }



                viewModel.DefaultImageUrl = String.IsNullOrEmpty(row.DefaultImageUrl) ? "" : row.DefaultImageUrl;

                viewModel.Description = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions;


                if (String.IsNullOrEmpty(row.CarImageUrl))
                    viewModel.SinglePhoto = row.DefaultImageUrl;
                else
                {


                    viewModel.SinglePhoto =
                                              String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                  ? row.DefaultImageUrl
                                                  : row.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                                           StringSplitOptions.
                                                                                               RemoveEmptyEntries).FirstOrDefault();

                }

                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

                viewModel.ListingId = row.ListingID;

                viewModel.DealershipId = dealer.DealershipId;

                viewModel.UploadPhotosURL = String.IsNullOrEmpty(row.ThumbnailImageURL) ? null : (from data in row.ThumbnailImageURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                viewModel.InventoryStatus = -1;

                viewModel.DaysInInvenotry = DateTime.Now.Subtract(row.DateInStock.Value).Days;

                if (row.NewUsed == "Used")
                {
                    try
                    {
                        viewModel.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(viewModel.Vin, dealer.CarFax, dealer.CarFaxPassword);
                    }
                    catch (Exception)
                    {

                    }

                    if (row.KBBTrimId == null)

                        viewModel.KBB = KellyBlueBookHelper.GetFullReport(viewModel.Vin, dealer.ZipCode, viewModel.Mileage);
                    else
                    {
                        viewModel.KBB = KellyBlueBookHelper.GetFullReport(viewModel.Vin, dealer.ZipCode, viewModel.Mileage, row.KBBTrimId.Value, row.KBBOptionsId);
                    }
                }
                else
                {
                    var carfax = new CarFaxViewModel();

                    carfax.ReportList = new List<CarFaxWindowSticker>();

                    carfax.Success = false;

                    var dealerPrice = new KellyBlueBookViewModel()
                    {
                        Vin = viewModel.Vin,

                    };

                    viewModel.CarFax = carfax;

                    viewModel.KBB = dealerPrice;
                }

                viewModel.BB = BlackBookService.GetFullReport(viewModel.Vin, viewModel.Mileage, dealer.State);


                var existingChartSelection = context.vincontrolchartselections.Where(s => s.listingId == listingId && s.screen == "Inventory" && s.sourceType == "AutoTrader").FirstOrDefault();

                if (existingChartSelection != null)
                {
                    viewModel.SavedSelections.IsAll = existingChartSelection.isAll != null ? Convert.ToBoolean(existingChartSelection.isAll) : false;
                    viewModel.SavedSelections.IsCarsCom = existingChartSelection.isCarsCom != null ? Convert.ToBoolean(existingChartSelection.isCarsCom) : false;
                    viewModel.SavedSelections.IsCertified = existingChartSelection.isCertified != null ? Convert.ToBoolean(existingChartSelection.isCertified) : false;
                    viewModel.SavedSelections.IsFranchise = existingChartSelection.isFranchise != null ? Convert.ToBoolean(existingChartSelection.isFranchise) : false;
                    viewModel.SavedSelections.IsIndependant = existingChartSelection.isIndependant != null ? Convert.ToBoolean(existingChartSelection.isIndependant) : false;
                    viewModel.SavedSelections.Options = existingChartSelection.options.ToLower() ?? "";
                    viewModel.SavedSelections.Trims = existingChartSelection.trims.ToLower() ?? "";
                }


                viewModel.IsSold = true;

                viewModel.CarFaxDealerId = dealer.CarFax;

                if (Session["DealerGroup"] != null)
                    viewModel.MultipleDealers = true;
                else
                    viewModel.MultipleDealers = false;

                // include Manheim Wholesales values
                try
                {
                    //var manheimCredential = LINQHelper.GetManheimCredential(dealer.DealershipId);
                    //if (manheimCredential != null)
                    //    viewModel.ManheimWholesales = LINQHelper.ManheimReportForSoldout(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim());
                    //else
                    viewModel.ManheimWholesales = new List<ManheimWholesaleViewModel>();
                }
                catch (Exception)
                {
                    viewModel.ManheimWholesales = new List<ManheimWholesaleViewModel>();
                }

                viewModel.ButtonPermissions = SQLHelper.GetButtonList(dealer.DealershipId, "Profile");

                if (viewModel.IsTruck)
                {
                    viewModel.SelectedTruckType = row.TruckType;

                    viewModel.SelectedTruckClass = row.TruckClass;

                    viewModel.SelectedTruckCategory = row.TruckCategory;

                    viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                    viewModel.TruckCategoryList =
                        SelectListHelper.InitalTruckCategoryList(
                            SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                    viewModel.TruckClassList = SelectListHelper.InitalTruckClassList();

                    return View("iTruckProfile", viewModel);
                }

                return View("iProfile", viewModel);


            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }


        public ActionResult EditDescription(int ListingID)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();

                var viewModel = new CarDescriptionModel()
                                    {
                                        ListingId = ListingID
                                    };

                viewModel.DescriptionList = new List<DescriptionSentenceGroup>();

                var descriptionList = context.vincontroldescriptions.ToList();

                foreach (var tmp in descriptionList.Select(x => x.Title).Distinct())
                {
                    var description = new DescriptionSentenceGroup();

                    description.Title = tmp;

                    description.Sentences = new List<DesctiptionSentence>();



                    foreach (var des in descriptionList.Where(x => x.Title == description.Title))
                    {


                        var detailDescript = new DesctiptionSentence()
                                                 {
                                                     DescriptionSentence = des.DescriptionSentence,
                                                     YesNo = des.YesNo.GetValueOrDefault()
                                                 };

                        description.Sentences.Add(detailDescript);

                    }

                    viewModel.DescriptionList.Add(description);
                }

                Session["Description"] = viewModel.DescriptionList;


                return View("EditDescription", viewModel);
            }
        }


        public ActionResult EditIProfile(int ListingID)
        {
            ResetSessionValue();
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();

                var dealer = (DealershipViewModel)Session["Dealership"];

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingID);

                var setting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

                var autoService = new ChromeAutoService();

                var vehicleInfo = autoService.GetVehicleInformationFromVin(row.VINNumber);

                if (vehicleInfo != null)
                {
                    var viewModel = InitCarInfoDataWhenVehicleNotNull(ListingID, dealer, row, setting, autoService, vehicleInfo);

                    if (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful ||
                        vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.ConditionallySuccessful)
                        viewModel.VinDecodeSuccess = true;

                    viewModel.BodyType = String.IsNullOrEmpty(row.BodyType) ? "" : row.BodyType;
                    viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeList();
                    return View("EditProfile", viewModel);
                }
                else
                {
                    var viewModel = InitCarInfoDataWhenVehicleIsNull(ListingID, dealer, row, setting, autoService);
                    return View("EditProfile", viewModel);
                }
            }
        }

        private static CarInfoFormViewModel InitCarInfoDataWhenVehicleIsNull(int listingID, DealershipViewModel dealer, whitmanenterprisedealershipinventory row, whitmanenterprisesetting setting, ChromeAutoService autoService)
        {
            var viewModel = new CarInfoFormViewModel();

            if (!String.IsNullOrEmpty(row.ChromeStyleId))
            {
                int chromeStyleId;
                Int32.TryParse(row.ChromeStyleId, out chromeStyleId);

                var styleArray = autoService.GetVehicleInformationFromStyleId(chromeStyleId);

                viewModel.Vin = row.VINNumber ?? string.Empty;

                viewModel.Make = row.Make ?? string.Empty;

                viewModel.ModelYear = row.ModelYear.GetValueOrDefault();

                viewModel.Model = row.Model ?? string.Empty;

                viewModel.Litters = row.Liters ?? string.Empty;

                viewModel.Fuel = row.FuelType ?? string.Empty;

                viewModel.Engine = row.EngineType ?? string.Empty;

                viewModel.Trim = row.Trim ?? string.Empty;

                viewModel.ChromeStyleId = row.ChromeStyleId;

                viewModel.ChromeModelId = row.ChromeModelId;

                //not yet decode
                viewModel.ACar = row.ACar.GetValueOrDefault();

                int styleId;
                if (Int32.TryParse(viewModel.ChromeStyleId, out styleId))
                {
                    bool existed;
                    viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, viewModel.Trim, styleArray.style, styleId, out existed);
                    if (!existed)
                    {
                        viewModel.CusTrim = viewModel.Trim;
                    }
                }
                else if (!String.IsNullOrEmpty(viewModel.Trim))
                {
                    bool existed;
                    viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, styleArray.style, viewModel.Trim, out existed);
                    if (!existed)
                    {
                        viewModel.CusTrim = viewModel.Trim;
                    }
                }
                else
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimList(styleArray.style);
                }

                viewModel.Title = row.AdditionalTitle ?? string.Empty;

                viewModel.Cylinder = row.Cylinders ?? string.Empty;

                viewModel.Tranmission = row.Tranmission ?? string.Empty;

                viewModel.IsCertified = row.Certified.GetValueOrDefault();

                viewModel.PriorRental = row.PriorRental.GetValueOrDefault();

                viewModel.WheelDrive = row.DriveTrain ?? string.Empty;

                viewModel.RetailPrice = row.RetailPrice ?? string.Empty;

                viewModel.DealerDiscount = row.DealerDiscount ?? string.Empty;

                viewModel.ManufacturerRebate = row.ManufacturerRebate ?? string.Empty;

                viewModel.BodyType = row.BodyType ?? string.Empty;

                viewModel.WindowStickerPrice = row.WindowStickerPrice ?? string.Empty;

                viewModel.ChromeFactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

                viewModel.ChromeFactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

                viewModel.WarrantyInfo = row.WarrantyInfo.GetValueOrDefault();

                if (String.IsNullOrEmpty(row.SalePrice))
                    viewModel.SalePrice = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.SalePrice, out priceFormat);
                    if (flag)
                        viewModel.SalePrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerCost))
                    viewModel.DealerCost = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.DealerCost, out priceFormat);
                    if (flag)
                        viewModel.DealerCost = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ACV))
                    viewModel.ACV = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.ACV, out priceFormat);
                    if (flag)
                        viewModel.ACV = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.RetailPrice))
                    viewModel.RetailPrice = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.RetailPrice, out priceFormat);
                    if (flag)
                        viewModel.RetailPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerDiscount))
                    viewModel.DealerDiscount = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                    if (flag)
                        viewModel.DealerDiscount = priceFormat.ToString("#,##0");
                }
                if (String.IsNullOrEmpty(row.WindowStickerPrice))
                    viewModel.WindowStickerPrice = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                    if (flag)
                        viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ManufacturerRebate))
                    viewModel.ManufacturerRebate = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                    if (flag)
                        viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
                }

                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

                viewModel.Mileage = row.Mileage ?? string.Empty;

                viewModel.DefaultImageUrl = row.DefaultImageUrl;

                viewModel.Description = row.Descriptions ?? string.Empty;

                viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeList();

                if (String.IsNullOrEmpty(row.CarImageUrl))
                    viewModel.SinglePhoto = row.DefaultImageUrl;
                else
                {
                    string[] totalImages = row.CarImageUrl.Split(new[] { "|", "," }, StringSplitOptions.RemoveEmptyEntries);
                    viewModel.SinglePhoto = totalImages[0];
                }

                if (String.IsNullOrEmpty(row.Doors))
                    viewModel.Door = 0;
                else
                {
                    int numberofdoor;
                    Int32.TryParse(row.Doors, out numberofdoor);
                    viewModel.Door = numberofdoor;
                }


                viewModel.BrandedTitle = row.BrandedTitle.GetValueOrDefault();


                viewModel.ACar = row.ACar.GetValueOrDefault();

                viewModel.ListingId = Convert.ToInt32(listingID);

                viewModel.DealershipId = dealer.DealershipId;

                viewModel.StockNumber = row.StockNumber ?? string.Empty;

                viewModel.Make = row.Make ?? string.Empty;

                viewModel.VehicleModel = row.Model ?? string.Empty;

                viewModel.SelectedExteriorColorValue = row.ExteriorColor ?? string.Empty;

                viewModel.SelectedExteriorColorCode = row.ColorCode ?? string.Empty;

                viewModel.SelectedInteriorColor = row.InteriorColor ?? string.Empty;

                viewModel.ChromeExteriorColorList = SelectListHelper.InitalExteriorColorList(null, viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

                viewModel.ChromeInteriorColorList = SelectListHelper.InitalInteriorColorList(null, viewModel.SelectedInteriorColor);

                viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;

                viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;

                viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

                viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

                viewModel.ExistOptions = String.IsNullOrEmpty(row.CarsOptions)
                                             ? null
                                             : (from data in row.CarsOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.ExistPackages = String.IsNullOrEmpty(row.CarsPackages)
                                              ? null
                                              : (from data in row.CarsPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.UploadPhotosURL = String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                ? null
                                                : (from data in row.ThumbnailImageURL.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                if (!String.IsNullOrEmpty(row.MSRP))
                {
                    double msrpFormat;
                    var msrpFlag = Double.TryParse(row.MSRP, out msrpFormat);
                    if (msrpFlag)
                        viewModel.MSRP = msrpFormat.ToString("#,##0");
                }

                viewModel.IsManual = row.EnableAutoDescription != null && !row.EnableAutoDescription.GetValueOrDefault();
                viewModel.EnableAutoDescriptionSetting = setting.AutoDescription.GetValueOrDefault();
                viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
                viewModel.WarrantyTypes = LinqHelper.GetWarrantyTypeList(SessionHandler.Dealership);
                viewModel.DateInStock = row.DateInStock;
            }
            else if (!String.IsNullOrEmpty(row.ChromeModelId))
            {
                int chromeModelId;
                Int32.TryParse(row.ChromeStyleId, out chromeModelId);
                var styleArray = autoService.GetStyles(Convert.ToInt32(chromeModelId));

                viewModel.Vin = row.VINNumber ?? string.Empty;

                viewModel.Make = row.Make ?? string.Empty;

                viewModel.ModelYear = row.ModelYear.GetValueOrDefault();

                viewModel.Model = row.Model ?? string.Empty;

                viewModel.Litters = row.Liters ?? string.Empty;

                viewModel.Fuel = row.FuelType ?? string.Empty;

                viewModel.Engine = row.EngineType ?? string.Empty;

                viewModel.Trim = row.Trim ?? string.Empty;

                viewModel.ChromeStyleId = row.ChromeStyleId;

                viewModel.ChromeModelId = row.ChromeModelId;

                //not yet decode
                viewModel.ACar = row.ACar.GetValueOrDefault();

                viewModel.EditTrimList = SelectListHelper.InitalTrimList(styleArray);

                viewModel.Title = row.AdditionalTitle ?? string.Empty;

                viewModel.Cylinder = row.Cylinders ?? string.Empty;

                viewModel.Tranmission = row.Tranmission ?? string.Empty;

                viewModel.IsCertified = row.Certified.GetValueOrDefault();

                viewModel.PriorRental = row.PriorRental.GetValueOrDefault();

                viewModel.WheelDrive = row.DriveTrain ?? string.Empty;

                viewModel.RetailPrice = row.RetailPrice ?? string.Empty;

                viewModel.DealerDiscount = row.DealerDiscount ?? string.Empty;

                viewModel.ManufacturerRebate = row.ManufacturerRebate ?? string.Empty;

                viewModel.BodyType = row.BodyType ?? string.Empty;

                viewModel.WindowStickerPrice = row.WindowStickerPrice ?? string.Empty;

                viewModel.ChromeFactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

                viewModel.ChromeFactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

                viewModel.WarrantyInfo = row.WarrantyInfo.GetValueOrDefault();

                viewModel.BrandedTitle = row.BrandedTitle.GetValueOrDefault();


                viewModel.ACar = row.ACar.GetValueOrDefault();


                if (String.IsNullOrEmpty(row.SalePrice))
                    viewModel.SalePrice = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.SalePrice, out priceFormat);
                    if (flag)
                        viewModel.SalePrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerCost))
                    viewModel.DealerCost = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.DealerCost, out priceFormat);
                    if (flag)
                        viewModel.DealerCost = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ACV))
                    viewModel.ACV = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.ACV, out priceFormat);
                    if (flag)
                        viewModel.ACV = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.RetailPrice))
                    viewModel.RetailPrice = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.RetailPrice, out priceFormat);
                    if (flag)
                        viewModel.RetailPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerDiscount))
                    viewModel.DealerDiscount = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                    if (flag)
                        viewModel.DealerDiscount = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.WindowStickerPrice))
                    viewModel.WindowStickerPrice = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                    if (flag)
                        viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ManufacturerRebate))
                    viewModel.ManufacturerRebate = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                    if (flag)
                        viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
                }


                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

                viewModel.Mileage = row.Mileage ?? string.Empty;

                viewModel.DefaultImageUrl = row.DefaultImageUrl;

                viewModel.Description = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions;

                viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeList();
                if (String.IsNullOrEmpty(row.CarImageUrl))
                    viewModel.SinglePhoto = row.DefaultImageUrl;
                else
                {
                    string[] totalImages = row.CarImageUrl.Split(new[] { "|", "," }, StringSplitOptions.RemoveEmptyEntries);
                    viewModel.SinglePhoto = totalImages[0];
                }

                if (String.IsNullOrEmpty(row.Doors))
                    viewModel.Door = 0;
                else
                {
                    int numberofdoor;
                    Int32.TryParse(row.Doors, out numberofdoor);
                    viewModel.Door = numberofdoor;
                }

                viewModel.ListingId = Convert.ToInt32(listingID);

                viewModel.DealershipId = dealer.DealershipId;

                viewModel.StockNumber = row.StockNumber ?? string.Empty;

                viewModel.Make = row.Make ?? string.Empty;

                viewModel.VehicleModel = row.Model ?? string.Empty;

                viewModel.SelectedExteriorColorValue = row.Model ?? string.Empty;

                viewModel.SelectedExteriorColorCode = row.ColorCode ?? string.Empty;

                viewModel.SelectedInteriorColor = row.InteriorColor ?? string.Empty;

                viewModel.ChromeExteriorColorList = SelectListHelper.InitalExteriorColorList(null, viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

                viewModel.ChromeInteriorColorList = SelectListHelper.InitalInteriorColorList(null, viewModel.SelectedInteriorColor);

                viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;

                viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;

                viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

                viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

                viewModel.ExistOptions = String.IsNullOrEmpty(row.CarsOptions)
                                             ? null
                                             : (from data in row.CarsOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.ExistPackages = String.IsNullOrEmpty(row.CarsPackages)
                                              ? null
                                              : (from data in row.CarsPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.UploadPhotosURL = String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                ? null
                                                : (from data in row.ThumbnailImageURL.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                if (!String.IsNullOrEmpty(row.MSRP))
                {
                    double msrpFormat;
                    var msrpFlag = Double.TryParse(row.MSRP, out msrpFormat);
                    if (msrpFlag)
                        viewModel.MSRP = msrpFormat.ToString("#,##0");
                }

                viewModel.IsManual = row.EnableAutoDescription != null && !row.EnableAutoDescription.GetValueOrDefault();
                viewModel.EnableAutoDescriptionSetting = setting.AutoDescription.GetValueOrDefault();
                viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
                viewModel.WarrantyTypes = LinqHelper.GetWarrantyTypeList(SessionHandler.Dealership);
                viewModel.DateInStock = row.DateInStock;
            }
            else
            {


                viewModel.Vin = row.VINNumber ?? string.Empty;

                viewModel.Make = row.Make ?? string.Empty;

                viewModel.ModelYear = row.ModelYear.GetValueOrDefault();

                viewModel.Model = row.Model ?? string.Empty;

                viewModel.Litters = row.Liters ?? string.Empty;

                viewModel.Fuel = row.FuelType ?? string.Empty;

                viewModel.Engine = row.EngineType ?? string.Empty;

                viewModel.Trim = row.Trim ?? string.Empty;

                viewModel.ChromeStyleId = row.ChromeStyleId;

                viewModel.ChromeModelId = row.ChromeModelId;

                //not yet decode
                viewModel.ACar = row.ACar.GetValueOrDefault();

                viewModel.EditTrimList = new List<SelectListItem>();

                viewModel.Title = row.AdditionalTitle ?? string.Empty;

                viewModel.Cylinder = row.Cylinders ?? string.Empty;

                viewModel.Tranmission = row.Tranmission ?? string.Empty;

                viewModel.IsCertified = row.Certified.GetValueOrDefault();

                viewModel.PriorRental = row.PriorRental.GetValueOrDefault();

                viewModel.WheelDrive = row.DriveTrain ?? string.Empty;

                viewModel.RetailPrice = row.RetailPrice ?? string.Empty;

                viewModel.DealerDiscount = row.DealerDiscount ?? string.Empty;

                viewModel.ManufacturerRebate = row.ManufacturerRebate ?? string.Empty;

                viewModel.BodyType = row.BodyType ?? string.Empty;

                viewModel.WindowStickerPrice = row.WindowStickerPrice ?? string.Empty;

                viewModel.ChromeFactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

                viewModel.ChromeFactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

                viewModel.WarrantyInfo = row.WarrantyInfo.GetValueOrDefault();

                viewModel.BrandedTitle = row.BrandedTitle.GetValueOrDefault();


                viewModel.ACar = row.ACar.GetValueOrDefault();


                if (String.IsNullOrEmpty(row.SalePrice))
                    viewModel.SalePrice = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.SalePrice, out priceFormat);
                    if (flag)
                        viewModel.SalePrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerCost))
                    viewModel.DealerCost = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.DealerCost, out priceFormat);
                    if (flag)
                        viewModel.DealerCost = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ACV))
                    viewModel.ACV = "NA";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.ACV, out priceFormat);
                    if (flag)
                        viewModel.ACV = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.RetailPrice))
                    viewModel.RetailPrice = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.RetailPrice, out priceFormat);
                    if (flag)
                        viewModel.RetailPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.DealerDiscount))
                    viewModel.DealerDiscount = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                    if (flag)
                        viewModel.DealerDiscount = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.WindowStickerPrice))
                    viewModel.WindowStickerPrice = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                    if (flag)
                        viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
                }

                if (String.IsNullOrEmpty(row.ManufacturerRebate))
                    viewModel.ManufacturerRebate = "";
                else
                {
                    double priceFormat;
                    var flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                    if (flag)
                        viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
                }


                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

                viewModel.Mileage = row.Mileage ?? string.Empty;

                viewModel.DefaultImageUrl = row.DefaultImageUrl;

                viewModel.Description = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions;

                viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeList();
                if (String.IsNullOrEmpty(row.CarImageUrl))
                    viewModel.SinglePhoto = row.DefaultImageUrl;
                else
                {
                    string[] totalImages = row.CarImageUrl.Split(new[] { "|", "," }, StringSplitOptions.RemoveEmptyEntries);
                    viewModel.SinglePhoto = totalImages[0];
                }

                if (String.IsNullOrEmpty(row.Doors))
                    viewModel.Door = 0;
                else
                {
                    int numberofdoor;
                    Int32.TryParse(row.Doors, out numberofdoor);
                    viewModel.Door = numberofdoor;
                }

                viewModel.ListingId = Convert.ToInt32(listingID);

                viewModel.DealershipId = dealer.DealershipId;

                viewModel.StockNumber = row.StockNumber ?? string.Empty;

                viewModel.Make = row.Make ?? string.Empty;

                viewModel.VehicleModel = row.Model ?? string.Empty;

                viewModel.SelectedExteriorColorValue = row.Model ?? string.Empty;

                viewModel.SelectedExteriorColorCode = row.ColorCode ?? string.Empty;

                viewModel.SelectedInteriorColor = row.InteriorColor ?? string.Empty;

                viewModel.ChromeExteriorColorList = SelectListHelper.InitalExteriorColorList(null, viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

                viewModel.ChromeInteriorColorList = SelectListHelper.InitalInteriorColorList(null, viewModel.SelectedInteriorColor);

                viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;

                viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;

                viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

                viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

                viewModel.ExistOptions = String.IsNullOrEmpty(row.CarsOptions)
                                             ? null
                                             : (from data in row.CarsOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.ExistPackages = String.IsNullOrEmpty(row.CarsPackages)
                                              ? null
                                              : (from data in row.CarsPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.UploadPhotosURL = String.IsNullOrEmpty(row.ThumbnailImageURL)
                                                ? null
                                                : (from data in row.ThumbnailImageURL.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

                viewModel.CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl;

                if (!String.IsNullOrEmpty(row.MSRP))
                {
                    double msrpFormat;
                    var msrpFlag = Double.TryParse(row.MSRP, out msrpFormat);
                    if (msrpFlag)
                        viewModel.MSRP = msrpFormat.ToString("#,##0");
                }

                viewModel.IsManual = row.EnableAutoDescription != null && !row.EnableAutoDescription.GetValueOrDefault();
                viewModel.EnableAutoDescriptionSetting = setting.AutoDescription.GetValueOrDefault();
                viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
                viewModel.WarrantyTypes = LinqHelper.GetWarrantyTypeList(SessionHandler.Dealership);
                viewModel.DateInStock = row.DateInStock;
            }

            return viewModel;
        }

        private static CarInfoFormViewModel InitCarInfoDataWhenVehicleNotNull(int listingId, DealershipViewModel dealer, whitmanenterprisedealershipinventory row, whitmanenterprisesetting setting, ChromeAutoService autoService, VehicleDescription vehicleInfo)
        {
            VehicleDescription styleInfo;

            //get chrome data from style ID
            if (!String.IsNullOrEmpty(row.ChromeStyleId))
            {
                int chromeStyleId;
                Int32.TryParse(row.ChromeStyleId, out chromeStyleId);
                styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);
            }
            else
            {
                var element = vehicleInfo.style.FirstOrDefault(x => x.trim == row.Trim);
                styleInfo = autoService.GetStyleInformationFromStyleId(element != null ? element.id : vehicleInfo.style.First().id);
            }

            CarInfoFormViewModel viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyleForEdit(vehicleInfo, styleInfo);

            viewModel.Vin = row.VINNumber ?? string.Empty;

            viewModel.Make = row.Make ?? string.Empty;

            viewModel.ModelYear = row.ModelYear.GetValueOrDefault();

            viewModel.Model = row.Model ?? string.Empty;

            viewModel.Litters = row.Liters ?? string.Empty;

            viewModel.Fuel = row.FuelType ?? string.Empty;

            viewModel.Engine = row.EngineType ?? string.Empty;

            viewModel.Trim = row.Trim ?? string.Empty;

            viewModel.ChromeStyleId = row.ChromeStyleId;

            viewModel.ChromeModelId = row.ChromeModelId;

            viewModel.ACar = row.ACar.GetValueOrDefault();


            viewModel.BrandedTitle = row.BrandedTitle.GetValueOrDefault();

            //get all the style and set the selected style
            int styleId;
            if (viewModel.ChromeStyleId != null && Int32.TryParse(viewModel.ChromeStyleId, out styleId))
            {
                bool existed;
                if (vehicleInfo.bestMakeName.Equals("Mercedes-Benz") && vehicleInfo.modelYear <= 2009)
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimListForMercedesBenz(viewModel, viewModel.Trim, vehicleInfo.style, styleId, out existed);
                }
                else
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, viewModel.Trim, vehicleInfo.style, styleId, out existed);
                }
                
                if (!existed)
                {
                    viewModel.CusTrim = viewModel.Trim;
                }
            }
            else if (!String.IsNullOrEmpty(viewModel.Trim))
            {
                bool existed;
                if (vehicleInfo.bestMakeName.Equals("Mercedes-Benz") && vehicleInfo.modelYear <= 2009)
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimListForMercedesBenz(viewModel, vehicleInfo.style, viewModel.Trim, out existed);
                }
                else
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, vehicleInfo.style, viewModel.Trim, out existed);
                }
             
                if (!existed)
                {
                    viewModel.CusTrim = viewModel.Trim;
                }
            }
            else
            {
                viewModel.EditTrimList = SelectListHelper.InitalTrimList(vehicleInfo.style);
            }

            if (String.IsNullOrEmpty(row.Doors))

                viewModel.Door = 0;
            else
            {
                int numberofdoor;
                Int32.TryParse(row.Doors, out numberofdoor);
                viewModel.Door = numberofdoor;
            }

            viewModel.Title = row.AdditionalTitle ?? string.Empty;

            viewModel.Cylinder = row.Cylinders ?? string.Empty;

            viewModel.Tranmission = row.Tranmission ?? string.Empty;

            viewModel.IsCertified = row.Certified.GetValueOrDefault();

            viewModel.PriorRental = row.PriorRental.GetValueOrDefault();

            viewModel.DealerDemo = row.DealerDemo.GetValueOrDefault();

            viewModel.Unwind = row.Unwind.GetValueOrDefault();

            viewModel.WheelDrive = row.DriveTrain ?? string.Empty;

            viewModel.RetailPrice = row.RetailPrice ?? string.Empty;

            viewModel.DealerDiscount = row.DealerDiscount ?? string.Empty;

            viewModel.ManufacturerRebate = row.ManufacturerRebate ?? string.Empty;

            viewModel.WindowStickerPrice = row.WindowStickerPrice ?? string.Empty;

            viewModel.ChromeFactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

            viewModel.ChromeFactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

            viewModel.WarrantyInfo = row.WarrantyInfo == null ? 0 : row.WarrantyInfo.GetValueOrDefault();

            if (String.IsNullOrEmpty(row.SalePrice))
                viewModel.SalePrice = "NA";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.SalePrice, out priceFormat);
                if (flag)
                    viewModel.SalePrice = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.DealerCost))
                viewModel.DealerCost = "NA";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.DealerCost, out priceFormat);
                if (flag)
                    viewModel.DealerCost = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.ACV))
                viewModel.ACV = "NA";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.ACV, out priceFormat);
                if (flag)
                    viewModel.ACV = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.RetailPrice))
                viewModel.RetailPrice = "";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.RetailPrice, out priceFormat);
                if (flag)
                    viewModel.RetailPrice = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.DealerDiscount))
                viewModel.DealerDiscount = "";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.DealerDiscount, out priceFormat);
                if (flag)
                    viewModel.DealerDiscount = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.WindowStickerPrice))
                viewModel.WindowStickerPrice = "";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.WindowStickerPrice, out priceFormat);
                if (flag)
                    viewModel.WindowStickerPrice = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.ManufacturerRebate))
                viewModel.ManufacturerRebate = "";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.ManufacturerRebate, out priceFormat);
                if (flag)
                    viewModel.ManufacturerRebate = priceFormat.ToString("#,##0");
            }

            viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

            if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                viewModel.OrginalName += " " + viewModel.Trim;

            viewModel.Mileage = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;

            viewModel.DefaultImageUrl = row.DefaultImageUrl;

            viewModel.Description = String.IsNullOrEmpty(row.Descriptions) ? "" : row.Descriptions;

            if (String.IsNullOrEmpty(row.CarImageUrl))
                viewModel.SinglePhoto = row.DefaultImageUrl;
            else
            {
                string[] totalImages = row.CarImageUrl.Split(new String[] { "|", "," }, StringSplitOptions.RemoveEmptyEntries);
                viewModel.SinglePhoto = totalImages[0];
            }

            viewModel.ListingId = Convert.ToInt32(listingId);

            viewModel.DealershipId = dealer.DealershipId;

            viewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber) ? "" : row.StockNumber;

            viewModel.Make = String.IsNullOrEmpty(row.Make) ? "" : row.Make;

            viewModel.VehicleModel = String.IsNullOrEmpty(row.Model) ? "" : row.Model;

            viewModel.SelectedExteriorColorCode = String.IsNullOrEmpty(row.ColorCode) ? "" : row.ColorCode;

            viewModel.SelectedExteriorColorValue = String.IsNullOrEmpty(row.ExteriorColor) ? "" : row.ExteriorColor;

            viewModel.SelectedInteriorColor = String.IsNullOrEmpty(row.InteriorColor) ? "" : row.InteriorColor;

            viewModel.ChromeExteriorColorList = viewModel.ExteriorColorList != null && viewModel.ExteriorColorList.Any()
                                                    ? SelectListHelper.InitalExteriorColorList(viewModel.ExteriorColorList.ToArray(), viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim())
                                                    : SelectListHelper.InitalExteriorColorList(null, viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

            if (viewModel.ChromeExteriorColorList.Any(x=>x.Selected))
            {
                viewModel.SelectedExteriorColorCode = viewModel.ChromeExteriorColorList.First(x => x.Selected).Value;
            }


            viewModel.ChromeInteriorColorList = viewModel.InteriorColorList != null && viewModel.InteriorColorList.Any()
                                                      ? SelectListHelper.InitalInteriorColorList(viewModel.InteriorColorList.ToArray(), viewModel.SelectedInteriorColor)
                                                      : SelectListHelper.InitalInteriorColorList(null, viewModel.SelectedInteriorColor);

            if (!String.IsNullOrEmpty(viewModel.SelectedExteriorColorCode))
            {
                if (viewModel.ExteriorColorList != null && viewModel.ExteriorColorList.Any())
                {
                    var list = viewModel.ExteriorColorList.Where(t => t.colorName.Equals(viewModel.SelectedExteriorColorValue.Trim()));

                    if (!list.Any())
                    {
                        viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;
                    }
                    else
                        viewModel.CusExteriorColor = string.Empty;
                }
                else
                    viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;
            }
            else
            {
                viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;
            }

            if (viewModel.InteriorColorList != null && viewModel.InteriorColorList.Any())
            {
                var list = viewModel.InteriorColorList.Where(t => t.colorName.Equals(viewModel.SelectedInteriorColor));

                if (!list.Any())
                {
                    viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;

                }
                else
                    viewModel.CusInteriorColor = string.Empty;
            }
            else
                viewModel.CusInteriorColor = row.InteriorColor ?? string.Empty;

            viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

            viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

            viewModel.ExistOptions = String.IsNullOrEmpty(row.CarsOptions)
                                         ? null
                                         : (from data in row.CarsOptions.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            viewModel.ExistPackages = String.IsNullOrEmpty(row.CarsPackages)
                                          ? null
                                          : (from data in row.CarsPackages.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            viewModel.UploadPhotosURL = String.IsNullOrEmpty(row.ThumbnailImageURL)
                                            ? null
                                            : (from data in row.ThumbnailImageURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            viewModel.CarImageUrl = row.CarImageUrl ?? string.Empty;

            bool MSRPFlag;
            if (!String.IsNullOrEmpty(row.MSRP))
            {
                double msrpFormat;
                MSRPFlag = Double.TryParse(row.MSRP, out msrpFormat);
                if (MSRPFlag)
                    viewModel.MSRP = msrpFormat.ToString("#,##0");
            }

            viewModel.IsManual = row.EnableAutoDescription == null ? false : !row.EnableAutoDescription.GetValueOrDefault();
            viewModel.EnableAutoDescriptionSetting = setting.AutoDescription.GetValueOrDefault();
            viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
            viewModel.WarrantyTypes = LinqHelper.GetWarrantyTypeList(SessionHandler.Dealership);
            viewModel.DateInStock = row.DateInStock;
            return viewModel;
        }



        public ActionResult EditIProfileForTruck(int ListingID)
        {
            ResetSessionValue();
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();

                var dealer = (DealershipViewModel)Session["Dealership"];

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingID);

                var setting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

                var autoService = new ChromeAutoService();

                var vehicleInfo = autoService.GetVehicleInformationFromVin(row.VINNumber);
                if (vehicleInfo != null)
                {
                    var viewModel = InitCarInfoDataWhenVehicleNotNull(ListingID, dealer, row, setting, autoService, vehicleInfo);
                    if (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful ||
                       vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.ConditionallySuccessful)
                        viewModel.VinDecodeSuccess = true;
                    InitTruckInfo(row, viewModel);
                    return View("EditProfileForTruck", viewModel);
                }
                else
                {
                    var viewModel = InitCarInfoDataWhenVehicleIsNull(ListingID, dealer, row, setting, autoService);
                    if (String.IsNullOrEmpty(row.Doors))
                        viewModel.Door = 0;
                    else
                    {
                        int numberofdoor = 0;
                        Int32.TryParse(row.Doors, out numberofdoor);
                        viewModel.Door = numberofdoor;
                    }
                    InitTruckInfo(row, viewModel);
                    return View("EditProfileForTruck", viewModel);
                }

            }
        }

        private static void InitTruckInfo(whitmanenterprisedealershipinventory row, CarInfoFormViewModel viewModel)
        {
            viewModel.SelectedTruckType = row.TruckType;

            viewModel.SelectedTruckClass = row.TruckClass;

            viewModel.SelectedTruckCategory = row.TruckCategory;

            viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

            viewModel.TruckCategoryList =
                SelectListHelper.InitalTruckCategoryList(
                    SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

            viewModel.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);


            viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveIProfile(CarInfoFormViewModel car)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            car.Trim = car.SelectedTrimItem;

            var dealer = (DealershipViewModel)Session["Dealership"];

            string additionalPackages = "";

            string additionalOptions = "";

            if (!String.IsNullOrEmpty(car.AfterSelectedPackage))

                additionalPackages = car.AfterSelectedPackage.Substring(0, car.AfterSelectedPackage.Length - 1);

            if (!String.IsNullOrEmpty(car.AfterSelectedOptions))

                additionalOptions = car.AfterSelectedOptions.Substring(0, car.AfterSelectedOptions.Length - 1);

            
            string finalMSRP = CommonHelper.RemoveSpecialCharactersForMsrp(car.MSRP);

            string RetailPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(car.RetailPrice);

            string DiscountPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(car.DealerDiscount);

            string ManufacturerRebate = CommonHelper.RemoveSpecialCharactersForPurePrice(car.ManufacturerRebate);

            int WindowStickerPrice = 0;

            int RetailPriceNumber = 0;

            int DiscountPriceNumber = 0;

            bool flagRetail = Int32.TryParse(RetailPrice, out RetailPriceNumber);

            bool flagDiscount = Int32.TryParse(DiscountPrice, out DiscountPriceNumber);

            if (flagRetail && flagDiscount)

                WindowStickerPrice = RetailPriceNumber - DiscountPriceNumber;

            if ((!String.IsNullOrEmpty(car.SelectedExteriorColorValue) && car.SelectedExteriorColorValue.Trim().Equals("Other Colors")) || (!String.IsNullOrEmpty(car.SelectedExteriorColorCode) && car.SelectedExteriorColorCode.Trim().Equals("Other Colors")))
                car.SelectedExteriorColorValue = car.CusExteriorColor;

        
            if (car.SelectedInteriorColor.Equals("Other Colors"))
                car.SelectedInteriorColor = car.CusInteriorColor;

            SQLHelper.UpdateIProfile(car.ListingId, car.Vin, car.StockNumber, CommonHelper.RemoveSpecialCharactersForPurePrice(car.ModelYear.ToString()), car.Make, car.VehicleModel, car.SelectedExteriorColorValue, car.SelectedInteriorColor, car.Trim, CommonHelper.RemoveSpecialCharactersForPurePrice(car.Mileage), car.SelectedTranmission, car.Cylinder, car.Litters, car.Door.ToString(), car.BodyType, car.Fuel
                , car.SelectedDriveTrain, car.Description, additionalPackages, additionalOptions, finalMSRP, car.IsCertified, RetailPrice, DiscountPrice, ManufacturerRebate, WindowStickerPrice.ToString(), dealer, car.SelectedExteriorColorCode, car.Title, car.ChromeStyleId, car.CusTrim, car.SelectedPackagesDescription, car.ACar,car.BrandedTitle);

          
            return RedirectToAction("ViewIProfile", new { ListingID = car.ListingId });

        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveITruckProfile(CarInfoFormViewModel car)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            car.Trim = car.SelectedTrimItem;
            var dealer = (DealershipViewModel)Session["Dealership"];

            string additionalPackages = "";

            string additionalOptions = "";

            if (!String.IsNullOrEmpty(car.AfterSelectedPackage))

                additionalPackages = car.AfterSelectedPackage.Substring(0, car.AfterSelectedPackage.Length - 1);

            if (!String.IsNullOrEmpty(car.AfterSelectedOptions))

                additionalOptions = car.AfterSelectedOptions.Substring(0, car.AfterSelectedOptions.Length - 1);

            string finalMSRP = CommonHelper.RemoveSpecialCharactersForMsrp(car.MSRP);

            string RetailPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(car.RetailPrice);

            string DiscountPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(car.DealerDiscount);

            string ManufacturerRebate = CommonHelper.RemoveSpecialCharactersForPurePrice(car.ManufacturerRebate);

            int WindowStickerPrice = 0;

            int RetailPriceNumber = 0;

            int DiscountPriceNumber = 0;

            bool flagRetail = Int32.TryParse(RetailPrice, out RetailPriceNumber);

            bool flagDiscount = Int32.TryParse(DiscountPrice, out DiscountPriceNumber);

            if (flagRetail && flagDiscount)

                WindowStickerPrice = RetailPriceNumber - DiscountPriceNumber;

            if ((!String.IsNullOrEmpty(car.SelectedExteriorColorValue) && car.SelectedExteriorColorValue.Trim().Equals("Other Colors")) || (!String.IsNullOrEmpty(car.SelectedExteriorColorCode) && car.SelectedExteriorColorCode.Trim().Equals("Other Colors")))
                car.SelectedExteriorColorValue = car.CusExteriorColor;
            if (car.SelectedInteriorColor.Equals("Other Colors"))
                car.SelectedInteriorColor = car.CusInteriorColor;
            //if (car.Trim.Equals("Other Trims"))
            //    car.Trim = "";
            SQLHelper.UpdateITruckProfile(car.ListingId.ToString(), car.Vin, car.StockNumber, CommonHelper.RemoveSpecialCharactersForPurePrice(car.ModelYear.ToString()), car.Make, car.VehicleModel, car.SelectedExteriorColorValue, car.SelectedInteriorColor, car.Trim, CommonHelper.RemoveSpecialCharactersForPurePrice(car.Mileage), car.SelectedTranmission, car.Cylinder, car.Litters, car.Door.ToString(), car.BodyType, car.Fuel, car.SelectedDriveTrain, car.Description, additionalPackages, additionalOptions, finalMSRP, car.IsCertified, car.SelectedTruckType, car.SelectedTruckClass, car.SelectedTruckCategory, RetailPrice, DiscountPrice, ManufacturerRebate, WindowStickerPrice.ToString(), dealer, car.SelectedExteriorColorCode, car.Title, car.ChromeStyleId, car.CusTrim, car.SelectedPackagesDescription, car.ACar,car.BrandedTitle);

            //return RedirectToAction("ViewInventory");
            return RedirectToAction("ViewIProfile", new { ListingID = car.ListingId });

        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CancelIProfile(CarInfoFormViewModel car)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            return RedirectToAction("ViewInventory");

        }

     
        public ActionResult ViewIProfileByVinInDistance(string distance)
        {
            var viewModel = new CarInfoFormViewModel();

            var dealer = (DealershipViewModel)Session["Dealership"];

            string sessionUniqueName = dealer.DealershipId + "SessionDistance100";

            viewModel = (CarInfoFormViewModel)Session[sessionUniqueName];

            viewModel.CurrentDistance = distance;

            return View("iProfile", viewModel);
        }

        public void UpdateAutoDescriptionStatus(int listingId, bool status)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var inventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(i => i.ListingID == listingId);
                if (inventory != null)
                {
                    inventory.EnableAutoDescription = status;
                    context.SaveChanges();
                }
            }
        }

        public string GenerateAutoDescription(int listingId)
        {
            var autoDescription = new AutoDescription();
            var dealer = (DealershipViewModel)Session["Dealership"];

            var context = new whitmanenterprisewarehouseEntities();

            var inventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(i => i.ListingID == listingId);

            if (autoDescription.AllowAutoDescription(inventory,  dealer.DealershipId))
            {
                var result = autoDescription.GenerateAutoDescription(inventory);
                return result;

            }

            return "Failed";
        }

        public ActionResult SaveDescription(int ListingId, string Description, string optionSelect, string StartEnd)
        {
            if (Session["Dealership"] == null)
            {
                return Json("SessionTimeOut");

            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var finalDescription = "";

                if (!string.IsNullOrEmpty(StartEnd))
                {
                    var context = new whitmanenterprisewarehouseEntities();

                    var row = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

                    var startSentence = String.IsNullOrEmpty(row.StartDescriptionSentence)
                                               ? ""
                                               : row.StartDescriptionSentence;

                    var endSentence = String.IsNullOrEmpty(row.EndDescriptionSentence)
                                               ? ""
                                               : row.EndDescriptionSentence;

                    if (StartEnd.Equals("Start"))
                    {
                        finalDescription = row.StartDescriptionSentence + Description;



                        if (!string.IsNullOrEmpty(optionSelect))
                        {

                            finalDescription = startSentence + " " + Description + " " +
                                               "This vehicle is equipped with " +
                                               optionSelect.Substring(0, optionSelect.Length - 1) + ".";

                        }

                    }
                    else if (StartEnd.Equals("End"))
                    {
                        finalDescription = Description + row.EndDescriptionSentence;

                        if (!string.IsNullOrEmpty(optionSelect))
                        {

                            finalDescription = Description + " " +
                                               "This vehicle is equipped with " +
                                               optionSelect.Substring(0, optionSelect.Length - 1) + ". " + endSentence;

                        }
                    }
                    else if (StartEnd.Equals("StartEnd"))
                    {
                        finalDescription = row.StartDescriptionSentence + Description + row.EndDescriptionSentence;

                        if (!string.IsNullOrEmpty(optionSelect))
                        {

                            finalDescription = startSentence + " " + Description + " " +
                                               "This vehicle is equipped with " +
                                               optionSelect.Substring(0, optionSelect.Length - 1) + ". " + endSentence;

                        }
                    }
                }
                else
                {
                    finalDescription = Description;

                    if (!string.IsNullOrEmpty(optionSelect))
                    {

                        finalDescription = Description +
                                           "This vehicle is equipped with " +
                                           optionSelect.Substring(0, optionSelect.Length - 1) + ".";

                    }
                }




                SQLHelper.UpdateDescription(ListingId, finalDescription);


                if (Request.IsAjaxRequest())
                {
                    return Json(finalDescription);

                }

            }
            return Json(ListingId + " NOT UPDATED ");



        }



        public ActionResult GetDealerAuctionDescription()
        {
            if (Session["Dealership"] == null)
            {
                return Json("SessionTimeOut");

            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                if (Request.IsAjaxRequest())
                {
                    return Json(dealer.AuctionSentence);

                }

            }
            return Json(" NOT UPDATED ");



        }


        public ActionResult GetDealerLoanerDescription()
        {
            if (Session["Dealership"] == null)
            {
                return Json("SessionTimeOut");

            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                if (Request.IsAjaxRequest())
                {
                    return Json(dealer.LoanerSentence);

                }

            }
            return Json(" NOT UPDATED ");



        }

        public ActionResult SaveKBBOptions(int ListingId, string optionSelect, int trimId, string BaseWholeSale, string WholeSale, string MileageAdjustment)
        {
            if (Session["Dealership"] == null)
            {
                return Json("SessionTimeOut");

            }
            else
            {
                SQLHelper.UpdateKBBOptions(ListingId, optionSelect, trimId, BaseWholeSale, WholeSale, MileageAdjustment);


                if (Request.IsAjaxRequest())
                {
                    return Json("Success");

                }

            }
            return Json(ListingId + " NOT UPDATED ");



        }

        public ActionResult UpdateSalePrice(int ListingId, string SalePrice)
        {
            SalePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(SalePrice);

            var oldPrice = SQLHelper.UpdateSalePrice(ListingId, SalePrice);

            if (Request.IsAjaxRequest())
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var emailsList = EmailQueryHelpers.GetEmailsForChangePriceNotification(dealer.DealershipId);

                if (!SalePrice.Equals(oldPrice))
                {
                    EmailHelper.SendEmail(emailsList, "Price Change",
                                          EmailHelper.CreateBodyEmailForUpdateSalePrice(dealer.DealershipId,
                                                                                        ListingId,
                                                                                        oldPrice, SalePrice,
                                                                                        User.Identity.Name));

                    LinqHelper.SavePriceChangeHistory(ListingId, "Inventory", Convert.ToDecimal(oldPrice),
                                                      Convert.ToDecimal(SalePrice), string.Empty, User.Identity.Name);
                }

                // Calling AutoDescription
                var autoDescription = new AutoDescription();
                autoDescription.GenerateAutoDescription(ListingId);

                return Json(ListingId + "Success" + oldPrice + "-" + SalePrice);


            }

            return Json(ListingId + " NOT UPDATED " + SalePrice);

        }


        public ActionResult UpdateACV(int ListingId, string acv)
        {

            acv = CommonHelper.RemoveSpecialCharactersForPurePrice(acv);

            string oldAcv = SQLHelper.UpdateACV(ListingId, acv);

            double priceFormat = 0;

            bool flag = Double.TryParse(acv, out priceFormat);

            if (flag)
                acv = priceFormat.ToString("#,##0");

            if (Request.IsAjaxRequest())
            {
                var dealer = (DealershipViewModel)Session["Dealership"];


                var emailsList = EmailQueryHelpers.GetEmailsForChangePriceNotification(dealer.DealershipId);

                if (!acv.Equals(oldAcv))
                {

                    EmailHelper.SendEmail(emailsList, "ACV Change",
                                          EmailHelper.CreateBodyEmailForUpdateSalePrice(dealer.DealershipId,
                                                                                        ListingId,
                                                                                        oldAcv, acv,
                                                                                        User.Identity.Name));

                }

                return Json(ListingId + "Success");


            }

            return Json(ListingId + " NOT UPDATED " + acv);

        }



        public ActionResult UpdateDealerCost(int ListingId, string dealerCost)
        {
            dealerCost = CommonHelper.RemoveSpecialCharactersForPurePrice(dealerCost);

            var oldDealerCost = SQLHelper.UpdateDealerCost(ListingId, dealerCost);

            double priceFormat = 0;

            bool flag = Double.TryParse(dealerCost, out priceFormat);

            if (flag)
                dealerCost = priceFormat.ToString("#,##0");

            if (Request.IsAjaxRequest())
            {
                var dealer = (DealershipViewModel)Session["Dealership"];


                var emailsList = EmailQueryHelpers.GetEmailsForChangePriceNotification(dealer.DealershipId);

                if (!oldDealerCost.Equals(dealerCost))
                {

                    EmailHelper.SendEmail(emailsList, "Dealer Cost Change",
                                          EmailHelper.CreateBodyEmailForUpdateSalePrice(dealer.DealershipId,
                                                                                        ListingId,
                                                                                        oldDealerCost, dealerCost,
                                                                                        User.Identity.Name));
                }


                return Json(ListingId + "Success");


            }
            return Json(ListingId + " NOT UPDATED " + dealerCost);

        }


        public ActionResult UpdateSalePriceFromInventoryPage(string SalePrice, int ListingId)
        {

            SalePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(SalePrice);

            var oldPrice = SQLHelper.UpdateSalePrice(ListingId, SalePrice);

            if (Request.IsAjaxRequest())
            {
                var dealer = (DealershipViewModel)Session["Dealership"];


                var emailsList = EmailQueryHelpers.GetEmailsForChangePriceNotification(dealer.DealershipId);


                try
                {
                    if (!oldPrice.Equals(SalePrice))
                    {
                        EmailHelper.SendEmail(emailsList, "Price Change",
                                              EmailHelper.CreateBodyEmailForUpdateSalePrice(dealer.DealershipId,
                                                                                            ListingId,
                                                                                            oldPrice, SalePrice,
                                                                                            User.Identity.Name));
                        LinqHelper.SavePriceChangeHistory(ListingId, "Inventory", Convert.ToDecimal(oldPrice),
                                                          Convert.ToDecimal(SalePrice), string.Empty, User.Identity.Name);
                    }

                  
                    var autoDescription = new AutoDescription();
                    autoDescription.GenerateAutoDescription(ListingId);

                    return Json(ListingId + "Success");

                }
                catch (Exception ex)
                {

                    //return Json(ListingId + ex.Message +ex.InnerException +ex.TargetSite + ex.StackTrace);
                }






            }
            return Json("Not Updated");
        }

        public ActionResult UpdateReconStatusFromInventoryPage(bool Reconstatus, int ListingId)
        {

            SQLHelper.UpdateReconStatus(ListingId, Reconstatus);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");
            }
            return Json("Not Updated");
        }


        public ActionResult UpdateMileageFromInventoryPage(string Mileage, string ListingId)
        {

            Mileage = CommonHelper.RemoveSpecialCharactersForPurePrice(Mileage);
            SQLHelper.UpdateMileage(ListingId, Mileage);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }


        public ActionResult UpdateCarImageUrl(int ListingId, string CarThubmnailImageURL)
        {
            CarThubmnailImageURL = CarThubmnailImageURL.Replace("|", ",");

            string CarImageURL = CarThubmnailImageURL.Replace("ThumbnailSizeImages", "NormalSizeImages");

            SQLHelper.UpdateCarImageURL(ListingId, CarImageURL, CarThubmnailImageURL);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Image Successful", JsonRequestBehavior.AllowGet);

            }

            return Json("Not Updated");


        }


        public ActionResult UpdateCarImageUrlFromImageSortFrame(ImageViewModel image)
        {
            SQLHelper.UpdateCarImageURLField(image);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Image Successful", JsonRequestBehavior.AllowGet);

            }

            return Json("Not Updated");


        }





        public ActionResult DeleteCarImageURL(int ListingId, string CarThubmnailImageURL)
        {
            try
            {
                string CarImageURL = CarThubmnailImageURL.Replace("ThumbnailSizeImages", "NormalSizeImages");

                SQLHelper.UpdateCarImageURL(ListingId, CarImageURL, CarThubmnailImageURL);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Image Successful");

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message + "****" + CarThubmnailImageURL + "And" + ListingId);
            }

            return Json("Updated But Not In Ajax");

        }

        public ActionResult DeleteCarImageSoldURL(int ListingId, string CarThubmnailImageURL)
        {
            try
            {
                string CarImageURL = CarThubmnailImageURL.Replace("ThumbnailSizeImages", "NormalSizeImages");

                SQLHelper.UpdateCarImageSoldURL(ListingId, CarImageURL, CarThubmnailImageURL);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Image Successful");

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message + "****" + CarThubmnailImageURL + "And" + ListingId);
            }

            return Json("Updated But Not In Ajax");

        }

        public InventoryFormViewModel PrepareDataForSorInventory(string id)
        {
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

            List<CarInfoFormViewModel> listFilter = viewModel.CarsList;

            if (String.IsNullOrEmpty(viewModel.previousCriteria))
                viewModel.previousCriteria = id;

            switch (id)
            {
                case "Year":


                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ModelYear).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;
                    break;
                case "Make":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }



                    viewModel.CarsList = listFilter;

                    break;
                case "Model":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Trim":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Trim).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                        viewModel.sortASCOrder = false;
                    }
                    viewModel.CarsList = listFilter;

                    break;
                case "Stock":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Age":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Miles":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Mileage = 0;
                        bool flag = Decimal.TryParse(car.Mileage, out Mileage);
                        car.MilageDecimal = Mileage;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {

                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;

                    break;
                case "Price":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Price = 0;
                        bool flag = Decimal.TryParse(car.SalePrice, out Price);
                        car.Price = Price;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Price).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;

                default:
                    listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                    viewModel.sortASCOrder = false;
                    viewModel.CarsList = listFilter;

                    break;
            }

            viewModel.previousCriteria = id;

            ViewData["Sort"] = true;
            return viewModel;
        }

        public ActionResult SortExpressBucketJump(string id)
        {
            var viewModel = PrepareDataForSorInventory(id);
            return View("ExpressBucketJump", viewModel);
        }

        public ActionResult SortInventory(string id)
        {
            var viewModel = PrepareDataForSorInventory(id);
            return View("ViewInventory", viewModel);
        }

        public ActionResult SortSmallInventory(string id)
        {
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

            var listFilter = viewModel.CarsList;

            switch (id)
            {
                case "Year":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ModelYear).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;
                    break;
                case "Make":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }



                    viewModel.CarsList = listFilter;

                    break;
                case "Model":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Trim":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Trim).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                        viewModel.sortASCOrder = false;
                    }
                    viewModel.CarsList = listFilter;

                    break;
                case "Stock":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Age":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Miles":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Mileage = 0;
                        bool flag = Decimal.TryParse(car.Mileage, out Mileage);
                        car.MilageDecimal = Mileage;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {

                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;

                    break;
                case "Price":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Price = 0;
                        bool flag = Decimal.TryParse(car.SalePrice, out Price);
                        car.Price = Price;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Price).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Color":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Owners":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                default:
                    listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                    viewModel.sortASCOrder = false;
                    viewModel.CarsList = listFilter;

                    break;
            }

            viewModel.previousCriteria = id;
            ViewData["Sort"] = true;
            return View("ViewSmallInventory", viewModel);
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult MarkUnsold(int ListingId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var searchResult = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == ListingId);

                    int returnId = SQLHelper.MarkUnsoldVehicle(searchResult);

                    return RedirectToAction("ViewIProfile", new { ListingID = returnId });

                }




            }

        }

        public ActionResult SortSmallAuctionInventory(string id)
        {
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

            var listFilter = viewModel.CarsList;

            switch (id)
            {
                case "Year":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ModelYear).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;
                    break;
                case "Make":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }



                    viewModel.CarsList = listFilter;

                    break;
                case "Model":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Trim":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Trim).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                        viewModel.sortASCOrder = false;
                    }
                    viewModel.CarsList = listFilter;

                    break;
                case "Stock":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Age":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Miles":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Mileage = 0;
                        bool flag = Decimal.TryParse(car.Mileage, out Mileage);
                        car.MilageDecimal = Mileage;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {

                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;

                    break;
                case "Price":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Price = 0;
                        bool flag = Decimal.TryParse(car.SalePrice, out Price);
                        car.Price = Price;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Price).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Color":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Owners":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                default:
                    listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                    viewModel.sortASCOrder = false;
                    viewModel.CarsList = listFilter;

                    break;
            }

            viewModel.previousCriteria = id;
            ViewData["Sort"] = true;
            return View("ViewSmallAuctionInventory", viewModel);
        }

        public ActionResult SortSmallLoanerInventory(string id)
        {
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

            var listFilter = viewModel.CarsList;

            switch (id)
            {
                case "Year":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ModelYear).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;
                    break;
                case "Make":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }



                    viewModel.CarsList = listFilter;

                    break;
                case "Model":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Trim":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Trim).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                        viewModel.sortASCOrder = false;
                    }
                    viewModel.CarsList = listFilter;

                    break;
                case "Stock":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Age":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Miles":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Mileage = 0;
                        bool flag = Decimal.TryParse(car.Mileage, out Mileage);
                        car.MilageDecimal = Mileage;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {

                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;

                    break;
                case "Price":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Price = 0;
                        bool flag = Decimal.TryParse(car.SalePrice, out Price);
                        car.Price = Price;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Price).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Color":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Owners":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                default:
                    listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                    viewModel.sortASCOrder = false;
                    viewModel.CarsList = listFilter;

                    break;
            }

            viewModel.previousCriteria = id;
            ViewData["Sort"] = true;
            return View("ViewSmallLoanerInventory", viewModel);
        }


        public ActionResult SortSmallReconInventory(string id)
        {
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

            var listFilter = viewModel.CarsList;

            switch (id)
            {
                case "Year":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ModelYear).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;
                    break;
                case "Make":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }



                    viewModel.CarsList = listFilter;

                    break;
                case "Model":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Trim":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Trim).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                        viewModel.sortASCOrder = false;
                    }
                    viewModel.CarsList = listFilter;

                    break;
                case "Stock":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Age":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Miles":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Mileage = 0;
                        bool flag = Decimal.TryParse(car.Mileage, out Mileage);
                        car.MilageDecimal = Mileage;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {

                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;

                    break;
                case "Price":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Price = 0;
                        bool flag = Decimal.TryParse(car.SalePrice, out Price);
                        car.Price = Price;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Price).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Color":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Owners":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                default:
                    listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                    viewModel.sortASCOrder = false;
                    viewModel.CarsList = listFilter;

                    break;
            }

            viewModel.previousCriteria = id;
            ViewData["Sort"] = true;
            return View("ViewSmallReconInventory", viewModel);
        }


        public ActionResult SortSmallWholeSaleInventory(string id)
        {
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];

            var listFilter = viewModel.CarsList;

            switch (id)
            {
                case "Year":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ModelYear).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;
                    break;
                case "Make":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Make).ThenBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }



                    viewModel.CarsList = listFilter;

                    break;
                case "Model":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Model).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Model).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Model).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Trim":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.Trim).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Trim).ToList();
                        viewModel.sortASCOrder = false;
                    }
                    viewModel.CarsList = listFilter;

                    break;
                case "Stock":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.StockNumber).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.StockNumber).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Age":
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.DaysInInvenotry).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Miles":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Mileage = 0;
                        bool flag = Decimal.TryParse(car.Mileage, out Mileage);
                        car.MilageDecimal = Mileage;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {

                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderByDescending(x => x.MilageDecimal).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.MilageDecimal).ToList();
                        viewModel.sortASCOrder = false;
                    }


                    viewModel.CarsList = listFilter;

                    break;
                case "Price":
                    foreach (CarInfoFormViewModel car in listFilter)
                    {
                        decimal Price = 0;
                        bool flag = Decimal.TryParse(car.SalePrice, out Price);
                        car.Price = Price;
                    }
                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.Price).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.Price).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Color":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.ExteriorColor).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                case "Owners":

                    if (viewModel.previousCriteria.Equals(id))
                    {
                        if (viewModel.sortASCOrder)
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = false;
                        }
                        else
                        {
                            listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                            viewModel.sortASCOrder = true;
                        }
                    }
                    else
                    {
                        listFilter = listFilter.OrderBy(x => x.CarFaxOwner).ToList();
                        viewModel.sortASCOrder = false;
                    }

                    viewModel.CarsList = listFilter;

                    break;
                default:
                    listFilter = listFilter.OrderBy(x => x.DaysInInvenotry).ToList();
                    viewModel.sortASCOrder = false;
                    viewModel.CarsList = listFilter;

                    break;
            }

            viewModel.previousCriteria = id;
            ViewData["Sort"] = true;
            return View("ViewWholeSaleInventory", viewModel);
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult MarkSold(CustomeInfoModel customer)
        {

            var dealer = (DealershipViewModel)Session["Dealership"];

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int LID = Convert.ToInt32(customer.ListingId);

                var searchResult = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

                SQLHelper.MarkSoldVehicle(searchResult, User.Identity.Name, customer);

            }

            if (Request.IsAjaxRequest())
            {
                return Json(dealer.DealershipId);

            }
            return Json(customer.ListingId + " NOT UPDATED " + customer.ListingId);


            //return ViewInventory(dealer.DealershipId.ToString());
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult ViewMarkSold(string ListingId)
        {
            if (Session["Dealership"] != null)
            {
                var viewModel = new CustomeInfoModel()
                {
                    ListingId = ListingId,
                    States = SelectListHelper.InitialStateList(),
                    Countries = SelectListHelper.InitialCountryList()
                };
                return View("MarkSold", viewModel);
            }
            else
            {
                var viewModel = new CustomeInfoModel();
                viewModel.SessionTimeOut = true;
                return Json("SessionTimeOut");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewPriceTracking(string ListingId)
        {
            if (Session["Dealership"] != null)
            {
                return View("PriceTracking",
                            new PriceChangeViewModel()
                                {
                                    Id = ListingId,
                                    PriceChangeHistory =
                                        DataHelper.GetPriceChangeList(ListingId, ChartTimeType.Last7Days, 1),
                                    InventoryStatus = 1
                                });
            }
            else
            {
                return Json("SessionTimeOut");
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewPriceTrackingForSold(string ListingId)
        {
            if (Session["Dealership"] != null)
            {
                return View("PriceTracking",
                            new PriceChangeViewModel()
                                {
                                    Id = ListingId,
                                    PriceChangeHistory =
                                        DataHelper.GetPriceChangeList(ListingId, ChartTimeType.Last7Days, -1),
                                    InventoryStatus = -1
                                });
            }
            else
            {
                return Json("SessionTimeOut");
            }
        }

        public ActionResult PriceTrackingChart(string itemId, ChartTimeType type, int inventoryStatus)
        {
            return PartialView("PriceTrackingChart",
                               new PriceChangeViewModel()
                                   {
                                       PriceChangeHistory = DataHelper.GetPriceChangeList(itemId, type, inventoryStatus),
                                       Id = itemId,
                                       ChartType = type,
                                       InventoryStatus = inventoryStatus
                                   });

        }

        public ActionResult RenderPriceChart(string itemId, ChartTimeType chartTimeType, int inventoryStatus)
        {
            var createDate = DataHelper.GetCreatedDate(itemId, inventoryStatus).Value;

            var chart = PDFController.CreateChart(DataHelper.GetPriceChangeListForChart(itemId, chartTimeType, createDate, inventoryStatus), RenderType.BinaryStreaming, 800, 300, chartTimeType, createDate);
            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms.ToArray(), "image/png", "mychart.png");
            }
        }

        public ActionResult DownloadBucketJumpReport(string name)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                try
                {
                    var bucketJumpReportFile = context.vincontrolbucketjumphistories.FirstOrDefault(i => i.AttachFile.ToLower().Equals(name.ToLower()));
                    string path = System.Web.HttpContext.Current.Server.MapPath("\\BucketJumpReports") + "\\" + SessionHandler.Dealership.DealershipId + "\\" + bucketJumpReportFile.Type + "\\" + bucketJumpReportFile.ListingId;
                    string filename = bucketJumpReportFile.AttachFile;
                    string fullPath = Path.Combine(@path, filename);

                    return File(fullPath, "application/pdf", "TodayBucketJumpReport.pdf");
                }
                catch (Exception)
                {
                    return File(new byte[] { }, "application/pdf");
                }
            }
        }

        public ActionResult PrintPriceTracking(string itemId, string type,int inventoryStatus)
        {
            return RedirectToAction("PrintPriceTracking", "PDF", new { itemId = itemId, type = type, inventoryStatus = inventoryStatus });
        }

        public ActionResult ViewBucketJumpTracking(string ListingId)
        {
            if (Session["Dealership"] != null)
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var convertedListingId = Convert.ToInt32(ListingId);
                    var history = context.vincontrolbucketjumphistories.Where(i => i.ListingId == convertedListingId && i.Type.ToLower().Equals("inventory")).OrderByDescending(i => i.DateStamp).ToList();
                    if (history.Count > 0)
                    {
                        return View("BucketJumpTracking", history.Select(i => new BucketJumpHistory()
                        {
                            AttachFile = i.AttachFile,
                            UserStamp = i.UserStamp,
                            DateStamp = i.DateStamp.GetValueOrDefault(),
                            SalePrice = i.SalePrice.GetValueOrDefault(),
                            RetailPrice = i.RetailPrice.GetValueOrDefault(),
                            Type = i.Type,
                            ListingId = i.ListingId.GetValueOrDefault(),
                            BucketJumpDayAlert = i.BucketJumpDayAlert.GetValueOrDefault(),
                            BucketJumpCompleteDate = i.BucketJumpCompleteDate.GetValueOrDefault()
                        }).ToList());
                    }
                }
                return View("BucketJumpTracking", new List<BucketJumpHistory>());
            }
            else
            {
                var viewModel = new CustomeInfoModel { SessionTimeOut = true };
                return Json("SessionTimeOut");
            }
        }

        public ActionResult ViewBucketJumpTrackingForSold(string ListingId)
        {
            if (Session["Dealership"] != null)
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                     var convertedListingId = Convert.ToInt32(ListingId);
                    var soldCar =
                        context.whitmanenterprisedealershipinventorysoldouts.First(x => x.ListingID == convertedListingId);

                    if (soldCar.OldListingId.GetValueOrDefault() > 0)
                    {
                        
                        var history =
                            context.vincontrolbucketjumphistories.Where(
                                i =>
                                i.ListingId == soldCar.OldListingId &&
                                i.Type.ToLower().Equals("inventory")).OrderByDescending(i => i.DateStamp).ToList();
                        if (history.Count > 0)
                        {
                            return View("BucketJumpTracking", history.Select(i => new BucketJumpHistory()
                                {
                                    AttachFile = i.AttachFile,
                                    UserStamp = i.UserStamp,
                                    DateStamp = i.DateStamp.GetValueOrDefault(),
                                    SalePrice = i.SalePrice.GetValueOrDefault(),
                                    RetailPrice = i.RetailPrice.GetValueOrDefault(),
                                    Type = i.Type,
                                    ListingId = i.ListingId.GetValueOrDefault(),
                                    BucketJumpDayAlert = i.BucketJumpDayAlert.GetValueOrDefault(),
                                    BucketJumpCompleteDate = i.BucketJumpCompleteDate.GetValueOrDefault()
                                }).ToList());
                        }
                    }
                }
                return View("BucketJumpTracking", new List<BucketJumpHistory>());
            }
            else
            {
                var viewModel = new CustomeInfoModel { SessionTimeOut = true };
                return Json("SessionTimeOut");
            }
        }

        public ActionResult SoldOutAlert(int listingID)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var row = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == listingID);

                ViewData["Year"] = String.IsNullOrEmpty(row.ModelYear.Value.ToString()) ? 0 : row.ModelYear;

                ViewData["Make"] = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;

                ViewData["Model"] = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;

                ViewData["Trim"] = String.IsNullOrEmpty(row.Trim) ? "NA" : row.Trim;

                ViewData["DateInStock"] = row.DateInStock.Value;

                ViewData["Vin"] = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;


                ViewData["ListingId"] = listingID;

            }



            return View("SoldOutAlert");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult MarkUnSoldFromVinDecode(FormCollection form)
        {
            string LisitngId = form["ListingId"];
            int LID = Convert.ToInt32(LisitngId);

            var context = new whitmanenterprisewarehouseEntities();
            var searchResult =
                context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == LID);

            int autoId = SQLHelper.MarkUnsoldVehicle(searchResult);


            return ViewIProfile(autoId);
        }




        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewAppraisal(FormCollection form)
        {
            string vin = form["Vin"];
            return RedirectToAction("DecodeProcessingByVin", "Decode", new { Vin = vin });
        }



        public ActionResult UpdateIProfile(string ListingId, string Vin, string StockNumber, string ModelYear, string Make, string Model, string ExteriorColor, string InteriorColor, string Trim, string Mileage, string Tranmission, string Cylinders, string Liters, string Doors, string Style, string Fuel, string Drive, string Description, string Options, string MSRP, bool Certified, bool PriorRental, string RetailPrice, string DiscountPrice, string ManufacturerRebate)
        {
            string finalMSRP = CommonHelper.RemoveSpecialCharactersForMsrp(MSRP);

            RetailPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(RetailPrice);

            DiscountPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(DiscountPrice);

            ManufacturerRebate = CommonHelper.RemoveSpecialCharactersForPurePrice(ManufacturerRebate);

            int WindowStickerPrice = 0;

            int RetailPriceNumber = 0;

            int DiscountPriceNumber = 0;

            bool flagRetail = Int32.TryParse(RetailPrice, out RetailPriceNumber);

            bool flagDiscount = Int32.TryParse(DiscountPrice, out DiscountPriceNumber);

            if (flagRetail && flagDiscount)

                WindowStickerPrice = RetailPriceNumber - DiscountPriceNumber;

            //SQLHelper.UpdateIProfile(ListingId, Vin, StockNumber, ModelYear, Make, Model, ExteriorColor, InteriorColor, Trim, Mileage, Tranmission, Cylinders, Liters, Doors, Style, Fuel, Drive, Description, Options, finalMSRP, Certified,PriorRental, RetailPrice, DiscountPrice, ManufacturerRebate, WindowStickerPrice.ToString());

            if (Request.IsAjaxRequest())
            {
                return Json(Model);

            }

            return Json(ListingId + " NOT UPDATED ");

        }
        //public ActionResult UpdateITruckProfile(string ListingId, string Vin, string StockNumber, string ModelYear, string Make, string Model, string ExteriorColor, string InteriorColor, string Trim, string Mileage, string Tranmission, string Cylinders, string Liters, string Doors, string Style, string Fuel, string Drive, string Description, string Options, string MSRP, bool Certified, bool PriorRental, string TruckType, string TruckClass, string TruckCategory, string RetailPrice, string DiscountPrice, string ManufacturerRebate)
        //{
        //    string finalMSRP = CommonHelper.RemoveSpecialCharactersForMSRP(MSRP);

        //    RetailPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(RetailPrice);

        //    DiscountPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(DiscountPrice);

        //    ManufacturerRebate = CommonHelper.RemoveSpecialCharactersForPurePrice(ManufacturerRebate);

        //    int WindowStickerPrice = 0;

        //    int RetailPriceNumber = 0;

        //    int DiscountPriceNumber = 0;

        //    bool flagRetail = Int32.TryParse(RetailPrice, out RetailPriceNumber);

        //    bool flagDiscount = Int32.TryParse(DiscountPrice, out DiscountPriceNumber);

        //    if (flagRetail && flagDiscount)

        //        WindowStickerPrice = RetailPriceNumber - DiscountPriceNumber;

        //    SQLHelper.UpdateITruckProfile(ListingId, Vin, StockNumber, ModelYear, Make, Model, ExteriorColor, InteriorColor, Trim, Mileage, Tranmission, Cylinders, Liters, Doors, Style, Fuel, Drive, Description, Options, finalMSRP, Certified,PriorRental, TruckType, TruckClass, TruckCategory, RetailPrice, DiscountPrice, ManufacturerRebate, WindowStickerPrice.ToString());

        //    if (Request.IsAjaxRequest())
        //    {
        //        return Json(Model);

        //    }

        //    return Json(ListingId + " NOT UPDATED ");
        //    //return Json("Not Updated");
        //}

        public ActionResult OpenSilverlightUploadWindow(int ListingId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);
                    var viewModel = new SilverlightImageViewModel()
                    {
                        ListingId = ListingId,
                        Vin = String.IsNullOrEmpty(row.VINNumber) ? "" : row.VINNumber,
                        DealerId = dealer.DealershipId,
                        InventoryStatus = 1,
                        ImageServiceURL = (System.Web.HttpContext.Current.Request.Url.Port == 80) ? String.Format("{0}://{1}/ImageHandlers/SilverlightHandler.ashx", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host) : String.Format("{0}://{1}:{2}/ImageHandlers/SilverlightHandler.ashx", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port)
                    };

                    return View("imageSilverlightSortFrame", viewModel);
                }
            }
            else
            {
                var viewModel = new SilverlightImageViewModel();
                viewModel.SessionTimeOut = true;
                return View("imageSilverlightSortFrame", viewModel);
            }
        }

        public ActionResult OpenUploadWindow(int ListingId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                using (var context = new whitmanenterprisewarehouseEntities())
                {


                    var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);


                    var viewModel = new ImageViewModel()
                    {
                        ListingId = ListingId,
                        Vin = String.IsNullOrEmpty(row.VINNumber) ? "" : row.VINNumber,
                        DealerId = dealer.DealershipId,
                        CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl,
                        InventoryStatus = 1
                    };

                    return View("imageSortFrame", viewModel);
                }


            }
            else
            {
                var viewModel = new ImageViewModel();
                viewModel.SessionTimeOut = true;
                return View("imageSortFrame", viewModel);
            }
        }

       

      

        public ActionResult OpenUploadWindowForSold(int ListingId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                using (var context = new whitmanenterprisewarehouseEntities())
                {


                    var row = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == ListingId);


                    var viewModel = new ImageViewModel()
                    {
                        ListingId = ListingId,
                        Vin = String.IsNullOrEmpty(row.VINNumber) ? "" : row.VINNumber,
                        DealerId = dealer.DealershipId,
                        CarImageUrl = String.IsNullOrEmpty(row.CarImageUrl) ? "" : row.CarImageUrl,
                        InventoryStatus = -1
                    };

                    return View("imageSortFrame", viewModel);
                }


            }
            else
            {
                var viewModel = new ImageViewModel();
                viewModel.SessionTimeOut = true;
                return View("imageSortFrame", viewModel);
            }
        }


        public ActionResult OpenVehicleTransferWindow(int ListingId)
        {
            if (Session["Dealership"] != null && Session["DealerGroup"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var vehicle = new CarInfoFormViewModel();

                var context = new whitmanenterprisewarehouseEntities();

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

                var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                vehicle.TransferDealerGroup = SelectListHelper.InitialDealerListExtract(dealerGroup, dealer.DealershipId);

                vehicle.ModelYear = row.ModelYear.GetValueOrDefault();

                vehicle.Make = row.Make;

                vehicle.Model = row.Model;

                vehicle.Trim = row.Trim;
             
                vehicle.StockNumber = row.StockNumber;

                vehicle.ListingId = row.ListingID;

                vehicle.DealershipName = dealer.DealershipName;

                return View("VehicleTransfer", vehicle);
            }
            else
            {
                var viewModel = new CarInfoFormViewModel {SessionTimeOut = true};
                return View("VehicleTransfer", viewModel);
            }
        }

        public ActionResult TransferVehicle(CarInfoFormViewModel car)
        {
            var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];
            if (!String.IsNullOrEmpty(car.StockNumber.Trim()))
            {
                if (!SQLHelper.CheckStockNumberExist(car.StockNumber, Convert.ToInt32(car.SelectedDealerTransfer)))
                {
                    SQLHelper.TransferVehicle(car.ListingId, Convert.ToInt32(car.SelectedDealerTransfer), car.StockNumber, dealerGroup);
                    return Json("Success");
                }
                else
                {
                    return Json("DuplicateStock");
                }
            }
            else
            {
                SQLHelper.TransferVehicle(car.ListingId, Convert.ToInt32(car.SelectedDealerTransfer), car.StockNumber, dealerGroup);
                return Json("Success");
            }
        }

        public ActionResult UpdateWarrantyInfo(int WarrantyInfo, int ListingId)
        {

            if (Session["Dealership"] != null)
            {

                SQLHelper.UpdateWarrantyInfo(WarrantyInfo, ListingId);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }

        }

        public ActionResult PriorRentalUpdate(bool PriorRental, int ListingId)
        {

            if (Session["Dealership"] != null)
            {

                SQLHelper.UpdatePriorRental(PriorRental, ListingId);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }

        }
        public ActionResult DealerDemoUpdate(bool DealerDemo, int ListingId)
        {

            if (Session["Dealership"] != null)
            {

                SQLHelper.UpdateDealerDemo(DealerDemo, ListingId);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }

        }

        public ActionResult UnwindUpdate(bool Unwind, int ListingId)
        {

            if (Session["Dealership"] != null)
            {

                SQLHelper.UpdateUnwind(Unwind, ListingId);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            else
            {
                var user = new UserRoleViewModel()
                {
                    SessionTimeOut = "TimeOut"
                };
                return Json(user);
            }

        }

        [HttpPost]
        public string UpdateIsFeatured(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var vehicle = context.whitmanenterprisedealershipinventories.Where(i => i.ListingID == listingId).FirstOrDefault();
                    if (vehicle != null)
                    {
                        if (vehicle.IsFeatured.HasValue && vehicle.IsFeatured.Value)
                            vehicle.IsFeatured = false;
                        else
                            vehicle.IsFeatured = true;
                        context.SaveChanges();

                        return "Updated successfully.";
                    }
                    else
                    {
                        var soldvehicle =
                            context.whitmanenterprisedealershipinventorysoldouts.Where(i => i.ListingID == listingId)
                                   .FirstOrDefault();

                        if (soldvehicle != null)
                        {
                            if (soldvehicle.IsFeatured.HasValue && soldvehicle.IsFeatured.Value)
                                soldvehicle.IsFeatured = false;
                            else
                                soldvehicle.IsFeatured = true;
                            context.SaveChanges();

                            return "Updated successfully.";
                        }
                    }
                
                        return "This vehicle doesn't exist in inventory.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public ActionResult GetCityAndStateFromZip(string zipCode)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int result;
                if (int.TryParse(zipCode, out result))
                {
                    var zip = context.whitmanenterpriseusazipcodes.FirstOrDefault(z => z.ZIPCode == result);
                    if (zip != null)
                    {
                        var returnValue = Json(new { zip.CityName, zip.StateName, zip.StateAbbr }, JsonRequestBehavior.AllowGet);
                        return returnValue;
                    }
                }
            }
            return Json(new { CityName = String.Empty, StateName = String.Empty, StateAbbr = String.Empty });
        }

        public int DoneTodayBucketJump(int listingId, string day)
        {
            if (Session["Dealership"] == null)
            {
                return 0;
            }

            var dealer = (DealershipViewModel)Session["Dealership"];

            var convertedId = Convert.ToInt32(listingId);
            var convertedDay = Convert.ToInt32(day);

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var inventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(i => i.ListingID == convertedId);
                if (inventory != null)
                {

                    int remain = convertedDay % dealer.IntervalBucketJump;

                    inventory.BucketJumpCompleteDay = convertedDay - remain;

                    context.SaveChanges();

                    var result =
                        from e in context.whitmanenterpriseusersnotifications
                        from et in context.whitmanenterpriseusers
                        where
                            e.DealershipId == dealer.DealershipId && (e.BucketJumpNotification != null && e.BucketJumpNotification.Value) &&
                            e.UserName == et.UserName && (et.Active != null && et.Active.Value)
                        select new
                        {
                            et.Name,
                            et.UserName,
                            et.Password,
                            et.Email,
                            et.Cellphone,
                            et.RoleName,
                            e.PriceChangeNotification
                        };

                    var bucketJumpReportFile = context.vincontrolbucketjumphistories.OrderByDescending(i => i.DateStamp).FirstOrDefault(i => i.ListingId == listingId && i.Type == "Inventory");
                    if (Session["CanViewBucketJumpReport"] != null && (bool)Session["CanViewBucketJumpReport"] && bucketJumpReportFile != null)
                    {
                        bucketJumpReportFile.BucketJumpDayAlert = convertedDay - remain;
                        bucketJumpReportFile.BucketJumpCompleteDate = DateTime.Now;
                        context.SaveChanges();

                        string path = System.Web.HttpContext.Current.Server.MapPath("\\BucketJumpReports") + "\\" + dealer.DealershipId + "\\" + bucketJumpReportFile.Type + "\\" + listingId;
                        string filename = bucketJumpReportFile.AttachFile;
                        string fullPath = Path.Combine(@path, filename);
                        var temp = new FileInfo(fullPath);
                        if (temp.Exists)
                        {
                            try
                            {
                                EmailHelper.SendEmail(result.Select(x => x.Email), "Today Bucket Jump", EmailHelper.CreateBodyEmailForBucketJumpAlert(dealer, inventory), fullPath);
                            }
                            catch (Exception)
                            {
                                EmailHelper.SendEmail(result.Select(x => x.Email), "Today Bucket Jump", EmailHelper.CreateBodyEmailForBucketJumpAlert(dealer, inventory));
                            }

                        }
                    }
                    else
                    {
                        EmailHelper.SendEmail(result.Select(x => x.Email), "Today Bucket Jump", EmailHelper.CreateBodyEmailForBucketJumpAlert(dealer, inventory));
                    }
                }
            }

            if (Session["InventoryObject"] != null)
            {
                var sessionInventory = (InventoryFormViewModel)Session["InventoryObject"];

                int returnResult = sessionInventory.CarsList.Count - 1;

                var lookupCar = sessionInventory.CarsList.FindIndex(x => x.ListingId == listingId);

                sessionInventory.CarsList.RemoveAt(lookupCar);

                Session["InventoryObject"] = sessionInventory;

                return returnResult;
            }

            return 0;
        }

        #region Helpers

        private ActionResult HandleInventoryView(RetrieveInventoryFunction RetrieveInventory)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                return RetrieveInventory((DealershipViewModel)Session["Dealership"], new whitmanenterprisewarehouseEntities());
            }
        }

        delegate ActionResult RetrieveInventoryFunction(DealershipViewModel dealer, whitmanenterprisewarehouseEntities context);

        private ActionResult GetInventory(IQueryable<whitmanenterprisedealershipinventory> avaiInventory, DealershipViewModel dealer, string viewName, bool isCompactView)
        {
            var viewModel = new InventoryFormViewModel { IsCompactView = isCompactView, CombineInventory = isCompactView };

            if (Session["DealerGroup"] != null)
            {
                viewModel.DealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
            }
            else
                viewModel.DealerList = SelectListHelper.InitialDealerList();

            var list = new List<CarInfoFormViewModel>();

            foreach (var tmp in avaiInventory)
            {
                var car = new CarInfoFormViewModel()
                {
                    ListingId = tmp.ListingID,
                    ModelYear = tmp.ModelYear.Value,
                    StockNumber = tmp.StockNumber,
                    Model = tmp.Model,
                    Make = tmp.Make,
                    Mileage = tmp.Mileage,
                    Trim = tmp.Trim,
                    ChromeStyleId = tmp.ChromeStyleId,
                    ChromeModelId = tmp.ChromeModelId,
                    Vin = tmp.VINNumber,
                    ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                    InteriorColor = String.IsNullOrEmpty(tmp.InteriorColor) ? "" : tmp.InteriorColor,
                    HasImage = !String.IsNullOrEmpty(tmp.CarImageUrl),
                    HasDescription = !String.IsNullOrEmpty(tmp.Descriptions),
                    HasSalePrice = !String.IsNullOrEmpty(tmp.SalePrice),
                    IsSold = false,
                    CarName = tmp.ModelYear + " " + tmp.Make + " " + tmp.Model,
                    DateInStock = tmp.DateInStock.Value,
                    DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                    HealthLevel = LogicHelper.GetHealthLevel(tmp),
                    SinglePhoto =
                        String.IsNullOrEmpty(tmp.ThumbnailImageURL)
                            ? tmp.DefaultImageUrl
                            : tmp.ThumbnailImageURL.Split(new string[] { ",", "|" },
                                                                     StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(),

                    SalePrice = tmp.SalePrice,
                    Price = CommonHelper.FormatPurePrice(tmp.SalePrice),
                    MarketRange = tmp.MarketRange.GetValueOrDefault(),
                    Reconstatus = tmp.Recon.GetValueOrDefault(),
                    CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault(),
                    IsFeatured = tmp.IsFeatured.GetValueOrDefault(),
                    IsTruck = !String.IsNullOrEmpty(tmp.VehicleType) && !tmp.VehicleType.Equals("Car"),
                    ACar = tmp.ACar.GetValueOrDefault()
                };
                list.Add(car);
            }


            //SET SORTING
            if (dealer.InventorySorting.Equals("Year"))
                viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
            else if (dealer.InventorySorting.Equals("Make"))
                viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
            else if (dealer.InventorySorting.Equals("Model"))
                viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
            else if (dealer.InventorySorting.Equals("Age"))
                viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
            else
                viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

            viewModel.previousCriteria = dealer.InventorySorting;

            viewModel.sortASCOrder = false;

            viewModel.CurrentOrSoldInventory = true;

            Session["InventoryObject"] = viewModel;

            return View(viewName, viewModel);
        }

        #endregion
    }
}
