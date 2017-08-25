namespace Vincontrol.Vinsell.DesktopVersion.Models
{
    public class CarfaxModel:BaseModel
    {
        private string _numberofOwners;
        public string NumberofOwners
        {
            get { return _numberofOwners; }
            set
            {
                if (_numberofOwners != value)
                {
                    _numberofOwners = value;
                    OnPropertyChanged("NumberofOwners");
                }
            }
        }

        private string _serviceRecords;
        public string ServiceRecords
        {
            get { return _serviceRecords; }
            set
            {
                if (_serviceRecords != value)
                {
                    _serviceRecords = value;
                    OnPropertyChanged("ServiceRecords");
                }
            }
        }
    }
}