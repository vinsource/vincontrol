using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace Vincontrol.Web.ReportTemplates
{
    public partial class ProfitManagementReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //ReportViewer1.ShowExportControls = false;

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
    }
}