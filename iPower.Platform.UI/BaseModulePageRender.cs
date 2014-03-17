//================================================================================
//  FileName: BaseModulePageRender.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/7/5
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

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace iPower.Platform.UI
{
    /// <summary>
    /// 重构页面。
    /// </summary>
    partial class BaseModulePage
    {
        #region 页面相关函数。
        /// <summary>
        /// 页面布局
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            string err = this.Request[WebUIConst.ErrorMessage_QueryField];
            if (!string.IsNullOrEmpty(err))
            {
                err = this.Server.UrlDecode(err);
                writer.AddAttribute("language", "javascript");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.Write(string.Format("javascript:alert('操作失败：\\r{0}');", err.Trim()));
                writer.RenderEndTag();
            }
        }
        #region 页面特效
        /// <summary>
        /// 特效开始。
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderEffectBegin(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.EffectImageURL))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "loadingLayer");
                writer.AddStyleAttribute("display", "none");
                writer.AddStyleAttribute("Z-INDEX", "99999");
                writer.AddStyleAttribute("LEFT", "0px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
                writer.AddStyleAttribute("CURSOR", "wait");
                writer.AddStyleAttribute("POSITION", "absolute");
                writer.AddStyleAttribute("TOP", "0px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                writer.AddAttribute(HtmlTextWriterAttribute.Bgcolor, "#B3B3B3");
                writer.AddStyleAttribute("FILTER", "Alpha(Opacity=85)");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                writer.AddAttribute(HtmlTextWriterAttribute.Src, this.EffectImageURL);
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();

                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.RenderEndTag();

                writer.RenderEndTag();

                writer.RenderEndTag();

                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.Write("\r\n");
                writer.Write("<script languge='javascript' type='text/javascript'>\r\n");
                writer.Write("//<![CDATA[\r\n");
                writer.Write("javascript:window.loadingLayer.style.display=\"block\";");
                writer.Write("\r\n//]]>\r\n");
                writer.Write("</script>\r\n");
            }
        }
        /// <summary>
        /// 特效结束。
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderEffectEnd(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.EffectImageURL))
            {
                writer.Write("\r\n");
                writer.Write("<script languge='javascript' type='text/javascript'>\r\n");
                writer.Write("//<![CDATA[\r\n");
                writer.Write("javascript:setTimeout('window.loadingLayer.style.display=\"none\"',500);");
                writer.Write("\r\n//]]>\r\n");
                writer.Write("</script>\r\n");
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 重载设置主题。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            string theme = this.Request["theme"];
            if (!string.IsNullOrEmpty(theme))
            {
                this.Page.Theme = theme;
            }
            base.OnPreInit(e);
        }
    }
}
