//================================================================================
//  FileName: IResourceStorage.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2012-01-09 
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

namespace iPower.Resources
{
    /// <summary>
    /// 资源存储接口。
    /// </summary>
    public interface IResourceStorage
    {
        /// <summary>
        /// 资源存储。
        /// </summary>
        string ResourceStorage { get; }
    }
}
