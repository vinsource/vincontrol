using System;
using vincontrol.Backend.Controls;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands.Import
{
    public class CreateProfileCommand : CommandBase
    {
        private readonly IncomingProfileTemplateViewModel _vm;
        public CreateProfileCommand(IncomingProfileTemplateViewModel vm)
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
            if (_vm != null)
            {
                var xmlHelper = new XMLHelper();
                var xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(_vm));
                string sampleDataXML = DataHelper.SerializeSampleData(_vm);
                var context = new vincontrolwarehouseEntities();
                var feedProfile = new importdatafeedprofile
                {
                    CompanyName = _vm.ProfileName,
                    Mapping = xmlContent,
                    SampleData = sampleDataXML,
                    //FeedUrl = _vm.FileURL,
                    Frequency = _vm.Frequency,
                    RunningTime = _vm.RunningTime,
                    Discontinued = _vm.Discontinue,
                    SchemaURL = _vm.SchemaURL,
                    ProfileName = _vm.ProfileName,
                    UseMappingInCode = _vm.UseSpecificMapping
                };

                context.AddToimportdatafeedprofiles(feedProfile);
                context.SaveChanges();
                _vm.ProfileId = feedProfile.Id;
            }
        }

      

        #endregion
    }
}
