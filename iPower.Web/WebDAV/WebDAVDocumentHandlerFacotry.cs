//================================================================================
//  FileName: WebDAVDocumentHandlerFacotry.cs
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
using iPower;
using iPower.Utility;
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// 文档数据操作工厂类。
    /// </summary>
    internal class WebDAVDocumentHandlerFacotry : IWebDAVDocumentHandler
    {
        #region 成员变量，构造函数。
        IWebDAVDocumentHandler documentHandler;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="documentHandler"></param>
        private WebDAVDocumentHandlerFacotry(IWebDAVDocumentHandler documentHandler)
        {
            if (documentHandler == null)
                throw new ArgumentNullException("没有实现文档数据操作接口！");
            this.documentHandler = documentHandler;
        }
        #endregion

        #region 静态实例。
        static object synchronizationObject = new object();
        static WebDAVDocumentHandlerFacotry documentHandlerFacotry;
        /// <summary>
        /// 获取静态实例。
        /// </summary>
        public static IWebDAVDocumentHandler Instance
        {
            get
            {
                lock (synchronizationObject)
                {
                    if (documentHandlerFacotry == null)
                    {
                        IWebDAVDocumentHandler documentHandler = null;
                        ModuleConfiguration config = new ModuleConfiguration();
                        string assembly = config.DocumentFactoryAssembly;
                        if (!string.IsNullOrEmpty(assembly))
                            documentHandler = TypeHelper.Create(assembly) as IWebDAVDocumentHandler;
                        else
                            documentHandler = new DefaultWebDAVDocumentHandler();
                        documentHandlerFacotry = new WebDAVDocumentHandlerFacotry(documentHandler);
                    }
                    return documentHandlerFacotry;
                }
            }
        }
        #endregion

        #region IWebDAVDocumentHandler 成员
        /// <summary>
        /// 获取文档数据。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Document GetDocument(HttpContext context)
        {
            return this.documentHandler.GetDocument(context);
        }
        /// <summary>
        /// 创建文档数据。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Document CreateDocument(HttpContext context)
        {
            return this.documentHandler.CreateDocument(context);
        }
        /// <summary>
        /// 修改文档内容。
        /// </summary>
        /// <param name="document"></param>
        public void ModifyDocumentContent(Document document)
        {
            this.documentHandler.ModifyDocumentContent(document);
        }

        #endregion
    }
}
