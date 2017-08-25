using System;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Model;
using vincontrol.Backend.Windows.DataFeed.Export;

namespace vincontrol.Backend.ViewModels.Export
{
    public class ExportProfileViewModel : ModelBase
    {
        private RelayCommand _openExportProfileSettingCommand;
        public RelayCommand OpenExportProfileSettingCommand
        {
            get { return _openExportProfileSettingCommand ?? (_openExportProfileSettingCommand = new RelayCommand(OpenExportProfileSetting,null)); }
        }

        private RelayCommand _openExportDealershipCommand;
        private bool _discontinued;
        private string _companyName;
        private string _profileName;
        private DateTime? _lastDepositedDate;
        private string _fileURL;
        public bool IsBundle { get; set; }

        public RelayCommand OpenExportDealershipCommand
        {
            get { return _openExportDealershipCommand ?? (_openExportDealershipCommand = new RelayCommand(OpenExportDealership,null)); }
        }

        void OpenExportProfileSetting(object parameter)
        {
            var chldWindow = new ExportProfileTemplateMappingWindow(this, null);
            chldWindow.ShowDialog();
        }

        void OpenExportDealership(object parameter)
        {
            var chldWindow = new ViewExportDealerOfProfileWindow(this);
            chldWindow.ShowDialog();
        }


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

        public int Id { get; set; }

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

        public string FileURL
        {
            get { return _fileURL; }
            set
            {
                if (_fileURL != value)
                {
                    _fileURL = value;
                    OnPropertyChanged("FileURL");
                }
            }
        }

        #region Overrides of ViewModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
