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
    
    public partial class UsaZipCode
    {
        public int UsaZipCodeId { get; set; }
        public string ZIPType { get; set; }
        public string CityName { get; set; }
        public string CityType { get; set; }
        public string CountyName { get; set; }
        public string CountyFIPS { get; set; }
        public string StateName { get; set; }
        public string StateAbbr { get; set; }
        public string StateFIPS { get; set; }
        public string MSACode { get; set; }
        public string AreaCode { get; set; }
        public string TimeZone { get; set; }
        public string UTC { get; set; }
        public string DST { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
    }
}
