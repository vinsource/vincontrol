using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using vincontrol.DataFeed.Model;

namespace vincontrol.DataFeed.Helper
{
    public class CSVWriter
    {
        #region Members

        private StreamWriter _streamWriter;
        private bool _replaceCarriageReturnsAndLineFeedsFromFieldValues = true;
        private string _carriageReturnAndLineFeedReplacement = ",";
        private ExportConvertHelper _exportConvertHelper;
        private ExportXMLHelper _exportXmlHelper;
        private CommonHelper _commonHelper;

        #endregion Members

        public CSVWriter()
        {
            _exportConvertHelper = new ExportConvertHelper();
            _exportXmlHelper = new ExportXMLHelper();
            _commonHelper = new CommonHelper();
        }

        #region Properties

        /// <summary>
        /// Gets or sets whether carriage returns and line feeds should be removed from 
        /// field values, the default is true 
        /// </summary>
        public bool ReplaceCarriageReturnsAndLineFeedsFromFieldValues
        {
            get
            {
                return _replaceCarriageReturnsAndLineFeedsFromFieldValues;
            }
            set
            {
                _replaceCarriageReturnsAndLineFeedsFromFieldValues = value;
            }
        }

        /// <summary>
        /// Gets or sets what the carriage return and line feed replacement characters should be
        /// </summary>
        public string CarriageReturnAndLineFeedReplacement
        {
            get
            {
                return _carriageReturnAndLineFeedReplacement;
            }
            set
            {
                _carriageReturnAndLineFeedReplacement = value;
            }
        }

        #endregion Properties
        
        public void GenerateCsv(MappingViewModel mappingTemplate, List<VehicleViewModel> models, string filePath, bool hasHeader, string delimeter)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var writer = new StreamWriter(filePath, false, Encoding.Default))
            {
                WriteToStream(mappingTemplate, models, writer, hasHeader, delimeter);
                writer.Flush();
                writer.Close();
            }
        }

        private void WriteToStream(MappingViewModel mappingTemplate, IEnumerable<VehicleViewModel> models, TextWriter writer, bool hasHeader, string delimeter)
        {
            var fields = new List<string>();
            var mappingFields = hasHeader ? mappingTemplate.Mappings.Where(i => i.XMLField != null && i.XMLField.Trim() != "").OrderBy(i => i.Order).ToList() : mappingTemplate.Mappings.Where(i => i.Order != 0).OrderBy(i => i.Order).ToList();

            if(hasHeader)
            {
                var dataTable = _exportConvertHelper.InitializeFileWithHeader("", mappingFields.Select(i => i.XMLField).ToList());
                fields = (from DataColumn column in dataTable.Columns select column.ColumnName).ToList();
            }

            WriteRecord(fields, writer, delimeter);

            foreach (var row in models)
            {
                fields.Clear();
                var count = 1;

                foreach (var field in mappingFields)
                {
                    try
                    {
                        if (mappingTemplate.SplitImage && field.DBField.Equals(ExportXMLHelper.CarImageUrl)) { continue; }

                        if (!hasHeader)
                        {
                            if (count < field.Order)
                            {
                                for (int i = count; i < field.Order; i++)
                                {
                                    fields.Add(string.Empty);
                                }
                            }
                            count = field.Order + 1;
                        }

                        var columnValue = _exportXmlHelper.GetValueFormMappingField(row, field.DBField);
                        columnValue = _exportXmlHelper.ApplyReplacement(field.Replaces, columnValue);
                        columnValue = _exportXmlHelper.GetValueAfterApplyCondition(row, field, columnValue);
                        columnValue = _exportXmlHelper.ApplyExpression(row, field.Expression, columnValue);

                        if (columnValue == null) columnValue = string.Empty;

                        fields.Add(columnValue);
                    }
                    catch (Exception)
                    {
                        //throw;
                        fields.Add(string.Empty);
                    }
                }

                if (mappingTemplate.SplitImage)
                {
                    var carImageField = mappingFields.FirstOrDefault(i => i.DBField.Equals(ExportXMLHelper.CarImageUrl));
                    if (carImageField != null)
                    {
                        var columnValue = _exportXmlHelper.GetValueFormMappingField(row, carImageField.DBField);
                        columnValue = _exportXmlHelper.ApplyReplacement(carImageField.Replaces, columnValue);
                        columnValue = _exportXmlHelper.GetValueAfterApplyCondition(row, carImageField, columnValue);
                        columnValue = _exportXmlHelper.ApplyExpression(row, carImageField.Expression, columnValue);

                        if (String.IsNullOrEmpty(columnValue)) columnValue = string.Empty;

                        fields.AddRange(columnValue.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
                    }
                }

                WriteRecord(fields, writer, delimeter);
            }
        }
        
        private void WriteRecord(IList<string> fields, TextWriter writer, string delimeter)
        {
            if (fields.Any())
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    if (fields[i] == null) continue;
                    bool escapeQuotes = fields[i].Contains("\"");
                    string fieldValue = (escapeQuotes ? fields[i].Replace("\"", "") : fields[i]);


                    if (ReplaceCarriageReturnsAndLineFeedsFromFieldValues &&
                        (fieldValue.Contains("\r") || fieldValue.Contains("\n")))
                    {
                        fieldValue = fieldValue.Replace("\r\n", CarriageReturnAndLineFeedReplacement);
                        fieldValue = fieldValue.Replace("\r", CarriageReturnAndLineFeedReplacement);
                        fieldValue = fieldValue.Replace("\n", CarriageReturnAndLineFeedReplacement);
                    }
                    
                    //writer.Write(string.Format("{0}{1}{0}{2}", (string.Empty), "\"" + fieldValue + "\"", (i < (fields.Count - 1) ? delimeter : string.Empty)));
                    writer.Write(string.Format("{0}{1}{0}{2}", (string.Empty), delimeter.Equals(",") ? "\"" + fieldValue + "\"" : fieldValue, (i < (fields.Count - 1) ? delimeter : string.Empty)));
                }

                writer.WriteLine();
            }
        }

        /// <summary>
        /// Disposes of all unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (_streamWriter == null)
                return;

            _streamWriter.Close();
            _streamWriter.Dispose();
        }
    }
}
