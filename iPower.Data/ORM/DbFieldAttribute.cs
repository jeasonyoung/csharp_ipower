//================================================================================
//  FileName: DbFieldAttribute.cs
//  Desc:�ֶ����ԡ�
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
    /// �ֶ����ԡ�
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited= true)]
    public class DbFieldAttribute : Attribute
    {
        #region ��Ա��������������������������
        object defaultValue;
        string  fieldName,description;
        DbFieldUsage usage;

        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="usage">�ֶ����͡�</param>
        /// <param name="defaultValue">ȱʡֵ��</param>
        /// <param name="description">�ֶ�������</param>
        public DbFieldAttribute(string fieldName, DbFieldUsage usage, object defaultValue, string description)
        {
            this.fieldName = fieldName;
            this.usage = usage;
            this.defaultValue = defaultValue;
            this.description = description;
        }
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="usage">�ֶ����͡�</param>
        /// <param name="description">�ֶ�������</param>
        public DbFieldAttribute(string fieldName, DbFieldUsage usage, string description)
            : this(fieldName, usage, null, description)
        {
        }
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="usage">�ֶ����͡�</param>
        public DbFieldAttribute(string fieldName, DbFieldUsage usage)
        {
            this.fieldName = fieldName;
            this.usage = usage;
        }
        /// <summary>
        /// �ֶ�����
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        public DbFieldAttribute(string fieldName)
            : this(fieldName, DbFieldUsage.None)
        {
        }
        /// <summary>
        /// ����������
        /// </summary>
        ~DbFieldAttribute()
        {
        }
        #endregion 

        #region ����
        /// <summary>
        /// ��ȡ�ֶ�����
        /// </summary>
        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
        }
        /// <summary>
        /// ��ȡ�ֶ����͡�
        /// </summary>
        internal DbFieldUsage FieldUsage
        {
            get
            {
                return this.usage;
            }
        }
        /// <summary>
        /// ��ȡ�ֶ�Ĭ��ֵ��
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }
        /// <summary>
        /// ��ȡ�ֶ�������
        /// </summary>
        public string FieldDescription
        {
            get
            {
                return this.description;
            }
        }
        /// <summary>
        /// ��ȡ�Ƿ��ǹؼ��֡�
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return (this.usage & DbFieldUsage.PrimaryKey) == DbFieldUsage.PrimaryKey;
            }
        }
        /// <summary>
        /// ��ȡ�Ƿ���ΨһԼ����
        /// </summary>
        public bool IsUniqueKey
        {
            get
            {
                return (this.usage & DbFieldUsage.UniqueKey) == DbFieldUsage.UniqueKey;
            }
        }
        /// <summary>
        /// ��ȡ�Ƿ������ֶΡ�
        /// </summary>
        public bool IsBySystem
        {
            get
            {
                return (this.usage & DbFieldUsage.BySystem) == DbFieldUsage.BySystem;
            }
        }
        /// <summary>
        /// ��ȡ�Ƿ�Ϊ�����ݲ������ֶΡ�
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
