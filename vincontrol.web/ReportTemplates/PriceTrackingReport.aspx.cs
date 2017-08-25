using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.ReportModel;
using Vincontrol.Web.DatabaseModel;
using System.Linq;

namespace Vincontrol.Web.ReportTemplates
{
    public partial class PriceTrackingReport : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            YearDropDownList.SelectedIndexChanged += YearDropDownList_SelectedIndexChanged;
            MakeDropDownList.SelectedIndexChanged += MakeDropDownList_SelectedIndexChanged;
            base.OnInit(e);
        }

        void MakeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
          var context = new VincontrolEntities();
            int selectedYear;
            int.TryParse(YearDropDownList.SelectedValue, out selectedYear);
            var firstModelList = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(x => x.Vehicle.Year == selectedYear && x.Vehicle.Make== MakeDropDownList.SelectedValue).Select(x => x.Vehicle.Model).ToList();
            var secondModelList = InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context).Where(x => x.Vehicle.Year == selectedYear && x.Vehicle.Make == MakeDropDownList.SelectedValue).Select(x => x.Vehicle.Model);

            firstModelList.AddRange(secondModelList);

            var modelList = new List<string>();

            modelList.Insert(0, string.Empty);

            var hashSet = new HashSet<string>();

            foreach (var tmp in firstModelList)
            {
                var modelName = tmp.ToLower();

                if (!hashSet.Contains(modelName))
                    modelList.Add(tmp);

                hashSet.Add(modelName);
            }
            
            ModelDropDownList.DataSource = modelList;
            ModelDropDownList.DataBind();
        }

        void YearDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            var context = new VincontrolEntities();
            int selectedYear;
            int.TryParse(YearDropDownList.SelectedValue, out selectedYear);
            var firstMakeList = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(x=>x.Vehicle.Year==selectedYear).Select(x => x.Vehicle.Make).ToList();
            var secondMakeList = InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context).Where(x => x.Vehicle.Year == selectedYear).Select(x => x.Vehicle.Make);

            firstMakeList.AddRange(secondMakeList);

            var makeList = new List<string>();

            makeList.Insert(0, string.Empty);

            var hashSet = new HashSet<string>();

            foreach (var tmp in firstMakeList)
            {
                var makeName = tmp.ToLower();

                if (!hashSet.Contains(makeName))
                    makeList.Add(tmp);

                hashSet.Add(makeName);
            }

            
            MakeDropDownList.DataSource = makeList;
            MakeDropDownList.DataBind();
            ModelDropDownList.Items.Clear();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var context = new VincontrolEntities();
                var firstYearList = InventoryQueryHelper.GetSingleOrGroupInventory(context).Select(x => x.Vehicle.Year).ToList();
                var secondYearList = InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context).Select(x => x.Vehicle.Year);

                firstYearList.AddRange(secondYearList);

                var yearList = new List<string>();

                yearList.Insert(0,string.Empty);

                yearList.AddRange(firstYearList.Where(i=>i.HasValue).Distinct().OrderByDescending(i => i.Value).ToList().Select(i =>  i.Value.ToString(CultureInfo.InvariantCulture)).ToList());

                YearDropDownList.DataSource = yearList;
                YearDropDownList.DataBind();
            }
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


            Response.AddHeader("Accept-Header", bytes.Length.ToString(CultureInfo.InvariantCulture));
            Response.ContentType = mimeType;

            Response.OutputStream.Write(bytes, 0, Convert.ToInt32(bytes.Length));

            Response.Flush();
            Response.End();
        }



        protected void btnFilter_Click(object sender, EventArgs e)
        {

            var source = new VinControlReport();
            var month = int.Parse(MonthDropDownList.SelectedValue);
            var dealer = SessionHandler.Dealer;
            int year;
            int.TryParse(YearDropDownList.SelectedValue, out year);
           
            var dataSource = source.GetHistoryChanged(dealer.DealershipId, month, year, MakeDropDownList.SelectedValue, ModelDropDownList.SelectedValue, txtStock.Text, VINNumberTextBox.Text);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("HistoryChanged", dataSource));
            ReportViewer1.LocalReport.ReportPath = @"ReportTemplates\PriceTrackingReport.rdlc";
          
        }

        protected void btnRest_Click(object sender, EventArgs e)
        {
            txtStock.Text = "";
            VINNumberTextBox.Text = "";
            MakeDropDownList.SelectedValue = "";
            ModelDropDownList.SelectedValue = "";
            YearDropDownList.SelectedValue = "";
            MonthDropDownList.SelectedValue = "";
        }
    }
}