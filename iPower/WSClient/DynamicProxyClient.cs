//================================================================================
//  FileName: DynamicProxyClient.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-11
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
using System.Net;
using System.IO;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using Microsoft.CSharp;
using iPower.Cryptography;
namespace iPower.WSClient
{
    /// <summary>
    /// 动态代理客户端。
    /// </summary>
    public class DynamicProxyClient : IDisposable
    {
        #region 成员变量，构造函数。
        private static Hashtable assemblyCache = Hashtable.Synchronized(new Hashtable()),
            assemblyTypeCache = Hashtable.Synchronized(new Hashtable()),
            assemblyInstanceCache = Hashtable.Synchronized(new Hashtable());
        private const string CONST_DEFAULT_NAMESPACE = "iPower.WSClient.Proxy";
        private string url, key;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="url">WebService URL</param>
        public DynamicProxyClient(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            this.url = url;
            this.key = HashCrypto.Hash(this.url, "md5");
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 加载程序集。
        /// </summary>
        /// <returns></returns>
        protected Assembly LoadAssembly()
        {
            Assembly assembly = assemblyCache[this.key] as Assembly;
            if (assembly == null)
            {
                lock (this)
                {
                    using (WebClient client = new WebClient())
                    {
                        //下载WSDL信息。
                        using (Stream stream = client.OpenRead(this.url))
                        {
                            //创建和格式化WSDL文档。
                            ServiceDescription sd = ServiceDescription.Read(stream);
                            //创建客户端代理类。
                            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                            //添加WSDL。
                            sdi.AddServiceDescription(sd, "", "");
                            //编译代理类。
                            CodeNamespace cn = new CodeNamespace(CONST_DEFAULT_NAMESPACE);//为代理类添加缺省全局命名空间。
                            CodeCompileUnit ccu = new CodeCompileUnit();
                            ccu.Namespaces.Add(cn);
                            sdi.Import(cn, ccu);
                           
                            CompilerParameters cplist = new CompilerParameters();
                            cplist.GenerateExecutable = false;
                            cplist.GenerateInMemory = true;
                            cplist.ReferencedAssemblies.Add("System.dll");
                            cplist.ReferencedAssemblies.Add("System.XML.dll");
                            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                            cplist.ReferencedAssemblies.Add("System.Data.dll");

                            CSharpCodeProvider provider = new CSharpCodeProvider();
                            CompilerResults cr = provider.CompileAssemblyFromDom(cplist, ccu);
                            if (cr.Errors.HasErrors)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (CompilerError ce in cr.Errors)
                                {
                                    sb.AppendFormat("{0}", ce);
                                    sb.AppendLine();
                                }
                                throw new Exception(sb.ToString());
                            }
                            assembly = cr.CompiledAssembly;
                            if (assembly != null)
                            {
                                assemblyCache[key] = assembly;
                            }
                        }
                    }
                }
            }
            return assembly;
        }
        /// <summary>
        /// 根据类名称在程序集中创建类对象实例。
        /// </summary>
        /// <param name="className"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object CreateClassInstance(string className, out Type type)
        {
            if (string.IsNullOrEmpty(className))
            {
                throw new ArgumentNullException("className");
            }
            lock (this)
            {
                string[] array = className.Split('.');
                string strType = string.Format("{0}.{1}", CONST_DEFAULT_NAMESPACE, array[array.Length - 1]);
                type = assemblyTypeCache[strType] as Type;

                #region 类型。
                if (type == null)
                {
                    Assembly assembly = this.LoadAssembly();
                    if (assembly != null)
                    {
                        type = assembly.GetType(strType, true, true);
                        if (type != null)
                        {
                            assemblyTypeCache[strType] = type;
                        }
                    }
                }
                #endregion

                object instance = null;
                #region 对象实例。
                if (type != null)
                {
                    instance = assemblyInstanceCache[type];
                    if (instance == null)
                    {
                        instance = Activator.CreateInstance(type);
                        if (instance != null)
                        {
                            assemblyInstanceCache[type] = instance;
                        }
                    }
                }
                #endregion
                return instance;
            }
        }
        /// <summary>
        /// 方法调用。
        /// </summary>
        /// <param name="className">类名。</param>
        /// <param name="method">方法名。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回值。</returns>
        public object Invoke(string className, string method, params object[] args)
        {
            if (className == null)
            {
                throw new ArgumentNullException("className");
            }
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            Type t = null;
            object instance = this.CreateClassInstance(className, out t);
            if (t == null)
            {
                throw new ArgumentException("类型名称[" + className + "]不存在！");
            }
            MethodInfo m = t.GetMethod(method);
            if (m == null)
            {
                throw new ArgumentException("方法名称[" + method + "]不存在！");
            }
            return m.Invoke(instance, args);
        }
        /// <summary>
        /// 方法调用。
        /// </summary>
        /// <param name="method">方法名。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回值。</returns>
        public object Invoke(string method, params object[] args)
        {
            return this.Invoke(this.loadClassName(), method, args);
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 获取类类型名称。
        /// </summary>
        /// <returns></returns>
        private string loadClassName()
        {
            if (!string.IsNullOrEmpty(this.url))
            {
                string[] parts = this.url.Split('/');
                string p = parts[parts.Length - 1];
                if (!string.IsNullOrEmpty(p))
                {
                    return p.Split(new char[] { '.', '?' })[0];
                }
            }
            return null;
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            assemblyInstanceCache.Clear();
            assemblyTypeCache.Clear();
            assemblyCache.Clear();
        }
        #endregion
    }
}