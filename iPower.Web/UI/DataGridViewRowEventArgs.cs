//================================================================================
//  FileName: DataGridViewRowEventArgs.cs
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
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示处理 <see cref="DataGridView"/> 控件的 <see cref="DataGridView.RowCreated"/> 和 <see cref="DataGridView.RowDataBound"/> 事件的方法。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DataGridViewRowEventHandler(object sender, DataGridViewRowEventArgs e);
    /// <summary>
    /// 提供有关 <see cref="DataGridView.RowCreated"/> 和<see cref="DataGridView.RowDataBound"/>  事件的数据。
    /// </summary>
    public class DataGridViewRowEventArgs : EventArgs
    {
        #region 成员变量，构造函数。
        DataGridViewRow row;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="row"></param>
        public DataGridViewRowEventArgs(DataGridViewRow row)
        {
            this.row = row;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取行对象。
        /// </summary>
        public DataGridViewRow Row
        {
            get { return this.row; }
        }
        #endregion
    }
}
