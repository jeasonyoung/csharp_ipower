//================================================================================
//  FileName: BaseException.cs
//  Desc:异常模块异常，框架的基础异常类，所有的异常请从本类派生。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-27
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
    /// 异常模块异常，框架的基础异常类，所有的异常请从本类派生。
    /// </summary>
    [Serializable]
    public class BaseException : ApplicationException
    {
        #region 构成员变量，造函数。
        /// <summary>
        /// 异常编号
        /// </summary>
        protected int errorNo;
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseException()
            : this(0, null, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public BaseException(string message, Exception innerException)
            : this(0, message, innerException)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public BaseException(string message)
            : this(0, message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorNo">异常编号</param>
        /// <param name="message">异常消息</param>
        public BaseException(int errorNo, string message)
            : this(errorNo, message, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorNo">异常编号</param>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public BaseException(int errorNo, string message, Exception innerException)
            : base(message, innerException)
        {
            this.errorNo = errorNo;
        }

        #endregion

        #region 属性。
        /// <summary>
        /// 异常编号
        /// </summary>
        public int ErrorNo
        {
            get { return this.errorNo; }
        }
        #endregion

        #region 静态函数。
        /// <summary>
        /// 查找原始的异常
        /// </summary>
        /// <param name="e">异常</param>
        /// <returns>原始的异常</returns>
        public static Exception FindSourceException(Exception e)
        {
            Exception e1 = e;
            while (e1 != null)
            {
                e = e1;
                e1 = e1.InnerException;
            }
            return e;
        }

        /// <summary>
        /// 从异常树种查找指定类型的异常
        /// </summary>
        /// <param name="e">异常</param>
        /// <param name="expectedExceptionType">期待的异常类型</param>
        /// <returns>所要求的异常，如果找不到，返回null</returns>
        public static Exception FindSourceException(Exception e, Type expectedExceptionType)
        {
            while (e != null)
            {
                if (e.GetType() == expectedExceptionType)
                {
                    return e;
                }
                e = e.InnerException;
            }
            return null;
        }
        #endregion
    }
}
