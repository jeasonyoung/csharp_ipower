//================================================================================
//  FileName: EntityEventArgs.cs
//  Desc:事件数据类。
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

namespace iPower.Platform.Engine.Service
{
    /// <summary>
    /// 事件数据类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityEventArgs<T> : EventArgs
    {
        #region 成员变量，构造函数。
        T entity;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="entity">实体类。</param>
        public EntityEventArgs(T entity)
        {
            this.entity = entity;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取实体数据。
        /// </summary>
        public T Entity
        {
            get { return this.entity; }
        }
        #endregion
    }
}
