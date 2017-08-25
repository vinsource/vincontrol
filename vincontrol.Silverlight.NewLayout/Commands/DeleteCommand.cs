using System;
using System.Windows.Input;
using vincontrol.Silverlight.NewLayout.ViewModels;

namespace vincontrol.Silverlight.NewLayout.Commands
{
    public class DeleteCommand : ICommand
    {
        private readonly UploadViewModel _vm;

        public DeleteCommand(UploadViewModel vm)
        {
            _vm = vm;
        }

        public void Execute(object parameter)
        {
            int count = _vm.Files.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                if (_vm.Files[i].IsMarkDeleted)
                {
                    _vm.Files.RemoveAt(i);
                }
            }
            
            if (DeleteCommandComplete != null)
                DeleteCommandComplete.Invoke(this, new EventArgs());
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler DeleteCommandComplete;
    }
}
