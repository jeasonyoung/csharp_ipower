
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
	/// ����������ѡ����б��ؼ�,
	/// DataTextFiled֧�ֶ�������а�,DataTextField����ֶ�֮����[,]�ֿ�
	/// </summary>
	[ToolboxData("<{0}:ListBoxEx runat=server></{0}:ListBoxEx>")]
    [ToolboxBitmap(typeof(ListBoxEx))]
    [ValidationProperty("SelectedItem")]
    [SupportsEventValidationEx]
	public class ListBoxEx : ListControlEx, IPostBackDataHandler
	{
		#region ��Ա���������캯��������������
        StringCollection m_CheckedValue;//Ĭ��ѡ�е�ֵ
 		/// <summary>
        /// ���캯����
		/// </summary>
		public ListBoxEx()
		{
			this.CssClass="ListBoxFlat";
		}
		#endregion

        #region ���ԡ�
        /// <summary>
        /// ��ȡ�����ÿؼ�����ʾ��������
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ�����ÿؼ�����ʾ��������")]
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
        /// ��ȡ������<see cref="ListBoxEx"/>  �ؼ���ѡ��ģʽ��
        /// </summary>
        [Category("Behavior")]
        [Description("��ȡ������ListBoxEx�ؼ���ѡ��ģʽ��")]
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
		/// ��ȡ������������֮��ķָ���,Ĭ��Ϊ[:]��
		/// </summary>
		[Bindable(true)]
		[Category("Other")]
		[Description("������֮��ķָ���,Ĭ��Ϊ[:]��")]
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
		///  ��ȡ�����ÿؼ���ǰѡ�е�ѡ���ֵ����(StringCollection)��
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
		/// ��ȡ�����ÿؼ���ǰѡ�е�ѡ���ֵ(String[])��
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

        #region ���ء�
        /// <summary>
        /// �����ṹ���� name��size��multiple �� onchange ��ӵ�Ҫ���ֵ����Ե��б��С�
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
        /// ��ȡ��ǰ�� <see cref="ListBoxEx"/> �ؼ���ѡ�����������ֵ���顣
        /// </summary>
        /// <returns></returns>
        public virtual int[] GetSelectedIndices()
        {
            return (int[])this.SelectedIndicsInternal.ToArray(typeof(int));
        }
        /// <summary>
        /// ��ȡListBox�ؼ���ǰ�������ֵ�б���split����ָ�����ַ��ָ�����
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
	 
        #region IPostBackDataHandler ��Ա
        /// <summary>
        /// ����б�ؼ����ѷ������ݲ�ͬ���ϴη��������ݣ�����ظ����ݡ�
        /// </summary>
        /// <param name="postDataKey">�ؼ��ļ���ʶ�������ڶ� postCollection ����������</param>
        /// <param name="postCollection">�������ɿؼ���ʶ��������ֵ��Ϣ��</param>
        /// <returns>������������ݲ�ͬ����һ�η��������ݣ���Ϊ true������Ϊ false��</returns>
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
        /// �� <see cref="ListBoxEx"/>  �ؼ����ѷ������ݷ�������ʱ������ <see cref="ListControlEx.OnSelectedIndexChanged"/>  ������
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
