//================================================================================
//  FileName: DataSourceViewEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/3
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

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace iPower.Web.UI
{
    /// <summary>
    /// 用作所有数据源视图类的基类，这些视图类定义数据源控件的功能。
    /// </summary>
    public abstract class DataSourceViewEx
    {
        #region 成员变量，构造函数。
        EventHandlerList events;
        string name;
        /// <summary>
        /// 构造函数。
        /// </summary>
        protected DataSourceViewEx(IDataSourceEx owner,string viewName)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            //if (string.IsNullOrEmpty(viewName))
               // throw new ArgumentNullException("viewName");

            this.name = viewName;
            owner.DataSourceChanged += new EventHandler(delegate(object sender, EventArgs e)
            {
                this.OnDataSourceViewChanged(e);
            });
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 在数据源视图已更改时发生。
        /// </summary>
        public event EventHandler DataSourceViewChanged;
        /// <summary>
        /// 触发<see cref="DataSourceViewChanged"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDataSourceViewChanged(EventArgs e)
        {
            EventHandler handler = this.DataSourceViewChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取数据源视图的名称。
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// 获取是否支持对通过<see cref="ExecuteSelect"/> 方法检索到的数据进行分页。
        /// </summary>
        public virtual bool CanPage
        {
            get { return false; }
        }
        /// <summary>
        /// 获取与当前 <see cref="DataSourceControl"/> 对象相关联的 <see cref="DataSourceViewEx"/> 对象是否支持检索数据的总行数（而不是数据）。
        /// </summary>
        public virtual bool CanRetrieveTotalRowCount
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 获取与当前  <see cref="DataSourceControl"/> 对象相关联的 <see cref="DataSourceViewEx"/> 对象是否支持基础数据源的排序视图。
        /// </summary>
        public virtual bool CanSort
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 获取数据源视图的事件处理程序委托的列表。
        /// </summary>
        protected EventHandlerList Events
        {
            get
            {
                if (this.events == null)
                    this.events = new EventHandlerList();
                return this.events;
            }
        }
        #endregion

        /// <summary>
        /// 从基础数据存储获取数据列表。
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        protected internal abstract IEnumerable ExecuteSelect(DataSourceSelectArgumentsEx arguments);

        /// <summary>
        /// 由 <see cref="DataSourceSelectArgumentsEx.RaiseUnsupportedCapabilitiesError"/> 方法调用，以将<see cref="ExecuteSelect"/> 操作所请求的功能与视图所支持的功能进行比较。
        /// </summary>
        /// <param name="capability"></param>
        protected internal virtual void RaiseUnsupportedCapabilityError(DataSourceCapabilities capability)
        {
            if (!this.CanPage && ((capability & DataSourceCapabilities.Page) != DataSourceCapabilities.None))
            {
                throw new NotSupportedException("DataSourceViewEx_NoPaging");
            }
            if (!this.CanSort && ((capability & DataSourceCapabilities.Sort) != DataSourceCapabilities.None))
            {
                throw new NotSupportedException("DataSourceViewEx_NoSorting");
            }
            if (!this.CanRetrieveTotalRowCount && ((capability & DataSourceCapabilities.RetrieveTotalRowCount) != DataSourceCapabilities.None))
            {
                throw new NotSupportedException("DataSourceViewEx_NoRowCount");
            }
        }
        /// <summary>
        /// 从基础数据存储中异步获取数据列表。
        /// </summary>
        /// <param name="arguments">用于请求对数据执行基本数据检索以外的操作。</param>
        /// <param name="callback">用于在异步操作完成时通知数据绑定控件的 <see cref="DataSourceViewSelectCallback"/> 委托。</param>
        public virtual void Select(DataSourceSelectArgumentsEx arguments, DataSourceViewSelectCallback callback)
        {
            if (callback == null)
                 throw new ArgumentNullException("callback");
             callback(this.ExecuteSelect(arguments));
        }
    }
}
