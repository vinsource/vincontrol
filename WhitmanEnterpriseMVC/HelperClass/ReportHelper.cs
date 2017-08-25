using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Models;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public sealed class ReportHelper
    {
        //public static void ExportToCSV<T>(this IQueryable<T> list, string filename, string[] exclude)
        //{
        //    StringWriter sw = new StringWriter();
        //    bool headerPrinted = false;
        //    foreach (var obj in list)
        //    {
        //        Type type = obj.GetType();
        //        PropertyInfo[] properties = type.GetProperties();

        //        string s = String.Empty;
        //        if (!headerPrinted)
        //        {
        //            foreach (PropertyInfo propertyInfo in properties)
        //            {
        //                if (exclude.Count(x => x.Contains(propertyInfo.Name)) > 0)
        //                    continue;

        //                if (s.Length > 0)
        //                    s = String.Join(",", new string[] { s, propertyInfo.Name });
        //                else
        //                    s = propertyInfo.Name;
        //            }
        //            sw.WriteLine(s);
        //            headerPrinted = true;
        //        }

        //        s = String.Empty;
        //        for (int idx = 0; idx < properties.Length; idx++)
        //        {

        //            if (exclude.Count(x => x.Contains(properties[idx].Name)) > 0)
        //                continue;

        //            var value = properties[idx].GetValue(obj, null);
        //            var formattedValue = value == null ? String.Empty : value.ToString();

        //            if (value != null)
        //            {
        //                if (value.GetType() == typeof(string))
        //                    formattedValue = "\"" + formattedValue + "\"";
        //            }

        //            if (s.Length > 0)
        //                s = String.Join(",", new string[] { s, formattedValue });
        //            else
        //                s = formattedValue;

        //        }
        //        sw.WriteLine(s);
        //    }
        //    HttpResponse response = System.Web.HttpContext.Current.Response;
        //    response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        //    response.ContentType = "application/ms-excel";
        //    response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        //    response.Write(sw);
        //    response.End();
        //}       

        public static byte[] ExportToCSV(List<CarInfoFormViewModel> carList, int dealerId, string Title)
        {
            var package = new ExcelPackage();
            // add a new worksheet to the empty workbook
            var worksheet = package.Workbook.Worksheets.Add(Title);

            WriteHeader(worksheet);
            //Switch the PageLayoutView back to normal
            // lets set the header text 
            worksheet.HeaderFooter.oddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\"" + Title;
            // add the page number to the footer plus the total number of pages
            worksheet.HeaderFooter.oddFooter.RightAlignedText =
                string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
            // add the sheet name to the footer
            worksheet.HeaderFooter.oddFooter.CenteredText = ExcelHeaderFooter.SheetName;
            // add the file path to the footer

            worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:1"];
            worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

            package.Workbook.Properties.Title = Title;

            worksheet = package.Workbook.Worksheets[1];

            int numberOfRow = worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row;
            int startRow = numberOfRow + 2;

            var context = new whitmanenterprisewarehouseEntities();
            var dtDealerSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealerId);


            //PlUG DATA
            startRow = PlugData(carList.Where(x => !x.Reconstatus).OrderBy(x => x.Make), worksheet, dtDealerSetting.DefaultStockImageUrl, startRow, false);
            PlugData(carList.Where(x => x.Reconstatus).OrderBy(x => x.Make), worksheet, dtDealerSetting.DefaultStockImageUrl, startRow, true);

            return package.GetAsByteArray();
        }

        private static void WriteHeader(ExcelWorksheet worksheet)
        {
            //Add the headers
            worksheet.Cells[1, 1].Value = "Year";
            worksheet.Cells[1, 2].Value = "Make";
            worksheet.Cells[1, 3].Value = "Model";
            worksheet.Cells[1, 4].Value = "Stock#";
            worksheet.Cells[1, 5].Value = "Vin";
            worksheet.Cells[1, 6].Value = "Mileage";
            worksheet.Cells[1, 7].Value = "Color";
            worksheet.Cells[1, 8].Value = "Price";
            worksheet.Cells[1, 9].Value = "Days";
            worksheet.Cells[1, 10].Value = "Pics";


            //Set column width
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 10;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 9;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 7;
            worksheet.Column(7).Width = 10;
            worksheet.Column(8).Width = 7;
            worksheet.Column(9).Width = 4;
            worksheet.Column(10).Width = 3;


            //Set NumberFormat

            using (ExcelRange row = worksheet.Cells["A1:J1"])
            {
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(23, 55, 93));
                row.Style.Font.Color.SetColor(System.Drawing.Color.White);
                row.Style.Font.Size = 9;
                row.Style.Font.Bold = true;
            }
        }

        private static int PlugData(IOrderedEnumerable<CarInfoFormViewModel> result, ExcelWorksheet worksheet, string defaultStockImageUrl, int startRow, bool isFormat)
        {
            foreach (var drRow in result)
            {
                FillData(worksheet, defaultStockImageUrl, startRow, drRow);

                if (isFormat)
                {
                    using (ExcelRange row = worksheet.Cells[startRow, 1, startRow, 10])
                    {
                        row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                        row.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        row.Style.Font.Size = 9;
                        row.Style.Font.Bold = true;
                    }
                }

                startRow++;
            }

            return startRow;
        }

        private static void FillData(ExcelWorksheet worksheet, string defaultStockImageUrl, int startRow, CarInfoFormViewModel drRow)
        {
            CarExcelInfoViewModel carExcelInfoViewModel = drRow.ConvertToCarExcelViewInfo(defaultStockImageUrl);
            worksheet.SetValue(startRow, 1, carExcelInfoViewModel.ModelYear.ToString());
            worksheet.SetValue(startRow, 2, carExcelInfoViewModel.Make.ToString());
            worksheet.SetValue(startRow, 3, carExcelInfoViewModel.Model.ToString());
            worksheet.SetValue(startRow, 4, carExcelInfoViewModel.StockNumber.ToString());
            worksheet.SetValue(startRow, 5, carExcelInfoViewModel.Vin.ToString());
            worksheet.SetValue(startRow, 6, carExcelInfoViewModel.Mileage.ToString());
            worksheet.SetValue(startRow, 7, carExcelInfoViewModel.ExteriorColor.ToString());
            worksheet.SetValue(startRow, 8, carExcelInfoViewModel.SalePrice.ToString());
            worksheet.SetValue(startRow, 9, carExcelInfoViewModel.Days);
            worksheet.SetValue(startRow, 10, carExcelInfoViewModel.imagesNum);
        }

        public static byte[] ExportToCSVForRebate(List<CarInfoFormViewModel> carList, int dealerId, string Title)
        {
            var package = new ExcelPackage();
            // add a new worksheet to the empty workbook
            var worksheet = package.Workbook.Worksheets.Add(Title);
            //Add the headers
            worksheet.Cells[1, 1].Value = "Year";
            worksheet.Cells[1, 2].Value = "Make";
            worksheet.Cells[1, 3].Value = "Model";
            worksheet.Cells[1, 4].Value = "Trim";
            worksheet.Cells[1, 5].Value = "Stock#";
            worksheet.Cells[1, 6].Value = "Vin";
            worksheet.Cells[1, 7].Value = "Mileage";
            worksheet.Cells[1, 8].Value = "MSRP";
            worksheet.Cells[1, 9].Value = "Rebate";
            worksheet.Cells[1, 10].Value = "Internet Price";

            //Set column width
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 10;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 9;
            worksheet.Column(5).Width = 10;
            worksheet.Column(6).Width = 17;
            worksheet.Column(7).Width = 5;
            worksheet.Column(8).Width = 7;
            worksheet.Column(9).Width = 4;
            worksheet.Column(10).Width = 12;


            //Set NumberFormat
            //worksheet.Column(6).Style.Numberformat.Format = "$#,##0.00";

            using (ExcelRange row = worksheet.Cells["A1:J1"])
            {
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(23, 55, 93));
                row.Style.Font.Color.SetColor(System.Drawing.Color.White);
                row.Style.Font.Size = 9;
                row.Style.Font.Bold = true;
            }

            //Switch the PageLayoutView back to normal
            //worksheet.View.PageLayoutView = true;

            // lets set the header text 
            worksheet.HeaderFooter.oddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\"" + Title;
            // add the page number to the footer plus the total number of pages
            worksheet.HeaderFooter.oddFooter.RightAlignedText =
                string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
            // add the sheet name to the footer
            worksheet.HeaderFooter.oddFooter.CenteredText = ExcelHeaderFooter.SheetName;
            // add the file path to the footer
            //worksheet.HeaderFooter.oddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

            worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:1"];
            worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

            package.Workbook.Properties.Title = Title;

            worksheet = package.Workbook.Worksheets[1];



            //PlUG DATA

            int numberOfRow = worksheet.Dimension.End.Row - worksheet.Dimension.Start.Row;
            int startRow = numberOfRow + 2;

            foreach (var drRow in carList)
            {
                worksheet.SetValue(startRow, 1, drRow.ModelYear.ToString());
                worksheet.SetValue(startRow, 2, drRow.Make.ToString());
                worksheet.SetValue(startRow, 3, drRow.Model.ToString());
                worksheet.SetValue(startRow, 4, drRow.Trim.ToString());
                worksheet.SetValue(startRow, 5, drRow.StockNumber.ToString());
                worksheet.SetValue(startRow, 6, drRow.Vin.ToString());
                worksheet.SetValue(startRow, 7, drRow.Mileage.ToString());
                worksheet.SetValue(startRow, 8, drRow.MSRP.ToString());
                worksheet.SetValue(startRow, 9, drRow.ManufacturerRebate.ToString());
                worksheet.SetValue(startRow, 10, drRow.SalePrice.ToString());


                startRow++;
            }


            return package.GetAsByteArray();
        }
    }
}