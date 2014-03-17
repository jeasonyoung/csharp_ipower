//================================================================================
//  FileName: FileCacheItem.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-26
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
using iPower.Utility;
namespace iPower.Cache
{
    /// <summary>
    /// 文件缓存项。
    /// </summary>
    public class FileCacheItem : CacheItem
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <param name="loadType">加载方式。</param>
        /// <param name="fileKeyType">缓存键类型。</param>
        public FileCacheItem(string filePath, DataLoadType loadType, CacheKeyType fileKeyType)
        {
            this.FilePath = filePath;
            if (!File.Exists(this.FilePath))
            {
                throw new ArgumentException(filePath + " 文件不存在！");
            }
            this.FileLoadType = loadType;
            this.FileKeyType = fileKeyType;
            this.FileName = Path.GetFileNameWithoutExtension(this.FilePath);
            this.FileExtension = Path.GetExtension(this.FilePath);
            this.LastAccessDate = DateTime.Now;
            this.ItemSize = new FileInfo(this.FilePath).Length;
            this.ItemKey = this.FilePath;

            if (this.FileKeyType == CacheKeyType.MD5 || this.FileLoadType == DataLoadType.Load)
            {
                using (FileStream fstream = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (this.FileKeyType == CacheKeyType.MD5)
                    {
                        this.ItemKey = iPower.Cryptography.HashCrypto.HashFile(fstream, "md5");
                        fstream.Seek(0, SeekOrigin.Begin);
                    }
                    if (this.FileLoadType == DataLoadType.Load)
                    {
                        using (BufferBlockUtil block = new BufferBlockUtil())
                        {
                            byte[] buf = new byte[1024];
                            int len = 0;
                            while ((len = fstream.Read(buf, 0, buf.Length)) > 0)
                            {
                                block.Write(buf, 0, len);
                            }
                            this.FileBytes = block.ToArray();
                        }
                    }
                    fstream.Close();
                }
            }
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置文件名称。
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 获取或设置文件扩展名。
        /// </summary>
        public string FileExtension { get; private set; }
        /// <summary>
        /// 获取文件全名（文件名称+扩展名）。
        /// </summary>
        public string FileTitle
        {
            get
            {
                return this.FileName + this.FileExtension;
            }
        }
        /// <summary>
        /// 获取或设置文件路径。
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 获取或设置文件内容。
        /// </summary>
        public byte[] FileBytes { get; private set; }
        /// <summary>
        /// 获取或设置文件加载类型。
        /// </summary>
        public DataLoadType FileLoadType { get; private set; }
        /// <summary>
        /// 获取或设置文件键类型。
        /// </summary>
        public CacheKeyType FileKeyType { get; private set; }
        #endregion

        /// <summary>
        /// 延时加载数据。
        /// </summary>
        internal void DelayLoad()
        {
            if ((this.FileBytes == null || this.FileBytes.Length == 0) && (this.FileLoadType == DataLoadType.DelayLoad) && File.Exists(this.FilePath))
            {
                using (FileStream fstream = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BufferBlockUtil block = new BufferBlockUtil())
                    {
                        byte[] buf = new byte[1024];
                        int len = 0;
                        while ((len = fstream.Read(buf, 0, buf.Length)) > 0)
                        {
                            block.Write(buf, 0, len);
                        }
                        this.FileBytes = block.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// 读取文件。
        /// </summary>
        /// <param name="offset">偏移量。</param>
        /// <param name="length">读取长度。</param>
        /// <returns></returns>
        public byte[] Read(long offset, long length)
        {
            this.DelayLoad();
            if (this.FileBytes == null || this.FileBytes.Length < offset + length)
            {
                return null;
            }
            byte[] buffer = new byte[length];
            Array.Copy(this.FileBytes, offset, buffer, 0, length);
            this.LastAccessDate = DateTime.Now;
            this.OnCacheItemChanged();
            return buffer;
        }
    }
}