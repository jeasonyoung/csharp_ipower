//================================================================================
//  FileName: IDBAccess.cs
//  Desc:ͳһ���ݷ��ʽӿڡ�
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
//  2009-12-30 yangyong �޸���������
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
namespace iPower.Data
{
    /// <summary>
    ///ͳһ���ݷ��ʽӿڡ�
    /// </summary>
    public interface IDBAccess : IDisposable
    {
        #region ���ԡ�
        /// <summary>
        ///  ��ȡ���������ʱʱ��(��)��
        /// </summary>
        int CommandTimeout
        {
            get;
            set;
        }
        #endregion

        #region ���Ӳ�����
        /// <summary>
        /// �����ݿ����ӡ�
        /// </summary>
        void OpenConnection();
        /// <summary>
        /// �ر����ݿ����ӣ��ع����� 
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// �ر����ݿ�����
        /// </summary>
        /// <param name="bCloseTrans">�Ƿ�ع�����true��ʾ�ع�����false��ʾ����������������ӡ�</param>
        void CloseConnection(bool bCloseTrans);
        #endregion

        #region ������
        /// <summary>
        /// ��ʼ���ݿ����񣨲�֧�ֲ�������Ҫ���ظú�������
        /// </summary>
        /// <returns>�ɹ����������򷵻�true�����򷵻�false��</returns>
        bool BeginTransaction();
        /// <summary>
        /// �ύ��ǰ���ݿ�����
        /// </summary>
        /// <returns>�ɹ��ύ�����򷵻�true�����򷵻�false��</returns>
        bool CommitTransaction();
        /// <summary>
        /// �ع���ǰ���ݿ����� 
        /// </summary>
        /// <returns>�ɹ��ع������򷵻�true�����򷵻�false��</returns>
        bool RollbackTransaction();
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        /// <returns>���������򷵻�true�����򷵻�false��</returns>
        bool HasTransaction();
        #endregion

        /// <summary>
        /// ���ַ���������ַ�ת��Ϊ���ݿ�֧���ַ���ʽ��
        /// </summary>
        /// <param name="context">��Ҫת�����ַ���</param>
        /// <returns>ת������ַ���</returns>
        string ConvertToDBString(string context);

        #region ��ѯ���ݡ� 
        /// <summary>
        /// ִ����������ݼ���
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>���ݼ���</returns>
        DataSet ExecuteDataset(string commandText);
        #endregion

        #region �������ݡ�
        /// <summary>
        /// ִ�����
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>Ӱ������������</returns>
        int ExecuteNonQuery(string commandText);
        #endregion

        #region ֻ����ȡ���ݡ�
        /// <summary>
        /// ִ�������ȡֻ������������ô˷�������Ҫ����CloseConnection���������ر����ӡ� 
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>ֻ���������</returns>
        IDataReader ExecuteReader(string commandText);
        #endregion

        #region ������е�һ�еĵ�һ�С�
        /// <summary>
        /// ִ����������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
        /// </summary>
        /// <param name="commandText">���</param>
        /// <returns>������е�һ�еĵ�һ�С�</returns>
        object ExecuteScalar(string commandText);
        #endregion

        #region ���������ݡ�
        /// <summary>
        /// �����������ݡ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="dtValue">����(�����ֶ���dtValue������)��</param>
        /// <returns>Ӱ������������</returns>
        int BatchInsert(string tableName, DataTable dtValue);
        /// <summary>
        /// �����������ݡ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="dtValue">����(�����ֶ���dtValue������)��</param>
        /// <returns>Ӱ������������</returns>
        int BatchUpdate(string tableName, DataTable dtValue);
        /// <summary>
        /// ����ɾ�����ݡ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="dtValue">����(�����ֶ���dtValue������)��</param>
        /// <returns>Ӱ������������</returns>
        int BatchDelete(string tableName, DataTable dtValue);
        #endregion
    }
}
