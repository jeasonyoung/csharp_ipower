
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
	/// 为用户输入显示一个文本框控件
	/// </summary>
    /// <remarks>
    /// 扩展的TextBox控件,实现了:
    ///     1.只能输入由0-9组成的数字;
    ///     2.文本框有焦点时按回车触发指定的按钮的单击事件
    ///     3.集成RequiredFieldValidator,RangeValidator,CompareValidator及RegularExpressValidator控件的功能
    /// </remarks>
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:TextBoxEx runat='server'></{0}:TextBoxEx>")]
    [ToolboxBitmap(typeof(TextBoxEx))]
	public class TextBoxEx :  TextBox
	{
		#region 成员变量，构造函数，析构函数
        CompareValidatorEx CompareValid;
        RequiredFieldValidatorEx RequiredValid;
	    RegularExpressionValidatorEx RegularValid;
        RangeValidatorEx RangeValid;
		/// <summary>
		/// 构造函数。
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
        /// 析构函数。
        /// </summary>
        ~TextBoxEx()
        {
        }
		#endregion

		#region 验证相关的属性

		#region EnableClientScript
        /// <summary>
        /// 获取或设置是否允许客户端验证。
        /// </summary>
        [Category("Valid")]
        [DefaultValue(true)]
        [Description("获取或设置是否允许客户端验证。")]
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
        /// 获取或设置MouseOut时的按钮的样式表。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置MouseOut时的按钮的样式表。")]
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
        /// 获取或设置MouseOver时的按钮的样式表。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置MouseOver时的按钮的样式表。")]
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
        /// 获取或设置Disabled时的按钮的样式表。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置Disabled时的按钮的样式表。")]
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
        /// 获取或设置该域是否必填。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置该域是否必填。")]
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
        /// 获取或设置必填时的出错提示信息。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置必填时的出错提示信息。")]
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
        /// 获取或设置确定字段验证模式的正则表达式。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置确定字段验证模式的正则表达式。")]
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
        /// 获取或设置输入值跟指定正则表达式不匹配时的提示信息。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置输入值跟指定正则表达式不匹配时的提示信息。")]
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
        /// 获取或设置允许输入的最大值。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置允许输入的最大值。")]
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
        /// 获取或设置允许输入的最小值。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置允许输入的最小值。")]
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
        /// 获取或设置输入值超出指定范围的提示信息。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置输入值超出指定范围的提示信息。")]
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
        /// 获取或设置要与所验证的输入控件进行比较的输入控件，不能与ValueToCompare同时设置。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置要与所验证的输入控件进行比较的输入控件，不能与ValueToCompare同时设置。")]
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
        /// 获取或设置要与所验证的输入控件进行比较的值,不能与ControlToCompare同时设置。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置要与所验证的输入控件进行比较的值,不能与ControlToCompare同时设置。")]
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
        /// 获取或设置要执行的比较操作。
        /// </summary>
        [Category("Valid")]
        [DefaultValue("Equal")]
        [Description("获取或设置要执行的比较操作。")]
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
        /// 获取或设置在比较之前将所比较的值转换到的数据类型。
        /// </summary>
        [Category("Valid")]
        [Bindable(true)]
        [DefaultValue("String")]
        [Description("获取或设置在比较之前将所比较的值转换到的数据类型。")]
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
        /// 获取或设置输入值跟指定控件的值进行指定比较不相符时的提示信息。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置输入值跟指定控件的值进行指定比较不相符时的提示信息。")]
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
		/// 只能输入由0-9组成的数字
		/// </summary>
		[Category("Appearance")]
		[Bindable(true)]
		[DefaultValue(false)]
		[Description("是否只能输入由0-9组成的数字。")]
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

        #region 数据格式。
        /// <summary>
        /// 获取或设置显示数据格式。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置显示数据格式。")]
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
        /// 获取或设置值。
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

        #region 重载OnLoad
        /// <summary>
		/// 重载。
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
		/// 当前的文本框有焦点时按回车触发的按钮,没有指定时不作动作。
		/// </summary>
		[Category("Appearance")]
		[Bindable(true)]
		[Description("当前的文本框有焦点时按回车触发的按钮。")]
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

		#region 重载AddAttributesToRender
		/// <summary>
		/// 将需要呈现的 HTML 属性和样式添加到指定的 System.Web.UI.HtmlTextWriter 中
		/// </summary>
		/// <param name="writer">表示要在客户端呈现 HTML 内容的输出流</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer) 
		{
			//只能输入数字
			if(this.OnlyNumber)
			{
				writer.AddAttribute("onkeypress","javascript:return event.keyCode>=48&&event.keyCode<=57;");
				writer.AddAttribute("onpaste","javascript:return !clipboardData.getData('text').match(/\\D/);");
				writer.AddAttribute("ondragenter","javascript:return false;");
			}

			//触发按钮事件
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
		/// 重载。
		/// </summary>
		protected override void CreateChildControls()
		{
			this.EnsureChildControls();

			base.CreateChildControls ();
			//必填
            this.RequiredValid = new RequiredFieldValidatorEx();
            this.RequiredValid.EnableClientScript = this.EnableClientScript;
            this.RequiredValid.ControlToValidate = this.ID;
            this.RequiredValid.ErrorMessage = this.RequiredErrorMessage;
            this.RequiredValid.InitialValue = string.Empty;
            this.Controls.Add(this.RequiredValid);
            //在指定范围内
            this.RangeValid = new RangeValidatorEx();
            this.RangeValid.EnableClientScript = this.EnableClientScript;
            this.RangeValid.ControlToValidate = this.ID;
            this.RangeValid.ErrorMessage = this.RangeErrorMessage;
            this.RangeValid.MinimumValue = this.MinValue;
            this.RangeValid.MaximumValue = this.MaxValue;
            this.Controls.Add(this.RangeValid);
            //与指定控件的值比较
            this.CompareValid = new CompareValidatorEx();
            this.CompareValid.EnableClientScript = this.EnableClientScript;
            this.CompareValid.ControlToValidate = this.ID;
            this.CompareValid.ErrorMessage = this.CompareErrorMessage;
            this.CompareValid.ControlToCompare = this.ControlToCompare;
            this.CompareValid.ValueToCompare = this.ValueToCompare;
            this.CompareValid.Operator = this.Operator;
            this.CompareValid.Type = this.Type;
            this.Controls.Add(this.CompareValid);
            //与指定的正则表达式匹配
            this.RegularValid = new RegularExpressionValidatorEx();
            this.RegularValid.EnableClientScript = this.EnableClientScript;
            this.RegularValid.ControlToValidate = this.ID;
            this.RegularValid.ErrorMessage = this.RegularErrorMessage;
            this.RegularValid.ValidationExpression = this.ValidationExpression;
            this.Controls.Add(this.RegularValid);
		}

		#region 重载Render
		/// <summary>
		/// 重载。
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
