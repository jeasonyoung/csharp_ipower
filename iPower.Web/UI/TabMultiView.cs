//================================================================================
//  FileName: TabMultiView.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/21
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
using System.Data;
using System.Drawing;
using System.ComponentModel;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示的控件用作一组 <see cref="TabView"/> 控件的容器。
    /// </summary>
    [ToolboxData("<{0}:TabMultiView runat='server'></{0}:TabMultiView>")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class TabMultiView : WebControl, INamingContainer, IPostBackEventHandler
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TabMultiView()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置默认激活的<see cref="TabView"/>的索引。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置默认激活的的索引。")]
        public int DefaultActiveTabIndex
        {
            get
            {
                object obj = this.ViewState["DefaultActiveTabIndex"];
                return obj == null ? 0 : (int)obj;
            }
            set
            {
                this.ViewState["DefaultActiveTabIndex"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否启用服务器端激活<see cref="TabView"/>。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置是否启用服务器端激活。")]
        public bool EnableServerActive
        {
            get
            {
                object obj = this.ViewState["EnableServerActive"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                if (this.EnableServerActive != value)
                    this.ViewState["EnableServerActive"] = value;
            }
        }
        /// <summary>
        ///获取或设置标题样式。
        /// </summary>
        [Category("styles")]
        [Description("获取或设置标题样式。")]
        public string TabHeaderCssName
        {
            get
            {
                object obj = this.ViewState["TabHeaderCssName"];
                return obj == null ? "TabHeader" : (string)obj;
            }
            set
            {
                this.ViewState["TabHeaderCssName"] = value;
            }
        }
        /// <summary>
        ///获取或设置标题普通样式。
        /// </summary>
        [Category("styles")]
        [Description("获取或设置标题普通样式。")]
        public string TabHeaderNormalCssName
        {
            get
            {
                object obj = this.ViewState["TabHeaderNormalCssName"];
                return obj == null ? "TabHeaderNormal" : (string)obj;
            }
            set
            {
                this.ViewState["TabHeaderNormalCssName"] = value;
            }
        }
        /// <summary>
        /// 获取或设置标题选中时的样式。
        /// </summary>
        [Category("styles")]
        [Description("获取或设置标题选中时的样式。")]
        public string TabHeaderSelectedCssName
        {
            get
            {
                object obj = this.ViewState["TabHeaderSelectedCssName"];
                return obj == null ? "TabHeaderSelected" : (string)obj;
            }
            set
            {
                this.ViewState["TabHeaderSelectedCssName"] = value;
            }
        }
        /// <summary>
        /// 获取或设置标题高亮的样式。
        /// </summary>
        [Category("styles")]
        [Description("获取或设置标题高亮的样式。")]
        public string TabHeaderHighLightCssName
        {
            get
            {
                object obj = this.ViewState["TabHeaderHighLightCssName"];
                return obj == null ? "TabHeaderHighLight" : (string)obj;
            }
            set
            {
                this.ViewState["TabHeaderHighLightCssName"] = value;
            }
        }
        /// <summary>
        /// 获取或设置Tab页背景样式。
        /// </summary>
        [Category("styles")]
        [Description("获取或设置Tab页背景样式。")]
        public string TabBackgroundCssName
        {
            get
            {
                object obj = this.ViewState["TabBackgroundCssName"];
                return obj == null ? "TabBackground" : obj.ToString();
            }
            set
            {
                this.ViewState["TabBackgroundCssName"] = value;
            }
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 切换tab页事件。
        /// </summary>
        [Category("Events")]
        [Description("切换tab页事件。")]
        public event EventHandler ChangedActiveTab;
        /// <summary>
        /// 触发切换tab页事件。
        /// </summary>
        protected void OnChangedActiveTab()
        {
            EventHandler handler = this.ChangedActiveTab;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        protected override void AddParsedSubObject(object obj)
        {
            if (obj is TabView)
            {
                this.TabViewControls.Add((TabView)obj);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override ControlCollection Controls
        {
            get
            {
                return this.TabViewControls;
            }
        }

        /// <summary>
        /// 获取<see cref="TabView"/>控件。
        /// </summary>
        protected virtual ControlCollection TabViewControls
        {
            get
            {
                return base.Controls;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new TabViewCollection(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0px");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0px");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.Page != null)
            {
                ClientScriptManager csm = this.Page.ClientScript;
                string scriptKey = string.Format("{0}_SelectdTab", this.ClientID);
                if (!this.EnableServerActive && !csm.IsClientScriptBlockRegistered(scriptKey))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type=\"text/javascript\">\r\n");
                    sb.AppendLine("//<![CDATA[");

                    sb.AppendLine(string.Format("function {0}(obj)", scriptKey));
                    sb.AppendLine("{");
                    sb.AppendLine("\tif(obj)");
                    sb.AppendLine("\t{");
                    //sb.AppendLine("\t\talert(obj.tagViewID);");
                    //sb.AppendLine("\t\talert(obj.parentNode.tagName);");
                    sb.AppendLine("\t\tvar td = obj.parentNode;");
                    sb.AppendLine("\t\tif(td)");
                    sb.AppendLine("\t\t{");
                    sb.AppendLine("\t\t\tvar spans = td.getElementsByTagName(\"span\")");
                    sb.AppendLine("\t\t\tif(spans && spans != null)");
                    sb.AppendLine("\t\t\t{");
                    sb.AppendLine("\t\t\t\tfor(var i=0; i<spans.length;i++)");
                    sb.AppendLine("\t\t\t\t{");
                    sb.AppendLine("\t\t\t\t\tif(spans[i].tagViewID != \"\")");
                    sb.AppendLine("\t\t\t\t\t{");
                    sb.AppendLine("\t\t\t\t\t\tvar tab = eval(\"document.all.\"+spans[i].tagViewID);");
                    sb.AppendLine("\t\t\t\t\t\tif(tab)");
                    sb.AppendLine("\t\t\t\t\t\t{");
                   // sb.AppendLine("\t\t\t\t\t\talert(tab.style.display);");
                    sb.AppendLine("\t\t\t\t\t\t\ttab.style.display=\"none\";");
                    //sb.AppendLine("\t\t\t\t\t\t\tspans[i].oclass=spans[i].className;");
                    sb.AppendLine(string.Format("\t\t\t\t\t\t\tspans[i].className=spans[i].oclass=\"{0}\";", this.TabHeaderNormalCssName));
                    sb.AppendLine("\t\t\t\t\t\t}");
                    sb.AppendLine("\t\t\t\t\t}");
                    sb.AppendLine("\t\t\t\t}");
                    sb.AppendLine("\t\t\t}");
                    sb.AppendLine("\t\t}");
                    sb.AppendLine("\t\tvar ctag=eval(\"document.all.\"+obj.tagViewID);");
                    sb.AppendLine("\t\tif(ctag)");
                    sb.AppendLine("\t\t{");
                   // sb.AppendLine("\t\t\tobj.oclass=obj.className;");
                    sb.AppendLine(string.Format("\t\t\tobj.oclass=obj.className=\"{0}\";", this.TabHeaderSelectedCssName));
                    sb.AppendLine("\t\t\tctag.style.display=\"\";");
                   // sb.AppendLine("\t\t\talert(obj.className);");
                    sb.AppendLine("\t\t}");
                    sb.AppendLine("\t}");
                    sb.AppendLine("}");
                    sb.AppendLine("//]]>");
                    sb.AppendLine("</script>");
                    csm.RegisterClientScriptBlock(this.GetType(), scriptKey, sb.ToString());
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            TableRow headRow = new TableRow();
            headRow.CssClass = this.TabHeaderCssName;
            TableCell cell = new TableCell();
            cell.Width = new Unit("100%");
            cell.VerticalAlign = VerticalAlign.Middle;

            TabViewCollection tabCollection = (TabViewCollection)this.TabViewControls;
            if (tabCollection != null && tabCollection.Count > 0)
            {
                string seletedCssName = this.TabHeaderSelectedCssName;
                string normalCssName = this.TabHeaderNormalCssName;
                string highLightCssName = this.TabHeaderHighLightCssName;
                int defaultActiveIndex = this.DefaultActiveTabIndex;
                for (int i = 0; i < tabCollection.Count; i++)
                {
                    TabView tab = tabCollection[i];
                    if (tab != null && tab.Visible)
                    {
                        HtmlGenericControl span = new HtmlGenericControl("span");
                        span.Attributes["style"] = "float:left;cursor:hand;";
                        span.InnerText = tab.Text;
                        span.Attributes["title"] = tab.Text;
                        span.Attributes["tagViewID"] = tab.ClientID;
                        span.Attributes["onclick"] = this.EnableServerActive ?
                            string.Format("javascript:{0};", this.Page.ClientScript.GetPostBackEventReference(this, "Active$" + tab.Index.ToString()))
                            : string.Format("javasscript:{0}_SelectdTab(this);", this.ClientID);
                        span.Attributes["class"] = span.Attributes["oclass"] = (tab.Index == defaultActiveIndex) ? seletedCssName : normalCssName;
                        span.Attributes["onmouseout"] = "this.className=this.oclass;";
                        span.Attributes["onmouseover"] = string.Format("this.className='{0}';", highLightCssName);
                        cell.Controls.Add(span);
                    }
                }
            }
            headRow.Cells.Add(cell);
            headRow.RenderControl(writer);

            //tr
            writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            //td
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, this.TabBackgroundCssName);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (tabCollection != null && tabCollection.Count > 0)
            {
                for (int i = 0; i < tabCollection.Count; i++)
                {
                    TabView tab = tabCollection[i];
                    if (tab != null && tab.Visible)
                        tab.RenderControl(writer);
                }
            }
            //base.RenderContents(writer);
            //end td 
            writer.RenderEndTag();
            //end tr
            writer.RenderEndTag();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.CssClass))
                this.CssClass = "TabMultiView";
            base.Render(writer);
        }
        #endregion

        #region IPostBackEventHandler 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            try
            {
                if (!string.IsNullOrEmpty(eventArgument))
                {
                    string[] args = eventArgument.Split('$');
                    if (args[0] == "Active")
                    {
                        this.DefaultActiveTabIndex = Convert.ToInt32(args[1]);
                        this.DataBind();

                        this.OnChangedActiveTab();
                    }
                }
            }
            catch (Exception) { }
        }

        #endregion
    }
}
