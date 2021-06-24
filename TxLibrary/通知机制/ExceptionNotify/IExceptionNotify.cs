using System;

namespace TxLibrary.Notification
{
    /// <summary>
    /// 异常触发事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="exception"></param>
    public delegate void TriggerExceptionHandle(object sender, Exception exception);

    /// <summary>
    /// 异常通知接口
    /// </summary>
    public interface IExceptionNotify
    {
        /// <summary>
        /// 发生异常时触发此事件
        /// </summary>
        event TriggerExceptionHandle TriggerExceptionEvent;

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <returns></returns>
        Exception GetException();

        /// <summary>
        /// 清除异常
        /// </summary>
        void ClearException();

        /// <summary>
        /// 判断是否存在异常
        /// </summary>
        /// <returns></returns>
        bool IsException();
    }

}