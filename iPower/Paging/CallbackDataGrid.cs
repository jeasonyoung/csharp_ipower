//================================================================================
//  FileName: CallbackDataGrid.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2014-1-9
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
    /// 反馈数据集。
    /// </summary>
    [Serializable]
    public class CallbackDataGrid<T>
    {
        /// <summary>
        /// 获取或设置总记录数。
        /// </summary>
        public long total { get; set; }
        /// <summary>
        /// 获取或设置反馈数据集合。
        /// </summary>
        public List<T> rows { get; set; }
    }
}