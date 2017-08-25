using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Vincontrol.VinsellDesktopDownloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new LoadingForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                form.Close();
                string executeFile =
                    ConfigurationManager.AppSettings["baseDirectory"].ToString(CultureInfo.InvariantCulture) +
                    System.Configuration.ConfigurationManager.AppSettings["runFilePath"].ToString(
                        CultureInfo.InvariantCulture);
                Process.Start(executeFile);

            }
        }
    }
}
