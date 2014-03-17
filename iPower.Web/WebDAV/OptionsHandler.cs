//================================================================================
//  FileName: OptionsHandler.cs
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
    /// 实现对Options请求的处理。
    /// </summary>
    internal class OptionsHandler  : IVerbHandler
    {
        #region IVerbHandler 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Process(HttpContext context)
        {
            context.Response.AppendHeader("DASL", "<DAV:sql>");
            context.Response.AppendHeader("DAV", "1, 2");
            context.Response.AppendHeader("Public", "OPTIONS, TRACE, GET, HEAD, DELETE, PUT, POST, COPY, MOVE, MKCOL, PROPFIND, PROPPATCH, LOCK, UNLOCK, SEARCH");
            context.Response.AppendHeader("Allow", "OPTIONS, TRACE, GET, HEAD, DELETE, PUT, COPY, MOVE, PROPFIND, PROPPATCH, SEARCH, LOCK, UNLOCK");
        }
        #endregion
    }
}
