using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using vincontrol.Application.Forms.TradeInManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.TradeInManagement;
using vincontrol.Helper;
using vincontrol.PdfHelper;

namespace vincontrol.VinTrade.Helpers
{
    public class TradeInEmailHelper
    {
        
        public static TradeinEmail SendEmail(TradeInVehicleModel vehicle, DealershipViewModel dealer)
        {
            var tradeinEmail = new TradeinEmail()
            {
                Receivers = String.IsNullOrEmpty(dealer.Email) ? new List<string>() : dealer.Email.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable()
            };

            var bodyEmail = CreateBannerBodyHtmlEmail(vehicle, dealer);
            var bodyPdf = CreateBannerBodyForPdf(vehicle, dealer);

            try
            {
                switch (dealer.EmailFormat)
                {
                    case 0:
                        SendTextContent(tradeinEmail.Receivers, vehicle, dealer, bodyPdf);
                        break;
                    case 1:
                        SendAdfContent(tradeinEmail.Receivers, vehicle, dealer, bodyPdf);
                        break;
                    case 2:
                        SendTextContent(tradeinEmail.Receivers, vehicle, dealer, bodyPdf);
                        SendAdfContent(tradeinEmail.Receivers, vehicle, dealer, bodyPdf);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                SendEmailForTradeInBannerToCustomer(vehicle.CustomerEmail,
                                              "Your Trade In Value For " + vehicle.SelectedYear + " " +
                                              vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " +
                                              vehicle.SelectedTrimValue, bodyEmail, bodyPdf);
            }





            return tradeinEmail;

        }

        public static void SendEmailForTradeInBannerToCustomer(string toAddress, string subject, string bodyEmail, string bodyPdf)
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

                var ntlmAuthentication =new System.Net.NetworkCredential(ConfigurationManager.AppSettings["TradeInEmail"].ToString(CultureInfo.InvariantCulture),
                        ConfigurationManager.AppSettings["TrackEmailPass"].ToString(CultureInfo.InvariantCulture));

                client.Credentials = ntlmAuthentication;

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["TradeInEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["TradeInDisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage(fromAddress,new MailAddress(toAddress))
                {
                    From = fromAddress,
                    Body = bodyEmail,
                    IsBodyHtml = true,
                    Subject = subject
                };


                if (!String.IsNullOrEmpty(bodyPdf))
                {
                    var pdfHelper = new Pdf();

                    var workStream = pdfHelper.WritePdf(bodyPdf);

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

        public static void SendEmailForTradeInBannerToDealer(IEnumerable<string> toAddress, string subject, string body, string bodyPdf, string customerEmail)
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
                    new System.Net.NetworkCredential(ConfigurationManager.AppSettings["TradeInEmail"].ToString(CultureInfo.InvariantCulture),
                        ConfigurationManager.AppSettings["TrackEmailPass"].ToString(CultureInfo.InvariantCulture));

                client.Credentials = ntlmAuthentication;

                var fromAddress = new MailAddress(customerEmail, customerEmail);

                var message = new MailMessage {From = fromAddress};

                foreach (var tmp in toAddress.Distinct().AsEnumerable())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(tmp);

                }
                message.Body = body;

                message.Subject = subject;


                if (!String.IsNullOrEmpty(bodyPdf))
                {

                    var pdfHelper = new Pdf();

                    var workStream = pdfHelper.WritePdf(bodyPdf);

                    var attach = new System.Net.Mail.Attachment(workStream, "TradeInValue.pdf", "application/pdf");

                    message.Attachments.Add(attach);

                }
                client.Send(message);

           

            }
            catch (Exception)
            {

            }


        }
        
        private  static void SendAdfContent(IEnumerable<string> receivers, TradeInVehicleModel vehicle, DealershipViewModel dealerSessionInfo, string bodyPdf)
        {
            string emailContent;
            if (String.IsNullOrEmpty(vehicle.InterestedVehicle))
            {
                emailContent = AdfLead.CreateBodyEmailForSendClientRequestForAdf("", vehicle, dealerSessionInfo);
            }
            else
            {
                emailContent = AdfLead.CreateBodyEmailForSendClientRequestForAdf("",
                                                                          vehicle, dealerSessionInfo);
            }

            SendEmailForTradeInBannerToDealer(
                    receivers,
                    "Customer Request Trade In Value For " + vehicle.SelectedYear + " " +
                    vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue +
                    " " +
                    vehicle.SelectedTrimValue, emailContent
                    , bodyPdf, vehicle.CustomerEmail);
          
        }

