//================================================================================
//  FileName: DataGridViewSorting.cs
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
using System.Data;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 排序处理部分。
    /// </summary>
    partial class DataGridView
    {
        #region 属性。
        /// <summary>
        /// 获取或设置是否启用排序功能。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置是否启用排序功能。")]
        [DefaultValue(false)]
        public virtual bool AllowSorting
        {
            get
            {
                object obj = this.ViewState["AllowSorting"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                bool allowSorting = this.AllowSorting;
                if (value != allowSorting)
                {
                    this.ViewState["AllowSorting"] = value;
                    if (base.Initialized)
                        base.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// 获取或设置正在排序的列的排序方向。
        /// </summary>
        [Category("Sorting")]
        [Description("获取或设置正在排序的列的排序方向。")]
        [DefaultValue(0)]
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SortDirection SortDirection
        {
            get
            {
                object obj = this.ViewState["SortDirection"];
                if (obj != null)
                    return (SortDirection)obj;
                return SortDirection.Ascending;
            }
            set
            {
                if ((value < SortDirection.Ascending) || (value > SortDirection.Descending))
                    throw new ArgumentOutOfRangeException("value");
                if (value != this.SortDirection)
                    this.ViewState["SortDirection"] = value;
            }
        }

        /// <summary>
        /// 获取或设置排序表达式。
        /// </summary>
        [Category("Sorting")]
        [Description("获取或设置排序表达式。")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SortExpression
        {
            get
            {
                object obj = this.ViewState["SortExpression"];
                return obj == null ? string.Empty : (string)obj;
             }
            set
            {
                if (this.SortExpression != value)
                    this.ViewState["SortExpression"] = value;
            }
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 排序操作进行处理之前发生。
        /// </summary>
        [Category("Action")]
        [Description("排序操作进行处理之前发生。")]
        public event DataGridViewSortEventHandler Sorting;
        /// <summary>
        /// 触发<see cref="Sorting"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSorting(DataGridViewSortEventArgs e)
        {
            DataGridViewSortEventHandler handler = this.Sorting;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// 排序操作进行处理之后发生。
        /// </summary>
        [Category("Action")]
        [Description("排序操作进行处理之前发生。")]
        public event EventHandler Sorted;
        /// <summary>
        /// 触发<see cref="Sorted"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSorted(EventArgs e)
        {
            EventHandler handler = this.Sorted;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        /// <summary>
        /// 构建回调参数字符串。
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        protected string BuildCallbackArgument(string sortExpression, SortDirection sortDirection)
        {
            return string.Concat(new object[] { "\"", this.PageIndex, "|", (int)sortDirection, "|", this.StateFormatter.Serialize(sortExpression), "|\"" });
        }
        string BuildCallbackArgument(int pageIndex)
        {
            string sortExpressionSerialized = this.StateFormatter.Serialize(this.SortExpression);
            return string.Concat(new object[] { "\"", pageIndex, "|", (int)this.SortDirection, "|", sortExpressionSerialized, "|\"" });
        }
        void HandleSort(string sortExpression)
        {
            if (this.AllowSorting)
            {
                SortDirection ascending = SortDirection.Ascending;
                if ((this.SortExpression == sortExpression) && (this.SortDirection == SortDirection.Ascending))
                    ascending = SortDirection.Descending;
                this.HandleSort(sortExpression, ascending);
            }
        }
        void HandleSort(string sortExpression, SortDirection sortDirection)
        {
            DataGridViewSortEventArgs e = new DataGridViewSortEventArgs(sortExpression, sortDirection);
            this.OnSorting(e);
            if (!e.Cancel)
            {
                string strHeaderText = string.Empty;
                foreach (DataControlFieldEx field in this.Columns)
                {
                    strHeaderText = field.HeaderText;
                    if (!string.IsNullOrEmpty(strHeaderText))
                    {
                        //清除所有的排序方向标志&uarr;&darr;
                        strHeaderText = strHeaderText.Replace("↑", "").Replace("↓", "");
                        //给当前排序列加方向标志
                        if (e.SortExpression == field.SortExpression)
                        {
                            strHeaderText += (e.SortDirection == SortDirection.Ascending) ? "↑" : "↓";
                        }
                        //重新设置排序方向标志
                        field.HeaderText = strHeaderText;
                    }
                }
                this.SortExpression = e.SortExpression;
                this.SortDirection = e.SortDirection;
                this.PageIndex = 0;
                this.OnSorted(EventArgs.Empty);
                this.RequiresDataBinding = true;
            }
        }
        /// <summary>
        /// 数据排序。
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        protected override object DataSourceSorting(object dataSource)
        {
            string sortExpression = this.SortExpression;
            if (dataSource != null && !string.IsNullOrEmpty(sortExpression))
            {
                DataView vw = null;
                if (dataSource is DataSet)
                    vw = ((DataSet)dataSource).Tables[0].DefaultView;
                else if (dataSource is DataTable)
                    vw = ((DataTable)dataSource).DefaultView;
                else if (dataSource is DataView)
                    vw = (DataView)dataSource;
                if (vw != null)
                {
                    if (this.SortDirection == SortDirection.Descending)
                        sortExpression += " desc";
                    vw.Sort = sortExpression;
                    dataSource = vw;
                }
            }
            return dataSource;
        }
    }
    /// <summary>
    /// 排序委托。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DataGridViewSortEventHandler(object sender,DataGridViewSortEventArgs e); 
    /// <summary>
    /// 为排序事件提供数据。
    /// </summary>
    public class DataGridViewSortEventArgs : CancelEventArgs
    {
        #region 成员变量，构造函数。
        SortDirection sortDirection;
        string sortExpression;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        public DataGridViewSortEventArgs(string sortExpression, SortDirection sortDirection)
        {
            this.sortExpression = sortExpression;
            this.sortDirection = sortDirection;
        }
        #endregion

        /// <summary>
        /// 获取或设置排序方向。
        /// </summary>
        public SortDirection SortDirection
        {
            get { return this.sortDirection; }
            set { this.sortDirection = value; }
        }
        /// <summary>
        /// 获取或设置排序的表达式。
        /// </summary>
        public string SortExpression
        {
            get { return this.sortExpression; }
            set { this.sortExpression = value; }
        }
    }
}
