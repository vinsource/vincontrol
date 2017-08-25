using System.ComponentModel;
using Microsoft.Win32;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands.Import
{
    public class CreateFileCommand : CommandBase
    {
        private ViewExportDealerOfProfileViewModel _vm;
        public CreateFileCommand(ViewExportDealerOfProfileViewModel vm)
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
            var worker = new BackgroundWorker();
            //this is where the long running process should go
            worker.DoWork += (o, ea) =>
            {
                //no direct interaction with the UI is allowed from this method
                var helper = new ExportConvertHelper();
                helper.ExportToFile(_vm.ProfileId);
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
