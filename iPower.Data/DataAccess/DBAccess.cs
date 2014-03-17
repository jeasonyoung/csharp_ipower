//================================================================================
//  FileName: DBAccess.cs
//  Desc:统一数据访问抽象类。
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Threading;

using iPower;
namespace iPower.Data.DataAccess
{
    /// <summary>
    /// 统一数据访问抽象类。
    /// </summary>
    public abstract class DBAccess : IDBAccess
    {
        #region 成员变量，构造函数。
        private static Hashtable schemaCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        public DBAccess(string connectionString)
        {
            Guard.ArgumentNotNullOrEmptyString("connectionString", connectionString, true);
            this.CommandTimeout = 300;
            this.Connection = this.CreateConnection(connectionString);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置命令超时时间(秒)。
        /// </summary>
        public int CommandTimeout { get; set; }
        /// <summary>
        /// 获取或设置数据库连接对象。
        /// </summary>
        protected DbConnection Connection { get; private set; }
        /// <summary>
        /// 获取或设置数据库事务对象。
        /// </summary>
        protected DbTransaction Transaction { get; private set; }
        /// <summary>
        /// 获取或设置数据库事务个数。
        /// </summary>
        protected int TransactionCount { get; private set; }
        #endregion

        #region 连接管理
        /// <summary>
        /// 打开数据连接。
        /// </summary>
        public void OpenConnection()
        {
            lock (this)
            {
                if (this.Connection != null)
                {
                    if (this.Connection.State != ConnectionState.Closed)
                    {
                        this.CloseConnection();
                    }
                    this.Connection.Open();
                }
            }
        }
        /// <summary>
        /// 关闭数据连接。
        /// </summary>
        public void CloseConnection()
        {
            this.CloseConnection(true);
        }
        /// <summary>
        /// 关闭数据连接。
        /// </summary>
        /// <param name="bCloseTrans">是否关闭事务。</param>
        public void CloseConnection(bool bCloseTrans)
        {
            lock (this)
            {
                if (this.Connection != null && (bCloseTrans || (this.TransactionCount <= 0)))
                {
                    if (this.Transaction != null)
                    {
                        this.Transaction.Dispose();
                        this.Transaction = null;
                        this.TransactionCount = 0;
                    }
                    this.Connection.Close();
                }
            }
        }
        #endregion

        #region 事务管理
        /// <summary>
        /// 开始事务。
        /// </summary>
        /// <returns>开始事务是否成功。</returns>
        public virtual bool BeginTransaction()
        {
            bool result;
            try
            {
                Monitor.Enter(this);
                this.OpenConnection();

                this.Transaction = this.Connection.BeginTransaction();
                this.TransactionCount += 1;

                this.CloseConnection(false);
                result = true;
            }
            catch(Exception e)
            {
                this.CloseConnection(true);
                throw e;
            }
            finally
            {
                Monitor.Exit(this);
            }
            return result;
        }
        /// <summary>
        /// 提交事务。
        /// </summary>
        /// <returns>提交事务是否成功。</returns>
        public bool CommitTransaction()
        {
            lock (this)
            {
                bool result = false;
                try
                {
                    if (this.Transaction != null &&
                        this.Transaction.Connection.State != ConnectionState.Closed)
                    {
                        this.Transaction.Commit();
                        this.TransactionCount -= 1;

                        this.CloseConnection(false);
                        result = true;
                    }
                }
                catch
                {
                    this.CloseConnection(true);
                    throw;
                }
                return result;
            }
        }
        /// <summary>
        /// 回滚事务。
        /// </summary>
        /// <returns>回滚事务是否成功。</returns>
        public bool RollbackTransaction()
        {
            lock (this)
            {
                bool result = false;
                try
                {
                    if (this.Transaction != null &&
                        this.Transaction.Connection.State != ConnectionState.Closed)
                    {
                        this.Transaction.Rollback();
                        this.TransactionCount = 0;
                        this.CloseConnection(false);
                        result = true;
                    }
                }
                catch
                {
                    this.CloseConnection(true);
                    throw;
                }
                return result;
            }
        }
        /// <summary>
        /// 是否有事务。
        /// </summary>
        /// <returns></returns>
        public bool HasTransaction()
        {
            return this.TransactionCount > 0;
        }
        #endregion

        #region 非法字符处理。
        /// <summary>
        /// 过滤非法字符。
        /// </summary>
        /// <param name="context">过滤字符。</param>
        /// <returns>过滤后的字符。</returns>
        public virtual string ConvertToDBString(string context)
        {
            return context;
        }
        #endregion

        #region 执行数据

        #region 查询数据。
        /// <summary>
        /// 执行命令返回数据集。
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>数据集。</returns>
        public DataSet ExecuteDataset(string commandText)
        {
            lock (this)
            {
                DataSet dsResult = null;
                try
                {
                    this.OpenConnection();
                    DbCommand cmd = this.Connection.CreateCommand();
                    if (this.HasTransaction())
                    {
                        cmd.Transaction = this.Transaction;
                    }
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.ConvertToDBString(commandText);
                    cmd.CommandTimeout = this.CommandTimeout;
                    DataAdapter adapter = this.CreateAdapter(cmd);
                    dsResult = new DataSet();
                    adapter.Fill(dsResult);
                }
                catch (Exception e)
                {
                    this.CloseConnection(true);
                    throw e;
                }
                finally
                {
                    this.CloseConnection(false);
                }
                return dsResult;
            }
        }
        #endregion

        #region 操作数据。
        /// <summary>
        /// 执行命令。
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>影响数据行数。</returns>
        public int ExecuteNonQuery(string commandText)
        {
            lock (this)
            {
                int num = 0;
                try
                {
                    this.OpenConnection();
                    IDbCommand cmd = this.Connection.CreateCommand();
                    if (this.HasTransaction())
                    {
                        cmd.Transaction = this.Transaction;
                    }
                    cmd.CommandType =  CommandType.Text;
                    cmd.CommandText = this.ConvertToDBString(commandText);
                    cmd.CommandTimeout = this.CommandTimeout;
                    num = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    this.CloseConnection(true);
                    throw e;
                }
                finally
                {
                    this.CloseConnection(false);
                }
                return num;
            }
        }
        #endregion

        #region 只读获取数据。
        /// <summary>
        /// 执行命令获取只进结果集，调用此方法后需要调用CloseConnection（）方法关闭连接。 
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>只进结果集。</returns>
        public IDataReader ExecuteReader(string commandText)
        {
            lock (this)
            {
                IDataReader reader = null;
                try
                {
                    this.OpenConnection();
                    DbCommand  cmd = this.Connection.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.ConvertToDBString(commandText);

                    cmd.CommandTimeout = this.CommandTimeout;
                    if (this.HasTransaction())
                    {
                        cmd.Transaction = this.Transaction;
                        return cmd.ExecuteReader();
                    }
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception e)
                {
                    this.CloseConnection(true);
                    throw e;
                }
                return reader;
            }
        }
      
        #endregion

        #region 结果集中第一行的第一列。
        /// <summary>
        /// 执行命令，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string commandText)
        {
            lock (this)
            {
                object obj = null;
                try
                {
                    this.OpenConnection();
                    IDbCommand cmd = this.Connection.CreateCommand();
                    if (this.HasTransaction())
                    {
                        cmd.Transaction = this.Transaction;
                    }
                    cmd.CommandType =  CommandType.Text;
                    cmd.CommandText = this.ConvertToDBString(commandText);
                    cmd.CommandTimeout = this.CommandTimeout;

                    obj = cmd.ExecuteScalar();
                }
                catch(Exception e)
                {
                    this.CloseConnection(true);
                    throw e;
                }
                finally
                {
                    this.CloseConnection(false);
                }
                return obj;
            }
        }
        #endregion

        #region 批处理数据。
        /// <summary>
        /// 批量插入数据。
        /// </summary>
        /// <param name="tableName">表名。</param>
        /// <param name="dtValue">数据(主键字段在dtValue中设置)。</param>
        /// <returns>影响数据行数。</returns>
        public virtual int BatchInsert(string tableName, DataTable dtValue)
        {
            //this.OnBatchBulkCopy(tableName, dtValue);
            return this.OnBatchUpdate(tableName, dtValue, BuildCommandOpera.Insert);
        }
        /// <summary>
        /// 批量更新数据。
        /// </summary>
        /// <param name="tableName">表名。</param>
        /// <param name="dtValue">数据(主键字段在dtValue中设置)。</param>
        /// <returns>影响数据行数。</returns>
        public virtual int BatchUpdate(string tableName, DataTable dtValue)
        {
            return this.OnBatchUpdate(tableName, dtValue, BuildCommandOpera.Update);
        }
        /// <summary>
        /// 批量删除数据。
        /// </summary>
        /// <param name="tableName">表名。</param>
        /// <param name="dtValue">数据(主键字段在dtValue中设置)。</param>
        /// <returns>影响数据行数。</returns>
        public virtual int BatchDelete(string tableName, DataTable dtValue)
        {
            return this.OnBatchUpdate(tableName, dtValue, BuildCommandOpera.Delete);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dtValue"></param>
        /// <param name="opera"></param>
        /// <returns></returns>
        protected virtual int OnBatchUpdate(string tableName, DataTable dtValue, BuildCommandOpera opera)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (dtValue == null)
            {
                throw new ArgumentNullException("dtValue");
            }
            DataColumnCollection cols = dtValue.Columns;
            DataColumn[] pks = dtValue.PrimaryKey;

            #region 整理行数据状态。
            dtValue.AcceptChanges();
            foreach (DataRow row in dtValue.Rows)
            {
                if (opera == BuildCommandOpera.Insert)
                {
                    row.SetAdded();
                }
                else if (opera == BuildCommandOpera.Update)
                {
                    row.SetModified();
                }
                else if (opera == BuildCommandOpera.Delete)
                {
                    row.Delete();
                }
            }
            #endregion

            DbDataAdapter adapter = this.CreateAdapter();
            if (adapter != null)
            {
                DbCommand cmd = this.BuildCommand(tableName, cols, pks, opera);
                cmd.UpdatedRowSource = UpdateRowSource.None;
                if (opera == BuildCommandOpera.Insert)
                {
                    adapter.InsertCommand = cmd;
                }
                else if (opera == BuildCommandOpera.Update)
                {
                    adapter.UpdateCommand = cmd;
                }
                else if (opera == BuildCommandOpera.Delete)
                {
                    adapter.DeleteCommand = cmd;
                }
                int count = dtValue.Rows.Count;
                adapter.UpdateBatchSize = count < 500 ? count : 500;
                return adapter.Update(dtValue);
            }
            return 0;
        }
        /// <summary>
        /// 批量复制数据。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dtValue"></param>
        /// <returns></returns>
        protected abstract int OnBatchBulkCopy(string tableName, DataTable dtValue);
        #endregion
        #endregion

        #region 抽象方法
        /// <summary>
        /// 创建适配器。
        /// </summary>
        /// <returns>适配器。</returns>
        protected abstract DbDataAdapter CreateAdapter();
        /// <summary>
        /// 创建连接。
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>连接对象</returns>
        protected abstract DbConnection CreateConnection(string connectionString);   
        #endregion 

        #region 辅助函数。
        /// <summary>
        /// 创建适配器。
        /// </summary>
        /// <param name="command">IDbCommand</param>
        /// <returns>适配器对象</returns>
        protected DbDataAdapter CreateAdapter(DbCommand command)
        {
            DbDataAdapter adapter = this.CreateAdapter();
            adapter.SelectCommand = command;
            return adapter;
        }
        /// <summary>
        /// 构建Command对象。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cols"></param>
        /// <param name="pks"></param>
        /// <param name="opera"></param>
        /// <returns></returns>
        protected abstract DbCommand BuildCommand(string tableName, DataColumnCollection cols, DataColumn[] pks, BuildCommandOpera opera);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        protected virtual SqlDbType ConvertTo(Type dataType)
        {
            if (dataType == typeof(Int64))
            {
                return SqlDbType.BigInt;
            }
            else if (dataType == typeof(byte[]))
            {
                return SqlDbType.Binary;
            }
            else if (dataType == typeof(bool) || dataType == typeof(Boolean))
            {
                return SqlDbType.Bit;
            }
            else if (dataType == typeof(char) || dataType == typeof(Char))
            {
                return SqlDbType.Char;
            }
            else if (dataType == typeof(DateTime))
            {
                return SqlDbType.DateTime;
            }
            else if (dataType == typeof(decimal) || dataType == typeof(Decimal))
            {
                return SqlDbType.Decimal;
            }
            else if (dataType == typeof(float) || dataType == typeof(double) || dataType == typeof(Double))
            {
                return SqlDbType.Float;
            }
            else if (dataType == typeof(int) || dataType == typeof(Int32))
            {
                return SqlDbType.Int;
            }
            else if (dataType == typeof(Int64))
            {
                return SqlDbType.BigInt;
            }
            else if (dataType == typeof(Int16))
            {
                return SqlDbType.SmallInt;
            }
            else if (dataType == typeof(Guid))
            {
                return SqlDbType.UniqueIdentifier;
            }
            return SqlDbType.NVarChar;
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 资源释放。
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (this.Transaction != null)
                {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }

                if (this.Connection != null)
                {
                    if (this.Connection.State != ConnectionState.Closed)
                    {
                        this.Connection.Close();
                    }
                    this.Connection.Dispose();
                    this.Connection = null;
                }
            }
        }
        #endregion

        #region 内置枚举。
        /// <summary>
        /// 构建Cmd类型。
        /// </summary>
        protected enum BuildCommandOpera
        {
            /// <summary>
            /// 插入。
            /// </summary>
            Insert,
            /// <summary>
            /// 更新。
            /// </summary>
            Update,
            /// <summary>
            /// 删除。
            /// </summary>
            Delete
        }
        #endregion
    }
}