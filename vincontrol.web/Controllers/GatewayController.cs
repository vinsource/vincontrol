using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Vincontrol.Payment;
using Vincontrol.Payment.com.securenet.gateway;
using Vincontrol.Payment.Model;

namespace Vincontrol.Web.Controllers
{
    public class GatewayController : Controller
    {
        //
        // GET: /Gateway/
        private readonly IPaymentService _paymentService = new PaymentService();

        private void Authenticate(SNData uiData)
        {
            var response = _paymentService.Authenticate(uiData);
            var processLoginResponse = new ProcessLoginResponse();
            if (response.IsSuccessStatusCode)
            {
                SposParamsModel posParams = response.Content.ReadAsAsync<SposParamsModel>().Result;
                Response.Cookies["InputValues"]["mySecureNetID"] = posParams.SecurenetID;
                Response.Cookies["InputValues"]["mySecureNetKey"] = posParams.SecureKey;
                Response.Cookies["InputValues"]["myUserID"] = posParams.USERID.ToString();
                Response.Cookies["InputValues"]["myGroupID"] = posParams.GroupID.ToString();

                processLoginResponse.responseStr = JsonConvert.SerializeObject(posParams, Formatting.Indented);
                processLoginResponse.posParams = posParams;
            }

        }

        [HttpPost]
        public ActionResult ProcessCardTransaction(SNData uiData)
        {
            Authenticate(uiData);

            uiData.Save(Response.Cookies, Request.Cookies);

            var card = new CARD { CARDNUMBER = uiData.cardNumber, CARDCODE = uiData.cardCode, EXPDATE = uiData.expDate };

            var transaction = new TRANSACTION
            {
                MERCHANT_KEY = _paymentService.GetMerchantKey(Request.Cookies),
                DEVELOPERID = "12345",
                VERSION = "v1.0",
                CUSTOMER_BILL = new CUSTOMER_BILL
                {
                    ADDRESS = "1655 W 6th St. Suite #111",
                    EMAIL = "travis@vincontrol.com",
                    EMAILRECEIPT = "TRUE",
                    PHONE = "(949) 910-7292",
                    CITY = "Corona",
                    COMPANY = "Vincontrol LLC",
                    COUNTRY = "USA",
                    FIRSTNAME = "Travis",
                    LASTNAME = "Le",
                    STATE = "CA",
                    ZIP = "92882"
                },
                CUSTOMER_SHIP = new CUSTOMER_SHIP
                {
                    CITY = @"Corona",
                    COMPANY = @"Vincontrol LLC",
                    COUNTRY = @"USA",
                    FIRSTNAME = @"Travis",
                    LASTNAME = @"Le",
                    STATE = @"CA",
                    ZIP = @"92882"
                },
                AMOUNT = decimal.Parse(uiData.amount ?? "0"),
                CODE = SNData.codes["AUTH_ONLY"],
                METHOD = SNData.methodCodes["CREDIT"],
                CARD = card,
                ORDERID = uiData.orderID,
                CUSTOMERID = uiData.customerID
            };


            var gateway = new Gateway();

            var responseModel = new ProcessGatewayResponse();
            try
            {
                responseModel.response = gateway.ProcessTransaction(transaction);
                responseModel.responseStr = JsonConvert.SerializeObject(responseModel.response, Formatting.Indented);
            }
            catch (Exception e)
            {
                responseModel.response = new GATEWAYRESPONSE { TRANSACTIONRESPONSE = new TRANSACTIONRESPONSE() };
                responseModel.responseStr = e.Message;
            }

            return PartialView("Result", responseModel);
        }
       
    }
}
