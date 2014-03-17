//================================================================================
//  FileName: FileStorageFactory.cs
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
using iPower;
using iPower.FileStorage;
namespace iPower.FileStorage.SQLServer
{
    /// <summary>
    /// 数据库存储文件工厂类。
    /// </summary>
    public class FileStorageFactory : IFileStorageFactory
    {
        #region 成员变量，构造函数。
        private IStorageConfig config;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public FileStorageFactory()
        {
            this.config = new StorageModuleConfiguration();
        }
        #endregion

        //#region IFileStorageFactory 成员
        /////// <summary>
        /////// 上传文件。
        /////// </summary>
        /////// <param name="fileID">文件ID。</param>
        /////// <param name="fileName">文件名称。</param>
        /////// <param name="contentType">MIME内容类型。</param>
        /////// <param name="content">文件内容。</param>
        /////// <returns>成功返回True，失败False。</returns>
        ////public bool Upload(string fileID, string fileName, string contentType, byte[] content)
        ////{
        ////    if (!string.IsNullOrEmpty(fileID) && content != null)
        ////    {
        ////        FileDataStorage fs = new FileDataStorage(this.config);
        ////        return fs.Upload(fileID, fileName, contentType, content);
        ////    }
        ////    return false;
        ////}
        /////// <summary>
        /////// 删除文件。
        /////// </summary>
        /////// <param name="fileID">文件ID。</param>
        /////// <returns>成功返回true,失败返回false。</returns>
        ////public bool DeleteFile(string fileID)
        ////{
        ////    if (!string.IsNullOrEmpty(fileID))
        ////    {
        ////        FileDataStorage fs = new FileDataStorage(this.config);
        ////        return fs.DeleteFile(fileID);
        ////    }
        ////    return false;
        ////}
        /////// <summary>
        /////// 下载文件。
        /////// </summary>
        /////// <param name="fileID">文件ID。</param>
        /////// <param name="fullFileName">文件全名称。</param>
        /////// <param name="contentType">MIME内容类型。</param>
        /////// <returns>文件数据。</returns>
        ////public byte[] Download(string fileID, out string fullFileName, out string contentType)
        ////{
        ////    fullFileName = contentType = null;
        ////    if (!string.IsNullOrEmpty(fileID))
        ////    {
        ////        FileDataStorage fs = new FileDataStorage(this.config);
        ////        return fs.Download(fileID, out fullFileName, out contentType);
        ////    }
        ////    return null;
        ////}

        //#endregion

        #region IFileStorageFactory 成员
        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <param name="offSet">偏移量。</param>
        /// <param name="content">文件内容。</param>
        /// <returns>成功返回True，失败False。</returns>
        public bool Upload(string fileName, long offSet, byte[] content)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>成功返回true,失败返回false。</returns>
        public bool DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>文件数据。</returns>
        public byte[] Download(string fileName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
