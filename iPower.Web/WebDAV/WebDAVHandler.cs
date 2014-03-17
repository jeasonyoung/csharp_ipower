//================================================================================
//  FileName: WebDAVHandler.cs
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
using System.Text.RegularExpressions;

using System.Web;
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// Office在线编辑处理。
    /// </summary>
    public class WebDAVHandler : IHttpHandler
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WebDAVHandler()
        {
        }
        #endregion

        #region IHttpHandler 成员
        /// <summary>
        /// 获取一个值，该值指示其他请求是否可以使用 <see cref="IHttpHandler"/> 实例。
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }
        /// <summary>
        /// 通过实现 <see cref="IHttpHandler"/> 接口的自定义 <see cref="IHttpHandler"/> 启用 HTTP Web 请求的处理。
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            context.Response.AddHeader("OpenWebDavServer", "1.0");
            string verb = request.HttpMethod;
            IVerbHandler handler = this.GetVerbHandler(verb);
            if (handler != null)
                handler.Process(context);
        }
        #endregion
        
        #region 辅助函数。
        IVerbHandler GetVerbHandler(string verb)
        {
            switch (verb.ToUpper())
            {
                case "OPTIONS":
                    return new OptionsHandler();
                case "LOCK":
                    return new LockHandler();
                case "UNLOCK":
                    return new UnLockHandler();
                case "GET":
                    return new GetHandler();
                case "PUT":
                    return new PutHandler();
                default:
                    return null;
            }
        }
        #endregion
    }
}
