//================================================================================
//  FileName: DataControlLinkButton.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/7
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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
namespace iPower.Web.UI
{
    /// <summary>
    /// 链接按钮数据控件。
    /// </summary>
    [SupportsEventValidationEx]
    internal class DataControlLinkButton :LinkButton
    {
        #region 成员变量，构造函数。
        string callbackArgument;
        IPostBackContainer container;
        bool enableCallbck;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="container"></param>
        public DataControlLinkButton(IPostBackContainer container)
        {
            this.container = container;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public override bool CausesValidation
        {
            get
            {
                if (this.container != null)
                    return false;
                return base.CausesValidation;
            }
            set
            {
                if (this.container != null)
                    throw new NotSupportedException("CannotSetValidationOnDataControlButtons");
                base.CausesValidation = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        public void EnableCallback(string argument)
        {
            this.enableCallbck = true;
            this.callbackArgument = argument;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override PostBackOptions GetPostBackOptions()
        {
            if (this.container != null)
                return this.container.GetPostBackOptions(this);
            return base.GetPostBackOptions();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.SetCallbackProperties();
            this.SetForeColor();
            base.Render(writer);
        }
        /// <summary>
        /// 设置前景色。
        /// </summary>
        protected virtual void SetForeColor()
        {
            if (!this.ControlStyle.IsEmpty)
            {
                Control parent = this;
                for (int i = 0; i < 3; i++)
                {
                    parent = parent.Parent;
                    if (parent == null)
                        break;
                    Color foreColor = ((WebControl)parent).ForeColor;
                    if (foreColor != Color.Empty)
                    {
                        this.ForeColor = foreColor;
                        break;
                    }
                }
            }
        }

        #region 辅助函数。
        void SetCallbackProperties()
        {
            if (this.enableCallbck)
            {
                ICallbackContainer container = this.container as ICallbackContainer;
                if (container != null)
                {
                    string callbackScript = container.GetCallbackScript(this, this.callbackArgument);
                    if (!string.IsNullOrEmpty(callbackScript))
                        this.OnClientClick = callbackScript;
                }
            }
        }
        #endregion
    }
}
