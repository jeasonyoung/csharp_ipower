//================================================================================
//  FileName: CacheItem.cs
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
    /// 缓存项基类。
    /// </summary>
    [Serializable]
    public class CacheItem : ICacheItem
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CacheItem()
        {
            this.LastAccessDate = DateTime.MinValue;
        }
        #endregion

        #region ICacheItem 成员
        /// <summary>
        /// 获取或设置缓存大小。
        /// </summary>
        public long ItemSize { get; protected set; }
        /// <summary>
        /// 获取或设置缓存键。
        /// </summary>
        public string ItemKey { get; set; }
        /// <summary>
        /// 获取或设置最后存活时间。
        /// </summary>
        public DateTime LastAccessDate { get; protected set; }
        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="accessDate"></param>
        public void RefreshItem(DateTime accessDate)
        {
            this.LastAccessDate = accessDate;
            this.OnCacheItemChanged();
        }
        /// <summary>
        /// 缓存项改变通知事件。
        /// </summary>
        public event CacheItemChangedEventHandler CacheItemChanged;
        #endregion

        /// <summary>
        /// 触发缓存项改变通知事件。
        /// </summary>
        protected virtual void OnCacheItemChanged()
        {
            CacheItemChangedEventHandler handler = this.CacheItemChanged;
            if (handler != null)
            {
                handler(new CacheItemChangedEventArgs(this.ItemKey, this.LastAccessDate));
            }
        }
    }
}