using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Net.FtpClient;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class BasicFTPClient
    {
        public string Username;

        public string Password;

        public string Host;

        public int Port;

        public FtpClient cl = null;

        public string keptFolder = null;



        public BasicFTPClient()
        {

            Username = System.Configuration.ConfigurationManager.AppSettings["UsernameIPage"].ToString();

            Password = System.Configuration.ConfigurationManager.AppSettings["PasswordIPage"].ToString();

            Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortIPage"].ToString());

            Host = System.Configuration.ConfigurationManager.AppSettings["HostIPage"].ToString();


            //Username = System.Configuration.ConfigurationManager.AppSettings["UsernameVinControl"].ToString();

            //Password = System.Configuration.ConfigurationManager.AppSettings["PasswordVinControl"].ToString();

            //Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortVinControl"].ToString());

            //Host = System.Configuration.ConfigurationManager.AppSettings["HostVinControl"].ToString();


            cl = new FtpClient(Username, Password, Host);

            //cl.Connect(Username, Password, Host, Port);



        }

        public void closeConnect()
        {
            cl.Disconnect();
        }

        public void connect()
        {
            cl.Connect(Username, Password, Host, Port);

        }


      

        private Uri BuildServerUri(string Path)
        {
            return new Uri(String.Format("ftp://{0}:{1}/{2}", Host, Port, Path));
        }

        /// <summary>
        /// This method downloads the given file name from the FTP server
        /// and returns a byte array containing its contents.
        /// Throws a WebException on encountering a network error.
        /// </summary>

        public byte[] DownloadData(string path)
        {
            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // Logon to the server using username + password
            request.Credentials = new NetworkCredential(Username, Password);

            return request.DownloadData(BuildServerUri(path));
        }

        /// <summary>
        /// This method downloads the FTP file specified by "ftppath" and saves
        /// it to "destfile".
        /// Throws a WebException on encountering a network error.
        /// </summary>
        public void DownloadFile(string ftppath, string destfile)
        {
            // Download the data
            byte[] Data = DownloadData(ftppath);

            // Save the data to disk
            FileStream fs = new FileStream(destfile, FileMode.Create);
            fs.Write(Data, 0, Data.Length);
            fs.Close();
        }

        /// <summary>
        /// Upload a byte[] to the FTP server
        /// </summary>
        /// <param name="path">Path on the FTP server (upload/myfile.txt)</param>
        /// <param name="Data">A byte[] containing the data to upload</param>
        /// <returns>The server response in a byte[]</returns>

        public void UploadData(string path, byte[] Data)
        {
            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // Logon to the server using username + password
            try
            {
                request.Credentials = new NetworkCredential(Username, Password);

                request.UploadData(BuildServerUri(path), Data);
            }
            catch (Exception ex)
            {
                //ServiceLog.ErrorLog("Exception is " + ex.Message);

            }
            finally
            {
                request.Dispose();
            }


        }

        /// <summary>
        /// Load a file from disk and upload it to the FTP server
        /// </summary>
        /// <param name="ftppath">Path on the FTP server (/upload/myfile.txt)</param>
        /// <param name="srcfile">File on the local harddisk to upload</param>
        /// <returns>The server response in a byte[]</returns>

        public void UploadFile(string ftppath, string srcfile)
        {
            // Read the data from disk
            FileStream fs = new FileStream(srcfile, FileMode.Open);
            byte[] FileData = new byte[fs.Length];

            int numBytesToRead = (int)fs.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = fs.Read(FileData, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            numBytesToRead = FileData.Length;
            fs.Close();

            // Upload the data from the buffer
            UploadData(ftppath, FileData);
        }

        private XmlDocument loadConfigDocumentGeneral()
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                string fullFileName = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];
                FileStream stream = File.Open(fullFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                doc.Load(stream);
                return doc;
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception("No configuration file found.", e);
            }
        }


        public void CreateDirectoryOnFTP(String inFTPServerAndPath, String inNewDirectory)
        {
            // Step 1 - Open a request using the full URI, ftp://ftp.server.tld/path/file.ext
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(inFTPServerAndPath + "/" + inNewDirectory);

            // Step 2 - Configure the connection request
            request.Credentials = new NetworkCredential(Username, Password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            // Step 3 - Call GetResponse() method to actually attempt to create the directory
            FtpWebResponse makeDirectoryResponse = (FtpWebResponse)request.GetResponse();
        }

        public void CreateDirectory(string Path)
        {
            FtpClient ftpClient = new FtpClient(Username, Password, Host);

            ftpClient.Connect(Username, Password, Host, Port);

            ftpClient.CreateDirectory(Path);

        }

        public void deleteDirectoryByPath(string path)
        {
            cl.SetWorkingDirectory(path);

            var list = cl.GetListing().AsEnumerable();

            //if (path.Equals(keptFolder))
            //{
            //    list = list.Where<FtpListItem>(t => t.Modify < DateTime.Today.AddDays(-7));
            //}

            foreach (FtpListItem item in list)
            {
                if (item.Type == FtpObjectType.File)
                {
                    cl.RemoveFile(item.Name);
                }

            }

            foreach (FtpListItem item in list)
            {

                string tmp = "/" + path + "/" + item.Name;

                if (item.Type == FtpObjectType.Directory)

                    deleteDirectoryByPath(tmp);


            }
            if (!path.Equals(keptFolder))
                cl.RemoveDirectory(path);


        }

        public void Upload(string folderPath, Stream stream, string Path, string FileName)
        {
            //string[] pathSub = Path.Split(new String[] { "/" }, StringSplitOptions.None);

            string workingDir = "/" + folderPath;

            cl.SetWorkingDirectory(folderPath);
            
            workingDir += "/" + Path;

            if (!cl.DirectoryExists(Path))
                cl.CreateDirectory(Path);

            cl.SetWorkingDirectory(workingDir);
            

            cl.Upload(stream, FileName);

        }

        public void Upload(Stream stream, string Path, string FileName)
        {
            string[] pathSub = Path.Split(new String[] { "/" }, StringSplitOptions.None);

            string workingDir = "/DealerImages";

            cl.SetWorkingDirectory("DealerImages");

            //foreach (string tmp in pathSub)
            //{
            //    workingDir += "/" + tmp;
            //    if (!cl.DirectoryExists(tmp))
            //        cl.CreateDirectory(tmp);

            //    cl.SetWorkingDirectory(workingDir);
            //}

            workingDir += "/" + Path;

            if (!cl.DirectoryExists(Path))
                cl.CreateDirectory(Path);

            cl.SetWorkingDirectory(workingDir);
            

            cl.Upload(stream, FileName);

        }


        public void UploadXML(Stream stream, string FileName)
        {
            cl = new FtpClient(Username, Password, Host);

            cl.Connect(Username, Password, Host, Port);

            cl.Upload(stream, FileName);


        }


    }
}

