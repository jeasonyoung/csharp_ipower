//================================================================================
//  FileName: ResourceCollection.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2012-01-09 
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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
namespace iPower.Resources
{
    /// <summary>
    /// 资源。
    /// </summary>
    [Serializable]
    [XmlRoot("Resource")]
    public class Resource
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Resource()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="resKey">资源键名。</param>
        /// <param name="resValue">资源键值。</param>
        /// <param name="description">注释说明。</param>
        public Resource(string resKey, string resValue, string description)
        {
            this.ResKey = resKey;
            this.ResValue = resValue;
            this.Description = description;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="resKey">资源键名。</param>
        /// <param name="resValue">资源键值。</param>
        public Resource(string resKey, string resValue)
            : this(resKey, resValue, string.Empty)
        {

        }
        #endregion

        /// <summary>
        /// 资源键名。
        /// </summary>
        [XmlAttribute("resKey")]
        public string ResKey { get; set; }
        /// <summary>
        /// 资源键值。
        /// </summary>
        [XmlAttribute("resValue")]
        public string ResValue { get; set; }
        /// <summary>
        /// 注释说明。
        /// </summary>
        [XmlAttribute("desc")]
        public string Description { get; set; }
    }
    /// <summary>
    /// 资源集合。
    /// </summary>
    [Serializable]
    [XmlRoot("Resources")]
    public class ResourceCollection : ICollection<Resource>, IComparer<Resource>, IListSource
    {
        #region 成员变量，构造函数。
        List<Resource> items;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ResourceCollection()
        {
            this.items = new List<Resource>();
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取索引数据。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [XmlIgnore]
        public Resource this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                    throw new ArgumentOutOfRangeException("index");
                return this.items[index];
            }
        }
        /// <summary>
        /// 获取索引数据。
        /// </summary>
        /// <param name="resKey"></param>
        /// <returns></returns>
        [XmlIgnore]
        public Resource this[string resKey]
        {
            get
            {
                if (string.IsNullOrEmpty(resKey))
                    throw new ArgumentNullException("resKey");
                Resource result = this.items.Find(new Predicate<Resource>(delegate(Resource sender)
                {
                    return (sender != null) && (sender.ResKey == resKey);
                }));
                return result;
            }
        }
        #endregion

        #region ICollection<Resource> 成员
        /// <summary>
        /// 添加。
        /// </summary>
        /// <param name="item"></param>
        public void Add(Resource item)
        {
            if (!this.Contains(item))
                this.items.Add(item);
        }
        /// <summary>
        /// 清空。
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
        }
        /// <summary>
        /// 判断是否存在。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Resource item)
        {
            if (item != null)
                return this[item.ResKey] != null;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(Resource[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public int Count
        {
            get { return this.items.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 移除。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(Resource item)
        {
            return this.items.Remove(item);
        }

        #endregion

        #region IEnumerable<Resource> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Resource> GetEnumerator()
        {
            this.items.Sort(this);
            return this.items.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            this.items.Sort(this);
            foreach (Resource r in this.items)
                yield return r;
        }

        #endregion

        #region IComparer<Resource> 成员
        /// <summary>
        /// 比较排序。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual int Compare(Resource x, Resource y)
        {
            return string.Compare(x.ResKey, y.ResKey);
        }

        #endregion

        #region 序列化与反序列化。
        /// <summary>
        /// 序列化。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="resources"></param>
        internal static void Serializer(Stream stream, ResourceCollection resources)
        {
            lock (typeof(ResourceCollection))
            {
                if (stream != null && resources != null)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ResourceCollection));
                    ser.Serialize(stream, resources);
                }
            }
        }
        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static ResourceCollection DeSerializer(Stream stream)
        {
            lock (typeof(ResourceCollection))
            {
                if (stream != null)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ResourceCollection));
                    return ser.Deserialize(stream) as ResourceCollection;
                }
                return null;
            }
        }
        #endregion

        #region IListSource 成员
        /// <summary>
        /// 
        /// </summary>
        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList IListSource.GetList()
        {
            return this.items;
        }

        #endregion
    }
}
