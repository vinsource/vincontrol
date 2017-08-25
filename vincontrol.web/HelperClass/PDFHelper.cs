﻿using System;
﻿using System.Configuration;
﻿using HiQPdf;
using System.Text;
using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace Vincontrol.Web.HelperClass
{
    public class PDFRender
    {
        public static string RenderViewAsString(string viewName, object model, ControllerContext controllerContext)
        {
            if (model != null)
            {
                // create a string writer to receive the HTML code
                StringWriter stringWriter = new StringWriter();

                // get the view to render
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
                // create a context to render a view based on a model
                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), stringWriter);
                // render the view to a HTML code
                viewResult.View.Render(viewContext, stringWriter);

                // return the HTML code
                return stringWriter.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}