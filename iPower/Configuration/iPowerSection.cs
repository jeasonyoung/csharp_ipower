//================================================================================
//  FileName: JeasonSection.cs
//  Desc:配置映射类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-28
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

using System.Configuration;
namespace iPower.Configuration
{
    /// <summary>
    /// 配置映射类 <see cref="ConfigurationSection"/>。
    /// </summary>
    /// <example>
    /// <code>
    /// &lt;configSections&gt;
    /// &lt;section name="xxx" type="iPower.Configuration.JeasonSection,iPower"/&gt;
    /// &lt;/configSections&gt;
    /// </code>
    /// </example>
    public class iPowerSection : ConfigurationSection
    {
        #region 构造函数，析构函数
        static ConfigurationProperty keyValueProperty;

        /// <summary>
        /// 静态函数。
        /// </summary>
        static iPowerSection()
        {
            keyValueProperty = new ConfigurationProperty(null, typeof(KeyValueConfigurationCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public iPowerSection()
            : base()
        {
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取key-value配置集合。
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public KeyValueConfigurationCollection Settings
        {
            get
            {
                return this[keyValueProperty] as KeyValueConfigurationCollection;
            }
        }
        #endregion
    }
}
