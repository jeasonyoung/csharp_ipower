//================================================================================
//  FileName: CommonEnumsEntity.cs
//  Desc:公共枚举实体类。
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

using iPower;
using iPower.Configuration;
using iPower.Platform.Engine.Domain;
namespace iPower.Platform.Engine.Persistence
{
    /// <summary>
    /// 公共枚举实体类。
    /// </summary>
    public class CommonEnumsEntity<K> : DbBaseEntity<CommonEnums,K>
        where K : BaseModuleConfiguration
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数，析够函数。
        /// </summary>
        /// <param name="config">模块配置实例。</param>
        public CommonEnumsEntity(K config)
            : base(config)
        {
        }
        #endregion

        #region 函数
        /// <summary>
        /// 获取枚举数据。
        /// </summary>
        /// <param name="type">枚举类型。</param>
        /// <param name="Value">枚举名称。</param>
        /// <returns>名称数据。</returns>
        public virtual string GetEnumMemberName(Type type, string Value)
        {
            lock (this)
            {
                DataTable dtSource = this.Cache[type.FullName] as DataTable;
                if (dtSource == null)
                {
                    dtSource = this.LoadEnumAndConst(type.FullName);
                    if (dtSource != null)
                        this.Cache[type.FullName] = dtSource.Copy();
                }
                dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Member"] };
                DataRow row = dtSource.Rows.Find(Value);
                if (row != null)
                    return Convert.ToString(row["MemberName"]);
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取枚举数据。
        /// </summary>
        /// <param name="type">枚举类型。</param>
        /// <param name="Value">值。</param>
        /// <returns>名称数据。</returns>
        public virtual string GetEnumMemberName(Type type, int Value)
        {
            lock (this)
            {
                DataTable dtSource = this.Cache[type.FullName] as DataTable;
                if (dtSource == null)
                {
                    dtSource = this.LoadEnumAndConst(type.FullName);
                    if (dtSource != null)
                        this.Cache[type.FullName] = dtSource.Copy();
                }
                dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Member"] };
                DataRow row = dtSource.Rows.Find(Enum.GetName(type, Value));
                if (row != null)
                    return Convert.ToString(row["MemberName"]);
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取枚举数据值。
        /// </summary>
        /// <param name="type">枚举类型。</param>
        /// <param name="value">名称数据。</param>
        /// <returns>值。</returns>
        public virtual int GetEnumMemberIntValue(Type type, string value)
        {
            lock (this)
            {
                DataTable dtSource = this.Cache[type.FullName] as DataTable;
                if (dtSource == null)
                {
                    dtSource = this.LoadEnumAndConst(type.FullName);
                    if (dtSource != null)
                        this.Cache[type.FullName] = dtSource.Copy();
                }
                DataRow[] rows = dtSource.Select(string.Format("(MemberName = '{0}') or (Member = '{0}')", value));
                if (rows == null)
                    return 0;
                return Convert.ToInt32(rows[0]["IntValue"]);
            }
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 获取枚举数据。
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>s
        public virtual DataTable LoadEnumAndConst(string className)
        {
            if (!string.IsNullOrEmpty(className))
            {
                return this.GetAllRecord(string.Format("EnumName = '{0}'", className), "OrderNo");
            }
            return null;
        }
        #endregion
    }
}
