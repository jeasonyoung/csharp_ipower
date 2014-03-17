//================================================================================
//  FileName: ModuleConfiguration.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/27
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
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// WebDAV模块配置键类。
    /// </summary>
    internal class ModuleConfigurationKeys
    {
        /// <summary>
        /// 文档处理程序集配置键。
        /// </summary>
        public const string DocumentFactoryAssemblyKey = "iPower.DocumentFactoryAssembly";
    }
    /// <summary>
    /// WebDAV模块配置类。
    /// </summary>
    /// <example>
    /// &lt;configuration&gt;
    ///     &lt;!--注册配置节--&gt;
    ///     &lt;configSections&gt;
    ///          &lt;!--注册WebDAV配置--&gt;
    ///          &lt;section name ="WebDAV" type="iPower.Configuration.iPowerSection,iPower"/&gt;
    ///     &lt;/configSections&gt;
    /// &lt;/configuration&gt;
    /// 
    /// &lt;!--WebDAV配置--&gt;
    /// &lt;WebDAV&gt;
    ///     &lt;!--文档处理程序集--&gt;
    ///     &lt;add key="iPower.DocumentFactoryAssembly" value="iPower.Web.WebDAV.DefaultWebDAVDocumentHandler,iPower.Web"/&gt;
    /// &lt;/WebDAV&gt;
    /// </example>
    public class ModuleConfiguration : iPowerConfiguration
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ModuleConfiguration()
            : base("WebDAV")
        {
        }
        #endregion

        /// <summary>
        /// 获取文档处理程序集配置(iPower.DocumentFactoryAssembly)。
        /// </summary>
        public string DocumentFactoryAssembly
        {
            get
            {
                return this[ModuleConfigurationKeys.DocumentFactoryAssemblyKey];
            }
        }
    }
}
