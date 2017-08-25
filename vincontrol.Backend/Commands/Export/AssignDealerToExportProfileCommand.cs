using vincontrol.Backend.Data;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;
using System.Linq;

namespace vincontrol.Backend.Commands.Import
{
    public class AssignDealerToExportProfileCommand : CommandBase
    {
        private readonly ViewExportDealerOfProfileViewModel _vm;
        public AssignDealerToExportProfileCommand(ViewExportDealerOfProfileViewModel vm)
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
            var context = new vincontrolwarehouseEntities();
         
            var dealer = (Dealer) parameter;

            bool flag =true;

            foreach (var tmp in _vm.SelectedDealers)
            {
                if (tmp.DealerId == dealer.Id)
                    flag = false;
            }
            if (flag)
            {

                var item = new datafeedlookup() {DealerId = dealer.Id, DataFeedProfileId = _vm.ProfileId};
                context.AddTodatafeedlookups(item);
                context.SaveChanges();
                _vm.SelectedDealers.Add(new DealerExportSetting()
                    {
                        Id = item.Id,
                        DealerId = dealer.Id,
                        DealerName = dealer.Name,
                        FileName = new FileNameFormat()
                    });
            }
        }

        #endregion
    }
}
