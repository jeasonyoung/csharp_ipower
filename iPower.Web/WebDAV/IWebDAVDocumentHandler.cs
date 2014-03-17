using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using iPower;
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// 文档数据操作接口。
    /// </summary>
    public interface IWebDAVDocumentHandler
    {
        /// <summary>
        /// 获取文档数据。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Document GetDocument(HttpContext context);
        /// <summary>
        /// 创建文档数据。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Document CreateDocument(HttpContext context);
        /// <summary>
        /// 修改文档内容。
        /// </summary>
        /// <param name="document"></param>
        void ModifyDocumentContent(Document document);
    }
}
