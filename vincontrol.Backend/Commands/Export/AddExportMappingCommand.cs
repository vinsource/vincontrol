using System;
using System.Linq;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.Commands.Export
{
    public class AddExportMappingCommand : CommandBase
    {
        private readonly ExportProfileTemplateViewModel _vm;

        public AddExportMappingCommand(ExportProfileTemplateViewModel vm):base(null)
        {
            _vm = vm;
        }

        public override void Do(object parameter)
        {
            var item = new ProfileMappingViewModel();

            if (_vm.Mappings != null && _vm.Mappings.Any())
            {
                item.DBFields = _vm.Mappings[0].DBFields;
            }
            else
            {
                var columnNames = DataHelper.GetInventoryColumnNames();
                columnNames.Insert(0, new Column { Text = String.Empty, Value = String.Empty });
                item.DBFields = columnNames;
            }

            if (_vm.Mappings != null) _vm.Mappings.Add(item);
        }
    }
}
