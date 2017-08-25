using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Import
{
    public class AddConditionExpressionCommand : CommandBase
    {
        private readonly ConditionListViewModel _vm;
        public AddConditionExpressionCommand(ConditionListViewModel vm)
            : base(null)
        {
            _vm = vm;
            //if (_vm.Conditions != null) _vm.Conditions.Add(new ConditionViewModel());

        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Do(object parameter)
        {
            _vm.Conditions.Add(new ConditionModel());
        }

        #endregion
    }
}
