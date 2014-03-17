//================================================================================
//  FileName: DataControlPagerLinkButton.cs
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
    internal class DataControlPagerLinkButton : DataControlLinkButton
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="container"></param>
        public DataControlPagerLinkButton(IPostBackContainer container)
            : base(container)
        {
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected override void SetForeColor()
        {
            if (!this.ControlStyle.IsEmpty)
            {
                Control parent = this;
                for (int i = 0; i < 6; i++)
                {
                    parent = parent.Parent;
                    if (parent == null)
                        return;
                    Color foreColor = ((WebControl)parent).ForeColor;
                    if (foreColor != Color.Empty)
                    {
                        this.ForeColor = foreColor;
                        return;
                    }
                }
            }
        }
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
                throw new NotSupportedException("CausesValidation");
            }
        }
    }
}
