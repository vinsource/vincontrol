using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace vincontrol.Backend.Helper
{
    public class EmailHelper
    {
        public static void SendEmail(IEnumerable<string> toAddress, string subject, string body, Stream pdfContent, string fileName)
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

                foreach (var tmp in toAddress)
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
                    //var attach = new Attachment(pdfContent, "Report.pdf", "application/pdf");
                    var attach = new Attachment(pdfContent, fileName);

                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);
                }

                client.Send(message);

                //SendBackUpEmail(subject, body);
            }
            catch (Exception)
            {

            }
        }


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

                foreach (var tmp in toAddress)
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
            }
            catch (Exception)
            {

            }
        }
    }
}
