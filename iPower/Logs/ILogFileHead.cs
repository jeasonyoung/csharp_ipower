//================================================================================
//  FileName: ILogFileHead.cs
//  Desc:生成日志文件配置接口。
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
    /// 生成日志文件配置接口。
    /// </summary>
    public interface ILogFileHead
    {
        /// <summary>
        /// 获取日志文件名。
        /// </summary>
        string LogFileHead { get;}
        /// <summary>
        /// 获取日志文件生成规则。
        /// </summary>
        EnumLogFileRule LogFileRule { get;}
    }
}
