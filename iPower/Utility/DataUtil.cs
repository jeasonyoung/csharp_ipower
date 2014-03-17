//================================================================================
//  FileName: DataUtil.cs
//  Desc: 数据集处理工具类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-29
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
using System.Data;

namespace iPower.Utility
{
    /// <summary>
    /// 数据集处理工具类。
    /// </summary>
    public static class DataUtil
    {
        /// <summary>
        /// 获取拥有一致数据记录的<see cref="System.Data.DataTable"/>对象。
        /// </summary>
        /// <param name="view">需要转化的
        /// <see cref="System.Data.DataView"/>对象，一般是另外一个DataTable通过过滤获得。</param>
        /// <returns>返回view结构一致、数据一致的System.Data.DataTable对象。</returns>
        public static DataTable GetDataTable(DataView view)
        {
            if (view == null)
            {
                return null;
            }
            DataTable table = view.Table.Clone();
            foreach (DataRowView view2 in view)
            {
                table.ImportRow(view2.Row);
            }
            return table;
        }

        #region DataSource2IEnumerator
        /// <summary>
        /// 将object类型的数据转化为IEnumerator对象,现在只支持DataSet,DataTable,DataView。
        /// </summary>
        /// <param name="oDataSource">现在只支持DataSet<see cref="DataSet"/>,DataTable<see cref="DataTable"/>,DataView<see cref="DataView"/>对象。</param>
        /// <returns>返回IEnumerator<see cref="IEnumerator"/>接口。</returns>
        public static IEnumerator DataSource2IEnumerator(object oDataSource)
        {
            if (oDataSource is DataSet)
                return ((DataSet)oDataSource).Tables[0].DefaultView.GetEnumerator();
            else if (oDataSource is DataTable)
                return ((DataTable)oDataSource).DefaultView.GetEnumerator();
            else if (oDataSource is DataView)
                return ((DataView)oDataSource).GetEnumerator();
            else
                return null;
        }
        #endregion
        /// <summary>
        /// 将object类型的数据转化为DataTable对象,现在只支持DataSet,DataTable,DataView。
        /// </summary>
        /// <param name="oDataSource">现在只支持DataSet<see cref="DataSet"/>,DataTable<see cref="DataTable"/>,DataView<see cref="DataView"/>对象。</param>
        /// <returns>返回DataTable<see cref="DataTable"/>对象。</returns>
        public static DataTable DataSourceToDataTable(object oDataSource)
        {
            if (oDataSource is DataSet)
                return ((DataSet)oDataSource).Tables[0].Copy();
            else if (oDataSource is DataTable)
                return ((DataTable)oDataSource).Copy();
            else if (oDataSource is DataView)
                return DataUtil.GetDataTable((DataView)oDataSource);
            else
                return null;
        }
    }
}
