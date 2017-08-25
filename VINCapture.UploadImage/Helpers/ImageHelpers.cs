using System;
using System.Configuration;
using System.IO;
using System.Text;
using HiQPdf;
using VINCapture.UploadImage.Models;

namespace VINCapture.UploadImage.USBHelpers
{
    public static class ImageHelpers
    {
        #region Helpers

        public static MemoryStream OverlayImage(string mainImageUrl, DealerUser user)
        {
            var htmlToImageConverter = new HtmlToImage
            {
                SerialNumber = ConfigurationManager.AppSettings["PDFSerialNumber"],
                BrowserWidth = 300,
                HtmlLoadedTimeout = 2,
                TransparentImage = false
            };

            var htmlCode =
                GenerateHtmlImageCode(mainImageUrl, user);

            if (!String.IsNullOrEmpty(htmlCode))
            {

                System.Drawing.Image imageObject = htmlToImageConverter.ConvertHtmlToImage(htmlCode, null)[0];
                var stream = new MemoryStream();
                imageObject.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Position = 0;
                return stream;
            }

            return null;

        }

        public static string GenerateHtmlImageCode(string mainImageUrl, DealerUser user)
        {
            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(user.HeaderOverlayUrl) && !String.IsNullOrEmpty(user.FooterOverlayUrl))
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
                //builder.Append(" padding-top: 2%;");

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
                builder.Append("  <div class=\"overlay-top\"><img src=\"" + user.HeaderOverlayUrl +
                               "\" /></div>");

                builder.Append("<div>  <img class=\"car-photo\" src=\"" + mainImageUrl + "\" /> </div>");
                builder.Append("  <div class=\"overlay-bottom\"><img src=\"" + user.FooterOverlayUrl +
                               "\" /></div>");

                builder.Append("</div>");
                builder.Append("</body>");

                builder.Append("</html>");
            }

            return builder.ToString();
        }
        #endregion
    }
}
