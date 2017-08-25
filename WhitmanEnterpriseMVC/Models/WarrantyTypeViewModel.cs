using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class WarrantyTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnglishVersionUrl { get; set; }
        public string SpanishVersionUrl { get; set; }
        public int DealerId { get; set; }
        public int CategoryId { get; set; }
    }
}