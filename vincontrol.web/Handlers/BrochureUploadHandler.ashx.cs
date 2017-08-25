using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace Vincontrol.Web.Handlers
{
    /// <summary>
    /// Summary description for BrochureUploadHandler
    /// </summary>
    public class BrochureUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            UploadBrochurePDF(context);
        }

        public bool IsReusable { get; private set; }

        private void UploadBrochurePDF(HttpContext context)
        {
            string message;
            try
            {
                string year = context.Request.QueryString["Year"];
                string make = context.Request.QueryString["Make"];
                string model = context.Request.QueryString["Model"];

                string fileName = context.Request.QueryString["PDFFileName"];

                CreateFolder(year, make, model);
                var fileFullPath = GetServerPath(fileName, year, make, model);
                if ((new FileInfo(fileFullPath)).Exists)
                {
                    message = "FileExisted";
                }
                else
                {
                    using (FileStream fileStream = File.Create(fileFullPath))
                    {
                        var bytes = new byte[4096]; //100MB max
                        int totalBytesRead;
                        while ((totalBytesRead = context.Request.InputStream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            fileStream.Write(bytes, 0, totalBytesRead);
                        }
                    }
                    message = "Successful";
                }
            }
            catch (Exception)
            {
                message = "Fail";
            }

            var js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(message));
        }
       
        private void CreateFolder(string year, string make, string model)
        {
            var path = string.Format("{0}\\Brochures\\{1}\\{2}\\{3}", System.Web.HttpContext.Current.Server.MapPath("\\PDF Files"),year , make, model);
            Directory.CreateDirectory(path);
        }

        private string GetServerPath(string fileName, string year, string make, string model)
        {
            return string.Format("{0}\\Brochures\\{1}\\{2}\\{3}\\{4}", System.Web.HttpContext.Current.Server.MapPath("\\PDF Files"), year, make, model, fileName);
        }

      
    }
}