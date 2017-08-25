using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using vincontrol.Constant;
using vincontrol.Data.Model;

namespace vincontrol.EmailHelper
{

   
    public class NotificationHelper
    {
        public static string CreateBodyEmailForBucketJumpAlert(Inventory inventory,EmailWaitingList notificationEmail)
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
            builder.AppendFormat("<span style=\" font-weight: bold;\">Today Bucket Jump Alert</span><br />");
            builder.AppendFormat(inventory.Vehicle.Year + " " + inventory.Vehicle.Make + " " + inventory.Vehicle.Model + " " + inventory.Vehicle.Trim + "<br />");
            builder.AppendFormat("Vin : " + inventory.Vehicle.Vin + "<br />");
            builder.AppendFormat("Stock Number : " + inventory.Stock + "<br />");
            builder.AppendFormat("Dealer : " + inventory.Dealer.Name + "<br />");
            builder.AppendFormat("Address : " + inventory.Dealer.Address + " " + inventory.Dealer.City + ", " + inventory.Dealer.State + " " + inventory.Dealer.ZipCode + "<br />");
            builder.AppendFormat("Suggested Sale Price : " + notificationEmail.NewValue.GetValueOrDefault().ToString("c0") + "<br />");
            builder.AppendFormat("Date: " + notificationEmail.DateStamp.GetValueOrDefault().ToShortDateString() + " " + notificationEmail.DateStamp.GetValueOrDefault().ToShortTimeString() + "<br />");
            builder.AppendFormat("</td>");
            builder.AppendFormat("<td align=\"right\" width=\"150\" style=\"border-bottom: 4px #003399 solid;\">");
            builder.AppendFormat("<img width=\"150\" src=\"http://vincontrol.com/Content/Images/logo.png\" alt=\"Vincontrol\"/>");
            builder.AppendFormat("</td>");
            builder.AppendFormat("</tr>");
            builder.AppendFormat("<tr><td colspan=\"3\" height=\"5\"></tr>");
            builder.AppendFormat("<tr>");
            builder.AppendFormat(
                "	<td colspan=\"3\" style=\"padding: 10px; font-size: .9em; background: #111111; color: white;\">");
            builder.AppendFormat("<p style=\"margin: 0;\">" + "Bucket jump was finished by " +notificationEmail.User.Name + ". The price was suggestively changed from " + inventory.SalePrice.GetValueOrDefault().ToString("C0") + " to " + notificationEmail.NewValue.GetValueOrDefault().ToString("C0") + " </p>");
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

        public static string CreateBodyEmailForUpdateSalePrice(Inventory inventory, EmailWaitingList notificationEmail, int changeType)
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
            builder.AppendFormat(inventory.Vehicle.Year + " " + inventory.Vehicle.Make + " " + inventory.Vehicle.Model + " " + inventory.Vehicle.Trim +
                                 "<br />");
            builder.AppendFormat("Stock : " + inventory.Stock + "<br />");
            builder.AppendFormat("Vin : " + inventory.Vehicle.Vin + "<br />");
            builder.AppendFormat("Dealer : " + inventory.Dealer.Name + "<br />");
            builder.AppendFormat("Date : " + notificationEmail.DateStamp.GetValueOrDefault().ToShortDateString() + " " + notificationEmail.DateStamp.GetValueOrDefault().ToShortTimeString() + "<br />");
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

