//================================================================================
//  FileName: ICreateDbCommonLog.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/7/1
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

namespace iPower.Platform.Logs
{
    /// <summary>
    /// 创建数据库日志接口。
    /// </summary>
    public interface ICreateDbCommonLog
    {
        /// <summary>
        /// 创建日志。
        /// </summary>
        /// <param name="systemID">系统ID。</param>
        /// <param name="systemName">系统名称。</param>
        /// <param name="createEmployeeID">创建日志用户ID。</param>
        /// <param name="createEmployeeName">创建日志用户名称。</param>
        /// <param name="relationTable">关联表名称。</param>
        /// <param name="log">日志内容。</param>
        void CreateCommonLog(GUIDEx systemID, string systemName, GUIDEx createEmployeeID, string createEmployeeName, string relationTable, string log);
    }
}
