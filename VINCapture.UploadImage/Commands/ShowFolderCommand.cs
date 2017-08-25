using System;
using System.Windows.Forms;
using System.Windows.Input;
using VINCapture.UploadImage.ViewModels;

namespace VINCapture.UploadImage.Commands
{
    public class ShowFolderCommand:ICommand
    {
        #region Implementation of ICommand

        private readonly DetailPageViewModel _vm;

        public ShowFolderCommand(DetailPageViewModel vm)
        {
            _vm = vm;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            //if (_vm.DoBackup)
            //{
                var dialog = new FolderBrowserDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _vm.BackupPath = dialog.SelectedPath;
                }
            //}
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
            return true;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}