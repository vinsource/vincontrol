using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Models
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
        public string Model { get; set; }
        public string Condition { get; set; }
        public string TradeInStatus { get; set; }
        public int ID { get; set; }
        public decimal TradeInFairValue { get; set; }
        public string EmailContent { get; set; }
    }
}
