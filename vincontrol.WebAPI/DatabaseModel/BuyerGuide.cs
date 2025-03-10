//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vincontrol.WebAPI.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class BuyerGuide
    {
        public int BuyerGuideId { get; set; }
        public Nullable<int> Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string Stock { get; set; }
        public int WarrantyType { get; set; }
        public Nullable<bool> IsAsWarranty { get; set; }
        public Nullable<bool> IsWarranty { get; set; }
        public Nullable<bool> IsFullWarranty { get; set; }
        public Nullable<bool> IsLimitedWarranty { get; set; }
        public Nullable<bool> IsServiceContract { get; set; }
        public string SystemCovered { get; set; }
        public string Durations { get; set; }
        public Nullable<double> PercentageOfLabor { get; set; }
        public Nullable<double> PercentageOfPart { get; set; }
        public string PriorRental { get; set; }
        public int DealerId { get; set; }
        public Nullable<bool> IsMixed { get; set; }
        public string SystemCoveredAndDurations { get; set; }
        public Nullable<int> LanguageVersion { get; set; }
    }
}
