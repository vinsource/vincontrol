using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.Backend.Windows.DataFeed.Export;

namespace vincontrol.Backend.Commands.Export
{
    public class OpenFileNameDealerWindowCommand : CommandBase
    {
        private ViewExportDealerOfProfileViewModel _vm;
        public OpenFileNameDealerWindowCommand(ViewExportDealerOfProfileViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        #region Overrides of CommandBase
        public override void Do(object parameter)
        {
            var window = new ExportFilenameFormatWindow(((DealerExportSetting)parameter).FileName, ExportType.Dealer);
            window.Show();
        }

        #endregion
    }
}
