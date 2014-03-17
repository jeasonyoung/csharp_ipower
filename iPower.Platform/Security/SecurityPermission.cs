//================================================================================
//  FileName: ISecurityPermission.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/13
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
using iPower.Data;
namespace iPower.Platform.Security
{
    /// <summary>
    /// 安全权限元操作。
    /// </summary>
    public class SecurityPermission
    {
        /// <summary>
        /// 获取或设置权限元操作ID。
        /// </summary>
        public string PermissionID { get; set; }
        /// <summary>
        /// 获取或设置权限元操作名称。
        /// </summary>
        public string PermissionName { get; set; }
    }
    /// <summary>
    /// 安全权限元操作集合。
    /// </summary>
    public class SecurityPermissionCollection : DataCollection<SecurityPermission>
    {
        /// <summary>
        /// 根据权限元操作ID获取操作接口。
        /// </summary>
        /// <param name="permissionID"></param>
        /// <returns></returns>
        public SecurityPermission this[string permissionID]
        {
            get
            {
                if (string.IsNullOrEmpty(permissionID))
                    return null;
                SecurityPermission sp = this.Items.Find(new Predicate<SecurityPermission>(delegate(SecurityPermission data)
                {
                    return (data != null) && (data.PermissionID == permissionID);
                }));
                return sp;
            }
        }
    }
}
