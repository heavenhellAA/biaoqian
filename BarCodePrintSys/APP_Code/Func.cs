using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace BarCodePrintSys
{
    public class Func
    {
         /// <summary>
        /// 站名
        /// </summary>
        public static string site_name = "艾华集团客户标签打印系统";
        /// <summary>
        /// 10进制转换成36进制
        /// </summary>
        public static string int10Convert36(int i)
        {
            string str = "";
            while (i > 35)
            {
                int j = i % 36;
                str += ((j <= 9) ? Convert.ToChar(j + '0') : Convert.ToChar(j - 10 + 'A'));
                i = i / 36;
            }
            str += ((i <= 9) ? Convert.ToChar(i + '0') : Convert.ToChar(i - 10 + 'A'));

            Char[] c = str.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }
        /// <summary>
        /// 防sql注入
        /// </summary>
        /// <param name="xstr">字符串</param>
        /// <returns></returns>
        public static string Zhuru(string xstr)
        {
            string zr = "1";
            string x = xstr + "";
            ///原字符+转化为小写的字符，为了方便对字符SQL检查，Indexof对大小写不敏感
            int i, j = 0;
            string[] ary = { "'", "and",  "exec", "insert", "select", "delete", "update", "count", "chr", "mid", "master", "truncate", "char", "declare" };
            for (i = 0; i < ary.Length; i++)
            {
                j = x.ToLower().IndexOf(ary[i]);
               // j = x.IndexOf(ary[i]);
                if (j != -1)
                    break;
            }
            if (j == -1)
            {
                zr = xstr;
            }
            else
            {
                //System.Web.HttpContext.Current.Response.Redirect("./");
                zr = "warning";
            }
            return zr;
        }
        /// <summary>
        /// 截取字符串1
        /// </summary>
        /// <param name="inputString">输入字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string Zifu(string inputString, int length)
        {
            int len = inputString.Length;
            //int js = 1;
            if (Encoding.UTF8.GetByteCount(inputString) <= length * 2)
            {
                return inputString;
            }
            string zf = "";
            //Single zs;
            if (len > length)
            {
                zf = inputString.Substring(0, length) + "..";
            }
            else
                zf = inputString;
            return zf;
        }
        /// <summary>
        /// 截取字符串2
        /// </summary>
        /// <param name="inputString">输入字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string SubString(string inputString, int length)
        {
            if (Encoding.UTF8.GetByteCount(inputString) <= length * 2)
            {
                return inputString;
            }
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                tempString += inputString.Substring(i, 1);
                if (tempLen >= (length - 1) * 2)
                    break;
            }
            //截取过长则加上半个省略号  
            if (System.Text.Encoding.Default.GetBytes(inputString).Length > length)
                tempString += "..";
            return tempString;
        }
        /// <summary>
        /// 加入Cookie值
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static void Cookie_in()
        {
            HttpCookie admin, flag;
            admin = HttpContext.Current.Request.Cookies["name"];
            flag = HttpContext.Current.Request.Cookies["role"];
            if (admin == null || flag == null)
            {
                HttpContext.Current.Response.Redirect("../Default/Login");
                HttpContext.Current.Response.End();
            }
        }
        /// <summary>
        /// 判断是否为整数字符串
        /// </summary>
        /// <param name=" message">输入字符串</param>
        /// <param name="result">返回结果</param>
        /// <returns></returns>
        public static bool IsNumeric(string message, out int result)
        {
            //判断是否为整数字符串
            //是的话则将其转换为数字并将其设为out类型的输出值、返回true, 否则为false
            result = -1;   //result 定义为out 用来输出值
            try
            {
                //当数字字符串的长度是少于4时，以下三种都可以转换，任选一种
                //如果位数超过4的话，请选用Convert.ToInt32() 和int.Parse()

                //result = int.Parse(message);
                //result = Convert.ToInt16(message);
                result = Convert.ToInt32(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 从左往右截取字符串
        /// </summary>
        /// <param name="param">输入字符串</param>
        /// <param name=length">返截取长度</param>
        /// <returns>返回结果</returns>
        public static string SubLeft(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }
        /// <summary>
        /// 从右往左截取字符串
        /// </summary>
        /// <param name="param">输入字符串</param>
        /// <param name=length">返截取长度</param>
        /// <returns>返回结果</returns>
        public static string SubRight(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }
        /// <summary>
        /// 从指定位置截取字符串
        /// </summary>
        /// <param name="param">输入字符串</param>
        /// <param name= startIndex">开始截取的位置</param>
        /// <param name=length">返截取长度</param>
        /// <returns>返回结果</returns>
        public static string SubMid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex, length);
            return result;
        }
        /// <summary>
        /// 在字符串末尾去除几个字符
        /// </summary>
        /// <param name="param">输入字符串</param>
        /// <param name= startIndex">去除字符数量</param>
        /// <returns>返回结果</returns>
        public static string LLeft(string sSource, int iLength)
        {
            return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
        }
        /// <summary>
        /// 正则判断是否为手机号码
        /// </summary>
        /// <param name="param">输入手机号码</param>
        /// <returns>返回结果</returns>
        public static bool IsTel(string tel)
        {
            Regex reg = new Regex(@"((^13[0-9]{1}[0-9]{8}|^15[0-9]{1}[0-9]{8}|^14[0-9]{1}[0-9]{8}|^16[0-9]{1}[0-9]{8}|^17[0-9]{1}[0-9]{8}|^18[0-9]{1}[0-9]{8}|^19[0-9]{1}[0-9]{8})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)");
            return reg.IsMatch(tel);
        }
        /// <summary>
        /// 正则判断是否为身份证号码
        /// </summary>
        /// <param name="param">输入身份证号码</param>
        /// <returns>返回结果</returns>
        public static bool IsIDcard(string str_idcard)
        {
            return Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        //保存日志记录文件的方法
        public static void save_emessage(string log, string logName, string path)
        {
            string tm = DateTime.Now.ToString("yyyyMMddHHmmss");
            string tm_date = DateTime.Now.ToString("yyyyMMdd");
            string p = path + logName + tm_date + ".txt";
            if (!Directory.Exists(path))//判断路径下目录是否存在
            {
                Directory.CreateDirectory(path);
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(p))
                    {
                        sw.WriteLine(log + "----------" + tm);
                    }

                }
                else
                {
                    using (StreamWriter sw = File.AppendText(p))
                    {
                        sw.WriteLine(log + "----------" + tm);
                    }
                }
            }
            else
            {
                if (!File.Exists(p))
                {
                    using (StreamWriter sw = File.CreateText(p))
                    {
                        sw.WriteLine(log + "----------" + tm);
                    }

                }
                else
                {
                    using (StreamWriter sw = File.AppendText(p))
                    {
                        sw.WriteLine(log + "----------" + tm);
                    }
                }
            }

        }
        //10进制任何进制转化通用函数方法，3个参数
        //原理：
        //以10进制数字026为例转化26进制：
        //数字长度位3：
        //第1位数字为0，由26/26^2(26的2次方),得0，余26；
        //第2位数字为2，由余数26/26^1，得1，余0；
        //第3位数字为6，由余数0/26^0，得0，余0,计算完成
        //所以026的26进制为010
        //以10进制数字025为例转化26进制：
        //数字长度位3：
        //第1位数字为0，由25/26^2(26的2次方),得0，余25；
        //第2位数字为2，由余数25/26^1，得0，余25；
        //第3位数字为5，由余数25/26^0，得25，余0,计算完成
        //所以025的26进制为00P
        public static string createNum(int num, int chars, int codes)//num为需要转换的十进制数,chars代表需要转化的进制位数,codes代表需要转化的十进制数位数
        {
            string[] source = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };//以0到z为例子的36进制转换
            string[] code = new string[10] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };//转换成10位的编码，前面补0
            //复制数组，长度为输入的chars和codes
            string[] jinzhi = new string[chars];
            Array.Copy(source, jinzhi, chars);

            string[] weishu = new string[codes];
            Array.Copy(code, weishu, codes);

            int yushu = num;//设置余数为num
            //Math.Abs(num).ToString().Length
            for (int i = 0; i < weishu.Length; i++)
            {
                int zheng = yushu / (int)Math.Pow(chars, (weishu.Length - i - 1));//求第i位的数字（用yushu整除chars的（code.length-i-1）次方）
                yushu = yushu % (int)Math.Pow(chars, (weishu.Length - i - 1));//保留上面的余数，作为下次循环的yushu
                weishu[i] = jinzhi[zheng];//第i位的数字从source里面取出
            }
            string b = string.Empty;
            for (int i = 0; i < weishu.Length; i++)
            {
                b = b + weishu[i];//将code的多位串联起来，生成一个字符串b
            }
            return b;//返回b
        }

    }
}