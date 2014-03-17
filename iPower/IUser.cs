//================================================================================
//  FileName: IUser.cs
//  Desc:用户信息接口。
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
using System;
using System.Collections.Generic;
using System.Text;

namespace iPower
{
    /// <summary>
    /// 用户信息接口。
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 获取或设置当前用户标示。
        /// </summary>
        GUIDEx CurrentUserID
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置当前用户名称。
        /// </summary>
        string CurrentUserName
        {
            get;
            set;
        }
    }
}
