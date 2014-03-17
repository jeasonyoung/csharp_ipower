
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

using System.Drawing;
using System.Globalization;
using iPower.Utility;

namespace iPower.Web.UI
{
	/// <summary>
	/// 允许单项或多项选择的列表框控件,
	/// DataTextFiled支持多个数据列绑定,DataTextField多个字段之间用[,]分开
	/// </summary>
	[ToolboxData("<{0}:ListBoxEx runat=server></{0}:ListBoxEx>")]
    [ToolboxBitmap(typeof(ListBoxEx))]
    [ValidationProperty("SelectedItem")]
    [SupportsEventValidationEx]
	public class ListBoxEx : ListControlEx, IPostBackDataHandler
	{
		#region 成员变量，构造函数，析构函数。
        StringCollection m_CheckedValue;//默认选中的值
 		/// <summary>
        /// 构造函数。
		/// </summary>
		public ListBoxEx()
		{
			this.CssClass="ListBoxFlat";
		}
		#endregion

        #region 属性。
        /// <summary>
        /// 获取或设置控件中显示的行数。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置控件中显示的行数。")]
        public virtual int Rows
        {
            get
            {
                object obj = this.ViewState["Rows"];
                return obj == null ? 4 : (int)obj;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                this.ViewState["Rows"] = value;
            }
        }
        /// <summary>
        /// 获取或设置<see cref="ListBoxEx"/>  控件的选择模式。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置ListBoxEx控件的选择模式。")]
        public virtual ListSelectionMode SelectionMode
        {
            get
            {
                object obj = this.ViewState["SelectionMode"];
                return obj == null ? ListSelectionMode.Single : (ListSelectionMode)obj;
            }
            set
            {
                if ((value < ListSelectionMode.Single) || (value > ListSelectionMode.Multiple))
                    throw new ArgumentOutOfRangeException("value");
                this.ViewState["SelectionMode"] = value;
            }
        }
        /// <summary>
		/// 获取或设置数据列之间的分隔符,默认为[:]。
		/// </summary>
		[Bindable(true)]
		[Category("Other")]
		[Description("数据列之间的分隔符,默认为[:]。")]
		public char SplitChar
		{
			get
			{
                object o = this.ViewState["SplitChar"];
                return (o == null) ? ':' : (char)o;
			}
			set
			{
                if (this.SplitChar != value)
                    this.ViewState["SplitChar"] = value;
			}
		}
		/// <summary>
		///  获取或设置控件当前选中的选项的值集合(StringCollection)。
		/// </summary>
		[Browsable(false)]
        [Bindable(false)]
		public StringCollection CheckValue
		{
			get
			{
                StringCollection sc = new StringCollection();
                foreach (ListItem li in this.Items)
                {
                    if (li.Selected)
                        sc.Add(li.Value);
                }
                return sc;
			}
			set
			{
				this.m_CheckedValue=value;
                this.OnDataBinding(new EventArgs());
			}
		}
		/// <summary>
		/// 获取或设置控件当前选中的选项的值(String[])。
		/// </summary>
		[Browsable(false)]
        [Bindable(false)]
		public string[] CheckedValue2
		{
			get
			{
				return ConvertEx.ToStringArray(this.CheckValue);
			}
			set
			{
                this.CheckValue = ConvertEx.ToStringCollection(value);
			}
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 基础结构。将 name、size、multiple 和 onchange 添加到要呈现的属性的列表中。
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Size, this.Rows.ToString(NumberFormatInfo.InvariantInfo));
            string uniqueID = this.UniqueID;
            if (!string.IsNullOrEmpty(uniqueID))
                writer.AddAttribute(HtmlTextWriterAttribute.Name, uniqueID);
            base.AddAttributesToRender(writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.Page != null && (this.SelectionMode == ListSelectionMode.Multiple) && this.Enabled)
                this.Page.RegisterRequiresPostBack(this);
        }
        #endregion
        /// <summary>
        /// 获取当前在 <see cref="ListBoxEx"/> 控件中选定的项的索引值数组。
        /// </summary>
        /// <returns></returns>
        public virtual int[] GetSelectedIndices()
        {
            return (int[])this.SelectedIndicsInternal.ToArray(typeof(int));
        }
        /// <summary>
        /// 获取ListBox控件当前所有项的值列表（以split参数指定的字符分隔）。
        /// </summary>
        /// <param name="split"></param>
        /// <returns></returns>
        public string GetItemValueList(char split)
        {
            string strValue = string.Empty;
            foreach (ListItem li in this.Items)
            {
                strValue += string.Format("{0}{1}", split, li.Value);
            }
            if (!string.IsNullOrEmpty(strValue))
                strValue = strValue.Substring(1);
            return strValue;
        }
	 
        #region IPostBackDataHandler 成员
        /// <summary>
        /// 如果列表控件的已发布内容不同于上次发布的内容，则加载该内容。
        /// </summary>
        /// <param name="postDataKey">控件的键标识符，用于对 postCollection 进行索引。</param>
        /// <param name="postCollection">它包含由控件标识符索引的值信息。</param>
        /// <returns>如果发布的内容不同于上一次发布的内容，则为 true；否则为 false。</returns>
        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            if (!this.IsEnabled)
                return false;
            bool result = false;
            string[] values = postCollection.GetValues(postDataKey);
            if (values == null)
            {
                if (this.SelectedIndex != -1)
                {
                    this.SetPostDataSelection(-1);
                    result = true;
                }
                return result;
            }

            if (this.SelectionMode == ListSelectionMode.Single)
            {
                this.ValidateEvent(postDataKey, values[0]);
                ListItem item = this.Items.FindByValue(values[0]);
                if (item != null)
                {
                    int selectedIndex = this.Items.IndexOf(item);
                    if (this.SelectedIndex != selectedIndex)
                    {
                        this.SetPostDataSelection(selectedIndex);
                        result = true;
                    }
                }
                return result;
            }

            int length = values.Length;
            ArrayList selectedIndicesInternal = this.SelectedIndicsInternal;
            ArrayList selectedIndices = new ArrayList(length);
            for (int i = 0; i < length; i++)
            {
                this.ValidateEvent(postDataKey, values[i]);
                ListItem item = this.Items.FindByValue(values[i]);
                if (item != null)
                    selectedIndices.Add(this.Items.IndexOf(item));
            }
            int count = 0;
            if (selectedIndicesInternal != null)
                count = selectedIndicesInternal.Count;
            if (count == length)
            {
                for (int i = 0; i < length; i++)
                {
                    if ((int)selectedIndices[i] != (int)selectedIndicesInternal[i])
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
                result = true;

            if (result)
                this.SelectInternal(selectedIndices);
            return result;
        }
        /// <summary>
        /// 当 <see cref="ListBoxEx"/>  控件的已发布数据发生更改时，调用 <see cref="ListControlEx.OnSelectedIndexChanged"/>  方法。
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            if (this.AutoPostBack && !this.Page.IsPostBackEventControlRegistered)
            {
                this.Page.AutoPostBackControl = this;
                if (this.CausesValidation)
                    this.Page.Validate(this.ValidationGroup);
            }
            this.OnSelectedIndexChanged(EventArgs.Empty);
        }

        #endregion
    }
}
