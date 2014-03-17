//================================================================================
//  FileName: WinServiceConfig.cs
//  Desc:Windows服务配置。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010/1/17
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
using iPower.Logs;
namespace iPower.WinService.Config
{
    /// <summary>
    /// Window Service配置键。
    /// </summary>
    internal sealed class ServiceConfigKey
    {
        /// <summary>
        /// 服务名称键。
        /// </summary>
        public const string ServiceNameKey = "iPower.ServiceName";
        /// <summary>
        /// 服务显示名称键。
        /// </summary>
        public const string DisplayNameKey = "iPower.DisplayName";
        /// <summary>
        /// 服务描述键。
        /// </summary>
        public const string DescriptionKey = "iPower.Description";
        /// <summary>
        /// 运行服务应用程序时将使用的用户帐户。
        /// </summary>
        public const string UserNameKey = "iPower.UserName";
        /// <summary>
        /// 运行服务应用程序时所使用用户帐户关联的密码。
        /// </summary>
        public const string PasswordKey = "iPower.Password";
        /// <summary>
        /// 日志文件名键。
        /// </summary>
        public const string LogFileHeadKey = "iPower.LogFileHead";
        /// <summary>
        /// 日志文件生成规则键。
        /// </summary>
        public const string LogFileRuleKey = "iPower.LogFileRule";
    }
    /// <summary>
    /// Window Service配置。
    /// </summary>
    public sealed class ServiceConfig : iPowerConfiguration, IJobLog
    {  
        #region 构造函数，析购函数
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ServiceConfig()
            : base("WinService")
        {
        }
        #endregion

        #region 静态属性定义
        static ServiceConfig m_config;
        /// <summary>
        /// 静态函数。
        /// </summary>
        public static ServiceConfig ModuleConfig
        {
            get
            {
                lock (typeof(ServiceConfig))
                {
                    if (m_config == null)
                    {
                        m_config = new ServiceConfig();
                    }
                    return m_config;
                }
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 服务名称(iPower.ServiceName)。
        /// </summary>
        public string ServiceName
        {
            get
            {
                return this[ServiceConfigKey.ServiceNameKey];
            }
        }
     
        /// <summary>
        /// 显示名称(iPower.DisplayName)。
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this[ServiceConfigKey.DisplayNameKey];
            }
        }
        /// <summary>
        /// 服务描述(iPower.Description)。
        /// </summary>
        public string Description
        {
            get
            {
                return this[ServiceConfigKey.DescriptionKey];
            }
        }
        /// <summary>
        /// 运行服务应用程序时将使用的用户帐户(iPower.UserName)。
        /// </summary>
        public string Username
        {
            get
            {
                return this[ServiceConfigKey.UserNameKey];
            }
        }
        /// <summary>
        /// 运行服务应用程序时所使用用户帐户关联的密码(iPower.Password)。
        /// </summary>
        public string Password
        {
            get
            {
                return this[ServiceConfigKey.PasswordKey];
            }
        }
        /// <summary>
        /// 日志文件名(iPower.LogFileHead)。
        /// </summary>
        public string LogFileHead
        {
            get
            {
                return this[ServiceConfigKey.LogFileHeadKey];
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
                    string ruleStr = this[ServiceConfigKey.LogFileRuleKey];
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
    }
}