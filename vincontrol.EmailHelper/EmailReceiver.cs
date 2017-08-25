using System;
using System.Collections.Generic;
using System.Linq;
using OpenPop.Pop3;
using Twilio;
using vincontrol.Data.Model;
using SMSMessage = vincontrol.Data.Model.SMSMessage;

namespace vincontrol.EmailHelper
{
    public class EmailReceiver
    {
        private const string HostServer = "pop.gmail.com";

        private const string UserName = "info@vincontrol.com";

        private const string Password = "info1451";

        private const int HostPort = 995;

        private const string MyEmail = "david@vincontrol.com";

        private const string MyPhone = "+19499107292";

        private const string AccountSid = "AC8730e843b18322a91266aba6b02c1af1";

        private const string AuthToken = "340526de8df87be71aa7f50c61cbea77";

        public static IEnumerable<int> GetShortCodes()
        {
            var returnList = new List<int>();
             using (var client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(HostServer, HostPort, true);

                // Authenticate ourselves towards the server
                client.Authenticate(UserName, Password);

                var messageCount = client.GetMessageCount();

                for (int i = 1; i <=messageCount; i++)
                {
                    var message = client.GetMessage(i);
                    if (message.Headers.From.Address.Contains(MyEmail))
                    {
                        var number = Convert.ToInt32(message.Headers.Subject);
                        returnList.Add(number);
                    }

                   
                }

        
               
            }
            return returnList.AsEnumerable();
        }

        public static List<SMSMessage> GetTwilioMessages()
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var beginningDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        
            var message = twilio.ListMessages().Messages.Where(x=>x.From==MyPhone && x.DateCreated > beginningDateTime).ToList();

            var context = new VincontrolEntities();

            var todayMessages = context.SMSMessages.Where(x => x.DateCreated > beginningDateTime).ToList();


            foreach (var tmp in message)
            {
                if(todayMessages.All(x => x.SID != tmp.Sid))
                {
                    var smsMessage = new SMSMessage()
                    {
                        Body = tmp.Body,
                        DateCreated = tmp.DateCreated,
                        FromPhone = tmp.From,
                        ToPhone = tmp.To,
                        Processed = false,
                        SID = tmp.Sid,
                        DateProcessed = null

                    };

                    context.AddToSMSMessages(smsMessage);
                }
                
            }

            context.SaveChanges();

            return context.SMSMessages.Where(x => x.DateCreated > beginningDateTime && x.Processed==false && x.DateProcessed ==null).ToList();
        }
    }
}
