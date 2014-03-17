//================================================================================
//  FileName: CryptographyException.cs
//  Desc:加密异常。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-4
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

namespace iPower.Cryptography
{
    /// <summary>
    /// 加密异常。
    /// </summary>
    public class CryptographyException :BaseException
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="message">异常信息</param>
        public CryptographyException(string message)
            : base(message)
        {
        }
        #endregion
    }
}
