using vincontrol.Backend.Models;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Import
{
    public class AddReplaceExpressionCommand : CommandBase
    {
        private readonly ReplaceListViewModel _vm;
        public AddReplaceExpressionCommand(ReplaceListViewModel vm)
            : base(null)
        {
            _vm = vm;
            //if (_vm.Replaces != null && !_vm.Replaces.Any()) _vm.Replaces.Add(new ReplaceViewModel());
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Do(object parameter)
        {
            _vm.Replaces.Add(new ReplaceModel());
        }

        #endregion
    }
}
