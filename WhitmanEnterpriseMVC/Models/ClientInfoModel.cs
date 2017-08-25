using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WhitmanEnterpriseMVC.Models
{
    public class ClientInfoModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Stock { get; set; }

        public string Vin { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string DealerID { get; set; }

        public string Submit { get; set; }

    }

}
