using System;
using System.Collections.Generic;
using System.Windows.Input;
using vincontrol.Backend.Data;
using vincontrol.Backend.ViewModels;
using vincontrol.DataFeed.Helper;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.Commands
{
    public class SaveImportMappingTemplateCommand : ICommand
    {
        private readonly IncomingProfileTemplateViewModel _vm;

        public SaveImportMappingTemplateCommand(IncomingProfileTemplateViewModel vm)
        {
            _vm = vm;
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            var xmlHelper = new XMLHelper();
            string xmlContent = xmlHelper.CreateMappingTemplate(GetMappingViewModel(_vm));
            var context = new vincontrolwarehouseEntities();
            var feedProfile = new importdatafeedprofile
                                  {
                Delimited = _vm.Delimeter,
                UseHeader = _vm.HasHeader,
                Mapping = xmlContent
            };

            context.AddToimportdatafeedprofiles(feedProfile);
            context.SaveChanges();

            var importFeed = new importfeed { DealerId = _vm.DealerId, ImportDataFeedProfileId = feedProfile.Id };
            context.AddToimportfeeds(importFeed);
            context.SaveChanges();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        #endregion

        private MappingViewModel GetMappingViewModel(IncomingProfileTemplateViewModel vm)
        {
            return new MappingViewModel
            {
                HasHeader = vm.HasHeader,
                Delimeter = vm.Delimeter,
                Mappings = GetMappingList()
            };
        }

        private IList<Mapping> GetMappingList()
        {
            var list = new List<Mapping>();
            foreach (var item in list)
            {
                list.Add(new Mapping
                             {
                    Conditions = item.Conditions,
                    DBField = item.DBField,
                    Expression = item.Expression,
                    Order = item.Order,
                    Replaces = item.Replaces,
                    XMLField = item.XMLField
                });
            }
            return list;
        }
    }
}
