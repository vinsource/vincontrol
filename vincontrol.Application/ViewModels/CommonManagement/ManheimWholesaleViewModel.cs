using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class ManheimWholesaleViewModel
    {
        public int VehicleId { get; set; }

        public decimal LowestPrice { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal HighestPrice { get; set; }

        public int Year { get; set; }

        public int MakeServiceId { get; set; }

        public int ModelServiceId { get; set; }

        public string TrimName { get; set; }

        public int TrimServiceId { get; set; }

        public bool IsSelected { get; set; }
    }

    public class ManheimReport
    {
        public ManheimReport() 
        {
            ManheimTransactions = new List<ManheimTransactionViewModel>();
        }

        public string LowestPrice { get; set; }

        public string AveragePrice { get; set; }

        public string HighestPrice { get; set; }

        public string Region { get; set; }

        public int NumberOfTransactions { get; set; }

        public string AverageOdometer { get; set; }

        public bool IsAuction { get; set; }

        public List<ManheimTransactionViewModel> ManheimTransactions { get; set; }
    }

    public class ManheimTransactionContract
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public List<ManheimTransactionViewModel> Data { get; set; }
    }

    public class ManheimTransactionViewModel
    {
        public string Type { get; set; }

        public string Odometer { get; set; }

        public string Price { get; set; }

        public string SaleDate { get; set; }

        public string Auction { get; set; }

        public string Region { get; set; }

        public string Engine { get; set; }

        public string TR { get; set; }

        public string Cond { get; set; }

        public string Color { get; set; }

        public string Sample { get; set; }

        public string Vin { get; set; }
    }

    public class ManheimTrim
    {
        public int ServiceId { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }
    }

    public class SelectItem
    {
        public bool Selected { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }

        public SelectItem() { }

        public SelectItem(string text, string value, bool selected)
        {
            Text = text;
            Value = value;
            Selected = selected;
        }
    }

    public class ManheimYearMakeModel
    {
        public int Year { get; set; }
        
        public string Make { get; set; }
        
        public string Model { get; set; }
    }

    public class ReplaceType
    {
        public string Original = string.Empty;
        public string Replace = string.Empty;
        public bool IsExpression;
    }

  

    public class SimulcastContract
    {
        public string locale { get; set; }
        public string vehicleGroupGoto { get; set; } //a:CADE_s:76108_c:OPEN_l:1_v:1_q:1-43
        public string redirect { get; set; }
        public string redirectVg { get; set; }
        public string isManheimAVPluginInstalled { get; set; } //false
        public string manheim_mobile_application_flag { get; set; }
        public string modifyOrContinue { get; set; }
        public string saleEventKey { get; set; } //CADE_76108_01
        public string vehicleGroupKey { get; set; } //a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900
        public string dealerships { get; set; } //5131094,a:CADE_s:76108_c:OPEN_l:1_v:1_q:1-43,a:CADE_s:76108_c:OPEN_l:1_v:1_q:76-900,a:CADE_s:76108_c:REDL_l:1_v:1_q:44-75,a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900
        public string initalDealer { get; set; }
        public string email { get; set; } //sbrown@jlr-mv.com
        public string cellphoneNPA { get; set; } //714
        public string cellphoneNXX { get; set; } //348
        public string cellphoneStationCode { get; set; } //8351
        public string faxNPA { get; set; } //714
        public string faxNXX { get; set; } //242
        public string faxStationCode { get; set; } //1875
        public string paymentMethod { get; set; } //CHECK
        public string floorPlanProvider { get; set; }
        public string comments { get; set; }
        public string postSaleInspection { get; set; } //7
        public string title { get; set; } //LOT
        public string transportation { get; set; } //DEALER
        public string transportContactName { get; set; } ///al american transport
        public string transportNPA { get; set; } //714
        public string transportNXX { get; set; } //400
        public string transportStationCode { get; set; } //7057
        public string confirmPreferences { get; set; } //on
    }
}
