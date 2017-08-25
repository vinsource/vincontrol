using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using vincontrol.KBB;
using vincontrol.Manheim;
using Vincontrol.Vinsell.WindesktopVersion.Models;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;
using vincontrol.Helper;

namespace Vincontrol.Vinsell.WindesktopVersion.Helper
{
    public static class DataHelper
    {
        public static ChartGraph MarketData(string vin, IAuctionManagement manheimAuctionManagement)
        {
            try
            {
                var user = new UserViewModel();
                return MarketDataHelper.GetMarketCarsOnAutoTraderVersion2(manheimAuctionManagement.GetVehicle(vin), user);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<ManheimWholesaleViewModel> ManheimData(string vin, string manheimUsername, string manheimPassword, IAuctionManagement manheimAuctionManagement)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(vin))
                {
                    var model = manheimAuctionManagement.GetVehicle(vin);

                    if (model.Mmr > 0 && model.MmrAbove > 0 && model.MmrBelow > 0)
                    {
                        var newRecord = new ManheimWholesaleViewModel()
                        {
                            LowestPrice =Convert.ToDecimal(CommonHelper.ConvertToCurrency(model.MmrBelow.ToString(CultureInfo.InvariantCulture))),
                            AveragePrice = Convert.ToDecimal(CommonHelper.ConvertToCurrency(model.Mmr.ToString(CultureInfo.InvariantCulture))),
                            HighestPrice = Convert.ToDecimal(CommonHelper.ConvertToCurrency(model.MmrAbove.ToString(CultureInfo.InvariantCulture))),
                            Year = model.Year,
                            TrimName = model.Trim
                        };
                        result.Add(newRecord);
                    }
                    else
                    {
                        //manheimService.ExecuteByVin(SessionHandler.User.Setting.Manheim, SessionHandler.User.Setting.ManheimPassword, vin.Trim());
                        var manheimService = new ManheimService();
                        result = manheimService.ManheimReport(model, manheimUsername, manheimPassword);
                    }
                }
                else
                {
                    result = new List<ManheimWholesaleViewModel>();
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return result;
        }

        public static List<SmallKarPowerViewModel> KarpowerData(string vin, string mileage, string kellyBlueBook, string kellyPassword, int dealerId)
        {
            List<SmallKarPowerViewModel> result;
            try
            {
                result = LinqHelper.GetKbbReport(vin);
                if (result == null)
                {
                    var karpowerService = new KBBService();
                    //result = karpowerService.Execute(vin, mileage, DateTime.Now, kellyBlueBook, kellyPassword, "Inventory");
                }

                var savedKbbTrim = LinqHelper.GetSavedKbbTrim(vin, dealerId);
                if (savedKbbTrim != null)
                {
                    foreach (var smallKarPowerViewModel in result)
                    {
                        smallKarPowerViewModel.IsSelected = smallKarPowerViewModel.SelectedTrimId == savedKbbTrim.TrimId;
                    }
                }
            }
            catch (Exception)
            {
                result = new List<SmallKarPowerViewModel>();
            }

            return result;
        }

        public static CarFax GetCarfaxFromWebService(int dealerid,string vin)
        {
            string url = "http://vincontrol.com:4411/vinsell/GetCarFaxData/"+dealerid+"/" + vin;
            string strResult = string.Empty;
            // declare httpwebrequet wrt url defined above
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            // set method as post
            webrequest.Method = "GET";
            // set content type
            webrequest.ContentType = "application/x-www-form-urlencoded";
            // declare & read response from service
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            // set utf8 encoding
            //Encoding enc = System.Text.Encoding.GetEncoding("utf-8?");
            // read response stream from response object
            StreamReader loResponseStream = new StreamReader
                (webresponse.GetResponseStream());
            // read string from stream data
            strResult = loResponseStream.ReadToEnd();
            // close the stream object
            loResponseStream.Close();
            // close the response object
            webresponse.Close();
            // assign the final result to text box

            var carfax = Deserialize<CarFax>(strResult);

            return carfax;
        }

        private  static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

     
    }
}
