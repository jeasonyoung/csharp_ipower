//================================================================================
//  FileName: WinServiceJobStatus.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/1/10
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

namespace iPower.WinService
{
    /// <summary>
    /// Windows服务Job状态枚举。
    /// </summary>
    [Serializable]
    public enum JobStatus
    {
        /// <summary>
        /// 停止。
        /// </summary>
        Stopped = 0x01,
        /// <summary>
        /// 运行中。 
        /// </summary>
        Running = 0x02,
        /// <summary>
        /// 运行结束。
        /// </summary>
        End = 0x04,
        /// <summary>
        /// 运行错误。
        /// </summary>
        Error = 0x08,
        /// <summary>
        /// 过期。
        /// </summary>
        Expired = 0x10,
        /// <summary>
        /// 加载服务失败。
        /// </summary>
        LoadFailure = 0x20,
        /// <summary>
        /// 加载服务成功。
        /// </summary>
        LoadSuccessfull = 0x40
    }
}