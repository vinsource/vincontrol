﻿using System;
﻿using System.Configuration;
﻿using HiQPdf;
using System.Text;
using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class PDFHelper
    {
        #region oldCode
        //        private void SetHeader(PdfDocumentControl htmlToPdfDocument)
        //        {
        //            // enable header display
        //            //htmlToPdfDocument.Header.Enabled = checkBoxAddHeader.Checked;

        //            if (!htmlToPdfDocument.Header.Enabled)
        //                return;

        //            // set header height
        //            htmlToPdfDocument.Header.Height = 50;

        //            float pdfPageWidth = htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
        //                                        htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;

        //            float headerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left - htmlToPdfDocument.Margins.Right;
        //            float headerHeight = htmlToPdfDocument.Header.Height;

        //            // set header background color
        //            htmlToPdfDocument.Header.BackgroundColor = Color.WhiteSmoke;

        //            string headerImageFile = Application.StartupPath + @"\DemoFiles\Images\HiQPdfLogo.png";
        //            PdfImage logoHeaderImage = new PdfImage(5, 5, 40, Image.FromFile(headerImageFile));
        //            // use alpha blending to render a transparent image if the document was not restricted to PDF/A standard
        //            logoHeaderImage.AlphaBlending = !checkBoxPdfA.Checked;
        //            htmlToPdfDocument.Header.Layout(logoHeaderImage);

        //            // layout HTML in header
        //            PdfHtml headerHtml = new PdfHtml(50, 5, 0, headerHeight,
        //                            @"<span style=""color:Navy; font-family:Times New Roman; font-style:italic"">
        //                            Quickly Create High Quality PDFs with </span><a href=""http://www.hiqpdf.com"">HiQPdf</a>", null);
        //            headerHtml.FitDestHeight = true;
        //            //headerHtml.FontEmbedding = checkBoxFontEmbedding.Checked;
        //            htmlToPdfDocument.Header.Layout(headerHtml);

        //            // create a border for header

        //            PdfRectangle borderRectangle = new PdfRectangle(1, 1, headerWidth - 2, headerHeight - 2);
        //            borderRectangle.LineStyle.LineWidth = 0.5f;
        //            borderRectangle.ForeColor = Color.Navy;
        //            htmlToPdfDocument.Header.Layout(borderRectangle);
        //        }

        //        private void SetFooter(PdfDocumentControl htmlToPdfDocument)
        //        {
        //            // enable footer display
        //            //htmlToPdfDocument.Footer.Enabled = checkBoxAddFooter.Checked;

        //            if (!htmlToPdfDocument.Footer.Enabled)
        //                return;

        //            // set footer height
        //            htmlToPdfDocument.Footer.Height = 50;

        //            // set footer background color
        //            htmlToPdfDocument.Footer.BackgroundColor = Color.WhiteSmoke;

        //            float pdfPageWidth = htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
        //                                        htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;

        //            float footerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left - htmlToPdfDocument.Margins.Right;
        //            float footerHeight = htmlToPdfDocument.Footer.Height;

        //            // layout HTML in footer
        //            PdfHtml footerHtml = new PdfHtml(5, 5, 0, footerHeight,
        //                            @"<span style=""color:Navy; font-family:Times New Roman; font-style:italic"">
        //                            Quickly Create High Quality PDFs with </span><a href=""http://www.hiqpdf.com"">HiQPdf</a>", null);
        //            footerHtml.FitDestHeight = true;
        //            //footerHtml.FontEmbedding = checkBoxFontEmbedding.Checked;
        //            htmlToPdfDocument.Footer.Layout(footerHtml);

        //            // add page numbering
        //            Font pageNumberingFont = new Font(new FontFamily("Times New Roman"), 8, GraphicsUnit.Point);
        //            //pageNumberingFont.Mea
        //            PdfText pageNumberingText = new PdfText(5, footerHeight - 12, "Page {CrtPage} of {PageCount}", pageNumberingFont);
        //            pageNumberingText.HorizontalAlign = PdfTextHAlign.Center;
        //            pageNumberingText.EmbedSystemFont = true;
        //            pageNumberingText.ForeColor = Color.DarkGreen;
        //            htmlToPdfDocument.Footer.Layout(pageNumberingText);

        //            string footerImageFile = Application.StartupPath + @"\DemoFiles\Images\HiQPdfLogo.png";
        //            PdfImage logoFooterImage = new PdfImage(footerWidth - 40 - 5, 5, 40, Image.FromFile(footerImageFile));
        //            // use alpha blending to render a transparent image if the document was not restricted to PDF/A standard
        //            logoFooterImage.AlphaBlending = !checkBoxPdfA.Checked;
        //            htmlToPdfDocument.Footer.Layout(logoFooterImage);

        //            // create a border for footer
        //            PdfRectangle borderRectangle = new PdfRectangle(1, 1, footerWidth - 2, footerHeight - 2);
        //            borderRectangle.LineStyle.LineWidth = 0.5f;
        //            borderRectangle.ForeColor = Color.DarkGreen;
        //            htmlToPdfDocument.Footer.Layout(borderRectangle);
        //        }
        public static byte[] GeneratePDFFromHtmlCode(string htmlCode)
        {
            var htmlToPdfConverter = new HtmlToPdf();

            byte[] byteInfo;

            // set a demo serial number
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

            // set PDF page margins
            htmlToPdfConverter.Document.Margins = new PdfMargins(-1, 0, 1, 0);

            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;

            // set triggering mode; for WaitTime mode set the wait time before convert
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            //switch (comboBoxTriggeringMode.SelectedItem.ToString())
            //{
            //    case "Auto":
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            //        break;
            //    case "WaitTime":
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.WaitTime;
            //        htmlToPdfConverter.WaitBeforeConvert = int.Parse(textBoxWaitTime.Text);
            //        break;
            //    case "Manual":
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Manual;
            //        break;
            //    default:
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            //        break;
            //}

            // set header and footer
            //SetHeader(htmlToPdfConverter.Document);
            //SetFooter(htmlToPdfConverter.Document);

            // set the document security
            //htmlToPdfConverter.Document.Security.OpenPassword = textBoxOpenPassword.Text.Trim();
            //htmlToPdfConverter.Document.Security.AllowPrinting = checkBoxAllowPrinting.Checked;

            // convert HTML to PDF
            //string pdfFile = null;
            try
            {
                // convert HTML code
                byteInfo = htmlToPdfConverter.ConvertHtmlToMemory(htmlCode, "");



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

            return byteInfo;
        }


        public static byte[] GeneratePDFFromHtml(string url)
        {
            var htmlToPdfConverter = new HtmlToPdf();

            byte[] byteInfo;

            // set a demo serial number
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

            // set PDF page margins
            htmlToPdfConverter.Document.Margins = new PdfMargins(-1, 0, 1, 0);

            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;

            // set triggering mode; for WaitTime mode set the wait time before convert
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            //switch (comboBoxTriggeringMode.SelectedItem.ToString())
            //{
            //    case "Auto":
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            //        break;
            //    case "WaitTime":
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.WaitTime;
            //        htmlToPdfConverter.WaitBeforeConvert = int.Parse(textBoxWaitTime.Text);
            //        break;
            //    case "Manual":
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Manual;
            //        break;
            //    default:
            //        htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            //        break;
            //}

            // set header and footer
            //SetHeader(htmlToPdfConverter.Document);
            //SetFooter(htmlToPdfConverter.Document);

            // set the document security
            //htmlToPdfConverter.Document.Security.OpenPassword = textBoxOpenPassword.Text.Trim();
            //htmlToPdfConverter.Document.Security.AllowPrinting = checkBoxAllowPrinting.Checked;

            // convert HTML to PDF
            //string pdfFile = null;
            try
            {
                // convert HTML code
                //byteInfo = htmlToPdfConverter.ConvertHtmlToMemory(HtmlCode, "");

                byteInfo = htmlToPdfConverter.ConvertUrlToMemory(url);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

            return byteInfo;
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

        #endregion

        public static string WriteHTMLCode()
        {
            var result = new StringBuilder();

            result.Append(@"<div id=""printable-list"">");
            result.Append(@" <div id=""vehicle-list"" style=""font-size: .6em"">");
            result.Append(@"<table>");
            result.Append(@"<tbody>");
            WriteHeader(result);
            WriteContent(result);
            result.Append(@"</tbody>");
            result.Append(@"</table>");
            result.Append(@" </div>");
            result.Append(@" </div>");

            return result.ToString();
        }

        private static void WriteHeader(StringBuilder result)
        {
            throw new NotImplementedException();
        }

        private static void WriteContent(StringBuilder result)
        {
            result.Append(@"<tr>");
            result.Append(@"<td>");
            result.Append(@"Year");
            result.Append(@"</td>");
            result.Append(@"</tr>");
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

        internal static void ConfigureConverterForBuyerGuide(HtmlToPdf htmlToPdfConverter)
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
            htmlToPdfConverter.Document.Margins = new PdfMargins(-25, 10, -70, 0);

            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;
            htmlToPdfConverter.Document.Header.Enabled = true;
            // set triggering mode; for WaitTime mode set the wait time before convert
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
        }
        public static MemoryStream WritePDF(string bodyPDF)
        {
            var workStream = new MemoryStream();
            var reader = new StringReader(bodyPDF);
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
            return workStream;
        }

        internal static void ConfigureAppraisalConverter(HtmlToPdf htmlToPdfConverter)
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
            htmlToPdfConverter.Document.Margins = new PdfMargins(0, 0, -37, 0);

            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;
            htmlToPdfConverter.Document.Header.Enabled = true;
            // set triggering mode; for WaitTime mode set the wait time before convert
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
        }

        internal static void ConfigureBucketJumplConverter(HtmlToPdf htmlToPdfConverter)
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
            htmlToPdfConverter.Document.PdfStandard = PdfStandard.Pdf;

            // set PDF page margins            htmlToPdfConverter.Document.Margins = new PdfMargins(-1, 0, 1, 0);
            //htmlToPdfConverter.Document.Margins = new PdfMargins(20, 0, 0, 0);

            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;
            htmlToPdfConverter.Document.Header.Enabled = true;
            htmlToPdfConverter.Document.Header.DisplayOnFirstPage = false;
            // set triggering mode; for WaitTime mode set the wait time before convert
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
        }
    }
}