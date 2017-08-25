using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Linq;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;
//using Vincontrol.Web.com.nationalautoresearch.www;
using Vincontrol.Web.Models;
using Vincontrol.Web.NationalAutosearch;


namespace Vincontrol.Web.HelperClass
{
    public class BlackBookService
    {
       
        private static DataTable GetValuesFromVin(string Vin, int Mileage,string State)
        {
            //UsedCarWS bbService = new UsedCarWS();

            //UserCredentials uc = new UserCredentials()
            //{
            //    userid = "whitman",
            //    password = "w3v1nbb1",
            //    producttype = "W",
            //    customer = "",

            //};

            //bbService.UserCredentialsValue = uc;

            //DataSet ds = bbService.getVINValues("U", "W", Vin, Mileage, State, 0, 0, 0, 0, false, true, true);

            //int sCode = bbService.UserCredentialsValue.returncode;

            //string sMessage = bbService.UserCredentialsValue.returnmessage;

            //if (sCode == 0)
            //{
            //    DataTable dtValues = ds.Tables["values"];
            //    //DataTable dtYears = ds.Tables["years"];
            //    //DataTable dtMakes = ds.Tables["makes"];
            //    //DataTable dtModels = ds.Tables["models"];
            //    //DataTable dtSeries = ds.Tables["series"];
            //    //DataTable dtStyles = ds.Tables["styles"];

            //    return dtValues;
            //}

            return null;
        }


        //public static BlackBookViewModel GetDealerPrice(string Vin,int Mileage,string State)
        //{
        //    BlackBookViewModel blackbook = new BlackBookViewModel()
        //    {
        //        Vin=Vin
        //    };


        //    DataTable dt = GetValuesFromVin(Vin, Mileage,State);

        //    if (dt != null)
        //    {
        //        blackbook.TrimList = new List<BlackBookTrimDetail>();

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            BlackBookTrimDetail trimDetail = new BlackBookTrimDetail()
        //            {
        //                TrimName = dr.Field<string>("series")
        //            };
        //            trimDetail.MaximumPrice = dr.Field<Decimal>("tradein_clean").ToString("c0");
        //            trimDetail.AveragePrice = dr.Field<Decimal>("tradein_avg").ToString("c0");
        //            trimDetail.MinimumPrice = dr.Field<Decimal>("tradein_rough").ToString("c0");

        //            blackbook.TrimList.Add(trimDetail);
        //        }
        //        blackbook.Success = true; 
                 
        //    }
        //    else
        //        blackbook.Success = false;


        //    return blackbook;

        //}

