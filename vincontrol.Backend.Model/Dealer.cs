using System;
using System.ComponentModel;

namespace vincontrol.Backend.Model
{
    public class Dealer : INotifyPropertyChanged
    {
        private string _name;
        public string DealerPassword { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

      

        public string Address { get; set; }
        public DateTime DateAdded { get; set; }
        public string Lattitude { get; set; }
        public string Longtitude { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public int DealerGroupID { get; set; }
        public string Email { get; set; }
        public int EmailFormat { get; set; }
        public bool OverideDealerKBBReport { get; set; }
        public int Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
