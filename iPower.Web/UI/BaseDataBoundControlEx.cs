//================================================================================
//  FileName: BaseDataBoundControlEx.cs
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
    /// 用作控件的基类。
    /// </summary>
    public abstract class BaseDataBoundControlEx :WebControl
    {
        #region 成员变量，构造函数。
        bool inited, requiresDataBinding;
        object dataSource;
        /// <summary>
        /// 构造函数。
        /// </summary>
        protected BaseDataBoundControlEx()
        {
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 在服务器控件绑定到数据源后发生。
        /// </summary>
        [Category("Data")]
        [Description("在服务器控件绑定到数据源后发生。")]
        public event EventHandler DataBound;

        /// <summary>
        /// 引发<see cref="DataBound"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDataBound(EventArgs e)
        {
            EventHandler handler = this.DataBound;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// 给控件提供数据源的事件。
        /// </summary>
        [Category("Action")]
        [Description("给控件提供数据源的事件。")]
        public event EventHandler BuildDataSource;
        /// <summary>
        ///  触发<see cref="BuildDataSource"/>事件。
        /// </summary>
        /// <param name="e">包含事件数据的对象。</param>
        protected virtual void OnBuildDataSource(EventArgs e)
        {
            EventHandler handler = this.BuildDataSource;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置一个值，该值指示是否应调用<see cref="DataBind"/> 方法
        /// </summary>
        protected bool RequiresDataBinding
        {
            get
            {
                return this.requiresDataBinding;
            }
            set
            {
                if (value && this.Page != null && !this.Page.IsCallback)
                {
                    this.requiresDataBinding = true;
                    this.DataBind();
                }
                else
                    this.requiresDataBinding = value;
            }
        }
        /// <summary>
        /// 获取或设置对象，数据绑定控件从该对象中检索其数据项列表。
        /// </summary>
        [Bindable(true)]
        [Themeable(false)]
        [Category("Data")]
        [Description("获取或设置对象，数据绑定控件从该对象中检索其数据项列表。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object DataSource
        {
            get { return this.dataSource; }
            set
            {
                if (value != null)
                    this.ValidateDataSource(value);
                this.dataSource = value;
                this.OnDataPropertyChanged();
            }
        }

        /// <summary>
        /// 设置指示数据绑定控件是否已经初始化。
        /// </summary>
        protected bool Initialized
        {
            get { return this.inited; }
        }
        bool SupportsEventValidation
        {
            get
            {
                return SupportsEventValidationExAttribute.SupportsEventValidation(this.GetType());
            }
        }
        #endregion

        /// <summary>
        /// 设置数据绑定控件的初始化状态。
        /// </summary>
        protected void ConfirmInitState()
        {
            this.inited = true;
        }

        #region 重载。
        /// <summary>
        /// 将数据源绑定到被调用的服务器控件及其所有子控件。
        /// </summary>
        public override void DataBind()
        {
            if (base.DesignMode)
            {
                //设计时
                IDictionary designModeState = this.GetDesignModeState();
                if (((designModeState == null) || designModeState["EnableDesignTimeDataBinding"] == null) && (base.Site == null))
                    return;
            }
            this.PerFormSelect();
        }
        /// <summary>
        /// 处理<see cref="Control.Init"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null)
            {
                this.Page.PreLoad += new EventHandler(this.OnPagePreLoad);
                if (!base.IsViewStateEnabled && this.Page.IsPostBack)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }
        /// <summary>
        /// 处理<see cref="Control.PreRender"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        #endregion

        /// <summary>
        /// 在加载数据绑定控件之前设置该控件的初始化状态。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPagePreLoad(object sender, EventArgs e)
        {
            this.inited = true;
            if (this.Page != null)
            {
                this.Page.PreLoad -= new EventHandler(this.OnPagePreLoad);
            }
        }

        /// <summary>
        /// 在某一基数据源标识属性更改后，将数据绑定控件重新绑定到其数据。
        /// </summary>
        protected virtual void OnDataPropertyChanged()
        {
            if (this.inited)
                this.RequiresDataBinding = true;
        }

        #region 抽象函数。
        /// <summary>
        /// 当在派生类中重写时，控制如何检索数据并将数据绑定到控件。
        /// </summary>
        protected abstract void PerFormSelect();
        /// <summary>
        /// 当在派生类中重写时，验证数据绑定控件绑定到的对象是它可以使用的对象。
        /// </summary>
        /// <param name="dataSource">数据绑定控件绑定到的对象。</param>
        protected abstract void ValidateDataSource(object dataSource);
        #endregion

        /// <summary>
        /// 验证客户端脚本事件。
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <param name="eventArgument"></param>
        public void ValidateEvent(string uniqueID, string eventArgument)
        {
            if (this.Page != null && this.SupportsEventValidation)
                this.Page.ClientScript.ValidateEvent(uniqueID, eventArgument);
        }
    }
}
