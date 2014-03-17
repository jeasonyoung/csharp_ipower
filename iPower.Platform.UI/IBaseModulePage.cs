//================================================================================
//  FileName: IBaseModulePage.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/6/30
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
using System.Web.UI;
using System.Web.UI.WebControls;

using iPower.Web.UI;
using iPower.Platform.Engine.DataSource;
using iPower.Platform.Engine.Service;
namespace iPower.Platform.UI
{
    /// <summary>
    /// UI接口。
    /// </summary>
    public interface IBaseModulePage : IBaseView
    {
        /// <summary>
        /// 获取页面ViewState。
        /// </summary>
        StateBag PageViewState
        {
            get;
        }
        /// <summary>
        /// 获取当前页面的标题。
        /// </summary>
        string CurrentPageTile
        {
            get;
            set;
        }
        /// <summary>
        /// 列表类型控件数据绑定。
        /// </summary>
        /// <param name="control">列表类型控件(BulletedList,CheckBoxList,DropDownList,ListBox,RadioButtonList)。</param>
        /// <param name="listControlsDataSource">数据源接口。</param>
        void ListControlsDataSourceBind(ListControl control, IListControlsData listControlsDataSource);
         /// <summary>
        /// 列表类型控件数据绑定。
        /// </summary>
        /// <param name="dropDownListEx">DropDownListEx。</param>
        /// <param name="listControlsTreeViewDataSource">数据源接口。</param>
        void ListControlsDataSourceBind(IDataDropDownList dropDownListEx, IListControlsTreeViewData listControlsTreeViewDataSource);
        /// <summary>
        /// 树形控件数据绑定。
        /// </summary>
        /// <param name="treeView">treeView。</param>
        /// <param name="listControlsTreeViewDataSource">数据源接口。</param>
        void ListControlsDataSourceBind(ITreeView treeView, IListControlsTreeViewData listControlsTreeViewDataSource);
    }
}
