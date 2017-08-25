using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Windows.DataFeed.Import;

namespace vincontrol.Backend.ViewModels.Import
{
    public class IncomingProfileManagementViewModel : ViewModelBase
    {
        #region Manage Data

        protected void InitializeAsync()
        {
            DoPendingTask(Initialize);
            _view.SetDataContext(this);
        }

        protected void Initialize()
        {
            var context = new vincontrolwarehouseEntities();

            var result = from h in context.importservicehistories
                         where h.Status == "Completed"
                         group h by h.ImportProfileId into g
                         select g.OrderByDescending(i => i.RunningDate).FirstOrDefault();

            Profiles = new ObservableCollection<ProfileViewModel>(
               (from p in context.importdatafeedprofiles
                join r in result
                on p.Id equals r.ImportProfileId into g
                from i in g.DefaultIfEmpty()
                join c in context.importdatafeedprofilefiles
                on p.Id equals c.ImportDataFeedProfileId into g1
                from re in g1.DefaultIfEmpty()
                select new ProfileViewModel { ProfileName = p.ProfileName, CompanyName = p.CompanyName, Id = p.Id, Discontinued = p.Discontinued ?? false, FileURL = i.ArchiveFileName, LastDepositedDate = i.RunningDate, ProfileStatus = re.Status, UseSpecificMapping = p.UseMappingInCode??false}).ToList())
                          ;
        }


        public void AddImportProfile(object parameter)
        {
            var chldWindow = new AddImportProfileWindow(null, this);
            //chldWindow.Unloaded += ChldWindowUnloaded;
            chldWindow.ShowDialog();
        }

        public void DeleteImportProfileTemplateAsync(object parameter)
        {
            DoPendingTask(DeleteImportProfileTemplate, parameter);

        }

        public void DeleteImportProfileTemplate(object parameter)
        {
            MessageBoxResult messageResult = MessageBox.Show("Do you want to delete this profile?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageResult == MessageBoxResult.Yes)
            {
                var profile = (ProfileViewModel)parameter;
                var context = new vincontrolwarehouseEntities();
                Application.Current.Dispatcher.BeginInvoke(new Action(() => Profiles.Remove((ProfileViewModel)parameter)));
                var result = context.importdatafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
                if (result != null)
                {
                    int id = result.Id;
                    string name = result.ProfileName;
                    DataHelper.RemoveImportTaskScheduler(result.ProfileName);

                    context.DeleteObject(result);
                    context.SaveChanges();
                    Tracking.Log(UserAction.Delete, App.CurrentUser.Id, DateTime.Now, id, string.Format("Profile:{0}", name), ItemType.ImportProfile);
                   
                }
            }
        }

        public void PlayProfileTemplateAsync(object parameter)
        {
            DoPendingTask(PlayProfileTemplate, parameter);
        }


        public void PlayProfileTemplate(object parameter)
        {
            MessageBoxResult messageResult = MessageBox.Show("Do you want to continue this profile?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageResult == MessageBoxResult.Yes)
            {
                var profile = (ProfileViewModel)parameter;
                var context = new vincontrolwarehouseEntities();
                //_vm.Profiles.Remove((ExportProfileViewModel)parameter);
                var result = context.importdatafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
                if (result != null)
                {
                    DataHelper.UpdateImportTask(false, result.ProfileName, result.RunningTime, result.Id, result.Frequency ?? 0);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                             {
                                                                                 result.Discontinued = false;
                                                                                 context.SaveChanges();
                                                                                 profile.Discontinued = false;
                                                                             }));
                    Tracking.Log(UserAction.Continue, App.CurrentUser.Id, DateTime.Now, result.Id, string.Format("Profile:{0}", result.ProfileName), ItemType.ImportProfile);

                
                }
            }
        }

        public void PauseProfileTemplateAsync(object parameter)
        {
            DoPendingTask(PauseProfileTemplate, parameter);
        }

        public void PauseProfileTemplate(object parameter)
        {
            MessageBoxResult messageResult = MessageBox.Show("Do you want to discontinue this profile?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageResult == MessageBoxResult.Yes)
            {
                var profile = (ProfileViewModel)parameter;
                var context = new vincontrolwarehouseEntities();
                //_vm.Profiles.Remove((ExportProfileViewModel)parameter);
                var result = context.importdatafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
                if (result != null)
                {
                    DataHelper.UpdateImportTask(true, result.ProfileName, result.RunningTime, result.Id, result.Frequency ?? 0);
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                              {
                                                                                  result.Discontinued = true;
                                                                                  context.SaveChanges();
                                                                                  profile.Discontinued = true;
                                                                              }));

                    Tracking.Log(UserAction.Pause, App.CurrentUser.Id, DateTime.Now, result.Id, string.Format("Profile:{0}", result.ProfileName), ItemType.ImportProfile);

                }
            }
        }

        #endregion

        public IncomingProfileManagementViewModel(IView view)
        {
            _view = view;
            UserPermission = new Permission { Roles = new List<string> { "Editor" } };
            InitializeAsync();
            //_view.SetDataContext(this);
        }

        public ObservableCollection<ProfileViewModel> Profiles
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

        private void DownloadFileAsync(object parameter)
        {
            DoPendingTask(DownloadFile, parameter);
        }

        private void DownloadFile(object obj)
        {
            if (obj != null)
            {
                DataHelper.DownloadImportFile(obj.ToString());
            }
        }

        private RelayCommand _downloadFileCommand;

        public RelayCommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFileAsync, null)); }
        }

        private RelayCommand _addImportProfileCommand;

        public RelayCommand AddImportProfileCommand
        {
            get { return _addImportProfileCommand ?? (_addImportProfileCommand = new RelayCommand(AddImportProfile, null)); }
        }

        private RelayCommand _deleteImportProfileTemplateCommand;
        public RelayCommand DeleteImportProfileTemplateCommand
        {
            get { return _deleteImportProfileTemplateCommand ?? (_deleteImportProfileTemplateCommand = new RelayCommand(DeleteImportProfileTemplateAsync, null)); }
        }

        private RelayCommand _playProfileTemplateCommand;
        public RelayCommand PlayProfileTemplateCommand
        {
            get { return _playProfileTemplateCommand ?? (_playProfileTemplateCommand = new RelayCommand(PlayProfileTemplateAsync, null)); }
        }

        private RelayCommand _pauseProfileTemplateCommand;
        public RelayCommand PauseProfileTemplateCommand
        {
            get { return _pauseProfileTemplateCommand ?? (_pauseProfileTemplateCommand = new RelayCommand(PauseProfileTemplateAsync, null)); }
        }

        public Permission UserPermission
        {
            get { return _userPermission; }
            set
            {
                _userPermission = value;
                OnPropertyChanged("UserPermission");
            }
        }

        private RelayCommand _attachDescriptionCommand;
         public RelayCommand AttachDescriptionCommand
        {
            get { return _attachDescriptionCommand ?? (_attachDescriptionCommand = new RelayCommand(AttachDescription, null)); }
        }

        private void AttachDescription(object obj)
        {
            var item = (ProfileViewModel)obj;
            var window = new AttachDescriptionWindow(item);
            window.Show();
        }


        private readonly IView _view;
        private ObservableCollection<ProfileViewModel> _profiles;
        private Permission _userPermission;

        public String ProfileStatus { get; set; }

        protected override string GetValidationError(string property)
        {
            throw new System.NotImplementedException();
        }
    }
}
