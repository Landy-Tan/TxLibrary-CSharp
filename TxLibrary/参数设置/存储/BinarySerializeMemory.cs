/**********************************************************************
 * File name:		BinarySerializeMemory.cs
 * Author:			landy
 * Create date:		2020年10月22日
 * Description:		二进制序列化存储方式
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年10月22日   landy       实现Save/Load方法
 **********************************************************************/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TxLibrary.Config.Memory
{
    /// <summary>
    /// 二进制序列化存储
    /// </summary>
    public class BinarySerializeMemory<T> : IMemory<T>
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
                T value = default;

                BinaryFormatter formatter = new BinaryFormatter();
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    value = (T)formatter.Deserialize(stream);
                }

                return value;
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
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Save(string file, T value)
        {
            try
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    formatter.Serialize(stream, value);
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
