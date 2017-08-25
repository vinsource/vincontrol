using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.ConfigurationManagement;
using Vincontrol.Youtube;

namespace VINControl.Video
{
    public class VideoGenerator
    {
        private YoutubeWrapper youtube;

        public VideoGenerator(){ youtube = new YoutubeWrapper(); }

        public string Generate(DealerViewModel dealer, CarShortViewModel car)
        {
            GeneratePowerPointFromImages(dealer, car);
            if (GenerateVideoFromPowerPoint(dealer, car))
            {
                System.Threading.Thread.Sleep(240000);

                try
                {
                    var video = new YoutubeVideo() { };
                    video.LocalFilePath = (ConfigurationHandler.DealerImages + "/" + dealer.DealerId + "/" + car.Vin + "/NormalSizeImages") + String.Format(@"\{0} {1} {2} {3}.wmv", car.ModelYear, car.Make, car.Model, car.Trim);
                    video.Categories.Add("Autos");
                    video.Tags.Add(car.ModelYear.ToString());
                    video.Tags.Add(car.Make);
                    video.Tags.Add(car.Model);
                    video.Tags.Add(dealer.Name);
                    video.Tags.Add("Vehicle");
                    video.Tags.Add("Auto");
                    video.Description = car.Description;
                    video.Title = String.Format(@"{0} {1} {2} {3}", car.ModelYear, car.Make, car.Model, car.Trim);
                    video.Latitude = Convert.ToDouble(dealer.Lattitude);
                    video.Longitude = Convert.ToDouble(dealer.Longitude);
                    string videoId;
                    youtube.UploadVideoToYouTube(video, out videoId);
                    return videoId.Split(':').Last();
                }
                catch (Exception)
                {
                    
                }
            }

            return string.Empty;
        }

