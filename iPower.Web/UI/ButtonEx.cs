//================================================================================
//  FileName: ButtonEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/2/23
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
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Security.Permissions;

using System.Globalization;
using System.Threading;

using iPower.Utility;
using iPower.Web.UI.Designer;
using iPower.Web.Utility;
using iPower.Platform;
namespace iPower.Web.UI
{
    /// <summary>
    /// 在 Web 页上显示普通按钮控件。
    /// </summary>
    /// <remarks>
    ///使用 Button 控件在 Web 页上创建普通按钮。既可以创建 submit 按钮，也可以创建 command 按钮。
    ///submit 按钮不具有与按钮关联的命令名（由 CommandName 属性指定）而只是将 Web 页回发到服务器。默认情况下，Button 控件是提交按钮。可以为 Click 事件提供事件处理程序，以便以编程方式控制在单击提交按钮时所执行的操作。
    ///通过设置 CommandName 属性，command 按钮具有与该按钮关联的命令名，例如 Sort。这使您可以在一个 Web 页上创建多个 Button 控件，并以编程方式确定单击了哪个 Button 控件。您还可以将 CommandArgument 属性与命令按钮一起使用，提供有关要执行的命令的附加信息，例如 Ascending。可以为 Command 事件提供事件处理程序，以便以编程方式控制在单击 command 按钮时所执行的操作。
    ///
    ///功能：
    ///	1.客户端的提示确认框.
    ///	2.处理了页面有校验控件时不能弹出提示框的bug
    ///	3.支持弹出模式窗口，并等模式窗口的业务处理完成后再执行服务端的点击事件
    ///	4.可以定义按钮的前置、后置空格
    ///	5.可以定义服务端事件执行之前要执行的客户端事件
    ///	6.可以定义MouseOver、MouseOut的样式
    ///	7.支持在客户端校验通过后先执行指定的脚本再提交(AfterValidScript)。
    ///</remarks>
    //[DefaultProperty("ConfirmMsg"), DefaultEvent("Click"),
    [ToolboxData("<{0}:ButtonEx runat=server id='btnButtonEx' Text='ButtonEx'></{0}:ButtonEx>")]
    //[Designer(typeof(ButtonExDesigner))]
    [ToolboxBitmap(typeof(ButtonEx))]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ButtonEx : WebControl, IButtonControl, IButton, IPostBackEventHandler
    {
        #region 成员函数，构造函数。
        static Regex reg;
        TextInfo textInfo;
        /// <summary>
        /// 构造函数。
        /// </summary>
        static ButtonEx()
        {
            reg = new Regex("(?<button>[a-zA-Z]+)(Disabled|MouseOut|MouseOver)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public ButtonEx()
            : base(HtmlTextWriterTag.Input)
        {
            this.Width = new Unit("70px");
            this.CssClass = "ButtonFlat";

            this.textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置是否启用Submit提交。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置是否启用Submit提交。")]
        [DefaultValue(true)]
        [Themeable(false)]
        public virtual bool UseSubmitBehavior
        {
            get
            {
                object obj = this.ViewState["UseSubmitBehavior"];
                return obj == null ? true : (bool)obj;
            }
            set
            {
                this.ViewState["UseSubmitBehavior"] = value;
            }
        }

        /// <summary>
        /// 获取或设置在引发控件的 <see cref="Click"/> 事件时所执行的客户端脚本。
        /// </summary>
        [Category("Behavior")]
        [Description("")]
        [DefaultValue("获取或设置在引发控件的事件时所执行的客户端脚本。")]
        [Themeable(false)]
        public virtual string OnClientClick
        {
            get
            {
                object obj = this.ViewState["OnClientClick"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["OnClientClick"] = value;
            }
        }
        /// <summary>
        /// 是否显示确认消息框,默认值为False.
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [DefaultValue("false")]
        [Description("是否显示确认消息框,默认值为False.")]
        public bool ShowConfirmMsg
        {
            get
            {
                object o = this.ViewState["ShowConfirmMsg"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                if (this.ShowConfirmMsg != value)
                    this.ViewState["ShowConfirmMsg"] = value;
            }
        }
        /// <summary>
        /// 要显示的提示消息。
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("要显示的提示消息。")]
        [Themeable(false)]
        public string ConfirmMsg
        {
            get
            {
                string s = (string)this.ViewState["ConfirmMsg"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.ConfirmMsg != value)
                    this.ViewState["ConfirmMsg"] = value;
            }
        }
        /// <summary>
        /// 按钮前置空格数，默认为0。
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [DefaultValue(0)]
        [Description("按钮前置空格数，默认为0。")]
        [Themeable(false)]
        public int LeftSpace
        {
            get
            {
                object o = this.ViewState["LeftSpace"];
                return (o == null) ? 0 : Convert.ToInt32(o);
            }
            set
            {
                if (this.LeftSpace != value)
                    this.ViewState["LeftSpace"] = value;
            }
        }
        /// <summary>
        /// 按钮后置空格数，默认为0。
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [DefaultValue(0)]
        [Description("按钮后置空格数，默认为0。")]
        [Themeable(false)]
        public int RightSpace
        {
            get
            {
                object o = this.ViewState["RightSpace"];
                return (o == null) ? 0 : Convert.ToInt32(o);
            }
            set
            {
                if (this.RightSpace != value)
                    this.ViewState["RightSpace"] = value;
            }
        }
        /// <summary>
        /// 入口参数(查询参数)。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        [Themeable(false)]
        public NameValueCollection Parameters
        {
            get
            {
                object o = this.ViewState["Parameters"];
                return (o == null) ? new NameValueCollection() : (NameValueCollection)o;
            }
            set
            {
                this.ViewState["Parameters"] = value;
            }
        }
        /// <summary>
        /// 入口参数(客户端参数)。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        [Themeable(false)]
        public NameValueCollection ObjParameters
        {
            get
            {
                object o = this.ViewState["ObjParameters"];
                return (o == null) ? null : (NameValueCollection)o;
            }
            set
            {
                this.ViewState["ObjParameters"] = value;
            }
        }
        /// <summary>
        /// 返回值。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        [Themeable(false)]
        public NameValueCollection ReturnValue
        {
            get
            {
                if (this.Page != null)
                    return ConvertEx.Base64StringToNameValueCollection(this.Page.Request.Form[string.Format("{0}_ReturnValue", this.ClientID)]);
                return new NameValueCollection();
            }
        }
        /// <summary>
        /// 弹出窗口的类型。
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(EnumWindowType.Modal)]
        [Description("弹出窗口的类型。")]
        [Themeable(false)]
        public EnumWindowType PickerType
        {
            get
            {
                object o = this.ViewState["PickerType"];
                return (o == null) ? EnumWindowType.Modal : (EnumWindowType)o;
            }
            set
            {
                if (this.PickerType != value)
                    this.ViewState["PickerType"] = value;
            }
        }
        /// <summary>
        /// 弹出窗口打开的页面。
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("弹出窗口打开的页面。")]
        [Themeable(false)]
        public string PickerPage
        {
            get
            {
                string s = (string)this.ViewState["PickerPage"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.PickerPage != value)
                    this.ViewState["PickerPage"] = value;
            }
        }
        /// <summary>
        /// 获取或设置弹出窗体宽度。
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("获取或设置弹出窗体宽度。")]
        [Themeable(false)]
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
        [Themeable(false)]
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

        /// <summary>
        /// 在按钮的单击事件执行之前执行的脚本(提示框之后，客户端校验之前)。
        /// </summary>
        [Category("Scripts")]
        [Description("在按钮的单击事件执行之前执行的脚本(提示框之后，客户端校验之前。)")]
        [Themeable(false)]
        public string BeforeClickScript
        {
            get
            {
                string s = (string)this.ViewState["BeforeClickScript"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.BeforeClickScript != value)
                    this.ViewState["BeforeClickScript"] = value;
            }
        }
        /// <summary>
        /// 在按钮的校验事件后要执行的脚本。
        /// </summary>
        [Category("Scripts")]
        [Description("在按钮的校验事件后要执行的脚本。")]
        [Themeable(false)]
        public string AfterValidScript
        {
            get
            {
                string s = (string)this.ViewState["AfterValidScript"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.AfterValidScript != value)
                    this.ViewState["AfterValidScript"] = value;
            }
        }

        /// <summary>
        /// MouseOver时的按钮的样式表。
        /// </summary>
        [Category("Styles")]
        [Description("MouseOver时的按钮的样式表")]
        public string MouseOverCssClass
        {
            get
            {
                string s = (string)this.ViewState["MouseOverCssClass"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.MouseOverCssClass != value)
                    this.ViewState["MouseOverCssClass"] = value;
            }
        }
        /// <summary>
        /// MouseOut时的按钮的样式表。
        /// </summary>
        [Category("Styles")]
        [Description("MouseOut时的按钮的样式表。")]
        public string MouseOutCssClass
        {
            get
            {
                string s = (string)this.ViewState["MouseOutCssClass"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.MouseOutCssClass != value)
                    this.ViewState["MouseOutCssClass"] = value;
            }
        }
        /// <summary>
        /// 获取或设置按钮类型。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置按钮类型（Search/Add/Save/Cancel/Del/Export/Import）。")]
        [Themeable(false)]
        public string ButtonType
        {
            get
            {
                string s = (string)this.ViewState["ButtonType"];
                return string.IsNullOrEmpty(s) ? "Flat" : s;
            }
            set { this.ViewState["ButtonType"] = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 已重写。<see cref="WebControl.AddAttributesToRender"/>
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);
            bool useSubmitBehavior = this.UseSubmitBehavior;
            if (useSubmitBehavior)
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Text);

            #region 脚本处理。
            PostBackOptions postBackOptions = this.GetPostBackOptions();
            string firstScript = string.Empty;
            if (this.IsEnabled)
            {
                firstScript = Util.EnsureEndWithSemiColon(this.OnClientClick);
                if (this.HasAttributes)
                {
                    string s = this.Attributes["onclick"];
                    if (!string.IsNullOrEmpty(s))
                    {
                        firstScript += Util.EnsureEndWithSemiColon(s);
                        this.Attributes.Remove("onclick");
                    }
                }
                if (this.Page != null)
                {
                    string postBackEventReference = this.Page.ClientScript.GetPostBackEventReference(postBackOptions, false);
                    if (!string.IsNullOrEmpty(postBackEventReference))
                        firstScript = Util.MergeScript(firstScript, postBackEventReference);
                }
            }
            if (this.Page != null)
                this.Page.ClientScript.RegisterForEventValidation(postBackOptions);
            if (string.IsNullOrEmpty(firstScript))
                firstScript = string.Format("return {0}_ClientClick(this);", this.ClientID);
            else
                firstScript = Util.MergeScript(firstScript, string.Format("return {0}_ClientClick(this);", this.ClientID));
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, firstScript);
            #endregion

            #region 处理样式。
            if (!string.IsNullOrEmpty(this.ButtonType))
            {
                if (string.IsNullOrEmpty(this.MouseOutCssClass))
                    this.MouseOutCssClass = string.Format("Btn{0}MouseOut", this.textInfo.ToTitleCase(this.ButtonType));

                if (string.IsNullOrEmpty(this.MouseOverCssClass))
                    this.MouseOverCssClass = string.Format("Btn{0}MouseOver", this.textInfo.ToTitleCase(this.ButtonType));
            }

            if (!string.IsNullOrEmpty(this.MouseOutCssClass) && !string.IsNullOrEmpty(this.MouseOverCssClass))
            {
                this.CssClass = this.MouseOutCssClass;
                if (!this.Enabled)
                {
                    Match match = reg.Match(this.CssClass);
                    if (match.Success)
                        this.CssClass = match.Result("${button}") + "Disabled";
                }

                writer.AddAttribute("language", "javascript");
                writer.AddAttribute("onmouseover",
                    string.Format("this.className='{0}';", this.MouseOverCssClass));
                writer.AddAttribute("onmouseout",
                    string.Format("this.className='{0}';", this.MouseOutCssClass));
            }
            #endregion

            if (this.Enabled && !this.IsEnabled)
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");

            base.AddAttributesToRender(writer);
        }
        /// <summary>
        /// 已重载。
        /// </summary>
        /// <param name="e">包含事件数据的 EventArgs 对象。</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            #region 脚本输出。
            ClientScriptManager clientScript = this.Page.ClientScript;
            string scriptKey = string.Format("{0}_ClientClick", this.ClientID);
            if (!clientScript.IsClientScriptBlockRegistered(scriptKey))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\r\n<script language=\"javascript\">\r\n");
                sb.AppendFormat("function {0}_ClientClick(btn)\r\n", this.ClientID);
                sb.Append("{\r\n");

                //是否显示提示信息
                if (this.ShowConfirmMsg && !string.IsNullOrEmpty(this.ConfirmMsg))
                {
                    sb.AppendFormat("\tif(!confirm('{0}'))\r\n\t", this.ConfirmMsg);
                    sb.Append("{\r\n\t\treturn false;\r\n\t}\r\n");
                }
                //提前执行的脚本
                if (!string.IsNullOrEmpty(this.BeforeClickScript))
                    sb.AppendFormat("\t{0}\r\n", this.BeforeClickScript);
                //有客户端校验
                if (this.CausesValidation && this.Page.Validators.Count > 0)
                {
                    sb.Append("\tif(typeof(Page_ClientValidate) == 'function')\r\n");
                    sb.Append("\t{\r\n");
                    sb.Append("\t\tif(Page_ClientValidate())\r\n");
                    sb.Append("\t\t{\r\n");
                    //校验后执行的脚本
                    if (!string.IsNullOrEmpty(this.AfterValidScript))
                        sb.AppendFormat("\t\t\t{0}\r\n", this.AfterValidScript);
                    sb.Append("\t\t}\r\n");
                    sb.Append("\t\telse\r\n");
                    sb.Append("\t\t\treturn false ;\r\n");
                    sb.Append("\t}\r\n\r\n");
                }
                else if (!string.IsNullOrEmpty(this.AfterValidScript))
                    sb.AppendFormat("\t{0}\r\n", this.AfterValidScript);

                //弹出窗口
                if (!string.IsNullOrEmpty(this.PickerPage))
                {
                    if (this.PickerType == EnumWindowType.Modal)//模式窗口
                    {
                        sb.Append("\tvar sReturn,vTmd=Math.random();\r\n");

                        //客户端参数
                        if (this.ObjParameters != null)
                        {
                            sb.AppendFormat("\tvar vObj='{0}';\r\n", ConvertEx.NameValueCollectionToBase64String(this.ObjParameters));
                        }

                        sb.AppendFormat("\tsReturn=window.showModalDialog(\"{0}{1}tmd=\"+vTmd", this.PickerPage,
                                this.PickerPage.IndexOf("?") > -1 ? "&" : "?");
                        //构造查询参数
                        if (this.Parameters != null && this.Parameters.Count > 0)
                        {
                            sb.Append("+\"");
                            NameValueCollection hash = this.Parameters;
                            foreach (string key in hash.Keys)
                            {
                                sb.AppendFormat("&{0}={1}", key, hash[key]);
                            }
                            sb.Append("\"");
                        }
                        sb.AppendFormat(",{0},\"dialogWidth:{1};dialogHeight:{2};help:0\");\r\n",
                            this.ObjParameters != null ? "vObj" : "window",
                            this.PickerWidth, this.PickerHeight);//

                        //sb.Append("alert(sReturn);\r\n");
                        sb.Append("\tif(typeof(sReturn)!=\"undefined\" && sReturn!=\"\")\r\n");
                        sb.Append("\t{\r\n");
                        sb.AppendFormat("\t\teval(\"document.all.{0}_ReturnValue.value='\"+sReturn+\"'\");\r\n", this.ClientID);
                        sb.Append("\t\treturn true;\r\n");
                        sb.Append("\t}\r\n");
                        sb.Append("\telse\r\n");
                        sb.Append("\t{\r\n");
                        sb.Append("\t\treturn false;\r\n");
                        sb.Append("\t}\r\n");
                    }
                    else//普通窗口
                    {
                        sb.AppendFormat("\twindow.open(\"{0}\",null,\"width={1},height={2},resizable=no,scrollbars=no,status=no,toolbar=no,menubar=no,location=no\");\r\n",
                            this.PickerPage, this.PickerWidth, this.PickerHeight);
                    }
                }
                sb.Append("\treturn true;\r\n");//在没有其它客户端动作时总是返回true

                sb.Append("}\r\n");
                sb.Append("</script>");

                clientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, sb.ToString());
            }
            #endregion

        }

        /// <summary> 
        /// 将此控件呈现给指定的输出参数。
        /// </summary>
        /// <param name="writer"> 要写出到的 HTML 编写器。 </param>
        protected override void Render(HtmlTextWriter writer)
        {
            HtmlInputHidden hidInput = new HtmlInputHidden();
            hidInput.ID = string.Format("{0}_ReturnValue", this.ClientID);
            hidInput.Name = string.Format("{0}_ReturnValue", this.ClientID);
            hidInput.RenderControl(writer);

            writer.AddAttribute("unselectable", "on");
            //加上前置空格
            if (this.LeftSpace > 0)
                writer.Write(ConvertEx.ReplicateString("&nbsp;", this.LeftSpace));
            base.Render(writer);
            //加上后置空格数
            if (this.RightSpace > 0)
                writer.Write(ConvertEx.ReplicateString("&nbsp;", this.RightSpace));
        }
        /// <summary>
        /// 创建表示控件的回发行为的 <see cref="PostBackOptions"/> 对象。
        /// </summary>
        /// <returns></returns>
        protected virtual PostBackOptions GetPostBackOptions()
        {
            PostBackOptions options = new PostBackOptions(this, string.Empty);
            options.ClientSubmit = false;
            if (this.Page != null)
            {
                if (this.CausesValidation && !string.IsNullOrEmpty(this.ValidationGroup) && (this.Page.GetValidators(this.ValidationGroup).Count > 0))
                {
                    options.PerformValidation = true;
                    options.ValidationGroup = this.ValidationGroup;
                }

                if (!string.IsNullOrEmpty(this.PostBackUrl))
                    options.ActionUrl = HttpUtility.UrlPathEncode(this.ResolveClientUrl(this.PostBackUrl));
                options.ClientSubmit = !this.UseSubmitBehavior;
            }
            return options;
        }
        #endregion
        
        #region IButtonControl 成员
        /// <summary>
        /// 获取或设置一个值，该值指示在单击<see cref="ButtonEx"/> 控件时是否执行了验证。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置一个值，该值指示在单击控件时是否执行了验证。")]
        [DefaultValue(false)]
        public bool CausesValidation
        {
            get
            {
                object o = this.ViewState["CausesValidation"];
                return o == null ? false : (bool)o;
            }
            set
            {
                this.ViewState["CausesValidation"] = value;
            }
        }
        /// <summary>
        /// 获取或设置命令名，该命令名与传递给 Command 事件的 Button 控件相关联。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置命令名，该命令名与传递给 Command 事件的 Button 控件相关联。")]
        public string CommandName
        {
            get
            {
                string s = (string)this.ViewState["CommandName"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["CommandName"] = value;
            }
        }
        /// <summary>
        /// 获取或设置可选参数，该参数与关联的 CommandName 一起被传递到 Command 事件。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置可选参数，该参数与关联的 CommandName 一起被传递到 Command 事件。")]
        public string CommandArgument
        {
            get
            {
                string s = (string)this.ViewState["CommandArgument"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["CommandArgument"] = value;
            }
        }
        /// <summary>
        /// 获取或设置单击<see cref="ButtonEx"/> 控件时从当前页发送到的网页的 URL。
        /// </summary>
        [Category("Behavior")]
        [DefaultValue("")]
        [Description("获取或设置单击控件时从当前页发送到的网页的 URL。")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [UrlProperty("*.aspx")]
        [Themeable(false)]
        public virtual string PostBackUrl
        {
            get
            {
                object obj = this.ViewState["PostBackUrl"];
                return obj == null ? string.Empty : (string)obj;

            }
            set
            {
                this.ViewState["PostBackUrl"] = value;
            }
        }
        /// <summary>
        /// 获取或设置在 Button 控件中显示的文本标题。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置在 Button 控件中显示的文本标题。")]
        public string Text
        {
            get
            {
                string s = (string)this.ViewState["Text"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["Text"] = value;
            }
        }
        /// <summary>
        /// 获取或设置在控件回发到服务器时要进行验证的控件组。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置在控件回发到服务器时要进行验证的控件组。")]
        [DefaultValue("")]
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
        /// <summary>
        /// 在单击Button控件时发生。
        /// </summary>
        [Category("Action")]
        [Description("在单击Button控件时发生。")]
        public event EventHandler Click;
        /// <summary>
        /// 在单击Button控件时发生
        /// </summary>
        [Category("Action")]
        [Description("在单击Button控件时发生")]
        public event CommandEventHandler Command;
        #endregion

        #region 事件注册函数。
        /// <summary>
        ///  引发 Button 控件的 Click 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected virtual void OnClick(EventArgs e)
        {
            EventHandler handler = this.Click;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 引发 Button 控件的 Command 事件。
        /// </summary>
        /// <param name="e">引发 Button 控件的 Command 事件。</param>
        protected virtual void OnCommand(CommandEventArgs e)
        {
            CommandEventHandler handler = this.Command;
            if (handler != null)
                handler(this, e);
            base.RaiseBubbleEvent(this, e);
        }
        #endregion

        #region IButton 成员。
        /// <summary>
        /// 获取或设置一个值，该值指示是否启用服务器控件。
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                Match match = reg.Match(this.CssClass);
                if (match.Success)
                {
                    if (value)
                        this.CssClass = match.Result("${button}") + "MouseOut";
                    else
                        this.CssClass = match.Result("${button}") + "Disabled";
                }
            }
        }
        #endregion

        #region IPostBackEventHandler 成员
        /// <summary>
        /// 使服务器控件能够处理将窗体发送到服务器时引发的事件。
        /// </summary>
        /// <param name="eventArgument"> 表示要传递到事件处理程序的可选事件参数。</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (this.Page != null && this.CausesValidation)
            {
                if (!string.IsNullOrEmpty(this.ValidationGroup))
                    this.Page.Validate(this.ValidationGroup);

                if (this.Page.Validators.Count > 0)
                    this.Page.Validate();
            }
            this.OnClick(new EventArgs());
            this.OnCommand(new CommandEventArgs(this.CommandName, this.CommandArgument));
        }

        #endregion
    }
}
