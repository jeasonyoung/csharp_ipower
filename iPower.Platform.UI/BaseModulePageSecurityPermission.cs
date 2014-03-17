//================================================================================
//  FileName: BaseModulePageSecurityPermission.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/7/5
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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using iPower.Web.UI;
using iPower.Platform.Security;
namespace iPower.Platform.UI
{
    /// <summary>
    /// 安全管理。
    /// </summary>
    partial class BaseModulePage : ISecurity
    {
        #region ISecurity 成员
        /// <summary>
        /// 获取或设置页面安全标识ID。
        /// </summary>
        public GUIDEx SecurityID
        {
            get
            {
                object obj = this.ViewState["SecurityID"];
                return obj == null ? GUIDEx.Null : new GUIDEx(obj);
            }
            set
            {
                this.ViewState["SecurityID"] = value;
            }
        }
        /// <summary>
        /// 获取或设置当前用户所属角色集合。
        /// </summary>
        public Roles CurrentRoles
        {
            get
            {
                object obj = this.ViewState["CurrentRoles"];
                return obj == null ? null : (Roles)obj;
            }
            set
            {
                this.ViewState["CurrentRoles"] = value;
            }
        }
        /// <summary>
        ///验证权限。
        /// </summary>
        /// <param name="permissions">权限集合。</param>
        public virtual void VerifyPermissions(SecurityPermissionCollection permissions)
        {
                this.VerifyPermissions(this, permissions);
        }
        /// <summary>
        /// 验证控件权限。
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="permissions"></param>
        protected virtual void VerifyPermissions(Control ctrl, SecurityPermissionCollection permissions)
        {
            if (ctrl != null && ctrl.HasControls())
            {
                foreach (Control ctr in ctrl.Controls)
                {
                    if (ctr is WebControl)
                        this.VerifyPermissionControls(ctr, permissions);
                    this.VerifyPermissions(ctr, permissions);
                }
            }
        }
        /// <summary>
        /// 控制控件状态。
        /// </summary>
        /// <param name="control">控件。</param>
        /// <param name="permissions">权限集合。</param>
        protected virtual void VerifyPermissionControls(Control control, SecurityPermissionCollection permissions)
        {
            if (control != null)
            {
                ButtonEx btn = control as ButtonEx;
                if (btn == null)
                    return;

                //查询。
                if (btn.ID == "btnSearch")
                {
                    btn.Enabled = (permissions != null) && (permissions[SecurityPermissionConstants.Query] != null);
                }
                //新增。
                if (btn.ID == "btnAdd")
                {
                    btn.Enabled = (permissions != null) && (permissions[SecurityPermissionConstants.Add] != null);
                }
                //删除。
                if (btn.ID == "btnDelete")
                {
                    btn.Enabled = (permissions != null) && (permissions[SecurityPermissionConstants.Delete] != null);
                }
                //保存。
                if (btn.ID == "btnSave")
                {
                    btn.Enabled = (permissions != null) && (permissions[SecurityPermissionConstants.Save] != null);
                }
                //导入。
                if (btn.ID == "btnImport")
                {
                    btn.Enabled = (permissions != null) && (permissions[SecurityPermissionConstants.Import] != null);
                }
                //导出。
                if (btn.ID == "btnExport")
                {
                    btn.Enabled = (permissions != null) && (permissions[SecurityPermissionConstants.Export] != null);
                }
            }
        }
        #endregion
    }
}
