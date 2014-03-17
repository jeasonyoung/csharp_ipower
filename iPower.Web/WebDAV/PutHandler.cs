//================================================================================
//  FileName: PutHandler.cs
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
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// Put行为处理。
    /// </summary>
    internal class PutHandler : IVerbHandler
    {
        #region 成员变量，构造函数。
        IWebDAVDocumentHandler docSvr = null;
        /// <summary>
        ///构造函数。
        /// </summary>
        public PutHandler()
        {
            this.docSvr = WebDAVDocumentHandlerFacotry.Instance;
        }
        #endregion

        #region IVerbHandler 成员
        /// <summary>
        ///  
        /// </summary>
        /// <param name="context"></param>
        public void Process(HttpContext context)
        {
            Document doc = this.docSvr.CreateDocument(context);
            if (doc == null)
            {
                context.Response.Write("文档不符合规则无法获取！");
                return;
            }
            this.docSvr.ModifyDocumentContent(doc);
        }

        #endregion
    }
}
