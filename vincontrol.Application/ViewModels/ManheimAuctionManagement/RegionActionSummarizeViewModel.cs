using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    [DataContract]
    public class RegionActionSummarizeViewModel
    {
        public RegionActionSummarizeViewModel(manheim_regions_auctions_summarize manheimRegionsAuctionsSummarize)
        {
            Name = manheimRegionsAuctionsSummarize.State;
            NumberOfRegions = manheimRegionsAuctionsSummarize.NumberOfRegions;
            Regions = manheimRegionsAuctionsSummarize.Auctions.Select(i => new AuctionSummarizeViewModel(i)).ToList();
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        //Means Number Of Auctions
        [DataMember(Name = "numberofregions")]
        public int NumberOfRegions { get; set; }
        [DataMember(Name = "regions")]
        public IEnumerable<AuctionSummarizeViewModel> Regions { get; set; }
    }
}
