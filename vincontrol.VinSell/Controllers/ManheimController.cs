using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using HiQPdf;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Helper;
using vincontrol.Manheim;
using vincontrol.VinSell.ExtensionClass;
using vincontrol.VinSell.Handlers;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;

namespace vincontrol.VinSell.Controllers
{
    public class ManheimController : BaseController
    {
        public ActionResult ManheimData(string vin, string year, string make, string model)
        {
            var results = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();
                results = manheimService.ManheimReport(new VehicleViewModel() { Vin = vin, Year = Convert.ToInt32(year), Make = make, Model = model }, SessionHandler.User.Setting.Manheim, SessionHandler.User.Setting.ManheimPassword);
            }
            catch (Exception)
            {
                
            }

            return View(results);
        }

        public ActionResult ManheimTransaction(string year, string make, string model, string trim)
        {
            var manheimTransactions = new List<ManheimTransactionViewModel>();
            try
            {
                var manheimService = new ManheimService();
                manheimTransactions = manheimService.GetManheimTransactions(year, make, model, trim, "NA");
            }
            catch (Exception)
            {

            }

            return PartialView("ManheimTransaction", manheimTransactions);
        }

        [HttpPost]
        public ActionResult ManheimReportDetail(string year, string make, string model, string trim, string region, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var manheimService = new ManheimService();
                manheimService.Execute("US", year, make, model, trim, region, pageIndex, pageSize);
                return View("ManheimReportDetail", manheimService.ManheimTransactions);
            }
            catch (Exception) { }

