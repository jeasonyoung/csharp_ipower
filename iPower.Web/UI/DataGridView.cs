//================================================================================
//  FileName: DataGridView.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/3
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
using System.Globalization;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;

using System.IO;

using iPower.Utility;
namespace iPower.Web.UI
{
    /// <summary>
    /// 数据呈现控件。
    /// </summary>
    [ToolboxData("<{0}:DataGridView runat=server></{0}:DataGridView>")]
    [ToolboxBitmap(typeof(DataGridView))]
    public partial class DataGridView : CompositeDataBoundControlEx, IPostBackContainer, IPostBackEventHandler, ICallbackContainer, ICallbackEventHandler
    {
        #region 成员变量，构造函数。
        bool renderClientScriptValid, renderClientScript;
        IStateFormatter stateFormatter;
        DataGridViewRowCollection rowsCollection;
        DataControlFieldExCollection fieldCollection;
        ArrayList rowsArray;
        StringCollection checkedValueCollection;
        OrderedDictionary boundFieldValues;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DataGridView()
        {
            
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取序列化对象。
        /// </summary>
        protected IStateFormatter StateFormatter
        {
            get
            {
                if (this.stateFormatter == null)
                    this.stateFormatter = new ObjectStateFormatter();
                return this.stateFormatter;
            }
        }
        /// <summary>
        /// 获取或设置客户端回调是否用于排序和分页操作。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置客户端回调是否用于排序和分页操作。")]
        [DefaultValue(false)]
        public virtual bool EnableSortingAndPagingCallbacks
        {
            get
            {
                object obj = this.ViewState["EnableSortingAndPagingCallbacks"];
                return (obj != null) && (bool)obj;
            }
            set { this.ViewState["EnableSortingAndPagingCallbacks"] = value; }
        }

        /// <summary>
        /// 获取数据行的对象的集合。
        /// </summary>
        [Description("获取数据行的对象的集合。")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual DataGridViewRowCollection Rows
        {
            get
            {
                if (this.rowsCollection == null)
                {
                    if (this.rowsArray == null)
                    {
                        this.EnsureChildControls();
                        this.rowsArray = new ArrayList();
                    }
                    this.rowsCollection = new DataGridViewRowCollection(this.rowsArray);
                }
                return this.rowsCollection;
            }
        }

        #region CheckedValue
        ///<summary>
        ///获取或设置选择列默认选中的值的集合,
        ///(仅有CheckBoxColumn列时该属性才起作用)。
        ///</summary>
        [Browsable(false)]
        [Bindable(false)]
        [Description("设置或获取选择列选中的值集合。)")]
        public StringCollection CheckedValue
        {
            get
            {
                if (this.checkedValueCollection == null)
                    this.checkedValueCollection = new StringCollection();
                else
                    this.checkedValueCollection.Clear();
                if (!this.DesignMode && this.Page != null && this.Page.Request != null)
                {
                    string strCheckValue = string.Empty;
                    foreach (string str in this.Page.Request.Form.AllKeys)
                    {
                        if (str.EndsWith("_cbSelect"))
                        {
                            strCheckValue = this.Page.Request[str];
                            if (!string.IsNullOrEmpty(strCheckValue))
                                 this.checkedValueCollection.Add(strCheckValue);
                         }
                    }
                }
                return this.checkedValueCollection;
            }
            set
            {
                this.checkedValueCollection = value;
                if (this.checkedValueCollection != null)
                {
                    foreach (DataGridViewRow row in this.Rows)
                    {
                        foreach (object obj in row.Cells[0].Controls)
                        {
                            if (obj is HtmlInputCheckBox)
                                 ((HtmlInputCheckBox)obj).Checked = this.checkedValueCollection.Contains(((HtmlInputCheckBox)obj).Value);
                         }
                    }
                }
            }
        }
        /// <summary>
        /// 获取已选择行的ID列表,以逗号(,)分隔。
        /// </summary>
        [Browsable(false)]
        [Description("获取已选择行的ID列表,以逗号(,)分开")]
        public string SelectedIDList
        {
            get
            {
                return String.Join(",", ConvertEx.ToStringArray(this.CheckedValue));
            }
        }
        #endregion

        /// <summary>
        /// 获取控件中列字段对象的集合。
        /// </summary>
        [Category("Default")]
        [Description("获取控件中列字段对象的集合。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        [Editor(typeof(System.Web.UI.Design.WebControls.DataControlFieldTypeEditor), typeof(UITypeEditor))]
        public virtual DataControlFieldExCollection Columns
        {
            get
            {
                if (this.fieldCollection == null)
                {
                    this.fieldCollection = new DataControlFieldExCollection();
                    this.fieldCollection.FieldsChanged += new EventHandler(this.OnFieldsChanged);
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.fieldCollection).TrackViewState();
                }
                return this.fieldCollection;
            }
        }
        /// <summary>
        /// 获取或设置用作控件的列标题的列的名称。提供此属性的目的是使辅助技术设备的用户更易于访问控件。
        /// </summary>
        [Category("Accessibility")]
        [Description("获取或设置用作控件的列标题的列的名称。提供此属性的目的是使辅助技术设备的用户更易于访问控件。")]
        [TypeConverter(typeof(System.Web.UI.Design.DataColumnSelectionConverter))]
        public virtual string RowHeaderColumn
        {
            get { return this.ViewState["RowHeaderColumn"] as string; }
            set { this.ViewState["RowHeaderColumn"] = value; }
        }
        /// <summary>
        /// 获取或设置控件是否以易于访问的格式呈现其标题。提供此属性的目的是使辅助技术设备的用户更易于访问控件。
        /// </summary>
        [Category("Accessibility")]
        [Description("获取或设置控件是否以易于访问的格式呈现其标题。提供此属性的目的是使辅助技术设备的用户更易于访问控件。")]
        [DefaultValue(true)]
        public virtual bool UseAccessibleHeader
        {
            get
            {
                object obj = this.ViewState["UseAccessibleHeader"];
                return obj == null ? true : (bool)obj;
            }
            set
            {
                if (this.UseAccessibleHeader != value)
                {
                    this.ViewState["UseAccessibleHeader"] = value;
                    if (this.Initialized)
                        this.RequiresDataBinding = true;
                }
            }
        }

        IOrderedDictionary BoundFieldValues
        {
            get
            {
                if (this.boundFieldValues == null)
                {
                    this.boundFieldValues = new OrderedDictionary(this.Columns.Count);
                }
                return this.boundFieldValues;
            }
        }
        #endregion

        /// <summary>
        /// 基础结构。创建一个新的子表。
        /// </summary>
        /// <returns></returns>
        protected virtual Table CreateChildTable()
        {
            return new ChildTable(string.IsNullOrEmpty(this.ID) ? null : this.ClientID);
        }

        #region 重载。
        /// <summary>
        /// 基础结构。为控件创建默认样式。
        /// </summary>
        /// <returns></returns>
        protected override Style CreateControlStyle()
        {
            TableStyle style = new TableStyle();
            style.GridLines = GridLines.Both;
            style.CellSpacing = 0;
            return style;
        }
        /// <summary>
        /// 基础结构。将 Web 服务器控件内容呈现给客户端浏览器。
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.CssClass))
                this.CssClass = "DataGrid";

            if (string.IsNullOrEmpty(this.HeaderStyle.CssClass))
                this.HeaderStyle.CssClass = "DataGridHeader";

            if (string.IsNullOrEmpty(this.RowStyle.CssClass))
                this.RowStyle.CssClass = "DataGridItem";

            if (string.IsNullOrEmpty(this.AlternatingRowStyle.CssClass))
                this.AlternatingRowStyle.CssClass = "DataGridAlter";

            if (string.IsNullOrEmpty(this.FooterStyle.CssClass))
                this.FooterStyle.CssClass = "DataGridFooter";

            this.Render(writer, !this.DesignMode);
        }
        /// <summary>
        /// 基础结构。已重载。 创建用于呈现控件层次结构。
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataBinding"></param>
        /// <returns></returns>
        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            PagedDataSourceEx pagedDataSource = null;

