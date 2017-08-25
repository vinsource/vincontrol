using System;
using System.Linq;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Data;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.TaskScheduler;

namespace vincontrol.Backend.Commands.Export
{
    public class CreateTaskProfileCommand : CommandBase
    {
         private ViewDealerOfProfileViewModel _vm;
         public CreateTaskProfileCommand(ViewDealerOfProfileViewModel vm)
             : base(null)
        {
            _vm = vm;
        }

        #region Overrides of CommandBase

        public override void Do(object parameter)
        {
            var saveCommand = new SaveDealerImportProfileCommand(_vm);
            saveCommand.Execute(null);
            CreateTask();
        }

        private void CreateTask()
        {
            var context = new vincontrolwarehouseEntities();
            var exportfeed = context.importdatafeedprofiles.FirstOrDefault(i => i.Id == _vm.ProfileId);
            var taskExecution = new TaskExecution();
            if (exportfeed != null)
                taskExecution.CreateDailyTask("Import_" + exportfeed.ProfileName, Environment.CurrentDirectory.Replace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "vincontrol.DataFeedService") + "\\vincontrol.DataFeedService.exe", "Import " + exportfeed.Id, exportfeed.RunningTime ?? DateTime.MinValue, exportfeed.Frequency ?? 0, @System.Configuration.ConfigurationManager.AppSettings["DataFeedUserDomain"], System.Configuration.ConfigurationManager.AppSettings["DataFeedPasswordDomain"]);
        }

        #endregion
    }
}
