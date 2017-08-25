using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace Vincontrol.Vinsell.WindesktopVersion
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
                    ConfigurationManager.AppSettings["runFilePath"].ToString(
                        CultureInfo.InvariantCulture);
                Process.Start(executeFile);


            }
            ////Application.Run(new PrintForm(TODO));
            //if (DialogResult.OK == new StartLoginForm().ShowDialog())
            //{
            //    Application.Run(new ManheimForm());
            //}
        }
    }
}
