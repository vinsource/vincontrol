using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using VINCapture.UploadImage.Models;
using VINCapture.UploadImage.ViewModels;
using vincontrol.Data.Model;

namespace VINCapture.UploadImage.USBHelpers
{
    class EmailHelpers
    {
        public static IEnumerable<string> GetEmails(int dealerId)
        {
            using (var dbContext = new VincontrolEntities())
            {
                var emailsList = from e in dbContext.UserNotifications
                                 from et in dbContext.Users
                                 where
                                     e.DealerId == dealerId && e.ImageUploadNotified && et.Active.Value && e.UserId == et.UserId
                                 select new
                                 {
                                     et.Email
                                 };

                if (emailsList.Any())
                    return emailsList.Select(x => x.Email).Distinct().ToList();
                return ConfigurationManager.AppSettings["CCEmails"].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        public static string GetHtmlCode(IEnumerable<USBCarViewModel> vehicles,DealerUser dealerUser)
        {
            var result = new StringBuilder();

            result.Append(@"<html>");
            result.Append(@"<head id=""Head1"" runat=""server"">");
            result.Append(@"  <style type=""text/css"">");
            result.Append(@"   div#vehicle-list");
            result.Append(@"  {");
            result.Append(@"  text-align: center;");
            result.Append(@"      font-size: .7em;");
            result.Append(@" margin:0 auto;");
            result.Append(@"  }");
            result.Append(@"  #vehicle-list td");
            result.Append(@"  {");
            result.Append(@"     padding: .3em .7em .3em .7em;");
            result.Append(@"     border-bottom: .1em #bbbbbb solid;");
            result.Append(@"   }");
            result.Append(@"  #printable-list");
            result.Append(@" {");
            result.Append(@"  text-align: center;");
            //result.Append(@"      display: block;");
            result.Append(@"width: 100%");
            result.Append(@" }");
            result.Append(@" #vehicle-list thead tr td");
            result.Append(@" {");
            result.Append(@"      font-weight: bold;");
            result.Append(@"      color: black;");
            result.Append(@"     border-bottom: #C80000 4px solid;");
            result.Append(@" }");
            result.Append(@" .graph-title-bar a");
            result.Append(@" {");
            result.Append(@"    display: none;");
            result.Append(@" }");
            result.Append(@" .padding-right");
            result.Append(@" {");
            result.Append(@"      text-align: right;");
            result.Append(@" }");
            result.Append(@"   </style>");
            result.Append(@" </head>");
            result.Append(@"   <body>");

            result.Append(@"  <div class=""graph-title-bar"" style=""margin-bottom:30px;"">");
            result.Append(@"    <h1 style=""font-size:40px; text-align:center;"">");
            result.Append(@"    Image Upload Report");
            result.Append(@"   </h1>");
            result.Append(@"  </div>");
            //result.Append(@"<div class=""graph-title-bar"" style=""margin-bottom:30px;"">");
            //result.Append(@"  <h2 style=""text-align:center;"">");
            ////result.Append(@"     Last 5 days");
            //result.Append(@"    </h2>");
            //result.Append(@" </div>");

            result.Append(@"     <div id=""printable-list"">");
            result.Append(@"   <div id=""vehicle-list"" style=""font-size: 12pt"">");

            result.Append(@"<table style=""margin-left:40px;"">");
            result.Append(@" <thead align=""left"" style=""display: table-header-group"">");
            WriteHeader(result);
            result.Append(@" </thead>");
            result.Append(@"<tbody>");

            WriteContent(result, vehicles);
            result.Append(@"</tbody>");
            result.Append(@"</table>");
            //result.Append(@" </div>");
            //result.Append(@" </div>");
            result.Append(@"       </div>");
            result.Append(@"  </div>");
            result.Append(@"<br>");
            result.Append(@"Store : " + dealerUser.DealerName);
            result.Append(@"<br>");
            result.Append(@"Uploaded by : " + dealerUser.Name);
            result.Append(@" </body>");
        
            result.Append(@"  </html>");
            return result.ToString();
        }

        public static void WriteHeader(StringBuilder result)
        {
            result.Append(@"<tr>");
            CreateContent(result, "Year");
            CreateContent(result, "Make");
            CreateContent(result, "Model");
            CreateContent(result, "Trim");
            CreateContent(result, "Color");
            CreateContent(result, "VIN");
            CreateContent(result, "Stock");
            CreateContent(result, "No of Pictures");
            CreateContent(result, "Datestamp");
            result.Append(@"</tr>");
        }

        public static void CreateContent(StringBuilder result, string header)
        {
            result.Append(@"<td>");
            result.Append(header);
            result.Append(@"</td>");
        }


        public static void WriteContent(StringBuilder result, IEnumerable<USBCarViewModel> vehicles)
        {
            foreach (var usbCarViewModel in vehicles)
            {
                result.Append(@"<tr>");
                CreateContent(result, usbCarViewModel.Year);
                CreateContent(result, usbCarViewModel.Make);
                CreateContent(result, usbCarViewModel.Model);
                CreateContent(result, usbCarViewModel.Trim);
                CreateContent(result, usbCarViewModel.Color);
                CreateContent(result, usbCarViewModel.Vin);
                CreateContent(result, usbCarViewModel.Stock);
                CreateContent(result, usbCarViewModel.Quantity.ToString(CultureInfo.InvariantCulture));
                CreateContent(result, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                result.Append(@"</tr>");
            }
        }

        public static string GetSubject()
        {
            return "Vincapture report : Images upload to the inventory on " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        public static string GetBody(IEnumerable<USBCarViewModel> vehicles,DealerUser dealerUser)
        {
            var builder = new StringBuilder();

            builder.AppendLine("There are following vehicles that has been uploaded images from Vincapture : ");

            builder.AppendLine("<br>");

            var index = 1;

            foreach (var tmp in vehicles)
            {
                builder.Append(index + ".  " + tmp.Year + " " + tmp.Make + " " + tmp.Model + " " + tmp.Trim + " / Stock : " + tmp.Stock + " - Vin : " + tmp.Vin + ".  Number of pictures = " + tmp.Quantity);
                builder.AppendLine("<br>");
                index++;
            }

            builder.AppendLine("<br>");
            builder.AppendLine("<br>");
            builder.AppendLine(dealerUser.DealerName); builder.AppendLine("<br>");
            builder.AppendLine("Uploaded by : " + dealerUser.Name);


            return builder.ToString();
        }

        //private string GetVin(IEnumerable<USBCarViewModel> vehicles)
        //{
        //    var result = vehicles.Aggregate(string.Empty, (current, usbCarViewModel) => current + (usbCarViewModel.Vin + ","));
        //    if (!result.Equals(String.Empty))
        //    {
        //        result = result.Substring(0, result.Length - 1);
        //    }
        //    return result;
        //}


        public static void SendEmail(IEnumerable<string> emailList, string subject, string body, bool bodyHtml, MemoryStream pdfContent)
        {
            try
            {

                var smtpServerAddress = ConfigurationManager.AppSettings["SMTPServer"];
                var defaultFromEmail = ConfigurationManager.AppSettings["DefaultFromEmail"];
                var displayName = ConfigurationManager.AppSettings["DisplayName"];
                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage
                {
                    From = fromAddress,
                    IsBodyHtml = bodyHtml,
                    Body = body,
                    Subject = subject
                };

                if (pdfContent != null)
                {
                    var attach = new Attachment(pdfContent, "UploadImageReport" + DateTime.Now.ToShortDateString() + ".pdf", "application/pdf");

                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);
                }

                foreach (var tmp in emailList)
                {
                    message.To.Add(new MailAddress(tmp));
                }

                foreach (var tmp in ConfigurationManager.AppSettings["CCEmails"].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.CC.Add(new MailAddress(tmp));
                }



                var client = new SmtpClient(smtpServerAddress, 587);
                var ntlmAuthentication = new System.Net.NetworkCredential
                    (ConfigurationManager.AppSettings["DefaultFromEmail"],
                    ConfigurationManager.AppSettings["TrackEmailPass"]);

                client.Credentials = ntlmAuthentication;
                client.EnableSsl = true;
                client.Send(message);


            }
            catch
            {

            }
        }

        public static void SendEmailByWcfService(IEnumerable<USBCarViewModel> vehicles, DealerUser dealerUser)
        {
            var usblist = new USBList()
                {
                    VehicleList = vehicles,
                    Dealer = dealerUser,

                };

            string strUri = ConfigurationManager.AppSettings["TaskServer"];
            
            var uri = new Uri(strUri);
            
            var request = WebRequest.Create(uri);
            
            request.Method = "POST";
            
            request.ContentType = "application/json; charset=utf-8";

            var jsonSerializer = new JavaScriptSerializer();
            string serOut = jsonSerializer.Serialize(usblist);

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(serOut);
            }

            request.GetResponse();

        }
    }
}
