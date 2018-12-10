using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Finex.Utility
{
   public class Encryption
    {
       public static string Encrypt(string clearText, bool useHashing)
       {
           //byte[] keyArray;
           //byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

           //System.Configuration.AppSettingsReader settingsReader =
           //                                    new AppSettingsReader();
           //// Get the key from config file

           //string key = "abc@123";
           //    //(string)settingsReader.GetValue("SecurityKey",
           //    //                                             typeof(String));
           ////System.Windows.Forms.MessageBox.Show(key);
           ////If hashing use get hashcode regards to your key
           //if (useHashing)
           //{
           //    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
           //    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
           //    //Always release the resources and flush data
           //    // of the Cryptographic service provide. Best Practice

           //    hashmd5.Clear();
           //}
           //else
           //    keyArray = UTF8Encoding.UTF8.GetBytes(key);

           //TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
           ////set the secret key for the tripleDES algorithm
           //tdes.Key = keyArray;
           ////mode of operation. there are other 4 modes.
           ////We choose ECB(Electronic code Book)
           //tdes.Mode = CipherMode.ECB;
           ////padding mode(if any extra byte added)

           //tdes.Padding = PaddingMode.PKCS7;

           //ICryptoTransform cTransform = tdes.CreateEncryptor();
           ////transform the specified region of bytes array to resultArray
           //byte[] resultArray =
           //  cTransform.TransformFinalBlock(toEncryptArray, 0,
           //  toEncryptArray.Length);
           ////Release resources held by TripleDes Encryptor
           //tdes.Clear();
           ////Return the encrypted data into unreadable string format
           //return Convert.ToBase64String(resultArray, 0, resultArray.Length);

           string EncryptionKey = "abc123";
           byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
           using (Aes encryptor = Aes.Create())
           {
               Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
               encryptor.Key = pdb.GetBytes(32);
               encryptor.IV = pdb.GetBytes(16);
               using (MemoryStream ms = new MemoryStream())
               {
                   using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                   {
                       cs.Write(clearBytes, 0, clearBytes.Length);
                       cs.Close();
                   }
                   clearText = Convert.ToBase64String(ms.ToArray());
               }
           }
           return clearText;
       }

    }
}
