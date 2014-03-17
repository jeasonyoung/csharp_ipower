//================================================================================
//  FileName:IButton.cs
//  Desc:按钮控制接口。
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-10-22 11:22:15
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
    /// 按钮控制接口。
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), 
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IButton
    {
        /// <summary>
        /// 获取或设置是否启用。
        /// </summary>
        bool Enabled
        {
            get;
            set;
        }
    }
}
