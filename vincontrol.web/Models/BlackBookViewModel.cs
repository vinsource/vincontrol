using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class BlackBookViewModel
    {
        public string Vin { get; set; }

        public List<BlackBookTrimDetail> TrimList { get; set; }

        public List<BlackBookTrimReport> TrimReportList { get; set; }

        public bool SessionTimeOut { get; set; }


        public int ExistDatabase { get; set; }

        public bool Success { get; set; }

        public long Mileage { get; set; }
    }



    public class BlackBookTrimDetail
    {
        public string TrimName { get; set; }

        public string MinimumPrice { get; set; }

        public string AveragePrice { get; set; }

        public string MaximumPrice { get; set; }
    }

    public class BlackBookTrimReport
    {
        public string TrimName { get; set; }

        public string BaseWholeSaleExtraClean { get; set; }

        public string BaseWholeSaleClean { get; set; }

        public string BaseWholeSaleAvg { get; set; }

        public string BaseWholeSaleRough { get; set; }

        public string WholeSaleExtraClean { get; set; }

        public string WholeSaleClean { get; set; }

        public string WholeSaleAvg { get; set; }

        public string WholeSaleRough { get; set; }

        public string MileageAdjWholeSaleExtraClean { get; set; }

        public string MileageAdjWholeSaleClean { get; set; }

        public string MileageAdjWholeSaleAvg { get; set; }

        public string MileageAdjWholeSaleRough { get; set; }

        public string ManualOrAutomaticAdjWholeSaleExtraClean { get; set; }

        public string ManualOrAutomaticAdjWholeSaleClean { get; set; }

        public string ManualOrAutomaticAdjWholeSaleAvg { get; set; }

        public string ManualOrAutomaticAdjWholeSaleRough { get; set; }

        public string BaseRetailExtraClean { get; set; }

        public string BaseRetailClean { get; set; }

        public string BaseRetailAvg { get; set; }

        public string BaseRetailRough { get; set; }

        public string RetailExtraClean { get; set; }

        public string RetailaClean { get; set; }

        public string RetailAvg { get; set; }

        public string RetailRough { get; set; }

        public string MileageAdjRetailExtraClean { get; set; }

        public string MileageAdjRetailClean { get; set; }

        public string MileageAdjRetailAvg { get; set; }

        public string MileageAdjRetailRough { get; set; }

        public string ManualOrAutomaticAdjRetailExtraClean { get; set; }

        public string ManualOrAutomaticAdjRetailClean { get; set; }

        public string ManualOrAutomaticAdjRetailAvg { get; set; }

        public string ManualOrAutomaticAdjRetailRough { get; set; }

        public decimal TradeInClean { get; set; }

        public decimal TradeInAvg { get; set; }

        public decimal TradeInRough { get; set; }

        public string BaseTradeInClean { get; set; }

        public string BaseTradeInAvg { get; set; }

        public string BaseTradeInRough { get; set; }

        public string MileageAdjTradeInClean { get; set; }

        public string MileageAdjTradeInAvg { get; set; }

        public string MileageAdjTradeInRough { get; set; }

        public string ManualOrAutomaticAdjTradeInClean { get; set; }

        public string ManualOrAutomaticAdjTradeInAvg { get; set; }

        public string ManualOrAutomaticAdjTradeInRough { get; set; }

      

      

    


        
    }
}
