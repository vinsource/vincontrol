using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace vincontrol.Helper
{
    public static class FileExtensions
    {
        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    //if we go this far, there is some permission issue on host
                    result = false;
                }
            }
            return result;
        }
    }
}
