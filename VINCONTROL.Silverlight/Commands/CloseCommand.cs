using System;
using System.Windows.Input;
using VINCONTROL.Silverlight.ViewModels;
using VINCONTROL.Silverlight.Helpers;

namespace VINCONTROL.Silverlight.Commands
{
    public class CloseCommand : ICommand
    {
        public CloseCommand(UploadViewModel vm)
        {
        }

        public void Execute(object parameter)
        {
            HtmlHelper.CloseForm();
        }

     
        public event EventHandler CanExecuteChanged;
        public event EventHandler CloseCommandComplete;


        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
