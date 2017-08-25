using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class EmailContentViewModel
    {
        public string TextContent { get; set; }
        public string ADFContent { get; set; }
        public string Receivers { get; set; }
    }
}
