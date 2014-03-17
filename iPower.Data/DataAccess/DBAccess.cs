//================================================================================
//  FileName: DBAccess.cs
//  Desc:ͳһ���ݷ��ʳ����ࡣ
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
    /// ͳһ���ݷ��ʳ����ࡣ
    /// </summary>
    public abstract class DBAccess : IDBAccess
    {
        #region ��Ա���������캯����
        private static Hashtable schemaCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="connectionString">�����ַ�����</param>
        public DBAccess(string connectionString)
        {
            Guard.ArgumentNotNullOrEmptyString("connectionString", connectionString, true);
            this.CommandTimeout = 300;
            this.Connection = this.CreateConnection(connectionString);
        }
        #endregion

        #region ���ԡ�
        /// <summary>
        /// ��ȡ���������ʱʱ��(��)��
        /// </summary>
        public int CommandTimeout { get; set; }
        /// <summary>
        /// ��ȡ���������ݿ����Ӷ���
        /// </summary>
        protected DbConnection Connection { get; private set; }
        /// <summary>
        /// ��ȡ���������ݿ��������
        /// </summary>
        protected DbTransaction Transaction { get; private set; }
        /// <summary>
        /// ��ȡ���������ݿ����������
        /// </summary>
        protected int TransactionCount { get; private set; }
        #endregion

        #region ���ӹ���
        /// <summary>
        /// ���������ӡ�
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
        /// �ر��������ӡ�
        /// </summary>
        public void CloseConnection()
        {
            this.CloseConnection(true);
        }
        /// <summary>
        /// �ر��������ӡ�
        /// </summary>
        /// <param name="bCloseTrans">�Ƿ�ر�����</param>
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

        #region �������
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns>��ʼ�����Ƿ�ɹ���</returns>
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
        /// �ύ����
        /// </summary>
        /// <returns>�ύ�����Ƿ�ɹ���</returns>
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
        /// �ع�����
        /// </summary>
        /// <returns>�ع������Ƿ�ɹ���</returns>
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
        /// �Ƿ�������
        /// </summary>
        /// <returns></returns>
        public bool HasTransaction()
        {
            return this.TransactionCount > 0;
        }
        #endregion

        #region �Ƿ��ַ�����
        /// <summary>
        /// ���˷Ƿ��ַ���
        /// </summary>
        /// <param name="context">�����ַ���</param>
        /// <returns>���˺���ַ���</returns>
        public virtual string ConvertToDBString(string context)
        {
            return context;
        }
        #endregion

        #region ִ������

        #region ��ѯ���ݡ�
        /// <summary>
        /// ִ����������ݼ���
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>���ݼ���</returns>
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

        #region �������ݡ�
        /// <summary>
        /// ִ�����
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>Ӱ������������</returns>
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

        #region ֻ����ȡ���ݡ�
        /// <summary>
        /// ִ�������ȡֻ������������ô˷�������Ҫ����CloseConnection���������ر����ӡ� 
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>ֻ���������</returns>
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

        #region ������е�һ�еĵ�һ�С�
        /// <summary>
        /// ִ����������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>������е�һ�еĵ�һ�С�</returns>
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

        #region ���������ݡ�
        /// <summary>
        /// �����������ݡ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="dtValue">����(�����ֶ���dtValue������)��</param>
        /// <returns>Ӱ������������</returns>
        public virtual int BatchInsert(string tableName, DataTable dtValue)
        {
            //this.OnBatchBulkCopy(tableName, dtValue);
            return this.OnBatchUpdate(tableName, dtValue, BuildCommandOpera.Insert);
        }
        /// <summary>
        /// �����������ݡ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="dtValue">����(�����ֶ���dtValue������)��</param>
        /// <returns>Ӱ������������</returns>
        public virtual int BatchUpdate(string tableName, DataTable dtValue)
        {
            return this.OnBatchUpdate(tableName, dtValue, BuildCommandOpera.Update);
        }
        /// <summary>
        /// ����ɾ�����ݡ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="dtValue">����(�����ֶ���dtValue������)��</param>
        /// <returns>Ӱ������������</returns>
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

            #region ����������״̬��
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
        /// �����������ݡ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dtValue"></param>
        /// <returns></returns>
        protected abstract int OnBatchBulkCopy(string tableName, DataTable dtValue);
        #endregion
        #endregion

        #region ���󷽷�
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns>��������</returns>
        protected abstract DbDataAdapter CreateAdapter();
        /// <summary>
        /// �������ӡ�
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <returns>���Ӷ���</returns>
        protected abstract DbConnection CreateConnection(string connectionString);   
        #endregion 

        #region ����������
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="command">IDbCommand</param>
        /// <returns>����������</returns>
        protected DbDataAdapter CreateAdapter(DbCommand command)
        {
            DbDataAdapter adapter = this.CreateAdapter();
            adapter.SelectCommand = command;
            return adapter;
        }
        /// <summary>
        /// ����Command����
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

        #region IDisposable ��Ա
        /// <summary>
        /// ��Դ�ͷš�
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

        #region ����ö�١�
        /// <summary>
        /// ����Cmd���͡�
        /// </summary>
        protected enum BuildCommandOpera
        {
            /// <summary>
            /// ���롣
            /// </summary>
            Insert,
            /// <summary>
            /// ���¡�
            /// </summary>
            Update,
            /// <summary>
            /// ɾ����
            /// </summary>
            Delete
        }
        #endregion
    }
}