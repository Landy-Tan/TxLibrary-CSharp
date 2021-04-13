/*
 * File:        TxFile.cs
 * Author:      Landy
 * Date:        2021年4月13日
 * Description：文件操作类，扩展System.IO.File类
 * History
 * <Date>           <Author>    <Description>
 * 2021年4月13日    Landy        创建
 */
using System;
using System.IO;

namespace TxLibrary.IO
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public static class TxFile 
    {
        /// <summary>
        /// 通过路径获取文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            int index = path.LastIndexOf("\\");
            if(index > 0)
            {
                return path.Substring(index + 1,
                    path.Length - index - 1);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFilePath(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentNullException("path");
            int index = file.LastIndexOf("\\");
            if (index != -1)
            {
                return file.Substring(0, index);
            }
            return string.Empty;
        }
    }
}
