using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Commands.Import
{
    public class DeleteExportDealerCommand : CommandBase
    {
        private readonly ViewExportDealerOfProfileViewModel _vm;
        public DeleteExportDealerCommand(ViewExportDealerOfProfileViewModel vm)
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
            var context = new vincontrolwarehouseEntities();
            var dealer = (DealerExportSetting)parameter;
            var importfeed = context.datafeedlookups.FirstOrDefault(i => i.Id == dealer.Id);
            if (importfeed != null)
            {
                context.DeleteObject(importfeed);
                context.SaveChanges();
                //_vm.Dealers.Add(dealer);
                _vm.SelectedDealers.Remove(dealer);
            }
        }

        #endregion
    }
}
