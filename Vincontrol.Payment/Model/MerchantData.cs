using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincontrol.Payment.Model
{
    public class MerchantData
    {
        [Required]
        [RegularExpression(@"^[A-Z][A-Za-z]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_FirstName;

        [Required]
        [RegularExpression(@"^[A-Z][A-Za-z]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_LastName;

        [RegularExpression(@"^[A-Z][A-Za-z]{1,10}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_Title;

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s]{1,30}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_Address1;

        [RegularExpression(@"^[A-Za-z0-9\s]{1,30}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_Address2;

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_City;

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_State;

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_Zip;

        public string Principal_Country;

        [Required]
        [RegularExpression(@"^[0-9\-\.]{10,}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_Phone;

        [Required]
        [RegularExpression(@"^[0-9\-\.]{10,}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Principal_Email;

        [Required]
        [RegularExpression(@"^[\w\d\-\@\#\$\%\^\&\*\!\s,\+]{4,}$",
         ErrorMessage = "Characters are not allowed.")]
        public string CompanyName;

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\-\@\#\$\%\^\&\*\!]{5,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string UserName;

        [Required]
        [RegularExpression(@"^(?=.*[^a-zA-Z])(?=.*[a-z])(?=.*[A-Z])\S{8,}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Password;

        [RegularExpression(@"^[0-9\-\.]{10,}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Phone;
    }
}
