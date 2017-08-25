using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.Windows.DataFeed.Export;

namespace vincontrol.Backend.Commands.Export
{
    public class OpenFileNameWindowCommand:CommandBase
    {
        private ExportProfileTemplateViewModel _vm;
        public OpenFileNameWindowCommand(ExportProfileTemplateViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        #region Overrides of CommandBase

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Do(object parameter)
        {
            var window = new ExportFilenameFormatWindow(_vm.FileName, ExportType.Profile);
            window.Show();
        }

        #endregion
    }
}
