using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace vincontrol.PdfHelper
{
    public interface IPdf
    {
        MemoryStream WritePdf(string bodyPdf);
    }
}
