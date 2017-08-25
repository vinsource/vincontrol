using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vincontrol.Payment.com.securenet.gateway;
using Vincontrol.Payment.Model;

namespace Vincontrol.Payment
{
    public interface IPaymentService
    {
        HttpResponseMessage Authenticate(SNData uiData);
        MERCHANT_KEY GetMerchantKey(HttpCookieCollection cookies);

    }
}
