using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCP104Library
{
    public class CommonClass
    {
        /// <summary>
        /// 十进制转成十六进制
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] ConvertUser10to16(byte[] buffer)
        {
            if (buffer != null)
            {
                for (int i = 0; i < buffer.Length - 1; i++)
                {
                    if (buffer[i] > 0)
                    {
                        int item = int.Parse(string.Format("{0}", buffer[i]));


                        buffer[i] = byte.Parse(Convert.ToString(item, 16));
                    }
                }
                return buffer;
            }
            return null;
        }
        /// <summary>
        /// 十六进制转成十进制
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] ConvertUser16to10(byte[] buffer)
        {
            if (buffer != null)
            {
                for (int i = 0; i < buffer.Length - 1; i++)
                {
                    if (buffer[i] > 0)
                    {
                        int item = int.Parse(string.Format("{0}", buffer[i]));


                        buffer[i] = byte.Parse(Convert.ToString(item, 10));
                    }
                }
                return buffer;
            }
            return null;
        }
        /// <summary>
        /// 十六进制转成十进制
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ConvertUser16to10(byte b)
        {
            int item = int.Parse(string.Format("{0}", b));

           return Convert.ToString(item, 10);
        }
        /// <summary>
        /// 十进制转成十六进制
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ConvertUser10to16(byte b)
        {
            int item = int.Parse(string.Format("{0}", b));

            string res = Convert.ToString(item, 16);
            if (res.Length == 1)
            {
                res = "0" + res;
            }
            return res;
        }

        /// <summary>
        /// 将多位值转成单值
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index">字节序号</param>
        /// <param name="length">字节长度</param>
        /// <returns></returns>
        public static string ConvertUser10to16(byte[] buffer, int index, int length)
        {
            int total = 0;
            for (int i = length; i > 0; i--)
            {
                int item = int.Parse(string.Format("{0}", buffer[index + i - 1]));
                total = total + item;
            }
            string res = Convert.ToString(total, 16);
            if (res.Length == 1)
            {
                res = "0" + res;
            }
            return res;
        }
        /// <summary>
        /// 将多位值转成单值
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index">字节序号</param>
        /// <param name="length">字节长度</param>
        /// <returns></returns>
        public static string ConvertUser10to16_Append(byte[] buffer, int index, int length)
        {
            StringBuilder str = new StringBuilder("");
            for (int i = length; i > 0; i--)
            {
                int item = int.Parse(string.Format("{0}", buffer[index + i - 1]));
                if (item > 0)
                {
                    string res = Convert.ToString(item, 16);
                    if (res.Length == 1)
                    {
                        res = "0" + res;
                    }
                    str.Append(res);
                }
            }
            if (str.Length == 0)
            {
                str.Append("00");
            }

            return str.ToString();
        }

        /// <summary>
        /// 根据可变结构定词返回序列号是否连续和数据的数量
        /// </summary>
        /// <param name="byt">可变结构定词</param>
        /// <param name="datacount">数据的数量，如 15个遥信数据</param>
        /// <returns></returns>
        public static bool getlianxuFlagandDataCount(byte byt,out int datacount)
        {
            datacount = 0;
            int item = int.Parse(string.Format("{0}", byt));
            string res = Convert.ToString(item, 2);
            string basestr = "00000000";
            string str = string.Empty;
            if (res.Length < 8)
            {
                str = basestr.Substring(0, 8 - res.Length) + res;
            }
            else
            {
                str = res;
            }
            bool lianxuFlag= str.Substring(0, 1) == "1";
            if (lianxuFlag)
            {
                datacount = int.Parse(string.Format("{0}", byt)) - 128;//-- 减80转成10进制是减128
                //if (datacount == 0)
                //{
                //    datacount = 1;
                //}
            }
            else
            {
                datacount = int.Parse(string.Format("{0}", byt));
            }
            return lianxuFlag;
        }
    }
}
