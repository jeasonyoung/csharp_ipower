//================================================================================
//  FileName: SupportsEventValidationAttributeEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/4
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
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 定义元数据属性，Web 服务器控件使用它来表明支持事件验证。无法继承此类。
    /// </summary>
    public sealed class SupportsEventValidationExAttribute : Attribute
    {
        #region 成员变量，构造函数。
        static Hashtable typesSupportsEventValidation = Hashtable.Synchronized(new Hashtable());
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool SupportsEventValidation(Type type)
        {
            object obj = typesSupportsEventValidation[type];
            if (obj != null)
                return (bool)obj;
            object[] customAttributes = type.GetCustomAttributes(typeof(SupportsEventValidationExAttribute), false);
            bool flag = (customAttributes != null) && (customAttributes.Length > 0);
            typesSupportsEventValidation[type] = flag;
            return flag;
        }
    }
}