        public static BlackBookViewModel GetDirectFullReport(string Vin, long Mileage, string State)
        {
            BlackBookViewModel blackbook = new BlackBookViewModel()
            {
                Vin = Vin

            };


            if (!String.IsNullOrEmpty(Vin))
            {
              


                        Hashtable hashUnique = new Hashtable();


                        DataTable dt = GetValuesFromVin(Vin,(int) Mileage, State);

                        if (dt != null)
                        {
                            blackbook.TrimReportList = new List<BlackBookTrimReport>();

                            foreach (DataRow dr in dt.Rows)
                            {
                                BlackBookTrimReport trimDetail = new BlackBookTrimReport();

                                if (!String.IsNullOrEmpty(dr["series"].ToString()))
                                    trimDetail.TrimName = dr.Field<string>("series");
                                else
                                    trimDetail.TrimName = dr.Field<string>("model");


                                if (!hashUnique.Contains(trimDetail.TrimName))
                                {
                                    hashUnique.Add(trimDetail.TrimName, trimDetail.TrimName);

                                    trimDetail.BaseWholeSaleExtraClean = dr.Field<Decimal>("base_whole_xclean").ToString("c0");
                                    trimDetail.BaseWholeSaleClean = dr.Field<Decimal>("base_whole_clean").ToString("c0");
                                    trimDetail.BaseWholeSaleAvg = dr.Field<Decimal>("base_whole_avg").ToString("c0");
                                    trimDetail.BaseWholeSaleRough = dr.Field<Decimal>("base_whole_rough").ToString("c0");
                                    trimDetail.WholeSaleExtraClean = dr.Field<Decimal>("whole_xclean").ToString("c0");
                                    trimDetail.WholeSaleClean = dr.Field<Decimal>("whole_clean").ToString("c0");
                                    trimDetail.WholeSaleAvg = dr.Field<Decimal>("whole_avg").ToString("c0");
                                    trimDetail.WholeSaleRough = dr.Field<Decimal>("whole_rough").ToString("c0");
                                    trimDetail.MileageAdjWholeSaleExtraClean = dr.Field<Decimal>("mileage_adj_whole_xclean").ToString("c0");
                                    trimDetail.MileageAdjWholeSaleClean = dr.Field<Decimal>("mileage_adj_whole_clean").ToString("c0");
                                    trimDetail.MileageAdjWholeSaleAvg = dr.Field<Decimal>("mileage_adj_whole_avg").ToString("c0");
                                    trimDetail.MileageAdjWholeSaleRough = dr.Field<Decimal>("mileage_adj_whole_rough").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjWholeSaleExtraClean = dr.Field<Decimal>("ad_adj_whole_xclean").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjWholeSaleClean = dr.Field<Decimal>("ad_adj_whole_clean").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjWholeSaleAvg = dr.Field<Decimal>("ad_adj_whole_avg").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjWholeSaleRough = dr.Field<Decimal>("ad_adj_whole_rough").ToString("c0");
                                    trimDetail.BaseRetailExtraClean = dr.Field<Decimal>("base_retail_xclean").ToString("c0");
                                    trimDetail.BaseRetailClean = dr.Field<Decimal>("base_retail_clean").ToString("c0");
                                    trimDetail.BaseRetailAvg = dr.Field<Decimal>("base_retail_avg").ToString("c0");
                                    trimDetail.BaseRetailRough = dr.Field<Decimal>("base_retail_rough").ToString("c0");
                                    trimDetail.RetailExtraClean = dr.Field<Decimal>("retail_xclean").ToString("c0");
                                    trimDetail.RetailaClean = dr.Field<Decimal>("retail_clean").ToString("c0");
                                    trimDetail.RetailAvg = dr.Field<Decimal>("retail_avg").ToString("c0");
                                    trimDetail.RetailRough = dr.Field<Decimal>("retail_rough").ToString("c0");
                                    trimDetail.MileageAdjRetailExtraClean = dr.Field<Decimal>("mileage_adj_retail_xclean").ToString("c0");
                                    trimDetail.MileageAdjRetailClean = dr.Field<Decimal>("mileage_adj_retail_clean").ToString("c0");
                                    trimDetail.MileageAdjRetailAvg = dr.Field<Decimal>("mileage_adj_retail_avg").ToString("c0");
                                    trimDetail.MileageAdjRetailRough = dr.Field<Decimal>("mileage_adj_retail_rough").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjRetailExtraClean = dr.Field<Decimal>("ad_adj_retail_xclean").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjRetailClean = dr.Field<Decimal>("ad_adj_retail_clean").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjRetailAvg = dr.Field<Decimal>("ad_adj_retail_avg").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjRetailRough = dr.Field<Decimal>("ad_adj_retail_rough").ToString("c0");
                                    trimDetail.TradeInClean = dr.Field<Decimal>("tradein_clean");
                                    trimDetail.TradeInAvg = dr.Field<Decimal>("tradein_avg");
                                    trimDetail.TradeInRough = dr.Field<Decimal>("tradein_rough");
                                    trimDetail.BaseTradeInClean = dr.Field<Decimal>("base_tradein_clean").ToString("c0");
                                    trimDetail.BaseTradeInAvg = dr.Field<Decimal>("base_tradein_avg").ToString("c0");
                                    trimDetail.BaseTradeInRough = dr.Field<Decimal>("base_tradein_rough").ToString("c0");
                                    trimDetail.MileageAdjTradeInClean = dr.Field<Decimal>("mileage_adj_tradein_clean").ToString("c0");
                                    trimDetail.MileageAdjTradeInAvg = dr.Field<Decimal>("mileage_adj_tradein_avg").ToString("c0");
                                    trimDetail.MileageAdjTradeInRough = dr.Field<Decimal>("mileage_adj_tradein_rough").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjTradeInClean = dr.Field<Decimal>("ad_adj_tradein_clean").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjTradeInAvg = dr.Field<Decimal>("ad_adj_tradein_avg").ToString("c0");
                                    trimDetail.ManualOrAutomaticAdjTradeInRough = dr.Field<Decimal>("ad_adj_tradein_rough").ToString("c0");


                                    blackbook.TrimReportList.Add(trimDetail);

                                }
                               

                            }

                            if (blackbook.TrimReportList.Count > 0)
                            {
                                blackbook.Success = true;
                                
                            }
                        }
                        else
                            blackbook.Success = false;
                    }
                    else
                        blackbook.Success = false;
                
     
            
            return blackbook;
        }


