using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Starksoft.Net.Ftp;
using VINCapture.UploadImage.Helpers;
using VINCapture.UploadImage.USBHelpers;
using VINCapture.UploadImage.View;
using VINCapture.UploadImage.ViewModels;

namespace VINCapture.UploadImage.Commands
{
    public class SyncCommand : ICommand
    {
        #region Implementation of ICommand
        private bool _isEnable = true;

        private readonly DetailPageViewModel _vm;
        private int _currentNumber;

        //private FtpClient _ftpClient;

        public SyncCommand(DetailPageViewModel vm)
        {
           
           
            _vm = vm;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
             if (_vm.Vehicles != null)
            {
                foreach (var usbCarViewModel in _vm.Vehicles)
                {
                    usbCarViewModel.Upload.UploadCommandComplete += UploadUploadCommandComplete;

                }

                if (_vm.Vehicles.Any())
                {
                    _vm.Vehicles[0].Upload.Execute(null);
                }
            }
        }


        void UploadUploadCommandComplete(object sender, EventArgs e)
        {
            _currentNumber++;
            if (_currentNumber < _vm.Vehicles.Count)
            {
                _vm.Vehicles[_currentNumber].Upload.Execute(null);
            }
            else
            {
                _vm.ResultMessage = "The uploading process has finished.";
                _isEnable = false;
                _vm.IsEnableSync = false;

                EmailHelpers.SendEmailByWcfService(_vm.Vehicles, ((App)Application.Current).Dealer);

                if (_vm.IsPortableDevice)
                {
                    PortableDeviceHelper.DeleteFromWpdToHardDrive();
                    const string backupPortablePath = @"C:\\VincaptureTemporary";
                    
                    if (Directory.Exists(backupPortablePath))
                    {
                        FileHelper.DeleteFilesAndFoldersRecursively(backupPortablePath);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(_vm.DealerFolder) && Directory.Exists(_vm.DealerFolder))
                    {
                        FileHelper.DeleteFilesAndFoldersRecursively(_vm.DealerFolder);
                    }
                }

              

            
            }
        }

      

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter)
        {
            return _isEnable;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
