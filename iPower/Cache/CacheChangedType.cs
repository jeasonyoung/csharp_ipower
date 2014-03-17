//================================================================================
//  FileName: CacheChangedType.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-26
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

namespace iPower.Cache
{
    /// <summary>
    /// 缓存改变类型枚举。
    /// </summary>
    public enum CacheChangedType
    {
        /// <summary>
        /// 添加。
        /// </summary>
        Added = 0,
        /// <summary>
        /// 访问。
        /// </summary>
        Access = 1,
        /// <summary>
        /// 移除。
        /// </summary>
        Removed = 2
    }
}
