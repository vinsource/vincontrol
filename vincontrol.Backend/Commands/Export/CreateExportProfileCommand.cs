using System;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Controls;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands.Export
{
    public class CreateExportProfileCommand : CommandBase
    {
        private readonly ExportProfileTemplateViewModel _vm;
        public CreateExportProfileCommand(ExportProfileTemplateViewModel vm)
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
            //if (_vm != null)
            //{
            //    var xmlHelper = new XMLHelper();
            //    var xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(_vm));
            //    string sampleDataXML = DataHelper.SerializeSampleData(_vm);
            //    var context = new vincontrolwarehouseEntities();
            //    var feedProfile = new importdatafeedprofile
            //    {
            //        CompanyName = _vm.ProfileName,
            //        Mapping = xmlContent,
            //        SampleData = sampleDataXML
            //    };

            //    context.AddToimportdatafeedprofiles(feedProfile);
            //    context.SaveChanges();
            //    _vm.ProfileId = feedProfile.Id;
            //    if (CreateProfileCommandComplete != null)
            //        CreateProfileCommandComplete.Invoke(this, new EventArgs());

            //}

            if (_vm != null)
            {
                var xmlHelper = new XMLHelper();
                var xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(_vm));
                //string sampleDataXML = DataHelper.SerializeSampleData(_vm);
                var context = new vincontrolwarehouseEntities();
                var feedProfile = new datafeedprofile
                {
                    Mapping = xmlContent,
                    Bundle = _vm.Bundle,
                    CompanyName = _vm.ProfileName,
                    DefaultPassword = _vm.FTPPassword,
                    DefaultUserName = _vm.FTPUserName,
                    FTPServer = _vm.FTPHost,
                    FileName = _vm.FileName.Value,
                    Frequency = _vm.Frequency,
                    RunningTime = _vm.RunningTime,
                    Discontinued = _vm.Discontinue,
                    ProfileName = _vm.ProfileName
                };

                context.AddTodatafeedprofiles(feedProfile);
                context.SaveChanges();
                _vm.ProfileId = feedProfile.Id;
               
            }
        }

      
        #endregion
    }
}
