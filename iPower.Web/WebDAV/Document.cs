//================================================================================
//  FileName: Document.cs
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

using iPower;
namespace iPower.Web.WebDAV
{
    /// <summary>
    /// Office文档实体类。
    /// </summary>
    public class Document
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Document()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置文件ID。
        /// </summary>
        public GUIDEx FileID
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置文件名称。
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置MIME内容类型。
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置文档内容。
        /// </summary>
        public byte[] Content
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        public IUser UserInfo
        {
            get;
            set;
        }
        #endregion
    }
}
