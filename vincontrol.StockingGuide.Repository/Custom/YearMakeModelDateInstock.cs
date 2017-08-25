using System;

namespace vincontrol.StockingGuide.Repository.Custom
{
    public class YearMakeModelDateInStock
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime DateInStock { get; set; }
    }
}
