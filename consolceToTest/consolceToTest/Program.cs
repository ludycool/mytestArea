using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consolceToTest
{
    class Program
    {
        static void Main(string[] args)
        {

            while(true)
            { 
            string ss = Console.ReadLine();
            byte[] byteArray = Encoding.UTF8.GetBytes(ss);




            string dd = byteArray.ToString();


            Console.WriteLine(dd);


           
            Console.ReadKey();
        }
        }

    }

    public enum ProtocolFlag
    {
        None = 0,
        SQL = 1, //SQL查询协议
        Upload = 2, //上传协议
        Download = 3, //下载协议
        RemoteStream = 4, //远程文件流协议
        Throughput = 5, //吞吐量测试协议
        Control = 8,
        LogOutput = 9,
    }
    static class hextool
    {

        /// <summary>
        /// 字节转16进制字符
        /// </summary>
        /// <param name="byts"></param>
        /// <returns></returns>
        public static String ToHexString(this byte[] byts)
        {
            String result = "";
            try
            {
                foreach (var item in byts)
                {
                    result += item.ToString("X").PadLeft(2, '0');
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }
        /// <summary>
        /// 十六进制字串 转 Byte数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace("   ", " ");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        ///<summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }
    }
}