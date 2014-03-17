//================================================================================
//  FileName: EnumeratorOnIList.cs
//  Desc:将IList转化为IEnumerator。
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
    ///  将IList转化为IEnumerator。
    /// </summary>
    public class EnumeratorOnIList : IEnumerator
    {
        #region 成员变量，构造函数。
        IList collection;
        int index, indexBounds, startIndex;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public EnumeratorOnIList(IList collection, int startIndex, int count)
        {
            this.collection = collection;
            this.startIndex = startIndex;
            this.index = -1;
            this.indexBounds = startIndex + count;
            if (this.indexBounds > collection.Count)
                this.indexBounds = collection.Count;
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
                if (this.index < 0)
                    throw new InvalidOperationException("当前索引越界。");
                return this.collection[this.startIndex + this.index];
            }
        }
        /// <summary>
        /// 下一个值。
        /// </summary>
        /// <returns>存在为True，否则为False。</returns>
        public bool MoveNext()
        {
            this.index++;
            return this.startIndex + this.index < this.indexBounds;
        }
        /// <summary>
        /// 复位。
        /// </summary>
        public void Reset()
        {
            this.index = -1;
        }

        #endregion
    }
}
