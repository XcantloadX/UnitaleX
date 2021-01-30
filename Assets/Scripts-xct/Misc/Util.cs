using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;

public class Util
{
    /// <summary> 
    /// 解压文件到指定目录
    /// </summary> 
    public static void Unzip(string unzipTo, byte[] ZipByte)
    {
        using (ZipInputStream s = new ZipInputStream(new MemoryStream(ZipByte)))
        {

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.Combine(unzipTo, Path.GetDirectoryName(theEntry.Name));
                string fileName = Path.GetFileName(theEntry.Name);

                // create directory
                if (directoryName.Length > 0)
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (fileName != String.Empty)
                {
                    Debug.Log("Unzipping: " + theEntry.Name + "|" + s.Length);
                    using (FileStream streamWriter = File.Create(Path.Combine(unzipTo, theEntry.Name)))
                    {

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
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

    /// <summary> 
    /// 解压文件到指定目录
    /// </summary> 
    public static void Unzip(string unzipTo, string zipFilePath)
    {
        Unzip(unzipTo, File.ReadAllBytes(zipFilePath));
    }

    /// <summary>
    /// 删除文件夹以及文件
    /// </summary>
    /// <param name="directoryPath"> 文件夹路径 </param>
    /// <param name="fileName"> 文件名称 </param>
    public static void DeleteDirectory(string directoryPath, string fileName)
    {

        //删除文件
        for (int i = 0; i < Directory.GetFiles(directoryPath).Length; i++)
        {
            if (Directory.GetFiles(directoryPath)[i] == fileName)
            {
                File.Delete(fileName);
            }
        }

        //删除文件夹
        for (int i = 0; i < Directory.GetDirectories(directoryPath).Length; i++)
        {
            if (Directory.GetDirectories(directoryPath)[i] == fileName)
            {
                Directory.Delete(fileName, true);
            }
        }
    }

}
