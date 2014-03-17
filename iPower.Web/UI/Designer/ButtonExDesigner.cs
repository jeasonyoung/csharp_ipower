//================================================================================
//  FileName: ButtonExDesigner.cs
//  Desc:��ť�ؼ�(ButtonEx)���������
//
//  Called by
//
//  Auth:���£�jeason1914@gmail.com��
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
    /// ��ť�ؼ�(ButtonEx)���������
    /// </summary>
    public class ButtonExDesigner : ControlDesigner
    {
        /// <summary>
        /// ��������
        /// </summary>
        public ButtonExDesigner()
        {
        }

        /// <summary>
        /// ���ء�
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
