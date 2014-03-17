//================================================================================
//  FileName: ModuleConst.cs
//  Desc: Windows服务全局常量。
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

namespace iPower.WinService
{
    /// <summary>
    /// Windows服务全局常量。
    /// </summary>
    public sealed class ModuleConst
    {
        /// <summary>
        /// 工作启动标示
        /// </summary>
        public const string CONST_STR_JOBSTARTFLAG = "1";
        /// <summary>
        /// 定时器时间间隔。
        /// </summary>
        public const double CONST_DOU_TIMEINTERVAL = 1000.0;
        /// <summary>
        /// 线程睡眠时间。
        /// </summary>
        public const int CONST_INT_THREAHSLEEP = 1000;
    }
}
