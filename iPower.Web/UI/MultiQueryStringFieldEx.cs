//================================================================================
//  FileName:MultiQueryStringFieldEx.cs
//  Desc:DataGridView 控件的列类型。
//
//  Called by
//
//  Auth:杨勇（yangyong@sxt.com.cn）
//  Date:2008-3-4
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//
//
//================================================================================
//  Copyright (C) 2004-2008 SXT Corporation
//================================================================================

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.ComponentModel;

using iPower.Utility;
namespace iPower.Web.UI
{
    /// <summary>
    /// <see cref="DataGridView"/> 控件的列类型。
    /// </summary>
    [ToolboxData("<{0}:MultiQueryStringFieldEx runat=server></{0}:MultiQueryStringFieldEx>")]
    public class MultiQueryStringFieldEx : BoundFieldEx
    {
        #region  成员变量，构造函数，析构函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public MultiQueryStringFieldEx()
            : base()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置数据源中要绑定到Column中的超级链接的 URL 的字段。
        /// </summary>
        [Category("URLs")]
        [Description("获取或设置数据源中要绑定到Column中的超级链接的 URL 的字段。")]
        public string DataNavigateUrlField
        {
            get
            {
                string s = (string)this.ViewState["DataNavigateUrlField "];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.DataNavigateUrlField != value)
                    this.ViewState["DataNavigateUrlField "] = value;
            }
        }
        /// <summary>
        /// 获取或设置当 URL 数据绑定到数据源中的字段时，Column 中的超级链接的 URL 的显示格式。
        /// </summary>
        [Category("URLs")]
        [Description("获取或设置当 URL 数据绑定到数据源中的字段时，Column 中的超级链接的 URL 的显示格式。")]
        public string DataNavigateUrlFormatString
        {
            get
            {
                string s = (string)this.ViewState["DataNavigateUrlFormatString"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.DataNavigateUrlFormatString != value)
                    this.ViewState["DataNavigateUrlFormatString"] = value;
            }
        }
        /// <summary>
        /// 获取或设置当单击 列中的超级链接时链接到的 URL。
        /// </summary>
        [Category("URLs")]
        [Description(" 获取或设置当单击 列中的超级链接时链接到的 URL。")]
        public string NavigateUrl
        {
            get
            {
                string s = (string)this.ViewState["NavigateUrl"];
                return String.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.NavigateUrl != value)
                    this.ViewState["NavigateUrl"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否弹出窗口,默认为false。
        /// </summary>
        [Category("URLs")]
        [Description("获取或设置是否弹出窗口,默认为false。")]
        public bool PopupWin
        {
            get
            {
                object o = this.ViewState["PopupWin"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                if (this.PopupWin != value)
                    this.ViewState["PopupWin"] = value;
            }
        }
        /// <summary>
        /// 获取或设置弹出窗口的类别，默认为普通窗口
        /// </summary>
        [Category("URLs")]
        [Description("获取或设置弹出窗口的类别，默认为普通窗口。")]
        public EnumWindowType WinType
        {
            get
            {
                object o = this.ViewState["WinType"];
                return (o == null) ? EnumWindowType.Normal : (EnumWindowType)o;
            }
            set
            {
                if (this.WinType != value)
                    this.ViewState["WinType"] = value;
            }
        }
        /// <summary>
        /// 获取或设置窗口的高度。
        /// </summary>
        [Category("URLs")]
        [Description("获取或设置窗口的高度。")]
        public Unit WinHeight
        {
            get
            {
                object o = this.ViewState["WinHeight"];
                return (o == null) ? new Unit("300px") : (Unit)o;
            }
            set
            {
                if(this.WinHeight != value)
                this.ViewState["WinHeight"] = value;
            }
        }
        /// <summary>
        /// 获取或设置窗口的宽度。
        /// </summary>
        [Category("URLs")]
        [Description("获取或设置窗口的宽度。")]
        public Unit WinWidth
        {
            get
            {
                object o = this.ViewState["WinWidth"];
                return (o == null) ? new Unit("400px") : (Unit)o;
            }
            set
            {
                if(this.WinWidth != value)
                this.ViewState["WinWidth"] = value;
            }
        }
        /// <summary>
        /// 获取或设置单击列中的超级链接时显示链接到的 Web 页内容的目标窗口或框架。
        /// </summary>
        [Category("URLs")]
        [Description("获取或设置单击列中的超级链接时显示链接到的 Web 页内容的目标窗口或框架。")]
        public string Target
        {
            get
            {
                object o = ViewState["Target"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["Target"] = value;
            }
        }
        #endregion 

        #region  重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newField"></param>
        protected override void CopyProperties(DataControlFieldEx newField)
        {
            ((MultiQueryStringFieldEx)newField).DataField = this.DataField;
            base.CopyProperties(newField);
            ((MultiQueryStringFieldEx)newField).NavigateUrl = this.NavigateUrl;
            ((MultiQueryStringFieldEx)newField).DataNavigateUrlFormatString = this.DataNavigateUrlFormatString;
            ((MultiQueryStringFieldEx)newField).DataNavigateUrlField = this.DataNavigateUrlField;
            ((MultiQueryStringFieldEx)newField).PopupWin = this.PopupWin;
            ((MultiQueryStringFieldEx)newField).WinType = this.WinType;
            ((MultiQueryStringFieldEx)newField).WinHeight = this.WinHeight;
            ((MultiQueryStringFieldEx)newField).WinWidth = this.WinWidth;
            ((MultiQueryStringFieldEx)newField).Target = this.Target;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DataControlFieldEx CreateField()
        {
            return new MultiQueryStringFieldEx();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object GetDesignTimeValue()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public override void InitializeCell(DataControlFieldCellEx cell, 
                                            DataControlCellType cellType, 
                                            DataGridViewRowState rowState, 
                                            int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            DataGridView owner = this.Control as DataGridView;
            if (owner == null)
                return;
            else if (cellType == DataControlCellType.Header && this.PopupWin)
            {
                if (this.WinType == EnumWindowType.Modal)
                    this.BuildModalWin(owner);
                else
                    this.BuildNormalWin(owner);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="rowState"></param>
        protected override void InitializeDataCell(DataControlFieldCellEx cell,
                                                   DataGridViewRowState rowState)
        {
            HtmlAnchor anchor = new HtmlAnchor();
            Control dataBindingContainer = anchor as Control;
            if (dataBindingContainer != null)
                dataBindingContainer.DataBinding += this.OnDataBindField;
            cell.Controls.Add(anchor);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDataBindField(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            HtmlAnchor anchorContainer = control as HtmlAnchor;
            if (anchorContainer != null)
            {
                if (!this.DesignMode)
                {
                    string sUrl = string.Empty;
                    if (!string.IsNullOrEmpty(this.DataNavigateUrlField))
                    {
                        object[] dataValue = this.GetValue(control.NamingContainer, this.DataNavigateUrlField);
                        sUrl = this.FormatDataValue(this.DataNavigateUrlFormatString, dataValue);
                    }
                    else
                    {
                        sUrl = this.NavigateUrl;
                    }

                    #region 链接。
                    if (!string.IsNullOrEmpty(sUrl))
                    {
                        if (this.PopupWin)//弹出窗口。
                        {
                            anchorContainer.HRef = string.Format("{0}",
                                this.WinType == EnumWindowType.Normal2 ? sUrl : "#");
                            if (this.WinType != EnumWindowType.Normal2)
                            {
                                DataGridView owner = this.Control as DataGridView;
                                if (owner != null)
                                    anchorContainer.Attributes["onclick"] = string.Format("javascript:{0}_{1}_{2}('{3}');",
                                                        owner.ClientID, this.DataField,
                                                        this.WinType == EnumWindowType.Modal ? "OpenModal" : "OpenNormal",
                                                        sUrl);
                            }
                        }
                        else
                        {
                            anchorContainer.HRef = sUrl;
                            if (!string.IsNullOrEmpty(this.Target))
                                anchorContainer.Target = this.Target;
                        }
                    }
                    #endregion
                }

                if (this.ShowToolTip && !string.IsNullOrEmpty(this.ToolTipField))
                {
                    object[] values = this.GetValue(control.NamingContainer, this.ToolTipField);
                    anchorContainer.Title = this.FormatDataValue(this.ToolTipFieldFormatString, values);
                }

                string strValue = this.GetValue(control.NamingContainer);
                
                int charCount = this.CharCount;
                if (charCount > 0)
                    strValue = ConvertEx.CutString(strValue, charCount, 1);
                anchorContainer.InnerText = strValue;
                if (this.WinType == EnumWindowType.Normal2)
                {
                    anchorContainer.Target = this.Target;
                }
            }
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 生成并注册弹出普通窗口的脚本。
        /// </summary>
        /// <param name="owner"></param>
        private void BuildNormalWin(DataGridView owner)
        {
            if (owner != null)
            {
                ClientScriptManager scriptManager = owner.Page.ClientScript;
                string columnSign = this.DataField;
                string scriptKey = string.Format("{0}_{1}_OpenNormal", owner.ClientID, columnSign);
                if (!scriptManager.IsClientScriptBlockRegistered(scriptKey))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type=\"text/javascript\">");
                    sb.AppendFormat("function {0}_{1}_OpenNormal(strUrl)", owner.ClientID, columnSign);
                    sb.Append("{");
                    sb.AppendFormat("window.open(strUrl,'{0}','height={1},width={2},status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes');",
                                    this.Target, this.WinHeight, this.WinWidth);
                    sb.Append("}");
                    sb.Append("</script>");

                    scriptManager.RegisterClientScriptBlock(this.GetType(), scriptKey, sb.ToString());
                }
            }
        }
        /// <summary>
        /// 生成并注册弹出模式窗口的脚本。
        /// </summary>
        /// <param name="owner"></param>
        private void BuildModalWin(DataGridView owner)
        {
            if (owner != null)
            {
                ClientScriptManager scriptManager = owner.Page.ClientScript;
                string columnSign = this.DataField;
                string scriptKey = string.Format("{0}_{1}_OpenModal", owner.ClientID, columnSign);
                if (!scriptManager.IsClientScriptBlockRegistered(scriptKey))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type=\"text/javascript\">\r\n");
                    sb.AppendFormat("function {0}_{1}_OpenModal(strUrl)\r\n",
                            owner.ClientID,
                            columnSign);
                    sb.Append("{\r\n");
                    sb.Append("\tvar vTmd=Math.random();\r\n");
                    sb.Append("\tvar vReturn='';\r\n");
                    sb.Append("\tif(strUrl.indexOf('?')>-1)\r\n");
                    sb.Append("\t{\r\n");
                    sb.AppendFormat("\t\tvReturn=window.showModalDialog(strUrl+'&tmd='+vTmd,null,'dialogWidth:{0};dialogHeight:{1};help:0');\r\n",
                        this.WinWidth,
                        this.WinHeight);
                    sb.Append("\t}\r\n");
                    sb.Append("\telse\r\n");
                    sb.Append("\t{\r\n");
                    sb.AppendFormat("\t\tvReturn=window.showModalDialog(strUrl+'?tmd='+vTmd,null,'dialogWidth:{0};dialogHeight:{1};help:0');\r\n",
                        this.WinWidth,
                        this.WinHeight);
                    sb.Append("\t}\r\n");

                    sb.Append("\tif(typeof(vReturn)!=\"undefined\" && vReturn!=\"\")\r\n"); 
                    sb.Append("\t{\r\n");
                    sb.AppendFormat("\t\t{0}\r\n", scriptManager.GetPostBackEventReference(owner, owner.ClientID));
                    
                    sb.Append("\t}\r\n");

                    sb.Append("}\r\n");
                    sb.Append("</script>\r\n");
                    scriptManager.RegisterClientScriptBlock(this.GetType(), scriptKey, sb.ToString());
                }
            }
        }
        #endregion
    }
}