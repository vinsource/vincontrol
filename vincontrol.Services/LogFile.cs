using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace vincontrol.Services
{
    public class LogFile
    {
        private string sLogFormat
        {
            get
            {
                //sLogFormat used to create log files format :
                // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
                return DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            }
        }
        private string sErrorTime
        {
            get
            {
                //this variable used to create log filename format "
                //for example filename : ErrorLogYYYYMMDD
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                return sYear + sMonth + sDay;
            }
        }

        private string _folderName;
        private string _fileName;

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
            StreamWriter sw = new StreamWriter(_folderName + "\\" + _fileName + sErrorTime+".txt", true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
    }
}
