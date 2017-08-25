using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using vincontrol.ConfigurationManagement;
using Vincontrol.Payment.com.securenet.gateway;
using Vincontrol.Payment.Model;

namespace Vincontrol.Payment
{
    public class PaymentService : IPaymentService
    {
         private HttpResponseMessage JsonRequest(string requestURL, string path)
        {
            var client = new HttpClient { BaseAddress = new Uri(requestURL) };

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(path).Result;  // Blocking call!
            return response;
        }

        public HttpResponseMessage Authenticate(SNData uiData)
        {
            uiData.userName = ConfigurationHandler.SecureNetUserName;
            uiData.password = ConfigurationHandler.SecureNetPassword;
            var url = "http://mobpos.demo.securenet.com/";
            var path = "mobileposapi/spos/merchant/sposlogin?userid=" + uiData.userName + "&password=" + uiData.password;
            var response = JsonRequest(url, path);
            return response;
        }

        public MERCHANT_KEY GetMerchantKey(HttpCookieCollection cookies)
        {
            var merchantKey = new MERCHANT_KEY();
            try
            {
                merchantKey.GROUPID = 0;
                merchantKey.SECURENETID = int.Parse(cookies["InputValues"]["mySecureNetID"]);
                merchantKey.SECUREKEY = cookies["InputValues"]["mySecureNetKey"];
            }
            catch { }
            return merchantKey;
        }
    }
}
