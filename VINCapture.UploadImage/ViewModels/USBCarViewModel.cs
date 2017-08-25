using System.Collections.Generic;
using System.ComponentModel;
using VINCapture.UploadImage.Commands;
using VINCapture.UploadImage.Models;

namespace VINCapture.UploadImage.ViewModels
{
    public class USBCarViewModel : INotifyPropertyChanged
    {
        public  USBCarViewModel()
        {
            Upload = new UploadCommand(this);
        }

        public string Name { get; set; }

        private int _uploadedFileNumber;
        public int UploadedFileNumber
        {
            get { return _uploadedFileNumber; }
            set
            {
                if (_uploadedFileNumber != value)
                {
                    _uploadedFileNumber = value;
                    Progress = (double)value/(double)Quantity;
                }
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged("Progress");
                }
            }
        }
      
        public bool IsOverlay { get; set; }
        public string Vin { get; set; }
        public int ListingId { get; set; }
        public string PhysicalFolderPath { get; set; }
        public string DestinationBackupFolder { get; set; }
        public string Stock { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Color { get; set; }

        public UploadCommand Upload { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class USBList
    {
        public IEnumerable<USBCarViewModel> VehicleList { get; set; }

        public DealerUser Dealer { get; set; }


    }
}