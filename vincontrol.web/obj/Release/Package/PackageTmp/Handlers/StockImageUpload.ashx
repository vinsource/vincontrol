<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using System.IO;
using Vincontrol.Web.HelperClass;


public class Upload : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;
        HttpPostedFile postedFile = context.Request.Files["Filedata"];
        string relativeImagePath = "";
        try
        {
            
            var DealerId = context.Request.QueryString["DealerId"];

            var imageFileName = DealerId + "DefaultStockImage-" + postedFile.FileName;
        
            var dir = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/DealerImages") + "/" + DealerId);

            if (!dir.Exists)
                dir.Create();

            relativeImagePath = GetWebAppRoot() + "DealerImages/" + DealerId + "/" + imageFileName;

            postedFile.SaveAs(dir.FullName + "/" + imageFileName);

            context.Response.Write(relativeImagePath);
            
            context.Response.StatusCode = 200;
       
        }
        catch (Exception ex)
        {
            context.Response.Write("Error: " + ex.Message);
        }
    }


   
    public bool IsReusable {
        get {
            return false;
        }
    }

    public string GetWebAppRoot()
    {
        return String.Format("{0}://{1}:{2}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port);
        //return String.Format("{0}://{1}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.OriginalString);
    }
    
  
}