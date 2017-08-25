using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vincontrol.VinsellDesktopDownloader
{
    public class DirectoryHelper
    {
        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
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

        public static void CreateSubFolderAndCopy(DirectoryInfo source, DirectoryInfo target)
        {
            string fullSubFolderName = target.FullName + "\\" + source.Name;
            if (Directory.Exists(fullSubFolderName) == false)
            {
                Directory.CreateDirectory(fullSubFolderName);
            }

            var diSubTarget = new DirectoryInfo(fullSubFolderName);

            CopyAll(source, diSubTarget);
        }

        //public static void CommbineFolderInVinsellDesktop()
        //{
        //    const string baseExceuteDirectory = @"C:\VinsellDesktopDownloader\ExceuteFolder";

        //    const string baseRequiredDirectory = @"C:\VinsellDesktopDownloader\RequiredFolder";

        //    const string baseRunDirectory = @"C:\VinsellDesktop\";

        //    try
        //    {
        //        if (!Directory.Exists(baseRunDirectory))
        //        {
        //            Directory.CreateDirectory(baseRunDirectory);

        //        }
        //        var dirList = Directory.GetDirectories(baseRequiredDirectory);

        //        foreach (var tmpPath in dirList)
        //        {
        //            var di = new DirectoryInfo(tmpPath);

        //            CreateSubFolderAndCopy(di,new DirectoryInfo(baseRunDirectory));
        //        }

        //        CopyAllFiles(new DirectoryInfo(baseRequiredDirectory), new DirectoryInfo(baseRunDirectory));

        //        CopyAllFiles(new DirectoryInfo(baseExceuteDirectory), new DirectoryInfo(baseRunDirectory));

              
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }

        //}

        //public static void CopyAllFiles(DirectoryInfo source, DirectoryInfo target)
        //{
        //    var fileList = source.GetFiles();

        //    foreach (var tmp in fileList)
        //    {
        //        var combinedPath = Path.Combine(target.FullName, tmp.Name);
        //        tmp.CopyTo(combinedPath, true);
        //    }
        //}



    }
}
