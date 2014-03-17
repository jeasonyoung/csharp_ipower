//================================================================================
//  FileName: IBaseView.cs
//  Desc: 界面视图接口。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-26
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

using iPower;
using iPower.Platform;
using iPower.Platform.Security;
using iPower.Platform.Engine.DataSource;
namespace iPower.Platform.Engine.Service
{
    /// <summary>
    /// 界面视图接口。
    /// </summary>
    public interface IBaseView : ISystem, IUser, IDataHandler, IUISettings, ISecurity, IMenuData
    {
        /// <summary>
        /// 获取或设置导航数据。
        /// </summary>
        string NavigationContent
        {
            get;
            set;
        }
        /// <summary>
        /// 检索虚拟路径（绝对的或相对的）或应用程序相关的路径映射到的物理路径。
        /// </summary>
        /// <param name="virtualPath"> 表示虚拟路径的 System.String。</param>
        /// <returns>与虚拟路径或应用程序相关的路径关联的物理路径。</returns>
        string MapPath(string virtualPath);
    }
    /// <summary>
    /// Picker视图接口。
    /// </summary>
    public interface IPickerView
    {
        /// <summary>
        /// 获取是否多选。
        /// </summary>
        bool MultiSelect { get; }
        /// <summary>
        /// 获取值数组。
        /// </summary>
        string[] Values { get; }
        /// <summary>
        /// 绑定查询结果。
        /// </summary>
        /// <param name="data"></param>
        void BindSearchResult(IListControlsData data);
    }
}
