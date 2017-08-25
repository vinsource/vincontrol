using System;
using System.Configuration;
using System.IO;
using System.Text;
﻿using HiQPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace vincontrol.Helper
{
    public class PDFHelper
    {
        #region oldCode
       public static byte[] GeneratePdfFromHtmlCode(string htmlCode)
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
        
        public static void ConfigureConverter(HtmlToPdf htmlToPdfConverter)
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

        public static void ConfigureStickerWithTemplateConverter(HtmlToPdf htmlToPdfConverter)
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
            htmlToPdfConverter.Document.Margins = new PdfMargins(36, 36, -14, 0);

            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;
            htmlToPdfConverter.Document.Header.Enabled = true;
            // set triggering mode; for WaitTime mode set the wait time before convert
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
        }




        public static void ConfigureConverterForBuyerGuide(HtmlToPdf htmlToPdfConverter)
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
            var reader = new StreamReader(bodyPDF);
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

        public static void ConfigureAppraisalConverter(HtmlToPdf htmlToPdfConverter)
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

        public static void ConfigureBucketJumpConverter(HtmlToPdf htmlToPdfConverter)
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