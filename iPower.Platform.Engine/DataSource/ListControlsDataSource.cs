//================================================================================
//  FileName: ListControlsDataSource.cs
//  Desc:列表控件数据源。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-23
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

namespace iPower.Platform.Engine.DataSource
{
    /// <summary>
    /// 列表控件数据源。
    /// </summary>
    public class ListControlsDataSource : IListControlsData
    {
        #region 成员变量，构造函数。
        string dataTextField, dataTextFormatString, dataValueField, orderNoField;
        object dataSource;
        /// <summary>
        ///  构造函数。
        /// </summary>
        /// <param name="dataTextField">显示字段。</param>
        /// <param name="dataTextFormatString">显示数据格式。</param>
        /// <param name="dataValueField">值字段。</param>
        /// <param name="orderNoField">排序字段。</param>
        /// <param name="dataSource">数据源。</param>
        public ListControlsDataSource(string dataTextField, string dataTextFormatString, string dataValueField, string orderNoField, object dataSource)
        {
            this.dataTextField = dataTextField;
            this.dataTextFormatString = dataTextFormatString;
            this.dataValueField = dataValueField;
            this.orderNoField = orderNoField;
            this.dataSource = dataSource;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="dataTextField">显示字段。</param>
        /// <param name="dataValueField">值字段。</param>
        /// <param name="dataSource">数据源。</param>
        public ListControlsDataSource(string dataTextField, string dataValueField, object dataSource)
            : this(dataTextField, null, dataValueField, dataTextField, dataSource)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ListControlsDataSource()
            : this(null, null, null)
        {
        }
        #endregion

        #region IListControlsData 成员
        /// <summary>
        /// 获取或设置显示字段。
        /// </summary>
        public virtual string DataTextField
        {
            get
            {
                return this.dataTextField;
            }
            set
            {
                this.dataTextField = value;
            }
        }
        /// <summary>
        /// 获取或设置显示数据格式。
        /// </summary>
        public virtual string DataTextFormatString
        {
            get
            {
                return this.dataTextFormatString;
            }
            set
            {
                this.dataTextFormatString = value;
            }
        }
        /// <summary>
        /// 获取或设置值字段。
        /// </summary>
        public virtual string DataValueField
        {
            get
            {
                return this.dataValueField;
            }
            set
            {
                this.dataValueField = value;
            }
        }
        /// <summary>
        /// 获取或设置排序字段。
        /// </summary>
        public virtual string OrderNoField
        {
            get
            {
                return this.orderNoField;
            }
            set
            {
                this.orderNoField = value;
            }
        }
        /// <summary>
        /// 获取或设置数据源。
        /// </summary>
        public virtual object DataSource
        {
            get
            {
                return this.dataSource;
            }
            set
            {
                this.dataSource = value;
            }
        }

        #endregion
    }

    /// <summary>
    ///  列表控件树型展示数据源。
    /// </summary>
    public class ListControlsTreeViewDataSource : ListControlsDataSource, IListControlsTreeViewData
    {
        #region 成员变量，构造函数。
        string parentDataValueField;
         /// <summary>
        ///  构造函数。
        /// </summary>
        /// <param name="dataTextField">显示字段。</param>
        /// <param name="dataTextFormatString">显示数据格式。</param>
        /// <param name="dataValueField">值字段。</param>
        /// <param name="parentDataValueField">父字段。</param>
        /// <param name="orderNoField">排序字段。</param>
        /// <param name="dataSource">数据源。</param>
        public ListControlsTreeViewDataSource(string dataTextField, string dataTextFormatString, string dataValueField, string parentDataValueField, string orderNoField, object dataSource)
            : base(dataTextField, dataTextFormatString, dataValueField, orderNoField, dataSource)
        {
            this.parentDataValueField = parentDataValueField;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="dataTextField">显示字段。</param>
        /// <param name="dataValueField">值字段。</param>
        /// <param name="parentDataValueField">父字段。</param>
        /// <param name="orderNoField">排序字段。</param>
        /// <param name="dataSource">数据源。</param>
        public ListControlsTreeViewDataSource(string dataTextField, string dataValueField, string parentDataValueField, string orderNoField, object dataSource)
            : this(dataTextField, null, dataValueField, parentDataValueField, orderNoField, dataSource)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="dataTextField">显示字段。</param>
        /// <param name="dataValueField">值字段。</param>
        /// <param name="parentDataValueField">父字段。</param>
        /// <param name="dataSource">数据源。</param>
        public ListControlsTreeViewDataSource(string dataTextField, string dataValueField, string parentDataValueField, object dataSource)
            : this(dataTextField, dataValueField, parentDataValueField, dataTextField, dataSource)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ListControlsTreeViewDataSource()
            : this(null, null, null, null, null)
        {
        }
        #endregion

        #region IListControlsTreeViewData 成员
        /// <summary>
        ///  获取或设置父字段。
        /// </summary>
        public string ParentDataValueField
        {
            get
            {
                return this.parentDataValueField;
            }
            set
            {
                this.parentDataValueField = value;
            }
        }

        #endregion
    }
}
