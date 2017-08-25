using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.Serialization.Json;
using vincontrol.Silverlight.NewLayout.Helpers;
using vincontrol.Silverlight.NewLayout.Models;
using vincontrol.Silverlight.NewLayout.ViewModels;

namespace vincontrol.Silverlight.NewLayout.Commands
{
    public class UploadCommand : ICommand
    {
        private readonly FileViewModel _vm;

        public UploadCommand(FileViewModel vm)
        {
            _vm = vm;
        }

        private void StartNextUpload()
        {
            _vm.Parent.StartAll.DownloadingNumber--;
            _vm.Parent.StartAll.Execute(null);
        }

        public void Execute(object parameter)
        {
            var uriBuilder = new UriBuilder(((App)Application.Current).ImageServiceURL);
            var r = new Random();
            uriBuilder.Query = string.Format("uploadedfile={0}&DealerId={1}&ListingId={2}&Vin={3}&Overlay={4}&r={5}&AppraisalId={6}", _vm.FileName, ((App)Application.Current).DealerId, ((App)Application.Current).ListingId, ((App)Application.Current).Vin, _vm.Parent.IsOverlay ? 1 : 0, r.Next(), ((App)Application.Current).AppraisalId);

            var webrequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
            webrequest.Method = "POST";
            webrequest.BeginGetRequestStream(new AsyncCallback(WriteCallback), webrequest);
        }

        private void WriteCallback(IAsyncResult asynchronousResult)
        {
            var webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the operation.
            var requestStream = webrequest.EndGetRequestStream(asynchronousResult);
            var buffer = new Byte[4096];
            var bytesRead = 0;
            Stream fileStream = _vm.FileInfo.OpenRead();
            fileStream.Position = 0;
            while ((bytesRead =
            fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();
            }

            fileStream.Close();
            requestStream.Close();
            webrequest.BeginGetResponse(new AsyncCallback(ReadCallback), webrequest);
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
                var response = (HttpWebResponse)webrequest.EndGetResponse(asynchronousResult);
                var serializer =
                              new DataContractJsonSerializer(typeof(ServerImage));

                var result = (ServerImage)serializer.ReadObject(response.GetResponseStream());
                _vm.ImageUrl = result.FileUrl;
                _vm.ThumbnailImageUrl = result.ThumbnailUrl;
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    var image = new BitmapImage(new Uri(result.FileUrl));
                    this._vm.ImageSource = image;
                    MarkComplete();
                });

            }
            catch (Exception e)
            {
                
                ErrorHandler.ShowWarning(e.InnerException + e.Message);
            }
        }

        private void MarkComplete()
        {
            _vm.IsComplete = true;
            _vm.IsFinish = true;
            _vm.Parent.NumberOfUploadedFiles++;
            _vm.Status = UploadStatus.Finish;
            StartNextUpload();
            //_vm.Parent.StartAll.DownloadingNumber--;
            //_vm.Parent.StartAll.Execute(null);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler UploadCommandComplete;
    }
}
