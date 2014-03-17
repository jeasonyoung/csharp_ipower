//================================================================================
//  FileName: ModuleConfiguration.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/22
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

using iPower;
using iPower.Configuration;
using iPower.FileStorage;
namespace iPower.FileStorage
{
    /// <summary>
    /// 模块配置键类。
    /// </summary>
    internal class StorageModuleConfigurationKeys : iPowerConfigurationKeys
    {
        /// <summary>
        /// 文件存储的实现工厂程序集键名。
        /// </summary>
        public const string FileStorageFactoryAssemblyKey = "iPower.FileStorageFactoryAssembly";
    }
    /// <summary>
    /// 模块配置类。
    /// </summary>
    /// <example>
    /// &lt;configuration&gt;
    ///     &lt;!--注册配置节--&gt;
    ///     &lt;configSections&gt;
    ///          &lt;!--注册附件存储配置--&gt;
    ///          &lt;section name ="FileStorage" type="iPower.Configuration.iPowerSection,iPower"/&gt;
    ///     &lt;/configSections&gt;
    /// &lt;/configuration&gt;
    /// 
    /// &lt;!--附件存储配置--&gt;
    /// &lt;FileStorage&gt;
    ///     &lt;!--文件存储的实现工厂程序集--&gt;
    ///     &lt;add key="iPower.FileStorageFactoryAssembly" value="iPower.FileStorage.SQLServer.FileStorageFactory,iPower.FileStorage"/&gt;
    ///     &lt;!--文件存储地址（数据库或本地文件夹）--&gt;
    ///     &lt;add key="iPower.DbConnectionString" value="DefaultDbServer"/&gt;
    /// &lt;/FileStorage&gt;
    /// </example>
    public class StorageModuleConfiguration : iPowerConfiguration, IStorageConfig
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public StorageModuleConfiguration()
            : base("FileStorage")
        {
        }
        #endregion

        #region IStorageConfig 成员
        /// <summary>
        /// 获取文件存储源。
        /// </summary>
        public string StorageSource
        {
            get { return this.DefaultDataConnectionString.ConnectionString; }
        }
        #endregion
        /// <summary>
        /// 获取文件存储的实现工厂程序集。
        /// </summary>
        internal string FileStorageFactoryAssembly
        {
            get { return this[StorageModuleConfigurationKeys.FileStorageFactoryAssemblyKey]; }
        }       
    }
}