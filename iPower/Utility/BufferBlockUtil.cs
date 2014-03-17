//================================================================================
//  FileName: BufferBlockUtil.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-1-8
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

namespace iPower.Utility
{
    /// <summary>
    /// 大块数据工具类。
    /// </summary>
    public sealed class BufferBlockUtil: IDisposable
    {
        #region 成员变量，构造函数。
        long totoal = 0;
        Queue<byte[]> queue = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BufferBlockUtil()
        {
            this.queue = new Queue<byte[]>();
        }
        #endregion

        /// <summary>
        /// 写入数据块。
        /// </summary>
        /// <param name="array">数据块。</param>
        /// <param name="offset">数据块内偏移。</param>
        /// <param name="count">有效数据长度。</param>
        public void Write(byte[] array, int offset, int count)
        {
            lock (this)
            {
                int len = 0;
                if (array != null && (len = array.Length) > 0)
                {
                    if ((offset > -1) && (offset < len) && (count > 0) && ((offset + count) <= len))
                    {
                        byte[] buf = new byte[count];
                        Array.Copy(array, offset, buf, 0, count);
                        this.queue.Enqueue(buf);
                        this.totoal += buf.Length;
                    }

                }
            }
        }

        /// <summary>
        /// 将数据转换为数组。
        /// </summary>
        /// <returns>数据块数组。</returns>
        public byte[] ToArray()
        {
            lock (this)
            {
                if (this.totoal > 0 && this.queue.Count > 0)
                {
                    byte[] result = new byte[this.totoal];
                    long index = 0;
                    while (this.queue.Count > 0)
                    {
                        byte[] data = this.queue.Dequeue();
                        int len = 0;
                        if ((len = data.Length) > 0)
                        {
                            Array.Copy(data, 0, result, index, len);
                            index += len;
                        }
                    }
                    return result;
                }
                return null;
            }
        }


        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.queue.Clear();
            this.queue = null;
            this.totoal = 0;
        }
        #endregion
    }
}
