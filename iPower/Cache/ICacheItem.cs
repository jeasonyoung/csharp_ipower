//================================================================================
//  FileName: ICacheItem.cs
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
    /// 缓存项接口。
    /// </summary>
    public interface ICacheItem
    {
        /// <summary>
        /// 获取或设置缓存大小。
        /// </summary>
        long ItemSize { get; }
        /// <summary>
        /// 获取或设置缓存键。
        /// </summary>
        string ItemKey { get; set; }
        /// <summary>
        /// 获取或设置最后存活时间。
        /// </summary>
        DateTime LastAccessDate { get; }
        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="accessDate"></param>
        void RefreshItem(DateTime accessDate);
        /// <summary>
        /// 缓存项改变通知事件。
        /// </summary>
        event CacheItemChangedEventHandler CacheItemChanged;
    }
}
