//================================================================================
//  FileName: LogContainer.cs
//  Desc:日志记录容器。
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
using System.IO;
using System.Text;
namespace iPower.Logs
{
    /// <summary>
    /// 记录日志到存储委托。
    /// </summary>
    /// <param name="logType">日志类型。</param>
    /// <param name="logContent">日志内容。</param>
    public delegate void LogRecordHandler(EnumLogsType logType, string logContent);
    /// <summary>
    /// 日志记录容器。
    /// </summary>
    public class LogContainer:IDisposable
    {
        #region 成员变量，构造函数，析构函数。
        /// <summary>
        /// 日志存储。
        /// </summary>
        protected ILogStorage storage;
        /// <summary>
        ///  构造函数。
        /// </summary>
        public LogContainer()
            : this(LogsConfig.ModuleConfig)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LogContainer(ILogFileHead head)
        {
            this.storage = new LogStorage(head);
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 日志记录事件。
        /// </summary>
        public event LogRecordHandler LogRecordEvent;
        /// <summary>
        /// 触发日志记录事件。.
        /// </summary>
        /// <param name="logType">日志类型。</param>
        /// <param name="logContent">日志内容。</param>
        protected virtual void OnLogRecordEvent(EnumLogsType logType, string logContent)
        {
            LogRecordHandler handler = this.LogRecordEvent;
            if (handler != null)
            {
                handler(logType, logContent);
            }
        }
        #endregion

        #region 日志记录。
        /// <summary>
        /// 创建普通日志。
        /// </summary>
        /// <param name="content">普通日志内容。</param>
        public void CreateNormalLog(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                this.RecordLogs(EnumLogsType.Normal, content);
            }
        }
        /// <summary>
        /// 创建警告日志。
        /// </summary>
        /// <param name="content">警告日志内容。</param>
        public void CreateWarningLog(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                this.RecordLogs(EnumLogsType.Warning, content);
            }
        }
        /// <summary>
        /// 创建错误日志。
        /// </summary>
        /// <param name="content">错误日志内容。</param>
        public void CreateErrorLog(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                this.RecordLogs(EnumLogsType.Error, content);
            }
        }
        /// <summary>
        /// 存储日志对象实例。
        /// </summary>
        /// <param name="logType">日志类型。</param>
        /// <param name="logContent">日志内容。</param>
        protected virtual void RecordLogs(EnumLogsType logType, string logContent)
        {
            try
            {
                this.OnLogRecordEvent(logType, logContent);
                this.storage.StorageLog(logType, logContent);
            }
            catch (Exception e)
            {
                string path = string.Format("{0}\\ERROR_{1:yyyyMMdd-HHmm}.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now);
                using (StreamWriter sw = new StreamWriter(path, true, UTF8Encoding.UTF8))
                {
                    sw.WriteLine();
                    sw.WriteLine(string.Format("记录日志时发生灾难性错误[{0:yyyy-MM-dd HH:mm:ss}]：", DateTime.Now));
                    this.WriteException(sw, e);
                }
            }
        }
        #endregion

        #region 辅助函数。
        private void WriteException(StreamWriter sw, Exception e)
        {
            if (sw != null && e != null)
            {
                sw.WriteLine("Message:" + e.Message);
                sw.WriteLine("Source:" + e.Source);
                sw.WriteLine("StackTrace:" + e.StackTrace);

                if (e.InnerException != null)
                {
                    sw.WriteLine("InnerException:");
                    this.WriteException(sw, e.InnerException);
                }
            }
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 销毁容器。
        /// </summary>
        public void Dispose()
        {
           
        }
        #endregion
    }
}