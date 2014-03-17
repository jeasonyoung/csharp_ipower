//================================================================================
//  FileName: IServiceJobLog.cs
//  Desc:
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

using iPower.Logs;
namespace iPower.WinService
{
    /// <summary>
    /// Windows服务的Job日志接口。
    /// </summary>
    public interface IJobLog : ILogFileHead
    {
    }
}
