using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace vincontrol.EmailHelper
{
    public interface IEmail
    {
        void SendEmail(MailAddress toAddress, string subject, string body);
        void SendEmail(IEnumerable<string> toAddress, string subject, string body);
        void SendEmail(IEnumerable<string> toAddress, string subject, string body, string customedName);
        void SendEmail(IEnumerable<string> toAddress, string subject, string body, MemoryStream pdfContent);
        void SendEmail(IEnumerable<string> emailList, string subject, string body, bool bodyHtml, MemoryStream pdfContent);
        void SendEmailForTradeInBanner(MailAddress toAddress, string subject, string bodyEmail, string bodyPdf);
        void SendEmailBucketJump(IEnumerable<string> toAddress, string subject, string body, string fileUrl);
        void SendNotificationEmail(IEnumerable<string> toAddress, string subject, string body);
        void SendFlyerEmail(IEnumerable<string> toAddress, string subject, string body, byte[] byteData);
        void SendAdfEmail(IEnumerable<string> toAddress, string subject, string body);
        void SendBrochureEmail(IEnumerable<string> toAddress, string subject, string body, Stream workStream);
    }
}
