//================================================================================
//  FileName: CollectionBase.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-2
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace iPower.Utility
{
    /// <summary>
    /// 集合基础类（泛型）。
    /// </summary>
    /// <typeparam name="T">集合收集对象的类型。</typeparam>
    public class CollectionBase<T> :NameObjectCollectionBase
    {
        #region 成员变量，构造函数。
        bool uniqueKey;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="uniqueKey">键是否限制为唯一。</param>
        public CollectionBase(bool uniqueKey)
            : base()
        {
            this.uniqueKey = uniqueKey;
        }
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CollectionBase()
            : this(false)
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置键是否限制为唯一。
        /// </summary>
        public virtual bool UniqueKey
        {
            get { return this.uniqueKey; }
            protected set { this.uniqueKey = value; }
        }

        /// <summary>
        /// 按索引的方式获取项。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>项。</returns>
        public virtual T this[int index]
        {
            get { return (T)base.BaseGet(index); }
        }

        /// <summary>
        /// 按键值方式获取项。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <returns>项。</returns>
        public virtual T this[string key]
        {
            get { return (T)base.BaseGet(key); }
        }

        /// <summary>
        /// 获取所以的值。
        /// </summary>
        public virtual T[] Values
        {
            get { return base.BaseGetAllValues(typeof(T)) as T[]; }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 添加一项到集合中。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <param name="value">项。</param>
        public virtual void Add(string key, T value)
        {
            if (this.UniqueKey && this.Contains(key))
                throw new Exception("已存在键值：" + key);
            this.BaseAdd(key, value);
        }

        /// <summary>
        /// 添加/替换一项到集合中。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <param name="value">项。</param>
        public virtual void Set(string key, T value)
        {
            this.BaseSet(key, value);
        }

        /// <summary>
        /// 集合中是否包含某项。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <returns>是/否。</returns>
        public bool Contains(string key)
        {
            return this.BaseGet(key) != null;
        }

        /// <summary>
        /// 移除某项。
        /// </summary>
        /// <param name="key">键值。</param>
        public virtual void Remove(string key)
        {
            this.BaseRemove(key);
        }

        /// <summary>
        /// 在指定处移除某项。
        /// </summary>
        /// <param name="index">索引。</param>
        public virtual void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        /// <summary>
        /// 清空集合中所有元素。
        /// </summary>
        public virtual void Clear()
        {
            this.BaseClear();
        }

        /// <summary>
        /// 复制到数组中。
        /// </summary>
        /// <returns>数组。</returns>
        public virtual T[] CopyToArray()
        {
            return this.Values;
        }

        /// <summary>
        /// 获取具有相同键值的项。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <returns>具有相同键值的项。</returns>
        public virtual T[] GetItems(string key)
        {
            List<T> list = new List<T>();
            foreach (string k in this.Keys)
            {
                if (string.Compare(k, key, true) == 0)
                    list.Add(this[key]);
            }
            return list.ToArray();
        }
        #endregion
    }
}
