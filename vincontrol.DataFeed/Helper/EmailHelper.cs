using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using vincontrol.DataFeed.Model;

namespace vincontrol.DataFeed.Helper
{
    public class EmailHelper
    {
        public void SendEmail(IEnumerable<string> toAddress, string subject, string body, bool bodyHtml)
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

                foreach (var tmp in toAddress.Where(tmp => !string.IsNullOrEmpty(tmp)))
                {
                    message.To.Add(new MailAddress(tmp));
                }

                var client = new SmtpClient(smtpServerAddress, 587);
                var ntlmAuthentication = new System.Net.NetworkCredential
                    (ConfigurationManager.AppSettings["DefaultFromEmail"],
                    ConfigurationManager.AppSettings["TrackEmailPass"]);

                client.Credentials = ntlmAuthentication;
                client.EnableSsl = true;
                client.Send(message);

                SendBackUpEmail(subject, body, bodyHtml);
            }
            catch (Exception)
            {

            }
        }

        public void SendEmail(MailAddress toAddress, string subject, string body, bool bodyHtml)
        {
            try
            {
                var smtpServerAddress = ConfigurationManager.AppSettings["SMTPServer"];
                var defaultFromEmail = ConfigurationManager.AppSettings["DefaultFromEmail"];
                var displayName = ConfigurationManager.AppSettings["DisplayName"];
                var fromAddress = new MailAddress(defaultFromEmail, displayName);

                var message = new MailMessage(fromAddress, toAddress)
                {
                    From = fromAddress,
                    Body = body,
                    IsBodyHtml = bodyHtml,
                    Subject = subject
                };

                var client = new SmtpClient(smtpServerAddress, 587);
                var ntlmAuthentication = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["DefaultFromEmail"],
                        ConfigurationManager.AppSettings["TrackEmailPass"]);

                client.Credentials = ntlmAuthentication;
                client.EnableSsl = true;
                client.Send(message);
            }
            catch (Exception)
            {

            }
        }

        private void SendBackUpEmail(string subject, string body, bool bodyHtml)
        {
            SendEmail(new List<MailAddress>()
                          {
                              new MailAddress(ConfigurationManager.AppSettings["TrackEmailAccount"]),
                              new MailAddress("david@vincontrol.com")
                          }
                , subject
                , body
                , bodyHtml);
        }

        public void SendEmail(List<MailAddress> toAddress, string subject, string body, bool bodyHtml)
        {
            foreach (var address in toAddress)
            {
                SendEmail(address, subject, body, bodyHtml);
            }
        }

        public List<MailAddress> CreateEmailListFromString(string emails)
        {
            return emails.Split(',').ToArray().Select(mailAddress => new MailAddress(mailAddress)).ToList();
        }

        public string CreateEmailTemplate(string content)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<!DOCTYPE html>");
            builder.AppendFormat("<html>");
            builder.AppendFormat("<head>");
            builder.AppendFormat("<title></title>");
            builder.AppendFormat("</head>");
            builder.AppendFormat("<body>");
            builder.AppendFormat("<p style=\"margin: 0; padding: 0; width: 700px; \">" + content + "</p>");
            builder.AppendFormat("</body>");

            builder.AppendFormat("</html>");

            return builder.ToString();
        }

        public string CreateEmailContent(CompanyViewModel model)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("<div> Dear " + model.Name + ",</div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>We were unable to deliver our usually scheduled data feed due to FTP connection and/or login failure. Below is the list of dealership exports affected by this problem:</div>");
            builder.AppendFormat("<br/>");
            if (model.Dealerships != null && model.Dealerships.Count > 0)
            {
                int i = 1;
                foreach (var dealer in model.Dealerships)
                {
                    builder.AppendFormat("<div><b>" + i++ + ". " + dealer.Name + " / " + dealer.Address + "</b></div>");
                }
            }
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>Please verify the FTP credentials you have on file with us:</div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>Host: " + model.FtpHost + "</div>");
            builder.AppendFormat("<div>Username: " + model.FtpUserName + "</div>");
            builder.AppendFormat("<div>Password: " + model.FtpPassword + "</div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>Please take look into this issue so we can keep our client’s inventory up to date with your services. If you feel that you have received this notification in error, please reply to this email. If you have any concerns, please do not hesitate to contact us at 1.855.VIN.CTRL.</div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>Thank you!</div>");
            builder.AppendFormat("<div>Vehicle Inventory Network</div>");

            return builder.ToString();
        }

        public string CreateEmailContent(string receiverName, string dealershipName, int numberOfDays)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("<div> Dear " + receiverName + ",</div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>We have not received an updated export for this dealership:</div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div><b>" + dealershipName + "</b></div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>Inventory for <b>" + dealershipName + "</b> has not been updated in about <b>" + numberOfDays + " day(s)</b></div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>Please review any feeds that you are currently sending to Vincontrol. If you feel that your inventory is up to date and you received this notification in error, please reply to this email. If you have any concerns, please do not hesitate to contact us at 1.855.VIN.CTRL.</div>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<br/>");
            builder.AppendFormat("<div>Thank you!</div>");
            builder.AppendFormat("<div>Vehicle Inventory Network</div>");
            return builder.ToString();
        }
    }
}
