//================================================================================
//  FileName: TabView.cs
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
    /// 表示一个控件，该控件作为 <see cref="TabMultiView"/> 控件中的一组控件的容器。
    /// </summary>
    [ToolboxData("<{0}:TabView runat='server'></{0}:TabView>")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class TabView : WebControl, INamingContainer
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TabView()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置标签名称。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置标签名称。")]
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
                }
            }
        }
        /// <summary>
        /// 获取或设置控件索引号。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置控件索引号。")]
        public int Index
        {
            get
            {
                return (int)this.TabIndex;
            }
            set
            {
                this.TabIndex = (short)value;
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
        public override ControlCollection Controls
        {
            get
            {
                return this.ViewControls;
            }
        }
        /// <summary>
        /// 获取<see cref="TabView"/>的内容子控件。
        /// </summary>
        protected virtual ControlCollection ViewControls
        {
            get
            {
                return base.Controls;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute("text", this.Text);
            writer.AddAttribute("index", this.Index.ToString());
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Overflow, "auto");
            TabMultiView parent = this.Parent as TabMultiView;
            if (parent != null && parent.DefaultActiveTabIndex != this.Index)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
        }
        #endregion
    }

    /// <summary>
    /// <see cref="TabView"/>控件集合。
    /// </summary>
    public class TabViewCollection : ControlCollection
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="owner"></param>
        public TabViewCollection(Control owner)
            : base(owner)
        {
        }
        #endregion

        #region 索引。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public new TabView this[int i]
        {
            get
            {
                return (TabView)base[i];
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        public override void Add(Control child)
        {
            if(!(child is TabView))
            {
                throw new ArgumentException("child控件不是TabView。");
            }
            base.Add(child);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="child"></param>
        public override void AddAt(int index, Control child)
        {
            if (!(child is TabView))
            {
                throw new ArgumentException("child控件不是TabView。");
            }
            base.AddAt(index, child);
        }
        #endregion
    }

}
