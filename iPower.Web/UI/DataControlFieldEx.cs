//================================================================================
//  FileName: DataControlFieldEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/6
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
namespace iPower.Web.UI
{
    /// <summary>
    /// 用作所有数据控件字段类型的基类。
    /// </summary>
    public abstract class DataControlFieldEx : IStateManager, IDataSourceViewSchemaAccessor
    {
        #region 成员变量，构造函数。
        private StateBag statebag;
        private object dataSourceViewSchema;
        private Control control;
        private Style controlStyle;
        private TableItemStyle footerStyle, headerStyle, itemStyle;
        private bool trackViewState, sortingEnabled;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DataControlFieldEx()
        {
            this.statebag = new StateBag();
            this.dataSourceViewSchema = null;
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 字段变更事件。
        /// </summary>
        internal EventHandler FieldChanged;
        /// <summary>
        /// 触发<see cref="FieldChanged"/>事件。
        /// </summary>
        protected virtual void OnFieldChanged()
        {
            EventHandler handler = this.FieldChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置某些控件中呈现为 AbbreviatedText 属性值的文本。
        /// </summary>
        [Category("Accessibility")]
        [Description("获取或设置某些控件中呈现为 AbbreviatedText 属性值的文本。")]
        public virtual string AccessibleHeaderText
        {
            get { return this.ViewState["AccessibleHeaderText"] as string; }
            set
            {
                string text = this.AccessibleHeaderText;
                if (!string.Equals(value, text))
                {
                    this.ViewState["AccessibleHeaderText"] = value;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置对象所包含的任何 Web 服务器控件的样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置对象所包含的任何 Web 服务器控件的样式。")]
        public Style ControlStyle
        {
            get
            {
                if (this.controlStyle == null)
                {
                    this.controlStyle = new Style();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.controlStyle).TrackViewState();
                }
                return this.controlStyle;
            }
        }

        /// <summary>
        /// 获取或设置数据控件字段的脚注项中显示的文本。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置数据控件字段的脚注项中显示的文本。")]
        [Localizable(true)]
        public virtual string FooterText
        {
            get { return this.ViewState["FooterText"] as string; }
            set
            {
                string text = this.FooterText;
                if (!string.Equals(text, value))
                {
                    this.ViewState["FooterText"] = value;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置数据控件字段脚注的样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置数据控件字段脚注的样式。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
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
        /// 获取或设置数据控件字段的标题项中显示的图像的 URL。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置数据控件字段的标题项中显示的图像的 URL。")]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        [UrlProperty]
        public virtual string HeaderImageUrl
        {
            get { return this.ViewState["HeaderImageUrl"] as string; }
            set
            {
                string text = this.HeaderImageUrl;
                if (!string.Equals(value, text))
                {
                    this.ViewState["HeaderImageUrl"] = value;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置数据控件字段的标题项中显示的文本。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置数据控件字段的标题项中显示的文本。")]
        [Localizable(true)]
        public virtual string HeaderText
        {
            get { return this.ViewState["HeaderText"] as string; }
            set
            {
                string text = this.HeaderText;
                if (!string.Equals(value, text))
                {
                    this.ViewState["HeaderText"] = value;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置数据控件字段标头的样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置数据控件字段标头的样式。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
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
        /// 获取获取由数据控件字段显示的任何基于文本的内容的样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取获取由数据控件字段显示的任何基于文本的内容的样式。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableItemStyle ItemStyle
        {
            get
            {
                if (this.itemStyle == null)
                {
                    this.itemStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.itemStyle).TrackViewState();
                }
                return this.itemStyle;
            }
        }

        /// <summary>
        /// 获取或设置是否呈现数据控件字段的标题项。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置是否呈现数据控件字段的标题项。")]
        public virtual bool ShowHeader
        {
            get
            {
                object obj = this.ViewState["ShowHeader"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                if (value != this.ShowHeader)
                {
                    this.ViewState["ShowHeader"] = value;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置数据源控件用来对数据进行排序的排序表达式。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置数据源控件用来对数据进行排序的排序表达式。")]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceViewSchemaConverter))]
        public virtual string SortExpression
        {
            get { return this.ViewState["SortExpression"] as string; }
            set
            {
                string sort = this.SortExpression;
                if (!string.Equals(value, sort))
                {
                    this.ViewState["SortExpression"] = value;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置指示是否呈现数据控件字段的值。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置指示是否呈现数据控件字段的值。")]
        public bool Visible
        {
            get
            {
                object obj = this.ViewState["Visible"];
                if (obj != null)
                    return (bool)obj;
                return true;
            }
            set
            {
                if (value != this.Visible)
                {
                    this.ViewState["Visible"] = value;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取状态信息的字典。
        /// </summary>
        protected StateBag ViewState
        {
            get { return this.statebag; }
        }
        /// <summary>
        /// 获取对数据控件的引用。
        /// </summary>
        protected Control Control
        {
            get { return this.control; }
        }
        /// <summary>
        /// 是否设计时模式。
        /// </summary>
        protected bool DesignMode
        {
            get
            {
                return (this.control != null) &&
                       (this.control.Site != null) &&
                       this.control.Site.DesignMode;
            }
        }
        #endregion

        #region IStateManager 成员
        /// <summary>
        /// 获取或设置是否将更改保存到其视图状态。
        /// </summary>
        public bool IsTrackingViewState
        {
            get { return this.trackViewState; }
        }
        /// <summary>
        /// 将数据源视图还原为保存过的前一视图状态。
        /// </summary>
        /// <param name="state"></param>
        public virtual void LoadViewState(object state)
        {
            if (state != null)
            {
                object[] objArray = (object[])state;
                if (objArray[0] != null)
                    ((IStateManager)this.ViewState).LoadViewState(objArray[0]);
                if (objArray[1] != null)
                    ((IStateManager)this.ItemStyle).LoadViewState(objArray[1]);
                if (objArray[2] != null)
                    ((IStateManager)this.HeaderStyle).LoadViewState(objArray[2]);
                if (objArray[3] != null)
                    ((IStateManager)this.FooterStyle).LoadViewState(objArray[3]);
            }
        }
        /// <summary>
        /// 保存在页回发到服务器后对视图状态所做的更改。
        /// </summary>
        /// <returns></returns>
        public virtual object SaveViewState()
        {
            object objVS = ((IStateManager)this.ViewState).SaveViewState();
            object objIS = this.itemStyle != null ? ((IStateManager)this.itemStyle).SaveViewState() : null;
            object objHS = this.headerStyle != null ? ((IStateManager)this.headerStyle).SaveViewState() : null;
            object objFS = this.footerStyle != null ? ((IStateManager)this.footerStyle).SaveViewState() : null;
            object objCS = this.controlStyle != null ? ((IStateManager)this.controlStyle).SaveViewState() : null;

            return new object[] { objVS, objIS, objHS, objFS, objCS };
        }
        /// <summary>
        /// 使对象跟踪对其视图状态所做的更改，以便这些更改可以存储在控件的 ViewState 属性中并且能够在同一页的不同请求间得以保持。
        /// </summary>
        public void TrackViewState()
        {
            this.trackViewState = true;
            ((IStateManager)this.ViewState).TrackViewState();
            if (this.itemStyle != null)
                ((IStateManager)this.itemStyle).TrackViewState();
            if (this.headerStyle != null)
                ((IStateManager)this.headerStyle).TrackViewState();
            if (this.footerStyle != null)
                ((IStateManager)this.footerStyle).TrackViewState();
            if (this.controlStyle != null)
                ((IStateManager)this.controlStyle).TrackViewState();
        }

        #endregion

        #region IDataSourceViewSchemaAccessor 成员
        /// <summary>
        /// 基础结构。获取或设置与该对象关联的架构。
        /// </summary>
        public object DataSourceViewSchema
        {
            get { return this.dataSourceViewSchema; }
            set { this.dataSourceViewSchema = value; }
        }

        #endregion


        /// <summary>
        /// 基础结构。创建当前 <see cref="DataControlFieldEx"/> 派生对象的副本。
        /// </summary>
        /// <returns></returns>
        protected internal DataControlFieldEx CloneField()
        {
            DataControlFieldEx newField = this.CreateField();
            this.CopyProperties(newField);
            return newField;
        }

        /// <summary>
        /// 将当前 <see cref="DataControlFieldEx"/> 派生对象的属性复制到指定的 <see cref="DataControlFieldEx"/>  对象。
        /// </summary>
        /// <param name="newField"></param>
        protected virtual void CopyProperties(DataControlFieldEx newField)
        {
            newField.AccessibleHeaderText = this.AccessibleHeaderText;
            newField.ControlStyle.CopyFrom(this.ControlStyle);
            newField.FooterStyle.CopyFrom(this.FooterStyle);
            newField.HeaderStyle.CopyFrom(this.HeaderStyle);
            newField.ItemStyle.CopyFrom(this.ItemStyle);
            newField.FooterText = this.FooterText;
            newField.HeaderImageUrl = this.HeaderImageUrl;
            newField.HeaderText = this.HeaderText;
            newField.ShowHeader = this.ShowHeader;
            newField.SortExpression = this.SortExpression;
            newField.Visible = this.Visible;
        }

        /// <summary>
        /// 当在派生类中重写时，创建一个空的 <see cref="DataControlFieldEx"/> 派生对象。
        /// </summary>
        /// <returns></returns>
        protected abstract DataControlFieldEx CreateField();

        /// <summary>
        /// 从当前表格单元格中提取数据控件字段的值，并将该值添加到指定的 <see cref="IDictionary"/> 集合中。
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="cell"></param>
        /// <param name="rowState"></param>
        /// <param name="includeReadOnly">如果要指示只读字段的值包括在 dictionary 集合中，则为 true；否则为 false。</param>
        public virtual void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCellEx cell, DataGridViewRowState rowState, bool includeReadOnly)
        {
        }

        /// <summary>
        /// 为数据控件字段执行基础实例初始化。
        /// </summary>
        /// <param name="sortingEnabled">是否支持数据列排序的值。</param>
        /// <param name="control">拥有 <see cref="DataControlFieldEx"/> 的数据控件。</param>
        /// <returns>始终返回 false。</returns>
        public virtual bool Initialize(bool sortingEnabled, Control control)
        {
            this.sortingEnabled = sortingEnabled;
            this.control = control;
            return false;
        }

        /// <summary>
        /// 将文本或控件添加到单元格的控件集合中。
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public virtual void InitializeCell(DataControlFieldCellEx cell, DataControlCellType cellType, DataGridViewRowState rowState, int rowIndex)
        {
            WebControl control = null;
            string sortExpression = string.Empty, headerText = string.Empty;
            ImageButton button = null;

            switch (cellType)
            {
                case DataControlCellType.Header:
                    {
                        control = null;
                        sortExpression = this.SortExpression;
                        bool bSort = this.sortingEnabled && !string.IsNullOrEmpty(sortExpression);
                        string headerImageUrl = this.HeaderImageUrl;
                        headerText = this.HeaderText;
                        if (string.IsNullOrEmpty(headerImageUrl))
                        {
                            if (bSort)
                            {
                                LinkButton link = null;
                                IPostBackContainer container = this.control as IPostBackContainer;
                                if (container != null)
                                {
                                    link = new DataControlLinkButton(container);
                                    ((DataControlLinkButton)link).EnableCallback(null);
                                }
                                else
                                    link = new LinkButton();
                                link.Text = headerText;
                                link.CommandName = "Sort";
                                link.CommandArgument = sortExpression;

                                if (!(link is DataControlLinkButton))
                                    link.CausesValidation = false;
                                control = link;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(headerText))
                                    headerText = "&nbsp;";
                                cell.Text = headerText;
                            }
                            goto Label_015C;
                        }
                        if (!bSort && !string.IsNullOrEmpty(headerImageUrl))
                        {
                            Image image = new Image();
                            image.ImageUrl = headerImageUrl;
                            control = image;
                            image.AlternateText = headerText;
                            goto Label_015C;
                        }

                        IPostBackContainer contrainer = this.control as IPostBackContainer;
                        if (contrainer == null)
                        {
                            button = new ImageButton();
                            break;
                        }
                        button = new DataControlImageButton(contrainer);
                        ((DataControlImageButton)button).EnableCallback(null);
                    }
                    break;
                case DataControlCellType.Footer:
                    {
                        string footerText = this.FooterText;
                        if (string.IsNullOrEmpty(footerText))
                            footerText = "&nbsp;";
                        cell.Text = footerText;
                    }
                    return;
                default:
                    return;
            }
            button.ImageUrl = this.HeaderImageUrl;
            button.CommandName = "Sort";
            button.CommandArgument = sortExpression;
            if (!(button is DataControlImageButton))
                button.CausesValidation = false;
            button.AlternateText = headerText;
            control = button;

        Label_015C:
            if (control != null)
                cell.Controls.Add(control);

        }
        /// <summary>
        /// 设置对象状态。
        /// </summary>
        public void SetDirty()
        {
            this.statebag.SetDirty(true);
            if (this.itemStyle != null)
                this.itemStyle.SetDirty();
            if (this.headerStyle != null)
                this.headerStyle.SetDirty();
            if (this.footerStyle != null)
                this.footerStyle.SetDirty();
            if (this.controlStyle != null)
                this.controlStyle.SetDirty();
        }

        /// <summary>
        /// 当在派生类中重写时，发出信号表示字段所包含的控件支持回调。
        /// </summary>
        public virtual void ValidateSupportsCallback()
        {
            throw new NotSupportedException("DataControlFieldEx_CallbacksNotSupported");
        }
    }
}