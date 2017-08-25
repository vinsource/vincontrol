using System;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    public class AnalysisItemViewModel
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Mmr { get; set; }

        public DateTime? DateStampSold { get; set; }
    }
}