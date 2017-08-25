using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Commands.Import
{
    public class DeleteImportDealerCommand: CommandBase
    {
        private readonly ViewDealerOfProfileViewModel _vm;
        public DeleteImportDealerCommand(ViewDealerOfProfileViewModel vm)
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
            var dealer = (DealerImportSetting)parameter;
            var importfeed = context.dealers_dealersetting.FirstOrDefault(i => i.Id == dealer.Id);
            if (importfeed != null)
            {
                importfeed.ImportDataFeedProfileId = null;
                context.SaveChanges();
                //_vm.Dealers.Add(dealer);
                _vm.SelectedDealers.Remove(dealer);
            }
        }

        #endregion
    }
}
