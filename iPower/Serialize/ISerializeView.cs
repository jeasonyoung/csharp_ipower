//================================================================================
//  FileName: ISerializeView.cs
//  Desc:序列化操作接口。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-29
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
using System.Xml;
namespace iPower.Serialize
{
    /// <summary>
    /// 序列化操作接口。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISerializeView<T>
        where T: class
    {
        /// <summary>
        /// 序列化。
        /// </summary>
        void Serialize();
        /// <summary>
        /// 反序列化。
        /// </summary>
        T Deserialize();
    }

    /// <summary>
    /// 序列化操作接口。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IXmlSerializeView<T> : ISerializeView<T>
        where T:class
    {
        /// <summary>
        /// 加载Xml。
        /// </summary>
        /// <returns></returns>
        XmlDocument LoadXmlDocument();
    }
}
