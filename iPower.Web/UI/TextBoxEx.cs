
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using iPower.Web.UI.Designer;
namespace iPower.Web.UI
{
	/// <summary>
	/// Ϊ�û�������ʾһ���ı���ؼ�
	/// </summary>
    /// <remarks>
    /// ��չ��TextBox�ؼ�,ʵ����:
    ///     1.ֻ��������0-9��ɵ�����;
    ///     2.�ı����н���ʱ���س�����ָ���İ�ť�ĵ����¼�
    ///     3.����RequiredFieldValidator,RangeValidator,CompareValidator��RegularExpressValidator�ؼ��Ĺ���
    /// </remarks>
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:TextBoxEx runat='server'></{0}:TextBoxEx>")]
    [ToolboxBitmap(typeof(TextBoxEx))]
	public class TextBoxEx :  TextBox
	{
		#region ��Ա���������캯������������
        CompareValidatorEx CompareValid;
        RequiredFieldValidatorEx RequiredValid;
	    RegularExpressionValidatorEx RegularValid;
        RangeValidatorEx RangeValid;
		/// <summary>
		/// ���캯����
		/// </summary>
        public TextBoxEx()
            : base()
        {
            this.CompareValid = new CompareValidatorEx();
            this.RequiredValid = new RequiredFieldValidatorEx();
            this.RegularValid = new RegularExpressionValidatorEx();
            this.RangeValid = new RangeValidatorEx();
        }
        /// <summary>
        /// ����������
        /// </summary>
        ~TextBoxEx()
        {
        }
		#endregion

		#region ��֤��ص�����

		#region EnableClientScript
        /// <summary>
        /// ��ȡ�������Ƿ�����ͻ�����֤��
        /// </summary>
        [Category("Valid")]
        [DefaultValue(true)]
        [Description("��ȡ�������Ƿ�����ͻ�����֤��")]
        public bool EnableClientScript
        {
            get
            {
                object obj1 = this.ViewState["EnableClientScript"];
                return ((obj1 == null) ? true : ((bool)obj1));
            }
            set
            {
                this.ViewState["EnableClientScript"] = value;
            }
        }
		#endregion     

