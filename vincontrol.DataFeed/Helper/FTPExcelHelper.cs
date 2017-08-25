using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace vincontrol.DataFeed.Helper
{
    public class FTPExcelHelper
    {
        public CachedCsvReader ParseDataFile(byte[] data, bool hasHeader, string delimeter)
        {
            try
            {
                var memoryStream = new MemoryStream(data);
                var reader = new StreamReader(memoryStream);
                var cr = !String.IsNullOrEmpty(delimeter) ? new CachedCsvReader(reader, hasHeader, Convert.ToChar(delimeter)): new CachedCsvReader(reader, hasHeader);

                return cr;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception is " + ex.Message);
            }
        }

        public CachedCsvReader ParseDataFile(Stream data, bool hasHeader, string delimeter)
        {
            try
            {
                var reader = new StreamReader(data);
                var cr = !String.IsNullOrEmpty(delimeter) ? new CachedCsvReader(reader, hasHeader, Convert.ToChar(delimeter)) : new CachedCsvReader(reader, hasHeader);

                return cr;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception is " + ex.Message);
            }
        }

        public CachedCsvReader ParseDataFile(string path, bool hasHeader, string delimeter, NetworkCredential credential)
        {
            var myClient = new FTPClientHelper(1); //FTPVinwindowUsername

            try
            {
                byte[] data = myClient.DownloadData(path, credential);
                var memoryStream = new MemoryStream(data);
                var reader = new StreamReader(memoryStream);
                var cr = !String.IsNullOrEmpty(delimeter) ? new CachedCsvReader(reader, hasHeader, Convert.ToChar(delimeter)) : new CachedCsvReader(reader, hasHeader);

                return cr;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception is " + ex.Message);
            }
        }
    }
}
