//================================================================================
//  FileName: GetHandler.cs
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
using System.IO;
using iPower;
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// Get动作处理。
    /// </summary>
    internal class GetHandler : IVerbHandler
    {
        #region 成员变量，构造函数。
        IWebDAVDocumentHandler docSvr = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public GetHandler()
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
            if (this.docSvr != null)
            {
                Document doc = this.docSvr.GetDocument(context);
                if (doc == null)
                {
                    context.Response.Write("文档不存在！");
                    return;
                }
                context.Response.Clear();
                context.Response.ContentType = doc.ContentType;
                //string fileName = string.Format("{0}{1}", doc.FileID, Path.GetExtension(doc.FileName));
                //fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);//必须编码，不然文件名会出现乱码。
                //context.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                //if (doc.Content != null && doc.Content.Length > 0)
                //{
                    context.Response.BinaryWrite(doc.Content);
                //}
                context.Response.End();
            }
        }

        #endregion
    }
}
