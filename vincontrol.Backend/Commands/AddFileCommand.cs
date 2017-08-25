using System;
using System.Windows.Input;
using Microsoft.Win32;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands
{
    public class AddFileCommand : ICommand
    {
        public AddFileCommand()
        {
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            var convertHelper = new ConvertHelper();
            var dlg = new OpenFileDialog { Filter = "All Files (*.*)|*.*", FilterIndex = 1, Multiselect = true };
            var result = dlg.ShowDialog();

            if (!result.Value) return;
            foreach (var fileStream in dlg.OpenFiles())
            {
                convertHelper.GetInventoriesFromFile(1, fileStream);
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
            return true;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
