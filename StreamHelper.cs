using System.IO;
using System.IO.Compression;

namespace EIP.Library
{
    public class StreamHelper
    {
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary> 
        /// 将 Stream 写入文件 
        /// </summary> 
        public void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary> 
        /// 从文件读取 Stream 
        /// </summary> 
        public Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }


        public static int ReadAllBytesFromStream(Stream stream, byte[] buffer)
        {
            // Use this method is used to read all bytes from a stream.
            int offset = 0;
            int totalCount = 0;
            while (true)
            {
                int bytesRead = stream.Read(buffer, offset, 100);
                if (bytesRead == 0)
                {
                    break;
                }
                offset += bytesRead;
                totalCount += bytesRead;
            }
            return totalCount;
        }


        /// <summary>
        /// 内存流压缩
        /// </summary>
        /// <param name="data">InputStream</param>
        /// <returns></returns>
        public byte[] CompressStream(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            // Use the newly created memory stream for the compressed data.
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(data, 0, data.Length);
            // Close the stream.
            compressedzipStream.Close();

            //----------------把流转换成byte[]数据---------------

            var b1 = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(b1, 0, b1.Length);
            ms.Close();
            return b1;
        }

        /// <summary>
        /// 内存流解压
        /// </summary>
        /// <param name="data"></param>
        /// <param name="actualSize"></param>
        /// <returns></returns>
        public byte[] DecompressStream(byte[] data, int actualSize)
        {
            //--------------把数据转换成流用来解压-----------------

            var ms1 = new MemoryStream();
            ms1.Write(data, 0, data.Length);
            ms1.Position = 0;
            //-------------------------------------------

            GZipStream zipStream = new GZipStream(ms1, CompressionMode.Decompress);
            byte[] decompressedBuffer = new byte[actualSize];
            // Use the ReadAllBytesFromStream to read the stream.
            //int totalCount = ReadAllBytesFromStream(zipStream, decompressedBuffer);
            zipStream.Read(decompressedBuffer, 0, actualSize);

            return decompressedBuffer;
        }
    }
}
