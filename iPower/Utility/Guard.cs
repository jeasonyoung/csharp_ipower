//================================================================================
//  FileName: Guard.cs
//  Desc:关于参数验证的一些实用方法。
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
using System.Globalization;

namespace iPower
{
    /// <summary>
    /// 关于参数验证的一些实用方法。
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// 检查参数是否为空引用（null）。
        /// </summary>
        /// <param name="argumentName">字符串名。</param>
        /// <param name="argumentValue">被检查的字符串。.</param>
        /// <param name="throwError">为空时是否抛出异常。</param>
        /// <returns>为空则返回true。</returns>
        public static bool ArgumentNotNull(string argumentName, object argumentValue, bool throwError)
        {
            if (string.IsNullOrEmpty(argumentName))
            {
                throw new ArgumentNullException("argumentName");
            }
            if (argumentValue == null)
            {
                if (throwError)
                {
                    throw new ArgumentNullException(argumentName);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查字符串是否为空（null或者长度为0）。
        /// </summary>
        /// <param name="argumentName">字符串名。</param>
        /// <param name="argumentValue">被检查的字符串。.</param>
        /// <param name="throwError">为空时是否抛出异常。</param>
        /// <returns>为空则返回true。</returns>
        public static bool ArgumentNotNullOrEmptyString(string argumentName, string argumentValue, bool throwError)
        {
            bool result = ArgumentNotNull(argumentName, argumentValue, throwError);
            result = (argumentValue.Trim().Length == 0);
            if (result && throwError)
                throw new ArgumentException("参数{0}为Null或为空", argumentName);
            return result;
        }

        /// <summary>
        /// 检查数组是否为空（长度为0）。
        /// </summary>
        /// <param name="argName">数组名。</param>
        /// <param name="argValue">被检查的数组实例。</param>
        /// <param name="throwError">为空引用时是否抛出异常。</param>
        /// <returns>为空则返回true。</returns>
        public static bool ArgumentEmptyArray(string argName, Array argValue, bool throwError)
        {
            if (string.IsNullOrEmpty(argName))
            {
                throw new ArgumentNullException("argName");
            }
            bool ret = (argValue == null || argValue.Length == 0);
            if (ret && throwError)
            {
                throw new ArgumentException("数组为空", argName);
            }
            return ret;
        }

        /// <summary>
        /// 判断某值是否在枚举内（位枚举）。
        /// </summary>
        /// <param name="checkingValue">被检测的枚举值。</param>
        /// <param name="expectedValue">期望的枚举值。</param>
        /// <returns>是否包含。</returns>
        public static bool CheckFlagsEnumEquals(Enum checkingValue, Enum expectedValue)
        {
            int intCheckingValue = Convert.ToInt32(checkingValue);
            int intExpectedValue = Convert.ToInt32(expectedValue);
            return (intCheckingValue & intExpectedValue) == intExpectedValue;
        }

        /// <summary>
        /// 判断枚举值是否属于该枚举。
        /// </summary>
        /// <param name="argumentName">参数名。</param>
        /// <param name="enumType">枚举类型。</param>
        /// <param name="checkingValue">被检测的枚举值</param>
        /// <param name="throwError">为空引用时是否抛出异常。</param>
        /// <returns>是否包含。</returns>
        public static bool CheckEnumValueIsDefined(string argumentName, Type enumType, object checkingValue, bool throwError)
        {
            bool ret = Enum.IsDefined(enumType, checkingValue);
            if (!ret && throwError)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                    "{0}枚举值{1}未定义",
                    argumentName, enumType.ToString()));
            }
            return ret;
        }
    }
}