            #region 获取数据源
            if (dataBinding)
            {
                bool allowPaging = this.AllowPaging;
                DataSourceViewEx data = this.GetData();
                DataSourceSelectArgumentsEx selectArguments = this.SelectArguments;
                if (data == null)
                    throw new HttpException("DataBoundControlEx_NullView");
                if ((allowPaging && !data.CanPage) && ((dataSource != null) && !(dataSource is ICollection)))
                {
                    selectArguments.StartRowIndex = this.PageSize * this.PageIndex;
                    selectArguments.MaximumRows = this.PageSize;
                    data.Select(selectArguments, new DataSourceViewSelectCallback(this.SelectCallback));
                }

                if (allowPaging && data.CanPage)
                {
                    if (data.CanRetrieveTotalRowCount)
                        pagedDataSource = this.CreateServerPagedDataSource(selectArguments.TotalRowCount);
                    else
                    {
                        ICollection collection = dataSource as ICollection;
                        if (collection == null)
                            throw new HttpException("DataBoundControlEx_NeedICollectionOrTotalRowCount");
                        pagedDataSource = this.CreateServerPagedDataSource(this.PageIndex * this.PageSize + collection.Count);
                    }
                }
                else
                    pagedDataSource = this.CreatePagedDataSource();
            }
            else
                pagedDataSource = this.CreatePagedDataSource();
            #endregion

