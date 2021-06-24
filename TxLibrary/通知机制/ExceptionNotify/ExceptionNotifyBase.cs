/*
 * File:        ExceptionNotifyBase.cs
 * Author:      Landy
 * Date:        2021年6月24日
 * Description：异常通知机制
 * History
 * <Date>           <Author>    <Description>
 * 2021年6月24日    Landy        创建
 */
using System;

namespace TxLibrary.Notification
{
    /// <summary>
    /// 异常通知基类
    /// </summary>
    public abstract class ExceptionNotifyBase : IExceptionNotify
    {
        /// <summary>
        /// 触发异常通知事件
        /// </summary>
        public event TriggerExceptionHandle TriggerExceptionEvent;

        /// <summary>
        /// 存储异常实例对象
        /// </summary>
        private Exception _exception;

        /// <summary>
        /// 获取异常
        /// </summary>
        /// <returns></returns>
        public Exception GetException()
        {
            return _exception;
        }

        /// <summary>
        /// 清除异常
        /// </summary>
        public void ClearException()
        {
            _exception = null;
        }

        /// <summary>
        /// 检查是否存在异常
        /// </summary>
        /// <returns></returns>
        public bool IsException()
        {
            return !(_exception is null);
        }

        /// <summary>
        /// 触发异常
        /// </summary>
        /// <param name="e"></param>
        protected void TriggerException(Exception e)
        {
            _exception = e;
            TriggerExceptionEvent?.Invoke(this, _exception);
        }
    }
}

