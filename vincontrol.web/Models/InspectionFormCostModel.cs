using System.Collections.Generic;
using System.Web.Mvc;

namespace Vincontrol.Web.Models
{
    public class InspectionFormCostModel
    {
        public int AppraisalId { get; set; }

        public decimal Mechanical { get; set; }
        public decimal FrontBumper { get; set; }
        public decimal RearBumper { get; set; }
        public decimal Glass { get; set; }
        public decimal Tires { get; set; }
        public decimal FrontEnd { get; set; }
        public decimal RearEnd { get; set; }
        public decimal DriverSide { get; set; }
        public decimal PassengerSide { get; set; }
        public decimal Interior { get; set; }
        public decimal LightsBulbs { get; set; }
        public decimal Other { get; set; }
        public decimal LMA { get; set; }
    }
}
