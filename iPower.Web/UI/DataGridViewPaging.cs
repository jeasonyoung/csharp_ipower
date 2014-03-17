//================================================================================
//  FileName: DataGridViewPaging.cs
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
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Globalization;
namespace iPower.Web.UI
{
    /// <summary>
    /// 分页处理部分。
    /// </summary>
    partial class DataGridView
    {
        #region 成员变量。
        PagerSettingsEx pagerSettings;
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置是否启用分页功能。
        /// </summary>
        [Category("Paging")]
        [Description("获取或设置是否启用分页功能。")]
        [DefaultValue(false)]
        public virtual bool AllowPaging
        {
            get
            {
                object obj = this.ViewState["AllowPaging"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                bool allowPaging = this.AllowPaging;
                if (value != allowPaging)
                {
                    this.ViewState["AllowPaging"] = value;
                }
            }
        }
        /// <summary>
        /// 获取或设置分页部分样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置分页部分样式。")]
        public string PagingDataCss
        {
            get
            {
                object obj = this.ViewState["DataGridPagingDataCss"];
                return obj == null ? "DataGridPagingDataCss" : (string)obj;
            }
            set
            {
                this.ViewState["DataGridPagingDataCss"] = value;
            }
        }

        /// <summary>
        /// 获取或设置分页中页面跳转样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置分页中页面跳转样式。")]
        public string FooterInputCSS
        {
            get
            {
                object obj = this.ViewState["DataGridFooterInputCSS"];
                return obj == null ? "DataGridFooterInputCSS" : (string)obj;
            }
            set
            {
                this.ViewState["DataGridFooterInputCSS"] = value;
            }
        }

        /// <summary>
        /// 获取或设置当前显示页的索引。
        /// </summary>
        [Category("Paging")]
        [Description("获取或设置当前显示页的索引。")]
        [Browsable(true)]
        [DefaultValue(0)]
        public virtual int PageIndex
        {
            get
            {
                object obj = this.ViewState["PageIndex"];
                return obj == null ? 0 : (int)obj;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                if (this.PageIndex != value)
                {
                    this.ViewState["PageIndex"] = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置每页上所显示的记录的数目。
        /// </summary>
        [Category("Paging")]
        [Description("获取或设置每页上所显示的记录的数目。")]
        public virtual int PageSize
        {
            get
            {
                object obj = this.ViewState["PageSize"];
                if (obj != null)
                    return (int)obj;
                return 10;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                if (this.PageSize != value)
                {
                    this.ViewState["PageSize"] = value;
                }
            }
        }

        /// <summary>
        /// 获取显示的页数。
        /// </summary>
        [Description("获取显示的页数。")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int PageCount
        {
            get
            {
                object obj = this.ViewState["PageCount"];
                int pageCount = (obj == null) ? 0 : (int)obj;
                return pageCount < 0 ? 0 : pageCount;
            }
            protected set
            {
                this.ViewState["PageCount"] = value;
            }
        }

        /// <summary>
        /// 获取页导航按钮的属性。
        /// </summary>
        [Category("Paging")]
        [Description("获取页导航按钮的属性。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PagerSettingsEx PagerSettings
        {
            get
            {
                if (this.pagerSettings == null)
                {
                    this.pagerSettings = new PagerSettingsEx();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.pagerSettings).TrackViewState();
                    this.pagerSettings.PropertyChanged += new EventHandler(this.OnPagerPropertyChanged);
                }
                return this.pagerSettings;
            }
        }

        int FirstDisplayPageIndex
        {
            get
            {
                object obj = this.ViewState["FirstDisplayPageIndex"];
                return obj == null ? -1 : (int)obj;
            }
            set
            {
                this.ViewState["FirstDisplayPageIndex"] = value;
            }
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 分页操作之前发生。
        /// </summary>
        public event DataGridViewPageEventHandler PageIndexChanging;
        /// <summary>
        /// 触发<see cref="PageIndexChanging"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPageIndexChanging(DataGridViewPageEventArgs e)
        {
            DataGridViewPageEventHandler handler = this.PageIndexChanging;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 分页操作之后发生。
        /// </summary>
        public event EventHandler PageIndexChanged;
        /// <summary>
        /// 触发<see cref="PageIndexChanged"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPageIndexChanged(EventArgs e)
        {
            EventHandler handler = this.PageIndexChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region 辅助函数。
        void HandlePage(int newPage)
        {
            if (this.AllowPaging)
            {
                DataGridViewPageEventArgs e = new DataGridViewPageEventArgs(newPage);
                this.OnPageIndexChanging(e);
                if (!e.Cancel)
                {
                    if (e.NewPageIndex <= -1)
                        return;
                    if ((e.NewPageIndex >= this.PageCount) && (this.PageIndex == this.PageCount - 1))
                        return;
                    this.PageIndex = e.NewPageIndex;
                    this.OnPageIndexChanged(EventArgs.Empty);
                    this.RequiresDataBinding = true;
                }
            }
        }
        void OnPagerPropertyChanged(object sender, EventArgs e)
        {
            if (this.Initialized)
                this.RequiresDataBinding = true;
        }
        void CreateNextPrevPager(TableRow row, PagedDataSourceEx pagedDataSource, bool addFirstLastPageButtons)
        {
            PagerSettingsEx pagerSettings = this.PagerSettings;
            string previousPageImageUrl = pagerSettings.PreviousPageImageUrl;
            string nextPageImageUrl = pagerSettings.NextPageImageUrl;
            bool isFirstPage = pagedDataSource.IsFirstPage;
            bool isLastPage = pagedDataSource.IsLastPage;

            #region 第一页
            if (addFirstLastPageButtons && !isFirstPage)
            {
                IButtonControl control = null;
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                string firstPageImageUrl = pagerSettings.FirstPageImageUrl;
                if (!string.IsNullOrEmpty(firstPageImageUrl))
                {
                    control = new DataControlImageButton(this);
                    ((DataControlImageButton)control).ImageUrl = firstPageImageUrl;
                    ((DataControlImageButton)control).AlternateText = HttpUtility.HtmlDecode(pagerSettings.FirstPageText);
                    ((DataControlImageButton)control).ToolTip = "第一页";
                    ((DataControlImageButton)control).EnableCallback(this.BuildCallbackArgument(0));
                }
                else
                {
                    control = new DataControlPagerLinkButton(this);
                    ((DataControlPagerLinkButton)control).Text = pagerSettings.FirstPageText;
                    ((DataControlPagerLinkButton)control).ToolTip = "第一页";
                    ((DataControlPagerLinkButton)control).EnableCallback(this.BuildCallbackArgument(0));
                }
                control.CommandName = "Page";
                control.CommandArgument = "First";
                cell.Controls.Add((Control)control);
            }
            #endregion

            #region 上一页
            if (!isFirstPage)
            {
                IButtonControl control = null;
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                if (!string.IsNullOrEmpty(previousPageImageUrl))
                {
                    control = new DataControlImageButton(this);
                    ((DataControlImageButton)control).ImageUrl = previousPageImageUrl;
                    ((DataControlImageButton)control).AlternateText = HttpUtility.HtmlDecode(pagerSettings.PreviousPageText);
                    ((DataControlImageButton)control).ToolTip = "上一页";
                    ((DataControlImageButton)control).EnableCallback(this.BuildCallbackArgument(this.PageIndex - 1));
                }
                else
                {
                    control = new DataControlPagerLinkButton(this);
                    ((DataControlPagerLinkButton)control).Text = pagerSettings.PreviousPageText;
                    ((DataControlPagerLinkButton)control).ToolTip = "上一页";
                    ((DataControlPagerLinkButton)control).EnableCallback(this.BuildCallbackArgument(this.PageIndex - 1));
                }
                control.CommandName = "Page";
                control.CommandArgument = "Prev";
                cell.Controls.Add((Control)control);
            }
            #endregion

            #region 下一页
            if (!isLastPage)
            {
                IButtonControl control = null;
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                if (!string.IsNullOrEmpty(nextPageImageUrl))
                {
                    control = new DataControlImageButton(this);
                    ((DataControlImageButton)control).ImageUrl = nextPageImageUrl;
                    ((DataControlImageButton)control).AlternateText = HttpUtility.HtmlDecode(pagerSettings.NextPageText);
                    ((DataControlImageButton)control).ToolTip = "下一页";
                    ((DataControlImageButton)control).EnableCallback(this.BuildCallbackArgument(this.PageIndex + 1));
                }
                else
                {
                    control = new DataControlPagerLinkButton(this);
                    ((DataControlPagerLinkButton)control).Text = pagerSettings.NextPageText;
                    ((DataControlPagerLinkButton)control).ToolTip = "下一页";
                    ((DataControlPagerLinkButton)control).EnableCallback(this.BuildCallbackArgument(this.PageIndex + 1));
                }
                control.CommandName = "Page";
                control.CommandArgument = "Next";
                cell.Controls.Add((Control)control);
            }
            #endregion

            #region 最后页
            if (addFirstLastPageButtons && !isLastPage)
            {
                IButtonControl control = null;
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                string lastPageImageUrl = pagerSettings.LastPageImageUrl;
                if (!string.IsNullOrEmpty(lastPageImageUrl))
                {
                    control = new DataControlImageButton(this);
                    ((DataControlImageButton)control).ImageUrl = lastPageImageUrl;
                    ((DataControlImageButton)control).AlternateText = HttpUtility.HtmlDecode(pagerSettings.LastPageText);
                    ((DataControlImageButton)control).ToolTip = "最后页";
                    ((DataControlImageButton)control).EnableCallback(this.BuildCallbackArgument(pagedDataSource.PageCount - 1));
                }
                else
                {
                    control = new DataControlPagerLinkButton(this);
                    ((DataControlPagerLinkButton)control).Text = pagerSettings.LastPageText;
                    ((DataControlPagerLinkButton)control).ToolTip = "最后页";
                    ((DataControlPagerLinkButton)control).EnableCallback(this.BuildCallbackArgument(pagedDataSource.PageCount - 1));
                }
                control.CommandName = "Page";
                control.CommandArgument = "Last";
                cell.Controls.Add((Control)control);
            }
            #endregion

        }
        void CreateNumericPager(TableRow row, PagedDataSourceEx pagedDataSource, bool addFirstLastPageButtons)
        {
            LinkButton button = null;
            PagerSettingsEx pagerSettings = this.PagerSettings;
            int pageCount = pagedDataSource.PageCount;
            int nextPageIndex = pagedDataSource.CurrentPageIndex + 1;
            int pageButtonCount = pagerSettings.PageButtonCount;
            int firstDisplayPageIndex = this.FirstDisplayPageIndex + 1;
            int pageIndex = (pageCount < pageButtonCount) ? pageCount : pageButtonCount;

            int startIndex = 1;
            if (nextPageIndex > pageIndex)
            {
                if (firstDisplayPageIndex > 0 && ((nextPageIndex - firstDisplayPageIndex >= 0) && (nextPageIndex - firstDisplayPageIndex < pageButtonCount)))
                    startIndex = firstDisplayPageIndex;
                else
                    startIndex = (pagedDataSource.CurrentPageIndex / pageButtonCount) + 1;
                pageIndex = startIndex + pageButtonCount - 1;
                if (pageIndex > pageCount)
                    pageIndex = pageCount;
                if ((pageIndex - startIndex) + 1 < pageButtonCount)
                    startIndex = Math.Max(1, (pageIndex - pageButtonCount + 1));
                this.FirstDisplayPageIndex = startIndex - 1;
            }

            #region 首页
            if (addFirstLastPageButtons && (nextPageIndex != 1) && (startIndex != 1))
            {
                IButtonControl control = null;
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                string firstPageImageUrl = pagerSettings.FirstPageImageUrl;
                if (!string.IsNullOrEmpty(firstPageImageUrl))
                {
                    control = new DataControlImageButton(this);
                    ((DataControlImageButton)control).ImageUrl = firstPageImageUrl;
                    ((DataControlImageButton)control).AlternateText = HttpUtility.HtmlDecode(pagerSettings.FirstPageText);
                    ((DataControlImageButton)control).ToolTip = "第一页";
                    ((DataControlImageButton)control).EnableCallback(this.BuildCallbackArgument(0));
                }
                else
                {
                    control = new DataControlPagerLinkButton(this);
                    ((DataControlPagerLinkButton)control).Text = pagerSettings.FirstPageText;
                    ((DataControlPagerLinkButton)control).ToolTip = "第一页";
                    ((DataControlPagerLinkButton)control).EnableCallback(this.BuildCallbackArgument(0));
                }
                control.CommandName = "Page";
                control.CommandArgument = "First";
                cell.Controls.Add((Control)control);
            }
            #endregion

            #region 前更多
            if (startIndex != 1)
            {
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                button = new DataControlPagerLinkButton(this);
                button.Text = "...";
                button.ToolTip = "更多...";
                button.CommandName = "Page";
                button.CommandArgument = (startIndex - 1).ToString(NumberFormatInfo.InvariantInfo);
                ((DataControlPagerLinkButton)button).EnableCallback(this.BuildCallbackArgument(startIndex - 2));
                cell.Controls.Add(button);
            }
            #endregion

            #region 页索引
            for (int i = startIndex; i <= pageIndex; i++)
            {
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                string str = i.ToString(NumberFormatInfo.InvariantInfo);
                if (i == nextPageIndex)
                {
                    Label child = new Label();
                    child.Text = str;
                    cell.Controls.Add(child);
                }
                else
                {
                    button = new DataControlPagerLinkButton(this);
                    button.Text = str;
                    button.ToolTip = str;
                    button.CommandName = "Page";
                    button.CommandArgument = str;
                    ((DataControlPagerLinkButton)button).EnableCallback(this.BuildCallbackArgument(i - 1));
                    cell.Controls.Add(button);
                }
            }
            #endregion

            #region 后更多
            if (pageCount > pageIndex)
            {
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                button = new DataControlPagerLinkButton(this);
                button.Text = "...";
                button.ToolTip = "更多...";
                button.CommandName = "Page";
                button.CommandArgument = (pageIndex + 1).ToString(NumberFormatInfo.InvariantInfo);
                ((DataControlPagerLinkButton)button).EnableCallback(this.BuildCallbackArgument(pageIndex));
                cell.Controls.Add(button);
            }
            #endregion

            #region 末页
            if (addFirstLastPageButtons && (nextPageIndex != pageCount) && (pageIndex != pageCount))
            {
                IButtonControl control = null;
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                string lastPageImageUrl = pagerSettings.LastPageImageUrl;
                if (!string.IsNullOrEmpty(lastPageImageUrl))
                {
                    control = new DataControlImageButton(this);
                    ((DataControlImageButton)control).ImageUrl = lastPageImageUrl;
                    ((DataControlImageButton)control).AlternateText = HttpUtility.HtmlDecode(pagerSettings.LastPageText);
                    ((DataControlImageButton)control).ToolTip = "最后页";
                    ((DataControlImageButton)control).EnableCallback(this.BuildCallbackArgument(pagedDataSource.PageCount - 1));
                }
                else
                {
                    control = new DataControlPagerLinkButton(this);
                    ((DataControlPagerLinkButton)control).Text = pagerSettings.LastPageText;
                    ((DataControlPagerLinkButton)control).ToolTip = "最后页";
                    ((DataControlPagerLinkButton)control).EnableCallback(this.BuildCallbackArgument(pagedDataSource.PageCount - 1));
                }
                control.CommandName = "Page";
                control.CommandArgument = "Last";
                cell.Controls.Add((Control)control);
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 初始化在分页功能启用时显示的页导航行。
        /// </summary>
        /// <param name="row">表示要初始化的页导航行。</param>
        /// <param name="columnSpan">页导航行应跨越的列数。</param>
        /// <param name="pagedDataSource">数据源。</param>
        protected virtual void InitializePager(DataGridViewRow row, int columnSpan, PagedDataSourceEx pagedDataSource)
        {
            TableCell cell = new TableCell();
            if (columnSpan > 1)
                cell.ColumnSpan = columnSpan;
            PagerSettingsEx pagerSettings = this.PagerSettings;

            Panel panel = new Panel();
            panel.CssClass = this.PagingDataCss;
            Table child = new Table();
            child.CellPadding = 0;
            child.CellSpacing = 0;
            child.BorderWidth = 0;
            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Left;
            switch (pagerSettings.Mode)
            {
                case PagerButtons.NextPrevious:
                    this.CreateNextPrevPager(tr, pagedDataSource, false);
                    break;
                case PagerButtons.Numeric:
                    this.CreateNumericPager(tr, pagedDataSource, false);
                    break;
                case PagerButtons.NextPreviousFirstLast:
                    // this.CreateNextPrevPager(tr, pagedDataSource, true);
                    this.CreateDefaultNextPreviousFirstLastPager(tr, pagedDataSource);
                    break;
                case PagerButtons.NumericFirstLast:
                    this.CreateNumericPager(tr, pagedDataSource, true);
                    break;
            }
            child.Rows.Add(tr);
            panel.Controls.Add(child);
            cell.Controls.Add(panel);
            //导出
            this.CreateExportControl(cell);
             
            row.Cells.Add(cell);

        }
        /// <summary>
        /// 创建默认分页
        /// </summary>
        /// <param name="row"></param>
        /// <param name="pagedDataSource"></param>
        protected virtual void CreateDefaultNextPreviousFirstLastPager(TableRow row, PagedDataSourceEx pagedDataSource)
        {
            PagerSettingsEx pagerSettings = this.PagerSettings;
            bool isFirstPage = pagedDataSource.IsFirstPage;
            bool isLastPage = pagedDataSource.IsLastPage;
            TableCell cell = null;
            if (this.AllowPaging)
            {
                //第一页。
                this.CreatePagerButton(row, pagerSettings.FirstPageImageUrl, "第一页", 0, "First", isFirstPage);
                //上一页
                if (!isFirstPage)
                    this.CreatePagerButton(row, pagerSettings.PreviousPageImageUrl, "上一页", this.PageIndex - 1, "Prev", isFirstPage);
                //下一页
                if (!isLastPage)
                    this.CreatePagerButton(row, pagerSettings.NextPageImageUrl, "下一页", this.PageIndex + 1, "Next", isLastPage);
                //最末页
                this.CreatePagerButton(row, pagerSettings.LastPageImageUrl, "最末页", this.PageCount + 1, "Last", isLastPage);

                //页面统计。
                cell = new TableCell();
                cell.Attributes["nowrap"] = "true";
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes["style"] = "float:left;margin-left:2px;";
                span.InnerText = string.Format("第{0}页/共{1}页 总记录数：{2}",
                    pagedDataSource.CurrentPageIndex + 1,
                    pagedDataSource.PageCount,
                    pagedDataSource.DataSourceCount);
                cell.Controls.Add(span);
                row.Cells.Add(cell);

                //页面跳转。
                cell = new TableCell();
                cell.Attributes["nowrap"] = "true";
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes["class"] = this.FooterInputCSS;

                span = new HtmlGenericControl("span");
                span.Attributes["style"] = "float:left;margin-left:2px;margin-top:3px;";
                span.InnerText = "到第";
                div.Controls.Add(span);

                LinkButton linkBtn = new DataControlPagerLinkButton(this);
                linkBtn.Attributes["style"] = "float:left;margin-top:2px;";
                linkBtn.Text = "[GO]";
                linkBtn.CommandName = "Page";
                linkBtn.CommandArgument = (pagedDataSource.CurrentPageIndex + 1).ToString(NumberFormatInfo.InvariantInfo);
                ((DataControlPagerLinkButton)linkBtn).EnableCallback(this.BuildCallbackArgument(pagedDataSource.CurrentPageIndex));

                TextBox numTextBox = new TextBox();
                numTextBox.Text = (pagedDataSource.CurrentPageIndex + 1).ToString(NumberFormatInfo.InvariantInfo);
                numTextBox.Attributes["style"] = "float:left;";
                numTextBox.Attributes["onkeypress"] = "javascript:return event.keyCode>=48&&event.keyCode<=57;";
                numTextBox.Attributes["ondragenter"] = "javascript:return false;";
                numTextBox.Attributes["onpaste"] = "javascript:return !clipboardData.getData('text').match(/\\D/);";
                numTextBox.Style.Add("ime-mode", "Disabled");
                numTextBox.MaxLength = 5;
                numTextBox.AutoPostBack = true;
                numTextBox.TextChanged += new EventHandler(delegate(object sender, EventArgs e)
                {
                    int pageNo = 1, pageCount = pagedDataSource.PageCount;
                    try
                    {
                        pageNo = int.Parse(((TextBox)sender).Text);
                    }
                    catch { }
                    if (pageNo > pageCount)
                        pageNo = pageCount;
                    linkBtn.CommandArgument = pageNo.ToString(NumberFormatInfo.InvariantInfo);
                    ((DataControlPagerLinkButton)linkBtn).EnableCallback(this.BuildCallbackArgument(pageNo - 1));
                });

                div.Controls.Add(numTextBox);

                span = new HtmlGenericControl("span");
                span.Attributes["style"] = "float:left;margin-top:3px;";
                span.InnerText = "页";
                div.Controls.Add(span);

                div.Controls.Add(linkBtn);

                cell.Controls.Add(div);
                row.Cells.Add(cell);
            }
            else
            {
                cell = new TableCell();
                cell.Controls.Add(new LiteralControl(string.Format("&nbsp; 总记录数：{0}",
                    pagedDataSource.DataSourceCount)));
                row.Cells.Add(cell);
            }
            cell = new TableCell();
            cell.Attributes["align"] = "right";
            LinkButton pagerLinkBtn = new LinkButton();
            pagerLinkBtn.Text = this.AllowPaging ? "[全部]" : "[分页]";
            pagerLinkBtn.CssClass = this.FooterLinkCSS;
            pagerLinkBtn.CausesValidation = false;
            pagerLinkBtn.Command += new CommandEventHandler(delegate(object sender, CommandEventArgs e)
            {
                this.AllowPaging = !this.AllowPaging;
                this.InvokeBuildDataSource();
            });
            cell.Controls.Add(pagerLinkBtn);
            row.Cells.Add(cell);
        }
        void CreatePagerButton(TableRow row, string imageUrl, string text, int pageIndex, string cmdArgument, bool noLink)
        {
            TableCell cell = new TableCell();
            cell.Attributes["nowrap"] = "true";
            if (!noLink)
            {
                IButtonControl btnCtrl;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    btnCtrl = new DataControlImageButton(this);
                    ((DataControlImageButton)btnCtrl).ImageUrl = imageUrl;
                    ((DataControlImageButton)btnCtrl).AlternateText = HttpUtility.HtmlDecode(text);
                    ((DataControlImageButton)btnCtrl).ToolTip = text;
                    ((DataControlImageButton)btnCtrl).EnableCallback(this.BuildCallbackArgument(pageIndex));
                }
                else
                {
                    btnCtrl = new DataControlPagerLinkButton(this);
                    ((DataControlPagerLinkButton)btnCtrl).Text = text;
                    ((DataControlPagerLinkButton)btnCtrl).ToolTip = text;
                    ((DataControlPagerLinkButton)btnCtrl).EnableCallback(this.BuildCallbackArgument(pageIndex));
                }
                btnCtrl.CommandName = "Page";
                btnCtrl.CommandArgument = cmdArgument;
                if (!string.IsNullOrEmpty(this.FooterLinkCSS))
                    ((WebControl)btnCtrl).CssClass = this.FooterLinkCSS;
                cell.Controls.Add((Control)btnCtrl);
            }
            else
            {
                Control ctrl = null;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    ctrl = new System.Web.UI.WebControls.Image();
                    ((System.Web.UI.WebControls.Image)ctrl).ImageUrl = imageUrl;
                    ((System.Web.UI.WebControls.Image)ctrl).AlternateText = HttpUtility.HtmlDecode(text);
                    ((System.Web.UI.WebControls.Image)ctrl).ToolTip = text;
                }
                else
                {
                    ctrl = new Label();
                    ((Label)ctrl).Text = text;
                    ((Label)ctrl).ToolTip = text;
                }
                cell.Controls.Add(ctrl);
            }
            row.Cells.Add(cell);
        }
    }
    /// <summary>
    /// 分页委托。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DataGridViewPageEventHandler(object sender, DataGridViewPageEventArgs e);
    /// <summary>
    /// 为分页提供数据。
    /// </summary>
    public class DataGridViewPageEventArgs : CancelEventArgs
    {
        #region 成员变量，构造函数。
        int newPageIndex;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="newPageIndex"></param>
        public DataGridViewPageEventArgs(int newPageIndex)
        {
            this.newPageIndex = newPageIndex;
        }
        #endregion

        /// <summary>
        /// 获取或设置新页索引。
        /// </summary>
        public int NewPageIndex
        {
            get { return this.newPageIndex; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                this.newPageIndex = value;
            }
        }
    }
}
