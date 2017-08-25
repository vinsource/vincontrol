using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DomainObject;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    public class AdvancedSearchViewModel
    {
        public AdvancedSearchViewModel()
        {
            YearsFrom = new List<ExtendedSelectListItem>();
            YearsTo = new List<ExtendedSelectListItem>();
            
            Makes = new List<ExtendedSelectListItem>();
            Models = new List<ExtendedSelectListItem>();
            Trims = new List<ExtendedSelectListItem>();

            Crs = new List<ExtendedSelectListItem>();
            for (int i = 5; i >= 1; i--)
            {
                Crs.Add(new ExtendedSelectListItem() { Text = String.Format("Above {0}.0", i), Value = i.ToString(), Selected = false });
            }

            Mmrs = new List<ExtendedSelectListItem>();
            for (int i = 1; i <= 7; i++)
            {
                Mmrs.Add(new ExtendedSelectListItem() { Text = String.Format("Under {0}", (i*10000).ToString("c0")), Value = (i*10000).ToString(), Selected = false });
            }

            Transmissions = new List<ExtendedSelectListItem>();
            Transmissions.Add(new ExtendedSelectListItem() { Text = "Manual", Value = "Manual", Selected = false });
            Transmissions.Add(new ExtendedSelectListItem() { Text = "Automatic", Value = "Automatic", Selected = false });

            BodyStyles = new List<ExtendedSelectListItem>();
            ExteriorColors = new List<ExtendedSelectListItem>();
            Regions = new List<ExtendedSelectListItem>();
            States = new List<ExtendedSelectListItem>();
            Auctions = new List<ExtendedSelectListItem>();
            Sellers = new List<ExtendedSelectListItem>();

            PagingRecords = 20;
        }

        public string Text { get; set; }
        public bool HasCarfax1Owner { get; set; }
        
        public int SelectedYearFrom { get; set; }
        public IList<ExtendedSelectListItem> YearsFrom { get; set; }
        
        public int SelectedYearTo { get; set; }
        public IList<ExtendedSelectListItem> YearsTo { get; set; }
        
        public string SelectedMake { get; set; }
        public IList<ExtendedSelectListItem> Makes { get; set; }
        
        public string SelectedModel { get; set; }
        public IList<ExtendedSelectListItem> Models { get; set; }

        public string SelectedTrim { get; set; }
        public IList<ExtendedSelectListItem> Trims { get; set; }

        public int MileageFrom { get; set; }
        public int MileageTo { get; set; }

        public int SelectedCr { get; set; }
        public IList<ExtendedSelectListItem> Crs { get; set; }

        public int SelectedMmr { get; set; }
        public IList<ExtendedSelectListItem> Mmrs { get; set; }

        public string SelectedBodyStyle { get; set; }
        public IList<ExtendedSelectListItem> BodyStyles { get; set; }

        public string SelectedTransmission { get; set; }
        public IList<ExtendedSelectListItem> Transmissions { get; set; }

        public string SelectedExteriorColor { get; set; }
        public IList<ExtendedSelectListItem> ExteriorColors { get; set; }

        public string SelectedRegion { get; set; }
        public IList<ExtendedSelectListItem> Regions { get; set; }

        public string SelectedState { get; set; }
        public IList<ExtendedSelectListItem> States { get; set; }

        public string SelectedAuction { get; set; }
        public IList<ExtendedSelectListItem> Auctions { get; set; }

        public string SelectedSeller { get; set; }
        public IList<ExtendedSelectListItem> Sellers { get; set; }

        public DateTime SaleDateFrom { get; set; }
        public DateTime SaleDateTo { get; set; }

        public int PagingRecords { get; set; }
    }
}
