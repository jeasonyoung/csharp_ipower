//================================================================================
//  FileName: LockHandler.cs
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
    /// 实现对Lock的支持。
    /// </summary>
    internal class LockHandler :IVerbHandler
    {
        #region IVerbHandler 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Process(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            string token = string.Format("{0}:{1}", Guid.NewGuid().ToString(), DateTime.Now.Ticks.ToString());

            context.Response.AppendHeader("Lock-Token", "<opaquelocktoken:" + token + ">");

            string responseBody = @"<?xml version=""1.0""?>
                                    <a:prop xmlns:a=""DAV:"">
                                        <a:lockdiscovery>
                                            <a:activelock>
                                                <a:locktype>
                                                    <a:write/>
                                                </a:locktype>
                                                <a:lockscope>
                                                    <a:exclusive/>
                                                </a:lockscope>
                                                <owner xmlns=""DAV:"">Administrator</owner>
                                                <a:locktoken>
                                                    <a:href>opaquelocktoken:{0}</a:href>
                                                </a:locktoken>
                                                <a:depth>0</a:depth>
                                                <a:timeout>Second-180</a:timeout>
                                            </a:activelock>
                                        </a:lockdiscovery>
                                    </a:prop>";
            context.Response.Write(String.Format(responseBody, token));
            context.Response.End();
        }

        #endregion
    }

    /// <summary>
    /// 实现对UnLock的支持。
    /// </summary>
    internal class UnLockHandler : IVerbHandler
    {

        #region IVerbHandler 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Process(HttpContext context)
        {
        }

        #endregion
    }
}
