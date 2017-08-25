using System.Linq;
using vincontrol.Backend.Controls;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.ViewModels;
using vincontrol.DataFeed.Helper;

namespace vincontrol.Backend.Commands.Import
{
    public class SaveMappingCommand : CommandBase
    {
        private readonly IncomingProfileTemplateViewModel _vm;

        public SaveMappingCommand(IncomingProfileTemplateViewModel vm)
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
            //if (_vm is IncomingProfileTemplateViewModel)
            //{
             

                
            //}
            //else
            //{
            //    var xmlHelper = new ExportXMLHelper();
            //    string xmlContent = xmlHelper.CreateMappingTemplate(MappingHelper.GetMappingViewModel(_vm));

            //    var context = new vincontrolwarehouseEntities();
            //    var firstPofile = context.exportdataprofilemappings.FirstOrDefault();
            //    if (firstPofile == null)
            //    {
            //        var exportFeedProfile = new exportdataprofilemapping()
            //                                    {
            //                                        Mapping = xmlContent
            //                                    };
            //        context.AddToexportdataprofilemappings(exportFeedProfile);
            //        context.SaveChanges();

            //        var feedProfile = new datafeedprofile()
            //        {
            //            CompanyName = "default",
            //            ExportDataProfileMapping_Id = exportFeedProfile.Id
            //        };

            //        context.AddTodatafeedprofiles(feedProfile);
            //        context.SaveChanges();

            //        var profileLookup = new datafeedlookup()
            //                                {
            //                                    DealerId = 1009,
            //                                    DataFeedProfileId = feedProfile.Id
            //                                };
            //        context.AddTodatafeedlookups(profileLookup);
            //        context.SaveChanges();
            //    }
            //    else
            //    {
            //        firstPofile.Mapping = xmlContent;
            //        context.SaveChanges();
            //    }
            //}
        }

        #endregion
    }
}
