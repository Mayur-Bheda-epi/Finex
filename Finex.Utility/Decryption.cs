using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Finex.Utility
{
   public class Decryption
    {
       public static string Decrypt(string cipherText, bool useHashing)
       {
           //byte[] keyArray;
           ////get the byte code of the string

           //byte[] toEncryptArray = Convert.FromBase64String(cipherString);

           //System.Configuration.AppSettingsReader settingsReader =
           //                                    new AppSettingsReader();
           ////Get your key from config file to open the lock!
           //string key = "abc@123";
           ////string key = (string)settingsReader.GetValue("SecurityKey",
           ////                                             typeof(String));

           //if (useHashing)
           //{
           //    //if hashing was used get the hash code with regards to your key
           //    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
           //    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
           //    //release any resource held by the MD5CryptoServiceProvider

           //    hashmd5.Clear();
           //}
           //else
           //{
           //    //if hashing was not implemented get the byte code of the key
           //    keyArray = UTF8Encoding.UTF8.GetBytes(key);
           //}

           //TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
           ////set the secret key for the tripleDES algorithm
           //tdes.Key = keyArray;
           ////mode of operation. there are other 4 modes. 
           ////We choose ECB(Electronic code Book)

           //tdes.Mode = CipherMode.ECB;
           ////padding mode(if any extra byte added)
           //tdes.Padding = PaddingMode.PKCS7;

           //ICryptoTransform cTransform = tdes.CreateDecryptor();
           //byte[] resultArray = cTransform.TransformFinalBlock(
           //                     toEncryptArray, 0, toEncryptArray.Length);
           ////Release resources held by TripleDes Encryptor                
           //tdes.Clear();
           ////return the Clear decrypted TEXT
           //return UTF8Encoding.UTF8.GetString(resultArray);

           string EncryptionKey = "abc123";
           cipherText = cipherText.Replace(" ", "+");
           byte[] cipherBytes = Convert.FromBase64String(cipherText);
           using (Aes encryptor = Aes.Create())
           {
               Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
               encryptor.Key = pdb.GetBytes(32);
               encryptor.IV = pdb.GetBytes(16);
               using (MemoryStream ms = new MemoryStream())
               {
                   using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                   {
                       cs.Write(cipherBytes, 0, cipherBytes.Length);
                       cs.Close();
                   }
                   cipherText = Encoding.Unicode.GetString(ms.ToArray());
               }
           }
           return cipherText;

       }
    }
}
