//================================================================================
//  FileName: BaseModuleConfiguration.cs
//  Desc:模块默认配置类。
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
using System.IO;
using System.Data;

using iPower;
using iPower.Utility;
using iPower.Data;
using iPower.Data.DataAccess;
using iPower.Configuration;

using iPower.Platform.Logs;
using iPower.Platform.WebPart;
using iPower.Platform.Security;
namespace iPower.Platform.Engine.Persistence
{
    /// <summary>
    /// 模块默认配置键。
    /// </summary>
    public class BaseModuleConfigurationKeys : iPowerConfigurationKeys
    {
        /// <summary>
        /// 获取系统标示键（iPower.SystemID）。
        /// </summary>
        public const string iPower_Const_SystemIdKey = "iPower.SystemID";
        /// <summary>
        /// 获取系统名称键（iPower.SystemName）。
        /// </summary>
        public const string iPower_Const_SystemNameKey = "iPower.SystemName";
        /// <summary>
        /// 获取版权名称键（iPower.CopyRight）。
        /// </summary>
        public const string iPower_Const_CopyRightKey = "iPower.CopyRight";
        /// <summary>
        /// 获取系统默认页面URL键（iPower.MyDefaultURL）。
        /// </summary>
        public const string iPower_Const_MyDefaultURLKey = "iPower.MyDefaultURL";
        /// <summary>
        /// 获取系统默认我的桌面URL键（iPower.MyDesktopURL）。
        /// </summary>
        public const string iPower_Const_MyDesktopURLKey = "iPower.MyDesktopURL";
        /// <summary>
        /// 获取顶部菜单键名(iPower.TopBannerMenus)。
        /// </summary>
        public const string iPower_Const_TopBannerMenus = "iPower.TopBannerMenus";
        /// <summary>
        /// 获取注销页面URL键（iPower.LogoutURL）。
        /// </summary>
        public const string iPower_Const_LogoutURLKey = "iPower.LogoutURL";
        /// <summary>
        /// 获取主菜单地址URI键（iPower.MainMenuDefineConfigFile）。
        /// </summary>
        public const string iPower_Const_MainMenuDefineConfigFileKey = "iPower.MainMenuDefineConfigFile";
        /// <summary>
        /// 获取菜单地址URI键（iPower.ModuleDefineConfigFile）。
        /// </summary>
        public const string iPower_Const_ModuleDefineConfigFileKey = "iPower.ModuleDefineConfigFile";
        /// <summary>
        /// 获取Benner头图片URL及宽度键（iPower.BannerImageUrl）。
        /// </summary>
        public const string iPower_Const_BannerImageUrlKey = "iPower.BannerImageUrl";
        /// <summary>
        /// 获取Logo图片URL及宽度键（iPower.LogoImageUrl）。
        /// </summary>
        public const string iPower_Const_LogoImageUrlKey = "iPower.LogoImageUrl";
        /// <summary>
        /// 特效图片URL键（iPower.EffectImageURL）。
        /// </summary>
        public const string iPower_Const_EffectImageURLKey = "iPower.EffectImageURL";
        /// <summary>
        /// 获取样式表资源键（iPower.WebCssPath）。
        /// </summary>
        public const string iPower_Const_WebCssPathKey = "iPower.WebCssPath";
        /// <summary>
        /// 获取脚本资源键（iPower.WebScriptPath）。
        /// </summary>
        public const string iPower_Const_WebScriptPathKey = "iPower.WebScriptPath";
        /// <summary>
        /// 获取WebPart管理程序集(iPower.WebPartMgrAssembly)。
        /// </summary>
        public const string iPower_Const_WebPartMgrAssemblyKey = "iPower.WebPartMgrAssembly";
        /// <summary>
        /// 获取日志记录程序集(iPower.DbCommonLogAssembly)。
        /// </summary>
        public const string iPower_Const_DbCommonLogAssemblyKey = "iPower.DbCommonLogAssembly";
    }

