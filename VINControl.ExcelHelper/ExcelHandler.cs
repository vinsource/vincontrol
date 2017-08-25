using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VINControl.ExcelHelper
{
    public class ExcelHandler
    {
        public static DataFeedResults ProcessDataFeed(string filePath) 
        {
            var result = new DataFeedResults() { };
            if (!File.Exists(filePath))
            {
                result.Message = GetEnumDescription(ExcelMessage.FileNotExist);
                return result;
            }

            try
            {
                var fi = new FileInfo(filePath);

                var workBook = new XLWorkbook(fi.FullName);
                var worksheet = workBook.Worksheet(1);

                var headerRow = worksheet.FirstRowUsed();
                var firstHeaderColumn = headerRow.Cell(2).GetString();
                while (!firstHeaderColumn.Equals("Stock\nNo."))
                {
                    headerRow = headerRow.RowBelow();
                    firstHeaderColumn = headerRow.Cell(2).GetString();
                }
                               

                //Get the column names from header row of excel
                //var keyValues = new Dictionary<int, string>();
                //for (int cell = 2; cell <= headerRow.CellCount(); cell++)
                //{
                //    keyValues.Add(cell, headerRow.Cell(cell).GetString());
                //}

                int valueId = 1;
                //Get the next row
                headerRow = headerRow.RowBelow();
                while (!headerRow.Cell(valueId).IsEmpty())
                {
                    int count = 1;
                    //var pc = new ExpandoObject();
                    var listOfValues = new List<string>();
                    while (count <= headerRow.CellCount())
                    {
                        //var data = headerRow.Cell(count).Value;
                        //((IDictionary<string, object>)pc).Add(keyValues[count], data);
                        listOfValues.Add(headerRow.Cell(count).Value.ToString());
                        count++;
                    }
                    headerRow = headerRow.RowBelow();

                    result.Data.Add(new DataFeedExcelObj() 
                    {
                        Stock = listOfValues[0],
                        Type = listOfValues[1],
                        Year = String.IsNullOrEmpty(listOfValues[2]) ? 0 : Convert.ToInt32(listOfValues[2]),
                        Make = listOfValues[3],
                        Model = listOfValues[4],
                        ModelNo = listOfValues[5],
                        ModelType = listOfValues[6],
                        BodyStyle = listOfValues[7],
                        Color = listOfValues[8],
                        Engine = listOfValues[9],
                        Retail = String.IsNullOrEmpty(listOfValues[10]) ? 0 : Convert.ToDecimal(listOfValues[10]),
                        Invoice = String.IsNullOrEmpty(listOfValues[11]) ? 0 : Convert.ToDecimal(listOfValues[11]),
                        Lot = String.IsNullOrEmpty(listOfValues[12]) ? 0 : Convert.ToInt32(listOfValues[12]),
                        Co = String.IsNullOrEmpty(listOfValues[13]) ? 0 : Convert.ToInt32(listOfValues[13]),
                        Age = String.IsNullOrEmpty(listOfValues[14]) ? 0 : Convert.ToInt32(listOfValues[14]),
                        Status = listOfValues[15],
                        VIN = listOfValues[16],
                        DealNo = listOfValues[17],
                        Mileage = String.IsNullOrEmpty(listOfValues[18]) ? 0 : Convert.ToInt32(listOfValues[18])
                    });
                }
            }
            catch (Exception)
            {
                result.Message = GetEnumDescription(ExcelMessage.Error);
                return result;
            }
            
            result.Message = GetEnumDescription(ExcelMessage.Success);
            return result;
        }

        private static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