        private void GeneratePowerPointFromImages(DealerViewModel dealer, CarShortViewModel car)
        {
            var pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();
            var pptPresentation = pptApplication.Presentations.Add(MsoTriState.msoFalse);
            var slides = pptPresentation.Slides;
            //CustomLayout customLayout = pptPresentation.SlideMaster.CustomLayouts[Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutBlank];

            var options = car.AdditonalOptions.Split(',').ToList();

            var sourcePath = (ConfigurationHandler.DealerImages + "/" + dealer.DealerId + "/" + car.Vin + "/NormalSizeImages");
            var dirInfo = new DirectoryInfo(sourcePath);
            var jpgFileList = new List<string>();
            if (dirInfo.Exists)
            {
                jpgFileList.AddRange(dirInfo.GetFiles().Where(f => f.Extension.Equals(".jpg")).OrderBy(f => f.LastWriteTime).Take(12).Select(fileToUpload => sourcePath + "\\" + fileToUpload.Name));
            }

            if (!jpgFileList.Any()) return;

            _Slide slide;
            for (int i = 0; i < jpgFileList.Count; i++)
            {
                slide = slides.Add(i + 1, PpSlideLayout.ppLayoutBlank);

                if (i.Equals(0))
                {
                    var shape = slide.Shapes.AddMediaObject2(ConfigurationHandler.DealerImages + @"\VINSoundLow.wav", MsoTriState.msoFalse, MsoTriState.msoTrue, 50, 50, 20, 20);
                    shape.MediaFormat.Volume = 0.25f;
                    shape.AnimationSettings.PlaySettings.PlayOnEntry = MsoTriState.msoTrue;
                    shape.AnimationSettings.PlaySettings.LoopUntilStopped = MsoTriState.msoTrue;
                    shape.AnimationSettings.PlaySettings.HideWhileNotPlaying = MsoTriState.msoTrue;
                    shape.AnimationSettings.PlaySettings.StopAfterSlides = jpgFileList.Count + 1;                                        

                    var fileName = sourcePath + String.Format(@"\{0} {1} {2} {3}.wav", car.ModelYear, car.Make, car.Model, car.Trim);
                    using (var stream = File.Create(fileName))
                    {
                        var speechEngine = new SpeechSynthesizer { Rate = -1, Volume = 100 };
                        speechEngine.SelectVoice(speechEngine.GetInstalledVoices()[0].VoiceInfo.Name);
                        speechEngine.SetOutputToWaveStream(stream);
                        var description = String.Format("Welcome to {0}. This's {1} {2} {3} {4}. ", dealer.Name, car.ModelYear, car.Make, car.Model, car.Trim);
                        description += "Amazing highly rated value carfax certified car. Great options available on the cars. ";
                        description = options.Aggregate(description, (current, option) => current + (option + ","));
                        description += String.Format(". {0} Invites You To Call {1}. With Any Questions Or To Schedule A Test Drive. The Entire Staff At {0} Are Dedicated And Experienced. We Always Strive To Provide A Level Of Service That Is Unsurpassed In Today's Busy And Automated World. {0} Offers Quality Pre-owned Vehicles At Competitive Prices. We Also Offer Extended Warranties, Financing, And Unmatched Customer Service. We Invite Trade-ins As Well", dealer.Name, dealer.Phone);
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
                    textshape.TextFrame.TextRange.Text = String.Format("{0} {1} {2} {3}", car.ModelYear, car.Make, car.Model, car.Trim);
                    textshape.TextFrame.TextRange.Font.Color.RGB = System.Drawing.Color.White.ToArgb();
                    textshape.TextFrame.TextRange.Font.Size = 26;
                    textshape.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectFlyFromLeft;
                    textshape.AnimationSettings.TextLevelEffect = PpTextLevelEffect.ppAnimateByFourthLevel;
                    textshape.AnimationSettings.TextUnitEffect = PpTextUnitEffect.ppAnimateByWord;
                }
                else if (i.Equals(2))
                {
                    var pictureshape = slide.Shapes.AddPicture(jpgFileList[i], MsoTriState.msoTrue, MsoTriState.msoFalse, 0, 0);

                    slides.Range(i + 1).SlideShowTransition.EntryEffect = PpEntryEffect.ppEffectRevealBlackLeft;
                    //slides.Range(i + 1).SlideShowTransition.Duration = options.Count > 1 ? 1.5f * options.Count : 3;

                    var textshape = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 50f, 20f, 450, 200);

                    textshape.Fill.Visible = MsoTriState.msoTrue;
                    textshape.Fill.ForeColor.RGB = 0;
                    var textOptions = options.Aggregate("", (current, option) => current + ((char)9642 + " " + (option + "\r\n")));
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
            if (!String.IsNullOrEmpty(dealer.Logo))
            {
                var logo = slide.Shapes.AddPicture(dealer.Logo, MsoTriState.msoTrue, MsoTriState.msoFalse, 0, 140f);
                logo.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectFlyFromLeft;
            }

            var address = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 350f, 160f, 350, 200);
            address.Fill.Visible = MsoTriState.msoTrue;
            address.Fill.ForeColor.RGB = Color.WhiteSmoke.ToArgb();
            address.TextFrame.TextRange.Text = String.Format("{0} {1}, {2} {3}", dealer.Address, dealer.City, dealer.State, dealer.ZipCode);
            address.TextFrame.TextRange.Font.Color.RGB = Color.Gray.ToArgb();
            address.TextFrame.TextRange.Font.Size = 26;
            address.AnimationSettings.EntryEffect = PpEntryEffect.ppEffectFlyFromLeft;

            pptPresentation.SaveAs(sourcePath + String.Format(@"\{0} {1} {2} {3}.pptx", car.ModelYear, car.Make, car.Model, car.Trim));
        }

        private bool GenerateVideoFromPowerPoint(DealerViewModel dealer, CarShortViewModel car)
        {
            try
            {
                var sourcePath = (ConfigurationHandler.DealerImages + "/" + dealer.DealerId + "/" + car.Vin + "/NormalSizeImages");
                var pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();
                Microsoft.Office.Interop.PowerPoint.Presentations oPresSet = pptApplication.Presentations;
                Microsoft.Office.Interop.PowerPoint._Presentation oPres =
                    oPresSet.Open(sourcePath + String.Format(@"\{0} {1} {2} {3}.pptx", car.ModelYear, car.Make, car.Model, car.Trim),
                        Microsoft.Office.Core.MsoTriState.msoFalse,
                        Microsoft.Office.Core.MsoTriState.msoFalse,
                        Microsoft.Office.Core.MsoTriState.msoTrue);

                string movie = sourcePath + String.Format(@"\{0} {1} {2} {3}.wmv", car.ModelYear, car.Make, car.Model, car.Trim);
                oPres.CreateVideo(movie, true, 4, 480, 30, 100);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
