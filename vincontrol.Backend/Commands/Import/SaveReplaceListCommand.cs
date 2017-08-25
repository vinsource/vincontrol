using System;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Import
{
    public class SaveReplaceListCommand : CommandBase
    {
        private readonly ReplaceListViewModel _vm;

        public SaveReplaceListCommand(ReplaceListViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Do(object parameter)
        {
            //if (_vm.AllPropertiesValid)
            //{
                if (_vm.ParentViewModel == null)
                {
                    _vm.ParentViewModel = new ProfileMappingViewModel();
                }
                _vm.ParentViewModel.Replaces = _vm;
            //}
        }

        #endregion

     
    }
}
