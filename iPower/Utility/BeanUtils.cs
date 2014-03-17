//================================================================================
//  FileName: BeanUtils.cs
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
using System.Reflection;

using iPower.Cryptography;
namespace iPower.Utility
{
    /// <summary>
    /// 对象
    /// </summary>
    public static class BeanUtils
    {
        /// <summary>
        /// 将源的字段赋予目标类型对象。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="source">源对象实例。</param>
        /// <returns>目标对象。</returns>
        public static T copyFieldsToProperties<T>(Object source)
            where T : class
        {
            if (source != null)
            {
                Type s = source.GetType(), t = typeof(T);
                if (s.IsArray && !t.IsArray)//数组。
                {
                    throw new ArgumentException("泛型应为数组类型" + t.FullName);
                }
                Object result = copyFieldsToProperties(source, t);
                return result as T;
            }
            return default(T);
        }
        /// <summary>
        /// 将源的字段赋予目标类型对象。
        /// </summary>
        /// <param name="source">源对象。</param>
        /// <param name="target">目标类型。</param>
        /// <returns>目标对象。</returns>
        public static Object copyFieldsToProperties(Object source, Type target)
        {
            if (source != null && target != null)
            {
                Type s = source.GetType();
                if (s.IsArray)
                {
                    Type t = target.IsArray ? target.GetElementType() : target;
                    List<Object> list = new List<object>();
                    Object[] sources = source as Object[];
                    if (sources != null && sources.Length > 0)
                    {
                        for (int i = 0; i < sources.Length; i++)
                        {
                            list.Add(copyFieldsToProperties(sources[i], t));
                        }
                    }
                    if (list.Count > 0)
                    {
                        Array array = Array.CreateInstance(t, list.Count);
                        for (int i = 0; i < list.Count; i++)
                        {
                            array.SetValue(list[i], i);
                        }
                        return array;
                    }
                }
                else
                {
                    Hashtable cache = Hashtable.Synchronized(new Hashtable());
                    foreach (FieldInfo fi in s.GetFields())
                    {
                        if (fi.IsPublic)
                        {
                            cache[fi.Name.ToLower()] = fi;
                        }
                    }

                    #region 赋值。
                    Object result = Activator.CreateInstance(target);
                    foreach (PropertyInfo tp in target.GetProperties())
                    {
                        if (tp.CanWrite)
                        {
                            FieldInfo sf = cache[tp.Name.ToLower()] as FieldInfo;
                            if (sf != null)
                            {
                                if (sf.FieldType.IsArray)//目标和源都是数组类型。
                                {
                                    Object[] values = copyFieldsToProperties(sf.GetValue(source), tp.PropertyType) as Object[];
                                    if (values != null && values.Length > 0)
                                    {
                                        Array array = Array.CreateInstance(tp.PropertyType.GetElementType(), values.Length);
                                        for (int i = 0; i < values.Length; i++)
                                        {
                                            array.SetValue(values[i], i);
                                        }
                                        tp.SetValue(result, array, null);
                                    }
                                }
                                else
                                {
                                    object value = sf.GetValue(source);
                                    tp.SetValue(result, value, null);
                                }
                            }
                        }
                    }
                    #endregion

                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 计算属性的签名。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string computeSignProperties(Object data)
        {
            return computeSignProperties(data, null);
        }
        /// <summary>
        /// 计算属性的签名。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string computeSignProperties(Object data, string[] ignoreNames)
        {
            string result = null;
            if (data != null)
            {
                Type t = data.GetType();
                StringBuilder sb = new StringBuilder();
                foreach (PropertyInfo p in t.GetProperties())
                {
                    if (p.CanRead && !IsIgnore(ignoreNames, p.Name))
                    {
                        sb.AppendFormat("{0}$", p.GetValue(data, null));
                    }
                }
                if (sb.Length > 0)
                {
                    result = HashCrypto.Hash(sb.ToString(), "md5");
                }
            }
            return result;
        }
        /// <summary>
        /// 复制属性。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void copyProperties(Object source, Object target)
        {
            copyProperties(source, target, null);
        }
        /// <summary>
        /// 复制属性值。
        /// </summary>
        /// <param name="source">复制源。</param>
        /// <param name="target">目标。</param>
        /// <param name="ignoreNames">忽略的字段组数。</param>
        public static void copyProperties(Object source, Object target, string[] ignoreNames)
        {
            if (source != null && target != null)
            {
                Type s = source.GetType(), t = target.GetType();
                foreach (PropertyInfo p in t.GetProperties())
                {
                    string colName = p.Name;
                    if (p.CanWrite && !IsIgnore(ignoreNames, colName))
                    {
                        PropertyInfo tp = s.GetProperty(p.Name);
                        if (tp != null && tp.CanRead)
                        {
                            object value = tp.GetValue(source, null);
                            p.SetValue(target, value, null);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 是否忽略。
        /// </summary>
        /// <param name="ignoreNames"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        private static bool IsIgnore(string[] ignoreNames, string colName)
        {
            if (ignoreNames != null && ignoreNames.Length > 0 && !string.IsNullOrEmpty(colName))
            {
                bool result = Array.Exists<String>(ignoreNames, new Predicate<string>(delegate(string v)
                {
                    return string.Equals(v, colName, StringComparison.InvariantCultureIgnoreCase);
                }));
                return result;
            }
            return false;
        }
    }
}
