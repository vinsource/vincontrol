using System;

namespace vincontrol.StockingGuide.Repository.Custom
{
    public class MakeModelDateInStock
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime DateInStock { get; set; }
    }
}
