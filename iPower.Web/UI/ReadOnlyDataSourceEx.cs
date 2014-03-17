//================================================================================
//  FileName: ReadOnlyDataSourceEx.cs
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
    public sealed class ReadOnlyDataSourceEx : IDataSourceEx
    {
        #region 成员变量，构造函数。
        string dataMember;
        object dataSource;
        static string[] ViewNames = new string[0];
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReadOnlyDataSourceEx(object dataSource, string dataMember)
        {
            this.dataSource = dataSource;
            this.dataMember = dataMember;
        }
        #endregion

        #region IDataSourceEx 成员
        /// <summary>
        /// 
        /// </summary>
        event EventHandler IDataSourceEx.DataSourceChanged
        {
            add
            {
            }
            remove
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public DataSourceViewEx GetView(string viewName)
        {
            IDataSourceEx source = this.dataSource as IDataSourceEx;
            if (source != null)
                return source.GetView(viewName);
            return new ReadOnlyDataSourceViewEx(this, this.dataMember, DataSourceHelper.GetResolvedDataSource(this.dataSource, this.dataMember));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection GetViewNames()
        {
            return ViewNames;
        }

        #endregion
    }
}
