/**********************************************************************
 * File name:		JsonMemory.cs
 * Author:			landy
 * Create date:		2020年10月22日
 * Description:		Json格式序列化存储方式
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年10月22日   landy       实现Save/Load方法
 **********************************************************************/

using System;
using System.IO;
using Newtonsoft.Json;

namespace TxLibrary.Config.Memory
{
    /// <summary>
    /// 以Json格式存储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonMemory<T> : IMemory<T> where T : new()
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
                string content = File.ReadAllText(file) ?? throw new ArgumentNullException(nameof(content));
                return JsonConvert.DeserializeObject<T>(content);
            }catch(Exception e)
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
                string content = JsonConvert.SerializeObject(value, Formatting.Indented) ?? throw new ArgumentNullException(nameof(content));
                File.WriteAllText(file, content);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
