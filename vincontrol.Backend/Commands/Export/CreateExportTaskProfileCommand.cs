using System;
using System.Linq;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Data;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.TaskScheduler;

namespace vincontrol.Backend.Commands.Export
{
    public class CreateExportTaskProfileCommand : CommandBase
    {
         private ViewExportDealerOfProfileViewModel _vm;
         public CreateExportTaskProfileCommand(ViewExportDealerOfProfileViewModel vm)
             : base(null)
        {
            _vm = vm;
        }

        #region Overrides of CommandBase

        public override void Do(object parameter)
        {
            var saveCommand = new SaveDealerExportProfileCommand(_vm);
            saveCommand.Execute(null);
            CreateTask();
        }

        private void CreateTask()
        {
            var context = new vincontrolwarehouseEntities();
            var exportfeed = context.datafeedprofiles.FirstOrDefault(i => i.Id == _vm.ProfileId);
            var taskExecution = new TaskExecution();
            if (exportfeed != null)
                taskExecution.CreateDailyTask("Export_" + exportfeed.ProfileName, Environment.CurrentDirectory.Replace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "vincontrol.DataFeedService") + "\\vincontrol.DataFeedService.exe", "Export " + exportfeed.Id, exportfeed.RunningTime ?? DateTime.MinValue, exportfeed.Frequency ?? 0, @System.Configuration.ConfigurationManager.AppSettings["DataFeedUserDomain"], System.Configuration.ConfigurationManager.AppSettings["DataFeedPasswordDomain"]);
        }

        #endregion
    }
}
