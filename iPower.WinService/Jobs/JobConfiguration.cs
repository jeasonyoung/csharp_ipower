//================================================================================
//  FileName: WinServiceJobConfiguration.cs
//  Desc:Windows服务的Job默认配置。
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

using iPower.Configuration;
using iPower.WinService;
using iPower.WinService.Logs;
using iPower.WinService.Config;
using iPower.Logs;
namespace iPower.WinService.Jobs
{
    /// <summary>
    /// Windows服务的Job默认配置键。
    /// </summary>
    public class JobConfigurationKey
    {
        /// <summary>
        /// 模块程序集键。
        /// </summary>
        public const string ModuleAssemblyKey = "iPower.ModuleAssembly";
        /// <summary>
        /// 日志文件键。
        /// </summary>
        public const string LogFileHeadKey = "iPower.LogFileHead";
        /// <summary>
        /// 日志文件生成规则键。
        /// </summary>
        public const string LogFileRuleKey = "iPower.LogFileRule";
        /// <summary>
        /// 运行周期键。 
        /// </summary>
        public const string RunCycleKey = "iPower.RunCycle";
        /// <summary>
        /// 运行时刻键。
        /// </summary>
        public const string RunTimeKey = "iPower.RunTime";
        /// <summary>
        /// 开始时间键。
        /// </summary>
        public const string StartTimeKey = "iPower.StartTime";
        /// <summary>
        /// 结束时间键。
        /// </summary>
        public const string EndTimeKey = "iPower.EndTime";
        /// <summary>
        /// 扩展配置文件键。
        /// </summary>
        public const string ConfigFileKey = "iPower.ConfigFile";
    }
    /// <summary>
    /// Job 配置基类。
    /// </summary>
    public class JobConfiguration : iPowerConfiguration, IJobLog, IDisposable
    {
        #region 成员变量，构造函数，析构函数
        WinServiceLog servLog,jobLog;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="winServiceJob">Job配置节名。</param>
        public JobConfiguration(string winServiceJob)
            : base(winServiceJob)
        {
            this.servLog = new WinServiceLog(ServiceConfig.ModuleConfig);
            this.jobLog = new WinServiceLog(this);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取Job程序集(iPower.ModuleAssembly)。
        /// </summary>
        public virtual string ModuleAssembly
        {
            get
            {
                lock (this)
                {
                    string str = this[JobConfigurationKey.ModuleAssemblyKey];
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] array = str.Split(new char[] { ',' });
                        if (array != null && array.Length > 1)
                        {
                            return array[1];
                        }
                    }
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 获取Job主类名称。
        /// </summary>
        public string ModuleClassName
        {
            get
            {
                lock (this)
                {
                    string str = this[JobConfigurationKey.ModuleAssemblyKey];
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] array = str.Split(new char[] { ',' });
                        if (array != null && array.Length > 0)
                        {
                            return array[0];
                        }
                    }
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 获取日志文件名称(iPower.LogFileHead)。 
        /// </summary>
        public virtual string LogFileHead
        {
            get
            {
                return this[JobConfigurationKey.LogFileHeadKey];
            }
        }
        /// <summary>
        /// 获取开始时间(iPower.StartTime)。 
        /// </summary>
        public virtual long StartTimeTicks
        {
            get
            {
                DateTime now = DateTime.Now;
                string str = this[JobConfigurationKey.StartTimeKey];
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        now = Convert.ToDateTime(str);
                        if (now.Year == 0)
                        {
                            now.AddDays((double)DateTime.Now.Day);
                            now.AddMonths(DateTime.Now.Month);
                            now.AddYears(DateTime.Now.Year);
                        }
                    }
                    catch (Exception e)
                    {
                        now = DateTime.Now;
                        this.servLog.ErrorLog(e);
                    }
                }
                return now.Ticks;
            }
        }
        /// <summary>
        /// 获取结束时间(iPower.EndTime)。
        /// </summary>
        public virtual long EndTimeTicks
        {
            get
            {
                DateTime now = DateTime.MaxValue;
                string str = this[JobConfigurationKey.EndTimeKey];
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        now = Convert.ToDateTime(str);
                    }
                    catch (Exception e)
                    {
                        now = DateTime.MaxValue;
                        this.servLog.ErrorLog(e);
                    }
                }
                return now.Ticks;
            }
        }
        /// <summary>
        /// 获取运行周期(iPower.RunCycle)。
        /// </summary>
        public virtual long CycleTicks
        {
            get
            {
                DateTime now = DateTime.MinValue.AddSeconds(1.0);
                string str = this[JobConfigurationKey.RunCycleKey];
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        now = DateTime.MinValue.AddSeconds((double)Convert.ToInt32(str));
                    }
                    catch (Exception e)
                    {
                        now = DateTime.MinValue.AddSeconds(1.0);
                        this.servLog.ErrorLog(e);
                    }
                }
                return now.Ticks;
            }
        }
        /// <summary>
        /// 获取同步时刻(iPower.RunTime)[HH:mm]。
        /// </summary>
        public virtual long RunTimeTicks
        {
            get
            {
                long tick = 0L;
                string str = this[JobConfigurationKey.RunTimeKey];
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        DateTime time = Convert.ToDateTime(str);
                        if (time.Year == 0)
                        {
                            time.AddDays((double)DateTime.Now.Day);
                            time.AddMonths(DateTime.Now.Month);
                            time.AddYears(DateTime.Now.Year);
                        }
                        tick = (time - DateTime.Now).Ticks;
                    }
                    catch (Exception e)
                    {
                        this.servLog.CreateErrorLog("获取同步时刻(iPower.RunTime)发生异常！");
                        this.servLog.ErrorLog(e);
                    }
                }
                return tick;
            }
        }
        /// <summary>
        /// 获取扩展配置文件(iPower.ConfigFile)。
        /// </summary>
        public virtual string ConfigFile
        {
            get
            {
                return this[JobConfigurationKey.ConfigFileKey];
            }
        }
        /// <summary>
        /// 日志文件生成规则(iPower.LogFileRule).
        /// </summary>
        public EnumLogFileRule LogFileRule
        {
            get
            {
                EnumLogFileRule rule = EnumLogFileRule.None;
                try
                {
                    string ruleStr = this[JobConfigurationKey.LogFileRuleKey];
                    if (!string.IsNullOrEmpty(ruleStr))
                    {
                        rule = (EnumLogFileRule)Enum.Parse(typeof(EnumLogFileRule), ruleStr);
                    }
                }
                catch (Exception) { }
                return rule;
            }
        }
        #endregion

        #region 虚函数。
        /// <summary>
        /// 释放配置资源。
        /// </summary>
        public virtual void Dispose()
        {
        }
        #endregion
    }
}