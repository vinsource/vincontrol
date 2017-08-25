using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.InventoryManagement
{
    public class VehicleLogTracking
    {
        public DateTime LogDate { get; set; }

        public string Description { get; set; }

        public VehicleLogTracking(VehicleLog log)
        {
            LogDate = log.DateStamp.GetValueOrDefault();
            Description = log.Description;
        }
    }

    public class SilentSalesmanViewModel
    {
        public SilentSalesmanViewModel(){}

        public SilentSalesmanViewModel(SilentSalesman obj)
        {
            Title = obj.Title;
            Engine = obj.Engine;
            AdditionalOptions = obj.AdditionalOptions;
            OtherOptions = obj.OtherOptions;
        }

        public string Title { get; set; }
        public string Engine { get; set; }
        public string AdditionalOptions { get; set; }
        public string OtherOptions { get; set; }
    }
}
