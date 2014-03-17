//================================================================================
//  FileName: CallResult.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010-3-1
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
using System.Xml.Serialization;
namespace iPower
{
    /// <summary>
    /// 调用返回结果。
    /// </summary>
    [Serializable]
    public class CallResult
    {
        #region 成员变量，构造函数。
        int resultCode;
        string resultMessage;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CallResult()
            : this(-1, "未知结果！")
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="resultCode">结果值。</param>
        /// <param name="resultMessage">结果信息。</param>
        public CallResult(int resultCode, string resultMessage)
        {
            this.resultCode = resultCode;
            this.resultMessage = resultMessage;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置结果值。
        /// </summary>
        public virtual int ResultCode
        {
            get { return this.resultCode; }
            set { this.resultCode = value; }
        }
        /// <summary>
        /// 获取或设置结果信息。
        /// </summary>
        public virtual string ResultMessage
        {
            get { return this.resultMessage; }
            set { this.resultMessage = value; }
        }
        #endregion
    }
}
