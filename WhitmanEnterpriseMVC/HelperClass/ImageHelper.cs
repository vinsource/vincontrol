using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using HiQPdf;
using WhitmanEnterpriseMVC.DatabaseModel;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class ImageHelper
    {
        public static string GenerateHtmlImageCode(string mainImageUrl,string dealerId)
        {
            var context = new whitmanenterprisewarehouseEntities();
            
            int DealerId = Convert.ToInt32(dealerId);

            var dealerSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == DealerId);

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
                builder.Append(" padding-top: 2%;");

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
                builder.Append("  <div class=\"overlay-top\"><img src=\"" + dealerSetting.HeaderOverlayURL +
                               "\" /></div>");

                builder.Append("  <img class=\"car-photo\" src=\"" + mainImageUrl + "\" />");
                builder.Append("  <div class=\"overlay-bottom\"><img src=\"" + dealerSetting.FooterOverlayURL +
                               "\" /></div>");

                builder.Append("</div>");
                builder.Append("</body>");

                builder.Append("</html>");
            }

            return builder.ToString();
        }

        public static void OverlayImage(string mainImageUrl, string rootImageUrl,string dealerId)
        {
            var htmlToImageConverter = new HtmlToImage();

            htmlToImageConverter.SerialNumber = ConfigurationManager.AppSettings["PDFSerialNumber"];
            // set browser width
            htmlToImageConverter.BrowserWidth = 300;



            // set HTML Load timeout
            htmlToImageConverter.HtmlLoadedTimeout = 2;

            htmlToImageConverter.TransparentImage = false;

            System.Drawing.Image imageObject = null;

            string htmlCode =
                GenerateHtmlImageCode(mainImageUrl,dealerId);

            if (!String.IsNullOrEmpty(htmlCode))
            {

                imageObject = htmlToImageConverter.ConvertHtmlToImage(htmlCode, null)[0];

                imageObject.Save(rootImageUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

             }

            
        }


    

    }
}
