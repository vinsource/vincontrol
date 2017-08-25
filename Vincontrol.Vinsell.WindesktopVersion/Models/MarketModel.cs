using Vincontrol.Vinsell.WindesktopVersion.Models;

namespace Vincontrol.Vinsell.WindesktopVersion.Models
{
    public class MarketModel : BaseModel
    {
        private string _averagePrice;
        public string AveragePrice
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

        private string _minimumPrice;
        public string MinimumPrice
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


        private string _maximumPrice;
        public string MaximumPrice
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