namespace Vincontrol.Vinsell.DesktopVersion.Models
{
    public class KBBModel :BaseModel
    {
        private string _wholesale;
        public string Wholesale
        {
            get { return _wholesale; }
            set
            {
                if (_wholesale != value)
                {
                    _wholesale = value;
                    OnPropertyChanged("Wholesale");
                }
            }
        }

        private string _mileageAdjustment;
        public string MileageAdjustment
        {
            get { return _mileageAdjustment; }
            set
            {
                if (_mileageAdjustment != value)
                {
                    _mileageAdjustment = value;
                    OnPropertyChanged("MileageAdjustment");
                }
            }
        }

        private string _baseWholesale;
        public string BaseWholesale
        {
            get { return _baseWholesale; }
            set
            {
                if (_baseWholesale != value)
                {
                    _baseWholesale = value;
                    OnPropertyChanged("BaseWholesale");
                }
            }
        }
    }
}