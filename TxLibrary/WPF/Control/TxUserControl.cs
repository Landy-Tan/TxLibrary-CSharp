/*
 * File:        TxUserControl.cs
 * Author:      Landy
 * Date:        2021年4月16日
 * Description：带属性通知的用户控件类
 * History
 * <Date>           <Author>    <Description>
 * 2021年4月16日    Landy        创建
 */
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TxLibrary.WPF.Interface;

namespace TxLibrary.WPF.Control
{
    /// <summary>
    /// 带属性通知的用户控件类
    /// </summary>
    public abstract class TxUserControl : UserControl, TxIProperty
    {
        /// <summary>
        /// 通知属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知属性的值发生了更改
        /// </summary>
        /// <param name="property">属性的名称</param>
        public void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// 设置字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="notifyPropertyChanged"></param>
        /// <returns></returns>
        public bool SetField<T>(ref T target, T value, [CallerMemberName] string notifyPropertyChanged = "")
        {
            target = value;
            NotifyPropertyChanged(notifyPropertyChanged);
            return true;
        }

        /// <summary>
        /// 获取字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public T GetField<T>(ref T target)
        {
            return target;
        }

        /// <summary>
        /// 退出清理
        /// </summary>
        public virtual void Dispose()
        {
            PropertyChanged = null;
        }
    }
}
