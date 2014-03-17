//================================================================================
//  FileName: ButtonEx.cs
//  Desc:
//
//  Called by
//
//  Auth:���£�jeason1914@gmail.com��
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
    /// �� Web ҳ����ʾ��ͨ��ť�ؼ���
    /// </summary>
    /// <remarks>
    ///ʹ�� Button �ؼ��� Web ҳ�ϴ�����ͨ��ť���ȿ��Դ��� submit ��ť��Ҳ���Դ��� command ��ť��
    ///submit ��ť�������밴ť���������������� CommandName ����ָ������ֻ�ǽ� Web ҳ�ط�����������Ĭ������£�Button �ؼ����ύ��ť������Ϊ Click �¼��ṩ�¼���������Ա��Ա�̷�ʽ�����ڵ����ύ��ťʱ��ִ�еĲ�����
    ///ͨ������ CommandName ���ԣ�command ��ť������ð�ť������������������ Sort����ʹ��������һ�� Web ҳ�ϴ������ Button �ؼ������Ա�̷�ʽȷ���������ĸ� Button �ؼ����������Խ� CommandArgument ���������ťһ��ʹ�ã��ṩ�й�Ҫִ�е�����ĸ�����Ϣ������ Ascending������Ϊ Command �¼��ṩ�¼���������Ա��Ա�̷�ʽ�����ڵ��� command ��ťʱ��ִ�еĲ�����
    ///
    ///���ܣ�
    ///	1.�ͻ��˵���ʾȷ�Ͽ�.
    ///	2.������ҳ����У��ؼ�ʱ���ܵ�����ʾ���bug
    ///	3.֧�ֵ���ģʽ���ڣ�����ģʽ���ڵ�ҵ������ɺ���ִ�з���˵ĵ���¼�
    ///	4.���Զ��尴ť��ǰ�á����ÿո�
    ///	5.���Զ��������¼�ִ��֮ǰҪִ�еĿͻ����¼�
    ///	6.���Զ���MouseOver��MouseOut����ʽ
    ///	7.֧���ڿͻ���У��ͨ������ִ��ָ���Ľű����ύ(AfterValidScript)��
    ///</remarks>
    //[DefaultProperty("ConfirmMsg"), DefaultEvent("Click"),
    [ToolboxData("<{0}:ButtonEx runat=server id='btnButtonEx' Text='ButtonEx'></{0}:ButtonEx>")]
    //[Designer(typeof(ButtonExDesigner))]
    [ToolboxBitmap(typeof(ButtonEx))]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ButtonEx : WebControl, IButtonControl, IButton, IPostBackEventHandler
    {
        #region ��Ա���������캯����
        static Regex reg;
        TextInfo textInfo;
        /// <summary>
        /// ���캯����
        /// </summary>
        static ButtonEx()
        {
            reg = new Regex("(?<button>[a-zA-Z]+)(Disabled|MouseOut|MouseOver)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// ���캯����
        /// </summary>
        public ButtonEx()
            : base(HtmlTextWriterTag.Input)
        {
            this.Width = new Unit("70px");
            this.CssClass = "ButtonFlat";

            this.textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
        }
        #endregion

        #region ���ԡ�
        /// <summary>
        /// ��ȡ�������Ƿ�����Submit�ύ��
        /// </summary>
        [Category("Behavior")]
        [Description("��ȡ�������Ƿ�����Submit�ύ��")]
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
        /// ��ȡ�������������ؼ��� <see cref="Click"/> �¼�ʱ��ִ�еĿͻ��˽ű���
        /// </summary>
        [Category("Behavior")]
        [Description("")]
        [DefaultValue("��ȡ�������������ؼ����¼�ʱ��ִ�еĿͻ��˽ű���")]
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
        /// �Ƿ���ʾȷ����Ϣ��,Ĭ��ֵΪFalse.
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [DefaultValue("false")]
        [Description("�Ƿ���ʾȷ����Ϣ��,Ĭ��ֵΪFalse.")]
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
        /// Ҫ��ʾ����ʾ��Ϣ��
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("Ҫ��ʾ����ʾ��Ϣ��")]
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
        /// ��ťǰ�ÿո�����Ĭ��Ϊ0��
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [DefaultValue(0)]
        [Description("��ťǰ�ÿո�����Ĭ��Ϊ0��")]
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
        /// ��ť���ÿո�����Ĭ��Ϊ0��
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [DefaultValue(0)]
        [Description("��ť���ÿո�����Ĭ��Ϊ0��")]
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
        /// ��ڲ���(��ѯ����)��
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
        /// ��ڲ���(�ͻ��˲���)��
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
        /// ����ֵ��
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
        /// �������ڵ����͡�
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(EnumWindowType.Modal)]
        [Description("�������ڵ����͡�")]
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
        /// �������ڴ򿪵�ҳ�档
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("�������ڴ򿪵�ҳ�档")]
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
        /// ��ȡ�����õ��������ȡ�
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("��ȡ�����õ��������ȡ�")]
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
        /// ��ȡ�����õ�������߶ȡ�
        /// </summary>
        [Category("Appearance")]
        [Bindable(true)]
        [Description("��ȡ�����õ�������߶ȡ�")]
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
        /// �ڰ�ť�ĵ����¼�ִ��֮ǰִ�еĽű�(��ʾ��֮�󣬿ͻ���У��֮ǰ)��
        /// </summary>
        [Category("Scripts")]
        [Description("�ڰ�ť�ĵ����¼�ִ��֮ǰִ�еĽű�(��ʾ��֮�󣬿ͻ���У��֮ǰ��)")]
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
        /// �ڰ�ť��У���¼���Ҫִ�еĽű���
        /// </summary>
        [Category("Scripts")]
        [Description("�ڰ�ť��У���¼���Ҫִ�еĽű���")]
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
        /// MouseOverʱ�İ�ť����ʽ��
        /// </summary>
        [Category("Styles")]
        [Description("MouseOverʱ�İ�ť����ʽ��")]
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
        /// MouseOutʱ�İ�ť����ʽ��
        /// </summary>
        [Category("Styles")]
        [Description("MouseOutʱ�İ�ť����ʽ��")]
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
        /// ��ȡ�����ð�ť���͡�
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ�����ð�ť���ͣ�Search/Add/Save/Cancel/Del/Export/Import����")]
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

        #region ����
        /// <summary>
        /// ����д��<see cref="WebControl.AddAttributesToRender"/>
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

            #region �ű�����
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

            #region ������ʽ��
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
        /// �����ء�
        /// </summary>
        /// <param name="e">�����¼����ݵ� EventArgs ����</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            #region �ű������
            ClientScriptManager clientScript = this.Page.ClientScript;
            string scriptKey = string.Format("{0}_ClientClick", this.ClientID);
            if (!clientScript.IsClientScriptBlockRegistered(scriptKey))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\r\n<script language=\"javascript\">\r\n");
                sb.AppendFormat("function {0}_ClientClick(btn)\r\n", this.ClientID);
                sb.Append("{\r\n");

                //�Ƿ���ʾ��ʾ��Ϣ
                if (this.ShowConfirmMsg && !string.IsNullOrEmpty(this.ConfirmMsg))
                {
                    sb.AppendFormat("\tif(!confirm('{0}'))\r\n\t", this.ConfirmMsg);
                    sb.Append("{\r\n\t\treturn false;\r\n\t}\r\n");
                }
                //��ǰִ�еĽű�
                if (!string.IsNullOrEmpty(this.BeforeClickScript))
                    sb.AppendFormat("\t{0}\r\n", this.BeforeClickScript);
                //�пͻ���У��
                if (this.CausesValidation && this.Page.Validators.Count > 0)
                {
                    sb.Append("\tif(typeof(Page_ClientValidate) == 'function')\r\n");
                    sb.Append("\t{\r\n");
                    sb.Append("\t\tif(Page_ClientValidate())\r\n");
                    sb.Append("\t\t{\r\n");
                    //У���ִ�еĽű�
                    if (!string.IsNullOrEmpty(this.AfterValidScript))
                        sb.AppendFormat("\t\t\t{0}\r\n", this.AfterValidScript);
                    sb.Append("\t\t}\r\n");
                    sb.Append("\t\telse\r\n");
                    sb.Append("\t\t\treturn false ;\r\n");
                    sb.Append("\t}\r\n\r\n");
                }
                else if (!string.IsNullOrEmpty(this.AfterValidScript))
                    sb.AppendFormat("\t{0}\r\n", this.AfterValidScript);

                //��������
                if (!string.IsNullOrEmpty(this.PickerPage))
                {
                    if (this.PickerType == EnumWindowType.Modal)//ģʽ����
                    {
                        sb.Append("\tvar sReturn,vTmd=Math.random();\r\n");

                        //�ͻ��˲���
                        if (this.ObjParameters != null)
                        {
                            sb.AppendFormat("\tvar vObj='{0}';\r\n", ConvertEx.NameValueCollectionToBase64String(this.ObjParameters));
                        }

                        sb.AppendFormat("\tsReturn=window.showModalDialog(\"{0}{1}tmd=\"+vTmd", this.PickerPage,
                                this.PickerPage.IndexOf("?") > -1 ? "&" : "?");
                        //�����ѯ����
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
                    else//��ͨ����
                    {
                        sb.AppendFormat("\twindow.open(\"{0}\",null,\"width={1},height={2},resizable=no,scrollbars=no,status=no,toolbar=no,menubar=no,location=no\");\r\n",
                            this.PickerPage, this.PickerWidth, this.PickerHeight);
                    }
                }
                sb.Append("\treturn true;\r\n");//��û�������ͻ��˶���ʱ���Ƿ���true

                sb.Append("}\r\n");
                sb.Append("</script>");

                clientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, sb.ToString());
            }
            #endregion

        }

        /// <summary> 
        /// ���˿ؼ����ָ�ָ�������������
        /// </summary>
        /// <param name="writer"> Ҫд������ HTML ��д���� </param>
        protected override void Render(HtmlTextWriter writer)
        {
            HtmlInputHidden hidInput = new HtmlInputHidden();
            hidInput.ID = string.Format("{0}_ReturnValue", this.ClientID);
            hidInput.Name = string.Format("{0}_ReturnValue", this.ClientID);
            hidInput.RenderControl(writer);

            writer.AddAttribute("unselectable", "on");
            //����ǰ�ÿո�
            if (this.LeftSpace > 0)
                writer.Write(ConvertEx.ReplicateString("&nbsp;", this.LeftSpace));
            base.Render(writer);
            //���Ϻ��ÿո���
            if (this.RightSpace > 0)
                writer.Write(ConvertEx.ReplicateString("&nbsp;", this.RightSpace));
        }
        /// <summary>
        /// ������ʾ�ؼ��Ļط���Ϊ�� <see cref="PostBackOptions"/> ����
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
        
        #region IButtonControl ��Ա
        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�ڵ���<see cref="ButtonEx"/> �ؼ�ʱ�Ƿ�ִ������֤��
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ������һ��ֵ����ֵָʾ�ڵ����ؼ�ʱ�Ƿ�ִ������֤��")]
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
        /// ��ȡ�����������������������봫�ݸ� Command �¼��� Button �ؼ��������
        /// </summary>
        [Category("Behavior")]
        [Description("��ȡ�����������������������봫�ݸ� Command �¼��� Button �ؼ��������")]
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
        /// ��ȡ�����ÿ�ѡ�������ò���������� CommandName һ�𱻴��ݵ� Command �¼���
        /// </summary>
        [Category("Behavior")]
        [Description("��ȡ�����ÿ�ѡ�������ò���������� CommandName һ�𱻴��ݵ� Command �¼���")]
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
        /// ��ȡ�����õ���<see cref="ButtonEx"/> �ؼ�ʱ�ӵ�ǰҳ���͵�����ҳ�� URL��
        /// </summary>
        [Category("Behavior")]
        [DefaultValue("")]
        [Description("��ȡ�����õ����ؼ�ʱ�ӵ�ǰҳ���͵�����ҳ�� URL��")]
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
        /// ��ȡ�������� Button �ؼ�����ʾ���ı����⡣
        /// </summary>
        [Category("Appearance")]
        [Description("��ȡ�������� Button �ؼ�����ʾ���ı����⡣")]
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
        /// ��ȡ�������ڿؼ��ط���������ʱҪ������֤�Ŀؼ��顣
        /// </summary>
        [Category("Behavior")]
        [Description("��ȡ�������ڿؼ��ط���������ʱҪ������֤�Ŀؼ��顣")]
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
        /// �ڵ���Button�ؼ�ʱ������
        /// </summary>
        [Category("Action")]
        [Description("�ڵ���Button�ؼ�ʱ������")]
        public event EventHandler Click;
        /// <summary>
        /// �ڵ���Button�ؼ�ʱ����
        /// </summary>
        [Category("Action")]
        [Description("�ڵ���Button�ؼ�ʱ����")]
        public event CommandEventHandler Command;
        #endregion

        #region �¼�ע�ắ����
        /// <summary>
        ///  ���� Button �ؼ��� Click �¼���
        /// </summary>
        /// <param name="e">�����¼����ݵ� System.EventArgs��</param>
        protected virtual void OnClick(EventArgs e)
        {
            EventHandler handler = this.Click;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// ���� Button �ؼ��� Command �¼���
        /// </summary>
        /// <param name="e">���� Button �ؼ��� Command �¼���</param>
        protected virtual void OnCommand(CommandEventArgs e)
        {
            CommandEventHandler handler = this.Command;
            if (handler != null)
                handler(this, e);
            base.RaiseBubbleEvent(this, e);
        }
        #endregion

        #region IButton ��Ա��
        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ����÷������ؼ���
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

        #region IPostBackEventHandler ��Ա
        /// <summary>
        /// ʹ�������ؼ��ܹ��������巢�͵�������ʱ�������¼���
        /// </summary>
        /// <param name="eventArgument"> ��ʾҪ���ݵ��¼��������Ŀ�ѡ�¼�������</param>
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
