//================================================================================
//  FileName: Logger.cs
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
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace iPower.Logs
{
    /// <summary>
    /// 写日志文件（支持多线程并发）。
    /// </summary>
    public class Logger : IDisposable
    {
        #region 成员变量，构造函数。
        private static IDictionary<string, FileAppender> logLst = new Dictionary<string, FileAppender>();
        private const string NEWLINE = "\r\n";
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="filePath"></param>
        public Logger(string filePath)
            : this(filePath, Encoding.UTF8)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encode"></param>
        public Logger(string filePath, Encoding encode)
        {
            this.FilePath = filePath;
            this.Encode = encode;
        }
        #endregion

        #region 定时释放对象。
        private const int CONST_IPERIOD = 60 * 1000;//计时器执行时间间隔。
        //定时器，用来定时释放不再使用的文件对象。
        private static readonly Timer timer = new Timer(new TimerCallback(delegate(object state) {
            KeyValuePair<string, FileAppender>[] kvps = new KeyValuePair<string,FileAppender>[logLst.Count];
            logLst.CopyTo(kvps, 0);
            for (int i = 0; i < kvps.Length; i++)
            {
                if (kvps[i].Value != null && kvps[i].Value.Count == 0)
                {
                    kvps[i].Value.Dispose();
                }
            }
        }), null, CONST_IPERIOD, CONST_IPERIOD);
        #endregion

        #region 属性。
        /// <summary>
        /// 设置文件路径。
        /// </summary>
        public string FilePath { private get; set; }
        /// <summary>
        /// 设置字符集。
        /// </summary>
        public Encoding Encode { private get; set; }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 根据文件路径创建Md5键。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateMd5Key(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
               return iPower.Cryptography.HashCrypto.Hash(path.ToLower(), "md5");
            }
            return null;
        }

        private void WriteText(string content, bool append)
        {
            string pathKey = CreateMd5Key(this.FilePath);
            FileAppender logger = null;
            lock (logLst)
            {
                if (logLst.ContainsKey(pathKey))
                {
                    logger = logLst[pathKey];
                }
                else
                {
                    logger = new FileAppender(this.FilePath, (this.Encode != null) ? this.Encode : Encoding.UTF8);
                    logLst.Add(pathKey, logger);
                }
            }
            logger.CallAddpender(content, append);
        }
        #endregion

        /// <summary>
        /// 追加数据。
        /// </summary>
        /// <param name="content">内容。</param>
        public void Append(string content)
        {
            this.WriteText(content, true);
        }
        /// <summary>
        /// 追加数据（结尾换行）。
        /// </summary>
        /// <param name="content">内容。</param>
        public void AppendLine(string content)
        {
            this.WriteText(content + NEWLINE, true);
        }
        /// <summary>
        /// 写入新文件数据。
        /// </summary>
        /// <param name="content">内容。</param>
        public void Write(string content)
        {
            this.WriteText(content, false);
        }
        /// <summary>
        /// 写入新文件数据（结尾换行）。
        /// </summary>
        /// <param name="content"></param>
        public void WriteLine(string content)
        {
            this.WriteText(content + NEWLINE, false);
        }

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
           
        }
        #endregion
    }
}