        public static BlackBookViewModel GetFullReport(string Vin, long Mileage, string State)
        {
            var blackbook = new BlackBookViewModel()
            {
                Vin = Vin

            };


            //if (!String.IsNullOrEmpty(Vin))
            //{
            //    blackbook = GetBBModelInDatabase(Vin);

            //    if (blackbook.ExistDatabase==1)
            //    {
            //        blackbook.Success = true;
            //        return blackbook;
            //    }
            //    else
            //    {

            //        int MileageResult = 0;

            //        bool flag = Int32.TryParse(Mileage, out MileageResult);

            //        if (flag)
            //        {

            //            Hashtable hashUnique = new Hashtable();


            //            DataTable dt = GetValuesFromVin(Vin, MileageResult, State);

            //            if (dt != null)
            //            {
            //                blackbook.TrimReportList = new List<BlackBookTrimReport>();

            //                foreach (DataRow dr in dt.Rows)
            //                {
            //                    BlackBookTrimReport trimDetail = new BlackBookTrimReport();

            //                    if (!String.IsNullOrEmpty(dr["series"].ToString()))
            //                        trimDetail.TrimName = dr.Field<string>("series");
            //                    else
            //                        trimDetail.TrimName = dr.Field<string>("model");


            //                    if (!hashUnique.Contains(trimDetail.TrimName))
            //                    {
            //                        hashUnique.Add(trimDetail.TrimName, trimDetail.TrimName);

            //                        trimDetail.BaseWholeSaleExtraClean = dr.Field<Decimal>("base_whole_xclean").ToString("c0");
            //                        trimDetail.BaseWholeSaleClean = dr.Field<Decimal>("base_whole_clean").ToString("c0");
            //                        trimDetail.BaseWholeSaleAvg = dr.Field<Decimal>("base_whole_avg").ToString("c0");
            //                        trimDetail.BaseWholeSaleRough = dr.Field<Decimal>("base_whole_rough").ToString("c0");
            //                        trimDetail.WholeSaleExtraClean = dr.Field<Decimal>("whole_xclean").ToString("c0");
            //                        trimDetail.WholeSaleClean = dr.Field<Decimal>("whole_clean").ToString("c0");
            //                        trimDetail.WholeSaleAvg = dr.Field<Decimal>("whole_avg").ToString("c0");
            //                        trimDetail.WholeSaleRough = dr.Field<Decimal>("whole_rough").ToString("c0");
            //                        trimDetail.MileageAdjWholeSaleExtraClean = dr.Field<Decimal>("mileage_adj_whole_xclean").ToString("c0");
            //                        trimDetail.MileageAdjWholeSaleClean = dr.Field<Decimal>("mileage_adj_whole_clean").ToString("c0");
            //                        trimDetail.MileageAdjWholeSaleAvg = dr.Field<Decimal>("mileage_adj_whole_avg").ToString("c0");
            //                        trimDetail.MileageAdjWholeSaleRough = dr.Field<Decimal>("mileage_adj_whole_rough").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjWholeSaleExtraClean = dr.Field<Decimal>("ad_adj_whole_xclean").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjWholeSaleClean = dr.Field<Decimal>("ad_adj_whole_clean").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjWholeSaleAvg = dr.Field<Decimal>("ad_adj_whole_avg").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjWholeSaleRough = dr.Field<Decimal>("ad_adj_whole_rough").ToString("c0");
            //                        trimDetail.BaseRetailExtraClean = dr.Field<Decimal>("base_retail_xclean").ToString("c0");
            //                        trimDetail.BaseRetailClean = dr.Field<Decimal>("base_retail_clean").ToString("c0");
            //                        trimDetail.BaseRetailAvg = dr.Field<Decimal>("base_retail_avg").ToString("c0");
            //                        trimDetail.BaseRetailRough = dr.Field<Decimal>("base_retail_rough").ToString("c0");
            //                        trimDetail.RetailExtraClean = dr.Field<Decimal>("retail_xclean").ToString("c0");
            //                        trimDetail.RetailaClean = dr.Field<Decimal>("retail_clean").ToString("c0");
            //                        trimDetail.RetailAvg = dr.Field<Decimal>("retail_avg").ToString("c0");
            //                        trimDetail.RetailRough = dr.Field<Decimal>("retail_rough").ToString("c0");
            //                        trimDetail.MileageAdjRetailExtraClean = dr.Field<Decimal>("mileage_adj_retail_xclean").ToString("c0");
            //                        trimDetail.MileageAdjRetailClean = dr.Field<Decimal>("mileage_adj_retail_clean").ToString("c0");
            //                        trimDetail.MileageAdjRetailAvg = dr.Field<Decimal>("mileage_adj_retail_avg").ToString("c0");
            //                        trimDetail.MileageAdjRetailRough = dr.Field<Decimal>("mileage_adj_retail_rough").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjRetailExtraClean = dr.Field<Decimal>("ad_adj_retail_xclean").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjRetailClean = dr.Field<Decimal>("ad_adj_retail_clean").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjRetailAvg = dr.Field<Decimal>("ad_adj_retail_avg").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjRetailRough = dr.Field<Decimal>("ad_adj_retail_rough").ToString("c0");
            //                        trimDetail.TradeInClean = dr.Field<Decimal>("tradein_clean").ToString("c0");
            //                        trimDetail.TradeInAvg = dr.Field<Decimal>("tradein_avg").ToString("c0");
            //                        trimDetail.TradeInRough = dr.Field<Decimal>("tradein_rough").ToString("c0");
            //                        trimDetail.BaseTradeInClean = dr.Field<Decimal>("base_tradein_clean").ToString("c0");
            //                        trimDetail.BaseTradeInAvg = dr.Field<Decimal>("base_tradein_avg").ToString("c0");
            //                        trimDetail.BaseTradeInRough = dr.Field<Decimal>("base_tradein_rough").ToString("c0");
            //                        trimDetail.MileageAdjTradeInClean = dr.Field<Decimal>("mileage_adj_tradein_clean").ToString("c0");
            //                        trimDetail.MileageAdjTradeInAvg = dr.Field<Decimal>("mileage_adj_tradein_avg").ToString("c0");
            //                        trimDetail.MileageAdjTradeInRough = dr.Field<Decimal>("mileage_adj_tradein_rough").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjTradeInClean = dr.Field<Decimal>("ad_adj_tradein_clean").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjTradeInAvg = dr.Field<Decimal>("ad_adj_tradein_avg").ToString("c0");
            //                        trimDetail.ManualOrAutomaticAdjTradeInRough = dr.Field<Decimal>("ad_adj_tradein_rough").ToString("c0");


            //                        blackbook.TrimReportList.Add(trimDetail);

            //                    }


            //                }

            //                if (blackbook.TrimReportList.Count > 0)
            //                {
            //                    blackbook.Success = true;

            //                    if (blackbook.ExistDatabase == 0)

            //                        SQLHelper.AddSimpleBBReport(blackbook);
            //                    else if (blackbook.ExistDatabase == 2)

            //                        SQLHelper.UpdateSimpleBBReport(blackbook);
                                
            //                }
            //            }
            //            else
            //                blackbook.Success = false;
            //        }
            //        else
            //            blackbook.Success = false;
            //    }
            //}
            //else
            //    blackbook.Success = false;

            blackbook.Success = false;

            return blackbook;
        }

