//================================================================================
//  FileName: IDBAccess.cs
//  Desc:统一数据访问接口。
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
//  2009-12-30 yangyong 修改批量处理。
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
    ///统一数据访问接口。
    /// </summary>
    public interface IDBAccess : IDisposable
    {
        #region 属性。
        /// <summary>
        ///  获取或设置命令超时时间(秒)。
        /// </summary>
        int CommandTimeout
        {
            get;
            set;
        }
        #endregion

        #region 连接操作。
        /// <summary>
        /// 打开数据库连接。
        /// </summary>
        void OpenConnection();
        /// <summary>
        /// 关闭数据库连接，回滚事务。 
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="bCloseTrans">是否回滚事务，true表示回滚事务，false表示不处理有事务的连接。</param>
        void CloseConnection(bool bCloseTrans);
        #endregion

        #region 事务处理。
        /// <summary>
        /// 开始数据库事务（不支持并行事务要重载该函数）。
        /// </summary>
        /// <returns>成功启动事务则返回true，否则返回false。</returns>
        bool BeginTransaction();
        /// <summary>
        /// 提交当前数据库事务。
        /// </summary>
        /// <returns>成功提交事务则返回true，否则返回false。</returns>
        bool CommitTransaction();
        /// <summary>
        /// 回滚当前数据库事务。 
        /// </summary>
        /// <returns>成功回滚事务则返回true，否则返回false。</returns>
        bool RollbackTransaction();
        /// <summary>
        /// 是否存在事务。
        /// </summary>
        /// <returns>存在事务则返回true，否则返回false。</returns>
        bool HasTransaction();
        #endregion

        /// <summary>
        /// 将字符里的特殊字符转化为数据库支持字符格式。
        /// </summary>
        /// <param name="context">需要转化的字符。</param>
        /// <returns>转化后的字符。</returns>
        string ConvertToDBString(string context);

        #region 查询数据。 
        /// <summary>
        /// 执行命令返回数据集。
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>数据集。</returns>
        DataSet ExecuteDataset(string commandText);
        #endregion

        #region 操作数据。
        /// <summary>
        /// 执行命令。
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>影响数据行数。</returns>
        int ExecuteNonQuery(string commandText);
        #endregion

        #region 只读获取数据。
        /// <summary>
        /// 执行命令获取只进结果集，调用此方法后需要调用CloseConnection（）方法关闭连接。 
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>只进结果集。</returns>
        IDataReader ExecuteReader(string commandText);
        #endregion

        #region 结果集中第一行的第一列。
        /// <summary>
        /// 执行命令，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="commandText">命令。</param>
        /// <returns>结果集中第一行的第一列。</returns>
        object ExecuteScalar(string commandText);
        #endregion

        #region 批处理数据。
        /// <summary>
        /// 批量插入数据。
        /// </summary>
        /// <param name="tableName">表名。</param>
        /// <param name="dtValue">数据(主键字段在dtValue中设置)。</param>
        /// <returns>影响数据行数。</returns>
        int BatchInsert(string tableName, DataTable dtValue);
        /// <summary>
        /// 批量更新数据。
        /// </summary>
        /// <param name="tableName">表名。</param>
        /// <param name="dtValue">数据(主键字段在dtValue中设置)。</param>
        /// <returns>影响数据行数。</returns>
        int BatchUpdate(string tableName, DataTable dtValue);
        /// <summary>
        /// 批量删除数据。
        /// </summary>
        /// <param name="tableName">表名。</param>
        /// <param name="dtValue">数据(主键字段在dtValue中设置)。</param>
        /// <returns>影响数据行数。</returns>
        int BatchDelete(string tableName, DataTable dtValue);
        #endregion
    }
}
