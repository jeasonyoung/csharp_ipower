using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.ComponentModel;
using System.Drawing;

[assembly: WebResource("iPower.Web.UI.PickerBase_Picker.gif", "image/gif")]
[assembly: WebResource("iPower.Web.UI.PickerBase_Clear.gif", "image/gif")]
namespace iPower.Web.UI
{
    /// <summary>
    /// 数据选择控件。
    /// </summary>
    [DefaultProperty("Text")]
    [ValidationProperty("Value")]
    [ToolboxBitmap(typeof(PickerBase))]
    public class PickerBase : WebControl, INamingContainer, IValidator, IPostBackEventHandler
    {
        #region 成员变量，构造函数。
        TextBox txtText;
        HtmlInputHidden txtValue;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PickerBase()
            : base()
        {
            this.Width = new Unit("200px");
            this.txtText = new TextBox();
            this.txtValue = new HtmlInputHidden();          
        }
        #endregion

        /// <summary>
        /// 数据发生改变事件。
        /// </summary>
        [Category("Event")]
        [Description("数据发生改变事件。")]
        public event EventHandler TextChanged;

        #region 属性。

        /// <summary>
        /// 获取或设置文本框模式。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置文本框模式。")]
        public TextBoxMode TextBoxMode
        {
            get { return this.txtText.TextMode; }
            set { this.txtText.TextMode = value; }
        }
        /// <summary>
        /// 获取或设置文本框行数。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置文本框行数。")]
        public int Rows
        {
            get { return this.txtText.Rows; }
            set { this.txtText.Rows = value; }
        }

