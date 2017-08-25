using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Model;
using vincontrol.DataFeed.Helper;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.ViewModels.Import
{
    public class IncomingProfileTemplateViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Manage Data

        private void CreateMappingAsync(object parameter)
        {
            DoPendingTask(CreateMapping, parameter);
        }

        private void CreateMapping(object parameter)
        {
            var requiredDBfields = new List<string> { "ModelYear", "Make", "Model", "Trim", "StockNumber", "Mileage", "ExteriorColor" };
            List<string> currentDBfileds = Mappings.Select(i => i.DBField).ToList();
            var found = requiredDBfields.All(currentDBfileds.Contains);

            if (!found && !UseSpecificMapping)
            {
                MessageBox.Show(
                    string.Format(
                        "Please map required fields: ModelYear,Make,Model,Trim,StockNumber,Mileage,ExteriorColor"));
            }
            else
            {
                var xmlHelper = new XMLHelper();
                var xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(this));
                string sampleDataXML = DataHelper.SerializeSampleData(this);
                var context = new vincontrolwarehouseEntities();
                var item = context.importdatafeedprofiles.FirstOrDefault(i => i.ProfileName.Equals(ProfileName));
                if (item == null)
                {

                    var feedProfile = new importdatafeedprofile
                    {
                        CompanyName = ProfileName,
                        Mapping = xmlContent,
                        SampleData = sampleDataXML,
                        //FeedUrl = _vm.FileURL,
                        Frequency = Frequency,
                        RunningTime = RunningTime,
                        Discontinued = Discontinue,
                        SchemaURL = SchemaURL,
                        ProfileName = ProfileName,
                        UseMappingInCode = UseSpecificMapping,
                        Email = Email
                    };

                    context.AddToimportdatafeedprofiles(feedProfile);
                    context.SaveChanges();
                    Tracking.Log(UserAction.Insert, App.CurrentUser.Id, DateTime.Now, feedProfile.Id, string.Format("Profile:{0}", feedProfile.ProfileName), ItemType.ImportProfile);

                    ProfileId = feedProfile.Id;
                    Application.Current.Dispatcher.BeginInvoke(
                        new Action(
                            () =>
                            {
                                _listvm.Profiles.Add(new ProfileViewModel
                                    {
                                        CompanyName = CompanyName,
                                        ProfileName = ProfileName,
                                        Id = ProfileId
                                    })
                                ;
                                _view.Close();
                            }));

                }
                else
                {
                    MessageBox.Show(
                        string.Format("Profile name {0} already existed. Please fill in other name for profile.",
                                      ProfileName));
                }
            }


        }

        public void SaveMappingAsync(object parameter)
        {
            DoPendingTask(SaveMapping, parameter);
        }

        public void SaveMapping(object parameter)
        {
            var requiredDBfields = new List<string> { "ModelYear", "Make", "Model", "Trim", "StockNumber", "Mileage", "ExteriorColor" };
            List<string> currentDBfileds = Mappings.Select(i => i.DBField).ToList();
            var found = requiredDBfields.All(currentDBfileds.Contains);

            if (!found && !UseSpecificMapping)
            {
                MessageBox.Show(
                    string.Format(
                        "Please map required fields: ModelYear,Make,Model,Trim,StockNumber,Mileage,ExteriorColor"));
            }
            else
            {
                var xmlHelper = new XMLHelper();
                var xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(this));
                string sampleDataXML = DataHelper.SerializeSampleData(this);
                var context = new vincontrolwarehouseEntities();
                var firstPofile = context.importdatafeedprofiles.FirstOrDefault(i => i.Id == ProfileId);
                if (firstPofile != null)
                {
                    firstPofile.CompanyName = CompanyName;
                    firstPofile.Mapping = xmlContent;
                    firstPofile.SampleData = sampleDataXML;

                    //firstPofile.FeedUrl = _vm.FileURL;
                    firstPofile.ProfileName = ProfileName;
                    firstPofile.Frequency = Frequency;
                    firstPofile.RunningTime = RunningTime;
                    firstPofile.Discontinued = Discontinue;
                    firstPofile.SchemaURL = SchemaURL;
                    firstPofile.UseMappingInCode = UseSpecificMapping;
                    firstPofile.Email = Email;
                    context.SaveChanges();
                    Tracking.Log(UserAction.Update, App.CurrentUser.Id, DateTime.Now, firstPofile.Id, string.Format("Profile:{0}", firstPofile.ProfileName), ItemType.ImportProfile);

                    DataHelper.UpdateImportTask(Discontinue, ProfileName, RunningTime, ProfileId, Frequency);
                }
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        UpdateCurrentVm();
                        _view.Close();
                    }));
            }
        }

        private void InitCommonFieldsAsync()
        {
            DoPendingTask(InitCommonFields);
        }

        private void InitCommonFields()
        {
            var columnNames = DataHelper.GetInventoryColumnNames();
            columnNames.Insert(0, new Column { Text = String.Empty, Value = String.Empty });
            var dbFields = columnNames;
            var result = GetResult();
            if (result == null) return;
            var sampleDataList = DataHelper.DesializeSampleData(result.SampleData);

            if (result.Mappings != null)
            {
                var list = result.Mappings.Select(i => new ProfileMappingViewModel(HasHeader)
                {
                    Conditions = MappingHelper.GetConditionsList(i),
                    DBField = i.DBField,
                    //Expression = i.Expression,
                    SampleData = sampleDataList.Where(item => item.XMLField.Equals(i.XMLField)).Select(item => item.Content).FirstOrDefault(),
                    Order = i.Order,
                    Replaces = MappingHelper.GetReplacesList(i),
                    XMLField = i.XMLField,
                    DBFields = dbFields
                }).ToList();

                Mappings =
                    new ObservableCollection<ProfileMappingViewModel>(list);
            }
            else
            {
                Mappings = new ObservableCollection<ProfileMappingViewModel>();
            }
            HasHeader = result.HasHeader;
            ProfileName = result.ProfileName;
            CompanyName = result.CompanyName;
            SchemaURL = result.SchemaURL;
            UseSpecificMapping = result.UseSpecificMapping;
            ExportFileType = result.ExportFileType;
            Delimiter = result.Delimeter;
            Email = result.Email;
            //FileURL = result.FileUrl;
            Frequency = result.Frequency;
            RunningTime = result.RunningTime;
            Discontinue = result.Discontinued;

        }

        private void Initializer()
        {

            ProfileId = _currentvm.Id;
            IsAdded = _view.IsAdded;
            InitCommonFieldsAsync();

            ValidatedProperties = new[] { "ProfileName", "Delimiter", "SchemaURL", "ExportFileType", "Email" };
        }

        //void ImportSampleFile(object parameter)
        //{
        //    if (PropertiesValid)
        //    {
        //        var dlg = new OpenFileDialog { Filter = "All Files (*.*)|*.*", FilterIndex = 1, Multiselect = false };
        //        var result = dlg.ShowDialog();

        //        if (!result.Value) return;

        //        for (int i = Mappings.Count - 1; i >= 0; i--)
        //        {
        //            Mappings.RemoveAt(i);
        //        }

        //        foreach (var fileStream in dlg.OpenFiles())
        //        {
        //            foreach (var item in DataHelper.GetData(HasHeader, Delimiter, fileStream))
        //            {
        //                if (!HasHeader)
        //                {
        //                    item.XMLField = item.XMLField.Replace("Column", "");
        //                }
        //                Mappings.Add(item);
        //            }
        //        }
        //    }
        //}

        private const string Root = "public_html";
        void ImportSampleFileFromFeedUrlAsync(object parameter)
        {
            if (PropertiesValid)
            {
                ProcessSchemaName();

                var worker = new BackgroundWorker();
                Mappings = new ObservableCollection<ProfileMappingViewModel>();

                DoPendingTask(ImportSampleFileFromFeedUrl, parameter);
            }
        }

        void ImportSampleFileFromFeedUrl(object parameter)
        {
            var byteArray = FTPHelper.DownloadFromFtpServer(SchemaURL);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var item in DataHelper.GetData(HasHeader, Delimiter, byteArray))
                {
                    if (!HasHeader)
                    {
                        int result;
                        int.TryParse(item.XMLField.Replace("Column", ""), out result);
                        item.XMLField = (result + 1).ToString(CultureInfo.InvariantCulture);
                    }
                    Mappings.Add(item);
                }
            }));
        }

        private void ProcessSchemaName()
        {
            if (!SchemaURL.Contains(Root))
            {
                if (SchemaURL.Substring(0, 1).Equals("/") || SchemaURL.Substring(0, 1).Equals("\\"))
                {
                    SchemaURL = Root + SchemaURL;
                }
                else
                {
                    SchemaURL = Root + "\\" + SchemaURL;
                }
            }
        }

        #endregion

        #region Commands

        //private RelayCommand _importSampleFileCommand;
        //public RelayCommand ImportSampleFileCommand
        //{
        //    get { return _importSampleFileCommand ?? (_importSampleFileCommand = new RelayCommand(ImportSampleFile, null)); }
        //}

        private RelayCommand _importSampleFileFromFeedUrlCommand;
        public RelayCommand ImportSampleFileFromFeedUrlCommand
        {
            get { return _importSampleFileFromFeedUrlCommand ?? (_importSampleFileFromFeedUrlCommand = new RelayCommand(ImportSampleFileFromFeedUrlAsync, CanImportSampleFileFromFeedUrl)); }
        }

        private bool CanImportSampleFileFromFeedUrl()
        {
            return PropertiesValid;
        }

        private RelayCommand _createProfileCommand;
        public RelayCommand CreateProfileCommand
        {
            get
            {
                return _createProfileCommand ??
                       (_createProfileCommand = new RelayCommand(CreateMappingAsync, CanCreateMapping));
            }
        }

        private RelayCommand _saveMappingCommand;
        public RelayCommand SaveMappingCommand
        {
            get { return _saveMappingCommand ?? (_saveMappingCommand = new RelayCommand(SaveMappingAsync, CanSaveMapping)); }
        }



        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    base.OnPropertyChanged("Email");
                }
            }
        }

        #endregion

        #region Command Contents

        private bool CanCreateMapping()
        {
            return PropertiesValid;
        }



        bool CanSaveMapping()
        {
            return PropertiesValid;
        }

        private void UpdateCurrentVm()
        {
            _currentvm.ProfileName = ProfileName;
            _currentvm.CompanyName = CompanyName;
            _currentvm.Discontinued = Discontinue;
            _currentvm.UseSpecificMapping = UseSpecificMapping;

        }

        #endregion

        #region Members
        //public bool IsBusy
        //{
        //    get { return _isBusy; }
        //    set
        //    {
        //        if (_isBusy != value)
        //        {
        //            _isBusy = value;
        //            base.OnPropertyChanged("IsBusy");
        //        }
        //    }
        //}

        public int ProfileId { get; set; }

        public string Delimiter
        {
            get { return _delimiter; }
            set
            {
                if (_delimiter != value)
                {
                    _delimiter = value;
                    base.OnPropertyChanged("Delimiter");
                }
            }
        }

        public bool Discontinue
        {
            get { return _discontinue; }
            set
            {
                if (_discontinue != value)
                {
                    _discontinue = value;
                    base.OnPropertyChanged("Discontinue");
                }
            }
        }

        public string SchemaURL
        {
            get { return _schemaURL; }
            set
            {
                if (_schemaURL != value)
                {
                    _schemaURL = value;
                    base.OnPropertyChanged("SchemaURL");
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
                    base.OnPropertyChanged("UseSpecificMapping");
                }
            }
        }

        public bool IsAdded
        {
            get { return _isAdded; }

            set
            {
                if (_isAdded != value)
                {
                    _isAdded = value;
                    base.OnPropertyChanged("IsAdded");
                }
            }

        }

        public List<KeyValuePair<string, string>> DelimiterList
        {
            get { return _delimiterList; }
            set
            {
                if (_delimiterList != value)
                {
                    _delimiterList = value;
                    base.OnPropertyChanged("DelimiterList");
                }
            }
        }

        private bool _isAdded;
        private readonly ProfileViewModel _currentvm;

        public ObservableCollection<ProfileMappingViewModel> Mappings
        {
            get { return _mappings; }
            set
            {
                if (_mappings != value)
                {
                    _mappings = value;
                    base.OnPropertyChanged("Mappings");
                }
            }
        }

        public bool HasHeader
        {
            get { return _hasHeader; }
            set
            {
                if (_hasHeader != value)
                {
                    _hasHeader = value;
                    base.OnPropertyChanged("HasHeader");
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
                    base.OnPropertyChanged("ProfileName");
                }
            }
        }

        private readonly IEditView _view;


        private bool _useSpecificMapping;
        private readonly IncomingProfileManagementViewModel _listvm;
        private List<KeyValuePair<string, string>> _fileTypeList;
        private List<KeyValuePair<string, string>> _delimiterList;
        private string _exportFileType;
        private ObservableCollection<ProfileMappingViewModel> _mappings;
        private string _delimiter;
        private bool _discontinue;
        private string _schemaURL;
        private bool _hasHeader;
        private string _profileName;
        private int _frequency;
        private string _companyName;
        private DateTime? _runningTime;
        private string _email;

        public int Frequency
        {
            get { return _frequency; }
            set
            {
                if (_frequency != value)
                {
                    _frequency = value;
                    base.OnPropertyChanged("Frequency");
                }
            }
        }

        public DateTime? RunningTime
        {
            get { return _runningTime; }
            set
            {
                if (_runningTime != value)
                {
                    _runningTime = value;
                    base.OnPropertyChanged("RunningTime");
                }
            }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                    base.OnPropertyChanged("CompanyName");
                }
            }
        }

        public List<KeyValuePair<string, string>> FileTypeList
        {
            get
            {
                return _fileTypeList ?? (_fileTypeList =
                 new List<KeyValuePair<string, string>>
                           {
                               new KeyValuePair<string, string>("txt", "txt"),
                               new KeyValuePair<string, string>("csv", "csv"), 
                           });
            }
        }

        public string ExportFileType
        {
            get { return _exportFileType; }
            set
            {
                if (_exportFileType != value)
                {
                    _exportFileType = value;
                    DelimiterList = DataHelper.GetDelimiterList(_exportFileType);
                    base.OnPropertyChanged("ExportFileType");
                    base.OnPropertyChanged("DelimiterList");
                    base.OnPropertyChanged("Delimiter");
                }
            }
        }

        #endregion



        public IncomingProfileTemplateViewModel(IEditView view, ProfileViewModel currentViewModel, IncomingProfileManagementViewModel exportProfileManagementViewModel)
        {
            _listvm = exportProfileManagementViewModel;
            _currentvm = currentViewModel ?? new ProfileViewModel { Id = 0 };
            _view = view;
            Initializer();
            _view.SetDataContext(this);
        }

        #region Overrides of BaseProfileTemplateViewModel

        protected MappingViewModel GetResult()
        {
            //UpdateProfileId();
            var xmlHelper = new XMLHelper();
            if (ProfileId != 0)
            {
                return xmlHelper.LoadMappingTemplateFromProfileId(ProfileId);
            }
            return new MappingViewModel();
        }



        #endregion



        #region Implementation of IDataErrorInfo

        protected override string GetValidationError(string property)
        {
            if (Array.IndexOf(ValidatedProperties, property) < 0)
                return null;
            string result;
            switch (property)
            {
                case "ProfileName":
                    result = ValidateProfileNameField();
                    break;
                case "Delimiter":
                    return String.IsNullOrEmpty(Delimiter) && !(!String.IsNullOrEmpty(ExportFileType) && ExportFileType.Equals("csv")) ? "Delimiter should have value" : null;
                case "Email":
                    return ValidationHelper.ValidateEmail(Email);
                case "SchemaURL":
                    return String.IsNullOrEmpty(SchemaURL) ? "SchemaURL should have value" : null;
                case "ExportFileType":
                    return String.IsNullOrEmpty(ExportFileType) ? "ExportFileType should have value" : null;
                default:
                    result = null;
                    break;
            }
            return result;
        }

        private string ValidateDelimiterField()
        {
            return String.IsNullOrEmpty(Delimiter) ? "Delimiter should have value" : null;
        }

        private string ValidateProfileNameField()
        {
            return String.IsNullOrEmpty(ProfileName) ? "ProfileName should have value" : null;
        }

        ///// <summary>
        ///// Gets an error message indicating what is wrong with this object.
        ///// </summary>
        ///// <returns>
        ///// An error message indicating what is wrong with this object. The default is an empty string ("").
        ///// </returns>
        //public string Error { get; private set; }

        #endregion
    }
}
