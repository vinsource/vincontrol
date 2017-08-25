using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Starksoft.Net.Ftp;
using vincontrol.Backend.Data;
using vincontrol.DataFeed.Model;

namespace vincontrol.DataFeed.Helper
{
    public class FTPHelper
    {
        public static FtpClient FtpClient;
        private static FTPClientHelper _ftpClientHelper;

        public static MemoryStream DownloadedFile { get; set; }
        
        public FTPHelper()
        {
            DownloadedFile = new MemoryStream();
        }

        #region Connect To FTP Server

        public static void ConnectToFtpServer(string companyName)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var exportProfile = context.datafeedprofiles.FirstOrDefault(i => i.CompanyName.ToLower().Equals(companyName.ToLower()));
                if (exportProfile == null) return;
                
                FtpClient = new FtpClient(exportProfile.FTPServer, 21);
                FtpClient.Open(exportProfile.DefaultUserName, exportProfile.DefaultPassword);
            }
        }

        public static void ConnectToFtpServer(string ftpHost, int ftpPort, string userName, string password)
        {
            FtpClient = new FtpClient(ftpHost, ftpPort);
            FtpClient.Open(userName, password);
        }

        public static void ConnectToFtpVinServer()
        {
            FtpClient = new FtpClient(System.Configuration.ConfigurationManager.AppSettings["VinFtpHost"], Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["VinFtpPort"]));

            FtpClient.Open(System.Configuration.ConfigurationManager.AppSettings["VinFtpUser"], System.Configuration.ConfigurationManager.AppSettings["VinFtpPassword"]);
        }

        #endregion

        public static void CloseConnectionToFtpSever()
        {
            FtpClient.Close();
        }

        public static void UploadToFtpServer(string localPath, string remotePath)
        {
            try
            {
                FtpClient.PutFile(localPath, remotePath, FileAction.Create);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                FtpClient.Close();
                FtpClient = null;
            }

        }

        public static void UploadToFtpServer(Stream inputStream, string remotePath)
        {
            try
            {
                FtpClient.PutFile(inputStream, remotePath, FileAction.Create);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                FtpClient.Close();
                FtpClient = null;
            }

        }

        public static void UploadToFtpServer(Stream inputStream, string remoteLocation, string remoteFileName)
        {
            try
            {
                FtpClient.ChangeDirectory(remoteLocation);
                
                FtpClient.PutFile(inputStream, remoteFileName, FileAction.Create);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                FtpClient.Close();
                FtpClient = null;
            }

        }

        public static void UploadToFtpServer(string subFolder, string localPath, string remotePath)
        {
            try
            {
                subFolder = "/" + subFolder;

                FtpClient.ChangeDirectory(subFolder);

                FtpClient.PutFile(localPath, remotePath, FileAction.Create);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                FtpClient.Close();
                FtpClient = null;
            }
        }

        public static byte[] DownloadFromFtpServer(string path)
        {
            try
            {
                //if (DownloadedFile == null)
                //    DownloadedFile = new MemoryStream();

                //FtpClient.GetFile(path, DownloadedFile, true);
                _ftpClientHelper = new FTPClientHelper(1);
                return _ftpClientHelper.DownloadData(path,
                                                     new NetworkCredential(
                                                         System.Configuration.ConfigurationManager.AppSettings["VinFtpUser"],
                                                         System.Configuration.ConfigurationManager.AppSettings["VinFtpPassword"]));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //FtpClient.Close();
                //FtpClient = null;
            }
        }

        public static void MakeDirectory(string remoteLocation)
        {
            try
            {
                FtpClient.MakeDirectory(remoteLocation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                FtpClient.Close();
                FtpClient = null;
            }
        }

        public static void StoreBackupFileOnLocal(string location, string fileName, byte[] fileToArray)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            string filename = Path.GetFileName(fileName);
            string fullPath = Path.Combine(location, filename);
            var temp = new FileInfo(fullPath);
            if (temp.Exists)
                temp.Delete();

            var fileToupload = new FileStream(Path.Combine(location, filename), FileMode.Create);

            //var fileToArray = CommonHelper.GetBytes(fileContent);
            fileToupload.Write(fileToArray, 0, fileToArray.Length);
            fileToupload.Close();
            fileToupload.Dispose();
        }
    }
}
