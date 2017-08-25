using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    public class AuctionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string State { get; set; }

        public AuctionViewModel() { }

        public AuctionViewModel(manheim_auctions obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            Code = obj.Code;
            State = obj.State;
        }
    }
}
