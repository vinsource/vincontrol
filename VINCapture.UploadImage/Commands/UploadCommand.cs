using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Starksoft.Net.Ftp;
using VINCapture.UploadImage.Helpers;
using VINCapture.UploadImage.USBHelpers;
using VINCapture.UploadImage.View;
using VINCapture.UploadImage.ViewModels;
using vincontrol.Data.Model;

namespace VINCapture.UploadImage.Commands
{
    public class ServerFile
    {
        public string FileUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }

    public class UploadCommand : ICommand
    {
        public List<ServerFile> FileUrLs { get; set; }
        private string[] _imageExtensions = new []{ ".jpg", ".jpeg", ".png", ".gif" };

        #region Implementation of ICommand

        private readonly USBCarViewModel _vm;
        public event EventHandler UploadCommandComplete;
        private FileInfo[] _fileList;
        //private FtpClient _ftpClient;
        private List<MemoryStream> _streamList;

        public UploadCommand(USBCarViewModel vm)
        {
            _vm = vm;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            //_ftpClient = (FtpClient)parameter;

            FileUrLs = new List<ServerFile>();
            _streamList=new List<MemoryStream>();
            _fileList = new DirectoryInfo(_vm.PhysicalFolderPath).GetFiles().Where(i => _imageExtensions.Contains(i.Extension.ToLower())).ToArray();

            if (!Directory.Exists(_vm.DestinationBackupFolder))
            {
                Directory.CreateDirectory(_vm.DestinationBackupFolder);
                
            }

            var workertranaction = new BackgroundWorker();
            workertranaction.DoWork += WorkertranactionDoWork;
            workertranaction.RunWorkerCompleted += workertranaction_RunWorkerCompleted;
            workertranaction.RunWorkerAsync();
        }

        void workertranaction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void WorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var fileInfo in _fileList)
            {
                try
                {
                    //Checking valid extension
                    if (!_imageExtensions.Contains(fileInfo.Extension.ToLower())) continue;

                    UploadFile(fileInfo, _vm.Vin);


                    fileInfo.CopyTo(_vm.DestinationBackupFolder + "\\" + fileInfo.Name, true);
                    _vm.UploadedFileNumber++;

                    SaveImageUrl();
                    UploadCommandComplete(null, null);
                }
                catch (Exception ex)
                {

                    ServiceLog.ErrorImportLog(
                        "********************************ERROR*****************************************");
                    ServiceLog.ErrorImportLog("EXCEPTION IS " + ex.Message);
                    ServiceLog.ErrorImportLog("TARGET IS " + ex.TargetSite);
                    ServiceLog.ErrorImportLog("SOURCE IS " + ex.Source);
                    ServiceLog.ErrorImportLog("INNER EXCEPTION IS " + ex.InnerException);
                    ServiceLog.ErrorImportLog("STACK TRACE " + ex.StackTrace);
                }
                
            }

            //try
            //{
            //    Directory.Delete(_vm.PhysicalFolderPath, true);

            //}
            //catch (Exception)
            //{

