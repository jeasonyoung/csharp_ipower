//================================================================================
//  FileName: DataBoundControlEx.cs
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
using System.Web.UI.WebControls.Adapters;
namespace iPower.Web.UI
{
    /// <summary>
    /// 作为所有以列表或表格形式显示数据的数据绑定控件的基类使用。
    /// </summary>
    public abstract class DataBoundControlEx : BaseDataBoundControlEx
    {
        #region 成员变量，构造函数。
        DataSourceSelectArgumentsEx arguments;
        IDataSourceEx currentDataSource;
        bool currentDataSourceValid, currentViewValid, ignoreDataSourceViewChanged, pagePreLoadFired;
        DataSourceViewEx currentView;

        const string DataBoundViewStateKey = "_!DataBound";
        /// <summary>
        /// 构造函数。
        /// </summary>
        protected DataBoundControlEx()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 当数据源包含多个不同的数据项列表时，获取或设置数据绑定控件绑定到的数据列表的名称。
        /// </summary>
        [Themeable(false)]
        [Category("Data")]
        [Description("当数据源包含多个不同的数据项列表时，获取或设置数据绑定控件绑定到的数据列表的名称。")]
        public virtual string DataMember
        {
            get
            {
                object obj = this.ViewState["DataMember"];
                if (obj != null)
                    return (string)obj;
                return string.Empty;
            }
            set
            {
                this.ViewState["DataMember"] = value;
                this.OnDataPropertyChanged();
            }
        }

        /// <summary>
        /// 获取数据绑定控件从数据源控件检索数据时使用的 <see cref="DataSourceSelectArgumentsEx"/> 对象。
        /// </summary>
        protected DataSourceSelectArgumentsEx SelectArguments
        {
            get
            {
                if (this.arguments == null)
                    this.arguments = this.CreateDataSourceSelectArguments();
                return this.arguments;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDataSourceEx DataSourceObject
        {
            get
            {
                return this.GetDataSource();
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 在某一基数据源标识属性更改后，将数据绑定控件重新绑定到其数据。
        /// </summary>
        protected override void OnDataPropertyChanged()
        {
            this.currentViewValid = false;
            this.currentDataSourceValid = false;
            base.OnDataPropertyChanged();
        }
        /// <summary>
        /// 触发<see cref=" Control.Load"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            this.ConfirmInitState();
            this.ConnectToDataSourceView();
            if (((this.Page != null) && (!this.pagePreLoadFired)) && (this.ViewState[DataBoundViewStateKey] == null))
            {
                if (!this.Page.IsPostBack)
                    this.RequiresDataBinding = true;
                else if (base.IsViewStateEnabled)
                    this.RequiresDataBinding = true;
            }
            base.OnLoad(e);
        }
        /// <summary>
        /// 在加载数据绑定控件之前设置该控件的初始化状态。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPagePreLoad(object sender, EventArgs e)
        {
            base.OnPagePreLoad(sender, e);
            if (this.Page != null)
            {
                if (!this.Page.IsPostBack)
                    base.RequiresDataBinding = true;
                else if (base.IsViewStateEnabled && (this.ViewState[DataBoundViewStateKey] == null))
                    base.RequiresDataBinding = true;
            }
            this.pagePreLoadFired = true;
        }
        /// <summary>
        /// 从关联的数据源中检索数据。
        /// </summary>
        protected override void PerFormSelect()
        {
            this.OnDataBinding(EventArgs.Empty);

            DataSourceViewEx data = this.GetData();
            this.arguments = this.CreateDataSourceSelectArguments();
            this.ignoreDataSourceViewChanged = true;
            base.RequiresDataBinding = false;
            this.MarkAsDataBound();
            data.Select(this.arguments, new DataSourceViewSelectCallback(this.OnDataSourceViewSelectCallback));
        }
        /// <summary>
        /// 验证数据绑定控件绑定到的对象是否可以和该控件一同使用。
        /// </summary>
        /// <param name="dataSource"></param>
        protected override void ValidateDataSource(object dataSource)
        {
            if ((dataSource != null) && !(dataSource is IListSource) && !(dataSource is IEnumerable) && !(dataSource is IDataSourceEx))
                throw new InvalidOperationException("DataBoundControl_InvalidDataSourceType");
        }
        #endregion

        /// <summary>
        /// 引发 DataSourceViewChanged 事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDataSourceViewChanged(object sender, EventArgs e)
        {
            if (!this.ignoreDataSourceViewChanged)
                base.RequiresDataBinding = true;
        }

        DataSourceViewEx ConnectToDataSourceView()
        {
            if (!this.currentViewValid || base.DesignMode)
            {
                if (this.currentView != null)
                    this.currentView.DataSourceViewChanged -= new EventHandler(this.OnDataSourceViewChanged);
                this.currentDataSource = this.GetDataSource();
                string dataMember = this.DataMember;
                if (this.currentDataSource == null)
                {
                    //this.OnBuildDataSource(EventArgs.Empty);
                    this.currentDataSource = new ReadOnlyDataSourceEx(this.DataSource, dataMember);
                }
                this.currentDataSourceValid = true;
                DataSourceViewEx view = this.currentDataSource.GetView(dataMember);
                if (view == null)
                    throw new InvalidOperationException("DataControl_ViewNotFound");
                else
                {
                    this.currentView = view;
                    this.currentView.DataSourceViewChanged += new EventHandler(this.OnDataSourceViewChanged);
                    this.currentViewValid = true;
                }
            }
            return this.currentView;
        }

        void OnDataSourceViewSelectCallback(IEnumerable data)
        {
            this.ignoreDataSourceViewChanged = false;
            this.PerformDataBinding(data);
            this.OnDataBound(EventArgs.Empty);
        }

        /// <summary>
        /// 如果未指定参数，则创建由数据绑定控件使用的默认 <see cref="DataSourceSelectArgumentsEx"/> 对象。
        /// </summary>
        /// <returns></returns>
        protected virtual DataSourceSelectArgumentsEx CreateDataSourceSelectArguments()
        {
            return DataSourceSelectArgumentsEx.Empty;
        }
        /// <summary>
        /// 检索数据绑定控件用于执行数据操作的 <see cref="DataSourceViewEx"/> 对象。
        /// </summary>
        /// <returns></returns>
        protected virtual DataSourceViewEx GetData()
        {
            return this.ConnectToDataSourceView();
        }
        /// <summary>
        /// 检索与数据绑定控件关联的 <see cref="IDataSource"/> 接口（如果有）。
        /// </summary>
        /// <returns></returns>
        protected virtual IDataSourceEx GetDataSource()
        {
            if ((!base.DesignMode && this.currentDataSourceValid) && (this.currentDataSource != null))
                return this.currentDataSource;
            return null;
        }
        /// <summary>
        /// 将视图状态中的控件状态设置为成功绑定到数据。
        /// </summary>
        protected void MarkAsDataBound()
        {
            this.ViewState[DataBoundViewStateKey] = true;
        }

        /// <summary>
        /// 在派生类中重写时，将数据源中的数据绑定到控件。
        /// </summary>
        /// <param name="data"></param>
        protected virtual void PerformDataBinding(IEnumerable data)
        {
        }
    }
}
