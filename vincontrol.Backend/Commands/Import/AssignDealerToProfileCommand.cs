using vincontrol.Backend.Data;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;
using System.Linq;

namespace vincontrol.Backend.Commands.Import
{
    public class AssignDealerToProfileCommand : CommandBase
    {
        private readonly ViewDealerOfProfileViewModel _vm;
        public AssignDealerToProfileCommand(ViewDealerOfProfileViewModel vm)
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
            var importfeed = context.dealers_dealersetting.FirstOrDefault(i => i.Id == dealer.Id);
            if (importfeed != null)
            {
                importfeed.ImportDataFeedProfileId = _vm.ProfileId;
                context.SaveChanges();
                //_vm.Dealers.Add(dealer);
                _vm.SelectedDealers.Add(new DealerImportSetting(){ DealerName = dealer.Name, DealerId =  dealer.Id, Id = importfeed.Id});
            }
        }

        #endregion
    }
}
