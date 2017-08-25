using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.TradeInManagement;

namespace vincontrol.VinTrade.Helpers
{
    public class LandingPageEmailHelper
    {
        public static void SendEmail(ContactViewModel customerinfo, DealershipViewModel dealer) // .NET 4 version 
        {
       
            try
            {
                switch (dealer.EmailFormat)
                {
                    case 0:
                        SendTextContent(customerinfo);
                        break;
                    case 1:
                        SendAdfContent(customerinfo);
                        break;
                    case 2:
                        SendTextContent(customerinfo);
                        SendAdfContent(customerinfo);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }

        }



        private static void SendAdfContent( ContactViewModel customerInfo)
        {
            using (var smtpClient = new SmtpClient()
            {
                Port = 587,
                EnableSsl = true
            }
            )
            using (var mailMessage = BuildMailMessageAdf(customerInfo))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            
         
        }

        private static void SendTextContent( ContactViewModel customerInfo)
        {
            using (var smtpClient = new SmtpClient())
            using (var mailMessage = BuildMailMessageText(customerInfo))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }

        }

        private static MailMessage BuildMailMessageAdf(ContactViewModel customerinfo)
        {
            string htmlBody = "";
            string subject = "";

            switch (customerinfo.contact_type)
            {
                case 1:
                    subject = "INFO REQUEST" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = AdfLead.AdfBodyInfoRequest(customerinfo);
                    break;
                case 2:
                    subject = "TEST DRIVE" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = AdfLead.AdfBodyTestDrive(customerinfo);
                    break;
                case 3:
                    subject = "MAKE OFFER" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = AdfLead.AdfBodyMakeOffer(customerinfo);
                    break;
                case 4:
                    subject = "VEHICLE SHARE NOTIFICATION" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = AdfLead.AdfBodyShareInfo(customerinfo);
                    SubmitToCustomer(customerinfo);
                    break;

                case 6:
                    subject = "GET A QUOTE" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = AdfLead.AdfBodyGetAQuote(customerinfo);
                    break;
            
                default:
                    subject = "INFO REQUEST" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = AdfLead.AdfBodyInfoRequest(customerinfo);
                    break;
            }

            var myMail = new MailMessage
            {
                From = new MailAddress(customerinfo.email_address, customerinfo.email_address)
            };

            IEnumerable<string> mailTo = customerinfo.DealerEmail.Split(',');
            foreach (var tmp in mailTo)
            {
                myMail.To.Add(tmp);
            }

            myMail.Subject = subject;
            myMail.Body = htmlBody;
            return myMail;
        }


        private static MailMessage BuildMailMessageText(ContactViewModel customerinfo)
        {
            string htmlBody = "";
            string subject = "";

            switch (customerinfo.contact_type)
            {
                case 1:
                    subject = "INFO REQUEST" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = TextLead.BodyRequestInfo(customerinfo);
                    break;
                case 2:
                    subject = "TEST DRIVE" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = TextLead.BodyTestDrive(customerinfo);
                    break;
                case 3:
                    subject = "MAKE OFFER" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = TextLead.BodyMakeOffer(customerinfo);
                    break;
                case 4:
                    subject = "VEHICLE SHARE NOTIFICATION" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = TextLead.BodyShareInfoNotification(customerinfo);
                    SubmitToCustomer(customerinfo);
                    break;

                case 6:
                    subject = "GET A QUOTE" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = TextLead.BodyGetQuote(customerinfo);
                    break;
               

                default:
                    subject = "INFO REQUEST" + " - " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                              customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
                    htmlBody = TextLead.BodyRequestInfo(customerinfo);
                    break;
            }

            var myMail = new MailMessage
            {
                From = new MailAddress(customerinfo.email_address, customerinfo.email_address)
            };

            IEnumerable<string> mailTo = customerinfo.DealerEmail.Split(',');
            foreach (var tmp in mailTo)
            {
                myMail.To.Add(tmp);
            }

            myMail.Subject = subject;
            myMail.Body = htmlBody;
            myMail.IsBodyHtml = true;
            return myMail;
        }

        private static void SubmitToCustomer(ContactViewModel customerinfo) // .NET 4 version 
        {
            using (var smtpClient = new SmtpClient())
            using (var mailMessage = BuildMailMessageToCustomer(customerinfo))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
        }


        public static MailMessage BuildMailMessageToCustomer(ContactViewModel customerinfo)
        {
            string htmlBody = "";
            string subject = "";

            subject = "[VEHICLE SHARE NOTIFICATION]" + " " + customerinfo.DealerName + "-" + customerinfo.ModelYear + " " +
                             customerinfo.Make + " " + customerinfo.Model + " " + customerinfo.Trim;
            htmlBody = TextLead.BodyShareFriendMailTo(customerinfo);

            var myMail = new MailMessage
            {
                From = new MailAddress(customerinfo.email_address, customerinfo.email_address)
            };
            ;
            myMail.To.Add(customerinfo.friendemail);
            myMail.Subject = subject;
            myMail.Body = htmlBody;
            myMail.IsBodyHtml = true;
            return myMail;
        }


      
    }
}