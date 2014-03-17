//================================================================================
//  FileName: DataBoundControlExHelper.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Globalization;

namespace iPower.Web.UI
{
    /// <summary>
    /// 帮助类。
    /// </summary>
    public static class DataBoundControlExHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="container"></param>
        public static void ExtractValuesFromBindableControls(IOrderedDictionary dictionary, Control container)
        {
            IBindableControl control = container as IBindableControl;
            if (control != null)
                 control.ExtractValues(dictionary);
            foreach (Control control2 in container.Controls)
            {
                ExtractValuesFromBindableControls(dictionary, control2);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBindableType(Type type)
        {
            if (type == null)
                return false;
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                type = underlyingType;
            if (((!type.IsPrimitive && (type != typeof(string))) && ((type != typeof(DateTime)) && (type != typeof(decimal)))) && ((type != typeof(Guid)) && (type != typeof(DateTimeOffset))))
                return (type == typeof(TimeSpan));
            return true;
        }
    }
}
