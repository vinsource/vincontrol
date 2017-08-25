namespace Vincontrol.Vinsell.DesktopVersion.Models
{
    public class MainheimModel : BaseModel
    {
        private string _highestPrice;
        public string HighestPrice
        {
            get { return _highestPrice; }
            set
            {
                if (_highestPrice != value)
                {
                    _highestPrice = value;
                    OnPropertyChanged("HighestPrice");
                }
            }
        }

     
        private string _lowestPrice;
        public string LowestPrice
        {
            get { return _lowestPrice; }
            set
            {
                if (_lowestPrice != value)
                {
                    _lowestPrice = value;
                    OnPropertyChanged("LowestPrice");
                }
            }
        }

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
    }
}