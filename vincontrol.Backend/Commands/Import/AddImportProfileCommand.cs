using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Import
{
    public class AddImportProfileCommand : CommandBase
    {
        private readonly IncomingProfileManagementViewModel _vm;
        public AddImportProfileCommand(IncomingProfileManagementViewModel vm)
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
            var chldWindow = new AddImportProfileWindow(null,_vm);
            //chldWindow.Unloaded += ChldWindowUnloaded;
            chldWindow.ShowDialog();
        }

        //void ChldWindowUnloaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    var idList = _vm.Profiles.Select(i => i.Id);
        //    var context = new vincontrolwarehouseEntities();

        //   var result = context.importdatafeedprofiles.Where(i => !idList.Contains(i.Id)).Select(i => new ProfileViewModel {ProfileName = i.ProfileName,CompanyName = i.CompanyName, Id = i.Id}).ToList();
        //    foreach (var item in result)
        //    {
        //        _vm.Profiles.Add(item); 
        //    }
        //}

        #endregion
    }
}