            //}
        } 

        private void SaveImageUrl()
        {
            using (var dbContext = new VincontrolEntities())
            {
                var row =
                    dbContext.Inventories.FirstOrDefault(x => x.InventoryId == _vm.ListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.PhotoUrl))
                        row.PhotoUrl = GetFileUrLs();
                    else
                    {
                        string[] carImagesList = row.PhotoUrl.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (carImagesList.First().Contains(GetWebURL()))
                        {
                            if (carImagesList.First().Contains("DefaultStockImage"))
                                row.PhotoUrl = GetFileUrLs(); 
                            else
                                row.PhotoUrl = row.PhotoUrl + "," + GetFileUrLs();
                        }
                        else
                        {
                            row.PhotoUrl = GetFileUrLs();
                        }
                    }

                    if (String.IsNullOrEmpty(row.ThumbnailUrl))
                    {
                        row.ThumbnailUrl = GetThumbnailFileUrLs();
                    }
                    else
                    {
                        string[] carImagesList = row.ThumbnailUrl.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (carImagesList.First().Contains(GetWebURL()))
                        {
                            if (carImagesList.First().Contains("DefaultStockImage"))
                                row.ThumbnailUrl = GetThumbnailFileUrLs();
                            else
                                row.ThumbnailUrl = row.ThumbnailUrl + "," + GetThumbnailFileUrLs();
                        }
                        else
                        {
                            row.ThumbnailUrl = GetThumbnailFileUrLs();
                        }

                    }

                    row.LastUpdated = DateTime.Now;

                    dbContext.SaveChanges();

                }
            }
        }

        private string GetThumbnailFileUrLs()
        {
            var result = FileUrLs.Aggregate(string.Empty, (current, serverFile) => current + (serverFile.ThumbnailUrl + ","));
            return !String.IsNullOrEmpty(result) ? result.Substring(0, result.Length - 1) : result;
        }

        private string GetFileUrLs()
        {
            var result = FileUrLs.Aggregate(string.Empty, (current, serverFile) => current + (serverFile.FileUrl + ","));
            return !String.IsNullOrEmpty(result) ? result.Substring(0, result.Length - 1) : result;
        }


        private void UploadFile(FileInfo fileInfo, string vin)
        {
            var url = new ServerFile();
            
            var stream = ImageHelpers.OverlayImage(fileInfo.FullName, ((App)Application.Current).Dealer);
            if (stream != null)
            {

                var fileGuid = Guid.NewGuid().ToString();
               
                var request =
                         WebRequest.Create(
                             string.Format("http://vincontrol.com:4411/vincapture/UploadSingleImage/{0}/{1}/{2}/{3}", fileInfo.Extension, fileGuid, vin, ((App)Application.Current).Dealer.DealerId));

                request.ContentType = "application/octet-stream";
                
                request.Method = WebRequestMethods.Http.Post;

                var requestStream = request.GetRequestStream();
                
                stream.CopyTo(requestStream);

                var requestResponse= request.GetResponse();

                requestResponse.Close();

                requestStream.Close();

                stream.Close();

                url.FileUrl = GetWebURL() + ConstructUrlForNormalSizeImage(((App)Application.Current).Dealer.DealerId,vin,fileGuid,fileInfo.Extension);
                url.ThumbnailUrl = GetWebURL() + ConstructUrlForThumbnailSizeImage(((App)Application.Current).Dealer.DealerId, vin, fileGuid, fileInfo.Extension);
                
            }
            else
            {
                var fileGuid = Guid.NewGuid().ToString();
                var fileStream = fileInfo.OpenRead();
               
                var request =
                                        WebRequest.Create(
                                            string.Format("http://vincontrol.com:4411/vincapture/UploadSingleImage/{0}/{1}/{2}/{3}", fileInfo.Extension, fileGuid, vin, ((App)Application.Current).Dealer.DealerId));

                request.ContentType = "application/octet-stream";

                request.Method = WebRequestMethods.Http.Post;

                var requestStream = request.GetRequestStream();

                fileStream.CopyTo(requestStream);

                var requestResponse = request.GetResponse();

                requestResponse.Close();

                requestStream.Close();

                fileStream.Close();

                url.FileUrl = GetWebURL() + ConstructUrlForNormalSizeImage(((App)Application.Current).Dealer.DealerId, vin, fileGuid, fileInfo.Extension);
                url.ThumbnailUrl = GetWebURL() + ConstructUrlForThumbnailSizeImage(((App)Application.Current).Dealer.DealerId, vin, fileGuid, fileInfo.Extension);

            }

            FileUrLs.Add(url);
        }

        private string ConstructUrlForNormalSizeImage(int dealerId, string vin, string fileName, string fileExtension)
        {
            return "DealerImages/" + dealerId + "/" + vin + "/" + "NormalSizeImages" + "/" + fileName + fileExtension;
        }

        private string ConstructUrlForThumbnailSizeImage(int dealerId, string vin, string fileName, string fileExtension)
        {
            return "DealerImages/" + dealerId + "/" + vin + "/" + "ThumbnailSizeImages" + "/" + fileName + fileExtension;
        }

        private FolderURL MakeFolder(FtpClient ftpClient)
        {
            string vinFolder = String.Format("{0}/{1}", ((App) Application.Current).Dealer.DealerId, _vm.Vin);
            string normalSizeImages = vinFolder + "/NormalSizeImages";
            var thumbnailSizeImages = vinFolder + "/ThumbnailSizeImages";

            try
            {
                ftpClient.MakeDirectory(normalSizeImages);
            }
            catch (Exception)
            {
            }

            try
            {
                ftpClient.MakeDirectory(thumbnailSizeImages);
            }
            catch (Exception)
            {
            }
            return new FolderURL {NormalSizeImages = normalSizeImages, ThumbnailSizeImages = thumbnailSizeImages};
        }

        private string GetWebURL()
        {
            return ConfigurationManager.AppSettings["WebURL"];
        }

        private static MemoryStream GetResizedStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var result = ImageResizer.ImageBuilder.Current.Build(stream, new ImageResizer.ResizeSettings("maxwidth=260&maxheight=260&format=jpg"));
            var resizedStream = new MemoryStream();
            result.Save(resizedStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            resizedStream.Position = 0;
            return resizedStream;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }

    public class FolderURL
    {
        public string NormalSizeImages { get; set; }
        public string ThumbnailSizeImages { get; set; }
    }
}