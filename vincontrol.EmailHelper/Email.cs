using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using vincontrol.ConfigurationManagement;
using vincontrol.PdfHelper;

namespace vincontrol.EmailHelper
{
    public class Email : IEmail
    {
        private const int SmtpPort = 587;

        private IPdf _pdfHelper;

        public Email()
        {
            _pdfHelper = new Pdf();
        }

        #region IEmail' Members


        public void SendNotificationEmail(IEnumerable<string> toAddress, string subject, string body)
        {
            try
            {
                var smtpServerAddress =
                    ConfigurationManager.AppSettings["SMTPGoogleServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["AlertEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage
                {
                    From = fromAddress,
                    IsBodyHtml = true,
                    Body = body,
                    Subject = "VinControl Notification - " + subject
                };

                foreach (var tmp in toAddress.Distinct())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(new MailAddress(tmp));

                }
                message.Bcc.Add(new MailAddress("david@vincontrol.com"));

                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["AlertEmail"].ToString(
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

        public void SendFlyerEmail(IEnumerable<string> toAddress, string subject, string body, byte[] byteData)
        {
            try
            {
                var smtpServerAddress =
              ConfigurationManager.AppSettings["SMTPGoogleServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["AlertEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage()
                {
                    From = fromAddress,
                    Body = body,
                    IsBodyHtml = true,
                    Subject = subject
                };


                foreach (var tmp in toAddress.Distinct())
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

                message.Bcc.Add(new MailAddress("david@vincontrol.com"));

                var ntlmAuthentication =
                 new System.Net.NetworkCredential(
                     System.Configuration.ConfigurationManager.AppSettings["AlertEmail"].ToString(
                         CultureInfo.InvariantCulture),
                     System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                         CultureInfo.InvariantCulture));

                client.Credentials = ntlmAuthentication;

                if (byteData!=null)
                {
            
                    var workStream = new MemoryStream();
                    workStream.Write(byteData, 0, byteData.Length);
                    workStream.Position = 0;
                    var attach = new System.Net.Mail.Attachment(workStream, "Flyer.pdf", "application/pdf");

                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);
                }

                client.Send(message);
            }
            catch (Exception)
            {

            }
        }

        public void SendBrochureEmail(IEnumerable<string> toAddress, string subject, string body, Stream workStream)
        {
            try
            {
                var smtpServerAddress =
              ConfigurationManager.AppSettings["SMTPGoogleServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["AlertEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage()
                {
                    From = fromAddress,
                    Body = body,
                    IsBodyHtml = true,
                    Subject = subject
                };


                foreach (var tmp in toAddress.Distinct())
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

                message.Bcc.Add(new MailAddress("david@vincontrol.com"));

                var ntlmAuthentication =
                 new System.Net.NetworkCredential(
                     System.Configuration.ConfigurationManager.AppSettings["AlertEmail"].ToString(
                         CultureInfo.InvariantCulture),
                     System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                         CultureInfo.InvariantCulture));

                client.Credentials = ntlmAuthentication;

                if (workStream != null)
                {
                    var attach = new System.Net.Mail.Attachment(workStream, "Brochure.pdf", "application/pdf");

                  message.Attachments.Add(attach);
                }

                client.Send(message);
            }
            catch (Exception)
            {

            }
        }

        public void SendEmailBucketJump(IEnumerable<string> toAddress, string subject, string body, string fileUrl)
        {
            try
            {
                var smtpServerAddress =
              ConfigurationManager.AppSettings["SMTPGoogleServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["AlertEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage
                {
                    From = fromAddress,
                    IsBodyHtml = true,
                    Body = body,
                    Subject = "VinControl Notification - " + subject
                };

                foreach (var tmp in toAddress.Distinct())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(new MailAddress(tmp));

                }

                message.Bcc.Add(new MailAddress("david@vincontrol.com"));

                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["AlertEmail"].ToString(
                            CultureInfo.InvariantCulture),
                        System.Configuration.ConfigurationManager.AppSettings["TrackEmailPass"].ToString(
                            CultureInfo.InvariantCulture));


                client.Credentials = ntlmAuthentication;

                try
                {
                    var webClient = new WebClient();

                    var byteArray = webClient.DownloadData(fileUrl);

                    if (fileUrl != null)
                    {
                        var attach = new Attachment(new MemoryStream(byteArray), "Bucket Jump Report.pdf",
                            "application/pdf");

                        message.Attachments.Add(attach);
                    }
                }
                finally
                {

                    client.Send(message);
                }

            }
            catch (Exception)
            {

            }
        }

        public void SendAdfEmail(IEnumerable<string> toAddress, string subject, string body)
        {
            try
            {
                var smtpServerAddress =
                    ConfigurationManager.AppSettings["SMTPGoogleServer"].ToString(CultureInfo.InvariantCulture);

                var defaultFromEmail =
                    ConfigurationManager.AppSettings["AlertEmail"].ToString(CultureInfo.InvariantCulture);

                var displayName = ConfigurationManager.AppSettings["DisplayName"].ToString(CultureInfo.InvariantCulture);

                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage
                {
                    From = fromAddress,
                    IsBodyHtml = false,
                    Body = body,
                    Subject = "VinControl Notification - " + subject
                };

                foreach (var tmp in toAddress.Distinct())
                {
                    if (!string.IsNullOrEmpty(tmp))
                        message.To.Add(new MailAddress(tmp));

                }
                message.Bcc.Add(new MailAddress("david@vincontrol.com"));

                var client = new SmtpClient()
                {
                    Host = smtpServerAddress,
                    Port = 587,
                    EnableSsl = true
                };

                var ntlmAuthentication =
                    new System.Net.NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["AlertEmail"].ToString(
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


        public void SendEmail(MailAddress toAddress, string subject, string body)
        {
            try
            {
                var client = CreateSmtpClient();
                var message = CreateMailMessage(GetDefaultFromEmail(), toAddress, "VinControl Notification - " + subject, body);
                message.Bcc.Add(new MailAddress(ConfigurationHandler.TrackEmailAccount));
                
                client.Send(message);
            }
            catch (Exception)
            {

            }
        }

        public void SendEmail(IEnumerable<string> toAddress, string subject, string body)
        {
            try
            {
                var client = CreateSmtpClient();
                var message = CreateMailMessage(GetDefaultFromEmail(), toAddress, "VinControl Notification - " + subject, body);
                message.Bcc.Add(new MailAddress(ConfigurationHandler.TrackEmailAccount));

                client.Send(message);
            }
            catch (Exception)
            {
                
            }
        }

        public void SendEmail(IEnumerable<string> toAddress, string subject, string body, string customedName)
        {
            try
            {
                var client = CreateSmtpClient();
                var message = CreateMailMessage(GetDefaultFromEmail(customedName), toAddress, customedName + " - " + subject, body);
                message.Bcc.Add(new MailAddress(ConfigurationHandler.TrackEmailAccount));

                client.Send(message);
            }
            catch (Exception)
            {

            }
        }

        public void SendEmail(IEnumerable<string> toAddress, string subject, string body, MemoryStream pdfContent)
        {
            try
            {
                var client = CreateSmtpClient();
                var message = CreateMailMessage(GetDefaultFromEmail(), toAddress, "VinControl Notification - " + subject, body);
                message.Bcc.Add(new MailAddress(ConfigurationHandler.TrackEmailAccount));

                if (pdfContent != null)
                {
                    var attach = new Attachment(pdfContent, "Report.pdf", "application/pdf");
                    /* Attach the newly created email attachment */
                    message.Attachments.Add(attach);
                }

                client.Send(message);
            }
            catch (Exception)
            {
                
            }
        }

        public void SendEmail(IEnumerable<string> emailList, string subject, string body, bool bodyHtml, MemoryStream pdfContent)
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

        public void SendEmailForTradeInBanner(MailAddress toAddress, string subject, string bodyEmail, string bodyPdf)
        {
            try
            {
                var client = CreateSmtpClient(true);
                var message = CreateMailMessage(GetTradeInFromEmail(), toAddress, subject, bodyEmail);
                message.Bcc.Add(new MailAddress(ConfigurationHandler.TrackEmailAccount));

                if (!String.IsNullOrEmpty(bodyPdf))
                {
                    var workStream = _pdfHelper.WritePdf(bodyPdf);

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


        #endregion

        #region Private Methods
  

        private MailMessage CreateMailMessage(MailAddress fromEmail, MailAddress toEmail, string subject, string body)
        {
            var message = new MailMessage(fromEmail, toEmail)
            {
                Body = body,
                IsBodyHtml = true,
                Subject = subject
            };
            
            return message;
        }

     
        private MailMessage CreateMailMessage(MailAddress fromEmail, IEnumerable<string> toEmails, string subject, string body)
        {
            var message = new MailMessage()
            {
                From = fromEmail,
                Body = body,
                IsBodyHtml = true,
                Subject = subject
            };
            foreach (var tmp in toEmails)
            {
                message.To.Add((tmp));
            }

            return message;
        }
        
        private SmtpClient CreateSmtpClient()
        {
            var client = new SmtpClient(ConfigurationHandler.SMTPServer, SmtpPort);
            var networkCredential = new System.Net.NetworkCredential(ConfigurationHandler.DefaultFromEmail, ConfigurationHandler.TrackEmailPass);
            client.Credentials = networkCredential;
            client.EnableSsl = true;
            return client;
        }

        private SmtpClient CreateSmtpClient(bool isTradeIn)
        {
            if (!isTradeIn) return CreateSmtpClient();

            var client = new SmtpClient(ConfigurationHandler.SMTPServer, SmtpPort);
            var networkCredential = new System.Net.NetworkCredential(ConfigurationHandler.TradeInEmail, ConfigurationHandler.TrackEmailPass);
            client.Credentials = networkCredential;
            client.EnableSsl = true;
            return client;
        }

        private MailAddress GetDefaultFromEmail()
        {
            return new MailAddress(ConfigurationHandler.DefaultFromEmail, ConfigurationHandler.DisplayName);
        }

        private MailAddress GetDefaultFromEmail(string customedName)
        {
            return new MailAddress(ConfigurationHandler.DefaultFromEmail, customedName);
        }

        private MailAddress GetTradeInFromEmail()
        {
            return new MailAddress(ConfigurationHandler.TradeInEmail, ConfigurationHandler.TradeInDisplayName);
        }
        #endregion
    }
}
