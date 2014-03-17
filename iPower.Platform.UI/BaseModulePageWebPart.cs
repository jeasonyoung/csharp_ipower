//================================================================================
//  FileName: BaseModulePageWebPart.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/6/30
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
using System.Web.UI;

using iPower.Utility;
using iPower.Platform.WebPart;
namespace iPower.Platform.UI
{
    /// <summary>
    /// WebPart部件集合委托。
    /// </summary>
    /// <returns></returns>
    public delegate WebPartQueryPropertiesCollection WebPartQueryCollectionHandler();
    /// <summary>
    /// WebPart。
    /// </summary>
    partial class BaseModulePage
    {
        /// <summary>
        /// 在指定的位置加载WebPart部件。
        /// </summary>
        /// <param name="ctrl">要加载WebPart的页面控件。</param>
        /// <param name="handler">WebPart集合委托。</param>
        protected virtual void LoadWebPart(Control ctrl, WebPartQueryCollectionHandler handler)
        {
            if (ctrl != null && handler != null)
            {
                WebPartQueryPropertiesCollection webPartQueryCollection = handler();
                if (webPartQueryCollection != null && webPartQueryCollection.Count > 0)
                {
                    IWebPart part = null;
                    foreach (WebPartQueryProperties webPartQuery in webPartQueryCollection)
                    {
                        if (!string.IsNullOrEmpty(webPartQuery.WebPartPath))
                        {
                            part = this.LoadControl(webPartQuery.WebPartPath) as IWebPart;
                            if (part != null)
                            {
                                part.PersonalWebPartID = webPartQuery.PersonalWebPartID;
                                part.WebPartData = this.GetWebPartData(webPartQuery);
                                part.WebPartProperties = webPartQuery.WebPartProperties;
                                part.DataBind();
                                ctrl.Controls.Add((Control)part);
                                part = null;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取指定WebPart数据源。
        /// </summary>
        /// <param name="webPartQuery">WebPart管理类。</param>
        /// <returns>WebPart数据源。</returns>
        protected virtual IWebPartData GetWebPartData(WebPartQuery webPartQuery)
        {
            lock (this)
            {
                IWebPartData oWebPartData = null;
                if (webPartQuery != null && !string.IsNullOrEmpty(webPartQuery.AssemblyName) && !string.IsNullOrEmpty(webPartQuery.ClassName))
                {
                    string key = string.Format("GWPD_WPD_{0}_{1}", webPartQuery.AssemblyName, webPartQuery.ClassName);
                    oWebPartData = this.cache[key] as IWebPartData;
                    if (oWebPartData == null)
                    {
                        oWebPartData = TypeHelper.Create(webPartQuery.ClassName, webPartQuery.AssemblyName) as IWebPartData;
                        if (oWebPartData != null)
                        {
                            this.cache[key] = oWebPartData;
                        }
                    }
                }
                return oWebPartData;
            }
        }
    }
}
