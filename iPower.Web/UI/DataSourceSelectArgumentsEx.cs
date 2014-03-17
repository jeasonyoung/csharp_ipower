//================================================================================
//  FileName: DataSourceSelectArgumentsEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/3
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
using System.Web.UI;
using System.Web.UI.WebControls;

using iPower.Utility;
namespace iPower.Web.UI
{
    /// <summary>
    /// 提供一种机制，数据绑定控件可使用这种机制向数据源控件请求在检索数据之后执行与数据相关的操作。
    /// </summary>
    public class DataSourceSelectArgumentsEx
    {
        #region 成员变量，构造函数。
        int maximumRows, startRowIndex, totalRowCount;
        DataSourceCapabilities requestedCapabilities, supportedCapabilities;
        bool retrieveTotalRowCount;
        string sortExpression;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DataSourceSelectArgumentsEx()
            : this(string.Empty, 0, 0)
        {

        }
        /// <summary>
        ///  构造函数。
        /// </summary>
        /// <param name="sortExpression">用来排序数据检索操作结果的排序表达式。</param>
        public DataSourceSelectArgumentsEx(string sortExpression)
            : this(sortExpression, 0, 0)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="startRowIndex">数据行的索引，标记数据检索操作返回数据的起始位置。</param>
        /// <param name="maximumRows">数据检索操作返回的最大行数。</param>
        public DataSourceSelectArgumentsEx(int startRowIndex, int maximumRows)
            : this(string.Empty, startRowIndex, maximumRows)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sortExpression">用来排序数据检索操作结果的排序表达式。</param>
        /// <param name="startRowIndex">数据行的索引，标记数据检索操作返回数据的起始位置。</param>
        /// <param name="maximumRows">数据检索操作返回的最大行数。</param>
        public DataSourceSelectArgumentsEx(string sortExpression, int startRowIndex, int maximumRows)
        {
            this.totalRowCount = -1;
            this.SortExpression = sortExpression;
            this.StartRowIndex = startRowIndex;
            this.MaximumRows = maximumRows;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取一个排序表达式设置为 Empty 的 <see cref="DataSourceSelectArgumentsEx"/>对象。 
        /// </summary>
        public static DataSourceSelectArgumentsEx Empty
        {
            get
            {
                return new DataSourceSelectArgumentsEx();
            }
        }
        /// <summary>
        /// 获取或设置在一次数据检索操作中，数据源控件返回的最大数据行数。
        /// </summary>
        public int MaximumRows
        {
            get
            {
                return this.maximumRows;
            }
            set
            {
                if (value == 0)
                {
                    if (this.startRowIndex == 0)
                        this.requestedCapabilities &= ~DataSourceCapabilities.Page;
                }
                else
                    this.requestedCapabilities |= DataSourceCapabilities.Page;
                this.maximumRows = value;
            }
        }
        /// <summary>
        /// 获取或设置在数据检索操作过程中，数据源控件是否应检索所有数据行的计数。
        /// </summary>
        public bool RetrieveTotalRowCount
        {
            get { return this.retrieveTotalRowCount; }
            set
            {
                if (value)
                    this.requestedCapabilities |= DataSourceCapabilities.RetrieveTotalRowCount;
                else
                    this.requestedCapabilities &= ~DataSourceCapabilities.RetrieveTotalRowCount;
                this.retrieveTotalRowCount = value;
            }
        }
        /// <summary>
        /// 获取或设置数据源视图使用该表达式对 Select(DataSourceSelectArguments, DataSourceViewSelectCallback) 方法检索的数据进行排序。
        /// </summary>
        public string SortExpression
        {
            get
            {
                if (this.sortExpression == null)
                    this.sortExpression = string.Empty;
                return this.sortExpression;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.requestedCapabilities &= ~DataSourceCapabilities.Sort;
                else
                    this.requestedCapabilities |= DataSourceCapabilities.Sort;
                this.sortExpression = value;
            }
        }
        /// <summary>
        /// 获取或设置在数据检索操作过程中，检索数据行时数据源控件应使用的起始位置。
        /// </summary>
        public int StartRowIndex
        {
            get { return this.startRowIndex; }
            set
            {
                if (value == 0)
                {
                    if (this.maximumRows == 0)
                        this.requestedCapabilities &= ~DataSourceCapabilities.Page;
                }
                else
                    this.requestedCapabilities |= DataSourceCapabilities.Page;
                this.startRowIndex = value;
            }
        }
        /// <summary>
        /// 获取或设置在数据检索操作过程中检索的行数。
        /// </summary>
        public int TotalRowCount
        {
            get { return this.totalRowCount; }
            set { this.totalRowCount = value; }
        }
        #endregion 

        /// <summary>
        /// 向  <see cref="DataSourceSelectArgumentsEx"/> 实例添加一个功能，用于对支持的功能和请求的功能进行比较。 
        /// </summary>
        /// <param name="capabilities"></param>
        public void AddSupportedCapabilities(DataSourceCapabilities capabilities)
        {
            this.supportedCapabilities |= capabilities;
        }
        /// <summary>
        /// 将为 ExecuteSelect(DataSourceSelectArguments) 操作请求的功能与指定的数据源视图所支持的功能进行比较。
        /// </summary>
        /// <param name="view"></param>
        public void RaiseUnsupportedCapabilitiesError(DataSourceViewEx view)
        {
            DataSourceCapabilities capabilities = this.requestedCapabilities & ~this.supportedCapabilities;
            if ((capabilities & DataSourceCapabilities.Sort) != DataSourceCapabilities.None)
                view.RaiseUnsupportedCapabilityError(DataSourceCapabilities.Sort);

            if ((capabilities & DataSourceCapabilities.Page) != DataSourceCapabilities.None)
                view.RaiseUnsupportedCapabilityError(DataSourceCapabilities.Page);

            if ((capabilities & DataSourceCapabilities.RetrieveTotalRowCount) != DataSourceCapabilities.None)
                view.RaiseUnsupportedCapabilityError(DataSourceCapabilities.RetrieveTotalRowCount);
        }
        /// <summary>
        /// 实例是否等于当前实例。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            DataSourceSelectArgumentsEx arguments = obj as DataSourceSelectArgumentsEx;
            if (arguments == null)
                return false;

            return (arguments.MaximumRows == this.maximumRows) &&
                   (arguments.RetrieveTotalRowCount == this.retrieveTotalRowCount) &&
                   (arguments.SortExpression == this.sortExpression) &&
                   (arguments.StartRowIndex == this.startRowIndex) &&
                   (arguments.TotalRowCount == this.totalRowCount);
        }
        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCodeCombiner.CombineHashCodes(this.maximumRows.GetHashCode(),
                                                     this.requestedCapabilities.GetHashCode(),
                                                     this.sortExpression.GetHashCode(),
                                                     this.startRowIndex.GetHashCode(),
                                                     this.totalRowCount.GetHashCode());
        }
    }
}
