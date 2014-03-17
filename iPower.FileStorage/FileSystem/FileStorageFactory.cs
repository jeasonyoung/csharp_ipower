//================================================================================
//  FileName: FileStorageFactory.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/23
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
using iPower.FileStorage;
namespace iPower.FileStorage.FileSystem
{
    /// <summary>
    /// 文件存储文件数据工厂类。
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
            if (content == null || content.Length == 0) return false;
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName", "文件名称不能为空！");
            string path = this.config.StorageSource;
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("未配置附件存储目录！");
            path += "\\" + fileName;
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                if (offSet > 0) fs.Seek(offSet, SeekOrigin.Begin);
                fs.Write(content, 0, content.Length);
            }
            return true;
        }
        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>成功返回true,失败返回false。</returns>
        public bool DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName", "文件名称不能为空！");
            string path = string.Format("{0}\\{1}", this.config.StorageSource, fileName);
            if (!File.Exists(path)) return false;
            File.Delete(path);
            return true;
        }
        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>文件数据。</returns>
        public byte[] Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName", "文件名称不能为空！");
            string path = string.Format("{0}\\{1}", this.config.StorageSource, fileName);
            if (!File.Exists(path)) return null;
            byte[] result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[1024];
                int len = 0;
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    while ((len = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, len);
                    }
                }
                result = ms.ToArray();
            }
            return result;
        }

        #endregion
    }
}