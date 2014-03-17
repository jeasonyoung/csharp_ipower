//================================================================================
//  FileName: JeasonAbstractConfiguration.cs
//  Desc:��ȡ�Զ������ýڵĻ����ࡣ
//
//  Called by
//
//  Auth:���£�jeason1914@gmail.com��
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
    /// ��ȡ�Զ������ýڵĻ����ࡣ
    /// </summary>
    /// <typeparam name="T">�Զ�������ý��ࣨConfigurationSection�����ࣩ��</typeparam>
    public abstract class iPowerAbstractConfiguration<T> where T : ConfigurationSection
    {
        #region ��Ա���������캯����
        NameValueCollection collection = null, appSettings = null;
        ConnectionStringConfigurationCollection connectionStrings = null;
        /// <summary>
        /// �Զ�������ý��ࡣ
        /// </summary>
        protected T Section;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="sectionName">���ýڵ����ơ�</param>
        protected iPowerAbstractConfiguration(string sectionName)
        {
            this.Section = this.CreateConfiguration(sectionName, out this.appSettings, out this.connectionStrings);
        }
        /// <summary>
        /// ���캯����
        /// </summary>
        protected iPowerAbstractConfiguration()
            : this(string.Empty)
        {
        }
        #endregion

        #region ���ԡ�
        /// <summary>
        /// ��ȡ��ֵ��
        /// </summary>
        /// <param name="key">������</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return this.GetKeyValue(key);
            }
        }
        /// <summary>
        /// ��ȡkey-value���ýڡ�
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
        /// Ĭ�����ýڡ�
        /// </summary>
        protected NameValueCollection AppSettings
        {
            get { return this.appSettings; }
        }
        /// <summary>
        /// Ĭ�����ݿ������ַ������ϡ�
        /// </summary>
        protected ConnectionStringConfigurationCollection ConnectionStrings
        {
            get { return this.connectionStrings; }
        }
        #endregion

        #region ������
        /// <summary>
        /// ��ȡ���ýڼ�ֵ�ԡ�
        /// </summary>
        /// <param name="key">����</param>
        /// <returns>ֵ��</returns>
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
        ///  ������ȡ���ö���
        /// </summary>
        /// <param name="sectionName">���ý����ơ�</param>
        /// <param name="appSettings">Ĭ�����ý����ݡ�</param>
        /// <param name="connectionStrings">Ĭ�������������ݡ�</param>
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
