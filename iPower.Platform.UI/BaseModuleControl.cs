//================================================================================
//  FileName: BaseModuleControl.cs
//  Desc:用户控件基类。
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
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using iPower;
using iPower.Platform;
using iPower.Platform.Security;
using iPower.Platform.Engine.DataSource;
using iPower.Platform.Engine.Service;
namespace iPower.Platform.UI
{
    /// <summary>
    /// 用户控件基类。
    /// </summary>
    public partial class BaseModuleControl : UserControl, IBaseView
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BaseModuleControl()
            : base()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取页面对象。
        /// </summary>
        protected IBaseModulePage ModulePage
        {
            get
            {
                return this.Page as IBaseModulePage;
            }
        }
        #endregion

        #region IUser 成员
        /// <summary>
        ///  获取当前用户标示。
        /// </summary>
        public GUIDEx CurrentUserID
        {
            get
            {
                return this.ModulePage == null ? GUIDEx.Null : this.ModulePage.CurrentUserID;
            }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 获取当前用户名称。
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.CurrentUserName;
            }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region IDataHandler 成员
        /// <summary>
        /// 数据加载。
        /// </summary>
        public virtual void LoadData()
        {

        }
        /// <summary>
        /// 数据保存。
        /// </summary>
        /// <returns>是否保存成功。</returns>
        public virtual bool SaveData()
        {
            return true;
        }
        /// <summary>
        /// 数据删除。
        /// </summary>
        /// <returns>是否成功删除数据。</returns>
        public bool DeleteData()
        {
            return true;
        }

        #endregion

        #region IUISettings 成员
        /// <summary>
        /// 获取当前目录标示。
        /// </summary>
        public string CurrentFolderID
        {
            get
            {
                return this.ModulePage == null ? string.Empty : this.ModulePage.CurrentFolderID;
            }
            set { }
        }
        /// <summary>
        /// 获取当前模块标题。
        /// </summary>
        public string CurrentModuleTitle
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.CurrentModuleTitle;
            }
            set { }
        }

        #endregion

        #region ITopResource 成员
        /// <summary>
        ///获取版权名称。
        /// </summary>
        public string CopyRight
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.CopyRight;
            }
            set { }
        }
        /// <summary>
        /// 获取Benner头图片路径。
        /// </summary>
        public string[] BannerImageUrl
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.BannerImageUrl;
            }
            set { }
        }
        /// <summary>
        /// 获取Logo图片路径。
        /// </summary>
        public string[] LogoImageUrl
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.LogoImageUrl;
            }
            set { }
        }
        /// <summary>
        /// 获取特效图片URL。
        /// </summary>
        public string EffectImageURL
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.EffectImageURL;
            }
            set { }
        }
        /// <summary>
        /// 获取样式表资源集合。
        /// </summary>
        public string[] WebCssPaths
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.WebCssPaths;
            }
            set { }
        }
        /// <summary>
        /// 获取脚本资源集合。
        /// </summary>
        public string[] WebScriptPaths
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.WebScriptPaths;
            }
            set { }
        }

        #endregion

        #region ISecurity 成员
        /// <summary>
        /// 获取系统安全标示。
        /// </summary>
        public GUIDEx SecurityID
        {
            get
            {
                return this.ModulePage == null ? GUIDEx.Null : this.ModulePage.SecurityID;
            }
            set {  }
        }
        #endregion

        #region ISystem 成员
        /// <summary>
        /// 获取当前系统ID。
        /// </summary>
        public GUIDEx CurrentSystemID
        {
            get
            {
                return this.ModulePage == null ? GUIDEx.Null : this.ModulePage.CurrentSystemID;
            }
            set { }
        }
        /// <summary>
        /// 获取当前系统名称。
        /// </summary>
        public string CurrentSystemName
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.CurrentSystemName;
            }
            set { }
        }

        #endregion

        #region IMenuData 成员
        /// <summary>
        /// 获取菜单数据。
        /// </summary>
        public ModuleDefineCollection MenuData
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.MenuData;
            }
            set { }
        }
        /// <summary>
        /// 获取主菜单数据。
        /// </summary>
        public ModuleDefineCollection MainMenuData
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.MainMenuData;
            }
            set { }
        }
        #endregion

        #region ISecurity 成员
        /// <summary>
        ///验证权限。
        /// </summary>
        /// <param name="permissions">权限集合。</param>
        public virtual void VerifyPermissions(SecurityPermissionCollection permissions)
        {
            ISecurity serv = this.Page as ISecurity;
            if (serv != null)
                serv.VerifyPermissions(permissions);
        }

        #endregion

        #region IBaseView 成员
        /// <summary>
        /// 获取导航数据。
        /// </summary>
        public string NavigationContent
        {
            get
            {
                return this.ModulePage == null ? null : this.ModulePage.NavigationContent;
            }
            set { }
        }

        #endregion
    }

    /// <summary>
    /// 数据绑定。
    /// </summary>
    partial class BaseModuleControl
    {
        /// <summary>
        /// 列表类型控件数据绑定。
        /// </summary>
        /// <param name="control">列表类型控件(BulletedList,CheckBoxList,DropDownList,ListBox,RadioButtonList)。</param>
        /// <param name="listControlsDataSource">数据源接口。</param>
        public void ListControlsDataSourceBind(ListControl control, IListControlsData listControlsDataSource)
        {
            IBaseModulePage page = this.Page as IBaseModulePage;
            if (page != null)
                page.ListControlsDataSourceBind(control, listControlsDataSource);
        }

        /// <summary>
        /// 列表类型控件数据绑定。
        /// </summary>
        /// <param name="dropDownListEx">DropDownListEx。</param>
        /// <param name="listControlsTreeViewDataSource">数据源接口。</param>
        public void ListControlsDataSourceBind(IDataDropDownList dropDownListEx, IListControlsTreeViewData listControlsTreeViewDataSource)
        {
            IBaseModulePage page = this.Page as IBaseModulePage;
            if (page != null)
                page.ListControlsDataSourceBind(dropDownListEx, listControlsTreeViewDataSource);
        }
    }
}
