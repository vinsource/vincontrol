using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.Win32;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.Backend.Windows.DataFeed.Import;
using System.Linq;
using System.Windows;

namespace vincontrol.Backend.ViewModels
{
    public class AttachDescriptionViewModel : ViewModelBase
    {
        public string CurrentStatus { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        //public ProfileStatusEnum ProfileStatus { get; set; }
        public List<KeyValuePair<string, string>> StatusList
        {
            get
            {
                return new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>(ProfileStatusEnum.Pending.ToString(),"Pending"),
                                                                                   new KeyValuePair<string, string>(ProfileStatusEnum.Completed.ToString(),"Completed"),
                                                                                   new KeyValuePair<string, string>(ProfileStatusEnum.WatingForImplemented.ToString(),"Wating For Implemented")
                                                                               };
            }
        }
        private IView _view;
        private ProfileViewModel _profile;

        private RelayCommand _saveFileProfileCommand;

        public RelayCommand SaveFileCommand
        {
            get { return _saveFileProfileCommand ?? (_saveFileProfileCommand = new RelayCommand(SaveFileAsync, null)); }
        }

        private RelayCommand _saveFileAndSendEmailCommand;

        public RelayCommand SaveFileAndSendEmailCommand
        {
            get { return _saveFileAndSendEmailCommand ?? (_saveFileAndSendEmailCommand = new RelayCommand(SaveFileAndSendEmailAsync, null)); }
        }

        private RelayCommand _attachFileCommand;

        public RelayCommand AttachFileCommand
        {
            get { return _attachFileCommand ?? (_attachFileCommand = new RelayCommand(AttachFile, null)); }
        }

        private RelayCommand _downloadFileCommand;

        public RelayCommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFile, null)); }
        }

        public void DownloadFile(object obj)
        {
            if (FileName != null)
            {
                var dialog = new SaveFileDialog { FileName = FileName };
            
                if (dialog.ShowDialog() == true)
                {

                    var fileStream = (FileStream)dialog.OpenFile(); // Get the file stream to do output
                    var writer = new StreamWriter(fileStream);

                    //writer.WriteRaw(xml);
                    //writer.Close();
                    var request =
                        WebRequest.Create(
                            string.Format("http://{0}/VinHelper/GetSpecificDataFeedImportFile/{1}",
                                          System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                          _profile.Id));
                    // ReSharper disable AssignNullToNotNullAttribute
                    var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
                    // ReSharper restore AssignNullToNotNullAttribute
                    var response = responseStream.ReadToEnd();
                    responseStream.Close();

                    writer.Write(response);
                    writer.Close();
                    fileStream.Close();
                    MessageBox.Show("File is Saved");
                }
            }
        }


        private Stream stream;
        private object _currentFileName;

        private void AttachFile(object obj)
        {
            var dlg = new OpenFileDialog { Filter = "All Files (*.*)|*.*", FilterIndex = 1};
            bool? result = dlg.ShowDialog();

            if (result.Value)
            {
               stream =  dlg.OpenFile();
                _currentFileName = dlg.SafeFileName;
            }
        }


        private void SaveFileAndSendEmailAsync(object obj)
        {
            DoPendingTask(SaveFileAndSendEmail);

        }

        private void SaveFileAndSendEmail()
        {
            SaveFile(false);
            SendEmail();
        }

        private void SendEmail()
        {
            var emails = ConfigurationManager.AppSettings["DeveloperAccounts"].ToString(CultureInfo.InvariantCulture);
            var subject = string.Format("Please implement import rule for profile Id {0}", _profile);
            var body = string.Format("Please implement import rule for profile Id {0}. The rule is {1}", _profile,
                                        Content);

            var request =
                    WebRequest.Create(
                        string.Format("http://{0}/VinHelper/GetSpecificDataFeedImportFile/{1}",
                                      System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                      _profile.Id));
            // ReSharper disable AssignNullToNotNullAttribute
            var responseStream = request.GetResponse().GetResponseStream();
            EmailHelper.SendEmail(emails.Split(';'), subject, body, responseStream, FileName );
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() => _view.Close()));
        }

        private void SaveFileAsync(object obj)
        {
            DoPendingTask(SaveAndClose);
        }

        private void SaveAndClose()
        {
            SaveFile(true);
        }

        private void SaveFile(bool closeView)
        {
            var context = new vincontrolwarehouseEntities();
            var item = context.importdatafeedprofilefiles.FirstOrDefault(i => i.ImportDataFeedProfileId == _profile.Id);
            if (item != null)
            {
                //FileName = item.FileName;
                //if (!string.IsNullOrEmpty(item.Status))
                //    ProfileStatus = (ProfileStatus)Enum.Parse(typeof(ProfileStatus), item.Status);
                item.Status = CurrentStatus;
                item.Description = Content;
                if (stream != null)
                {
                    var request =
                        WebRequest.Create(
                            string.Format("http://{0}/VinHelper/AttachDataFeedImportFileByStream/{1}/{2}",
                                          System.Configuration.ConfigurationManager.AppSettings["TaskServer"],_profile.Id, _currentFileName));
                    request.ContentType = "application/octet-stream";
                    request.Method = WebRequestMethods.Http.Post;
                    byte[] buf = new byte[16 * 1024];

                    using (var requestStream = request.GetRequestStream())
                    {
                        //byte[] fileDataInByte = null;
                        //using (BinaryReader binaryReader = new BinaryReader(stream))
                        //{
                        //    fileDataInByte = binaryReader.ReadBytes(stream.ContentLength);
                        //}
                        //requestStream.Write(fileDataInByte, 0, fileDataInByte.Length);

                        // as the file is streamed up in chunks, the server will be processing the file
                        int bytesRead = stream.Read(buf, 0, buf.Length);
                        while (bytesRead > 0)
                        {
                            requestStream.Write(buf, 0, bytesRead);
                            bytesRead = stream.Read(buf, 0, buf.Length);
                        }
                        requestStream.Close();
                    }


                    // ReSharper disable AssignNullToNotNullAttribute
                    var responseStream =
                        new StreamReader(request.GetResponse().GetResponseStream());
                    // ReSharper restore AssignNullToNotNullAttribute
                    var response = responseStream.ReadToEnd();
                    responseStream.Close();
                    MessageBox.Show(!response.Trim().ToLower().Equals("\"true\"")
                                        ? response
                                        : "File Uploaded Successfully.");
                }
            }
            else
            {
                context.AddToimportdatafeedprofilefiles(new importdatafeedprofilefile() { Description = Content, Status = CurrentStatus, ImportDataFeedProfileId = _profile.Id });

            }
            context.SaveChanges();

            Application.Current.Dispatcher.BeginInvoke(
           new Action(() =>
                          {
                              _profile.ProfileStatus = CurrentStatus;
                              if (closeView)
                              {
                                  _view.Close();
                              }
                          }));

        }

        public AttachDescriptionViewModel(IView view, ProfileViewModel profileId)
        {
            _view = view;
            _profile = profileId;
            InitData();
            _view.SetDataContext(this);
        }

        private void InitData()
        {
            var context = new vincontrolwarehouseEntities();
            var item = context.importdatafeedprofilefiles.FirstOrDefault(i => i.ImportDataFeedProfileId == _profile.Id);
            if (item != null)
            {
                Content = item.Description;

                CurrentStatus = item.Status;
                FileName = item.FileName;
            }
        }

        #region Overrides of ModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public enum ProfileStatusEnum
    {
        Pending, Completed, WatingForImplemented
    }
}