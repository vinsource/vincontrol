using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using VINCapture.UploadImage.Helpers;
using VINCapture.UploadImage.Models;
using VINCapture.UploadImage.USBHelpers;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;


namespace VINCapture.UploadImage.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DealerUser Dealer { get; set; }
        //public string ImageServiceUrl { get; set; }
        public Dictionary<string, StartupWindow> FormList;
        public StartupWindow Startform;
        public MediaEvent MediaEvent;

        Mutex m;
        //public App()
        //{

           
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
            bool isnew;
            m = new Mutex(true, "VincaptureSingleInstance", out isnew);
            if (!isnew)
            {
                Environment.Exit(0);
            }
            else
            {
                base.OnStartup(e);
                Init();
            }
            
        }

        private void Init()
        {
            FormList = new Dictionary<string, StartupWindow>();
            CreateIcon();
            //AddInsetUSBHandler();
            //AddInsertPortableHandler();

            var flag = false;

            var logicalDrives = GetLogicalDiskDevices().Where(x=>!String.IsNullOrEmpty(x.VolumeName));
            var logicalVincapture = logicalDrives.FirstOrDefault(x => x.VolumeName.ToLower().Equals("hornburg camera")
                || x.VolumeName.ToLower().Equals("vincapture"));
            if (logicalVincapture != null)
            {
                flag = true;
                var name = logicalVincapture.Caption;
                if (!FormList.ContainsKey(name))
                {
                    var form = new StartupWindow(name, false);
                    FormList.Add(name, form);
                    form.Show();
                }
            }
            else
            {
                var usbDevices = GetUsbDevices();

                var portableVincaptureDevice = usbDevices.FirstOrDefault(x => x.Caption.ToLower().Equals("hornburg camera")
                    || x.Caption.ToLower().Equals("vincapture"));
                if (portableVincaptureDevice != null)
                {
                    flag = true;
                    var name = portableVincaptureDevice.PnpDeviceID;
                    if (!FormList.ContainsKey(name))
                    {
                        var form = new StartupWindow(name, true);
                        FormList.Add(name, form);
                        form.Show();
                    }

                }

        
            }

            if (flag == false)
            {
                MessageBox.Show("Vincapture can not detect any camera or usb devices that are compatible to the program!",
                      "Information", MessageBoxButton.OK);
                var localProcess = Process.GetCurrentProcess();
                localProcess.Kill();
            }


        }

        private IEnumerable<UsbDeviceInfo> GetLogicalDiskDevices()
        {
            var devices = new List<UsbDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_LogicalDisk"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new UsbDeviceInfo(
                    (string) device.GetPropertyValue("DeviceID"),
                    (string) device.GetPropertyValue("PNPDeviceID"),
                    (string) device.GetPropertyValue("Description"),
                    (string) device.GetPropertyValue("Caption"),
                    (string) device.GetPropertyValue("Name"),
                    (string) device.GetPropertyValue("CreationClassName"),
                    (string) device.GetPropertyValue("SystemName"),
                    (string) device.GetPropertyValue("Status"),
                    (string) device.GetPropertyValue("VolumeName")
                    ));
            }

            collection.Dispose();
            return devices;
        }

        private IEnumerable<UsbDeviceInfo> GetUsbDevices()
        {
            var devices = new List<UsbDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new UsbDeviceInfo(
                    (string)device.GetPropertyValue("DeviceID"),
                    (string)device.GetPropertyValue("PNPDeviceID"),
                    (string)device.GetPropertyValue("Description"),
                    (string)device.GetPropertyValue("Caption"),
                    (string)device.GetPropertyValue("Name"),
                    (string)device.GetPropertyValue("CreationClassName"),
                    (string)device.GetPropertyValue("SystemName"),
                    (string)device.GetPropertyValue("Status")
              
                    ));
            }

            collection.Dispose();
            return devices;
        }


        private void CreateIcon()
        {
            var notifyIcon = new NotifyIcon
            {
                BalloonTipText = UploadImage.Properties.Resources.App_Init_The_app_has_been_minimised__Click_the_tray_icon_to_show_,
                BalloonTipTitle = UploadImage.Properties.Resources.Balloon_Tip_Title,
                Text = UploadImage.Properties.Resources.Balloon_Tip_Title,
                Visible = true
            };
            var streamResourceInfo = GetResourceStream(new Uri("pack://application:,,,/VINCapture.UploadImage;component/Resources/VinCapture.ico"));
            if (streamResourceInfo != null)
            {
                notifyIcon.Icon = new System.Drawing.Icon(streamResourceInfo.Stream);
            }

            notifyIcon.ContextMenu = new ContextMenu(new[]{new MenuItem("Close", (s, ex) =>
                                                                                                   {
                                                                                                       ClearFormList();
                                                                                                       Current.
                                                                                                           Shutdown();
                                                                                                       notifyIcon.
                                                                                                           Dispose();
                                                                                                   })
                                                                              });

        }

        private void ClearFormList()
        {
            foreach (var startupWindow in FormList)
            {
                if (startupWindow.Value.Dispatcher.CheckAccess())
                    startupWindow.Value.Close();
                else
                    startupWindow.Value.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(startupWindow.Value.Close));
            }

            FormList.Clear();
        }

        void MediaEventMediaWatcher(object sender, USBDriveInfo driveInfo)
        {
            if (driveInfo.DriveStatus == MediaEvent.DriveStatus.Inserted)
            {
                InitForm(UsbStateChange.Added, driveInfo.DriveName);
            }
            else if (driveInfo.DriveStatus == MediaEvent.DriveStatus.Ejected)
            {
                InitForm(UsbStateChange.Removed, driveInfo.DriveName);
            }
        }

        
        private void InitForm(UsbStateChange e, string name)
        {
            if (e.Equals(UsbStateChange.Removed))
            {
                if (FormList.ContainsKey(name))
                {
                    var form = FormList[name];
                    if (form.Dispatcher.CheckAccess())
                        form.Close();
                    else
                        form.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(form.Close));
                    FormList.Remove(name);
                }
            }
            else
            {
                var drive = new DriveInfo(name);
                if (drive.IsReady)
                {
                    var directory = new DirectoryInfo(name);
                    if (directory.GetDirectories().Any(i => i.Name.ToLower() == "vincapture"))
                    {
                        if (e.Equals(UsbStateChange.Added))
                        {

                            var thread = new Thread(DisplayFormThreadForStorage);
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.Start(name);
                        }
                    }
                }
            }
        }

        private void DisplayFormThreadForStorage(object obj)
        {
            var name = obj.ToString();
            if (!FormList.ContainsKey(name))
            {
                var form = new StartupWindow(name,false);
                FormList.Add(name, form);
                form.Show();
            }
            System.Windows.Threading.Dispatcher.Run();
        }

        private void DisplayFormThreadForPortable(object obj)
        {
            var name = obj.ToString();
            if (!FormList.ContainsKey(name))
            {
                var form = new StartupWindow(name,true);
                FormList.Add(name, form);
                form.Show();
            }
            System.Windows.Threading.Dispatcher.Run();
        }

        ManagementEventWatcher _w;

        void AddInsetUSBHandler()
        {
            var scope = new ManagementScope("root\\CIMV2") {Options = {EnablePrivileges = true}};

            try
            {

                var q = new WqlEventQuery
                            {
                                EventClassName = "__InstanceCreationEvent",
                                WithinInterval = new TimeSpan(0, 0, 3),
                                Condition = @"TargetInstance ISA 'Win32_LogicalDisk'"
                            };
                _w = new ManagementEventWatcher(scope, q);
                _w.EventArrived += UsbAdded;
                
                _w.Start();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                if (_w != null)
                    _w.Stop();

            }

        }

        private void AddInsertPortableHandler()
        {
            var scope = new ManagementScope("root\\CIMV2") { Options = { EnablePrivileges = true } };

            try
            {

                var q = new WqlEventQuery
                {
                    EventClassName = "__InstanceCreationEvent",
                    WithinInterval = new TimeSpan(0, 0, 3),
                    Condition = @"TargetInstance ISA 'Win32_USBHub'"
                };
                _w = new ManagementEventWatcher(scope, q);
                _w.EventArrived += PortableAdded;
                _w.Start();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                if (_w != null)
                    _w.Stop();

            }

        }

        private void PortableAdded(object sender, EventArrivedEventArgs e)
        {
            try
            {
                var pd = e.NewEvent.Properties["TargetInstance"];
                {
                    var mbo = pd.Value as ManagementBaseObject;
                    
                    if (mbo != null)
                    {
                        
                        var driveinfo = mbo.Properties["Caption"].Value.ToString();
                        
                        if (driveinfo.Contains("USB Composite Device"))
                        {
                            var thread = new Thread(DisplayFormThreadForPortable);
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.Start("vincapture");
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                
                ServiceLog.ErrorImportLog(ex.Message);
                ServiceLog.ErrorImportLog(ex.StackTrace);
            }
            
         
        }

        private object GetDeviceID(string toString)
        {
            //toString is: USB\\VID_04E8&PID_6860\\4DF1EE7C1D446F2F
            //should return VID_04E8&PID_6860
            return toString;
        }

        public void UsbAdded(object sender, EventArrivedEventArgs e)
        {
            var pd = e.NewEvent.Properties["TargetInstance"];
            {
                var mbo = pd.Value as ManagementBaseObject;
              
                if (mbo != null)
                {
                    var drivePath = mbo.Properties["Caption"].Value.ToString();
                    var drive = new DriveInfo(drivePath);
                    if (drive.IsReady)
                    {
                        InitForm(UsbStateChange.Added, drivePath);
                    }
                    else
                    {
                        MediaEvent = new MediaEvent();
                        MediaEvent.MediaWatcher += MediaEventMediaWatcher;
                        MediaEvent.Monitor(drivePath, MediaEvent);
                    }
                }
            }
        }
    }
}
