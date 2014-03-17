//================================================================================
//  FileName:ResourceProvider.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-09 16:28:14
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
using System.IO;
using System.Resources;
using System.Globalization;
using System.Web.Compilation;
using System.Runtime.Serialization.Formatters.Binary;
namespace iPower.Resources
{
    /// <summary>
    /// 资源提供基础类。
    /// </summary>
    internal abstract class ResourceProvider : IResourceProvider
    {
        #region 成员变量，构造函数。
        IResourceStorage resourceStorage;
        ResourcesManagement resourcesManagement;
        static Hashtable cache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ResourceProvider(IResourceStorage resourceStorage)
        {
            this.resourceStorage = resourceStorage;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取资源存储地址接口。
        /// </summary>
        protected IResourceStorage ResourceStorage
        {
            get { return this.resourceStorage; }
        }
        /// <summary>
        /// 获取或设置资源类的名称。
        /// </summary>
        public string ClassKey
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置资源文件的路径。
        /// </summary>
        public string VirtualPath
        {
            get;
            set;
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 创建资源集合。
        /// </summary>
        /// <returns></returns>
        protected abstract ResourceCollection CreateResources();
        #endregion

        #region IResourceProvider 成员
        /// <summary>
        /// 返回键和区域性的资源对象。
        /// </summary>
        /// <param name="resourceKey">标识特定资源的键。</param>
        /// <param name="culture">标识资源本地化值的区域性。</param>
        /// <returns>包含 resourceKey 和 culture 的资源值。</returns>
        public virtual object GetObject(string resourceKey, CultureInfo culture)
        {
            object result = null;
            if (!string.IsNullOrEmpty(resourceKey))
            {
                result = cache[resourceKey];
                if (result == null && this.ResourceReader != null)
                {
                    lock (this)
                    {
                        ResourcesManagement mgr = this.ResourceReader as ResourcesManagement;
                        if (mgr != null && mgr.Resources != null)
                        {
                            mgr.ResourcesPersistAfter += new EventHandler(delegate(object sender, EventArgs e)
                            {
                                cache.Clear();
                            });

                            Resource r = mgr.Resources[resourceKey];
                            if (r != null)
                            {
                                string data = r.ResValue;
                                if (data.StartsWith("0x") && (data.Length % 2 == 0) && data.Length > 50)
                                {
                                    byte[] bytes = new byte[data.Length / 2];
                                    for (int i = 0; i < bytes.Length; i++)
                                    {
                                        bytes[i] = byte.Parse(data.Substring(i * 2, 2), NumberStyles.HexNumber);
                                    }

                                    if (bytes.Length > 0)
                                    {
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            ms.Write(bytes, 0, bytes.Length);
                                            ms.Position = 0;

                                            BinaryFormatter formatter = new BinaryFormatter();
                                            result = formatter.Deserialize(ms);
                                            ms.Close();
                                        }
                                    }
                                }
                                else
                                    result = data;
                                //添加缓存。
                                cache[resourceKey] = result;
                            }
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取一个对象，以便从资源文件中读取资源值。
        /// </summary>
        public virtual IResourceReader ResourceReader
        {
            get
            {
                this.EnsureResourceManager();
                return this.resourcesManagement;
            }
        }

        #endregion

        #region 辅助函数。
        /// <summary>
        /// 确保资源管理类。
        /// </summary>
        /// <returns></returns>
        void EnsureResourceManager()
        {
            if (this.resourcesManagement == null)
            {
                ResourceCollection resource = this.CreateResources();
                if (resource != null)
                    this.resourcesManagement = new ResourcesManagement(resource);
            }
        }
        #endregion
    }
}
