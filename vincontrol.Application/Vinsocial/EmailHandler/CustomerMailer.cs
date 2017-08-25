using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;

namespace vincontrol.Application.Vinsocial.EmailHandler
{
    public class CustomerMailer
    {
        private const string US_EMAIL = "vinleads@vincontrol.com";
        private const string US_PW = "jeff1451";

        private static MailMessage MailCommon(string mailFrom, string mailTo, string mailName, string subject, string body)
        {
            var myMail = new MailMessage { From = new MailAddress(mailFrom, mailFrom) };

            IEnumerable<string> mailToSplit = mailTo.Split(',');
            foreach (var tmp in mailToSplit)
            {
                myMail.To.Add(tmp);
            }

            myMail.Subject = subject;
            myMail.Body = body;
            myMail.IsBodyHtml = true;
            return myMail;
        }


        private static MailMessage BuildMailMessage(ContactViewModel customerinfo)
        {
            string subject = "INFO REQUEST" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
            string body = BodyRequestInfo(customerinfo);
            var myMail = MailCommon(customerinfo.email_address, customerinfo.DealerEmail, "Thanks You for your Contact Request!", subject, body);
            return myMail;

           
        }

        public void Submit(ContactViewModel customerinfo) // .NET 4 version 
        {
            
            using (var mailMessage = BuildMailMessage(customerinfo))
            {
                var cred = new System.Net.NetworkCredential(US_EMAIL, US_PW);
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = cred,
                    EnableSsl = true
                };
                
                smtpClient.Send(mailMessage);
            }
        }

        public static string BodyRequestInfo(ContactViewModel customerinfo)
        {
            // HTML file
            string customerName = customerinfo.firstname + " " + customerinfo.lastname;
            string link = "http://www.newportcoastauto.com";
            link = customerinfo.DetailUrl;
            string moreInfo = "<a href='" + link + "'>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</a>";
            string htmlBody = "<div style=\"width: 500px;\"><div style=\"padding: 6px; background: #eee; overflow: hidden\">";
            htmlBody += "<h2 style=\"margin: 0; float: left; width: 70%\">" + customerName + " is asking for more information on a vehicle!</h2>";
            htmlBody += "<img src=\"http://apps.vincontrol.com/Content/Images/logo-vincontrol.png\" height=\"45\" style=\"float: right;\" /> </div>";
            htmlBody += "<ul style=\"list-style-type: none; margin-left: 0; padding-left: 0;\">";
            htmlBody += "<h3 style=\"background: #222222; color: white; padding: 5px;\">Vehicle Information</h3>";
            // htmlBody += "<li><b>" + customerinfo.ModelYear + " " + customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim + "</b></li>";

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
}
