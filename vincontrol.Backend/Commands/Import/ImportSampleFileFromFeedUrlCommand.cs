using Microsoft.Win32;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands.Import
{
    public class ImportSampleFileFromFeedUrlCommand : CommandBase
    {
        private const string Root = "public_html";
        private readonly IncomingProfileTemplateViewModel _vm;
        public ImportSampleFileFromFeedUrlCommand(IncomingProfileTemplateViewModel vm)
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
                if (!_vm.SchemaURL.Contains(Root))
                {
                    if (_vm.SchemaURL.Substring(0, 1).Equals("/") || _vm.SchemaURL.Substring(0, 1).Equals("\\"))
                    {
                        _vm.SchemaURL = Root + _vm.SchemaURL;
                    }
                    else
                    {
                        _vm.SchemaURL = Root + "\\" + _vm.SchemaURL;
                    }
                }
                var byteArray = FTPHelper.DownloadFromFtpServer(_vm.SchemaURL);

                foreach (var item in DataHelper.GetData(_vm.HasHeader, _vm.Delimiter, byteArray))
                {
                    if (!_vm.HasHeader)
                    {
                        item.XMLField = item.XMLField.Replace("Column", "");
                    }
                    _vm.Mappings.Add(item);
                }
            }

        }

        #endregion
    }
}
