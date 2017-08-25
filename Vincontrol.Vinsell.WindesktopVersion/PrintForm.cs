using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class PrintForm : Form
    {
        public PrintForm(string token, string vin, string manheimTrimId, string kbbTrimId)
        {
            InitializeComponent();
            manheimBrowser.IsScriptingEnabled = true;
            manheimBrowser.Enabled = true;
            manheimBrowser.IsAccessible = true;
            manheimBrowser.AllowDownloads = true;
            manheimBrowser.Navigate(string.Format("http://vinsell.com/auction/viewvehicleonwindow?token={0}&vin={1}&manheimTrimId={2}&kbbTrimId={3}",token, vin,manheimTrimId, kbbTrimId));
            //manheimBrowser.Navigated+=new WebBrowserNavigatedEventHandler(manheimBrowser_Navigated);
  
            
        }


      

        private void manheimBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            manheimBrowser.ShowPrintDialog();
        }

        //private void manheimBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        //{
        //    btnPrint.Enabled = true;
        //}


        //private void BtnPrintClick(object sender, EventArgs e)
        //{
        //  manheimBrowser.ShowPrintDialog();
        //}
    }
}
