/*
 * File:        TxZip.cs
 * Author:      Landy
 * Date:        2021年4月13日
 * Description：文件（夹）压缩类
 * History
 * <Date>           <Author>    <Description>
 * 2021年4月13日    Landy        创建
 */
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxLibrary.Threads;

namespace TxLibrary.IO
{
    /// <summary>
    /// 文件压缩类
    /// </summary>
    public static class TxZip
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="path"></param>
        /// <param name="zipPath"></param>
        /// <returns></returns>
        public static bool Zip(string path, string zipPath)
        {
            bool isPathIsFile = false;
            if (File.Exists(path))
                isPathIsFile = true;
            else if (Directory.Exists(path))
                isPathIsFile = false;
            else
                throw new Exception($"未知的文件:{path}");

            string[] files = null;
            if (isPathIsFile)
                files = new string[] { path };
            else
                files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            if (files is null)
                throw new ArgumentNullException(nameof(files));

            object objLock = new object();
            using (Stream stream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
            {
                ZipOutputStream output = new ZipOutputStream(stream);

                TxTaskThreads threads = new TxTaskThreads();
                threads.Init(files.Length > 8 ? 8 : files.Length);

                foreach (string file in files)
                {
                    threads.AppendTask(new TxCommandTask((o) => {
                        string f = o as string;
                        string a = string.Empty;
                        if (isPathIsFile)
                            a = TxFile.GetFileName(f);
                        else
                            f.Replace(path + "\\", "");
                        ZipEntry entry = new ZipEntry(a);
                        byte[] content = File.ReadAllBytes(f);
                        lock(objLock)
                        {
                            output.PutNextEntry(entry);
                            output.Write(content, 0, content.Length);
                        }
                    }, file));
                }

                threads.WaitAllTasks(0);
                output.Finish();
                output.Close();
                threads.Uninit();
            }

            return true;
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipPath"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool UnZip(string zipPath, string path)
        {
            if (!File.Exists(zipPath)) throw new FileNotFoundException(zipPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            using (Stream stream = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
            {
                ZipInputStream input = new ZipInputStream(stream);

                TxTaskThreads threads = new TxTaskThreads();
                threads.Init(4);

                ZipEntry entry = null;
                do
                {
                    entry = input.GetNextEntry();
                    if (entry is null) break;
                    byte[] content = new byte[entry.Size];
                    input.Read(content, 0, content.Length);
                    threads.AppendTask(new TxCommandTask((o) =>
                    {
                        var param = o as object[];
                        string fileName = param[0] as string;
                        byte[] data = param[1] as byte[];

                        string filePath = TxFile.GetFilePath(fileName);
                        if (!string.IsNullOrWhiteSpace(filePath)
                        && !Directory.Exists(Path.Combine(path, filePath)))
                            Directory.CreateDirectory(Path.Combine(path, filePath));

                        string fullPath = Path.Combine(path, fileName);
                        var fi = File.Create(fullPath);
                        fi.Write(data, 0, data.Length);
                        fi.Close();
                    }, new object[] { entry.Name, content }));
                } while (entry != null);

                threads.WaitAllTasks(0);
                input.Close();
            }
            return true;
        }
    }
}
