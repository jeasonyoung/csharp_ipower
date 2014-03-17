//================================================================================
//  FileName: CommonEnums.cs
//  Desc:公共枚举类。
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

using iPower.Data.ORM;
namespace iPower.Platform.Engine.Domain
{
    /// <summary>
    /// 公共枚举类。
    /// </summary>
    [DbTable("tblCommonEnums")]
    public class CommonEnums
    {
        #region 成员变量，构造函数，析构函数。
        string enumName, member, memberName;
        int intValue, orderNo;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CommonEnums()
        {
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~CommonEnums()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置枚举名称。
        /// </summary>
        [DbField("EnumName", DbFieldUsage.PrimaryKey)]
        public string EnumName
        {
            get { return this.enumName; }
            set { this.enumName = value; }
        }
        /// <summary>
        /// 获取或设置枚举成员。
        /// </summary>
        [DbField("Member", DbFieldUsage.PrimaryKey)]
        public string Member
        {
            get { return this.member; }
            set { this.member = value; }
        }
        /// <summary>
        /// 获取或设置成员中文名称。
        /// </summary>
        [DbField("MemberName")]
        public string MemberName
        {
            get { return this.memberName; }
            set { this.memberName = value; }
        }
        /// <summary>
        /// 获取或设置成员值。
        /// </summary>
        [DbField("IntValue")]
        public int IntValue
        {
            get { return this.intValue; }
            set { this.intValue = value; }
        }
        /// <summary>
        /// 获取或设置序号。
        /// </summary>
        [DbField("OrderNo")]
        public int OrderNo
        {
            get { return this.orderNo; }
            set { this.orderNo = value; }
        }
        #endregion
    }
}
