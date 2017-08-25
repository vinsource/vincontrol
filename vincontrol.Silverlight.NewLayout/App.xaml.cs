using System;
using System.Windows;

namespace vincontrol.Silverlight.NewLayout
{
    public partial class App : Application
    {
        private string _imageServiceUrl = "";
        public string ImageServiceURL { get { return _imageServiceUrl; } }

        private string _dealerId = "";
        public string DealerId { get { return _dealerId; } }

        private string _listingId = "";
        public string ListingId { get { return _listingId; } }

        private string _appraisalId = "";
        public string AppraisalId { get { return _appraisalId; } }

        private string _inventoryStatus = "";
        public string InventoryStatus { get { return _inventoryStatus; } }

        private string _vin = "";
        public string Vin { get { return _vin; } }

        private string _user = "";
        public string User { get { return _user; } }

        public App()
        {
            Startup += this.Application_Startup;
            Exit += Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                _imageServiceUrl = e.InitParams["ImageServiceURL"];
            }
            catch
            { }

            try
            {
                _dealerId = e.InitParams["DealerID"];
            }
            catch
            { }

            try
            {
                _listingId = e.InitParams["ListingId"];
            }
            catch { }

            try
            {
                _appraisalId = e.InitParams["AppraisalId"];
            }
            catch { }

            try
            {
                _vin = e.InitParams["Vin"];
            }
            catch
            { }

             try
            {
                _inventoryStatus = e.InitParams["InventoryStatus"];
            }               
            catch (Exception)
            { }
             try
             {
                 _user = e.InitParams["User"];
             }
             catch (Exception)
             { }

            RootVisual = new MainPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(() => ReportErrorToDOM(e));
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
