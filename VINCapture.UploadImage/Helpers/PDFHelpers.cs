using System;
using System.Configuration;
using HiQPdf;

namespace VINCapture.UploadImage.USBHelpers
{
    public class PDFHelpers
    {
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
    }
}
