//================================================================================
//  FileName: SqlDBAccess.cs
//  Desc:SqlServer访问数据库。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-16
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//2009-12-30 yangyong 修改批量处理。
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace iPower.Data.DataAccess
{
    /// <summary>
    /// SqlServer访问数据库
    /// </summary>
    public sealed class SqlDBAccess : DBAccess
    {
        #region 构造函数,析构函数
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="connection"></param>
        public SqlDBAccess(string connection)
            : base(connection)
        {
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~SqlDBAccess()
        {
            this.CloseConnection();
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 启动事务。
        /// </summary>
        /// <returns></returns>
        public override bool BeginTransaction()
        {
            if (this.HasTransaction())
            {
                return false;
            }
            return base.BeginTransaction();
        }
        /// <summary>
        /// 字符过滤。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string ConvertToDBString(string context)
        {
            return context;
        }
        /// <summary>
        /// 创建适配器。
        /// </summary>
        /// <returns></returns>
        protected override DbDataAdapter CreateAdapter()
        {
            return new SqlDataAdapter();
        }
        /// <summary>
        /// 创建连接。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 批量插入。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dtValue"></param>
        /// <returns></returns>
        protected override int OnBatchBulkCopy(string tableName, DataTable dtValue)
        {
            int result = 0;
            try
            {
                this.OpenConnection();
                result = dtValue.Rows.Count;
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)this.Connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    foreach (DataColumn col in dtValue.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    bulkCopy.WriteToServer(dtValue);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.CloseConnection();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cols"></param>
        /// <param name="pks"></param>
        /// <param name="opera"></param>
        /// <returns></returns>
        protected override DbCommand BuildCommand(string tableName, DataColumnCollection cols, DataColumn[] pks, DBAccess.BuildCommandOpera opera)
        {
            if (!string.IsNullOrEmpty(tableName) && (cols != null && cols.Count > 0))
            {
                if ((opera == BuildCommandOpera.Update || opera == BuildCommandOpera.Delete) && (pks == null || pks.Length == 0))
                {
                    throw new ArgumentNullException("pks");
                }
                StringBuilder cmdText = null;
                List<SqlParameter> listParams = null;

                #region command and params
                switch (opera)
                {
                    case BuildCommandOpera.Insert:
                        {
                            List<string> listCols = new List<string>(), listColsParams = new List<string>();
                            listParams = new List<SqlParameter>();
                            foreach (DataColumn dc in cols)
                            {
                                string colName = dc.ColumnName.ToUpper();
                                listCols.Add(colName);
                                listColsParams.Add("@" + colName);
                                SqlParameter p = new SqlParameter("@" + colName, this.ConvertTo(dc.DataType));
                                p.SourceColumn = dc.ColumnName;
                                listParams.Add(p);
                            }
                            cmdText = new StringBuilder();
                            cmdText.AppendFormat("insert into {0} ({1}) values ({2})", tableName, string.Join(",", listCols.ToArray()), string.Join(",", listColsParams.ToArray()));
                        }
                        break;
                    case BuildCommandOpera.Update:
                        {
                            List<string> listSets = new List<string>(), listWheres = new List<string>();
                            listParams = new List<SqlParameter>();
                            foreach (DataColumn dc in cols)
                            {
                                bool isfind = Array.Exists<DataColumn>(pks, new Predicate<DataColumn>(delegate(DataColumn v)
                                {
                                    return (v != null) && string.Equals(dc.ColumnName, v.ColumnName, StringComparison.InvariantCultureIgnoreCase);
                                }));
                                string colName = dc.ColumnName.ToUpper();
                                SqlParameter p = new SqlParameter("@" + colName, this.ConvertTo(dc.DataType));
                                p.SourceColumn = dc.ColumnName;
                                listParams.Add(p);
                                if (!isfind)
                                {
                                    listSets.Add(string.Format("{0}=@{0}", colName));
                                }
                                else
                                {
                                    listWheres.Add(string.Format("{0}=@{0}", colName));
                                }
                            }
                            cmdText = new StringBuilder();
                            cmdText.AppendFormat("update {0} set {1} where {2}", tableName, string.Join(",", listSets.ToArray()), string.Join(" and ", listWheres.ToArray()));
                        }
                        break;
                    case BuildCommandOpera.Delete:
                        {
                            List<string> listWheres = new List<string>();
                            listParams = new List<SqlParameter>();
                            foreach (DataColumn dc in pks)
                            {
                                string colName = dc.ColumnName.ToUpper();
                                SqlParameter p = new SqlParameter("@" + colName, this.ConvertTo(dc.DataType));
                                p.SourceColumn = dc.ColumnName;
                                listParams.Add(p);
                                listWheres.Add(string.Format("{0}=@{0}", colName));
                            }
                            cmdText = new StringBuilder();
                            cmdText.AppendFormat("delete from {0} where {1}", tableName, string.Join(" and ", listWheres.ToArray()));
                        }
                        break;
                }
                #endregion

                if (cmdText != null && cmdText.Length > 0)
                {
                    SqlCommand cmd = new SqlCommand(cmdText.ToString(), (SqlConnection)this.Connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = this.CommandTimeout;
                    cmd.Parameters.AddRange(listParams.ToArray());
                    return cmd;
                }
            }
            return null;
        }
        #endregion
    }
}