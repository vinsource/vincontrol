using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;

namespace vincontrol.VinSocial.YoutubeHelper
{
    /// <summary>
    /// General Utility class for samples.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Returns the name of the application currently being run.
        /// </summary>
        public static string ApplicationName
        {
            get { return Assembly.GetEntryAssembly().GetName().Name; }
        }

        /// <summary>
        /// Tries to retrieve and return the content of the clipboard. Will trim the content to the specified length.
        /// Removes all new line characters from the input.
        /// </summary>
        /// <remarks>Requires the STAThread attribute on the Main method.</remarks>
        /// <returns>Trimmed content of the clipboard, or null if unable to retrieve.</returns>
        public static string GetSingleLineClipboardContent(int maxLen)
        {
            try
            {
                string text = Clipboard.GetText().Replace("\r", "").Replace("\n", "");
                if (text.Length > maxLen)
                {
                    return text.Substring(0, maxLen);
                }
                return text;
            }
            catch (ExternalException)
            {
                return null; // Something is preventing us from getting the clipboard content -> return.
            }
        }

        /// <summary>
        /// Changes the clipboard content to the specified value.
        /// </summary>
        /// <remarks>Requires the STAThread attribute on the Main method.</remarks>
        /// <param name="text"></param>
        public static void SetClipboard(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (ExternalException) { }
        }
    }
}