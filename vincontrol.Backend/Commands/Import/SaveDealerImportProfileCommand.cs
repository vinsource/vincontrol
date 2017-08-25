using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Commands.Import
{
    public class SaveDealerImportProfileCommand:CommandBase
    {
        private ViewDealerOfProfileViewModel _vm;
        public SaveDealerImportProfileCommand(ViewDealerOfProfileViewModel vm)
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
            var setting = (DealerImportSetting) parameter;
            var context = new vincontrolwarehouseEntities();
            var importfeed = context.dealers_dealersetting.FirstOrDefault(i => i.Id == setting.Id);
            if (importfeed != null)
            {
                importfeed.ImportFeedUrl = setting.FeedUrl;
                importfeed.DiscontinuedImport = setting.Discontinued;
                context.SaveChanges();
                //_vm.Dealers.Add(dealer);
            } 
        }

        #endregion
    }
}