        public static BlackBookViewModel GetBBModelInDatabase(string Vin)
        {
            var dealerPrice = new BlackBookViewModel()
            {
                Vin = Vin,
                TrimReportList=new List<BlackBookTrimReport>()

            };
            int status = SQLHelper.CheckVinHasBbReport(Vin);
            if (status ==1)
            {

                var context = new VincontrolEntities();

                var list = context.BlackBooks.Where(x => x.Vin == Vin);
                
                foreach (BlackBook bb in list)
                {
                    BlackBookTrimReport TrimReport = new BlackBookTrimReport()
                    {
                        TradeInRough =bb.TradeInFairPrice.GetValueOrDefault(),
                        TradeInAvg =bb.TradeInGoodPrice.GetValueOrDefault(),
                        TradeInClean = bb.TradeInVeryGoodPrice.GetValueOrDefault(),
                        TrimName=bb.Trim

                    };
                    dealerPrice.TrimReportList.Add(TrimReport);
                    dealerPrice.ExistDatabase = status;

                }
            }
            else
            {
                dealerPrice.ExistDatabase = status;
            }
            return dealerPrice;
        }

        public static DataTable GetDealerPriceTable(string Vin, int Mileage)
        {
            DataTable dt = GetValuesFromVin(Vin, Mileage,"CA");

            return dt;


        }


    }
}
