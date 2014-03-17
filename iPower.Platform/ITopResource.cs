//================================================================================
//  FileName: ITopResource.cs
//  Desc:资源数据接口。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-23
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

namespace iPower.Platform
{
    /// <summary>
    /// 资源数据接口。
    /// </summary>
    public interface ITopResource
    {
        /// <summary>
        /// 获取或设置版权名称。
        /// </summary>
        string CopyRight
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置Benner头图片路径。
        /// </summary>
        string[] BannerImageUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置Logo图片路径。
        /// </summary>
        string[] LogoImageUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置特效图片URL。
        /// </summary>
        string EffectImageURL
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置样式表资源集合。
        /// </summary>
        string[] WebCssPaths
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置脚本资源集合。
        /// </summary>
        string[] WebScriptPaths
        {
            get;
            set;
        }
    }
}
