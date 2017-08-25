using System;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Model;
using vincontrol.Backend.Windows;
using vincontrol.Backend.Windows.DataFeed;
using vincontrol.Backend.Windows.DataFeed.Import;

namespace vincontrol.Backend.ViewModels.Import
{
    public class ProfileViewModel : ModelBase
    {
        private RelayCommand _openProfileSettingCommand;
        public RelayCommand OpenProfileSettingCommand
        {
            get { return _openProfileSettingCommand ?? (_openProfileSettingCommand = new RelayCommand(OpenProfileSetting, null)); }
        }

        private RelayCommand _openDealershipCommand;
        public RelayCommand OpenDealershipCommand
        {
            get { return _openDealershipCommand ?? (_openDealershipCommand = new RelayCommand(OpenDealership, null)); }
        }

        void OpenProfileSetting(object parameter)
        {
            var chldWindow = new IncomingProfileTemplateMappingWindow(this);
            chldWindow.ShowDialog();
        }

        void OpenDealership(object parameter)
        {
            var chldWindow = new ViewDealerOfProfileWindow(this);
            chldWindow.ShowDialog();
        }

        public int Id { get; set; }

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                    OnPropertyChanged("CompanyName");
                }
            }
        }

        public string ProfileName
        {
            get { return _profileName; }
            set
            {
                if (_profileName != value)
                {
                    _profileName = value;
                    OnPropertyChanged("ProfileName");
                }
            }
        }

        public string ProfileStatus
        {
            get { return _profileStatus; }
            set
            {
                if (_profileStatus != value)
                {
                    _profileStatus = value;
                    OnPropertyChanged("ProfileStatus");
                }
            }
        }

        public bool UseSpecificMapping
        {
            get { return _useSpecificMapping; }
            set
            {
                if (_useSpecificMapping != value)
                {
                    _useSpecificMapping = value;
                    OnPropertyChanged("UseSpecificMapping");
                }
            }
        }

        public bool Discontinued
        {
            get { return _discontinued; }
            set
            {
                if (_discontinued != value)
                {
                    _discontinued = value;
                    OnPropertyChanged("Discontinued");
                }
            }
        }

        public string FileURL
        {
            get { return _fileURL; }
            set { _fileURL = value; }
        }

        public DateTime? LastDepositedDate
        {
            get { return _lastDepositedDate; }
            set
            {
                if (_lastDepositedDate != value)
                {
                    _lastDepositedDate = value;
                    OnPropertyChanged("LastDepositedDate");
                }
            }
        }

        private string _companyName;
        private string _profileName;
        private bool _discontinued;
        private string _profileStatus;
        private string _fileURL;
        private DateTime? _lastDepositedDate;
        private bool _useSpecificMapping;

        #region Overrides of ViewModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
