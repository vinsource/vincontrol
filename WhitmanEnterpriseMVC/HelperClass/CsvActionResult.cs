using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;



namespace WhitmanEnterpriseMVC.HelperClass
{
    public class CsvActionResult : FileResult
    {
        private readonly DataTable _dataTable;

        public CsvActionResult(DataTable dataTable)
            : base("text/csv")
        {
            _dataTable = dataTable;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var outputStream = response.OutputStream;
            using (var memoryStream = new MemoryStream())
            {
                WriteDataTable(memoryStream);
                outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            //var memoryStream = new MemoryStream();
            //WriteCSVFile(_dataTable, out memoryStream);
            //outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        private void WriteDataTable(Stream stream)
        {
            var streamWriter = new StreamWriter(stream, Encoding.Default);

            WriteHeaderLine(streamWriter);
            streamWriter.WriteLine();
            WriteDataLines(streamWriter);

            streamWriter.Flush();
        }

        private void WriteHeaderLine(StreamWriter streamWriter)
        {
            foreach (DataColumn dataColumn in _dataTable.Columns)
            {
                WriteValue(streamWriter, dataColumn.ColumnName);
            }
        }

        private void WriteDataLines(StreamWriter streamWriter)
        {
            foreach (DataRow dataRow in _dataTable.Rows)
            {
                foreach (DataColumn dataColumn in _dataTable.Columns)
                {
                    WriteValue(streamWriter, dataRow[dataColumn.ColumnName].ToString());
                }
                streamWriter.WriteLine();
            }
        }


        private static void WriteValue(StreamWriter writer, String value)
        {
            writer.Write("\"");
            writer.Write(value.Replace("\"", "\"\""));
            writer.Write("\",");
        }


        //public static string CreateTemplateFile()
        //{
        //    FileInfo newFile = new FileInfo(@"C:\DataConversion\Template.xlsx");
        //    if (newFile.Exists)
        //    {
        //        newFile.Delete();  // ensures we create a new workbook
        //        newFile = new FileInfo(@"C:\DataConversion\Template.xlsx");
        //    }
        //    using (ExcelPackage package = new ExcelPackage(newFile))
        //    {
        //        // add a new worksheet to the empty workbook
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("InventoryReport");
        //        //Add the headers
        //        worksheet.Cells[1, 1].Value = "Year";
        //        worksheet.Cells[1, 2].Value = "Make";
        //        worksheet.Cells[1, 3].Value = "Model";
        //        worksheet.Cells[1, 4].Value = "Stock#";
        //        worksheet.Cells[1, 5].Value = "Vin";
        //        worksheet.Cells[1, 6].Value = "Mileage";
        //        worksheet.Cells[1, 7].Value = "DaysInStock";
        //        worksheet.Cells[1, 8].Value = "Color";
        //        worksheet.Cells[1, 9].Value = "Price";



        //        //Set column width
        //        worksheet.Column(1).Width = 30;
        //        worksheet.Column(2).Width = 10;
        //        worksheet.Column(3).Width = 10;
        //        worksheet.Column(4).Width = 15;
        //        worksheet.Column(5).Width = 25;
        //        worksheet.Column(6).Width = 25;
        //        worksheet.Column(7).Width = 25;
        //        worksheet.Column(8).Width = 15;
        //        worksheet.Column(9).Width = 15;



        //        //Set NumberFormat
        //        //worksheet.Column(6).Style.Numberformat.Format = "$#,##0.00";



        //        using (ExcelRange row = worksheet.Cells["A1:Z1"])
        //        {
        //            row.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(23, 55, 93));
        //            row.Style.Font.Color.SetColor(System.Drawing.Color.White);
        //            row.Style.Font.Size = 12;
        //            row.Style.Font.Bold = true;
        //        }

        //        //Switch the PageLayoutView back to normal
        //        //worksheet.View.PageLayoutView = true;

        //        // lets set the header text 
        //        worksheet.HeaderFooter.oddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" REPORT";
        //        // add the page number to the footer plus the total number of pages
        //        worksheet.HeaderFooter.oddFooter.RightAlignedText =
        //            string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
        //        // add the sheet name to the footer
        //        worksheet.HeaderFooter.oddFooter.CenteredText = ExcelHeaderFooter.SheetName;
        //        // add the file path to the footer
        //        worksheet.HeaderFooter.oddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

        //        worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];
        //        worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

        //        package.Workbook.Properties.Title = "INVENTORYREPORT";

        //        package.Save();

        //    }

        //    return newFile.FullName;
        //}

        //public static void WriteCSVFile(DataTable dataTable , out MemoryStream stream)
        //{
        //    FileInfo newFile = new FileInfo(@"C:\DataConversion\Template.xlsx");
            
        //    using (ExcelPackage package = new ExcelPackage(newFile))
        //    {
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

        //        int numberOfRow = worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row;
        //        int startRow = numberOfRow + 2;

        //        foreach (DataRow drRow  in dataTable.Rows)
        //        {

        //            worksheet.SetValue(startRow, 1, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 2, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 3, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 4, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 5, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 6, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 7, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 8, drRow["ModelYear"].ToString());
        //            worksheet.SetValue(startRow, 9, drRow["ModelYear"].ToString());
           

        //            startRow++;
        //        }


        //        // save our new workbook and we are done!

        //        stream = package.Stream;
        //    }
           
        //}





    }
}
