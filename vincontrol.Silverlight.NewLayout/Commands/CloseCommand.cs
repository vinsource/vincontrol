using System;
using System.Windows.Input;
using vincontrol.Silverlight.NewLayout.Helpers;
using vincontrol.Silverlight.NewLayout.ViewModels;

namespace vincontrol.Silverlight.NewLayout.Commands
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
