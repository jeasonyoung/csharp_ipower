//================================================================================
//  FileName: OleDBAccess.cs
//  Desc:OleDb���ݿ���ʡ�
//
//  Called by
//
//  Auth:���£�jeason1914@gmail.com��
//  Date: 2009-11-16
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//2009-12-30 yangyong �޸���������
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace iPower.Data.DataAccess
{
    /// <summary>
    /// OleDb���ݿ���ʡ�
    /// </summary>
    public class OleDBAccess : DBAccess
    {
        #region ���캯��,��������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="connection"></param>
        public OleDBAccess(string connection)
            : base(connection)
        {
        }
        /// <summary>
        /// ����������
        /// </summary>
        ~OleDBAccess()
        {
            this.CloseConnection();
        }
        #endregion

        #region ���ء�
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public override bool BeginTransaction()
        {
            if (this.HasTransaction())
                return false;
            return base.BeginTransaction();
        }
        /// <summary>
        /// �ַ����ˡ�
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string ConvertToDBString(string context)
        {
            return context;
        }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        protected override DbDataAdapter CreateAdapter()
        {
            return new OleDbDataAdapter();
        }
        /// <summary>
        /// �������ӡ�
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected override DbConnection CreateConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dtValue"></param>
        /// <returns></returns>
        protected override int OnBatchBulkCopy(string tableName, DataTable dtValue)
        {
            throw new NotImplementedException("�÷���Ŀǰֻ֧��SqlServer��");
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
                List<OleDbParameter> listParams = null;
                
                #region command and params
                switch (opera)
                {
                    case BuildCommandOpera.Insert:
                        {
                            List<string> listCols = new List<string>(), listColsParams = new List<string>();
                            listParams = new List<OleDbParameter>();
                            foreach (DataColumn dc in cols)
                            {
                                string colName = dc.ColumnName.ToUpper();
                                listCols.Add(colName);
                                listColsParams.Add("@" + colName);
                                OleDbParameter p = new OleDbParameter("@" + colName, this.ConvertTo(dc.DataType));
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
                            listParams = new List<OleDbParameter>();
                            foreach (DataColumn dc in cols)
                            {
                                bool isfind = Array.Exists<DataColumn>(pks, new Predicate<DataColumn>(delegate(DataColumn v)
                                {
                                    return (v != null) && string.Equals(dc.ColumnName, v.ColumnName, StringComparison.InvariantCultureIgnoreCase);
                                }));
                                string colName = dc.ColumnName.ToUpper();
                                OleDbParameter p = new OleDbParameter("@" + colName, this.ConvertTo(dc.DataType));
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
                            listParams = new List<OleDbParameter>();
                            foreach (DataColumn dc in pks)
                            {
                                string colName = dc.ColumnName.ToUpper();
                                OleDbParameter p = new OleDbParameter("@" + colName, this.ConvertTo(dc.DataType));
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
                    OleDbCommand cmd = new OleDbCommand(cmdText.ToString(), (OleDbConnection)this.Connection);
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
