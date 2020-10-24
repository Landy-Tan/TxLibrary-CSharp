/**********************************************************************
 * File name:		BinarySerializeMemory.cs
 * Author:			landy
 * Create date:		2020年10月22日
 * Description:		二进制序列化存储方式
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年10月22日   landy       实现Save/Load方法
 * 2020年10月23日   landy       重命名为BinarySerialize，原命名为BinarySerializeMemory
 **********************************************************************/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TxLibrary.Config.Serialize
{
    /// <summary>
    /// 二进制序列化存储
    /// 注意：此序列化方法需要被序列化类显示声明SerializableAttribute
    /// </summary>
    public class BinarySerialize<T> : ISerialize<T>
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
