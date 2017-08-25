using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using VINCapture.UploadImage.Helpers;
using vincontrol.Data.Model;

namespace VINCapture.UploadImage.View
{
    /// <summary>
    /// Interaction logic for StartupPage.xaml
    /// </summary>
    public partial class StartupPage : Page
    {
        private readonly string _path;
        private readonly bool _portable;
        private NavigationService _navigationService;

        public StartupPage(string path,bool portable)
        {
            InitializeComponent();
            _path = path;
            _portable = portable;

            if (portable == false)
            {
                var drive = new DriveInfo(_path);
                if (drive.IsReady)
                {
                    txtUSBNoSize.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                txtUSBNoSize.Visibility = Visibility.Hidden;
            }
        }

        private void btnSignin_Click(object sender, RoutedEventArgs e)
        {
            _navigationService = this.NavigationService;

            
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;

                var latestVersionNumber = GetLatestVersionNumber();

                if (myVersion.Revision < latestVersionNumber)
                {

                    MessageBox.Show(
                        "There is a newer version deployed. Please close the program and reopen to get the latest update! ",
                        "Information", MessageBoxButton.OK);
                    var localProcess = Process.GetCurrentProcess();
                    localProcess.Kill();

                }
                else
                {
                    if (_navigationService != null)
                    {
                        LoadingImage.Visibility = Visibility.Visible;
                        txtUSBNoSize.Visibility = Visibility.Visible;
                        txtUSBNoSize.Text =
                            "Please wait to transfer files from Samsung Camera to hard drive... This process might take up to a couple minutes. Please don't close the window!";
                        var workerThread = new BackgroundWorker();
                        workerThread.DoWork += new DoWorkEventHandler(workerThread_DoWork);
                        workerThread.RunWorkerCompleted +=
                            new RunWorkerCompletedEventHandler(workerThread_RunWorkerCompleted);
                        workerThread.RunWorkerAsync();


                    }

                }
            }



            else
            {


                if (_navigationService != null)
                {
                    LoadingImage.Visibility = Visibility.Visible;
                    txtUSBNoSize.Visibility = Visibility.Visible;
                    txtUSBNoSize.Text =
                        "Please wait to transfer files from Samsung Camera to hard drive... This process might take up to a couple minutes. Please don't close the window!";
                    var workerThread = new BackgroundWorker();
                    workerThread.DoWork += new DoWorkEventHandler(workerThread_DoWork);
                    workerThread.RunWorkerCompleted +=
                        new RunWorkerCompletedEventHandler(workerThread_RunWorkerCompleted);
                    workerThread.RunWorkerAsync();


                }


            }



        }

        public static int GetLatestVersionNumber()
        {
            using (var context = new VincontrolEntities())
            {
                var versionNumber = (from l in context.VincaptureDesktopTools
                                 orderby l.VersionId descending
                                 select l).First();

                return versionNumber.VersionNumber;
            }

        }

   

        private void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(_portable==false)
                _navigationService.Navigate(new LoginPage(_path));
            else
                _navigationService.Navigate(new LoginPage(null));
        }

        private void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {

            const string backupPortablePath = @"C:\\VincaptureTemporary";

            const string backupStoragePath = @"C:\\VincaptureImageBackup";

           
            if (_portable == false)
            {
                var folder = new DirectoryInfo(_path);

                var vinCaptureFolder = folder.GetDirectories().FirstOrDefault(i => i.Name.Equals("VinCapture"));
                
                if (!Directory.Exists(backupStoragePath))
                {
                    Directory.CreateDirectory(backupStoragePath);
                }
                CopyAll(vinCaptureFolder, new DirectoryInfo(backupStoragePath));
            }
            else
            {
                if (Directory.Exists(backupPortablePath))
                {
                    FileHelper.DeleteFilesAndFoldersRecursively(backupPortablePath);
                }

                PortableDeviceHelper.TransferFilesFromWpdToHardDrive(backupPortablePath);

                var folder = new DirectoryInfo(backupPortablePath);
                var vinCaptureFolder = folder.GetDirectories().FirstOrDefault(i => i.Name.Equals("VinCapture"));
                if(vinCaptureFolder!=null)
                    CopyAll(vinCaptureFolder, new DirectoryInfo(backupStoragePath));

              
            }



        }

     

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {

                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

    }
}
