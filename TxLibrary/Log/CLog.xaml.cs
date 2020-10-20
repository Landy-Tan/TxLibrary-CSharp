/**********************************************************************
 * File name:		Log.cs
 * Author:			txl
 * Create date:		
 * Description:		日志记录
 * Warning:         程序退出时请手动调用CLog.Release()方法
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年8月26日    txl         修复Add方法：添加一条记录时同步向窗口更新
 * 2020年8月31日    txl         修复窗口显示时不显示以往的记录；修改item的文本前景色为黑色
 * 2020年9月1日     txl         优化性能，将写文件过程放入到一个线程中执行
 * 2020年9月7日     txl         迁移到WPF（未测试）
 * 2020年9月9日     txl         WPF测试完成
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TxLibrary.Log
{
    /// <summary>
    /// CLog.xaml 的交互逻辑
    /// </summary>
    public partial class CLog : UserControl
    {
        /// <summary>
        /// 日志信息
        /// </summary>
        public class CInfo
        {
            /// <summary>
            /// 日志类型
            /// </summary>
            public enum emType
            {
                /// <summary>
                /// 信息
                /// </summary>
                Info,

                /// <summary>
                /// 警告
                /// </summary>
                Warning,

                /// <summary>
                /// 错误
                /// </summary>
                Error
            }
            /// <summary>
            /// 时间
            /// </summary>
            public DateTime time { get; set; }

            /// <summary>
            /// 日志类型
            /// </summary>
            public emType type { get; set; }

            /// <summary>
            /// 线程ID
            /// </summary>
            public int threadID { get; set; }

            /// <summary>
            /// 文件
            /// </summary>
            public string file { get; set; }

            /// <summary>
            /// 行号
            /// </summary>
            public int line { get; set; }

            /// <summary>
            /// 函数
            /// </summary>
            public string function { get; set; }

            /// <summary>
            /// 错误ID
            /// </summary>
            public int errID { get; set; }

            /// <summary>
            /// 描述信息
            /// </summary>
            public string description { get; set; }

            /// <summary>
            /// 构造一条空的记录
            /// </summary>
            public CInfo() { }

            /// <summary>
            /// 构造一条带数据的记录
            /// </summary>
            /// <param name="type"></param>
            /// <param name="errID"></param>
            /// <param name="description"></param>
            public CInfo(emType type, int errID, string description)
            {
                this.time = DateTime.Now;
                this.type = type;
                this.threadID = Thread.CurrentThread.ManagedThreadId;
                this.file = "";
                this.line = 0;
                this.function = "";
                this.errID = errID;
                this.description = description;
            }

            /// <summary>
            /// 转换成字符串
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string str = $"[{time.Hour}:{time.Minute}:{time.Second}:{time.Millisecond}] [{type.ToString()}] [{threadID}] [{file}:{line}] [{function}] {description}";
                return str;
            }

            /// <summary>
            /// 获取记录时间
            /// </summary>
            public string Time { get { return $"{time.Hour}:{time.Minute}:{time.Second}:{time.Millisecond}"; } }
        }

        /// <summary>
        /// 实例对象
        /// </summary>
        private static CLog _instance = null;

        /// <summary>
        /// 获取/插件实例对象
        /// </summary>
        public static CLog Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CLog();
                return _instance;
            }
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public static void Release()
        {
            try { _instance.thread?.Abort(); } catch { }
            _instance = null;
        }

        /// <summary>
        /// 视图
        /// </summary>
        private GridView View = new GridView();

        /// <summary>
        /// 日志记录集合
        /// </summary>
        private static List<CInfo> Logs = new List<CInfo>();

        /// <summary>
        /// 控制写Log线程的执行
        /// </summary>
        private bool IsClose = false;

        /// <summary>
        /// 线程
        /// 将日志记录写入文件
        /// </summary>
        protected Thread thread{ get; set; }

        /// <summary>
        /// 记录集合锁
        /// </summary>
        private object lockWirteLogs = new object();

        /// <summary>
        /// 记录集合
        /// </summary>
        private Queue<CInfo> wirteLogs = new Queue<CInfo>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public CLog()
        {
            InitializeComponent();
            this.Loaded += CLog_Loaded;
            thread = new Thread(() => { WriteThread(); });
            thread.Start();
        }

        private void CLog_Loaded(object sender, RoutedEventArgs e)
        {
            View.Columns.Clear();
            View.Columns.Add(new GridViewColumn() { Header = "时间", Width = 80, DisplayMemberBinding = new Binding("Time") });
            View.Columns.Add(new GridViewColumn() { Header = "类型", Width = 80, DisplayMemberBinding = new Binding("type") });
            View.Columns.Add(new GridViewColumn() { Header = "错误码", Width = 80, DisplayMemberBinding = new Binding("errID") });
            View.Columns.Add(new GridViewColumn() { Header = "描述", Width = 220, DisplayMemberBinding = new Binding("description") });
            listView.View = View;
        }

        /// <summary>
        /// 写文件线程方法
        /// </summary>
        private void WriteThread()
        {
            if (!Directory.Exists("Log")) Directory.CreateDirectory("Log");
            CInfo info = null;
            while (true)
            {
                while (wirteLogs.Count > 0)
                {
                    lock (lockWirteLogs)
                    {
                        info = wirteLogs.Dequeue();
                    }

                    if (info != null)
                    {
                        string path = System.IO.Path.Combine("Log", DateTime.Now.ToString("yyyy-MM-dd")) + ".log";
                        using (StreamWriter writer = new StreamWriter(path, true))
                        {
                            writer.WriteLine(info.ToString());
                        }


                        if (Logs.Count > 0xffff) Logs.RemoveRange(0, 0xff);
                        Logs.Add(info);

                        if (IsClose) return;
                        this.Dispatcher.Invoke(() =>
                        {
                            ListViewItem item = new ListViewItem();
                            item.Content = info;
                            if(info.type == CInfo.emType.Error)
                                item.Background = Brushes.Red;
                            listView.Items.Add(item);
                        });

#if DEBUG
                        //Console.WriteLine(info.ToString());
#endif
                        info = null;
                    }
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 添加一条记录
        /// 内部使用，不对外公开
        /// </summary>
        /// <param name="info"></param>
        protected static void Add(CInfo info)
        {
            try
            {
                Logs.Add(info);
                
                lock (CLog.Instance.lockWirteLogs)
                {
                    CLog.Instance.wirteLogs.Enqueue(info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 添加一条记录信息
        /// </summary>
        /// <param name="description"></param>
        public static void Info(string description)
        {
            StackFrame frame = new StackTrace(true).GetFrame(1);
            Add(new CInfo()
            {
                time = DateTime.Now,
                type = CInfo.emType.Info,
                threadID = Thread.CurrentThread.ManagedThreadId,
                errID = 0,
                file = frame.GetFileName(),
                line = frame.GetFileLineNumber(),
                function = frame.GetMethod().ToString(),
                description = description
            });
        }

        /// <summary>
        /// 添加一条警告信息
        /// </summary>
        /// <param name="errID"></param>
        /// <param name="description"></param>
        public static void Warning(int errID, string description)
        {
            StackFrame frame = new StackTrace(true).GetFrame(1);
            Add(new CInfo()
            {
                time = DateTime.Now,
                type = CInfo.emType.Warning,
                threadID = Thread.CurrentThread.ManagedThreadId,
                errID = errID,
                file = frame.GetFileName(),
                line = frame.GetFileLineNumber(),
                function = frame.GetMethod().ToString(),
                description = description
            });
        }

        /// <summary>
        /// 添加一条错误信息
        /// </summary>
        /// <param name="errID"></param>
        /// <param name="description"></param>
        public static void Error(int errID, string description)
        {
            StackFrame frame = new StackTrace(true).GetFrame(1);
            Add(new CInfo()
            {
                time = DateTime.Now,
                type = CInfo.emType.Error,
                threadID = Thread.CurrentThread.ManagedThreadId,
                errID = errID,
                file = frame.GetFileName(),
                line = frame.GetFileLineNumber(),
                function = frame.GetMethod().ToString(),
                description = description
            });
        }

    }
}
