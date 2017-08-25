using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;

namespace vincontrol.WebAPI.Helper
{
    public sealed class WebApiCarFaxHelper
    {
        private static string CreateCarFaxXmlRequest(string Vin, string carFaxUsername, string carFaxPassword)
        {

            var builder = new StringBuilder();
            builder.AppendLine("<carfax-request>");
            builder.AppendLine("<vin>" + Vin + "</vin>");
            builder.AppendLine("<product-data-id>" + ConfigurationManager.AppSettings["CaxFaxProductDataId"] + "</product-data-id>");
            builder.AppendLine("<username>" + carFaxUsername + "</username>");
            builder.AppendLine("<password>" + carFaxPassword + "</password>");
            builder.AppendLine("<purchase>Y</purchase>");
            builder.AppendLine("<report-type>VHR</report-type>");
            builder.AppendLine("<online-listing>Y</online-listing>");
            builder.AppendLine("</carfax-request>");

            return builder.ToString();
        }

        public static void AddCarFaxReport(VincontrolEntities context, CarFaxViewModel carfax)
        {

            int number = 0;

            bool flag =Int32.TryParse(carfax.NumberofOwners, out number);

            var e = new Carfax()
            {
                Vin = carfax.Vin,
                DateStamp = DateTime.Now,
                LastUpdated = DateTime.Now,
                Expiration = DateTime.Now.AddDays((DateTime.Now.Day - 1)*-1).AddMonths(3),
                Owner = number,
                ServiceRecord = carfax.ServiceRecords,
                PriorRental = carfax.PriorRental,
                Accident = carfax.AccidentCounts,
                VehicleId = carfax.VehicleId


            };
            var builder = new StringBuilder();
            foreach (CarFaxWindowSticker tmp in carfax.ReportList)
            {
                builder.Append(tmp.Text);
                builder.Append("|");
                builder.Append(tmp.Image);
                builder.Append(",");
            }
            if (!String.IsNullOrEmpty(builder.ToString()))
            {
                builder.Remove(builder.ToString().Length - 1, 1);
                e.WindowSticker = builder.ToString();
            }


            context.AddToCarfaxes(e);


            context.SaveChanges();

        }

        public static XmlDocument MakeApiCall(string vin, string carFaxUsername, string carFaxPassword)
        {
            var requestBody = CreateCarFaxXmlRequest(vin, carFaxUsername, carFaxPassword);

            var xmlDoc = new XmlDocument();

            var apiServerUrl = ConfigurationManager.AppSettings["CaxFaxServerURL"];

            var request = (HttpWebRequest)WebRequest.Create(apiServerUrl);

            request.Method = "POST";

            request.ContentType = "text/xml";

            var encoding = new UTF8Encoding();

            var dataLen = encoding.GetByteCount(requestBody);

            var utf8Bytes = new byte[dataLen];

            Encoding.UTF8.GetBytes(requestBody, 0, requestBody.Length, utf8Bytes, 0);

            Stream str = null;
            try
            {
                //Set the request Stream
                str = request.GetRequestStream();
                //Write the request to the Request Steam
                str.Write(utf8Bytes, 0, utf8Bytes.Length);
                str.Close();
                //Get response into stream
                var response = request.GetResponse();
                str = response.GetResponseStream();

                // Get Response into String
                var sr = new StreamReader(str);
                xmlDoc.LoadXml(sr.ReadToEnd());
                sr.Close();
                str.Close();
            }
            catch (Exception Ex)
            {
                System.Web.HttpContext.Current.Response.Write("Errors=" + Ex.Message);
            }
            return xmlDoc;
        }
        
        public static CarFaxViewModel GetCarFaxReportInDatabase(VincontrolEntities context, int vehicleId)
        {
            var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>() };

            var carfaxRecord = context.Carfaxes.FirstOrDefault(x => x.VehicleId == vehicleId);

            if (carfaxRecord != null)
            {

                carfax.Vin = carfaxRecord.Vin;
                carfax.NumberofOwners = carfaxRecord.Owner.ToString();
                carfax.ServiceRecords = carfaxRecord.ServiceRecord.GetValueOrDefault();
                carfax.AccidentCounts = carfaxRecord.Accident.GetValueOrDefault();
                carfax.PriorRental = carfaxRecord.PriorRental.GetValueOrDefault();

                var stringList = carfaxRecord.WindowSticker.Split(new String[] { "," },
                    StringSplitOptions.RemoveEmptyEntries);

                if (carfax.AccidentCounts > 0)
                {
                    carfax.ReportList.Add(new CarFaxWindowSticker()
                    {
                        Text = carfax.AccidentCounts + " Accident(s) / Damage Reported to CARFAX",
                        Image = "http://vincontrol.com/alpha/content/CarfaxWarning.jpg"
                    });
                }

                if (carfax.PriorRental)
                {
                    carfax.ReportList.Add(new CarFaxWindowSticker()
                    {
                        Text = "Prior Rental",
                        Image = "http://vincontrol.com/alpha/content/CarfaxWarning.jpg"
                    });
                }


                foreach (string c in stringList)
                {
                    var pair = c.Split(new String[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    var sticker = new CarFaxWindowSticker()
                    {
                        Text = pair.First(),
                        Image = pair.Last()
                    };

                    carfax.ReportList.Add(sticker);

                }

                carfax.Success = true;
            }



            return carfax;
        }


        public static CarFaxViewModel GetCarFaxReportInDatabase(VinsellEntities context, int vehicleId)
        {
            var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>() };

            var carfaxRecord = context.manheim_Carfax.FirstOrDefault(x => x.VehicleId == vehicleId);

            if (carfaxRecord != null)
            {

                carfax.Vin = carfaxRecord.Vin;
                carfax.NumberofOwners = carfaxRecord.Owner.ToString();
                carfax.ServiceRecords = carfaxRecord.ServiceRecord.GetValueOrDefault();
                carfax.AccidentCounts = carfaxRecord.Accident.GetValueOrDefault();
                carfax.PriorRental = carfaxRecord.PriorRental.GetValueOrDefault();

                var stringList = carfaxRecord.WindowSticker.Split(new String[] { "," },
                    StringSplitOptions.RemoveEmptyEntries);

                if (carfax.AccidentCounts > 0)
                {
                    carfax.ReportList.Add(new CarFaxWindowSticker()
                    {
                        Text = carfax.AccidentCounts + " Accident(s) / Damage Reported to CARFAX",
                        Image = "http://vincontrol.com/alpha/content/CarfaxWarning.jpg"
                    });
                }

                if (carfax.PriorRental)
                {
                    carfax.ReportList.Add(new CarFaxWindowSticker()
                    {
                        Text = "Prior Rental",
                        Image = "http://vincontrol.com/alpha/content/CarfaxWarning.jpg"
                    });
                }


                foreach (string c in stringList)
                {
                    var pair = c.Split(new String[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    var sticker = new CarFaxWindowSticker()
                    {
                        Text = pair.First(),
                        Image = pair.Last()
                    };

                    carfax.ReportList.Add(sticker);

                }

                carfax.Success = true;
            }



            return carfax;
        }

    }
}