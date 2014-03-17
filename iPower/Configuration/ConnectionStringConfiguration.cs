//================================================================================
//  FileName:ConnectionString.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-10 11:44:25
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

namespace iPower.Configuration
{
    /// <summary>
    /// 表示连接字符串配置文件节中的命名连接字符串的集合。
    /// </summary>
    public class ConnectionStringConfigurationCollection : ICollection<ConnectionStringConfiguration>
    {
        #region 成员变量，构造函数。
        private Dictionary<string, ConnectionStringConfiguration> dic;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ConnectionStringConfigurationCollection()
        {
            this.dic = new Dictionary<string, ConnectionStringConfiguration>();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="item">表示连接字符串配置文件节中的单个命名连接字符串。</param>
        public ConnectionStringConfigurationCollection(ConnectionStringConfiguration item)
            : this()
        {
            this.Add(item);
        }
        #endregion

        #region 索引。
        /// <summary>
        /// 获取表示连接字符串配置文件节中的单个命名连接字符串。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>表示连接字符串配置文件节中的单个命名连接字符串。</returns>
        public ConnectionStringConfiguration this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name)) return null;
                if (this.dic.ContainsKey(name))
                    return this.dic[name];
                return null;
            }
        }
        #endregion
        
        #region ICollection<ConnectionStringConfiguration> 成员
        /// <summary>
        /// 添加表示连接字符串配置文件节中的单个命名连接字符串。
        /// </summary>
        /// <param name="item">表示连接字符串配置文件节中的单个命名连接字符串。</param>
        public void Add(ConnectionStringConfiguration item)
        {
            if (item != null)
            {
                this.dic[item.Name] = item;
            }
        }
        /// <summary>
        /// 清除全部表示连接字符串配置文件节中的单个命名连接字符串。
        /// </summary>
        public void Clear()
        {
            this.dic.Clear();
        }
        /// <summary>
        /// 判断是否存在表示连接字符串配置文件节中的单个命名连接字符串。
        /// </summary>
        /// <param name="item">表示连接字符串配置文件节中的单个命名连接字符串。</param>
        /// <returns>存在返回True，否则返回False。</returns>
        public bool Contains(ConnectionStringConfiguration item)
        {
            if (item == null) return false;
            return this.dic.ContainsKey(item.Name);
        }
        /// <summary>
        /// 复制到数组。
        /// </summary>
        /// <param name="array">数组。</param>
        /// <param name="arrayIndex">下标。</param>
        public void CopyTo(ConnectionStringConfiguration[] array, int arrayIndex)
        {
            this.dic.Values.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 获取集合中的数量。
        /// </summary>
        public int Count
        {
            get { return this.dic.Count; }
        }
        /// <summary>
        /// 获取是否只读。
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 移除指定表示连接字符串配置文件节中的单个命名连接字符串。
        /// </summary>
        /// <param name="item">表示连接字符串配置文件节中的单个命名连接字符串。</param>
        /// <returns>成功返回True，否则返回False。</returns>
        public bool Remove(ConnectionStringConfiguration item)
        {
            if (item != null && this.dic.ContainsKey(item.Name))
            {
                return this.dic.Remove(item.Name);
            }
            return false;
        }

        #endregion

        #region IEnumerable<ConnectionStringConfiguration> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ConnectionStringConfiguration> GetEnumerator()
        {
            return this.dic.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (ConnectionStringConfiguration c in this.dic.Values)
                yield return c;
        }

        #endregion
    }
    /// <summary>
    /// 表示连接字符串配置文件节中的单个命名连接字符串。
    /// </summary>
    public class ConnectionStringConfiguration
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="providerName">提供程序名称属性。</param>
        /// <param name="connectionString">连接字符串。</param>
        public ConnectionStringConfiguration(string name, string providerName, string connectionString)
        {
            this.Name = name;
            this.ProviderName = providerName;
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 获取或设置名称。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置提供程序名称属性。
        /// </summary>
        public string ProviderName { get; internal set; }
        /// <summary>
        /// 获取或设置连接字符串。
        /// </summary>
        public string ConnectionString { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name + "," + this.ProviderName + "," + this.ConnectionString;
        }
    }
}
