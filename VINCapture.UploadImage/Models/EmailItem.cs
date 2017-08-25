using System.ComponentModel;

namespace VINCapture.UploadImage.Models
{
    public class EmailItem : INotifyPropertyChanged
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}