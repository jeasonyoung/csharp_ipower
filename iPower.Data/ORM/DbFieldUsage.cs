//================================================================================
//  FileName: DbFieldUsage.cs
//  Desc:字段属性枚举。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-16
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
namespace iPower.Data.ORM
{
    /// <summary>
    /// 字段属性枚举。
    /// </summary>
    [Flags]
    public enum DbFieldUsage
    {
        /// <summary>
        /// 未设置。
        /// </summary>
        None = 1,
        /// <summary>
        /// 关键字。
        /// </summary>
        PrimaryKey = 2,
        /// <summary>
        /// 唯一约束。
        /// </summary>
        UniqueKey = 4,
        /// <summary>
        /// 自增字段。
        /// </summary>
        BySystem = 8,
        /// <summary>
        /// 空数据不更新。
        /// </summary>
        EmptyOrNullNotUpdate = 16
    }
}
