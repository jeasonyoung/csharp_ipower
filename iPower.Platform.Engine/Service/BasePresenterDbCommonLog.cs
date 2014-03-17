//================================================================================
//  FileName: BasePresenterDbCommonLog.cs
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

using iPower.Utility;
using iPower.Platform.Logs;
namespace iPower.Platform.Engine.Service
{
    /// <summary>
    /// 日志记录。
    /// </summary>
    partial class BasePresenter<T, K>
    {
        /// <summary>
        /// 创建日志记录到数据库。
        /// </summary>
        /// <param name="head"></param>
        /// <param name="content"></param>
        protected virtual void CreateCommonLog(string head, string content)
        {
            ICreateDbCommonLog commonLog = this.ModuleConfig.DbCommonLogAssembly;
            if (commonLog != null && this.View != null)
            {
                commonLog.CreateCommonLog(this.View.CurrentSystemID, this.View.CurrentSystemName,
                                          this.View.CurrentUserID, this.View.CurrentUserName,
                                          head, content);
            }
        }
    }
}
