//================================================================================
//  FileName: WinServiceJobFunction.cs
//  Desc:Windows服务的Job函数基类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010-1-19
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
using iPower.WinService.Logs;
namespace iPower.WinService.Jobs
{
    /// <summary>
    /// Windows服务的Job函数接口。
    /// </summary>
    public interface IJobFunction
    {
        /// <summary>
        /// 执行入口函数。
        /// </summary>
        void Run(params string[] args);
    }
}