using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    [DataContract]
    public class AuctionSummarizeViewModel
    {
        public AuctionSummarizeViewModel(auctions_summarize manheimRegionsAuctionsSummarize)
        {
            Code = manheimRegionsAuctionsSummarize.RegionCode;
            Name = manheimRegionsAuctionsSummarize.RegionName;
            NumberofVehicles = manheimRegionsAuctionsSummarize.NumberofVehicles;
        }

        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "numberofvehicles")]
        public int NumberofVehicles { get; set; }
    }
}
