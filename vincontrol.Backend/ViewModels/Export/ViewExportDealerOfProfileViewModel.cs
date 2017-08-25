using System;
using System.Collections.Generic;
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
using vincontrol.Backend.Windows.DataFeed.Export;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.ViewModels.Export
{
    public class ViewExportDealerOfProfileViewModel : ViewModelBase
    {
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

        private RelayCommand _searchExportDealerCommand;

        public RelayCommand SearchExportDealerCommand
        {
            get { return _searchExportDealerCommand ?? (_searchExportDealerCommand = new RelayCommand(SearchExportDealerAsync, null)); }
        }

        public string SearchContent { get; set; }

        #endregion

        #region Manipulate Data

        private void InitDataAsync()
        {
            DoPendingTask(InitData);
            _view.SetDataContext(this);
        }

        private void InitData()
        {
            var context = new vincontrolwarehouseEntities();
            _searchResultDealers = new ObservableCollection<Dealer>();

            var firstOrDefault = context.datafeedprofiles.FirstOrDefault(i => i.Id == ProfileId);
            if (firstOrDefault != null)
                IsBundle = firstOrDefault.Bundle ?? false;

            var result = from h in context.exportservicehistories
                         where h.DatafeedProfileId == ProfileId
                         && h.Status == "Completed"
                         group h by h.DealerId into g
                         select g.OrderByDescending(i => i.RunningDate).FirstOrDefault();

            //SelectedDealers = new ObservableCollection<DealerExportSetting>(context.datafeedlookups.Where(i => i.DataFeedProfileId == ProfileId).Select(i => new DealerExportSetting { Id = i.Id, DealerId = i.DealerId, DealerName = i.dealer.Name, FileName = new FileNameFormat { Value = i.FileName }, SelectedFileName = i.FileName, Discontinued = i.Discontinued ?? false, IsBundle = IsBundle }).ToList());

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                      {
                                                                          SelectedDealers = new ObservableCollection<DealerExportSetting>((
            from d in context.datafeedlookups
            where d.DataFeedProfileId == ProfileId
            join r in result
            on d.DealerId equals r.DealerId into g
            from i in g.DefaultIfEmpty()
            select new DealerExportSetting
            {
                FileUrl = i.ArchiveFileName,
                LastDepositedDate = i.RunningDate,
                Id = d.Id,
                DealerId = d.dealer.Id,
                DealerName = d.dealer.Name,
                FileName = new FileNameFormat { Value = d.FileName },
                SelectedFileName = d.FileName,
                Discontinued = d.Discontinued ?? false,
                IsBundle = IsBundle
            }).ToList());
                                                                          TaskExisted = DataHelper.InitTaskStatus("Export_" + _currentvm.ProfileName);

                                                                      }));

        }



        void SearchExportDealerAsync(object parameter)
        {
            DoPendingTask(SearchExportDealer, parameter);
        }

        void SearchExportDealer(object parameter)
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
                    SearchResultDealers.Remove(
                        SearchResultDealers[i]);
                }
                foreach (var item in dealers)
                {
                    SearchResultDealers.Add(item);
                }
            }));
        }

        void DeleteExportDealerAsync(object parameter)
        {
            DoPendingTask(DeleteExportDealer, parameter);
        }

        void DeleteExportDealer(object parameter)
        {
            var context = new vincontrolwarehouseEntities();
            var dealer = (DealerExportSetting)parameter;
            var importfeed = context.datafeedlookups.FirstOrDefault(i => i.Id == dealer.Id);
            if (importfeed != null)
            {
                context.DeleteObject(importfeed);
                context.SaveChanges();
                //_vm.Dealers.Add(dealer);
                Tracking.Log(UserAction.Delete, App.CurrentUser.Id, DateTime.Now, importfeed.Id, string.Format("Export Profile:{0} Dealer:{1}",importfeed.datafeedprofile.ProfileName, importfeed.dealer.Name), ItemType.ExportProfileDealer);

                Application.Current.Dispatcher.BeginInvoke(new Action(() => SelectedDealers.Remove(dealer)));
            }
        }

        void SaveDealerExportProfileAsync(object parameter)
        {
            DoPendingTask(SaveDealerExportProfile, parameter);
        }

        void SaveDealerExportProfile(object parameter)
        {
            if (CheckCanExecute())
            {
                var context = new vincontrolwarehouseEntities();
                foreach (var item in SelectedDealers)
                {
                    UpdateExportFeed(context, item);
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => _view.Close()));
            }
            else
            {
                MessageBox.Show("All filename has to be filled in.");
            }
        }


        private void UpdateExportFeed(vincontrolwarehouseEntities context, DealerExportSetting setting)
        {
            var exportfeed = context.datafeedlookups.FirstOrDefault(i => i.Id == setting.Id);
            if (exportfeed != null)
            {
                exportfeed.FileName = setting.FileName.Value;
                exportfeed.Discontinued = setting.Discontinued;
                context.SaveChanges();
                Tracking.Log(UserAction.Update, App.CurrentUser.Id, DateTime.Now, exportfeed.Id, string.Format("Export Profile:{0} Dealer:{1}", exportfeed.datafeedprofile.ProfileName, exportfeed.dealer.Name), ItemType.ExportProfileDealer);

                //_vm.Dealers.Add(dealer);
            }
        }

        void OpenFileNameDealerWindow(object parameter)
        {
            _currentItem = (DealerExportSetting)parameter;
            var window = new ExportFilenameFormatWindow(((DealerExportSetting)parameter).FileName, ExportType.Dealer, ((DealerExportSetting)parameter).DealerId, ProfileId);
            window.Closed += WindowClosed;
            window.Show();
        }

        void WindowClosed(object sender, EventArgs e)
        {
            var w = (ExportFilenameFormatWindow)sender;
            if (w.IsDirty)
            {
                _currentItem.SelectedFileName = _currentItem.FileName.Value;
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
                MessageBox.Show("All filename has to be filled in.");
            }
        }

        void Apply(object parameter)
        {

            SaveDealerExportProfile(parameter);

            var context = new vincontrolwarehouseEntities();
            var exportfeed = context.datafeedprofiles.FirstOrDefault(i => i.Id == ProfileId);
            if (exportfeed != null)
            {
                if (exportfeed.Discontinued == null || !exportfeed.Discontinued.Value)
                {
                    CreateTask();
                    Tracking.Log(UserAction.CreateTask, App.CurrentUser.Id, DateTime.Now, exportfeed.Id, string.Format("Export Profile:{0} Create Task", exportfeed.ProfileName), ItemType.ExportProfile);

                }
            }

            Application.Current.Dispatcher.BeginInvoke(new Action(() => _view.Close()));
        }

        private void CreateTaskAsync()
        {
            DoPendingTask(CreateTask);
        }

        private void CreateTask()
        {
            var context = new vincontrolwarehouseEntities();
            var exportfeed = context.datafeedprofiles.FirstOrDefault(i => i.Id == ProfileId);
            //var taskExecution = new TaskExecution();
            //if (exportfeed != null)
            //    taskExecution.CreateDailyTask("Export_" + exportfeed.ProfileName, Environment.CurrentDirectory.Replace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "vincontrol.DataFeedService") + "\\vincontrol.DataFeedService.exe", "Export " + exportfeed.Id, exportfeed.RunningTime ?? DateTime.MinValue, exportfeed.Frequency ?? 0, @System.Configuration.ConfigurationManager.AppSettings["DataFeedUserDomain"], System.Configuration.ConfigurationManager.AppSettings["DataFeedPasswordDomain"]);
            if (exportfeed != null)
            {
                DateTime dateTime = exportfeed.RunningTime ?? DateTime.MinValue;
                if (exportfeed.Discontinued == null || !exportfeed.Discontinued.Value)
                {
                    var request =
                        WebRequest.Create(
                            string.Format("http://{0}/VinHelper/CreateDataFeedTask/{1}/{2}/{3}/{4}/{5}",
                                          System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                          "Export_" + exportfeed.ProfileName, "Export " + exportfeed.Id, dateTime.Hour,
                                          dateTime.Minute
                                          , exportfeed.Frequency ?? 0));
                    // ReSharper disable AssignNullToNotNullAttribute
                    var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
                    // ReSharper restore AssignNullToNotNullAttribute
                    responseStream.ReadToEnd();
                    responseStream.Close();
                }
            }
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
                DataHelper.DownloadExportFile(parameter.ToString());
            }
        }


        private void PushManuallyAsync(object parameter)
        {
            if (CheckCanExecute())
            {
                DoPendingTask(PushManually, parameter);
            }
            else
            {
                MessageBox.Show("All filename has to be filled in.");
            }
        }

        private void PushManually(object parameter)
        {
            var request =
                       WebRequest.Create(
                           string.Format("http://{0}/VinHelper/ExportDataFeed/{1}",
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
            }
            else
            {
                RefreshTracking();
                Tracking.Log(UserAction.PushManually, App.CurrentUser.Id, DateTime.Now, 0, string.Format("Export Profile:{0}", ProfileId), ItemType.ExportProfile);

            }

        }

        private void RefreshTracking()
        {
            InitData();
            UpdateParentView();
        }

        private void UpdateParentView()
        {
            _currentvm.LastDepositedDate = SelectedDealers.Max(i => i.LastDepositedDate);
            if (IsBundle)
            {
                _currentvm.FileURL = SelectedDealers.Where(i => i.LastDepositedDate.Equals(_currentvm.LastDepositedDate)).Select(i => i.FileUrl).FirstOrDefault();
            }
        }

        void AssignDealerToExportProfileAsync(object parameter)
        {
            DoPendingTask(AssignDealerToExportProfile, parameter);
        }

        void AssignDealerToExportProfile(object parameter)
        {
            var context = new vincontrolwarehouseEntities();

            var dealer = (Dealer)parameter;
            var dealerinDataSource = context.datafeedlookups.FirstOrDefault(i => i.DealerId == dealer.Id && i.DataFeedProfileId == ProfileId);
            if (dealerinDataSource != null)
            {
                MessageBox.Show(string.Format("Dealer {0} already existed in profile {1}.", dealer.Name,
                                              dealerinDataSource.datafeedprofile.ProfileName));
            }
            else
            {
                var item = new datafeedlookup { DealerId = dealer.Id, DataFeedProfileId = ProfileId };
                context.AddTodatafeedlookups(item);
                context.SaveChanges();
                Tracking.Log(UserAction.CreateTask, App.CurrentUser.Id, DateTime.Now, item.Id, string.Format("Export Profile:{0} Dealer:{1}", item.datafeedprofile.ProfileName, item.dealer.Name), ItemType.ExportProfile);

                Application.Current.Dispatcher.BeginInvoke(new Action(() => SelectedDealers.Add(new DealerExportSetting
                {
                    Id = item.Id,
                    DealerId = dealer.Id,
                    DealerName = dealer.Name,
                    FileName = new FileNameFormat()
                })));
            }
        }

        void CreateFileAsync(object parameter)
        {
            if (CheckCanExecute())
            {
                DoPendingTask(CreateFile, parameter);
            }
            else
            {
                MessageBox.Show("All filename has to be filled in.");
            }
        }

        void CreateFile(object parameter)
        {
            //no direct interaction with the UI is allowed from this method
            var helper = new ExportConvertHelper();
            helper.TestExportToFile(ProfileId);
        }

        #endregion

        #region Members

        private DealerExportSetting _currentItem;

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

        public bool IsBundle
        {
            get { return _isBundle; }
            set
            {
                if (_isBundle != value)
                {
                    _isBundle = value;
                    base.OnPropertyChanged("IsBundle");
                }
            }
        }

        private RelayCommand _deleteExportDealerCommand;

        public RelayCommand DeleteExportDealerCommand
        {
            get { return _deleteExportDealerCommand ?? (_deleteExportDealerCommand = new RelayCommand(DeleteExportDealerAsync, null)); }
        }

        private RelayCommand _saveDealerExportProfileCommand;

        public RelayCommand SaveDealerExportProfileCommand
        {
            get
            {
                return _saveDealerExportProfileCommand ??
                       (_saveDealerExportProfileCommand = new RelayCommand(SaveDealerExportProfileAsync, null));
            }
        }


        private RelayCommand _openFileNameDealerWindowCommand;

        public RelayCommand OpenFileNameDealerWindowCommand
        {
            get { return _openFileNameDealerWindowCommand ?? (_openFileNameDealerWindowCommand = new RelayCommand(OpenFileNameDealerWindow, null)); }
        }

        private RelayCommand _applyCommand;

        public RelayCommand ApplyCommand
        {
            get
            {
                return _applyCommand ??
                       (_applyCommand = new RelayCommand(ApplyAsync, null));
            }
        }

        private readonly IEditNameView _view;
        public ObservableCollection<DealerExportSetting> SelectedDealers
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


        public List<Dealer> Dealers { get; set; }

        private RelayCommand _assignDealerToExportProfileCommand;
        private RelayCommand _createFileCommand;
        private ObservableCollection<DealerExportSetting> _selectedDealers;
        private bool _taskExisted;
        private bool _isBundle;
        private readonly ExportProfileViewModel _currentvm;
        private RelayCommand _pushManuallyCommand;
        private RelayCommand _downloadFileCommand;

        public int ProfileId { get; set; }

        public RelayCommand AssignDealerToExportProfileCommand
        {
            get { return _assignDealerToExportProfileCommand ?? (_assignDealerToExportProfileCommand = new RelayCommand(AssignDealerToExportProfileAsync, null)); }
        }

        public RelayCommand CreateFileCommand
        {
            get { return _createFileCommand ?? (_createFileCommand = new RelayCommand(CreateFileAsync, null)); }
        }

        public RelayCommand PushManuallyCommand
        {
            get { return _pushManuallyCommand ?? (_pushManuallyCommand = new RelayCommand(PushManuallyAsync, null)); }
        }

        public RelayCommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFileAsync, null)); }
        }

        #endregion

        public ViewExportDealerOfProfileViewModel(IEditNameView view, ExportProfileViewModel vm)
        {
            _view = view;
            _currentvm = vm;
            ProfileId = _currentvm.Id;
            InitDataAsync();

        }

        bool CheckCanExecute()
        {
            return SelectedDealers.All(dealerExportSetting => !String.IsNullOrEmpty(dealerExportSetting.SelectedFileName) || IsBundle);
        }

        #region Overrides of ViewModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
