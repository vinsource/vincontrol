using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace vincontrol.VinSocial.YoutubeHelper
{
    /// <summary>
    /// Provides access to the user's "AppData" folder
    /// </summary>
    public static class AppData
    {
        /// <summary>
        /// Path to the Application specific %AppData% folder.
        /// </summary>
        public static string SpecificPath
        {
            get
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appData, Util.ApplicationName);
            }
        }

        /// <summary>
        /// Returns the path to the specified AppData file. Ensures that the AppData folder exists.
        /// </summary>
        public static string GetFilePath(string file)
        {
            string dir = SpecificPath;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return Path.Combine(SpecificPath, file);
        }

        /// <summary>
        /// Reads the specific file (if it exists), or returns null otherwise.
        /// </summary>
        /// <returns>File contents or null.</returns>
        public static byte[] ReadFile(string file)
        {
            string path = GetFilePath(file);
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }

        /// <summary>
        /// Returns true if the specified file exists in this AppData folder.
        /// </summary>
        public static bool Exists(string file)
        {
            return File.Exists(GetFilePath(file));
        }

        /// <summary>
        /// Writes the content to the specified file. Will create directories and files as necessary.
        /// </summary>
        public static void WriteFile(string file, byte[] contents)
        {
            string path = GetFilePath(file);
            File.WriteAllBytes(path, contents);
        }
    }
}