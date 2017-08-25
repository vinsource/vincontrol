using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VINCapture.UploadImage.Helpers
{
    public class ServiceLog
    {
        public static void ErrorImportLog(string sErrMsg)
        {
            const string pathFolderName = @"C:\VincontrolServiceLog\ImportLog\Log\";

            string newSubPath = System.IO.Path.Combine(pathFolderName, DateTime.Today.ToLongDateString());

            if (!Directory.Exists(newSubPath))

                System.IO.Directory.CreateDirectory(newSubPath);

            var sPathName = newSubPath + "//" + "ImportVinControlLog_" + "_" + DateTime.Today.Month + DateTime.Today.Day +
                            DateTime.Today.Year + ".txt";


            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            try
            {

                var sw = new StreamWriter(sPathName, true, Encoding.UTF8);
                sw.WriteLine(sLogFormat + sErrMsg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                var sw = new StreamWriter(sPathName, true, Encoding.UTF8);
                sw.WriteLine(sLogFormat + ex.Message);
                sw.WriteLine(sLogFormat + ex.InnerException);
                sw.WriteLine(sLogFormat + ex.TargetSite);
                sw.Flush();
                sw.Close();
            }


        }
    }
}
