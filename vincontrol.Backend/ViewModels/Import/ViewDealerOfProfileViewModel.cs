using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Model;
using System.Linq;
using vincontrol.Backend.Windows.DataFeed.Import;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.ViewModels.Import
{
    public class ViewDealerOfProfileViewModel : ViewModelBase
    {
        #region Manage Data
        void SearchDealerAsync(object parameter)
        {
            DoPendingTask(SearchDealer, parameter);
        }

        void SearchDealer(object parameter)
        {
            int result;
            int.TryParse(SearchContent, out result);
            var context = new vincontrolwarehouseEntities();
            var dealers =
                context.dealers.Where(i => i.Name.ToLower().Contains(SearchContent.ToLower()) || i.Id == result).Select(i => new Dealer { Id = i.Id, Name = i.Name }).ToList();
            int count = SearchResultDealers.Count - 1;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    for (int i = count; i >= 0; i--)
                    {
                        SearchResultDealers.Remove(SearchResultDealers[i]);
                    }
                    foreach (var item in dealers)
                    {
                        SearchResultDealers.Add(item);
                    }
                }));
        }

        void DeleteImportDealerAsync(object parameter)
        {
            DoPendingTask(DeleteImportDealer, parameter);
        }

        void DeleteImportDealer(object parameter)
        {
            var context = new vincontrolwarehouseEntities();
            var dealer = (DealerImportSetting)parameter;
            var importfeed = context.dealers_dealersetting.FirstOrDefault(i => i.Id == dealer.Id);
            if (importfeed != null)
            {
                importfeed.ImportDataFeedProfileId = null;
                context.SaveChanges();
                Tracking.Log(UserAction.Delete, App.CurrentUser.Id, DateTime.Now, importfeed.Id, string.Format("Profile:{0} Dealer:{1}", importfeed.importdatafeedprofile.ProfileName, importfeed.dealer.Name), ItemType.ImportProfileDealer);

                //_vm.Dealers.Add(dealer);
                Application.Current.Dispatcher.BeginInvoke(new Action(() => SelectedDealers.Remove(dealer)));
            }
        }

        void SaveDealerImportProfileAsync(object parameter)
        {
            DoPendingTask(SaveDealerImportProfile, parameter);
        }

        void SaveDealerImportProfile(object parameter)
        {
            if (CheckCanExecute())
            {
                //var setting = (DealerImportSetting)parameter;
                var context = new vincontrolwarehouseEntities();
                var setting = context.dealers_dealersetting.Where(i => i.ImportDataFeedProfileId == ProfileId);
                //var importfeed = context.dealers_dealersetting.FirstOrDefault(i => i.Id == setting.Id);
                foreach (var item in setting)
                {
                    var result = SelectedDealers.FirstOrDefault(i => i.DealerId == item.DealershipId);
                    if (result == null) continue;
                    item.ImportFeedUrl = result.FeedUrl;
                    item.DiscontinuedImport = result.Discontinued;
                    Tracking.Log(UserAction.Update, App.CurrentUser.Id, DateTime.Now, item.Id, string.Format("Profile:{0} Dealer:{1}", item.importdatafeedprofile.ProfileName, item.dealer.Name), ItemType.ImportProfileDealer);

                }

                //if (importfeed != null)
                //{
                //    importfeed.ImportFeedUrl = setting.FeedUrl;
                //    = setting.Discontinued;
                context.SaveChanges();
                //    //_vm.Dealers.Add(dealer);
                //}

                Application.Current.Dispatcher.BeginInvoke(
                    new Action(
                        () => _view.Close()));
            }
            else
            {
                MessageBox.Show("All feed URL has to be filled in.");
            }
        }

        private void InitDataAsync()
        {
            DoPendingTask(InitData);
            _view.SetDataContext(this);
        }

        private void InitData()
        {
            var context = new vincontrolwarehouseEntities();
            _searchResultDealers = new ObservableCollection<Dealer>();
            var result = from h in context.importservicehistories
                         where h.ImportProfileId == ProfileId
                         && h.Status == "Completed"
                         group h by h.DealerId into g
                         select g.OrderByDescending(i => i.RunningDate).FirstOrDefault();
            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>

            SelectedDealers =
                new ObservableCollection<DealerImportSetting>((from d in context.dealers_dealersetting
                                                               where d.ImportDataFeedProfileId == ProfileId
                                                               join r in result
                                                                   on d.DealershipId equals r.DealerId
                                                                   into g
                                                               from i in g.DefaultIfEmpty()
                                                               select new DealerImportSetting
                                                                       {
                                                                           FileUrl = i.ArchiveFileName,
                                                                           LastDepositedDate = i.RunningDate,
                                                                           Id = d.Id,
                                                                           DealerId = d.dealer.Id,
                                                                           DealerName = d.dealer.Name,
                                                                           FeedUrl = d.ImportFeedUrl,
                                                                           Discontinued = d.DiscontinuedImport ?? false
                                                                       }).ToList())
            ;
            //));
        }

        private void RefreshTracking()
        {
            InitData();
            UpdateParentView();
        }

        private void UpdateParentView()
        {
            _currentvm.LastDepositedDate = SelectedDealers.Max(i => i.LastDepositedDate);
            _currentvm.FileURL = SelectedDealers.Where(i => i.LastDepositedDate.Equals(_currentvm.LastDepositedDate)).Select(i => i.FileUrl).FirstOrDefault();
        }

        private void DownloadFileAsync(object parameter)
        {
            if (parameter != null)
            {
                DoPendingTask(DownloadFile, parameter);
            }
        }

        private void DownloadFile(object parameter)
        {
            if (parameter != null)
            {
                DataHelper.DownloadImportFile(parameter.ToString());
            }
        }
        private void CreateTask()
        {
            var context = new vincontrolwarehouseEntities();
            var importfeed = context.importdatafeedprofiles.FirstOrDefault(i => i.Id == ProfileId);
            //var taskExecution = new TaskExecution();
            //if (exportfeed != null)
            //    taskExecution.CreateDailyTask("Export_" + exportfeed.ProfileName, Environment.CurrentDirectory.Replace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "vincontrol.DataFeedService") + "\\vincontrol.DataFeedService.exe", "Export " + exportfeed.Id, exportfeed.RunningTime ?? DateTime.MinValue, exportfeed.Frequency ?? 0, @System.Configuration.ConfigurationManager.AppSettings["DataFeedUserDomain"], System.Configuration.ConfigurationManager.AppSettings["DataFeedPasswordDomain"]);
            if (importfeed != null)
            {
                if (importfeed.Discontinued == null || !importfeed.Discontinued.Value)
                {
                    DateTime dateTime = importfeed.RunningTime ?? DateTime.MinValue;
                    var request =
                        WebRequest.Create(
                            string.Format("http://{0}/VinHelper/CreateDataFeedTask/{1}/{2}/{3}/{4}/{5}",
                                          System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                          "Import_" + importfeed.ProfileName, "Import " + importfeed.Id, dateTime.Hour,
                                          dateTime.Minute
                                          , importfeed.Frequency ?? 0));
                    // ReSharper disable AssignNullToNotNullAttribute
                    var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
                    // ReSharper restore AssignNullToNotNullAttribute
                    responseStream.ReadToEnd();
                    responseStream.Close();
                }
            }
        }

        void TestImportAsync(object parameter)
        {
            if (CheckCanExecute())
            {
                DoPendingTask(TestImport, parameter);
            }
            else
            {
                MessageBox.Show("All filename has to be filled in.");
            }
        }

        void TestImport(object parameter)
        {
            var convertHelper = new ConvertHelper();
            convertHelper.ImportFileToDatabaseByProfile(ProfileId);
        }

        void AssignDealerToProfileAsync(object parameter)
        {
            DoPendingTask(AssignDealerToProfile, parameter);
        }

        void AssignDealerToProfile(object parameter)
        {
            var context = new vincontrolwarehouseEntities();
            var dealer = (Dealer)parameter;

            var importfeed = context.dealers_dealersetting.FirstOrDefault(i => i.Id == dealer.Id);
            if (importfeed != null)
            {
                if (importfeed.ImportDataFeedProfileId != null)
                {
                    if (importfeed.ImportDataFeedProfileId == ProfileId)
                    {
                        MessageBox.Show(string.Format("Cannot add dealer. Dealer {0} already existed in this profile.",
                                                      dealer.Name));
                    }
                    else
                    {
                        if (App.CurrentUser.Roles.Contains("Admin"))
                        {
                            MessageBoxResult messageResult = MessageBox.Show(string.Format("Dealer {0} already existed in {1} profile.Do you want to continue?", dealer.Name, importfeed.importdatafeedprofile.ProfileName), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (messageResult == MessageBoxResult.Yes)
                            {
                                AddDealerToProfile(context, dealer, importfeed);
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Cannot add dealer. Dealer {0} already existed in {1} profile. In order to do that you have to have admin right.",
                                                dealer.Name, importfeed.importdatafeedprofile.ProfileName));
                        }

                    }
                }
                else
                {
                    AddDealerToProfile(context, dealer, importfeed);
                }
            }
        }

        private void AddDealerToProfile(vincontrolwarehouseEntities context, Dealer dealer, dealers_dealersetting importfeed)
        {
            importfeed.ImportDataFeedProfileId = ProfileId;
            importfeed.ImportFeedUrl = String.Empty;
            context.SaveChanges();
            Tracking.Log(UserAction.CreateTask, App.CurrentUser.Id, DateTime.Now, importfeed.Id, string.Format("Profile:{0} Dealer:{1}", importfeed.importdatafeedprofile.ProfileName, importfeed.dealer.Name), ItemType.ExportProfileDealer);

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    SelectedDealers.Add(new DealerImportSetting
                    {
                        DealerName = dealer.Name,
                        DealerId = dealer.Id,
                        Id = importfeed.Id
                    })));
        }


        #endregion

        #region Search

        private ObservableCollection<Dealer> _searchResultDealers;

        public ObservableCollection<Dealer> SearchResultDealers
        {
            get { return _searchResultDealers; }
            set
            {
                if (_searchResultDealers != value)
                {
                    _searchResultDealers = value;
                    base.OnPropertyChanged("SearchResultDealers");
                }
            }
        }

        private RelayCommand _searchDealerCommand;

        public RelayCommand SearchDealerCommand
        {
            get { return _searchDealerCommand ?? (_searchDealerCommand = new RelayCommand(SearchDealerAsync, null)); }
        }



        public string SearchContent { get; set; }

        #endregion

        public bool TaskExisted
        {
            get { return _taskExisted; }
            set
            {
                if (_taskExisted != value)
                {
                    _taskExisted = value;
                    base.OnPropertyChanged("TaskExisted");
                }
            }
        }

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
        //private bool _isBusy;

        private RelayCommand _deleteImportDealerCommand;

        public RelayCommand DeleteImportDealerCommand
        {
            get { return _deleteImportDealerCommand ?? (_deleteImportDealerCommand = new RelayCommand(DeleteImportDealerAsync, null)); }
        }

        private RelayCommand _saveDealerImportProfileCommand;

        public RelayCommand SaveDealerImportProfileCommand
        {
            get
            {
                return _saveDealerImportProfileCommand ??
                       (_saveDealerImportProfileCommand = new RelayCommand(SaveDealerImportProfileAsync, null));
            }
        }

        private RelayCommand _showHistoryCommand;

        public RelayCommand ShowHistoryCommand
        {
            get
            {
                return _showHistoryCommand ??
                       (_showHistoryCommand = new RelayCommand(ShowHistory, null));
            }
        }

        private void ShowHistory(object obj)
        {
            var window = new ImportHistoryWindow(ProfileId);
            window.Show();
        }

        private readonly IEditView _view;
        public ObservableCollection<DealerImportSetting> SelectedDealers
        {
            get { return _selectedDealers; }
            set
            {
                if (_selectedDealers != value)
                {
                    _selectedDealers = value;
                    base.OnPropertyChanged("SelectedDealers");
                }
            }
        }



        //public List<Dealer> Dealers { get; set; }
        //public Dealer AddedDealer { get; set; }

        private RelayCommand _assignDealerToProfileCommand;
        private RelayCommand _testImportCommand;
        private ObservableCollection<DealerImportSetting> _selectedDealers;
        private readonly ProfileViewModel _currentvm;
        private bool _taskExisted;
        private RelayCommand _createTaskProfileCommand;
        private RelayCommand _importManuallyCommand;
        private RelayCommand _downloadFileCommand;
        private RelayCommand _applyCommand;

        public ViewDealerOfProfileViewModel(IEditView view, ProfileViewModel profilevm)
        {
            _currentvm = profilevm;
            _view = view;
            ProfileId = _currentvm.Id;
            InitDataAsync();
            TaskExisted = DataHelper.InitTaskStatus("Import_" + _currentvm.ProfileName);

            //_view.SetDataContext(this);
        }

        public RelayCommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFileAsync, null)); }
        }

        public RelayCommand AssignDealerToProfileCommand
        {
            get { return _assignDealerToProfileCommand ?? (_assignDealerToProfileCommand = new RelayCommand(AssignDealerToProfileAsync, null)); }
        }

        public RelayCommand TestImportCommand
        {
            get { return _testImportCommand ?? (_testImportCommand = new RelayCommand(TestImportAsync, null)); }
        }

        public RelayCommand ImportManuallyCommand
        {
            get { return _importManuallyCommand ?? (_importManuallyCommand = new RelayCommand(ImportManuallyAsync, null)); }
        }

        private void ImportManuallyAsync(object obj)
        {
            if (CheckCanExecute())
            {
                DoPendingTask(ImportManually, obj);
            }
            else
            {
                MessageBox.Show("All feed URL has to be filled in.");
            }
        }

        private void ImportManually(object obj)
        {
            var request =
                       WebRequest.Create(
                           string.Format("http://{0}/VinHelper/ImportDataFeedByProfile/{1}",
                                         System.Configuration.ConfigurationManager.AppSettings[
                                             "TaskServer"],
                                         ProfileId));
            // ReSharper disable AssignNullToNotNullAttribute
            var responseStream =
                new StreamReader(request.GetResponse().GetResponseStream());
            // ReSharper restore AssignNullToNotNullAttribute
            var response = responseStream.ReadToEnd();
            responseStream.Close();
            if (!response.Trim().ToLower().Equals("\"true\""))
            {
                MessageBox.Show(response);
                Tracking.Log(UserAction.PushManually, App.CurrentUser.Id, DateTime.Now, 0, string.Format("Import Profile:{0}", ProfileId), ItemType.ImportProfile);
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                          {
                                                                              RefreshTracking();
                                                                              var window =
                                                                                  new ImportHistoryWindow(ProfileId);
                                                                              window.Show();
                                                                          }));
            }
        }


        public RelayCommand ApplyCommand
        {
            get
            {
                return _applyCommand ??
                       (_createTaskProfileCommand = new RelayCommand(ApplyAsync, null));
            }
        }

        void ApplyAsync(object parameter)
        {
            if (CheckCanExecute())
            {
                DoPendingTask(Apply, parameter);
            }
            else
            {
                MessageBox.Show("All feed URL has to be filled in.");
            }
        }

        void Apply(object parameter)
        {
            SaveDealerImportProfileAsync(parameter);
            CreateTask();
            Tracking.Log(UserAction.CreateTask, App.CurrentUser.Id, DateTime.Now, 0, string.Format("Import Profile:{0} Create Task", ProfileId), ItemType.ImportProfile);

            Application.Current.Dispatcher.BeginInvoke(new Action(() => _view.Close()));
        }

        bool CheckCanExecute()
        {
            return SelectedDealers.All(dealerExportSetting => !String.IsNullOrEmpty(dealerExportSetting.FeedUrl));
        }

        public int ProfileId { get; set; }

        #region Overrides of ViewModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
