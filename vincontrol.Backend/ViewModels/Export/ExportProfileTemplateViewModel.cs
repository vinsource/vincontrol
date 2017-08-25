using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using vincontrol.Backend.Commands;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.Backend.Windows.DataFeed.Export;
using vincontrol.DataFeed.Helper;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.ViewModels.Export
{
    public class ExportProfileTemplateViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Members
        private readonly ExportProfileViewModel _currentvm;
        private readonly ExportProfileManagementViewModel _listvm;
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

        private List<KeyValuePair<string, string>> _fileTypeList;
        private List<KeyValuePair<string, string>> _inventoryStatusList;
        private bool _isAdded;
        private readonly IEditView _view;

        public List<KeyValuePair<int, string>> NumberList { get; set; }
        public int ProfileId { get; set; }
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



        public string InventoryStatus
        {
            get { return _inventoryStatus; }
            set
            {
                if (_inventoryStatus != value)
                {
                    _inventoryStatus = value;
                    base.OnPropertyChanged("InventoryStatus");
                }
            }
        }

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

        public FileNameFormat FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    SelectedFileName = _fileName.Value;
                    base.OnPropertyChanged("FileName");
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
                    base.OnPropertyChanged("SelectedFileName");
                }
            }
        }

        public bool Bundle
        {
            get { return _bundle; }
            set
            {
                if (_bundle != value)
                {
                    _bundle = value;
                    base.OnPropertyChanged("Bundle");
                    base.OnPropertyChanged("SelectedFileName");
                }
            }
        }

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

        public string FTPHost
        {
            get { return _ftpHost; }
            set
            {
                if (_ftpHost != value)
                {
                    _ftpHost = value;
                    base.OnPropertyChanged("FTPHost");

                }
            }
        }

        public string FTPUserName
        {
            get { return _ftpUserName; }
            set
            {

                if (_ftpUserName != value)
                {
                    _ftpUserName = value;
                    base.OnPropertyChanged("FTPUserName");

                }
            }
        }

        public string FTPPassword
        {
            get { return _ftpPassword; }
            set
            {

                if (_ftpPassword != value)
                {
                    _ftpPassword = value;
                    base.OnPropertyChanged("FTPPassword");

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

        public bool SplitImage
        {
            get { return _splitImage; }
            set
            {
                if (_splitImage != value)
                {
                    _splitImage = value;
                    base.OnPropertyChanged("SplitImage");

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
        public List<KeyValuePair<string, string>> InventoryStatusList
        {
            get
            {
                return _inventoryStatusList ?? (_inventoryStatusList =
                 new List<KeyValuePair<string, string>>
                           {
                               new KeyValuePair<string, string>("Both", "Both"),
                               new KeyValuePair<string, string>("New", "New"), 
                               new KeyValuePair<string, string>("Used", "Used"),
                           });
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

        #region Command
        private RelayCommand _createExportProfileCommand;
        public RelayCommand CreateExportProfileCommand
        {
            get
            {
                return _createExportProfileCommand ??
                       (_createExportProfileCommand = new RelayCommand(CreateExportProfileAsync, CanCreateExportProfile));
            }
        }

        private bool CanCreateExportProfile()
        {
            //if (Mappings.Any(item => !item.PropertiesValid))
            //{
            //    return false;
            //}
            return PropertiesValid;
        }

        private OrderChangedCommand _orderChangedCommand;
        public OrderChangedCommand OrderChangedCommand
        {
            get { return _orderChangedCommand ?? (_orderChangedCommand = new OrderChangedCommand(this)); }
        }

        private RelayCommand _inFocusCommand;
        public RelayCommand InFocusCommand
        {
            get { return _inFocusCommand ?? (_inFocusCommand = new RelayCommand(InFocus, null)); }
        }

        private void InFocus(object obj)
        {
            var item = (ProfileMappingViewModel)obj;
            if (item.DBField == null && Mappings[Mappings.Count - 1] == item)
            {
                AddMapping();
            }
        }


        private RelayCommand _openFileNameWindowCommand;
        public RelayCommand OpenFileNameWindowCommand
        {
            get
            {
                return _openFileNameWindowCommand ??
                       (_openFileNameWindowCommand = new RelayCommand(OpenFileNameWindow, null));
            }
        }



        #endregion

        public ExportProfileTemplateViewModel(IEditView view, ExportProfileViewModel exportProfilevm, ExportProfileManagementViewModel exportProfileManagementViewModel)
        {
            _view = view;
            _view.SetDataContext(this);
            _currentvm = exportProfilevm ?? new ExportProfileViewModel { Id = 0 };
            _listvm = exportProfileManagementViewModel;

            Initializer();

        }

        #endregion

        #region Manipulate Data

        void CreateExportProfileAsync(object parameter)
        {
            DoPendingTask(CreateExportProfile, parameter);
        }

        void CreateExportProfile(object parameter)
        {

            var xmlHelper = new ExportXMLHelper();
            var xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(this));
            //string sampleDataXML = DataHelper.SerializeSampleData(_vm);
            var context = new vincontrolwarehouseEntities();
            var item = context.datafeedprofiles.FirstOrDefault(i => i.ProfileName.Equals(ProfileName));
            if (item == null)
            {
                var feedProfile = new datafeedprofile
                {
                    Mapping = xmlContent,
                    Bundle = Bundle,
                    CompanyName = ProfileName,
                    DefaultPassword = FTPPassword,
                    DefaultUserName = FTPUserName,
                    FTPServer = FTPHost,
                    FileName = FileName.Value,
                    Frequency = Frequency,
                    RunningTime = RunningTime,
                    Discontinued = Discontinue,
                    ProfileName = ProfileName,
                    Email = Email
                };

                context.AddTodatafeedprofiles(feedProfile);
                context.SaveChanges();
                Tracking.Log(UserAction.Insert, App.CurrentUser.Id, DateTime.Now, feedProfile.Id, string.Format("Profile:{0}", feedProfile.ProfileName), ItemType.ExportProfile);

                ProfileId = feedProfile.Id;
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                          {
                                                                              _listvm.Profiles.Add(new ExportProfileViewModel
                                                                                                       {
                                                                                                           CompanyName =
                                                                                                               CompanyName,
                                                                                                           ProfileName =
                                                                                                               ProfileName,
                                                                                                           Id =
                                                                                                               ProfileId,
                                                                                                           IsBundle =
                                                                                                               Bundle,
                                                                                                           Discontinued
                                                                                                               = false
                                                                                                       });
                                                                              _view.Close();
                                                                          }));
            }
            else
            {
                MessageBox.Show(string.Format("Profile name {0} already existed. Please fill in other name for profile.", ProfileName));
            }
        }

        void OpenFileNameWindow(object parameter)
        {
            var window = new ExportFilenameFormatWindow(FileName, ExportType.Profile, null, ProfileId);
            window.Closed += new EventHandler(WindowClosed);
            window.Show();
        }

        void WindowClosed(object sender, EventArgs e)
        {
            var w = (ExportFilenameFormatWindow)sender;
            if (w.IsDirty)
            {
                this.SelectedFileName = this.FileName.Value;
            }
        }

        protected MappingViewModel GetResult()
        {
            var xmlHelper = new ExportXMLHelper();
            if (_currentvm.Id > 0)
            {
                var result = xmlHelper.LoadMappingTemplateByCompanyId(_currentvm.Id);
                return result;
            }
            return new MappingViewModel();
        }

        private void SaveMappingAsync(object parameter)
        {
            DoPendingTask(SaveMapping, parameter);
        }

        private void SaveMapping(object parameter)
        {
            var xmlHelper = new ExportXMLHelper();
            string xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(this));
            var context = new vincontrolwarehouseEntities();

            var firstPofile = context.datafeedprofiles.FirstOrDefault(i => i.Id == ProfileId);

            if (firstPofile != null)
            {
                firstPofile.Mapping = xmlContent;
                firstPofile.Bundle = Bundle;
                firstPofile.CompanyName = ProfileName;
                firstPofile.DefaultPassword = FTPPassword;
                firstPofile.DefaultUserName = FTPUserName;
                firstPofile.FTPServer = FTPHost;
                firstPofile.FileName = FileName.Value;
                firstPofile.Frequency = Frequency;
                firstPofile.RunningTime = RunningTime;
                firstPofile.Discontinued = Discontinue;
                firstPofile.ProfileName = ProfileName;
                firstPofile.Email = Email;
                //firstPofile.UseHeader 
                context.SaveChanges();
                Tracking.Log(UserAction.Update, App.CurrentUser.Id, DateTime.Now, firstPofile.Id, string.Format("Export Profile:{0}", firstPofile.ProfileName), ItemType.ExportProfile);

                Application.Current.Dispatcher.BeginInvoke(new Action(UpdateCurrentVm));
                DataHelper.UpdateExportTask(Discontinue,ProfileName,RunningTime,ProfileId,Frequency);
                Application.Current.Dispatcher.BeginInvoke(new Action(() => _view.Close()));
            }
        }

      
        private void InitCommonFieldsAsync()
        {
            DoPendingTask(InitCommonFields);

        }

        private void InitCommonFields()
        {
            var result = GetResult();
            if (result == null) return;

            var columnNames = DataHelper.GetInventoryColumnNames();
            columnNames.Insert(0,
                               new Column
                               {
                                   Text = String.Empty,
                                   Value =
                                       String.Empty
                               });
            var dbFields = columnNames;
            NumberList.Add( new KeyValuePair<int, string>(0, String.Empty));

            for (int i = 1; i < dbFields.Count; i++)
            {
                
                NumberList.Add(
                    new KeyValuePair<int, string>(i, i.ToString()));
            }

            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{



            //var sampleDataList = new List<SampleData>(
            //    DataHelper.DesializeSampleData(
            //        result.SampleData);


            if (result.Mappings != null)
            {
                var list =
                    result.Mappings.Where(
                        i =>
                        (!result.HasHeader &&
                         i.Order != 0) ||
                        (result.HasHeader &&
                         !String.IsNullOrEmpty(i.XMLField)))
                        .Select(
                            i =>
                            new ProfileMappingViewModel(
                                HasHeader)
                                {
                                    Conditions =
                                        MappingHelper.
                                        GetConditionsList
                                        (i),
                                    DBField = i.DBField,
                                    Expression =
                                        i.Expression,
                                    //SampleData =
                                    //    sampleDataList.
                                    //    Where(
                                    //        item =>
                                    //        item.XMLField
                                    //            .Equals(
                                    //                i.
                                    //                    XMLField))
                                    //    .Select(
                                    //        item =>
                                    //        item.Content)
                                    //    .FirstOrDefault(),
                                    Order = i.Order,
                                    Replaces =
                                        MappingHelper.
                                        GetReplacesList(i),
                                    XMLField = i.XMLField,
                                    DBFields = dbFields

                                }).ToList();

                Mappings =
                    new ObservableCollection
                        <ProfileMappingViewModel>(list);
            }
            else if (IsAdded)
            {

                //Mappings = new ObservableCollection<ProfileMappingViewModel>(dbFields.Where(i => !String.IsNullOrEmpty(i.Value)).Select(field => new ProfileMappingViewModel(HasHeader) { DBFields = dbFields, DBField = field.Text }).ToList());
                Mappings = new ObservableCollection
                    <ProfileMappingViewModel>
                                                                                             {
                                                                                                 new ProfileMappingViewModel
                                                                                                     (HasHeader)
                                                                                                     {
                                                                                                         DBFields =
                                                                                                             dbFields
                                                                                                     }
                                                                                             };
            }
            else
            {
                Mappings =
                    new ObservableCollection
                        <ProfileMappingViewModel>();
            }


            HasHeader = result.HasHeader;
            ProfileName = result.ProfileName;
            ExportFileType = result.ExportFileType;
            InventoryStatus = result.InventoryStatus;
            FTPHost = result.FTPHost;
            FTPUserName = result.FTPUserName;
            FTPPassword = result.FTPPassword;
            Frequency = result.Frequency;
            RunningTime = result.RunningTime;
            Bundle = result.Bundle;
            FileName = new FileNameFormat { Value = result.FileName };
            Discontinue = result.Discontinued;
            Delimiter = result.Delimeter;
            CompanyName = result.CompanyName;
            Email = result.Email;
            SplitImage = result.SplitImage;
            //}));
        }

        private void Initializer()
        {
            ProfileId = _currentvm.Id;
            IsAdded = _view.IsAdded;
            NumberList = new List<KeyValuePair<int, string>>();
            ValidatedProperties = new[] { "ProfileName", "ExportFileType", "FTPHost", "FTPUserName", "FTPPassword", "Delimiter", "Email", "SelectedFileName" };

            InitCommonFieldsAsync();
        }
        #endregion

        #region Common


        private RelayCommand _saveExportMappingCommand;
        private bool _hasHeader;
        private FileNameFormat _fileName;
        private RelayCommand _addExportMappingCommand;
        private string _exportFileType;
        private List<KeyValuePair<string, string>> _delimiterList;
        private string _selectedFileName;
        private bool _bundle;
        private ObservableCollection<ProfileMappingViewModel> _mappings;
        private string _inventoryStatus;
        private string _profileName;
        private string _delimiter;
        private int _frequency;
        private DateTime? _runningTime;
        private string _ftpHost;
        private string _ftpUserName;
        private string _ftpPassword;
        private bool _discontinue;
        private string _email;
        private bool _splitImage;


        public RelayCommand SaveExportMappingCommand
        {
            get
            {
                return _saveExportMappingCommand ??
                       (_saveExportMappingCommand = new RelayCommand(SaveMappingAsync, CanSaveMapping));
            }
        }

        private bool CanSaveMapping()
        {
            if (Mappings == null)
            {
                return PropertiesValid;
            }

            if (Mappings.Any(item => !item.PropertiesValid))
            {
                return false;
            }

            return PropertiesValid;
        }


        public RelayCommand AddExportMappingCommand
        {
            get { return _addExportMappingCommand ?? (_addExportMappingCommand = new RelayCommand(AddMapping, null)); }
        }

        private void AddMapping(object parameter)
        {
            AddMapping();
        }

        private void AddMapping()
        {
            var item = new ProfileMappingViewModel(HasHeader);

            if (Mappings != null && Mappings.Any())
            {
                item.DBFields = Mappings[0].DBFields;
            }
            else
            {
                var columnNames = DataHelper.GetInventoryColumnNames();
                columnNames.Insert(0, new Column { Text = String.Empty, Value = String.Empty });
                item.DBFields = columnNames;
            }

            if (Mappings != null)
            {
                item.Order = !Mappings.Any() ? 0 : Mappings.Select(i => i.Order).Max() + 1;
                if (Mappings != null)
                {
                    Mappings.Add(item);
                    //((IScrollable)_view).ScrollToEnd();
                }
            }
        }




        public string CompanyName { get; set; }


        #endregion

        private void UpdateCurrentVm()
        {
            _currentvm.Discontinued = Discontinue;
            _currentvm.ProfileName = ProfileName;
            _currentvm.CompanyName = CompanyName;
            _currentvm.IsBundle = Bundle;
        }

        #region Overrides of ViewModelBase

        protected override string GetValidationError(string property)
        {
            if (Array.IndexOf(ValidatedProperties, property) < 0)
                return null;
            switch (property)
            {
                case "ProfileName":
                    return String.IsNullOrEmpty(ProfileName) ? "ProfileName should have value" : null;
                case "Email":
                    return ValidationHelper.ValidateEmail(Email);
                case "ExportFileType":
                    return String.IsNullOrEmpty(ExportFileType) ? "Format should have value" : null;
                case "Delimiter":
                    return String.IsNullOrEmpty(Delimiter) && !(!String.IsNullOrEmpty(ExportFileType) && ExportFileType.Equals("csv")) ? "Delimiter should have value" : null;
                case "FTPHost":
                    return String.IsNullOrEmpty(FTPHost) ? "FTPHost should have value" : null;
                case "FTPUserName":
                    return String.IsNullOrEmpty(FTPUserName) ? "FTPUserName should have value" : null;
                case "FTPPassword":
                    return String.IsNullOrEmpty(FTPPassword) ? "FTPPassword should have value" : null;
                case "SelectedFileName":
                    return String.IsNullOrEmpty(SelectedFileName) && Bundle ? "FileName should have value" : null;
                default:
                    return null;
            }
        }

        #endregion
    }
}
