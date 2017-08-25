using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class InspectionAppraisalViewModel
    {
        public AppraisalInfo AppraisalInfo { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public IEnumerable<WalkaroundInfo> WalkaroundInfo { get; set; }
        public IEnumerable<AppraisalAnswer> AppraisalAnswer { get; set; }
    }

    public class AppraisalInfo
    {
        public int AppraisalId { get; set; }
        public string VinNumber { get; set; }
        public string StockNumber { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string Transmission { get; set; }
        public string Odometer { get; set; }
        public string Cylinders { get; set; }
        public string Liters { get; set; }
        public string Doors { get; set; }
        public string Fuel { get; set; }
        public string MSRP { get; set; }
        public string DriveType { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Photo { get; set; }
        public List<string> Options { get; set; }
        public List<string> StandardOptions { get; set; }
        public List<string> Packages { get; set; }
        public string AppraisalDate { get; set; }
        public string AppraisalBy { get; set; }
        public string AppraisalTime { get; set; }
        public string EngineType { get; set; }
    }

    public class CustomerInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Street { get; set; }
        public byte[] Signature { get; set; }
    }

    public class WalkaroundInfo
    {
        public int Order { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Note { get; set; }       
    }

    public class AppraisalAnswer
    {
        public int AppraisalId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Comment { get; set; }
        public int QuestionType { get; set; }
        public int Order { get; set; }
    }
}