    /// <summary>
    /// 模块默认配置类。
    /// </summary>
    public class BaseModuleConfiguration : iPowerConfiguration,ISystem,ITopResource
    {
        #region 成员变量，构造函数，析构函数。
        /// <summary>
        /// Hashtable
        /// </summary>
        protected static Hashtable htbCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sectionName">配置节名称。</param>
        public BaseModuleConfiguration(string sectionName)
            : base(sectionName)
        {
        }
        #endregion

        #region ISystem 成员
        /// <summary>
        /// 获取系统标示。
        /// </summary>
        public virtual GUIDEx CurrentSystemID
        {
            get { return new GUIDEx(this[BaseModuleConfigurationKeys.iPower_Const_SystemIdKey]); }
            set { throw new NotSupportedException(); }
        }
        /// <summary>
        /// 获取系统名称。
        /// </summary>
        public virtual string CurrentSystemName
        {
            get { return this[BaseModuleConfigurationKeys.iPower_Const_SystemNameKey]; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region 系统相关属性。
        /// <summary>
        /// 获取主菜单的物理地址。
        /// </summary>
        public virtual string MainMenuDefineConfigFile
        {
            get
            {
                lock (this)
                {
                    string key = string.Format("{0}_MainMenuDefineConfigFile", this.CurrentSystemID);
                    string path = htbCache[key] as string;
                    if (string.IsNullOrEmpty(path))
                    {
                        path = this[BaseModuleConfigurationKeys.iPower_Const_MainMenuDefineConfigFileKey];
                        if (!File.Exists(path))
                        {
                            path = Path.GetFullPath(string.Format(@"{0}\{1}",
                               AppDomain.CurrentDomain.BaseDirectory,
                               path));
                        }
                        if (File.Exists(path))
                        {
                            htbCache[key] = path;
                        }
                    }
                    return path;
                }
            }
        }
        /// <summary>
        /// 获取菜单的物理地址。
        /// </summary>
        public virtual string ModuleDefineConfigFile
        {
            get
            {
                lock (this)
                {
                    string key = string.Format("{0}_ModuleDefineConfigFile", this.CurrentSystemID);
                    string path = htbCache[key] as string;
                    if (string.IsNullOrEmpty(path))
                    {
                        path = this[BaseModuleConfigurationKeys.iPower_Const_ModuleDefineConfigFileKey];
                        if (!File.Exists(path))
                        {
                            path = Path.GetFullPath(string.Format(@"{0}\{1}",
                                   AppDomain.CurrentDomain.BaseDirectory,
                                   path));
                        }
                        if (File.Exists(path))
                        {
                            htbCache[key] = path;
                        }
                    }
                    return path;
                }
            }
        }
        /// <summary>
        /// 获取系统默认页面URL。
        /// </summary>
        public virtual string MyDefaultURL
        {
            get { return this[BaseModuleConfigurationKeys.iPower_Const_MyDefaultURLKey]; }
        }
        /// <summary>
        /// 获取系统我的桌面URL。
        /// </summary>
        public virtual string MyDesktopURL
        {
            get { return this[BaseModuleConfigurationKeys.iPower_Const_MyDesktopURLKey];}
        }
        /// <summary>
        /// 获取顶部菜单集合(规则：Name|Target|Url;)。
        /// </summary>
        public virtual TopBannerMenuCollection TopBannerMenus
        {
            get
            {
                lock (this)
                {
                    string key = string.Format("{0}_TopBannerMenus", this.CurrentSystemID);
                    TopBannerMenuCollection collection = htbCache[key] as TopBannerMenuCollection;
                    if (collection == null || collection.Count == 0)
                    {
                        collection = new TopBannerMenuCollection();
                        #region 我的桌面。
                        string myDeskUrl = this[BaseModuleConfigurationKeys.iPower_Const_MyDesktopURLKey];
                        if (!string.IsNullOrEmpty(myDeskUrl))
                        {
                            TopBannerMenu deskMenu = new TopBannerMenu();
                            deskMenu.Name = "我的桌面";
                            deskMenu.Target = "_self";
                            deskMenu.Url = myDeskUrl;
                            collection.Add(deskMenu);
                        }
                        #endregion

                        #region 添加定义数据。
                        try
                        {
                            string topBannerMenus = this[BaseModuleConfigurationKeys.iPower_Const_TopBannerMenus];
                            if (!string.IsNullOrEmpty(topBannerMenus))
                            {
                                string[] menus = topBannerMenus.Split(';');
                                if (menus != null && menus.Length > 0)
                                {
                                    for (int i = 0; i < menus.Length; i++)
                                    {
                                        string[] arr = menus[i].Split('|');
                                        if (arr != null && arr.Length == 3)
                                        {
                                            TopBannerMenu menu = new TopBannerMenu();
                                            menu.Name = arr[0];
                                            menu.Target = arr[1];
                                            menu.Url = arr[2];
                                            if (!collection.Contains(menu))
                                            {
                                                collection.Add(menu);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception) { }
                        #endregion

                        #region 注销登录。
                        string LogoutURL = this[BaseModuleConfigurationKeys.iPower_Const_LogoutURLKey];
                        if (!string.IsNullOrEmpty(LogoutURL))
                        {
                            TopBannerMenu logoutMenu = new TopBannerMenu();
                            logoutMenu.Name = "注销登录";
                            logoutMenu.Target = "_self";
                            logoutMenu.Url = LogoutURL;
                            collection.Add(logoutMenu);
                        }
                        #endregion

                        if (collection.Count > 0)
                        {
                            htbCache[key] = collection;
                        }
                    }
                    return collection;
                }
            }
        }
        /// <summary>
        /// 获取注销页面URL。
        /// </summary>
        public virtual string LogoutURL
        {
            get { return this[BaseModuleConfigurationKeys.iPower_Const_LogoutURLKey]; }
        }
        #endregion

        #region ITopResource 成员
        /// <summary>
        /// 获取版权名称。
        /// </summary>
        public virtual string CopyRight
        {
            get { return this[BaseModuleConfigurationKeys.iPower_Const_CopyRightKey]; }
            set { }
        }
        #region Top相关
        /// <summary>
        /// 获取Benner头图片路径。
        /// </summary>
        public virtual string[] BannerImageUrl
        {
            get
            {
                string str = this[BaseModuleConfigurationKeys.iPower_Const_BannerImageUrlKey];
                if (!string.IsNullOrEmpty(str))
                    return str.Split(new char[] { ';', ',', ':' });
                return null;
            }
            set { }
        }
        /// <summary>
        /// 获取Logo图片路径。
        /// </summary>
        public virtual string[] LogoImageUrl
        {
            get
            {
                string str = this[BaseModuleConfigurationKeys.iPower_Const_LogoImageUrlKey];
                if (!string.IsNullOrEmpty(str))
                    return str.Split(new char[] { ';', ',', ':' });
                return null;
            }
            set { }
        }
        /// <summary>
        /// 获取特效图片URL。
        /// </summary>
        public virtual string EffectImageURL
        {
            get { return this[BaseModuleConfigurationKeys.iPower_Const_EffectImageURLKey]; }
            set { }
        }
        #endregion

        #region 资源属性。
        /// <summary>
        /// 获取样式表资源集合。
        /// </summary>
        public virtual string[] WebCssPaths
        {
            get
            {
                string str = this[BaseModuleConfigurationKeys.iPower_Const_WebCssPathKey];
                if (!string.IsNullOrEmpty(str))
                    return str.Split(new char[] { ';', ',', ':' });
                return null;
            }
            set { }
        }
        /// <summary>
        /// 获取脚本资源集合。
        /// </summary>
        public virtual string[] WebScriptPaths
        {
            get
            {
                string str = this[BaseModuleConfigurationKeys.iPower_Const_WebScriptPathKey];
                if (!string.IsNullOrEmpty(str))
                    return str.Split(new char[] { ';', ',', ':' });
                return null;
            }
            set { }
        }
        #endregion

        #endregion

        #region 模块实例。
        /// <summary>
        /// 获取默认数据库连接实体。
        /// </summary>
        public virtual IDBAccess ModuleDefaultDatabase
        {
            get
            {
                lock (this)
                {
                    string key = string.Format("{0}_ModuleDefaultDatabase", this.CurrentSystemID);
                    IDBAccess access = htbCache[key] as IDBAccess;
                    if (access == null)
                    {
                        ConnectionStringConfiguration conn = this.DefaultDataConnectionString;
                        if (conn == null) return null;
                        EnumDbType dbType = EnumDbType.SqlServer;
                        if (!string.IsNullOrEmpty(conn.ProviderName))
                        {
                            dbType = (EnumDbType)Enum.Parse(typeof(EnumDbType), conn.ProviderName, true);
                        }
                        if ((access = DatabaseFactory.Instance(conn.ConnectionString, dbType)) != null)
                        {
                            htbCache[key] = access;
                        }
                    }
                    return access;
                }
            }
        }
        /// <summary>
        /// 获取WebPart管理接口。
        /// </summary>
        public virtual IWebPartMgr WebPartMgrAssembly
        {
            get
            {
                lock (this)
                {
                    string key = string.Format("{0}_WebPartMgrAssembly", this.CurrentSystemID);
                    IWebPartMgr oWebPartMgr = htbCache[key] as IWebPartMgr;
                    if (oWebPartMgr == null)
                    {
                        string assembly = this[BaseModuleConfigurationKeys.iPower_Const_WebPartMgrAssemblyKey];
                        if (!string.IsNullOrEmpty(assembly))
                        {
                            if((oWebPartMgr = TypeHelper.Create(assembly) as IWebPartMgr) != null)
                            {
                                htbCache[key] = oWebPartMgr;
                            }
                        }
                    }
                    return oWebPartMgr;
                }
            }
        }
        /// <summary>
        /// 获取记录日志程序集。
        /// </summary>
        public virtual ICreateDbCommonLog DbCommonLogAssembly
        {
            get
            {
                lock (this)
                {
                    string key = string.Format("{0}_DbCommonLogAssembly", this.CurrentSystemID);
                    ICreateDbCommonLog log = htbCache[key] as ICreateDbCommonLog;
                    if (log == null)
                    {
                        string assembly = this[BaseModuleConfigurationKeys.iPower_Const_DbCommonLogAssemblyKey];
                        if (!string.IsNullOrEmpty(assembly))
                        {
                            if((log = TypeHelper.Create(assembly) as ICreateDbCommonLog) != null)
                            {
                                htbCache[key] = log;
                            }
                        }
                    }
                    return log;
                }
            }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 获取菜单配置数据。
        /// </summary>
        /// <param name="file">菜单文件物理地址。</param>
        /// <param name="systemID">系统ID。</param>
        /// <returns></returns>
        internal virtual ModuleSystemDefine GetModuleDefineConfig(string file, GUIDEx systemID)
        {
            lock (this)
            {
                ModuleSystemDefine systemDefine = null;
                if (File.Exists(file))
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        ModuleDefineFactory factory = ModuleDefineFactory.DeSerializer(fs);
                        fs.Close();
                        if (factory != null)
                        {
                            if (systemID.IsValid)
                                systemDefine = factory.System[systemID];
                            else
                                systemDefine = factory.System[0];
                        }
                    }
                }
                return systemDefine;
            }
        }
        #endregion 
    }

    #region 顶部菜单类。
    /// <summary>
    /// 顶部菜单。
    /// </summary>
    [Serializable]
    public class TopBannerMenu
    {
        /// <summary>
        /// 获取或设置菜单名称。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置链接方式。
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 获取或设置URL地址。
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 获取或设置排序。
        /// </summary>
        public int Order { get; set; }
    }
    /// <summary>
    /// 顶部菜单集合。
    /// </summary>
    [Serializable]
    public class TopBannerMenuCollection : DataCollection<TopBannerMenu>
    {
        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Contains(TopBannerMenu item)
        {
            if (item != null)
            {
                TopBannerMenu topBannerMenu = this.Items.Find(new Predicate<TopBannerMenu>(delegate(TopBannerMenu data)
                {
                    return (data != null) && (data.Name == item.Name);
                }));
                return topBannerMenu != null;
            }
            return base.Contains(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void Add(TopBannerMenu item)
        {
            if (item != null)
            {
                if (item.Order == 0)
                {
                    item.Order = this.Count + 1;
                }
                base.Add(item);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override int Compare(TopBannerMenu x, TopBannerMenu y)
        {
            return x.Order - y.Order;
        }
        #endregion
    }
    #endregion
}
