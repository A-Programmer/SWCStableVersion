using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PortalCore.Logic
{
    public class EncryptDecrypt
    {
        private const string SecretKey = "M0N1R+ks"; // any 8 char
        private byte[] _key = { };
        private readonly byte[] _b4 = { 202, 007, 031, 037, 246, 121, 113, 191 }; // any 8 numbers(255 is max)

        //Decrypt
        public string Decrypt(string txt)
        {
            if (txt == null || txt.Trim() == "")
            {
                return null;
            }
            string encryptKey = SecretKey;
            try
            {
                _key = System.Text.Encoding.UTF8.GetBytes(encryptKey.ToCharArray(), 0, 8);
                var des = new DESCryptoServiceProvider();
                var inputByte = Convert.FromBase64String(txt.Trim());
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(_key, _b4), CryptoStreamMode.Write);
                cs.Write(inputByte, 0, inputByte.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        //Encrypt
        public string Encrypt(string txt)
        {
            if (txt == null || txt.Trim() == "")
            {
                return null;
            }
            const string encryptKey = SecretKey;
            try
            {
                _key = System.Text.Encoding.UTF8.GetBytes(encryptKey.ToCharArray(), 0, 8);
                var des = new DESCryptoServiceProvider();
                var inputByte = Encoding.UTF8.GetBytes(txt);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(_key, _b4), CryptoStreamMode.Write);
                cs.Write(inputByte, 0, inputByte.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

    }
}