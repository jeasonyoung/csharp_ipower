//================================================================================
//  FileName: TemplateFieldEx.cs
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
    /// 表示在数据绑定控件中显示自定义内容的字段。
    /// </summary>
    public class TemplateFieldEx : DataControlFieldEx
    {
        #region 成员变量，构造函数。
        ITemplate alternatingItemTemplate, headerTemplate, itemTemplate, footerTemplate; 
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TemplateFieldEx()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置交替项的模板。
        /// </summary>
        [Browsable(false)]
        [Description("获取或设置交替项的模板。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(IDataItemContainer), BindingDirection.TwoWay)]
        public virtual ITemplate AlternatingItemTemplate
        {
            get { return this.alternatingItemTemplate; }
            set
            {
                this.alternatingItemTemplate = value;
                this.OnFieldChanged();
            }
        }
        /// <summary>
        /// 获取或设置象绑定到的值是 Empty，它是否应转换为 null。
        /// </summary>
        [Category("Behavior")]
        [Description("获取或设置象绑定到的值是 Empty，它是否应转换为 null。")]
        public virtual bool ConvertEmptyStringToNull
        {
            get
            {
                object obj = this.ViewState["ConvertEmptyStringToNull"];
                return (obj == null) ? true : (bool)obj;
            }
            set{this.ViewState["ConvertEmptyStringToNull"] = value;}
        }

        /// <summary>
        /// 获取或设置脚注部分的模板。
        /// </summary>
        [Browsable(false)]
        [Description("获取或设置脚注部分的模板。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(IDataItemContainer))]
        public virtual ITemplate FooterTemplate
        {
            get{return this.footerTemplate;}
            set
            {
                this.footerTemplate = value;
                this.OnFieldChanged();
            }
        }
        /// <summary>
        /// 获取或设置标头部分的模板。
        /// </summary>
        [Browsable(false)]
        [Description("获取或设置标头部分的模板。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(IDataItemContainer))]
        public virtual ITemplate HeaderTemplate
        {
            get{return this.headerTemplate;}
            set
            {
                this.headerTemplate = value;
                this.OnFieldChanged();
            }
        }
        /// <summary>
        /// 获取或设置用于显示数据绑定控件中的项的模板。
        /// </summary>
        [Browsable(false)]
        [Description("获取或设置用于显示数据绑定控件中的项的模板。")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(IDataItemContainer), BindingDirection.TwoWay)]
        public virtual ITemplate ItemTemplate
        {
            get{return this.itemTemplate;}
            set
            {
                this.itemTemplate = value;
                this.OnFieldChanged();
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 复制。
        /// </summary>
        /// <param name="newField"></param>
        protected override void  CopyProperties(DataControlFieldEx newField)
        {
            ((TemplateFieldEx)newField).ConvertEmptyStringToNull = this.ConvertEmptyStringToNull;
            ((TemplateFieldEx)newField).AlternatingItemTemplate = this.AlternatingItemTemplate;
            ((TemplateFieldEx)newField).ItemTemplate = this.ItemTemplate;
            ((TemplateFieldEx)newField).FooterTemplate = this.FooterTemplate;
            ((TemplateFieldEx)newField).HeaderTemplate =this.HeaderTemplate;
             base.CopyProperties(newField);
        }
        /// <summary>
        /// 创建控件。
        /// </summary>
        /// <returns></returns>
        protected override DataControlFieldEx CreateField()
        {
            return new TemplateFieldEx();
        }
        /// <summary>
        /// 从当前表单元格提取由一个或多个双向绑定语句 (DataBind) 指定的数据控件字段的值，并将这些值添加到指定的 IOrderedDictionary 集合。
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="cell"></param>
        /// <param name="rowState"></param>
        /// <param name="includeReadOnly"></param>
        public override void  ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCellEx cell, DataGridViewRowState rowState, bool includeReadOnly)
        {
            DataBoundControlExHelper.ExtractValuesFromBindableControls(dictionary, cell);
            IBindableTemplate itemTemplate = this.ItemTemplate as IBindableTemplate;
            if (((rowState & DataGridViewRowState.AlterNate) != DataGridViewRowState.Normal) && (this.AlternatingItemTemplate != null))
                itemTemplate = this.AlternatingItemTemplate as IBindableTemplate;
            if (itemTemplate != null)
            {
                bool convertEmptyStringToNull = this.ConvertEmptyStringToNull;
                foreach (DictionaryEntry entry in itemTemplate.ExtractValues(cell.BindingContainer))
                {
                    object obj = entry.Value;
                    if (convertEmptyStringToNull && (obj is string) && (((string)obj).Length == 0))
                        dictionary[entry.Key] = null;
                    else
                        dictionary[entry.Key] = obj;
                }
            }
        }
        /// <summary>
        /// 将文本或控件添加到单元格的控件集合中。
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public override void InitializeCell(DataControlFieldCellEx cell, DataControlCellType cellType, DataGridViewRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            ITemplate template = null;
            switch (cellType)
            {
                case DataControlCellType.Header:
                    template = this.headerTemplate;
                    break;
                case DataControlCellType.Footer:
                    template = this.footerTemplate;
                    break;
                case DataControlCellType.DataCell:
                    template = this.itemTemplate;
                    if (((rowState & DataGridViewRowState.AlterNate) != DataGridViewRowState.Normal) && (this.alternatingItemTemplate != null))
                        template = this.alternatingItemTemplate;
                    break;
            }
            if (template != null)
            {
                cell.Text = string.Empty;
                template.InstantiateIn(cell);
            }
            else if (cellType == DataControlCellType.DataCell)
                cell.Text = "&nbsp;";
        }
        /// <summary>
        /// 
        /// </summary>
        public override void ValidateSupportsCallback()
        {
            throw new NotSupportedException("TemplateFieldEx_CallbacksNotSupported");
        }
        #endregion
    }
}
