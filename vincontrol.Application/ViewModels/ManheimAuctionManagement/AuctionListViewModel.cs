using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    [DataContract]
    public class AuctionListViewModel
    {
        public int SelectedLane { get; set; }
        public int SelectedRun { get; set; }
        public string SelectedCategory { get; set; }
        public string State { get; set; }
        public string Auction { get; set; }
        public string AuctionCode { get; set; }
        
        public IList<VehicleViewModel> AuctionList { get; set; }
        public IList<ExtendedSelectListItem> Year { get; set; }
        public IList<ExtendedSelectListItem> Make { get; set; }
        public IList<ExtendedSelectListItem> Model { get; set; }
        public IList<ExtendedSelectListItem> Trim { get; set; }
        public IList<ExtendedSelectListItem> Lane { get; set; }
        public IList<ExtendedSelectListItem> Run { get; set; }
        public IList<ExtendedSelectListItem> Seller { get; set; }
        public IList<ExtendedSelectListItem> Category { get; set; }
        public IList<DateTime> Date { get; set; }
        public string JsonAuctionList { get; set; }
        public AuctionListViewModel()
        {
            Year = new List<ExtendedSelectListItem>();
            Make = new List<ExtendedSelectListItem>();
            Model = new List<ExtendedSelectListItem>();
            Trim = new List<ExtendedSelectListItem>();
            Lane = new List<ExtendedSelectListItem>();
            Run = new List<ExtendedSelectListItem>();
            Seller = new List<ExtendedSelectListItem>();
            Category = new List<ExtendedSelectListItem>();
        }

        public AuctionListViewModel(IList<VehicleViewModel> list)
        {
            AuctionList = list;
            var result = (from i in list
                         group i by i.Make into g
                         select new MakeItem { Make = g.Key, Count = g.Count() }).ToList();

            var serializer = new DataContractJsonSerializer(typeof(List<MakeItem>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, result);
                JsonAuctionList = Encoding.Default.GetString(stream.ToArray());
            }

            //Initialize dropdownlists
            Year = list.OrderByDescending(i => i.Year).Select(i => i.Year).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();
            Make = new List<ExtendedSelectListItem>();
            Model = new List<ExtendedSelectListItem>();
            Trim = new List<ExtendedSelectListItem>();
            Lane = list.OrderBy(i => i.Lane).Select(i => i.Lane).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = (i == SelectedLane) }).ToList();
            Run = list.OrderBy(i => i.Run).Select(i => i.Run).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = (i == SelectedRun) }).ToList();
            Seller = list.OrderBy(i => i.Seller).Select(i => i.Seller).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();
            
            Category = list.Any(i => i.Category != null) ? list.Where(i => i.Category != null).OrderBy(i => i.Category).Select(i => i.Category).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }


        public AuctionListViewModel(IList<ManheimYearMakeModel> list)
        {
            Year = list.OrderByDescending(i => i.Year).Select(i => i.Year).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();
            //Make = list.OrderBy(i => i.Make).Select(i => i.Make).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();
        }
    }

    [DataContract]
    public class YearItem
    {
        [DataMember(Name = "year")]
        public int Year { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
    }

    [DataContract]
    public class MakeItem
    {
        [DataMember(Name = "make")]
        public string Make { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
    }

    [DataContract]
    public class ModelItem
    {
        [DataMember(Name = "model")]
        public string Model { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
    }
}
