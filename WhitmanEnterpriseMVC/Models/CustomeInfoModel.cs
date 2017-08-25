using System.Collections.Generic;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Models
{
    public class CustomeInfoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ListingId { get; set; }
        public bool DeleteImmediately { get; set; }
        public bool SessionTimeOut { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public string Country { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
