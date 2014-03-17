//================================================================================
//  FileName: BaseModulePageDataBind.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/7/5
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

using System.Web.UI.WebControls;
using iPower.Web.UI;
using iPower.Platform.Engine.DataSource;
namespace iPower.Platform.UI
{
    /// <summary>
    /// 数据绑定
    /// </summary>
    partial class BaseModulePage
    {
        /// <summary>
        /// 列表类型控件数据绑定。
        /// </summary>
        /// <param name="control">列表类型控件(BulletedList,CheckBoxList,DropDownList,ListBox,RadioButtonList)。</param>
        /// <param name="listControlsDataSource">数据源接口。</param>
        public void ListControlsDataSourceBind(ListControl control, IListControlsData listControlsDataSource)
        {
            if (control != null && listControlsDataSource != null)
            {
                control.DataTextField = listControlsDataSource.DataTextField;
                if (!string.IsNullOrEmpty(listControlsDataSource.DataTextFormatString))
                    control.DataTextFormatString = listControlsDataSource.DataTextFormatString;
                control.DataValueField = listControlsDataSource.DataValueField;
                control.DataSource = listControlsDataSource.DataSource;
                control.DataBind();
            }
        }
        /// <summary>
        /// 列表类型控件数据绑定。
        /// </summary>
        /// <param name="dropDownListEx">DropDownListEx。</param>
        /// <param name="listControlsTreeViewDataSource">数据源接口。</param>
        public void ListControlsDataSourceBind(IDataDropDownList dropDownListEx, IListControlsTreeViewData listControlsTreeViewDataSource)
        {
            if (dropDownListEx != null && listControlsTreeViewDataSource != null)
            {
                dropDownListEx.DataTextField = listControlsTreeViewDataSource.DataTextField;
                if (!string.IsNullOrEmpty(listControlsTreeViewDataSource.DataTextFormatString))
                    dropDownListEx.DataTextFormatString = listControlsTreeViewDataSource.DataTextFormatString;
                dropDownListEx.DataValueField = listControlsTreeViewDataSource.DataValueField;
                dropDownListEx.DataValueParentField = listControlsTreeViewDataSource.ParentDataValueField;

                dropDownListEx.DataSource = listControlsTreeViewDataSource.DataSource;
                dropDownListEx.DataBind();
            }
        }

        ///// <summary>
        ///// 列表类型控件数据绑定。
        ///// </summary>
        ///// <param name="dropDownListEx">DropDownListEx。</param>
        ///// <param name="listControlsDataSource">数据源接口。</param>
        //public void ListControlsDataSourceBind(IDataDropDownList dropDownListEx, IListControlsData listControlsDataSource)
        //{
        //    if (dropDownListEx != null && listControlsDataSource != null)
        //    {
        //        dropDownListEx.DataTextField = listControlsDataSource.DataTextField;
        //        if (!string.IsNullOrEmpty(listControlsDataSource.DataTextFormatString))
        //            dropDownListEx.DataTextFormatString = listControlsDataSource.DataTextFormatString;
        //        dropDownListEx.DataValueField = listControlsDataSource.DataValueField;

        //        dropDownListEx.DataSource = listControlsDataSource.DataSource;
        //        dropDownListEx.DataBind();
        //    }
        //}

        /// <summary>
        /// 树形控件数据绑定。
        /// </summary>
        /// <param name="treeView">treeView。</param>
        /// <param name="listControlsTreeViewDataSource">数据源接口。</param>
        public void ListControlsDataSourceBind(ITreeView treeView, IListControlsTreeViewData listControlsTreeViewDataSource)
        {
            if (treeView != null && listControlsTreeViewDataSource != null)
            {
                treeView.DataSource = listControlsTreeViewDataSource.DataSource;
                treeView.IDField = listControlsTreeViewDataSource.DataValueField;
                treeView.PIDField = listControlsTreeViewDataSource.ParentDataValueField;
                treeView.TitleField = listControlsTreeViewDataSource.DataTextField;
                treeView.OrderNoField = listControlsTreeViewDataSource.OrderNoField;
                treeView.BuildTree();
            }
        }
        /// <summary>
        /// 树形控件数据绑定。
        /// </summary>
        /// <param name="treeView">treeView。</param>
        /// <param name="listControlsDataSource">数据源接口。</param>
        public void ListControlsDataSourceBind(ITreeView treeView, IListControlsData listControlsDataSource)
        {
            if (treeView != null && listControlsDataSource != null)
            {
                treeView.DataSource = listControlsDataSource.DataSource;
                treeView.IDField = listControlsDataSource.DataValueField;
                treeView.TitleField = listControlsDataSource.DataTextField;

                string orderNoField = listControlsDataSource.OrderNoField;
                if (!string.IsNullOrEmpty(orderNoField))
                    treeView.OrderNoField = orderNoField;
                treeView.BuildTree();

            }
        }
    }
}
