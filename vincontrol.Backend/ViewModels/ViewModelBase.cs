using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.ViewModels
{
    public abstract class ViewModelBase : ModelBase
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    base.OnPropertyChanged("IsBusy");
                }
            }
        }

        public void DoPendingTask(Action<object> function, object parameter)
        {
            var worker = new BackgroundWorker();
            //this is where the long running process should go
            worker.DoWork += (o, ea) =>
            {
                try
                {
                    function(parameter);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Exception Caught",
                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                //work has completed. you can now interact with the UI
                IsBusy = false;
            };
            //set the IsBusy before you start the thread
            IsBusy = true;
            worker.RunWorkerAsync();
        }

        public void DoPendingTask(Action function)
        {
            var worker = new BackgroundWorker();
            //this is where the long running process should go
            worker.DoWork += (o, ea) =>
                                 {
                                     try
                                     {
                                         function();
                                     }
                                     catch (Exception e)
                                     {
                                         MessageBox.Show(e.Message, "Exception Caught",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                                     }
                                 };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                //work has completed. you can now interact with the UI
                IsBusy = false;
            };
            //set the IsBusy before you start the thread
            IsBusy = true;
            worker.RunWorkerAsync();
        }
    }
}
