//================================================================================
//  FileName: CacheKeyType.cs
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
    /// 缓存键类型。
    /// </summary>
    public enum CacheKeyType
    {
        /// <summary>
        /// MD5值。
        /// </summary>
        MD5 = 0,
        /// <summary>
        /// 文件路径。
        /// </summary>
        FilePath = 1,
        /// <summary>
        /// 自定义。
        /// </summary>
        Custom = 2
    }
}