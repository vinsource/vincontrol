namespace Vincontrol.Vinsell.DesktopVersion.Models
{
    public class MarketModel : BaseModel
    {
        private decimal _averagePrice;
        public decimal AveragePrice
        {
            get { return _averagePrice; }
            set
            {
                if (_averagePrice != value)
                {
                    _averagePrice = value;
                    OnPropertyChanged("AveragePrice");
                }
            }
        }

        private decimal _minimumPrice;
        public decimal MinimumPrice
        {
            get { return _minimumPrice; }
            set
            {
                if (_minimumPrice != value)
                {
                    _minimumPrice = value;
                    OnPropertyChanged("MinimumPrice");
                }
            }
        }


        private decimal _maximumPrice;
        public decimal MaximumPrice
        {
            get { return _maximumPrice; }
            set
            {
                if (_maximumPrice != value)
                {
                    _maximumPrice = value;
                    OnPropertyChanged("MaximumPrice");
                }
            }
        }
    }
}