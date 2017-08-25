namespace vincontrol.StockingGuide.Entity.Custom
{
    public class StockingGuideInfo
    {
        public int History { get; set; }
        public int Stock { get; set; }
        public int Supply { get { return (Stock / History) * 30; } }
        public int TurnOver { get { return (History / Stock) * 12; } }
        public decimal YearlyGrossProfit { get; set; }
    }
}
