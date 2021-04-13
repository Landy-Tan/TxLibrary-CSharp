/*
 * File:        TxDirectory.cs
 * Author:      Landy
 * Date:        2021年4月13日
 * Description：文件夹操作类，扩展System.IO.Directory类
 * History
 * <Date>           <Author>    <Description>
 * 2021年4月13日    Landy        创建
 */
using System;
using System.IO;

namespace TxLibrary.IO
{
    /// <summary>
    /// 文件夹操作类
    /// </summary>
    public static class TxDirectory
    {
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool CopyDirectory(string source, string target)
        {
            if (string.IsNullOrWhiteSpace(source)) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(target)) throw new ArgumentNullException(nameof(target));

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(source);
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            if (!Directory.Exists(target)) throw new InvalidOperationException("Create directory error.");

            string[] files = Directory.GetFiles(source, "*", SearchOption.AllDirectories) ?? throw new InvalidOperationException("Get files error.");
            foreach (string file in files)
            {
                string sourceFile = file;
                string targetFile = file.Replace(source, target);
                File.Copy(sourceFile, targetFile);
            }
            return true;
        }
    }
}
