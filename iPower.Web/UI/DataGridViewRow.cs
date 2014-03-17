//================================================================================
//  FileName: DataGridViewRow.cs
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
    /// 表示 <see cref="DataGridView"/> 控件中的单独行。
    /// </summary>
    public class DataGridViewRow : TableRow, IDataItemContainer, INamingContainer
    {
        #region 成员变量，构造函数。
        object dataItem;
        int dataItemIndex, rowIndex;
        DataGridViewRowState rowState;
        DataGridViewRowType rowType;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="dataItemIndex"></param>
        /// <param name="rowType"></param>
        /// <param name="rowState"></param>
        public DataGridViewRow(int rowIndex, int dataItemIndex, DataGridViewRowType rowType, DataGridViewRowState rowState)
        {
            this.rowIndex = rowIndex;
            this.dataItemIndex = dataItemIndex;
            this.rowType = rowType;
            this.rowState = rowState;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取来自 <see cref="DataGridView"/> 控件的 <see cref="DataGridView.Rows"/>  集合的 <see cref="DataGridViewRow"/>  对象的索引。
        /// </summary>
        public virtual int RowIndex
        {
            get { return this.rowIndex; }
        }
        /// <summary>
        /// 获取或设置当前对象的行状态。
        /// </summary>
        public virtual DataGridViewRowState RowState
        {
            get { return this.rowState; }
            set { this.rowState = value; }
        }
        /// <summary>
        /// 获取或设置当前对象的行类型。
        /// </summary>
        public virtual DataGridViewRowType RowType
        {
            get { return this.rowType; }
            set { this.rowType = value; }
        }

        #endregion

        #region 重载。
        /// <summary>
        /// 确定是否将事件沿页面的 ASP.NET 服务器控件层次结构向上传递。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (args is CommandEventArgs)
            {
                DataGridViewCommandEventArgs e = new DataGridViewCommandEventArgs(this, source, (CommandEventArgs)args);
                base.RaiseBubbleEvent(this, e);
                return true;
            }
            return false;
        }
        #endregion

        #region IDataItemContainer 成员
        /// <summary>
        /// 获取或设置在简化的数据绑定操作中所使用的 object。
        /// </summary>
        public virtual object DataItem
        {
            get { return this.dataItem; }
            set { this.dataItem = value; }
        }
        /// <summary>
        /// 获取绑定到控件的数据项的索引。
        /// </summary>
        public virtual int DataItemIndex
        {
            get { return this.dataItemIndex; }
        }
        /// <summary>
        /// 获取在控件中所显示的数据项的位置。
        /// </summary>
        public int DisplayIndex
        {
            get { return this.RowIndex; }
        }

        #endregion
    }
    /// <summary>
    /// 指定数据控件中行的状态。
    /// </summary>
    [Flags]
    public enum DataGridViewRowState
    {
        /// <summary>
        /// 正常行。
        /// </summary>
        Normal = 0x00,
        /// <summary>
        /// 交替行。
        /// </summary>
        AlterNate = 0x01,
        /// <summary>
        /// 选中行。
        /// </summary>
        Selected = 0x02
    }
    /// <summary>
    ///  指定数据控件中行的类型。
    /// </summary>
    public enum DataGridViewRowType
    {
        /// <summary>
        /// 标题行。
        /// </summary>
        Header,
        /// <summary>
        /// 脚注行。
        /// </summary>
        Footer,
        /// <summary>
        /// 数据行。
        /// </summary>
        DataRow
    }
}
