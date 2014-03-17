//================================================================================
//  FileName: EnumLogFileRule.cs
//  Desc:日志文件记录方式枚举。
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
namespace iPower.Logs
{
    /// <summary>
    /// 日志文件记录方式枚举。
    /// </summary>
    public enum EnumLogFileRule
    {
        /// <summary>
        /// 未定义。
        /// </summary>
        None = 0x00,
        /// <summary>
        /// 年。
        /// </summary>
        Year = 0x01,
        /// <summary>
        /// 月。
        /// </summary>
        Month = 0x02,
        /// <summary>
        /// 周。
        /// </summary>
        Week = 0x03,
        /// <summary>
        /// 日。
        /// </summary>
        Date = 0x04,
        /// <summary>
        /// 时。
        /// </summary>
        Hour = 0x05
    }
}