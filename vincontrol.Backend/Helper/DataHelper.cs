using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.DataFeed.Helper;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.Helper
{
    public static class DataHelper
    {
        public static void DownloadExportFile(string obj)
        {
            if (obj != null)
            {
                var dialog = new SaveFileDialog { FileName = obj };

                // You can use this line to set the initial FileName:
                //dialog.GetType().GetMethod("set_DefaultFileName").Invoke(dialog, new object[] { "SomeFileName.xml" });


                //dialog.DefaultExt = "*.xml";
                //dialog.Filter = "Excel Xml (*.xml)|*.xml|All files (*.*)|*.*";

                //var result = obj.ToString().Split('.');
                //dialog.Filter = "*.*";
                if (dialog.ShowDialog() == true)
                {

                    var fileStream = (FileStream)dialog.OpenFile(); // Get the file stream to do output
                    var writer = new StreamWriter(fileStream);

                    //writer.WriteRaw(xml);
                    //writer.Close();
                    var request =
                        WebRequest.Create(
                            string.Format("http://{0}/VinHelper/GetDataFeedExportFile/{1}",
                                          System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                          obj));
                    // ReSharper disable AssignNullToNotNullAttribute
                    var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
                    // ReSharper restore AssignNullToNotNullAttribute
                    var response = responseStream.ReadToEnd();
                    responseStream.Close();

                    writer.Write(response);
                    writer.Close();
                    fileStream.Close();
                    MessageBox.Show("File is Saved");
                }
            }
        }

        public static void UpdateExportTask(bool discontinue, string profileName, DateTime? runningTime, int profileId, int frequency)
        {
            UpdateTask(discontinue, profileName, runningTime, profileId, frequency, "Export");
        }

        public static void UpdateImportTask(bool discontinue, string profileName, DateTime? runningTime, int profileId, int frequency)
        {
            UpdateTask(discontinue, profileName, runningTime, profileId, frequency, "Import");
        }

        private static void UpdateTask(bool discontinue, string profileName, DateTime? runningTime, int profileId, int frequency, string type)
        {
            if (discontinue)
            {
                RemoveTaskScheduler(profileName, type);
            }
            else
            {
                UpdateTaskScheduler(profileName, runningTime, profileId, frequency, type);
            }
        }

        public static void RemoveExportTaskScheduler(string profileName)
        {
            RemoveTaskScheduler(profileName, "Export");
        }

        public static void RemoveImportTaskScheduler(string profileName)
        {
            RemoveTaskScheduler(profileName, "Import");
        }

        private static void RemoveTaskScheduler(string profileName, string type)
        {
            if (DataHelper.InitTaskStatus(type + "_" + profileName))
            {
                var request =
                    WebRequest.Create(
                        string.Format("http://{0}/VinHelper/DeleteDataFeedTask/{1}",
                                      System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                      type + "_" + profileName));
                // ReSharper disable AssignNullToNotNullAttribute
                var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
                // ReSharper restore AssignNullToNotNullAttribute
                responseStream.ReadToEnd();
                responseStream.Close();
            }
        }

        private static void UpdateExportTaskScheduler(string profileName, DateTime? runningTime, int profileId, int frequency)
        {
            UpdateTaskScheduler(profileName, runningTime, profileId, frequency, "Export");
        }

        private static void UpdateTaskScheduler(string profileName, DateTime? runningTime, int profileId, int frequency, string type)
        {
            if (DataHelper.InitTaskStatus(type + "_" + profileName))
            {
                DateTime dateTime = runningTime ?? DateTime.MinValue;
                var request =
                    WebRequest.Create(
                        string.Format("http://{0}/VinHelper/CreateDataFeedTask/{1}/{2}/{3}/{4}/{5}",
                                      System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                      type + "_" + profileName, type + " " + profileId, dateTime.Hour, dateTime.Minute
                                      , frequency));
                // ReSharper disable AssignNullToNotNullAttribute
                var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
                // ReSharper restore AssignNullToNotNullAttribute
                responseStream.ReadToEnd();
                responseStream.Close();
            }
        }


        public static IEnumerable<ProfileMappingViewModel> GetData(bool hasHeader, string delimeter, byte[] data)
        {
            var ftpExcelHelper = new FTPExcelHelper();
            var csv = ftpExcelHelper.ParseDataFile(data, hasHeader, delimeter);
            var dtTemporaryNoheader = new DataTable();
            dtTemporaryNoheader.Load(csv);
            var cloneTable = dtTemporaryNoheader.Clone();
            cloneTable.ImportRow(dtTemporaryNoheader.Rows[0]);

            var dbFields = GetInventoryColumnNames();
            return (from column in cloneTable.Columns.Cast<DataColumn>() select new ProfileMappingViewModel(hasHeader) { DBFields = dbFields, XMLField = column.ColumnName, SampleData = cloneTable.Rows[0][column].ToString() }).ToList();
        }

        public static IEnumerable<ProfileMappingViewModel> GetData(bool hasHeader, string delimeter, Stream data)
        {
            var ftpExcelHelper = new FTPExcelHelper();
            var csv = ftpExcelHelper.ParseDataFile(data, hasHeader, delimeter);
            var dtTemporaryNoheader = new DataTable();
            dtTemporaryNoheader.Load(csv);
            var cloneTable = dtTemporaryNoheader.Clone();
            cloneTable.ImportRow(dtTemporaryNoheader.Rows[0]);

            var dbFields = GetInventoryColumnNames();
            return (from column in cloneTable.Columns.Cast<DataColumn>() select new ProfileMappingViewModel(hasHeader) { DBFields = dbFields, XMLField = column.ColumnName, SampleData = cloneTable.Rows[0][column].ToString() }).ToList();
        }

        public static string SerializeSampleData(IncomingProfileTemplateViewModel vm)
        {

            //var xmlnsEmpty = new XmlSerializerNamespaces(new[]{XmlQualifiedName.Empty });
            //var xmlSerializer = new XmlSerializer(typeof(List<SampleData>));
            //xmlSerializer.Serialize(XmlWriter.Create(stream), vm.Mappings.Select(s => new SampleData() { Content = s.SampleData, XMLField = s.XMLField }).ToList(), xmlnsEmpty);
            //return Encoding.UTF8.GetString(stream.ToArray());

            //string test = "test";
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
            var strm = new MemoryStream();
            var writer = XmlWriter.Create(strm, settings);
            var serializer = new XmlSerializer(typeof(List<SampleData>));
            serializer.Serialize(writer, vm.Mappings.Select(s => new SampleData { Content = s.SampleData, XMLField = s.XMLField }).ToList());
            strm.Position = 0;
            var reader = new StreamReader(strm);
            var x = reader.ReadToEnd();
            //x.Dump();
            writer.Close();
            reader.Close();
            strm.Close();
            return x;

        }


        public static IEnumerable<SampleData> DesializeSampleData(string content)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<SampleData>));
            try
            {
                return (List<SampleData>)xmlSerializer.Deserialize(new StringReader(content));
            }
            catch (Exception)
            {
                return new List<SampleData>();
            }
        }

        public static List<Column> GetInventoryColumnNames()
        {
            var sqlHelper = new MySQLHelper();
            var result = sqlHelper.GetInventoryColumnNames();
            var excludeList = new List<string>
                {"AddToInventoryBy","AppraisalID","CarFaxOwner","CarRanking","NumberOfCar","LastUpdated","ChromeModelId",
            "ChromeStyleId","KBBOptionsId","KBBTrimId","MarketAveragePrice","MarketHighestPrice","MarketLowestPrice","PackageDescriptions"
            ,"Template","ThumbnailImageURL","ListingID","EnableAutoDescription","AdditionalTitle","IsFeatured","ColorCode","InteriorSurface","WindowStickerPrice","PreWholeSale","Unwind","Loaner","Auction","MarketRange","PriorRental"};
            return result.Where(i => !excludeList.Contains(i.Text)).ToList();
        }

        public static List<KeyValuePair<string, string>> GetDelimiterList(string exportFileType)
        {
            switch (exportFileType)
            {
                case "txt":
                    return new List<KeyValuePair<string, string>>
                               {
                                   new KeyValuePair<string, string>("\t", "Tab delimiter"),
                                   new KeyValuePair<string, string>("|", "| delimiter"),
                                   //new KeyValuePair<string, string>(";", "; delimiter"),
                                   new KeyValuePair<string, string>(",", ", delimiter")
                               };
                case "csv":
                    return new List<KeyValuePair<string, string>>();
                default:
                    return null;
            }
        }

        public static bool InitTaskStatus(string taskName)
        {
            var request = WebRequest.Create(string.Format("http://{0}/VinHelper/CheckTaskNameExist/{1}", System.Configuration.ConfigurationManager.AppSettings["TaskServer"], taskName));
            // ReSharper disable AssignNullToNotNullAttribute
            var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
            // ReSharper restore AssignNullToNotNullAttribute
            var response = responseStream.ReadToEnd();
            responseStream.Close();
            return response.ToLower().Equals("true");
        }

        public static void DownloadImportFile(string obj)
        {
            if (obj != null)
            {
                var dialog = new SaveFileDialog { FileName = obj };

                // You can use this line to set the initial FileName:
                //dialog.GetType().GetMethod("set_DefaultFileName").Invoke(dialog, new object[] { "SomeFileName.xml" });


                //dialog.DefaultExt = "*.xml";
                //dialog.Filter = "Excel Xml (*.xml)|*.xml|All files (*.*)|*.*";

                //var result = obj.ToString().Split('.');
                //dialog.Filter = "*.*";
                if (dialog.ShowDialog() == true)
                {

                    var fileStream = (FileStream)dialog.OpenFile(); // Get the file stream to do output
                    var writer = new StreamWriter(fileStream);

                    //writer.WriteRaw(xml);
                    //writer.Close();
                    var request =
                        WebRequest.Create(
                            string.Format("http://{0}/VinHelper/GetDataFeedImportFile/{1}",
                                          System.Configuration.ConfigurationManager.AppSettings["TaskServer"],
                                          obj));
                    // ReSharper disable AssignNullToNotNullAttribute
                    var responseStream = new StreamReader(request.GetResponse().GetResponseStream());
                    // ReSharper restore AssignNullToNotNullAttribute
                    var response = responseStream.ReadToEnd();
                    responseStream.Close();

                    writer.Write(response);
                    writer.Close();
                    fileStream.Close();
                    MessageBox.Show("File is Saved");
                }
            }
        }
    }

    public class SampleData
    {
        public string XMLField { get; set; }
        public string Content { get; set; }
    }
}

