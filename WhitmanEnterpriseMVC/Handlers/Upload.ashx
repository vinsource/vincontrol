<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using System.IO;
using WhitmanEnterpriseMVC.HelperClass;



public class Upload : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;
        HttpPostedFile postedFile = context.Request.Files["Filedata"];
        try
        {
            string DealerId = context.Request.QueryString["DealerId"];

            string ListingId = context.Request.QueryString["ListingId"];

            string imageFileName = ListingId + "-" + postedFile.FileName;
        
            BasicFTPClient MyClient = new BasicFTPClient();

            MyClient.connect();

            MyClient.Upload("vinanalysis", postedFile.InputStream, DealerId, imageFileName);

            MyClient.closeConnect();

            string relativePath = "http://vinanalysis.com/" + DealerId + "/";
           
            context.Response.Write(relativePath + imageFileName);
            
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
    
  
}