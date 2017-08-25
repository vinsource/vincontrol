using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VINCapture.UploadImage.Models;

namespace VINCapture.UploadImage.Helpers
{
    public class PortableDeviceHelper
    {
        private static string _copyPath;

        public static void TransferFilesFromWpdToHardDrive(string path)
        {
            _copyPath = path;

            var collection = new PortableDeviceCollection();

            collection.Refresh();

            foreach (var device in collection)
            {
                device.Connect();

                var folder = device.GetContents();

                var item = folder.Files.FirstOrDefault(x => x.Name.ToLower().Equals("camera") || x.Name.ToLower().Equals("tablet"));

                if(item==null) continue;

                NavigateObject(device, item);

                device.Disconnect();
            }

        }

        public static void NavigateObject(PortableDevice device, PortableDeviceObject portableDeviceObject)
        {
            if (portableDeviceObject is PortableDeviceFolder)
            {
                NavigateFolderContents(device, (PortableDeviceFolder) portableDeviceObject);
            }
        }

        public static void NavigateFolderContents(PortableDevice device, PortableDeviceFolder folder)
        {
            var folderPath = _copyPath + "\\" + "VinCapture";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            var item = folder.Files.FirstOrDefault(x => x.Name.ToLower().Equals("vincapture"));

            var itemSub = (PortableDeviceFolder) item;

            CopyFolderInsideVincapture(device, itemSub, folderPath);
        }

        public static void CopyFolderInsideVincapture(PortableDevice device, PortableDeviceFolder vinFolder,
            string baseUrl)
        {

            foreach (var tmp in vinFolder.Files)
            {

                if (tmp is PortableDeviceFolder)
                {
                    var folderPath = baseUrl + "\\" + tmp.Name;
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);


                    }
                    CopyFolderInsideVincapture(device, (PortableDeviceFolder) tmp, folderPath);

                }
                if (tmp is PortableDeviceFile)
                {
                    var tmpFile = (PortableDeviceFile) tmp;

                    device.DownloadFile(tmpFile, baseUrl);

                }


            }
        }

        public static void DeleteFromWpdToHardDrive()
        {


            var collection = new PortableDeviceCollection();

            collection.Refresh();

            foreach (var device in collection)
            {
                device.Connect();

                var folder = device.GetContents();

                var item = folder.Files.FirstOrDefault(x => x.Name.ToLower().Equals("camera"));

                NavigateObjectForDelete(device, item);

                device.Disconnect();
            }
        }

        public static void NavigateObjectForDelete(PortableDevice device, PortableDeviceObject portableDeviceObject)
        {
            if (portableDeviceObject is PortableDeviceFolder)
            {
                NavigateFolderContentsForDelete(device, (PortableDeviceFolder) portableDeviceObject);
            }
        }

        public static void NavigateFolderContentsForDelete(PortableDevice device, PortableDeviceFolder folder)
        {
            var item = folder.Files.FirstOrDefault(x => x.Name.ToLower().Equals("vincapture"));

            var itemSub = (PortableDeviceFolder) item;

            DeleteFolderVincapture(device, itemSub);

        }


        public static void DeleteFolderVincapture(PortableDevice device, PortableDeviceFolder vinFolder)
        {

            foreach (var tmp in vinFolder.Files)
            {

                if (tmp is PortableDeviceFolder)
                {
                    DeleteFolderVincapture(device, (PortableDeviceFolder)tmp);
                      var tmpFile = (PortableDeviceFolder)tmp;

                    device.DeleteFolder(tmpFile);

                }
                if (tmp is PortableDeviceFile)
                {
                    var tmpFile = (PortableDeviceFile)tmp;

                
                    device.DeleteFile(tmpFile);
                   
                }


            }
        }

    
    }
}
