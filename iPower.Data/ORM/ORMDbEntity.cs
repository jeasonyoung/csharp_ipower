//================================================================================
//  FileName: ORMDbEntity.cs
//  Desc:ORM实体基础类。
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
//
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.ComponentModel;

using iPower;
using iPower.Data;
using iPower.Utility;
namespace iPower.Data.ORM
{
    /// <summary>
    /// 数据变更日志委托。
    /// </summary>
    /// <param name="head">日志头。</param>
    /// <param name="content">日志体。</param>
    public delegate void DbEntityDataChangeLogHandler(string head, string content);
    /// <summary>
    /// ORM实体基础类。
    /// </summary>
    /// <typeparam name="T">实体类。</typeparam>
    public abstract class ORMDbEntity<T> : IDisposable
        where T : new()
    {
        #region 成员变量，构造函数。
        private static Hashtable tablesCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable membersCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable primaryFieldsCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable allFieldsCache = Hashtable.Synchronized(new Hashtable());
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;
        private Type classType;
        private IDBAccess oDbAccess;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ORMDbEntity()
        {
            this.classType = typeof(T);
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 数据变更写入日志事件。
        /// </summary>
        public event DbEntityDataChangeLogHandler DbEntityDataChangeLogEvent;
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="head">日志头。</param>
        /// <param name="logContent">日志体。</param>
        protected virtual void OnDbEntityDataChangeLogHandler(string head, string logContent)
        {
            DbEntityDataChangeLogHandler handler = this.DbEntityDataChangeLogEvent;
            if (handler != null)
            {
                handler(head, logContent);
            }
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取表名。
        /// </summary>
        public string TableName
        {
            get
            {
                lock (this)
                {
                    string strName = tablesCache[this.classType] as String;
                    if (string.IsNullOrEmpty(strName))
                    {
                        this.loadFieldMembersAttributes(ref tablesCache, ref membersCache, ref primaryFieldsCache, ref allFieldsCache);
                        strName = tablesCache[this.classType] as String;
                    }
                    return strName;
                }
            }
        }
        /// <summary>
        /// 获取或设置数据访问接口。
        /// </summary>
        public IDBAccess DatabaseAccess
        {
            get
            {
                if (this.oDbAccess == null)
                {
                    this.oDbAccess = this.CreateDBAccess();
                }
                return this.oDbAccess;
            }
            set
            {
                this.oDbAccess = value;
            }
        }
        /// <summary>
        /// 获取所有字段名称。
        /// </summary>
        protected String[] AllFieldName
        {
            get
            {
                lock (this)
                {
                    String[] allFields = allFieldsCache[this.classType] as String[];
                    if (allFields == null || allFields.Length == 0)
                    {
                        this.loadFieldMembersAttributes(ref tablesCache, ref membersCache, ref primaryFieldsCache, ref allFieldsCache);
                        allFields = allFieldsCache[this.classType] as String[];
                    }
                    return allFields;
                }
            }
        }
        /// <summary>
        /// 获取主键字段。
        /// </summary>
        protected String[] PrimaryFieldName
        {
            get
            {
                lock (this)
                {
                    String[] pks = primaryFieldsCache[this.classType] as String[];
                    if (pks == null || pks.Length == 0)
                    {
                        this.loadFieldMembersAttributes(ref tablesCache, ref membersCache, ref primaryFieldsCache, ref allFieldsCache);
                        pks = primaryFieldsCache[this.classType] as String[];
                    }
                    return pks;
                }
            }
        }
        /// <summary>
        /// 加载字段数据信息。
        /// </summary>
        private FieldMemberInfo[] LoadFieldMemberInfoData
        {
            get
            {
                lock (this)
                {
                    FieldMemberInfo[] members = membersCache[this.classType] as FieldMemberInfo[];
                    if (members == null || members.Length == 0)
                    {
                        this.loadFieldMembersAttributes(ref tablesCache, ref membersCache, ref primaryFieldsCache, ref allFieldsCache);
                        members = membersCache[this.classType] as FieldMemberInfo[];
                    }
                    return members;
                }
            }
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableCache"></param>
        /// <param name="members"></param>
        /// <param name="pks"></param>
        /// <param name="all"></param>
        private void loadFieldMembersAttributes(ref Hashtable tableCache, ref Hashtable members, ref Hashtable pks, ref Hashtable all)
        {
            lock (this)
            {
                if (this.classType != null)
                {
                    #region 表名称。
                    object[] tables = this.classType.GetCustomAttributes(typeof(DbTableAttribute), true);
                    if (tables != null && tables.Length > 0 && (tables[0] is DbTableAttribute))
                    {
                        string tableName = ((DbTableAttribute)tables[0]).TableName;
                        if (!string.IsNullOrEmpty(tableName))
                        {
                            tableCache[this.classType] = tableName;
                        }
                    }
                    #endregion

                    #region 属性/字段。
                    List<FieldMemberInfo> membersList = new List<FieldMemberInfo>();
                    List<String> pksList = new List<String>(), allList = new List<String>();
                    foreach (PropertyInfo info in this.classType.GetProperties(bindingFlags))
                    {
                        foreach (object obj in info.GetCustomAttributes(typeof(DbFieldAttribute), true))
                        {
                            if (obj is DbFieldAttribute)
                            {
                                DbFieldAttribute attr = obj as DbFieldAttribute;
                                string fieldName = attr.FieldName;
                                if (!string.IsNullOrEmpty(fieldName))
                                {
                                    fieldName = fieldName.ToUpper();
                                    if (!allList.Contains(fieldName))
                                    {
                                        allList.Add(fieldName);
                                    }
                                    if (attr.IsPrimaryKey && !pksList.Contains(fieldName))
                                    {
                                        pksList.Add(fieldName);
                                    }
                                    membersList.Add(new FieldMemberInfo(attr, info));
                                }
                            }
                        }
                    }
                    //字段成员方法。
                    members[this.classType] = membersList.ToArray();
                    //主键。
                    pks[this.classType] = pksList.ToArray();
                    //所有字段。
                    all[this.classType] = allList.ToArray();
                    #endregion
                }
            }
        }
        #endregion

        #region 保护性虚函数声明
        /// <summary>
        /// 创建数据访问接口。
        /// </summary>
        /// <returns>数据访问接口</returns>
        protected abstract IDBAccess CreateDBAccess();
        #endregion

        #region 保护性函数
        /// <summary>
        /// 给属性赋值。
        /// </summary>
        /// <param name="row">数据行。</param>
        /// <returns>数据对象实例。</returns>
        protected T Assignment(DataRow row)
        {
            T objEntity = default(T);
            if (row != null)
            {
                objEntity = new T();
                foreach (FieldMemberInfo info in this.LoadFieldMemberInfoData)
                {
                    info.SetValue(objEntity, row[info.FieldAttribute.FieldName]);
                }
            }
            return objEntity;
        }
        /// <summary>
        /// 给属性赋值。
        /// </summary>
        /// <param name="reader">数据行。</param>
        /// <returns>数据对象实例。</returns>
        protected T Assignment(IDataReader reader)
        {
            T objEntity = default(T);
            if (reader != null)
            {
                objEntity = new T();
                foreach (FieldMemberInfo info in this.LoadFieldMemberInfoData)
                {
                    info.SetValue(objEntity, reader[info.FieldAttribute.FieldName]);
                }
            }
            return objEntity;
        }
        /// <summary>
        /// 获取对象实例的值。
        /// </summary>
        /// <param name="entity">对象实例。</param>
        /// <param name="primaryNameValues">主键键-值。</param>
        /// <param name="fieldNameValues">非主键键-值。</param>
        /// <returns>获取数据成功。</returns>
        protected bool LoadEntityValues(ref T entity, out NameValueCollection primaryNameValues, out NameValueCollection fieldNameValues)
        {
            lock (this)
            {
                primaryNameValues = new NameValueCollection();
                fieldNameValues = new NameValueCollection();

                string strValue = string.Empty;
                foreach (FieldMemberInfo info in this.LoadFieldMemberInfoData)
                {
                    strValue = Convert.ToString(info.GetValue(entity));
                    if (info.FieldAttribute.IsPrimaryKey)
                    {
                        primaryNameValues.Add(info.FieldAttribute.FieldName, strValue);
                    }
                    else
                    {
                        fieldNameValues.Add(info.FieldAttribute.FieldName, strValue);
                    }
                }
                return primaryNameValues.Count + fieldNameValues.Count > 0;
            }
        }
        /// <summary>
        /// 设置DataTable的主键。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pks">主键字段名称数组。</param>
        protected virtual void SetPrimaryKeys(ref DataTable dt, string[] pks)
        {
            if (pks != null && pks.Length > 0)
            {
                List<DataColumn> listPks = new List<DataColumn>();
                foreach (DataColumn col in dt.Columns)
                {
                    bool result = Array.Exists<string>(pks, new Predicate<string>(delegate(string v)
                    {
                        if (!string.IsNullOrEmpty(v))
                        {
                            return v.Equals(col.ColumnName, StringComparison.InvariantCultureIgnoreCase);
                        }
                        return false;
                    }));
                    if (result)
                    {
                        listPks.Add(col);
                    }
                }
                if (listPks.Count > 0)
                {
                    dt.PrimaryKey = listPks.ToArray();
                }
            }
        }
        #endregion

        #region 公开函数。

        #region 加载数据。
        /// <summary>
        /// 加载数据。
        /// </summary>
        /// <param name="entity">实例。</param>
        /// <returns></returns>
        public virtual bool LoadRecord(ref T entity)
        {
            return this.LoadRecord(ref entity, true);
        }
        /// <summary>
        /// 加载数据。
        /// </summary>
        /// <param name="entity">实例。</param>
        /// <param name="bPrimary">是否主键。</param>
        /// <returns>加载成功为true。</returns>
        public virtual bool LoadRecord(ref T entity, bool bPrimary)
        {
            lock (this)
            {
                bool result = false;
                NameValueCollection primaryValues, fieldValues;
                if (this.LoadEntityValues(ref entity, out primaryValues, out fieldValues))
                {
                    NameValueCollection whereCollection = new NameValueCollection();
                    if (bPrimary)
                    {
                        whereCollection.Add(primaryValues);
                    }
                    else
                    {
                        whereCollection.Add(fieldValues);
                    }
                    List<T> list = this.LoadRecord(whereCollection);
                    result = (list != null && list.Count > 0);
                    if (result)
                    {
                        entity = list[0];
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// 加载数据。
        /// </summary>
        /// <param name="primaryValue">主键值（只有一个主键）。</param>
        /// <returns>返回实例。</returns>
        protected virtual T LoadRecord(object primaryValue)
        {
            if (primaryValue != null)
            {
                NameValueCollection nvc = new NameValueCollection();
                foreach (string s in this.PrimaryFieldName)
                {
                    nvc.Add(s, primaryValue == null ? string.Empty : primaryValue.ToString());
                    break;
                }
                List<T> list = this.LoadRecord(nvc);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            return default(T);
        }
        /// <summary>
        /// 加载数据。
        /// </summary>
        /// <param name="where">条件集合（字段名称/字段值）。</param>
        /// <returns>实例集合。</returns>
        protected virtual List<T> LoadRecord(NameValueCollection where)
        {
            StringBuilder filterBuilder = new StringBuilder();
            if (where != null)
            {
                foreach (string fieldName in where.AllKeys)
                {
                    if (filterBuilder.Length > 0)
                    {
                        filterBuilder.Append(" and ");
                    }
                    filterBuilder.AppendFormat(" {0} = '{1}' ", fieldName, where[fieldName]);
                }
            }
            return this.LoadRecord(filterBuilder.ToString());
        }
        /// <summary>
        /// 加载数据。
        /// </summary>
        /// <param name="where">条件集合（字段名称/字段值）。</param>
        /// <returns>实例集合。</returns>
        protected virtual List<T> LoadRecord(string where)
        {
            return this.ConvertDataSource(this.GetAllRecord(where));
        }
        #endregion

        #region 获取全部记录。
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <returns>DataTable.</returns>
        public virtual DataTable GetAllRecord()
        {
            return this.GetAllRecord((string[])null, null, null);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="colsName">列名。</param>
        /// <returns>DataTable</returns>
        public virtual DataTable GetAllRecord(string[] colsName)
        {
            return this.GetAllRecord(colsName, null, null);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="colsName">列名。</param>
        /// <param name="sortExpression">排序表达式。</param>
        /// <returns>DataTable</returns>
        public virtual DataTable GetAllRecord(string[] colsName, string sortExpression)
        {
            return this.GetAllRecord(colsName, null, sortExpression);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="filterExpression">过滤条件。</param>
        /// <returns>DataTable</returns>
        public virtual DataTable GetAllRecord(string filterExpression)
        {
            return this.GetAllRecord((string[])null, filterExpression, null);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="filterExpression">过滤条件。</param>
        /// <param name="sortExpression">排序表达式</param>
        /// <returns>DataTable</returns>
        public virtual DataTable GetAllRecord(string filterExpression, string sortExpression)
        {
            return this.GetAllRecord((string[])null, filterExpression, sortExpression);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="colsName">列名。</param>
        /// <param name="filterExpression">过滤条件。</param>
        /// <param name="sortExpression">排序。</param>
        /// <returns>DataTable</returns>
        public virtual DataTable GetAllRecord(string[] colsName, string filterExpression, string sortExpression)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ");
            if (colsName == null || colsName.Length == 0)
                builder.Append(" * ");
            else
                builder.AppendFormat(" {0} ", string.Join(",", colsName));

            builder.AppendFormat(" from {0} ", this.TableName);

            if (!string.IsNullOrEmpty(filterExpression))
                builder.AppendFormat(" where {0} ", filterExpression);

            if (!string.IsNullOrEmpty(sortExpression))
                builder.AppendFormat(" order by {0}", sortExpression);

            if (this.DatabaseAccess == null)
                throw new ArgumentNullException("数据访问接口不存在实例。");

            DataTable dtSource = this.DatabaseAccess.ExecuteDataset(builder.ToString()).Tables[0].Copy();
            if (dtSource != null)
            {
                this.SetPrimaryKeys(ref dtSource, this.PrimaryFieldName);
            }
            return dtSource;
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="colsName">列名。</param>
        /// <param name="filterExpression">过滤条件。</param>
        /// <param name="sortExpression">排序。</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader GetReaderRecord(string[] colsName, string filterExpression, string sortExpression)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ");
            if (colsName == null || colsName.Length == 0)
                builder.Append(" * ");
            else
                builder.AppendFormat(" {0} ", string.Join(",", colsName));

            builder.AppendFormat(" from {0} ", this.TableName);

            if (!string.IsNullOrEmpty(filterExpression))
                builder.AppendFormat(" where {0} ", filterExpression);

            if (!string.IsNullOrEmpty(sortExpression))
                builder.AppendFormat(" order by {0}", sortExpression);

            if (this.DatabaseAccess == null)
                throw new ArgumentNullException("数据访问接口不存在实例。");

            return this.DatabaseAccess.ExecuteReader(builder.ToString());
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="filterExpression">过滤条件。</param>
        /// <param name="sortExpression">排序表达式。</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader GetReaderRecord(string filterExpression, string sortExpression)
        {
            return this.GetReaderRecord((string[])null, filterExpression, sortExpression);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="filterExpression">过滤条件。</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader GetReaderRecord(string filterExpression)
        {
            return this.GetReaderRecord((string[])null, filterExpression, null);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="colsName">列名。</param>
        /// <param name="sortExpression">排序表达式。</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader GetReaderRecord(string[] colsName, string sortExpression)
        {
            return this.GetReaderRecord(colsName, null, sortExpression);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <param name="colsName">列名。</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader GetReaderRecord(string[] colsName)
        {
            return this.GetReaderRecord(colsName, null, null);
        }
        /// <summary>
        /// 获取全部记录。
        /// </summary>
        /// <returns>IDataReader.</returns>
        public virtual IDataReader GetReaderRecord()
        {
            return this.GetReaderRecord((string[])null, null, null);
        }
        #endregion

        #region 插入数据。
        /// <summary>
        /// 插入数据。
        /// </summary>
        /// <param name="colsNameValues">字段及数据。</param>
        /// <returns>成功返回true,否则false。</returns>
        protected bool InsertRecord(NameValueCollection colsNameValues)
        {
            if (colsNameValues != null && colsNameValues.Count > 0)
            {
                string[] values = new string[colsNameValues.Count];
                int i = 0;
                foreach (string key in colsNameValues.AllKeys)
                {
                    values[i++] = colsNameValues[key];
                }

                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(" insert into {0} ({1}) values ('{2}')",
                    this.TableName,
                    string.Join(",", colsNameValues.AllKeys),
                    string.Join("','", values));

                this.OnDbEntityDataChangeLogHandler("insert", builder.ToString());
                return this.DatabaseAccess.ExecuteNonQuery(builder.ToString()) > 0;
            }
            return false;
        }
        #endregion

        #region 更新数据。
        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="entity">实例。</param>
        /// <returns>成功返回true,否则false。</returns>
        public virtual bool UpdateRecord(T entity)
        {
            lock (this)
            {
                bool result = false;
                if (entity != null)
                {
                    string strValue = string.Empty, strFieldName = string.Empty;
                    NameValueCollection colsNameValues = new NameValueCollection();
                    NameValueCollection primaryNameValues = new NameValueCollection();
                    StringBuilder where = new StringBuilder();
                    foreach (FieldMemberInfo info in this.LoadFieldMemberInfoData)
                    {
                        if (!info.FieldAttribute.IsBySystem)
                        {
                            strFieldName = info.FieldAttribute.FieldName;
                            strValue = Convert.ToString(info.GetValue(entity));
                            if (!(info.FieldAttribute.IsEmptyOrNullNotUpdate && string.IsNullOrEmpty(strValue)))
                            {
                                if (string.IsNullOrEmpty(strValue) && info.FieldAttribute.DefaultValue != null)
                                {
                                    strValue = info.FieldAttribute.DefaultValue.ToString();
                                }
                                if (info.FieldAttribute.IsPrimaryKey)
                                {
                                    if (where.Length > 0)
                                    {
                                        where.Append(" and ");
                                    }
                                    where.AppendFormat(" {0}='{1}' ", strFieldName, strValue);
                                    primaryNameValues.Add(strFieldName, strValue);
                                }
                                else /*if (!info.FieldAttribute.IsUniqueKey)*/
                                {
                                    colsNameValues.Add(strFieldName, strValue);
                                }
                            }
                        }
                    }
                    if (colsNameValues.Count > 0 && where.Length > 0)
                    {
                        result = this.UpdateRecord(colsNameValues, where.ToString());
                        if (!result)
                        {
                            colsNameValues.Add(primaryNameValues);
                            result = this.InsertRecord(colsNameValues);
                        }
                    }
                    else if (primaryNameValues.Count > 0 && colsNameValues.Count == 0)
                    {
                        result = this.InsertRecord(primaryNameValues);
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="colsNameValues">列名－数据。</param>
        /// <param name="whereExpression">过滤条件。</param>
        /// <returns>成功返回true,否则false。</returns>
        protected virtual bool UpdateRecord(NameValueCollection colsNameValues, string whereExpression)
        {
            lock (this)
            {
                bool result = false;
                if (colsNameValues != null && colsNameValues.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select ");
                    sb.Append(string.Join(",", colsNameValues.AllKeys));
                    sb.AppendFormat(" from {0} ", this.TableName);
                    if (!string.IsNullOrEmpty(whereExpression))
                    {
                        sb.AppendFormat(" where {0}", whereExpression);
                    }
                    if (this.DatabaseAccess == null)
                    {
                        throw new ArgumentNullException("数据访问接口不存在实例。");
                    }
                    StringBuilder log = new StringBuilder();
                    log.AppendFormat("更新数据：[{0}] ", this.TableName);
                    log.AppendLine();
                    DataTable dtSource = this.DatabaseAccess.ExecuteDataset(sb.ToString()).Tables[0].Copy();
                    if (dtSource.Rows.Count > 0)
                    {
                        string strOldValue = string.Empty, strNewValue = string.Empty;
                        foreach (DataRow row in dtSource.Rows)
                        {
                            foreach (string colName in colsNameValues.AllKeys)
                            {
                                strOldValue = Convert.ToString(row[colName]);
                                strNewValue = colsNameValues[colName];
                                if (!strOldValue.Equals(strNewValue))
                                {
                                    log.AppendFormat("[{0}]的值由[{1}]更新为[{2}]；", colName, strOldValue, strNewValue);
                                }
                            }
                        }
                    }
                    sb = new StringBuilder();
                    sb.AppendFormat(" update {0} set ", this.TableName);
                    int i = 0;
                    foreach (string colName in colsNameValues.AllKeys)
                    {
                        if (i++ > 0)
                        {
                            sb.Append(",");
                        }
                        sb.AppendFormat(" {0}='{1}'", colName, colsNameValues[colName]);
                    }
                    if (!string.IsNullOrEmpty(whereExpression))
                    {
                        sb.AppendFormat(" where {0}", whereExpression);
                    }
                    this.OnDbEntityDataChangeLogHandler("update", sb.ToString());
                    result = this.DatabaseAccess.ExecuteNonQuery(sb.ToString()) > 0;
                    if (result)
                    {
                        log.Append("。");
                        this.OnDbEntityDataChangeLogHandler("Change", log.ToString());
                    }
                }
                return result;
            }
        }
        #endregion

        #region 删除数据。
        /// <summary>
        /// 删除数据。
        /// </summary>
        /// <param name="entity">实例。</param>
        /// <returns>成功返回true,否则false。</returns>
        public virtual bool DeleteRecord(T entity)
        {
            lock (this)
            {
                if (entity != null)
                {
                    NameValueCollection colsNameValues, primaryNameValues;
                    if (this.LoadEntityValues(ref entity, out primaryNameValues, out colsNameValues))
                    {
                        return this.DeleteRecord(primaryNameValues);
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 批量删除(适用于只有一个主键的表)。
        /// </summary>
        /// <param name="primaryValues">主键值集合</param>
        /// <returns>成功返回true,否则false。</returns>
        public virtual bool DeleteRecord(StringCollection primaryValues)
        {
            if (primaryValues != null)
            {
                string[] priValues = new string[primaryValues.Count];
                primaryValues.CopyTo(priValues, 0);
                return this.DeleteRecord(priValues);
            }
            return false;
        }
        /// <summary>
        /// 批量删除(适用于只有一个主键的表)。
        /// </summary>
        /// <param name="primaryValues">主键值集合</param>
        /// <returns></returns>
        protected virtual bool DeleteRecord(string[] primaryValues)
        {
            if (this.PrimaryFieldName == null || this.PrimaryFieldName.Length > 1)
            {
                throw new ArgumentException("数据表没定义主键或有多个主键。");
            }
            StringBuilder where = new StringBuilder();
            if (primaryValues != null)
            {
                where.AppendFormat("{0} in ('{1}')", this.PrimaryFieldName[0], string.Join("','", primaryValues));
            }
            return this.DeleteRecord(where.ToString());
        }
        /// <summary>
        /// 删除数据。
        /// </summary>
        /// <param name="primaryValues"></param>
        /// <returns></returns>
        protected virtual bool DeleteRecord(NameValueCollection primaryValues)
        {
            StringBuilder where = new StringBuilder();
            if (primaryValues != null)
            {
                foreach (string priName in primaryValues)
                {
                    if (where.Length > 0)
                    {
                        where.Append(" and ");
                    }
                    where.AppendFormat(" {0}='{1}' ", priName, primaryValues[priName]);
                }
            }
            return this.DeleteRecord(where.ToString());
        }
        /// <summary>
        ///  删除数据。
        /// </summary>
        /// <param name="where">条件。</param>
        /// <returns>成功返回true,否则false。</returns>
        protected virtual bool DeleteRecord(string where)
        {
            StringBuilder delSql = new StringBuilder();
            delSql.AppendFormat("delete from {0}", this.TableName);
            if (!string.IsNullOrEmpty(where))
            {
                delSql.AppendFormat(" where {0}", where);
            }
            if (this.DatabaseAccess == null)
            {
                throw new ArgumentNullException("数据访问接口不存在实例。");
            }
            this.OnDbEntityDataChangeLogHandler("Delete", delSql.ToString());
            int count = this.DatabaseAccess.ExecuteNonQuery(delSql.ToString());
            if (count > 0)
            {
                this.OnDbEntityDataChangeLogHandler("DeleteData", string.Format("删除数据：[{0}][{1}]，共删除{2}条数据。", this.TableName,
                    delSql.ToString(), count));
            }
            return count > 0;
        }
        #endregion

        #region 获取数据源对象。
        /// <summary>
        /// 获取数据源。
        /// </summary>
        /// <param name="where">查询条件。</param>
        /// <returns></returns>
        public virtual List<T> DataSource(string where)
        {
            return this.LoadRecord(where);
        }
        /// <summary>
        /// 数据源转换。
        /// </summary>
        /// <param name="dtSource">数据源。</param>
        /// <returns></returns>
        public virtual List<T> ConvertDataSource(DataTable dtSource)
        {
            List<T> list = new List<T>();
            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                foreach (DataRow row in dtSource.Rows)
                {
                    list.Add(this.Assignment(row));
                }
            }
            return list;
        }
        /// <summary>
        /// 数据源转换。
        /// </summary>
        /// <param name="drSource">数据源。</param>
        /// <returns></returns>
        public virtual List<T> ConvertDataSource(IDataReader drSource)
        {
            List<T> list = new List<T>();
            if (drSource != null)
            {
                using (drSource)
                {
                    if (drSource != null)
                    {
                        while (drSource.Read())
                        {
                            list.Add(this.Assignment(drSource));
                        }
                        drSource.Close();
                    }
                }
            }
            return list;
        }
        #endregion

        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
        #endregion
    }

    #region 辅助类。
    /// <summary>
    /// 字段成员信息。
    /// </summary>
    internal class FieldMemberInfo
    {
        #region 成员变量，构造函数。
        DbFieldAttribute fieldAttribute;
        PropertyInfo property;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldAttribute">字段属性。</param>
        /// <param name="property">函数属性。</param>
        public FieldMemberInfo(DbFieldAttribute fieldAttribute, PropertyInfo property)
        {
            this.fieldAttribute = fieldAttribute;
            this.property = property;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取字段属性。
        /// </summary>
        public DbFieldAttribute FieldAttribute
        {
            get
            {
                return this.fieldAttribute;
            }
        }
        /// <summary>
        /// 获取函数属性。
        /// </summary>
        public PropertyInfo Property
        {
            get
            {
                return this.property;
            }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 获取字段的值。
        /// </summary>
        /// <param name="entity">包含此字段的实例。</param>
        /// <returns>返回字段的值。</returns>
        public object GetValue(object entity)
        {
            object objValue = null;
            if (entity != null && this.Property != null && this.Property.CanRead)
            {
                Type type = this.Property.PropertyType;
                objValue = this.property.GetValue(entity, null);

                objValue = this.GetConvertToPropertyType(objValue, type);
                if (type == typeof(DateTime))
                    objValue = this.ConvertDateTimeToString(objValue, type);
            }
            return objValue;
        }

        /// <summary>
        /// 设置字段的值。
        /// </summary>
        /// <param name="entity">包含此字段的实例。</param>
        /// <param name="value">字段的值。</param>
        public void SetValue(object entity, object value)
        {
            if (entity != null && this.Property != null && this.Property.CanWrite)
            {
                Type type = this.Property.PropertyType;
                if (Convert.IsDBNull(value))
                {
                    if (this.FieldAttribute.DefaultValue != null)
                        value = this.FieldAttribute.DefaultValue;
                    else if (!type.IsValueType)
                        value = null;
                    else
                        return;
                }
                this.Property.SetValue(entity, this.SetConvertToPropertyType(value, type), null);
            }
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 将时间类型转化为带微秒的字符串，其他类型不变。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ConvertDateTimeToString(object obj, Type type)
        {
            if (type == typeof(DateTime))
            {
                DateTime dt = Convert.ToDateTime(obj);
                if (obj != null && string.Equals(obj, DateTime.MinValue))
                    return string.Empty;
                return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ConvertToPropertyType(object value, Type type)
        {
            if (type.BaseType == typeof(Enum))
            {
                if (value == null || value == DBNull.Value)
                    return Enum.ToObject(type, 0);
                return Enum.ToObject(type, Convert.ToInt32(value));
            }
            if (type == typeof(GUIDEx))
            {
                return new GUIDEx(value);
            }

            if (type == typeof(Guid))
            {
                if (value == DBNull.Value || value == null)
                    return Guid.Empty;
                return new Guid(Convert.ToString(value));
            }
            if (value == DBNull.Value)
                return null;
            if (type == typeof(string))
            {
                if (value == null)
                    return value;
                return value.ToString();
            }

            try
            {
                return Convert.ChangeType(value, type);
            }
            catch { }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object GetConvertToPropertyType(object value, Type type)
        {
            return ConvertToPropertyType(value, type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object SetConvertToPropertyType(object value, Type type)
        {
            return ConvertToPropertyType(value, type);
        }
        #endregion
    }
    #endregion
}