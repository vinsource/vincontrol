using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Models
{
    public class ManheimWholesaleViewModel
    {
        public int ListingId { get; set; }
                
        public string LowestPrice { get; set; }
                
        public string AveragePrice { get; set; }
                
        public string HighestPrice { get; set; }
                
        public int Year { get; set; }
                
        public int MakeServiceId { get; set; }
                
        public int ModelServiceId { get; set; }
                
        public string TrimName { get; set; }
                
        public int TrimServiceId { get; set; }
    }

    public class ManheimReport
    {
        public string LowestPrice { get; set; }

        public string AveragePrice { get; set; }

        public string HighestPrice { get; set; }

        public List<ManheimTransactionViewModel> ManheimTransactions { get; set; }
    }

    public class ManheimTransactionContract
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public List<ManheimTransactionViewModel> Data { get; set; }
    }

    public class ManheimTransactionViewModel
    {
        public string Type { get; set; }

        public string Odometer { get; set; }

        public string Price { get; set; }

        public string SaleDate { get; set; }

        public string Auction { get; set; }

        public string Engine { get; set; }

        public string TR { get; set; }

        public string Cond { get; set; }

        public string Color { get; set; }

        public string Sample { get; set; }
    }

    public class ManheimTrim
    {
        public int ServiceId { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }
    }

    public class SelectItem
    {
        public bool Selected { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }

        public SelectItem() { }

        public SelectItem(string text, string value, bool selected)
        {
            Text = text;
            Value = value;
            Selected = selected;
        }
    }
}