/**********************************************************************
 * File name:		XmlMemory.cs
 * Author:			landy
 * Create date:		2020年10月23日
 * Description:		XML格式序列化存储方式
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年10月23日   landy       实现Save/Load方法
 * 2020年10月23日   landy       重命名为XmlSerialize，原命名为XmlMemory
 **********************************************************************/

using System;
using System.IO;

namespace TxLibrary.Config.Serialize
{
    /// <summary>
    /// 以XML格式存储
    /// 注意：要存储的对象访问权限必须为public
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlSerialize<T> : ISerialize<T>
    {
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public T Load(string file)
        {
            try
            {
                if (!File.Exists(file)) throw new Exception("File not exist.");
                using (var stream = new StreamReader(file))
                {
                    return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                return default;
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="file"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Save(string file, T value)
        {
            try
            {
                using (Stream stream = File.Open(file, FileMode.Create))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(T)).Serialize(stream, value);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
