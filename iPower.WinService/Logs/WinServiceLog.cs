//================================================================================
//  FileName: WinServiceLog.cs
//  Desc: Windows服务的日志类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010-1-19
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
using iPower.Logs;
namespace iPower.WinService.Logs
{
    /// <summary>
    /// Windows服务的日志类。
    /// </summary>
    public class WinServiceLog : LogContainer
    {
        #region 成员变量，构造函数，析构函数
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="jobConfig"></param>
        public WinServiceLog(IJobLog jobConfig)
            : base(jobConfig)
        {
            this.storage = new WinServiceLogStorage(jobConfig);
        }
        #endregion

        #region 写入日志
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="logs">日志内容</param>
        public void ContentLog(string logs)
        {
            this.CreateNormalLog(logs);
        }
        /// <summary>
        /// 记录错误日志。
        /// </summary>
        /// <param name="e">Exception</param>
        public void ErrorLog(Exception e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}\r\n", e.Message);
            builder.AppendFormat("Source:{0}\r\n", e.Source);
            builder.AppendFormat("StackTrace:{0}\r\n", e.StackTrace);
            if (e.TargetSite != null)
            {
                builder.AppendFormat("TargetSite:{0}\r\n", e.TargetSite.ToString());
            }
            this.CreateErrorLog(builder.ToString());

            if (e.InnerException != null)
            {
                this.ErrorLog(e.InnerException);
            }
        }
        /// <summary>
        /// 记录错误日志。
        /// </summary>
        /// <param name="strLog">日志内容</param>
        public void ErrorLog(string strLog)
        {
            this.CreateErrorLog(strLog);
        }
        /// <summary>
        /// 记录运行日志。
        /// </summary>
        /// <param name="strLog">日志内容</param>
        public void RuuningLog(string strLog)
        {
            this.CreateNormalLog(strLog);
        }
        /// <summary>
        /// 记录警告日志。
        /// </summary>
        /// <param name="strLog">日志内容</param>
        public void WarringLog(string strLog)
        {
            this.CreateWarningLog(strLog);
        }
        /// <summary>
        /// 启动失败日志。
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void StartFailureLog(string serviceName)
        {
            this.RuuningLog(string.Format("服务[{0}]启动失败。", serviceName));
        }
        /// <summary>
        /// 启动成功日志。
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void StartSuccessfullLog(string serviceName)
        {
            this.RuuningLog(string.Format("服务[{0}]启动成功。", serviceName));
        }
        /// <summary>
        /// 停止失败日志。
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void StopFailureLog(string serviceName)
        {
            this.RuuningLog(string.Format("服务[{0}]停止失败。", serviceName));
        }
        /// <summary>
        /// 停止成功。
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void StopSuccessfullLog(string serviceName)
        {
            this.RuuningLog(string.Format("服务[{0}]停止成功。", serviceName));
        }
        #endregion

        #region 内置类。
        /// <summary>
        /// 日志存储。
        /// </summary>
        class WinServiceLogStorage : LogStorage
        {
            #region 成员变量，构造函数。
            /// <summary>
            /// 构造函数。
            /// </summary>
            /// <param name="head"></param>
            public WinServiceLogStorage(ILogFileHead head)
                : base(head)
            {
            }
            #endregion

            #region 重载。
            /// <summary>
            /// 
            /// </summary>
            /// <param name="logContent"></param>
            /// <returns></returns>
            protected override string CreateLogBody(string logContent)
            {
                if (!string.IsNullOrEmpty(logContent))
                {
                    return string.Format("[{0}]{1}", this.LogRecordDateTime, logContent);
                }
                return null;
            }
            #endregion
        }
        #endregion
    }
}