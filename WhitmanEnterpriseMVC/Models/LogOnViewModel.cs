using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class LogOnViewModel
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }

        public string LoginStatus { get; set; }


    }
}
