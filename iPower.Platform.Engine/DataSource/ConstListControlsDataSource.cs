//================================================================================
//  FileName: ConstListControlsDataSource.cs
//  Desc:枚举下拉数据源。
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

using System.Data;
using iPower.Data.DataAccess;
using iPower.Platform.Engine.Persistence;
namespace iPower.Platform.Engine.DataSource
{
    /// <summary>
    /// 枚举下拉数据源。
    /// </summary>
    public class ConstListControlsDataSource<K> : ListControlsDataSource
        where K : BaseModuleConfiguration
    {
        #region 成员变量，构造函数。
        Type enumClassType;
        int[] ignoreValues;

        CommonEnumsEntity<K> commEnumsData = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="enumClassType">枚举类型。</param>
        /// <param name="ignoreValues">过滤的值。</param>
        /// <param name="config">模块配置实例。</param>
        public ConstListControlsDataSource(Type enumClassType, int[] ignoreValues, K config)
        {
            this.enumClassType = enumClassType;
            this.ignoreValues = ignoreValues;

            this.commEnumsData = new CommonEnumsEntity<K>(config);
        }
        /// <summary>
        ///  构造函数。
        /// </summary>
        /// <param name="enumClassType">枚举类型。</param>
        /// <param name="config">模块配置实例。</param>
        public ConstListControlsDataSource(Type enumClassType, K config)
            : this(enumClassType, null, config)
        {
        }
        /// <summary>
        ///  构造函数。
        /// </summary>
        /// <param name="config">模块配置实例。</param>
        public ConstListControlsDataSource(K config)
            : this(null,config)
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置枚举类型。
        /// </summary>
        public Type EnumClassType
        {
            get { return this.enumClassType; }
            set { this.enumClassType = value; }
        }
        /// <summary>
        /// 获取或设置过滤的值。
        /// </summary>
        public int[] IgnoreValues
        {
            get { return this.ignoreValues; }
            set { this.ignoreValues = value; }
        }
        #endregion

        #region 重载。
        /// <summary>
        ///  获取显示字段。
        /// </summary>
        public override string DataTextField
        {
            get
            {
                return "MemberName";
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        /// <summary>
        /// 获取值字段。
        /// </summary>
        public override string DataValueField
        {
            get
            {
                return "IntValue";
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        /// <summary>
        /// 获取数据源。
        /// </summary>
        public override object DataSource
        {
            get
            {
                lock (this)
                {
                    DataTable dtSource = this.commEnumsData.LoadEnumAndConst(this.EnumClassType.FullName);
                    if (dtSource != null)
                    {
                        dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["IntValue"] };
                        if (this.IgnoreValues != null)
                        {
                            DataRow row = null;
                            foreach (int num in this.IgnoreValues)
                            {
                                row = dtSource.Rows.Find(num);
                                if (row != null)
                                    dtSource.Rows.Remove(row);
                            }
                            dtSource.AcceptChanges();
                        }
                        return dtSource.Copy();
                    }
                    return null;
                }
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        #endregion
    }
}
