using System;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiQPdf;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using vincontrol.Helper;
using Vincontrol.Web.HelperClass;

namespace Vincontrol.Web.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult GetBodyBrochure(int userID, int dealerID, int year, string make, string model,
                                            string photoUrl)
        {
            if (string.IsNullOrEmpty(photoUrl))
                photoUrl = string.Format("{0}/Content/images/vincontrol/car_default.png",
                                         System.Web.Configuration.WebConfigurationManager.AppSettings["WebServerURL"
                                             ]);

            var content = GenerateBrochureStringContentForService(userID, dealerID,
                                                                  year, make, model, photoUrl);
            string dealerName = string.Empty;
            using (var context = new VincontrolEntities())
            {
                var dealer = context.Dealers.FirstOrDefault(d => d.DealerId == dealerID);
                dealerName = dealer.Name;
            }
            return File(GenerateFlyerByteContent(content, dealerName), "application/pdf");
        }

        private string GenerateBrochureStringContentForService(int userID, int dealerID, int year, string make, string model, string photoUrl)
        {
            try
            {
                {
                    string htmlToConvert = string.Empty;
                    using (var context = new VincontrolEntities())
                    {
                        CarInfoFormViewModel inventoryViewModel;
                        var user = context.Users.FirstOrDefault(u => u.UserId == userID);
                        var dealer = context.Dealers.FirstOrDefault(d => d.DealerId == dealerID);
                        var inventory =
                            context.Inventories.FirstOrDefault(
                                i =>
                                i.Vehicle.Year == year && i.Vehicle.Make == make && i.Vehicle.Model == model &&
                                i.DealerId == dealerID);
                        if (inventory != null)
                        {
                            inventoryViewModel = new CarInfoFormViewModel(inventory);
                            inventoryViewModel.Dealer = dealer;
                            inventoryViewModel.CurrentUser = user;
                            htmlToConvert = PDFRender.RenderViewAsString("brochure", inventoryViewModel, ControllerContext);
                        }
                        else
                        {
                            inventoryViewModel = new CarInfoFormViewModel();
                            inventoryViewModel.Dealer = dealer;
                            inventoryViewModel.CurrentUser = user;
                            inventoryViewModel.ModelYear = year;
                            inventoryViewModel.Make = make;
                            inventoryViewModel.Model = model;
                            inventoryViewModel.Trim = string.Empty;
                            inventoryViewModel.SinglePhoto = HttpUtility.UrlDecode(photoUrl);
                            htmlToConvert = PDFRender.RenderViewAsString("brochure", inventoryViewModel, ControllerContext);
                        }
                    }
                    return htmlToConvert;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public byte[] GenerateFlyerByteContent(string htmlToConvert, string dealerName)
        {
            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();

            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealerName);
            return (pdfDocument.WriteToMemory());
        }

        private static void FormatHeader(PdfDocument pdfDocument, string dealershipName)
        {
            FormatHeader(pdfDocument, dealershipName, true);
        }

        private static void FormatHeader(PdfDocument pdfDocument, string dealershipName, bool showDateTime)
        {
            pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, 10);
            var sysFont = new Font("Times New Roman", 10, GraphicsUnit.Point);
            //pdfDocument.CreateFont(sysFont);
            PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);

            if (showDateTime)
            {
                pdfDocument.Header.Layout(new PdfText { Text = DateTime.Now.ToShortDateString(), TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Right });
            }
            pdfDocument.Header.Layout(new PdfText { Text = dealershipName, TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Center });
        }
    }
}
