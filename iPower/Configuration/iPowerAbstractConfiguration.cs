//================================================================================
//  FileName: JeasonAbstractConfiguration.cs
//  Desc:获取自定义配置节的基础类。
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
using System.IO;
using System.Collections.Specialized;
using System.Configuration;
using iPower.Utility;
namespace iPower.Configuration
{
    /// <summary>
    /// 获取自定义配置节的基础类。
    /// </summary>
    /// <typeparam name="T">自定义的配置节类（ConfigurationSection的子类）。</typeparam>
    public abstract class iPowerAbstractConfiguration<T> where T : ConfigurationSection
    {
        #region 成员变量，构造函数。
        NameValueCollection collection = null, appSettings = null;
        ConnectionStringConfigurationCollection connectionStrings = null;
        /// <summary>
        /// 自定义的配置节类。
        /// </summary>
        protected T Section;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sectionName">配置节的名称。</param>
        protected iPowerAbstractConfiguration(string sectionName)
        {
            this.Section = this.CreateConfiguration(sectionName, out this.appSettings, out this.connectionStrings);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        protected iPowerAbstractConfiguration()
            : this(string.Empty)
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取键值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return this.GetKeyValue(key);
            }
        }
        /// <summary>
        /// 获取key-value配置节。
        /// </summary>
        public virtual NameValueCollection Settings
        {
            get
            {
                iPowerSection section = this.Section as iPowerSection;
                if (this.collection == null && section != null)
                {
                    this.collection = new NameValueCollection();
                    foreach (KeyValueConfigurationElement element in section.Settings)
                        this.collection.Add(element.Key, element.Value);
                }
                return this.collection;
            }
        }
        /// <summary>
        /// 默认配置节。
        /// </summary>
        protected NameValueCollection AppSettings
        {
            get { return this.appSettings; }
        }
        /// <summary>
        /// 默认数据库连接字符串集合。
        /// </summary>
        protected ConnectionStringConfigurationCollection ConnectionStrings
        {
            get { return this.connectionStrings; }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 获取配置节键值对。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>值。</returns>
        protected virtual string GetKeyValue(string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                if (this.Settings != null)
                    result = this.Settings[key];

                if (string.IsNullOrEmpty(result) && this.AppSettings != null)
                    result = this.AppSettings[key];
            }
            return result;
        }
        /// <summary>
        ///  创建获取配置对象。
        /// </summary>
        /// <param name="sectionName">配置节名称。</param>
        /// <param name="appSettings">默认配置节数据。</param>
        /// <param name="connectionStrings">默认数据连接数据。</param>
        /// <returns></returns>
        protected virtual T CreateConfiguration(string sectionName, out NameValueCollection appSettings, out ConnectionStringConfigurationCollection connectionStrings)
        {
            T result = default(T);
            appSettings = ConfigurationManager.AppSettings;
            connectionStrings = new ConnectionStringConfigurationCollection();
            ConnectionStringSettingsCollection cssc = ConfigurationManager.ConnectionStrings;

            bool isExt = false;
            string extConfigPath = appSettings["ExtCofigFile"];
            if (!string.IsNullOrEmpty(extConfigPath))
            {
                if (!File.Exists(extConfigPath))
                {
                    extConfigPath = Path.GetFullPath(string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, extConfigPath));
                }
                if (File.Exists(extConfigPath))
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = extConfigPath;

                    System.Configuration.Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    if (isExt = (cfg != null))
                    {
                        cssc = cfg.ConnectionStrings.ConnectionStrings;
                        KeyValueConfigurationCollection collection = cfg.AppSettings.Settings;
                        if (collection != null && collection.Count > 0)
                        {
                            appSettings = new NameValueCollection();
                            foreach (KeyValueConfigurationElement kv in collection)
                            {
                                appSettings.Add(kv.Key, kv.Value);
                            }
                        }
                        //
                        if (!string.IsNullOrEmpty(sectionName))
                        {
                            result = cfg.GetSection(sectionName) as T;
                        }
                    }
                }
            }
            if (cssc != null && cssc.Count > 0)
            {
                foreach (ConnectionStringSettings c in cssc)
                {
                    connectionStrings.Add(new ConnectionStringConfiguration(c.Name, c.ProviderName, c.ConnectionString));
                }
            }
            if (!isExt && !string.IsNullOrEmpty(sectionName))
            {
                //object obj = System.Web.HttpContext.Current.GetSection(sectionName);
                //if (obj != null)
                //{
                //    return obj as T;
                //}
                return ConfigurationManager.GetSection(sectionName) as T;
            }
            return result;
        }
        #endregion
    }
}
