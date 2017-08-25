using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Models
{
    public class ChartSelectionViewModel
    {
        public int Id { get; set; }
        public bool IsCarsCom { get; set; }
        public string Options { get; set; }
        public string Trims { get; set; }
        public bool IsCertified { get; set; }
        public bool IsAll { get; set; }
        public bool IsFranchise { get; set; }
        public bool IsIndependant { get; set; }
        public string Screen { get; set; }
        //public string Mileage { get; set; }
        //public string SalePrice { get; set; }
        public string PdfContent { get; set; }
        public ChartSelection CarsCom { get; set; }
        public string FilterOptions { get; set; }
    }

    public class ChartSelection
    {
        public bool IsCarsCom { get; set; }
        public string Options { get; set; }
        public string Trims { get; set; }
        public bool IsCertified { get; set; }
        public bool IsAll { get; set; }
        public bool IsFranchise { get; set; }
        public bool IsIndependant { get; set; }
        public string Screen { get; set; }
    }
}