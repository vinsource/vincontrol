using vincontrol.Backend.Models;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Import
{
    public class DeleteReplaceExpressionCommand : CommandBase
    {
        private readonly ReplaceListViewModel _vm;
        public DeleteReplaceExpressionCommand(ReplaceListViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Do(object parameter)
        {
            _vm.Replaces.Remove((ReplaceModel)parameter);
        }

        #endregion
    }
}
