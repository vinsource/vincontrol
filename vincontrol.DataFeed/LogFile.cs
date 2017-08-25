using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace vincontrol.DataFeed
{
    public class LogFile
    {
        private static string LogFormat
        {
            get
            {
                return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ==> ";
            }
        }
        private static string ErrorTime
        {
            get
            {
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                return sYear + sMonth + sDay;
            }
        }

        private readonly string _folderName;
        private readonly string _fileName;

        public LogFile(string folderName, string fileName)
        {
            _folderName = folderName;
            _fileName = fileName;

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
        }

        public void ErrorLog(string sErrMsg)
        {
            var sw = new StreamWriter(_folderName + "\\" + _fileName + ErrorTime + ".txt", true);
            sw.WriteLine(LogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
    }
}
