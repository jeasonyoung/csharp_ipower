//================================================================================
//  FileName: IVerbHandler.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/27
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
using System.Web;
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// Verb处理接口。
    /// </summary>
    internal interface IVerbHandler
    {
        /// <summary>
        /// 处理函数。
        /// </summary>
        /// <param name="context"></param>
        void Process(HttpContext context);
    }
}
