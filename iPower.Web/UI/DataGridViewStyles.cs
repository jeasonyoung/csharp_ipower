//================================================================================
//  FileName: DataGridViewStyles.cs
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
    /// 样式控制。
    /// </summary>
    partial class DataGridView
    {
        #region 成员变量。
        TableItemStyle headerStyle, footerStyle, alternatingRowStyle, rowStyle;
        DataGridViewRow headerRow, footerRow;
        #endregion

        #region 属性。
        /// <summary>
        ///  获取或设置背景中显示的图像的 URL。
        /// </summary>
        [UrlProperty]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        [Category("Appearance")]
        [Description("获取或设置背景中显示的图像的 URL。")]
        public virtual string BackImageUrl
        {
            get
            {
                if (!this.ControlStyleCreated)
                    return string.Empty;
                return ((TableStyle)this.ControlStyle).BackImageUrl;
            }
            set
            {
                ((TableStyle)this.ControlStyle).BackImageUrl = value;
            }
        }

        /// <summary>
        /// 获取或设置标题元素中呈现的文本。
        /// </summary>
        [Category("Accessibility")]
        [Description("获取或设置标题元素中呈现的文本。")]
        [Localizable(true)]
        public virtual string Caption
        {
            get { return this.ViewState["Caption"] as string; }
            set { this.ViewState["Caption"] = value; }
        }

        /// <summary>
        ///  获取或设置标题元素的水平或垂直位置。
        /// </summary>
        [Category("Accessibility")]
        [Description("获取或设置标题元素的水平或垂直位置。")]
        [DefaultValue(0)]
        public virtual TableCaptionAlign CaptionAlign
        {
            get
            {
                object obj = this.ViewState["CaptionAlign"];
                if (obj == null)
                    return TableCaptionAlign.NotSet;
                return (TableCaptionAlign)obj;
            }
            set
            {
                if ((value < TableCaptionAlign.NotSet) || (value > TableCaptionAlign.Right))
                    throw new ArgumentOutOfRangeException("value");
                this.ViewState["CaptionAlign"] = value;
            }
        }

        /// <summary>
        /// 获取或设置单元格的内容和单元格的边框之间的空间量。
        /// </summary>
        [Category("Layout")]
        [Description("获取或设置单元格的内容和单元格的边框之间的空间量。")]
        [DefaultValue(-1)]
        public virtual int CellPadding
        {
            get
            {
                if (!this.ControlStyleCreated)
                    return -1;
                return ((TableStyle)this.ControlStyle).CellPadding;
            }
            set { ((TableStyle)this.ControlStyle).CellPadding = value; }
        }

        /// <summary>
        /// 获取或设置单元格间的空间量。
        /// </summary>
        [Category("Layout")]
        [Description("获取或设置单元格间的空间量。")]
        [DefaultValue(0)]
        public virtual int CellSpacing
        {
            get
            {
                if (!this.ControlStyleCreated)
                    return 0;
                return ((TableStyle)this.ControlStyle).CellSpacing;
            }
            set { ((TableStyle)this.ControlStyle).CellSpacing = value; }
        }

        /// <summary>
        /// 获取或设置控件在页面上的水平对齐方式。
        /// </summary>
        [Category("Layout")]
        [Description("获取或设置控件在页面上的水平对齐方式。")]
        [DefaultValue(0)]
        public virtual HorizontalAlign HorizontalAlign
        {
            get
            {
                if (!this.ControlStyleCreated)
                    return HorizontalAlign.NotSet;
                return ((TableStyle)this.ControlStyle).HorizontalAlign;
            }
            set
            {
                ((TableStyle)this.ControlStyle).HorizontalAlign = value;
            }
        }

        /// <summary>
        /// 获取或设置控件的网格线样式。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置控件的网格线样式。")]
        [DefaultValue(3)]
        public virtual GridLines GridLines
        {
            get
            {
                if (!this.ControlStyleCreated)
                    return GridLines.Both;
                return ((TableStyle)this.ControlStyle).GridLines;
            }
            set { ((TableStyle)this.ControlStyle).GridLines = value; }
        }
        /// <summary>
        /// 获取或设置鼠标移动时高亮显示的样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置鼠标移动时高亮显示的样式。")]
        public string MouseoverCssClass
        {
            get
            {
                string s = (string)this.ViewState["MouseoverCssClass"];
                return string.IsNullOrEmpty(s) ? "DataGridHighLight" : s;
            }
            set
            {
                if (this.MouseoverCssClass != value)
                    this.ViewState["MouseoverCssClass"] = value;
            }
        }
        #endregion

        #region 外观属性。
        /// <summary>
        /// 获取控件中交替数据行的外观。
        /// </summary>
        [Category("Styles")]
        [Description("获取控件中交替数据行的外观。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TableItemStyle AlternatingRowStyle
        {
            get
            {
                if (this.alternatingRowStyle == null)
                {
                    this.alternatingRowStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.alternatingRowStyle).TrackViewState();
                }
                return this.alternatingRowStyle;
            }
        }

        /// <summary>
        /// 获取控件中的数据行的外观。
        /// </summary>
        [Category("Styles")]
        [Description("获取控件中的数据行的外观。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TableItemStyle RowStyle
        {
            get
            {
                if (this.rowStyle == null)
                {
                    this.rowStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.rowStyle).TrackViewState();
                }
                return this.rowStyle;
            }
        }

        /// <summary>
        /// 获取或设置显示标题行。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置显示标题行。")]
        [DefaultValue(true)]
        public virtual bool ShowHeader
        {
            get
            {
                object obj = this.ViewState["ShowHeader"];
                if (obj != null)
                    return (bool)obj;
                return true;
            }
            set
            {
                bool showHeader = this.ShowHeader;
                if (value != showHeader)
                {
                    this.ViewState["ShowHeader"] = value;
                    if (this.Initialized)
                        this.RequiresDataBinding = true;
                }
            }
        }
        /// <summary>
        /// 获取标题行的外观。
        /// </summary>
        [Category("Styles")]
        [Description("获取标题行的外观。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableItemStyle HeaderStyle
        {
            get
            {
                if (this.headerStyle == null)
                {
                    this.headerStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.headerStyle).TrackViewState();
                }
                return this.headerStyle;
            }
        }

        /// <summary>
        /// 获取标题行对象。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual DataGridViewRow HeaderRow
        {
            get
            {
                if (this.headerRow == null)
                    this.EnsureChildControls();
                return this.headerRow;
            }
        }

        /// <summary>
        /// 获取或设置是否显示脚注行。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置是否显示脚注行。")]
        public virtual bool ShowFooter
        {
            get
            {
                object obj = this.ViewState["ShowFooter"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                if (value != this.ShowFooter)
                {
                    this.ViewState["ShowFooter"] = value;
                    if (this.Initialized)
                        this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// 获取脚注行的外观。
        /// </summary>
        [Category("Styles")]
        [Description("获取脚注行的外观。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableItemStyle FooterStyle
        {
            get
            {
                if (this.footerStyle == null)
                {
                    this.footerStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.footerStyle).TrackViewState();
                }
                return this.footerStyle;
            }
        }

        /// <summary>
        /// 获取脚注行对象。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRow FooterRow
        {
            get
            {
                if (this.footerRow == null)
                    this.EnsureChildControls();
                return this.footerRow;
            }
        }
        #endregion
    }
}
