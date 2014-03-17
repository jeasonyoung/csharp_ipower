//================================================================================
//  FileName: ListControlEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/22
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
using System.Text;
using System.Data;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Globalization;

using iPower.Utility;
using iPower.Web.Utility;
namespace iPower.Web.UI
{
    /// <summary>
    /// 用作定义所有列表类型控件通用的属性、方法和事件的抽象基类。
    /// </summary>
    [ParseChildren(true, "Items")]
    [ControlValueProperty("SelectValue")]
    [DataBindingHandler(typeof(System.Web.UI.Design.WebControls.ListControlDataBindingHandler))]
    [Designer(typeof(System.Web.UI.Design.WebControls.ListControlDesigner))]
    public abstract class ListControlEx : DataBoundControlEx, IEditableTextControl, ITextControl
    {
        #region 成员变量，构造函数。
        ListItemCollection items;
        int cachedSelectedIndex;
        string cachedSelectedValue;
        //bool stateLoaded;
        ArrayList cachedSelectedIndices;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ListControlEx()
        {
            this.cachedSelectedIndex = -1;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置是否在绑定数据之前清除列表项。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置是否在绑定数据之前清除列表项。")]
        [Themeable(false)]
        public virtual bool AppendDataBoundItems
        {
            get
            {
                object obj = this.ViewState["AppendDataBoundItems"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                this.ViewState["AppendDataBoundItems"] = value;
                if (this.Initialized)
                    this.RequiresDataBinding = true;
            }
        }
        /// <summary>
        /// 获取或设置当用户更改列表中的选定内容时是否自动产生向服务器的回发。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置当用户更改列表中的选定内容时是否自动产生向服务器的回发。")]
        [Themeable(false)]
        public virtual bool AutoPostBack
        {
            get
            {
                object obj = this.ViewState["AutoPostBack"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                this.ViewState["AutoPostBack"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否执行验证。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置是否执行验证。")]
        [Themeable(false)]
        public virtual bool CausesValidation
        {
            get
            {
                object obj = this.ViewState["CausesValidation"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                this.ViewState["CausesValidation"] = value;
            }
        }
        /// <summary>
        /// 获取或设置为列表项提供文本内容的数据源字段。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置为列表项提供文本内容的数据源字段。")]
        [Themeable(false)]
        public virtual string DataTextField
        {
            get
            {
                object obj = this.ViewState["DataTextField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["DataTextField"] = value;
                if (this.Initialized)
                    this.RequiresDataBinding = true;
            }
        }
        /// <summary>
        /// 获取或设置格式化字符串，该字符串用来控制如何显示绑定到列表控件的数据。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置格式化字符串，该字符串用来控制如何显示绑定到列表控件的数据。")]
        [Themeable(false)]
        public virtual string DataTextFormatString
        {
            get
            {
                object obj = this.ViewState["DataTextFormatString"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["DataTextFormatString"] = value;
                if (this.Initialized)
                    this.RequiresDataBinding = true;
            }
        }

        /// <summary>
        /// 获取或设置为各列表项提供值的数据源字段。
        /// </summary>
        [Category("Data")]
        [Description(" 获取或设置为各列表项提供值的数据源字段。")]
        [Themeable(false)]
        public virtual string DataValueField
        {
            get
            {
                object obj = this.ViewState["DataValueField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["DataValueField"] = value;
                if (this.Initialized)
                    this.RequiresDataBinding = true;
            }
        }

        /// <summary>
        /// 获取或设置是否以树状形式列表数据。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置是否以树状形式列表数据。")]
        public bool ShowTreeView
        {
            get
            {
                object o = this.ViewState["ShowTreeView"];
                return (o != null) && (bool)o;
            }
            set
            {
                this.ViewState["ShowTreeView"] = value;
            }
        }

        /// <summary>
        /// 获取或设置父字段。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置父字段。")]
        public string ParentDataValueField
        {
            get
            {
                string s = (string)this.ViewState["ParentDataValueField"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["ParentDataValueField"] = value;
            }
        }

        internal virtual bool IsMultiSelectInternal
        {
            get { return false; }
        }
        /// <summary>
        /// 获取列表控件项的集合。
        /// </summary>
        [Category("Default")]
        [Description("获取列表控件项的集合。")]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        [Editor(typeof(System.Web.UI.Design.WebControls.ListItemsCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual ListItemCollection Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new ListItemCollection();
                    if (this.IsTrackingViewState)
                        ((IStateManager)this.items).TrackViewState();
                }
                return this.items;
            }
        }
        /// <summary>
        /// 获取或设置列表中选定项的最低序号索引。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置列表中选定项的最低序号索引。")]
        [Browsable(false)]
        [Themeable(false)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectedIndex
        {
            get
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                        return i;
                }
                return -1;
            }
            set
            {
                if (value < -1)
                {
                    if (this.Items.Count != 0)
                        throw new ArgumentOutOfRangeException("value");
                    value = -1;
                }
                if ((this.Items.Count != 0) && (value < this.Items.Count))
                {
                    this.ClearSelection();
                    if (value >= 0)
                    {
                        this.Items[value].Selected = true;
                    }
                }
                this.cachedSelectedIndex = value;
            }
        }
        internal virtual ArrayList SelectedIndicsInternal
        {
            get
            {
                this.cachedSelectedIndices = new ArrayList(3);
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                        this.cachedSelectedIndices.Add(i);
                }
                return this.cachedSelectedIndices;
            }
        }
        /// <summary>
        /// 获取列表控件中索引最小的选定项。
        /// </summary>
        [Category("Behavior")]
        [Description("获取列表控件中索引最小的选定项。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public virtual ListItem SelectedItem
        {
            get
            {
                int selectedIndex = this.SelectedIndex;
                if(selectedIndex >= 0)
                    return this.Items[selectedIndex];
                return null;
            }
        }
        /// <summary>
        /// 获取列表控件中选定项的值，或选择列表控件中包含指定值的项。
        /// </summary>
        [Category("Behavior")]
        [Description(" 获取列表控件中选定项的值，或选择列表控件中包含指定值的项。")]
        [Themeable(false)]
        [Bindable(true, BindingDirection.TwoWay)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SelectedValue
        {
            get
            {
                int selectedIndex = this.SelectedIndex;
                if (selectedIndex >= 0)
                    return this.Items[selectedIndex].Value;
                return string.Empty;
            }
            set
            {
                if (this.Items.Count != 0)
                {
                    if ((value == null) || (this.DesignMode && (value.Length == 0)))
                    {
                        this.ClearSelection();
                        return;
                    }
                    ListItem item = this.Items.FindByValue(value);
                    if (item != null)
                    {
                        this.ClearSelection();
                        item.Selected = true;
                    }
                }
                this.cachedSelectedValue = value;
            }
        }
        /// <summary>
        /// 获取或设置控件组对该组控件进行验证。 
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置控件组对该组控件进行验证。")]
        [Themeable(false)]       
        public virtual string ValidationGroup
        {
            get
            {
                object obj = this.ViewState["ValidationGroup"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["ValidationGroup"] = value;
            }
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 当列表控件的选定项在信息发往服务器之间变化时发生。
        /// </summary>
        [Category("Action")]
        [Description("当列表控件的选定项在信息发往服务器之间变化时发生。")]
        public event EventHandler SelectedIndexChanged;
        /// <summary>
        /// 触发<see cref="SelectedIndexChanged"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedIndexChanged;
            if (handler != null)
                handler(this, e);
            this.OnTextChanged(e);
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Select;
            }
        }
        /// <summary>
        /// 应用 HTML 属性和样式，以呈现到指定的 <see cref="HtmlTextWriter"/> 对象。
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);

            if (this.IsMultiSelectInternal)
                writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "multiple");

            if (this.AutoPostBack && (this.Page != null))
            {
                string str = null;
                if (this.HasAttributes)
                {
                    str = this.Attributes["onchange"];
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = Util.EnsureEndWithSemiColon(str);
                        this.Attributes.Remove("onchange");
                    }
                }

                PostBackOptions options = new PostBackOptions(this, string.Empty);
                if (this.CausesValidation)
                {
                    options.PerformValidation = true;
                    options.ValidationGroup = this.ValidationGroup;
                }

                if (this.Page.Form != null)
                    options.AutoPostBack = true;

                str = Util.MergeScript(str, this.Page.ClientScript.GetPostBackEventReference(options, true));
                writer.AddAttribute(HtmlTextWriterAttribute.Onchange, str);
            }
            if (this.Enabled && !this.IsEnabled)
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            base.AddAttributesToRender(writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                Triplet triplet = (Triplet)savedState;
                base.LoadViewState(triplet.First);
                ((IStateManager)this.Items).LoadViewState(triplet.Second);
                ArrayList third = triplet.Third as ArrayList;
                if (third != null)
                    this.SelectInternal(third);
            }
            else
                base.LoadViewState(savedState);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            object x = base.SaveViewState();
            object y = ((IStateManager)this.Items).SaveViewState();
            object z = this.SelectedIndicsInternal;

            if (x == null && y == null && z == null)
                return null;
            return new Triplet(x, y, z);
         }
        /// <summary>
        /// 
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            ((IStateManager)this.Items).TrackViewState();
        }
        /// <summary>
        /// 基础结构。引发 <see cref="DataBinding"/> 事件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            IEnumerable data = this.GetData().ExecuteSelect(DataSourceSelectArgumentsEx.Empty);
            this.PerformDataBinding(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.Page != null && this.IsEnabled)
            {
                if (this.AutoPostBack)
                {
                    this.Page.RegisterRequiresPostBack(this);
                    if (this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0)
                        this.Page.ClientScript.RegisterForEventValidation(this.UniqueID);
                }
            }
        }
        /// <summary>
        /// 将指定的数据源绑定到从 <see cref="ListControlEx"/> 类派生的控件。
        /// </summary>
        /// <param name="data"></param>
        protected override void PerformDataBinding(IEnumerable data)
        {
            base.PerformDataBinding(data);
            if (data != null)
            {
                if (!this.AppendDataBoundItems)
                    this.Items.Clear();
                if (this.ShowTreeView)
                {
                    DataTable dtSource = null;
                    if (data is DataTable)
                        dtSource = (DataTable)data;
                    else if (data is DataSet)
                        dtSource = ((DataSet)data).Tables[0];
                    this.BuildTreeView(string.Empty, string.Empty, dtSource);
                }
                else
                {
                    string[] dataTextFields = string.IsNullOrEmpty(this.DataTextField) ?  null : this.DataTextField.Split(',');
                    string dataTextFormatString = this.DataTextFormatString;
                    string dataValueField = this.DataValueField;

                    ICollection collection = data as ICollection;
                    if (collection != null)
                        this.Items.Capacity = collection.Count + this.Items.Count;

                    foreach (object obj in data)
                    {
                        ListItem item = new ListItem();
                        if (!string.IsNullOrEmpty(dataValueField) && dataTextFields != null)
                        {
                            item.Text = this.ConvertDataText(obj, dataTextFields, dataTextFormatString);
                            item.Value = DataBinder.GetPropertyValue(obj, dataValueField, null);
                        }
                        else
                        {
                            item.Text = string.IsNullOrEmpty(dataTextFormatString) ? obj.ToString() : string.Format(CultureInfo.CurrentCulture, dataTextFormatString, obj);
                            item.Value = obj.ToString();
                        }
                        this.Items.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(this.cachedSelectedValue))
                {
                    int selectedIndex = -1;
                    ListItem item = this.Items.FindByValue(this.cachedSelectedValue);
                    if (item != null)
                        selectedIndex = this.Items.IndexOf(item);
                    this.SelectedIndex = selectedIndex;
                    this.cachedSelectedValue = null;
                    this.cachedSelectedIndex = -1;
                }
                else if (this.cachedSelectedIndex != -1)
                {
                    this.SelectedIndex = this.cachedSelectedIndex;
                    this.cachedSelectedIndex = -1;
                }
            }
        }
        /// <summary>
        /// 从关联的数据源中检索数据。
        /// </summary>
        protected override void PerFormSelect()
        {
            this.OnDataBinding(EventArgs.Empty);
            this.RequiresDataBinding = false;
            this.MarkAsDataBound();
            this.OnDataBound(EventArgs.Empty);
        }
        /// <summary>
        /// 呈现 <see cref="ListControlEx"/> 控件中的项。
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            ListItemCollection items = this.Items;
            int count = items.Count;
            if (count > 0)
            {
                bool flag = false;
                for (int i = 0; i < count; i++)
                {
                    ListItem item = items[i];
                    if (item.Enabled)
                    {
                        writer.WriteBeginTag("option");
                        if (item.Selected)
                        {
                            if (flag)
                                this.VerifyMultSelect();
                            flag = true;
                            writer.WriteAttribute("selected", "selected");
                        }
                        writer.WriteAttribute("value", item.Value, true);
                        if (item.Attributes.Count > 0)
                            item.Attributes.Render(writer);
                        if (this.Page != null)
                            this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, item.Value);
                        writer.Write('>');
                        HttpUtility.HtmlEncode(item.Text, writer);
                        writer.WriteEndTag("option");
                        writer.WriteLine();
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 发送页面之后，设置<see cref="ListItem"/>  控件的<see cref="ListItem.Selected"/>  属性。
        /// </summary>
        /// <param name="selectedIndex"></param>
        protected void SetPostDataSelection(int selectedIndex)
        {
            if (this.Items.Count > 0 && selectedIndex < this.Items.Count)
            {
                this.ClearSelection();
                if (selectedIndex >= 0)
                    this.Items[selectedIndex].Selected = true;
            }
        }

        /// <summary>
        /// 验证多选。
        /// </summary>
        protected internal virtual void VerifyMultSelect()
        {
            if (!this.IsMultiSelectInternal)
                throw new HttpException("不容许多选！");
        }

        /// <summary>
        /// 构建树视图。
        /// </summary>
        /// <param name="strPIdValue"></param>
        /// <param name="strLeftPad"></param>
        /// <param name="dtSource"></param>
        protected virtual void BuildTreeView(string strPIdValue, string strLeftPad, DataTable dtSource)
        {
            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                string pFieldValueName = this.ParentDataValueField;
                if (dtSource.Columns.Contains(pFieldValueName))
                {
                    DataView dvView = dtSource.DefaultView;
                    Type pFieldValueNameType = dtSource.Columns[pFieldValueName].DataType;
                    if ((pFieldValueNameType == typeof(Int16)) || pFieldValueNameType == typeof(Int32) || pFieldValueNameType == typeof(Int64))
                    {
                        if (!string.IsNullOrEmpty(strPIdValue))
                            dvView.RowFilter = string.Format("{0}={1}", pFieldValueName, strPIdValue);
                        else
                            dvView.RowFilter = string.Format("{0}=0", pFieldValueName);
                    }
                    else
                        dvView.RowFilter = string.Format("isnull({0},'') = '{1}'", pFieldValueName, strPIdValue);

                    if (dvView.Count > 0)
                    {
                        string[] dataTextFields = this.DataTextField.Split(',');
                        string formatString = this.DataTextFormatString;
                        string strDataValueField = string.Empty;
                        foreach (DataRowView row in dvView)
                        {
                            strDataValueField =  Convert.ToString(row[this.DataValueField]);
                            ListItem item = new ListItem(string.Format("{0}{1}", strLeftPad, this.ConvertDataText(row, dataTextFields, formatString)),
                                                        strDataValueField);
                            this.Items.Add(item);
                            this.BuildTreeView(strDataValueField,
                                string.Format("{0}{1}", strLeftPad, HttpUtility.HtmlEncode("&nbsp;&nbsp;&nbsp;&nbsp;")),
                                dtSource.Copy());
                        }
                    }
                }
            }
        }

        string ConvertDataText(DataRowView row, string[] dataTextFields, string formatString)
        {
            string result = string.Empty;
            if (dataTextFields != null && dataTextFields.Length > 0)
            {
                if (dataTextFields.Length == 1)
                {
                    object obj = row[dataTextFields[0]];
                    if (!string.IsNullOrEmpty(formatString))
                        result = string.Format(formatString, obj);
                    else
                        result = Convert.ToString(obj);
                }
                else
                {
                    List<object> list = new List<object>();
                    foreach (string field in dataTextFields)
                    {
                        try
                        {
                            object obj = row[field];
                            if (obj != null)
                                list.Add(obj);
                        }
                        catch { }
                    }

                    object[] objs = new object[list.Count];
                    list.CopyTo(objs, 0);
                    result = string.Format(CultureInfo.CurrentCulture, formatString, objs);
                }
            }
            return result;
        }

        string ConvertDataText(object data, string[] dataTextFields, string formatString)
        {
            string result = string.Empty;
            if (data != null && dataTextFields != null)
            {
                if (dataTextFields.Length == 1)
                    result = DataBinder.GetPropertyValue(data, dataTextFields[0], formatString);
                else
                {
                    List<object> list = new List<object>();
                    foreach (string field in dataTextFields)
                    {
                        try
                        {
                            object obj = DataBinder.GetPropertyValue(data, field);
                            if (obj != null)
                                list.Add(obj);
                        }
                        catch { }
                    }
                    object[] objs = new object[list.Count];
                    list.CopyTo(objs, 0);
                    result = string.Format(CultureInfo.CurrentCulture, formatString, objs);
                }
            }
            return result;
        }
        /// <summary>
        /// 清除列表选择并将所有项的 <see cref="ListItem.Selected"/> 属性设置为 false。
        /// </summary>
        public virtual void ClearSelection()
        {
            for (int i = 0; i < this.Items.Count; i++)
                this.Items[i].Selected = false;
        }

        internal void SelectInternal(ArrayList selecetedIndices)
        {
            this.ClearSelection();
            for (int i = 0; i < selecetedIndices.Count; i++)
            {
                int index = (int)selecetedIndices[i];
                if (index >= 0 && index < this.Items.Count)
                    this.Items[index].Selected = true;
            }
            this.cachedSelectedIndices = selecetedIndices;
        }
        
        #region IEditableTextControl 成员
        /// <summary>
        /// 当 <see cref="Text"/> 和 <see cref="SelectedValue"/> 属性更改时发生。
        /// </summary>
        [Category("Action")]
        [Description("当 Text 和 SelectedValue 属性更改时发生。")]
        public event EventHandler TextChanged;
        /// <summary>
        /// 触发<see cref="TextChanged"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler handler = this.TextChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region ITextControl 成员
        /// <summary>
        /// 获取或设置 <see cref="ListControlEx"/> 控件的 <see cref="ListControlEx.SelectedValue"/> 属性。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置ListControlEx 控件的 SelectedValue属性。")]
        [Browsable(false)]
        [Themeable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Text
        {
            get
            {
                return this.SelectedValue;
            }
            set
            {
                this.SelectedValue = value;
            }
        }

        #endregion
    }
}
