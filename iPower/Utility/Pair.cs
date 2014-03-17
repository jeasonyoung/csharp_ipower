//================================================================================
//  FileName: Pair.cs
//  Desc:提供用于存储两个相关对象的基本实用工具类。 
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010/4/12
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

namespace iPower.Utility
{
    /// <summary>
    /// 提供用于存储两个相关对象的基本实用工具类。 
    /// </summary>
    public class Pair<TFirst,TSecond>
    {
        #region 成员变量，构造函数。
        readonly TFirst first;
        readonly TSecond second;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Pair(TFirst first, TSecond second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            this.first = first;
            this.second = second;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取TFirst。
        /// </summary>
        public TFirst First
        {
            get { return this.first; }
        }
        /// <summary>
        /// 获取TSecond。
        /// </summary>
        public TSecond Second
        {
            get { return this.second; }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            Pair<TFirst, TSecond> pair = obj as Pair<TFirst, TSecond>;
            return ((pair != null) && pair.first.Equals(this.first)) && pair.second.Equals(this.second);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = this.first.GetHashCode();
            return ((hashCode << 5) + hashCode) ^ this.second.GetHashCode();
        }
        #endregion
    }
}
