//================================================================================
//  FileName: IOLock.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-25
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
using System.Threading;
namespace iPower.Logs
{
    /// <summary>
    /// 读写线程锁。
    /// </summary>
    internal sealed class IOLock
    {
        #region 成员变量，构造函数。
        ReaderWriterLock rwLock;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public IOLock()
        {
            this.rwLock = new ReaderWriterLock();
        }
        #endregion

        /// <summary>
        /// 超时读线程锁。
        /// </summary>
        public void AcquireReaderLock()
        {
            this.rwLock.AcquireReaderLock(-1);
        }
        /// <summary>
        /// 减少读锁计数。
        /// </summary>
        public void ReleaseReaderLock()
        {
            this.rwLock.ReleaseReaderLock();
        }
        /// <summary>
        /// 超时写线程锁。
        /// </summary>
        public void AcquireWriterLock()
        {
            this.rwLock.AcquireWriterLock(-1);
        }
        /// <summary>
        /// 减少写锁计数。
        /// </summary>
        public void ReleaseWriterLock()
        {
            this.rwLock.ReleaseWriterLock();
        }
    }
}
