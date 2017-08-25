using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VINControl.ExcelHelper
{
    public enum ExcelMessage
    {
        [Description("Success")]
        Success,
        [Description("Error")]
        Error,
        [Description("FileNotExist")]
        FileNotExist
    }
    
    public class DataFeedExcelObj
    {
        public string Stock { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ModelNo { get; set; }
        public string ModelType { get; set; }
        public string BodyStyle { get; set; }
        public string Color { get; set; }
        public string Engine { get; set; }
        public decimal Retail { get; set; }
        public decimal Invoice { get; set; }
        public int Lot { get; set; }
        public int Co { get; set; }
        public int Age { get; set; }
        public string Status { get; set; }
        public string VIN { get; set; }
        public string DealNo { get; set; }
        public decimal Mileage { get; set; }
    }

    public class DataFeedResults
    {
        public DataFeedResults() { Data = new List<DataFeedExcelObj>(); }

        public string Message { get; set; }
        public List<DataFeedExcelObj> Data { get; set; }
    }
}