            ICollection dataCollection = dataSource as ICollection;
            if (!dataBinding && dataSource != null && dataCollection == null)
                throw new HttpException("DataControls_DataSourceMustBeCollectionWhenNotDataBinding");

            pagedDataSource.DataSource = dataSource;
            if (pagedDataSource.DataSourceCount == 0)
                this.PageCount = 0;
            else
                this.PageCount = pagedDataSource.PageCount;
            if (pagedDataSource.IsPagingEnabled)
            {
                int pageCount = pagedDataSource.PageCount;
                if (pagedDataSource.CurrentPageIndex >= pageCount)
                    pagedDataSource.CurrentPageIndex = this.PageIndex = (pageCount - 1) > 0 ? pageCount - 1 : 0;
            }

            #region 获取字段
            ICollection dataColumns = this.CreateColumns(dataBinding ? pagedDataSource : null, dataBinding);
            int colCount = (dataColumns != null) ? dataColumns.Count : 0;
            DataControlFieldEx[] colsArray = new DataControlFieldEx[colCount];
            if (colCount > 0)
            {
                dataColumns.CopyTo(colsArray, 0);
                for (int i = 0; i < colsArray.Length; i++)
                {
                    colsArray[i].Initialize(this.AllowSorting, this);
                    if (this.DetermineRenderClientScript())
                        colsArray[i].ValidateSupportsCallback();
                }
            }
            #endregion

            #region 绘制数据。
            int rowIndex = 0, dataSourceIndex = pagedDataSource.FirstIndexInPage;
            bool isPagingEnabled = pagedDataSource.IsPagingEnabled;
            int capacity = isPagingEnabled ? pagedDataSource.PageSize : pagedDataSource.DataSourceCount;
            this.rowsArray = new ArrayList(capacity);
            this.rowsCollection = null;

            Table child = this.CreateChildTable();
            this.Controls.Add(child);
            TableRowCollection rows = child.Rows;
            if (colCount > 0)
            {
                this.headerRow = this.CreateRow(-1, -1, DataGridViewRowType.Header, DataGridViewRowState.Normal, dataBinding, null, colsArray, rows, null);
                if (!this.ShowHeader)
                    this.headerRow.Visible = false;

                if (dataSource != null)
                {
                    IEnumerator enumerator = pagedDataSource.GetEnumerator();
                    DataGridViewRow row = null;
                    while (enumerator.MoveNext())
                    {
                        object container = enumerator.Current;
                        DataGridViewRowState normal = DataGridViewRowState.Normal;
                        if ((rowIndex % 2) != 0)
                            normal |= DataGridViewRowState.AlterNate;
                        row = this.CreateRow(rowIndex, dataSourceIndex, DataGridViewRowType.DataRow, normal, dataBinding, container, colsArray, rows, null);
                        this.rowsArray.Add(row);
                        dataSourceIndex++;
                        rowIndex++;
                    }

                    if (dataCollection.Count > 0)
                    {
                        this.footerRow = this.CreateRow(-1, -1, DataGridViewRowType.Footer, DataGridViewRowState.Normal, dataBinding, null, colsArray, rows, pagedDataSource);
                        if (!this.ShowFooter)
                            this.footerRow.Visible = false;
                    }
                }
            }
        
            #endregion              
                       
