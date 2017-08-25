using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Windows.DataFeed.Export;

namespace vincontrol.Backend.ViewModels.Export
{
    public class ExportProfileManagementViewModel : ViewModelBase
    {
        private void ViewExportHistory(object parameter)
        {
            var chldWindow = new ViewExportHistoryWindow((ExportProfileViewModel)parameter);
            chldWindow.ShowDialog();
        }

        void AddExportProfile(object parameter)
        {
            var chldWindow = new AddExportProfileWindow(null, this);
            chldWindow.ShowDialog();
        }

        #region Manage Data

        private void DownloadFileAsync(object parameter)
        {
            DoPendingTask(DownloadFile, parameter);
        }

        private void DownloadFile(object obj)
        {
            if (obj != null)
            {
                DataHelper.DownloadExportFile(obj.ToString());
            }
        }

        protected void InitializeAsync()
        {
            DoPendingTask(Initialize);
            _view.SetDataContext(this);
        }

        protected void Initialize()
        {

            var context =
                new vincontrolwarehouseEntities();
            var result =
                from h in context.exportservicehistories
                where h.Status == "Completed"
                group h by h.DatafeedProfileId
                    into g
                    select
                        g.OrderByDescending(i => i.RunningDate)
                        .FirstOrDefault();

            Profiles = new ObservableCollection
                <ExportProfileViewModel>(
                (from p in context.datafeedprofiles
                 join r in result
                     on p.Id equals r.DatafeedProfileId
                     into g
                 from i in g.DefaultIfEmpty()
                 select
                     new ExportProfileViewModel
                         {
                             ProfileName = p.ProfileName,
                             CompanyName = p.CompanyName,
                             Id = p.Id,
                             Discontinued =
                                 p.Discontinued ?? false,
                             FileURL = i.ArchiveFileName,
                             LastDepositedDate =
                                 i.RunningDate,
                             IsBundle = p.Bundle ?? false
                         }).ToList());
        }

        void DeleteExportProfileTemplateAsync(object parameter)
        {
            DoPendingTask(DeleteExportProfileTemplate, parameter);
        }

        void DeleteExportProfileTemplate(object parameter)
        {
            MessageBoxResult messageResult = MessageBox.Show("Do you want to delete this profile?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageResult == MessageBoxResult.Yes)
            {
                var profile = (ExportProfileViewModel)parameter;

                Application.Current.Dispatcher.BeginInvoke(new Action(() => Profiles.Remove((ExportProfileViewModel)parameter)));
                var context = new vincontrolwarehouseEntities();
                var result = context.datafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
                if (result != null)
                {
                    int id = result.Id;
                    string name = result.ProfileName;
                    DataHelper.RemoveExportTaskScheduler(result.ProfileName);

                    context.DeleteObject(result);
                    context.SaveChanges();

                    Tracking.Log(UserAction.Delete, App.CurrentUser.Id, DateTime.Now, id, string.Format("Profile:{0}", name), ItemType.ExportProfile);

                }
            }
        }

        void PlayExportProfileTemplateAsync(object parameter)
        {
            DoPendingTask(PlayExportProfileTemplate, parameter);
        }

        void PlayExportProfileTemplate(object parameter)
        {
            MessageBoxResult messageResult = MessageBox.Show("Do you want to continue this profile?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageResult == MessageBoxResult.Yes)
            {
                var profile = (ExportProfileViewModel)parameter;
                var context = new vincontrolwarehouseEntities();
                //_vm.Profiles.Remove((ExportProfileViewModel)parameter);
                var result = context.datafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
                if (result != null)
                {
                    DataHelper.UpdateExportTask(false, result.ProfileName, result.RunningTime, result.Id, result.Frequency ?? 0);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                              {
                                                                                  result.Discontinued = false;
                                                                                  context.SaveChanges();
                                                                                  profile.Discontinued = false;
                                                                              }));
                    Tracking.Log(UserAction.Continue, App.CurrentUser.Id, DateTime.Now, result.Id, string.Format("Profile:{0}", result.ProfileName), ItemType.ExportProfile);

                
                }
            }
        }

        void PauseExportProfileTemplateAsync(object parameter)
        {
            DoPendingTask(PauseExportProfileTemplate, parameter);
        }

        void PauseExportProfileTemplate(object parameter)
        {
            MessageBoxResult messageResult = MessageBox.Show("Do you want to discontinue this profile?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageResult == MessageBoxResult.Yes)
            {
                var profile = (ExportProfileViewModel)parameter;
                var context = new vincontrolwarehouseEntities();
                //_vm.Profiles.Remove((ExportProfileViewModel)parameter);
                var result = context.datafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
                if (result != null)
                {
                    DataHelper.UpdateExportTask(true, result.ProfileName, result.RunningTime, result.Id, result.Frequency ?? 0);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                              {
                                                                                  result.Discontinued = true;
                                                                                  context.SaveChanges();
                                                                                  profile.Discontinued = true;
                                                                              }));
                    Tracking.Log(UserAction.Pause, App.CurrentUser.Id, DateTime.Now, result.Id, string.Format("Profile:{0}", result.ProfileName), ItemType.ExportProfile);

                }
            }
        }

        #endregion

        #region Members

        public ObservableCollection<ExportProfileViewModel> Profiles
        {
            get { return _profiles; }
            set
            {
                if (_profiles != value)
                {
                    _profiles = value;
                    base.OnPropertyChanged("Profiles");
                }
            }
        }

        private RelayCommand _deleteExportProfileTemplateCommand;
        public RelayCommand DeleteExportProfileTemplateCommand
        {
            get { return _deleteExportProfileTemplateCommand ?? (_deleteExportProfileTemplateCommand = new RelayCommand(DeleteExportProfileTemplateAsync, null)); }
        }

        private RelayCommand _playExportProfileTemplateCommand;
        public RelayCommand PlayExportProfileTemplateCommand
        {
            get { return _playExportProfileTemplateCommand ?? (_playExportProfileTemplateCommand = new RelayCommand(PlayExportProfileTemplateAsync, null)); }
        }

        private RelayCommand _pauseExportProfileTemplateCommand;
        public RelayCommand PauseExportProfileTemplateCommand
        {
            get { return _pauseExportProfileTemplateCommand ?? (_pauseExportProfileTemplateCommand = new RelayCommand(PauseExportProfileTemplateAsync, null)); }
        }

        private RelayCommand _addExportProfileCommand;

        public RelayCommand AddExportProfileCommand
        {
            get { return _addExportProfileCommand ?? (_addExportProfileCommand = new RelayCommand(AddExportProfile, null)); }
        }

        private RelayCommand _viewExportHistoryCommand;

        public RelayCommand ViewExportHistoryCommand
        {
            get { return _viewExportHistoryCommand ?? (_viewExportHistoryCommand = new RelayCommand(ViewExportHistory, null)); }
        }

        private RelayCommand _downloadFileCommand;

        public RelayCommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFileAsync, null)); }
        }
        #endregion

        private readonly IView _view;
        private ObservableCollection<ExportProfileViewModel> _profiles;

        public ExportProfileManagementViewModel(IView view)
        {
            _view = view;
            InitializeAsync();
        }

        #region Overrides of ModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
