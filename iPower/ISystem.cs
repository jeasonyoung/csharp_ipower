﻿//================================================================================
//  FileName: ISystem.cs
//  Desc:系统信息接口。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-16
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
namespace iPower
{
    /// <summary>
    /// 系统信息接口。
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// 获取或设置当前系统标示。
        /// </summary>
        GUIDEx CurrentSystemID
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置当前系统名称。
        /// </summary>
        string CurrentSystemName
        {
            get;
            set;
        }
    }
}