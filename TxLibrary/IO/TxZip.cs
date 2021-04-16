/*
 * File:        TxZip.cs
 * Author:      Landy
 * Date:        2021年4月13日
 * Description：文件（夹）压缩类
 * History
 * <Date>           <Author>    <Description>
 * 2021年4月13日    Landy        创建
 * 2021年4月16日    Landy        适配WinRAR-ZIP
 */
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

                List<Task> tasks = new List<Task>();

                foreach (string _file in files)
                {
                    var task = new Task((o) => {
                        string file = o as string;
                        string name = string.Empty;
                        if (isPathIsFile)
                            name = TxFile.GetFileName(file);
                        else
                            name = file.Replace(path + "\\", "").Replace("\\", "/");

                        byte[] data = null;
                        ZipEntry entry = new ZipEntry(name);
                        var info = new FileInfo(file);
                        entry.DateTime = info.LastWriteTime;
                        data = File.ReadAllBytes(file);
                        
                        lock(objLock)
                        {
                            output.PutNextEntry(entry);
                            if (data != null)
                                output.Write(data, 0, data.Length);
                        }
                    }, _file);
                    task.Start();
                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());
                output.Finish();
                output.Close();
            }

            return true;
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipPath">压缩包路径</param>
        /// <param name="path">解压缩路径</param>
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
                List<Task> tasks = new List<Task>();
                ZipEntry entry = null;
                do
                {
                    entry = input.GetNextEntry();
                    if (entry is null) break;
                    if (entry.IsDirectory)
                    {// 文件夹
                        string tempPath = Path.Combine(path, entry.Name.Replace("/", "\\"));
                        var info = new DirectoryInfo(tempPath);
                        if (!info.Exists)
                            info.Create();
                        info.LastWriteTime = entry.DateTime;
                    }
                    else if (entry.IsFile)
                    {// 文件
                        byte[] content = new byte[entry.Size];
                        input.Read(content, 0, content.Length);

                        var task = new Task((o) =>
                        {
                            var _ = (Tuple<ZipEntry, byte[]>) o;
                            //var param = (entry: _.Item1, content: _.Item2);
                            var _entry = _.Item1;
                            var _content = _.Item2;
                            string name = _entry.Name.Replace("/", "\\");
                            // 如果路径不存在，创建路径
                            string filePath = TxFile.GetFilePath(name);
                            if (!string.IsNullOrWhiteSpace(filePath)
                            && !Directory.Exists(Path.Combine(path, filePath)))
                                Directory.CreateDirectory(Path.Combine(path, filePath));

                            // 写文件
                            string fullPath = Path.Combine(path, name);
                            var fi = File.Create(fullPath);
                            fi.Write(_content, 0, _content.Length);
                            fi.Close();
                            FileInfo info = new FileInfo(fullPath);
                            info.LastWriteTime = _entry.DateTime;

                        }, Tuple.Create(entry, content));
                        task.Start();
                        tasks.Add(task);
                        
                    }
                } while (entry != null);
                
                input.Close();
                Task.WaitAll(tasks.ToArray());
            }
            return true;
        }
    }
}
