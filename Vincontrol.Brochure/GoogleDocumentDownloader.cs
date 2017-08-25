using System;
using System.Data.OleDb;
using System.IO;
using System.Net;
using log4net;

namespace Vincontrol.Brochure
{
    public class GoogleDocumentDownloader
    {
        #region Logging
        private static readonly ILog log4netEngine = LogManager.GetLogger(typeof(GoogleDocumentDownloader));
        static Properties.Settings _AppSettings = Properties.Settings.Default;

        static void ConfigureLogging()
        {
            string logFile = System.IO.Path.Combine(GetDesktopApplicationExecutingAssemblyFolderAbsolutePath(),
                                          _AppSettings.Log4netConfigFile);
            if (System.IO.File.Exists(logFile))
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(logFile));
            }
        }

        public static string GetDesktopApplicationExecutingAssemblyFolderAbsolutePath()
        {
            string executingAssemblyFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo executingAssemblyFolder = new DirectoryInfo(executingAssemblyFolderPath);
            return executingAssemblyFolder.FullName;
        }
        #endregion Logging

        public void DownloadDoc()
        {
            ConfigureLogging();
            ReadExcel();
        }

        public static void ReadExcel()
        {
            string fileName = @"C:\Brochures\Brochure.xlsx";
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName +
                             ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\";";

            using (OleDbConnection connection = new OleDbConnection(strConn))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    log4netEngine.Error("Start reading excel");
                    while (dr.Read())
                    {
                        if (dr[0].ToString() != "Year")
                        {
                            string year = dr[0].ToString();
                            string make = dr[1].ToString();
                            string model = dr[2].ToString();
                            string link = dr[3].ToString();
                            try
                            {
                                if (!string.IsNullOrEmpty(link))
                                {
                                    link = link.Replace("https://docs.google.com/a/vincontrol.com/file/d/", "");
                                    string id = link.Split('/')[0];
                                    string url = string.Format("https://docs.google.com/uc?export=download&id={0}", id);
                                    DownloadFile(url, year, make, model);
                                    log4netEngine.Error(
                                        string.Format(
                                            "download year: {0} make: {1} model: {2} url: {3} successful",
                                            year, make, model,
                                            link
                                            ));
                                }
                            }
                            catch (Exception ex)
                            {
                                log4netEngine.Error(
                                    string.Format(
                                        "error when download year: {0} make: {1} model: {2} url: {3}, with message: {4}",
                                        year, make, model,
                                        link,
                                        ex.Message));
                            }
                        }
                    }
                    log4netEngine.Error("End reading excel");
                }
            }
        }

        public static string DownloadFile(String downloadUrl,string year, string make, string model)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(downloadUrl));
                //auth.ApplyAuthenticationToRequest(request);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                if (!Directory.Exists(string.Format(@"C:\Temp\{0}",year)))
                {
                    Directory.CreateDirectory(string.Format(@"C:\Temp\{0}", year));
                }
                if (!Directory.Exists(string.Format(@"C:\Temp\{0}\{1}", year,make)))
                {
                    Directory.CreateDirectory(string.Format(@"C:\Temp\{0}\{1}", year, make));
                }
                if (!Directory.Exists(string.Format(@"C:\Temp\{0}\{1}\{2}", year,make,model)))
                {
                    Directory.CreateDirectory(string.Format(@"C:\Temp\{0}\{1}\{2}", year, make, model));
                }
                using (Stream s = File.Create(string.Format(@"C:\Temp\{0}\{1}\{2}\{0}_{1}_{2}.pdf", year, make, model)))
                {
                    stream.CopyTo(s);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return result;
        }
    }
}
