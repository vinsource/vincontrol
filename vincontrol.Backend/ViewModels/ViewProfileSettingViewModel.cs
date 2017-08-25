using vincontrol.Backend.Interface;

namespace vincontrol.Backend.ViewModels
{
    public class ViewProfileSettingViewModel
    {
        private readonly IEditView _view;
        public ViewProfileSettingViewModel(IEditView view)
        {
            _view = view;
            _view.SetDataContext(this);
        }

        public int ProfileId { get; set; }
    }
}
