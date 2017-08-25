<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;



public class Handler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";//"application/json";
        var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        string physicalImagePath = "";
        string physicalImageThumbNailPath = "";
        string relativeImagePath="";
        string relativeImageThumbNailPath = "";
        foreach (string file in context.Request.Files)
        {
            HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
            string FileName = string.Empty;
            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
            {
                string[] files = hpf.FileName.Split(new char[] { '\\' });
                FileName = files[files.Length - 1];
            }
            else
            {
                FileName = hpf.FileName;
            }
            if (hpf.ContentLength == 0)
                continue;

            string DealerId = context.Request.QueryString["DealerId"];

            string ListingId = context.Request.QueryString["ListingId"];

            string Vin = context.Request.QueryString["Vin"];

            int Overlay = Convert.ToInt32(context.Request.QueryString["Overlay"]);

            string imageFileName = ListingId + "-" + FileName;

            physicalImagePath = System.Web.HttpContext.Current.Server.MapPath("/DealerImages") + "/" + DealerId + "/" + Vin + "/NormalSizeImages";

            physicalImageThumbNailPath = System.Web.HttpContext.Current.Server.MapPath("/DealerImages") + "/" + DealerId + "/" + Vin + "/ThumbnailSizeImages";

            var dirNormal = new DirectoryInfo(physicalImagePath);

            var dirThumbnail = new DirectoryInfo(physicalImageThumbNailPath);

            if (!dirNormal.Exists)
                dirNormal.Create();

            if (!dirThumbnail.Exists)
                dirThumbnail.Create();

            hpf.SaveAs(dirNormal.FullName + "/" + imageFileName);

            if (Overlay == 1)
            {

                ImageHelper.OverlayImage(
                    GetWebAppRoot() + "DealerImages/" + DealerId + "/" + Vin + "/NormalSizeImages/" +
                    imageFileName, dirNormal.FullName + "/" + imageFileName, DealerId);



            }

            ImageResizer.ImageBuilder.Current.Build(dirNormal.FullName + "/" + imageFileName, dirThumbnail.FullName + "/" + imageFileName, new ImageResizer.ResizeSettings("maxwidth=260&maxheight=260&format=jpg"));
            
            relativeImagePath = GetWebAppRoot() + "DealerImages/" + DealerId + "/" + Vin + "/NormalSizeImages/" + imageFileName;

            relativeImageThumbNailPath = GetWebAppRoot() + "DealerImages/" + DealerId + "/" + Vin + "/ThumbnailSizeImages/" + imageFileName;


             //using (Image Img = Image.FromFile(dir.FullName + "/" + imageFileName))
            //{
            //    Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 79);
                

            //    using (Image ImgThnail = new Bitmap(Img, NewSize.Width, NewSize.Height))
            //    {
            //        //string ss = savedFileName.Substring(0, savedFileName.Length - Path.GetFileName(savedFileName).Length) + Path.GetFileNameWithoutExtension(savedFileName) + "-thumb" + Path.GetExtension(savedFileName);
            //        ImgThnail.Save(savedFileName + ".tmp", Img.RawFormat);
            //        ImgThnail.Dispose();
            //    }
            //    Img.Dispose();
            //}


            //if (System.Web.HttpContext.Current.Request.Path.Contains("beta"))

            //    relativeImagePath = GetWebAppRoot() + "beta/DealerImages/" + DealerId + "/" + imageFileName;
            //else
            //    relativeImagePath = GetWebAppRoot() + "VinControl/DealerImages/" + DealerId + "/" + imageFileName;
            //string relativeImagePath = "http://vinanalysis.com/" + DealerId + "/" + imageFileName;

            //BasicFTPClient MyClient = new BasicFTPClient();

            //MyClient.connect();

            //MyClient.Upload("vinanalysis",hpf.InputStream, DealerId, imageFileName);

            //MyClient.closeConnect();

                    
            r.Add(new ViewDataUploadFilesResult()
            {
                Thumbnail_url = relativeImageThumbNailPath,
                Image_url=relativeImagePath,
                Name = imageFileName,
                Length = hpf.ContentLength,
                Type = hpf.ContentType
            });
            var uploadedFiles = new
            {
                files = r.ToArray()
            };
            var jsonObj = js.Serialize(uploadedFiles);
          
            context.Response.StatusCode = 200;
            context.Response.Write(jsonObj.ToString());
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public string GetWebAppRoot()
    {
        return String.Format("{0}://{1}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host);

    }

}

public class ViewDataUploadFilesResult
{
    public string Thumbnail_url { get; set; }
    public string Image_url { get; set; }
    public string Name { get; set; }
    public int Length { get; set; }
    public string Type { get; set; }
}