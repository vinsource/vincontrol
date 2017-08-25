using System;
using System.Windows.Input;
using VINCONTROL.Silverlight.ViewModels;

namespace VINCONTROL.Silverlight.Commands
{
    public class SelectAllCommand : ICommand
    {
        private readonly UploadViewModel _vm;

        public SelectAllCommand(UploadViewModel vm)
        {
            _vm = vm;
        }

        public void Execute(object parameter)
        {
            foreach (var file in _vm.Files)
            {
                file.IsMarkDeleted = _vm.IsSelectAll;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler SelectAllCommandComplete;
    }
}
