//================================================================================
//  FileName: TextBoxCalendar.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/14
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
using System.Text.RegularExpressions;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;

[assembly: WebResource("iPower.Web.UI.TextBoxCalendar.js", "text/javascript")]
namespace iPower.Web.UI
{
    /// <summary>
    /// 为用户提供一个输入日期的控件。
    /// </summary>
    [ToolboxData("<{0}:TextBoxCalendar runat='server'></{0}:TextBoxCalendar>")]
    public class TextBoxCalendar : WebControl, IPostBackDataHandler, IEditableTextControl, ITextControl, INamingContainer, IValidator
    {
        #region 成员变量，构造函数。
        const string const_regex_str = @"^(\d{4})(-)(01|02|03|04|05|06|07|08|09|10|11|12)(-)([0-3]?\d)$";
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TextBoxCalendar()
        {
        }
        #endregion

        #region 属性。
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
        /// 获取或设置控件是否是不可编辑的。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置控件是否是不可编辑的。")]
        [DefaultValue("false")]
        [Bindable(true)]
        public bool Readonly
        {
            get
            {
                object obj = this.ViewState["Readonly"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                this.ViewState["Readonly"] = value;
            }
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
                return HtmlTextWriterTag.Div;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string ClientID
        {
            get
            {
                return string.Format("{0}_host", base.ClientID);
            }
        }
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
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("float", "left");
            base.AddAttributesToRender(writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            ClientScriptManager scriptManage = this.Page.ClientScript;
            if (scriptManage != null)
            {
                #region 验证非空。
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
                    string strScript = string.Format("{0}_RequiredFieldValidator", base.ClientID);
                    if (!scriptManage.IsClientScriptBlockRegistered(strScript))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type=\"text/javascript\">\r\n");
                        sb.Append("<!--\r\n");
                        sb.AppendFormat("function {0}_RequiredFieldValidator(val)\r\n", base.ClientID);
                        sb.Append("{\r\n");
                        sb.Append("\tvar inputText=document.getElementById(val.controltovalidate);\r\n");
                        sb.Append("\tif (inputText.value == \"\")\r\n");
                        sb.Append("\t\treturn false;\r\n");

                        sb.AppendFormat("\treturn TextBoxCalendar_Onblur('{0}','{1:yyyy-MM-dd}');\r\n", base.ClientID, DateTime.Now);
                        sb.Append("}\r\n");
                        sb.Append("// -->\r\n");
                        sb.Append("</script>\r\n");

                        scriptManage.RegisterArrayDeclaration("Page_Validators", string.Format("document.getElementById(\"{0}_RequiredField\")", base.ClientID));
                        scriptManage.RegisterClientScriptBlock(this.GetType(), strScript, sb.ToString());
                    }
                }
                #endregion

                #region 注册脚本。
                scriptManage.RegisterClientScriptResource(typeof(TextBoxCalendar), "iPower.Web.UI.TextBoxCalendar.js");
                #endregion
            }
            base.OnPreRender(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            #region 输入框。
            HtmlGenericControl inputDiv = new HtmlGenericControl("div");
            inputDiv.Attributes["style"] = "float:left; width:100%";
            inputDiv.ID = string.Format("{0}_InputParent", base.ClientID);

            Unit w = this.Width;
            HtmlInputText input = new HtmlInputText("text");
            input.ID = base.UniqueID;
            input.Attributes["class"] = "TextBoxFlat";
            if (this.Readonly)
                input.Attributes["readonly"] = "readonly";
            else
            {
                w = new Unit(w.Value - 25);
                if (this.IsRequired)
                    w = new Unit(w.Value - 5);
            }
            input.Attributes["style"] = string.Format("float:left;width:{0}", w);
            string strText = this.Text;
            if (string.IsNullOrEmpty(strText))
                strText = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            input.Value = string.Format("{0:yyyy-MM-dd}", strText);
            inputDiv.Controls.Add(input);
            if (!this.Readonly)
            {
                input.Attributes["onfocus"] = string.Format("javascript:TextBoxCalendar_Show('{0}',true);", base.ClientID);
                string defaultTime = this.Text;
                if (string.IsNullOrEmpty(defaultTime))
                    defaultTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                input.Attributes["onblur"] = string.Format("javascript:TextBoxCalendar_Onblur('{0}','{1}');", base.ClientID, defaultTime);

                HtmlGenericControl img = new HtmlGenericControl("img");
                img.Attributes["style"] = "float:left; width:18px;cursor:hand;";
                img.Attributes["src"] = this.Page.ClientScript.GetWebResourceUrl(typeof(TextBoxCalendar), "iPower.Web.UI.PickerBase_Clear.gif");
                img.Attributes["onclick"] = string.Format("javascript:TextBoxCalendar_Clean('{0}');", base.ClientID);
                img.Attributes["alt"] = "清空";
                inputDiv.Controls.Add(img);

                if (this.IsRequired)
                {
                    HtmlGenericControl span = new HtmlGenericControl("span");
                    span.ID = string.Format("{0}_RequiredField", base.ClientID);
                    span.Attributes["style"] = "float:left;width:3px;color:red;";
                    span.Attributes["controltovalidate"] = string.Format("{0}_InputText", base.ClientID);
                    span.Attributes["errormessage"] = this.ErrorMessage;
                    span.Attributes["isvalid"] = this.IsValid.ToString();
                    span.Attributes["evaluationfunction"] = string.Format("{0}_RequiredFieldValidator", base.ClientID);

                    span.InnerText = "*";
                    inputDiv.Controls.Add(span);
                }
            }
            inputDiv.RenderControl(writer);
            #endregion

            #region 日历框。
            HtmlGenericControl calenderDiv = new HtmlGenericControl("div");
            calenderDiv.ID = string.Format("{0}_CalenLayer", base.ClientID);
            calenderDiv.Attributes["style"] = string.Format("position:absolute;width:178px;display:none;background-color:#fff;z-index:9999;border:solid 1px #fff;");
            //position:absolute;margin-top:24px;
            calenderDiv.RenderControl(writer);
            #endregion
        }
        #endregion

