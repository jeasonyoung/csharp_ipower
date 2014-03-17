//================================================================================
//  FileName: FileCache.cs
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
using System.Threading;
using iPower.Utility;
namespace iPower.Cache
{
    /// <summary>
    /// 文件缓存。
    /// </summary>
    public class FileCache
    {
        #region 成员变量，构造函数。
        private static IDictionary<string, ICacheItem> cache = new Dictionary<string, ICacheItem>();
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static FileCache()
        {
            CacheMaxSize = 200;
            ItemQueue = new List<string>();
            Effective = 5;
            CheckInterval = 1;
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 缓存改变事件。
        /// </summary>
        public event CacheChangedEventHandler CacheChanged;
        /// <summary>
        /// 触发缓存改变事件。
        /// </summary>
        /// <param name="e"></param>
        protected void OnCacheChanged(CacheChangedEventArgs e)
        {
            CacheChangedEventHandler handler = this.CacheChanged;
            if (handler != null)
            {
                handler(e);
            }
        }
        /// <summary>
        /// 触发缓存改变事件。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        protected void OnCacheChanged(CacheChangedType type, ICacheItem item)
        {
            CacheChangedEventArgs e = new CacheChangedEventArgs();
            e.ChangedType = type;
            if (item != null)
            {
                e.Item = item;
                e.ItemKey = item.ItemKey;
            }
            this.OnCacheChanged(e);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置缓存器最大容量(M)。
        /// </summary>
        public static long CacheMaxSize { get; set; }
        /// <summary>
        /// 获取缓存器当前容量(M)。
        /// </summary>
        public static long CurrentSize { get; private set; }
        /// <summary>
        /// 获取或设置缓存有效时间(分钟)。
        /// </summary>
        public static int Effective { get; set; }
        /// <summary>
        /// 获取或设置缓存检测间隔时间(分钟)。
        /// </summary>
        public static int CheckInterval { get; set; }
        /// <summary>
        /// 获取缓存列表。
        /// </summary>
        public static List<ICacheItem> CacheItems
        {
            get
            {
                return new List<ICacheItem>(cache.Values);
            }
        }
        /// <summary>
        /// 获取缓存键队列。
        /// </summary>
        public static List<string> ItemQueue { get; private set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public ICacheItem this[string itemKey]
        {
            get
            {
                return this.GetItem(itemKey);
            }
        }

        #region 公共函数。
        /// <summary>
        /// 判断缓存是否存在。
        /// </summary>
        /// <param name="cacheItemKey">缓存键。</param>
        /// <returns></returns>
        public bool ContainsKey(string cacheItemKey)
        {
            if (cache.Count > 0)
            {
                return cache.ContainsKey(cacheItemKey);
            }
            return false;
        }
        /// <summary>
        /// 添加缓存。
        /// </summary>
        /// <param name="item">缓存项。</param>
        /// <returns>缓存键。</returns>
        public string AddCache(ICacheItem item)
        {
            if (item == null)
            {
                return null;
            }
            if (cache.ContainsKey(item.ItemKey))
            {
                cache[item.ItemKey].RefreshItem(DateTime.Now);
            }
            else
            {
                cache[item.ItemKey] = item;
                item.CacheItemChanged += new CacheItemChangedEventHandler(delegate(CacheItemChangedEventArgs e)
                {
                    if ((e.ChangedType == CacheChangedType.Access) && ItemQueue.Contains(e.CacheItemKey))
                    {
                        int index = ItemQueue.IndexOf(e.CacheItemKey);
                        SortHelper.MoveItemToEnd<string>(ItemQueue, index);
                        this.OnCacheChanged(CacheChangedType.Access, cache[e.CacheItemKey]);
                    }
                });
                if (item.ItemSize > 0)
                {
                    CurrentSize += item.ItemSize / 1024 / 1024;
                }
                if (CurrentSize >= CacheMaxSize)
                {
                    startCheckWorker();
                }
                this.OnCacheChanged(CacheChangedType.Added, item);
            }
            if (!ItemQueue.Contains(item.ItemKey))
            {
                ItemQueue.Add(item.ItemKey);
            }
            return item.ItemKey;
        }
        /// <summary>
        /// 添加缓存。
        /// </summary>
        /// <param name="item">缓存项。</param>
        /// <returns>缓存键。</returns>
        public string AddFileCache(FileCacheItem item)
        {
            return this.AddCache(item);
        }
        /// <summary>
        /// 添加文件缓存。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <param name="loadType">加载类型。</param>
        /// <returns>缓存键。</returns>
        public string AddFileCache(string filePath, DataLoadType loadType)
        {
            return this.AddFileCache(new FileCacheItem(filePath, loadType, CacheKeyType.MD5));
        }
        /// <summary>
        /// 获取缓存项。
        /// </summary>
        /// <param name="cacheItemKey">缓存键。</param>
        /// <returns>缓存项对象。</returns>
        public ICacheItem GetItem(string cacheItemKey)
        {
            ICacheItem item = null;
            if (!string.IsNullOrEmpty(cacheItemKey) && cache.ContainsKey(cacheItemKey))
            {
                item = cache[cacheItemKey];
                if (item is FileCacheItem)
                {
                    ((FileCacheItem)item).DelayLoad();
                }
                item.RefreshItem(DateTime.Now);
            }
            return item;
        }
        /// <summary>
        /// 获取文件缓存项。
        /// </summary>
        /// <param name="cacheItemKey">缓存键。</param>
        /// <returns>文件缓存项对象。</returns>
        public FileCacheItem GetFileCacheItem(string cacheItemKey)
        {
            return this.GetItem(cacheItemKey) as FileCacheItem;
        }
        /// <summary>
        /// 移除缓存。
        /// </summary>
        /// <param name="cacheItemKey"></param>
        public void Remove(string cacheItemKey)
        {
            if (!string.IsNullOrEmpty(cacheItemKey) && cache != null && cache.ContainsKey(cacheItemKey))
            {
                lock (cache)
                {
                    ICacheItem item = cache[cacheItemKey];
                    cache.Remove(cacheItemKey);
                    CurrentSize -= item.ItemSize / 1024 / 1024;
                    if (ItemQueue.Contains(cacheItemKey))
                    {
                        ItemQueue.Remove(cacheItemKey);
                    }
                    this.OnCacheChanged(CacheChangedType.Removed, item);
                }
            }
        }
        #endregion

        #region 定时检测。
        //定时检测。
        private static readonly Timer timer = new Timer(new TimerCallback(delegate(object o)
        {
            startCheckWorker();
        }), null, CheckInterval * 60 * 1000, CheckInterval * 60 * 1000);
        /// <summary>
        /// 开始检测。
        /// </summary>
        private static void startCheckWorker()
        {
            if (cache == null || cache.Count == 0)
            {
                return;
            }
            lock (cache)
            {
                List<ICacheItem> items = new List<ICacheItem>(cache.Values);
                FileCache fc = new FileCache();
                DateTime now = DateTime.Now;
                for (int i = 0; i < items.Count; i++)
                {
                    ICacheItem item = items[i];
                    if (item.LastAccessDate.AddMinutes(Effective) < now)
                    {
                        fc.Remove(item.ItemKey);
                        i--;
                    }
                }

                if (CurrentSize > CacheMaxSize)
                {
                    while (CurrentSize > CacheMaxSize)
                    {
                        string key = ItemQueue[0];
                        fc.Remove(key);
                    }
                }
            }
        }
        #endregion
    }
}