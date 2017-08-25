using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.ViewModels.Export;

namespace vincontrol.Backend.Commands
{
    public class SaveFileNameCommand : CommandBase
    {
        private ExportFilenameFormatViewModel _vm;
        public SaveFileNameCommand(ExportFilenameFormatViewModel vm):base(null)
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
            var firstItem = _vm.FileNameFormatList.FirstOrDefault(i => i.IsSelected);
            _vm.SelectedFileNameFormat.Value = _vm.OtherFileNameFormat.IsSelected
                ? _vm.OtherFileNameFormat.Value
                : (firstItem == null ? String.Empty : firstItem.Value);
        }

        #endregion
    }
}
