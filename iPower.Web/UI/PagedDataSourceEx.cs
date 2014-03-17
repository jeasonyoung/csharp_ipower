//================================================================================
//  FileName: PagedDataSourceEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/8
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
using System.Globalization;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;

using iPower.Utility;
namespace iPower.Web.UI
{
    /// <summary>
    /// 数据绑定控的与分页相关的属性，以允许该控件执行分页操作。无法继承此类。
    /// </summary>
    public sealed class PagedDataSourceEx : ICollection, IEnumerable, ITypedList
    {
        #region 成员变量，构造函数。
        bool allowPaging, allowServerPaging, allowCustomPaging;
        int currentPageIndex, pageSize, virtualCount;
        IEnumerable dataSource;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PagedDataSourceEx()
        {
            this.pageSize = 10;
            this.currentPageIndex = this.virtualCount = 0;
            this.allowPaging = this.allowServerPaging = this.allowCustomPaging = false;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置启用自定义分页。
        /// </summary>
        public bool AllowCustomPaging
        {
            get { return this.allowCustomPaging; }
            set { this.allowCustomPaging = value; }
        }
        /// <summary>
        /// 获取或设置启用分页。
        /// </summary>
        public bool AllowPaging
        {
            get { return this.allowPaging; }
            set { this.allowPaging = value; }
        }
        /// <summary>
        /// 获取或设置是否启用服务器端分页。
        /// </summary>
        public bool AllowServerPaging
        {
            get { return this.allowServerPaging; }
            set { this.allowServerPaging = value; }
        }
        /// <summary>
        /// 获取是否启用分页。
        /// </summary>
        public bool IsPagingEnabled
        {
            get { return this.allowPaging && (this.pageSize != 0); }
        }
        /// <summary>
        /// 获取是否启用自定义分页。
        /// </summary>
        public bool IsCustomPagingEnabled
        {
            get { return this.IsPagingEnabled && this.allowCustomPaging; }
        }
        /// <summary>
        /// 获取是否启用服务器端分页支持。
        /// </summary>
        public bool IsServerPagingEnabled
        {
            get { return this.IsPagingEnabled && this.allowServerPaging; }
        }
        /// <summary>
        /// 获取当前页是否是首页。
        /// </summary>
        public bool IsFirstPage
        {
            get
            {
                if (this.IsPagingEnabled)
                    return this.currentPageIndex == 0;
                return true;
            }
        }
        /// <summary>
        /// 获取当前页是否是最后一页。
        /// </summary>
        public bool IsLastPage
        {
            get
            {
                if (this.IsPagingEnabled)
                    return this.CurrentPageIndex == (this.PageCount - 1);
                return true;
            }
        }
        /// <summary>
        ///  获取或设置当前页的索引。
        /// </summary>
        public int CurrentPageIndex
        {
            get { return this.currentPageIndex; }
            set { this.currentPageIndex = value; }
        }
        /// <summary>
        /// 获取或设置数据源。
        /// </summary>
        public IEnumerable DataSource
        {
            get { return this.dataSource; }
            set { this.dataSource = value; }
        }
        /// <summary>
        /// 获取数据源中的项数。
        /// </summary>
        public int DataSourceCount
        {
            get
            {
                if (this.dataSource == null)
                    return 0;
                if (this.IsCustomPagingEnabled || this.IsServerPagingEnabled)
                    return this.virtualCount;
                if (!(this.dataSource is ICollection))
                    throw new HttpException("PagedDataSourceEx_Cannot_Get_Count");
                return ((ICollection)this.dataSource).Count;
            }
        }
        /// <summary>
        /// 获取页面中显示的首条记录的索引。
        /// </summary>
        public int FirstIndexInPage
        {
            get
            {
                if ((this.dataSource != null) && this.IsPagingEnabled && !this.IsCustomPagingEnabled && !this.IsServerPagingEnabled)
                    return this.currentPageIndex * this.pageSize;
                return 0;
            }
        }
        /// <summary>
        /// 获取总页数。
        /// </summary>
        public int PageCount
        {
            get
            {
                if (this.dataSource == null)
                    return 0;
                int dataSourceCount = this.DataSourceCount;
                if (!this.IsPagingEnabled || (dataSourceCount <= 0))
                    return 1;
                int sum = (dataSourceCount + this.pageSize) - 1;
                if (sum < 0)
                    return 1;
                return sum / this.pageSize;
            }
        }
        /// <summary>
        /// 获取或设置要在单页上显示的项数。
        /// </summary>
        public int PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }
        /// <summary>
        /// 获取或设置在使用自定义分页时数据源中的实际项数。
        /// </summary>
        public int VirtualCount
        {
            get { return this.virtualCount; }
            set { this.virtualCount = value; }
        }
        #endregion

        #region ICollection 成员
        /// <summary>
        /// 复制。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
                array.SetValue(enumerator.Current, index++);
        }
        /// <summary>
        /// 获取要从数据源使用的项数。
        /// </summary>
        public int Count
        {
            get
            {
                if (this.dataSource == null)
                    return 0;
                if (!this.IsPagingEnabled)
                    return this.DataSourceCount;
                if (!this.IsCustomPagingEnabled && this.IsLastPage)
                    return this.DataSourceCount - this.FirstIndexInPage;
                return this.pageSize;
            }
        }
        /// <summary>
        /// 获取是否同步对数据源的访问（线程安全）。
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }
        /// <summary>
        /// 获取可用于同步集合访问的对象。
        /// </summary>
        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region IEnumerable 成员
        /// <summary>
        /// 包含数据源中的所有项。
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            int firstIndexInPage = this.FirstIndexInPage;
            int count = -1;
            if (this.dataSource is ICollection)
                count = this.Count;
            if (this.dataSource is IList)
                return new EnumeratorOnIList((IList)this.dataSource, firstIndexInPage, count);
            if (this.dataSource is Array)
                return new EnumeratorOnArray((object[])this.dataSource, firstIndexInPage, count);
            if (this.dataSource is ICollection)
                return new EnumeratorOnICollection((ICollection)this.dataSource, firstIndexInPage, count);
            if (!this.allowCustomPaging && !this.allowServerPaging)
                return this.dataSource.GetEnumerator();
            return new EnumeratorOnIEnumerator(this.dataSource.GetEnumerator(), this.Count);
        }

        #endregion

        #region ITypedList 成员
        /// <summary>
        /// 返回表示用于绑定数据的每项上属性。
        /// </summary>
        /// <param name="listAccessors"></param>
        /// <returns></returns>
        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if ((this.dataSource != null) && (this.dataSource is ITypedList))
                return ((ITypedList)this.dataSource).GetItemProperties(listAccessors);
            return null;
        }
        /// <summary>
        /// 返回列表的名称。此方法不适用于此类。
        /// </summary>
        /// <param name="listAccessors"></param>
        /// <returns></returns>
        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return string.Empty;
        }

        #endregion
    }
}
