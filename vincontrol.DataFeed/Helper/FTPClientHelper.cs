using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace vincontrol.DataFeed.Helper
{
    public class FTPClientHelper
    {
        private string _username;

        private string _password;

        private readonly string _host;

        private readonly int _port;

        public FTPClientHelper() {}

        public FTPClientHelper(int serverId)
        {
            switch (serverId)
            {
                case 0:
                    _username = System.Configuration.ConfigurationManager.AppSettings["CLAPPFTPUsername"];
                    _password = System.Configuration.ConfigurationManager.AppSettings["CLAPPFTPPassword"];
                    _port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CLAPPFTPPort"]);
                    _host = System.Configuration.ConfigurationManager.AppSettings["CLAPPFTPHost"];
                    break;
                case 1:
                    _username = System.Configuration.ConfigurationManager.AppSettings["VinFtpUser"];
                    _password = System.Configuration.ConfigurationManager.AppSettings["VinFtpPassword"];
                    _port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["VinFtpPort"]);
                    _host = System.Configuration.ConfigurationManager.AppSettings["VinFtpHost"];
                    break;
                case 2:
                    _username = System.Configuration.ConfigurationManager.AppSettings["FTPVinworldUsername"];
                    _password = System.Configuration.ConfigurationManager.AppSettings["FTPVinworldPassword"];
                    _port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FTPVinworldwPort"]);
                    _host = System.Configuration.ConfigurationManager.AppSettings["FTPVinworldHost"];
                    break;
            }
        }


        public byte[] DownloadData(string path, NetworkCredential clappCredential)
        {
            // Get the object used to communicate with the server.
            var request = new WebClient {Credentials = clappCredential};

            // Logon to the server using username + password

            byte[] data = request.DownloadData(BuildServerUri(path));

            request.Dispose();

            return data;
        }

        private Uri BuildServerUri(string path)
        {
            return new Uri(String.Format("ftp://{0}:{1}/{2}", _host, _port, path));
        }
    }
}
