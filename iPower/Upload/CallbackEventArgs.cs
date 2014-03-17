//================================================================================
//  FileName: UploadResultEventArgs.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-27
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
namespace iPower.Upload
{
    /// <summary>
    /// 上传结果处理事件。
    /// </summary>
    /// <param name="e">结果处理参数。</param>
    /// <returns></returns>
    public delegate bool CallbackEventHandler(CallbackEventArgs e);
    /// <summary>
    /// 上传结果处理参数。
    /// </summary>
    public class CallbackEventArgs
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="callback"></param>
        public CallbackEventArgs(Stream callback)
        {
            this.Callback = callback;
        }
        /// <summary>
        /// 获取反馈数据流。
        /// </summary>
        public Stream Callback { get; private set; }
    }
}