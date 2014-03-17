//================================================================================
//  FileName: SerializeQueueUtil.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2012-12-27
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using System.Xml.Serialization;
namespace iPower.FileStorage
{
    /// <summary>
    /// 序列化处理队列
    /// </summary>
    internal sealed class SerializeQueueUtil<T> : IDisposable
        where T:class
    {
        #region 成员变量，构造函数。
        Queue<T> queue = null;
        Timer timer = null;
        bool isRuning = false;
        string output = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        private SerializeQueueUtil(string output)
        {
            this.output = output;
            this.queue = new Queue<T>();
            this.timer = new Timer();
            this.timer.Interval = 5000;
            this.timer.Enabled = false;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }
        #endregion

        #region 静态对象实例。
        static object synchronizationObject = new object();
        static SerializeQueueUtil<T> instance;
        /// <summary>
        /// 获取对象实例。
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static SerializeQueueUtil<T> Instance(string output)
        {
            lock (synchronizationObject)
            {
                if (instance == null)
                {
                    instance = new SerializeQueueUtil<T>(output);
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (item != null)
            {
                this.queue.Enqueue(item);
                if (!this.timer.Enabled && !this.isRuning)
                {
                    this.timer.Start();
                }
            }
        }

        #region 辅助函数。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                this.isRuning = true;
                this.timer.Stop();
                if (this.queue.Count > 0)
                {
                    T o = this.queue.Dequeue();
                    if (o != null)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(this.output))
                            {
                                T t = o;
                                using (FileStream fs = new FileStream(this.output, FileMode.Create, FileAccess.Write))
                                {
                                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                                    serializer.Serialize(fs, t);
                                }
                            }
                        }
                        catch (Exception x)
                        {
                            OnExceptionRecord(x);
                        }
                        finally
                        {
                            if (this.queue.Count > 0)
                            {
                                this.timer.Start();
                            }
                            this.isRuning = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 异常处理记录。
        /// </summary>
        /// <param name="e"></param>
        public static void OnExceptionRecord(Exception e)
        {
            if (e == null)
            {
                return;
            }
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(delegate(object sender)
            {
                Type type = typeof(SerializeQueueUtil<T>);
                lock (type)
                {
                    string path = Path.GetFullPath(string.Format("{0}\\{1}_{2:yyyyMMdd}.log", AppDomain.CurrentDomain.BaseDirectory, type.FullName, DateTime.Now));
                    using (StreamWriter sw = new StreamWriter(path, true, UTF8Encoding.UTF8))
                    {
                        sw.WriteLine(new String('-', 50));
                        string msg = e.Message, source = e.Source, trace = e.StackTrace;
                        Exception x = e.InnerException;
                        if (x != null)
                        {
                            msg += "  " + x.Message;
                            source += "  " + x.Source;
                            trace += "  " + x.StackTrace;
                        }
                        sw.WriteLine(string.Format("Message:{0}", msg));
                        sw.WriteLine(string.Format("Source:{0}", source));
                        sw.WriteLine(string.Format("StackTrace:{0}", trace));
                        sw.WriteLine(string.Format("DateTime:{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                        sw.WriteLine(new String('-', 50));
                    }
                }
            }));
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Elapsed -= new ElapsedEventHandler(this.timer_Elapsed);
                this.timer.Dispose();
            }
        }

        #endregion
    }
}