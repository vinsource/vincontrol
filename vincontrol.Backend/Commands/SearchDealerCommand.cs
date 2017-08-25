using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Data;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Commands
{
    public class SearchDealerCommand : CommandBase
    {
        private ViewDealerOfProfileViewModel _vm;
        public SearchDealerCommand(ViewDealerOfProfileViewModel vm):base(null)
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
            int result;
            int.TryParse(_vm.SearchContent, out result);
            var context = new vincontrolwarehouseEntities();
            var dealers =
                context.dealers.Where(i => i.Name.ToLower().Contains(_vm.SearchContent.ToLower()) || i.Id == result).Select(i => new Dealer() { Id = i.Id, Name = i.Name }).ToList();
            int count = _vm.SearchResultDealers.Count - 1;
            for (int i = count; i >= 0; i--)
            {
                _vm.SearchResultDealers.Remove(_vm.SearchResultDealers[i]);
            }
            foreach (var item in dealers)
            {
                _vm.SearchResultDealers.Add(item);
            }

        }

        #endregion
    }
}
