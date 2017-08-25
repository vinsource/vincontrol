using System.Collections.Generic;
using VINCapture.UploadImage.Models;

namespace VINCapture.UploadImage.ViewModels
{
    internal class EmailComparer : IEqualityComparer<EmailItem>
    {
        public bool Equals(EmailItem x, EmailItem y)
        {
            return x.Email.Trim().Equals(y.Email.Trim());
        }

        public int GetHashCode(EmailItem obj)
        {
            return obj.Email.GetHashCode();
        }
    }
}