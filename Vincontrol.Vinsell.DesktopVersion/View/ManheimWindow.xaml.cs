using System;
using System.Web;
using System.Windows;
using Vincontrol.Vinsell.DesktopVersion.ViewModels;
using vincontrol.WPF.Helpers.Interface;

namespace Vincontrol.Vinsell.DesktopVersion.View
{
    /// <summary>
    /// Interaction logic for ManheimWindow.xaml
    /// </summary>
    public partial class ManheimWindow : Window, IView
    {
        private ManheimViewModel _vm;

        public ManheimWindow()
        {
            InitializeComponent();
            Loaded += ManheimWindowLoaded;
        }

        void ManheimWindowLoaded(object sender, RoutedEventArgs e)
        {
            myBrowser.Navigate(new Uri("http://vehicleinventorynetwork.com/test/chart/"));
            myBrowser.Navigated += MyBrowserNavigated;
            _vm = new ManheimViewModel(this);
        }

        void MyBrowserNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var vin = HttpUtility.ParseQueryString(e.Uri.Query).Get("vin");
            if (String.IsNullOrEmpty(vin)) return;
            var dealer = ((App)Application.Current).Dealer;
            _vm.LoadData(vin, dealer);
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        #endregion
        //private void btnExternal_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    Uri ui = new Uri(txtLoad.Text.Trim(), UriKind.RelativeOrAbsolute);
        //    myBrowser.Navigate(ui);
        //}
        //private void btnInternal_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    myBrowser.Navigate(new Uri("http://www.c-sharpcorner.com"));
        //}

        //private void InjectDisableScript()
        //{
        //    HTMLDocumentClass doc = myBrowser.Document as HTMLDocumentClass;
        //    HTMLDocument doc2 = Browser.Document as HTMLDocument;


        //    //Questo crea lo script per la soprressione degli errori
        //    IHTMLScriptElement scriptErrorSuppressed = (IHTMLScriptElement) doc2.createElement("SCRIPT");
        //    scriptErrorSuppressed.type = "text/javascript";
        //    scriptErrorSuppressed.text = DisableScriptError;

        //    IHTMLElementCollection nodes = doc.getElementsByTagName("head");

        //    foreach (IHTMLElement elem in nodes)
        //    {
        //        //Appendo lo script all'head cosi Ã¨ attivo

        //        HTMLHeadElementClass head = (HTMLHeadElementClass) elem;
        //        head.appendChild((IHTMLDOMNode) scriptErrorSuppressed);
        //    }
        //}
    }
}
