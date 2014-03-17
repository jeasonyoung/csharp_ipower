//================================================================================
//  FileName: JeasonConfiguration.cs
//  Desc:获取配置类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-28
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
using System.Collections.Specialized;
using System.Configuration;
namespace iPower.Configuration
{
    /// <summary>
    /// 默认数据库连接接口。
    /// </summary>
    public interface IDefaultDataConnection
    {
        /// <summary>
        /// 默认数据库连接字符串。
        /// </summary>
        ConnectionStringConfiguration DefaultDataConnectionString
        {
            get;
        }
    }

    /// <summary>
    /// 获取配置类。
    /// </summary>
    public class iPowerConfiguration : iPowerAbstractConfiguration<iPowerSection>, IDefaultDataConnection
    {
        #region 构造函数,析构函数,成员变量。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sectionName">配置节名。</param>
        public iPowerConfiguration(string sectionName)
            : base(sectionName)
        {
        }      
        #endregion

        #region 属性
        /// <summary>
        /// 获取默认数据库连接字符串(Jeason.DbConnectionString)。
        /// </summary>
        public ConnectionStringConfiguration DefaultDataConnectionString
        {
            get
            {
                return this.LoadConnectionConfiguration(iPowerConfigurationKeys.iPower_DbConnectionStringKey);
            }
        }
        #endregion

        #region 辅助函数.
        /// <summary>
        /// 加载数据库连接配置.
        /// </summary>
        /// <param name="key">配置键名.</param>
        /// <returns>数据库连接配置对象.</returns>
        protected ConnectionStringConfiguration LoadConnectionConfiguration(string key)
        {
            ConnectionStringConfiguration conn = null;
            if (!string.IsNullOrEmpty(key))
            {
                string key_value = this[key];
                if (!string.IsNullOrEmpty(key_value))
                {
                    if (this.ConnectionStrings != null && this.ConnectionStrings.Count > 0)
                    {
                        conn = this.ConnectionStrings[key_value];
                    }
                    if (conn == null)
                    {
                        conn = new ConnectionStringConfiguration("Default", "SQLServer", key_value);
                    }
                }
            }
            return conn;
        }
        #endregion
    }
}
