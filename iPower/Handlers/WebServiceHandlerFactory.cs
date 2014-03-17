//================================================================================
//  FileName: WebServiceHandlerFactory.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-10-11
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
using System.Web.Services;
using System.Reflection;
using System.Security.Permissions;

namespace iPower.Handlers
{
    /// <summary>
    /// WebService的Handler处理工厂类。
    /// 使用此类可以让WebService服务不需要.asmx文件。
    /// </summary>
    [PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
    internal class WebServiceHandlerFactory: System.Web.Services.Protocols.WebServiceHandlerFactory, IHttpHandlerFactory
    {
        #region 成员变量，构造函数。
        private static readonly MethodInfo CoreGetHandler = typeof(System.Web.Services.Protocols.WebServiceHandlerFactory).GetMethod("CoreGetHandler",
                                                                                                                                      BindingFlags.NonPublic | BindingFlags.Instance,
                                                                                                                                      null,
                                                                                                                                      new Type[] { typeof(Type), typeof(HttpContext), typeof(HttpRequest), typeof(HttpResponse) },
                                                                                                                                      null);
        private Type serviceType;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="serviceType">服务类型。</param>
        public WebServiceHandlerFactory(Type serviceType)
        {
            this.serviceType = serviceType;
        }
        #endregion

        #region IHttpHandlerFactory 成员
        /// <summary>
        /// 获取处理的HttpHandler对象。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestType"></param>
        /// <param name="url"></param>
        /// <param name="pathTranslated"></param>
        /// <returns></returns>
        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (this.serviceType == null) throw new ArgumentNullException("serviceType");
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();
            return (IHttpHandler)CoreGetHandler.Invoke(this, new object[] { this.serviceType, context, context.Request, context.Response });
        }
        /// <summary>
        /// 释放Handler。
        /// </summary>
        /// <param name="handler"></param>
        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
        {
            base.ReleaseHandler(handler);
        }
        #endregion
    }
}