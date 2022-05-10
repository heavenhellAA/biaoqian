using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Configuration;

namespace BarCodePrintSys
{
    public class MD5Helper
    {
        /*
        webConfig配置：
        <!--Md5加密key-->
        <add key="sKey" value="JUNDAOXT"/>
        //string IPassword = MD5Help.MD5Encrypt(password, ConfigurationManager.AppSettings["sKey"].ToString()); //加密
        //string JPassword = MD5Help.MD5Decrypt(Password, ConfigurationManager.AppSettings["sKey"].ToString()); //解密
        */
        /// <summary>
        /// 32位MD5加密不解密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5to32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        private static string sKey = ConfigurationManager.AppSettings["sKey"].ToString();//密钥
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pToEncrypt">加密字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string pToEncrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();

        }

        /// <summary>
        /// MD5解密
        /// </summary>
        /// <param name="pToDecrypt">解密字符串</param>
        /// <returns></returns>
        public static string MD5Decrypt(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
    }
}