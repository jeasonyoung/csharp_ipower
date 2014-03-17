//================================================================================
//  FileName: ChildTable.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/4
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
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 子表格类。
    /// </summary>
    [SupportsEventValidationEx]
    [ToolboxItem(false)]
    public class ChildTable : Table
    {
        #region 成员变量，构造函数。
        string parentID;
        bool parentIDSet;
        int parentLevel;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ChildTable()
            : this(1)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="parentLevel"></param>
        public ChildTable(int parentLevel)
        {
            this.parentLevel = parentLevel;
            this.parentIDSet = false;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="parentID"></param>
        public ChildTable(string parentID)
        {
            this.parentID = parentID;
            this.parentIDSet = true;
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            string parentID = this.parentID;
            if (!this.parentIDSet)
                parentID = this.GetParentID();
            if (parentID != null)
                writer.AddAttribute(HtmlTextWriterAttribute.Id, parentID);
        }
        #endregion

        string GetParentID()
        {
            if (this.ID == null)
            {
                Control parent = this;
                for (int i = 0; i < this.parentLevel; i++)
                {
                    parent = parent.Parent;
                    if (parent == null)
                        break;
                }
                if ((parent != null) && !string.IsNullOrEmpty(parent.ID))
                    return parent.ClientID;
            }
            return null;
        }
    }
}
