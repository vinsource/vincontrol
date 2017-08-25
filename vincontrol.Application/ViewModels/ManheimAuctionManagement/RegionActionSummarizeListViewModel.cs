using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    public class RegionActionSummarizeListViewModel
    {
        public IList<RegionActionSummarizeViewModel> RegionAuctionSummarizeList { get; set; }
        public string JsonRegionAuctionSummarizeList { get; set; }
        public RegionActionSummarizeListViewModel() { }

        public RegionActionSummarizeListViewModel(IList<RegionActionSummarizeViewModel> list)
        {
            RegionAuctionSummarizeList = list;
            var serializer = new DataContractJsonSerializer(typeof(List<RegionActionSummarizeViewModel>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, list);
                JsonRegionAuctionSummarizeList = Encoding.Default.GetString(stream.ToArray());
            }
        }
    }
}
