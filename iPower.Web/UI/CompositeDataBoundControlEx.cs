//================================================================================
//  FileName: CompositeDataBoundControlEx.cs
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

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iPower.Web.UI
{
    /// <summary>
    /// 由其他服务器控件组成的表格数据绑定控件的基类。
    /// </summary>
    public abstract class CompositeDataBoundControlEx : DataBoundControlEx, INamingContainer
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 
        /// </summary>
        public const string ItemCountViewStateKey = "_!ItemCount";
        /// <summary>
        /// 构造函数。
        /// </summary>
        protected CompositeDataBoundControlEx()
        {
        }
        #endregion

        /// <summary>
        /// 获取复合数据绑定控件内的子控件的集合。
        /// </summary>
        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }
        #region 重载。
        /// <summary>
        /// 创建用于呈现复合数据绑定控件的控件层次结构。
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.OnBuildDataSource(EventArgs.Empty);
            object dataSource = this.DataSource;
            if (dataSource != null)
                dataSource = this.DataSourceSorting(dataSource);
            this.CreateChildControls(DataSourceHelper.GetResolvedDataSource(dataSource, this.DataMember), false);
            this.ClearChildViewState();
        }
        /// <summary>
        /// 将数据源中的数据绑定到复合数据绑定控件。
        /// </summary>
        /// <param name="data"></param>
        protected override void PerformDataBinding(IEnumerable data)
        {
            base.PerformDataBinding(data);
            this.Controls.Clear();
            this.ClearChildViewState();
            this.TrackViewState();
            int num = this.CreateChildControls(data, true);
            this.ChildControlsCreated = true;
            this.ViewState[ItemCountViewStateKey] = num;
        }
        #endregion

        /// <summary>
        /// 由 ASP.NET 页面框架调用，以通知使用基于合成的实现的服务器控件创建它们包含的任何子控件，以便为回发或呈现做准备。
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataBinding"></param>
        /// <returns></returns>
        protected abstract int CreateChildControls(IEnumerable dataSource, bool dataBinding);

        /// <summary>
        /// 给数据源排序。
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        protected abstract object DataSourceSorting(object dataSource);
    }
}
