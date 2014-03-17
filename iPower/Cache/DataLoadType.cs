//================================================================================
//  FileName: CacheLoadType.cs
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
    /// 数据加载类型。
    /// </summary>
    public enum DataLoadType
    {
        /// <summary>
        /// 不加载。
        /// </summary>
        None = 0,
        /// <summary>
        /// 加载。
        /// </summary>
        Load = 1,
        /// <summary>
        /// 延迟加载。
        /// </summary>
        DelayLoad = 2
    }
}