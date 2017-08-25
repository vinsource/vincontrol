using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Vincontrol.Web.HelperClass
{
    public class EncryptionHelper
    {
        public static string EncryptString(string clearText)
        {
            if (!string.IsNullOrEmpty(clearText))
            {

                byte[] clearTextBytes = Encoding.UTF8.GetBytes(clearText);

                var rijn = SymmetricAlgorithm.Create();

                var ms = new MemoryStream();
                byte[] rgbIV = Encoding.ASCII.GetBytes("ayojvlzmdalyglrj");
                byte[] key = Encoding.ASCII.GetBytes("hlxilkqbbhczfeultgbskdmaunivmfuo");
                var cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV),
                                          CryptoStreamMode.Write);

                cs.Write(clearTextBytes, 0, clearTextBytes.Length);

                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            return string.Empty;
        }

        public static string DecryptString(string encryptedText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText.Replace(" ", "+").Replace(@"\/", "/"));

            var ms = new MemoryStream();

            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();


            byte[] rgbIV = Encoding.ASCII.GetBytes("ayojvlzmdalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("hlxilkqbbhczfeultgbskdmaunivmfuo");

            var cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIV),
            CryptoStreamMode.Write);

            cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());

        }
    }
}
