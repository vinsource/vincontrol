namespace vincontrol.Models.KarPower
{
    public class SmallKarPowerViewModel
    {
        public int SelectedTrimId { get; set; }
        public string SelectedTrimName { get; set; }
        public decimal BaseWholesale { get; set; }
        public decimal MileageAdjustment { get; set; }
        public decimal Wholesale { get; set; }
        public bool IsSelected { get; set; }
    }
}