        private static void SendTextContent(IEnumerable<string> receivers, TradeInVehicleModel vehicle, DealershipViewModel dealerSessionInfo, string bodyPdf)
        {
            string emailContent;
            if (String.IsNullOrEmpty(vehicle.InterestedVehicle))
            {
                emailContent = TextLead.BodyClientRequestText(
                        vehicle, dealerSessionInfo);
            }
            else
            {
                emailContent = TextLead.BodyClientRequestText("",
                                                                        vehicle, dealerSessionInfo);
            }
            SendEmailForTradeInBannerToDealer(
                  receivers,
                  "Customer Request Trade In Value For " + vehicle.SelectedYear + " " +
                  vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue +
                  " " +
                  vehicle.SelectedTrimValue, emailContent
                 , bodyPdf,vehicle.CustomerEmail);
           
        }
        public static string CreateBannerBodyHtmlEmail(TradeInVehicleModel vehicle, DealershipViewModel dealer)
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
            builder.AppendLine("<h3 style=\"margin: 0; color: #880000;\">Request Details</h3>");
            builder.AppendLine("<ul style=\"margin: 0; list-style-type: none; padding: 0;\">");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Dealership: " + dealer.DealershipName + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Phone: " + dealer.Phone + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Address: " + dealer.DealershipAddress + "</strong></li>");
            if (!String.IsNullOrEmpty(vehicle.Vin))
            {
                builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Vin: " + vehicle.Vin + "</strong></li>");
            }
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Vehicle: " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Mileage: " + CommonHelper.FormatNumberInThousand(vehicle.Mileage) + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Options: " + vehicle.SelectedOptions + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Condition: " + vehicle.Condition + "</strong></li>");

