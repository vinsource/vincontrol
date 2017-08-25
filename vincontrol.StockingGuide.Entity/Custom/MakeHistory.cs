namespace vincontrol.StockingGuide.Entity.Custom
{
    public class MakeHistory
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int History { get; set; }
    }

    public class MakeHistoryWithLongitudeAndLattitude : MakeHistory
    {
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
    }
}
