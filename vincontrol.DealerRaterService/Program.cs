using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using vincontrol.Crawler.Helpers;
using vincontrol.Data.Model;

namespace vincontrol.DealerRaterService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var crawlerService = new Service();
            crawlerService.Execute();
            //NOTE: Testing
            //crawlerService.Execute("plus.google.com", 4);
        }
    }
}
