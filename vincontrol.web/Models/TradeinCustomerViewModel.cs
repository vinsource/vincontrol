using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vincontrol.Web.Models
{
    public class TradeinCustomerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MileageAdjustment { get; set; }
        public string Date { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string ShortMake { get; set; }
        public string Model { get; set; }
        public string Condition { get; set; }
        public string TradeInStatus { get; set; }
        public int ID { get; set; }
        public decimal TradeInFairValue { get; set; }
        public string EmailContent { get; set; }
        public int Age { get; set; }

        public string URLDetail { get; set; }

        public string SortVin { get; set; }

        public string StrTrim { get; set; }

        public string StrExteriorColor { get; set; }

        public string StrCarFaxOwner { get; set; }

        public string StrClientName { get; set; }

        public string StrMileage { get; set; }

        public string StrACV { get; set; }

        public string URLImage { get; set; }

        public string AppraisalModelSort { get; set; }

        public string ClientContact { get; set; }
    }
}
