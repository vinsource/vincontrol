using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class TradeinEmail
    {
        public string TextContent { get; set; }
        public string AdfContent { get; set; }
        public List<string> Receivers { get; set; }      
    }
}
