using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Net.Mail;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using Vincontrol.Web.Objects;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Vincontrol.Web.Models;
using vincontrol.Helper;

namespace Vincontrol.Web.HelperClass
{
    public sealed class EmailHelper
    {
        public static void SendEmail(IEnumerable<string> toAddress, string subject, string body)
        {
            try
            {
                var smtpServerAddress =
              ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage();

                message.From = fromAddress;

                message.IsBodyHtml = true;

                message.Body = body;

                message.Subject = "VinControl Notification - " + subject;

                foreach (var tmp in toAddress.Distinct().AsEnumerable())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(new MailAddress(tmp));

                }
                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                            CultureInfo.InvariantCulture));


                client.Credentials = ntlmAuthentication;


                client.Send(message);

                SendBackUpEmail(subject, body);
            }
            catch (Exception)
            {
               
            }

          

        }

        public static void SendEmail(IEnumerable<string> toAddress, string subject, string body, MemoryStream pdfContent)
        {
            try
            {
                var smtpServerAddress =
              ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage();

                message.From = fromAddress;

                message.IsBodyHtml = true;

                message.Body = body;

                message.Subject = "VinControl Notification - " + subject;

                foreach (var tmp in toAddress.Distinct().AsEnumerable())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(new MailAddress(tmp));

                }
                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                            CultureInfo.InvariantCulture));


                client.Credentials = ntlmAuthentication;

                if (pdfContent != null)
                {
                    var attach = new Attachment(pdfContent, "Report.pdf", "application/pdf");

                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);
                }

                client.Send(message);

                SendBackUpEmail(subject, body);
            }
            catch (Exception)
            {

            }
        }

        public static void SendEmail(MailAddress toAddress, string subject, string body)
        {
            try
            {
                var smtpServerAddress =
                   ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage(fromAddress, toAddress);

                message.From = fromAddress;

                message.Body = body;

                message.IsBodyHtml = true;


                message.Subject = "VinControl Notification - " + subject;

                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                            CultureInfo.InvariantCulture));

                client.Credentials = ntlmAuthentication;

                client.Send(message);
            }
            catch (Exception)
            {
                
            }
          
        }

        public static void SendEmailForTradeInBanner(MailAddress toAddress, string subject, string bodyEmail, string bodyPDF)
        {
            try
            {
                var smtpServerAddress =
                   ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);

                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["TradeInEmail"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                            CultureInfo.InvariantCulture));

                client.Credentials = ntlmAuthentication;

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["TradeInEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["TradeInDisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage(fromAddress, toAddress)
                    {
                        From = fromAddress,
                        Body = bodyEmail,
                        IsBodyHtml = true,
                        Subject = subject
                    };


                //if (!String.IsNullOrEmpty(bodyPDF))
                //{
                //    var workStream = PDFHelper.WritePDF(bodyPDF);

                //    var attach = new System.Net.Mail.Attachment(workStream, "TradeInValue.pdf", "application/pdf");

                //    /* Attach the newly created email attachment */
                //    message.Attachments.Add(attach);

                //}

               

                client.Send(message);
             

            }
            catch (Exception)
            {
                
            }

          
        }

        public static void SendEmail(IEnumerable<string> toAddress, string subject, string body, string fileUrl)
        {
            try
            {
                var smtpServerAddress =
              ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage();

                message.From = fromAddress;

                message.IsBodyHtml = true;

                message.Body = body;

                message.Subject = "VinControl Notification - " + subject;

                foreach (var tmp in toAddress.Distinct().AsEnumerable())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(new MailAddress(tmp));

                }
                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                            CultureInfo.InvariantCulture));


                client.Credentials = ntlmAuthentication;

                if (fileUrl != null)
                {
                    var attach = new Attachment(fileUrl, "application/pdf");

                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);
                }

                client.Send(message);

                SendBackUpEmail(subject, body);
            }
            catch (Exception)
            {

            }
        }

        public static void SendEmailForTradeInBannerToDealer(List<string> toAddress, string subject, string body,string tradeinBody)
        {
            try
            {
                var smtpServerAddress =
                    ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);

                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["TradeInEmail"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                            CultureInfo.InvariantCulture));

                client.Credentials = ntlmAuthentication;

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["TradeInEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName =
                    ConfigurationManager.AppSettings["TradeInDisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage();

                message.From = fromAddress;

                foreach (var tmp in toAddress.Distinct().AsEnumerable())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(new MailAddress(tmp));

                }

                message.From = fromAddress;

                message.Body = body;

                message.Subject = subject;
                               
                var workStream = new MemoryStream();

                if (!String.IsNullOrEmpty(tradeinBody))
                {

                    var reader = new StringReader(tradeinBody);
                    var document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;
                    var worker = new HTMLWorker(document);
                    document.Open();
                    worker.StartDocument();
                    worker.Parse(reader);
                    worker.EndDocument();
                    document.Close();
                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    var attach = new Attachment(workStream, "TradeInValue.pdf", "application/pdf");

                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);

                }
                client.Send(message);

                //*****************SEND BACK UP MAIL********************************

                message = new MailMessage(fromAddress,  new MailAddress(
                                System.Configuration.ConfigurationManager.AppSettings["TradeInTrackEmailAccount"].ToString(CultureInfo.InvariantCulture)));

                message.Body = body;

                message.Subject = subject;

                if (!String.IsNullOrEmpty(tradeinBody))
                {
                    workStream = new MemoryStream();

                    var reader = new StringReader(tradeinBody);
                    var document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;
                    var worker = new HTMLWorker(document);
                    document.Open();
                    worker.StartDocument();
                    worker.Parse(reader);
                    worker.EndDocument();
                    document.Close();
                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    var attach = new Attachment(workStream, "TradeInValue.pdf", "application/pdf");

                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);

                }

               client.Send(message);

            }
            catch (Exception)
            {

            }


        }


        public static void SendPhoneEmail(IEnumerable<string> toAddress, string subject, string body)
        {

            var smtpServerAddress = ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);

            var defaultFromEmail = ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(CultureInfo.InvariantCulture);

            var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

            var fromAddress = new MailAddress(defaultFromEmail, displayName);

            var message = new MailMessage {From = fromAddress, Body = body};

            foreach (var tmp in toAddress)
            {
                message.To.Add(new MailAddress(tmp + "@tomomail.net"));

            }


            var client = new SmtpClient()
            {
                Host = smtpServerAddress,
                Port = 587,
                EnableSsl = true
            };

            var ntlmAuthentication = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["DefaultFromEmail"].ToString(CultureInfo.InvariantCulture), System.Configuration.ConfigurationManager.AppSettings["DefaultPass"].ToString(CultureInfo.InvariantCulture));

            client.UseDefaultCredentials = true;

            client.Credentials = ntlmAuthentication;


            client.Send(message);
        }
        public static void SendMaintenanceEmail()
        {
            var info = MaintenanceInfo.GetServerMaintenance(0);
            var emailList = GetDealerEmailList().ToList();
            var subject = string.Format("Server maintenance from {0} to {1}",info.DateStart, info.DateEnd);
            var body = string.Format("Server maintenance from {0} to {1}", info.DateStart, info.DateEnd);
            //SendEmail(emailList, subject, body);
        }

        private static IEnumerable<string> GetDealerEmailList()
        {
            var context = new VincontrolEntities();
            return context.Dealers.Where(i => !String.IsNullOrEmpty(i.Email)).Select(i => i.Email);
        }

   
       

        public static string CreateBodyEmailForSendClientRequestForAdf(TradeInVehicleModel vehicle, DealershipViewModel dealer)
        {

            var builder = new StringBuilder();

            builder.Append("<?xml version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<?adf version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<adf>" + Environment.NewLine);

            builder.Append("<prospect>" + Environment.NewLine);

            builder.Append("<requestdate>" +System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ",System.Globalization.CultureInfo.InvariantCulture) +"</requestdate>" + Environment.NewLine);

            builder.Append("<status>" + "used" + "</status>" + Environment.NewLine);

            builder.Append("<vehicle>" + Environment.NewLine);

            builder.Append("<year>" + vehicle.SelectedYear + "</year>" + Environment.NewLine);

            builder.Append("<make>" + vehicle.SelectedMakeValue + "</make>" + Environment.NewLine);

            builder.Append("<model>" + vehicle.SelectedModelValue + "</model>" + Environment.NewLine);

            builder.Append("<vin>" + vehicle.Vin + "</vin>" + Environment.NewLine);

            builder.Append("<stock></stock>" + Environment.NewLine);

            builder.Append("<trim>" + vehicle.SelectedTrim + "</trim>" + Environment.NewLine);


            builder.Append("<odometer status='replaced' units='miles'>" + CommonHelper.FormatNumberInThousand(vehicle.Mileage) + "</odometer>" + Environment.NewLine);

            builder.Append("</vehicle>" + Environment.NewLine);

            builder.Append("<customer>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + vehicle.CustomerFirstName + " " + vehicle.CustomerLastName + "</name>" +
                           Environment.NewLine);

            builder.Append("<phone>" + vehicle.CustomerPhone + "</phone>" + Environment.NewLine);

            builder.Append("<email>" + vehicle.CustomerEmail + "</email>" + Environment.NewLine);

            builder.Append("<address type=\"home\">" + Environment.NewLine);

            builder.Append("<city></city>" + Environment.NewLine);

            builder.Append("</address>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("<comments>I would like to trade in " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + " for " + (!vehicle.TradeInFairPrice.HasValue ? "Call Dealer For Price" : vehicle.TradeInFairPrice.Value.ToString()) + "" + "</comments>");

            builder.Append("</customer>" + Environment.NewLine);

            builder.Append("<vendor>" + Environment.NewLine);

            builder.Append("<vendorname></vendorname>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + dealer.DealershipName + "</name>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</vendor>" + Environment.NewLine);

            builder.Append("<provider>" + Environment.NewLine);

            builder.Append("<name part=\"full\">Vincontrol</name>" + Environment.NewLine);

            builder.Append("<service>Digital Marketing Solutions</service>" + Environment.NewLine);

            builder.Append("<url>http://vinclapp.com/</url>" + Environment.NewLine);

            builder.Append("<email>jeff@vincontrol.com</email>" + Environment.NewLine);

            builder.Append("<phone>855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<contact primarycontact=\"1\">" + Environment.NewLine);

            builder.Append("<name part=\"full\">VinClapp Service</name>" + Environment.NewLine);

            builder.Append("<email>alerts@vehicleinventorynetwork.com</email>" + Environment.NewLine);

            builder.Append("<phone type=\"voice\" time=\"day\">855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<address>" + Environment.NewLine);

            builder.Append("<street line=\"1\">9881 Irivne Center Dr</street>" + Environment.NewLine);

            builder.Append("<street line=\"2\"></street>" + Environment.NewLine);

            builder.Append("<city>Irvine</city>" + Environment.NewLine);

            builder.Append("<regioncode>CA</regioncode>" + Environment.NewLine);

            builder.Append("<postalcode>92618</postalcode>" + Environment.NewLine);

            builder.Append("<country>US</country>" + Environment.NewLine);

            builder.Append("</address>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</provider>" + Environment.NewLine);

            builder.Append("</prospect>" + Environment.NewLine);

            builder.Append("</adf>" + Environment.NewLine);



            return builder.ToString();
        }

 
        public static string CreateBodyEmailForAddToInventory(DealershipViewModel dealer, AppraisalViewFormModel appraisal)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");
            builder.AppendFormat(
                "<table width=\"500\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");
            builder.AppendFormat("	<td colspan=\"2\" style=\"padding-top: 40px; border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("<span style=\" font-weight: bold;\">VinControl Alert - Add New Car To Inventory</span><br />");
            builder.AppendFormat(appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.AppraisalModel + "<br />");
            builder.AppendFormat("Vin : " + appraisal.VinNumber + "<br />");
            builder.AppendFormat("</td>");
            builder.AppendFormat("<td align=\"right\" width=\"150\" style=\"border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat(
                "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; background: #111111; color: white;\">");
            builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.AppraisalModel + " was just added to inventory by " + appraisal.AppraisalById + " from " + dealer.DealershipName + "</p>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "<td colspan=\"3\" style=\"color: white; background: #003399; padding: 5px; font-weight: bold;\">1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span></td>");
            builder.AppendFormat("	</tr>");
            builder.AppendFormat("</table>");
            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");

            return builder.ToString();

        }


        public static string CreateBodyEmailForAddToInventory(DealershipViewModel dealer, int ListingId,string person)
        {
            var context = new VincontrolEntities();

            var car = context.Inventories.FirstOrDefault(x => x.InventoryId == ListingId);


            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");
            builder.AppendFormat(
                "<table width=\"500\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");
            builder.AppendFormat("	<td colspan=\"2\" style=\"padding-top: 40px; border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("<span style=\" font-weight: bold;\">VinControl Alert - Add New Car To Inventory</span><br />");
            builder.AppendFormat(car.Vehicle.Year + " " + car.Vehicle.Make + " " + car.Vehicle.Model + car.Vehicle.Trim + "<br />");
            builder.AppendFormat("Vin : " + car.Vehicle.Vin + "<br />");
            builder.AppendFormat("Stock : " + car.Stock + "<br />");
            builder.AppendFormat("</td>");
            builder.AppendFormat("<td align=\"right\" width=\"150\" style=\"border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat(
                "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; background: #111111; color: white;\">");
            builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 car.Vehicle.Year + " " + car.Vehicle.Make + " " + car.Vehicle.Model + car.Vehicle.Trim + " was just added to inventory by " + person + " from " + dealer.DealershipName + "</p>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "<td colspan=\"3\" style=\"color: white; background: #003399; padding: 5px; font-weight: bold;\">1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span></td>");
            builder.AppendFormat("	</tr>");
            builder.AppendFormat("</table>");
            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");

            return builder.ToString();

        }
        public static string CreateBodyEmailForAddToWholeSale(DealershipViewModel dealer, int ListingId, string person)
        {
            var context = new VincontrolEntities();

            var car = context.Inventories.FirstOrDefault(x => x.InventoryId == ListingId);


            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");
            builder.AppendFormat(
                "<table width=\"500\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");
            builder.AppendFormat("	<td colspan=\"2\" style=\"padding-top: 40px; border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("<span style=\" font-weight: bold;\">VinControl Alert - Add New Car To Inventory</span><br />");
            builder.AppendFormat(car.Vehicle.Year + " " + car.Vehicle.Make + " " + car.Vehicle.Model + car.Vehicle.Trim + "<br />");
            builder.AppendFormat("Vin : " + car.Vehicle.Vin + "<br />");
            builder.AppendFormat("Stock : " + car.Stock + "<br />");
            builder.AppendFormat("</td>");
            builder.AppendFormat("<td align=\"right\" width=\"150\" style=\"border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat(
               "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; background: #111111; color: white;\">");
            builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 car.Vehicle.Year + " " + car.Vehicle.Make + " " + car.Vehicle.Model + car.Vehicle.Trim + " was just added to inventory by " + person + " from " + dealer.DealershipName + "</p>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "<td colspan=\"3\" style=\"color: white; background: #003399; padding: 5px; font-weight: bold;\">1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span></td>");
            builder.AppendFormat("	</tr>");
            builder.AppendFormat("</table>");
            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");

            return builder.ToString();

        }
        public static string CreateBodyEmailForUpdateSalePrice(int dealershipID, int listingId, decimal oldPrice, decimal salePrice, string name, short vehicleStatusCodeId)
        {
            

            var context = new VincontrolEntities();
            if (vehicleStatusCodeId == Constanst.VehicleStatus.Appraisal)
            {
                var car = context.Appraisals.FirstOrDefault(x => x.AppraisalId == listingId);
                return GetContent(oldPrice, salePrice, name, car.Vehicle, car.Stock);
            }
            else
            {
            var car = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
                return GetContent(oldPrice, salePrice, name, car.Vehicle, car.Stock);

            }
        }

        private static string GetContent(decimal oldPrice, decimal salePrice, string name, Vehicle car, string stock)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");
            builder.AppendFormat(
                "<table width=\"500\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");
            builder.AppendFormat("	<td colspan=\"2\" style=\"padding-top: 40px; border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("<span style=\" font-weight: bold;\">VinControl Alert - Change Price</span><br />");
            builder.AppendFormat(car.Year + " " + car.Make + " " + car.Model + " " + car.Trim +
                                 "<br />");
            builder.AppendFormat("Stock : " + stock + "<br />");
            builder.AppendFormat("Vin : " + car.Vin + "<br />");
            builder.AppendFormat("</td>");
            builder.AppendFormat("<td align=\"right\" width=\"150\" style=\"border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat(
                "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; background: #111111; color: white;\">");
            builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 "Price is changed from " + oldPrice + " to " + salePrice + " by " + name + "." + "</p>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "<td colspan=\"3\" style=\"color: white; background: #003399; padding: 5px; font-weight: bold;\">1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span></td>");
            builder.AppendFormat("	</tr>");
            builder.AppendFormat("</table>");
            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");
            return builder.ToString();
        }

        public static string CreateBodyEmailForUpdateMarketPriceRange(int dealershipID, int listingId, int carRanking, int numberOfCars, int oldCarRanking, int oldNumberOfCars, string name, MarketCarInfo car)
        {
            var builder = new StringBuilder();

            var context = new VincontrolEntities();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");
            builder.AppendFormat(
                "<table width=\"500\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");
            builder.AppendFormat("	<td colspan=\"2\" style=\"padding-top: 40px; border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("<span style=\" font-weight: bold;\">VinControl Alert - Change Market Price Range</span><br />");
            builder.AppendFormat(car.Year + " " + car.Make + " " + car.Model + " " + car.Trim + "<br />");
            builder.AppendFormat("Stock : " + car.AutoTraderStockNo + "<br />");
            builder.AppendFormat("Vin : " + car.Vin + "<br />");
            builder.AppendFormat("Price : " + CommonHelper.FormatNumberInThousand(car.CurrentPrice) + "<br />");
            builder.AppendFormat("</td>");
            builder.AppendFormat("<td align=\"right\" width=\"150\" style=\"border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat(
             "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; background: #111111; color: white;\">");
            builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 "Market price range is changed from 'Ranks " + oldCarRanking + " out of " + oldNumberOfCars + " on Market'" + " to 'Ranks " + carRanking + " out of " + numberOfCars + " on Market'" + " by " + name + "." + "</p>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "<td colspan=\"3\" style=\"color: white; background: #003399; padding: 5px; font-weight: bold;\">1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span></td>");
            builder.AppendFormat("	</tr>");
            builder.AppendFormat("</table>");
            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");
            return builder.ToString();

        }

        public static void SendBackUpEmail(string subject, string body)
        {
            SendEmail(
                          new MailAddress(
                              System.Configuration.ConfigurationManager.AppSettings["TrackEmailAccount"].ToString(CultureInfo.InvariantCulture)),
                         subject,
                          body);
        }


        public static string CreateBannerBodyHtmlEmail(TradeInVehicleModel vehicle,DealershipViewModel dealer)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");

            builder.AppendFormat("<span style=\"font-size:9px;\">Exclusively for:  |   	" + vehicle.CustomerFirstName + " " + vehicle.CustomerLastName + "</span><br />");
            builder.AppendFormat("<span style=\"font-size:12px;\">Please find your trade in pdf file in attachment</span><br />");
            builder.AppendFormat(
                "<table width=\"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");
            
            builder.AppendFormat("<td colspan=\"2\" style=\"padding-top: 40px;\">");


            builder.AppendFormat("<h2 style=\"font-weight: bold; margin: 0; padding: 0; position: relative; bottom: -29px; color: green; font-style: italic\">" + vehicle.CustomerFirstName + " " + vehicle.CustomerLastName + ". Here's Your Trade-In Value!</h2>");
            builder.AppendLine("</td>");
            builder.AppendLine("<td align=\"right\" width=\"150\">");
            builder.AppendFormat(
           "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendLine("</td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendLine("<tr>");
            builder.AppendLine("	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; border: 4px solid #99B189; border-radius: 4px; -moz-border-radius: 4px; color: #111111;\">");
            builder.AppendLine("<h3 style=\"margin: 0; color: #003399;\">Request Details</h3>");
            builder.AppendLine("<ul style=\"margin: 0; list-style-type: none; padding: 0;\">");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Dealership: "+dealer.DealershipName+"</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Phone: " + dealer.Phone+ "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Address: " + dealer.DealershipAddress + "</strong></li>");
            if (!String.IsNullOrEmpty(vehicle.Vin))
            {
                builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Vin: " + vehicle.Vin + "</strong></li>");
            }
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Vehicle: " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Mileage: " +CommonHelper.FormatNumberInThousand(vehicle.Mileage) + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Options: " + vehicle.SelectedOptionList + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Condition: "+vehicle.Condition+"</strong></li>");
                      
            builder.AppendLine("</ul>");
            builder.AppendLine("<p style=\"background: #bbbbbb; padding-top: 2px;\"></p>"); 
            builder.AppendLine("<h3 style=\"color: #003399; margin-bottom: 0;\">Trade-In Value</h3>");
            builder.AppendLine("<p style=\"margin-left: 220px;font-size:30px;font-weight: bold\">" + (!vehicle.TradeInFairPrice.HasValue?"Call Dealer For Price" : vehicle.TradeInFairPrice.Value.ToString()) + "</p>");
            builder.AppendLine("</td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendLine("<tr>");
            builder.AppendLine("<td colspan=\"3\" style=\"border-radius: 4px; -moz-border-radius: 4px; color: white; background: green; padding: 5px; font-weight: bold;\">Trade-In | 1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span</td>");
            builder.AppendLine("</tr>");
            builder.AppendFormat("</table>");



            builder.AppendLine("<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Please note this email was sent to: " + vehicle.CustomerEmail + "</font>");

            builder.AppendLine("<br/><br/>");

            builder.AppendLine("<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">This trade in value is an estimate. The actual trade in value might change due to vehicle and market condtions, and is subject to change without notice." + "</font>");

            builder.AppendLine("<br/><br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">To ensure that you continue to receive email from us, please add us to your Address Book (tradein@vehicleinventorynetwork.com.). Thank you. "+ "</font>");

            builder.AppendLine("<br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Do not reply to this email. For customer service please email customerservice@vehicleinventorynetwork.com. " + "</font>");



            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");
            return builder.ToString();

        }

        public static string CreateBannerBodyForPdf(TradeInVehicleModel vehicle, DealershipViewModel dealer)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");

            builder.AppendFormat("<span style=\"font-size:9px;\">Exclusively for:  |   	" + vehicle.CustomerFirstName + " " + vehicle.CustomerLastName + "</span><br />");
            builder.AppendFormat(
                "<table width=\"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");

            builder.AppendFormat("<td colspan=\"2\" style=\"padding-top: 40px;\">");


            builder.AppendFormat("<h2 style=\"font-weight: bold; margin: 0; padding: 0; position: relative; bottom: -29px; color: green; font-style: italic\">" + vehicle.CustomerFirstName + " " + vehicle.CustomerLastName + ". Here's Your Trade-In Value!</h2>");
            builder.AppendLine("</td>");
            builder.AppendLine("<td align=\"right\" width=\"150\">");
            builder.AppendFormat(
          "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendLine("</td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendLine("<tr>");
            builder.AppendLine("	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; border: 4px solid #99B189; border-radius: 4px; -moz-border-radius: 4px; color: #111111;\">");
            builder.AppendLine("<h3 style=\"margin: 0; color: #003399;\">Request Details</h3>");
            builder.AppendLine("<ul style=\"margin: 0; list-style-type: none; padding: 0;\">");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Dealership: " + dealer.DealershipName + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Phone: " + dealer.Phone + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Address: " + dealer.DealershipAddress + "</strong></li>");
            if (!String.IsNullOrEmpty(vehicle.Vin))
            {
                builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Address: " + vehicle.Vin + "</strong></li>");
            }
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Vehicle: " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Mileage: " + CommonHelper.FormatNumberInThousand(vehicle.Mileage) + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Options: " + vehicle.SelectedOptionList + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Condition: " + vehicle.Condition + "</strong></li>");

            builder.AppendLine("</ul>");
            builder.AppendLine("<p style=\"background: #bbbbbb; padding-top: 2px;\"></p>");
            builder.AppendLine("<h3 style=\"color: #003399; margin-bottom: 0;\">Trade-In Value</h3>");
            builder.AppendLine("<p style=\"margin-left: 220px;font-size:30px;font-weight: bold\">" + (!vehicle.TradeInFairPrice.HasValue ? "Call Dealer For Price" : vehicle.TradeInFairPrice.ToString()) + "</p>");
            builder.AppendLine("</td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendLine("<tr>");
            builder.AppendLine("<td colspan=\"3\" style=\"border-radius: 4px; -moz-border-radius: 4px; color: white; background: green; padding: 5px; font-weight: bold;\">Trade-In | 1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span</td>");
            builder.AppendLine("</tr>");
            builder.AppendFormat("</table>");



            builder.AppendLine("<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Please note this email was sent to: " + vehicle.CustomerEmail + "</font>");

            builder.AppendLine("<br/><br/>");

            builder.AppendLine("<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">This trade in value is an estimate. The actual trade in value might change due to vehicle and market condtions, and is subject to change without notice." + "</font>");

            builder.AppendLine("<br/><br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">To ensure that you continue to receive email from us, please add us to your Address Book (tradein@vehicleinventorynetwork.com.). Thank you. " + "</font>");

            builder.AppendLine("<br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Do not reply to this email. For customer service please email customerservice@vehicleinventorynetwork.com. " + "</font>");



            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");
            return builder.ToString();

        }

        public static string CreateBannerBodyForPdfNew(TradeInVehicleModel vehicle, DealershipViewModel dealer,string acv)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");

            builder.AppendFormat("<span style=\"font-size:9px;\">Exclusively for:  |   	" + vehicle.CustomerFirstName + " " + vehicle.CustomerLastName + "</span><br />");
            builder.AppendFormat(
                "<table width=\"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");

            builder.AppendFormat("<td colspan=\"2\" style=\"padding-top: 40px;\">");


            builder.AppendFormat("<h2 style=\"font-weight: bold; margin: 0; padding: 0; position: relative; bottom: -29px; color: green; font-style: italic\">" + vehicle.CustomerFirstName + " " + vehicle.CustomerLastName + ". Here's Your Trade-In Value!</h2>");
            builder.AppendLine("</td>");
            builder.AppendLine("<td align=\"right\" width=\"150\">");
            builder.AppendFormat(
          "<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendLine("</td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendLine("<tr>");
            builder.AppendLine("	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; border: 4px solid #99B189; border-radius: 4px; -moz-border-radius: 4px; color: #111111;\">");
            builder.AppendLine("<h3 style=\"margin: 0; color: #003399;\">Request Details</h3>");
            builder.AppendLine("<ul style=\"margin: 0; list-style-type: none; padding: 0;\">");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Dealership: " + dealer.DealershipName + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Phone: " + dealer.Phone + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Address: " + dealer.DealershipAddress + "</strong></li>");
            if (!String.IsNullOrEmpty(vehicle.Vin))
            {
                builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Address: " + vehicle.Vin + "</strong></li>");
            }
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Vehicle: " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Mileage: " + CommonHelper.FormatNumberInThousand(vehicle.Mileage) + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Options: " + vehicle.SelectedOptionList + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Condition: " + vehicle.Condition + "</strong></li>");

            builder.AppendLine("</ul>");
            builder.AppendLine("<p style=\"background: #bbbbbb; padding-top: 2px;\"></p>");
            builder.AppendLine("<h3 style=\"color: #003399; margin-bottom: 0;\">Trade-In Value</h3>");
            builder.AppendLine("<p style=\"margin-left: 220px;font-size:30px;font-weight: bold\">" + (string.IsNullOrEmpty(acv) ? "Call Dealer For Price" : acv) + "</p>");
            builder.AppendLine("</td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendLine("<tr>");
            builder.AppendLine("<td colspan=\"3\" style=\"border-radius: 4px; -moz-border-radius: 4px; color: white; background: green; padding: 5px; font-weight: bold;\">Trade-In | 1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span</td>");
            builder.AppendLine("</tr>");
            builder.AppendFormat("</table>");



            builder.AppendLine("<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Please note this email was sent to: " + vehicle.CustomerEmail + "</font>");

            builder.AppendLine("<br/><br/>");

            builder.AppendLine("<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">This trade in value is an estimate. The actual trade in value might change due to vehicle and market condtions, and is subject to change without notice." + "</font>");

            builder.AppendLine("<br/><br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">To ensure that you continue to receive email from us, please add us to your Address Book (tradein@vehicleinventorynetwork.com.). Thank you. " + "</font>");

            builder.AppendLine("<br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Do not reply to this email. For customer service please email customerservice@vehicleinventorynetwork.com. " + "</font>");



            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");
            return builder.ToString();

        }


        public static string CreateBodyEmailForSendClientRequestForText(TradeInVehicleModel vehicle, DealershipViewModel dealer)
        {

            var builder = new StringBuilder();

            builder.Append("Text Format" + Environment.NewLine);

            builder.Append(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + Environment.NewLine);

            builder.Append("Year: " + vehicle.SelectedYear + Environment.NewLine);

            builder.Append("Make: " + vehicle.SelectedMakeValue + Environment.NewLine);
                       
            builder.Append("Model: " + vehicle.SelectedModelValue + Environment.NewLine);

            builder.Append("Trim: " + vehicle.SelectedTrimValue + Environment.NewLine);

            builder.Append("First Name: " + vehicle.CustomerFirstName + Environment.NewLine);

            builder.Append("Last Name: " + vehicle.CustomerLastName + Environment.NewLine);

            builder.Append("Phone: " + vehicle.CustomerPhone + Environment.NewLine);

            builder.Append("Email: " + vehicle.CustomerEmail + Environment.NewLine);

            builder.Append("Notes: I would like to trade in " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + " for " + (!vehicle.TradeInFairPrice.HasValue ? "Call Dealer For Price" : vehicle.TradeInFairPrice.ToString()) + Environment.NewLine);

            builder.Append("Dealer: " + dealer.DealershipName + Environment.NewLine);

            builder.Append("Provider: Vincontrol, 9881 Irvine Center Dr, Irvine, CA 92618, USA" + Environment.NewLine);

            builder.Append("Provider Phone: 855-VIN-CTRL" + Environment.NewLine);

            builder.Append("Provider Email: alerts@vehicleinventorynetwork.com" + Environment.NewLine);

            return builder.ToString();

        }

        public static string CreateBodyEmailForPasswordResetRequest(int userId, Guid forgotPasswordId)
        {
            var builder = new StringBuilder();
            string link = "http://carslisting.us/Account/ChangePassword?userId=" + userId + "&forgotPasswordId=" + forgotPasswordId;

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");
            builder.AppendFormat(
                "<table width=\"auto\" cellpadding=\"0\" cellspacing=\"0\" style=\"background: #eeeeee; padding: 10px; font-family: Trebuchet MS, Arial, sans-serif !important;\">");
            builder.AppendFormat("<tr>");
            builder.AppendFormat("	<td colspan=\"2\" style=\"padding-top: 40px; border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("Hello,<br /><br />");
            builder.AppendFormat("Please follow the link below to reset your VINControl password:<br />");
            builder.AppendFormat("<a href=\"" + link + "\">" + link + "</a><br />");
            builder.AppendFormat("Please note: for security reasons this link will expire in 24 hours.<br /><br />");
            builder.AppendFormat("Sincerely,<br />");
            builder.AppendFormat("VINControl Support<br />");
            builder.AppendFormat("<a href=\"mailto: Support@vincontrol.com\">Support@vincontrol.com</a>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("<td align=\"right\" width=\"150\" style=\"border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("<img style=\"margin-left: 10px\" width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat("	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; background: #111111; color: white;\">");
            builder.AppendFormat("<p style=\"margin: 0;\">" + "</p>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "<td colspan=\"3\" style=\"color: white; background: #003399; padding: 5px; font-weight: bold;\">1.855.VIN.CTRL <span style=\"font-size: .7em; font-style: italic; font-weight: 200\">* This is an automated message, do not reply. *</span></td>");
            builder.AppendFormat("	</tr>");
            builder.AppendFormat("</table>");
            builder.AppendFormat("</body>");

            builder.AppendFormat("</html>");

            return builder.ToString();
        }
    }
}
