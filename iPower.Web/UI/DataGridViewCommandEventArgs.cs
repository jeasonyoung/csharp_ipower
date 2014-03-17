//================================================================================
//  FileName: DataGridViewCommandEventArgs.cs
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
    /// 单击控件按钮委托。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DataGridViewCommandEventHandler(object sender,DataGridViewCommandEventArgs e);
    /// <summary>
    /// 为 RowCommand 事件提供数据。
    /// </summary>
    public class DataGridViewCommandEventArgs : CommandEventArgs
    {
        #region 成员变量，构造函数。
        object commandSource;
        DataGridViewRow row;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DataGridViewCommandEventArgs(object commandSource, CommandEventArgs args)
            : base(args)
        {
            this.commandSource = commandSource;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="row"></param>
        /// <param name="commandSource"></param>
        /// <param name="args"></param>
        public DataGridViewCommandEventArgs(DataGridViewRow row, object commandSource, CommandEventArgs args)
            : base(args)
        {
            this.row = row;
            this.commandSource = commandSource;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取命令源。
        /// </summary>
        public object CommandSource
        {
            get { return this.commandSource; }
        }
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
