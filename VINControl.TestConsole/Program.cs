using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using Aspose.Slides;
using AviFile;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using NReco.VideoConverter;
using VINControl.CarMax;
using vincontrol.ChromeAutoService;
using VINControl.Craigslist;
using VINControl.TruckTrader;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Youtube.v3;
using Google.Apis.Youtube.v3.Data;
using Vincontrol.Youtube;
using Vincontrol.Youtube.Common;
//using VINControl.ExcelHelper;
using VINControl.ImageHelper;
using System.Text.RegularExpressions;
using System.Threading;
using ClosedXML.Excel;
using VINCONTROL.Services.Business.Helpers;

namespace VINControl.TestConsole
{
    public class Program
    {
        private const string MyEmail = "david@vincontrol.com";

        static void Main(string[] args)
        {
            //CarMaxExecuteByApi();
            //CarMaxExecuteByMake();
            //CarMaxMarkSold();
            //CarMaxInsertStore();

            var craigslistService = new CraigslistService();
            //craigslistService.UpdatePrice("TestyFlavor518@yahoo.com", "wCjdpMyD02", 134471, 65000);
            //craigslistService.Execute("TestyFlavor518@yahoo.com", "wCjdpMyD02", "http://losangeles.craigslist.org/");
            var states = craigslistService.GetStateList();
            //craigslistService.GetConfirmationPaymentInfo("https://post.craigslist.org/k/THBGxGmF4xG9kqIjysgB6Q/vfvjy");

            //carmaxService.ExecuteByMake(ConfigurationManager.AppSettings["carmax:MakeName"], Convert.ToInt64(ConfigurationManager.AppSettings["carmax:MakeValue"])); 
            //carmaxService.ExecuteByStore();
            //carmaxService.AddNewCarMaxVehicle(2011, "Acura", "MDX", "", 10358462, "http://www.carmax.com/enus/view-car/default.html?id=10358462&AVi=35&No=20&Rp=R&D=90&zip=92627&N=4294963164+4294962757&Q=9c0af5c7-3fe8-4c80-8faf-651ccdf4b9c4&Ep=search:results:results%20page");

            //try
            //{
            //    //Console.WriteLine("START to marking sold on CommercialTrucks");
            //    //Console.WriteLine("START to getting cars on CommercialTrucks with {0}", ConfigurationManager.AppSettings["commercialtruck:Category"]);

            //    var truckTraderService = new TruckTraderService();
            //    //truckTraderService.InsertTruckDealerIntoDatabase();
            //    truckTraderService.GetTrucksByCategory(ConfigurationManager.AppSettings["commercialtruck:Category"]);
            //    //truckTraderService.MarkSoldCommercialTrucks();

            //    //Console.WriteLine("DONE");
            //    Thread.Sleep(3000);
            //}
            //catch (Exception ex)
            //{
            //    //Console.WriteLine("ERROR to marking sold on CommercialTrucks");
            //    Console.WriteLine("ERROR to getting cars on CommercialTrucks with {0}", ConfigurationManager.AppSettings["commercialtruck:Category"]);
            //    Console.ReadLine();
            //}


            //var ff = new FFMpegConverter();
            //ff.ConvertMedia(@"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.wmv", @"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.avi", Format.avi);

            //var sourcePath = @"C:\SCBBR53W26C036262\NormalSizeImages";
            //var extension = ".jpg";
            //GenerateVideoByAviManager(sourcePath, extension);

            //GenerateVideoFromPowerPoint(sourcePath, extension);
            //GenerateVideoFromPowerPoint();
            //System.Threading.Thread.Sleep(240000);
            //var ff = new FFMpegConverter();
            //ff.ConvertMedia(@"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.wmv", @"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.avi", Format.avi);
            //EmbedAudio(@"C:\SCBBR53W26C036262");
            //GenerateVideoFromAspose(sourcePath, extension);

            //UploadVideo();

            //ExcelHandler.ProcessDataFeed(@"D:\USED INV IMPIRV.xlsx");
            //ImageHandler.Migrate(@"D:\37375", @"D:\VincontrolDeployment\DealerImages\37375", 37375, "NormalSizeImages", true);
            //ImageHandler.Rename(@"D:\VincontrolDeployment\DealerImages\37375", @"D:\VincontrolDeployment\DealerImages\37375", 37375, "NormalSizeImages");
            //ImageHandler.Rename(@"D:\VincontrolDeployment\DealerImages\37695", @"D:\VincontrolDeployment\DealerImages\37695", 37695, "NormalSizeImages");

            //ParseDataFeedFileName(@"72818 Nov 03 21:13 DataCube.txt");

            //ExportVehiclesFromChromeAutoService();
        }

