using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.Windows.DataFeed.Import.PopupChildWindow;

namespace vincontrol.Backend.Commands.Export
{
    public class OpenExpressionWindowCommand : CommandBase
    {
        private readonly ProfileMappingViewModel _vm;
        //private int _dealerId;
        public OpenExpressionWindowCommand(ProfileMappingViewModel vm)
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
            var chldWindow = new ExpressionWindow(_vm);
            chldWindow.Show();
        }

        #endregion
    }
}