            if(changeType==Constanst.NotificationType.SalePriceChange)
                builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 "Price is changed from " + notificationEmail.OldValue.GetValueOrDefault().ToString("C0") + " to " + notificationEmail.NewValue.GetValueOrDefault().ToString("C0") + " by " + notificationEmail.User.Name + "." + "</p>");
            if (changeType == Constanst.NotificationType.AcvChange)
                builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 "ACV is changed from " + notificationEmail.OldValue.GetValueOrDefault().ToString("C0") + " to " + notificationEmail.NewValue.GetValueOrDefault().ToString("C0") + " by " + notificationEmail.User.Name + "." + "</p>");
            if (changeType == Constanst.NotificationType.DealerCostChange)
                builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 "Dealer Cost is changed from " + notificationEmail.OldValue.GetValueOrDefault().ToString("C0") + " to " + notificationEmail.NewValue.GetValueOrDefault().ToString("C0") + " by " + notificationEmail.User.Name + "." + "</p>");
            if (changeType == Constanst.NotificationType.MileageChange)
                builder.AppendFormat("<p style=\"margin: 0;\">" +
                                 "Mileage is changed from " + notificationEmail.OldValue.GetValueOrDefault().ToString("C0") + " to " + notificationEmail.NewValue.GetValueOrDefault().ToString("C0") + " by " + notificationEmail.User.Name + "." + "</p>");

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

        public static string CreateBodyEmailForAddToInventory(Inventory inventory, EmailWaitingList notificationEmail)
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
            builder.AppendFormat(inventory.Vehicle.Year + " " + inventory.Vehicle.Make + " " + inventory.Vehicle.Model + inventory.Vehicle.Trim + "<br />");
            builder.AppendFormat("Vin : " + inventory.Vehicle.Vin + "<br />");
            builder.AppendFormat("Stock : " + inventory.Stock + "<br />");
            builder.AppendFormat("Dealer : " + inventory.Dealer.Name + "<br />");
            builder.AppendFormat("Date : " + notificationEmail.DateStamp.GetValueOrDefault().ToShortDateString() + " " + notificationEmail.DateStamp.GetValueOrDefault().ToShortTimeString() + "<br />");
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
                                 inventory.Vehicle.Year + " " + inventory.Vehicle.Make + " " + inventory.Vehicle.Model + inventory.Vehicle.Trim + " was just added to inventory by " + notificationEmail.User.Name + " from " + inventory.Dealer.Name + "</p>");
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

        public static string CreateBodyEmailForAddToWholeSale(Inventory inventory, EmailWaitingList notificationEmail)
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
            builder.AppendFormat(inventory.Vehicle.Year + " " + inventory.Vehicle.Make + " " + inventory.Vehicle.Model + inventory.Vehicle.Trim + "<br />");
            builder.AppendFormat("Vin : " + inventory.Vehicle.Vin + "<br />");
            builder.AppendFormat("Stock : " + inventory.Stock + "<br />");
            builder.AppendFormat("Dealer : " + inventory.Dealer.Name + "<br />");
            builder.AppendFormat("Date : " + notificationEmail.DateStamp.GetValueOrDefault().ToShortDateString() + " " + notificationEmail.DateStamp.GetValueOrDefault().ToShortTimeString() + "<br />");
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
                                 inventory.Vehicle.Year + " " + inventory.Vehicle.Make + " " + inventory.Vehicle.Model + inventory.Vehicle.Trim + " was just added to inventory by " + notificationEmail.User.Name + " from " + inventory.Dealer.Name + "</p>");
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

        public static string CreateBodyEmailForAppraisal(Appraisal appraisal, EmailWaitingList notificationEmail)
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
            builder.AppendFormat("<span style=\" font-weight: bold;\">VinControl Alert - New Appraisal</span><br />");
            builder.AppendFormat(appraisal.Vehicle.Year + " " + appraisal.Vehicle.Make + " " + appraisal.Vehicle.Model  + " " + appraisal.Vehicle.Trim + "<br />");
            builder.AppendFormat("Vin : " + appraisal.Vehicle.Vin+ "<br />");
            builder.AppendFormat("Dealer : " + appraisal.Dealer.Name + "<br />");
            builder.AppendFormat("Date : " + notificationEmail.DateStamp.GetValueOrDefault().ToShortDateString() + " " + notificationEmail.DateStamp.GetValueOrDefault().ToShortTimeString() + "<br />");
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
                                 appraisal.Vehicle.Year + " " + appraisal.Vehicle.Make + " " + appraisal.Vehicle.Model + " " + appraisal.Vehicle.Trim + " was just appriased by " + notificationEmail.User.Name + " from " + appraisal.Dealer.Name + "</p>");
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

        public static string CreateBodyEmailForSendAppraisalRequestForAdf(Appraisal appraisal, EmailWaitingList notificationEmail)
        {
            var builder = new StringBuilder();

            builder.AppendLine("<?xml version=\"1.0\"?>");
            builder.AppendLine("<?adf version=\"1.0\"?>");

            builder.AppendLine("<adf>");
            builder.AppendLine("<prospect>");
            builder.AppendLine("<requestdate>" + notificationEmail.DateStamp.GetValueOrDefault().ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) + "</requestdate>");
            builder.AppendLine("<vehicle interest=\"buy\" status=\"used\">");
            builder.AppendLine("<year>" + appraisal.Vehicle.Year + "</year>");
            builder.AppendLine("<make>" + appraisal.Vehicle.Make + "</make>");
            builder.AppendLine("<model>" + appraisal.Vehicle.Model + "</model>");
            builder.AppendLine("<trim>" + appraisal.Vehicle.Trim + "</trim>");
            builder.AppendLine("<vin>" + appraisal.Vehicle.Vin + "</vin>");
            builder.AppendLine("<stock>" + appraisal.Stock + "</stock>");
            builder.AppendLine("</vehicle>");

            if (appraisal.AppraisalCustomerId.GetValueOrDefault()>0)
            {
                builder.AppendLine("<customer>");
                builder.AppendLine("<contact>");
                builder.AppendLine("<name part=\"full\">" + appraisal.AppraisalCustomer.FirstName + " " +
                               appraisal.AppraisalCustomer.LastName + "</name>" );
                builder.AppendLine("<phone>" + appraisal.AppraisalCustomer.Phone + "</phone>");
                builder.AppendLine("<email>" + appraisal.AppraisalCustomer.Email + "</email>");
                builder.AppendLine("<address type=\"home\">");
                builder.AppendLine("<city></city>");
                builder.AppendLine("</address>");
                builder.AppendLine("</contact>");


                builder.AppendLine("<comments>" + appraisal.Note + "</comments>");

                builder.AppendLine("</customer>");
            }

            builder.AppendLine("<vendor>");

            builder.AppendLine("<vendorname></vendorname>");

            builder.AppendLine("<contact>");

            builder.AppendLine("<name part=\"full\">" + appraisal.Dealer.Name + "</name>");

            builder.AppendLine("</contact>");

            builder.AppendLine("</vendor>");

            builder.AppendLine("<provider>");

            builder.AppendLine("<name part=\"full\">Vincontrol LLC</name>");
            builder.AppendLine("<service>Digital Marketing Solutions</service>");
            builder.AppendLine("<url> http://vincontrol.com </url>");
            builder.AppendLine("<email>support@vincontrol.com</email>");
            builder.AppendLine("<phone>855-VIN-CTRL</phone>");
            builder.AppendLine("<contact primarycontact=\"1\">");
            builder.AppendLine("<name part=\"full\">Vincontrol Appraisal Service</name>");
            builder.AppendLine("<email>support@vincontrol.com</email>");
            builder.AppendLine("<phone type=\"voice\" time=\"day\">855-VIN-CTRL</phone>");
            builder.AppendLine("<address>");
            builder.AppendLine("<street line=\"1\">9881 Irivne Center Dr</street>");
            builder.AppendLine("<street line=\"2\"></street>");
            builder.AppendLine("<city>Irvine</city>");
            builder.AppendLine("<regioncode>CA</regioncode>");
            builder.AppendLine("<postalcode>92618</postalcode>");
            builder.AppendLine("<country>US</country>");

            builder.AppendLine("</address>");
            builder.AppendLine("</contact>");
            builder.AppendLine("</provider>");
            builder.AppendLine("</prospect>");
            builder.AppendLine("</adf>");

            return builder.ToString();
        }

        public static string CreateBodyEmailForFlyer(Inventory inventory, EmailWaitingList notificationEmail)
        {
            var webClient = new WebClient();
            var emailContent = webClient.DownloadString("http://apps.vincontrol.com/templates/flyeremail.txt");

            emailContent = emailContent.Replace(EmailTemplateReader.UserFullName, notificationEmail.User.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.DealerName, inventory.Dealer.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.Year, inventory.Vehicle.Year.GetValueOrDefault().ToString());
            emailContent = emailContent.Replace(EmailTemplateReader.Make, inventory.Vehicle.Make);
            emailContent = emailContent.Replace(EmailTemplateReader.Model, inventory.Vehicle.Model);
            emailContent = emailContent.Replace(EmailTemplateReader.Trim, inventory.Vehicle.Trim);
            emailContent = emailContent.Replace(EmailTemplateReader.Phone, inventory.Dealer.Phone);
            emailContent = emailContent.Replace(EmailTemplateReader.Address, inventory.Dealer.Address + " " + inventory.Dealer.City + ", " + inventory.Dealer.State + " " + inventory.Dealer.ZipCode);
            if (!String.IsNullOrEmpty(inventory.Vehicle.Trim))
            {
                emailContent = emailContent.Replace(EmailTemplateReader.LandingPageURL,
                                               string.Format("http://www.vincapture.com/Inventory/{0}/{1}-{2}-{3}-{4}/{5}",
                                                   inventory.Dealer.Name.ToLower().Replace(" ", ""),
                                                   inventory.Vehicle.Year,
                                                   inventory.Vehicle.Make.Replace(" ", ""),
                                                   inventory.Vehicle.Model.Replace(" ", ""),
                                                   inventory.Vehicle.Trim.Replace(" ", ""),
                                                   inventory.Vehicle.Vin));
            }
            else
            {
                emailContent = emailContent.Replace(EmailTemplateReader.LandingPageURL,
                                                string.Format("http://www.vincapture.com/Inventory/{0}/{1}-{2}-{3}-{4}/{5}",
                                                    inventory.Dealer.Name.ToLower().Replace(" ", ""),
                                                    inventory.Vehicle.Year,
                                                    inventory.Vehicle.Make.Replace(" ", ""),
                                                    inventory.Vehicle.Model.Replace(" ", ""),
                                                    "NA",
                                                    inventory.Vehicle.Vin));
            }
           
            
            return emailContent;
        }

        public static string CreateBodyEmailForBrochure(EmailWaitingList notificationEmail)
        {
            string brochureUrl = string.Format("{0}/Inventory/DownloadBrochure?Year={1}&Make={2}&Model={3}", "http://apps.vincontrol.com/", notificationEmail.Year
                , notificationEmail.Make.Replace(" ", "%20"), notificationEmail.Model.Replace(" ", "%20"));
            var webClient = new WebClient();
            var emailContent = webClient.DownloadString("http://apps.vincontrol.com/templates/BrochureEmail.txt");
            emailContent = emailContent.Replace(EmailTemplateReader.UserFullName, notificationEmail.User.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.DealerName, notificationEmail.User.Dealer.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.Year, notificationEmail.Year.ToString());
            emailContent = emailContent.Replace(EmailTemplateReader.Make, notificationEmail.Make);
            emailContent = emailContent.Replace(EmailTemplateReader.Model, notificationEmail.Model);
            emailContent = emailContent.Replace(EmailTemplateReader.Phone, notificationEmail.User.Dealer.Phone);
            emailContent = emailContent.Replace(EmailTemplateReader.Address, notificationEmail.User.Dealer.Address + " " + notificationEmail.User.Dealer.City + ", "
                + notificationEmail.User.Dealer.State + " " + notificationEmail.User.Dealer.ZipCode);
            emailContent = emailContent.Replace(EmailTemplateReader.BrochureURL, brochureUrl);

            return emailContent;
        }

        public static string CreatePdfHtmlCodeForFlyer(Inventory inventory, Carfax carfax)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title>Vincontrol Flyer</title>");

            builder.AppendFormat("<style type=\"text/css\">");
            builder.AppendFormat("body {{ font-family: Arial; margin: 0px; }}");
            builder.AppendFormat(".flyer-container {{ width: 1100px; margin: 0px auto; padding-bottom: 30px; }}");
            builder.AppendFormat(".bottom_line {{ border-bottom: 2.5px solid gray; }}");
            builder.AppendFormat(".top_line {{ border-top: 2.5px solid #333; margin-top: 2px; }}");
            builder.AppendFormat(".flyer_header_right {{ float: right; width: 45%; text-align: right; padding-right: 10px; padding-top: 25px; }}");
            builder.AppendFormat(".flyer_header_left {{ font-size: 26px; color: #555; float: left; width: 50%; font-weight: bold; padding: 24px 5px; }}");
            builder.AppendFormat(".flyer_header {{ height: 80px; background-color: #F8F8F8; }}");
            builder.AppendFormat(".flyer_header_name {{ display: block; font-size: 19px; font-weight: bold; color: #555; }}");
            builder.AppendFormat(".flyer_content_left {{ float: left; width: 40%; }}");
            builder.AppendFormat(".flyer_title {{ font-size: 22px; height: 35px; font-weight: bold; color: #777; }}");
            builder.AppendFormat(".flyer_content_right {{ padding: 5px; float: right; width: 58%; position: relative; }}");
            builder.AppendFormat(".flyer_dealer_address_bottom {{ position: absolute; bottom: 0px; text-align: center; font-weight:bold; font-size: 17px; color: #555555; }}");
            builder.AppendFormat(".flyer_cardes {{ line-height: 21px; font-size: 12px; padding-left: 28px; }}");
            builder.AppendFormat(".flyer_content_holder {{ padding-top: 5px; }}");
            builder.AppendFormat(".flyer_content_right > div {{ padding: 20px; }}");
            builder.AppendFormat(".flyer_mainImg > img {{ width: 96%; }}");
            builder.AppendFormat(".flyer_carfax_info ul {{ list-style-type: none; padding-left: 20px; margin: 10px 0px; }}");
            builder.AppendFormat(".flyer_carfax_info ul li {{ font-size: 13px; padding-bottom: 5px; }}");
            builder.AppendFormat(".flyer_carfax_logo {{ margin-top: 10px; }}");
            builder.AppendFormat(".flyer_list_imgs_item {{ width: 31%; float: left; margin-right: 5px; margin-top: 2px; }}");
            builder.AppendFormat(".flyer_list_imgs_item > img {{ width: 100%; }}");
            builder.AppendFormat(".flyer_ymm {{ font-size: 30px; font-weight: bold; color: #555; }}");
            builder.AppendFormat(".flyer_dealer_address {{ line-height: 26px; font-size: 19px !important; }}");
            builder.AppendFormat(".flyer_carinfo ul {{ list-style-type: none; padding-left: 20px; }}");
            builder.AppendFormat(".flyer_carinfo ul li {{ font-size: 14px; margin-bottom: 5px; }}");
            builder.AppendFormat(".flyer_carinfo nobr {{ font-size: 15px; font-weight: bold; color: #444; }}");
            builder.AppendFormat(".flyer_noadditional {{ padding: 15px; }}");
            builder.AppendFormat(".flyer_noadditional label {{ display: block; text-align: center; font-size: 28px; color: #555; }}");
            builder.AppendFormat(".flyer_noadditional ul {{ list-style-type: none; margin: 0px 10px; padding-left: 15px; }}");
            builder.AppendFormat(".flyer_noadditional li {{ font-size: 13px; margin-bottom: 5px; color: #444444; }}");
            builder.AppendFormat(".flyer_map_holder {{ margin-top: 5px; }}");
            builder.AppendFormat(".flyer_map_holder small a {{ text-align: center !important; display: block; padding-top: 5px; font-size: 22px; color: #333; text-decoration: none; }}");
            builder.AppendFormat(".flyer_cardes {{ text-align: justify; }}");
            builder.AppendFormat("</style>");

            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");

            // Start flyer-container
            builder.AppendFormat("<div class=\"flyer-container\">");

            builder.AppendFormat("<div class=\"flyer_header bottom_line\">");
            builder.AppendFormat("<div class=\"flyer_header_left\">" + inventory.Dealer.Name + "</div>");
            builder.AppendFormat("<div class=\"flyer_header_right\"><label class=\"flyer_header_name\">" + "Contact Us" + "</label><label class=\"flyer_header_name\">" + inventory.Dealer.Phone + "</label>" + "</div>");
            builder.AppendFormat("</div>");

            // Start top_line flyer_content_holder
            builder.AppendFormat("<div class=\"top_line flyer_content_holder\">");

            // Start flyer_content_left
            builder.AppendFormat("<div class=\"flyer_content_left\">");
            builder.AppendFormat("<div class=\"flyer_mainImg\">" + "<img src=\"" + (String.IsNullOrEmpty(inventory.PhotoUrl) ? inventory.Vehicle.DefaultStockImage : inventory.PhotoUrl.Split(',').ToArray()[0]) + "\" />");
            builder.AppendFormat("</div>");

            if (carfax != null)
            {
                builder.AppendFormat("<div class=\"flyer_carfax_holder\">");
                builder.AppendFormat("<div class=\"flyer_carfax_logo\">" + "<img src=\"http://www.carfax.com/media/img/cfx/homepage/carfax-logo.png\" style=\"width:157px;height:30px;\" />" + "</div>");
                builder.AppendFormat("<div class=\"flyer_carfax_info\">");
                builder.AppendFormat("<ul>");
                builder.AppendFormat("<li>" + carfax.Owner.GetValueOrDefault() + " Owner(s)</li>");

                var windowStickers = carfax.WindowSticker.Split('|').ToArray();
                var textwindowStickers = windowStickers.Where(i => !string.IsNullOrEmpty(i.Replace("http://www.carfaxonline.com/phoenix/img/checkmark.jpg", "").Replace(",", "")))
                                          .Select(i => i.Replace("http://www.carfaxonline.com/phoenix/img/checkmark.jpg", "").Replace(",", "")).ToArray();
                
                foreach (var item in textwindowStickers)
                {
                    builder.AppendFormat("<li>" + item + "</li>");
                }

                builder.AppendFormat("</ul>");
                builder.AppendFormat("</div>");
                builder.AppendFormat("</div>");
            }
         
            builder.AppendFormat("<div class=\"flyer_list_imgs\">");
            // Foreach Model.CarImageUrl
            if (!String.IsNullOrEmpty(inventory.ThumbnailUrl))
            {
                foreach (var image in inventory.ThumbnailUrl.Split(',').ToArray())
                {
                    builder.AppendFormat("<div class=\"flyer_list_imgs_item\">");
                    builder.AppendFormat("<img src=\"" + image + "\" />");
                    builder.AppendFormat("</div>");
                }
            }

            // End flyer_content_left
            builder.AppendFormat("</div>");
            builder.AppendFormat("</div>");

            // Start center_line flyer_content_right
            builder.AppendFormat("<div class=\"center_line flyer_content_right\">");

            builder.AppendFormat("<div class=\"flyer_carinfo_holder bottom_line\">");
            builder.AppendFormat("<div class=\"flyer_ymm\">" + inventory.Vehicle.Year + " " + inventory.Vehicle.Make + " " + inventory.Vehicle.Model + " " + inventory.Vehicle.Trim + "</div>");
            builder.AppendFormat("<div class=\"flyer_carinfo\">");
            builder.AppendFormat("<ul>");
            builder.AppendFormat("<li><nobr>Vin:</nobr>" + inventory.Vehicle.Vin + "</li>");
            builder.AppendFormat("<li><nobr>Stock:</nobr>" + inventory.Stock + "</li>");
            builder.AppendFormat("<li><nobr>Odometer:</nobr>" + inventory.Mileage + "</li>");
            builder.AppendFormat("<li><nobr>Ext. Color:</nobr>" + inventory.ExteriorColor + "</li>");
            builder.AppendFormat("<li><nobr>Int. Color:</nobr>" + inventory.Vehicle.InteriorColor + "</li>");
            builder.AppendFormat("<li><nobr>Trans:</nobr>" + inventory.Vehicle.Tranmission + "</li>");
            builder.AppendFormat("<li><nobr>Engine:</nobr>" + inventory.Vehicle.EngineType + "</li>");
            builder.AppendFormat("</ul>");
            builder.AppendFormat("</div>");
            builder.AppendFormat("</div>");

            builder.AppendFormat("<div class=\"flyer_carinfo_holder bottom_line top_line\">");
            builder.AppendFormat("<div class=\"flyer_title\">Description" + "</div><div class=\"flyer_cardes\">" + inventory.Descriptions + "<div>");
            builder.AppendFormat("</div>");

            builder.AppendFormat("<div class=\"flyer_carinfo_holder top_line\">");
            builder.AppendFormat("<div class=\"flyer_title\">Standard Options" + "</div>");
            if (!String.IsNullOrEmpty(inventory.Vehicle.StandardOptions) )
            {
                builder.AppendFormat("<div class=\"flyer_noadditional\">");
                builder.AppendFormat("<ul>");
                foreach (var standardOption in inventory.Vehicle.StandardOptions.Split(',').ToArray())
                {
                    builder.AppendFormat("<li>" + standardOption + "</li>");
                }
                builder.AppendFormat("</ul>");
                builder.AppendFormat("</div>");
            }
            builder.AppendFormat("</div>");

            builder.AppendFormat("<div class=\"flyer_carinfo_holder bottom_line top_line\">");
            builder.AppendFormat("<div class=\"flyer_title\">Additional Options and Packages</div>");
            if (!String.IsNullOrEmpty(inventory.AdditionalOptions))
            {
                builder.AppendFormat("<div class=\"flyer_noadditional\">");
                builder.AppendFormat("<ul>");
                foreach (var option in inventory.AdditionalOptions.Split(',').ToArray())
                {
                    builder.AppendFormat("<li>" + option + "</li>");
                }
                builder.AppendFormat("</ul>");
                builder.AppendFormat("<label>Call us at</label><label>" + inventory.Dealer.Phone + "</label><label>For More Information</label>");
                builder.AppendFormat("</div>");
            }
            else
            {
                builder.AppendFormat("<div class=\"flyer_noadditional\">");
                builder.AppendFormat("</div>");
            }
            builder.AppendFormat("</div>");

            builder.AppendFormat("<div class=\"flyer_carinfo_holder top_line\">");
            builder.AppendFormat("<div class=\"flyer_noadditional\">");
            builder.AppendFormat("<label>" + inventory.Dealer.Name + "</label>");
            builder.AppendFormat("<label class=\"flyer_dealer_address\">" + inventory.Dealer.Address + "</label>");
            builder.AppendFormat("<label class=\"flyer_dealer_address\">" + inventory.Dealer.City + ", " + inventory.Dealer.State + " " + inventory.Dealer.ZipCode+"</label>");
            builder.AppendFormat("</div>");
            builder.AppendFormat("</div>");

            // End center_line flyer_content_right
            builder.AppendFormat("</div>");

            builder.AppendFormat("<div style=\"clear: both\"></div>");
            // End top_line flyer_content_holder


            // End flyer-container
            builder.AppendFormat("</div>");

          
            builder.AppendFormat("</body>");

            builder.AppendFormat("</html>");

            return builder.ToString();
        }

        public static string CreateBodyExpiredRebate(IEnumerable<Rebate> rebates)
        {
            var builder = new StringBuilder();

            builder.AppendLine("There are following rebates that have stopped being applied today : ");

            builder.AppendLine("<br>");

            var index = 1;

            foreach (var tmp in rebates)
            {
                builder.Append(index + ".  " + tmp.Year + " " + tmp.Make + " " + tmp.Model + " " + tmp.Trim + " / Body Type : " + tmp.BodyType + " - Manufacturer Rebate  : " + tmp.ManufactureReabte + ".  Expiration Date = " + tmp.ExpiredDate.GetValueOrDefault().ToShortDateString());
                builder.AppendLine("<br>");
                index++;
            }

            builder.AppendLine("<br>");
            builder.AppendLine("<br>");
         
            builder.AppendLine("Delivered by : The Vincontrol Team");

            return builder.ToString();
        }

        public static string CreateBodyNewRebate(IEnumerable<Rebate> rebates)
        {
            var builder = new StringBuilder();

            builder.AppendLine("There are following rebates that have been applied today : ");

            builder.AppendLine("<br>");

            var index = 1;

            foreach (var tmp in rebates)
            {
                builder.Append(index + ".  " + tmp.Year + " " + tmp.Make + " " + tmp.Model + " " + tmp.Trim + " / Body Type : " + tmp.BodyType + " - Manufacturer Rebate  : " + tmp.ManufactureReabte + ".  Expiration Date = " + tmp.ExpiredDate.GetValueOrDefault().ToShortDateString());
                builder.AppendLine("<br>");
                index++;
            }

            builder.AppendLine("<br>");
            builder.AppendLine("<br>");

            builder.AppendLine("Delivered by : The Vincontrol Team");


            return builder.ToString();
        }
    }
}
