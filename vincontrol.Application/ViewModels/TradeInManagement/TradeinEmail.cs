using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vincontrol.Application.ViewModels.TradeInManagement
{
    public class TradeinEmail
    {
        public string TextContent { get; set; }
        public string AdfContent { get; set; }
        public IEnumerable<string> Receivers { get; set; }
    }
}