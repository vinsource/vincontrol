using vincontrol.Backend.Commands.Import;

namespace vincontrol.Backend.ViewModels
{
    public class ProfileViewModel
    {
        private OpenProfileSettingCommand _openProfileSettingCommand;
        public OpenProfileSettingCommand OpenProfileSettingCommand
        {
            get { return _openProfileSettingCommand ?? (_openProfileSettingCommand = new OpenProfileSettingCommand(this)); }
        }

        private OpenDealershipCommand _openDealershipCommand;
        public OpenDealershipCommand OpenDealershipCommand
        {
            get { return _openDealershipCommand ?? (_openDealershipCommand = new OpenDealershipCommand(this)); }
        }

      

        

        public string Name { get; set; }
        public int Id { get; set; }
    }
}
