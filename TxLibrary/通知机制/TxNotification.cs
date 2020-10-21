using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxLibrary.Notification
{
    /// <summary>
    /// 通知
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="args"></param>
    public delegate void NotificationHandler(TxNotification.Notification notification, object[] args);

    /// <summary>
    /// 通知机制
    /// </summary>
    public class TxNotification
    {
        /// <summary>
        /// 接收通知的事件
        /// </summary>
        public event NotificationHandler NotificationEvent;

        private static TxNotification _instance = null;

        /// <summary>
        /// 单例设计模式，返回自身
        /// </summary>
        public static TxNotification Instance
        {
            get
            {
                if (_instance == null) _instance = new TxNotification();
                return _instance;
            }
        }

        /// <summary>
        /// 同步发送通知
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="args"></param>
        public void SendNotification(Notification notification, object[] args) => 
            NotificationEvent?.Invoke(notification, args);

        /// <summary>
        /// 异步发送通知
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="args"></param>
        public void SendAsyncNotification(Notification notification, object[] args) => 
            Task.Run(() => NotificationEvent?.Invoke(notification, args));

        /// <summary>
        /// 通知类型
        /// </summary>
        public enum Notification
        {
            /// <summary>
            /// 初始化
            /// </summary>
            Initialize,

            /// <summary>
            /// 打开
            /// </summary>
            Open,

            /// <summary>
            /// 关闭
            /// </summary>
            Close,

            // ...
        }
    }
}
