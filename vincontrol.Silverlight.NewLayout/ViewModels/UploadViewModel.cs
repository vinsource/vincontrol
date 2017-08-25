using System;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Text;
using vincontrol.Silverlight.NewLayout.Commands;
using vincontrol.Silverlight.NewLayout.Helpers;
using vincontrol.Silverlight.NewLayout.Interfaces;
using vincontrol.Silverlight.NewLayout.Models;

namespace vincontrol.Silverlight.NewLayout.ViewModels
{
    public class UploadViewModel : INotifyPropertyChanged
    {
        private readonly IView _view;

        #region commands

        private AddCommand _addCommand;
        public AddCommand Add
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new AddCommand(this);
                    _addCommand.AddCommandComplete += _addCommand_AddCommandComplete;
                }

                return _addCommand;
            }
        }

        private SelectAllCommand _selectAllCommand;
        public SelectAllCommand SelectAll
        {
            get { return _selectAllCommand ?? (_selectAllCommand = new SelectAllCommand(this)); }
        }

        private CloseCommand _closeCommand;
        public CloseCommand Close
        {
            get { return _closeCommand ?? (_closeCommand = new CloseCommand(this)); }
        }


        private SaveCommand _saveCommand;
        public SaveCommand Save
        {
            get { return _saveCommand ?? (_saveCommand = new SaveCommand(this)); }
        }

        private StartAllCommand _startAll;
        public StartAllCommand StartAll
        {
            get
            {
                if (_startAll == null)
                {
                    _startAll = new StartAllCommand(this);
                    _startAll.StartAllCommandComplete += _startAll_StartAllCommandComplete;
                }

                return _startAll;
            }
        }



        private DeleteCommand _deleteCommand;
        public DeleteCommand Delete
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new DeleteCommand(this);
                    _deleteCommand.DeleteCommandComplete += _deleteCommand_DeleteCommandComplete;
                }

                return _deleteCommand;
            }
        }

        #endregion

        #region Properties

        private string _progressContext;
        public string ProgressContext
        {
            get { return _progressContext; }
            set
            {
                _progressContext = value;
                RaisePropertyChangedEvent("ProgressContext");
            }
        }

        private double _totalPercentUploaded;
        public double TotalPercentUploaded
        {
            get { return _totalPercentUploaded; }
            set
            {
                _totalPercentUploaded = value;
                RaisePropertyChangedEvent("TotalPercentUploaded");
            }
        }

        private long _totalNumberOfFiles;
        public long TotalNumberOfFiles
        {
            get { return _totalNumberOfFiles; }
            set
            {
                _totalNumberOfFiles = value;
                if (_totalNumberOfFiles != 0)
                {
                    TotalPercentUploaded = NumberOfUploadedFiles / _totalNumberOfFiles;
                }
                ProgressContext = string.Format("{0} of {1} files uploaded", NumberOfUploadedFiles, TotalNumberOfFiles);
                RaisePropertyChangedEvent("TotalNumberOfFiles");
            }
        }

        private double _numberOfUploadedFiles;
        public double NumberOfUploadedFiles
        {
            get { return _numberOfUploadedFiles; }
            set
            {
                _numberOfUploadedFiles = value;
                if (TotalNumberOfFiles != 0)
                {
                    TotalPercentUploaded = _numberOfUploadedFiles / TotalNumberOfFiles;
                }
                ProgressContext = string.Format("{0} of {1} files uploaded", NumberOfUploadedFiles, TotalNumberOfFiles);
                RaisePropertyChangedEvent("NumberOfUploadedFiles");
            }
        }


        private bool _isBusy;
        /// <summary>
        /// UI binds the busy indicator to this property
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChangedEvent("IsBusy");
                }
            }
        }

       

        private bool _isSelectAll;
        /// <summary>
        /// UI binds the busy indicator to this property
        /// </summary>
        public bool IsSelectAll
        {
            get { return _isSelectAll; }
            set
            {
                if (_isSelectAll != value)
                {
                    _isSelectAll = value;
                    RaisePropertyChangedEvent("IsSelectAll");
                }
            }
        }

        private bool _isOverlay = true;
        /// <summary>
        /// UI binds the busy indicator to this property
        /// </summary>
        public bool IsOverlay
        {
            get { return _isOverlay; }
            set
            {
                if (_isOverlay != value)
                {
                    _isOverlay = value;
                    RaisePropertyChangedEvent("IsOverlay");
                }
            }
        }



        #endregion

        #region Handlers

        void _deleteCommand_DeleteCommandComplete(object sender, EventArgs e)
        {
            _view.SetDataContext(this);
        }

        void _addCommand_AddCommandComplete(object sender, EventArgs e)
        {
            _view.SetDataContext(this);
        }

        void _startAll_StartAllCommandComplete(object sender, EventArgs e)
        {

        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(e.Result)))
            {

                var serializer = new DataContractJsonSerializer(typeof(List<ServerImage>));
                try
                {
                    var result = (List<ServerImage>)serializer.ReadObject(ms);

                    foreach (var url in result)
                    {
                        var file = new FileViewModel(this, 0, String.Empty, new BitmapImage(new Uri(url.FileUrl)))
                            {
                                ImageUrl = url.FileUrl,
                                ThumbnailImageUrl = url.ThumbnailUrl,
                                IsComplete = true,
                                IsOnServer = true,
                                Status = UploadStatus.Existed
                            };
                        Files.Add(file);
                    }
                }
                catch
                {
                }

                IsBusy = false;
            }
        }

        #endregion

        public ObservableCollection<FileViewModel> Files
        {
            get;
            set;
        }

        public UploadViewModel(IView view)
        {
            _view = view;
            IsBusy = true;
            Intialize();
            _view.SetDataContext(this);

        }

        private void Intialize()
        {
            var vehicleStatus = 0;Int32.TryParse(((App) Application.Current).InventoryStatus,out vehicleStatus);

            if (vehicleStatus == Constants.VehicleStatus.Inventory || vehicleStatus == Constants.VehicleStatus.SoldOut)
            {
                Files = new ObservableCollection<FileViewModel>();

                var uriBuilder = new UriBuilder(((App) Application.Current).ImageServiceURL);
                var r = new Random();
                uriBuilder.Query = string.Format("Function=GetImageUrlList&ListingId={0}&r={1}&VehicleStatus={2}",
                    ((App) Application.Current).ListingId, r.Next(),vehicleStatus);

                var client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(uriBuilder.Uri);
            }
            if (vehicleStatus == Constants.VehicleStatus.Appraisal)
            {
                Files = new ObservableCollection<FileViewModel>();

                var uriBuilder = new UriBuilder(((App) Application.Current).ImageServiceURL);
                var r = new Random();
                uriBuilder.Query = string.Format("Function=GetImageUrlList&AppraisalId={0}&r={1}&VehicleStatus={2}",
                    ((App)Application.Current).AppraisalId, r.Next(), vehicleStatus);
                
                var client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(uriBuilder.Uri);
            }
           
        }

        public void BindView()
        {
            _view.SetDataContext(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                if (!Deployment.Current.Dispatcher.CheckAccess())
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
                }
                else
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
