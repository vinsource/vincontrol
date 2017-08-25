using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Vincontrol.Brochure.View;
using Vincontrol.Brochure.ViewModels;
using vincontrol.Data.Model;

using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Vincontrol.Brochure
{
    /// <summary>
    /// Interaction logic for UploadImage.xaml
    /// </summary>
    public partial class UploadImage : System.Windows.Window, IView
    {
        //private int SelectedYear
        //{
        //    get
        //    {
        //        return (int)YearCbb.SelectedItem;
        //    }
        //}

        //private int SelectedYearMakeId
        //{
        //    get
        //    {
        //        if (MakeCbb.SelectedItem != null)
        //            return ((YearMakeItem) MakeCbb.SelectedItem).YearMakeId;
        //         return 0;
                
        //    }
        //}

        //private string SelectedMakeName
        //{
        //    get
        //    {
        //        if (MakeCbb.SelectedItem != null)

        //        return ((YearMakeItem)MakeCbb.SelectedItem).MakeValue;
        //        return String.Empty;

        //    }
        //}

        //private int SelectedModelId
        //{
        //    get
        //    {
        //        if (ModelCbb.SelectedItem != null)

        //        return ((ModelItem)ModelCbb.SelectedItem).ModelId;
        //        return 0;

        //    }
        //}

        //private string SelectedModelName
        //{
        //    get
        //    {
        //        if (ModelCbb.SelectedItem != null)

        //        return ((ModelItem)ModelCbb.SelectedItem).ModelValue;
        //        return String.Empty;


        //    }
        //}

        //private int SelectedTrimId
        //{
        //    get
        //    {
        //        if (TrimCbb.SelectedItem != null)

        //        return ((TrimItem)TrimCbb.SelectedItem).TrimId;
        //         return 0;

        //    }
        //}

       

        public UploadImage()
        {
            InitializeComponent();
            Loaded += delegate
            {
                new TradeinValueViewModel(this);
            };
            //var year = ChromeHelper.GetYears();
            //YearCbb.ItemsSource = year;
        }

        //private void UpLoadImageBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    //Validate before upload
        //    Execute();
        //}

        //private void YearCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedYear = SelectedYear;
        //    MakeCbb.ItemsSource = ChromeHelper.GetMakes(selectedYear);
        //    EstimatedMileage.Text = GetEstimatedMileage(selectedYear).ToString(CultureInfo.InvariantCulture);
        //}

        //private void MakeCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //ModelCbb.ItemsSource = ChromeHelper.GetModels(SelectedYearMakeId);

        //}

        //private void ModelCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //TrimCbb.ItemsSource = ChromeHelper.GetTrims(SelectedModelId);
        //}

     

        //private void BtnSave_Click_1(object sender, RoutedEventArgs e)
        //{
        //    //var validateResult = ValidateData();
        //    //if (!validateResult.IsSuccessfull)
        //    //{
        //    //    ErrorMessage.Text = validateResult.ErrorMessage;
        //    //}
        //    //else
        //    //{
        //    //    ErrorMessage.Text = String.Empty;
        //    //    CreateData(SelectedTrimId);
        //    //}
        //}

       

        //private void UpdateData(int trimId)
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var tradein = context.TrimTradeIns.FirstOrDefault(i => i.TrimId == trimId);

        //    }
        //}

        //private ValidateResult ValidateData()
        //{
        //    var result = new ValidateResult { IsSuccessfull = false };
        //    if ((YearCbb.SelectedValue == null) || (MakeCbb.SelectedValue == null) || (ModelCbb.SelectedValue == null) ||
        //        (TrimCbb.SelectedValue == null) || (String.IsNullOrEmpty(EstimatedMileage.Text)) ||
        //        (String.IsNullOrEmpty(TradeInValue.Text)))
        //    {
        //        result.ErrorMessage = "Required fields";
        //        return result;
        //    }
        //    var tradeInValue = int.Parse(TradeInValue.Text);
        //    if (tradeInValue < 0)
        //    {
        //        result.ErrorMessage = "Trade in value should be positive";
        //        return result;
        //    }

        //    result.IsSuccessfull = true;
        //    return result;
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var list = ChromeHelper.GetTradeInReport();
            CreateExcelFile(list);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var list = ChromeHelper.GetSampleVinReport();
            CreateExcelFile(list);
        }

        private void CreateExcelFile(List<TradeInReport> list)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                Console.WriteLine("EXCEL could not be started. Check that your office installation and project references are correct.");
                return;
            }
            xlApp.Visible = true;

            Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = (Worksheet)wb.Worksheets[1];

            if (ws == null)
            {
                Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
            }

            // Select the Excel cells, in the range c1 to c7 in the worksheet.
            //Range aRange = ws.get_Range("C1", "C7");

            //if (aRange == null)
            //{
            //    Console.WriteLine("Could not get a range. Check to be sure you have the correct versions of the office DLLs.");
            //}

            //// Fill the cells in the C1 to C7 range of the worksheet with the number 6.
            //Object[] args = new Object[1];
            //args[0] = 6;
            //aRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, aRange, args);

            //// Change the cells in the C1 to C7 range of the worksheet to the number 8.
            //aRange.Value2 = 8;

            Range columYear = ws.get_Range("A1", "A1");
            columYear.Value = "Year";

            Range columMake = ws.get_Range("B1", "B1");
            columMake.Value = "Make";

            Range columModel = ws.get_Range("C1", "C1");
            columModel.Value = "Model";

            Range columTrim = ws.get_Range("D1", "D1");
            columTrim.Value = "Trim";

            for (int i = 2; i < list.Count; i++)
            {
                Range colum1 = ws.get_Range("A" + i, "A" + i);
                colum1.Value = list[i].Year;

                Range colum2 = ws.get_Range("B" + i, "B" + i);
                colum2.Value = list[i].Make;

                Range colum3 = ws.get_Range("C" + i, "C" + i);
                colum3.Value = list[i].Model;

                Range colum4 = ws.get_Range("D" + i, "D" + i);
                colum4.Value = list[i].TrimName;
            }
        }

        public void SetDataContext(object context)
        {
            DataContext = context;
        }
    }

    public class RequestResult
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public HttpWebRequest WebRequest { get; set; }
    }

    public class ValidateResult
    {
        public bool IsSuccessfull { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class TradeinItem
    {
        public int TrimId { get; set; }
        public long EstimatedMileage { get; set; }
        public decimal TradeinValue { get; set; }
        public string SampleVin { get; set; }
    }
}
