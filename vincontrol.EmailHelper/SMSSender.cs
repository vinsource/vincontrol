using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;
using vincontrol.Data.Model;
using SMSMessage = Twilio.SMSMessage;

namespace vincontrol.EmailHelper
{
    
    public class SmsSender
    {
        private const string AccountSid = "AC8730e843b18322a91266aba6b02c1af1";

        private const string AuthToken = "340526de8df87be71aa7f50c61cbea77";

        private const string MyPhone = "9499107292";

        private const string FromPhone = "+19492875564";

        public static void  SendSmsMessage(string body)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
          
           
            var message = twilio.SendMessage(FromPhone, MyPhone, "Jenny please?! I love you <3", new string[] { "http://www.example.com/hearts.png" });
        }
    }
}
