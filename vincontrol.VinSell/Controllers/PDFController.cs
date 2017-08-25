using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiQPdf;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.Chart;
using vincontrol.VinSell.Handlers;

namespace vincontrol.VinSell.Controllers
{
    public class PDFController : BaseController
    {
        //
        // GET: /PDF/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrintGraphInfo(string content)
        {
            var result = GetGraphData(content);
            return GetPDFStream(result, "GraphInfo", result.DealshipName);
            //return View("GraphInfo", result);
        }

        private ContentViewModel GetGraphData(string content)
        {
            var dealer = (UserViewModel)SessionHandler.User;
            return new ContentViewModel { Text = string.Format("{0}", HttpUtility.HtmlDecode(content)), DealshipName = dealer.DealerName };
        }

        private ActionResult GetPDFStream(object result, string viewName, string dealershipName)
        {
            string htmlToConvert = RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealershipName);
            return GetStream(pdfDocument.WriteToMemory());
        }

        public static void FormatHeader(PdfDocument pdfDocument, string dealershipName)
        {
            FormatHeader(pdfDocument, dealershipName, true);
        }

        private static void FormatHeader(PdfDocument pdfDocument, string dealershipName, bool showDateTime)
        {
            pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, 10);
            Font sysFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            PdfFont pdfFont = pdfDocument.CreateFont(sysFont);
            PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);

            if (showDateTime)
            {
                pdfDocument.Header.Layout(new PdfText() { Text = DateTime.Now.ToShortDateString(), TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Right });
            }
            pdfDocument.Header.Layout(new PdfText() { Text = dealershipName, TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Center });
        }

        private ActionResult GetStream(byte[] pdfBuffer)
        {
            var workStream = new MemoryStream();
            workStream.Write(pdfBuffer, 0, pdfBuffer.Length);
            workStream.Position = 0;
            string fileName = "Report-" + DateTime.Now.Millisecond + ".pdf";
            HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + fileName);
            return new FileStreamResult(workStream, "application/pdf");
        }

        public static string RenderViewAsString(string viewName, object model, ControllerContext controllerContext)
        {
            if (model != null)
            {
                // create a string writer to receive the HTML code
                StringWriter stringWriter = new StringWriter();

                // get the view to render
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
                // create a context to render a view based on a model
                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), stringWriter);
                // render the view to a HTML code
                viewResult.View.Render(viewContext, stringWriter);

                // return the HTML code
                return stringWriter.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        internal static void ConfigureConverter(HtmlToPdf htmlToPdfConverter)
        {
            htmlToPdfConverter.SerialNumber = ConfigurationManager.AppSettings["PDFSerialNumber"];
            // set browser width
            htmlToPdfConverter.BrowserWidth = 1200;

            // set HTML Load timeout
            //htmlToPdfConverter.HtmlLoadedTimeout = int.Parse(textBoxLoadHtmlTimeout.Text);

            // set PDF page size and orientation
            htmlToPdfConverter.Document.PageSize = GetSelectedPageSize("Letter");
            htmlToPdfConverter.Document.PageOrientation = GetSelectedPageOrientation("Portrait");

            // set the PDF standard used by the document
            htmlToPdfConverter.Document.PdfStandard = PdfStandard.PdfA;

            // set PDF page margins            htmlToPdfConverter.Document.Margins = new PdfMargins(-1, 0, 1, 0);
            htmlToPdfConverter.Document.Margins = new PdfMargins(5);


            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;
            htmlToPdfConverter.Document.Header.Enabled = true;
            // set triggering mode; for WaitTime mode set the wait time before convert
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
        }

        private static PdfPageSize GetSelectedPageSize(string pageSize)
        {
            switch (pageSize)
            {
                case "A0":
                    return PdfPageSize.A0;
                case "A1":
                    return PdfPageSize.A1;
                case "A10":
                    return PdfPageSize.A10;
                case "A2":
                    return PdfPageSize.A2;
                case "A3":
                    return PdfPageSize.A3;
                case "A4":
                    return PdfPageSize.A4;
                case "A5":
                    return PdfPageSize.A5;
                case "A6":
                    return PdfPageSize.A6;
                case "A7":
                    return PdfPageSize.A7;
                case "A8":
                    return PdfPageSize.A8;
                case "A9":
                    return PdfPageSize.A9;
                case "ArchA":
                    return PdfPageSize.ArchA;
                case "ArchB":
                    return PdfPageSize.ArchB;
                case "ArchC":
                    return PdfPageSize.ArchC;
                case "ArchD":
                    return PdfPageSize.ArchD;
                case "ArchE":
                    return PdfPageSize.ArchE;
                case "B0":
                    return PdfPageSize.B0;
                case "B1":
                    return PdfPageSize.B1;
                case "B2":
                    return PdfPageSize.B2;
                case "B3":
                    return PdfPageSize.B3;
                case "B4":
                    return PdfPageSize.B4;
                case "B5":
                    return PdfPageSize.B5;
                case "Flsa":
                    return PdfPageSize.Flsa;
                case "HalfLetter":
                    return PdfPageSize.HalfLetter;
                case "Ledger":
                    return PdfPageSize.Ledger;
                case "Legal":
                    return PdfPageSize.Legal;
                case "Letter":
                    return PdfPageSize.Letter;
                case "Letter11x17":
                    return PdfPageSize.Letter11x17;
                case "Note":
                    return PdfPageSize.Note;
                default:
                    return PdfPageSize.A4;
            }
        }

        private static PdfPageOrientation GetSelectedPageOrientation(string pageOrientation)
        {
            return (pageOrientation == "Portrait") ?
                PdfPageOrientation.Portrait : PdfPageOrientation.Landscape;
        }



    }
}