        /// <summary>
        /// 获取或设置事件回发。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置事件回发。")]
        public virtual bool AutoPostBack
        {
            get
            {
                object obj2 = this.ViewState["AutoPostBack"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["AutoPostBack"] = value;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool Enabled
        {
            get
            {
                this.EnsureChildControls();
                return true;
            }
            set
            {
                this.EnsureChildControls();
                this.txtText.Enabled = value;
            }
        }
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
        /// 获取或设置是否允许多选择,默认为多选。
        /// </summary>
        [Category("Appearance"), Description("获取或设置是否允许多选择,默认为多选。"),Bindable(true)]
        public bool MultiSelect
        {
            get
            {
                object obj = this.ViewState["MultiSelect"];
                return ((obj == null) || ((bool)obj));
            }
            set
            {
                this.ViewState["MultiSelect"] = value;
            }
        }
        /// <summary>
        /// 获取或设置要打开的页面。
        /// </summary>
        [Description("获取或设置要打开的页面。")]
        public string PickerPage
        {
            get
            {
                object obj = this.ViewState["PickerPage"];
                return ((obj == null) ? "" : obj.ToString());
            }
            set
            {
                this.ViewState["PickerPage"] = value;
            }
        }
        /// <summary>
        /// 获取或设置控件是否是不可编辑的。
        /// </summary>
        [Category("Appearance"), Description("获取或设置控件是否是不可编辑的。"), DefaultValue("false"), Bindable(true)]
        public bool Readonly
        {
            get
            {
                this.EnsureChildControls();
                object obj2 = this.ViewState["Readonly"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.EnsureChildControls();
                this.ViewState["Readonly"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否将已选择的值做为查询参数传给选择页面(QueryString的参数名为Value)，默认为true(多选用户时建议设为false)。
        /// </summary>
        [DefaultValue(true), Description("获取或设置是否将已选择的值做为查询参数传给选择页面(QueryString的参数名为Value)，默认为true(多选用户时建议设为false)。")]
        public bool SendPickedValueToPickerPage
        {
            get
            {
                object obj = this.ViewState["SendPickedValueToPickerPage"];
                return ((obj == null) || ((bool)obj));
            }
            set
            {
                this.ViewState["SendPickedValueToPickerPage"] = value;
            }
        }
        /// <summary>
        /// 获取或设置控件显示值。
        /// </summary>
        [Category("Appearance"), Description("获取或设置控件显示值。"), Bindable(true)]
        public string Text
        {
            get
            {
                this.EnsureChildControls();
                return this.txtText.Text;
            }
            set
            {
                this.EnsureChildControls();
                this.txtText.Text = value;
            }
        }
        /// <summary>
        /// 获取或设置控件返回值。
        /// </summary>
        [Category("Appearance"), Description("获取或设置控件返回值。"), Bindable(true)]
        public string Value
        {
            get
            {
                this.EnsureChildControls();
                return this.txtValue.Value;
            }
            set
            {
                this.EnsureChildControls();
                this.txtValue.Value = value;
            }
        }
        /// <summary>
        /// 获取或设置是否允许输入。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置是否允许输入。")]
        public bool AllowInput
        {
            get
            {
                object obj = this.ViewState["AllowInput"];
                return obj == null ? false : (bool)obj;
            }
            set
            {
                this.ViewState["AllowInput"] = value;
            }
        }
        /// <summary>
        /// 获取或设置弹出窗体宽度。
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("获取或设置弹出窗体宽度。")]
        public Unit PickerWidth
        {
            get
            {
                object o = this.ViewState["PickerWidth"];
                return o == null ? new Unit("600px") : (Unit)o;
            }
            set
            {
                this.ViewState["PickerWidth"] = value;
            }
        }
        /// <summary>
        /// 获取或设置弹出窗体高度。
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("获取或设置弹出窗体高度。")]
        public Unit PickerHeight
        {
            get
            {
                object o = this.ViewState["PickerHeight"];
                return o == null ? new Unit("400px") : (Unit)o;
            }
            set
            {
                this.ViewState["PickerHeight"] = value;
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 重载OnInit方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null)
                this.Page.Validators.Add(this);
        }
        /// <summary>
        /// 重载OnUnload。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnload(EventArgs e)
        {
            if (this.Page != null)
                this.Page.Validators.Remove(this);
            base.OnUnload(e);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.EnsureChildControls();
            //
            this.txtText.Width = new Unit("100%");
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.txtValue);
            // 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            string strKey = string.Format("{0}_Picker", this.ClientID);
            ClientScriptManager scriptManage = this.Page.ClientScript;
            if (scriptManage != null)
            {
                if (this.IsRequired)
                {
                    const string validatorResourceKey = "ValidatorIncludeScript";
                    const string ValidatorIncludeScriptKey = "ValidatorIncludeScript";
                    const string ValidatorOnSubmitKey = "ValidatorOnSubmit";

                    if (!scriptManage.IsClientScriptBlockRegistered(typeof(BaseValidator), validatorResourceKey))
                    {
                        scriptManage.RegisterClientScriptResource(typeof(BaseValidator), "WebUIValidation.js");
                        StringBuilder ValidatorStartupScript = new StringBuilder();
                        ValidatorStartupScript.Append("<script type=\"text/javascript\">\r\n");
                        ValidatorStartupScript.Append("<!--\r\n");
                        ValidatorStartupScript.Append("var Page_ValidationActive = false;\r\n");
                        ValidatorStartupScript.Append("if (typeof(ValidatorOnLoad) == \"function\") {\r\n");
                        ValidatorStartupScript.Append("\tValidatorOnLoad();\r\n");
                        ValidatorStartupScript.Append("}\r\n");

                        ValidatorStartupScript.Append("function ValidatorOnSubmit() {\r\n");
                        ValidatorStartupScript.Append("\tif (Page_ValidationActive) {\r\n");
                        ValidatorStartupScript.Append("\t\treturn ValidatorCommonOnSubmit();\r\n");
                        ValidatorStartupScript.Append("\t}else {\r\n");
                        ValidatorStartupScript.Append("\t\treturn true;\r\n");
                        ValidatorStartupScript.Append("\t}\r\n");
                        ValidatorStartupScript.Append("}\r\n");

                        ValidatorStartupScript.Append("// -->\r\n");
                        ValidatorStartupScript.Append("</script>\r\n");
                        scriptManage.RegisterStartupScript(typeof(BaseValidator), ValidatorIncludeScriptKey, ValidatorStartupScript.ToString());

                        string ValidatorOnSubmitScript = "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;";
                        scriptManage.RegisterOnSubmitStatement(typeof(BaseValidator), ValidatorOnSubmitKey, ValidatorOnSubmitScript);
                    }
                    string strScript = string.Format("{0}_RequiredFieldValidator", this.ClientID);
                    if (!scriptManage.IsClientScriptBlockRegistered(strScript))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type=\"text/javascript\">\r\n");
                        sb.Append("<!--\r\n");
                        sb.AppendFormat("function {0}_RequiredFieldValidator(val)\r\n", this.ClientID);
                        sb.Append("{\r\n");
                        sb.Append("\tvar oPicker=document.getElementById(val.controltovalidate);\r\n");
                        sb.Append("\tif (oPicker.value == \"\")\r\n");
                        sb.Append("\t\treturn false;\r\n");

                        sb.Append("\treturn true;\r\n");
                        sb.Append("}\r\n");
                        sb.Append("// -->\r\n");
                        sb.Append("</script>\r\n");

                        scriptManage.RegisterArrayDeclaration("Page_Validators", string.Format("document.getElementById(\"{0}_RequiredField\")", this.ClientID));
                        scriptManage.RegisterClientScriptBlock(this.GetType(), strScript, sb.ToString());
                    }
                }
                if (!scriptManage.IsClientScriptBlockRegistered(typeof(PickerBase), strKey))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<script language=\"javascript\">\r\n");
                    builder.AppendFormat("function {0}()\r\n", strKey);
                    builder.Append("{\r\n");
                    builder.Append("\tvar vTmd=Math.random();\r\n");

                    if(this.SendPickedValueToPickerPage)
                        builder.AppendFormat("\tvar strValue = document.all.{0}.value;\r\n", this.txtValue.ClientID);

                    builder.AppendFormat("\tvar sReturn = window.showModalDialog(\"{0}{1}tmd=\"+vTmd+\"", this.PickerPage,
                        this.PickerPage.IndexOf("?") > -1 ? "&" : "?");

                    if (this.SendPickedValueToPickerPage)
                        builder.Append("&Value=\"+strValue+\"");

                    builder.AppendFormat("&MultiSelect={0}\",window,\"dialogWidth:{1};dialogHeight:{2};help:0\");\r\n",
                        this.MultiSelect, this.PickerWidth, this.PickerHeight);

                    builder.Append("\tif(typeof(sReturn)!=\"undefined\")\r\n");
                    builder.Append("\t{\r\n");
                    builder.Append("\t\tif(sReturn!=\"\")\r\n");
                    builder.Append("\t\t{\r\n");
                    builder.Append("\t\t\tvReturn=sReturn.split('|');\r\n");
                    builder.AppendFormat("\t\t\tdocument.all.{0}.value=vReturn[0];\r\n", this.txtText.ClientID);
                    builder.AppendFormat("\t\t\tdocument.all.{0}.value=vReturn[1];\r\n", this.txtValue.ClientID);
                    builder.AppendFormat("\t\t\tdocument.all.{0}.value=vReturn[1];\r\n", this.ClientID);

                    builder.Append("\t\t\tif(typeof(vReturn[2])!=\"undefined\")\r\n");
                    builder.Append("\t\t\t{\r\n");
                    builder.Append("\t\t\t\teval(vReturn[2]);\r\n");
                    builder.Append("\t\t\t}\r\n");
                    builder.Append("\t\t}\r\n");
                    builder.Append("\t}\r\n");
                    //AutoPostBack
                    if (this.AutoPostBack)
                    {
                        builder.AppendFormat("\t{0}\r\n", scriptManage.GetPostBackEventReference(this,string.Empty));
                    }
                    builder.Append("}\r\n");
                    builder.Append("</script>\r\n");

                    scriptManage.RegisterClientScriptBlock(typeof(PickerBase), strKey, builder.ToString());
                }
            }
            base.OnPreRender(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);

            this.txtText.Attributes["id"] = this.txtText.ClientID;
            this.txtText.CssClass = "TextBoxFlat";

            this.txtValue.Attributes["id"] = this.txtValue.ClientID;

            this.AddAttributesToRender(writer);

            writer.AddAttribute("PickerPage", this.PickerPage, true);
           // writer.AddAttribute("value", this.Value);
            writer.AddAttribute("MultiSelect", this.MultiSelect.ToString());
            //writer.AddAttribute("CallBackFunction", this.CallBackFunction.ToString());
            //writer.AddAttribute("SendValue", this.SendPickedValueToPickerPage.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0", false);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "95%", false);
           
            if (!this.Readonly)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                if (!this.AllowInput)
                    writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true", false);
                this.txtText.RenderControl(writer);
                this.txtValue.RenderControl(writer);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Width, "20", false);
                if (this.TextBoxMode == TextBoxMode.MultiLine)
                    writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                string str = string.Format("javascript:{0}_Picker();return false;", this.ClientID);

