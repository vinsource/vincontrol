using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HiQPdf;
using vincontrol.Data.Model;

namespace VINControl.ImageHelper
{
    public class ImageHandler
    {
        public static void Migrate(string fromPath, string toPath, int dealerId, string alternativeSubFolder = "Corrected", bool makeOverlay = false)
        {
            var context = new VincontrolEntities();
            var rootFolder = new DirectoryInfo(fromPath);
            if (rootFolder.Exists)
            {
                var subFolders = Directory.GetDirectories(fromPath);
                foreach (var item in subFolders)
                {
                    var subFolder = new DirectoryInfo(item + (String.IsNullOrEmpty(alternativeSubFolder) ? "" : "\\" + alternativeSubFolder));
                    var subFolderName = Path.GetFileName(item);
                    var listOfNormalImages = new List<string>();
                    var listOfThumbnailImages = new List<string>();

                    var inventory = context.Inventories.FirstOrDefault(i => i.Vehicle.Vin.Equals(subFolderName) && i.DealerId.Equals(dealerId));
                    if (inventory == null) continue;                        

                    var newNormalFolder = toPath + "\\" + subFolderName + "\\" + "NormalSizeImages";
                    var newThumbnailFolder = toPath + "\\" + subFolderName + "\\" + "ThumbnailSizeImages";
                    if (!Directory.Exists(newNormalFolder)) Directory.CreateDirectory(newNormalFolder);
                    if (!Directory.Exists(newThumbnailFolder)) Directory.CreateDirectory(newThumbnailFolder);

                    foreach (FileInfo image in subFolder.GetFiles().OrderBy(f => f.CreationTime))
                    {
                        if (image != null && (image.Extension.Equals(".jpg") || image.Extension.Equals(".png")) && (String.IsNullOrEmpty(inventory.PhotoUrl) || inventory.PhotoUrl.ToLower().Contains(image.Name.ToLower())))
                        {
                            var newImageName = String.IsNullOrEmpty(alternativeSubFolder) ? image.Name.Trim() : image.Name.Replace(alternativeSubFolder.ToLower(), "").Trim();
                            //System.IO.File.Copy(image.FullName, newNormalFolder + "\\" + newImageName, true);
                            image.CopyTo(newNormalFolder + "\\" + newImageName, true);
                            if (makeOverlay) OverlayImage(newNormalFolder + "\\" + newImageName, newNormalFolder + "\\" + newImageName, dealerId);

                            var imgObj = Image.FromFile(/*image.FullName*/newNormalFolder + "\\" + newImageName);
                            var tmp = Resize(imgObj, 260, 210);
                            WriteFile(tmp, newThumbnailFolder + "\\", newImageName);
                            //if (makeOverlay) OverlayImage(newThumbnailFolder + "\\" + newImageName, newThumbnailFolder + "\\" + newImageName, dealerId);
                            
                            listOfNormalImages.Add(String.Format("http://apps.vincontrol.com/DealerImages/{0}/{1}/{2}/{3}", dealerId, subFolderName, "NormalSizeImages", newImageName));
                            listOfThumbnailImages.Add(String.Format("http://apps.vincontrol.com/DealerImages/{0}/{1}/{2}/{3}", dealerId, subFolderName, "ThumbnailSizeImages", newImageName));
                        }
                    }

                    if (listOfNormalImages.Any())
                    {
                        {
                            inventory.PhotoUrl = listOfNormalImages.Aggregate((x, y) => x + "," + y);
                            inventory.ThumbnailUrl = listOfThumbnailImages.Aggregate((x, y) => x + "," + y);
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public static void Rename(string fromPath, string toPath, int dealerId, string alternativeSubFolder = "Corrected", bool makeOverlay = false)
        {
            Console.WriteLine("*** START ***");
            var context = new VincontrolEntities();
            var rootFolder = new DirectoryInfo(fromPath);
            if (rootFolder.Exists)
            {
                var subFolders = Directory.GetDirectories(fromPath);
                foreach (var item in subFolders)
                {
                    var subFolder = new DirectoryInfo(item + (String.IsNullOrEmpty(alternativeSubFolder) ? "" : "\\" + alternativeSubFolder));
                    var subFolderName = Path.GetFileName(item);
                    var listOfNormalImages = new List<string>();
                    var listOfThumbnailImages = new List<string>();
                    Console.WriteLine("*** {0} ***", subFolderName);
                    var inventory = context.Inventories.FirstOrDefault(i => i.Vehicle.Vin.Equals(subFolderName) && i.DealerId.Equals(dealerId));
                    if (inventory == null) continue;

                    var newNormalFolder = toPath + "\\" + subFolderName + "\\" + "NormalSizeImages";
                    var newThumbnailFolder = toPath + "\\" + subFolderName + "\\" + "ThumbnailSizeImages";
                    if (!Directory.Exists(newNormalFolder)) Directory.CreateDirectory(newNormalFolder);
                    if (!Directory.Exists(newThumbnailFolder)) Directory.CreateDirectory(newThumbnailFolder);

                    var count = 1;
                    foreach (FileInfo image in subFolder.GetFiles().OrderBy(f => f.CreationTime))
                    {
                        if (image != null && (image.Extension.Equals(".jpg") || image.Extension.Equals(".png")) && (String.IsNullOrEmpty(inventory.PhotoUrl) || inventory.PhotoUrl.ToLower().Contains(image.Name.ToLower())))
                        {
                            var newImageName = String.Format("{0} {1} {2} {3}.jpg", inventory.Vehicle.Year, inventory.Vehicle.Make, inventory.Vehicle.Model, string.IsNullOrEmpty(inventory.Vehicle.Trim) ? (count++).ToString() : inventory.Vehicle.Trim + " " + (count++).ToString());
                            newImageName = newImageName.Replace(" ", "-").Replace("/", "").Replace("\\", "");
                            //image.CopyTo(newNormalFolder + "\\" + newImageName, true);
                            //File.Copy(image.FullName, newNormalFolder + "\\" + newImageName, true);
                            if (!File.Exists(newNormalFolder + "\\" + newImageName))
                                image.CopyTo(newNormalFolder + "\\" + newImageName, true);
                            
                            if (makeOverlay) OverlayImage(newNormalFolder + "\\" + newImageName, newNormalFolder + "\\" + newImageName, dealerId);
                            Console.WriteLine(newNormalFolder + "\\" + newImageName);
                            var imgObj = Image.FromFile(/*image.FullName*/newNormalFolder + "\\" + newImageName);
                            var tmp = Resize(imgObj, 260, 210);
                            WriteFile(tmp, newThumbnailFolder + "\\", newImageName);
                            //if (makeOverlay) OverlayImage(newThumbnailFolder + "\\" + newImageName, newThumbnailFolder + "\\" + newImageName, dealerId);

                            listOfNormalImages.Add(String.Format("http://apps.vincontrol.com/DealerImages/{0}/{1}/{2}/{3}", dealerId, subFolderName, "NormalSizeImages", newImageName));
                            listOfThumbnailImages.Add(String.Format("http://apps.vincontrol.com/DealerImages/{0}/{1}/{2}/{3}", dealerId, subFolderName, "ThumbnailSizeImages", newImageName));
                        }
                    }

                    if (listOfNormalImages.Any())
                    {
                        {
                            inventory.PhotoUrl = listOfNormalImages.Aggregate((x, y) => x + "," + y);
                            inventory.ThumbnailUrl = listOfThumbnailImages.Aggregate((x, y) => x + "," + y);
                        }
                    }
                    
                }
                context.SaveChanges();
            }
            Console.WriteLine("*** END ***");
        }

        public static void OverlayImage(string mainImageUrl, string rootImageUrl, int dealerId)
        {
            var htmlToImageConverter = new HtmlToImage
            {
                SerialNumber = ConfigurationManager.AppSettings["PDFSerialNumber"],
                BrowserWidth = 300,
                HtmlLoadedTimeout = 2,
                TransparentImage = false
            };

            string htmlCode = GenerateHtmlImageCode(mainImageUrl, dealerId);

            if (!String.IsNullOrEmpty(htmlCode))
            {
                System.Drawing.Image imageObject = htmlToImageConverter.ConvertHtmlToImage(htmlCode, null)[0];

                imageObject.Save(rootImageUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        public static string GenerateHtmlImageCode(string mainImageUrl, int dealerId)
        {
            var context = new VincontrolEntities();
            var dealerSetting = context.Settings.First(x => x.DealerId == dealerId);

            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(dealerSetting.HeaderOverlayURL) && !String.IsNullOrEmpty(dealerSetting.FooterOverlayURL))
            {
                builder.Append("<html>");

                builder.Append("<head>");
                builder.Append("  <title></title>");
                builder.Append("</head>");

                builder.Append("<style type=\"text/css\">");
                builder.Append("html, body, div {");
                builder.Append("  padding: 0;");
                builder.Append("  margin: 0;");
                builder.Append("}");
                builder.Append("div {overflow: hidden;}");
                builder.Append("  .image-wrap {");
                builder.Append("    display: inline-block;");
                builder.Append("    position: relative;");
                builder.Append("  }");
                builder.Append(".image-wrap img.car-photo{");
                builder.Append("  }");
                builder.Append("  .overlay-top {");
                builder.Append("    width: 100%;");
                builder.Append("    background: white;");
                builder.Append("    text-align: center;");
                builder.Append(" }");
                builder.Append("  .overlay-top img {");
                builder.Append("    width: 100%;");
                builder.Append("}");
                builder.Append("  .overlay-bottom {");
                builder.Append("    font-size: 1.5em;");
                builder.Append("    width: 100%;");
                builder.Append("    color: white;");
                builder.Append("   font-weight: bold;");
                builder.Append("  }");
                builder.Append("  .overlay-bottom img{");
                builder.Append("   width: 100%;");
                builder.Append("}");
                builder.Append("</style>");

                builder.Append("<body>");
                builder.Append("<div class=\"image-wrap\">");
                builder.Append("  <div class=\"overlay-top\"><img src=\"" + dealerSetting.HeaderOverlayURL + "\" /></div>");
                builder.Append("  <img class=\"car-photo\" src=\"" + mainImageUrl + "\" />");
                builder.Append("  <div class=\"overlay-bottom\"><img src=\"" + dealerSetting.FooterOverlayURL + "\" /></div>");
                builder.Append("</div>");
                builder.Append("</body>");

                builder.Append("</html>");
            }
            else if (!String.IsNullOrEmpty(dealerSetting.OverlayHtmlUrl))
            {
                using (var client = new WebClient())
                {
                    var htmlCode = client.DownloadString(dealerSetting.OverlayHtmlUrl);

                    htmlCode = htmlCode.Replace("PLACEHOLDER", mainImageUrl);

                    builder.Append(htmlCode);
                }
            }

            return builder.ToString();
        }
        
        private static Image Resize(Image image, int width, int height)
        {
            int w = image.Width;
            int h = image.Height;
            Image resized = null;
            resized = width > height ? ResizeImage(image, width, width) : ResizeImage(image, height, height);
            //Image output = CropImage(resized, width, height);
            //return output;
            return resized;
        }

        private static Image Resize(MemoryStream ms, int width, int height)
        {
            var image = Image.FromStream(ms, true, true);
            int w = image.Width;
            int h = image.Height;
            Image resized = null;
            resized = width > height ? ResizeImage(image, width, width) : ResizeImage(image, height, height);
            //Image output = CropImage(resized, width, height);
            //return output;
            return resized;
        }

        private static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            int width = image.Width;
            int height = image.Height;
            if (width > maxWidth || height > maxHeight)
            {
                //The flips are in here to prevent any embedded image thumbnails -- usually from cameras
                //from displaying as the thumbnail image later, in other words, we want a clean
                //resize, not a grainy one.
                image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
                image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);

                float ratio = 0;
                if (width > height)
                {
                    ratio = (float)width / (float)height;
                    width = maxWidth;
                    height = Convert.ToInt32(Math.Round((float)width / ratio));
                }
                else
                {
                    ratio = (float)height / (float)width;
                    height = maxHeight;
                    width = Convert.ToInt32(Math.Round((float)height / ratio));
                }

                //return the resized image
                return image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            }
            //return the original resized image
            return image;
        }

        private static Image CropImage(Image image, int width, int height)
        {
            try
            {
                //check the image height against our desired image height
                if (image.Height < height)
                {
                    height = image.Height;
                }

                if (image.Width < width)
                {
                    width = image.Width;
                }

                //create a bitmap window for cropping
                Bitmap bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);

                //create a new graphics object from our image and set properties
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //now do the crop
                grPhoto.DrawImage(image, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel);

                // Save out to memory and get an image from it to send back out the method.
                MemoryStream mm = new MemoryStream();
                bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                image.Dispose();
                bmPhoto.Dispose();
                grPhoto.Dispose();
                Image outimage = Image.FromStream(mm);

                return outimage;
            }
            catch (Exception ex)
            {
                throw new Exception("Error cropping image, the error was: " + ex.Message);
            }
        }

        private static void WriteFile(Image img, string path, string imageName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var ms = new MemoryStream())
            {
                using (var fs = new FileStream(path + imageName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    img.Save(ms, ImageFormat.Jpeg);
                    byte[] bytes = ms.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
    }
}
