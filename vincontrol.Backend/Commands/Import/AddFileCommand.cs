using System.ComponentModel;
using System.Linq;
using Microsoft.Win32;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands.Import
{
    public class AddFileCommand : CommandBase
    {
        private ViewDealerOfProfileViewModel _vm;
        public AddFileCommand(ViewDealerOfProfileViewModel vm)
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
            //var convertHelper = new ConvertHelper();
            //var dlg = new OpenFileDialog { Filter = "All Files (*.*)|*.*", FilterIndex = 1, Multiselect = true };
            //var result = dlg.ShowDialog();
            //var dealer = (Dealer) parameter;
            //if (!result.Value) return;
            //foreach (var fileStream in dlg.OpenFiles())
            //{
            //    convertHelper.ImportFileToDatabase(dealer.Id, fileStream);
            //}

            //var importfeed = context.importdatafeedprofiles.FirstOrDefault(i => i.Id == _vm.ProfileId);
            //if (importfeed != null)
            //{

            //}

            var worker = new BackgroundWorker();
            //this is where the long running process should go
            worker.DoWork += (o, ea) =>
            {
                var context = new vincontrolwarehouseEntities();
                var dealer = (DealerImportSetting)parameter;
                var convertHelper = new ConvertHelper();
                convertHelper.ImportFileToDatabase(dealer.DealerId);
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                //work has completed. you can now interact with the UI
                _vm.IsBusy = false;
            };
            //set the IsBusy before you start the thread
            _vm.IsBusy = true;
            worker.RunWorkerAsync();
        }

        #endregion
    }
}
