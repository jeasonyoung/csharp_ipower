//================================================================================
//  FileName: FileStorageFactory.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/27
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
using System.Text;

using iPower;
using iPower.Utility;
namespace iPower.FileStorage
{
    /// <summary>
    /// 文件存储工程实例类。
    /// </summary>
    /// <example>
    /// &lt;configuration&gt;
    ///     &lt;!--注册配置节--&gt;
    ///     &lt;configSections&gt;
    ///          &lt;!--注册附件存储配置--&gt;
    ///          &lt;section name ="FileStorage" type="iPower.Configuration.iPowerSection,iPower"/&gt;
    ///     &lt;/configSections&gt;
    /// &lt;/configuration&gt;
    /// 
    /// &lt;!--附件存储配置--&gt;
    /// &lt;FileStorage&gt;
    ///     &lt;!--文件存储的实现工厂程序集--&gt;
    ///     &lt;add key="iPower.FileStorageFactoryAssembly" value="iPower.FileStorage.SQLServer.FileStorageFactory,iPower.FileStorage"/&gt;
    ///     &lt;!--文件存储地址（数据库或本地文件夹）--&gt;
    ///     &lt;add key="iPower.DbConnectionString" value="DefaultDbServer"/&gt;
    /// &lt;/FileStorage&gt;
    /// </example>
    public sealed class FileStorageFactoryInstance : IFileStorageFactory
    {
        #region 成员变量，构造函数。
        IFileStorageFactory factory = null;
        static Hashtable Cache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 构造函数。
        /// </summary>
        private FileStorageFactoryInstance(string fileStorageFactoryAssembly)
        {
            if (string.IsNullOrEmpty(fileStorageFactoryAssembly))
                throw new ArgumentNullException("未配置文件存储的实现工厂程序集");
            this.factory = Cache[fileStorageFactoryAssembly] as IFileStorageFactory;
            if (this.factory == null)
            {
                this.factory = TypeHelper.Create(fileStorageFactoryAssembly) as IFileStorageFactory;
                if (this.factory != null)
                    Cache[fileStorageFactoryAssembly] = this.factory;
            }
        }
        #endregion

        #region 静态实例。
        static object synchronizationObject = new object();
        static FileStorageFactoryInstance facotryInstance;
        /// <summary>
        /// 获取实例对象。
        /// </summary>
        public static FileStorageFactoryInstance Instance
        {
            get
            {
                lock (synchronizationObject)
                {
                    if (facotryInstance == null)
                    {
                        facotryInstance = new FileStorageFactoryInstance(new StorageModuleConfiguration().FileStorageFactoryAssembly);
                    }
                    return facotryInstance;
                }
            }
        }
        #endregion

        #region IFileStorageFactory 成员
        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <param name="offSet">偏移量。</param>
        /// <param name="content">文件内容。</param>
        /// <returns>成功返回True，失败False。</returns>
        public bool Upload(string fileName, long offSet, byte[] content)
        {
            if (this.factory == null) return false;
            return this.factory.Upload(fileName, offSet, content);
        }
        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>成功返回true,失败返回false。</returns>
        public bool DeleteFile(string fileName)
        {
            if (this.factory == null) return false;
            return this.factory.DeleteFile(fileName);
        }
        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>文件数据。</returns>
        public byte[] Download(string fileName)
        {
            if (this.factory == null) return null;
            return this.factory.Download(fileName);
        }

        #endregion
    }
}