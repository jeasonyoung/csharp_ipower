//================================================================================
//  FileName: IMenuData.cs
//  Desc:菜单数据接口。
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

using System.Data;
namespace iPower.Platform
{
    /// <summary>
    /// 菜单数据接口。
    /// </summary>
    public interface IMenuData
    {
        /// <summary>
        /// 获取或设置主菜单数据。
        /// </summary>
        ModuleDefineCollection MainMenuData
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置菜单数据。
        /// </summary>
        ModuleDefineCollection MenuData
        {
            get;
            set;
        }
    }
}
