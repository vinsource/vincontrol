namespace vincontrol.StockingGuide.Repository.Custom
{
    public class MakeModelQuantityPrice
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Count { get; set; }
        public double? Age { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
        public int? AveragePrice { get; set; }
    }
}
