using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Vincontrol.Vinsell.DesktopVersion.View;
using vincontrol.DomainObject;

namespace Vincontrol.Vinsell.DesktopVersion
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DealerUser Dealer { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var login = new StartupWindow();
            login.ShowDialog();
        }
    }

   

    
}
