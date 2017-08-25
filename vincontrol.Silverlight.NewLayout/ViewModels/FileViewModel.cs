using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.IO;
using vincontrol.Silverlight.NewLayout.Commands;
using vincontrol.Silverlight.NewLayout.Helpers;

namespace vincontrol.Silverlight.NewLayout.ViewModels
{
    public class FileViewModel :  INotifyPropertyChanged
    {
        public UploadViewModel Parent { get; set; }

        public FileViewModel(UploadViewModel viewModel, long length, string fileName, BitmapImage imageSource)
        {
            Parent = viewModel;
            FileLength = length;
            FileName = Guid.NewGuid().ToString() + fileName;
            ImageSource = imageSource;
        }

        public FileViewModel(UploadViewModel viewModel, long length, string fileName, BitmapImage imageSource, FileInfo fileInfo)
            : this(viewModel, length, fileName, imageSource)
        {
            FileInfo = fileInfo;
        }

        private FileInfo _fileInfo;
        public FileInfo FileInfo
        {
            get { return _fileInfo; }
            set
            {
                _fileInfo = value;
                RaisePropertyChangedEvent("FileInfo");
            }
        }

        private UploadCommand _uploadCommand;
        public UploadCommand Upload
        {
            get
            {
                if (_uploadCommand == null)
                {
                    _uploadCommand = new UploadCommand(this);
                    _uploadCommand.UploadCommandComplete += _uploadCommand_UploadCommandComplete;
                }

                return _uploadCommand;
            }
        }

        void _uploadCommand_UploadCommandComplete(object sender, EventArgs e)
        {
            //this.Parent.BindView();
        }

        #region Properties

        private UploadStatus _status = UploadStatus.Uploading;
        public UploadStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                RaisePropertyChangedEvent("ImageUrl");
            }
        }

        private string _thumbnailImageUrl;
        public string ThumbnailImageUrl
        {
            get { return _thumbnailImageUrl; }
            set
            {
                _thumbnailImageUrl = value;
                RaisePropertyChangedEvent("ThumbnailImageUrl");
            }
        }     

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                RaisePropertyChangedEvent("ImageSource");
            }
        }       

        private string _filename;
        public string FileName
        {
            get { return _filename; }
            set
            {
                _filename = value;
                RaisePropertyChangedEvent("Filename");
            }
        }

        private long _fileLength;
        public long FileLength
        {
            get { return _fileLength; }
            set
            {
                _fileLength = value;
                RaisePropertyChangedEvent("FileLength");
            }
        }

        private bool _isOnServer;
        public bool IsOnServer
        {
            get { return _isOnServer; }
            set
            {
                _isOnServer = value;
                RaisePropertyChangedEvent("IsOnServer");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private double _percentUploaded;
        public double PercentUploaded
        {
            get { return _percentUploaded; }
            set
            {
                _percentUploaded = value;
                RaisePropertyChangedEvent("PercentUploaded");
            }
        }

        private long _localLength;
        public long LocalLength
        {
            get { return _localLength; }
            set
            {
                _localLength = value;
                RaisePropertyChangedEvent("LocalLength");
                PercentUploaded = _localLength / ((double)_fileLength);
            }
        }

        private bool _isFinish = true;
        /// <summary>
        /// UI binds the busy indicator to this property
        /// </summary>
        public bool IsFinish
        {
            get { return _isFinish; }
            set
            {
                _isFinish = value;
                RaisePropertyChangedEvent("IsFinish");
            }
        }

        private Boolean _isMarkDeleted;
        public Boolean IsMarkDeleted
        {
            get { return _isMarkDeleted; }
            set
            {
                _isMarkDeleted = value;
                RaisePropertyChangedEvent("IsMarkDeleted");
            }
        }

        private bool _isComplete;
        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                _isComplete = value;
                if (_isComplete)
                {
                    PercentUploaded = 1;
                }
                RaisePropertyChangedEvent("IsComplete");             
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                RaisePropertyChangedEvent("IsRunning");
            }
        }
        #endregion

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
