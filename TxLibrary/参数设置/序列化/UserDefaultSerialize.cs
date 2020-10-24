/**********************************************************************
 * File name:		UserDefaultSerialize.cs
 * Author:			landy
 * Create date:		2020年10月24日
 * Description:		自定义序列化，格式为：参数名称：值
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年10月24日   landy       实现Save/Load方法
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TxLibrary.Config.Serialize
{
    /// <summary>
    /// 自定义序列化方法
    /// 序列化格式为：参数名称：值
    /// 注意：无法序列化数组类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserDefaultSerialize<T> : ISerialize<T> where T:new()
    {
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public T Load(string file)
        {
            if (!File.Exists(file)) throw new Exception("File not exist.");
            T value = new T();
            using (var stream = new StreamReader(file))
            {
                while (!stream.EndOfStream)
                {
                    string content = stream.ReadLine();
                    // 如果读出的数据是空的、无效的，则重新读取
                    if (string.IsNullOrWhiteSpace(content)) continue;
                    // 删除行首的空格
                    content = content.TrimStart(' ');

                    // 判断是否为注释行
                    if (content[0] != '#')
                    {
                        // Name:string
                        string[] pair = content.Split(":".ToArray(), StringSplitOptions.RemoveEmptyEntries) ?? throw new ArgumentNullException(nameof(pair));
                        if (pair.Length == 0) continue;
                        string parametName = pair[0];
                        string parametValue = null;
                        if (pair.Length == 2)
                        {
                            parametValue = pair[1];
                        } else if (pair.Length > 2)
                        {
                            // Name:string:string...
                            int index = content.IndexOf(":");
                            parametValue = content.Substring(index + 1);
                        }

                        if(parametValue != null)
                        {
                            PropertyInfo property = typeof(T).GetProperty(parametName) ?? throw new ArgumentNullException(nameof(property));
                            Type type = property.PropertyType;
                            if (type.Name == typeof(string).Name)
                            {
                                property.SetValue(value, parametValue);
                            }
                            else
                            {
                                MethodInfo meth = type.GetMethod("Parse",
                                    new Type[] { typeof(string) },
                                    new ParameterModifier[] { new ParameterModifier(1) }) ?? 
                                    throw new Exception($"参数：{parametName},Type：{type.Name},无法执行的转换");
                                object obj = meth?.Invoke(null, new object[] { parametValue });
                                property.SetValue(value, obj);
                            }
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="file"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Save(string file, T value)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            using (var stream = new StreamWriter(file, false))
            {
                foreach (PropertyInfo property in properties)
                {
                    stream.WriteLine($"{property.Name}:{property.GetValue(value)}");
                }
            }
            return true;
        }
    }
}
