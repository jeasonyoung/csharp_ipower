//================================================================================
//  FileName: IDataSourceEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/4
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

namespace iPower.Web.UI
{
    /// <summary>
    /// 表示数据绑定控件绑定到的抽象数据源。
    /// </summary>
    public interface IDataSourceEx
    {
        /// <summary>
        /// 当数据源控件以某种影响数据绑定控件的方式发生变化时发生。 
        /// </summary>
        event EventHandler DataSourceChanged;

        /// <summary>
        /// 获取与数据源控件关联的命名的数据源视图。
        /// </summary>
        /// <param name="viewName">要检索的视图的名称。 </param>
        /// <returns></returns>
        DataSourceViewEx GetView(string viewName);
        /// <summary>
        /// 获取名称的集合，这些名称表示与 <see cref="IDataSourceEx"/> 接口关联的视图对象的列表。
        /// </summary>
        /// <returns></returns>
        ICollection GetViewNames();

    }
}
