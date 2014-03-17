//================================================================================
//  FileName: SecurityPermissionConstants.cs
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

namespace iPower.Platform.Security
{
    /// <summary>
    /// 元操作权限常量。
    /// </summary>
    public static class SecurityPermissionConstants
    {
        /// <summary>
        /// 查询(AAA000000000000000000000000000A1)。
        /// </summary>
        public const string Query = "AAA000000000000000000000000000A1";
        /// <summary>
        /// 新增(AAA000000000000000000000000000A2)。
        /// </summary>
        public const string Add = "AAA000000000000000000000000000A2";
        /// <summary>
        /// 保存(AAA000000000000000000000000000A3)。
        /// </summary>
        public const string Save = "AAA000000000000000000000000000A3";
        /// <summary>
        /// 删除(AAA000000000000000000000000000A4)。
        /// </summary>
        public const string Delete = "AAA000000000000000000000000000A4";
        /// <summary>
        /// 导出(AAA000000000000000000000000000A5)。
        /// </summary>
        public const string Export = "AAA000000000000000000000000000A5";
        /// <summary>
        /// 导入(AAA000000000000000000000000000A6)。
        /// </summary>
        public const string Import = "AAA000000000000000000000000000A6";
    }
}
