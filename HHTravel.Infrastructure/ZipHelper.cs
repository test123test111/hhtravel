using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace HHTravel.CRM.Booking_Online.Infrastructure
{
    /// <summary>
    /// 有待进一步优化
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="args">数组(数组[0]: 要压缩的目录; 数组[1]: 压缩的文件名)</param>
        public static void ZipFolder(string[] args)
        {
            string[] filenames = Directory.GetFiles(args[0]);

            Crc32 crc = new Crc32();
            ZipOutputStream s = new ZipOutputStream(File.Create(args[1]));
            s.SetLevel(6);

            foreach (string file in filenames)
            {
                //打开压缩文件
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                //====================================================
                string[] filenamebysp = file.Replace("\\", "/").Split('/');
                int filenameslength = filenamebysp.Length - 1;
                string filename = filenamebysp[filenameslength];
                //====================================================
                ZipEntry entry = new ZipEntry(filename);

                entry.DateTime = DateTime.Now;

                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);

                entry.Crc = crc.Value;

                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);

            }

            s.Finish();
            s.Close();
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="inFile">要进行压缩的文件名</param>
        /// <param name="outFile">压缩后生成的文件名</param>
        public static void ZipFile(string inFile, string outFile)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(inFile))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + inFile + " 不存在!");
            }
            FileStream fs = File.OpenRead(inFile);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            FileStream ZipFile = File.Create(outFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(6);

            ZipStream.Write(buffer, 0, buffer.Length);
            ZipStream.Finish();
            ZipStream.Close();
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="inFile">要进行压缩的文件名</param>
        /// <param name="outFile">压缩后生成的文件名</param>
        /// <param name="compressionLevel">压缩级别</param>
        ///  <param name="blockSize">缓冲尺寸</param>
        public static void ZipFile(string inFile, string outFile, int compressionLevel, int blockSize)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(inFile))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + inFile + " 不存在!");
            }

            System.IO.FileStream StreamToZip = new System.IO.FileStream(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream ZipFile = System.IO.File.Create(outFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(compressionLevel);
            byte[] buffer = new byte[blockSize];
            System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
            ZipStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToZip.Length)
                {
                    int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            ZipStream.Finish();
            ZipStream.Close();
            StreamToZip.Close();
        }
        /// <summary>
        /// 解压缩文件(压缩文件中含有子目录)
        /// </summary>
        /// <param name="zipFile">待解压缩的文件</param>
        /// <param name="unZipPath">解压缩到指定目录</param>
        public static void UnZip(string zipFile, string unZipPath)
        {
            using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFile)))
            {
                string directoryName = Path.GetDirectoryName(unZipPath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                ZipEntry theEntry;
                while ((theEntry = zipInputStream.GetNextEntry()) != null)
                {
                    if (theEntry.IsDirectory)
                    {
                        string subDirectoryName = Path.GetDirectoryName(unZipPath + theEntry.Name);
                        if (!Directory.Exists(subDirectoryName))
                        {
                            Directory.CreateDirectory(subDirectoryName);
                        }
                    }
                    else if (theEntry.IsFile)
                    {
                        using (FileStream streamWriter = File.Create(unZipPath + theEntry.Name))
                        {
                            if (theEntry.CompressedSize == 0)
                                continue;

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = zipInputStream.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
