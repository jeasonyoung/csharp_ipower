//================================================================================
//  FileName:TopBannerPresenter.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-13 09:35:19
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
using System.Collections.Generic;
using System.Text;

using iPower;
using iPower.Utility;
using iPower.Platform.Engine.Persistence;
namespace iPower.Platform.Engine.Service
{
    /// <summary>
    /// 默认Banner头视图。
    /// </summary>
    public interface ITopBannerView : IBaseView
    {
        /// <summary>
        /// 获取或设置顶部菜单集合。
        /// </summary>
        TopBannerMenuCollection TopBannerMenus
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 默认Banner头行为类。
    /// </summary>
    public class TopBannerPresenter<K> : BasePresenter<ITopBannerView, K>
        where K : BaseModuleConfiguration,new()
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="view">默认Banner头视图。</param>
        public TopBannerPresenter(ITopBannerView view)
            : base(view)
        {
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 加载资源配置。
        /// </summary>
        protected override void LoadResourceConfiguration()
        {
            base.LoadResourceConfiguration();
            ITopBannerView view = this.View as ITopBannerView;
            if (view != null)
                view.TopBannerMenus = this.ModuleConfig.TopBannerMenus;
        }
        /// <summary>
        /// 创建模块配置对象。
        /// </summary>
        /// <returns></returns>
        protected override K CreateModuleConfiguration()
        {
            return new K();
        }
        /// <summary>
        /// 数据加载。
        /// </summary>
        protected override void LoadLastData()
        {
            base.LoadLastData();

        }
        #endregion
    }
}
