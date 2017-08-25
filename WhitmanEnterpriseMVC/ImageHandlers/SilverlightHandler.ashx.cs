using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.IO;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.HelperClass;
using System.Web.Script.Serialization;
using WhitmanEnterpriseMVC.Models;
using System.Runtime.Serialization;

namespace WhitmanEnterpriseMVC.ImageHandlers
{
    /// <summary>
    /// Summary description for TestHandler
    /// </summary>
    public class SilverlightHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string function = context.Request.QueryString["Function"];
            switch (function)
            {
                case "GetImageUrl":
                    GetImageUrl(context);
                    break;
                case "GetImageUrlList":
                    GetImageUrlList(context);
                    break;
                case "SaveImageUrl":
                    SaveImageUrl(context);
                    break;
                case "UploadAndSaveImageUrl":
                    UploadAndSaveImageUrl(context);
                    break;
                default:
                    UploadImage(context, false);
                    break;
            }
        }

        private void UploadAndSaveImageUrl(HttpContext context)
        {
            UploadImage(context, true);
        }

        private void GetImageUrlList(HttpContext context)
        {
            string listingId = context.Request.QueryString["ListingId"];
            int result;
            if (int.TryParse(listingId, out result))
            {
                var carImageUrLs = SQLHelper.GetImageURLs(result);
                string outputString = null;
                if (!String.IsNullOrEmpty(carImageUrLs.ImageURLs))
                {
                    var list = new List<ServerImage>();

                    var fullImageUrLs = CommonHelper.GetArrayFromString(carImageUrLs.ImageURLs);
                    var thumbnailImageUrLs = CommonHelper.GetArrayFromString(carImageUrLs.ThumnailURLs);
                    int count = fullImageUrLs.Length;

                    for (int i = 0; i < count; i++)
                    {
                        var image = new ServerImage
                            {
                                FileUrl = fullImageUrLs[i],
                                ThumbnailUrl = thumbnailImageUrLs[i]
                            };
                        list.Add(image);
                    }

                    var js = new JavaScriptSerializer();
                    outputString = js.Serialize(list);
                }

                context.Response.Write(outputString ?? "0");
            }
        }

        private void SaveImageUrl(HttpContext context)
        {
            if (context.Request.QueryString["InventoryStatus"] != null && context.Request.QueryString["ListingId"] != null)
            {
                int inventoryStatus = int.Parse(context.Request.QueryString["InventoryStatus"].ToString(CultureInfo.InvariantCulture));
                int listingId = int.Parse(context.Request.QueryString["ListingId"].ToString(CultureInfo.InvariantCulture));

                var reader = new StreamReader(context.Request.InputStream);
                string result = reader.ReadToEnd();
                var js = new JavaScriptSerializer();
                var carImage = js.Deserialize<CarImage>(result);

                var image = new ImageViewModel
                    {
                        InventoryStatus = inventoryStatus,
                        ListingId = listingId,
                        ImageUploadFiles = carImage.FileUrLs,
                        ThumbnailImageUploadFiles = carImage.ThumbnailUrLs
                    };

                SQLHelper.ReplaceCarImageURL(image);
                context.Response.Write("Successful");
            }
        }

        private void GetImageUrl(HttpContext context)
        {
            var fileName = context.Request.QueryString["uploadedfile"];
            var dealerId = context.Request.QueryString["DealerId"];
            var vin = context.Request.QueryString["Vin"];

            var image = new ServerImage
                {
                    FileUrl = string.Format("{0}DealerImages/{1}/{2}/NormalSizeImages/{3}", GetWebAppRoot(), dealerId, vin, fileName),
                    ThumbnailUrl = string.Format("{0}DealerImages/{1}/{2}/ThumbnailSizeImages/{3}", GetWebAppRoot(), dealerId, vin, fileName)
                };
            var js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(image));
        }

        private void UploadImage(HttpContext context, bool saveImage)
        {
            string dealerId = context.Request.QueryString["DealerId"];
            string vin = context.Request.QueryString["Vin"];
            int overlay = Convert.ToInt32(context.Request.QueryString["Overlay"]);

            string fileName = context.Request.QueryString["uploadedfile"];

            string imageFileName = fileName;
            //string imageFileName = ListingId + "-" + FileName;

            ImageDirectories imageDirectories = CreateFolder(dealerId, vin);
            string resultFileName = imageDirectories.FullSizeDirectory.FullName + "/" + imageFileName + ".jpg";
            using (FileStream fileStream = File.Create(resultFileName))
            {
                var bytes = new byte[4096]; //100MB max
                int totalBytesRead;
                while ((totalBytesRead = context.Request.InputStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    fileStream.Write(bytes, 0, totalBytesRead);
                }
            }

            var image = ProcessImage(dealerId, vin, overlay, imageFileName, imageDirectories.FullSizeDirectory, imageDirectories.ThumbnailDirectory);

            if (saveImage)
            {
                int listingId = int.Parse(context.Request.QueryString["ListingId"].ToString(CultureInfo.InvariantCulture));
                using (var dbContext = new whitmanenterprisewarehouseEntities())
                {
                    var row =
                        dbContext.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == listingId);
                    if (row != null)
                    {
                        if (String.IsNullOrEmpty(row.CarImageUrl))
                            row.CarImageUrl = image.FileUrl;
                        else
                        {
                            row.CarImageUrl = row.CarImageUrl + ',' + image.FileUrl;    
                        }

                        if (String.IsNullOrEmpty(row.ThumbnailImageURL))
                        {
                            row.ThumbnailImageURL =image.ThumbnailUrl;
                        }
                        else
                        {
                            row.ThumbnailImageURL = row.ThumbnailImageURL + ',' + image.ThumbnailUrl;
                        }

                       

                        row.LastUpdated = DateTime.Now;

                        dbContext.SaveChanges();

                    }
                }
            }


            var js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(image));
        }

        private ServerImage ProcessImage(string dealerId, string vin, int overlay, string imageFileName, DirectoryInfo dirNormal, DirectoryInfo dirThumbnail)
        {
            if (overlay == 1)
            {
                //ImageHelper.OverlayImage(
                //    GetWebAppRoot() + "DealerImages/" + dealerId + "/" + vin + "/NormalSizeImages/" +
                //    imageFileName, dirNormal.FullName + "/" + imageFileName, dealerId);

                ImageHelper.OverlayImage(
               dirNormal.FullName + "/" + imageFileName + ".jpg", dirNormal.FullName + "/" + imageFileName + ".jpg", dealerId);
            }

            ImageResizer.ImageBuilder.Current.Build(dirNormal.FullName + "/" + imageFileName + ".jpg", dirThumbnail.FullName + "/" + imageFileName + ".jpg", new ImageResizer.ResizeSettings("maxwidth=260&maxheight=260&format=jpg"));

            return new ServerImage
            {
                FileUrl = GetWebAppRoot() + "DealerImages/" + dealerId + "/" + vin + "/NormalSizeImages/" + imageFileName + ".jpg",
                //NewFileUrl = GetWebAppRoot() + "DealerImages/" + dealerId + "/" + vin + "/NormalSizeImages/" + Guid.NewGuid() + '.' + (imageFileName.Split('.').Count() >= 2 ? imageFileName.Split('.')[1] : String.Empty),
                ThumbnailUrl = GetWebAppRoot() + "DealerImages/" + dealerId + "/" + vin + "/ThumbnailSizeImages/" + imageFileName + ".jpg",
                //NewThumbnailUrl = GetWebAppRoot() + "DealerImages/" + dealerId + "/" + vin + "/ThumbnailSizeImages/" + Guid.NewGuid() + '.' + (imageFileName.Split('.').Count() >= 2 ? imageFileName.Split('.')[1] : String.Empty)
            };
        }

        public string GetWebAppRoot()
        {
            //return String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            return String.Format("{0}://{1}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host);
        }

        private static ImageDirectories CreateFolder(string dealerId, string vin)
        {
            var physicalImagePath = HttpContext.Current.Server.MapPath("/DealerImages") + "/" + dealerId + "/" + vin + "/NormalSizeImages";

            var physicalImageThumbNailPath = HttpContext.Current.Server.MapPath("/DealerImages") + "/" + dealerId + "/" + vin + "/ThumbnailSizeImages";

            var dirNormal = new DirectoryInfo(physicalImagePath);

            var dirThumbnail = new DirectoryInfo(physicalImageThumbNailPath);

            if (!dirNormal.Exists)
                dirNormal.Create();

            if (!dirThumbnail.Exists)
                dirThumbnail.Create();

            return new ImageDirectories { FullSizeDirectory = dirNormal, ThumbnailDirectory = dirThumbnail };
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    #region Models

    public class ImageDirectories
    {
        public DirectoryInfo FullSizeDirectory { get; set; }
        public DirectoryInfo ThumbnailDirectory { get; set; }
    }

    public class ProcessedImage
    {
        public string RelativeImagePath { get; set; }
        public string RelativeImageThumbNailPath { get; set; }
    }

    public class ViewDataUploadFilesResult
    {
        public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }

    [DataContract]
    public class ServerImage
    {
        [DataMember]
        public string FileUrl { get; set; }
        [DataMember]
        public string ThumbnailUrl { get; set; }
        //public string NewThumbnailUrl { get; set; }
        //public string NewFileUrl { get; set; }
    }

    [DataContract]
    public class CarImage
    {
        [DataMember]
        public string ThumbnailUrLs { get; set; }
        [DataMember]
        public string FileUrLs { get; set; }
    }

    #endregion
}