            return PartialView("ManheimReportDetail", new List<ManheimTransactionViewModel>());
        }

        public ActionResult ManheimTransactionDetail(string year, string make, string model, string trim, string region = "NA", int pageIndex = 0, int pageSize = 0)
        {
            ViewData["ManheimYear"] = year;
            ViewData["ManheimMake"] = make;
            ViewData["ManheimModel"] = model;
            ViewData["ManheimTrim"] = trim;
            ViewData["ManheimRegion"] = region;

            ViewData["ManheimPageSize"] = pageSize;
            ViewData["ManheimPageIndex"] = pageIndex;
            try
            {
                var manheimService = new ManheimService();
                manheimService.Execute("US", year, make, model, trim, region, pageIndex, pageSize);
                ViewData["ManheimNumberOfTransactions"] = manheimService.NumberOfManheimTransactions;
                ViewData["HighPrice"] = manheimService.HighPrice;
                ViewData["LowPrice"] = manheimService.LowPrice;
                ViewData["AvgPrice"] = manheimService.AveragePrice;
                ViewData["AvgOdo"] = manheimService.AverageOdometer;

                var manheimReportViewModel = new ManheimReport()
                {
                    ManheimTransactions = manheimService.ManheimTransactions,
                    Region = region,
                    HighestPrice = manheimService.HighPrice,
                    LowestPrice = manheimService.LowPrice,
                    AveragePrice = manheimService.AveragePrice,
                    AverageOdometer = manheimService.AverageOdometer,
                    NumberOfTransactions = manheimService.NumberOfManheimTransactions
                };

                SessionHandler.ManheimTransactionsReport = manheimReportViewModel;
                return View("ManheimReportWindow", manheimReportViewModel);
            }
            catch (Exception)
            {
                return View("ManheimReportWindow", new ManheimReport());
            }
        }

        public void OpenManaheimLoginWindow(string url)
        {
            var encode = System.Text.Encoding.GetEncoding("utf-8");

            var manheimLink = HttpUtility.HtmlDecode(url);

            var myRequest = (HttpWebRequest)WebRequest.Create(manheimLink);

            myRequest.Method = "GET";

            var myResponse = myRequest.GetResponse();

            var receiveStream = myResponse.GetResponseStream();

            var readStream = new StreamReader(receiveStream, encode);

            string result = "";
            string line;
            while ((line = readStream.ReadLine()) != null)
            {
                if (line.Contains("stylesheets"))

                    line = line.Replace("href=\"/stylesheets", "href=\"https://www.manheim.com/stylesheets");
                else if (line.Contains("javascripts"))
                    line = line.Replace("src=\"/javascripts", "src=\"https://www.manheim.com/javascripts");
                else if (line.Contains("form accept-charset=\"UTF-8\" action"))
                    line = line.Replace("action=\"/login/authenticate\"", "action=\"https://www.manheim.com/login/authenticate\"");
                else
                {
                    if (!line.Contains("https://www.manheim.com"))
                        line = line.Replace("href=\"", "href=\"https://www.manheim.com");
                }

                //FINAL LOGIN
                if (line.Contains("Username:"))
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + SessionHandler.User.Setting.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + SessionHandler.User.Setting.ManheimPassword + "\" />" + Environment.NewLine;

                if (line.Contains("</body>"))
                {
                    line = "<script type=\"text/javascript\"> jQuery(document).ready(function($){document.forms[1].submit(); });</script></body>" + Environment.NewLine;
                }

                result += line;
            }

            Response.Write(result);
        }
        
        public void OpenManaheimLoginWindowWithVin(string vin)
        {
            
            var encode = System.Text.Encoding.GetEncoding("utf-8");

            var manheimLink =
                "https://www.manheim.com/members/internetmmr/?vin=" + vin;

            var myRequest = (HttpWebRequest)WebRequest.Create(manheimLink);

            myRequest.Method = "GET";

            var myResponse = myRequest.GetResponse();

            var receiveStream = myResponse.GetResponseStream();

            var readStream = new StreamReader(receiveStream, encode);
            
            string result = "";
            string line;
            while ((line = readStream.ReadLine()) != null)
            {
                if (line.Contains("stylesheets"))

                    line = line.Replace("href=\"/stylesheets", "href=\"https://www.manheim.com/stylesheets");
                else if (line.Contains("javascripts"))
                    line = line.Replace("src=\"/javascripts", "src=\"https://www.manheim.com/javascripts");
                else if (line.Contains("form accept-charset=\"UTF-8\" action"))
                    line = line.Replace("action=\"/login/authenticate\"", "action=\"https://www.manheim.com/login/authenticate\"");
                else
                {
                    if (!line.Contains("https://www.manheim.com"))
                        line = line.Replace("href=\"", "href=\"https://www.manheim.com");
                }

                //FINAL LOGIN
                if (line.Contains("Username:"))
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + SessionHandler.User.Setting.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + SessionHandler.User.Setting.ManheimPassword + "\" />" + Environment.NewLine;

                if (line.Contains("</body>"))
                {
                    line = "<script type=\"text/javascript\"> jQuery(document).ready(function($){document.forms[1].submit(); });</script></body>" + Environment.NewLine;
                }

                result += line;

            }


            Response.Write(result);


        }


        public ActionResult PrintManheimReportDetail(string year, string make, string model, string trim, string regionValue, string regionName, int reportType)
        {
            try
            {
                var manheimReportViewModel = new ManheimReport() { Region = regionName };
                if (SessionHandler.ManheimTransactionsReport != null)
                {
                    manheimReportViewModel.HighestPrice = SessionHandler.ManheimTransactionsReport.HighestPrice;
                    manheimReportViewModel.LowestPrice = SessionHandler.ManheimTransactionsReport.LowestPrice;
                    manheimReportViewModel.AveragePrice = SessionHandler.ManheimTransactionsReport.AveragePrice;
                    manheimReportViewModel.AverageOdometer = SessionHandler.ManheimTransactionsReport.AverageOdometer;
                    manheimReportViewModel.NumberOfTransactions = SessionHandler.ManheimTransactionsReport.NumberOfTransactions;
                    manheimReportViewModel.ManheimTransactions = SessionHandler.ManheimTransactionsReport.ManheimTransactions.OrderByDescending(i => i.SaleDate).ToList();
                }
                else
                {
                    var manheimService = new ManheimService();
                    manheimService.Execute("US", year, make, model, trim, regionValue, 0, 0);
                    manheimReportViewModel.HighestPrice = manheimService.HighPrice;
                    manheimReportViewModel.LowestPrice = manheimService.LowPrice;
                    manheimReportViewModel.AveragePrice = manheimService.AveragePrice;
                    manheimReportViewModel.AverageOdometer = manheimService.AverageOdometer;
                    manheimReportViewModel.NumberOfTransactions = manheimService.NumberOfManheimTransactions;
                    manheimReportViewModel.ManheimTransactions = manheimService.ManheimTransactions.OrderByDescending(i => i.SaleDate).ToList();
                }

                if (reportType == Constanst.ReportType.Pdf)
                {
                    string htmlToConvert = PDFRender.RenderViewAsString("ManheimTransaction", manheimReportViewModel, ControllerContext);

                    //// instantiate the HiQPdf HTML to PDF converter
                    var htmlToPdfConverter = new HtmlToPdf();
                    PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
                    PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
                    //AddDateTimeHeader(pdfDocument);
                    var bytes = pdfDocument.WriteToMemory();
                    var workStream = new MemoryStream();
                    workStream.Write(bytes, 0, bytes.Length);
                    workStream.Position = 0;
                    HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + String.Format("Manheim Transaction Report {0}.pdf", DateTime.Now.ToString("ddMMyyHHmmss")));
                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");
                    worksheet.FirstRow().Style.Font.Bold = true;
                    worksheet.Cell("A1").Value = new[] { new { Col1 = "Sale date", Col2 = "Auction", Col3 = "Odometer", Col4 = "Price", Col5 = "Engine", Col6 = "Cond", Col7 = "Color", Col8 = "In Sample" } };
                    worksheet.Cell("A2").Value = manheimReportViewModel.ManheimTransactions.Where(i => i.Price != "0" && i.Odometer != "0")
                        .Select(i => new
                        {
                            Col1 = i.SaleDate,
                            Col2 = i.Auction,
                            Col3 = i.Odometer,
                            Col4 = i.Price,
                            Col5 = i.Engine,
                            Col6 = i.Cond,
                            Col7 = i.Color,
                            Col8 = i.Sample
                        }).OrderByDescending(i => i.Col1);
                    return new ExcelResult(workbook, String.Format("Manheim Transaction Report {0}", DateTime.Now.ToString("ddMMyyHHmmss")));
                }
            }
            catch (Exception)
            {
                HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Error.pdf");
                return new FileStreamResult(null, "application/pdf");
            }
        }
    }
}
