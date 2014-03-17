//================================================================================
//  FileName: LabelEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/9/9
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
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示在网页上显示文本的标签。
    /// </summary>
    [ToolboxData("<{0}:LabelEx runat=\"server\"></{0}:LabelEx>")]
    [DefaultProperty("Text")]
    [ControlBuilder(typeof(LabelControlBuilder))]
    [Designer(typeof(System.Web.UI.Design.WebControls.LabelDesigner))]
    [DataBindingHandler(typeof(System.Web.UI.Design.TextDataBindingHandler))]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level= AspNetHostingPermissionLevel.Minimal)]
    public class LabelEx : Label
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LabelEx()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置样式。")]
        public new string Style
        {
            get
            {
                object o = this.ViewState["Style"];
                return o == null ? string.Empty : (string)o;
            }
            set
            {
                if (this.Style != value)
                {
                    this.ViewState["Style"] = value;
                }
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            string s = this.Style;
            if (!string.IsNullOrEmpty(s))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, s);
            }
        }
        #endregion
    }
}