            return rowIndex;
        }
        /// <summary>
        /// 创建 <see cref="DataSourceSelectArgumentsEx"/> 对象，该对象包含传递到数据源进行处理的参数。
        /// </summary>
        /// <returns></returns>
        protected override DataSourceSelectArgumentsEx CreateDataSourceSelectArguments()
        {
            DataSourceSelectArgumentsEx arguments = new DataSourceSelectArgumentsEx();
            DataSourceViewEx data = this.GetData();
            string sortExpressionInternal = this.SortExpression;
            if ((this.SortDirection == SortDirection.Descending) && !string.IsNullOrEmpty(sortExpressionInternal))
                sortExpressionInternal += " DESC ";
            arguments.SortExpression = sortExpressionInternal;
            if (this.AllowPaging && data.CanPage)
            {
                if (data.CanRetrieveTotalRowCount)
                {
                    arguments.RetrieveTotalRowCount = true;
                    arguments.MaximumRows = this.PageSize;
                }
                else
                    arguments.MaximumRows = -1;
                arguments.StartRowIndex = this.PageSize * this.PageIndex;
            }
            return arguments;
        }
        /// <summary>
        /// 加载以前保存的控件的视图状态。
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;
                if (objArray != null)
                {
                    base.LoadViewState(objArray[0]);
                    if (objArray[1] != null)
                        ((IStateManager)this.Columns).LoadViewState(objArray[1]);
                    
                    if (objArray[2] != null)
                        ((IStateManager)this.HeaderStyle).LoadViewState(objArray[2]);
                    if (objArray[3] != null)
                        ((IStateManager)this.FooterStyle).LoadViewState(objArray[3]);
                    if (objArray[4] != null)
                        ((IStateManager)this.RowStyle).LoadViewState(objArray[4]);
                    if (objArray[5] != null)
                        ((IStateManager)this.AlternatingRowStyle).LoadViewState(objArray[5]);
                    if (objArray[6] != null)
                        ((IStateManager)this.PagerSettings).LoadViewState(objArray[6]);
                    if (objArray[7] != null)
                        OrderedDictionaryStateHelper.LoadViewState((OrderedDictionary)this.BoundFieldValues, (ArrayList)objArray[7]);
                    if (objArray[8] != null)
                        ((IStateManager)this.ControlStyle).LoadViewState(objArray[8]);
                }
            }
            else
                base.LoadViewState(null);
        }
        /// <summary>
        /// 基础结构。加载以前保存的控件的视图状态。 
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            return new object[]{
                                   base.SaveViewState(),
                                   this.fieldCollection != null ? ((IStateManager)this.fieldCollection).SaveViewState():null,
                                   this.headerStyle != null ? ((IStateManager)this.headerStyle).SaveViewState():null,
                                   this.footerStyle != null ? ((IStateManager)this.footerStyle).SaveViewState():null,
                                   this.rowStyle != null ? ((IStateManager)this.rowStyle).SaveViewState():null,
                                   this.alternatingRowStyle != null ? ((IStateManager)this.alternatingRowStyle).SaveViewState():null,
                                   this.pagerSettings != null ? ((IStateManager)this.pagerSettings).SaveViewState():null,
                                   this.boundFieldValues != null ? OrderedDictionaryStateHelper.SaveViewState(this.boundFieldValues):null,
                                   this.ControlStyleCreated ? ((IStateManager)this.ControlStyle).SaveViewState():null
                                };
        }
        /// <summary>
        /// 基础结构。跟踪控件的视图状态更改
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (this.fieldCollection != null)
                ((IStateManager)this.fieldCollection).TrackViewState();
            if (this.headerStyle != null)
                ((IStateManager)this.headerStyle).TrackViewState();
            if (this.footerStyle != null)
                ((IStateManager)this.footerStyle).TrackViewState();
            if (this.rowStyle != null)
                ((IStateManager)this.rowStyle).TrackViewState();
            if (this.alternatingRowStyle != null)
                ((IStateManager)this.alternatingRowStyle).TrackViewState();
            if (this.pagerSettings != null)
                ((IStateManager)this.pagerSettings).TrackViewState();
            if (this.ControlStyleCreated)
                ((IStateManager)this.ControlStyle).TrackViewState();
        }
        #endregion

        #region IPostBackContainer 成员
        /// <summary>
        /// 为指定的按钮控件返回回发脚本所需的选项。
        /// </summary>
        /// <param name="buttonControl">包含生成 buttonControl 的回发脚本所需的选项。</param>
        /// <returns></returns>
        public PostBackOptions GetPostBackOptions(IButtonControl buttonControl)
        {
            if (buttonControl == null)
                throw new ArgumentNullException("buttonControl");
            if (buttonControl.CausesValidation)
                throw new InvalidOperationException("CannotUseParentPostBackWhenValidating");
            PostBackOptions options = new PostBackOptions(this, buttonControl.CommandName + "$" + buttonControl.CommandArgument);
            options.RequiresJavaScriptProtocol = true;
            return options;
        }

        #endregion

        #region IPostBackEventHandler 成员
        /// <summary>
        ///  控件回发到服务器时引发此控件的合适的事件。
        /// </summary>
        /// <param name="eventArgument">事件参数。</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            this.ValidateEvent(this.UniqueID, eventArgument);
            int index = eventArgument.IndexOf('$');
            if (index >= 0)
            {
                CommandEventArgs originalArgs = new CommandEventArgs(eventArgument.Substring(0, index), eventArgument.Substring(index + 1));
                DataGridViewCommandEventArgs e = new DataGridViewCommandEventArgs(null, this, originalArgs);
                this.HandleEvent(e, false, string.Empty);
            }
            else
            {
                this.InvokeBuildDataSource();
            }
        }

        #endregion

        #region ICallbackContainer 成员
        /// <summary>
        /// 创建一个脚本，以便启动针对 Web 服务器的客户端回调。
        /// </summary>
        /// <param name="buttonControl">启动回调请求的控件。</param>
        /// <param name="argument">用于生成回调脚本的参数。</param>
        /// <returns>在客户端运行时，将启动针对 Web 服务器回调的脚本。</returns>
        public virtual string GetCallbackScript(IButtonControl buttonControl, string argument)
        {
            if (!this.DetermineRenderClientScript())
                return null;
            if (string.IsNullOrEmpty(argument) && (buttonControl.CommandName == "Sort"))
                argument = this.BuildCallbackArgument(buttonControl.CommandArgument, this.SortDirection);
            if (this.Page != null)
                this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, argument);
            return "javascript:__dgv" + this.ClientID + ".callback" + "(" + argument + ");return false;";
        }

        #endregion

        #region ICallbackEventHandler 成员
        /// <summary>
        /// 返回以控件为目标的回调事件的结果。
        /// </summary>
        /// <returns>回调的结果。</returns>
        public virtual string GetCallbackResult()
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter textWriter = new HtmlTextWriter(writer);
            IStateFormatter stateFormatter = this.StateFormatter;
            this.RenderTableContents(textWriter);
            textWriter.Flush();
            textWriter.Close();

            string strSortExpression = stateFormatter.Serialize(this.SortExpression);
            return Convert.ToString(this.PageIndex, CultureInfo.InvariantCulture) + "|" +
                Convert.ToString((int)this.SortDirection, CultureInfo.InvariantCulture) + "|" + strSortExpression + "|" + writer.ToString();
        }
        /// <summary>
        /// 处理以控件为目标的回调事件。
        /// </summary>
        /// <param name="eventArgument">示要传递到事件处理程序的事件参数。</param>
        public virtual void RaiseCallbackEvent(string eventArgument)
        {
            string[] strArray = eventArgument.Split(new char[] { '|' });
            IStateFormatter stateFormatter = this.StateFormatter;
            this.ValidateEvent(this.UniqueID, "\"" + strArray[0] + "|" + strArray[1] + "|" + strArray[2] + "|" + strArray[3] + "\"");
            int num = int.Parse(strArray[0], CultureInfo.InvariantCulture);
            string serializedSortExpression = strArray[2];

            if (num == this.PageIndex)
            {
                SortDirection ascending = SortDirection.Ascending;
                string strSortExpression = (string)stateFormatter.Deserialize(serializedSortExpression);
                if ((strSortExpression == this.SortExpression) && (this.SortDirection == SortDirection.Ascending))
                    ascending = SortDirection.Descending;
                this.SortExpression = strSortExpression;
                this.SortDirection = ascending;
            }
            else
                this.PageIndex = num;
            this.DataBind();
        }

        #endregion

        /// <summary>
        /// 创建行。
        /// </summary>
        /// <param name="rowIndex">要创建的行的索引。</param>
        /// <param name="dataSourceIndex">要绑定到行的数据源项的索引。</param>
        /// <param name="rowType">行类型。</param>
        /// <param name="rowState">行状态。</param>
        /// <returns></returns>
        protected virtual DataGridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataGridViewRowType rowType, DataGridViewRowState rowState)
        {
            DataGridViewRow row = new DataGridViewRow(rowIndex, dataSourceIndex, rowType, rowState);
            if (rowType == DataGridViewRowType.DataRow)
            {
                row.Attributes.Add("onmouseout", string.Format("this.className='{0}';",
                           rowState == DataGridViewRowState.AlterNate ? this.AlternatingRowStyle.CssClass : this.RowStyle.CssClass));
                row.Attributes.Add("onmouseover", string.Format("this.className='{0}';", this.MouseoverCssClass));
            }
            return row;
        }

        #region 辅助函数。
        DataGridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataGridViewRowType rowType, DataGridViewRowState rowState, bool dataBind,
            object dataItem, DataControlFieldEx[] fields, TableRowCollection rows, PagedDataSourceEx pagedDataSource)
        {
            DataGridViewRow row = this.CreateRow(rowIndex, dataSourceIndex, rowType, rowState);
            DataGridViewRowEventArgs e = new DataGridViewRowEventArgs(row);
            if (rowType != DataGridViewRowType.Footer)
                this.InitializeRow(row, fields);
            else
                this.InitializePager(row, fields.Length, pagedDataSource);
            if (dataBind)
                row.DataItem = dataItem;
            this.OnRowCreated(e);
            rows.Add(row);
            if (dataBind)
            {
                row.DataBind();
                this.OnRowDataBound(e);
                row.DataItem = null;
            }
            return row;
        }

        PagedDataSourceEx CreatePagedDataSource()
        {
            PagedDataSourceEx source = new PagedDataSourceEx();
            source.CurrentPageIndex = this.PageIndex;
            source.PageSize = this.PageSize;
            source.AllowPaging = this.AllowPaging;
            source.AllowCustomPaging = false;
            source.AllowServerPaging = false;
            source.VirtualCount = 0;
            return source;
        }

        PagedDataSourceEx CreateServerPagedDataSource(int totalRowCount)
        {
            PagedDataSourceEx source = new PagedDataSourceEx();
            source.CurrentPageIndex = this.PageIndex;
            source.PageSize = this.PageSize;
            source.AllowPaging = this.AllowPaging;
            source.AllowCustomPaging = false;
            source.AllowServerPaging = true;
            source.VirtualCount = totalRowCount;
            return source;
        }

        void SelectCallback(IEnumerable data)
        {
            throw new HttpException("DataBoundControlEx_DataSourceDoesntSupportPaging");
        }

        void OnFieldsChanged(object sender, EventArgs e)
        {
            if (this.Initialized)
                this.RequiresDataBinding = true;
        }

        bool DetermineRenderClientScript()
        {
            if (!this.renderClientScriptValid)
            {
                this.renderClientScript = false;
                if (this.EnableSortingAndPagingCallbacks && (this.Context != null) && (this.Page != null) && (this.Page.Request != null) &&
                    this.Page.Request.Browser.SupportsCallback)
                {
                    HttpBrowserCapabilities browser = this.Page.Request.Browser;
                    bool bEMajorVer = browser.EcmaScriptVersion.Major > 0;
                    bool bWMajorVer = browser.W3CDomVersion.Major > 0;
                    bool flag = !StringUtil.EqualsIgnoreCase(browser["tagwriter"], typeof(Html32TextWriter).FullName);
                    this.renderClientScript = bEMajorVer && bWMajorVer && flag;
                }
                this.renderClientScriptValid = true;
            }
            return this.renderClientScript;
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 客户端回调事件处理。
        /// </summary>
        /// <param name="e"></param>
        /// <param name="causesValidation"></param>
        /// <param name="validationGroup"></param>
        protected virtual bool HandleEvent(EventArgs e, bool causesValidation, string validationGroup)
        {
            bool result = false;
            if (causesValidation)
                this.Page.Validate(validationGroup);
            DataGridViewCommandEventArgs args = e as DataGridViewCommandEventArgs;
            if (args != null)
            {
                this.OnRowCommand(args);
                result = true;
                string commandName = args.CommandName;
                if (StringUtil.EqualsIgnoreCase(commandName, "Page"))
                {
                    string commandArgument = args.CommandArgument as string;
                    int pageIndex = this.PageIndex;
                    if (StringUtil.EqualsIgnoreCase(commandArgument, "Next"))
                        pageIndex++;
                    else if (StringUtil.EqualsIgnoreCase(commandArgument, "Prev"))
                        pageIndex--;
                    else if (StringUtil.EqualsIgnoreCase(commandArgument, "First"))
                        pageIndex = 0;
                    else if (StringUtil.EqualsIgnoreCase(commandArgument, "Last"))
                        pageIndex = this.PageCount - 1;
                    else
                        pageIndex = Convert.ToInt32(commandArgument, CultureInfo.InvariantCulture) - 1;

                    this.HandlePage(pageIndex);
                    return result;
                }
                else if (StringUtil.EqualsIgnoreCase(commandName, "Sort"))
                {
                    this.HandleSort(args.CommandArgument as string);
                    return result;
                }
                else if (StringUtil.EqualsIgnoreCase(commandName, "RowSelectingEvent"))
                {
                    //选中数据。
                    try
                    {
                        int rowIndex = Convert.ToInt32(args.CommandArgument);
                        if (rowIndex > -1)
                        {
                            IEnumerable enumerable = DataSourceHelper.GetResolvedDataSource(this.DataSource, this.DataMember);
                            if (enumerable != null)
                            {
                                IEnumerator enumerator = enumerable.GetEnumerator();
                                int index = 0;
                                object data = null;
                                while (enumerator.MoveNext())
                                {
                                    if (index == rowIndex)
                                    {
                                        data = enumerator.Current;
                                        break;
                                    }
                                    index++;
                                }
                                this.OnRowSelecting(data);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            return result;
        }
        /// <summary>
        /// 单击 <see cref="DataGridView"/> 控件中的按钮时发生。
        /// </summary>
        [Category("Action")]
        [Description("单击控件中的按钮时发生。")]
        public event DataGridViewCommandEventHandler RowCommand;
        /// <summary>
        /// 触发<see cref="RowCommand"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRowCommand(DataGridViewCommandEventArgs e)
        {
            DataGridViewCommandEventHandler handler = this.RowCommand;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 创建行时发生。
        /// </summary>
        [Category("Action")]
        [Description("创建行时发生。")]
        public event DataGridViewRowEventHandler RowCreated;
        /// <summary>
        /// 触发<see cref="RowCreated"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRowCreated(DataGridViewRowEventArgs e)
        {
            DataGridViewRowEventHandler handler = this.RowCreated;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 将数据行绑定到数据时发生。
        /// </summary>
        [Category("Action")]
        [Description("将数据行绑定到数据时发生。")]
        public event DataGridViewRowEventHandler RowDataBound;
        /// <summary>
        /// 触发<see cref="RowDataBound"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRowDataBound(DataGridViewRowEventArgs e)
        {
            DataGridViewRowEventHandler handler = this.RowDataBound;
            if (handler != null)
            {
                if (e != null && (e.Row.RowType == DataGridViewRowType.Header))
                {
                    e.Row.Attributes["align"] = "center";
                    e.Row.Attributes["valign"] = "middle";
                }
                handler(this, e);
            }
        }
       
        #endregion

        /// <summary>
        /// 触发获取数据源事件的方法。
        /// </summary>
        public void InvokeBuildDataSource()
        {
            this.OnBuildDataSource(EventArgs.Empty);
            this.DataBind();
        }

        /// <summary>
        /// 重载绑定。
        /// </summary>
        public sealed override void DataBind()
        {
            base.DataBind();
        }

        #region 绘制HTML。
        /// <summary>
        /// 绘制表内容。
        /// </summary>
        /// <param name="writer"></param>
        protected void RenderTableContents(HtmlTextWriter writer)
        {
            this.Render(writer, false);
        }

        /// <summary>
        /// 绘制Web服务器控件，输出页面。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="renderPanel"></param>
        protected void Render(HtmlTextWriter writer, bool renderPanel)
        {
            if (this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);

            this.PrepareControlHierarchy();
            if (renderPanel)
            {
                string clientID = this.ClientID;
                if (this.DetermineRenderClientScript())
                {
                    if (clientID == null)
                        throw new HttpException("DataGridView_MustBeParented");

                    StringBuilder builder = new StringBuilder("__dgv", clientID.Length + 9);
                    builder.Append(clientID);
                    builder.Append("__div");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, builder.ToString(), true);
                }
            }
            this.RenderContents(writer);
        }
        /// <summary>
        /// 基础结构。建立控件层次结构。
        /// </summary>
        protected virtual void PrepareControlHierarchy()
        {
            if (this.Controls.Count != 0)
            {
                bool controlStyleCreated = this.ControlStyleCreated;
                Table table = (Table)this.Controls[0];
                table.CopyBaseAttributes(this);
                if (controlStyleCreated && !this.ControlStyle.IsEmpty)
                    table.ApplyStyle(this.ControlStyle);
                else
                {
                    table.GridLines = GridLines.Both;
                    table.CellSpacing = 0;
                }
                table.Caption = this.Caption;
                table.CaptionAlign = this.CaptionAlign;
                TableRowCollection rows = table.Rows;

                int cellSum = 0;
                bool bCellSum = true;

                #region Rows
                foreach (DataGridViewRow row in rows)
                {
                    switch (row.RowType)
                    {
                        case DataGridViewRowType.Header:
                            if (this.ShowHeader && (this.headerStyle != null))
                                row.MergeStyle(this.headerStyle);
                            break;

                        case DataGridViewRowType.Footer:
                            if (this.ShowFooter && (this.footerStyle != null))
                                row.MergeStyle(this.footerStyle);
                            break;

                        case DataGridViewRowType.DataRow:
                            if (((row.RowState & DataGridViewRowState.AlterNate) != DataGridViewRowState.Normal) && (this.alternatingRowStyle != null))
                                row.MergeStyle(this.alternatingRowStyle);
                            else if (this.rowStyle != null)
                                row.MergeStyle(this.rowStyle);
                            break;

                        default:
                            break;
                    }

                    #region Cells
                    foreach (TableCell cell in row.Cells)
                    {
                        DataControlFieldCellEx cellEx = cell as DataControlFieldCellEx;
                        if (cellEx != null)
                        {
                            DataControlFieldEx containingField = cellEx.ContainingField;
                            if (containingField != null)
                            {
                                if (!containingField.Visible)
                                {
                                    cell.Visible = false;
                                    continue;
                                }
                                if ((row.RowType == DataGridViewRowType.DataRow) && bCellSum)
                                    cellSum++;
                                Style styleInternal = null;
                                switch (row.RowType)
                                {
                                    case DataGridViewRowType.Header:
                                        styleInternal = containingField.HeaderStyle;
                                        break;
                                    case DataGridViewRowType.Footer:
                                        styleInternal = containingField.FooterStyle;
                                        break;
                                    default:
                                        styleInternal = containingField.ItemStyle;
                                        break;
                                }
                                if (styleInternal != null)
                                    cell.MergeStyle(styleInternal);

                                if (row.RowType == DataGridViewRowType.DataRow)
                                {
                                    foreach (Control control in cell.Controls)
                                    {
                                        WebControl webControl = control as WebControl;
                                        if (webControl != null)
                                        {
                                            Style controlStyleInternal = containingField.ControlStyle;
                                            if ((controlStyleInternal != null) && !controlStyleInternal.IsEmpty)
                                                webControl.ControlStyle.CopyFrom(controlStyleInternal);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    if (row.RowType == DataGridViewRowType.DataRow)
                        bCellSum = false;
                }
                #endregion
            }
        }
        #endregion

        /// <summary>
        /// 创建用来构建控件层次结构的列字段集。
        /// </summary>
        /// <param name="dataSource">表示数据源。</param>
        /// <param name="useDataSource">true 表示使用 dataSource 参数指定的数据源；否则为 false。</param>
        /// <returns></returns>
        protected virtual ICollection CreateColumns(PagedDataSourceEx dataSource, bool useDataSource)
        {
            ArrayList list = new ArrayList();
            foreach (DataControlFieldEx field in this.Columns)
            {
                list.Add(field);
            }
            return list;
        }
        /// <summary>
        /// 检索在指定行中声明的每个字段的值，并将它们存储在指定的 <see cref="IOrderedDictionary"/> 对象中。
        /// </summary>
        /// <param name="fieldValues">用来存储字段值的<see cref="IOrderedDictionary"/> 。</param>
        /// <param name="row">将从其中检索字段值的<see cref="DataGridViewRow"/>。</param>
        /// <param name="includeReadOnlyFields">true 表示包含只读字段；否则为 false。</param>
        /// <param name="includePrimaryKey">true 表示包含主键字段；否则为 false。</param>
        protected virtual void ExtractRowValues(IOrderedDictionary fieldValues, DataGridViewRow row, bool includeReadOnlyFields, bool includePrimaryKey)
        {
            if (fieldValues != null)
            {
                ICollection collection = this.CreateColumns(null, false);
                int count = collection.Count;
                object[] array = new object[count];
                collection.CopyTo(array, 0);
                for (int i = 0; (i < count) && (i < row.Cells.Count); i++)
                {
                    if (((DataControlFieldEx)array[i]).Visible)
                    {
                        OrderedDictionary dictionary = new OrderedDictionary();
                        ((DataControlFieldEx)array[i]).ExtractValuesFromCell(dictionary, row.Cells[i] as DataControlFieldCellEx, row.RowState, includeReadOnlyFields);
                        foreach (DictionaryEntry entry in dictionary)
                        {
                           // if (includePrimaryKey)
                                fieldValues[entry.Key] = entry.Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化行。
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fields"></param>
        protected virtual void InitializeRow(DataGridViewRow row, DataControlFieldEx[] fields)
        {
            DataGridViewRowType rowType = row.RowType;
            DataGridViewRowState rowState = row.RowState;
            int rowIndex = row.RowIndex;

            TableCellCollection cells = row.Cells;
            string rowHeaderColumn = this.RowHeaderColumn;
            bool useAccessibleHeader = false;
            if (rowType == DataGridViewRowType.Header)
                useAccessibleHeader = this.UseAccessibleHeader;

            for (int i = 0; i < fields.Length; i++)
            {
                DataControlFieldCellEx cell = null;
                DataControlCellType header = DataControlCellType.DataCell;
                if ((rowType == DataGridViewRowType.Header) && useAccessibleHeader)
                {
                    cell = new DataControlFieldHeaderCellEx(fields[i]);
                    ((DataControlFieldHeaderCellEx)cell).Scope = TableHeaderScope.Column;
                    ((DataControlFieldHeaderCellEx)cell).AbbreviatedText = fields[i].AccessibleHeaderText;
                }
                else
                {
                    BoundFieldEx field = fields[i] as BoundFieldEx;
                    if (!string.IsNullOrEmpty(rowHeaderColumn) && (field != null) && (field.DataField == rowHeaderColumn))
                    {
                        cell = new DataControlFieldHeaderCellEx(fields[i]);
                        ((DataControlFieldHeaderCellEx)cell).Scope = TableHeaderScope.Row;
                    }
                    else
                        cell = new DataControlFieldCellEx(fields[i]);
                }

                switch (rowType)
                {
                    case DataGridViewRowType.Header:
                        header = DataControlCellType.Header;
                        break;
                    case DataGridViewRowType.Footer:
                        header = DataControlCellType.Footer;
                        break;
                    default:
                        header = DataControlCellType.DataCell;
                        break;
                }

                fields[i].InitializeCell(cell,header, rowState, rowIndex);
                cells.Add(cell);
            }
        }
        /// <summary>
        /// 确定指定的数据类型是否能绑定到控件中的列。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsBindableType(Type type)
        {
            return DataBoundControlExHelper.IsBindableType(type);
        }
    }
}