            builder.AppendLine("</ul>");
            builder.AppendLine("<p style=\"background: #bbbbbb; padding-top: 2px;\"></p>");
            builder.AppendLine("<h3 style=\"color: #880000; margin-bottom: 0;\">Trade-In Value</h3>");
            builder.AppendLine("<p style=\"margin-left: 220px;font-size:30px;font-weight: bold\">" + (vehicle.TradeInFairPrice == 0 ? "Call Dealer For Price" : ((int)vehicle.TradeInFairPrice).ToString("C")) + Environment.NewLine + "</p>");
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
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">To ensure that you continue to receive email from us, please add us to your Address Book (tradein@vincontrol.com.). Thank you. " + "</font>");

            builder.AppendLine("<br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Do not reply to this email. For customer service please email customerservice@vincontrol.com. " + "</font>");



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
            builder.AppendLine("<h3 style=\"margin: 0; color: #880000;\">Request Details</h3>");
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
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Options: " + vehicle.SelectedOptions + "</strong></li>");
            builder.AppendLine("<li style=\"background: white; padding: .5em; margin-bottom: 5px;\"><strong>Condition: " + vehicle.Condition + "</strong></li>");

            builder.AppendLine("</ul>");
            builder.AppendLine("<p style=\"background: #bbbbbb; padding-top: 2px;\"></p>");
            builder.AppendLine("<h3 style=\"color: #880000; margin-bottom: 0;\">Trade-In Value</h3>");
            builder.AppendLine("<p style=\"margin-left: 220px;font-size:30px;font-weight: bold\">" + (vehicle.TradeInFairPrice == 0 ? "Call Dealer For Price" : ((int)vehicle.TradeInFairPrice).ToString("C")) + Environment.NewLine + "</p>");
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
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">To ensure that you continue to receive email from us, please add us to your Address Book (tradein@vincontrol.com.). Thank you. " + "</font>");

            builder.AppendLine("<br/>");

            builder.AppendLine(
                "<font size=\"1\" face=\"Arial, Helvetica, sans-serif\" color=\"#666666\" style=\"font-size:9px;\">Do not reply to this email. For customer service please email customerservice@vincontrol.com. " + "</font>");



            builder.AppendFormat("</body>");


            builder.AppendFormat("</html>");
            return builder.ToString();

        }

       
    }

    public class TextLead
    {
        public static string BodyClientRequestText(TradeInVehicleModel vehicle, DealershipViewModel dealer)
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

            builder.Append("Notes: I would like to trade in " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + " for " + (vehicle.TradeInFairPrice == 0 ? "Call Dealer For Price" : ((int)vehicle.TradeInFairPrice).ToString("C")) + Environment.NewLine);

            builder.Append("Dealer: " + dealer.DealershipName + Environment.NewLine);

            builder.Append("Provider: Vincontrol LLC, 9881 Irvine Center Dr, Irvine, CA 92618, USA" + Environment.NewLine);

            builder.Append("Provider Phone: 855-VIN-CTRL" + Environment.NewLine);

            builder.Append("Provider Email: info@vincontrol.com" + Environment.NewLine);

            return builder.ToString();

        }

        public static string BodyClientRequestText(string vin, TradeInVehicleModel vehicle, DealershipViewModel dealer)
        {

            var builder = new StringBuilder();

            ITradeInManagementForm tradeInManagement = new TradeInManagementForm();

            var existingInventory = tradeInManagement.GetInventory(vin, dealer.DealershipId);


            if (existingInventory.VehicleId > 0)
            {

                builder.Append("Text Format" + Environment.NewLine);

                builder.Append(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() +
                               Environment.NewLine);

                builder.Append("Year: " + existingInventory.SelectedYear + Environment.NewLine);

                builder.Append("Make: " + existingInventory.SelectedMakeValue + Environment.NewLine);

                builder.Append("Model: " + existingInventory.SelectedModelValue + Environment.NewLine);

                builder.Append("Trim: " + existingInventory.SelectedTrimValue + Environment.NewLine);

                builder.Append("Stock: " + existingInventory.StockNumber + Environment.NewLine);

                builder.Append("Vin: " + existingInventory.Vin + Environment.NewLine);

                builder.Append("First Name: " + vehicle.CustomerFirstName + Environment.NewLine);

                builder.Append("Last Name: " + vehicle.CustomerLastName + Environment.NewLine);

                builder.Append("Phone: " + vehicle.CustomerPhone + Environment.NewLine);

                builder.Append("Email: " + vehicle.CustomerEmail + Environment.NewLine);

                builder.Append("Notes: I would like to trade in " + vehicle.SelectedYear + " " +
                               vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue +
                               " for " +
                               (vehicle.TradeInFairPrice == 0 ? "Call Dealer For Price" : ((int)vehicle.TradeInFairPrice).ToString("C")) + Environment.NewLine);

                builder.Append("Dealer: " + dealer.DealershipName + Environment.NewLine);

                builder.Append(
                    "Provider: Vincontrol LLC, 9881 Irvine Center Dr, Irvine, CA 92618, USA" +
                    Environment.NewLine);

                builder.Append("Provider Phone: 855-VIN-CTRL" + Environment.NewLine);

                builder.Append("Provider Email: info@vincontrol.com" + Environment.NewLine);

                return builder.ToString();
            }
            else
            {
                builder.Append("Text Format" + Environment.NewLine);

                builder.Append(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() +
                               Environment.NewLine);

                builder.Append("Year: " + vehicle.SelectedYear + Environment.NewLine);

                builder.Append("Make: " + vehicle.SelectedMake + Environment.NewLine);

                builder.Append("Model: " + vehicle.SelectedModel + Environment.NewLine);

                builder.Append("Trim: " + vehicle.SelectedTrim + Environment.NewLine);

                builder.Append("Vin: " + vehicle.Vin + Environment.NewLine);

                builder.Append("First Name: " + vehicle.CustomerFirstName + Environment.NewLine);

                builder.Append("Last Name: " + vehicle.CustomerLastName + Environment.NewLine);

                builder.Append("Phone: " + vehicle.CustomerPhone + Environment.NewLine);

                builder.Append("Email: " + vehicle.CustomerEmail + Environment.NewLine);

                builder.Append("Notes: I would like to trade in " + vehicle.SelectedYear + " " +
                               vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue +
                               " for " +
                               (vehicle.TradeInFairPrice == 0 ? "Call Dealer For Price" : ((int)vehicle.TradeInFairPrice).ToString("C")) + Environment.NewLine);

                builder.Append("Dealer: " + dealer.DealershipName + Environment.NewLine);

                builder.Append(
                    "Provider: Vincontrol LLC, 9881 Irvine Center Dr, Irvine, CA 92618, USA" +
                    Environment.NewLine);

                builder.Append("Provider Phone: 855-VIN-CTRL" + Environment.NewLine);

                builder.Append("Provider Email: info@vincontrol.com" + Environment.NewLine);

                return builder.ToString();

            }

        }

        private static string BodyTradein(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }

            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber + "</a>";

            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + "  want to have a Trade-In Appraisal for a vehicle!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Vehicle Information</h3>";
            htmlBody += "<li><b>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</b></li>";
            if (customerinfo.vinnumber != null)
            {
                htmlBody += "<li>VINNumber: " + customerinfo.vinnumber + "</li>";
            }

            htmlBody += "<li>Engine: " + customerinfo.engine + "</li>";
            htmlBody += "<li>Transmission: " + customerinfo.transmission + "</li>";
            htmlBody += "<li>Exterior Color: " + customerinfo.exterior_color + "</li>";
            htmlBody += "<li>Mileage: " + customerinfo.mileage + "</li>";
            htmlBody += "<li>Condition: " + customerinfo.condition + "</li>";
            if (customerinfo.offer_value != 0)
            {
                htmlBody += "<li>Pay-off: " + customerinfo.offer_value + "$</li>";
            }
            htmlBody += "</ul>";

            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Customer Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Address: </b>" + customerinfo.address + " " + customerinfo.city + " " + customerinfo.state + " " + customerinfo.zipcode + "</li>";

            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }

            htmlBody += "</ul></div>";

            return htmlBody;
        }

        private static string BodyConsignment(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }

            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + "  want to have a Consignment for a vehicle!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Vehicle Information</h3>";
            htmlBody += "<li><b>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</b></li>";
            if (customerinfo.vinnumber != null)
            {
                htmlBody += "<li>VINNumber: " + customerinfo.vinnumber + "</li>";
            }

            htmlBody += "<li>Engine: " + customerinfo.engine + "</li>";
            htmlBody += "<li>Transmission: " + customerinfo.transmission + "</li>";
            htmlBody += "<li>Exterior Color: " + customerinfo.exterior_color + "</li>";
            htmlBody += "<li>Mileage: " + customerinfo.mileage + "</li>";
            htmlBody += "<li>Condition: " + customerinfo.condition + "</li>";
            if (customerinfo.offer_value != 0)
            {
                htmlBody += "<li>Pay-off: " + customerinfo.offer_value + "$</li>";
            }
            htmlBody += "</ul>";

            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Customer Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Address: </b>" + customerinfo.address + " " + customerinfo.city + " " + customerinfo.state + " " + customerinfo.zipcode + "</li>";
            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }
            htmlBody += "</ul></div>";

            return htmlBody;
        }

        public static string BodyShareInfoNotification(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</a>";
            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + "  has shared a vehicle!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Sharing Vehicle Information</h3>";

            htmlBody += "<li>" + moreInfo + "</a>";
            htmlBody += "<li>Stock: #" + customerinfo.StockNumber + "</li></ul>";
            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Sharing Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Phone: </b>" + customerinfo.phone_number + "</li>";
            htmlBody += "<li><b>Shared With:</b> " + customerinfo.friendname + " [" + customerinfo.friendemail + "]</li>";
            htmlBody += "<li><b>Contact Prefered: </b>" + customerinfo.contact_prefer + "</li>";
            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }
            htmlBody += "</ul></div>";

            return htmlBody;
        }

        public static string BodyShareFriendMailTo(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</a>";
            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + "  has shared a vehicle with you!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Sharing Vehicle Information</h3>";
            // htmlBody += "<li><b>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</b></li>";

            htmlBody += "<li>" + moreInfo + "</a>";
            htmlBody += "<li>Stock: #" + customerinfo.StockNumber + "</li></ul>";
            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Your Friend Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Phone: </b>" + customerinfo.phone_number + "</li>";
            htmlBody += "<li><b>Contact Prefered: </b>" + customerinfo.contact_prefer + "</li>";
            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }
            htmlBody += "</ul></div>";

            return htmlBody;
        }

        public static string BodyGetQuote(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</a>";
            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + " is asking for get a quote on a vehicle!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Vehicle Information</h3>";
            htmlBody += "<li>" + moreInfo + "</a>";
            htmlBody += "<li>Stock: #" + customerinfo.StockNumber + "</li></ul>";
            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Customer Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Phone: </b>" + customerinfo.phone_number + "</li>";
            htmlBody += "<li><b>Contact Prefered: </b>" + customerinfo.contact_prefer + "</li>";
            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }
            htmlBody += "</ul></div>";

            return htmlBody;
        }

        public static string BodyMakeOffer(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</a>";
            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + " has made an offer an a vehicle!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Vehicle Information</h3>";
            htmlBody += "<li>" + moreInfo + "</a>";
            htmlBody += "<li>Stock: #" + customerinfo.StockNumber + "</li></ul>";
            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Customer Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Phone: </b>" + customerinfo.phone_number + "</li>";
            htmlBody += "<li><b>Offer Amount: </b>" + customerinfo.offer_value + "$</li>";
            htmlBody += "<li><b>Contact Prefered: </b>" + customerinfo.contact_prefer + "</li>";
            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }
            htmlBody += "</ul></div>";

            return htmlBody;
        }

        public static string BodyTestDrive(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";//"http://www.newportcoastauto.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</a>";
            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + " is looking to test drive a vehicle!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Vehicle Information</h3>";

            htmlBody += "<li>" + moreInfo + "</a>";
            htmlBody += "<li>Stock: #" + customerinfo.StockNumber + "</li></ul>";
            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Customer Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Phone: </b>" + customerinfo.phone_number + "</li>";
            htmlBody += "<li><b>Date: </b>" + customerinfo.schedule_date.ToShortDateString() + "</li>";
            htmlBody += "<li><b>Time: </b>" + customerinfo.schedule_time + "</li>";
            htmlBody += "<li><b>Contact Prefered: </b>" + customerinfo.contact_prefer + "</li>";
            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }
            htmlBody += "</ul></div>";

            return htmlBody;
        }

        public static string BodyRequestInfo(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://apps.vincontrol.com";//"http://www.newportcoastauto.com";
            if (customerinfo.IsSolded)
            {
                link += "/inventory/soldout/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            else
            {
                link += "/inventory/detail/" + customerinfo.ModelYear + "-" + customerinfo.Make.Replace(" ", "-") + "-" + customerinfo.Model.Replace(" ", "-") + "/" + customerinfo.vinnumber;
            }
            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</a>";
            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + " is asking for more information on a vehicle!</h2>";
            htmlBody += "<img src=\"http://vincontrol.com/Content/Images/logo.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Vehicle Information</h3>";

            htmlBody += "<li>" + moreInfo + "</a>";
            htmlBody += "<li>Stock: #" + customerinfo.StockNumber + "</li></ul>";
            htmlBody += "<ul style=\"list-style-type: none; margin-top: 5px; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Customer Information</h3>";
            htmlBody += "<li><b>First Name: </b>" + customerinfo.firstname + "</li>";
            htmlBody += "<li><b>Last Name: </b>" + customerinfo.lastname + "</li>";
            htmlBody += "<li><b>Email: </b>" + customerinfo.email_address + "</li>";
            htmlBody += "<li><b>Phone: </b>" + customerinfo.phone_number + "</li>";
            htmlBody += "<li><b>Contact Prefered: </b>" + customerinfo.contact_prefer + "</li>";
            if (customerinfo.comment != null)
            {
                htmlBody += "<li><b>Comments: </b>" + customerinfo.comment + "</li>";
            }
            htmlBody += "</ul></div>";

            return htmlBody;
        }
    }

    public class AdfLead
    {
        public static string CreateBodyEmailForSendClientRequestForAdf(string empty, TradeInVehicleModel vehicle, DealershipViewModel dealer)
        {

            var builder = new StringBuilder();

            builder.Append("<?xml version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<?adf version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<adf>" + Environment.NewLine);

            builder.Append("<prospect>" + Environment.NewLine);

            builder.Append("<requestdate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) + "</requestdate>" + Environment.NewLine);

            builder.Append("<status>" + "used" + "</status>" + Environment.NewLine);

            builder.Append("<vehicle>" + Environment.NewLine);

            builder.Append("<year>" + vehicle.SelectedYear + "</year>" + Environment.NewLine);

            builder.Append("<make>" + vehicle.SelectedMakeValue + "</make>" + Environment.NewLine);

            builder.Append("<model>" + vehicle.SelectedModelValue + "</model>" + Environment.NewLine);

            builder.Append("<vin>" + vehicle.Vin + "</vin>" + Environment.NewLine);

            builder.Append("<stock></stock>" + Environment.NewLine);

            builder.Append("<trim>" + vehicle.SelectedTrimValue + "</trim>" + Environment.NewLine);


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

            builder.Append("<comments>I would like to trade in " + vehicle.SelectedYear + " " + vehicle.SelectedMakeValue + " " + vehicle.SelectedModelValue + " " + vehicle.SelectedTrimValue + " for " + (vehicle.TradeInFairPrice == 0 ? "Call Dealer For Price" : ((int)vehicle.TradeInFairPrice).ToString("C")) + Environment.NewLine);

            builder.Append("</customer>" + Environment.NewLine);

            builder.Append("<vendor>" + Environment.NewLine);

            builder.Append("<vendorname></vendorname>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + dealer.DealershipName + "</name>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</vendor>" + Environment.NewLine);

            builder.Append("<provider>" + Environment.NewLine);

            builder.Append("<name part=\"full\">VINCONTROL LLC</name>" + Environment.NewLine);

            builder.Append("<service>Automotive Dealer Softwares</service>" + Environment.NewLine);

            builder.Append("<url> http://vincontrol.com </url>" + Environment.NewLine);

            builder.Append("<email> info@vincontrol.com </email>" + Environment.NewLine);

            builder.Append("<phone>855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<contact primarycontact=\"1\">" + Environment.NewLine);

            builder.Append("<name part=\"full\">Vinadvisor Service</name>" + Environment.NewLine);

            builder.Append("<email>support@vincontrol.com</email>" + Environment.NewLine);

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

        public static string AdfBodyInfoRequest(ContactViewModel customerInfo)
        {

            var builder = new StringBuilder();

            builder.Append("<?xml version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<?adf version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<adf>" + Environment.NewLine);

            builder.Append("<prospect>" + Environment.NewLine);

            builder.Append("<requestdate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) + "</requestdate>" + Environment.NewLine);

            builder.Append("<status>" + "used" + "</status>" + Environment.NewLine);

            builder.Append("<vehicle>" + Environment.NewLine);

            builder.Append("<year>" + customerInfo.ModelYear + "</year>" + Environment.NewLine);

            builder.Append("<make>" + customerInfo.Make + "</make>" + Environment.NewLine);

            builder.Append("<model>" + customerInfo.Model + "</model>" + Environment.NewLine);

            builder.Append("<vin>" + customerInfo.vinnumber + "</vin>" + Environment.NewLine);

            builder.Append("<stock>"+customerInfo.StockNumber+"</stock>" + Environment.NewLine);

            builder.Append("<trim>" + customerInfo.Trim + "</trim>" + Environment.NewLine);
            
            builder.Append("<odometer status='replaced' units='miles'>" + CommonHelper.FormatNumberInThousand(customerInfo.mileage) + "</odometer>" + Environment.NewLine);

            builder.Append("</vehicle>" + Environment.NewLine);

            builder.Append("<customer>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.firstname + " " + customerInfo.lastname + "</name>" +
                           Environment.NewLine);

            builder.Append("<phone>" + customerInfo.phone_number + "</phone>" + Environment.NewLine);

            builder.Append("<email>" + customerInfo.email_address + "</email>" + Environment.NewLine);

            builder.Append("<address type=\"home\">" + Environment.NewLine);

            builder.Append("<city></city>" + Environment.NewLine);

            builder.Append("</address>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);
            builder.Append("<comments>I would like to request more info about this vehicle" + Environment.NewLine);

            builder.Append("</customer>" + Environment.NewLine);

            builder.Append("<vendor>" + Environment.NewLine);

            builder.Append("<vendorname></vendorname>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.DealerName + "</name>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</vendor>" + Environment.NewLine);

            builder.Append("<provider>" + Environment.NewLine);

            builder.Append("<name part=\"full\">VINCONTROL LLC</name>" + Environment.NewLine);

            builder.Append("<service>Automotive Dealer Softwares</service>" + Environment.NewLine);

            builder.Append("<url>http://vincontrol.com</url>" + Environment.NewLine);

            builder.Append("<email>info@vincontrol.com</email>" + Environment.NewLine);

            builder.Append("<phone>855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<contact primarycontact=\"1\">" + Environment.NewLine);

            builder.Append("<name part=\"full\">Vinadvisor Service</name>" + Environment.NewLine);

            builder.Append("<email>support@vincontrol.com</email>" + Environment.NewLine);

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

        public static string AdfBodyTestDrive(ContactViewModel customerInfo)
        {

            var builder = new StringBuilder();

            builder.Append("<?xml version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<?adf version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<adf>" + Environment.NewLine);

            builder.Append("<prospect>" + Environment.NewLine);

            builder.Append("<requestdate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) + "</requestdate>" + Environment.NewLine);

            builder.Append("<status>" + "used" + "</status>" + Environment.NewLine);

            builder.Append("<vehicle>" + Environment.NewLine);

            builder.Append("<year>" + customerInfo.ModelYear + "</year>" + Environment.NewLine);

            builder.Append("<make>" + customerInfo.Make + "</make>" + Environment.NewLine);

            builder.Append("<model>" + customerInfo.Model + "</model>" + Environment.NewLine);

            builder.Append("<vin>" + customerInfo.vinnumber + "</vin>" + Environment.NewLine);

            builder.Append("<stock>" + customerInfo.StockNumber + "</stock>" + Environment.NewLine);

            builder.Append("<trim>" + customerInfo.Trim + "</trim>" + Environment.NewLine);

            builder.Append("<odometer status='replaced' units='miles'>" + CommonHelper.FormatNumberInThousand(customerInfo.mileage) + "</odometer>" + Environment.NewLine);

            builder.Append("</vehicle>" + Environment.NewLine);

            builder.Append("<customer>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.firstname + " " + customerInfo.lastname + "</name>" +
                           Environment.NewLine);

            builder.Append("<phone>" + customerInfo.phone_number + "</phone>" + Environment.NewLine);

            builder.Append("<email>" + customerInfo.email_address + "</email>" + Environment.NewLine);

            builder.Append("<address type=\"home\">" + Environment.NewLine);

            builder.Append("<city></city>" + Environment.NewLine);

            builder.Append("</address>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("<comments>I would like to schedule test drive at " + customerInfo.schedule_time + " on " + customerInfo.schedule_date.ToShortDateString()  + Environment.NewLine);

            builder.Append("</customer>" + Environment.NewLine);

            builder.Append("<vendor>" + Environment.NewLine);

            builder.Append("<vendorname></vendorname>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.DealerName + "</name>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</vendor>" + Environment.NewLine);

            builder.Append("<provider>" + Environment.NewLine);

            builder.Append("<name part=\"full\">VINCONTROL LLC</name>" + Environment.NewLine);

            builder.Append("<service>Automotive Dealer Softwares</service>" + Environment.NewLine);

            builder.Append("<url>http://vincontrol.com</url>" + Environment.NewLine);

            builder.Append("<email>info@vincontrol.com</email>" + Environment.NewLine);

            builder.Append("<phone>855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<contact primarycontact=\"1\">" + Environment.NewLine);

            builder.Append("<name part=\"full\">Vinadvisor Service</name>" + Environment.NewLine);

            builder.Append("<email>support@vincontrol.com</email>" + Environment.NewLine);

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

        public static string AdfBodyMakeOffer(ContactViewModel customerInfo)
        {

            var builder = new StringBuilder();

            builder.Append("<?xml version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<?adf version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<adf>" + Environment.NewLine);

            builder.Append("<prospect>" + Environment.NewLine);

            builder.Append("<requestdate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) + "</requestdate>" + Environment.NewLine);

            builder.Append("<status>" + "used" + "</status>" + Environment.NewLine);

            builder.Append("<vehicle>" + Environment.NewLine);

            builder.Append("<year>" + customerInfo.ModelYear + "</year>" + Environment.NewLine);

            builder.Append("<make>" + customerInfo.Make + "</make>" + Environment.NewLine);

            builder.Append("<model>" + customerInfo.Model + "</model>" + Environment.NewLine);

            builder.Append("<vin>" + customerInfo.vinnumber + "</vin>" + Environment.NewLine);

            builder.Append("<stock>" + customerInfo.StockNumber + "</stock>" + Environment.NewLine);

            builder.Append("<trim>" + customerInfo.Trim + "</trim>" + Environment.NewLine);

            builder.Append("<odometer status='replaced' units='miles'>" + CommonHelper.FormatNumberInThousand(customerInfo.mileage) + "</odometer>" + Environment.NewLine);

            builder.Append("</vehicle>" + Environment.NewLine);

            builder.Append("<customer>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.firstname + " " + customerInfo.lastname + "</name>" +
                           Environment.NewLine);

            builder.Append("<phone>" + customerInfo.phone_number + "</phone>" + Environment.NewLine);

            builder.Append("<email>" + customerInfo.email_address + "</email>" + Environment.NewLine);

            builder.Append("<address type=\"home\">" + Environment.NewLine);

            builder.Append("<city></city>" + Environment.NewLine);

            builder.Append("</address>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("<comments>I would like to make offer  " + CommonHelper.FormatNumberInThousand(customerInfo.offer_value) + " on this vehicle" + Environment.NewLine);

            builder.Append("</customer>" + Environment.NewLine);

            builder.Append("<vendor>" + Environment.NewLine);

            builder.Append("<vendorname></vendorname>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.DealerName + "</name>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</vendor>" + Environment.NewLine);

            builder.Append("<provider>" + Environment.NewLine);

            builder.Append("<name part=\"full\">VINCONTROL LLC</name>" + Environment.NewLine);

            builder.Append("<service>Automotive Dealer Softwares</service>" + Environment.NewLine);

            builder.Append("<url>http://vincontrol.com</url>" + Environment.NewLine);

            builder.Append("<email>info@vincontrol.com</email>" + Environment.NewLine);

            builder.Append("<phone>855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<contact primarycontact=\"1\">" + Environment.NewLine);

            builder.Append("<name part=\"full\">Vinadvisor Service</name>" + Environment.NewLine);

            builder.Append("<email>support@vincontrol.com</email>" + Environment.NewLine);

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

        public static string AdfBodyShareInfo(ContactViewModel customerInfo)
        {

            var builder = new StringBuilder();

            builder.Append("<?xml version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<?adf version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<adf>" + Environment.NewLine);

            builder.Append("<prospect>" + Environment.NewLine);

            builder.Append("<requestdate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) + "</requestdate>" + Environment.NewLine);

            builder.Append("<status>" + "used" + "</status>" + Environment.NewLine);

            builder.Append("<vehicle>" + Environment.NewLine);

            builder.Append("<year>" + customerInfo.ModelYear + "</year>" + Environment.NewLine);

            builder.Append("<make>" + customerInfo.Make + "</make>" + Environment.NewLine);

            builder.Append("<model>" + customerInfo.Model + "</model>" + Environment.NewLine);

            builder.Append("<vin>" + customerInfo.vinnumber + "</vin>" + Environment.NewLine);

            builder.Append("<stock>" + customerInfo.StockNumber + "</stock>" + Environment.NewLine);

            builder.Append("<trim>" + customerInfo.Trim + "</trim>" + Environment.NewLine);

            builder.Append("<odometer status='replaced' units='miles'>" + CommonHelper.FormatNumberInThousand(customerInfo.mileage) + "</odometer>" + Environment.NewLine);

            builder.Append("</vehicle>" + Environment.NewLine);

            builder.Append("<customer>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.firstname + " " + customerInfo.lastname + "</name>" +
                           Environment.NewLine);

            builder.Append("<phone>" + customerInfo.phone_number + "</phone>" + Environment.NewLine);

            builder.Append("<email>" + customerInfo.email_address + "</email>" + Environment.NewLine);

            builder.Append("<address type=\"home\">" + Environment.NewLine);

            builder.Append("<city></city>" + Environment.NewLine);

            builder.Append("</address>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("<comments>Share Info about this vehicle with my friend " + customerInfo.friendname + " at " +customerInfo.friendemail + Environment.NewLine);

            builder.Append("</customer>" + Environment.NewLine);

            builder.Append("<vendor>" + Environment.NewLine);

            builder.Append("<vendorname></vendorname>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.DealerName + "</name>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</vendor>" + Environment.NewLine);

            builder.Append("<provider>" + Environment.NewLine);

            builder.Append("<name part=\"full\">VINCONTROL LLC</name>" + Environment.NewLine);

            builder.Append("<service>Automotive Dealer Softwares</service>" + Environment.NewLine);

            builder.Append("<url>http://vincontrol.com</url>" + Environment.NewLine);

            builder.Append("<email>info@vincontrol.com</email>" + Environment.NewLine);

            builder.Append("<phone>855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<contact primarycontact=\"1\">" + Environment.NewLine);

            builder.Append("<name part=\"full\">Vinadvisor Service</name>" + Environment.NewLine);

            builder.Append("<email>support@vincontrol.com</email>" + Environment.NewLine);

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


        public static string AdfBodyGetAQuote(ContactViewModel customerInfo)
        {

            var builder = new StringBuilder();

            builder.Append("<?xml version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<?adf version=\"1.0\"?>" + Environment.NewLine);

            builder.Append("<adf>" + Environment.NewLine);

            builder.Append("<prospect>" + Environment.NewLine);

            builder.Append("<requestdate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) + "</requestdate>" + Environment.NewLine);

            builder.Append("<status>" + "used" + "</status>" + Environment.NewLine);

            builder.Append("<vehicle>" + Environment.NewLine);

            builder.Append("<year>" + customerInfo.ModelYear + "</year>" + Environment.NewLine);

            builder.Append("<make>" + customerInfo.Make + "</make>" + Environment.NewLine);

            builder.Append("<model>" + customerInfo.Model + "</model>" + Environment.NewLine);

            builder.Append("<vin>" + customerInfo.vinnumber + "</vin>" + Environment.NewLine);

            builder.Append("<stock>" + customerInfo.StockNumber + "</stock>" + Environment.NewLine);

            builder.Append("<trim>" + customerInfo.Trim + "</trim>" + Environment.NewLine);

            builder.Append("<odometer status='replaced' units='miles'>" + CommonHelper.FormatNumberInThousand(customerInfo.mileage) + "</odometer>" + Environment.NewLine);

            builder.Append("</vehicle>" + Environment.NewLine);

            builder.Append("<customer>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.firstname + " " + customerInfo.lastname + "</name>" +
                           Environment.NewLine);

            builder.Append("<phone>" + customerInfo.phone_number + "</phone>" + Environment.NewLine);

            builder.Append("<email>" + customerInfo.email_address + "</email>" + Environment.NewLine);

            builder.Append("<address type=\"home\">" + Environment.NewLine);

            builder.Append("<city></city>" + Environment.NewLine);

            builder.Append("</address>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("<comments>I would like to get a quote for this vehicle" + Environment.NewLine);

            builder.Append("</customer>" + Environment.NewLine);

            builder.Append("<vendor>" + Environment.NewLine);

            builder.Append("<vendorname></vendorname>" + Environment.NewLine);

            builder.Append("<contact>" + Environment.NewLine);

            builder.Append("<name part=\"full\">" + customerInfo.DealerName + "</name>" + Environment.NewLine);

            builder.Append("</contact>" + Environment.NewLine);

            builder.Append("</vendor>" + Environment.NewLine);

            builder.Append("<provider>" + Environment.NewLine);

            builder.Append("<name part=\"full\">VINCONTROL LLC</name>" + Environment.NewLine);

            builder.Append("<service>Automotive Dealer Softwares</service>" + Environment.NewLine);

            builder.Append("<url>http://vincontrol.com</url>" + Environment.NewLine);

            builder.Append("<email>info@vincontrol.com</email>" + Environment.NewLine);

            builder.Append("<phone>855-VIN-CTRL</phone>" + Environment.NewLine);

            builder.Append("<contact primarycontact=\"1\">" + Environment.NewLine);

            builder.Append("<name part=\"full\">Vinadvisor Service</name>" + Environment.NewLine);

            builder.Append("<email>support@vincontrol.com</email>" + Environment.NewLine);

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
    }
}