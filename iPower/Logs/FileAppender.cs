//================================================================================
//  FileName: FileAppender.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-25
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using iPower.Utility;
namespace iPower.Logs
{
    /// <summary>
    /// 提供写文件操作。
    /// </summary>
    internal class FileAppender:IDisposable
    {
        #region 成员变量，构造函数。
        private static IDictionary<string, Queue<Pair<string,bool>>> queueLst = new Dictionary<string, Queue<Pair<string,bool>>>();
        private static IDictionary<string, Pair<string, Encoding>> pathLst = new Dictionary<string, Pair<string, Encoding>>();
        private string pathKey;
        private Encoding encode = Encoding.UTF8;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径。</param>
        public FileAppender(string path)
        {
            this.pathKey = Logger.CreateMd5Key(path);
            lock (pathLst)
            {
                if (!pathLst.ContainsKey(this.pathKey))
                {
                    pathLst.Add(this.pathKey, new Pair<string, Encoding>(path, this.encode));
                }
                else
                {
                    pathLst[this.pathKey] = new Pair<string, Encoding>(path, this.encode);
                }
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encode"></param>
        public FileAppender(string path, Encoding encode)
            : this(path)
        {
            this.encode = encode;
        }
        #endregion

        /// <summary>
        /// 获取队列集合数。
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                if (queueLst.Count > 0)
                {
                    KeyValuePair<string, Queue<Pair<string, bool>>>[] kvps = new KeyValuePair<string, Queue<Pair<string, bool>>>[queueLst.Count];
                    queueLst.CopyTo(kvps, 0);
                    for (int i = 0; i < kvps.Length; i++)
                    {
                        if (kvps[i].Value != null)
                        {
                            count += kvps[i].Value.Count;
                        }
                    }
                }
                return count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="append"></param>
        public void CallAddpender(string content, bool append)
        {
            lock (queueLst)
            {
                Pair<string, bool> pair = new Pair<string, bool>(content, append);
                try
                {
                    if (queueLst.ContainsKey(this.pathKey) && queueLst[this.pathKey] != null)
                    {
                        queueLst[this.pathKey].Enqueue(pair);
                    }
                    else
                    {
                        Queue<Pair<string, bool>> q = new Queue<Pair<string, bool>>();
                        q.Enqueue(pair);
                        queueLst.Add(this.pathKey, q);
                    }
                }
                catch (Exception)
                {
                    this.CallAddpender(content, append);
                }
            }
        }

        #region 定时写入文件。
        private const int CONST_IPERIOD = 500;//计时器执行时间间隔。
        ////定时器，用来定时写入文件。
        private static readonly Timer timer = new Timer(new TimerCallback(delegate(object state)
        {
            KeyValuePair<string, Queue<Pair<string, bool>>>[] kvps = new KeyValuePair<string, Queue<Pair<string, bool>>>[queueLst.Count];
            queueLst.CopyTo(kvps, 0);
            int timeInterval = 10;
            for (int i = 0; i < kvps.Length; i++)
            {
                KeyValuePair<string, Queue<Pair<string, bool>>> kvp = kvps[i];
                if (string.IsNullOrEmpty(kvp.Key))
                {
                    continue;
                }
                if (pathLst.ContainsKey(kvp.Key))
                {
                    Pair<string, Encoding> fp = pathLst[kvp.Key];
                    DateTime start = DateTime.Now;
                    object obj_lock = new object();
                    Queue<Pair<string, bool>> q = kvp.Value;
                    while ((q != null) && (q.Count > 0))
                    {
                        try
                        {
                            Pair<string, bool> p = q.Peek();//获取队列头。
                            if (p != null)
                            {
                                lock (obj_lock)
                                {
                                    using (FileStream fs = new FileStream(fp.First, (p.Second ? FileMode.Append : FileMode.Create), (p.Second ? FileAccess.Write : FileAccess.ReadWrite)))
                                    {
                                        using (StreamWriter sw = new StreamWriter(fs, fp.Second))
                                        {
                                            sw.Write(p.First);
                                            sw.Flush();
                                        }
                                    }
                                    //将队头移出队列。
                                    q.Dequeue();
                                    //线程暂停时间。
                                    Thread.Sleep(10);
                                }
                            }
                            //时间阀值到达时跳出循环。
                            if ((DateTime.Now - start).TotalSeconds >= timeInterval)
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            //发生异常时跳出队列循环。
                            break;
                        }
                    }
                }
            }
        }), null, CONST_IPERIOD, CONST_IPERIOD); 
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (queueLst.ContainsKey(this.pathKey))
            {
                lock (queueLst)
                {
                    if (queueLst[this.pathKey].Count == 0)
                    {
                        queueLst.Remove(this.pathKey);
                    }
                }
            }
           
        }
        #endregion
    }
}