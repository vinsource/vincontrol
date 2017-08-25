using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vincontrol.Vinsell.WindesktopVersion.Helper;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class CarfaxForm : Form
    {
        public CarfaxForm()
        {
            InitializeComponent();
        }

        public CarfaxForm(string vin)
        {
            InitializeComponent();
            string carFaxUrl =
                "http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=" + SessionVar.CurrentDealer.DealerSetting.CarFaxUsername + "&vin="+vin ;

            carfaxBrowser.AllowNewWindows = true;
            carfaxBrowser.AllowNavigation = true;
            
            carfaxBrowser.NewWindowCreated += new WebKit.NewWindowCreatedEventHandler(carfaxBrowser_NewWindowCreated);
            carfaxBrowser.Navigate(carFaxUrl);
            
        }
        void carfaxBrowser_NewWindowCreated(object sender, WebKit.NewWindowCreatedEventArgs e)
        {

            var popupBrowser = e.WebKitBrowser;
            popupBrowser.Show();
        }

    }
}
