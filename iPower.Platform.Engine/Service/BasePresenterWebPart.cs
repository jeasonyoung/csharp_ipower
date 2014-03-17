//================================================================================
//  FileName: BasePresenterWebPart.cs
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

using iPower.Utility;
using iPower.Platform.WebPart;
namespace iPower.Platform.Engine.Service
{
    /// <summary>
    /// WebPart处理。
    /// </summary>
    partial class BasePresenter<T, K>
    {
        /// <summary>
        /// 获取当前系统当前用户指定位置的WebPart组件集合。
        /// </summary>
        /// <param name="zoneMode"></param>
        /// <returns></returns>
        public virtual WebPartQueryPropertiesCollection GetWebPartQueryCollection(EnumWebPartAlignment zoneMode)
        {
            IWebPartMgr oWebPartMgr = this.ModuleConfig.WebPartMgrAssembly;
            if (oWebPartMgr != null && this.View != null)
            {
                WebPartQueryCollection webPartQueryCollection = oWebPartMgr.QueryList(zoneMode, this.View.CurrentSystemID, this.View.CurrentUserID);
                if (webPartQueryCollection != null && webPartQueryCollection.Count > 0)
                {
                    WebPartQueryPropertiesCollection webPartQueryPropertiesCollection = new WebPartQueryPropertiesCollection();
                    foreach (WebPartQuery query in webPartQueryCollection)
                    {
                        WebPartQueryProperties queryProperties = new WebPartQueryProperties(query);
                        if (queryProperties != null)
                        {
                            queryProperties.WebPartProperties = oWebPartMgr.QueryProperties(query.PersonalWebPartID);
                            webPartQueryPropertiesCollection.Add(queryProperties);
                        }
                    }
                    return webPartQueryPropertiesCollection;
                }
            }
            return null;
        }
    }
}
