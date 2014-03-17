//================================================================================
//  FileName: Util.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/22
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

namespace iPower.Web.Utility
{
    /// <summary>
    /// 工具类。
    /// </summary>
    internal static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string EnsureEndWithSemiColon(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int length = value.Length;
                if ((length > 0) && (value[length - 1] != ';'))
                {
                    return (value + ";");
                }
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstScript"></param>
        /// <param name="secondScript"></param>
        /// <returns></returns>
        internal static string MergeScript(string firstScript, string secondScript)
        {
            if (!string.IsNullOrEmpty(firstScript))
            {
                return (firstScript + secondScript);
            }
            if (secondScript.TrimStart(new char[0]).StartsWith("javascript:", StringComparison.Ordinal))
            {
                return secondScript;
            }
            return ("javascript:" + secondScript);
        }


    }
}
