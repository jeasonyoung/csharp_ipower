//================================================================================
//  FileName: DataGridViewExport.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/11
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

using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 导出数据。
    /// </summary>
    partial class DataGridView
    {
        #region 成员变量。
        DropDownList ddlFileType;
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置一个值，该值指示是否启用数据导出功能。
        /// </summary>
        [Category("Data")]
        [DefaultValue(false)]
        [Description("获取或设置一个值，该值指示是否启用数据导出功能。")]
        public bool AllowExport
        {
            get
            {
                object o = this.ViewState["AllowExport"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                if (this.AllowExport != value)
                    this.ViewState["AllowExport"] = value;
            }
        }
        /// <summary>
        /// 获取或设置导出保存的文件名。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置导出保存的文件名。")]
        public string DownloadFileName
        {
            get
            {
                string s = (string)this.ViewState["DownloadFileName"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.DownloadFileName != value)
                    this.ViewState["DownloadFileName"] = value;
            }
        }
        /// <summary>
        /// 获取或设置表尾中的连接的样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置表尾中的连接的样式。")]
        public string FooterLinkCSS
        {
            get
            {
                string s = (string)this.ViewState["FooterLinkCSS"];
                return string.IsNullOrEmpty(s) ? "DataGridFooterLinkCSS" : s;
            }
            set
            {
                if (this.FooterLinkCSS != value)
                    this.ViewState["FooterLinkCSS"] = value;
            }
        }
        /// <summary>
        /// 获取或设置导出框样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置导出框样式。")]
        public string ExportDataCss
        {
            get
            {
                string s = (string)this.ViewState["ExportDataCss"];
                return string.IsNullOrEmpty(s) ? "DataGridExportDataCss" : s;
            }
            set
            {
                if (this.ExportDataCss != value)
                    this.ViewState["ExportDataCss"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 数据导出控制。
        /// </summary>
        /// <param name="cell"></param>
        protected virtual void CreateExportControl(TableCell cell)
        {
            if (this.AllowExport)
            {
                Panel panel = new Panel();
                panel.CssClass = this.ExportDataCss;
                panel.Controls.Add(new LiteralControl("导出格式："));

                this.ddlFileType = new DropDownList();
                Type fileType = typeof(ExportFileType);
                foreach (string str in Enum.GetNames(fileType))
                    this.ddlFileType.Items.Add(new ListItem(str, str));
                panel.Controls.Add(this.ddlFileType);

                LinkButton lbExportBtn = new LinkButton();
                lbExportBtn.CssClass = this.FooterLinkCSS;
                lbExportBtn.CausesValidation = false;
                lbExportBtn.Text = lbExportBtn.ToolTip = string.Format("[{0}]", "导出");
                lbExportBtn.Command += new CommandEventHandler(ExportCommand);
                panel.Controls.Add(lbExportBtn);

                cell.Controls.Add(panel);
            }
        }
       
        void ExportCommand(object sender, CommandEventArgs e)
        {
            ExportFileType type = (ExportFileType)Enum.Parse(typeof(ExportFileType), this.ddlFileType.SelectedValue);
            this.ExportCommand(type);
        }
        /// <summary>
        /// 导出数据。
        /// </summary>
        /// <param name="type"></param>
        protected virtual void ExportCommand(ExportFileType type)
        {
            lock (this)
            {
                ExportData export = new ExportData(this);
                export.ExportFileType = type;
                export.DownloadFileName = this.DownloadFileName;
                export.Download();
            }
        }
    }

    /// <summary>
    /// 导出文件的类型。
    /// </summary>
    public enum ExportFileType
    {
        /// <summary>
        /// CSV格式(逗分隔)。
        /// </summary>
        csv,
        /// <summary>
        /// 文本格式(制表符分隔)。
        /// </summary>
        txt
    }
}
