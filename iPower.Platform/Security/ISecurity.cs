//================================================================================
//  FileName: ISecurity.cs
//  Desc:安全接口。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-23
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
using iPower.Platform.Security;
namespace iPower.Platform.Security
{
    /// <summary>
    /// 安全接口。
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// 获取或设置系统安全标示。
        /// </summary>
        GUIDEx SecurityID
        {
            get;
            set;
        }
        /// <summary>
        ///验证权限。
        /// </summary>
        /// <param name="permissions">权限集合。</param>
        void VerifyPermissions(SecurityPermissionCollection permissions);
    }
}
