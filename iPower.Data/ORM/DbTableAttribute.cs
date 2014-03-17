//================================================================================
//  FileName: DbTableAttribute.cs
//  Desc:表属性。
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
    /// 表属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DbTableAttribute : Attribute
    {
        #region 成员变量，构造函数。
        string tableName;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="tableName">表名。</param>
        public DbTableAttribute(string tableName)
        {
            this.tableName = tableName;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取表名。
        /// </summary>
        public string TableName
        {
            get { return this.tableName; }
        }
        #endregion
    }
}
