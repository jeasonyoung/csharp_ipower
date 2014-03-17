//================================================================================
//  FileName: DataControlFieldCellEx.cs
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
using System.Drawing.Design;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示表格 ASP.NET 数据绑定控件的已呈现表中的单元格。
    /// </summary>
    public class DataControlFieldCellEx : TableCell
    {
        #region 成员变量，构造函数。
        DataControlFieldEx containingField;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="containingField"></param>
        public DataControlFieldCellEx(DataControlFieldEx containingField)
        {
            this.containingField = containingField;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取包含当前单元格的 <see cref="DataControlFieldEx"/> 对象。
        /// </summary>
        public DataControlFieldEx ContainingField
        {
            get { return this.containingField; }
        }
        #endregion
    }
}
