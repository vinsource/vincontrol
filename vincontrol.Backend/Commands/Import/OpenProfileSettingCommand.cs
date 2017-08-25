using vincontrol.Backend.ViewModels;
using vincontrol.Backend.Windows.DataFeed.Import;

namespace vincontrol.Backend.Commands.Import
{
    public class OpenProfileSettingCommand : CommandBase
    {
        private ProfileViewModel _vm;
        public OpenProfileSettingCommand(ProfileViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        //private int _dealerId;
        //public OpenProfileSettingCommand(int dealerId)
        //{
        //    _dealerId = dealerId;
        //}

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Do(object parameter)
        {
            var chldWindow = new IncomingProfileTemplateMappingWindow(_vm);
            chldWindow.ShowDialog();
        }

        #endregion
    }
}
