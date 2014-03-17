//================================================================================
//  FileName: DbFieldAttribute.cs
//  Desc:字段属性。
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
using System.Collections.Generic;
using System.Text;

namespace iPower.Data.ORM
{
    /// <summary>
    /// 字段属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited= true)]
    public class DbFieldAttribute : Attribute
    {
        #region 成员变量，构造行数，析构函数。
        object defaultValue;
        string  fieldName,description;
        DbFieldUsage usage;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名。</param>
        /// <param name="usage">字段类型。</param>
        /// <param name="defaultValue">缺省值。</param>
        /// <param name="description">字段描述。</param>
        public DbFieldAttribute(string fieldName, DbFieldUsage usage, object defaultValue, string description)
        {
            this.fieldName = fieldName;
            this.usage = usage;
            this.defaultValue = defaultValue;
            this.description = description;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名。</param>
        /// <param name="usage">字段类型。</param>
        /// <param name="description">字段描述。</param>
        public DbFieldAttribute(string fieldName, DbFieldUsage usage, string description)
            : this(fieldName, usage, null, description)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名。</param>
        /// <param name="usage">字段类型。</param>
        public DbFieldAttribute(string fieldName, DbFieldUsage usage)
        {
            this.fieldName = fieldName;
            this.usage = usage;
        }
        /// <summary>
        /// 字段属性
        /// </summary>
        /// <param name="fieldName">字段名。</param>
        public DbFieldAttribute(string fieldName)
            : this(fieldName, DbFieldUsage.None)
        {
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~DbFieldAttribute()
        {
        }
        #endregion 

        #region 属性
        /// <summary>
        /// 获取字段名。
        /// </summary>
        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
        }
        /// <summary>
        /// 获取字段类型。
        /// </summary>
        internal DbFieldUsage FieldUsage
        {
            get
            {
                return this.usage;
            }
        }
        /// <summary>
        /// 获取字段默认值。
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }
        /// <summary>
        /// 获取字段描述。
        /// </summary>
        public string FieldDescription
        {
            get
            {
                return this.description;
            }
        }
        /// <summary>
        /// 获取是否是关键字。
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return (this.usage & DbFieldUsage.PrimaryKey) == DbFieldUsage.PrimaryKey;
            }
        }
        /// <summary>
        /// 获取是否是唯一约束。
        /// </summary>
        public bool IsUniqueKey
        {
            get
            {
                return (this.usage & DbFieldUsage.UniqueKey) == DbFieldUsage.UniqueKey;
            }
        }
        /// <summary>
        /// 获取是否自增字段。
        /// </summary>
        public bool IsBySystem
        {
            get
            {
                return (this.usage & DbFieldUsage.BySystem) == DbFieldUsage.BySystem;
            }
        }
        /// <summary>
        /// 获取是否为空数据不更新字段。
        /// </summary>
        public bool IsEmptyOrNullNotUpdate
        {
            get
            {
                return (this.usage & DbFieldUsage.EmptyOrNullNotUpdate) == DbFieldUsage.EmptyOrNullNotUpdate;
            }
        }
        #endregion
    }
}
