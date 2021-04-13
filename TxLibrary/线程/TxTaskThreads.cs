/*
 * File:        TxTaskThreads.cs
 * Author:      Landy
 * Date:        2020年09月20日
 * Description：任务线程池
 * History
 * <Date>           <Author>    <Description>
 * 2020年09月20日   Landy        创建
 * 2021年4月13日    Landy        新增线程状态、等待方法
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TxLibrary.Threads
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface TxICommandTask
    {
        /// <summary>
        /// 任务处理过程
        /// </summary>
        void Run();
    }

    /// <summary>
    /// 通用任务
    /// </summary>
    public class TxCommandTask : TxICommandTask
    {
        /// <summary>
        /// 任务
        /// </summary>
        protected Action actTask { get; set; }

        /// <summary>
        /// 带参数的任务
        /// </summary>
        protected Action<object> actTask1 { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        protected object parameter { get; set; }

        /// <summary>
        /// 创建一个Action任务
        /// </summary>
        /// <param name="task"></param>
        public TxCommandTask(Action task)
        {
            actTask = task;
        }

        /// <summary>
        /// 创建一个Action任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="param"></param>
        public TxCommandTask(Action<object> task, object param)
        {
            actTask1 = task;
            parameter = param;
        }

        /// <summary>
        /// 任务过程
        /// </summary>
        public void Run()
        {
            if (actTask != null)
                actTask();
            else if (actTask1 != null)
                actTask1(parameter);
        }
    }

    /// <summary>
    /// 多任务线程池
    /// </summary>
    public class TxTaskThreads : IDisposable
    {
        /// <summary>
        /// 线程状态
        /// </summary>
        protected enum EThreadStatus
        {
            /// <summary>
            /// 初始化
            /// </summary>
            Initialize,

            /// <summary>
            /// 运行中
            /// </summary>
            Runing,

            /// <summary>
            /// 等待任务
            /// </summary>
            WaitTask
        }

        /// <summary>
        /// 线程状态更改锁
        /// </summary>
        object objThreadStatusChangedLock = null;

        /// <summary>
        /// 存储线程状态信息
        /// </summary>
        protected Dictionary<Thread, EThreadStatus> dictThreadStatus = null;

        /// <summary>
        /// 线程数量
        /// </summary>
        protected int nThreadNumber = 0;

        /// <summary>
        /// 线程
        /// </summary>
        protected Thread[] Threads = null;

        /// <summary>
        /// 停止线程
        /// </summary>
        protected bool bAbort = false;

        /// <summary>
        /// 任务锁
        /// </summary>
        protected Mutex mtxTasks = null;

        /// <summary>
        /// 任务集合
        /// </summary>
        protected Queue<TxICommandTask> Tasks = null;

        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount => nThreadNumber;

        /// <summary>
        /// 任务数量
        /// </summary>
        public int TaskCount => Tasks.Count;


        /// <summary>
        /// 创建线程池
        /// </summary>
        public TxTaskThreads()
        {
            objThreadStatusChangedLock = new object();
            mtxTasks = new Mutex();
            Tasks = new Queue<TxICommandTask>();
            dictThreadStatus = new Dictionary<Thread, EThreadStatus>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="threadNumber">线程数量</param>
        /// <returns></returns>
        public bool Init(int threadNumber)
        {
            nThreadNumber = threadNumber;
            Uninit();
            bAbort = false;
            Threads = new Thread[nThreadNumber];
            for(int i = 0; i < nThreadNumber; i++ )
            {
                Threads[i] = new Thread(Process);
                Threads[i].Start();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Uninit()
        {
            bAbort = true;
            if (Threads != null)
            {
                foreach (var th in Threads)
                {
                    try { th?.Abort(); } catch { }
                }
                Threads = null;
            }
            dictThreadStatus.Clear();
            return true;
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool AppendTask(TxICommandTask task)
        {
            try
            {
                mtxTasks.WaitOne();
                Tasks.Enqueue(task);
                mtxTasks.ReleaseMutex();
                return true;
            }
            catch
            {
                mtxTasks.ReleaseMutex();
                return false;
            }
        }

        /// <summary>
        /// 等待所有任务执行完毕
        /// </summary>
        /// <param name="timeout">超时时间, 0：一直等待</param>
        /// <returns>超时返回false；成功返回true</returns>
        public bool WaitAllTasks(int timeout)
        {
            //Thread.Sleep(20);// 等待10ms,确保所有线程都有执行
            Stopwatch watch = Stopwatch.StartNew();
            bool runing = true;
            while (runing || TaskCount > 0)
            {
                try
                {
                    runing = false;
                    foreach (var status in dictThreadStatus.Values)
                    {
                        if (status == EThreadStatus.Runing)
                        {
                            runing = true;
                            break;
                        }
                    }

                    if (runing)
                    {
                        if (timeout != 0
                            && watch.ElapsedMilliseconds > timeout)
                            return false;
                        Thread.Sleep(100);
                    }
                }
                catch
                {
                    runing = true;
                }
            }
            return true;
        }


        /// <summary>
        /// 线程池执行过程
        /// </summary>
        protected void Process()
        {
            TxICommandTask task = null;
            while (!bAbort)
            {
                task = null;
                while (Tasks.Count <= 0)
                {
                    SetThreadStatus(Thread.CurrentThread, EThreadStatus.WaitTask);
                    Thread.Sleep(20);
                }
                SetThreadStatus(Thread.CurrentThread, EThreadStatus.Runing);

                mtxTasks.WaitOne();
                try { task = Tasks.Dequeue(); } catch { }
                mtxTasks.ReleaseMutex();
                task?.Run();
            }
        }

        /// <summary>
        /// 设置线程状态
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="status"></param>
        protected void SetThreadStatus(Thread thread, EThreadStatus status)
        {
            if (dictThreadStatus.ContainsKey(thread)
                && dictThreadStatus[thread] == status) return;
            lock(objThreadStatusChangedLock)
            {
                dictThreadStatus[thread] = status;
            }
        }

        public void Dispose()
        {
            Uninit();
        }
    }
}
