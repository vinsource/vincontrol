using System;
using System.Web.Mvc;
//For converting HTML TO PDF- START
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Security;


namespace WhitmanEnterpriseMVC.Controllers
{
    public class MarketReportController : SecurityController
    {
        //
        // GET: /MarketReport/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewKBBReport(string kbbVehicleId, int listingId, int trimId, int PrintOption)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            var reader = new StringReader(KellyBlueBookHelper.BuildKBBReportInHtml(listingId, kbbVehicleId, trimId, dealer, PrintOption));
            var workStream = new MemoryStream();

            var document = new Document(PageSize.A4);

            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            var worker = new HTMLWorker(document);

            document.Open();
            worker.StartDocument();
            worker.Parse(reader);
            worker.EndDocument();
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
        public ActionResult ViewKBBReportInAppraisal(string kbbVehicleId, int appraisalId, int trimId, int PrintOption)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            var reader = new StringReader(KellyBlueBookHelper.BuildKBBReportInHtmlForAppraisal(appraisalId, kbbVehicleId, trimId, dealer, PrintOption));

            var workStream = new MemoryStream();

            var document = new Document(PageSize.A4);

            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            var worker = new HTMLWorker(document);

            document.Open();
            worker.StartDocument();
            worker.Parse(reader);
            worker.EndDocument();
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

     

    }
}
