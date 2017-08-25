using System;
using System.Linq;
using vincontrol.Backend.Controls;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands.Import
{
    public class SaveExportMappingCommand : CommandBase
    {
        private readonly ExportProfileTemplateViewModel _vm;

        public SaveExportMappingCommand(ExportProfileTemplateViewModel vm)
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
            var xmlHelper = new ExportXMLHelper();
            string xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(_vm));
            var context = new vincontrolwarehouseEntities();

            var firstPofile = context.datafeedprofiles.FirstOrDefault(i=>i.Id == _vm.ProfileId);
          
            if (firstPofile != null)
            {
                firstPofile.Mapping = xmlContent;
                firstPofile.Bundle = _vm.Bundle;
                firstPofile.CompanyName = _vm.ProfileName;
                firstPofile.DefaultPassword = _vm.FTPPassword;
                firstPofile.DefaultUserName = _vm.FTPUserName;
                firstPofile.FTPServer = _vm.FTPHost;
                firstPofile.FileName = _vm.FileName.Value;
                firstPofile.Frequency = _vm.Frequency;
                firstPofile.RunningTime = _vm.RunningTime;
                firstPofile.Discontinued = _vm.Discontinue;
                firstPofile.ProfileName = _vm.ProfileName;
                //firstPofile.UseHeader 
                context.SaveChanges();
            }
        }


        #endregion
    }
}
