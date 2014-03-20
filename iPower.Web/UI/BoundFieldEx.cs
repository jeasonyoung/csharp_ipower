//================================================================================
//  FileName: BoundFieldEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/7
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Globalization;

using iPower.Utility;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示数据绑定控件中作为文本显示的字段。
    /// </summary>
    public class BoundFieldEx : DataControlFieldEx
    {
        #region 成员变量，构造函数。
        private string dataField, toolTipField, dataFormatString,toolTipFieldFormatString;
        private bool htmlEncode, htmlEncodeFormatString, htmlEncodeFormatStringSet, htmlEncodeSet, supressHeaderTextFieldChange;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BoundFieldEx()
            : base()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置是否启用行选中事件。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置是否启用行选中事件。")]
        public virtual bool ShowRowSelectingEvent
        {
            get
            {
                object o = this.ViewState["ShowRowSelectingEvent"];
                return o == null ? false : (bool)o;
            }
            set
            {
                if (this.ShowRowSelectingEvent != value)
                    this.ViewState["ShowRowSelectingEvent"] = value;
            }
        }

        #region ToolTip
        /// <summary>
        /// 获取或设置ToolTip。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置ToolTip。")]
        public virtual bool ShowToolTip
        {
            get
            {
                object o = this.ViewState["ShowToolTip"];
                return o == null ? false : (bool)o;
            }
            set
            {
                if (this.ShowToolTip != value)
                    this.ViewState["ShowToolTip"] = value;
            }
        }
        /// <summary>
        /// 获取或设置要显示的字符数，一个汉字当作两个字符;
        /// 设置该属性时列表显示的文本具有ToolTip的功能。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置要显示的字符数;设置该属性时列表显示的文本具有ToolTip的功能。")]
        public virtual int CharCount
        {
            get
            {
                object o = this.ViewState["CharCount"];
                return o == null ? 0 : (int)o;
            }
            set
            {
                if (this.CharCount != value)
                    this.ViewState["CharCount"] = value;
            }
        }
        /// <summary>
        /// 获取或设置ToolTip字段的名称。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置ToolTip字段的名称。")]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceViewSchemaConverter))]
        public virtual string ToolTipField
        {
            get
            {
                if (string.IsNullOrEmpty(this.toolTipField))
                {
                    this.toolTipField = this.ViewState["ToolTipField"] as string;
                }
                return this.toolTipField;
            }
            set
            {
                if (!string.Equals(value, this.ToolTipField))
                {
                    this.ViewState["ToolTipField"] = this.toolTipField = value;
                    this.OnFieldChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置ToolTip值的显示格式。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置ToolTip值的显示格式。")]
        public virtual string ToolTipFieldFormatString
        {
            get
            {
                if (string.IsNullOrEmpty(this.toolTipFieldFormatString))
                {
                    this.toolTipFieldFormatString = this.ViewState["ToolTipFieldFormatString"] as string;
                }
                return this.toolTipFieldFormatString;
            }
            set
            {
                if (!string.Equals(value, this.toolTipFieldFormatString))
                {
                    this.ViewState["ToolTipFieldFormatString"] = this.toolTipFieldFormatString = value;
                    this.OnFieldChanged();
                }
            }
        }
        #endregion
        /// <summary>
        /// 获取或设置数据字段的名称。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置数据字段的名称。")]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceViewSchemaConverter))]
        public virtual string DataField
        {
            get
            {
                if (string.IsNullOrEmpty(this.dataField))
                {
                    this.dataField = this.ViewState["DataField"] as string;
                }
                return this.dataField;
            }
            set
            {
                if (!string.Equals(value, this.DataField))
                {
                    this.ViewState["DataField"] = this.dataField = value;
                    this.OnFieldChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置字段值的显示格式。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置字段值的显示格式。")]
        public virtual string DataFormatString
        {
            get
            {
                if (string.IsNullOrEmpty(this.dataFormatString))
                {
                    this.dataFormatString = this.ViewState["DataFormatString"] as string;
                }
                return this.dataFormatString;
            }
            set
            {
                if (!string.Equals(value, this.DataFormatString))
                {
                    this.ViewState["DataFormatString"] = this.dataFormatString = value;
                    this.OnFieldChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置显示在数据控件标头中的文本。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置显示在数据控件标头中的文本。")]
        public override string HeaderText
        {
            get
            {
                return base.HeaderText;
            }
            set
            {
                if (!string.Equals(value, this.HeaderText))
                {
                    base.ViewState["HeaderText"] = value;
                    if (!this.supressHeaderTextFieldChange)
                        this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置显示字段值之前，是否对这些字段值进行 HTML 编码。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置显示字段值之前，是否对这些字段值进行 HTML 编码。")]
        public virtual bool HtmlEncode
        {
            get
            {
                if (!this.htmlEncodeSet)
                {
                    object obj = this.ViewState["HtmlEncode"];
                    this.htmlEncode = (obj == null) ? true : (bool)obj;
                    this.htmlEncodeSet = true;
                }
                return this.htmlEncode;
            }
            set
            {
                if (value != this.HtmlEncode)
                {
                    this.ViewState["HtmlEncode"] = this.htmlEncode = value;
                    this.htmlEncodeSet = true;
                    this.OnFieldChanged();
                }
            }
        }
       
        /// <summary>
        /// 获取或设置格式化的文本在显示时是否应经过 HTML 编码。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置格式化的文本在显示时是否应经过 HTML 编码。")]
        public virtual bool HtmlEncodeFormatString
        {
            get
            {
                if (!this.htmlEncodeFormatStringSet)
                {
                    object obj = this.ViewState["HtmlEncodeFormatString"];
                    this.htmlEncodeFormatString = (obj == null) ? true : (bool)obj;
                    this.htmlEncodeFormatStringSet = true;
                }
                return this.htmlEncodeFormatString;
            }
            set
            {
                if (value != this.HtmlEncodeFormatString)
                {
                    this.ViewState["HtmlEncodeFormatString"] = this.HtmlEncodeFormatString = value;
                    this.htmlEncodeFormatStringSet = true;
                    this.OnFieldChanged();
                }
            }
        }

        /// <summary>
        /// 是否支持 HTML 编码。
        /// </summary>
        protected virtual bool SupportsHtmlEncode
        {
            get { return true; }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 复制属性。
        /// </summary>
        /// <param name="newField"></param>
        protected override void CopyProperties(DataControlFieldEx newField)
        {
            ((BoundFieldEx)newField).DataField = this.DataField;
            ((BoundFieldEx)newField).ToolTipField = this.ToolTipField;
            ((BoundFieldEx)newField).DataFormatString = this.DataFormatString;
            ((BoundFieldEx)newField).HtmlEncode = this.HtmlEncode;
            ((BoundFieldEx)newField).HtmlEncodeFormatString = this.HtmlEncodeFormatString;

            base.CopyProperties(newField);
        }
        /// <summary>
        /// 创建字段对象。
        /// </summary>
        /// <returns></returns>
        protected override DataControlFieldEx CreateField()
        {
            return new BoundFieldEx();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="sortingEnabled"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public override bool Initialize(bool sortingEnabled, Control control)
        {
            base.Initialize(sortingEnabled, control);
            return false;
        }
        /// <summary>
        /// 初始化为指定的行状态。
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public override void InitializeCell(DataControlFieldCellEx cell, DataControlCellType cellType, DataGridViewRowState rowState, int rowIndex)
        {
            string headerText = string.Empty;
            bool flag = false, flag2 = false;
            if ((cellType == DataControlCellType.Header) && this.SupportsHtmlEncode && this.HtmlEncode)
            {
                headerText = this.HeaderText;
                flag2 = true;
            }

            if (flag2 && !string.IsNullOrEmpty(headerText))
            {
                this.supressHeaderTextFieldChange = true;
                this.HeaderText = HttpUtility.HtmlEncode(headerText);
                flag = true;
            }
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            if (flag)
            {
                this.HeaderText = headerText;
                this.supressHeaderTextFieldChange = false;
            }

            if (cellType == DataControlCellType.DataCell)
            {
                if (this.ShowRowSelectingEvent)
                {
                    IButtonControl control;
                    IPostBackContainer container = base.Control as IPostBackContainer;
                    if (container != null)
                    {
                        control = new DataControlLinkButton(container);
                    }
                    else
                        control = new LinkButton();
                    control.CommandName = "RowSelectingEvent";
                    control.CommandArgument = rowIndex.ToString(CultureInfo.InvariantCulture);
                    ((WebControl)control).DataBinding += new EventHandler(this.OnDataBindField);
                    cell.Controls.Add((WebControl)control);
                }
                else
                    this.InitializeDataCell(cell, rowState);
            }
        }
        /// <summary>
        /// 还原视图。
        /// </summary>
        /// <param name="state"></param>
        public override void LoadViewState(object state)
        {
            this.dataField = null;
            this.dataFormatString = null;
            this.htmlEncodeSet = this.htmlEncodeFormatStringSet = false;
            base.LoadViewState(state);
        }
        /// <summary>
        /// 对象中的控件是否支持回调。
        /// </summary>
        public override void ValidateSupportsCallback()
        {
        }
        #endregion
        /// <summary>
        /// 引发 FieldChanged 事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDataBindField(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Control namingContainer = control.NamingContainer;
            object[] dataValue = this.GetValue(namingContainer, this.DataField);
            bool encode = this.SupportsHtmlEncode && this.HtmlEncode && (control is TableCell);
            string str = this.FormatDataValue(this.DataFormatString, dataValue, encode);
            int charCount = this.CharCount;
            if (string.IsNullOrEmpty(str))
                str = "&nbsp;";
            else if ((charCount > 0) && (str.Length > charCount))
                str = ConvertEx.CutString(str, charCount, 1);
            if (control is TableCell)
            {
                ((TableCell)control).Text = str;
                if (this.ShowToolTip && !string.IsNullOrEmpty(this.ToolTipField))
                {
                    object[] values = this.GetValue(namingContainer, this.ToolTipField);
                    ((TableCell)control).ToolTip = this.FormatDataValue(this.ToolTipFieldFormatString, values);
                }
            }
            else if (control is IButtonControl)
            {
                ((IButtonControl)control).Text = str;
                if ((control is WebControl) && this.ShowToolTip && !string.IsNullOrEmpty(this.ToolTipField))
                {
                    object[] values = this.GetValue(namingContainer, this.ToolTipField);
                    ((WebControl)control).ToolTip = this.FormatDataValue(this.ToolTipFieldFormatString, values);
                }
            }
        }
        /// <summary>
        /// 初始化为指定的行状态。
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="rowState"></param>
        protected virtual void InitializeDataCell(DataControlFieldCellEx cell, DataGridViewRowState rowState)
        {
            Control control = null;
            if (!string.IsNullOrEmpty(this.DataField))
                control = cell;
            if ((control != null) && base.Visible)
                control.DataBinding += new EventHandler(this.OnDataBindField);
        }

        /// <summary>
        /// 格式化指定字段值。
        /// </summary>
        /// <param name="dataFormat">数据格式化。</param>
        /// <param name="dataValue">字段值对象。</param>
        /// <param name="encode">若对该值进行编码，则为 true；否则，为 false。</param>
        /// <returns>已转换为所指定格式的字段值。</returns>
        protected virtual string FormatDataValue(string dataFormat, object[] dataValue, bool encode)
        {
            try
            {
                if (dataValue == null || dataValue.Length == 0) return string.Empty;
                if (string.IsNullOrEmpty(dataFormat)) dataFormat = "{0}";
                string value = string.Format(CultureInfo.CurrentCulture, dataFormat, dataValue);
                if (encode) return HttpUtility.HtmlEncode(value);
                return value;
            }
            catch (Exception e)
            {
                return e.Message + "[" + dataFormat + "]";
            }
        }
        /// <summary>
        /// 格式化指定字段值。
        /// </summary>
        /// <param name="dataFormat">数据格式化。</param>
        /// <param name="dataValue">字段值对象。</param>
        /// <returns>已转换为所指定格式的字段值。</returns>
        protected virtual string FormatDataValue(string dataFormat, object[] dataValue)
        {
            return this.FormatDataValue(dataFormat, dataValue, false);
        }
 
        /// <summary>
        /// 设计时用于字段值的值。
        /// </summary>
        /// <returns></returns>
        protected virtual object GetDesignTimeValue()
        {
            return string.Format("[{0}]", this.DataField);
        }
        /// <summary>
        /// 检索绑定到对象的字段值。
        /// </summary>
        /// <param name="controlContainer">字段值的容器。</param>
        /// <param name="dataField">字段名称</param>
        /// <returns></returns>
        protected virtual object[] GetValue(Control controlContainer, string dataField)
        {
            if (controlContainer == null)
                throw new HttpException("DataControlFieldEx_NoContainer");
            if (string.IsNullOrEmpty(dataField))
                throw new ArgumentNullException("DataControlFieldEx_dataField");
            if (this.DesignMode)
            {
                return new object[] { this.GetDesignTimeValue() };
            }
            object component = DataBinder.GetDataItem(controlContainer);
            if (component == null) return null;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
            if (properties == null || properties.Count == 0) return null;
            string[] fields = dataField.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<object> list = new List<object>();
            PropertyDescriptor pd = null;
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == null || string.IsNullOrEmpty(fields[i]))
                {
                    list.Add(null);
                    continue;
                }
                pd = properties.Find(fields[i], true);
                if (pd == null)
                    throw new HttpException("Field_Not_Found :" + fields[i]);
                list.Add(pd.GetValue(component));
            }
            return list.ToArray();
        }
        /// <summary>
        /// 检索绑定到对象的字段值。
        /// </summary>
        /// <param name="controlContainer">字段值的容器。</param>
        /// <returns></returns>
        protected virtual string GetValue(Control controlContainer)
        {
            object[] values = this.GetValue(controlContainer, this.DataField);
            return this.FormatDataValue(this.DataFormatString, values);
        }
    }
}