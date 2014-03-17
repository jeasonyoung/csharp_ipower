//================================================================================
//  FileName: DataControlFieldHeaderCellEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/9
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
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Globalization;
namespace iPower.Web.UI
{
    /// <summary>
    /// 呈现表中的标题单元格。
    /// </summary>
    public class DataControlFieldHeaderCellEx: DataControlFieldCellEx
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="containingField"></param>
        public DataControlFieldHeaderCellEx(DataControlFieldEx containingField)
            : base(containingField)
        {
        }
        #endregion

        /// <summary>
        /// 获取或设置缩写文本，该文本呈现在 HTML abbr 属性中并由屏幕阅读器使用。
        /// </summary>
        public virtual string AbbreviatedText
        {
            get { return this.ViewState["AbbreviatedText"] as string; }
            set { this.ViewState["AbbreviatedText"] = value; }
        }
        /// <summary>
        /// 获取或设置 HTML 表内标题单元格的范围。
        /// </summary>
        public virtual TableHeaderScope Scope
        {
            get
            {
                object obj = this.ViewState["Scope"];
                return obj == null ? TableHeaderScope.NotSet : (TableHeaderScope)obj;
            }
            set { this.ViewState["Scope"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            switch (this.Scope)
            {
                case TableHeaderScope.NotSet:
                    break;
                case TableHeaderScope.Column:
                    writer.AddAttribute(HtmlTextWriterAttribute.Scope, "col");
                    break;
                default:
                    writer.AddAttribute(HtmlTextWriterAttribute.Scope, "row");
                    break;
            }
            string abbreviatedText = this.AbbreviatedText;
            if (!string.IsNullOrEmpty(abbreviatedText))
                writer.AddAttribute(HtmlTextWriterAttribute.Abbr, abbreviatedText);
        }
    }
}
