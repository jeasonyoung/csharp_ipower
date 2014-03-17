//================================================================================
//  FileName: CacheItemChangedEventArgs.cs
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
    /// 缓存项改变委托。
    /// </summary>
    /// <param name="e"></param>
    public delegate void CacheItemChangedEventHandler(CacheItemChangedEventArgs e);
    /// <summary>
    /// 缓存项改变事件。
    /// </summary>
    public class CacheItemChangedEventArgs
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="cacheItemKey">缓存项键。</param>
        /// <param name="lastAccessDate">缓存项最后活动时间。</param>
        public CacheItemChangedEventArgs(string cacheItemKey,DateTime lastAccessDate)
        {
            this.CacheItemKey = cacheItemKey;
            this.LastAccessDate = lastAccessDate;
        }
        /// <summary>
        /// 获取或设置缓存项键。
        /// </summary>
        public string CacheItemKey { get; set; }
        /// <summary>
        /// 获取或设置缓存项最后活动时间。
        /// </summary>
        public DateTime LastAccessDate { get; set; }
        /// <summary>
        /// 获取或设置缓存项改变类型。
        /// </summary>
        public CacheChangedType ChangedType { get; set; }
    }
}
