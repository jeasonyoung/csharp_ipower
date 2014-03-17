//================================================================================
//  FileName:IDataDropDownList.cs
//  Desc:下拉框数据接口。
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-10-22 11:19:59
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
using System.Web;
using System.Security.Permissions;
namespace iPower.Platform
{
    /// <summary>
    /// 下拉框数据接口。
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IDataDropDownList
    {
        /// <summary>
        /// 获取或设置显示字段。
        /// </summary>
        string DataTextField
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置显示格式。
        /// </summary>
        string DataTextFormatString
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置值字段。
        /// </summary>
        string DataValueField
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置值父字段。
        /// </summary>
        string DataValueParentField
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置数据源。
        /// </summary>
        object DataSource
        {
            get;
            set;
        }
        /// <summary>
        /// 绑定数据。
        /// </summary>
        void DataBind();
    }
}
