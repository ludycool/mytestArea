using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WHC.OrderWater.Commons
{
    /// <summary>
    ///������ṩ��ʵ�÷������ֽ������ͼ��֮���ת����
    /// </summary>
    public sealed class ByteImageConvertor
    {
        private ByteImageConvertor()
        {
        }

        /// <summary>
        /// ��PO���ֽ�����ת��VO������
        /// </summary>
        /// <param name="bytes">��PO�ֽ����顣</param>
        /// <returns>ͼ�����</returns>
        public static Image ByteToImage(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            Image image = null;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                image = Image.FromStream(stream);
            }
            return image;
        }

        /// <summary>
        /// �����ֽ�ת����VO�������Ա��
        /// </summary>
        /// <param name="image">��VO��Image����</param>
        /// <returns>�ֽ����顣</returns>
        public static byte[] ImageToByte(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}