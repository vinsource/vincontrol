using System;
using System.Management;

namespace VINCapture.UploadImage.USBHelpers
{
    /// <summary>Media watcher delegate.</summary>
    /// <param name="sender"></param>
    /// <param name="driveStatus"></param>
    public delegate void MediaWatcherEventHandler(object sender, USBDriveInfo driveStatus);
    /// 
    public class USBDriveInfo
    {
        public MediaEvent.DriveStatus DriveStatus { get; set; }
        public string DriveName { get; set; }
    }
    /// <summary>Class to monitor devices.</summary>
    /// 
    public class MediaEvent
    {
        #region Variables

        /*------------------------------------------------------------------------*/
        private string _mLogicalDrive;
        private ManagementEventWatcher _mManagementEventWatcher;
        /*------------------------------------------------------------------------*/
        #endregion

        #region Events
        /*------------------------------------------------------------------------*/
        public event MediaWatcherEventHandler MediaWatcher;
        /*------------------------------------------------------------------------*/
        #endregion


        #region Enums
        /*------------------------------------------------------------------------*/
        /// <summary>The drive types.</summary>
        public enum DriveType
        {
            Unknown = 0,
            NoRootDirectory = 1,
            RemoveableDisk = 2,
            LocalDisk = 3,
            NetworkDrive = 4,
            CompactDisk = 5,
            RamDisk = 6
        }



        /// <summary>The drive status.</summary>
        public enum DriveStatus
        {
            Unknown = -1,
            Ejected = 0,
            Inserted = 1,
        }
        /*-----------------------------------------------------------------------*/
        #endregion


        #region Monitoring
        /*-----------------------------------------------------------------------*/
        /// <summary>Starts the monitoring of device.</summary>
        /// <param name="path"></param>
        /// <param name="mediaEvent"></param>
        public void Monitor(string path, MediaEvent mediaEvent)
        {
            if (null == mediaEvent)
            {
                throw new ArgumentException("Media event cannot be null!");
            }

            //In case same class was called make sure only one instance is running
            /////////////////////////////////////////////////////////////
            Exit();

            //Keep logica drive to check
            /////////////////////////////////////////////////////////////
            _mLogicalDrive = GetLogicalDrive(path);

            var observer = new
                ManagementOperationObserver();

            //Bind to local machine
            /////////////////////////////////////////////////////////////
            var opt = new ConnectionOptions {EnablePrivileges = true};

            //Sets required privilege
            /////////////////////////////////////////////////////////////
            var scope = new ManagementScope("root\\CIMV2", opt);

            try
            {
                var wql = new WqlEventQuery
                              {
                                  EventClassName = "__InstanceModificationEvent",
                                  WithinInterval = new TimeSpan(0, 0, 1),
                                  Condition =
                                      String.Format(
                                          @"TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.DeviceId = '{0}'",
                                          _mLogicalDrive)
                              };

                _mManagementEventWatcher = new ManagementEventWatcher(scope, wql);

                //Register async. event handler
                /////////////////////////////////////////////////////////////
                _mManagementEventWatcher.EventArrived += mediaEvent.MediaEventArrived;
                _mManagementEventWatcher.Start();
            }
            catch (Exception e)
            {
                Exit();
                throw new Exception("Media Check: " + e.Message);
            }
        }

        /// <summary>Stops the monitoring of device.</summary>
        public void Exit()
        {
            //In case same class was called make sure only one instance is running
            /////////////////////////////////////////////////////////////
            if (null != _mManagementEventWatcher)
            {
                try
                {
                    _mManagementEventWatcher.Stop();
                    _mManagementEventWatcher = null;
                }
                catch
                {
                }
            }
        }
        /*-----------------------------------------------------------------------*/
        #endregion


        #region Helpers
        /*-----------------------------------------------------------------------*/

        private DriveStatus _mDriveStatus = DriveStatus.Unknown;
        /// <summary>Triggers the event when change on device occured.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaEventArrived(object sender, EventArrivedEventArgs e)
        {
            // Get the Event object and display it
            var pd = e.NewEvent.Properties["TargetInstance"];
            DriveStatus driveStatus = _mDriveStatus;
            var usbDriveInfo = new USBDriveInfo();
            {
                var mbo = pd.Value as ManagementBaseObject;
                if (mbo != null)
                {
                    var info = new System.IO.DriveInfo((string)mbo.Properties["DeviceID"].Value);
                    driveStatus = info.IsReady ? DriveStatus.Inserted : DriveStatus.Ejected;
                    usbDriveInfo.DriveName = (string)mbo.Properties["DeviceID"].Value;
                }
                usbDriveInfo.DriveStatus = driveStatus;
             
            }

            if (driveStatus != _mDriveStatus)
            {
                _mDriveStatus = driveStatus;
                if (null != MediaWatcher)
                {
                    MediaWatcher(sender, usbDriveInfo);
                }
            }
        }


        /// <summary>Gets the logical drive of a given path.</summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetLogicalDrive(string path)
        {
            var dirInfo = new System.IO.DirectoryInfo(path);
            string root = dirInfo.Root.FullName;
            string logicalDrive = root.Remove(root.IndexOf(System.IO.Path.DirectorySeparatorChar));
            return logicalDrive;
        }
        /*-----------------------------------------------------------------------*/
        #endregion
    }
}

