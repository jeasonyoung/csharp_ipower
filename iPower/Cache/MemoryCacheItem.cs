//================================================================================
//  FileName: MemoryCacheItem.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-5-8
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
namespace iPower.Cache
{
    /// <summary>
    /// 内存缓存项。
    /// </summary>
    [Serializable]
    public class MemoryCacheItem : CacheItem
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="fileKeyType"></param>
        public MemoryCacheItem(MemoryStream ms, CacheKeyType fileKeyType)
            : base()
        {
            if (ms == null)
            {
                throw new ArgumentNullException("ms");
            }
            if (fileKeyType == CacheKeyType.MD5 && ms.Length > 0)
            {
                ms.Seek(0, SeekOrigin.Begin);
                this.ItemKey = iPower.Cryptography.HashCrypto.HashFile(ms, "md5");
            }
            this.DataBytes = ms.ToArray();
        }
        #endregion

        /// <summary>
        /// 获取或设置文件内容。
        /// </summary>
        public byte[] DataBytes { get; private set; }
    }
}
