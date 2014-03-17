//================================================================================
//  FileName: IFileStorageFactory.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/22
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
using System.IO;

using iPower;
namespace iPower.FileStorage
{
    /// <summary>
    /// 文件存储工厂接口。
    /// </summary>
    public interface IFileStorageFactory
    {
        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <param name="offSet">偏移量。</param>
        /// <param name="content">文件内容。</param>
        /// <returns>成功返回True，失败False。</returns>
        bool Upload(string fileName, long offSet, byte[] content);
        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>成功返回true,失败返回false。</returns>
        bool DeleteFile(string fileName);
        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>文件数据。</returns>
        byte[] Download(string fileName);
    }
}
