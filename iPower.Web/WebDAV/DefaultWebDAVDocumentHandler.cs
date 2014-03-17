//================================================================================
//  FileName: DefaultWebDAVDocument.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

using iPower;
using iPower.FileStorage;
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// 默认文档数据操作。
    /// </summary>
    internal class DefaultWebDAVDocumentHandler : IWebDAVDocumentHandler
    {
        #region 成员变量，构造函数。
        IFileStorageFactory storageFactory = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DefaultWebDAVDocumentHandler()
        {
            this.storageFactory = FileStorageFactoryInstance.Instance;
        }
        #endregion

        #region IWebDAVDocumentHandler 成员
        ///<summary>
        /// 获取文档数据。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Document GetDocument(HttpContext context)
        {
            GUIDEx fileID = this.GetDocumentID(context);
            Document doc = null;
            if (fileID.IsValid)
            {
                doc = new Document();
                doc.FileID = fileID;
                string fileName = null, contentType = null;
                doc.Content = this.storageFactory.Download(doc.FileID); //this.storageFactory.Download(doc.FileID, out fileName, out contentType);
                doc.FileName = fileName;
                doc.ContentType = contentType;
                doc.UserInfo = context.Handler as IUser;
            }
            return doc;
        }
        /// <summary>
        /// 创建文档数据。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Document CreateDocument(HttpContext context)
        {
            Document doc = null;
            GUIDEx fileID = this.GetDocumentID(context);
            if (fileID.IsValid)
            {
                HttpRequest request = context.Request;
                doc = new Document();
                doc.FileID = fileID;
                doc.ContentType = request.ContentType;
                doc.UserInfo = context.Handler as IUser;
                using (Stream dataStream = request.InputStream)
                {
                    byte[] data = new byte[request.ContentLength];
                    dataStream.Read(data, 0, data.Length);
                    doc.Content = data;
                    dataStream.Close();
                }
            }
            return doc;
        }
        /// <summary>
        /// 修改文档内容。
        /// </summary>
        /// <param name="document"></param>
        public void ModifyDocumentContent(Document document)
        {
            if (document != null && document.FileID.IsValid && document.Content != null)
            {
                //this.storageFactory.Upload(document.FileID, document.FileName, document.ContentType, document.Content);
                this.storageFactory.Upload(document.FileID, 0, document.Content);
            }

        }

        #endregion


        /// <summary>
        /// 获取文档的ID。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected GUIDEx GetDocumentID(HttpContext context)
        {
            if (context != null)
            {
                string url = context.Request.Url.ToString();
                string[] strArr = url.Split('/');
                string strName = strArr[strArr.Length - 1];
                if (!string.IsNullOrEmpty(strName))
                {
                    return new GUIDEx(strName.Split('.')[0]);
                }
            }
            return GUIDEx.Null;
        }
    }
}
