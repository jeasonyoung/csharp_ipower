﻿//================================================================================
//  FileName: EnumData.cs
//  Desc:
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
//2009-12-30 yangyong 修改批处理类型枚举的范围修饰符。
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;

namespace iPower.Data.DataAccess
{
    /// <summary>
    /// 数据库类型枚举。
    /// </summary>
    public enum EnumDbType
    {
        /// <summary>
        /// SqlServer数据库类型。
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// OleDb数据库类型。
        /// </summary>
        OleDb = 1
    }
}