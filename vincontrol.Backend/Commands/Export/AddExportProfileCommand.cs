using System.Linq;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Data;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Export
{
    public class AddExportProfileCommand : CommandBase
    {
        private readonly ExportProfileManagementViewModel _vm;
        public AddExportProfileCommand(ExportProfileManagementViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        #region Implementation of ICommand

        public override void Do(object parameter)
        {
            var chldWindow = new AddExportProfileWindow(null,_vm);
            //chldWindow.Unloaded += ChldWindowUnloaded;
            chldWindow.ShowDialog();
        }

        //void ChldWindowUnloaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    var idList = _vm.Profiles.Select(i => i.Id);
        //    var context = new vincontrolwarehouseEntities();

        //    var result = context.datafeedprofiles.Where(i => !idList.Contains(i.Id)).Select(i => new ExportProfileViewModel { Name = i.CompanyName, Id = i.Id }).ToList();
        //    foreach (var item in result)
        //    {
        //        _vm.Profiles.Add(item);
        //    }
        //}

        #endregion
    }
}
