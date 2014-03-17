//================================================================================
//  FileName: ITreeView.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/1
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
using System.Data;
using System.Web;
using System.Security.Permissions;
namespace iPower.Platform
{
    /// <summary>
    /// 树形控件接口。
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface ITreeView
    {
        /// <summary>
        /// 获取或设置数据源。
        /// </summary>
        object DataSource { get; set; }
        /// <summary>
        /// 获取或设置ID映射字段。
        /// </summary>
        string IDField { get; set; }
        /// <summary>
        /// 获取或设置父ID值映射字段。
        /// </summary>
        string PIDField { get; set; }
        /// <summary>
        /// 获取或设置标题映射字段。
        /// </summary>
        string TitleField { get; set; }
        /// <summary>
        /// 获取或设置排序映射字段。
        /// </summary>
        string OrderNoField { get; set; }
        /// <summary>
        ///  构建树。
        /// </summary>
        void BuildTree();
    }
}
