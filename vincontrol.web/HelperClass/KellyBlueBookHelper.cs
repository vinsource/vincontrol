using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using Vincontrol.Web.KBBServiceEndPoint;
using System.Text;
using Vincontrol.Web.Models;
using Vincontrol.Web.DatabaseModel;
using vincontrol.Data.Model;
using ExtendedEquipmentOption = Vincontrol.Web.Models.ExtendedEquipmentOption;
using KellyBlueBookAuctionDetail = Vincontrol.Web.Models.KellyBlueBookAuctionDetail;
using KellyBlueBookTradeInDetail = Vincontrol.Web.Models.KellyBlueBookTradeInDetail;
using KellyBlueBookTrimReport = Vincontrol.Web.Models.KellyBlueBookTrimReport;
using KellyBlueBookViewModel = Vincontrol.Web.Models.KellyBlueBookViewModel;

namespace Vincontrol.Web.HelperClass
{
    public sealed class KellyBlueBookHelperBAK
    {
        public static decimal GetMileageAjustment(string Vin, string ZipCode, string Mileage, int TrimId)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            decimal mileageAdjustment = 0;

            try
            {
                string authenticationKey = "";

                try
                {
                    authenticationKey =
                    m_serviceClient.Login(
                        System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                            CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {

                    return mileageAdjustment;
                }



                if (!String.IsNullOrEmpty(Vin))
                {


                    if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                    {

                        VehicleConfiguration[] vehicleConfigArray =
                            m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin, ZipCode, DateTime.Now);

                        if (vehicleConfigArray.Count() == 1)
                        {
                            int mileageNumber = 0;
                            Int32.TryParse(Mileage, out mileageNumber);

                            var firstConfig = vehicleConfigArray.FirstOrDefault();

                            firstConfig.Mileage = mileageNumber;

                            var vehicleValue = m_serviceClient.GetVehicleValuesByVehicleConfiguration(
                                authenticationKey, firstConfig, ApplicationCategory.Dealer, VehicleCondition.VeryGood,
                                ZipCode, DateTime.Now);

                            mileageAdjustment = vehicleValue.MileageAdjustment;

                        }
                        else
                        {
                            if (TrimId > 0)
                            {
                                if (vehicleConfigArray.Any(x => x.Trim.Id == TrimId))
                                {
                                    var vehicleConfig = vehicleConfigArray.First(x => x.Trim.Id == TrimId);

                                    int mileageNumber = 0;
                                    Int32.TryParse(Mileage, out mileageNumber);
                                    vehicleConfig.Mileage = mileageNumber;

                                    var vehicleValue =
                                        m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(
                                            authenticationKey, vehicleConfig, ApplicationCategory.Dealer, ZipCode,
                                            DateTime.Now);

                                    if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value))
                                    {
                                        mileageAdjustment = vehicleValue.MileageAdjustment;

                                    }

                                }
                            }

                        }



                    }


                }

            }

            catch (Exception)
            {
                mileageAdjustment = -1;
            }

