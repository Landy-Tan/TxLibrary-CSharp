/*
 * File:        TxIProperty.cs
 * Author:      Landy
 * Date:        2021年4月14日
 * Description：属性更改通知接口
 * History
 * <Date>           <Author>    <Description>
 * 2021年4月14日    Landy        创建
 */
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TxLibrary.WPF.Interface
{
    /// <summary>
    /// 属性接口
    /// 任何拥有属性的类都建议实现该接口
    /// </summary>
    public interface TxIProperty : INotifyPropertyChanged
    {
        /// <summary>
        /// 通知属性的值发生了更改
        /// </summary>
        /// <param name="property">属性名称</param>
        void NotifyPropertyChanged([CallerMemberName]string property = "");

        /// <summary>
        /// 设置字段的值并通知属性的值发生了更改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="notifyPropertyChanged"></param>
        /// <returns></returns>
        bool SetField<T>(ref T target, T value, [CallerMemberName]string notifyPropertyChanged = "");

        /// <summary>
        /// 获取字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        T GetField<T>(ref T target);
    }
}
