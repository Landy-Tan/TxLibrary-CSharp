/**********************************************************************
 * File name:		Single.cs
 * Author:			Landy
 * Create date:		2020年8月25日
 * Description:		单例模板类，通过该类创建单例对象
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年8月25日    Landy         创建，实现GetInstance和Release方法
 * 2020年8月27日    Landy         GetInstance方法增加权限管控限定
 * 2020年9月2日     Landy         创建对象通过程序集创建，防止不同程序集的客户端创建多个对象
 **********************************************************************/


using System;
using System.Collections.Generic;
using System.Reflection;

namespace TxLibrary.DesignMode
{
    static class SingleInstances
    {
        public struct str_instance_info
        {
            public string ClassName { get; set; }
            public string AssemblyPath { get; set; }
        }
        public static Dictionary<str_instance_info, object> _instances = new Dictionary<str_instance_info, object>();
    }

    /// <summary>
    /// 单例模板类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Single<T> where T : new()
    {
        /// <summary>
        /// 获取对象实例
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            var assemblyName = typeof(T).Assembly.ManifestModule.Name;
            var className = typeof(T).FullName;
            return GetInstance(className, assemblyName);
        }

        /// <summary>
        /// 获取对象实例
        /// </summary>
        /// <param name="className">类名（包括名称空间）</param>
        /// <param name="assemblyPath">程序集名称</param>
        /// <returns></returns>
        public static T GetInstance(string className, string assemblyPath)
        {
            try {
                var info = new SingleInstances.str_instance_info() { ClassName = className, AssemblyPath = assemblyPath };
                if (SingleInstances._instances.ContainsKey(info)) return (T)SingleInstances._instances[info];

                Assembly ass = Assembly.LoadFrom(assemblyPath);
                Type t = ass.GetType(className);
                if (t == null) return default(T);

                var obj = (T)ass.CreateInstance(className);
                if (obj != null)
                    SingleInstances._instances.Add(info, obj);
                return obj;
            }catch(Exception e) { System.Windows.MessageBox.Show(e.ToString());return default(T); }
        }

        /// <summary>
        /// 释放对象实例
        /// </summary>
        public static void Release()
        {
            var info = new SingleInstances.str_instance_info() { ClassName = typeof(T).FullName, AssemblyPath = typeof(T).Assembly.ManifestModule.Name };
            if (SingleInstances._instances.ContainsKey(info))
                SingleInstances._instances.Remove(info);
        }
    }
}
