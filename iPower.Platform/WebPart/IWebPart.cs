//================================================================================
//  FileName: IWebPart.cs
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

namespace iPower.Platform.WebPart
{
    /// <summary>
    /// WebPart组件实现的数据和属性接口。
    /// </summary>
    public interface IWebPart
    {
        /// <summary>
        /// 获取WebPart属性的值。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        /// <returns></returns>
        string QueryPropertyValue(string propertyName);
        /// <summary>
        /// 获取或设置WebPart配置的唯一标识。
        /// </summary>
        string PersonalWebPartID { get; set; }
        /// <summary>
        /// 获取或设置WebPart组件数据接口。
        /// </summary>
        IWebPartData WebPartData { get; set; }
        /// <summary>
        /// 获取或设置WebPart属性集合。
        /// </summary>
        WebPartPropertyCollection WebPartProperties { get; set; }
        /// <summary>
        /// 数据绑定到其所有子控件。
        /// </summary>
        void DataBind();
    }
}
