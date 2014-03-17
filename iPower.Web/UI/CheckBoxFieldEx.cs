//================================================================================
//  FileName: CheckBoxFieldEx.cs
//  Desc: 表示数据绑定控件中作为CheckBox显示的字段扩展。  
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-17
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.Globalization;
using System.Web.Util;
using System.Drawing;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示数据绑定控件中作为CheckBox<see cref="CheckBox"/>显示的字段扩展。 
    /// </summary>
    [ToolboxData("<{0}:CheckBoxFieldEx runat=server></{0}:CheckBoxFieldEx>")]
    [ToolboxBitmap(typeof(CheckBoxFieldEx))]
    public class CheckBoxFieldEx : BoundFieldEx
    {
        #region 成员变量，构造函数，析构函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CheckBoxFieldEx()
            : base()
        {
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 将当前 CheckBoxFieldEx<seealso cref="CheckBoxFieldEx"/> 对象的属性复制到指定的 DataControlField<see cref="DataControlField"/> 对象。
        /// </summary>
        /// <param name="newField">当前 CheckBoxFieldEx<seealso cref="CheckBoxFieldEx"/> 的属性将复制到的 DataControlField<see cref="DataControlField"/>。</param>
        protected override void CopyProperties(DataControlFieldEx newField)
        {
            ((CheckBoxFieldEx)newField).DataField = this.DataField;
            base.CopyProperties(newField);
        }
        /// <summary>
        /// 创建一个空的 CheckBoxFieldEx<seealso cref="CheckBoxFieldEx"/> 对象。
        /// </summary>
        /// <returns>一个空 CheckBoxFieldEx<seealso cref="CheckBoxFieldEx"/>。</returns>
        protected override DataControlFieldEx CreateField()
        {
            return new CheckBoxFieldEx();
        }
        /// <summary>
        /// 检索在设计器中呈现 CheckBoxFieldEx<seealso cref="CheckBoxFieldEx"/> 对象时用于字段值的值。
        /// </summary>
        /// <returns>要在设计器中作为字段值显示的值。</returns>
        protected override object GetDesignTimeValue()
        {
            return "CheckBox";
        }
        /// <summary>
        /// 将指定的 TableCell 对象初始化为指定的行状态。
        /// </summary>
        /// <param name="cell">要初始化的 TableCell。</param>
        /// <param name="cellType">DataControlCellType<see cref="DataControlCellType"/> 值之一。</param>
        /// <param name="rowState">DataControlRowState<see cref="DataControlRowState"/> 值之一。</param>
        /// <param name="rowIndex">从零开始的行索引。</param>
        public override void InitializeCell(DataControlFieldCellEx cell,
                                            DataControlCellType cellType,
                                            DataGridViewRowState rowState,
                                            int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            DataGridView owner = this.Control as DataGridView;
            if (owner == null)
                return;
            else if (cellType == DataControlCellType.Header)
            {
                ClientScriptManager scriptManager = owner.Page.ClientScript;
                string scriptKey = string.Format("{0}_SelectAll", owner.ClientID);
                if (!scriptManager.IsClientScriptBlockRegistered(this.GetType(), scriptKey))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type=\"text/javascript\">\r\n");
                    sb.AppendFormat("function {0}_SelectAll()\r\n", owner.ClientID);
                    sb.Append("{\r\n");
                    sb.AppendFormat("\tvar c=document.all.{0}_cbSelectAll.checked;\r\n", owner.ClientID);
                    sb.AppendFormat("\tvar cb=document.all.{0}.getElementsByTagName(\"input\");\r\n", owner.ClientID);
                    sb.Append("\tif(cb && cb!=null)\r\n");
                    sb.Append("\t{\r\n");
                    sb.Append("\t\tfor(var i=0;i<cb.length;i++)\r\n");
                    sb.Append("\t\t{\r\n");
                    sb.AppendFormat("\t\t\tif(cb[i].type==\"checkbox\" && cb[i].id.indexOf(\"{0}\")>-1)\r\n", owner.ClientID);
                    sb.Append("\t\t\t{\r\n");
                    sb.Append("\t\t\tcb[i].checked=c;\r\n");
                    sb.Append("\t\t\t}\r\n");
                    sb.Append("\t\t}\r\n");
                    sb.Append("\t}\r\n");
                    sb.Append("}\r\n");
                    sb.Append("</script>\r\n");

                    scriptManager.RegisterClientScriptBlock(this.GetType(), scriptKey, sb.ToString());
                }
                HtmlInputCheckBox cb = new HtmlInputCheckBox();
                cb.Attributes["ID"] = string.Format("{0}_cbSelectAll", owner.ClientID);
                cb.Attributes["onclick"] = string.Format("javascript:{0}_SelectAll()", owner.ClientID);
                cell.Controls.Add(cb);
            }
        }
        /// <summary>
        /// 将指定的 TableCell 对象初始化为指定的行状态。
        /// </summary>
        /// <param name="cell">要初始化的 TableCell。</param>
        /// <param name="rowState">DataControlRowState<see cref="DataControlRowState"/> 值之一。</param>
        protected override void InitializeDataCell(DataControlFieldCellEx cell, DataGridViewRowState rowState)
        {
            HtmlInputCheckBox cb = new HtmlInputCheckBox();
            cb.ID = string.Format("{0}_cbSelect", ((DataGridView)this.Control).ClientID);

            Control dataBindingContainer = (Control)cb;
            dataBindingContainer.DataBinding += this.OnDataBindField;
            cell.Controls.Add(cb);
        }
        /// <summary>
        /// 将字段的值绑定到 CheckBoxFieldEx<seealso cref="CheckBoxFieldEx"/> 对象。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">包含事件数据的 EventArgs<see cref="EventArgs"/>。</param>
        protected override void OnDataBindField(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            HtmlInputCheckBox checkContainer = control as HtmlInputCheckBox;
            if (checkContainer != null)
            {
                string strValue = this.GetValue(((Control)sender).NamingContainer);
                checkContainer.Value = strValue;
                DataGridView owner = this.Control as DataGridView;
                if (owner != null && !string.IsNullOrEmpty(strValue))
                {
                    StringCollection collection = owner.CheckedValue;
                    if (collection != null && collection.Contains(strValue))
                        checkContainer.Checked = true;
                }
            }
        }
        #endregion
    }
}