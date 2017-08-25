using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace VINCapture.UploadImage.Helpers
{
    public class FileHelper
    {
        public static void DeleteFilesAndFoldersRecursively(string targetDir)
        {

            try
            {
                foreach (string file in Directory.GetFiles(targetDir))
                {
                    File.Delete(file);
                }

                foreach (string subDir in Directory.GetDirectories(targetDir))
                {
                    DeleteFilesAndFoldersRecursively(subDir);
                }

                Thread.Sleep(1); // This makes the difference between whether it works or not. Sleep(0) is not enough.
                Directory.Delete(targetDir);
            }
            catch (Exception)
            {
            }

        }
    }
}
