//================================================================================
//  FileName: IStorageConfig.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/22
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

namespace iPower.FileStorage
{
    /// <summary>
    /// 存储的配置接口。
    /// </summary>
    public interface IStorageConfig
    {
        /// <summary>
        /// 获取存储源。
        /// </summary>
        string StorageSource { get; }
    }
}
