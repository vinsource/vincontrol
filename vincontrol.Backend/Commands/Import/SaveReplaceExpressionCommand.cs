using System;
using System.Windows.Input;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands
{
    public class SaveReplaceExpressionCommand : ICommand
    {
        private readonly ReplaceListViewModel _vm;
        public SaveReplaceExpressionCommand(ReplaceListViewModel vm)
        {
            //_vm.ParentView = vm;
            _vm = vm;
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _vm.Replaces.Add(new ReplaceViewModel(_vm));
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
