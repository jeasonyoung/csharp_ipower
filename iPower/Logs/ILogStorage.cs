//================================================================================
//  FileName: ILogStorage.cs
//  Desc:日志存储接口。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-16
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
namespace iPower.Logs
{
    /// <summary>
    /// 日志存储接口。
    /// </summary>
    public interface ILogStorage : IDisposable
    {
        /// <summary>
        /// 存储日志。
        /// </summary>
        /// <param name="logType">日志类型。</param>
        /// <param name="logContent">日志内容。</param>
        void StorageLog(EnumLogsType logType, string logContent);
    }
}
