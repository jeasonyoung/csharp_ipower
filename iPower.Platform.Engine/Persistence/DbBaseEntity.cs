//================================================================================
//  FileName: DbBaseEntity.cs
//  Desc: ORM基类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-23
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
using System.Data;

using iPower;
using iPower.Configuration;
using iPower.Data;
using iPower.Data.ORM;
namespace iPower.Platform.Engine.Persistence
{
    /// <summary>
    /// ORM基类。
    /// </summary>
    /// <typeparam name="T">实体类。</typeparam>
    /// <typeparam name="K">配置类。</typeparam>
    public class DbBaseEntity<T, K> : ORMDbEntity<T>//,ICache
        where T : new()
        where K : BaseModuleConfiguration
    {
        #region 成员变量，构造函数。
        K mConfig;
        static Hashtable htCache;
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static DbBaseEntity()
        {
            htCache = Hashtable.Synchronized(new Hashtable());
        }
        /// <summary>
        /// 构造函数。
        /// <param name="config">模块配置实例。</param>
        /// </summary>
        public DbBaseEntity(K config)
            : base()
        {
            this.mConfig = config;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取模块配置信息。
        /// </summary>
        protected K ModuleConfig
        {
            get { return this.mConfig; }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 创建数据链接。
        /// </summary>
        /// <returns></returns>
        protected override IDBAccess CreateDBAccess()
        {
            return this.ModuleConfig.ModuleDefaultDatabase;
        }
        #endregion

        #region ICache 成员
        /// <summary>
        /// 获取缓存。
        /// </summary>
        protected Hashtable Cache
        {
            get
            {
                lock (this)
                {
                    return htCache;
                }
            }
        }
        #endregion
    }
}
