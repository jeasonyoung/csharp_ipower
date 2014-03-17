//================================================================================
//  FileName: IDataHandler.cs
//  Desc:页面数据处理接口。
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

namespace iPower.Platform
{
    /// <summary>
    /// 页面数据处理接口。
    /// </summary>
    public interface IDataHandler
    {
        /// <summary>
        /// 数据绑定。
        /// </summary>
        void DataBind();
        /// <summary>
        /// 数据加载。
        /// </summary>
        void LoadData();
        /// <summary>
        /// 数据保存。
        /// </summary>
        /// <returns>是否保存成功。</returns>
        bool SaveData();
        /// <summary>
        /// 数据删除。
        /// </summary>
        /// <returns>是否成功删除数据。</returns>
        bool DeleteData();
    }
}
