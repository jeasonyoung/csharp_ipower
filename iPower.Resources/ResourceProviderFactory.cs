//================================================================================
//  FileName: ResourceProviderFactory.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2012-01-09 
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Globalization;
namespace iPower.Resources
{
    /// <summary>
    /// 资源提供工厂。
    /// </summary>
    internal class ResourceProviderFactory : ResourceProvider
    {
        #region 成员变量，构造函数。
        CultureInfo culture;
        ResourceFactory resouceFactory;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="rs"></param>
        public ResourceProviderFactory(IResourceStorage rs)
            : base(rs)
        {
            this.resouceFactory = new ResourceFactory(rs);
            this.culture = CultureInfo.CurrentCulture;
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ResourceCollection CreateResources()
        {
            this.resouceFactory.SetResourcesCulture(this.culture);
            return this.resouceFactory.Resources;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object GetObject(string resourceKey, CultureInfo culture)
        {
            if (culture != null)
                this.culture = culture;
            return base.GetObject(resourceKey, culture);
        }
        #endregion
    }
}
