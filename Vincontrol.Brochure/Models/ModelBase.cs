using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Vincontrol.Brochure.Annotations;

namespace Vincontrol.Brochure.Models
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected string[] ValidatedProperties { get; set; }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

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
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
  
}
