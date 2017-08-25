using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VINCapture.UploadImage.Models
{
    public class UsbDeviceInfo
    {
        public UsbDeviceInfo(string deviceID, string pnpDeviceID, string description, string caption, string name, string creationClassName, string systemName, string status, string volumeName)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
            this.Caption = caption;
            this.Name = name;
            this.CreationClassName = creationClassName;
            this.SystemName = systemName;
            this.SystemName = systemName;
            this.Status = status;
            this.VolumeName = volumeName;
        }
        public UsbDeviceInfo(string deviceID, string pnpDeviceID, string description, string caption, string name, string creationClassName, string systemName, string status)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
            this.Caption = caption;
            this.Name = name;
            this.CreationClassName = creationClassName;
            this.SystemName = systemName;
            this.SystemName = systemName;
            this.Status = status;
          
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
        public string Caption { get; private set; }
        public string Name { get; private set; }
        public string CreationClassName { get; private set; }
        public string SystemName { get; private set; }
        public string Status { get; private set; }
        public string VolumeName { get; private set; }
        
    }
}
