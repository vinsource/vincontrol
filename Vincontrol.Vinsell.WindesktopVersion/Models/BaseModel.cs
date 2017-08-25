using System.ComponentModel;
using Vincontrol.Vinsell.WindesktopVersion.Annotations;

namespace Vincontrol.Vinsell.WindesktopVersion.Models
{
    public class BaseModel : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isFinishedLoading;
        public bool IsFinishedLoading
        {
            get { return _isFinishedLoading; }
            set
            {
                if (_isFinishedLoading != value)
                {
                    _isFinishedLoading = value;
                    OnPropertyChanged("IsFinishedLoading");
                }
            }
        }
    }
}