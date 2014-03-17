//================================================================================
//  FileName: IPaging.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2014-1-8
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

namespace iPower.Paging
{
    /// <summary>
    /// 请求分页参数接口。
    /// </summary>
    public interface IReqPaging
    {
        /// <summary>
        /// 获取或设置每页数据量。
        /// </summary>
        int rows { get; set; }
        /// <summary>
        /// 获取或设置当前页码。
        /// </summary>
        int page { get; set; }
        /// <summary>
        /// 获取或设置排序字段。
        /// </summary>
        string sort { get; set; }
        /// <summary>
        /// 获取或设置排序方式。
        /// </summary>
        string order { get; set; }
    }
}