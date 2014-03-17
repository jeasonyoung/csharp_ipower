//================================================================================
//  FileName:ResourceProviderFactory.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-09 16:51:19
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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Globalization;
namespace iPower.Resources
{
    /// <summary>
    /// 资源工厂。
    /// </summary>
    public sealed class ResourceFactory
    {
        #region 成员变量，构造函数。
        IResourceStorage storage;
        ResourcesManagement resourcesManagement;
        string cultureName;
        static Hashtable cache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 构造函数。
        /// </summary>
        internal ResourceFactory(IResourceStorage rs)
        {
            if (rs == null)
                throw new ArgumentNullException("资源存储未配置。");
            this.storage = rs;
        }
        #endregion

        #region 静态属性。
        /// <summary>
        /// 获取静态对象。
        /// </summary>
        public static ResourceFactory Instance
        {
            get
            {
                lock (typeof(ResourceFactory))
                {
                    return new ResourceFactory(ResourceConfiguration.ModuleConfig);
                }
            }
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取资源集合。
        /// </summary>
        public ResourceCollection Resources
        {
            get
            {
                lock (this)
                {
                    string path = this.GetResoucePath();
                    ResourceCollection resources = cache[path] as ResourceCollection;
                    if ((resources == null) && File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            resources = ResourceCollection.DeSerializer(fs);
                            if (resources != null)
                                cache[path] = resources;
                        }
                    }
                    return resources;
                }
            }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name">键。</param>
        /// <param name="value">值。</param>
        /// <param name="description">描述。</param>
        public void AddResource(string name, byte[] value, string description)
        {
            if (!string.IsNullOrEmpty(name) && (value != null))
            {
                this.EnsureResourcesManagement();
                this.resourcesManagement.AddResource(name, value, description);
            }
        }
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name">键。</param>
        /// <param name="value">值。</param>
        /// <param name="description">描述。</param>
        public void AddResource(string name, object value, string description)
        {
            if (!string.IsNullOrEmpty(name) && (value != null))
            {
                this.EnsureResourcesManagement();
                this.resourcesManagement.AddResource(name, value, description);
            }
        }
        /// <summary>
        /// 添加资源。
        /// </summary>
        /// <param name="name">键。</param>
        /// <param name="value">值。</param>
        /// <param name="description">描述。</param>
        public void AddResource(string name, string value, string description)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.EnsureResourcesManagement();
                this.resourcesManagement.AddResource(name, value, description);
            }
        }
        /// <summary>
        /// 关闭资源。
        /// </summary>
        public void Close()
        {
            if (this.resourcesManagement != null)
                this.resourcesManagement.Close();
        }
        /// <summary>
        /// 资源写到输出文件或输出流中。
        /// </summary>
        public void Generate()
        {
            if (this.resourcesManagement != null)
                this.resourcesManagement.Generate();
        }
        /// <summary>
        /// 移除资源。
        /// </summary>
        /// <param name="name"></param>
        public void RemoveResource(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.EnsureResourcesManagement();
                this.resourcesManagement.RemoveResource(name);
            }
        }
        /// <summary>
        /// 设置资源语言区域。
        /// </summary>
        /// <param name="cultrueInfo"></param>
        public void SetResourcesCulture(CultureInfo cultrueInfo)
        {
            if (cultrueInfo != null)
                this.cultureName = cultrueInfo.Name;
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            if (this.resourcesManagement != null)
                this.resourcesManagement.Dispose();
        }

        #endregion

        #region 辅助函数。
        /// <summary>
        /// 取保资源管理对象。
        /// </summary>
        void EnsureResourcesManagement()
        {
            if (this.resourcesManagement == null)
            {

                ResourceCollection resources = this.Resources;
                if (resources == null)
                    resources = new ResourceCollection();

                this.resourcesManagement = new ResourcesManagement(resources);
                this.resourcesManagement.ResourcesPersist += this.ResourcesManagement_ResourcesPersist;
                this.resourcesManagement.ResourcesPersistAfter += new EventHandler(delegate(object sender, EventArgs e)
                {
                    cache.Clear();
                });
            }
        }
        /// <summary>
        /// 获取资源文件路径。
        /// </summary>
        /// <returns></returns>
        string GetResoucePath()
        {
            string path = this.storage.ResourceStorage;
            if (!string.IsNullOrEmpty(this.cultureName))
                path = string.Format("{0}.{1}{2}", path.Split('.')[0], this.cultureName, Path.GetExtension(path));

            if (!File.Exists(path))
                path = Path.GetFullPath(string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, path));
            return path;
        }
        /// <summary>
        /// 资源持久化。
        /// </summary>
        /// <param name="resources"></param>
        void ResourcesManagement_ResourcesPersist(ResourceCollection resources)
        {
            if (resources != null)
            {
                lock (this)
                {
                    string path = this.GetResoucePath();
                    if (!string.IsNullOrEmpty(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            ResourceCollection.Serializer(fs, resources);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
