//================================================================================
//  FileName: IServiceContainer.cs
//  Desc:提供服务的容器。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010-3-19
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

namespace iPower
{
    /// <summary>
    /// 提供一种回调机制，它可以即需创建服务的实例。
    /// </summary>
    /// <param name="container">请求创建服务的服务容器。</param>
    /// <param name="serviceType">要创建的服务的类型。</param>
    /// <returns>由 serviceType 指定的服务；如果未能创建服务，则为 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing）。</returns>
    public delegate Object ServiceCreatorCallback(IServiceContainer container,Type serviceType);
    /// <summary>
    /// 提供服务的容器。
    /// </summary>
    public interface IServiceContainer : IServiceProvider
    {
        /// <summary>
        /// 检查服务容器中服务类型是否存在。
        /// </summary>
        /// <param name="serviceType">服务类型。</param>
        /// <returns>是否存在。</returns>
        bool HasService(Type serviceType);
        /// <summary>
        /// 将指定的服务添加到服务容器中。
        /// </summary>
        /// <param name="serviceType">要添加的服务类型。</param>
        /// <param name="callback">用于创建服务的回调对象。这允许将服务声明为可用，但将对象的创建延迟到请求该服务之后。</param>
        void AddService(Type serviceType,ServiceCreatorCallback callback);
        /// <summary>
        /// 将指定的服务添加到服务容器中。
        /// </summary>
        /// <param name="serviceType">要添加的服务类型。</param>
        /// <param name="serviceInstance">要添加的服务类型的实例。此对象必须实现 serviceType 参数指示的类型或从其继承。</param>
        void AddService(Type serviceType, Object serviceInstance);
        /// <summary>
        /// 从服务容器中移除指定的服务类型。
        /// </summary>
        /// <param name="serviceType">要移除的服务类型。</param>
        void RemoveService(Type serviceType);
    }
}
