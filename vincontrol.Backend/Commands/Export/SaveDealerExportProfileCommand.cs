using System;
using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Commands.Import
{
    public class SaveDealerExportProfileCommand:CommandBase
    {
        private ViewExportDealerOfProfileViewModel _vm;
        public SaveDealerExportProfileCommand(ViewExportDealerOfProfileViewModel vm)
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
            var setting = (DealerExportSetting) parameter;
            var context = new vincontrolwarehouseEntities();
            foreach (var item in _vm.SelectedDealers)
            {
                UpdateExportFeed(context, item);
            }
           
        }

        private void UpdateExportFeed(vincontrolwarehouseEntities context, DealerExportSetting setting)
        {
            var exportfeed = context.datafeedlookups.FirstOrDefault(i => i.Id == setting.Id);
            if (exportfeed != null)
            {
                exportfeed.FileName = setting.FileName.Value;
                exportfeed.Discontinued = setting.Discontinued;
                context.SaveChanges();
                //_vm.Dealers.Add(dealer);
            }
        }


        #endregion
    }
}
