//================================================================================
//  FileName: ReadOnlyDataSourceViewEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/4
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace iPower.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ReadOnlyDataSourceViewEx : DataSourceViewEx
    {
        #region 成员变量，构造函数。
        IEnumerable dataSource;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReadOnlyDataSourceViewEx(ReadOnlyDataSourceEx owner, string name, IEnumerable dataSource)
            : base(owner, name)
        {
            this.dataSource = dataSource;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        protected internal override IEnumerable ExecuteSelect(DataSourceSelectArgumentsEx arguments)
        {
            //arguments.RaiseUnsupportedCapabilitiesError(this);
            return this.dataSource;
        }
    }
}
