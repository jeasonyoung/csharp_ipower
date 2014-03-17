//================================================================================
//  FileName: IWinServiceJob.cs
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

using iPower.Logs;
namespace iPower.WinService.Jobs
{
    /// <summary>
    /// 日志变更信息。
    /// </summary>
    /// <param name="logType"></param>
    /// <param name="logContent"></param>
    public delegate void LogChangedHandler(EnumLogsType logType, string logContent);
    /// <summary>
    /// Windows服务Job的接口。
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// 获取任务名称。
        /// </summary>
        string JobName { get; }
        /// <summary>
        /// 日志记录输出。
        /// </summary>
        event LogChangedHandler LogChanged;
        /// <summary>
        /// 启动。
        /// </summary>
        void Start(params string[] args);
        /// <summary>
        /// 暂停。
        /// </summary>
        void Pause();
        /// <summary>
        /// 停止。
        /// </summary>
        void Stop();
        /// <summary>
        ///  判断是否具备运行条件。
        /// </summary>
        /// <returns></returns>
        bool CanRun();
        /// <summary>
        ///  运行。
        /// </summary>
        void Run();
    }
}