                writer.Write(string.Format("<img style=\"cursor:hand;\" src=\"{0}\"", this.Page.ClientScript.GetWebResourceUrl(typeof(PickerBase), "iPower.Web.UI.PickerBase_Picker.gif")));
                if (this.txtText.Enabled)
                {
                    writer.Write(" onclick=\"" + str + "\"");
                }
                writer.Write(">");
                writer.RenderEndTag();
                if (this.TextBoxMode == TextBoxMode.MultiLine)
                    writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                string str2 = ((("javascript:document.all." + this.txtText.ClientID + ".value='';") + "document.all." + this.txtValue.ClientID + ".value='';") + "document.all." + this.ClientID + ".value='';") + "return false;";
                writer.Write(string.Format("<img style=\"cursor:hand;\" src=\"{0}\"", this.Page.ClientScript.GetWebResourceUrl(typeof(PickerBase), "iPower.Web.UI.PickerBase_Clear.gif")));
                if (this.txtText.Enabled)
                {
                    writer.Write(" onclick=\"" + str2 + "\"");
                }
                writer.Write(">");
                writer.RenderEndTag();
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                if (!this.AllowInput)
                    writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true", false);
                this.txtText.RenderControl(writer);
                this.txtValue.RenderControl(writer);
                writer.RenderEndTag();
            }
           
