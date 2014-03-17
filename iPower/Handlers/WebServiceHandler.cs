//================================================================================
//  FileName: WebServiceHandler.cs
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
namespace iPower.Handlers
{
    /// <summary>
    /// WebService处理基类。
    /// 集成本类可以在HttpHandlers中直接配置WebService服务，
    /// 无需使用.asmx文件。
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public abstract class WebServiceHandler :WebService,IHttpHandler
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WebServiceHandler()
        {
            this.Handlers = new WebServiceHandlerFactory(this.GetType());
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取处理工厂。
        /// </summary>
        protected IHttpHandlerFactory Handlers { get; private set; }
        #endregion

        #region IHttpHandler 成员
        /// <summary>
        /// 获取实例是否可再次使用。
        /// </summary>
        public virtual bool IsReusable
        {
            get { return true; }
        }
        /// <summary>
        /// Handler处理。
        /// </summary>
        /// <param name="context">当前上下文。</param>
        public virtual void ProcessRequest(HttpContext context)
        {
            IHttpHandler handler = this.Handlers.GetHandler(context, null, null, null);
            handler.ProcessRequest(context);
        }

        #endregion
    }
}