        #region IPostBackDataHandler 成员
        /// <summary>
        /// 处理回发数据。
        /// </summary>
        /// <param name="postDataKey">控件的主要标识符。 </param>
        /// <param name="postCollection">所有传入名称值的集合。</param>
        /// <returns>如果服务器控件的状态由于回发而发生更改，则为 true；否则为 false。</returns>
        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string strText = postCollection[postDataKey];
            if (!this.Readonly && !string.IsNullOrEmpty(strText))
            {
                this.Text = strText;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 用信号要求服务器控件对象通知 ASP.NET 应用程序该控件的状态已更改。
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            this.OnTextChanged(EventArgs.Empty);
        }

        #endregion

        //#region IPostBackEventHandler 成员
        ///// <summary>
        ///// 使服务器控件能够处理将窗体发送到服务器时引发的事件。
        ///// </summary>
        ///// <param name="eventArgument">传递到事件处理程序的可选事件参数。</param>
        //public void RaisePostBackEvent(string eventArgument)
        //{
        //    this.OnTextChanged(EventArgs.Empty);
        //}

        //#endregion

        #region IEditableTextControl 成员
        /// <summary>
        /// 当文本的内容在向服务器的发送操作之间更改时发生。 
        /// </summary>
        public event EventHandler TextChanged;
        /// <summary>
        /// 触发<see cref="TextChanged"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected void OnTextChanged(EventArgs e)
        {
            EventHandler handler = this.TextChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region ITextControl 成员
        /// <summary>
        /// 获取或设置控件的文本内容。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置控件的文本内容。")]
        public string Text
        {
            get
            {
                object obj = this.ViewState["Text"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                if (this.Text != value)
                {
                    this.ViewState["Text"] = value;
                    this.OnTextChanged(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region IValidator 成员
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
        /// 获取或设置条件验证失败时生成的错误消息。
        /// </summary>
        [Category("Valid")]
        [Description("获取或设置条件验证失败时生成的错误消息。")]
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
        /// 获取或设置用户在指定控件中输入的内容是否通过验证。
        /// </summary>
        [Browsable(false)]
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
        /// 将计算它检查的条件，然后更新 <see cref="IsValid"/> 属性。
        /// </summary>
        public void Validate()
        {
            this.IsValid = true;//默认情况下验证是成功的
            //验证必选
            if (this.IsRequired)
                this.IsValid = !string.IsNullOrEmpty(this.Text);
            if (this.IsValid)
            {
                Regex r = new Regex(const_regex_str, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                this.IsValid = r.IsMatch(this.Text);
            }
        }

        #endregion
    }
}
