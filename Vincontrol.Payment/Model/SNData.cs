using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Vincontrol.Payment.Model
{
    public class SNData
    {
        public SNData()
        {
            cardNumber = "4444333322221111";
            trackData = "";
        }

        public static DateTime DateFromDayOfMonth(int dayOfMonth, int minDaysAhead)
        {
            DateTime today = DateTime.Today;
            if (dayOfMonth <= today.Day + minDaysAhead)
                today = today.AddMonths(1);
            DateTime dateOnDayOfMonth = new DateTime(today.Year, today.Month, dayOfMonth);
            return dateOnDayOfMonth;
        }

        public static DateTime DateYearsFromToday(int years)
        {
            DateTime today = DateTime.Today;
            return today.AddYears(years);
        }

        public static DateTime DateMonthsFromDate(DateTime startDate, int numMonths)
        {
            return startDate.AddMonths(numMonths);
        }

        public static class Constants
        {
            public const string SN_BATCH_ID_CURRENT_BATCH = "0";
            public const string SN_BATCH_ID_PREV_BATCH = "1";
        }

        public static Dictionary<string, string>
        codes = new Dictionary<string, string> { 
            { "AUTH_ONLY", "0000" },
            { "AUTH_CAPTURE", "0100" },
            { "PRIOR_AUTH_CAPTURE", "0200" },
            { "UPDATE_TRANS_INFO", "0201" },
            { "CAPTURE_ONLY", "0300" },
            { "VOID", "0400" },
            { "CREDIT", "0500" },
            { "FORCE_CREDIT", "0600" },
            { "VERIFICATION", "0700" } };

        public static Dictionary<string, int>
        responseCodes = new Dictionary<string, int> { // for your convenience
            { "Approved", 1 },
            { "Declined", 2 },
            { "Error", 3 } }; // or invalid data

        public static Dictionary<string, int>
        actionCodes = new Dictionary<string, int> {
            { "ADD", 1 },
            { "UPDATE", 2 },
            { "DELETE", 3 } };

        public static Dictionary<string, string>
        industrySpecificData = new Dictionary<string, string> {
            //  For E-commerce transactions:
            { "PHYSICAL_GOODS", "P" },
            { "DIGITAL_GOODS", "D" },
        
            //  For MO/TO transactions:

            //  Single purchase transaction (AVS is required)
            { "SINGLE_PURCHASE", "1" },
            //  Recurring billing transaction (do not submit AVS)
            { "RECURRING_BILLING", "2" },
            //  Installment transaction
            { "INSTALLMENT", "3" } };

        public static Dictionary<string, string>
        secCodes = new Dictionary<string, string> {
            { "AccountsReceiveableEntry"      , "ARC" },
            { "BackOfficeConversion"          , "BOC" },
            { "CorporateCashDisbursement"     , "CCD" },
            { "PointOfSale"                   , "POS" },
            { "PrearrangedPaymentAndDeposits" , "PPD" },
            { "TelephoneInitiatedEntry"       , "TEL" },
            { "WebInitiatedEntry"             , "WEB" },
            { "BankAccount"                   , "POP" } };

        public static Dictionary<string, string>
        methodCodes = new Dictionary<string, string> {
            { "CREDIT", "CC" },
            { "DEBIT", "DB" },
            { "STORED_VALUE", "SV" },
            { "ELECTRONIC_BENEFITS_TRANSFER", "EBT" },
            { "ECHECK", "CHECK21" },
            { "PINLESS_DEBIT", "PD" } };

        public enum TransactionService
        {
            Regular = 0, VaultUsingCustomerID = 1,
            VaultAddCustomerAndAccount = 2,
            VaultProcessTransactionAddCustomerAndAccount = 3
        };

        public void Save(HttpCookieCollection responseCookies, HttpCookieCollection requestCookies)
        {  
            responseCookies["InputValues"]["cardNumber"] = cardNumber;
            responseCookies["InputValues"]["trackData"] = trackData;
            responseCookies["InputValues"]["cardCode"] = cardCode;
            responseCookies["InputValues"]["expDate"] = expDate;
            responseCookies["InputValues"]["customerID"] = customerID;
            responseCookies["InputValues"]["orderID"] = orderID;
            responseCookies["InputValues"]["batchID"] = batchID;
            responseCookies["InputValues"]["transactionID"] = transactionID;
            responseCookies["InputValues"]["authCode"] = authCode;
            responseCookies["InputValues"]["paymentID"] = paymentID;
            responseCookies["InputValues"]["userName"] = userName;
            responseCookies["InputValues"]["groupID"] = groupID;
            responseCookies["InputValues"]["planID"] = planID;
            responseCookies["InputValues"]["amount"] = amount;
            if (requestCookies["InputValues"] != null)
            {
                string mySecureNetID = requestCookies["InputValues"]["mySecureNetID"];
                string mySecureNetKey = requestCookies["InputValues"]["mySecureNetKey"];
                string myUserID = requestCookies["InputValues"]["myUserID"];
                string myGroupID = requestCookies["InputValues"]["myGroupID"];
                if (myUserID != null)
                {
                    responseCookies["InputValues"]["myUserID"] = myUserID;
                    responseCookies["InputValues"]["myGroupID"] = myGroupID;
                }
                if (mySecureNetID != null)
                {
                    responseCookies["InputValues"]["mySecureNetID"] = mySecureNetID;
                    responseCookies["InputValues"]["mySecureNetKey"] = mySecureNetKey;
                }
            }
        }

        public string cardNumber { get; set; }
        public string trackData { get; set; }
        public string cardCode { get; set; }
        public string expDate { get; set; }
        public string customerID { get; set; }
        public string orderID { get; set; }
        public string batchID { get; set; }
        public string transactionID { get; set; }
        public string authCode { get; set; }
        public string paymentID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string groupID { get; set; }
        public string planID { get; set; }
        public string amount { get; set; }

        public string cpUserName { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string repeatNewPassword { get; set; }

    }
}
