// ================================================================================
// 		File: ServerAlert.cs
// 		Desc: 
//  
// 		Called by:   
//               
// 		Auth: ������
// 		Date: 2005��3��18��
// ================================================================================
// 		Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//      2007-7-25   chenh               ��������
// ================================================================================
// Copyright (C) 2004-2005 SXT Corporation
// ================================================================================
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Drawing;

namespace iPower.Web.UI
{
	/// <summary>
	/// �ڷ����ʵ����JavaScript��alert("message")��Ч����Ϣ��ʾ��
	/// ˵����һ��ҳ�������һ�������Ŀؼ���
	/// ���ڱ��ؼ�����дҳ���window.onload������Ϊ����ʹ���˱��ؼ���ҳ���ϵ�onload�¼�������ִ�еķ�����
	/// ��Ϊ�ؼ���������BeforeAlertFunction��
	/// </summary>
	[DefaultProperty("Message")]
    [ToolboxData("<{0}:ServerAlert runat=server></{0}:ServerAlert>")]
    [ToolboxBitmap(typeof(ServerAlert))]
	public class ServerAlert : WebControl
    {
        #region ��Ա���������캯������������
        Regex r = null;
        bool bAlert = false;
		/// <summary>
		/// ���캯����
		/// </summary>
        public ServerAlert()
            : base(HtmlTextWriterTag.Span)
        {
            this.r = new Regex("\r\n|\r", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region ����
        /// <summary>
		/// ��ȡ������Ҫ��ʾ����Ϣ��
		/// </summary>
        [Category("Data")]
		[Description("��ȡ������Ҫ��ʾ����ʾ��Ϣ��")]
		public string Message
		{
			get
			{
                return this.ViewState["Message"] as String;
			}
			set
			{
                if (!string.IsNullOrEmpty(value))
                {
                    this.ViewState["Message"] = r.Replace(value, "\\r");
                    this.Alert = true;
                }
			}
		}

		/// <summary>
		/// �Ƿ���ʾ��Ϣ��
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
		[Description("��ȡ�������Ƿ���ʾ��Ϣ��")]
		public bool Alert
		{
			get
			{
                return this.bAlert;
			}
			set
			{
                this.bAlert = value;
			}
		}

		/// <summary>
		/// �ڷ�����ʾ��ϢǰҪִ�е�javascript������
		/// </summary>
        [Category("Appearance")]
		[Description("�ڷ�����ʾ��ϢǰҪִ�е�javascript������")]
		public string BeforeAlertFunction
		{
			get
			{
                object o = this.ViewState["BeforeAlertFunction"];
                return (o == null) ? string.Empty : (string)o;
			}
			set
			{
                if (this.BeforeAlertFunction != value)
                    this.ViewState["BeforeAlertFunction"] = value;
			}
		}

		/// <summary>
		/// �ڷ�����ʾ��Ϣ��Ҫִ�е�javascript������
		/// </summary>
        [Category("Appearance")]
        [Description("�ڷ�����ʾ��Ϣ��Ҫִ�е�javascript������")]
		public string AfterAlertFunction
		{
			get
			{
                object o = this.ViewState["AfterAlertFunction"];
                return (o == null) ? string.Empty : (string)o;
			}
			set
			{
                if (this.AfterAlertFunction != value)
                    this.ViewState["AfterAlertFunction"] = value;
			}
        }
        #endregion

        #region ����
        /// <summary>
		/// ���ء�
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
            string scriptKey = string.Format("{0}_Alert", this.ClientID);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), scriptKey))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script language=\"javascript\">\r\n");
                sb.Append("<!--").Append("\r\n");
                sb.Append("$(window).load(function(){").Append("\r\n");
                if (!string.IsNullOrEmpty(this.BeforeAlertFunction))
                {
                    sb.AppendFormat("\t{0}\r\n", this.BeforeAlertFunction);
                }
                sb.AppendFormat("\tvar obj = $(\"#{0}\");\r\n", this.ClientID);
                //sb.Append("alert(obj);console.info(obj);alert(obj.attr('value'));console.info(obj.html())").Append("\r\n");
                sb.Append("\tif(obj && obj.attr('value') == 'true')").Append("\r\n");
                sb.Append("\t{\r\n");
                sb.Append("\t\talert(obj.html());\r\n");
                sb.Append("\t}\r\n");
                if (!string.IsNullOrEmpty(this.AfterAlertFunction))
                {
                    sb.AppendFormat("\t{0}\r\n", this.AfterAlertFunction);
                }
                sb.Append("\r\n").Append("});").Append("\r\n");
                sb.Append("//-->").Append("\r\n");
                sb.Append("</script>\r\n");

                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, sb.ToString());
            }
		}
         
		/// <summary>
		/// ���ء�
		/// </summary>
		/// <param name="writer"></param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
            writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Alert.ToString().ToLower());
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
			//writer.AddAttribute("message",this.Message);
			base.AddAttributesToRender (writer);
		}

		/// <summary>
		/// ���ء�
		/// </summary>
		/// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
                writer.Write(string.Format("[{0}]", this.Message));
            base.Render(writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(HttpUtility.HtmlEncode(this.Message));
            base.RenderContents(writer);
        }
        #endregion
    }
}
