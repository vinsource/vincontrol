using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            bgWorker.RunWorkerAsync();
          
        }

        public byte[] DownloadData(string path, NetworkCredential ClappCredential)
        {
            // Get the object used to communicate with the server.
            var request = new WebClient { Credentials = ClappCredential };

            // Logon to the server using username + password

            byte[] data = request.DownloadData(BuildServerUri(path));

            request.Dispose();

            return data;
        }


        private Uri BuildServerUri(string Path)
        {
            return new Uri(String.Format("ftp://{0}:{1}/{2}", ConfigurationManager.AppSettings["DatafeedFTPHost"].ToString(CultureInfo.InvariantCulture), 21, Path));
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string baseDirectory =
                ConfigurationManager.AppSettings["baseDirectory"].ToString(CultureInfo.InvariantCulture);

            try
            {
                if (Directory.Exists(baseDirectory))

                    Directory.Delete(baseDirectory, true);


                var ftpDatafeedCredential = new NetworkCredential(ConfigurationManager.AppSettings["DatafeedFTPUsername"].ToString(CultureInfo.InvariantCulture),
                    ConfigurationManager.AppSettings["DatafeedFTPPassword"].ToString(CultureInfo.InvariantCulture));

                byte[] data = DownloadData("/VinsellDesktop/VinsellDesktop.rar", ftpDatafeedCredential);


                using (Stream stream = new MemoryStream(data))
                {
                    var reader = ReaderFactory.Open(stream);
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            reader.WriteEntryToDirectory(baseDirectory,
                                ExtractOptions.ExtractFullPath |
                                ExtractOptions.Overwrite);
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException + ex.Message);
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
