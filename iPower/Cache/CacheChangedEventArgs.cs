//================================================================================
//  FileName: FileCacheChangedEventArgs.cs
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
    /// 缓存改变委托。
    /// </summary>
    /// <param name="e">缓存改变委托参数。</param>
    public delegate void CacheChangedEventHandler(CacheChangedEventArgs e);
    /// <summary>
    /// 缓存改变委托参数。
    /// </summary>
    public class CacheChangedEventArgs
    {
        /// <summary>
        /// 获取或设置缓存项键。
        /// </summary>
        public string ItemKey { get; set; }
        /// <summary>
        /// 获取或设置缓存对象。
        /// </summary>
        public ICacheItem Item { get; set; }
        /// <summary>
        /// 获取或设置缓存变更类型。
        /// </summary>
        public CacheChangedType ChangedType { get; set; }
    }
}
