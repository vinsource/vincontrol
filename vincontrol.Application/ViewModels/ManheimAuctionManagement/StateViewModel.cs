using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    public class StateViewModel
    {
        public string Name { get; set; }
        public IList<AuctionViewModel> Auctions { get; set; }
    }
}
