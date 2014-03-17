//================================================================================
//  FileName: WinServiceJobsConfig.cs
//  Desc:Window服务的所有的Job的运行状态配置。
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using iPower.Configuration;
namespace iPower.WinService.Config
{
    /// <summary>
    /// Window服务的所有的Job的运行状态配置。
    /// </summary>
    public sealed class ServiceJobsConfig : iPowerConfiguration
    {
        #region 成员变量，构造函数，析构函数.
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ServiceJobsConfig()
            : base("WinServiceJobs")
        {
        }
        #endregion

        #region 静态属性定义
        static ServiceJobsConfig m_config;
        /// <summary>
        /// 静态属性。
        /// </summary>
        public static ServiceJobsConfig ModuleConfig
        {
            get
            {
                lock (typeof(ServiceJobsConfig))
                {
                    if (m_config == null)
                        m_config = new ServiceJobsConfig();
                    return m_config;
                }
            }
        }
        #endregion

        #region 节点集合
        /// <summary>
        ///获取所有工作集合。
        /// </summary>
        public List<String> AllJobs
        {
            get
            {
                lock (this)
                {
                    List<String> jobs = new List<string>();
                    foreach (KeyValueConfigurationElement elem in this.Section.Settings)
                    {
                        string jobName = elem.Key;
                        if (!jobs.Contains(jobName) && string.Equals(elem.Value, ModuleConst.CONST_STR_JOBSTARTFLAG))
                        {
                            jobs.Add(jobName);
                        }
                    }
                    return jobs;
                }
            }
        }
        #endregion
    }
}