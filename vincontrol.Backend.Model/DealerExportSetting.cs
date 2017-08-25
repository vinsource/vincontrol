using System;
using System.ComponentModel;

namespace vincontrol.Backend.Model
{
    public class DealerExportSetting : ModelBase, INotifyPropertyChanged
    {
        public DealerExportSetting()
        {
            ValidatedProperties = new[] { "SelectedFileName" };

        }
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public bool Discontinued { get; set; }
        public FileNameFormat FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    SelectedFileName = _fileName.Value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        public string SelectedFileName
        {
            get { return _selectedFileName; }
            set
            {
                if (_selectedFileName != value)
                {
                    _selectedFileName = value;
                    OnPropertyChanged("SelectedFileName");
                }
            }
        }

        public bool IsBundle
        {
            get { return _isBundle; }
            set
            {
                if (_isBundle != value)
                {
                    _isBundle = value;
                    OnPropertyChanged("IsBundle");
                }
            }
        }

        public string FileUrl { get; set; }

        public DateTime? LastDepositedDate { get; set; }

        private FileNameFormat _fileName;
        private bool _isBundle;
        private string _selectedFileName;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override string GetValidationError(string property)
        {

            if (Array.IndexOf(ValidatedProperties, property) < 0)
                return null;
            string result;
            switch (property)
            {
                case "SelectedFileName":
                    return String.IsNullOrEmpty(SelectedFileName) ? "SelectedFileName should have value" : null;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}
