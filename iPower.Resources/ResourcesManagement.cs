//================================================================================
//  FileName:ResourceManage.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-09 16:30:37
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
// ----  ------  -----------
//
//================================================================================
//  Copyright (C) 2009-2010 Jeason Young Corporation
//================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Resources;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace iPower.Resources
{
    /// <summary>
    /// 更新资源到持久层。
    /// </summary>
    /// <param name="resources"></param>
    internal delegate void UpdateResourcesPersistHandler(ResourceCollection resources);
    /// <summary>
    /// 资源管理。
    /// </summary>
    internal class ResourcesManagement : IResourceReader, IResourceWriter
    {
        #region 成员变量，构造函数。
        ResourceCollection resources;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="resources"></param>
        public ResourcesManagement(ResourceCollection resources)
        {
            if (resources == null)
                throw new ArgumentNullException("resources");
            this.resources = resources;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取资源。
        /// </summary>
        public ResourceCollection Resources
        {
            get { return this.resources; }
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 资源持久事件。
        /// </summary>
        public event UpdateResourcesPersistHandler ResourcesPersist;
        /// <summary>
        /// 触发资源持久事件。
        /// </summary>
        /// <param name="resources"></param>
        protected virtual void OnResourcesPersist(ResourceCollection resources)
        {
            UpdateResourcesPersistHandler handler = this.ResourcesPersist;
            if (handler != null && resources != null)
                handler(resources);
        }
        /// <summary>
        /// 资源持久事件之后事件。
        /// </summary>
        public event EventHandler ResourcesPersistAfter;
        /// <summary>
        /// 触发资源持久事件之后事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnResourcesPersistAfter(EventArgs e)
        {
            EventHandler handler = this.ResourcesPersistAfter;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region IResourceReader 成员
        /// <summary>
        /// 释放与资源阅读器关联的所有资源后将该阅读器关闭。
        /// </summary>
        public virtual void Close()
        {
        }
        /// <summary>
        /// 返回此阅读器的资源。
        /// </summary>
        /// <returns></returns>
        public IDictionaryEnumerator GetEnumerator()
        {
            return new ResourceEnumerator(this.resources);
        }

        #endregion

        #region IEnumerable 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.resources.GetEnumerator();
        }

        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }

        #endregion

        #region IResourceWriter 成员
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddResource(string name, byte[] value)
        {
            this.AddResource(name, value, null);
        }
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public void AddResource(string name, byte[] value, string description)
        {
            if (!string.IsNullOrEmpty(name) && value != null)
            {
                lock (this)
                {
                    StringBuilder builder = new StringBuilder("0x");
                    foreach (byte b in value)
                        builder.Append(b.ToString("x2"));
                    this.AddResource(name, builder.ToString(), description);
                }
            }
        }
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddResource(string name, object value)
        {
            this.AddResource(name, value, null);
        }
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public void AddResource(string name, object value, string description)
        {
            if (!string.IsNullOrEmpty(name) && (value != null))
            {
                lock (this)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(ms, value);

                        byte[] buffer = ms.ToArray();
                        ms.Close();
                        this.AddResource(name, buffer, description);
                    }
                }
            }
        }
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddResource(string name, string value)
        {
            this.AddResource(name, value, null);
        }
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public void AddResource(string name, string value, string description)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Resource item = this.resources[name];
                if (item == null)
                {
                    item = new Resource();
                    item.ResKey = name;
                    this.resources.Add(item);
                }
                item.ResValue = value;
                if (!string.IsNullOrEmpty(description))
                    item.Description = description;
            }
        }
        /// <summary>
        /// 资源写到输出文件或输出流中。
        /// </summary>
        public void Generate()
        {
            this.OnResourcesPersist(this.resources);
            this.OnResourcesPersistAfter(EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// 移除资源。
        /// </summary>
        /// <param name="name"></param>
        public void RemoveResource(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Resource item = this.resources[name];
                if (item != null)
                    this.resources.Remove(item);
            }
        }

        #region 内置类。
        class ResourceEnumerator : IDictionaryEnumerator, IEnumerator
        {
            #region 成员变量，构造函数。
            ResourceCollection resources;
            bool currentIsValid;
            int index = -1;
            /// <summary>
            /// 构造函数。
            /// </summary>
            /// <param name="resources"></param>
            public ResourceEnumerator(ResourceCollection resources)
            {
                this.resources = resources;
            }
            #endregion

            #region IDictionaryEnumerator 成员
            /// <summary>
            /// 
            /// </summary>
            public DictionaryEntry Entry
            {
                get
                {
                    lock (this)
                    {
                        if (!this.currentIsValid)
                            throw new InvalidOperationException();
                        Resource r = this.resources[this.index];
                        if (r == null)
                            throw new ArgumentNullException();
                        return new DictionaryEntry(r.ResKey, r.ResValue);
                    }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public object Key
            {
                get
                {
                    return this.Entry.Key;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public object Value
            {
                get
                {
                    return this.Entry.Value;
                }
            }

            #endregion

            #region IEnumerator 成员
            /// <summary>
            /// 
            /// </summary>
            public object Current
            {
                get { return this.Entry; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (this.index == (this.resources.Count - 1))
                {
                    this.currentIsValid = false;
                    this.index = this.resources.Count - 1;
                    return false;
                }
                this.currentIsValid = true;
                this.index++;
                return true;
            }
            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                this.currentIsValid = false;
                this.index = -1;
            }
            #endregion
        }
        #endregion
    }
}
