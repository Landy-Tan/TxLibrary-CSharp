/*
 * File:        TxTaskThreads.cs
 * Author:      Landy
 * Date:        2020年09月20日
 * Description：任务线程池
 * History
 * <Date>           <Author>    <Description>
 * 2020年09月20日   Landy        创建
 */
using System;
using System.Collections.Generic;
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
        /// 创建一个Action任务
        /// </summary>
        /// <param name="task"></param>
        public TxCommandTask(Action task)
        {
            actTask = task;
        }

        /// <summary>
        /// 任务过程
        /// </summary>
        public void Run()
        {
            actTask?.Invoke();
        }
    }

    /// <summary>
    /// 多任务线程池
    /// </summary>
    public class TxTaskThreads
    {
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
        /// 创建线程池
        /// </summary>
        public TxTaskThreads()
        {
            mtxTasks = new Mutex();
            Tasks = new Queue<TxICommandTask>();
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
        /// 线程池执行过程
        /// </summary>
        protected void Process()
        {
            while(!bAbort)
            {
                TxICommandTask task = null;
                while (Tasks.Count <= 0) Thread.Sleep(20);
                mtxTasks.WaitOne();
                try { task = Tasks.Dequeue(); } catch { }
                mtxTasks.ReleaseMutex();
                task?.Run();
                Thread.Sleep(10);
            }
        }
    }
}
