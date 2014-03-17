//================================================================================
//  FileName: Role.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2012-01-09 
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
using System.Xml.Serialization;

using iPower.Data;
namespace iPower.Platform.Security
{
    /// <summary>
    /// 角色信息。
    /// </summary>
    [XmlRoot("Role")]
    [Serializable]
    public class Role
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Role()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="roleID">角色ID。</param>
        /// <param name="roleSign">角色标识。</param>
        /// <param name="roleName">角色名称。</param>
        /// <param name="description">描述。</param>
        public Role(GUIDEx roleID, string roleSign, string roleName, string description)
        {
            this.RoleID = roleID;
            this.RoleSign = roleSign;
            this.RoleName = roleName;
            this.Description = description;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="roleID">角色ID。</param>
        /// <param name="roleName">角色名称。</param>
        /// <param name="description">描述。</param>
        public Role(GUIDEx roleID, string roleName, string description)
        {
            this.RoleID = roleID;
            this.RoleName = roleName;
            this.Description = description;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="roleID">角色ID。</param>
        /// <param name="roleName">角色名称。</param>
        public Role(GUIDEx roleID, string roleName)
        {
            this.RoleID = roleID;
            this.RoleName = roleName;
        }
        #endregion

        /// <summary>
        /// 获取或设置角色ID。
        /// </summary>
        [XmlElement("RoleID", DataType="String")]
        public GUIDEx RoleID { get; set; }
        /// <summary>
        /// 获取或设置角色标识。
        /// </summary>
        [XmlElement("RoleSign")]
        public string RoleSign { get; set; }
        /// <summary>
        /// 获取或设置角色名称。
        /// </summary>
        [XmlElement("RoleName")]
        public string RoleName { get; set; }
        /// <summary>
        /// 获取或设置描述。
        /// </summary>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat("[", this.RoleSign, "]", this.RoleName);
        }
    }
    /// <summary>
    /// 角色集合。
    /// </summary>
    [XmlRoot("Roles")]
    [Serializable]
    public class Roles : DataCollection<Role>
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Roles()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取角色。
        /// </summary>
        /// <param name="roleID">角色ID。</param>
        /// <returns></returns>
        public Role this[GUIDEx roleID]
        {
            get
            {
                if (roleID.IsValid)
                {
                    Role role = this.Items.Find(new Predicate<Role>(delegate(Role sender)
                    {
                        return (sender != null) && (sender.RoleID == roleID);
                    }));
                    return role;
                }
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 是否存在。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Contains(Role item)
        {
            if (item != null)
            {
                return this[item.RoleID] != null;
            }
            return false;
        }
        /// <summary>
        /// 排序。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override int Compare(Role x, Role y)
        {
            return string.Compare(x.RoleName, y.RoleName);
        }
    }
}
