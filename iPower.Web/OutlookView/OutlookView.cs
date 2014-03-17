//================================================================================
//  FileName: OutlookView.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/8/11
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Xml;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Permissions;

//using System.Drawing;
using System.ComponentModel;

using iPower.Web.TreeView;

[assembly: WebResource("iPower.Web.OutlookView.OutlookNormal.png","image/x-png")]
[assembly: WebResource("iPower.Web.OutlookView.OutlookExpand.png", "image/x-png")]
namespace iPower.Web.OutlookView
{
    /// <summary>
    /// Outlook风格控件。
    /// </summary>
    [ToolboxData("<{0}:OutlookView runat='server'></{0}:OutlookView>")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class OutlookView : TreeView.TreeView
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OutlookView()
        {
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ClientScriptManager csm = this.Page.ClientScript;
            if (csm != null)
            {
                string openOutlookKey = string.Format("{0}_openOutlook", this.GetType().Name);
                if (!csm.IsClientScriptBlockRegistered(this.GetType(), openOutlookKey))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("<script type=\"text/javascript\">");
                    builder.AppendLine(string.Format("function {0}(obj)", openOutlookKey));
                    builder.AppendLine("{");

                    builder.AppendLine("\ttry{");

                    builder.AppendLine(string.Format("\t\tvar expand = \"{0}\";", this.GetImageUrl(true)));
                    builder.AppendLine(string.Format("\t\tvar normal = \"{0}\";", this.GetImageUrl(false)));

                    builder.AppendLine("\t\tif(obj){");
                    builder.AppendLine("\t\t\tvar p = obj.parentNode;");
                    builder.AppendLine("\t\t\tvar tr = obj.parentNode.nextSibling;");

                    builder.AppendLine("\t\t\tif(p && tr){");
                    builder.AppendLine("\t\t\t\tvar t = p.parentNode;");
                    builder.AppendLine("\t\t\t\tif(t && t.childNodes.length > 0){");
                    builder.AppendLine("\t\t\t\t\tfor(var i = 0; i < t.childNodes.length; i++){");
                    builder.AppendLine("\t\t\t\t\t\tif(i % 2 != 0){");
                    builder.AppendLine("\t\t\t\t\t\t\tvar o = t.childNodes[i];");
                    builder.AppendLine("\t\t\t\t\t\t\tif(o && o.tagName == \"TR\" && o.style.display == \"\"){");
                    builder.AppendLine("\t\t\t\t\t\t\t\to.style.display = \"none\";");
                    builder.AppendLine("\t\t\t\t\t\t\t\tvar x = o.previousSibling;");
                    builder.AppendLine("\t\t\t\t\t\t\t\tif(x){");
                    builder.AppendLine("\t\t\t\t\t\t\t\t\tvar xo = x.childNodes[0];");
                    builder.AppendLine("\t\t\t\t\t\t\t\t\tif(xo)");
                    builder.AppendLine("\t\t\t\t\t\t\t\t\t\txo.style.backgroundImage = \"url(\"+normal+\")\";");
                    builder.AppendLine("\t\t\t\t\t\t\t\t}");
                    builder.AppendLine("\t\t\t\t\t\t\t}");
                    builder.AppendLine("\t\t\t\t\t\t}");
                    builder.AppendLine("\t\t\t\t\t}");
                    builder.AppendLine("\t\t\t\t}");

                    builder.AppendLine("\t\t\t\tvar td = p.childNodes[0];");
                    builder.AppendLine("\t\t\t\tif(td){");
                    builder.AppendLine("\t\t\t\t\tif(tr.style.display == \"\"){");
                    builder.AppendLine("\t\t\t\t\t\ttr.style.display = \"none\";");
                    builder.AppendLine("\t\t\t\t\t\ttd.style.backgroundImage = \"url(\"+normal+\")\";");
                    builder.AppendLine("\t\t\t\t\t}else{");
                    builder.AppendLine("\t\t\t\t\t\ttr.style.display = \"\";");
                    builder.AppendLine("\t\t\t\t\t\ttd.style.backgroundImage = \"url(\"+expand+\")\";");
                    builder.AppendLine("\t\t\t\t\t}");
                   
                   
                    builder.AppendLine("\t\t\t\t}");
                    builder.AppendLine("\t\t\t}");
                    builder.AppendLine("\t\t}");
                    builder.AppendLine("\t}catch(e){alert(e.description);}");
                    builder.AppendLine("}");
                    builder.AppendLine("</script>");
                    builder.AppendLine();
                    csm.RegisterClientScriptBlock(this.GetType(), openOutlookKey, builder.ToString());
                }
            }
        }
        /// <summary>
        /// 绘制内容。
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Page != null && this.Items.Count > 0)
            {
                TableRow tr = null;
                TableCell cell = null;
                string strName = this.GetType().Name;
                foreach (TreeViewNode item in this.Items)
                {
                    if (item.Childs.Count > 0)
                    {
                        //outlook
                        tr = new TableRow();
                        cell = new TableCell();
                        cell.Width = new Unit("100%");
                        cell.Height = new Unit("30px");
                        cell.VerticalAlign = VerticalAlign.Middle;
                        cell.HorizontalAlign = HorizontalAlign.Center;
                        cell.Style.Add(HtmlTextWriterStyle.BackgroundImage, string.Format("url({0})", this.GetImageUrl(item.Expand)));
                        cell.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                        cell.Text = cell.ToolTip = item.Text;
                        cell.Attributes.Add("onclick", string.Format("{0}_openOutlook(this);", strName));
                        tr.Cells.Add(cell);
                        tr.RenderControl(writer);

                        #region childs。
                        tr = new TableRow();
                        if (!item.Expand)
                            tr.Style.Add(HtmlTextWriterStyle.Display, "none");
                        cell = new TableCell();
                        cell.Width = new Unit("100%");
                        cell.VerticalAlign = VerticalAlign.Top;
                        this.CreateTreeView(item, cell);
                        tr.Cells.Add(cell);
                        tr.RenderControl(writer);
                        #endregion
                    }
                }
            }
        }
        #endregion

        #region 辅助函数。
        string GetImageUrl(bool expand)
        {
            if (expand)
                return this.Page.ClientScript.GetWebResourceUrl(typeof(OutlookView), "iPower.Web.OutlookView.OutlookExpand.png");
            else
                return this.Page.ClientScript.GetWebResourceUrl(typeof(OutlookView), "iPower.Web.OutlookView.OutlookNormal.png");
        }
        #endregion
    }
}
