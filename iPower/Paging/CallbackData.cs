//================================================================================
//  FileName: CallbackData.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2014-1-8
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

namespace iPower.Paging
{
    /// <summary>
    /// 分页反馈数据。
    /// </summary>
    [Serializable]
    public class CallbackData : ICallbackData<object>
    {
        #region ICallbackData<T> 成员
        /// <summary>
        /// 获取或设置是否成功。
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 获取或设置反馈数据。
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 获取或设置反馈附加消息。
        /// </summary>
        public string Msg { get; set; }
        #endregion
    }
}