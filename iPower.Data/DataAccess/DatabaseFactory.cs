//================================================================================
//  FileName: DatabaseFactory.cs
//  Desc: 数据访问工厂。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-16
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
// 2009-12-30   yangyong 修改对象缓存实现。
// 2010-06-24   yangyong 修改将密封类取消密封，单件类。
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Data;
using System.Collections.Generic;
using iPower;
using iPower.Utility;
using iPower.Configuration;
namespace iPower.Data.DataAccess
{
    /// <summary>
    /// 数据访问工厂。
    /// </summary>
    public class DatabaseFactory
    {
        #region 成员变量，构造函数，析构函数
        static Dictionary<string, IDBAccess> objCache;

        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static DatabaseFactory()
        {
            objCache = new Dictionary<string, IDBAccess>();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        protected DatabaseFactory()
        {
         
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~DatabaseFactory()
        {
        }
        #endregion

        #region 静态对象。
        /// <summary>
        /// 静态数据访问对象。 
        /// </summary>
        /// <param name="connectionString">数据链接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>数据访问接口</returns>
        public static IDBAccess Instance(string connectionString, EnumDbType dbType)
        {
            Guard.ArgumentNotNullOrEmptyString("数据链接字符串", connectionString, true);
            lock (typeof(DatabaseFactory))
            {
                IDBAccess instance = objCache.ContainsKey(connectionString) ? objCache[connectionString] : null;
                if (instance == null)
                {
                    instance = new DatabaseFactory().CreateInstance(dbType,connectionString);
                    if (instance != null)
                        objCache[connectionString] = instance;
                }
                return instance;
            }
        }
        /// <summary>
        /// 静态数据访问对象,默认为SqlServer数据库。 
        /// </summary>
        /// <param name="connectionString">数据链接字符串</param>
        /// <returns></returns>
        public static IDBAccess Instance(string connectionString)
        {
            return Instance(connectionString, EnumDbType.SqlServer);
        }
        /// <summary>
        /// 静态数据访问对象。 
        /// </summary>
        /// <param name="csc">数据库配置。</param>
        /// <returns>数据访问接口</returns>
        public static IDBAccess Instance(ConnectionStringConfiguration csc)
        {
            if (csc == null)
                return null;
            EnumDbType dbType = EnumDbType.SqlServer;
            try
            {
                if (!string.IsNullOrEmpty(csc.ProviderName))
                    dbType = (EnumDbType)Enum.Parse(typeof(EnumDbType), csc.ProviderName);
            }
            catch (Exception) { }

            return Instance(csc.ConnectionString, dbType);
        }
        #endregion

        #region 创建数据访问实例
        /// <summary>
        /// 创建数据访问接口实例。
        /// </summary>
        /// <returns></returns>
        protected virtual IDBAccess CreateInstance(EnumDbType dbType, string connString)
        {
            IDBAccess access = null;
            switch (dbType)
            {
                case EnumDbType.SqlServer:
                    access = new SqlDBAccess(connString);
                    break;
                case EnumDbType.OleDb:
                    access = new OleDBAccess(connString);
                    break;
                default:break;
            }
            return access;
        }
        #endregion
    }
}
