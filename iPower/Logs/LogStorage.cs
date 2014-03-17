//================================================================================
//  FileName: LogStorage.cs
//  Desc:默认日志存储。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-16
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
using System.IO;
namespace iPower.Logs
{
    /// <summary>
    /// 默认日志存储。
    /// </summary>
    public class LogStorage : ILogStorage
    {
        #region 成员变量，构造函数。
        private static IDictionary<string, Logger> logLst = new Dictionary<string, Logger>();
        LogsConfig config;
        LogHead head;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="logHead">日志头配置接口。</param>
        public LogStorage(ILogFileHead logHead)
        {
            this.config = LogsConfig.ModuleConfig;
            this.head = new LogHead(this.config);
            if (logHead != null)
            {
                if (!string.IsNullOrEmpty(logHead.LogFileHead))
                {
                    this.head.LogFileHead = logHead.LogFileHead;
                }
                if (((int)logHead.LogFileRule) > ((int)EnumLogFileRule.None))
                {
                    this.head.LogFileRule = logHead.LogFileRule;
                }
            }
        }
        #endregion

        #region 内置类。
        /// <summary>
        /// 
        /// </summary>
        private class LogHead : ILogFileHead
        {
            /// <summary>
            /// 构造函数。
            /// </summary>
            /// <param name="head"></param>
            public LogHead(ILogFileHead head)
            {
                this.LogFileHead = head.LogFileHead;
                this.LogFileRule = head.LogFileRule;
            }

            #region ILogFileHead 成员
            /// <summary>
            /// 
            /// </summary>
            public string LogFileHead { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public EnumLogFileRule LogFileRule { get; set; }
            #endregion
        }
        #endregion

        #region 静态函数。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static Logger loadLogger(string filePath)
        {
            Logger logger = null;
            lock (logLst)
            {
                string key = Logger.CreateMd5Key(filePath);
                if (logLst.ContainsKey(key))
                {
                    logger = logLst[key];
                }
                else
                {
                    logger = new Logger(filePath);
                    logLst.Add(key, logger);
                }
            }
            return logger;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取日志记录时间。
        /// </summary>
        protected string LogRecordDateTime
        {
            get
            {
                string result = string.Empty;
                try
                {
                    result = DateTime.Now.ToString(this.config.DateTimeFormat);
                }
                catch (Exception)
                {
                    result = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                }
                return result;
            }
        }
        /// <summary>
        /// 日志文件时间戳。
        /// </summary>
        protected virtual string LogFileEndTime
        {
            get
            {
                string result = string.Empty;
                if (((int)this.head.LogFileRule) > ((int)EnumLogFileRule.None))
                {
                    switch (this.head.LogFileRule)
                    {
                        case EnumLogFileRule.Year:
                            {
                                result = string.Format("{0:yyyy}", DateTime.Now);
                            }
                            break;
                        case EnumLogFileRule.Month:
                            {
                                result = string.Format("{0:yyyyMM}", DateTime.Now);
                            }
                            break;
                        case EnumLogFileRule.Week:
                            {
                                DateTime dtTime = DateTime.Now;
                                result = string.Format("[{0:yyyyMMdd}-{1:yyyyMMdd}]",
                                        dtTime.AddDays(-((int)dtTime.DayOfWeek)),
                                        dtTime.AddDays(((int)DayOfWeek.Saturday - (int)dtTime.DayOfWeek)));
                            }
                            break;
                        case EnumLogFileRule.Date:
                            {
                                result = string.Format("{0:yyyyMMdd}", DateTime.Now);
                            }
                            break;
                        case EnumLogFileRule.Hour:
                            {
                                result = string.Format("{0:yyyyMMdd-HH}", DateTime.Now);
                            }
                            break;
                    }
                }
                return result;
            }
        }
        #endregion

        #region ILogStorage 成员
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="logType">日志类型。</param>
        /// <param name="logContent">日志内容。</param>
        public void StorageLog(EnumLogsType logType, string logContent)
        {
            string path = this.GetLogStoragePath(logType);
            string log = this.CreateLogBody(logContent);
            if (!string.IsNullOrEmpty(log) && !string.IsNullOrEmpty(path))
            {
                Logger logger = loadLogger(path);
                if (logger != null)
                {
                    logger.AppendLine(log);
                }
            }
        }

        #endregion

        #region 辅助函数。
        /// <summary>
        /// 日志文件的完整路径。
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        protected virtual string GetLogStoragePath(EnumLogsType logType)
        {
            string filename = string.Format("{0}_{1}_{2}.log", this.head.LogFileHead, logType, this.LogFileEndTime);
            return Path.GetFullPath(string.Format("{0}\\{1}", this.config.StoragePath, filename));
        }
        /// <summary>
        /// 格式化日志内容。
        /// </summary>
        /// <param name="logContent">日志内容。</param>
        /// <returns>格式化后的日志内容。</returns>
        protected virtual string CreateLogBody(string logContent)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(logContent))
            {
                string strInterval = new string(this.config.IntervalMark, this.config.IntervalMarkLength);
                sb.AppendFormat("{0}\r\n", strInterval);
                sb.AppendFormat("Time: {0}\r\n", this.LogRecordDateTime);
                sb.Append("Message:\r\n");
                sb.AppendFormat("{0}\r\n", logContent);
                sb.AppendFormat("{0}\r\n", strInterval);
            }
            return sb.ToString();
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 销毁对象。
        /// </summary>
        public void Dispose()
        {
        }
        #endregion
    }
}