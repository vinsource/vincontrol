using System.ComponentModel;
using System.Linq;

namespace vincontrol.DataFeed.Model
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected static string[] ValidatedProperties;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool PropertiesValid
        {
            get
            {
                return ValidatedProperties.All(property => GetValidationError(property) == null);
            }
        }

        protected abstract string GetValidationError(string property);

        public string this[string columnName]
        {
            get
            {
                return GetValidationError(columnName);
            }
        }

        public string Error { get; private set; }
    }
}
