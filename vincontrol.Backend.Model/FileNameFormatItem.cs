using System;
using System.ComponentModel;



namespace vincontrol.Backend.Model
{
    public class FileNameFormat : ModelBase, INotifyPropertyChanged
    {
        private bool _isSelected;
        private string _value;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public string Text { get; set; }
      
        public string Example { get; set; }
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
                case "Value":
                    return String.IsNullOrEmpty(Value) ? "Value should have value" : null;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}
