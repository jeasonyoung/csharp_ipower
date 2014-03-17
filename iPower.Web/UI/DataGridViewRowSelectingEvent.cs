//================================================================================
//  FileName: DataGridViewRowSelectingEvent.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/7/8
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

using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 选中事件处理。
    /// </summary>
    partial class DataGridView
    {
        /// <summary>
        /// 选中事件。由<see cref="BoundFieldEx.ShowRowSelectingEvent"/>触发。
        /// </summary>
        [Category("Events")]
        [Description("选中事件。")]
        public event EventHandler RowSelecting;

        /// <summary>
        /// 触发事件。
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnRowSelecting(object data)
        {
            EventHandler handler = this.RowSelecting;
            if (handler != null)
            {
                handler(data, EventArgs.Empty);
            }
        }
    }
}
