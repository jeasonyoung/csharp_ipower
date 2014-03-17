//================================================================================
//  FileName: CheckBoxListEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/12/5
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
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.Collections.Specialized;
namespace iPower.Web.UI
{
    /// <summary>
    /// 创建多项选择复选框组，该复选框组可以通过将控件绑定到数据源动态创建。
	/// 扩展功能:
	///		1.支持验证是否至少选择一个选项；
	///		2.支持通过CheckedValue(2)属性来设置或获取选中的选项(String[]或StringCollection)；
    /// </summary>
    [ToolboxData("<{0}:CheckBoxListEx runat=server></{0}:CheckBoxListEx>")]
    public class CheckBoxListEx : CheckBoxList, IValidator
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CheckBoxListEx()
        {
        }
        #endregion

        /// <summary>
        /// 获取或设置是否为必选项。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置是否为必选项。")]
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

        #region 获取或设置控件当前选中的选项的值(String[]或StringCollection)
        /// <summary>
        /// 获取或设置控件当前选中的选项的值集合(StringCollection)。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        [Description("获取或设置控件当前选中的选项的值集合(StringCollection)。")]
        public StringCollection CheckedValue
        {
            get
            {
               StringCollection sc = new StringCollection();
                foreach (ListItem li in this.Items)
                {
                    if (li.Selected)
                    {
                        sc.Add(li.Value);
                    }
                }
                return sc;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    foreach (ListItem li in this.Items)
                    {
                        if (value.Contains(li.Value))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
        }
         /// <summary>
        /// 获取或设置控件当前选中的选项的值集合(数组)。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        [Description("获取或设置控件当前选中的选项的值集合(数组)。")]
        public string[] CheckedValueToArray
        {
            get
            {
                StringCollection collection = this.CheckedValue;
                if (collection != null && collection.Count > 0)
                {
                    string[] data = new string[collection.Count];
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = collection[i];
                    }
                    return data;
                }
                return null;
            }
            set
            {
                string[] data = value;
                if (data != null && data.Length > 0)
                {
                    StringCollection collection = new StringCollection();
                    for (int i = 0; i < data.Length; i++)
                    {
                        string str = data[i];
                        if (!string.IsNullOrEmpty(str) && !collection.Contains(str))
                        {
                            collection.Add(str);
                        }
                    }
                    if (collection.Count > 0)
                    {
                        this.CheckedValue = collection;
                    }
                }
            }
        }
        #endregion

        #region IValidator 成员
        /// <summary>
        /// 获取或设置条件验证失败时生成的错误信息。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置条件验证失败时生成的错误信息。")]
        public string ErrorMessage
        {
            get
            {
                object o = this.ViewState["ErrorMessage"];
                return o == null ? string.Empty : (string)o;
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
        [Description("获取或设置是否通过验证。")]
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
            {
                this.IsValid = (this.SelectedIndex > -1);
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
            if (this.Page != null)
                this.Page.Validators.Add(this);
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnUnload(EventArgs e)
        //{
        //    if (this.Page != null)
        //        this.Page.Validators.Remove(this);
        //    base.OnUnload(e);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("float", "left");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            string css = this.CssClass;
            if (!string.IsNullOrEmpty(css)) writer.AddAttribute(HtmlTextWriterAttribute.Class, css); 
            base.Render(writer);
            writer.RenderEndTag();
            if (this.IsRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}_RequiredField", this.ClientID));
                writer.AddAttribute("controltovalidate", this.ClientID);
                writer.AddAttribute("errormessage", this.ErrorMessage);
                writer.AddAttribute("isvalid", this.IsValid.ToString());
                writer.AddAttribute("evaluationfunction", string.Format("{0}_RequiredFieldValidator", this.ClientID));
                //writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
                writer.AddStyleAttribute("float", "left");
                writer.AddStyleAttribute(HtmlTextWriterStyle.MarginTop, "6px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("*");
                writer.RenderEndTag();
            }
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
                        sb.Append("\tvar olist =document.getElementById(val.controltovalidate);\r\n");

                        sb.AppendLine("\tif(olist)");
                        sb.AppendLine("\t{");
                        sb.AppendLine("\t\tvar items = olist.getElementsByTagName('input');");
                        sb.AppendLine("\t\tif(items && items.length > 0)");
                        sb.AppendLine("\t\t{");
                        sb.AppendLine("\t\t\tvar result = false;");
                        sb.AppendLine("\t\t\tfor(var i = 0; i < items.length; i++)");
                        sb.AppendLine("\t\t\t{");
                        sb.AppendLine("\t\t\t\tvar item = items[i];");
                        sb.AppendLine("\t\t\t\tif(item.type == 'checkbox' && item.checked){");
                        sb.AppendLine("\t\t\t\t\tresult = true;");
                        sb.AppendLine("\t\t\t\t\tbreak;");
                        sb.AppendLine("\t\t\t\t}");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine("\t\t\treturn result;");
                        sb.AppendLine("\t\t}");
                        sb.AppendLine("\t}");

                        sb.Append("\treturn true;\r\n");
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
        #endregion
    }
}
