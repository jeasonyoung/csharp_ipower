//================================================================================
//  FileName: IWinShellTest.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-3-27
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

namespace iPower.WinService.Test
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    public delegate void ChangedHandler(string content);
    /// <summary>
    /// WinShell测试接口。
    /// </summary>
    public interface IWinShellTest
    {
        /// <summary>
        /// 获取是否一个周期。
        /// </summary>
        bool IsOnce { get; set; }
        /// <summary>
        /// 获取或设置服务名称。
        /// </summary>
        string ServiceName { get; set; }
        /// <summary>
        /// 消息事件。
        /// </summary>
        event ChangedHandler Changed;
        /// <summary>
        /// 启动服务。
        /// </summary>
        /// <param name="args"></param>
        void OnStart(string[] args);
        /// <summary>
        /// 继续服务。
        /// </summary>
        void OnContinue();
        /// <summary>
        /// 暂停服务。
        /// </summary>
        void OnPause();
        /// <summary>
        /// 停止服务。
        /// </summary>
        void OnStop();
    }
}
