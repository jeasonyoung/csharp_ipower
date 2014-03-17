//================================================================================
//  FileName: ReqPaging.cs
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
    /// 请求分页参数类。
    /// </summary>
    [Serializable]
    public class ReqPaging :IReqPaging
    {
        #region IReqPaging 成员
        /// <summary>
        /// 获取或设置每页数据量。
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 获取或设置当前页码。
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 获取或设置排序字段。
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// 获取或设置排序方式。
        /// </summary>
        public string order { get; set; }

        #endregion
    }
}