            if (this.IsRequired)
            {
                if (this.TextBoxMode == TextBoxMode.MultiLine)
                    writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                
                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}_RequiredField", this.ClientID));
                //writer.AddAttribute("controltovalidate", this.ClientID);
                writer.AddAttribute("controltovalidate", this.AllowInput ? this.txtText.ClientID : this.txtValue.ClientID);
                writer.AddAttribute("errormessage", this.ErrorMessage);
                writer.AddAttribute("isvalid", this.IsValid.ToString());
                writer.AddAttribute("evaluationfunction", string.Format("{0}_RequiredFieldValidator", this.ClientID));
                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("*");
                writer.RenderEndTag();

                writer.RenderEndTag();
            }

            writer.RenderEndTag();
            writer.RenderEndTag();
        }
        #endregion

        #region 验证成员
        /// <summary>
        /// 至少必选一项非空值。
        /// </summary>
        [Category("Valid")]
        [DefaultValue(false)]
        [Description("至少必选一项非空值。")]
        public bool IsRequired
        {
            get
            {
                object o = this.ViewState["IsRequired"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                if (this.IsRequired != value)
                    this.ViewState["IsRequired"] = value;
            }
        }
        /// <summary>
        /// 获取或设置条件验证失败时生成的错误信息。
        /// </summary>
        [Category("Valid")]
        [Description("没有选择项时的提示信息。")]
        public string ErrorMessage
        {
            get
            {
                string s = (string)this.ViewState["ErrorMessage"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.ErrorMessage != value)
                    this.ViewState["ErrorMessage"] = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，通过该值指示用户在指定控件中输入的内容是否通过验证。
        /// </summary>
        [Category("Valid")]
        [Browsable(false)]
        [DefaultValue("true")]
        public bool IsValid
        {
            get
            {
                object o = this.ViewState["IsValid"];
                return (o == null) ? true : (bool)o;
            }
            set
            {
                if (this.IsValid != value)
                    this.ViewState["IsValid"] = value;
            }
        }
        /// <summary>
        /// 计算它检查的条件并更新 IsValid 属性。
        /// </summary>
        public void Validate()
        {
            this.IsValid = true;//默认情况下验证是成功的
            //验证必选
            if (this.IsRequired)
            {
                this.IsValid = !string.IsNullOrEmpty(this.Value);
            }
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 触发数据发生改变事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler handler = this.TextChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region IPostBackEventHandler 成员
        /// <summary>
        /// 事件发回处理。
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            this.OnTextChanged(new EventArgs());
        }

        #endregion
    }
}
