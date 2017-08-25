using Microsoft.Win32;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Import
{
    public class ImportSampleFileCommand : CommandBase
    {
        private readonly IncomingProfileTemplateViewModel _vm;
        public ImportSampleFileCommand(IncomingProfileTemplateViewModel vm)
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
            if (_vm.PropertiesValid)
            {
                var dlg = new OpenFileDialog {Filter = "All Files (*.*)|*.*", FilterIndex = 1, Multiselect = false};
                var result = dlg.ShowDialog();

                if (!result.Value) return;

                for (int i = _vm.Mappings.Count - 1; i >= 0; i--)
                {
                    _vm.Mappings.RemoveAt(i);
                }

                foreach (var fileStream in dlg.OpenFiles())
                {
                    foreach (var item in DataHelper.GetData(_vm.HasHeader, _vm.Delimiter, fileStream))
                    {
                        if (!_vm.HasHeader)
                        {
                            item.XMLField = item.XMLField.Replace("Column", "");
                        }
                        _vm.Mappings.Add(item);
                    }
                }
            }
        }

        #endregion
    }
}
