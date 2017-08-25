using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhitmanEnterpriseMVC.KBBServiceEndPoint;

namespace WhitmanEnterpriseMVC.ReportModel
{

    public class KellyBlueBook
    {
        public string Vin { get; set; }

        public int ModelYear { get; set; }

        public string StockNumber { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string Litters { get; set; }

        public string Tranmission { get; set; }

        public string WheelDrive { get; set; }
    }



}
