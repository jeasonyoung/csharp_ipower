//================================================================================
//  FileName:WebResourceProviderFactory.cs.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-09 16:32:32
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
using System.Collections.Generic;
using System.Text;
using System.Web.Compilation;
using System.Globalization;
using System.Resources;

namespace iPower.Resources
{
    /// <summary>
    /// 资源提供类。
    /// <example>
    ///  <code>
    ///     &lt;system.web&gt;
    ///         &lt;!--资源文件配置--&gt;
    ///         &lt;globalization culture="auto:zh-CN" uiCulture="auto:zh-CN" requestEncoding="UTF-8" responseEncoding="UTF-8" resourceProviderFactoryType="iPower.Resources.WebResourceProviderFactory,iPower.Resources"/&gt;
    ///     &lt;/system.web&gt;
    ///  </code>
    /// </example>
    /// </summary>
    public class WebResourceProviderFactory : System.Web.Compilation.ResourceProviderFactory
    {
        #region 成员变量，构造函数。
        ResourceProvider provider;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WebResourceProviderFactory()
        {
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 
        /// </summary>
        void EnsureResourceProvider()
        {
            if (this.provider == null)
                this.provider = new ResourceProviderFactory(ResourceConfiguration.ModuleConfig);
        }
        #endregion

         #region 重载。
         /// <summary>
        /// 创建一个全局资源提供程序。 
        /// </summary>
        /// <param name="classKey">资源类的名称。</param>
        /// <returns></returns>
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            this.EnsureResourceProvider();
            this.provider.ClassKey = classKey;
            this.provider.VirtualPath = string.Empty;
            return this.provider;
        }
        /// <summary>
        /// 创建一个本地资源提供程序。 
        /// </summary>
        /// <param name="virtualPath">资源文件的路径。</param>
        /// <returns></returns>
        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            this.EnsureResourceProvider();
            this.provider.ClassKey = string.Empty;
            this.provider.VirtualPath = virtualPath;
            return this.provider;
        }
        #endregion
    }
}
