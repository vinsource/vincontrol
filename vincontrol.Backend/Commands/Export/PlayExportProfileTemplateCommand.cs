using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Data;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Export
{
    public class PlayExportProfileTemplateCommand : CommandBase
    {
        private ExportProfileManagementViewModel _vm;
        public PlayExportProfileTemplateCommand(ExportProfileManagementViewModel vm)
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

            var profile = (ExportProfileViewModel)parameter;
            var context = new vincontrolwarehouseEntities();
            //_vm.Profiles.Remove((ExportProfileViewModel)parameter);
            var result = context.datafeedprofiles.FirstOrDefault(i => profile.Id == i.Id);
            if (result != null)
            {
                result.Discontinued = false;
                context.SaveChanges();
                profile.Discontinued = false;
            }
        }
        #endregion
    }

       

}
