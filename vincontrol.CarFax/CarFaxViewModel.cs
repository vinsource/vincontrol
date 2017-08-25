using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.CarFax
{
    public class CarFaxViewModel
    {
        public string Vin { get; set; }
        public string NumberofOwners { get; set; }
        public string ServiceRecords { get; set; }
        public int AccidentCounts { get; set; }
        public string AccidentCountsXml { get; set; }
        public bool MajorProblemIndicator { get; set; }
        public bool DamageIndicator { get; set; }
        public bool FrameDamageIndicator { get; set; }
        public bool BrandedTitleIndicator { get; set; }
        public bool BuyBackGuarantee { get; set; }
        public bool AccidentIndicator { get; set; }
        public string CarFaxXMLResponse { get; set; }
        public string Disclaimer { get; set; }
        public List<CarFaxWindowSticker> ReportList { get; set; }
        public bool Success { get; set; }
        public int ExistDatabase { get; set; }
        public bool PriorRental { get; set; }
    }

    public class CarFaxWindowSticker
    {
        public string Text { get; set; }
        public string Image { get; set; }
    }
}
