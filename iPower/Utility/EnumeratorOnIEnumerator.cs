//================================================================================
//  FileName: EnumeratorOnIEnumerator.cs
//  Desc:将IEnumerator转化为IEnumerator。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-12-31
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

namespace iPower.Utility
{
    /// <summary>
    /// 将IEnumerator转化为IEnumerator。
    /// </summary>
    public class EnumeratorOnIEnumerator : IEnumerator
    {
        #region 成员变量，构造函数。
        IEnumerator realEnum;
        int index, indexBounds;
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="realEnum"></param>
        /// <param name="count"></param>
        public EnumeratorOnIEnumerator(IEnumerator realEnum, int count)
        {
            this.realEnum = realEnum;
            this.index = -1;
            this.indexBounds = count;
        }
        #endregion
        
        #region IEnumerator 成员
        /// <summary>
        /// 获取当前值。
        /// </summary>
        public object Current
        {
            get
            {
                return this.realEnum.Current;
            }
        }
        /// <summary>
        /// 下一个值。
        /// </summary>
        /// <returns>存在为True，否则为False。</returns>
        public bool MoveNext()
        {
            bool flag = this.realEnum.MoveNext();
            this.index++;
            return flag && (this.index < this.indexBounds);
        }
        /// <summary>
        /// 复位。
        /// </summary>
        public void Reset()
        {
            this.realEnum.Reset();
            this.index = -1;
        }

        #endregion
    }
}
