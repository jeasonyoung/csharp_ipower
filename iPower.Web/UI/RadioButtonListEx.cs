using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.Text;
using System.Drawing;

namespace iPower.Web.UI
{
    /// <summary>
    /// 单选框列表。
    /// </summary>
    [ToolboxData("<{0}:RadioButtonListEx runat=server id=rdListBox/>")]
    [ToolboxBitmap(typeof(RadioButtonListEx))]
    public class RadioButtonListEx : RadioButtonList, IValidator
    {
        #region 属性。
        /// <summary>
        /// 获取或设置是否必选。
        /// </summary>
        [Category("Valid")]
        [DefaultValue(true)]
        [Description("获取或设置是否必选。")]
        public bool IsRequired
        {
            get
            {
                object obj = this.ViewState["IsRequired"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                this.ViewState["IsRequired"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否启用客户端校验。
        /// </summary>
        [Category("Valid")]
        [DefaultValue(true)]
        [Description("获取或设置是否启用客户端校验。")]
        public bool EnableClientScript
        {
            get
            {
                object obj = this.ViewState["EnableClientScript"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                this.ViewState["EnableClientScript"] = value;
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.Validators.Add(this);
        }
        /// <summary>
        /// 
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
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.IsRequired)
            {
                ClientScriptManager clientManager = this.Page.ClientScript;
                if (clientManager != null)
                {
                    const string validatorResourceKey = "ValidatorIncludeScript";
                    const string ValidatorIncludeScriptKey = "ValidatorIncludeScript";
                    const string ValidatorOnSubmitKey = "ValidatorOnSubmit";
                    if (!clientManager.IsClientScriptBlockRegistered(typeof(BaseValidator), validatorResourceKey))
                    {
                        clientManager.RegisterClientScriptResource(typeof(BaseValidator), "WebUIValidation.js");
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
                        clientManager.RegisterStartupScript(typeof(BaseValidator), ValidatorIncludeScriptKey, ValidatorStartupScript.ToString());

                        string ValidatorOnSubmitScript = "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;";
                        clientManager.RegisterOnSubmitStatement(typeof(BaseValidator), ValidatorOnSubmitKey, ValidatorOnSubmitScript);
                    }
                    string strScript = string.Format("{0}_RequiredFieldValidator", this.ClientID);
                    if (!clientManager.IsClientScriptBlockRegistered(strScript))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type=\"text/javascript\">\r\n");
                        sb.Append("<!--\r\n");
                        sb.AppendFormat("function {0}_RequiredFieldValidator(val)\r\n", this.ClientID);
                        sb.Append("{\r\n");
                        sb.Append("\tvar bCheck=false;\r\n");
                        sb.Append("\tvar oRadioButtonList=document.getElementById(val.controltovalidate);\r\n");
                        sb.Append("\tvar oRadioButtons=oRadioButtonList.getElementsByTagName(\"input\");\r\n");
                        sb.Append("\tfor(var i=0;i<oRadioButtons.length;i++){\r\n");
                        sb.Append("\t\tif(oRadioButtons[i].type==\"radio\" && oRadioButtons[i].checked){\r\n");
                        sb.Append("\t\t\tbCheck=true;\r\n");
                        sb.Append("\t\t\treturn bCheck;\r\n");
                        sb.Append("\t\t}\r\n");
                        sb.Append("\t}\r\n");

                        sb.Append("\treturn bCheck;\r\n");
                        sb.Append("}\r\n");
                        sb.Append("// -->\r\n");
                        sb.Append("</script>\r\n");

                        clientManager.RegisterArrayDeclaration("Page_Validators", string.Format("document.getElementById(\"{0}_RequiredField\")", this.ClientID));
                        clientManager.RegisterClientScriptBlock(this.GetType(), strScript, sb.ToString());
                    }
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
            writer.AddStyleAttribute("float", "left");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (!string.IsNullOrEmpty(this.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
            }
            base.Render(writer);
            writer.RenderEndTag();
            if (this.IsRequired && this.EnableClientScript)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}_RequiredField", this.ClientID));
                writer.AddAttribute("controltovalidate", this.ClientID);
                writer.AddAttribute("errormessage", this.ErrorMessage);
                writer.AddAttribute("isvalid", this.IsValid.ToString());
                writer.AddAttribute("evaluationfunction", string.Format("{0}_RequiredFieldValidator", this.ClientID));
                //writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
                writer.AddStyleAttribute("float", "left");
                writer.AddStyleAttribute(HtmlTextWriterStyle.MarginTop, "5px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("*");
                writer.RenderEndTag();
            }
        }
        #endregion

        #region IValidator 成员
        /// <summary>
        /// 获取或设置选择项没有被选择时的提示信息。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置选择项没有被选择时的提示信息。")]
        public string ErrorMessage
        {
            get
            {
                object obj = this.ViewState["ErrorMessage"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["ErrorMessage"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否通过验证。
        /// </summary>
        [Category("Valid")]
        public bool IsValid
        {
            get
            {
                object obj = this.ViewState["IsValid"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                this.ViewState["IsValid"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Validate()
        {
            this.IsValid = true;
            if (this.IsRequired)
                this.IsValid = this.SelectedIndex > -1;
        }

        #endregion
    }
}
