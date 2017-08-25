using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HiQPdf;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.EmailHelper;
using vincontrol.Helper;
using Font = System.Drawing.Font;
using Inventory = vincontrol.Data.Model.Inventory;
using PdfDocument = HiQPdf.PdfDocument;
using PdfFont = HiQPdf.PdfFont;
using PdfImage = HiQPdf.PdfImage;
using PdfPage = HiQPdf.PdfPage;

namespace Vincontrol.Marketing
{
    public class FakeController : Controller
    {

    }

    
    public class Marketing : IMarketing
    {
      
        public override void SendFlyer(int inventoryId, string emails, string names, int userId)
        {
             IEmail emailWrapper=new Email();
            using (var context = new VincontrolEntities())
            {
                var existingInventory = context.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
                var user = context.Users.FirstOrDefault(i => i.UserId == userId);
            
                var content = GenerateFlyerStringContent(existingInventory);
                var workStream = GetStreamFromByte(GenerateFlyerByteContent(existingInventory.Dealer.Name,content));

                TrackActivity(emails, names, existingInventory, user);

                if (existingInventory != null)
                {
                    var trim = existingInventory.Vehicle.Trim;
                    if (String.IsNullOrEmpty(trim))
                        trim = "Base";
                    var emailContent = EmailTemplateReader.GetFlyerEmailContent();
                    emailContent = emailContent.Replace(EmailTemplateReader.UserFullName, user.Name);
                    emailContent = emailContent.Replace(EmailTemplateReader.DealerName, existingInventory.Dealer.Name);
                    emailContent = emailContent.Replace(EmailTemplateReader.Year, existingInventory.Vehicle.Year.GetValueOrDefault().ToString());
                    emailContent = emailContent.Replace(EmailTemplateReader.Make, existingInventory.Vehicle.Make);
                    emailContent = emailContent.Replace(EmailTemplateReader.Model, existingInventory.Vehicle.Model);
                    emailContent = emailContent.Replace(EmailTemplateReader.Trim, existingInventory.Vehicle.Trim);
                    emailContent = emailContent.Replace(EmailTemplateReader.Phone, user.CellPhone);
                    emailContent = emailContent.Replace(EmailTemplateReader.Address, existingInventory.Dealer.Address + " " + existingInventory.Dealer.City + ", " + existingInventory.Dealer.State + " " + existingInventory.Dealer.ZipCode);
                    emailContent = emailContent.Replace(EmailTemplateReader.LandingPageURL,
                                                        String.Format("http://vinadvisor.com/Inventory/{0}/{1}-{2}-{3}-{4}/{5}",
                                                            existingInventory.Dealer.Name.ToLower().Replace(" ", ""),
                                                            existingInventory.Vehicle.Year,
                                                            existingInventory.Vehicle.Make.Replace(" ", ""),
                                                            existingInventory.Vehicle.Model.Replace(" ", ""),
                                                            trim,
                                                            existingInventory.Vehicle.Vin));
                    
                    emailWrapper.SendEmail(emails.Split(',').ToList(), "You received a flyer from " + existingInventory.Dealer.Name, emailContent, workStream);
                }
            }
        }

        public string GenerateFlyerStringContent(Inventory inventory)
        {
            ICarFaxService _carFaxService = new CarFaxService();
            try
            {
                {
                    var inventoryViewModel = inventory == null
                                                 ? new CarInfoFormViewModel()
                                                 : new CarInfoFormViewModel(inventory);

                    if (inventoryViewModel.Condition == Constanst.ConditionStatus.Used)
                    {
                        try
                        {
                            inventoryViewModel.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(inventory.Vehicle.VehicleId, inventory.Vehicle.Vin, inventory.Dealer.Setting.CarFax, inventory.Dealer.Setting.CarFaxPassword);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>(), Success = false };

                        inventoryViewModel.CarFax = carfax;
                      
                    }

                    //string htmlToConvert = RenderRazorViewToString(new FakeController(), "Flyer", inventoryViewModel);
                    return "";
                }
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }


        private MemoryStream GetStreamFromByte(byte[] pdfBuffer)
        {
            var workStream = new MemoryStream();
            workStream.Write(pdfBuffer, 0, pdfBuffer.Length);
            workStream.Position = 0;
            return workStream;
        }

        public byte[] GenerateFlyerByteContent(string dealerName,string htmlToConvert)
        {
            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();

            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealerName);
            return (pdfDocument.WriteToMemory());
        }

        public static void FormatHeader(PdfDocument pdfDocument, string dealershipName)
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


        private void TrackActivity(string emails, string names, Inventory existingInventory, User user)
        {
            var emailList = emails.Split(',').ToList();
            var nameList = names.Split(',').ToList();
            using (var context = new VincontrolEntities())
            {
                for (var i = 0; i < emailList.Count; i++)
                {
                    var activity = new FlyerShareDealerActivity
                    {
                        CustomerEmail = emailList[i],
                        CustomerName = nameList[i],
                        DateStamp = DateTime.Now,
                        UserStampId = user.UserId,
                        DealerId = existingInventory.Dealer.DealerId,
                        Year = existingInventory.Vehicle.Year,
                        Make = existingInventory.Vehicle.Make,
                        Model = existingInventory.Vehicle.Model,
                        Trim = existingInventory.Vehicle.Trim,
                        Stock = existingInventory.Stock,
                        VIN = existingInventory.Vehicle.Vin,
                        IsBrochure = false
                    };
                    context.AddToFlyerShareDealerActivities(activity);
                }
                context.SaveChanges();
            }
        }
    }
}
