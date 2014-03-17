//================================================================================
//  FileName: UploadProgressEventArgs.cs
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

namespace iPower.Upload
{
    /// <summary>
    /// 上传进度委托。
    /// </summary>
    /// <param name="e">进度参数。</param>
    public delegate void UploadProgressEventHandler(ProgressEventArgs e);
    /// <summary>
    /// 上传进度参数。
    /// </summary>
    public class ProgressEventArgs
    {
        /// <summary>
        /// 获取已上传进度((int)(已经上传数据 * (int.MaxValue / 总数据)))。
        /// </summary>
        public int Value { get; internal set; }
        /// <summary>
        /// 获取已用时间(秒)。
        /// </summary>
        public double Time { get; internal set; }
        /// <summary>
        /// 获取已用时间信息。
        /// </summary>
        public string TimeText { get; internal set; }
        /// <summary>
        /// 获取速度信息。
        /// </summary>
        public string SpeedText { get; internal set; }
        /// <summary>
        /// 获取状态信息。
        /// </summary>
        public string Status { get; internal set; }
    }
}