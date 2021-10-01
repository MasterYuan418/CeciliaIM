using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CeciliaIM.Server.Backend.Utils
{
    class EncryptionHelper
    {
        public static string SHA256(string text)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(text);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] bte = Sha256.ComputeHash(SHA256Data);
            return BitConverter.ToString(bte).Replace("-", "").ToLower();
        }
        public static string MD5(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            HMACMD5 MD5 = new HMACMD5();
            byte[] d = MD5.ComputeHash(data);
            return BitConverter.ToString(d).Replace("-", "").ToLower();
        }
        public static string AES(string Publickey,string Iv,string text)
        {
            AESHelper.Iv = Iv;
            if (Publickey.ToCharArray().Length != 32) throw new Exception();
            AESHelper.PublicKey = Publickey;
            LogUtil.WriteVerbose("AES module set successful.");
            return AESHelper.Encrypt(text);
        }
    }
    internal class AESHelper
    {
        /// <summary>
        /// 默认密钥-密钥的长度必须是32
        /// </summary>
        public static string PublicKey { set; get; }

        /// <summary>
        /// 默认向量
        /// </summary>
        public static string Iv { get; set; }
        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="str">需要加密字符串</param>  
        /// <returns>加密后字符串</returns>  
        public static String Encrypt(string str)
        {
            return Encrypt(str, PublicKey);
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="str">需要解密字符串</param>  
        /// <returns>解密后字符串</returns>  
        public static String Decrypt(string str)
        {
            return Decrypt(str, PublicKey);
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = System.Text.Encoding.UTF8.GetBytes(str);
            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return System.Text.Encoding.UTF8.GetString(resultArray);
        }
    }
}
