//================================================================================
//  FileName: BaseModuleControlWebPart.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/8/15
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
using iPower.Platform.WebPart;
namespace iPower.Platform.UI
{
    /// <summary>
    /// WebPart接口。
    /// </summary>
    partial class BaseModuleControl : IWebPart
    {
        #region IWebPart 成员
        /// <summary>
        /// 获取WebPart属性的值。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        /// <returns></returns>
        public virtual string QueryPropertyValue(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                WebPartPropertyCollection collection = this.WebPartProperties;
                if (collection != null && collection.Count > 0)
                {
                    WebPartProperty wpp = collection[propertyName];
                    if (wpp != null)
                        return wpp.PropertyValue;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取或设置WebPart配置的唯一标识。
        /// </summary>
        public virtual string PersonalWebPartID
        {
            get
            {
                object obj = this.ViewState["PersonalWebPartID"];
                return obj == null ? GUIDEx.Null : new GUIDEx(obj);
            }
            set
            {
                this.ViewState["PersonalWebPartID"] = value;
            }
        }
        /// <summary>
        ///  获取或设置WebPart组件数据接口。
        /// </summary>
        public virtual IWebPartData WebPartData
        {
            get
            {
                return this.ViewState["WebPartData"] as IWebPartData;
            }
            set
            {
                this.ViewState["WebPartData"] = value;
            }
        }
        /// <summary>
        /// 获取或设置WebPart属性集合。
        /// </summary>
        public virtual WebPartPropertyCollection WebPartProperties
        {
            get
            {
                object obj = this.ViewState["WebPartProperties"];
                return obj == null ? new WebPartPropertyCollection() : (obj as WebPartPropertyCollection);
            }
            set
            {
                this.ViewState["WebPartProperties"] = value;
            }
        }

        #endregion
    }
}
