//================================================================================
//  FileName:ResourceConfiguration.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-09 16:16:36
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
// ----  ------  -----------
//
//================================================================================
//  Copyright (C) 2009-2010 Jeason Young Corporation
//================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using iPower.Utility;
using iPower.Configuration;
namespace iPower.Resources
{
    /// <summary>
    /// 资源配置键。
    /// </summary>
    internal class ResourceConfigurationKeys
    {
        /// <summary>
        /// 资源存储地址键(iPower.Resources.Storage)。
        /// </summary>
        public const string ResourceStorageKey = "iPower.Resources.Storage";
    }
    /// <summary>
    /// 资源配置（配置节点：Resources）。
    /// </summary>
    internal class ResourceConfiguration : iPowerConfiguration, IResourceStorage
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ResourceConfiguration()
            : base("Resources")
        {
        }
        #endregion

        #region 静态属性。
        static ResourceConfiguration config;
        /// <summary>
        /// 获取静态配置属性。
        /// </summary>
        public static ResourceConfiguration ModuleConfig
        {
            get
            {
                lock (typeof(ResourceConfiguration))
                {
                    if (config == null)
                        config = new ResourceConfiguration();
                    return config;
                }
            }
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取资源存储地址。
        /// </summary>
        public string ResourceStorage
        {
            get
            {
                return this[ResourceConfigurationKeys.ResourceStorageKey];
            }
        }
        #endregion
    }
}
