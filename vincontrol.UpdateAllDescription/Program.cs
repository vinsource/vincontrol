using System;
using System.Configuration;
using System.Linq;
using vincontrol.Services;

namespace vincontrol.UpdateAllDescription
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var process = new UpdateAllDescriptionProcess();
                process.RunUsedCars();

                process.RunNewCars();
               
            }
            catch (Exception ex)
            {
                var log = new LogFile(ConfigurationManager.AppSettings["UpdateAllDescriptionLogFolder"], "UpdateAllDescriptionLogFolder");
                log.ErrorLog(ex.Message + " - " + ex.StackTrace);
            }
            
        }
    }
}
