//================================================================================
//  FileName: DatabaseFactory.cs
//  Desc: ���ݷ��ʹ�����
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
// 2009-12-30   yangyong �޸Ķ��󻺴�ʵ�֡�
// 2010-06-24   yangyong �޸Ľ��ܷ���ȡ���ܷ⣬�����ࡣ
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Data;
using System.Collections.Generic;
using iPower;
using iPower.Utility;
using iPower.Configuration;
namespace iPower.Data.DataAccess
{
    /// <summary>
    /// ���ݷ��ʹ�����
    /// </summary>
    public class DatabaseFactory
    {
        #region ��Ա���������캯������������
        static Dictionary<string, IDBAccess> objCache;

        /// <summary>
        /// ��̬���캯����
        /// </summary>
        static DatabaseFactory()
        {
            objCache = new Dictionary<string, IDBAccess>();
        }
        /// <summary>
        /// ���캯����
        /// </summary>
        protected DatabaseFactory()
        {
         
        }
        /// <summary>
        /// ����������
        /// </summary>
        ~DatabaseFactory()
        {
        }
        #endregion

        #region ��̬����
        /// <summary>
        /// ��̬���ݷ��ʶ��� 
        /// </summary>
        /// <param name="connectionString">���������ַ���</param>
        /// <param name="dbType">���ݿ�����</param>
        /// <returns>���ݷ��ʽӿ�</returns>
        public static IDBAccess Instance(string connectionString, EnumDbType dbType)
        {
            Guard.ArgumentNotNullOrEmptyString("���������ַ���", connectionString, true);
            lock (typeof(DatabaseFactory))
            {
                IDBAccess instance = objCache.ContainsKey(connectionString) ? objCache[connectionString] : null;
                if (instance == null)
                {
                    instance = new DatabaseFactory().CreateInstance(dbType,connectionString);
                    if (instance != null)
                        objCache[connectionString] = instance;
                }
                return instance;
            }
        }
        /// <summary>
        /// ��̬���ݷ��ʶ���,Ĭ��ΪSqlServer���ݿ⡣ 
        /// </summary>
        /// <param name="connectionString">���������ַ���</param>
        /// <returns></returns>
        public static IDBAccess Instance(string connectionString)
        {
            return Instance(connectionString, EnumDbType.SqlServer);
        }
        /// <summary>
        /// ��̬���ݷ��ʶ��� 
        /// </summary>
        /// <param name="csc">���ݿ����á�</param>
        /// <returns>���ݷ��ʽӿ�</returns>
        public static IDBAccess Instance(ConnectionStringConfiguration csc)
        {
            if (csc == null)
                return null;
            EnumDbType dbType = EnumDbType.SqlServer;
            try
            {
                if (!string.IsNullOrEmpty(csc.ProviderName))
                    dbType = (EnumDbType)Enum.Parse(typeof(EnumDbType), csc.ProviderName);
            }
            catch (Exception) { }

            return Instance(csc.ConnectionString, dbType);
        }
        #endregion

        #region �������ݷ���ʵ��
        /// <summary>
        /// �������ݷ��ʽӿ�ʵ����
        /// </summary>
        /// <returns></returns>
        protected virtual IDBAccess CreateInstance(EnumDbType dbType, string connString)
        {
            IDBAccess access = null;
            switch (dbType)
            {
                case EnumDbType.SqlServer:
                    access = new SqlDBAccess(connString);
                    break;
                case EnumDbType.OleDb:
                    access = new OleDBAccess(connString);
                    break;
                default:break;
            }
            return access;
        }
        #endregion
    }
}
