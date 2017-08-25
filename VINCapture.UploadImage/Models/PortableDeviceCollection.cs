using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PortableDeviceApiLib;

namespace VINCapture.UploadImage.Models
{
    public class PortableDeviceCollection : Collection<PortableDevice>
    {
        private readonly PortableDeviceManager _deviceManager;

        public PortableDeviceCollection()
        {
            this._deviceManager = new PortableDeviceManager();
        }

        public void Refresh()
        {
            this._deviceManager.RefreshDeviceList();

            // Determine how many WPD devices are connected
            var deviceIds = new string[1];
            uint count = 1;
            this._deviceManager.GetDevices(ref deviceIds[0], ref count);

            // Retrieve the device id for each connected device
            deviceIds = new string[count];
            this._deviceManager.GetDevices(ref deviceIds[0], ref count);
            foreach (var deviceId in deviceIds)
            {
                Add(new PortableDevice(deviceId));
            }
        }

        public bool IsVincaptureCamera()
        {
            this._deviceManager.RefreshDeviceList();

            // Determine how many WPD devices are connected
            var deviceIds = new string[1];
            uint count = 1;
            this._deviceManager.GetDevices(ref deviceIds[0], ref count);

            // Retrieve the device id for each connected device
            deviceIds = new string[count];
            this._deviceManager.GetDevices(ref deviceIds[0], ref count);
            foreach (var deviceId in deviceIds)
            {
                var tmp = new PortableDevice(deviceId);

                tmp.Connect();

                if (tmp.FriendlyName.ToLower().Equals("vincapture"))
                {
                    tmp.Disconnect();
                    return true;
                }
            }

            return false;
        }
    }

   
}