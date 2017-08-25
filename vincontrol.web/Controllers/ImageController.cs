using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using vincontrol.Data.Model;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Security;

namespace Vincontrol.Web.Controllers
{
    public class ImageController : SecurityController
    {
        [HttpPost]
        public string UploadDealerPhoto(FormCollection formCollection)
        {
            try
            {
                if (Request.Files.Count == 0) return Controllers.Image.ErrorMessage.NoFile;

                if (Request.Files[0].ContentLength == 0) return Controllers.Image.ErrorMessage.NoContent;

                var folder = System.Web.HttpContext.Current.Server.MapPath("~/DealerImages") + "/" + SessionHandler.Dealer.DealershipId;
                var filename = String.Format("{0}DefaultStockImage-", SessionHandler.Dealer.DealershipId) + Path.GetFileName(Request.Files[0].FileName);
                var fileExtension = Path.GetExtension(filename).Substring(1);
                if (!Controllers.Image.SupportedTypes.Contains(fileExtension)) return Controllers.Image.ErrorMessage.NoSupport;

                // The maximum allowed file size is 512Kb.
                if (Request.Files[0].ContentLength > ((0.5 * 1024 * 1024))) return Controllers.Image.ErrorMessage.OverContent;

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                //if (System.IO.File.Exists(Path.Combine(folder, filename))) return Controllers.Image.ErrorMessage.FileExists;
                
                {
                    {
                        string filePath = Path.Combine(folder, filename);

                        Request.Files[0].SaveAs(filePath);
                        var defaultStockImageUrl = GetWebAppRoot() + "DealerImages/" + SessionHandler.Dealer.DealershipId + "/" + filename;
                        SQLHelper.UpdateDefaultStockImageUrl(SessionHandler.Dealer.DealershipId, defaultStockImageUrl);
                        SessionHandler.Dealer.DealerSetting.DefaultStockImageUrl = defaultStockImageUrl;

                        return defaultStockImageUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                return Controllers.Image.ErrorMessage.SystemError;
            }
        }

        [HttpPost]
        public void UploadPhoto(string dealerId, string listingId, string vin, FormCollection form)
        {
            var overlay = form["overlay"].Split(',')[0];
            var list = new List<string>();
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                UploadImage(dealerId, Convert.ToInt32(listingId), vin, Convert.ToInt32(overlay), file);
                //list.Add(UploadImage(dealerId, Convert.ToInt32(listingId), vin, Convert.ToInt32(overlay), file));
            }

            //return Json(new { list });
        }

        [HttpPost]
        public JsonResult DeleteInventoryPhoto(string listingId, string photo)
        {
            int id = Convert.ToInt32(listingId);
            try
            {
                using (var dbContext = new VincontrolEntities())
                {
                    var row = dbContext.Inventories.FirstOrDefault(x => x.InventoryId == id);
                    if (row != null)
                    {
                        var normalPhoto = photo.Replace("ThumbnailSizeImages", "NormalSizeImages");

                        var photoList = row.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        photoList.Remove(normalPhoto);
                        var thumbnailPhotoList = row.ThumbnailUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        thumbnailPhotoList.Remove(photo);

                        row.PhotoUrl = String.Join(",", photoList);
                        row.ThumbnailUrl = String.Join(",", thumbnailPhotoList);
                        
                        row.LastUpdated = DateTime.Now;

                        dbContext.SaveChanges();
                    }
                }

                return Json(new { status = "OK" });
            }
            catch (Exception)
            {
                return Json(new { status = "Failed" });
            }            
        }

        public JsonResult UpdateOrder(string listingId, List<String> values)
        {
            int id = Convert.ToInt32(listingId);
            try
            {
                using (var dbContext = new VincontrolEntities())
                {
                    var row = dbContext.Inventories.FirstOrDefault(x => x.InventoryId == id);
                    if (row != null)
                    {
                        var photos = String.Join(",", values);

                        row.PhotoUrl = photos.Replace("ThumbnailSizeImages", "NormalSizeImages");
                        row.ThumbnailUrl = photos;

                        row.LastUpdated = DateTime.Now;

                        dbContext.SaveChanges();
                    }
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception)
            {
                return Json(new { status = "Failed" });
            }
            
        }

        #region Private Methods

        private void UploadImage(string dealerId, int listingId, string vin, int overlay, HttpPostedFileBase file)
        {            
            string imageFileName = (Regex.Replace(file.FileName, @"[^0-9a-zA-Z]+", "-"));
            
            ImageDirectories imageDirectories = CreateFolder(dealerId, vin);
            string resultFileName = imageDirectories.FullSizeDirectory.FullName + "/" + imageFileName + ".jpg";
            //using (FileStream fileStream = System.IO.File.Create(resultFileName))
            //{
            //    var bytes = new byte[4096]; //100MB max
            //    int totalBytesRead;
            //    while ((totalBytesRead = HttpContext.Request.InputStream.Read(bytes, 0, bytes.Length)) != 0)
            //    {
            //        fileStream.Write(bytes, 0, totalBytesRead);
            //    }
            //}
            file.SaveAs(resultFileName);

            var image = ProcessImage(dealerId, vin, overlay, imageFileName, imageDirectories.FullSizeDirectory, imageDirectories.ThumbnailDirectory);
                        
            {                
                using (var dbContext = new VincontrolEntities())
                {
                    var row = dbContext.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
                    if (row != null)
                    {
                        if (String.IsNullOrEmpty(row.PhotoUrl))
                            row.PhotoUrl = image.FileUrl;
                        else
                        {
                            row.PhotoUrl = row.PhotoUrl + ',' + image.FileUrl;
                        }

                        if (String.IsNullOrEmpty(row.ThumbnailUrl))
                        {
                            row.ThumbnailUrl = image.ThumbnailUrl;
                        }
                        else
                        {
                            row.ThumbnailUrl = row.ThumbnailUrl + ',' + image.ThumbnailUrl;
                        }

                        row.LastUpdated = DateTime.Now;

                        dbContext.SaveChanges();
                    }
                }
            }

            //var js = new JavaScriptSerializer();
            //HttpContext.Response.Write(js.Serialize(image));
            HttpContext.Response.Write(image.ThumbnailUrl);
            //return image.ThumbnailUrl;
        }

        private ServerImage ProcessImage(string dealerId, string vin, int overlay, string imageFileName, DirectoryInfo dirNormal, DirectoryInfo dirThumbnail)
        {
            if (overlay == 1)
            {
                ImageHelper.OverlayImage(dirNormal.FullName + "/" + imageFileName + ".jpg", dirNormal.FullName + "/" + imageFileName + ".jpg", dealerId);
            }

            ImageResizer.ImageBuilder.Current.Build(dirNormal.FullName + "/" + imageFileName + ".jpg", dirThumbnail.FullName + "/" + imageFileName + ".jpg", new ImageResizer.ResizeSettings("maxwidth=260&maxheight=260&format=jpg"));

            return new ServerImage
            {
                FileUrl = GetWebAppRoot() + "DealerImages/" + dealerId + "/" + vin + "/NormalSizeImages/" + imageFileName + ".jpg",
                ThumbnailUrl = GetWebAppRoot() + "DealerImages/" + dealerId + "/" + vin + "/ThumbnailSizeImages/" + imageFileName + ".jpg",

            };
        }

        private ImageDirectories CreateFolder(string dealerId, string vin)
        {
            var physicalImagePath = System.Web.HttpContext.Current.Server.MapPath("/DealerImages") + "/" + dealerId + "/" + vin + "/NormalSizeImages";

            var physicalImageThumbNailPath = System.Web.HttpContext.Current.Server.MapPath("/DealerImages") + "/" + dealerId + "/" + vin + "/ThumbnailSizeImages";

            var dirNormal = new DirectoryInfo(physicalImagePath);

            var dirThumbnail = new DirectoryInfo(physicalImageThumbNailPath);

            if (!dirNormal.Exists)
                dirNormal.Create();

            if (!dirThumbnail.Exists)
                dirThumbnail.Create();

            return new ImageDirectories { FullSizeDirectory = dirNormal, ThumbnailDirectory = dirThumbnail };
        }
        
        private string GetWebAppRoot()
        {
            return (System.Web.HttpContext.Current.Request.Url.Port == 80) ? String.Format("{0}://{1}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host) : String.Format("{0}://{1}:{2}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

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

    public class File
    {
        public static class ErrorMessage
        {
            public const string SystemError = "SystemError";
            public const string NoFile = "NoFile";
            public const string NoContent = "NoContent";
            public const string NoSupport = "NoSupport";
            public const string OverContent = "OverContent";
            public const string FileExists = "FileExists";
        }

        public static string UploadedFolder = "~/UploadedFiles";
        public static string[] SupportedTypes = new[] { "doc", "docx", "pdf" };
    }

    public class Image
    {
        public static class ErrorMessage
        {
            public const string SystemError = "SystemError";
            public const string NoFile = "NoFile";
            public const string NoContent = "NoContent";
            public const string NoSupport = "NoSupport";
            public const string OverContent = "OverContent";
            public const string FileExists = "FileExists";
        }

        public static string UploadedFolder = "~/UploadedFiles";
        public static string[] SupportedTypes = new[] { "jpg", "jpeg", "gif", "png" };
    }
}
