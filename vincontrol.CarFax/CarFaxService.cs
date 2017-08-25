using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using vincontrol.Application.ViewModels.Chart;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.ConfigurationManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using System.Xml.Serialization;

namespace vincontrol.CarFax
{
    public class CarFaxService : ICarFaxService
    {
        protected IUnitOfWork UnitOfWork;

        private const string CarfaxWarningImage = "http://vincontrol.com/alpha/content/CarfaxWarning.jpg";
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)";
        private const string ContentType = "text/xml;charset=\"utf-8\"";
        private const int ReadWriteTimeOut = 100000;
        private const int TimeOut = 100000;

        public CarFaxService() : this(new SqlUnitOfWork()) { }

        public CarFaxService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #region ICarFaxService's Members

        public CarFaxViewModel XmlSerializeCarFax(string vin, string carFaxUsername, string carFaxPassword)
        {
            var carfax = GetCarFaxReportInDatabase(vin);

            if (carfax != null)
            {
                carfax.Success = true;
                return carfax;
            }

            carfax = new CarFaxViewModel() { Success = false };

            var doc = MakeApiCall(vin, carFaxUsername, carFaxPassword);

            var root = doc["carfax-response"];

            carfax.ReportList = new List<CarFaxWindowSticker>();

            if (!String.IsNullOrEmpty(root.InnerText))
            {
                if (root["vin-info"] != null)
                {
                    try
                    {
                        carfax.Success = true;

                        carfax.Vin = root["vin-info"]["vin"].InnerText;

                        carfax.NumberofOwners = root["vin-info"]["number-of-owners-indicator"]["number-of-owners-indicator-value"].InnerText;
                        carfax.ServiceRecords = Convert.ToInt32(root["vin-info"]["number-of-service-records-indicator"].InnerText);
                        carfax.AccidentCountsXml = root["vin-info"]["accident-count"].InnerText;
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

                        //if (carfax.ExistDatabase == 0)
                        //    AddCarFaxReport(contextcarfax);
                        //else
                        //    UpdateCarFaxReport(carfax);
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
            }
            else
                carfax.Success = false;

            return carfax;
        }

        public XmlDocument MakeApiCall(string vin, string carFaxUsername, string carFaxPassword)
        {
            var requestBody = CreateXmlRequest(vin, carFaxUsername, carFaxPassword);

            var xmlDoc = new XmlDocument();
            var apiServerURL = ConfigurationHandler.CaxFaxServerURL;

            var request = (HttpWebRequest)WebRequest.Create(apiServerURL);
            request.Method = "POST";
            request.ContentType = "text/xml";

            var encoding = new UTF8Encoding();

            var dataLen = encoding.GetByteCount(requestBody);

            var utf8Bytes = new byte[dataLen];

            Encoding.UTF8.GetBytes(requestBody, 0, requestBody.Length, utf8Bytes, 0);
            try
            {
                //Set the request Stream
                var str = request.GetRequestStream();
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
            catch (Exception ex)
            {
                return null;
            }

            return xmlDoc;
        }

        public string GetVinNumberFromDetailUrl(string detailUrl)
        {
            var result = "";
            var htmlWeb = new HtmlWeb();

            try
            {
                var htmlDoc = htmlWeb.Load(detailUrl);
                var carfaxLinkNode = htmlDoc.DocumentNode.SelectSingleNode(".//a[@class='carFaxTileURL']");

                if (carfaxLinkNode != null && carfaxLinkNode.Attributes.Any(x => x.Name == "href"))
                {
                    var carFaxLink = "http://www.autotrader.com" + carfaxLinkNode.Attributes.First(x => x.Name == "href").Value;

                    htmlDoc = htmlWeb.Load(carFaxLink);

                    var selectNodesCarVin = htmlDoc.DocumentNode.SelectSingleNode(".//span[@class='vin']") ?? htmlDoc.DocumentNode.SelectSingleNode(".//dd[@id='vinNumber']");

                    if (selectNodesCarVin != null) result = selectNodesCarVin.InnerText.Trim();
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public CarFaxViewModel ConvertXmlToCarFaxModelAndSave(string vin, string carFaxUsername, string carFaxPassword)
        {
            var carfax = GetCarFaxReportInDatabase(vin);

            if (carfax.ExistDatabase == 1)
            {
                carfax.Success = true;
                return carfax;
            }

            var doc = MakeApiCall(vin, carFaxUsername, carFaxPassword);

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
                        carfax.ServiceRecords = Convert.ToInt32(root["vin-info"]["number-of-service-records-indicator"].InnerText);
                        carfax.AccidentCountsXml = root["vin-info"]["accident-count"].InnerText;
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
                                for (var i = 0; i < content.ChildNodes.Count; i++)
                                {
                                    if (!content.ChildNodes[i].InnerText.Equals("Rental")) continue;
                                    carfax.PriorRental = true;
                                    break;
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(root["vin-info"]["WindowSticker"]["AccidentIndicators"]["AccidentIndicatorsText"].InnerText))
                        {
                            var imageURL = root["vin-info"]["WindowSticker"]["AccidentIndicators"]["CheckmarkImage"].InnerText.Replace("_bw", "");
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
                            UnitOfWork.CarFax.UpdatePriorRental(vin);
                            UnitOfWork.CommitVincontrolModel();
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

                        //if (carfax.ExistDatabase == 0)
                        //{
                        //    UnitOfWork.CarFax.AddCarFaxReport(AddCarFaxReport(carfax));
                        //    UnitOfWork.CommitVincontrolModel();
                        //}
                        //else
                        //{
                        //    UpdateCarFaxReport(carfax);
                        //    UnitOfWork.CommitVincontrolModel();
                        //}
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

            return carfax;
        }

        public CarFaxViewModel ConvertXmlToCarFaxModelAndSave(int vehicleId, string vin, string carFaxUsername, string carFaxPassword)
        {

            var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>() };

            var doc = MakeApiCall(vin, carFaxUsername, carFaxPassword);

            var root = doc["carfax-response"];

            if (!String.IsNullOrEmpty(root.InnerText))
            {
                if (root["vin-info"] != null)
                {
                    try
                    {
                        string textTmp = root["vin-info"]["number-of-owners-indicator"]["number-of-owners-indicator-value"].InnerText;
                        var owner = 0;
                        Int32.TryParse(textTmp, out owner);
                        carfax.NumberofOwners = owner.ToString();
                        carfax.ServiceRecords = Convert.ToInt32(root["vin-info"]["number-of-service-records-indicator"].InnerText);
                        carfax.AccidentCountsXml = root["vin-info"]["accident-count"].InnerText;
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
                            var cars = UnitOfWork.Inventory.GetInventories(vehicleId);
                            foreach (var item in cars)
                            {
                                item.PriorRental = true;
                            }
                            if (cars.Any())
                            {
                                UnitOfWork.CommitVincontrolModel();
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

                        if(vehicleId>0)
                            carfax.VehicleId = vehicleId;

                        carfax.Vin = vin;

                        AddCarFaxReport(carfax);
                    }
                    catch (Exception)
                    {
                        carfax.Success = false;
                    }
                }
            }

            return carfax;
        }


        public CarFaxViewModel ConvertXmlToCarFaxModelForVinsell(int vehicleId, string vin, string carFaxUsername, string carFaxPassword)
        {

            var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>() };

            var doc = MakeApiCall(vin, carFaxUsername, carFaxPassword);

            var root = doc["carfax-response"];

            if (!String.IsNullOrEmpty(root.InnerText))
            {
                if (root["vin-info"] != null)
                {
                    try
                    {
                        string textTmp = root["vin-info"]["number-of-owners-indicator"]["number-of-owners-indicator-value"].InnerText;
                        var owner = 0;
                        Int32.TryParse(textTmp, out owner);
                        carfax.NumberofOwners = owner.ToString();
                        carfax.ServiceRecords = Convert.ToInt32(root["vin-info"]["number-of-service-records-indicator"].InnerText);
                        carfax.AccidentCountsXml = root["vin-info"]["accident-count"].InnerText;
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

                        if (vehicleId > 0)
                            carfax.VehicleId = vehicleId;

                        carfax.Vin = vin;

                        AddCarFaxReportForVinsell(carfax);
                    }
                    catch (Exception)
                    {
                        carfax.Success = false;
                    }
                }
            }

            return carfax;
        }


        public CarFaxViewModel GetCarFaxReportInDatabase(string vin)
        {
            var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>(), NumberofOwners = "" };

            var existingCarFaxReport = UnitOfWork.CarFax.GetCarFaxReportByVin(vin);

            int status = UnitOfWork.CarFax.CheckVinHasCarFaxReport(existingCarFaxReport);

            if (status == 1)
            {
                try
                {
                    carfax.Vin = existingCarFaxReport.Vin;
                    carfax.NumberofOwners = existingCarFaxReport.Owner.ToString();
                    carfax.ServiceRecords = existingCarFaxReport.ServiceRecord.GetValueOrDefault();
                    carfax.AccidentCounts = existingCarFaxReport.Accident.GetValueOrDefault();
                    carfax.PriorRental = existingCarFaxReport.PriorRental.GetValueOrDefault();

                    var stringList = existingCarFaxReport.WindowSticker.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    if (carfax.AccidentCounts > 0)
                    {
                        carfax.ReportList.Add(new CarFaxWindowSticker()
                        {
                            Text = carfax.AccidentCounts + " Accident(s) / Damage Reported to CARFAX",
                            Image = CarfaxWarningImage
                        });
                    }

                    if (carfax.PriorRental)
                    {
                        carfax.ReportList.Add(new CarFaxWindowSticker()
                        {
                            Text = "Prior Rental",
                            Image = CarfaxWarningImage
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

        public CarFaxViewModel GetCarFaxReportInDatabase(int vehicleId)
        {
            var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>() };

            var carfaxRecord = UnitOfWork.CarFax.GetCarFaxReportByVehicleId(vehicleId);

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

        public string GetCarFaxReportLinkFromAutoTrader(int autotraderId)
        {
            var url = "http://www.autotrader.com/cars-for-sale/vehicledetails.xhtml?listingId=" + autotraderId;
            var xmlDocument = DownloadDocument(url);
            var carfaxNode = xmlDocument.SelectSingleNode("//*[@class='vehicle-tile atcui-clear'][last()-1]/*/@href");
            return carfaxNode != null && !carfaxNode.Value.Equals("/finance_insurance/insurance.jsp") ? carfaxNode.Value : string.Empty;
        }

        #endregion

        #region Private Methods

        private string CreateXmlRequest(string vin, string carFaxUsername, string carFaxPassword)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<carfax-request>");
            builder.AppendLine("<vin>" + vin + "</vin>");
            builder.AppendLine("<product-data-id>" + ConfigurationHandler.CaxFaxProductDataId + "</product-data-id>");
            builder.AppendLine("<username>" + carFaxUsername + "</username>");
            builder.AppendLine("<password>" + carFaxPassword + "</password>");
            builder.AppendLine("<purchase>Y</purchase>");
            builder.AppendLine("<report-type>VHR</report-type>");
            builder.AppendLine("<online-listing>Y</online-listing>");
            builder.AppendLine("</carfax-request>");

            return builder.ToString();
        }

        private void AddCarFaxReport(CarFaxViewModel carfax)
        {
            var owner = String.IsNullOrEmpty(carfax.NumberofOwners) ? 0 : Convert.ToInt32(carfax.NumberofOwners);

            if (owner > 0)
            {
                var e = new Carfax()
                {
                    Vin = carfax.Vin,
                    DateStamp = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Expiration = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3),
                    Owner = owner,
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

                UnitOfWork.CarFax.AddCarFaxReport(e);
                UnitOfWork.CommitVincontrolModel();
            }
        }

        private void AddCarFaxReportForVinsell(CarFaxViewModel carfax)
        {
            var owner = String.IsNullOrEmpty(carfax.NumberofOwners) ? 0 : Convert.ToInt32(carfax.NumberofOwners);

            if (owner > 0)
            {
                var e = new manheim_Carfax()
                {
                    Vin = carfax.Vin,
                    DateStamp = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Expiration = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3),
                    Owner = owner,
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

                UnitOfWork.CarFax.AddCarFaxReportForVinsell(e);
                UnitOfWork.CommitVinSell();
            }
        }

        private XmlDocument DownloadDocument(string url)
        {
            var content = GetContentFromWebPage(url, 3);
            var doc = new XmlDocument { PreserveWhitespace = true, XmlResolver = null };
            var i = content.IndexOf("<rss");
            if (i == -1)
            {
                using (var xhtmlConverter = new Sgml.SgmlReader())
                {
                    xhtmlConverter.DocType = "HTML";
                    xhtmlConverter.WhitespaceHandling = WhitespaceHandling.All;
                    xhtmlConverter.CaseFolding = Sgml.CaseFolding.ToLower;
                    xhtmlConverter.InputStream = new System.IO.StringReader(content);
                    doc.Load(xhtmlConverter);
                    xhtmlConverter.Close();
                }
            }
            else
            {
                content = content.Substring(i);
                doc.LoadXml(content);
            }

            return doc;
        }

        private string GetContentFromWebPage(string url, int retryNumber)
        {
            string content = string.Empty;

            //at least, try to do once
            retryNumber = retryNumber > 0 ? retryNumber : 1;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = ContentType;
            request.UserAgent = UserAgent;
            request.Accept = "application/xml";
            request.KeepAlive = false;
            request.ReadWriteTimeout = ReadWriteTimeOut;
            request.Timeout = TimeOut;
            request.AllowWriteStreamBuffering = false;

            while (retryNumber > 0)
            {
                try
                {
                    retryNumber--;
                    using (var response = request.GetResponse())
                    {
                        using (var objStream = response.GetResponseStream())
                        {
                            using (var objReader = new StreamReader(objStream))
                            {
                                content = objReader.ReadToEnd();
                                objReader.Close();
                                objReader.Dispose();

                            }
                            objStream.Flush();
                            objStream.Close();
                            objStream.Dispose();
                        }
                        response.Close();
                    }


                    request = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
                catch (OutOfMemoryException ex)
                {
                    Console.WriteLine("Common.cs - DownloadWebPage(): OutOfMemoryException \n" + ex.Message);
                    GC.Collect();
                    break;
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Common.cs - DownloadWebPage(): WebException \n" + ex.Message);
                    GC.Collect();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Common.cs - DownloadWebPage(): Exception \n" + ex.Message);
                    GC.Collect();
                    break;
                }
                finally
                {
                    GC.Collect();
                }
                retryNumber = 0;
            }

            return content;
        }

        #endregion
    }
}