        /// <summary>
        /// ��ȡ������MouseOutʱ�İ�ť����ʽ��
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ������MouseOutʱ�İ�ť����ʽ��")]
        public string MouseOutCssClass
        {
            get
            {
                string s = (string)this.ViewState["MouseOutCssClass"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["MouseOutCssClass"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������MouseOverʱ�İ�ť����ʽ��
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ������MouseOverʱ�İ�ť����ʽ��")]
        public string MouseOverCssClass
        {
            get
            {
                string s = (string)this.ViewState["MouseOverCssClass"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["MouseOverCssClass"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������Disabledʱ�İ�ť����ʽ��
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ������Disabledʱ�İ�ť����ʽ��")]
        public string DisabledCssClass
        {
            get
            {
                string s = (string)this.ViewState["DisabledCssClass"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["DisabledCssClass"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����ø����Ƿ���
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ�����ø����Ƿ���")]
        public bool IsRequired
        {
            get
            {
                object obj = this.ViewState["IsRequired"];
                return obj == null ? false : (bool)obj;
            }
            set
            {
                this.ViewState["IsRequired"] = value;
            }
        }
        /// <summary>
        /// ��ȡ�����ñ���ʱ�ĳ�����ʾ��Ϣ��
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ�����ñ���ʱ�ĳ�����ʾ��Ϣ��")]
        public string RequiredErrorMessage
        {
            get
            {
                string s = (string)this.ViewState["RequiredErrorMessage"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["RequiredErrorMessage"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������ȷ���ֶ���֤ģʽ��������ʽ��
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ������ȷ���ֶ���֤ģʽ��������ʽ��")]
        public string ValidationExpression
        {
            get
            {
                string s = (string)this.ViewState["ValidationExpression"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["ValidationExpression"] = value;
            }
        }
        /// <summary>
        /// ��ȡ����������ֵ��ָ��������ʽ��ƥ��ʱ����ʾ��Ϣ��
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ����������ֵ��ָ��������ʽ��ƥ��ʱ����ʾ��Ϣ��")]
        public string RegularErrorMessage
        {
            get
            {
                string s = (string)this.ViewState["RegularErrorMessage"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["RegularErrorMessage"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������������������ֵ��
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ������������������ֵ��")]
        public string MaxValue
        {
            get
            {
                string s = (string)this.ViewState["MaxValue"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["MaxValue"] = value;
            }
        }
        /// <summary>
        /// ��ȡ�����������������Сֵ��
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ�����������������Сֵ��")]
        public string MinValue
        {
            get
            {
                string s = (string)this.ViewState["MinValue"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["MinValue"] = value;
            }
        }
        /// <summary>
        /// ��ȡ����������ֵ����ָ����Χ����ʾ��Ϣ��
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ����������ֵ����ָ����Χ����ʾ��Ϣ��")]
        public string RangeErrorMessage
        {
            get
            {
                string s = (string)this.ViewState["RangeErrorMessage"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["RangeErrorMessage"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������Ҫ������֤������ؼ����бȽϵ�����ؼ���������ValueToCompareͬʱ���á�
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ������Ҫ������֤������ؼ����бȽϵ�����ؼ���������ValueToCompareͬʱ���á�")]
        public string ControlToCompare
        {
            get
            {
                string s = (string)this.ViewState["ControlToCompare"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["ControlToCompare"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������Ҫ������֤������ؼ����бȽϵ�ֵ,������ControlToCompareͬʱ���á�
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ������Ҫ������֤������ؼ����бȽϵ�ֵ,������ControlToCompareͬʱ���á�")]
        public string ValueToCompare
        {
            get
            {
                string s = (string)this.ViewState["ValueToCompare"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["ValueToCompare"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������Ҫִ�еıȽϲ�����
        /// </summary>
        [Category("Valid")]
        [DefaultValue("Equal")]
        [Description("��ȡ������Ҫִ�еıȽϲ�����")]
        public ValidationCompareOperator Operator
        {
            get
            {
                object o = this.ViewState["Operator"];
                return o == null ? ValidationCompareOperator.Equal : (ValidationCompareOperator)o;
            }
            set
            {
                this.ViewState["Operator"] = value;
            }
        }
        /// <summary>
        /// ��ȡ�������ڱȽ�֮ǰ�����Ƚϵ�ֵת�������������͡�
        /// </summary>
        [Category("Valid")]
        [Bindable(true)]
        [DefaultValue("String")]
        [Description("��ȡ�������ڱȽ�֮ǰ�����Ƚϵ�ֵת�������������͡�")]
        public ValidationDataType Type
        {
            get
            {
                object o = this.ViewState["Type"];
                return o == null ? ValidationDataType.String : (ValidationDataType)o;
            }
            set
            {
                this.ViewState["Type"] = value;
            }
        }
        /// <summary>
        /// ��ȡ����������ֵ��ָ���ؼ���ֵ����ָ���Ƚϲ����ʱ����ʾ��Ϣ��
        /// </summary>
        [Category("Valid")]
        [Description("��ȡ����������ֵ��ָ���ؼ���ֵ����ָ���Ƚϲ����ʱ����ʾ��Ϣ��")]
        public string CompareErrorMessage
        {
            get
            {
                string s = (string)this.ViewState["CompareErrorMessage"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["CompareErrorMessage"] = value;
            }
        }
		#endregion

		#region OnlyNumber
		/// <summary>
		/// ֻ��������0-9��ɵ�����
		/// </summary>
		[Category("Appearance")]
		[Bindable(true)]
		[DefaultValue(false)]
		[Description("�Ƿ�ֻ��������0-9��ɵ����֡�")]
		public bool OnlyNumber
		{
			get
			{
                object o = this.ViewState["OnlyNumber"];
                return (o == null) ? false : (bool)o;
			}
			set
			{
                this.ViewState["OnlyNumber"] = value;
			}
		}
		#endregion

        #region ���ݸ�ʽ��
        /// <summary>
        /// ��ȡ��������ʾ���ݸ�ʽ��
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ��������ʾ���ݸ�ʽ��")]
        public string DataFormatString
        {
            get
            {
                return this.ViewState["DataFormatString"] as string;
            }
            set
            {
                this.ViewState["DataFormatString"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������ֵ��
        /// </summary>
        public override string Text
        {
            get
            {
                string dataValue = base.Text;
                if (!string.IsNullOrEmpty(this.DataFormatString))
                {
                    return string.Format(this.DataFormatString, dataValue);
                }
                return dataValue;
            }
            set
            {
                base.Text = value;
            }
        }
        #endregion

        #region ����OnLoad
        /// <summary>
		/// ���ء�
		/// </summary>
		/// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CssClass))
            {
                if (this.TextMode == TextBoxMode.MultiLine)
                {
                    this.CssClass = "MultiLineTextBoxFlat";
                }
                else
                {
                    this.CssClass = "TextBoxFlat";
                }
            }
            base.OnLoad(e);
        }
		#endregion

		#region EnterInvokeControlID
		/// <summary>
		/// ��ǰ���ı����н���ʱ���س������İ�ť,û��ָ��ʱ����������
		/// </summary>
		[Category("Appearance")]
		[Bindable(true)]
		[Description("��ǰ���ı����н���ʱ���س������İ�ť��")]
		[TypeConverter(typeof(ButtonExConverter))]
		public string EnterInvokeControlID
		{
			get
			{
                object o = this.ViewState["EnterInvokeControlID"];
                return (o == null) ? string.Empty : (string)o;
			}
			set
			{
                if (this.EnterInvokeControlID != value)
                    this.ViewState["EnterInvokeControlID"] = value;			
			}
		}
		#endregion

		#region ����AddAttributesToRender
		/// <summary>
		/// ����Ҫ���ֵ� HTML ���Ժ���ʽ��ӵ�ָ���� System.Web.UI.HtmlTextWriter ��
		/// </summary>
		/// <param name="writer">��ʾҪ�ڿͻ��˳��� HTML ���ݵ������</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer) 
		{
			//ֻ����������
			if(this.OnlyNumber)
			{
				writer.AddAttribute("onkeypress","javascript:return event.keyCode>=48&&event.keyCode<=57;");
				writer.AddAttribute("onpaste","javascript:return !clipboardData.getData('text').match(/\\D/);");
				writer.AddAttribute("ondragenter","javascript:return false;");
			}

			//������ť�¼�
			if(!string.IsNullOrEmpty(this.EnterInvokeControlID))
			{
				Control ctl=this.Parent.FindControl(this.EnterInvokeControlID);
                if (ctl != null)
                {
                    string script = "javascript:if(window.event.keyCode==13){";
                    script += string.Format("window.document.all.{0}.focus();window.document.all.{0}.click(); return false;", ctl.ClientID);
                    script += "}";
                    writer.AddAttribute("onkeydown", script);
                }
			}

            if (!this.Enabled && !string.IsNullOrEmpty(this.DisabledCssClass))
            {
                this.CssClass = this.DisabledCssClass;
            }
            else if (!string.IsNullOrEmpty(this.MouseOutCssClass) &&
                    !string.IsNullOrEmpty(this.MouseOverCssClass))
            {
                writer.AddAttribute("language", "javascript");
                writer.AddAttribute("onmouseover", string.Format("this.className='{0}';", this.MouseOverCssClass));
                writer.AddAttribute("onmouseout", string.Format("this.className='{0}';", this.MouseOutCssClass));
            }
			base.AddAttributesToRender(writer);
		}
		#endregion

		/// <summary>
		/// ���ء�
		/// </summary>
		protected override void CreateChildControls()
		{
			this.EnsureChildControls();

			base.CreateChildControls ();
			//����
            this.RequiredValid = new RequiredFieldValidatorEx();
            this.RequiredValid.EnableClientScript = this.EnableClientScript;
            this.RequiredValid.ControlToValidate = this.ID;
            this.RequiredValid.ErrorMessage = this.RequiredErrorMessage;
            this.RequiredValid.InitialValue = string.Empty;
            this.Controls.Add(this.RequiredValid);
            //��ָ����Χ��
            this.RangeValid = new RangeValidatorEx();
            this.RangeValid.EnableClientScript = this.EnableClientScript;
            this.RangeValid.ControlToValidate = this.ID;
            this.RangeValid.ErrorMessage = this.RangeErrorMessage;
            this.RangeValid.MinimumValue = this.MinValue;
            this.RangeValid.MaximumValue = this.MaxValue;
            this.Controls.Add(this.RangeValid);
            //��ָ���ؼ���ֵ�Ƚ�
            this.CompareValid = new CompareValidatorEx();
            this.CompareValid.EnableClientScript = this.EnableClientScript;
            this.CompareValid.ControlToValidate = this.ID;
            this.CompareValid.ErrorMessage = this.CompareErrorMessage;
            this.CompareValid.ControlToCompare = this.ControlToCompare;
            this.CompareValid.ValueToCompare = this.ValueToCompare;
            this.CompareValid.Operator = this.Operator;
            this.CompareValid.Type = this.Type;
            this.Controls.Add(this.CompareValid);
            //��ָ����������ʽƥ��
            this.RegularValid = new RegularExpressionValidatorEx();
            this.RegularValid.EnableClientScript = this.EnableClientScript;
            this.RegularValid.ControlToValidate = this.ID;
            this.RegularValid.ErrorMessage = this.RegularErrorMessage;
            this.RegularValid.ValidationExpression = this.ValidationExpression;
            this.Controls.Add(this.RegularValid);
		}

		#region ����Render
		/// <summary>
		/// ���ء�
		/// </summary>
		/// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if ((!string.IsNullOrEmpty(this.ControlToCompare) || !string.IsNullOrEmpty(this.ValueToCompare)) && this.CompareValid != null)
            {
                this.CompareValid.RenderControl(writer);
            }
            if ((!string.IsNullOrEmpty(this.MinValue) || !string.IsNullOrEmpty(this.MaxValue)) && this.RangeValid != null)
            {
                this.RangeValid.RenderControl(writer);
            }
            if (!string.IsNullOrEmpty(this.ValidationExpression) && RegularValid != null)
            {
                this.RegularValid.RenderControl(writer);
            }
            if (this.IsRequired && this.RequiredValid != null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("*");
                writer.RenderEndTag();

                this.RequiredValid.RenderControl(writer);
            }
        }
		#endregion
    }
}
