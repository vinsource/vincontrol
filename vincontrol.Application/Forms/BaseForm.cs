using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using vincontrol.Data.Interface;

namespace vincontrol.Application.Forms
{
    public abstract class BaseForm
    {
        protected IUnitOfWork UnitOfWork;

        protected DateTime Today
        {
            get
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
        }

        protected string EncryptString(string clearText)
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

        protected string DecryptString(string encryptedText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText.Replace(" ", "+").Replace(@"\/", "/"));

            var ms = new MemoryStream();

            var rijn = SymmetricAlgorithm.Create();

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