            return mileageAdjustment;

        }

        public static KellyBlueBookViewModel GetDirectFullReport(string Vin, string ZipCode, long mileage)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = Vin,

                };

            try
            {

                string authenticationKey = "";

                try
                {
                    authenticationKey =
                    m_serviceClient.Login(
                        System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                            CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {

                    dealerPrice.Success = false;

                    return dealerPrice;
                }



                if (!String.IsNullOrEmpty(Vin))
                {


                    if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                    {

                        var vehicleConfigArray = m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin,
                                                                                              ZipCode, DateTime.Now);

                        dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                        foreach (var vehicleConfig in vehicleConfigArray)
                        {

                          
                            vehicleConfig.Mileage =(int) mileage;



                            var vehicleValue =
                                m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey,
                                                                                                    vehicleConfig,
                                                                                                    ApplicationCategory
                                                                                                        .Dealer, ZipCode,
                                                                                                    DateTime.Now);

                            if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value))
                            {

                                var trimReport = new KellyBlueBookTrimReport()
                                    {
                                        TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                        TrimId = vehicleConfig.Trim.Id,
                                        MileageAdjustment = vehicleValue.MileageAdjustment,
                                        MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                        OptionValuation = new List<OptionValuation>(),
                                        OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                        VehicleId = vehicleConfig.Id

                                    };

                                var TradeInDetail = new KellyBlueBookTradeInDetail();

                                var AuctionDetail = new KellyBlueBookAuctionDetail();

                                var resultWholeSale =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        ZipCode,
                                                                                        PriceType.Wholesale,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);


                                var resultReatil =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        ZipCode,
                                                                                        PriceType.Retail,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);




                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                                {
                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected
                                        };


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }

                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                                {

                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected,


                                        };
                                    if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale =
                                            resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                            PriceAdjustment.ToString("c0");
                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale = "0";
                                    }
                                    if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                    {
                                        extendedOption.PriceAdjustmentForRetail =
                                            resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                         PriceAdjustment.ToString("c0");
                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForRetail = "0";
                                    }


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }


                                foreach (Valuation va in vehicleValue.ValuationPrices)
                                {

                                    if (va.PriceType == PriceType.TradeInFair)
                                        TradeInDetail.TradeInFairPrice = va.Value;
                                    else if (va.PriceType == PriceType.TradeInGood)

                                        TradeInDetail.TradeInGoodPrice = va.Value;

                                    else if (va.PriceType == PriceType.TradeInVeryGood)

                                        TradeInDetail.TradeInVeryGoodPrice = va.Value;
                                    else if (va.PriceType == PriceType.TradeInExcellent)

                                        TradeInDetail.TradeInExcellentPrice = va.Value;
                                    else if (va.PriceType == PriceType.Wholesale)

                                        trimReport.WholeSale = va.Value;

                                    else if (va.PriceType == PriceType.Retail)

                                        trimReport.Retail = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionFair)

                                        AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionGood)

                                        AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionVeryGood)

                                        AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionExcellent)

                                        AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.FairPurchasePrice)

                                        trimReport.FairPurchasePrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.BaseRetail)
                                    {
                                        decimal wholeRetail =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Retail).Value;

                                        trimReport.BaseRetail =
                                            (wholeRetail - trimReport.MileageAdjustment).ToString("c0");
                                    }

                                    else if (va.PriceType == PriceType.BaseWholesale)
                                    {
                                        decimal wholeSale =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Wholesale).Value;

                                        trimReport.BaseWholesale =
                                            (wholeSale - trimReport.MileageAdjustment);


                                    }

                                    else if (va.PriceType == PriceType.CPO)

                                        trimReport.CPO = va.Value.ToString("c0");


                                    else if (va.PriceType == PriceType.Invoice)

                                        trimReport.Invoice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.MSRP)

                                        trimReport.MSRP = va.Value.ToString("c0");





                                }



                                trimReport.TradeInPrice = TradeInDetail;

                                trimReport.AuctionPrice = AuctionDetail;

                                dealerPrice.TrimReportList.Add(trimReport);
                            }


                        }

                        if (dealerPrice.TrimReportList.Count > 0)
                        {
                            dealerPrice.Success = true;

                        }
                    }


                }
                else
                    dealerPrice.Success = false;
            }


            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }

            dealerPrice.ZipCode = ZipCode;
            return dealerPrice;

        }

        public static KellyBlueBookViewModel GetDirectFullReport(string Vin, string ZipCode, long Mileage, int TrimId)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = Vin,

                };
            try
            {

                string authenticationKey = "";

                try
                {
                    authenticationKey =
                    m_serviceClient.Login(
                        System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                            CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {

                    dealerPrice.Success = false;
                    return dealerPrice;
                }



                if (!String.IsNullOrEmpty(Vin))
                {


                    if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                    {

                        VehicleConfiguration[] vehicleConfigArray =
                            m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin, ZipCode, DateTime.Now);

                        dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                        foreach (var vehicleConfig in vehicleConfigArray)
                        {

                           
                            vehicleConfig.Mileage =(int) Mileage;

                            var vehicleValue =
                                m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey,
                                                                                                    vehicleConfig,
                                                                                                    ApplicationCategory
                                                                                                        .Dealer, ZipCode,
                                                                                                    DateTime.Now);

                            if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value) && vehicleConfig.Trim.Id == TrimId)
                            {

                                var trimReport = new KellyBlueBookTrimReport()
                                    {
                                        TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                        TrimId = vehicleConfig.Trim.Id,
                                        MileageAdjustment = vehicleValue.MileageAdjustment,
                                        MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                        OptionValuation = new List<OptionValuation>(),
                                        OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                        VehicleId = vehicleConfig.Id

                                    };

                                var TradeInDetail = new KellyBlueBookTradeInDetail();

                                var AuctionDetail = new KellyBlueBookAuctionDetail();

                                var resultWholeSale =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        ZipCode,
                                                                                        PriceType.Wholesale,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);


                                var resultReatil =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        ZipCode,
                                                                                        PriceType.Retail,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);

                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                                {
                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected
                                        };


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }
                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                                {

                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected,


                                        };
                                    if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale =
                                            resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                            PriceAdjustment.ToString("c0");
                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale = "0";
                                    }
                                    if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                    {
                                        extendedOption.PriceAdjustmentForRetail =
                                            resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                         PriceAdjustment.ToString("c0");
                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForRetail = "0";
                                    }


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }



                                foreach (Valuation va in vehicleValue.ValuationPrices)
                                {

                                    if (va.PriceType == PriceType.TradeInFair)
                                        TradeInDetail.TradeInFairPrice = va.Value;
                                    else if (va.PriceType == PriceType.TradeInGood)

                                        TradeInDetail.TradeInGoodPrice = va.Value;

                                    else if (va.PriceType == PriceType.TradeInVeryGood)

                                        TradeInDetail.TradeInVeryGoodPrice = va.Value;
                                    else if (va.PriceType == PriceType.TradeInExcellent)

                                        TradeInDetail.TradeInExcellentPrice = va.Value;
                                    else if (va.PriceType == PriceType.Wholesale)

                                        trimReport.WholeSale = va.Value;

                                    else if (va.PriceType == PriceType.Retail)

                                        trimReport.Retail = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionFair)

                                        AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionGood)

                                        AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionVeryGood)

                                        AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionExcellent)

                                        AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.FairPurchasePrice)

                                        trimReport.FairPurchasePrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.BaseRetail)
                                    {
                                        decimal wholeRetail =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Retail).Value;

                                        trimReport.BaseRetail =
                                            (wholeRetail - trimReport.MileageAdjustment).ToString("c0");
                                    }

                                    else if (va.PriceType == PriceType.BaseWholesale)
                                    {
                                        decimal wholeSale =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Wholesale).Value;

                                        trimReport.BaseWholesale =
                                            (wholeSale - trimReport.MileageAdjustment);


                                    }

                                    else if (va.PriceType == PriceType.CPO)

                                        trimReport.CPO = va.Value.ToString("c0");


                                    else if (va.PriceType == PriceType.Invoice)

                                        trimReport.Invoice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.MSRP)

                                        trimReport.MSRP = va.Value.ToString("c0");





                                }



                                trimReport.TradeInPrice = TradeInDetail;

                                trimReport.AuctionPrice = AuctionDetail;

                                dealerPrice.TrimReportList.Add(trimReport);

                                break;

                            }


                        }

                        if (dealerPrice.TrimReportList.Count > 0)
                        {
                            dealerPrice.Success = true;

                        }
                    }


                }
                else
                    dealerPrice.Success = false;
            }

            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }
            dealerPrice.ZipCode = ZipCode;
            return dealerPrice;

        }

        public static KellyBlueBookViewModel GetDirectFullReport(string Vin, string ZipCode, long Mileage,
                                                                 string SavedOption)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            decimal wholeSaleAdjustment = 0;

            decimal retailAdjustment = 0;

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = Vin,

                };

            try
            {

                string authenticationKey = "";

                try
                {
                    authenticationKey =
                    m_serviceClient.Login(
                        System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                            CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {

                    dealerPrice.Success = false;
                    return dealerPrice;
                }



                if (!String.IsNullOrEmpty(Vin))
                {


                    if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                    {

                        VehicleConfiguration[] vehicleConfigArray =
                            m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin, ZipCode, DateTime.Now);

                        dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                        foreach (var vehicleConfig in vehicleConfigArray)
                        {

                        
                            vehicleConfig.Mileage =(int) Mileage;

                            var vehicleValue =
                                m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey,
                                                                                                    vehicleConfig,
                                                                                                    ApplicationCategory.
                                                                                                        Dealer, ZipCode,
                                                                                                    DateTime.Now);



                            var trimReport = new KellyBlueBookTrimReport()
                                {
                                    TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                    TrimId = vehicleConfig.Trim.Id,
                                    MileageAdjustment = vehicleValue.MileageAdjustment,
                                    MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                    OptionValuation = new List<OptionValuation>(),
                                    OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                    VehicleId = vehicleConfig.Id

                                };

                            var TradeInDetail = new KellyBlueBookTradeInDetail();

                            var AuctionDetail = new KellyBlueBookAuctionDetail();

                            var resultWholeSale =
                                m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                    vehicleConfig.Id,
                                                                                    ZipCode,
                                                                                    PriceType.Wholesale,
                                                                                    DateTime.Now,
                                                                                    ApplicationCategory.
                                                                                        Dealer);


                            var resultReatil =
                                m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                    vehicleConfig.Id,
                                                                                    ZipCode,
                                                                                    PriceType.Retail,
                                                                                    DateTime.Now,
                                                                                    ApplicationCategory.
                                                                                        Dealer);

                            foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                            {
                                var extendedOption = new ExtendedEquipmentOption()
                                    {
                                        VehicleOptionId = tmp.VehicleOptionId,
                                        DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                        DisplayName = tmp.DisplayName,
                                        IsSelected = tmp.IsSelected
                                    };


                                trimReport.OptionalEquipment.Add(extendedOption);
                            }


                            foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                            {

                                var extendedOption = new ExtendedEquipmentOption()
                                    {
                                        VehicleOptionId = tmp.VehicleOptionId,
                                        DisplayNameAdditionalData =
                                            tmp.DisplayNameAdditionalData,
                                        DisplayName = tmp.DisplayName,
                                        IsSelected = tmp.IsSelected,


                                    };
                                if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                {

                                    extendedOption.PriceAdjustmentForWholeSale =
                                        resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                        PriceAdjustment.ToString("c0");


                                }
                                else
                                {
                                    extendedOption.PriceAdjustmentForWholeSale = "0";
                                }

                                if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                {
                                    extendedOption.PriceAdjustmentForRetail =
                                        resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                     PriceAdjustment.ToString("c0");
                                }
                                else
                                {
                                    extendedOption.PriceAdjustmentForRetail = "0";
                                }

                                if (!String.IsNullOrEmpty(SavedOption) &&
                                    SavedOption.Contains(extendedOption.VehicleOptionId.ToString()))
                                {
                                    wholeSaleAdjustment +=
                                        resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                        PriceAdjustment;


                                    retailAdjustment +=
                                        resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                     PriceAdjustment;

                                    extendedOption.IsSaved = true;
                                }
                                trimReport.OptionalEquipment.Add(extendedOption);
                            }



                            foreach (Valuation va in vehicleValue.ValuationPrices)
                            {

                                if (va.PriceType == PriceType.TradeInFair)
                                    TradeInDetail.TradeInFairPrice = va.Value;
                                else if (va.PriceType == PriceType.TradeInGood)

                                    TradeInDetail.TradeInGoodPrice = va.Value;

                                else if (va.PriceType == PriceType.TradeInVeryGood)

                                    TradeInDetail.TradeInVeryGoodPrice = va.Value;
                                else if (va.PriceType == PriceType.TradeInExcellent)

                                    TradeInDetail.TradeInExcellentPrice = va.Value;
                                else if (va.PriceType == PriceType.Wholesale)

                                    trimReport.WholeSale = (va.Value + wholeSaleAdjustment);

                                else if (va.PriceType == PriceType.Retail)

                                    trimReport.Retail = (va.Value + retailAdjustment).ToString("c0");

                                else if (va.PriceType == PriceType.AuctionFair)

                                    AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                else if (va.PriceType == PriceType.AuctionGood)

                                    AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                else if (va.PriceType == PriceType.AuctionVeryGood)

                                    AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                else if (va.PriceType == PriceType.AuctionExcellent)

                                    AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                else if (va.PriceType == PriceType.FairPurchasePrice)

                                    trimReport.FairPurchasePrice = va.Value.ToString("c0");

                                else if (va.PriceType == PriceType.BaseRetail)
                                {
                                    decimal wholeRetail =
                                        vehicleValue.ValuationPrices.FirstOrDefault(x => x.PriceType == PriceType.Retail)
                                                    .Value;

                                    trimReport.BaseRetail =
                                        (wholeRetail - trimReport.MileageAdjustment + retailAdjustment).ToString("c0");

                                    //trimReport.BaseRetail = (va.Value + retailAdjustment).ToString("c0");
                                }

                                else if (va.PriceType == PriceType.BaseWholesale)
                                {
                                    decimal wholeSale =
                                        vehicleValue.ValuationPrices.FirstOrDefault(
                                            x => x.PriceType == PriceType.Wholesale).Value;

                                    trimReport.BaseWholesale =
                                        (wholeSale - trimReport.MileageAdjustment + wholeSaleAdjustment);

                                    //trimReport.BaseWholesale = (va.Value + wholeSaleAdjustment).ToString("c0");


                                }

                                else if (va.PriceType == PriceType.CPO)

                                    trimReport.CPO = va.Value.ToString("c0");


                                else if (va.PriceType == PriceType.Invoice)

                                    trimReport.Invoice = va.Value.ToString("c0");

                                else if (va.PriceType == PriceType.MSRP)

                                    trimReport.MSRP = va.Value.ToString("c0");





                            }



                            trimReport.TradeInPrice = TradeInDetail;

                            trimReport.AuctionPrice = AuctionDetail;

                            dealerPrice.TrimReportList.Add(trimReport);

                            break;

                        }




                        if (dealerPrice.TrimReportList.Count > 0)
                        {
                            dealerPrice.Success = true;

                        }
                    }


                }
                else
                    dealerPrice.Success = false;
            }

            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }
            dealerPrice.ZipCode = ZipCode;
            return dealerPrice;

        }

        public static KellyBlueBookViewModel GetDirectFullReport(string vin, string zipCode, long mileage, int trimId,
                                                                 string savedOption)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            decimal wholeSaleAdjustment = 0;

            decimal retailAdjustment = 0;

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = vin,

                };

            try
            {
                string authenticationKey = "";

                try
                {
                    authenticationKey =
                    m_serviceClient.Login(
                        System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                            CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {

                    dealerPrice.Success = false;
                    return dealerPrice;
                }


                if (!String.IsNullOrEmpty(vin))
                {


                    if (m_serviceClient.ValidateVIN(authenticationKey, vin) == VINStatus.V)
                    {

                        VehicleConfiguration[] vehicleConfigArray =
                            m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, vin, zipCode, DateTime.Now);

                        dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                        foreach (var vehicleConfig in vehicleConfigArray)
                        {

                            
                            vehicleConfig.Mileage =(int) mileage;

                            var vehicleValue =
                                m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey,
                                                                                                    vehicleConfig,
                                                                                                    ApplicationCategory
                                                                                                        .Dealer, zipCode,
                                                                                                    DateTime.Now);

                            if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value) && vehicleConfig.Trim.Id == trimId)
                            {

                                var trimReport = new KellyBlueBookTrimReport()
                                    {
                                        TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                        TrimId = vehicleConfig.Trim.Id,
                                        MileageAdjustment = vehicleValue.MileageAdjustment,
                                        MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                        OptionValuation = new List<OptionValuation>(),
                                        OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                        VehicleId = vehicleConfig.Id

                                    };

                                var TradeInDetail = new KellyBlueBookTradeInDetail();

                                var AuctionDetail = new KellyBlueBookAuctionDetail();

                                var resultWholeSale =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        zipCode,
                                                                                        PriceType.Wholesale,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);


                                var resultReatil =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        zipCode,
                                                                                        PriceType.Retail,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);

                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                                {
                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected,
                                            PriceAdjustmentForWholeSale =
                                                resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                                PriceAdjustment.ToString("c0"),
                                            PriceAdjustmentForRetail =
                                                resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                             PriceAdjustment.ToString("c0"),

                                        };


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }


                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                                {

                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData =
                                                tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected,


                                        };
                                    if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                    {

                                        extendedOption.PriceAdjustmentForWholeSale =
                                            resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                            PriceAdjustment.ToString("c0");


                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale = "0";
                                    }

                                    if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                    {
                                        extendedOption.PriceAdjustmentForRetail =
                                            resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                         PriceAdjustment.ToString("c0");
                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForRetail = "0";
                                    }

                                    if (!String.IsNullOrEmpty(savedOption) &&
                                        savedOption.Contains(extendedOption.VehicleOptionId.ToString()))
                                    {
                                        wholeSaleAdjustment +=
                                            resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                            PriceAdjustment;


                                        retailAdjustment +=
                                            resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                         PriceAdjustment;

                                        extendedOption.IsSaved = true;
                                    }
                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }



                                foreach (Valuation va in vehicleValue.ValuationPrices)
                                {

                                    if (va.PriceType == PriceType.TradeInFair)
                                        TradeInDetail.TradeInFairPrice = (va.Value + wholeSaleAdjustment);
                                    else if (va.PriceType == PriceType.TradeInGood)

                                        TradeInDetail.TradeInGoodPrice = (va.Value + wholeSaleAdjustment);

                                    else if (va.PriceType == PriceType.TradeInVeryGood)

                                        TradeInDetail.TradeInVeryGoodPrice =
                                            (va.Value + wholeSaleAdjustment);
                                    else if (va.PriceType == PriceType.TradeInExcellent)

                                        TradeInDetail.TradeInExcellentPrice =
                                            (va.Value + wholeSaleAdjustment);
                                    else if (va.PriceType == PriceType.Wholesale)

                                        trimReport.WholeSale = (va.Value + wholeSaleAdjustment);

                                    else if (va.PriceType == PriceType.Retail)

                                        trimReport.Retail = (va.Value + retailAdjustment).ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionFair)

                                        AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionGood)

                                        AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionVeryGood)

                                        AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionExcellent)

                                        AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.FairPurchasePrice)

                                        trimReport.FairPurchasePrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.BaseRetail)
                                    {
                                        decimal wholeRetail =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Retail).Value;

                                        trimReport.BaseRetail =
                                            (wholeRetail - trimReport.MileageAdjustment + retailAdjustment).ToString(
                                                "c0");

                                        //trimReport.BaseRetail = (va.Value + retailAdjustment).ToString("c0");
                                    }

                                    else if (va.PriceType == PriceType.BaseWholesale)
                                    {
                                        decimal wholeSale =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Wholesale).Value;

                                        trimReport.BaseWholesale =
                                            (wholeSale - trimReport.MileageAdjustment + wholeSaleAdjustment);

                                        //trimReport.BaseWholesale = (va.Value + wholeSaleAdjustment).ToString("c0");


                                    }

                                    else if (va.PriceType == PriceType.CPO)

                                        trimReport.CPO = va.Value.ToString("c0");


                                    else if (va.PriceType == PriceType.Invoice)

                                        trimReport.Invoice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.MSRP)

                                        trimReport.MSRP = va.Value.ToString("c0");





                                }



                                trimReport.TradeInPrice = TradeInDetail;

                                trimReport.AuctionPrice = AuctionDetail;

                                dealerPrice.TrimReportList.Add(trimReport);

                                break;

                            }


                        }

                        if (dealerPrice.TrimReportList.Count > 0)
                        {
                            dealerPrice.Success = true;

                        }
                    }


                }
                else
                    dealerPrice.Success = false;
            }

            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }
            dealerPrice.ZipCode = zipCode;
            return dealerPrice;

        }

        public static KellyBlueBookViewModel GetFullReport(string Vin, string ZipCode, long Mileage)
        {

            var m_serviceClient = new VehicleInformationService2008R2Client();

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = Vin,

                };
            try
            {
                string authenticationKey = "";

                if (!String.IsNullOrEmpty(Vin))
                {
                    dealerPrice = GetKbbModelInDatabase(Vin);

                    if (dealerPrice.StatusExistInDatabase == 1)
                    {
                        dealerPrice.Success = true;
                        return dealerPrice;
                    }
                    else
                    {
                        try
                        {
                            authenticationKey =
                            m_serviceClient.Login(
                                System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                                    CultureInfo.InvariantCulture),
                                System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                                    CultureInfo.InvariantCulture));
                        }
                        catch (Exception)
                        {

                            dealerPrice.Success = false;
                            return dealerPrice;
                        }



                        if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                        {

                            VehicleConfiguration[] vehicleConfigArray =
                                m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin, ZipCode,
                                                                             DateTime.Now);

                            dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                            foreach (VehicleConfiguration vehicleConfig in vehicleConfigArray)
                            {
                               
                                vehicleConfig.Mileage = (int)Mileage;
                                VehicleValuation vehicleValue =
                                    m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(
                                        authenticationKey, vehicleConfig, ApplicationCategory.Dealer, ZipCode,
                                        DateTime.Now);

                                if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value))
                                {

                                    var trimReport = new KellyBlueBookTrimReport()
                                        {
                                            TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                            TrimId = vehicleConfig.Trim.Id,
                                            MileageAdjustment = vehicleValue.MileageAdjustment,
                                            MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                            OptionValuation = new List<OptionValuation>(),
                                            OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                            VehicleId = vehicleConfig.Id

                                        };

                                    var TradeInDetail = new KellyBlueBookTradeInDetail();

                                    var AuctionDetail = new KellyBlueBookAuctionDetail();

                                    var resultWholeSale =
                                        m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                            vehicleConfig.Id,
                                                                                            ZipCode,
                                                                                            PriceType.Wholesale,
                                                                                            DateTime.Now,
                                                                                            ApplicationCategory.
                                                                                                Dealer);


                                    var resultReatil =
                                        m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                            vehicleConfig.Id,
                                                                                            ZipCode,
                                                                                            PriceType.Retail,
                                                                                            DateTime.Now,
                                                                                            ApplicationCategory.
                                                                                                Dealer);


                                    foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                                    {
                                        var extendedOption = new ExtendedEquipmentOption()
                                            {
                                                VehicleOptionId = tmp.VehicleOptionId,
                                                DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                                DisplayName = tmp.DisplayName,
                                                IsSelected = tmp.IsSelected
                                            };


                                        trimReport.OptionalEquipment.Add(extendedOption);
                                    }

                                    foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                                    {

                                        var extendedOption = new ExtendedEquipmentOption()
                                            {
                                                VehicleOptionId = tmp.VehicleOptionId,
                                                DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                                DisplayName = tmp.DisplayName,
                                                IsSelected = tmp.IsSelected,


                                            };
                                        if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                        {
                                            extendedOption.PriceAdjustmentForWholeSale =
                                                resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                                PriceAdjustment.ToString("c0");
                                        }
                                        else
                                        {
                                            extendedOption.PriceAdjustmentForWholeSale = "0";
                                        }
                                        if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                        {
                                            extendedOption.PriceAdjustmentForRetail =
                                                resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                             PriceAdjustment.ToString("c0");
                                        }
                                        else
                                        {
                                            extendedOption.PriceAdjustmentForRetail = "0";
                                        }


                                        trimReport.OptionalEquipment.Add(extendedOption);
                                    }





                                    foreach (Valuation va in vehicleValue.ValuationPrices)
                                    {

                                        if (va.PriceType == PriceType.TradeInFair)
                                            TradeInDetail.TradeInFairPrice = va.Value;
                                        else if (va.PriceType == PriceType.TradeInGood)

                                            TradeInDetail.TradeInGoodPrice = va.Value;

                                        else if (va.PriceType == PriceType.TradeInVeryGood)

                                            TradeInDetail.TradeInVeryGoodPrice = va.Value;
                                        else if (va.PriceType == PriceType.TradeInExcellent)

                                            TradeInDetail.TradeInExcellentPrice = va.Value;
                                        else if (va.PriceType == PriceType.Wholesale)

                                            trimReport.WholeSale = va.Value;

                                        else if (va.PriceType == PriceType.Retail)

                                            trimReport.Retail = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionFair)

                                            AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionGood)

                                            AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionVeryGood)

                                            AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionExcellent)

                                            AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.FairPurchasePrice)

                                            trimReport.FairPurchasePrice = va.Value.ToString("c0");


                                        else if (va.PriceType == PriceType.BaseRetail)
                                        {
                                            decimal wholeRetail =
                                                vehicleValue.ValuationPrices.FirstOrDefault(
                                                    x => x.PriceType == PriceType.Retail).Value;

                                            trimReport.BaseRetail =
                                                (wholeRetail - trimReport.MileageAdjustment).ToString("c0");
                                        }

                                        else if (va.PriceType == PriceType.BaseWholesale)
                                        {
                                            decimal wholeSale =
                                                vehicleValue.ValuationPrices.FirstOrDefault(
                                                    x => x.PriceType == PriceType.Wholesale).Value;

                                            trimReport.BaseWholesale =
                                                (wholeSale - trimReport.MileageAdjustment);


                                        }


                                        else if (va.PriceType == PriceType.CPO)

                                            trimReport.CPO = va.Value.ToString("c0");


                                        else if (va.PriceType == PriceType.Invoice)

                                            trimReport.Invoice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.MSRP)

                                            trimReport.MSRP = va.Value.ToString("c0");





                                    }

                                    trimReport.TradeInPrice = TradeInDetail;

                                    trimReport.AuctionPrice = AuctionDetail;

                                    dealerPrice.TrimReportList.Add(trimReport);
                                }


                            }

                            if (dealerPrice.TrimReportList.Count > 0)
                            {
                                dealerPrice.Success = true;

                                if (dealerPrice.StatusExistInDatabase == 0)

                                    SQLHelper.AddSimpleKbbReport(dealerPrice);
                                else if (dealerPrice.StatusExistInDatabase == 2)

                                    SQLHelper.UpdateSimpleKbbReport(dealerPrice);
                            }
                        }

                    }

                }
                else
                    dealerPrice.Success = false;
            }


            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }

            dealerPrice.ZipCode = ZipCode;
            return dealerPrice;

        }


        public static KellyBlueBookViewModel GetFullReport(string Vin, string ZipCode, string Mileage, int TrimId,
                                                           string SavedOption)
        {

            var m_serviceClient = new VehicleInformationService2008R2Client();


            decimal wholeSaleAdjustment = 0;

            decimal retailAdjustment = 0;

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = Vin,

                };

            string authenticationKey = "";
            try
            {
               
                if (!String.IsNullOrEmpty(Vin))
                {
                    dealerPrice = GetKbbModelInDatabase(Vin, TrimId);

                    if (dealerPrice.StatusExistInDatabase == 1)
                    {
                        dealerPrice.Success = true;
                        return dealerPrice;
                    }
                    else
                    {
                        try
                        {
                            authenticationKey =
                            m_serviceClient.Login(
                                System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                                    CultureInfo.InvariantCulture),
                                System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                                    CultureInfo.InvariantCulture));
                        }
                        catch (Exception)
                        {

                            dealerPrice.Success = false;
                            return dealerPrice;
                        }

                        if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                        {

                            VehicleConfiguration[] vehicleConfigArray =
                                m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin, ZipCode,
                                                                             DateTime.Now);

                            dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                            foreach (VehicleConfiguration vehicleConfig in vehicleConfigArray)
                            {
                                int mileageNumber = 0;
                                Int32.TryParse(Mileage, out mileageNumber);
                                vehicleConfig.Mileage = mileageNumber;

                                VehicleValuation vehicleValue =
                                    m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(
                                        authenticationKey, vehicleConfig, ApplicationCategory.Dealer, ZipCode,
                                        DateTime.Now);

                                if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value))
                                {

                                    var trimReport = new KellyBlueBookTrimReport()
                                        {
                                            TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                            TrimId = vehicleConfig.Trim.Id,
                                            MileageAdjustment = vehicleValue.MileageAdjustment,
                                            MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                            OptionValuation = new List<OptionValuation>(),
                                            OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                            VehicleId = vehicleConfig.Id

                                        };

                                    var TradeInDetail = new KellyBlueBookTradeInDetail();

                                    var AuctionDetail = new KellyBlueBookAuctionDetail();

                                    var resultWholeSale =
                                        m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                            vehicleConfig.Id,
                                                                                            ZipCode,
                                                                                            PriceType.Wholesale,
                                                                                            DateTime.Now,
                                                                                            ApplicationCategory.
                                                                                                Dealer);


                                    //var resultReatil =
                                    //m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                    //                                                            vehicleConfig.Id,
                                    //                                                            ZipCode,
                                    //                                                            PriceType.Retail,
                                    //                                                            DateTime.Now,
                                    //                                                            ApplicationCategory.
                                    //                                                                Dealer);


                                    foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                                    {
                                        var extendedOption = new ExtendedEquipmentOption()
                                            {
                                                VehicleOptionId = tmp.VehicleOptionId,
                                                DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                                DisplayName = tmp.DisplayName,
                                                IsSelected = tmp.IsSelected
                                            };


                                        trimReport.OptionalEquipment.Add(extendedOption);
                                    }

                                    foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                                    {

                                        var extendedOption = new ExtendedEquipmentOption()
                                            {
                                                VehicleOptionId = tmp.VehicleOptionId,
                                                DisplayNameAdditionalData =
                                                    tmp.DisplayNameAdditionalData,
                                                DisplayName = tmp.DisplayName,
                                                IsSelected = tmp.IsSelected,


                                            };
                                        if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                        {

                                            extendedOption.PriceAdjustmentForWholeSale =
                                                resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                                PriceAdjustment.ToString("c0");


                                        }
                                        else
                                        {
                                            extendedOption.PriceAdjustmentForWholeSale = "0";
                                        }

                                        //if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                        //{
                                        //    extendedOption.PriceAdjustmentForRetail =
                                        //        resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                        //            PriceAdjustment.ToString("c0");
                                        //}
                                        //else
                                        //{
                                        //    extendedOption.PriceAdjustmentForRetail = "0";
                                        //}

                                        if (!String.IsNullOrEmpty(SavedOption) &&
                                            SavedOption.Contains(extendedOption.VehicleOptionId.ToString()))
                                        {
                                            wholeSaleAdjustment +=
                                                resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                                PriceAdjustment;


                                            //retailAdjustment +=
                                            //    resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                            //        PriceAdjustment;

                                            extendedOption.IsSaved = true;
                                        }
                                        trimReport.OptionalEquipment.Add(extendedOption);
                                    }



                                    foreach (Valuation va in vehicleValue.ValuationPrices)
                                    {

                                        if (va.PriceType == PriceType.TradeInFair)
                                            TradeInDetail.TradeInFairPrice =
                                                (va.Value + wholeSaleAdjustment);
                                        else if (va.PriceType == PriceType.TradeInGood)

                                            TradeInDetail.TradeInGoodPrice =
                                                (va.Value + wholeSaleAdjustment);

                                        else if (va.PriceType == PriceType.TradeInVeryGood)

                                            TradeInDetail.TradeInVeryGoodPrice =
                                                (va.Value + wholeSaleAdjustment);
                                        else if (va.PriceType == PriceType.TradeInExcellent)

                                            TradeInDetail.TradeInExcellentPrice =
                                                (va.Value + wholeSaleAdjustment);
                                        else if (va.PriceType == PriceType.Wholesale)

                                            trimReport.WholeSale = (va.Value + wholeSaleAdjustment);

                                        else if (va.PriceType == PriceType.Retail)

                                            trimReport.Retail = (va.Value + retailAdjustment).ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionFair)

                                            AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionGood)

                                            AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionVeryGood)

                                            AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.AuctionExcellent)

                                            AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.FairPurchasePrice)

                                            trimReport.FairPurchasePrice = va.Value.ToString("c0");

                                            //else if (va.PriceType == PriceType.BaseRetail)
                                            //{
                                            //    decimal wholeRetail = vehicleValue.ValuationPrices.FirstOrDefault(x => x.PriceType == PriceType.Retail).Value;

                                            //    trimReport.BaseRetail = (wholeRetail - trimReport.MileageAdjustment + retailAdjustment).ToString("c0");

                                            //    //trimReport.BaseRetail = (va.Value + retailAdjustment).ToString("c0");
                                            //}

                                        else if (va.PriceType == PriceType.BaseWholesale)
                                        {
                                            decimal wholeSale =
                                                vehicleValue.ValuationPrices.FirstOrDefault(
                                                    x => x.PriceType == PriceType.Wholesale).Value;

                                            trimReport.BaseWholesale =
                                                (wholeSale - trimReport.MileageAdjustment + wholeSaleAdjustment)
                                                    ;

                                            //trimReport.BaseWholesale = (va.Value + wholeSaleAdjustment).ToString("c0");


                                        }

                                        else if (va.PriceType == PriceType.CPO)

                                            trimReport.CPO = va.Value.ToString("c0");


                                        else if (va.PriceType == PriceType.Invoice)

                                            trimReport.Invoice = va.Value.ToString("c0");

                                        else if (va.PriceType == PriceType.MSRP)

                                            trimReport.MSRP = va.Value.ToString("c0");





                                    }




                                    trimReport.TradeInPrice = TradeInDetail;

                                    trimReport.AuctionPrice = AuctionDetail;

                                    dealerPrice.TrimReportList.Add(trimReport);
                                }


                            }

                            if (dealerPrice.TrimReportList.Count > 0)
                            {
                                dealerPrice.Success = true;

                                if (dealerPrice.StatusExistInDatabase == 0)

                                    SQLHelper.AddSimpleKbbReport(dealerPrice);
                                else if (dealerPrice.StatusExistInDatabase == 2)

                                    SQLHelper.UpdateSimpleKbbReport(dealerPrice);
                            }
                        }

                    }

                }
                else
                    dealerPrice.Success = false;
            }

            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }

            dealerPrice.ZipCode = ZipCode;
            return dealerPrice;

        }


        public static KellyBlueBookViewModel GetFullReportWithSavingChanges(string Vin, string ZipCode, long Mileage)
        {

            var m_serviceClient = new VehicleInformationService2008R2Client();

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = Vin,

                };

            try
            {
                string authenticationKey = "";

                try
                {
                    authenticationKey =
                    m_serviceClient.Login(
                        System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                            CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {

                    dealerPrice.Success = false;
                    return dealerPrice;
                }



                if (!String.IsNullOrEmpty(Vin))
                {



                    if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                    {

                        VehicleConfiguration[] vehicleConfigArray =
                            m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin, ZipCode, DateTime.Now);

                        dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                        foreach (VehicleConfiguration vehicleConfig in vehicleConfigArray)
                        {
                           
                            vehicleConfig.Mileage =(int) Mileage;
                            VehicleValuation vehicleValue =
                                m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey,
                                                                                                    vehicleConfig,
                                                                                                    ApplicationCategory.
                                                                                                        Dealer, ZipCode,
                                                                                                    DateTime.Now);

                            if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value))
                            {

                                var trimReport = new KellyBlueBookTrimReport()
                                    {
                                        TrimName =
                                            CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                        TrimId = vehicleConfig.Trim.Id,
                                        MileageAdjustment = vehicleValue.MileageAdjustment,
                                        MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                        OptionValuation = new List<OptionValuation>(),
                                        OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                        VehicleId = vehicleConfig.Id

                                    };

                                var TradeInDetail = new KellyBlueBookTradeInDetail();

                                var AuctionDetail = new KellyBlueBookAuctionDetail();

                                var resultWholeSale =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        ZipCode,
                                                                                        PriceType.Wholesale,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);


                                var resultReatil =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        ZipCode,
                                                                                        PriceType.Retail,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);


                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                                {
                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData =
                                                tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected
                                        };


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }

                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                                {

                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData =
                                                tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected,


                                        };
                                    if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale =
                                            resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                            PriceAdjustment.ToString("c0");
                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale = "0";
                                    }
                                    if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                    {
                                        extendedOption.PriceAdjustmentForRetail =
                                            resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                         PriceAdjustment.ToString("c0");
                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForRetail = "0";
                                    }


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }





                                foreach (Valuation va in vehicleValue.ValuationPrices)
                                {

                                    if (va.PriceType == PriceType.TradeInFair)
                                        TradeInDetail.TradeInFairPrice = va.Value;
                                    else if (va.PriceType == PriceType.TradeInGood)

                                        TradeInDetail.TradeInGoodPrice = va.Value;

                                    else if (va.PriceType == PriceType.TradeInVeryGood)

                                        TradeInDetail.TradeInVeryGoodPrice = va.Value;
                                    else if (va.PriceType == PriceType.TradeInExcellent)

                                        TradeInDetail.TradeInExcellentPrice = va.Value;
                                    else if (va.PriceType == PriceType.Wholesale)

                                        trimReport.WholeSale = va.Value;

                                    else if (va.PriceType == PriceType.Retail)

                                        trimReport.Retail = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionFair)

                                        AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionGood)

                                        AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionVeryGood)

                                        AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionExcellent)

                                        AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.FairPurchasePrice)

                                        trimReport.FairPurchasePrice = va.Value.ToString("c0");


                                    else if (va.PriceType == PriceType.BaseRetail)
                                    {
                                        decimal wholeRetail =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Retail).Value;

                                        trimReport.BaseRetail =
                                            (wholeRetail - trimReport.MileageAdjustment).ToString("c0");
                                    }

                                    else if (va.PriceType == PriceType.BaseWholesale)
                                    {
                                        decimal wholeSale =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Wholesale).Value;

                                        trimReport.BaseWholesale =
                                            (wholeSale - trimReport.MileageAdjustment);


                                    }


                                    else if (va.PriceType == PriceType.CPO)

                                        trimReport.CPO = va.Value.ToString("c0");


                                    else if (va.PriceType == PriceType.Invoice)

                                        trimReport.Invoice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.MSRP)

                                        trimReport.MSRP = va.Value.ToString("c0");





                                }

                                trimReport.TradeInPrice = TradeInDetail;

                                trimReport.AuctionPrice = AuctionDetail;

                                dealerPrice.TrimReportList.Add(trimReport);
                            }


                        }

                        if (dealerPrice.TrimReportList.Count > 0)
                        {
                            dealerPrice.Success = true;
                            SQLHelper.UpdateSimpleKbbReport(dealerPrice);
                        }
                    }

                }
                else
                    dealerPrice.Success = false;

            }


            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }

            dealerPrice.ZipCode = ZipCode;
            return dealerPrice;

        }

        public static KellyBlueBookViewModel GetFullReportWithSavingChanges(string Vin, string ZipCode, long Mileage,
                                                                            int TrimId, string SavedOption)
        {

            var m_serviceClient = new VehicleInformationService2008R2Client();


            decimal wholeSaleAdjustment = 0;

            decimal retailAdjustment = 0;

            var dealerPrice = new KellyBlueBookViewModel()
                {
                    Vin = Vin,

                };

            try
            {

                string authenticationKey = "";

                try
                {
                    authenticationKey =
                    m_serviceClient.Login(
                        System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                            CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {

                    dealerPrice.Success = false;
                    return dealerPrice;
                }


                if (!String.IsNullOrEmpty(Vin))
                {


                    if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                    {

                        VehicleConfiguration[] vehicleConfigArray =
                            m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin, ZipCode, DateTime.Now);

                        dealerPrice.TrimReportList = new List<KellyBlueBookTrimReport>();

                        foreach (VehicleConfiguration vehicleConfig in vehicleConfigArray)
                        {
                           
                            vehicleConfig.Mileage = (int)Mileage;

                            VehicleValuation vehicleValue =
                                m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey,
                                                                                                    vehicleConfig,
                                                                                                    ApplicationCategory
                                                                                                        .Dealer, ZipCode,
                                                                                                    DateTime.Now);

                            if (!String.IsNullOrEmpty(vehicleConfig.Trim.Value))
                            {

                                var trimReport = new KellyBlueBookTrimReport()
                                    {
                                        TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                                        TrimId = vehicleConfig.Trim.Id,
                                        MileageAdjustment = vehicleValue.MileageAdjustment,
                                        MileageZeroPoint = vehicleValue.MileageZeroPoint,
                                        OptionValuation = new List<OptionValuation>(),
                                        OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                        VehicleId = vehicleConfig.Id

                                    };

                                var TradeInDetail = new KellyBlueBookTradeInDetail();

                                var AuctionDetail = new KellyBlueBookAuctionDetail();

                                var resultWholeSale =
                                    m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                        vehicleConfig.Id,
                                                                                        ZipCode,
                                                                                        PriceType.Wholesale,
                                                                                        DateTime.Now,
                                                                                        ApplicationCategory.
                                                                                            Dealer);


                                //var resultReatil =
                                //m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                //                                                            vehicleConfig.Id,
                                //                                                            ZipCode,
                                //                                                            PriceType.Retail,
                                //                                                            DateTime.Now,
                                //                                                            ApplicationCategory.
                                //                                                                Dealer);


                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
                                {
                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected
                                        };


                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }

                                foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                                {

                                    var extendedOption = new ExtendedEquipmentOption()
                                        {
                                            VehicleOptionId = tmp.VehicleOptionId,
                                            DisplayNameAdditionalData =
                                                tmp.DisplayNameAdditionalData,
                                            DisplayName = tmp.DisplayName,
                                            IsSelected = tmp.IsSelected,


                                        };
                                    if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                    {

                                        extendedOption.PriceAdjustmentForWholeSale =
                                            resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                            PriceAdjustment.ToString("c0");


                                    }
                                    else
                                    {
                                        extendedOption.PriceAdjustmentForWholeSale = "0";
                                    }

                                    //if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                                    //{
                                    //    extendedOption.PriceAdjustmentForRetail =
                                    //        resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                    //            PriceAdjustment.ToString("c0");
                                    //}
                                    //else
                                    //{
                                    //    extendedOption.PriceAdjustmentForRetail = "0";
                                    //}

                                    if (!String.IsNullOrEmpty(SavedOption) &&
                                        SavedOption.Contains(extendedOption.VehicleOptionId.ToString()))
                                    {
                                        wholeSaleAdjustment +=
                                            resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                                            PriceAdjustment;


                                        //retailAdjustment +=
                                        //    resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                                        //        PriceAdjustment;

                                        extendedOption.IsSaved = true;
                                    }
                                    trimReport.OptionalEquipment.Add(extendedOption);
                                }



                                foreach (Valuation va in vehicleValue.ValuationPrices)
                                {

                                    if (va.PriceType == PriceType.TradeInFair)
                                        TradeInDetail.TradeInFairPrice = (va.Value + wholeSaleAdjustment);
                                    else if (va.PriceType == PriceType.TradeInGood)

                                        TradeInDetail.TradeInGoodPrice = (va.Value + wholeSaleAdjustment);

                                    else if (va.PriceType == PriceType.TradeInVeryGood)

                                        TradeInDetail.TradeInVeryGoodPrice =
                                            (va.Value + wholeSaleAdjustment);
                                    else if (va.PriceType == PriceType.TradeInExcellent)

                                        TradeInDetail.TradeInExcellentPrice =
                                            (va.Value + wholeSaleAdjustment);
                                    else if (va.PriceType == PriceType.Wholesale)

                                        trimReport.WholeSale = (va.Value + wholeSaleAdjustment);

                                    else if (va.PriceType == PriceType.Retail)

                                        trimReport.Retail = (va.Value + retailAdjustment).ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionFair)

                                        AuctionDetail.AuctionFairPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionGood)

                                        AuctionDetail.AuctionGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionVeryGood)

                                        AuctionDetail.AuctionVeryGoodPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.AuctionExcellent)

                                        AuctionDetail.AuctionExcellentPrice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.FairPurchasePrice)

                                        trimReport.FairPurchasePrice = va.Value.ToString("c0");

                                        //else if (va.PriceType == PriceType.BaseRetail)
                                        //{
                                        //    decimal wholeRetail = vehicleValue.ValuationPrices.FirstOrDefault(x => x.PriceType == PriceType.Retail).Value;

                                        //    trimReport.BaseRetail = (wholeRetail - trimReport.MileageAdjustment + retailAdjustment).ToString("c0");

                                        //    //trimReport.BaseRetail = (va.Value + retailAdjustment).ToString("c0");
                                        //}

                                    else if (va.PriceType == PriceType.BaseWholesale)
                                    {
                                        decimal wholeSale =
                                            vehicleValue.ValuationPrices.FirstOrDefault(
                                                x => x.PriceType == PriceType.Wholesale).Value;

                                        trimReport.BaseWholesale =
                                            (wholeSale - trimReport.MileageAdjustment + wholeSaleAdjustment);

                                        //trimReport.BaseWholesale = (va.Value + wholeSaleAdjustment).ToString("c0");


                                    }

                                    else if (va.PriceType == PriceType.CPO)

                                        trimReport.CPO = va.Value.ToString("c0");


                                    else if (va.PriceType == PriceType.Invoice)

                                        trimReport.Invoice = va.Value.ToString("c0");

                                    else if (va.PriceType == PriceType.MSRP)

                                        trimReport.MSRP = va.Value.ToString("c0");





                                }




                                trimReport.TradeInPrice = TradeInDetail;

                                trimReport.AuctionPrice = AuctionDetail;

                                dealerPrice.TrimReportList.Add(trimReport);
                            }


                        }

                        if (dealerPrice.TrimReportList.Count > 0)
                        {
                            dealerPrice.Success = true;

                            SQLHelper.UpdateSimpleKbbReport(dealerPrice);
                        }
                    }

                }
                else
                    dealerPrice.Success = false;

            }

            catch (Exception)
            {
                dealerPrice.Success = false;
                return dealerPrice;
            }
            dealerPrice.ZipCode = ZipCode;
            return dealerPrice;

        }


        public static KellyBlueBookViewModel GetKbbModelInDatabase(string vin)
        {
            var dealerPrice = new KellyBlueBookViewModel()
            {
                Vin = vin,
                TrimReportList = new List<KellyBlueBookTrimReport>()

            };
            int status = SQLHelper.CheckVinHasKbbReport(vin);
            if (status == 1)
            {

                var context = new VincontrolEntities();

                var list = context.KellyBlueBooks.Where(x => x.Vin==vin);

                foreach (KellyBlueBook kbb in list)
                {
                    //KellyBlueBookTradeInDetail TradeInDetail = new KellyBlueBookTradeInDetail()
                    //{
                    //    TradeInFairPrice =CommonHelper.ConvertToCurrency (kbb.),
                    //    TradeInGoodPrice = kbb.TradeInGoodPrice),
                    //    TradeInVeryGoodPrice =CommonHelper.ConvertToCurrency( kbb.TradeInVeryGoodPrice),
                    //};
                    var trimReport = new KellyBlueBookTrimReport()
                    {
                        //TradeInPrice = TradeInDetail,
                        BaseWholesale = kbb.BaseWholeSale.GetValueOrDefault(),
                        MileageAdjustment = kbb.MileageAdjustment.GetValueOrDefault(),
                        WholeSale =kbb.WholeSale.GetValueOrDefault(),
                        TrimName = kbb.Trim,
                        TrimId = kbb.TrimId.GetValueOrDefault()
                    };
                    dealerPrice.TrimReportList.Add(trimReport);
                    dealerPrice.StatusExistInDatabase = status;

                }
            }
            else
            {
                dealerPrice.StatusExistInDatabase = status;
            }
            return dealerPrice;
        }


        public static KellyBlueBookViewModel GetKbbModelInDatabase(string Vin, int TrimId)
        {
            var dealerPrice = new KellyBlueBookViewModel()
            {
                Vin = Vin,
                TrimReportList = new List<KellyBlueBookTrimReport>()

            };
            int status = SQLHelper.CheckVinHasKbbReport(Vin);
            if (status == 1)
            {

                var context = new VincontrolEntities();

                var list = context.KellyBlueBooks.Where(x => x.Vin==Vin && x.TrimId == TrimId);

                foreach (KellyBlueBook kbb in list)
                {
                    //KellyBlueBookTradeInDetail TradeInDetail = new KellyBlueBookTradeInDetail()
                    //{
                    //    TradeInFairPrice =CommonHelper.ConvertToCurrency (kbb.),
                    //    TradeInGoodPrice =CommonHelper.ConvertToCurrency( kbb.TradeInGoodPrice),
                    //    TradeInVeryGoodPrice =CommonHelper.ConvertToCurrency( kbb.TradeInVeryGoodPrice),
                    //};
                    var trimReport = new KellyBlueBookTrimReport()
                    {
                        //TradeInPrice = TradeInDetail,
                        BaseWholesale = kbb.BaseWholeSale.GetValueOrDefault(),
                        MileageAdjustment = kbb.MileageAdjustment.GetValueOrDefault(),
                        WholeSale = kbb.WholeSale.GetValueOrDefault(),
                        TrimName = kbb.Trim,
                        TrimId = kbb.TrimId.GetValueOrDefault()
                    };
                    dealerPrice.TrimReportList.Add(trimReport);
                    dealerPrice.StatusExistInDatabase = status;

                }
            }
            else
            {
                dealerPrice.StatusExistInDatabase = status;
            }
            return dealerPrice;
        }

        public static List<IdStringPair> GetKBBYears()
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            string authenticationKey = "";

            var returnList = new List<IdStringPair>();

            try
            {
                authenticationKey =
                m_serviceClient.Login(
                    System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                        CultureInfo.InvariantCulture),
                    System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                        CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                return returnList;
            }


            var result = m_serviceClient.GetYears(authenticationKey, VehicleClass.UsedCar, ApplicationCategory.Consumer,
                                                  DateTime.Now);

          

            foreach (var idStringPair in result)
            {
                returnList.Add(idStringPair);
            }
            return returnList;
        }

        public static List<IdStringPair> GetKBBMakesByYear(int YearId)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();


            var returnList = new List<IdStringPair>();

            string authenticationKey = "";
            try


            {
                authenticationKey =
                m_serviceClient.Login(
                    System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                        CultureInfo.InvariantCulture),
                    System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                        CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                return returnList;
            }

            var result = m_serviceClient.GetMakesByYear(authenticationKey, VehicleClass.UsedCar, ApplicationCategory.Consumer, YearId,
                                                  DateTime.Now);



            foreach (var idStringPair in result)
            {
                returnList.Add(idStringPair);
            }
            return returnList;
        }


        public static List<IdStringPair> GetKBBModelByMakeId(int YearId, int MakeId)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();


            var returnList = new List<IdStringPair>();

            string authenticationKey = "";
            try
            {
                authenticationKey =
                m_serviceClient.Login(
                    System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                        CultureInfo.InvariantCulture),
                    System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                        CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                return returnList;
            }


            var result = m_serviceClient.GetModelsByYearAndMake(authenticationKey, VehicleClass.UsedCar, ApplicationCategory.Consumer, MakeId, YearId,
                                                  DateTime.Now);

           
            foreach (var idStringPair in result)
            {
                returnList.Add(idStringPair);
            }
            return returnList;
        }


        public static List<VehicleTrim> GetKBBTrimByModelId(int YearId, int ModelId)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            string authenticationKey =
                m_serviceClient.Login(System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(CultureInfo.InvariantCulture),
                                      System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(CultureInfo.InvariantCulture));

            var result = m_serviceClient.GetTrimsAndVehicleIdsByYearAndModel(authenticationKey, VehicleClass.UsedCar, ApplicationCategory.Consumer, ModelId, YearId,
                                                  DateTime.Now);

            var returnList = new List<VehicleTrim>();

            foreach (var idStringPair in result)
            {

                returnList.Add(idStringPair);
            }
            return returnList;
        }


        public static TradeInVehicleModel GetKBBVehicleFromVehicleId(int VehicleId, int Mileage, string ZipCode)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            string authenticationKey =
                m_serviceClient.Login(System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(CultureInfo.InvariantCulture),
                                      System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(CultureInfo.InvariantCulture));

            var vehicleConfig = m_serviceClient.GetVehicleConfigurationByVehicleId(authenticationKey,
                                                                                            ApplicationCategory.Consumer,
                                                                                            VehicleId, ZipCode,
                                                                                            DateTime.Now);
            vehicleConfig.Mileage = Mileage;

            var returnVehicle = new TradeInVehicleModel()
                                    {

                                        SelectedYear = vehicleConfig.Year.Value,

                                        SelectedMake = vehicleConfig.Make.
                                            Value,

                                        SelectedModel = vehicleConfig.Model.
                                            Value,

                                        SelectedTrim = vehicleConfig.Trim.
                                            Value,

                                        VehicleId = VehicleId,

                                        OptionalEquipment = new List<ExtendedEquipmentOption>(),

                                        Mileage = Mileage.ToString(CultureInfo.InvariantCulture),



                                    };

            var resultWholeSale =
                                           m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                                     vehicleConfig.Id, ZipCode, PriceType.Wholesale, DateTime.Now,
                                                                                             ApplicationCategory.
                                                                                                 Consumer);

            foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
            {
                var extendedOption = new ExtendedEquipmentOption()
                                         {
                                             VehicleOptionId = tmp.VehicleOptionId,
                                             DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                             DisplayName = tmp.DisplayName,
                                             IsSelected = tmp.IsSelected,
                                         };

                if (resultWholeSale.Any() && resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                {
                    if (resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).PriceAdjustment > 0)
                    {
                        extendedOption.PriceAdjustmentForWholeSale =
                            resultWholeSale.FirstOrDefault(
                                x => x.Id == tmp.VehicleOptionId).
                                PriceAdjustment.ToString("c0");

                        returnVehicle.OptionalEquipment.Add(extendedOption);
                    }
                }



            }

            return returnVehicle;
        }

        public static TradeInVehicleModel GetKBBTrimsOrOptionsFromVin(string Vin, int Mileage, string ZipCode)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            string authenticationKey =
                m_serviceClient.Login(System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(CultureInfo.InvariantCulture),
                                      System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(CultureInfo.InvariantCulture));

            var returnVehicle = new TradeInVehicleModel()
                                       {
                                           ValidVin = false
                                       };

            if (!String.IsNullOrEmpty(Vin))
            {
                if (m_serviceClient.ValidateVIN(authenticationKey, Vin) == VINStatus.V)
                {
                    returnVehicle = new TradeInVehicleModel()
                                        {
                                            ValidVin = true,
                                            SpecificKBBTrimList = new List<VehicleConfiguration>(),
                                            Mileage = Mileage.ToString(CultureInfo.InvariantCulture),
                                            Vin = Vin
                                        };


                    var vehicleConfigArray = m_serviceClient.GetVehicleConfigurationByVIN(authenticationKey, Vin,
                                                                                          ZipCode, DateTime.Now);


                    if (vehicleConfigArray.Count() == 1)
                    {

                        var vehicleConfig = vehicleConfigArray.First();

                        vehicleConfig.Mileage = Mileage;

                        returnVehicle = new TradeInVehicleModel
                                            {
                                                SelectedYear = vehicleConfig.Year.Value,
                                                SelectedMake = vehicleConfig.Make.
                                                    Value,
                                                SelectedModel = vehicleConfig.Model.
                                                    Value,
                                                SelectedTrim = vehicleConfig.Trim.
                                                    Value,
                                                SpecificKBBTrimList = new List<VehicleConfiguration>(),
                                                OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                                ValidVin = true,
                                                VehicleId = vehicleConfig.Id,
                                                Mileage = Mileage.ToString(CultureInfo.InvariantCulture)
                                            };


                        returnVehicle.SpecificKBBTrimList.Add(vehicleConfig);


                        var resultWholeSale =
                            m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                vehicleConfig.Id, ZipCode,
                                                                                PriceType.Wholesale, DateTime.Now,
                                                                                ApplicationCategory.
                                                                                    Consumer);

                        foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
                        {
                            var extendedOption = new ExtendedEquipmentOption()
                                                     {
                                                         VehicleOptionId = tmp.VehicleOptionId,
                                                         DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                                         DisplayName = tmp.DisplayName,
                                                         IsSelected = tmp.IsSelected,

                                                     };


                            if (resultWholeSale.Any() && resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                                extendedOption.PriceAdjustmentForWholeSale =
                                    resultWholeSale.FirstOrDefault(
                                        x => x.Id == tmp.VehicleOptionId).
                                        PriceAdjustment.ToString("c0");

                            if (!String.IsNullOrEmpty(extendedOption.PriceAdjustmentForWholeSale) && !extendedOption.PriceAdjustmentForWholeSale.Equals("$0"))

                                returnVehicle.OptionalEquipment.Add(extendedOption);
                        }

                    }
                    else
                    {
                        foreach (var vehicleConfiguration in vehicleConfigArray)
                        {
                            vehicleConfiguration.Mileage = Mileage;
                            returnVehicle.SpecificKBBTrimList.Add(vehicleConfiguration);
                        }
                    }

                }
            }

            return returnVehicle;
        }

        public static TradeInVehicleModel GetKBBTradeInValue(int VehicleId, int Mileage, string SelectedOptions, string ZipCode)
        {
            return GetKBBTradeInValue(VehicleId, Mileage, SelectedOptions, ZipCode, false, 0);
        }

        public static TradeInVehicleModel GetKBBTradeInValue(int VehicleId, int Mileage, string SelectedOptions, string ZipCode, bool setPrice, decimal variance)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            string authenticationKey =
                m_serviceClient.Login(
                    System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(
                        CultureInfo.InvariantCulture),
                    System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(
                        CultureInfo.InvariantCulture));

            var vehicleConfig = m_serviceClient.GetVehicleConfigurationByVehicleId(authenticationKey,
                                                                                   ApplicationCategory.Consumer,
                                                                                   VehicleId, ZipCode,
                                                                                   DateTime.Now);

            vehicleConfig.Mileage = Mileage;

            var returnVehicle = new TradeInVehicleModel()
                                    {
                                        VehicleId = vehicleConfig.Id,
                                        SelectedOptions = SelectedOptions,
                                        SelectedYear = vehicleConfig.Year.Value,
                                        SelectedMake = vehicleConfig.Make.
                                            Value,
                                        SelectedModel = vehicleConfig.Model.
                                            Value,
                                        SelectedTrim = vehicleConfig.Trim.
                                            Value,
                                        SpecificKBBTrimList = new List<VehicleConfiguration>(),
                                        OptionalEquipment = new List<ExtendedEquipmentOption>(),
                                        Mileage = Mileage.ToString(CultureInfo.InvariantCulture),
                                        SelectedOptionAdjustment = 0,
                                        ValidVin = true,
                                    };


            var resultWholeSale =
                m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                    vehicleConfig.Id,
                                                                    ZipCode,
                                                                    PriceType.Wholesale,
                                                                    DateTime.Now,
                                                                    ApplicationCategory.
                                                                        Consumer);


            foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
            {
                var extendedOption = new ExtendedEquipmentOption()
                                         {
                                             VehicleOptionId = tmp.VehicleOptionId,
                                             DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                             DisplayName = tmp.DisplayName,
                                             IsSelected = tmp.IsSelected,

                                         };

                if (!String.IsNullOrEmpty(SelectedOptions) &&
                    SelectedOptions.Contains(tmp.VehicleOptionId.ToString(CultureInfo.InvariantCulture)))
                {

                    returnVehicle.SelectedOptionAdjustment +=
                        resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).PriceAdjustment;

                    returnVehicle.OptionalEquipment.Add(extendedOption);
                    returnVehicle.SelectedOptionList += extendedOption.DisplayName + ", ";
                }

            }


            if (!String.IsNullOrEmpty(returnVehicle.SelectedOptionList) && returnVehicle.SelectedOptionList.Contains(", "))

                returnVehicle.SelectedOptionList = returnVehicle.SelectedOptionList.Substring(0,
                                                                                              returnVehicle.
                                                                                                  SelectedOptionList.LastIndexOf(", ", System.StringComparison.Ordinal));



            var vehicleValue = m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey,
                                                                                                   vehicleConfig,
                                                                                                   ApplicationCategory.
                                                                                                       Consumer, ZipCode,
                                                                                                   DateTime.Now);


            if (vehicleValue.ValuationPrices.Any())
            {

                SetPrice(returnVehicle, vehicleValue, setPrice, variance);
            }
            else
            {
                returnVehicle.TradeInFairPrice = null;
                returnVehicle.TradeInGoodPrice = null;
            }




            return returnVehicle;
        }

        private static void SetPrice(TradeInVehicleModel returnVehicle, VehicleValuation vehicleValue, bool setPrice, decimal variance)
        {
            var tradeInFair = vehicleValue.ValuationPrices.FirstOrDefault(i => i.PriceType == PriceType.TradeInFair);
            var tradeInGood = vehicleValue.ValuationPrices.FirstOrDefault(i => i.PriceType == PriceType.TradeInGood);
            if (tradeInGood != null)
            {
                decimal calculatedGoodPrice = tradeInGood.Value + returnVehicle.SelectedOptionAdjustment;
                returnVehicle.TradeInGoodPrice = (tradeInGood.Value > 0) ? calculatedGoodPrice : 0;

                if (!setPrice)
                {
                    returnVehicle.TradeInFairPrice = (tradeInFair != null && tradeInFair.Value > 0) ? (tradeInFair.Value + returnVehicle.SelectedOptionAdjustment) : 0;
                }
                else
                {
                    returnVehicle.TradeInFairPrice = GetTradeInFairPrice(returnVehicle, tradeInFair, calculatedGoodPrice,variance);
                }
            }
        }

        private static decimal GetTradeInFairPrice(TradeInVehicleModel returnVehicle, Valuation tradeInFair, decimal calculatedGoodPrice, decimal variance)
        {
            if ((tradeInFair != null && tradeInFair.Value > 0))
            {
                if (returnVehicle.TradeInGoodPrice==0)
                {
                    return (tradeInFair.Value + returnVehicle.SelectedOptionAdjustment);
                }
                if (tradeInFair.Value + returnVehicle.SelectedOptionAdjustment + variance > calculatedGoodPrice)
                {
                    return (calculatedGoodPrice);
                }
                return (tradeInFair.Value + returnVehicle.SelectedOptionAdjustment + variance);
            }
            return 0;
        }


        public static string BuildKbbReportInHtml(int listingId, string kbbVehicleId, int trimId, DealershipViewModel dealer, int printOption)
        {
            var context = new VincontrolEntities();

            var row = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            var EffectiveDate = "";

            if (!DateTime.Now.DayOfWeek.Equals(DayOfWeek.Thursday))
                EffectiveDate = String.Format("{0:M/d/yyyy}", DateTime.Now) + "-" + String.Format("{0:M/d/yyyy}", CommonHelper.GetNextFriday());
            else
                EffectiveDate = String.Format("{0:M/d/yyyy}", DateTime.Now.AddDays(-7)) + "-" + String.Format("{0:M/d/yyyy}", DateTime.Now);

            var trimDetail = GetVehicleSpecificationFromKBB(kbbVehicleId, dealer, row.Mileage.GetValueOrDefault(), row.Vehicle.KBBOptionsId, printOption);

            var builder = new StringBuilder();

            builder.AppendLine(" <!DOCTYPE html>");

            builder.AppendLine("<html>");

            builder.AppendLine("<head>");

            builder.AppendLine("<title></title>");

            builder.AppendLine("</head>");

            builder.AppendLine("<body>");

            builder.AppendLine("<font face=\"Times\">");

            builder.AppendLine("<table width=\"550\" height=\"900\" align=\"center\" valign=\"middle\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"middle\" align=\"center\">");

            builder.AppendLine(" <table width=\"550\" height=\"750\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine(" <tr>");

            builder.AppendLine("  <td>");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"left\"> <b>License Plate # N/A</b></td>");

            if (String.IsNullOrEmpty(row.Stock))

                builder.AppendLine("<td align=\"right\"><b>Stock #N/A<br />" + String.Format("{0:MMM d, yyyy}", DateTime.Now) + "</b></td>");
            else
                builder.AppendLine("<td align=\"right\"><b>Stock " + row.Stock + "<br />" + String.Format("{0:MMM d, yyyy}", DateTime.Now) + "</b></td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"top\">");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"center\">");

            builder.AppendLine("<h2>");

            switch (printOption)
            {
                case 0:
                    builder.AppendLine("<b>Lending/Suggested Retail Breakdown</b><br />");
                    break;

                case 1:
                    builder.AppendLine("<b>Suggested Trade In Breakdown</b><br />");
                    break;
                case 2:
                    builder.AppendLine("<b>Suggested Wholesale Breakdown</b><br />");
                    break;
                case 3:
                    builder.AppendLine("<b>Suggested Retail Breakdown</b><br />");
                    break;

            }

            //builder.AppendLine("<b>Lending/Suggested Retail Breakdown</b><br />");

            builder.AppendLine("<b>Kelly Blue Book</b><br />");

            builder.AppendLine("<b>Effective Dates: " + EffectiveDate + "</b><br /><br />");

            builder.AppendLine("</h2>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"top\">");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"left\" colspan=\"2\"><b>" + GetVehicleNameFromKBB(trimDetail.VehicleConfiguration) + ".........</b></td>");

            builder.AppendLine("<td align=\"right\"><b>" + trimDetail.LendingRetailPriceWithoutAdditonalOption + "</b></td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"center\" colspan=\"3\"></td>");

            builder.AppendLine(" </tr>");

            builder.AppendLine(" <tr>");

            builder.AppendLine("<td align=\"center\" colspan=\"3\"><b>VIN: " + row.Vehicle.Vin + "</b></td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td width=\"100\"></td>");

            builder.AppendLine("<td align=\"left\">");

            builder.AppendLine("<b>" + trimDetail.Engine.Value + ".........................................</b><br />");

            builder.AppendLine("<b>" + trimDetail.Tranmission.Value + ".........</b><br />");

            builder.AppendLine("<b>" + trimDetail.DriveTrain.Value + "........................................................</b>");

            builder.AppendLine("</td>");

            builder.AppendLine("<td align=\"right\">");

            builder.AppendLine("<b>Included</b><br />");

            builder.AppendLine("<b>Included</b><br />");

            builder.AppendLine("<b>Included</b><br />");


            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"top\">");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");


            builder.AppendLine("   <td align=\"center\" colspan=\"2\"><b>*** Equipment ***</b></td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine("    <tr>");

            builder.AppendLine("  <td align=\"left\" valign=\"top\">");

            builder.AppendLine("  <table width=\"250\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("  <tr>");

            builder.AppendLine("<td width=\"150\" height=\"100\"  align=\"left\">");

            var filterEquipmentList = trimDetail.OptionalEquipment.Where(x => x.VehicleOptionId != trimDetail.Tranmission.Id && x.VehicleOptionId != trimDetail.Engine.Id && x.VehicleOptionId != trimDetail.DriveTrain.Id);

            //if (filterEquipmentList.Count() > 28)
            //    filterEquipmentList = filterEquipmentList.Take(28);

            int index = 0;

            if (filterEquipmentList.Count() % 2 == 0)

                index = filterEquipmentList.Count() / 2;
            else
            {
                index = filterEquipmentList.Count() / 2 + 1;
            }


            var partialList1 = filterEquipmentList.Take(index).ToArray();

            var partialList2 = filterEquipmentList.Skip(index).Take(Math.Min(index, filterEquipmentList.Count() - index)).ToArray();


            foreach (var tmp in partialList1)
            {

                builder.AppendLine(" <b>" + tmp.DisplayName + "</b><br />");

            }

            builder.AppendLine("  </td>");

            builder.AppendLine("  <td width=\"100\" height=\"100\" align=\"right\">");

            foreach (var tmp in partialList1)
            {
                if (!tmp.IsSelected)

                    builder.AppendLine(" <b>" + tmp.PriceAdjustmentForWholeSale + "/" + tmp.PriceAdjustmentForRetail + "</b><br />");
                else
                {
                    builder.AppendLine(" <b>Included</b><br />");
                }


            }

            builder.AppendLine("   </td>");

            builder.AppendLine("    </tr>");

            builder.AppendLine("   </table>");

            builder.AppendLine(" </td>");






            builder.AppendLine("   <td align=\"right\" valign=\"top\">");

            builder.AppendLine("  <table width=\"250\" valign cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("  <tr>");

            builder.AppendLine(" <td width=\"150\" height=\"100\" align=\"left\">");

            foreach (var tmp in partialList2)
            {

                builder.AppendLine(" <b>" + tmp.DisplayName + "</b><br />");


            }

            builder.AppendLine(" </td>");

            builder.AppendLine("  <td width=\"100\" height=\"100\" align=\"right\">");

            foreach (var tmp in partialList2)
            {
                if (!tmp.IsSelected)

                    builder.AppendLine(" <b>" + tmp.PriceAdjustmentForWholeSale + "/" + tmp.PriceAdjustmentForRetail + "</b><br />");
                else
                {
                    builder.AppendLine(" <b>Included</b><br />");
                }

            }


            builder.AppendLine("</td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine(" </table>");

            builder.AppendLine("   </td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine("  </table>");

            builder.AppendLine("  </td>");

            builder.AppendLine("   </tr>");


            builder.AppendLine("  <!-- ROW: Equipment -->");

            builder.AppendLine(" <!-- ROW: Fuel Economy/Summary -->");

            builder.AppendLine(" <tr>");

            builder.AppendLine("  <td valign=\"top\">");

            builder.AppendLine("<table width=\"500\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"center\" colspan=\"2\"><b>Fuel City/Hwy " + row.Vehicle.FuelEconomyCity + "/" + row.Vehicle.FuelEconomyHighWay + " MPG</b></td><br />");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr align=\"center\">");

            builder.AppendLine("<td   width=\"400\" align=\"left\">");

            builder.AppendLine("<b align=\"center\">Total Value without mileage................</b><br />");

            builder.AppendLine("<b align=\"center\">Mileage Adjustment(" + row.Mileage.GetValueOrDefault() + ") miles................</b><br /><br />");

            builder.AppendLine("</td>");

            builder.AppendLine("<td width=\"100\" align=\"right\">");

            builder.AppendLine("<b>" + trimDetail.LendingRetailPriceWithAdditonalOption + "</b><br />");

            builder.AppendLine("<b>" + trimDetail.MileageAdjustment.ToString("c0") + "</b><br />");

            builder.AppendLine("</td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine(" </table>");

            builder.AppendLine("<td align=\"left\">");

            builder.AppendLine("<table width=\"500\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"left\" width=\"400\">");

            switch (printOption)
            {
                case 0:
                    builder.AppendLine("<b>***Total Wholesale Lending/Suggested Retail Value</b>");
                    break;

                case 1:
                    builder.AppendLine("<b>***Total Suggested Trade In Value</b>");
                    break;
                case 2:
                    builder.AppendLine("<b>***Total Suggested Wholesale Value</b>");
                    break;
                case 3:
                    builder.AppendLine("<b>***Total Suggested Retail Value</b>");
                    break;

            }

            //builder.AppendLine("<b>***Total Wholesale Lending/Suggested Retail Value</b>");

            builder.AppendLine("</td>");

            builder.AppendLine("<td align=\"right\" width=\"100\">");

            builder.AppendLine("<b>" + trimDetail.FinalLendingRetailPrice + "</b><br />");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine(" </table>");

            builder.AppendLine("  </td>");

            builder.AppendLine("  </td>");

            builder.AppendLine("  </tr>	");

            builder.AppendLine("   </table>");

            builder.AppendLine(" </td>");

            builder.AppendLine("   </tr>");

            builder.AppendLine(" <!-- ROW: Footer -->");

            builder.AppendLine(" <tr align=\"center\">");

            if (filterEquipmentList.Count() <= 26)
            {
                builder.AppendLine(" <td align=\"center\" valign=\"bottom\"><br />");

                for (int i = 0; i < 13 - index; i++)
                {
                    builder.AppendLine("<br />");
                }

            }
            else
            {
                builder.AppendLine(" <td align=\"center\" valign=\"bottom\"><br/>");
                //for (int i = 0; i < 38; i++)
                //{
                //    builder.AppendLine("<br />");
                //}
            }

            //builder.AppendLine(" <td align=\"center\" valign=\"bottom\">");
            if (dealer.DealerSetting.OverrideDealerKbbReport)
            {
                var dealerGroupName =
                    context.DealerGroups.First(x => x.DealerGroupId == dealer.DealerGroupId).
                        DealerGroupName;

                builder.AppendLine("<h5>" + dealerGroupName + "</h5><br/>");    
            }
            else
            {
                builder.AppendLine("<h5>" + dealer.DealershipName + "</h5><br/>");    
            }
            

            builder.AppendLine("<h5>" + EffectiveDate + " Kelley Blue Book® KARPOWER Online's(SM) values for California. <br/> ");

            builder.AppendLine("Values are subjective opinions. Kelley Blue Book assumes no liability for errors<br/> ");

            builder.AppendLine("or omissions as to values, manufacturer or   dealer information.<br/> ");

            builder.AppendLine("©Copyright Kelley Blue Book 2012.All rights reserved.</h5>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</font>");

            builder.AppendLine("</body>");

            builder.AppendLine("</html>");

            return builder.ToString();

        }
        public static string BuildKBBReportInHtmlForAppraisal(int appraisalId, string kbbVehicleId, int trimId, DealershipViewModel dealer, int PrintOption)
        {

            var context = new VincontrolEntities();

            var row = context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);

            string EffectiveDate = "";

            if (!DateTime.Now.DayOfWeek.Equals(DayOfWeek.Thursday))
                EffectiveDate = String.Format("{0:M/d/yyyy}", DateTime.Now) + "-" + String.Format("{0:M/d/yyyy}", CommonHelper.GetNextFriday());
            else
                EffectiveDate = String.Format("{0:M/d/yyyy}", DateTime.Now.AddDays(-7)) + "-" + String.Format("{0:M/d/yyyy}", DateTime.Now);

            var trimDetail = GetVehicleSpecificationFromKBB(kbbVehicleId, dealer, row.Mileage.GetValueOrDefault(), row.Vehicle.KBBOptionsId, PrintOption);

            var builder = new StringBuilder();

            builder.AppendLine(" <!DOCTYPE html>");

            builder.AppendLine("<html>");

            builder.AppendLine("<head>");

            builder.AppendLine("<title></title>");

            builder.AppendLine("</head>");

            builder.AppendLine("<body>");

            builder.AppendLine("<font face=\"Times\">");

            builder.AppendLine("<table width=\"550\" height=\"900\" align=\"center\" valign=\"middle\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"middle\" align=\"center\">");

            builder.AppendLine(" <table width=\"550\" height=\"750\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine(" <tr>");

            builder.AppendLine("  <td>");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"left\"> <b>License Plate # N/A</b></td>");

            if (String.IsNullOrEmpty(row.Stock))

                builder.AppendLine("<td align=\"right\"><b>Stock #N/A<br />" + String.Format("{0:MMM d, yyyy}", DateTime.Now) + "</b></td>");
            else
                builder.AppendLine("<td align=\"right\"><b>Stock " + row.Stock + "<br />" + String.Format("{0:MMM d, yyyy}", DateTime.Now) + "</b></td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"top\">");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"center\">");

            builder.AppendLine("<h2>");

            switch (PrintOption)
            {
                case 0:
                    builder.AppendLine("<b>Lending/Suggested Retail Breakdown</b><br />");
                    break;

                case 1:
                    builder.AppendLine("<b>Suggested Trade In Breakdown</b><br />");
                    break;
                case 2:
                    builder.AppendLine("<b>Suggested Wholesale Breakdown</b><br />");
                    break;
                case 3:
                    builder.AppendLine("<b>Suggested Retail Breakdown</b><br />");
                    break;

            }

            //builder.AppendLine("<b>Lending/Suggested Retail Breakdown</b><br />");

            builder.AppendLine("<b>Kelly Blue Book</b><br />");

            builder.AppendLine("<b>Effective Dates: " + EffectiveDate + "</b><br /><br />");

            builder.AppendLine("</h2>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"top\">");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"left\" colspan=\"2\"><b>" + GetVehicleNameFromKBB(trimDetail.VehicleConfiguration) + ".........</b></td>");

            builder.AppendLine("<td align=\"right\"><b>" + trimDetail.LendingRetailPriceWithoutAdditonalOption + "</b></td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"center\" colspan=\"3\"></td>");

            builder.AppendLine(" </tr>");

            builder.AppendLine(" <tr>");

            builder.AppendLine("<td align=\"center\" colspan=\"3\"><b>VIN: " + row.Vehicle.Vin + "</b></td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td width=\"100\"></td>");

            builder.AppendLine("<td align=\"left\">");

            builder.AppendLine("<b>" + trimDetail.Engine.Value + ".........................................</b><br />");

            builder.AppendLine("<b>" + trimDetail.Tranmission.Value + ".........</b><br />");

            builder.AppendLine("<b>" + trimDetail.DriveTrain.Value + "........................................................</b>");

            builder.AppendLine("</td>");

            builder.AppendLine("<td align=\"right\">");

            builder.AppendLine("<b>Included</b><br />");

            builder.AppendLine("<b>Included</b><br />");

            builder.AppendLine("<b>Included</b><br />");


            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td valign=\"top\">");

            builder.AppendLine("<table width=\"550\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");


            builder.AppendLine("   <td align=\"center\" colspan=\"2\"><b>*** Equipment ***</b></td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine("    <tr>");

            builder.AppendLine("  <td align=\"left\" valign=\"top\">");

            builder.AppendLine("  <table width=\"250\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("  <tr>");

            builder.AppendLine("<td width=\"150\" height=\"100\"  align=\"left\">");

            //var bufferEquipmentList = trimDetail.OptionalEquipment.Where(x => x.VehicleOptionId != trimDetail.Tranmission.Id && x.VehicleOptionId != trimDetail.Engine.Id && x.VehicleOptionId != trimDetail.DriveTrain.Id);

            var filterEquipmentList = trimDetail.OptionalEquipment.Where(x => x.VehicleOptionId != trimDetail.Tranmission.Id && x.VehicleOptionId != trimDetail.Engine.Id && x.VehicleOptionId != trimDetail.DriveTrain.Id);

            //if (filterEquipmentList.Count() > 28)
            //    filterEquipmentList = filterEquipmentList.Take(28);

            int index = 0;

            if (filterEquipmentList.Count() % 2 == 0)

                index = filterEquipmentList.Count() / 2;
            else
            {
                index = filterEquipmentList.Count() / 2 + 1;
            }


            var partialList1 = filterEquipmentList.Take(index).ToArray();

            var partialList2 = filterEquipmentList.Skip(index).Take(Math.Min(index, filterEquipmentList.Count() - index)).ToArray();


            foreach (var tmp in partialList1)
            {

                builder.AppendLine(" <b>" + tmp.DisplayName + "</b><br />");

            }

            builder.AppendLine("  </td>");

            builder.AppendLine("  <td width=\"100\" height=\"100\" align=\"right\">");

            foreach (var tmp in partialList1)
            {
                if (!tmp.IsSelected)

                    builder.AppendLine(" <b>" + tmp.PriceAdjustmentForWholeSale + "/" + tmp.PriceAdjustmentForRetail + "</b><br />");
                else
                {
                    builder.AppendLine(" <b>Included</b><br />");
                }


            }

            builder.AppendLine("   </td>");

            builder.AppendLine("    </tr>");

            builder.AppendLine("   </table>");

            builder.AppendLine(" </td>");






            builder.AppendLine("   <td align=\"right\" valign=\"top\">");

            builder.AppendLine("  <table width=\"250\" valign cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("  <tr>");

            builder.AppendLine(" <td width=\"150\" height=\"100\" align=\"left\">");

            foreach (var tmp in partialList2)
            {

                builder.AppendLine(" <b>" + tmp.DisplayName + "</b><br />");


            }

            builder.AppendLine(" </td>");

            builder.AppendLine("  <td width=\"100\" height=\"100\" align=\"right\">");

            foreach (var tmp in partialList2)
            {
                if (!tmp.IsSelected)

                    builder.AppendLine(" <b>" + tmp.PriceAdjustmentForWholeSale + "/" + tmp.PriceAdjustmentForRetail + "</b><br />");
                else
                {
                    builder.AppendLine(" <b>Included</b><br />");
                }

            }


            builder.AppendLine("</td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine(" </table>");

            builder.AppendLine("   </td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine("  </table>");

            builder.AppendLine("  </td>");

            builder.AppendLine("   </tr>");


            builder.AppendLine("  <!-- ROW: Equipment -->");

            builder.AppendLine(" <!-- ROW: Fuel Economy/Summary -->");

            builder.AppendLine(" <tr>");

            builder.AppendLine("  <td valign=\"top\">");

            builder.AppendLine("<table width=\"500\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"center\" colspan=\"2\"><b>Fuel City/Hwy " + row.Vehicle.FuelEconomyCity + "/" + row.Vehicle.FuelEconomyHighWay + " MPG</b></td><br />");

            builder.AppendLine("</tr>");

            builder.AppendLine("<tr align=\"center\">");

            builder.AppendLine("<td   width=\"400\" align=\"left\">");

            builder.AppendLine("<b align=\"center\">Total Value without mileage................</b><br />");

            builder.AppendLine("<b align=\"center\">Mileage Adjustment(" +row.Mileage + ") miles................</b><br /><br />");

            builder.AppendLine("</td>");

            builder.AppendLine("<td width=\"100\" align=\"right\">");

            builder.AppendLine("<b>" + trimDetail.LendingRetailPriceWithAdditonalOption + "</b><br />");

            builder.AppendLine("<b>" + trimDetail.MileageAdjustment.ToString("c0") + "</b><br />");

            builder.AppendLine("</td>");

            builder.AppendLine("  </tr>");

            builder.AppendLine(" </table>");

            builder.AppendLine("<td align=\"left\">");

            builder.AppendLine("<table width=\"500\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

            builder.AppendLine("<tr>");

            builder.AppendLine("<td align=\"left\" width=\"400\">");

            switch (PrintOption)
            {
                case 0:
                    builder.AppendLine("<b>***Total Wholesale Lending/Suggested Retail Value</b>");
                    break;

                case 1:
                    builder.AppendLine("<b>***Total Suggested Trade In Value</b>");
                    break;
                case 2:
                    builder.AppendLine("<b>***Total Suggested Wholesale Value</b>");
                    break;
                case 3:
                    builder.AppendLine("<b>***Total Suggested Retail Value</b>");
                    break;

            }

            //builder.AppendLine("<b>***Total Wholesale Lending/Suggested Retail Value</b>");

            builder.AppendLine("</td>");

            builder.AppendLine("<td align=\"right\" width=\"100\">");

            builder.AppendLine("<b>" + trimDetail.FinalLendingRetailPrice + "</b><br />");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine(" </table>");

            builder.AppendLine("  </td>");

            builder.AppendLine("  </td>");

            builder.AppendLine("  </tr>	");

            builder.AppendLine("   </table>");

            builder.AppendLine(" </td>");

            builder.AppendLine("   </tr>");

            builder.AppendLine(" <!-- ROW: Footer -->");

            builder.AppendLine(" <tr align=\"center\">");

            if (filterEquipmentList.Count() <= 26)
            {
                builder.AppendLine(" <td align=\"center\" valign=\"bottom\"><br />");

                for (int i = 0; i < 13 - index; i++)
                {
                    builder.AppendLine("<br />");
                }

            }
            else
            {
                builder.AppendLine(" <td align=\"center\" valign=\"bottom\"><br/>");
                //for (int i = 0; i < 38; i++)
                //{
                //    builder.AppendLine("<br />");
                //}
            }



            builder.AppendLine("<h5>" + dealer.DealershipName + "</h5><br/>");

            builder.AppendLine("<h5>" + EffectiveDate + " Kelley Blue Book® KARPOWER Online's(SM) values for California. <br/> ");

            builder.AppendLine("Values are subjective opinions. Kelley Blue Book assumes no liability for errors<br/> ");

            builder.AppendLine("or omissions as to values, manufacturer or   dealer information.<br/> ");

            builder.AppendLine("©Copyright Kelley Blue Book 2012.All rights reserved.</h5>");

            builder.AppendLine("</td>");

            builder.AppendLine("</tr>");

            builder.AppendLine("</table>");

            builder.AppendLine("</font>");

            builder.AppendLine("</body>");

            builder.AppendLine("</html>");

            return builder.ToString();

        }

        private static KellyBlueBookTrimReport GetVehicleSpecificationFromKBB(string KBBVehicleId, DealershipViewModel dealer, long Mileage, string SavedOption, int PrintOption)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            string authenticationKey =
                m_serviceClient.Login(System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(CultureInfo.InvariantCulture),
                                      System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(CultureInfo.InvariantCulture));



            int Id = Convert.ToInt32(KBBVehicleId);

            decimal wholeSaleAdjustment = 0;

            decimal retailAdjustment = 0;


            var transPair = m_serviceClient.GetTransmissionsByVehicleId(authenticationKey,
                                                                                   ApplicationCategory.Dealer, Id,
                                                                                   dealer.ZipCode, DateTime.Now);

            var drivePair = m_serviceClient.GetDrivetrainsByVehicleId(authenticationKey,
                                                                                 ApplicationCategory.Dealer, Id,
                                                                                 dealer.ZipCode, DateTime.Now);

            var engPair = m_serviceClient.GetEnginesByVehicleId(authenticationKey, ApplicationCategory.Dealer,
                                                                           Id, dealer.ZipCode, DateTime.Now);

            var vehicleConfig = m_serviceClient.GetVehicleConfigurationByVehicleId(authenticationKey,
                                                                                              ApplicationCategory.Dealer,
                                                                                              Id, dealer.ZipCode,
                                                                                              DateTime.Now);

            var optionValuation = m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                                    Id, dealer.ZipCode,
                                                                                                    PriceType.Wholesale,
                                                                                                    DateTime.Now,
                                                                                                    ApplicationCategory.
                                                                                                        Dealer);
            var trimReport = new KellyBlueBookTrimReport()
            {
                TrimName = CommonHelper.TrimString(vehicleConfig.Trim.Value, 17),
                TrimId = vehicleConfig.Trim.Id,
                OptionValuation = optionValuation.ToList(),
                OptionalEquipment = new List<ExtendedEquipmentOption>(),
                VehicleId = vehicleConfig.Id,
                Tranmission = transPair.FirstOrDefault(),
                Engine = engPair.FirstOrDefault(),
                DriveTrain = drivePair.FirstOrDefault(),
                VehicleConfiguration = vehicleConfig,

            };


            var resultWholeSale =
                                         m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                                                   vehicleConfig.Id,
                                                                                           dealer.ZipCode,
                                                                                           PriceType.Wholesale,
                                                                                           DateTime.Now,
                                                                                           ApplicationCategory.
                                                                                               Dealer);


            var resultReatil =
            m_serviceClient.GetEquipmentOptionsValueByVehicleId(authenticationKey,
                                                                        vehicleConfig.Id,
                                                                        dealer.ZipCode,
                                                                        PriceType.Retail,
                                                                        DateTime.Now,
                                                                        ApplicationCategory.
                                                                            Dealer);

            foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => x.IsSelected))
            {
                var extendedOption = new ExtendedEquipmentOption()
                                         {
                                             VehicleOptionId = tmp.VehicleOptionId,
                                             DisplayNameAdditionalData = tmp.DisplayNameAdditionalData,
                                             DisplayName = tmp.DisplayName,
                                             IsSelected = tmp.IsSelected,

                                             PriceAdjustmentForWholeSale =
                                                 resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId && x.PriceType == PriceType.Wholesale).
                                                 PriceAdjustment.ToString("c0"),
                                             PriceAdjustmentForRetail =
                                                 resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId && x.PriceType == PriceType.Retail).
                                                 PriceAdjustment.ToString("c0"),

                                         };


                trimReport.OptionalEquipment.Add(extendedOption);
            }


            foreach (var tmp in vehicleConfig.OptionalEquipment.Where(x => !x.IsSelected))
            {

                var extendedOption = new ExtendedEquipmentOption()
                {
                    VehicleOptionId = tmp.VehicleOptionId,
                    DisplayNameAdditionalData =
                        tmp.DisplayNameAdditionalData,
                    DisplayName = tmp.DisplayName,
                    IsSelected = tmp.IsSelected,


                };
                if (resultWholeSale.Any(x => x.Id == tmp.VehicleOptionId))
                {

                    extendedOption.PriceAdjustmentForWholeSale =
                        resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                            PriceAdjustment.ToString("c0");


                }
                else
                {
                    extendedOption.PriceAdjustmentForWholeSale = "0";
                }

                if (resultReatil.Any(x => x.Id == tmp.VehicleOptionId))
                {
                    extendedOption.PriceAdjustmentForRetail =
                        resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                            PriceAdjustment.ToString("c0");
                }
                else
                {
                    extendedOption.PriceAdjustmentForRetail = "0";
                }

                if (!String.IsNullOrEmpty(SavedOption) &&
                    SavedOption.Contains(extendedOption.VehicleOptionId.ToString()))
                {
                    wholeSaleAdjustment +=
                        resultWholeSale.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                            PriceAdjustment;


                    retailAdjustment +=
                        resultReatil.FirstOrDefault(x => x.Id == tmp.VehicleOptionId).
                            PriceAdjustment;

                    extendedOption.IsSaved = true;

                    trimReport.OptionalEquipment.Add(extendedOption);
                }

            }



       
            vehicleConfig.Mileage =(int) Mileage;

            trimReport = GetVehicleValueFromKBB(trimReport, vehicleConfig, dealer, wholeSaleAdjustment, retailAdjustment, PrintOption);

            return trimReport;

        }

        private static string GetVehicleNameFromKBB(VehicleConfiguration vConfig)
        {
            return vConfig.Year.Value + " " + vConfig.Make.Value + " " + vConfig.Model.Value + " " + vConfig.Trim.Value;

        }

        private static KellyBlueBookTrimReport GetVehicleValueFromKBB(KellyBlueBookTrimReport kellyModel, VehicleConfiguration vehicleConfig, DealershipViewModel dealer, decimal wholeSaleAdjustment, decimal retailAdjustment, int PrintOption)
        {
            var m_serviceClient = new VehicleInformationService2008R2Client();

            string PriceWithoutMileage = "";

            string PriceWithoutMileageButOption = "";

            string PriceWithMileage = "";

            string authenticationKey =
              m_serviceClient.Login(System.Configuration.ConfigurationManager.AppSettings["KBBUserName"].ToString(CultureInfo.InvariantCulture),
                                    System.Configuration.ConfigurationManager.AppSettings["KBBPassword"].ToString(CultureInfo.InvariantCulture));


            var vehicleValue = m_serviceClient.GetVehicleValuesByVehicleConfigurationAllConditions(authenticationKey, vehicleConfig, ApplicationCategory.Dealer, dealer.ZipCode, DateTime.Now);

            kellyModel.MileageAdjustment = vehicleValue.MileageAdjustment;

            kellyModel.MileageZeroPoint = vehicleValue.MileageZeroPoint;



            switch (PrintOption)
            {
                case 0:
                    foreach (var va in vehicleValue.ValuationPrices)
                    {

                        if (va.PriceType == PriceType.Wholesale)
                        {

                            PriceWithoutMileage += vehicleValue.ValuationPrices.First(x => x.PriceType == PriceType.BaseWholesale).Value.ToString("c0");

                            PriceWithoutMileageButOption += (va.Value - vehicleValue.MileageAdjustment + wholeSaleAdjustment).ToString("c0");

                            PriceWithMileage += (va.Value + wholeSaleAdjustment).ToString("c0");


                            break;


                        }


                    }
                    foreach (var va in vehicleValue.ValuationPrices)
                    {

                        if (va.PriceType == PriceType.Retail)
                        {
                            PriceWithoutMileage += "/" + vehicleValue.ValuationPrices.First(x => x.PriceType == PriceType.BaseRetail).Value.ToString("c0");

                            PriceWithoutMileageButOption += "/" + (va.Value - vehicleValue.MileageAdjustment + retailAdjustment).ToString("c0");

                            PriceWithMileage += "/" + (va.Value + retailAdjustment).ToString("c0");
                            break;

                        }
                    }
                    break;

                case 1:
                    foreach (var va in vehicleValue.ValuationPrices)
                    {

                        if (va.PriceType == PriceType.TradeInFair)
                        {

                            PriceWithoutMileage = "N/A";

                            PriceWithoutMileageButOption = (va.Value - vehicleValue.MileageAdjustment + wholeSaleAdjustment).ToString("c0");

                            PriceWithMileage = (va.Value + wholeSaleAdjustment).ToString("c0");


                            break;


                        }


                    }

                    break;
                case 2:
                    foreach (var va in vehicleValue.ValuationPrices)
                    {

                        if (va.PriceType == PriceType.Wholesale)
                        {

                            PriceWithoutMileage = vehicleValue.ValuationPrices.First(x => x.PriceType == PriceType.BaseWholesale).Value.ToString("c0");

                            PriceWithoutMileageButOption = (va.Value - vehicleValue.MileageAdjustment + wholeSaleAdjustment).ToString("c0");

                            PriceWithMileage = (va.Value + wholeSaleAdjustment).ToString("c0");


                            break;


                        }


                    }
                    break;
                case 3:
                    foreach (var va in vehicleValue.ValuationPrices)
                    {

                        if (va.PriceType == PriceType.Retail)
                        {
                            PriceWithoutMileage = vehicleValue.ValuationPrices.First(x => x.PriceType == PriceType.BaseRetail).Value.ToString("c0");

                            PriceWithoutMileageButOption = (va.Value - vehicleValue.MileageAdjustment + retailAdjustment).ToString("c0");

                            PriceWithMileage = (va.Value + retailAdjustment).ToString("c0");
                            break;

                        }
                    }

                    break;

                default:
                    foreach (var va in vehicleValue.ValuationPrices)
                    {

                        if (va.PriceType == PriceType.Wholesale)
                        {
                            //decimal finalPrice = 0;

                            PriceWithoutMileage += vehicleValue.ValuationPrices.First(x => x.PriceType == PriceType.BaseWholesale).Value.ToString("c0");

                            PriceWithoutMileageButOption += (va.Value - vehicleValue.MileageAdjustment + wholeSaleAdjustment).ToString("c0");

                            PriceWithMileage += (va.Value + wholeSaleAdjustment).ToString("c0");


                            break;


                        }


                    }
                    foreach (var va in vehicleValue.ValuationPrices)
                    {

                        if (va.PriceType == PriceType.Retail)
                        {
                            PriceWithoutMileage += "/" + vehicleValue.ValuationPrices.First(x => x.PriceType == PriceType.BaseRetail).Value.ToString("c0");

                            PriceWithoutMileageButOption += "/" + (va.Value - vehicleValue.MileageAdjustment + retailAdjustment).ToString("c0");

                            PriceWithMileage += "/" + (va.Value + retailAdjustment).ToString("c0");
                            break;

                        }
                    }
                    break;
            }




            kellyModel.LendingRetailPriceWithoutAdditonalOption = PriceWithoutMileage;

            kellyModel.LendingRetailPriceWithAdditonalOption = PriceWithoutMileageButOption;

            kellyModel.FinalLendingRetailPrice = PriceWithMileage;

            return kellyModel;



        }

    }


}
