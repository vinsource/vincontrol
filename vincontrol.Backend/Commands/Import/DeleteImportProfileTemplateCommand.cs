using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Import
{
    public class DeleteImportProfileTemplateCommand: CommandBase
    {
        private IncomingProfileManagementViewModel _vm;
        public DeleteImportProfileTemplateCommand(IncomingProfileManagementViewModel vm)
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
            var profile =(ProfileViewModel)parameter;
            var context = new vincontrolwarehouseEntities();
            _vm.Profiles.Remove((ProfileViewModel)parameter);
            var result = context.importdatafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
            if(result!=null)
            {
                context.DeleteObject(result);
                context.SaveChanges();
            }
        }

        #endregion
    }
}
