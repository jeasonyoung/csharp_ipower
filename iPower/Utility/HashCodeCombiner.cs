//================================================================================
//  FileName: Utils.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/3
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

namespace iPower.Utility
{
    /// <summary>
    /// 哈希函数拼接工具类。
    /// </summary>
    public static class HashCodeCombiner
    {
        /// <summary>
        /// 拼接特定类型的哈希函数。
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <returns></returns>
        public static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }
        /// <summary>
        /// 拼接特定类型的哈希函数。
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <param name="h3"></param>
        /// <param name="h4"></param>
        /// <returns></returns>
        public static int CombineHashCodes(int h1, int h2, int h3, int h4)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
        }
        /// <summary>
        ///  拼接特定类型的哈希函数。
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <param name="h3"></param>
        /// <param name="h4"></param>
        /// <param name="h5"></param>
        /// <returns></returns>
        public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), h5);
        }
    }
}
