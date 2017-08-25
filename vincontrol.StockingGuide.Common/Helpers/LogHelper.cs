using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace vincontrol.StockingGuide.Common.Helpers
{
    public static class LogHelper
    {
        public static void InitLogger(ILog logEngine)
        {
            //string logFile = System.IO.Path.Combine(GetDesktopApplicationExecutingAssemblyFolderAbsolutePath(),
            //             ConfigurationManager.AppSettings["LogConfigFile"]);
            string logFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (System.IO.File.Exists(logFile))
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(logFile));
            }
        }

        private static string GetDesktopApplicationExecutingAssemblyFolderAbsolutePath()
        {
            string executingAssemblyFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo executingAssemblyFolder = new DirectoryInfo(executingAssemblyFolderPath);
            return executingAssemblyFolder.FullName;
        }
    }
}
