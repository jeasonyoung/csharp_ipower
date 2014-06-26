// ================================================================================
// 		File: ServerAlert.cs
// 		Desc: 
//  
// 		Called by:   
//               
// 		Auth: 吴智雄
// 		Date: 2005年3月18日
// ================================================================================
// 		Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//      2007-7-25   chenh               升级更新
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
	/// 在服务端实现与JavaScript的alert("message")等效的消息提示框。
	/// 说明：一个页面仅能有一个这样的控件。
	/// 由于本控件会重写页面的window.onload方法，为了在使用了本控件的页面上的onload事件里增加执行的方法，
	/// 特为控件增加属性BeforeAlertFunction。
	/// </summary>
	[DefaultProperty("Message")]
    [ToolboxData("<{0}:ServerAlert runat=server></{0}:ServerAlert>")]
    [ToolboxBitmap(typeof(ServerAlert))]
	public class ServerAlert : WebControl
    {
        #region 成员变量，构造函数，析构函数
        Regex r = null;
        bool bAlert = false;
		/// <summary>
		/// 构造函数。
		/// </summary>
        public ServerAlert()
            : base(HtmlTextWriterTag.Span)
        {
            this.r = new Regex("\r\n|\r", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 属性
        /// <summary>
		/// 获取或设置要显示的消息。
		/// </summary>
        [Category("Data")]
		[Description("获取或设置要显示的提示消息。")]
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
		/// 是否显示消息。
		/// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
		[Description("获取或设置是否显示消息。")]
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
		/// 在发出提示信息前要执行的javascript方法。
		/// </summary>
        [Category("Appearance")]
		[Description("在发出提示信息前要执行的javascript方法。")]
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
		/// 在发出提示信息后要执行的javascript方法。
		/// </summary>
        [Category("Appearance")]
        [Description("在发出提示信息后要执行的javascript方法。")]
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

        #region 重载
        /// <summary>
		/// 重载。
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
		/// 重载。
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
		/// 重载。
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
