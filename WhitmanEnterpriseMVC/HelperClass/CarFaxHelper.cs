using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.DatabaseModel;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public sealed class CarFaxHelper
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

        public static XmlDocument MakeApiCall(string Vin, string carFaxUsername, string carFaxPassword)
        {
            var requestBody = CreateCarFaxXmlRequest(Vin, carFaxUsername, carFaxPassword);

            var xmlDoc = new XmlDocument();

            var apiServerURL = ConfigurationManager.AppSettings["CaxFaxServerURL"];

            var request = (HttpWebRequest)WebRequest.Create(apiServerURL);

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

        public static void GetXmlContent(string Vin, string carFaxUsername, string carFaxPassword)
        {
            var doc = MakeApiCall(Vin, carFaxUsername, carFaxPassword);
            doc.Save("D:\\duyvo.xml");
        }

        public static CarFaxViewModel ConvertXmlToCarFaxModelAndSave(string Vin, string carFaxUsername, string carFaxPassword)
        {

            var carfax = GetCarFaxReportInDatabase(Vin);

            if (carfax.ExistDatabase == 1)
            {
                carfax.Success = true;
                return carfax;
            }
            else
            {

                var doc = MakeApiCall(Vin, carFaxUsername, carFaxPassword);

                var root = doc["carfax-response"];

                carfax.ReportList = new List<CarFaxWindowSticker>();

                if (!String.IsNullOrEmpty(root.InnerText))
                {
                    if (root["vin-info"] != null)
                    {
                        try
                        {
                            carfax.Vin = root["vin-info"]["vin"].InnerText;


                            carfax.NumberofOwners = root["vin-info"]["number-of-owners-indicator"]["number-of-owners-indicator-value"].InnerText;
                            carfax.ServiceRecords = root["vin-info"]["number-of-service-records-indicator"].InnerText;
                            carfax.AccidentCountsXml= root["vin-info"]["accident-count"].InnerText;
                            carfax.AccidentIndicator = root["vin-info"]["accident-indicator"].InnerText.Equals("N") ? false : true;
                            carfax.FrameDamageIndicator = root["vin-info"]["frame-damage-indicator"]["frame-damage-value"].InnerText.Equals("N") ? false : true;
                            carfax.BrandedTitleIndicator = root["vin-info"]["branded-title-indicator"]["branded-title-indicator-value"].InnerText.Equals("N") ? false : true;
                            carfax.BuyBackGuarantee = root["vin-info"]["bbg-indicator"]["bbg-indicator-value"].InnerText.Equals("N") ? false : true;
                            carfax.MajorProblemIndicator = root["vin-info"]["major-problem-indicator"]["major-problem-value"].InnerText.Equals("N") ? false : true;
                            carfax.DamageIndicator = root["vin-info"]["damage-indicator"]["damage-indicator-value"].InnerText.Equals("N") ? false : true;
                            
                            carfax.PriorRental = false;
                            
                            if (root["vin-info"] != null)
                            {
                                var content = root["vin-info"]["types-of-use"];
                                if (content != null)
                                {
                                    for (int i = 0; i < content.ChildNodes.Count; i++)
                                    {
                                        if (content.ChildNodes[i].InnerText.Equals("Rental"))
                                        {
                                            carfax.PriorRental = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!String.IsNullOrEmpty(root["vin-info"]["WindowSticker"]["AccidentIndicators"]["AccidentIndicatorsText"].InnerText))
                            {
                                string imageURL = root["vin-info"]["WindowSticker"]["AccidentIndicators"]["CheckmarkImage"].InnerText.Replace("_bw", "");
                                var tmp = new CarFaxWindowSticker()
                                {
                                    Text = root["vin-info"]["WindowSticker"]["AccidentIndicators"]["AccidentIndicatorsText"].InnerText,
                                    Image = imageURL
                                };

                                carfax.ReportList.Add(tmp);
                            }

                            int result;

                            if (int.TryParse(carfax.AccidentCountsXml, out result) && result > 0)
                            {
                                carfax.AccidentCounts = result;

                            }

                            if (carfax.PriorRental)
                            {
                            
                                var context = new whitmanenterprisewarehouseEntities();
                                var cars =
                                    context.whitmanenterprisedealershipinventories.Where(i => i.VINNumber.Equals(Vin));
                                foreach (var item in cars)
                                {
                                    item.PriorRental = true;
                                }
                                if (cars.Any())
                                {
                                    context.SaveChanges();
                                }
                            }

                            if (!String.IsNullOrEmpty(root["vin-info"]["WindowSticker"]["AirbagDeployment"]["AirbagDeploymentText"].InnerText))
                            {
                                string imageURL = root["vin-info"]["WindowSticker"]["AirbagDeployment"]["CheckmarkImage"].InnerText.Replace("_bw", "");
                                var tmp = new CarFaxWindowSticker()
                                {
                                    Text = root["vin-info"]["WindowSticker"]["AirbagDeployment"]["AirbagDeploymentText"].InnerText,
                                    Image = imageURL
                                };
                                carfax.ReportList.Add(tmp);

                            }

                            if (!String.IsNullOrEmpty(root["vin-info"]["WindowSticker"]["FrameDamage"]["FrameDamageText"].InnerText))
                            {
                                string imageURL = root["vin-info"]["WindowSticker"]["FrameDamage"]["CheckmarkImage"].InnerText.Replace("_bw", "");
                                var tmp = new CarFaxWindowSticker()
                                {
                                    Text = root["vin-info"]["WindowSticker"]["FrameDamage"]["FrameDamageText"].InnerText,
                                    Image = imageURL
                                };
                                carfax.ReportList.Add(tmp);

                            }
                            if (!String.IsNullOrEmpty(root["vin-info"]["WindowSticker"]["ManufacturerRecall"]["ManufacturerRecallText"].InnerText))
                            {
                                string imageURL = root["vin-info"]["WindowSticker"]["ManufacturerRecall"]["CheckmarkImage"].InnerText.Replace("_bw", "");
                                var tmp = new CarFaxWindowSticker()
                                {
                                    Text = root["vin-info"]["WindowSticker"]["ManufacturerRecall"]["ManufacturerRecallText"].InnerText,
                                    Image = imageURL
                                };
                                carfax.ReportList.Add(tmp);

                            }
                            if (!String.IsNullOrEmpty(root["vin-info"]["WindowSticker"]["OdometerRollback"]["OdometerRollbackText"].InnerText))
                            {
                                string imageURL = root["vin-info"]["WindowSticker"]["OdometerRollback"]["CheckmarkImage"].InnerText.Replace("_bw", "");
                                var tmp = new CarFaxWindowSticker()
                                {
                                    Text = root["vin-info"]["WindowSticker"]["OdometerRollback"]["OdometerRollbackText"].InnerText,
                                    Image = imageURL
                                };
                                carfax.ReportList.Add(tmp);

                            }

                            if (!String.IsNullOrEmpty(root["vin-info"]["WindowSticker"]["Disclaimer"].InnerText))
                                carfax.Disclaimer = root["vin-info"]["WindowSticker"]["Disclaimer"].InnerText;

                            carfax.Success = true;

                            if (carfax.ExistDatabase == 0)

                                SQLHelper.AddCarFaxReport(carfax);
                            else
                            {

                                SQLHelper.UpdateCarFaxReport(carfax);
                            }


                        }
                        catch (Exception)
                        {
                            carfax.Success = false;
                        }
                    }
                    else
                    {
                        carfax.Success = false;
                    }

                    carfax.CarFaxXMLResponse = root.InnerXml;
                }
                else
                    carfax.Success = false;
            }

            return carfax;
        }

        public static CarFaxViewModel GetCarFaxReportInDatabase(string Vin)
        {
            var carfax = new CarFaxViewModel {ReportList = new List<CarFaxWindowSticker>()};

            int status = SQLHelper.CheckVinHasCarFaxReport(Vin);

            if (status == 1)
            {
                var context = new whitmanenterprisewarehouseEntities();

                var tmp = context.whitmanenteprisecarfaxes.First(x => x.Vin.Equals(Vin));
                try
                {
                    carfax.Vin = tmp.Vin;
                    carfax.NumberofOwners = tmp.Owner.ToString();
                    carfax.ServiceRecords = tmp.ServiceRecord;
                    carfax.AccidentCounts = tmp.Accident.GetValueOrDefault();
                    carfax.PriorRental = tmp.PriorRental.GetValueOrDefault();

                    var stringList = tmp.WindowSticker.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

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

                    carfax.ExistDatabase = status;

                }
                catch (Exception)
                {
                    carfax.ExistDatabase = status;
                }


            }
            else
                carfax.ExistDatabase = status;

            return carfax;
        }

        public static string GetVinNumberFromDetailUrl(string detailUrl)
        {

            var result = "";

            var htmlWeb = new HtmlWeb();

            try
            {
                var htmlDoc = htmlWeb.Load(detailUrl);
                var carfaxLinkNode = htmlDoc.DocumentNode.SelectSingleNode(".//a[@class='carFaxTileURL']");

                if (carfaxLinkNode != null && carfaxLinkNode.Attributes.Any(x => x.Name == "href"))
                {
                    var carFaxLink = "http://www.autotrader.com" +
                                        carfaxLinkNode.Attributes.First(x => x.Name == "href").Value;

                    htmlDoc = htmlWeb.Load(carFaxLink);

                    var selectNodesCarVin = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='vin']");

                    if (selectNodesCarVin == null)
                    {
                        selectNodesCarVin = htmlDoc.DocumentNode.SelectSingleNode(".//dd[@id='vinNumber']");
                    }

                    if (selectNodesCarVin != null)
                        result = selectNodesCarVin.InnerText.Trim();
                }

            }
            catch (Exception ex)
            {

            }

            return result;

        }

    }



}
