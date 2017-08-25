using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class CarFaxDetailViewModel
    {
        public CarFaxViewModel CarFax { get; set; }
        public string CarFaxDealerId { get; set; }
        public string Vin { get; set; }
    }
}
