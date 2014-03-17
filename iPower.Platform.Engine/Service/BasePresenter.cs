//================================================================================
//  FileName: BasePresenter.cs
//  Desc:行为抽象类。
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
using iPower.Resources;
using iPower.Configuration;

using iPower.Platform;
using iPower.Platform.Security;
using iPower.Platform.Engine.Persistence;
using iPower.Platform.Engine.DataSource;
namespace iPower.Platform.Engine.Service
{
    /// <summary>
    /// 行为抽象类。
    /// </summary>
    public abstract partial class BasePresenter<T, K> : IServiceContainer
        where T : IBaseView
        where K : BaseModuleConfiguration
    {
        #region 成员变量，构造函数。
        T theView;
        K theModule;
        static SortedList localServices;
        static Hashtable htbCache;
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static BasePresenter()
        {
            htbCache = Hashtable.Synchronized(new Hashtable());
            localServices = SortedList.Synchronized(new SortedList());
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="view">界面接口。</param>
        public BasePresenter(T view)
        {
            this.theView = view;

            this.theModule = this.CreateModuleConfiguration();
            this.AddServiceToContainer(this);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取界面接口。
        /// </summary>
        protected T View
        {
            get { return this.theView; }
        }
        /// <summary>
        /// 获取模块配置。
        /// </summary>
        protected K ModuleConfig
        {
            get { return this.theModule; }
        }
        /// <summary>
        /// 获取缓存。
        /// </summary>
        protected Hashtable Cache
        {
            get { return htbCache; }
        }
        /// <summary>
        /// 获取导航分隔符。
        /// </summary>
        protected virtual string NavigationSeperator
        {
            get
            {
                return ">>";
            }
        }
        /// <summary>
        /// 获取主菜单定义配置。
        /// </summary>
        protected virtual ModuleDefineCollection MainMenuDefineConfig
        {
            get
            {
                lock (this)
                {
                    if (this.View != null)
                    {
                        string strKey = "MainMenuDefineConfig";
                        ModuleDefineCollection mdc = this.Cache[strKey] as ModuleDefineCollection;
                        if (mdc == null)
                        {
                            ModuleSystemDefine sysDefine = this.ModuleConfig.GetModuleDefineConfig(this.ModuleConfig.MainMenuDefineConfigFile, GUIDEx.Null);
                            if (sysDefine != null)
                                mdc = sysDefine.Modules;
                            if (mdc != null)
                                this.Cache[strKey] = mdc;
                        }
                        return mdc;
                    }
                    return null;
                }
            }
        }
        /// <summary>
        /// 获取模块菜单定义配置。
        /// </summary>
        protected virtual ModuleDefineCollection ModuleDefineConfig
        {
            get
            {
                lock (this)
                {
                    if (this.View != null && this.View.CurrentSystemID.IsValid)
                    {
                        string strKey = string.Format("ModuleDefineConfig_{0}", this.View.CurrentSystemID);
                        ModuleDefineCollection mdc = this.Cache[strKey] as ModuleDefineCollection;
                        if (mdc == null)
                        {
                            ModuleSystemDefine sysDefine = this.ModuleConfig.GetModuleDefineConfig(this.ModuleConfig.ModuleDefineConfigFile, this.View.CurrentSystemID);
                            if (sysDefine != null)
                                mdc = sysDefine.Modules;
                            if (mdc != null)
                                this.Cache[strKey] = mdc;
                        }
                        return mdc;
                    }
                    return null;
                }
            }
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 初始化数据加载事件。
        /// </summary>
        public event EventHandler InitComponent;
        /// <summary>
        /// 触发<see cref="InitComponent">InitComponent</see>事件。
        /// </summary>
        /// <param name="e"></param>
        protected void OnInitComponent(EventArgs e)
        {
            EventHandler handler = this.InitComponent;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 在ViewLoad前发生。
        /// </summary>
        public event EventHandler PreViewLoad;
        /// <summary>
        /// 触发<see cref="PreViewLoad">PreViewLoad</see>事件。
        /// </summary>
        /// <param name="e"></param>
        protected void OnPreViewLoad(EventArgs e)
        {
            EventHandler handler = this.PreViewLoad;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 在ViewLoad后发生。
        /// </summary>
        public event EventHandler LastViewLoad;
        /// <summary>
        /// 触发<see cref="LastViewLoad">LastViewLoad</see>事件。
        /// </summary>
        /// <param name="e"></param>
        protected void OnLastViewLoad(EventArgs e)
        {
            EventHandler handler = this.LastViewLoad;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region 抽象函数。
        /// <summary>
        /// 创建模块配置对象。
        /// </summary>
        /// <returns>配置类实体。</returns>
        protected abstract K CreateModuleConfiguration();
        #endregion

        #region 初始化数据加载。
        /// <summary>
        /// 初始化数据加载。
        /// </summary>
        public virtual void InitializeComponent()
        {
            this.LoadResourceConfiguration();
            this.OnInitComponent(EventArgs.Empty);
            this.PreViewLoadData();
            this.View.LoadData();
            this.LoadLastData();
        }
        #endregion

        #region 保护性函数。
        /// <summary>
        /// 添加服务到服务容器。
        /// </summary>
        /// <returns></returns>
        protected virtual void AddServiceToContainer(IServiceContainer container)
        {
            if (container != null)
            {
                //枚举。
                if (!container.HasService(typeof(CommonEnumsEntity<>)))
                    container.AddService(typeof(CommonEnumsEntity<>), new CommonEnumsEntity<K>(this.ModuleConfig));
            }
        }
        /// <summary>
        /// 资源配置数据。
        /// </summary>
        protected virtual void LoadResourceConfiguration()
        {
            if (this.View != null)
            {
                #region 系统信息。
                ISystem system = this.ModuleConfig as ISystem;//this.ModuleConfig as ISystem;
                if (system != null)
                {
                    this.View.CurrentSystemID = system.CurrentSystemID;
                    this.View.CurrentSystemName = system.CurrentSystemName;
                }
                #endregion

                #region 系统资源数据。
                ITopResource resource = this.ModuleConfig as ITopResource;//this.ModuleConfig as ITopResource;
                if (resource != null)
                {
                    this.View.CopyRight = resource.CopyRight;
                    this.View.BannerImageUrl = resource.BannerImageUrl;
                    this.View.EffectImageURL = resource.EffectImageURL;
                    this.View.LogoImageUrl = resource.LogoImageUrl;
                    this.View.BannerImageUrl = resource.BannerImageUrl;
                    this.View.WebCssPaths = resource.WebCssPaths;
                    this.View.WebScriptPaths = resource.WebScriptPaths;
                }
                #endregion

                #region 菜单相关。
                //主菜单。
                ModuleDefineCollection main = this.MainMenuDefineConfig;
                if (main != null)
                {
                    if (this.View is IMenuData)
                        ((IMenuData)this.View).MainMenuData = main;
                }
                //菜单。
                ModuleDefineCollection mdc = this.ModuleDefineConfig;
                if (mdc != null)
                {
                    if (this.View is IMenuData)
                        ((IMenuData)this.View).MenuData = mdc;
                    if (!string.IsNullOrEmpty(this.View.CurrentFolderID))
                    {
                        ModuleDefine moduleDefine = mdc[this.View.CurrentFolderID];
                        if (moduleDefine != null)
                            this.View.CurrentModuleTitle = moduleDefine.ModuleName;
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// 在View的LoadData前加载。
        /// </summary>
        protected virtual void PreViewLoadData()
        {
            this.OnPreViewLoad(EventArgs.Empty);
        }
        /// <summary>
        /// 最后加载的数据。
        /// </summary>
        protected virtual void LoadLastData()
        {
            this.OnLastViewLoad(EventArgs.Empty);
            this.View.NavigationContent = this.RenderNavigation(this.NavigationSeperator);
        }
        /// <summary>
        /// 获取枚举数据。
        /// </summary>
        /// <param name="type">枚举类型。</param>
        /// <param name="Value">枚举名称。</param>
        /// <returns>名称数据。</returns>
        public virtual string GetEnumMemberName(Type type, string Value)
        {
            CommonEnumsEntity<K> entity = this.GetService(typeof(CommonEnumsEntity<>)) as CommonEnumsEntity<K>;
            if (entity != null)
                return entity.GetEnumMemberName(type, Value);
            return string.Empty;
        }
        /// <summary>
        /// 获取枚举数据。
        /// </summary>
        /// <param name="type">枚举类型。</param>
        /// <param name="Value">值。</param>
        /// <returns>名称数据。</returns>
        public virtual string GetEnumMemberName(Type type, int Value)
        {
            CommonEnumsEntity<K> entity = this.GetService(typeof(CommonEnumsEntity<>)) as CommonEnumsEntity<K>;
            if (entity != null)
                return entity.GetEnumMemberName(type, Value);
            return string.Empty;
        }
        /// <summary>
        /// 获取枚举数据值。
        /// </summary>
        /// <param name="type">枚举类型。</param>
        /// <param name="Value">名称数据。</param>
        /// <returns>值。</returns>
        public virtual int GetEnumMemberIntValue(Type type, string Value)
        {
            CommonEnumsEntity<K> entity = this.GetService(typeof(CommonEnumsEntity<>)) as CommonEnumsEntity<K>;
            if (entity != null)
                return entity.GetEnumMemberIntValue(type, Value);
            return -1;
        }

        /// <summary>
        /// 绑定枚举数据源。
        /// </summary>
        /// <param name="type">枚举类。</param>
        /// <returns></returns>
        public virtual IListControlsData EnumDataSource(Type type)
        {
            if (type == null)
                return null;
            return new ConstListControlsDataSource<K>(type, this.ModuleConfig);
        } 
        #endregion
        
        #region IServiceContainer 成员。
        /// <summary>
        /// 检查服务容器中服务类型是否存在。
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public bool HasService(Type serviceType)
        {
            return (serviceType != null) && (BasePresenter<T, K>.localServices.ContainsKey(serviceType.FullName));
        }
        /// <summary>
        /// 将指定的服务添加到服务容器中。
        /// </summary>
        /// <param name="serviceType">要添加的服务类型。</param>
        /// <param name="callback">用于创建服务的回调对象。这允许将服务声明为可用，但将对象的创建延迟到请求该服务之后。</param>
        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            if (serviceType != null && callback != null)
                BasePresenter<T, K>.localServices[serviceType.FullName] = callback;
        }
        /// <summary>
        /// 将指定的服务添加到服务容器中。
        /// </summary>
        /// <param name="serviceType">要添加的服务类型。</param>
        /// <param name="serviceInstance">要添加的服务类型的实例。此对象必须实现 serviceType 参数指示的类型或从其继承。</param>
        public void AddService(Type serviceType, object serviceInstance)
        {
            if (serviceType != null && serviceInstance != null)
                BasePresenter<T, K>.localServices[serviceType.FullName] = serviceInstance;
        }
        /// <summary>
        /// 从服务容器中移除指定的服务类型。
        /// </summary>
        /// <param name="serviceType">要移除的服务类型。</param>
        public void RemoveService(Type serviceType)
        {
            if (serviceType != null && BasePresenter<T, K>.localServices.ContainsKey(serviceType.FullName))
                BasePresenter<T, K>.localServices.Remove(serviceType.FullName);
        }

        #endregion

        #region IServiceProvider 成员
        /// <summary>
        /// 获取指定类型的服务对象。
        /// </summary>
        /// <param name="serviceType">一个对象，它指定要获取的服务对象的类型。</param>
        /// <returns>serviceType 类型的服务对象。- 或 - 如果没有 serviceType 类型的服务对象，则为 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing）。</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                return null;
            object serviceInstance = BasePresenter<T, K>.localServices[serviceType.FullName];
            if (serviceInstance == null)
                return null;
            if (serviceInstance.GetType() == typeof(ServiceCreatorCallback))
                return ((ServiceCreatorCallback)serviceInstance)(this, serviceType);
            return serviceInstance;
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 绘制导航数据。
        /// </summary>
        /// <param name="seperator">分隔符。</param>
        /// <returns></returns>
        protected virtual string RenderNavigation(string seperator)
        {
            StringBuilder builder = new StringBuilder();
            GUIDEx currentFolderID = this.View.CurrentFolderID;
            if (!currentFolderID.IsValid)
                currentFolderID = this.View.SecurityID;
            ModuleDefine moduleDefine = this.View.MenuData == null ? null : this.View.MenuData[currentFolderID];
            if (moduleDefine != null)
            {
                Stack<string> stack = new Stack<string>();
                this.PushModuleDefine(ref stack, moduleDefine);
                int i = 0;
                foreach (string strName in stack)
                {
                    builder.AppendFormat("{0}{1}", i++ == 0 ? string.Empty : seperator, strName);
                }
            }
            return builder.ToString();
        }
        void PushModuleDefine(ref Stack<string> stack, ModuleDefine define)
        {
            if (define != null)
            {
                stack.Push(define.ModuleName);
                if (define.Parent != null)
                    this.PushModuleDefine(ref stack, define.Parent);
            }
        }
        #endregion
    }
}
