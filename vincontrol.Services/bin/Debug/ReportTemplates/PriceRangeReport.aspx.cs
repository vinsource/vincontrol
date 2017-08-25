using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WebForms;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.ReportModel;


namespace WhitmanEnterpriseMVC.ReportTemplates
{
    public partial class PriceRangeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];
            var report = new VinControlReport();
            //ReportViewer1.ShowExportControls = false;
            var source = report.GetVinControlUsedVehicles(dealer.DealershipId);
            lbl0.Text = GetNumberOfItems(source, 0, 9999);
            lbl1000.Text = GetNumberOfItems(source, 10000, 19999);
            lbl2000.Text = GetNumberOfItems(source, 20000, 29999);
            lbl3000.Text = GetNumberOfItems(source, 30000, 39999);
            lbl4000.Text = GetNumberOfItems(source, 40000, 49999);
            lbl5000.Text = GetNumberOfItems(source, 50000, 69999);
            lbl7000.Text = GetNumberOfItems(source, 70000, null);
        }

        private string GetNumberOfItems(IEnumerable<VinControlVehicleReport> source, int min, int? max)
        {
            long result = 0;
            return max.HasValue ? source.Count(i => long.TryParse(i.SalePrice, out result) && result >= min && result <= max.Value).ToString() : source.Count(i => long.TryParse(i.SalePrice, out result) && result >= min).ToString();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string mimeType;
            string encoding;
            string fileNameExtension;
            string[] streams;

            Warning[] warnings;
            byte[] bytes = ReportViewer1.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);


            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;


            Response.AddHeader("Accept-Header", bytes.Length.ToString());
            Response.ContentType = mimeType;

            Response.OutputStream.Write(bytes, 0, Convert.ToInt32(bytes.Length));

            Response.Flush();
            Response.End();


        }

        protected void btn0_Click(object sender, EventArgs e)
        {
            UpdateReport(0, 9999);
        }

        private void UpdateReport(int min, int? max)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];
            var source = new VinControlReport();
            var dataSource = source.GetVinControlUsedVehiclesRange(dealer.DealershipId, min, max);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("VincontrolReportSource", dataSource));
            ReportViewer1.LocalReport.ReportPath = @"ReportTemplates\PriceRangeReport.rdlc";
        }

        protected void btn1000_Click(object sender, EventArgs e)
        {
            UpdateReport(10000, 19999);
        }

        protected void btn2000_Click(object sender, EventArgs e)
        {
            UpdateReport(20000, 29999);
        }

        protected void btn3000_Click(object sender, EventArgs e)
        {
            UpdateReport(30000, 39999);

        }

        protected void btn4000_Click(object sender, EventArgs e)
        {
            UpdateReport(40000, 49999);
        }

        protected void btn5000_Click(object sender, EventArgs e)
        {
            UpdateReport(50000, 69999);
        }

        protected void btn7000_Click(object sender, EventArgs e)
        {
            UpdateReport(70000, null);
        }


    }
}