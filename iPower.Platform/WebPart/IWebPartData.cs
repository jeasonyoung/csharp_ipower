//================================================================================
//  FileName: IWebPartData.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/6/30
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
using System.Data;

using iPower.Data;
namespace iPower.Platform.WebPart
{
    /// <summary>
    /// WebPart组件的数据提供接口，为相应的WebPart组件提供相应的数据。
    /// </summary>
    public interface IWebPartData
    {
        /// <summary>
        /// 获取动态文本数据源。
        /// </summary>
        /// <param name="employeeID">用户ID。</param>
        /// <returns>动态文本数据。</returns>
        string DynamicTextData(string employeeID);
        /// <summary>
        /// 获取WebPart组件数据。
        /// </summary>
        /// <param name="employeeID">用户ID。</param>
        /// <param name="dataType">数据类型。</param>
        /// <returns></returns>
        WebPartDataCollection DataSource(string employeeID, string dataType);
    }

    #region 数据。
    /// <summary>
    /// WebPart数据。
    /// </summary>
    public class WebPartData
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="title">显示数据。</param>
        /// <param name="url">URL数据。</param>
        public WebPartData(string title, string url)
        {
            this.Title = title;
            this.Url = url;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WebPartData()
        {
        }
        #endregion

        /// <summary>
        /// 获取或设置显示数据。
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 获取或设置URL数据。
        /// </summary>
        public string Url { get; set; }
    }
    /// <summary>
    /// WebPart数据集合。
    /// </summary>
    public class WebPartDataCollection : DataCollection<WebPartData>
    {

    }
    #endregion
}
