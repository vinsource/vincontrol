using System.Collections.Generic;
using vincontrol.DomainObject;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.Models
{
    public class CustomeInfoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int ListingId { get; set; }
        public string Email { get; set; }
        public int AppraisalId { get; set; }
        public int TradeInCustomerId { get; set; }
        public bool DeleteImmediately { get; set; }
        public bool SessionTimeOut { get; set; }
        public IEnumerable<ExtendedSelectListItem> States { get; set; }
        public string Country { get; set; }
        public IEnumerable<ExtendedSelectListItem> Countries { get; set; }
    }
}
