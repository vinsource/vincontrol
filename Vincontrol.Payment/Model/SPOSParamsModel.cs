using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincontrol.Payment.com.securenet.gateway;

namespace Vincontrol.Payment.Model
{
    public class SposParamsModel
    {
        public bool isADMIN { get; set; }
        public bool REFUND { get; set; }
        public bool VOID { get; set; }
        public bool SALE { get; set; }
        public bool CAPTURE { get; set; }
        public int USERID { get; set; }
        public int GroupID { get; set; }
        public int GatewayID { get; set; }
        public string ResellerID { get; set; }
        public string PasswordPrompt { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int ISOId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string AdminEmailAddress { get; set; }
        public string SecurenetID { get; set; }
        public string SecureKey { get; set; }
        public string Message { get; set; }
        public bool IsValid { get; set; }
        public bool LoadKey { get; set; }
        public bool AutoLogin { get; set; }
        public string IndustryCode { get; set; }
    }

        public class MerchantIDAndKey
        {
            public String MerchantID { get; set; }
            public String MerchantKey { get; set; }
        }

        public class ProcessLoginResponse
        {
            public String responseStr { get; set; }
            public SposParamsModel posParams { get; set; }
        }

        public class ChangePasswordResponse
        {
            public String responseStr { get; set; }
        }

        public class ProcessGatewayResponse
        {
            public GATEWAYRESPONSE response { get; set; }
            public String responseStr { get; set; }
        }

        public class ProcessImageResponse
        {
            public IMAGERESPONSE response { get; set; }
            public String responseStr { get; set; }
        }

        public class ProcessBatchResponse
        {
            public BATCHDATA response { get; set; }
            public String responseStr { get; set; }
        }

     
}
