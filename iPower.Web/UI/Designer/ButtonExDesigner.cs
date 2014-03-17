//================================================================================
//  FileName: ButtonExDesigner.cs
//  Desc:控钮控件(ButtonEx)的设计器。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-17
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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;

namespace iPower.Web.UI.Designer
{
    /// <summary>
    /// 控钮控件(ButtonEx)的设计器。
    /// </summary>
    public class ButtonExDesigner : ControlDesigner
    {
        /// <summary>
        /// 构造器。
        /// </summary>
        public ButtonExDesigner()
        {
        }

        /// <summary>
        /// 重载。
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            ButtonEx btn = base.Component as ButtonEx;
            if (btn != null)
            {
                string strText = btn.Text;
                bool flag = strText.Trim().Length == 0;

                if (flag)
                {
                    btn.Text = "[" + btn.ID + "]";
                }

                string strHtml = base.GetDesignTimeHtml();
                if (flag)
                {
                    btn.Text = strText;
                }

                return strHtml;
            }
            return string.Empty;
        }

    }
}
