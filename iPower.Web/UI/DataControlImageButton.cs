//================================================================================
//  FileName: DataControlImageButton.cs
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
    /// 
    /// </summary>
    [SupportsEventValidationEx]
    internal class DataControlImageButton : ImageButton
    {
        #region 成员变量，构造函数。
        string callbackArgument;
        IPostBackContainer container;
        bool enableCallback;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="container"></param>
        public DataControlImageButton(IPostBackContainer container)
        {
            this.container = container;
        }
        #endregion

        /// <summary>
        /// 启用回调函数。
        /// </summary>
        /// <param name="argument"></param>
        public void EnableCallback(string argument)
        {
            this.enableCallback = true;
            this.callbackArgument = argument;
        }

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        public override bool CausesValidation
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException("CannotSetValidationOnDataControlButtons");
            }
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
            base.Render(writer);
        }
        void SetCallbackProperties()
        {
            if (this.enableCallback)
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
