//================================================================================
//  FileName: IWebPartMgr.cs
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

using iPower.Data;
namespace iPower.Platform.WebPart
{
    /// <summary>
    /// WebPart管理程序接口
    /// 获取WebPart列表接口。
    /// </summary>
    public interface IWebPartMgr
    {
        /// <summary>
        /// 获取某个位置的WebPart集合。
        /// </summary>
        /// <param name="zoneMode">WebPart组件显示的位置。</param>
        /// <param name="systemID">当前系统ID。</param>
        /// <param name="employeeID">用户ID。</param>
        /// <returns></returns>
        WebPartQueryCollection QueryList(EnumWebPartAlignment zoneMode, string systemID, string employeeID);
        /// <summary>
        /// 获取WebPart属性数据集。
        /// </summary>
        /// <param name="personalWebPartID">WebPart配置的唯一标识。</param>
        /// <returns></returns>
        WebPartPropertyCollection QueryProperties(string personalWebPartID);
    }

    #region WebPart。
    /// <summary>
    /// WebPart布局枚举。
    /// </summary>
    public enum EnumWebPartAlignment
    {
        /// <summary>
        /// 左
        /// </summary>
        Left = 0x00,
        /// <summary>
        /// 中
        /// </summary>
        Middle = 0x001,
        /// <summary>
        /// 右
        /// </summary>
        Right = 0x002,
    }
    /// <summary>
    /// WebPart信息数据。
    /// </summary>
    public class WebPartQuery
    {
        /// <summary>
        /// 获取或设置WebPart配置的唯一标识。
        /// </summary>
        public string PersonalWebPartID { get; set; }
        /// <summary>
        /// 获取或设置WebPart路径。
        /// </summary>
        public string WebPartPath { get; set; }
        /// <summary>
        /// 获取或设置程序集名称。
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 获取或设置类名称。
        /// </summary>
        public string ClassName { get; set; }
    }
    /// <summary>
    /// WebPart信息和属性数据。
    /// </summary>
    public class WebPartQueryProperties : WebPartQuery
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WebPartQueryProperties()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="webPartQuery"></param>
        public WebPartQueryProperties(WebPartQuery webPartQuery)
        {
            if (webPartQuery != null)
            {
                this.PersonalWebPartID = webPartQuery.PersonalWebPartID;
                this.WebPartPath = webPartQuery.WebPartPath;
                this.AssemblyName = webPartQuery.AssemblyName;
                this.ClassName = webPartQuery.ClassName;
                this.WebPartProperties = new WebPartPropertyCollection();
            }
        }
        #endregion
        /// <summary>
        /// 获取或设置WebPart属性集合。
        /// </summary>
        public WebPartPropertyCollection WebPartProperties { get; set; }
    }
    /// <summary>
    /// WebPart信息数据集合。
    /// </summary>
    public class WebPartQueryCollection : DataCollection<WebPartQuery>
    {
        #region 函数。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalWebPartID"></param>
        /// <returns></returns>
        public virtual WebPartQuery this[string personalWebPartID]
        {
            get
            {
                if (string.IsNullOrEmpty(personalWebPartID))
                    return null;
                WebPartQuery query = this.Items.Find(new Predicate<WebPartQuery>(delegate(WebPartQuery data)
                {
                    return (data != null) && (data.PersonalWebPartID == personalWebPartID);
                }));
                return query;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Contains(WebPartQuery item)
        {
            if (item != null)
            {
                return this[item.PersonalWebPartID] != null;
            }
            return false;
        }
        #endregion
    }
    /// <summary>
    ///  WebPart信息和属性数据集合。
    /// </summary>
    public class WebPartQueryPropertiesCollection : DataCollection<WebPartQueryProperties>
    {
        #region 函数。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalWebPartID"></param>
        /// <returns></returns>
        public virtual WebPartQueryProperties this[string personalWebPartID]
        {
            get
            {
                if (string.IsNullOrEmpty(personalWebPartID))
                    return null;
                WebPartQueryProperties query = this.Items.Find(new Predicate<WebPartQueryProperties>(delegate(WebPartQueryProperties data)
                {
                    return (data != null) && (data.PersonalWebPartID == personalWebPartID);
                }));
                return query;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Contains(WebPartQueryProperties item)
        {
            if (item != null)
            {
                return this[item.PersonalWebPartID] != null;
            }
            return false;
        }
        #endregion
    }
    #endregion

    #region WebPartProperty。
    /// <summary>
    /// WebPart属性数据。
    /// </summary>
    public class WebPartProperty
    {
        /// <summary>
        /// 获取或设置属性名称。
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 获取或设置属性值。
        /// </summary>
        public string PropertyValue { get; set; }
    }
    /// <summary>
    /// WebPart属性数据集合。
    /// </summary>
    public class WebPartPropertyCollection : DataCollection<WebPartProperty>
    {
        /// <summary>
        /// 根据属性名称获取数据对象。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual WebPartProperty this[string propertyName]
        {
            get
            {
                WebPartProperty property = null;
                if (!string.IsNullOrEmpty(propertyName))
                {
                    property = this.Items.Find(new Predicate<WebPartProperty>(delegate(WebPartProperty data)
                    {
                        return (data != null) && (data.PropertyName == propertyName);
                    }));
                }
                return property;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Contains(WebPartProperty item)
        {
            if (item != null)
            {
                return this[item.PropertyName] != null;
            }
            return false;
        }
    }
    #endregion
}