        private static void CarMaxExecuteByApi()
        {
            var startDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
            try
            {
                var carmaxService = new CarMaxService();
                carmaxService.ExecuteByApi();

                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] DONE ExecuteByApi"), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")));
            }
            catch (Exception ex)
            {
                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] ERROR ExecuteByApi"), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")) + "\n" + ex.Message + " ###### " + ex.StackTrace);
            }
        }

        private static void CarMaxExecuteByMake()
        {
            var startDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
            try
            {
                var carmaxService = new CarMaxService();
                carmaxService.ExecuteByMake(ConfigurationManager.AppSettings["carmax:MakeName"], Convert.ToInt64(ConfigurationManager.AppSettings["carmax:MakeValue"]));

                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] DONE ExecuteByMake {0} {1}", ConfigurationManager.AppSettings["carmax:MakeName"], ConfigurationManager.AppSettings["carmax:MakeValue"]), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")));
            }
            catch (Exception ex)
            {
                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] ERROR ExecuteByMake {0} {1}", ConfigurationManager.AppSettings["carmax:MakeName"], ConfigurationManager.AppSettings["carmax:MakeValue"]), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")) + "\n" + ex.Message + " ###### " + ex.StackTrace);
            }            
        }

        private static void CarMaxInsertStore()
        {
            var startDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
            try
            {
                var carmaxService = new CarMaxService();
                carmaxService.InsertStoresIntoDatabase();

                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] DONE InsertStore"), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")));
            }
            catch (Exception ex)
            {
                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] ERROR InsertStore"), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")) + "\n" + ex.Message + " ###### " + ex.StackTrace);
            }            
        }

        private static void CarMaxMarkSold()
        {
            var startDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
            try
            {
                var carmaxService = new CarMaxService();
                carmaxService.MarkSoldCarMaxVehicles();

                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] DONE MarkSold"), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")));
            }
            catch (Exception ex)
            {
                var endDate = CommonHelper.GetChicagoDateTime(DateTime.Now);
                EmailHelper.SendEmail(new List<string>() { MyEmail }, string.Format("[CarMax] ERROR MarkSold"), string.Format("From {0} to {1}", startDate.ToString("MM/dd/yyyy HH:mm:ss"), endDate.ToString("MM/dd/yyyy HH:mm:ss")) + "\n" + ex.Message + " ###### " + ex.StackTrace);
            }            
        }

        private static void ExportVehiclesFromChromeAutoService()
        {
            // Export vehicles from Chrome Auto Service
            var autoService = new ChromeAutoService();
            var vehicles = new List<ExportVehicleInfo>();
            for (int i = 2014; i >= 2005; i--)
            {
                var makes = autoService.GetDivisions(i);
                foreach (var make in makes)
                {
                    var models = autoService.GetModelsByDivision(i, make.id);
                    if (models != null && models.Any())
                    {
                        foreach (var model in models)
                        {
                            //var styles = autoService.GetStyles(model.id);
                            var trims = autoService.GetTrims(i, make.Value, model.Value);
                            if (trims == null) continue;
                            foreach (var trim in trims)
                            {
                                if (String.IsNullOrEmpty(trim.Value)) trim.Value = "Standard";
                                if (vehicles.Any(x => x.Year.Equals(i) && x.Vendor.Equals(make.Value) && x.Model.Equals(model.Value) && x.Trim.Equals(trim.Value))) continue;
                                Console.WriteLine("{0} {1} {2} {3}", i, make.Value, model.Value, trim.Value);
                                var vehicle = autoService.GetVehicleInformationFromStyleId(trim.id);
                                var newExportItem = new ExportVehicleInfo() { Vendor = make.Value, Model = model.Value, Trim = trim.Value, Year = i, Turbo = "no" };
                                if (vehicle != null)
                                {
                                    try
                                    {
                                        newExportItem.BodyStyle = ((vehicle.style[0].bodyType[0])).Value;

                                        if (vehicle.style != null && vehicle.style.Any())
                                        {
                                            newExportItem.MaxSeating = vehicle.style[0].passDoors;
                                            newExportItem.Doors = vehicle.style[0].passDoors;
                                            newExportItem.DriveTrain = (vehicle.style[0].drivetrain).ToString();
                                        }

                                        if (vehicle.engine != null && vehicle.engine.Any())
                                        {
                                            newExportItem.FuelType = vehicle.engine[0].fuelType.Value;
                                            newExportItem.EngineCapacity = vehicle.engine[0].displacement.liters;
                                            newExportItem.Horsepower = vehicle.engine[0].horsepower.value + "@" + vehicle.engine[0].horsepower.rpm;
                                            newExportItem.Torque = vehicle.engine[0].netTorque.value + "@" + vehicle.engine[0].netTorque.rpm;
                                        }

                                        if (vehicle.genericEquipment != null && vehicle.genericEquipment.Any())
                                        {
                                            newExportItem.DriverAirBag =
                                                vehicle.genericEquipment.Any(
                                                    x =>
                                                        ((
                                                            vincontrol.ChromeAutoService.AutomativeService.
                                                                CategoryDefinition)x.Item).category.Value.Contains(
                                                                    "Driver Air Bag"))
                                                    ? "yes"
                                                    : "no";
                                            newExportItem.FrontPassengerAirBag =
                                                vehicle.genericEquipment.Any(
                                                    x =>
                                                        ((
                                                            vincontrol.ChromeAutoService.AutomativeService.
                                                                CategoryDefinition)x.Item).category.Value.Contains(
                                                                    "Passenger Air Bag Sensor"))
                                                    ? "yes"
                                                    : "no";
                                        }

                                        if (vehicle.standard != null && vehicle.standard.Any())
                                        {
                                            newExportItem.ABSBrake = vehicle.standard.Any(x => x.description.Contains("(ABS)")) ? "yes" : "no";
                                            newExportItem.EBDBrake = vehicle.standard.Any(x => x.description.Contains("(EBD)")) ? "yes" : "no";
                                            newExportItem.EBABrake = vehicle.standard.Any(x => x.description.Contains("(EBA)")) ? "yes" : "no";
                                            newExportItem.ESP = vehicle.standard.Any(x => x.description.Contains("ESP")) ? "yes" : "no";
                                            newExportItem.CruiseControl = vehicle.standard.Any(x => x.description.Contains("Cruise control")) ? "yes" : "no";
                                            newExportItem.ReverseSensors = vehicle.standard.Any(x => x.description.Contains("reverse driver")) ? "yes" : "no";
                                        }

                                        vehicles.Add(newExportItem);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (vehicles.Any())
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Sheet 1");
                worksheet.FirstRow().Style.Font.Bold = true;
                worksheet.ColumnWidth = 20;
                worksheet.Cell("A1").Value = new[]
                {
                    new
                    {
                        Col1 = "Vendor",
                        Col2 = "Model",
                        Col3 = "Trim",
                        Col4 = "Year",
                        Col5 = "Origin (Made in)",
                        Col6 = "BodyStyle",
                        Col7 = "MaxSeating",
                        Col8 = "Doors",
                        Col9 = "FuelType",
                        Col10 = "EngineCapacity(L)",
                        Col11 = "Turbo",
                        Col12 = "Horsepower (kW)",
                        Col13 = "Torque",
                        Col14 = "TransmissionTypeId",
                        Col15 = "TransmissionLevelId",
                        Col16 = "DriveTrain",
                        Col17 = "Length(mm)",
                        Col18 = "Width(mm)",
                        Col19 = "Height(mm)",
                        Col20 = "WheelBase(mm)",
                        Col21 = "MinimumGroundClearance (mm)",
                        Col22 = "TurningRadius (m)",
                        Col23 = "CurbWeight (kg)",
                        Col24 = "FrontSuspension",
                        Col25 = "RearSuspension",
                        Col26 = "FrontBrake",
                        Col27 = "RearBrake",
                        Col28 = "FrontTiresSize",
                        Col29 = "RearTiresSize",
                        Col30 = "Rim",
                        Col31 = "ABSBrake",
                        Col32 = "EBDBrake",
                        Col33 = "EBABrake",
                        Col34 = "ESP",
                        Col35 = "CruiseControl",
                        Col36 = "ReverseSensors",
                        Col37 = "TopDoor",
                        Col38 = "DriverAirBag",
                        Col39 = "FrontPassengerAirBag",
                        Col40 = "Video 1",
                        Col41 = "Video 2",
                        Col42 = "Video 3",
                        Col43 = "Article"
                    }
                };

                worksheet.Cell("A2").Value = vehicles.Select(i => new
                {
                    Col1 = i.Vendor,
                    Col2 = i.Model,
                    Col3 = i.Trim,
                    Col4 = i.Year,
                    Col5 = "Nhập khẩu",
                    Col6 = i.BodyStyle,
                    Col7 = i.MaxSeating,
                    Col8 = i.Doors,
                    Col9 = i.FuelType,
                    Col10 = i.EngineCapacity,
                    Col11 = i.Turbo,
                    Col12 = i.Horsepower,
                    Col13 = i.Torque,
                    Col14 = i.TransmissionTypeId,
                    Col15 = i.TransmissionLevelId.Equals(0) ? "" : i.TransmissionLevelId.ToString(),
                    Col16 = i.DriveTrain,
                    Col17 = i.Length.Equals(0) ? "" : i.Length.ToString("0,0"),
                    Col18 = i.Width.Equals(0) ? "" : i.Width.ToString("0,0"),
                    Col19 = i.Height.Equals(0) ? "" : i.Height.ToString("0,0"),
                    Col20 = i.WheelBase.Equals(0) ? "" : i.WheelBase.ToString("0,0"),
                    Col21 = i.MinimumGroundClearance.Equals(0) ? "" : i.MinimumGroundClearance.ToString("0,0"),
                    Col22 = i.TurningRadius.Equals(0) ? "" : i.TurningRadius.ToString("0,0"),
                    Col23 = i.CurbWeight.Equals(0) ? "" : i.CurbWeight.ToString("0,0"),
                    Col24 = i.FrontSuspension,
                    Col25 = i.RearSuspension,
                    Col26 = i.FrontBrake,
                    Col27 = i.RearBrake,
                    Col28 = i.FrontTiresSize,
                    Col29 = i.RearTiresSize,
                    Col30 = i.Rim,
                    Col31 = i.ABSBrake,
                    Col32 = i.EBDBrake,
                    Col33 = i.EBABrake,
                    Col34 = i.ESP,
                    Col35 = i.CruiseControl,
                    Col36 = i.ReverseSensors,
                    Col37 = i.TopDoor,
                    Col38 = i.DriverAirBag,
                    Col39 = i.FrontPassengerAirBag,
                    Col40 = string.Empty,
                    Col41 = string.Empty,
                    Col42 = string.Empty,
                    Col43 = string.Empty
                });

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    using (var file = new FileStream(@"D:\DataOfTrims.xlsx", FileMode.Create, FileAccess.Write))
                    {
                        memoryStream.WriteTo(file);
                    }
                }
            }
        }

        private static void EmbedAudio(string sourcePath)
        {
            String fileName = sourcePath + "\\2006 Bentley Continental Fly.wav";
            using (var stream = File.Create(fileName))
            {
                var speechEngine = new SpeechSynthesizer {Rate = 1, Volume = 100};
                speechEngine.SelectVoice(speechEngine.GetInstalledVoices()[0].VoiceInfo.Name);
                speechEngine.SetOutputToWaveStream(stream);
                var description = "Welcome to Newport Coast Auto. This's 2006 Bentley Continental Fly. ";
                description += "Amazing highly rated value carfax certified car with previous Beverley Hills owner. Great options available on the cars. Newport Coast Auto Invites You To Call 949-515-0800. With Any Questions Or To Schedule A Test Drive. The Entire Staff At Newport Coast Auto Are Dedicated And Experienced. We Always Strive To Provide A Level Of Service That Is Unsurpassed In Today's Busy And Automated World. Newport Coast Auto Offers Quality Pre-owned Vehicles At Competitive Prices. We Also Offer Extended Warranties, Financing, And Unmatched Customer Service. We Invite Trade-ins As Well";
                speechEngine.Speak(description.Replace("***", ".").Replace("###", ".").Replace("*", "").Replace("#", ""));
                stream.Flush();
            }

            var aviManager = new AviManager(sourcePath + "\\2006 Bentley Continental Fly.avi", true);
            aviManager.AddAudioStream(fileName, 1);
            //aviManager.AddAudioStream(sourcePath + "\\Test.wav", 1);
            aviManager.Close();

            //var aviManager = new AviManager(sourcePath + "\\2006 Bentley Continental Fly.avi", true);
            ////aviManager.AddAudioStream(fileName, 1);
            //aviManager.AddAudioStream(sourcePath + "\\sound.wav", 1);

            //aviManager.Close();
        }

        private static void GenerateVideoByAviManager(string sourcePath, string extension)
        {
            var dirInfo = new DirectoryInfo(sourcePath);
            var jpgFileList = new List<string>();
            if (dirInfo.Exists)
            {
                jpgFileList.AddRange(dirInfo.GetFiles().Where(f => f.Extension.Equals(extension)).OrderBy(f => f.LastWriteTime).Take(30).Select(fileToUpload => sourcePath + "\\" + fileToUpload.Name));
            }

            var bitmap = (Bitmap)Image.FromFile(jpgFileList.First());

            var aviManager = new AviManager(sourcePath + "\\2006 Bentley Continental Fly.avi", false);

            var aviStream = aviManager.AddVideoStream(false, 0.2, bitmap);
            //aviStream = aviManager.AddVideoStream(false, 0.2, 400, 640, 360, PixelFormat.Format24bppRgb);

            jpgFileList.Skip(1).ToList().ForEach(file =>
            {
                bitmap = (Bitmap)Bitmap.FromFile(file);
                aviStream.AddFrame(bitmap);
                bitmap.Dispose();
            });

            aviManager.Close();

            String fileName = sourcePath + "\\2006 Bentley Continental Fly.wav";
            using (var stream = File.Create(fileName))
            {
                var speechEngine = new SpeechSynthesizer();
                speechEngine.Rate = -1;
                speechEngine.Volume = 90;
                speechEngine.SelectVoice(speechEngine.GetInstalledVoices()[0].VoiceInfo.Name);
                speechEngine.SetOutputToWaveStream(stream);
                var description = "*** BEVERLY HILLS OWNED ***  CARFAX CERTIFIED SUPPORTING A HIGHER RATED VALUE *** NEVER ANY NEGATIVE HISTORY *** CONVENIENCE PACKAGE WITH PRIVACY HANDSET AND BLUETOOTH *** MASSAGE SEATS *** MULTI-FUNCTION STEERING  WHEEL *** 19\" 5-SPOKE SPORT ALLOY WHEELS AND NEW TIRES *** FRONT AND REAR HEATED AND COOLED SEATS * FRONT AND REAR 4-ZONE CLIMATE CONTROL * REAR PASSENGER SIDE WINDOW DEFROST * REAR PASSENGER POWER LUMBAR SEATS * DUAL MULTI ADJUST FRONT SEATS WITH LUMBAR AND LEG EXTENSION * FRONT AND REAR PARKING SENSORS * PREMIUM AUDIO WITH 6-DISC CD CHANGER * NAVIGATION * SOFT CLOSE DOORS * POWER REAR SUNSHADE * POWER BOOT/TRUNK OPEN AND CLOSE * XENON HEADLIGHTS * REAR TRUNK PASS-THROUGH * 2-TONE COLORED MULTI-FUNCTION STEERING WHEEL * REMOTE GARAGE DOOR OPENER * ALL BOOKS/OWNERS MANUALS AND KEYS (2)*     Newport Coast Auto Invites You To Call 949-515-0800 Or Email Sales@newportcoastauto.com With Any Questions Or To Schedule A Test Drive. The Entire Staff At Newport Coast Auto Are Dedicated And Experienced. We Always Strive To Provide A Level Of Service That Is Unsurpassed In Today's Busy And Automated World. Newport Coast Auto Offers Quality Pre-owned Vehicles At Competitive Prices. We Also Offer Extended Warranties, Financing, And Unmatched Customer Service. We Invite Trade-ins As Well.   *** *** SERVICED TO DATE AND COMPLETELY INSPECTED *** NEVER ANY ACCIDENTS OR NEGATIVE HISTORY *** ####### FREE CERTIFIED CARFAX ON EVERY VEHICLE JUST BY VISITING NEWPORTCOASTAUTO.COM ####### 1.99% APR FINANCING ON APPROVED CREDIT ######## Newport Coast Auto Invites You to Call 949-515-0800 or Email Sales@newportcoastauto.com";
                speechEngine.Speak(description.Replace("***", ".").Replace("###", ".").Replace("*", "").Replace("#", ""));
                stream.Flush();
            }

            aviManager = new AviManager(sourcePath + "\\2006 Bentley Continental Fly.avi", true);
            //int countFrames = aviManager.GetVideoStream().CountFrames;
            //for (int i = 1; i < countFrames; i += 50)
            //{
            //    aviManager.AddAudioStream(fileName, i);    
            //}
            aviManager.AddAudioStream(fileName, 1);
            aviManager.Close();
        }

        private static void GenerateVideoFromAspose(string sourcePath, string extension)
        {
            var pptPresentation = new Aspose.Slides.Presentation();
            ISlideCollection slides = pptPresentation.Slides;

            var dirInfo = new DirectoryInfo(sourcePath);
            var jpgFileList = new List<string>();
            if (dirInfo.Exists)
            {
                jpgFileList.AddRange(dirInfo.GetFiles().Where(f => f.Extension.Equals(extension)).OrderBy(f => f.LastWriteTime).Take(3).Select(fileToUpload => sourcePath + "\\" + fileToUpload.Name));
            }

            //Aspose.Slides.Slide slide;
            for (int i = 0; i < jpgFileList.Count; i++)
            {
                var slide = slides.AddEmptySlide(pptPresentation.LayoutSlides[i + 1]);
                //Set the background with Image
                slide.Background.Type = BackgroundType.OwnBackground;
                slide.Background.FillFormat.FillType = FillType.Picture;
                slide.Background.FillFormat.PictureFillFormat.PictureFillMode = PictureFillMode.Stretch;

                //Set the picture
                var img = (System.Drawing.Image)new Bitmap(jpgFileList[i]);

                //Add image to presentation's images collection
                IPPImage imgx = pptPresentation.Images.AddImage(img);

                slide.Background.FillFormat.PictureFillFormat.Picture.Image = imgx;
            }

            //Save the PPTX file to the Disk
            pptPresentation.Save(sourcePath + "\\EmptySlide.pptx", Aspose.Slides.Export.SaveFormat.Pptx);
        }

        private static void GenerateVideoFromPowerPoint(string sourcePath, string extension)
        {
            var pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();
            var pptPresentation = pptApplication.Presentations.Add(MsoTriState.msoFalse);
            var slides = pptPresentation.Slides;
            //CustomLayout customLayout = pptPresentation.SlideMaster.CustomLayouts[Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutBlank];
            //Directory.GetCurrentDirectory() + @"/Untitled.jpg";

            var options = new List<string>();
            options.Add("Dark Stained Burr Walnut Veneer");
            options.Add("19\" 5-Spoke Chromed Alloy Sport Wheels");
            options.Add("Wood & Hide-Trimmed Steering Wheel");
            options.Add("Deep-Pile Carpet Mats W/Leather Trimming");
            options.Add("Valet Parking Key");
            options.Add("Universal Garage Door Opener");
            options.Add("Bluetooth Connection");

            var dirInfo = new DirectoryInfo(sourcePath);
            var jpgFileList = new List<string>();
            if (dirInfo.Exists)
            {
                jpgFileList.AddRange(dirInfo.GetFiles().Where(f => f.Extension.Equals(extension)).OrderBy(f => f.LastWriteTime).Take(12).Select(fileToUpload => sourcePath + "\\" + fileToUpload.Name));
            }

            _Slide slide;

            for (int i = 0; i < jpgFileList.Count; i++)
            {
                slide = slides.Add(i + 1, PpSlideLayout.ppLayoutBlank);
                
                if (i.Equals(0))
                {
                    var shape = slide.Shapes.AddMediaObject2(@"C:\SCBBR53W26C036262\VINSoundLow.mp3", MsoTriState.msoFalse, MsoTriState.msoTrue, 50, 50, 20, 20);
                    //shape.AnimationSettings.SoundEffect.ImportFromFile(@"C:\SCBBR53W26C036262\ChillingMusic.wav");
                    shape.MediaFormat.Volume = 0.25f;
                    shape.AnimationSettings.PlaySettings.PlayOnEntry = MsoTriState.msoTrue;
                    shape.AnimationSettings.PlaySettings.LoopUntilStopped = MsoTriState.msoTrue;
                    shape.AnimationSettings.PlaySettings.HideWhileNotPlaying = MsoTriState.msoTrue;
                    shape.AnimationSettings.PlaySettings.StopAfterSlides = jpgFileList.Count + 1;

                    var fileName = @"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.wav";
                    using (var stream = File.Create(fileName))
                    {
                        var speechEngine = new SpeechSynthesizer { Rate = -1, Volume = 100 };
                        speechEngine.SelectVoice(speechEngine.GetInstalledVoices()[0].VoiceInfo.Name);
                        speechEngine.SetOutputToWaveStream(stream);
                        var description = "Welcome to Newport Coast Auto. This's 2006 Bentley Continental Fly. ";
                        description += "Amazing highly rated value carfax certified car with previous Beverley Hills owner. Great options available on the cars. ";
                        description = options.Aggregate(description, (current, option) => current + (option + ","));
                        description += ". Newport Coast Auto Invites You To Call 949-515-0800. With Any Questions Or To Schedule A Test Drive. The Entire Staff At Newport Coast Auto Are Dedicated And Experienced. We Always Strive To Provide A Level Of Service That Is Unsurpassed In Today's Busy And Automated World. Newport Coast Auto Offers Quality Pre-owned Vehicles At Competitive Prices. We Also Offer Extended Warranties, Financing, And Unmatched Customer Service. We Invite Trade-ins As Well";
                        speechEngine.Speak(description.Replace("***", ".").Replace("###", ".").Replace("*", "").Replace("#", ""));
                        stream.Flush();
                    }
                    var shapeSpeech = slide.Shapes.AddMediaObject2(fileName, MsoTriState.msoFalse, MsoTriState.msoTrue, 50, 50, 20, 20);
                    shapeSpeech.MediaFormat.Volume = 1;
                    shapeSpeech.AnimationSettings.PlaySettings.PlayOnEntry = MsoTriState.msoTrue;
                    shapeSpeech.AnimationSettings.PlaySettings.LoopUntilStopped = MsoTriState.msoTrue;
                    shapeSpeech.AnimationSettings.PlaySettings.HideWhileNotPlaying = MsoTriState.msoTrue;
                    shapeSpeech.AnimationSettings.PlaySettings.StopAfterSlides = jpgFileList.Count + 1;

                    var pictureshape = slide.Shapes.AddPicture(jpgFileList[i], MsoTriState.msoTrue, MsoTriState.msoFalse, 0, 0);
                    //pictureshape.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectDissolve;

                    var textshape = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 50f, 140f, 400, 200);
                    textshape.Fill.Visible = MsoTriState.msoTrue;
                    textshape.Fill.ForeColor.RGB = Color.Black.ToArgb();
                    textshape.TextFrame.TextRange.Text = "2006 Bentley Continental Fly";
                    textshape.TextFrame.TextRange.Font.Color.RGB = System.Drawing.Color.White.ToArgb();
                    textshape.TextFrame.TextRange.Font.Size = 26;
                    textshape.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectFlyFromLeft;
                    textshape.AnimationSettings.TextLevelEffect = PpTextLevelEffect.ppAnimateByFourthLevel;
                    textshape.AnimationSettings.TextUnitEffect = PpTextUnitEffect.ppAnimateByWord;
                }
                else if (i.Equals(3))
                {
                    var pictureshape = slide.Shapes.AddPicture(jpgFileList[i], MsoTriState.msoTrue, MsoTriState.msoFalse, 0, 0);
                    
                    slides.Range(i + 1).SlideShowTransition.EntryEffect = PpEntryEffect.ppEffectRevealBlackLeft;
                    //slides.Range(i + 1).SlideShowTransition.Duration = options.Count > 1 ? 1.5f * options.Count : 3;

                    var textshape = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 50f, 20f, 450, 200);
                    
                    textshape.Fill.Visible = MsoTriState.msoTrue;
                    textshape.Fill.ForeColor.RGB = 0;
                    var textOptions = options.Aggregate("", (current, option) => current + ((char) 9642 + " " + (option + "\r\n")));
                    textshape.TextFrame.TextRange.Text = textOptions;
                    textshape.TextFrame.TextRange.Font.Color.RGB = System.Drawing.Color.White.ToArgb();
                    textshape.TextFrame.TextRange.Font.Size = 22;
                    textshape.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectZoomCenter;
                    textshape.AnimationSettings.TextLevelEffect = PpTextLevelEffect.ppAnimateByFourthLevel;
                    textshape.AnimationSettings.TextUnitEffect = PpTextUnitEffect.ppAnimateByWord;
                }
                else
                {
                    var pictureshape = slide.Shapes.AddPicture(jpgFileList[i], MsoTriState.msoTrue, MsoTriState.msoFalse, 0, 0);
                    slides.Range(i + 1).SlideShowTransition.EntryEffect = PpEntryEffect.ppEffectRevealBlackLeft;
                }
            }

            slide = slides.Add(jpgFileList.Count + 1, PpSlideLayout.ppLayoutBlank);
            var logo = slide.Shapes.AddPicture(@"C:\SCBBR53W26C036262\logo.png", MsoTriState.msoTrue, MsoTriState.msoFalse, 0, 140f);
            logo.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectFlyFromLeft;

            var address = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 350f, 160f, 350, 200);
            address.Fill.Visible = MsoTriState.msoTrue;
            address.Fill.ForeColor.RGB = Color.WhiteSmoke.ToArgb();
            address.TextFrame.TextRange.Text = "1719 Pomona Ave Costa Mesa, CA 92627";
            address.TextFrame.TextRange.Font.Color.RGB = Color.Gray.ToArgb();
            address.TextFrame.TextRange.Font.Size = 26;
            address.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectFlyFromLeft;
            
            pptPresentation.SaveAs(@"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.pptx");
        }

        private static void GenerateVideoFromPowerPoint()
        {
            var pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();
            Microsoft.Office.Interop.PowerPoint.Presentations oPresSet = pptApplication.Presentations;
            Microsoft.Office.Interop.PowerPoint._Presentation oPres =
                oPresSet.Open(@"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.pptx",
                    Microsoft.Office.Core.MsoTriState.msoFalse,
                    Microsoft.Office.Core.MsoTriState.msoFalse,
                    Microsoft.Office.Core.MsoTriState.msoTrue);

            string movie = @"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.wmv";
            oPres.CreateVideo(movie, true, 4, 480, 30, 100);
        }

        private static void UploadVideo()
        {
            try
            {
                var youtube = new YoutubeWrapper();

                var video = new YoutubeVideo() { };
                video.LocalFilePath = @"C:\SCBBR53W26C036262\2006 Bentley Continental Fly.wmv";
                video.Categories.Add("Autos");
                video.Tags.Add("Bentley");
                video.Tags.Add("Continental");
                video.Tags.Add("Newport Coast Auto");
                video.Tags.Add("Vehicle");
                video.Tags.Add("Auto");
                video.Description = "2006 Bentley Continental Fly";
                video.Title = "2006 Bentley Continental Fly";
                video.Latitude = 33.648280;
                video.Longitude = -117.915538;
                var videoId = string.Empty;
                youtube.UploadVideoToYouTube(video, out videoId);
            }
            catch (Exception ex)
            {
                
            }
            
        }

        private static void ParseDataFeedFileName(string filename)
        {
            var sizePattern = @"\d+";
            var namePattern = @"\b\w*\.";

            var regex = new Regex(sizePattern);
            Match match = regex.Match(filename);
            var size = match.Groups.Count > 0 ? Convert.ToInt32(match.Groups[0].Value) : 0;

            regex = new Regex(namePattern);
            match = regex.Match(filename);
            var name = match.Groups.Count > 0 ? (match.Groups[0].Value.Replace(".","")) : string.Empty;

            var dateString = filename.Replace(size.ToString(), "").Replace(String.Format("{0}.txt", name), "");
            DateTime date;
            if (DateTime.TryParse(String.Format("{0} {1}", DateTime.Now.Year,  dateString), out date))
            {
            }
            else
            {
                date = DateTime.Now;
            }
        }

    }

    public class ExportVehicleInfo
    {
        public string Vendor { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int Year { get; set; }
        public string MadeIn { get; set; }
        public string BodyStyle { get; set; }
        public int MaxSeating { get; set; }
        public int Doors { get; set; }
        public string FuelType { get; set; }
        public double EngineCapacity { get; set; }
        public string Turbo { get; set; }
        public string Horsepower { get; set; }
        public string Torque { get; set; }
        public string TransmissionTypeId { get; set; }
        public int TransmissionLevelId { get; set; }
        public string DriveTrain { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal WheelBase { get; set; }
        public decimal MinimumGroundClearance { get; set; }
        public decimal TurningRadius { get; set; }
        public decimal CurbWeight { get; set; }
        public string FrontSuspension { get; set; }
        public string RearSuspension { get; set; }
        public string FrontBrake { get; set; }
        public string RearBrake { get; set; }
        public string FrontTiresSize { get; set; }
        public string RearTiresSize { get; set; }
        public string Rim { get; set; }
        public string ABSBrake { get; set; }
        public string EBDBrake { get; set; }
        public string EBABrake { get; set; }
        public string ESP { get; set; }
        public string CruiseControl { get; set; }
        public string ReverseSensors { get; set; }
        public string TopDoor { get; set; }
        public string DriverAirBag { get; set; }
        public string FrontPassengerAirBag { get; set; }
        public string Video1 { get; set; }
        public string Video2 { get; set; }
        public string Video3 { get; set; }
        public string Article { get; set; }
    }
}
