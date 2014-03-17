//================================================================================
//  FileName: IListControlsData.cs
//  Desc:列表类型控件数据接口。
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

namespace iPower.Platform.Engine.DataSource
{
    /// <summary>
    ///  列表类型控件数据接口。
    /// </summary>
    public interface IListControlsData
    {
        /// <summary>
        /// 获取或设置显示字段。
        /// </summary>
        string DataTextField
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置显示数据格式。
        /// </summary>
        string DataTextFormatString
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置值字段。
        /// </summary>
        string DataValueField
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置排序字段。
        /// </summary>
        string OrderNoField
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置数据源。
        /// </summary>
        object DataSource
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 列表类型控件树型数据展示数据接口。
    /// </summary>
    public interface IListControlsTreeViewData : IListControlsData
    {
        /// <summary>
        /// 获取或设置父字段。
        /// </summary>
        string ParentDataValueField
        {
            get;
            set;
        }
    }
}
