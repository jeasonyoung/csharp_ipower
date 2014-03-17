//================================================================================
//  FileName: LogsConfig.cs
//  Desc:日志全局配置。
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
using iPower.Configuration;
namespace iPower.Logs
{
    /// <summary>
    /// 日志全局配置节键。
    /// </summary>
    internal sealed class LogsConfigKeys
    {
        /// <summary>
        /// 文件存储目录键(iPower.Logs.StoragePath)。
        /// </summary>
        public const string iPower_Logs_StoragePathKey = "iPower.Logs.StoragePath";
        /// <summary>
        /// 分隔符键(iPower.Logs.IntervalMark)。
        /// </summary>
        public const string iPower_Logs_IntervalMarkKey = "iPower.Logs.IntervalMark";
        /// <summary>
        /// 分隔符长度键(iPower.Logs.IntervalMarkLength)。
        /// </summary>
        public const string iPower_Logs_IntervalMarkLengthKey = "iPower.Logs.IntervalMarkLength";
        /// <summary>
        /// 时间格式键(iPower.Logs.DateTimeFormat)。
        /// </summary>
        public const string iPower_Logs_DateTimeFormatKey = "iPower.Logs.DateTimeFormat";
        /// <summary>
        /// 日志文件头键(iPower.Logs.FileHead)。
        /// </summary>
        public const string iPower_Logs_FileHeadKey = "iPower.Logs.FileHead";
        /// <summary>
        /// 日志文件生成规则(Year-年，Month-月，Week-周，Date-日， Hour-时)(iPower.Logs.LogFileRule)。
        /// </summary>
        public const string iPower_Logs_LogFileRuleKey = "iPower.Logs.LogFileRule";
    }
    /// <summary>
    /// 系统日志配置。
    /// </summary>
    public class LogsConfig :iPowerConfiguration, ILogFileHead
    {
        #region 构造函数，析构函数
        private string logFileHead;
        private EnumLogFileRule rule = EnumLogFileRule.None;
        /// <summary>
        /// 构造函数
        /// </summary>
        public LogsConfig()
            : base("Logs")
        {
        }
        #endregion

        #region 静态属性定义
        static LogsConfig m_config;
        /// <summary>
        /// 静态属性。
        /// </summary>
        public static LogsConfig ModuleConfig
        {
            get
            {
                lock (typeof(LogsConfig))
                {
                    if (m_config == null)
                    {
                        m_config = new LogsConfig();
                    }
                    return m_config;
                }
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取日志文件存储目录(iPower.Logs.StoragePath)。
        /// </summary>
        public string StoragePath
        {
            get
            {
                string path = this[LogsConfigKeys.iPower_Logs_StoragePathKey];
                if (string.IsNullOrEmpty(path))
                {
                    path = System.AppDomain.CurrentDomain.BaseDirectory;
                }
                return path;
            }
        }
        /// <summary>
        /// 获取间隔字符(iPower.Logs.IntervalMark)。
        /// </summary>
        public char IntervalMark
        {
            get
            {
                char space = '-';
                string str = this[LogsConfigKeys.iPower_Logs_IntervalMarkKey];
                if (!string.IsNullOrEmpty(str))
                {
                    space = str[0];
                }
                return space;
            }
        }
        /// <summary>
        /// 获取分隔符长度(iPower.Logs.IntervalMarkLength)。
        /// </summary>
        public int IntervalMarkLength
        {
            get
            {
                int length = 60;
                try
                {
                    length = int.Parse(this[LogsConfigKeys.iPower_Logs_IntervalMarkLengthKey]);
                }
                catch (Exception) { }
                return length;
            }
        }
        /// <summary>
        /// 获取时间格式(iPower.Logs.DateTimeFormat)。
        /// </summary>
        public string DateTimeFormat
        {
            get
            {
                string formatter = this[LogsConfigKeys.iPower_Logs_DateTimeFormatKey];
                if (string.IsNullOrEmpty(formatter))
                {
                    formatter = "yyyy-MM-dd HH:mm:ss";
                }
                return formatter;
            }
        }
        #endregion

        #region ILogFileHead 成员
        /// <summary>
        /// 获取日志文件头(iPower.Logs.FileHead)。
        /// </summary>
        public string LogFileHead
        {
            get
            {
                if (string.IsNullOrEmpty(this.logFileHead))
                {
                    this.logFileHead = this[LogsConfigKeys.iPower_Logs_FileHeadKey];
                }
                return this.logFileHead;
            }
        }
        /// <summary>
        /// 获取日志文件生成规则(Year-年，Month-月，Week-周，Date-日， Hour-时)
        /// </summary>
        public EnumLogFileRule LogFileRule
        {
            get
            {
                if (this.rule == EnumLogFileRule.None)
                {
                    try
                    {
                        string strRule = this[LogsConfigKeys.iPower_Logs_LogFileRuleKey];
                        this.rule = (EnumLogFileRule)Enum.Parse(typeof(EnumLogFileRule), strRule, true);
                    }
                    catch (Exception)
                    {
                        return EnumLogFileRule.Year;
                    }
                }
                return this.rule;
            }
        }

        #endregion
    }
}