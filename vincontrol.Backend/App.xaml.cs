using System.Collections.Generic;
using System.Windows;

namespace vincontrol.Backend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class App : Application
// ReSharper restore RedundantExtendsListEntry
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //if (this.DoHandle)
            //{
                //Handling the exception within the UnhandledException handler.
                MessageBox.Show(e.Exception.Message, "Exception Caught",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            //}
            //else
            //{
            //    //If you do not set e.Handled to true, the application will close due to crash.
            //    MessageBox.Show("Application is going to close! ", "Uncaught Exception");
            //    e.Handled = false;
            //}
        }

        public static User CurrentUser { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
    }
}
