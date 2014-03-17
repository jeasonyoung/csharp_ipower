//================================================================================
//  FileName: IUISettings.cs
//  Desc:界面设置接口。
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

namespace iPower.Platform
{
    /// <summary>
    /// 界面设置接口。
    /// </summary>
    public interface IUISettings : ITopResource
    {
        /// <summary>
        /// 获取或设置当前目录标示。
        /// </summary>
        string CurrentFolderID
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置当前模块标题。
        /// </summary>
        string CurrentModuleTitle
        {
            get;
            set;
        }
    }
}
